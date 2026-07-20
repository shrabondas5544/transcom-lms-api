using System;
using System.ComponentModel.DataAnnotations;

namespace transcom_lms_api.DTOs
{
    public class UpdateEmployeeProfileDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(100)]
        public string FatherName { get; set; } = string.Empty;

        [StringLength(100)]
        public string MotherName { get; set; } = string.Empty;

        public string PresentAddress { get; set; } = string.Empty;

        public string PermanentAddress { get; set; } = string.Empty;

        public string Dob { get; set; } = string.Empty; // Format: yyyy-MM-dd

        public string Phone { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string Nationality { get; set; } = "Bangladeshi";
        public string Religion { get; set; } = "Muslim";
        public string Sex { get; set; } = "Male";
        public string BloodType { get; set; } = string.Empty;

        [RegularExpression(@"^(.{10,17})?$", ErrorMessage = "NID must be between 10 and 17 characters.")]
        public string Nid { get; set; } = string.Empty;

        [RegularExpression(@"^([a-zA-Z0-9]{7,9})?$", ErrorMessage = "Passport must be alphanumeric and 7-9 characters long.")]
        public string Passport { get; set; } = string.Empty;
        public string Hobbies { get; set; } = string.Empty;

        // Social Links
        public string Facebook { get; set; } = string.Empty;
        public string Instagram { get; set; } = string.Empty;
        public string XLink { get; set; } = string.Empty;
        public string Linkedin { get; set; } = string.Empty;

        public string MaritalStatus { get; set; } = "Single";
        public string MarriageDate { get; set; } = string.Empty; // Format: yyyy-MM-dd

        // Nested Education details for updating
        public EducationSSCDetailsDto EducationSSC { get; set; } = new();
        public EducationHSCDetailsDto EducationHSC { get; set; } = new();
        public EducationDiplomaDetailsDto EducationDiploma { get; set; } = new();
        public EducationGraduateDetailsDto EducationGraduate { get; set; } = new();
        public EducationPostGraduateDetailsDto EducationPostGraduate { get; set; } = new();

        // Nested documents details for updating
        public DocumentsDetailsDto Documents { get; set; } = new();

        public string AvatarImage { get; set; } = string.Empty;
        public double AvatarScale { get; set; } = 1.0;
        public double AvatarX { get; set; } = 0.0;
        public double AvatarY { get; set; } = 0.0;

        public bool IsAssessor { get; set; }
        public bool CanTakeAssessment { get; set; }
        public bool CanConductAudit { get; set; }
    }
}
