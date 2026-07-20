using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using transcom_lms_api.Data;
using transcom_lms_api.Models;

namespace transcom_lms_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowroomVisitEvaluationController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ShowroomVisitEvaluationController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/ShowroomVisitEvaluation
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateEvaluation([FromBody] CreateEvaluationDto dto)
        {
            if (dto == null) return BadRequest("Invalid payload.");

            var employeeExists = await _context.EmployeeProfiles.AnyAsync(e => e.Id == dto.EmployeeProfileId);
            if (!employeeExists)
            {
                return BadRequest("Employee profile not found.");
            }

            // Calculate overall score (average of non-zero/non-skipped criteria)
            var scoresList = new List<int>();
            if (dto.CustomerDealingScore > 0) scoresList.Add(dto.CustomerDealingScore);
            if (dto.ProductKnowledgeScore > 0) scoresList.Add(dto.ProductKnowledgeScore);
            if (dto.GroomingScore > 0) scoresList.Add(dto.GroomingScore);
            if (dto.DemonstrationSkillScore > 0) scoresList.Add(dto.DemonstrationSkillScore);
            if (dto.DisciplineScore > 0) scoresList.Add(dto.DisciplineScore);

            double totalScore = scoresList.Count > 0 ? scoresList.Average() : 0.0;

            var evaluation = new ShowroomVisitEvaluation
            {
                EmployeeProfileId = dto.EmployeeProfileId,
                VisitDate = DateTime.SpecifyKind(dto.VisitDate ?? DateTime.UtcNow, DateTimeKind.Utc),
                TotalScore = Math.Round(totalScore, 2),
                CustomerDealingScore = dto.CustomerDealingScore,
                CustomerDealingRemarks = dto.CustomerDealingRemarks ?? string.Empty,
                ProductKnowledgeScore = dto.ProductKnowledgeScore,
                ProductKnowledgeRemarks = dto.ProductKnowledgeRemarks ?? string.Empty,
                GroomingScore = dto.GroomingScore,
                GroomingRemarks = dto.GroomingRemarks ?? string.Empty,
                DemonstrationSkillScore = dto.DemonstrationSkillScore,
                DemonstrationSkillRemarks = dto.DemonstrationSkillRemarks ?? string.Empty,
                DisciplineScore = dto.DisciplineScore,
                DisciplineRemarks = dto.DisciplineRemarks ?? string.Empty
            };

            _context.ShowroomVisitEvaluations.Add(evaluation);
            await _context.SaveChangesAsync();

            return StatusCode(StatusCodes.Status201Created, evaluation);
        }

        // PUT: api/ShowroomVisitEvaluation/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateEvaluation(int id, [FromBody] CreateEvaluationDto dto)
        {
            if (dto == null) return BadRequest("Invalid payload.");

            var evaluation = await _context.ShowroomVisitEvaluations.FindAsync(id);
            if (evaluation == null)
            {
                return NotFound();
            }

            var scoresList = new List<int>();
            if (dto.CustomerDealingScore > 0) scoresList.Add(dto.CustomerDealingScore);
            if (dto.ProductKnowledgeScore > 0) scoresList.Add(dto.ProductKnowledgeScore);
            if (dto.GroomingScore > 0) scoresList.Add(dto.GroomingScore);
            if (dto.DemonstrationSkillScore > 0) scoresList.Add(dto.DemonstrationSkillScore);
            if (dto.DisciplineScore > 0) scoresList.Add(dto.DisciplineScore);

            double totalScore = scoresList.Count > 0 ? scoresList.Average() : 0.0;

            evaluation.CustomerDealingScore = dto.CustomerDealingScore;
            evaluation.CustomerDealingRemarks = dto.CustomerDealingRemarks ?? string.Empty;
            evaluation.ProductKnowledgeScore = dto.ProductKnowledgeScore;
            evaluation.ProductKnowledgeRemarks = dto.ProductKnowledgeRemarks ?? string.Empty;
            evaluation.GroomingScore = dto.GroomingScore;
            evaluation.GroomingRemarks = dto.GroomingRemarks ?? string.Empty;
            evaluation.DemonstrationSkillScore = dto.DemonstrationSkillScore;
            evaluation.DemonstrationSkillRemarks = dto.DemonstrationSkillRemarks ?? string.Empty;
            evaluation.DisciplineScore = dto.DisciplineScore;
            evaluation.DisciplineRemarks = dto.DisciplineRemarks ?? string.Empty;
            evaluation.TotalScore = Math.Round(totalScore, 2);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/ShowroomVisitEvaluation
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetEvaluations(
            [FromQuery] string? search, 
            [FromQuery] string? sortDirection,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var employeesQuery = _context.EmployeeProfiles.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var q = search.Trim().ToLower();
                employeesQuery = employeesQuery.Where(profile =>
                    profile.Name.ToLower().Contains(q) ||
                    profile.EmployeeCode.ToLower().Contains(q) ||
                    profile.Showroom.ToLower().Contains(q) ||
                    profile.Division.ToLower().Contains(q) ||
                    profile.Designation.ToLower().Contains(q)
                );
            }

            var profiles = await employeesQuery.ToListAsync();
            var evaluations = await _context.ShowroomVisitEvaluations.ToListAsync();
            var list = new List<EvaluationDetailsDto>();

            foreach (var profile in profiles)
            {
                var empEvaluations = evaluations
                    .Where(e => e.EmployeeProfileId == profile.Id)
                    .OrderByDescending(e => e.VisitDate)
                    .ToList();

                var latest = empEvaluations.FirstOrDefault();

                if (latest != null)
                {
                    list.Add(new EvaluationDetailsDto
                    {
                        Id = "REP-" + latest.Id,
                        EmployeeProfileId = profile.Id,
                        EmployeeId = profile.EmployeeCode,
                        EmployeeName = profile.Name,
                        Designation = profile.Designation,
                        Showroom = profile.Showroom,
                        Division = profile.Division,
                        GradeGroup = profile.GradeGroup,
                        Date = latest.VisitDate,
                        Score = latest.TotalScore,
                        Scores = new Dictionary<string, int>
                        {
                            { "customerDealing", latest.CustomerDealingScore },
                            { "productKnowledge", latest.ProductKnowledgeScore },
                            { "grooming", latest.GroomingScore },
                            { "demonstrationSkill", latest.DemonstrationSkillScore },
                            { "discipline", latest.DisciplineScore }
                        },
                        Remarks = new Dictionary<string, string>
                        {
                            { "customerDealing", latest.CustomerDealingRemarks },
                            { "productKnowledge", latest.ProductKnowledgeRemarks },
                            { "grooming", latest.GroomingRemarks },
                            { "demonstrationSkill", latest.DemonstrationSkillRemarks },
                            { "discipline", latest.DisciplineRemarks }
                        },
                        AvatarImage = profile.AvatarImage,
                        AvatarScale = profile.AvatarScale,
                        AvatarX = profile.AvatarX,
                        AvatarY = profile.AvatarY
                    });
                }
                else
                {
                    list.Add(new EvaluationDetailsDto
                    {
                        Id = "NOAUDIT-" + profile.Id,
                        EmployeeProfileId = profile.Id,
                        EmployeeId = profile.EmployeeCode,
                        EmployeeName = profile.Name,
                        Designation = profile.Designation,
                        Showroom = profile.Showroom,
                        Division = profile.Division,
                        GradeGroup = profile.GradeGroup,
                        Date = DateTime.MinValue,
                        Score = 0.0,
                        Scores = new Dictionary<string, int>
                        {
                            { "customerDealing", 0 },
                            { "productKnowledge", 0 },
                            { "grooming", 0 },
                            { "demonstrationSkill", 0 },
                            { "discipline", 0 }
                        },
                        Remarks = new Dictionary<string, string>
                        {
                            { "customerDealing", "" },
                            { "productKnowledge", "" },
                            { "grooming", "" },
                            { "demonstrationSkill", "" },
                            { "discipline", "" }
                        },
                        AvatarImage = profile.AvatarImage,
                        AvatarScale = profile.AvatarScale,
                        AvatarX = profile.AvatarX,
                        AvatarY = profile.AvatarY
                    });
                }
            }

            // Apply sort direction
            if (sortDirection == "asc")
            {
                list = list.OrderBy(x => x.Score).ToList();
            }
            else
            {
                list = list.OrderByDescending(x => x.Score).ToList();
            }

            var totalCount = list.Count;
            var items = list
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Ok(new
            {
                items = items,
                totalPages = (int)Math.Ceiling((double)totalCount / pageSize),
                totalCount = totalCount
            });
        }

        // GET: api/ShowroomVisitEvaluation/employee/{employeeCodeOrId}
        [HttpGet("employee/{employeeCodeOrId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetEmployeeEvaluations(string employeeCodeOrId)
        {
            // First locate employee by code/username or profile primary key (Id)
            EmployeeProfile? profile = null;
            if (int.TryParse(employeeCodeOrId, out var parsedId))
            {
                profile = await _context.EmployeeProfiles.FirstOrDefaultAsync(p => p.Id == parsedId);
            }
            if (profile == null)
            {
                profile = await _context.EmployeeProfiles.FirstOrDefaultAsync(p => p.EmployeeCode == employeeCodeOrId);
            }

            if (profile == null)
            {
                return Ok(new EmployeeHistoryDto());
            }

            var evaluations = await _context.ShowroomVisitEvaluations
                .Where(e => e.EmployeeProfileId == profile.Id)
                .OrderBy(e => e.VisitDate)
                .Select(e => new EvaluationHistoryItemDto
                {
                    Date = e.VisitDate.ToString("dd MMM yyyy"), // format date nicely like: 19 Jan 2026, 26 Feb 2026
                    Score = e.TotalScore,
                    Showroom = profile.Showroom,
                    Scores = new Dictionary<string, int>
                    {
                        { "customerDealing", e.CustomerDealingScore },
                        { "productKnowledge", e.ProductKnowledgeScore },
                        { "grooming", e.GroomingScore },
                        { "demonstrationSkill", e.DemonstrationSkillScore },
                        { "discipline", e.DisciplineScore }
                    },
                    Remarks = new Dictionary<string, string>
                    {
                        { "customerDealing", e.CustomerDealingRemarks },
                        { "productKnowledge", e.ProductKnowledgeRemarks },
                        { "grooming", e.GroomingRemarks },
                        { "demonstrationSkill", e.DemonstrationSkillRemarks },
                        { "discipline", e.DisciplineRemarks }
                    }
                })
                .ToListAsync();

            return Ok(new EmployeeHistoryDto
            {
                Info = new EmployeeInfoDto
                {
                    Name = profile.Name,
                    Designation = profile.Designation,
                    Showroom = profile.Showroom,
                    Division = profile.Division,
                    GradeGroup = profile.GradeGroup,
                    EmployeeCode = profile.EmployeeCode,
                    AvatarImage = profile.AvatarImage,
                    AvatarScale = profile.AvatarScale,
                    AvatarX = profile.AvatarX,
                    AvatarY = profile.AvatarY
                },
                History = evaluations
            });
        }
    }

    public class CreateEvaluationDto
    {
        public int EmployeeProfileId { get; set; }
        public DateTime? VisitDate { get; set; }
        public int CustomerDealingScore { get; set; }
        public string? CustomerDealingRemarks { get; set; }
        public int ProductKnowledgeScore { get; set; }
        public string? ProductKnowledgeRemarks { get; set; }
        public int GroomingScore { get; set; }
        public string? GroomingRemarks { get; set; }
        public int DemonstrationSkillScore { get; set; }
        public string? DemonstrationSkillRemarks { get; set; }
        public int DisciplineScore { get; set; }
        public string? DisciplineRemarks { get; set; }
    }

    public class EvaluationDetailsDto
    {
        public string Id { get; set; } = string.Empty;
        public int EmployeeProfileId { get; set; }
        public string EmployeeId { get; set; } = string.Empty;
        public string EmployeeName { get; set; } = string.Empty;
        public string Designation { get; set; } = string.Empty;
        public string Showroom { get; set; } = string.Empty;
        public string Division { get; set; } = string.Empty;
        public string GradeGroup { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public double Score { get; set; }
        public Dictionary<string, int> Scores { get; set; } = new();
        public Dictionary<string, string> Remarks { get; set; } = new();
        public string AvatarImage { get; set; } = string.Empty;
        public double AvatarScale { get; set; } = 1.0;
        public double AvatarX { get; set; } = 0.0;
        public double AvatarY { get; set; } = 0.0;
    }

    public class EmployeeHistoryDto
    {
        public EmployeeInfoDto? Info { get; set; }
        public List<EvaluationHistoryItemDto> History { get; set; } = new();
    }

    public class EmployeeInfoDto
    {
        public string Name { get; set; } = string.Empty;
        public string Designation { get; set; } = string.Empty;
        public string Showroom { get; set; } = string.Empty;
        public string Division { get; set; } = string.Empty;
        public string GradeGroup { get; set; } = string.Empty;
        public string EmployeeCode { get; set; } = string.Empty;
        public string AvatarImage { get; set; } = string.Empty;
        public double AvatarScale { get; set; } = 1.0;
        public double AvatarX { get; set; } = 0.0;
        public double AvatarY { get; set; } = 0.0;
    }

    public class EvaluationHistoryItemDto
    {
        public string Date { get; set; } = string.Empty;
        public double Score { get; set; }
        public string Showroom { get; set; } = string.Empty;
        public Dictionary<string, int> Scores { get; set; } = new();
        public Dictionary<string, string> Remarks { get; set; } = new();
    }
}
