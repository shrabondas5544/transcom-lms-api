namespace transcom_lms_api.DTOs
{
    /// <summary>
    /// Aggregate statistics returned by GetDashboardStats().
    /// These power the four KPI summary cards on the Admin Dashboard.
    /// </summary>
    public class AdminDashboardStatsDto
    {
        /// <summary>Total number of employees registered in the system.</summary>
        public int TotalEmployees { get; set; }

        /// <summary>Number of assessment templates that are currently active (deadline not expired).</summary>
        public int ActiveAssessments { get; set; }

        /// <summary>Average performance score (0–10) across all employees and all assessment types.</summary>
        public double AvgPerformanceScore { get; set; }

        /// <summary>Number of showroom audit events that have been flagged and require HR review.</summary>
        public int AuditAlertsPending { get; set; }

        /// <summary>Total number of showroom visits logged this month.</summary>
        public int ShowroomVisitsThisMonth { get; set; }

        /// <summary>Percentage change in average score compared to last month (can be negative).</summary>
        public double ScoreDeltaPercent { get; set; }
    }
}
