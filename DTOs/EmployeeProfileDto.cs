using System;

namespace transcom_lms_api.DTOs
{
    /// <summary>
    /// A clean data transfer object representing the employee profile data contract sent to the frontend.
    /// Dates are converted to formatted strings for easier integration with client-side widgets.
    /// </summary>
    public class EmployeeProfileDto
    {
        public int Id { get; set; }
        public string EmployeeCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string FatherName { get; set; } = string.Empty;
        public string MotherName { get; set; } = string.Empty;
        public string PresentAddress { get; set; } = string.Empty;
        public string PermanentAddress { get; set; } = string.Empty;
        public string Dob { get; set; } = string.Empty; // Format: YYYY-MM-DD
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Nationality { get; set; } = string.Empty;
        public string Religion { get; set; } = string.Empty;
        public string Sex { get; set; } = string.Empty;
        public string BloodType { get; set; } = string.Empty;
        public string Nid { get; set; } = string.Empty;
        public string MaritalStatus { get; set; } = string.Empty;
        public string? MarriageDate { get; set; } // Format: YYYY-MM-DD (Null if single)
        public string Designation { get; set; } = string.Empty;
        public string Showroom { get; set; } = string.Empty;
        public string Division { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string JoinDate { get; set; } = string.Empty; // Format: YYYY-MM-DD

        public string Passport { get; set; } = string.Empty;
        public string Hobbies { get; set; } = string.Empty;
        public string Facebook { get; set; } = string.Empty;
        public string Instagram { get; set; } = string.Empty;
        public string XLink { get; set; } = string.Empty;
        public string Linkedin { get; set; } = string.Empty;

        // Nested Education details
        public EducationSSCDetailsDto EducationSSC { get; set; } = new();
        public EducationHSCDetailsDto EducationHSC { get; set; } = new();
        public EducationDiplomaDetailsDto EducationDiploma { get; set; } = new();
        public EducationGraduateDetailsDto EducationGraduate { get; set; } = new();
        public EducationPostGraduateDetailsDto EducationPostGraduate { get; set; } = new();

        // Nested documents details
        public DocumentsDetailsDto Documents { get; set; } = new();

        public string AvatarImage { get; set; } = string.Empty;
        public double AvatarScale { get; set; } = 1.0;
        public double AvatarX { get; set; } = 0.0;
        public double AvatarY { get; set; } = 0.0;

        public bool IsAssessor { get; set; }
        public bool CanTakeAssessment { get; set; }
        public bool CanConductAudit { get; set; }
    }

    public class EducationSSCDetailsDto
    {
        public string Institution { get; set; } = string.Empty;
        public string Division { get; set; } = string.Empty;
        public string Group { get; set; } = string.Empty;
        public string YearPassed { get; set; } = string.Empty;
        public string Gpa { get; set; } = string.Empty;
    }

    public class EducationHSCDetailsDto
    {
        public string Institution { get; set; } = string.Empty;
        public string Division { get; set; } = string.Empty;
        public string Group { get; set; } = string.Empty;
        public string YearPassed { get; set; } = string.Empty;
        public string Gpa { get; set; } = string.Empty;
    }

    public class EducationDiplomaDetailsDto
    {
        public string Institution { get; set; } = string.Empty;
        public string YearPassed { get; set; } = string.Empty;
        public string Cgpa { get; set; } = string.Empty;
    }

    public class EducationGraduateDetailsDto
    {
        public string Institution { get; set; } = string.Empty;
        public string PassedYear { get; set; } = string.Empty;
        public string Degree { get; set; } = string.Empty;
        public string Result { get; set; } = string.Empty;
        public string Achievement { get; set; } = string.Empty;
        public string? AchievementFile { get; set; }
    }

    public class EducationPostGraduateDetailsDto
    {
        public string Institution { get; set; } = string.Empty;
        public string PassedYear { get; set; } = string.Empty;
        public string Degree { get; set; } = string.Empty;
        public string Result { get; set; } = string.Empty;
        public string Achievement { get; set; } = string.Empty;
        public string? AchievementFile { get; set; }
    }

    public class DocumentsDetailsDto
    {
        public string? Resume { get; set; }
        public string? NidEmpFront { get; set; }
        public string? NidEmpBack { get; set; }
        public string? NidNomFront { get; set; }
        public string? NidNomBack { get; set; }
        public string? Covid { get; set; }
        public string? Release { get; set; }
        public string? Payslip { get; set; }
        public string? Tax { get; set; }
    }
}
