using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace transcom_lms_api.Models
{
    public class EmployeeDocument
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int EmployeeProfileId { get; set; }

        [ForeignKey("EmployeeProfileId")]
        public EmployeeProfile? EmployeeProfile { get; set; }

        [MaxLength(250)]
        public string Resume { get; set; } = string.Empty;

        [MaxLength(250)]
        public string NidEmpFront { get; set; } = string.Empty;

        [MaxLength(250)]
        public string NidEmpBack { get; set; } = string.Empty;

        [MaxLength(250)]
        public string NidNomFront { get; set; } = string.Empty;

        [MaxLength(250)]
        public string NidNomBack { get; set; } = string.Empty;

        [MaxLength(250)]
        public string Covid { get; set; } = string.Empty;

        [MaxLength(250)]
        public string Release { get; set; } = string.Empty;

        [MaxLength(250)]
        public string Payslip { get; set; } = string.Empty;

        [MaxLength(250)]
        public string Tax { get; set; } = string.Empty;
    }
}
