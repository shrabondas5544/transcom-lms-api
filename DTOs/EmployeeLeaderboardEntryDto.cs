namespace transcom_lms_api.DTOs
{
    /// <summary>
    /// Represents a single row in the Admin Performance Leaderboard table.
    /// Combines skill assessment scores and showroom audit scores into a unified ranking entry.
    /// </summary>
    public class EmployeeLeaderboardEntryDto
    {
        /// <summary>Unique employee code, e.g. EMP-2085.</summary>
        public string EmployeeCode { get; set; } = string.Empty;

        /// <summary>Full name of the employee.</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>Job title / designation, e.g. Senior Sales Executive.</summary>
        public string Designation { get; set; } = string.Empty;

        /// <summary>Outlet/showroom name the employee is assigned to.</summary>
        public string Showroom { get; set; } = string.Empty;

        /// <summary>Geographic division, e.g. Dhaka Division.</summary>
        public string Division { get; set; } = string.Empty;

        /// <summary>Score achieved on training/knowledge quizzes (0–10).</summary>
        public double SkillScore { get; set; }

        /// <summary>Score achieved on showroom floor visit audits (0–10).</summary>
        public double AuditScore { get; set; }

        /// <summary>
        /// Weighted combined score: 60% SkillScore + 40% AuditScore.
        /// This is the primary sort field for the leaderboard.
        /// </summary>
        public double CombinedScore { get; set; }

        /// <summary>
        /// Performance tier label derived from CombinedScore:
        /// "Excellent" (≥ 8.5), "Good" (≥ 7.0), "Satisfactory" (≥ 5.5), "Needs Improvement" (below 5.5).
        /// </summary>
        public string Status { get; set; } = string.Empty;
    }

    /// <summary>
    /// Wrapper response for the paginated leaderboard endpoint.
    /// </summary>
    public class EmployeeLeaderboardResponseDto
    {
        public List<EmployeeLeaderboardEntryDto> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }
}
