using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace transcom_lms_api.Models
{
    public class EmployeeEducation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int EmployeeProfileId { get; set; }

        [ForeignKey("EmployeeProfileId")]
        public EmployeeProfile? EmployeeProfile { get; set; }

        [Required]
        [MaxLength(50)]
        public string Level { get; set; } = string.Empty; // SSC, HSC, Diploma, Graduation, PostGraduation

        [MaxLength(150)]
        public string Institution { get; set; } = string.Empty;

        [MaxLength(100)]
        public string BoardOrDivision { get; set; } = string.Empty;

        [MaxLength(100)]
        public string GroupOrMajor { get; set; } = string.Empty;

        [MaxLength(10)]
        public string PassingYear { get; set; } = string.Empty;

        [MaxLength(20)]
        public string Result { get; set; } = string.Empty;

        [MaxLength(250)]
        public string Achievement { get; set; } = string.Empty;

        [MaxLength(250)]
        public string AchievementFile { get; set; } = string.Empty;
    }
}
