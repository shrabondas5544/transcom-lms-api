using System;
using System.ComponentModel.DataAnnotations;

namespace transcom_lms_api.Models
{
    /// <summary>
    /// Represents the database entity for an employee's detailed profile.
    /// Maps directly to the SQL database table via Entity Framework Core.
    /// </summary>
    public class EmployeeProfile
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string EmployeeCode { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string FatherName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string MotherName { get; set; } = string.Empty;

        [Required]
        [MaxLength(250)]
        public string PresentAddress { get; set; } = string.Empty;

        [Required]
        [MaxLength(250)]
        public string PermanentAddress { get; set; } = string.Empty;

        [Required]
        public DateTime Dob { get; set; }

        [Required]
        [MaxLength(20)]
        public string Phone { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Nationality { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Religion { get; set; } = string.Empty;

        [Required]
        [MaxLength(10)]
        public string Sex { get; set; } = string.Empty;

        [Required]
        [MaxLength(5)]
        public string BloodType { get; set; } = string.Empty;

        [Required]
        [MaxLength(30)]
        public string Nid { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string MaritalStatus { get; set; } = string.Empty;

        public DateTime? MarriageDate { get; set; }

        [MaxLength(50)]
        public string Passport { get; set; } = string.Empty;

        [MaxLength(250)]
        public string Hobbies { get; set; } = string.Empty;

        [MaxLength(250)]
        public string Facebook { get; set; } = string.Empty;

        [MaxLength(250)]
        public string Instagram { get; set; } = string.Empty;

        [MaxLength(250)]
        public string XLink { get; set; } = string.Empty;

        [MaxLength(250)]
        public string Linkedin { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Designation { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Showroom { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Division { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Department { get; set; } = string.Empty;

        [Required]
        public DateTime JoinDate { get; set; }

        [Required]
        [MaxLength(255)]
        public string PasswordHash { get; set; } = string.Empty;

        [MaxLength(100)]
        public string EmpStat { get; set; } = string.Empty;

        [MaxLength(100)]
        public string GradeGroup { get; set; } = string.Empty;

        [MaxLength(100)]
        public string JobGrade { get; set; } = string.Empty;

        [MaxLength(100)]
        public string EqGrade { get; set; } = string.Empty;

        [MaxLength(100)]
        public string ConfirmDate { get; set; } = string.Empty;

        public string AvatarImage { get; set; } = string.Empty;
        public double AvatarScale { get; set; } = 1.0;
        public double AvatarX { get; set; } = 0.0;
        public double AvatarY { get; set; } = 0.0;

        public bool IsAssessor { get; set; } = false;
        public bool CanTakeAssessment { get; set; } = false;
        public bool CanConductAudit { get; set; } = false;
    }
}
