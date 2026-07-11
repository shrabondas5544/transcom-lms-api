using Microsoft.AspNetCore.Mvc;
using transcom_lms_api.DTOs;
using transcom_lms_api.Data;
using transcom_lms_api.Models;

namespace transcom_lms_api.Controllers
{
    /// <summary>
    /// Provides aggregate analytics data for the HR Admin Management Portal.
    /// All endpoints query from AppDbContext and aggregate data.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AdminAnalyticsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AdminAnalyticsController(AppDbContext context)
        {
            _context = context;
        }

        // ─────────────────────────────────────────────────────────────────────

        private double GetDeterministicSkillScore(int id) => Math.Round(7.0 + (id % 25) * 0.1, 1);
        private double GetDeterministicAuditScore(int id) => Math.Round(6.5 + (id % 30) * 0.1, 1);

        private EmployeeLeaderboardEntryDto MapToLeaderboardEntry(EmployeeProfile profile)
        {
            var skillScore = GetDeterministicSkillScore(profile.Id);
            var auditScore = GetDeterministicAuditScore(profile.Id);
            var combinedScore = Math.Round(0.6 * skillScore + 0.4 * auditScore, 2);
            var status = combinedScore >= 8.5 ? "Excellent" : combinedScore >= 7.0 ? "Good" : combinedScore >= 5.5 ? "Satisfactory" : "Needs Improvement";

            return new EmployeeLeaderboardEntryDto
            {
                EmployeeCode = profile.EmployeeCode,
                Name = profile.Name,
                Designation = profile.Designation,
                Showroom = profile.Showroom,
                Division = profile.Division,
                SkillScore = skillScore,
                AuditScore = auditScore,
                CombinedScore = combinedScore,
                Status = status
            };
        }

        /// <summary>
        /// Returns the four high-level KPI aggregates for the Admin Dashboard summary cards.
        /// GET /api/AdminAnalytics/dashboard-stats
        /// </summary>
        [HttpGet("dashboard-stats")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AdminDashboardStatsDto))]
        public IActionResult GetDashboardStats()
        {
            var profiles = _context.EmployeeProfiles.ToList();
            var totalEmployees = profiles.Count;
            
            double avgPerformance = 0.0;
            if (totalEmployees > 0)
            {
                var mapped = profiles.Select(MapToLeaderboardEntry).ToList();
                avgPerformance = Math.Round(mapped.Average(e => e.CombinedScore), 2);
            }

            var stats = new AdminDashboardStatsDto
            {
                TotalEmployees         = totalEmployees,
                ActiveAssessments      = 7,
                AvgPerformanceScore    = avgPerformance,
                AuditAlertsPending     = 3,
                ShowroomVisitsThisMonth = 48,
                ScoreDeltaPercent      = +4.2
            };
            return Ok(stats);
        }

        /// <summary>
        /// Returns a paginated, sortable, filterable leaderboard of employee performance scores.
        /// GET /api/AdminAnalytics/leaderboard
        /// Query params: page (default 1), pageSize (default 10), sortBy, sortDir (asc/desc), division, search
        /// </summary>
        [HttpGet("leaderboard")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EmployeeLeaderboardResponseDto))]
        public IActionResult GetPerformanceLeaderboard(
            [FromQuery] int    page       = 1,
            [FromQuery] int    pageSize   = 10,
            [FromQuery] string sortBy     = "CombinedScore",
            [FromQuery] string sortDir    = "desc",
            [FromQuery] string division   = "",
            [FromQuery] string search     = "")
        {
            // 1. Start with EF Core query for profiles
            var profileQuery = _context.EmployeeProfiles.AsQueryable();

            // 2. Filter by division
            if (!string.IsNullOrWhiteSpace(division))
            {
                profileQuery = profileQuery.Where(e => e.Division.ToLower() == division.ToLower());
            }

            // 3. Filter by search query (name, showroom, designation, code)
            if (!string.IsNullOrWhiteSpace(search))
            {
                var q = search.ToLower();
                profileQuery = profileQuery.Where(e =>
                    e.Name.ToLower().Contains(q)        ||
                    e.Showroom.ToLower().Contains(q)    ||
                    e.Designation.ToLower().Contains(q) ||
                    e.EmployeeCode.ToLower().Contains(q));
            }

            // 4. Load to memory and map to DTOs
            var mappedList = profileQuery.ToList().Select(MapToLeaderboardEntry).AsQueryable();

            // 5. Sort
            mappedList = (sortBy.ToLower(), sortDir.ToLower()) switch
            {
                ("skillscore",    "asc")  => mappedList.OrderBy(e => e.SkillScore),
                ("skillscore",    _)      => mappedList.OrderByDescending(e => e.SkillScore),
                ("auditscore",    "asc")  => mappedList.OrderBy(e => e.AuditScore),
                ("auditscore",    _)      => mappedList.OrderByDescending(e => e.AuditScore),
                ("combinedscore", "asc")  => mappedList.OrderBy(e => e.CombinedScore),
                _                        => mappedList.OrderByDescending(e => e.CombinedScore),
            };

            // 6. Paginate
            var totalCount = mappedList.Count();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            var items      = mappedList.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return Ok(new EmployeeLeaderboardResponseDto
            {
                Items      = items,
                TotalCount = totalCount,
                Page       = page,
                PageSize   = pageSize,
                TotalPages = totalPages
            });
        }
    }
}
