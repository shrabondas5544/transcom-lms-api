using System;
using System.ComponentModel.DataAnnotations;

namespace transcom_lms_api.Models
{
    public class ShowroomVisitEvaluation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int EmployeeProfileId { get; set; }

        [Required]
        public DateTime VisitDate { get; set; }

        [Required]
        public double TotalScore { get; set; }

        public int CustomerDealingScore { get; set; }
        public string CustomerDealingRemarks { get; set; } = string.Empty;

        public int ProductKnowledgeScore { get; set; }
        public string ProductKnowledgeRemarks { get; set; } = string.Empty;

        public int GroomingScore { get; set; }
        public string GroomingRemarks { get; set; } = string.Empty;

        public int DemonstrationSkillScore { get; set; }
        public string DemonstrationSkillRemarks { get; set; } = string.Empty;

        public int DisciplineScore { get; set; }
        public string DisciplineRemarks { get; set; } = string.Empty;
    }
}
