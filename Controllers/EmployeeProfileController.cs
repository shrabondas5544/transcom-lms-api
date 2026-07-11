using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using transcom_lms_api.Data;
using transcom_lms_api.DTOs;
using transcom_lms_api.Models;

namespace transcom_lms_api.Controllers
{
    /// <summary>
    /// API Controller containing endpoints to query and manage employee profile info.
    /// Handles requests mapped to "api/EmployeeProfile".
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeProfileController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EmployeeProfileController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves an employee profile by its identifier.
        /// GET /api/EmployeeProfile/{id}
        /// </summary>
        /// <param name="id">The internal database ID of the profile.</param>
        /// <returns>An EmployeeProfileDto if found; otherwise, 404 NotFound.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EmployeeProfileDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProfile(int id)
        {
            // 1. Fetch from database using the EF Core db context
            var profile = await _context.EmployeeProfiles
                .FirstOrDefaultAsync(p => p.Id == id);

            if (profile == null)
            {
                return NotFound(new { message = $"Employee profile with ID {id} not found." });
            }

            var educations = await _context.EmployeeEducations
                .Where(e => e.EmployeeProfileId == id)
                .ToListAsync();

            var document = await _context.EmployeeDocuments
                .FirstOrDefaultAsync(d => d.EmployeeProfileId == id);

            // 2. Transform the database entity model into the clean DTO contract requested by the frontend
            var dto = new EmployeeProfileDto
            {
                Id = profile.Id,
                EmployeeCode = profile.EmployeeCode,
                Name = profile.Name,
                FatherName = profile.FatherName,
                MotherName = profile.MotherName,
                PresentAddress = profile.PresentAddress,
                PermanentAddress = profile.PermanentAddress,
                Dob = profile.Dob.ToString("yyyy-MM-dd"),
                Phone = profile.Phone,
                Email = profile.Email,
                Nationality = profile.Nationality,
                Religion = profile.Religion,
                Sex = profile.Sex,
                BloodType = profile.BloodType,
                Nid = profile.Nid,
                MaritalStatus = profile.MaritalStatus,
                MarriageDate = profile.MarriageDate?.ToString("yyyy-MM-dd"),
                Designation = profile.Designation,
                Showroom = profile.Showroom,
                Division = profile.Division,
                Department = profile.Department,
                JoinDate = profile.JoinDate.ToString("yyyy-MM-dd"),
                Passport = profile.Passport,
                Hobbies = profile.Hobbies,
                Facebook = profile.Facebook,
                Instagram = profile.Instagram,
                XLink = profile.XLink,
                Linkedin = profile.Linkedin
            };

            var ssc = educations.FirstOrDefault(e => e.Level == "SSC") ?? new();
            var hsc = educations.FirstOrDefault(e => e.Level == "HSC") ?? new();
            var diploma = educations.FirstOrDefault(e => e.Level == "Diploma") ?? new();
            var graduate = educations.FirstOrDefault(e => e.Level == "Graduation") ?? new();
            var postGrad = educations.FirstOrDefault(e => e.Level == "PostGraduation") ?? new();

            dto.EducationSSC = new EducationSSCDetailsDto
            {
                Institution = ssc.Institution,
                Division = ssc.BoardOrDivision,
                Group = ssc.GroupOrMajor,
                YearPassed = ssc.PassingYear,
                Gpa = ssc.Result
            };
            dto.EducationHSC = new EducationHSCDetailsDto
            {
                Institution = hsc.Institution,
                Division = hsc.BoardOrDivision,
                Group = hsc.GroupOrMajor,
                YearPassed = hsc.PassingYear,
                Gpa = hsc.Result
            };
            dto.EducationDiploma = new EducationDiplomaDetailsDto
            {
                Institution = diploma.Institution,
                YearPassed = diploma.PassingYear,
                Cgpa = diploma.Result
            };
            dto.EducationGraduate = new EducationGraduateDetailsDto
            {
                Institution = graduate.Institution,
                PassedYear = graduate.PassingYear,
                Degree = graduate.GroupOrMajor,
                Result = graduate.Result,
                Achievement = graduate.Achievement,
                AchievementFile = graduate.AchievementFile
            };
            dto.EducationPostGraduate = new EducationPostGraduateDetailsDto
            {
                Institution = postGrad.Institution,
                PassedYear = postGrad.PassingYear,
                Degree = postGrad.GroupOrMajor,
                Result = postGrad.Result,
                Achievement = postGrad.Achievement,
                AchievementFile = postGrad.AchievementFile
            };

            if (document != null)
            {
                dto.Documents = new DocumentsDetailsDto
                {
                    Resume = document.Resume,
                    NidEmpFront = document.NidEmpFront,
                    NidEmpBack = document.NidEmpBack,
                    NidNomFront = document.NidNomFront,
                    NidNomBack = document.NidNomBack,
                    Covid = document.Covid,
                    Release = document.Release,
                    Payslip = document.Payslip,
                    Tax = document.Tax
                };
            }

            // 3. Return HTTP 200 OK along with the serialized DTO payload
            return Ok(dto);
        }

        /// <summary>
        /// Updates an employee profile by its identifier.
        /// PUT /api/EmployeeProfile/{id}
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateProfile(int id, [FromBody] UpdateEmployeeProfileDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var profile = await _context.EmployeeProfiles
                .FirstOrDefaultAsync(p => p.Id == id);

            if (profile == null)
            {
                return NotFound(new { message = $"Employee profile with ID {id} not found." });
            }

            // Map edited properties
            profile.Name = dto.Name;
            profile.FatherName = dto.FatherName;
            profile.MotherName = dto.MotherName;
            profile.PresentAddress = dto.PresentAddress;
            profile.PermanentAddress = dto.PermanentAddress;
            profile.Phone = dto.Phone;
            profile.Email = dto.Email;
            profile.Nationality = dto.Nationality;
            profile.Religion = dto.Religion;
            profile.Sex = dto.Sex;
            profile.BloodType = dto.BloodType;
            profile.Nid = dto.Nid;
            profile.Passport = dto.Passport;
            profile.Hobbies = dto.Hobbies;
            profile.Facebook = dto.Facebook;
            profile.Instagram = dto.Instagram;
            profile.XLink = dto.XLink;
            profile.Linkedin = dto.Linkedin;
            profile.MaritalStatus = dto.MaritalStatus;

            // Handle date conversions safely
            if (DateTime.SpecifyKind(DateTime.TryParse(dto.Dob, out var parsedDob) ? parsedDob : DateTime.MinValue, DateTimeKind.Utc) != DateTime.MinValue)
            {
                profile.Dob = DateTime.SpecifyKind(parsedDob, DateTimeKind.Utc);
            }
            if (!string.IsNullOrEmpty(dto.MarriageDate) && DateTime.TryParse(dto.MarriageDate, out var parsedMDate))
            {
                profile.MarriageDate = DateTime.SpecifyKind(parsedMDate, DateTimeKind.Utc);
            }
            else
            {
                profile.MarriageDate = null;
            }

            // Save/Upsert Education details
            var educations = await _context.EmployeeEducations
                .Where(e => e.EmployeeProfileId == id)
                .ToListAsync();

            // SSC
            var ssc = educations.FirstOrDefault(e => e.Level == "SSC");
            if (ssc == null)
            {
                ssc = new EmployeeEducation { EmployeeProfileId = id, Level = "SSC" };
                _context.EmployeeEducations.Add(ssc);
            }
            ssc.Institution = dto.EducationSSC.Institution ?? string.Empty;
            ssc.BoardOrDivision = dto.EducationSSC.Division ?? string.Empty;
            ssc.GroupOrMajor = dto.EducationSSC.Group ?? string.Empty;
            ssc.PassingYear = dto.EducationSSC.YearPassed ?? string.Empty;
            ssc.Result = dto.EducationSSC.Gpa ?? string.Empty;

            // HSC
            var hsc = educations.FirstOrDefault(e => e.Level == "HSC");
            if (hsc == null)
            {
                hsc = new EmployeeEducation { EmployeeProfileId = id, Level = "HSC" };
                _context.EmployeeEducations.Add(hsc);
            }
            hsc.Institution = dto.EducationHSC.Institution ?? string.Empty;
            hsc.BoardOrDivision = dto.EducationHSC.Division ?? string.Empty;
            hsc.GroupOrMajor = dto.EducationHSC.Group ?? string.Empty;
            hsc.PassingYear = dto.EducationHSC.YearPassed ?? string.Empty;
            hsc.Result = dto.EducationHSC.Gpa ?? string.Empty;

            // Diploma
            var diploma = educations.FirstOrDefault(e => e.Level == "Diploma");
            if (diploma == null)
            {
                diploma = new EmployeeEducation { EmployeeProfileId = id, Level = "Diploma" };
                _context.EmployeeEducations.Add(diploma);
            }
            diploma.Institution = dto.EducationDiploma.Institution ?? string.Empty;
            diploma.PassingYear = dto.EducationDiploma.YearPassed ?? string.Empty;
            diploma.Result = dto.EducationDiploma.Cgpa ?? string.Empty;

            // Graduation
            var graduate = educations.FirstOrDefault(e => e.Level == "Graduation");
            if (graduate == null)
            {
                graduate = new EmployeeEducation { EmployeeProfileId = id, Level = "Graduation" };
                _context.EmployeeEducations.Add(graduate);
            }
            graduate.Institution = dto.EducationGraduate.Institution ?? string.Empty;
            graduate.PassingYear = dto.EducationGraduate.PassedYear ?? string.Empty;
            graduate.GroupOrMajor = dto.EducationGraduate.Degree ?? string.Empty;
            graduate.Result = dto.EducationGraduate.Result ?? string.Empty;
            graduate.Achievement = dto.EducationGraduate.Achievement ?? string.Empty;
            graduate.AchievementFile = dto.EducationGraduate.AchievementFile ?? string.Empty;

            // PostGraduation
            var postGrad = educations.FirstOrDefault(e => e.Level == "PostGraduation");
            if (postGrad == null)
            {
                postGrad = new EmployeeEducation { EmployeeProfileId = id, Level = "PostGraduation" };
                _context.EmployeeEducations.Add(postGrad);
            }
            postGrad.Institution = dto.EducationPostGraduate.Institution ?? string.Empty;
            postGrad.PassingYear = dto.EducationPostGraduate.PassedYear ?? string.Empty;
            postGrad.GroupOrMajor = dto.EducationPostGraduate.Degree ?? string.Empty;
            postGrad.Result = dto.EducationPostGraduate.Result ?? string.Empty;
            postGrad.Achievement = dto.EducationPostGraduate.Achievement ?? string.Empty;
            postGrad.AchievementFile = dto.EducationPostGraduate.AchievementFile ?? string.Empty;

            // Save/Upsert Document details
            var document = await _context.EmployeeDocuments
                .FirstOrDefaultAsync(d => d.EmployeeProfileId == id);

            if (document == null)
            {
                document = new EmployeeDocument { EmployeeProfileId = id };
                _context.EmployeeDocuments.Add(document);
            }
            document.Resume = dto.Documents.Resume ?? string.Empty;
            document.NidEmpFront = dto.Documents.NidEmpFront ?? string.Empty;
            document.NidEmpBack = dto.Documents.NidEmpBack ?? string.Empty;
            document.NidNomFront = dto.Documents.NidNomFront ?? string.Empty;
            document.NidNomBack = dto.Documents.NidNomBack ?? string.Empty;
            document.Covid = dto.Documents.Covid ?? string.Empty;
            document.Release = dto.Documents.Release ?? string.Empty;
            document.Payslip = dto.Documents.Payslip ?? string.Empty;
            document.Tax = dto.Documents.Tax ?? string.Empty;

            _context.Entry(profile).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Retrieves all employee profiles in the database with computed completion metrics.
        /// GET /api/EmployeeProfile
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllProfiles()
        {
            var profiles = await _context.EmployeeProfiles.ToListAsync();
            var educationsList = await _context.EmployeeEducations.ToListAsync();
            var documentsList = await _context.EmployeeDocuments.ToListAsync();

            var result = profiles.Select(profile =>
            {
                var educations = educationsList.Where(e => e.EmployeeProfileId == profile.Id).ToList();
                var document = documentsList.FirstOrDefault(d => d.EmployeeProfileId == profile.Id);

                var completion = CalculateCompletion(profile, educations, document);
                var isVerified = !string.IsNullOrEmpty(profile.Nid)
                    && document != null
                    && !string.IsNullOrEmpty(document.NidEmpFront)
                    && !string.IsNullOrEmpty(document.NidEmpBack);

                return new
                {
                    code = profile.EmployeeCode,
                    name = profile.Name,
                    department = profile.Department,
                    empStat = profile.EmpStat,
                    doj = profile.JoinDate.ToString("yyyy-MM-dd"),
                    gradeGroup = profile.GradeGroup,
                    designation = profile.Designation,
                    jobGrade = profile.JobGrade,
                    dob = profile.Dob.ToString("yyyy-MM-dd"),
                    eqGrade = profile.EqGrade,
                    locationOutlet = profile.Showroom,
                    confirmDate = profile.ConfirmDate,
                    completion = completion,
                    isVerified = isVerified
                };
            }).OrderBy(e => e.code).ToList();

            return Ok(result);
        }

        /// <summary>
        /// Imports or updates a batch of employee records from an admin-uploaded spreadsheet.
        /// POST /api/EmployeeProfile/import
        /// </summary>
        [HttpPost("import")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ImportProfiles([FromBody] List<EmployeeImportDto> dtos)
        {
            if (dtos == null || dtos.Count == 0)
            {
                return BadRequest("No employee records to import.");
            }

            try
            {
                foreach (var dto in dtos)
                {
                    if (string.IsNullOrWhiteSpace(dto.Code)) continue;

                    var code = dto.Code.Trim();
                    var profile = await _context.EmployeeProfiles
                        .FirstOrDefaultAsync(p => p.EmployeeCode.ToLower() == code.ToLower());

                    if (profile != null)
                    {
                        // Update existing record with spreadsheet values, preserving password/personal details
                        profile.Name = !string.IsNullOrWhiteSpace(dto.Name) ? dto.Name.Trim() : profile.Name;
                        profile.Department = dto.Department ?? string.Empty;
                        profile.EmpStat = dto.EmpStat ?? string.Empty;
                        profile.Designation = dto.Designation ?? string.Empty;
                        profile.GradeGroup = dto.GradeGroup ?? string.Empty;
                        profile.JobGrade = dto.JobGrade ?? string.Empty;
                        profile.EqGrade = dto.EqGrade ?? string.Empty;
                        profile.Showroom = dto.LocationOutlet ?? string.Empty;
                        profile.ConfirmDate = dto.ConfirmDate ?? string.Empty;

                        if (!string.IsNullOrWhiteSpace(dto.Doj))
                        {
                            profile.JoinDate = ParseDateOrDefault(dto.Doj, profile.JoinDate);
                        }
                        if (!string.IsNullOrWhiteSpace(dto.Dob))
                        {
                            profile.Dob = ParseDateOrDefault(dto.Dob, profile.Dob);
                        }
                        
                        _context.Entry(profile).State = EntityState.Modified;
                    }
                    else
                    {
                        // Create new employee registry record
                        var newProfile = new EmployeeProfile
                        {
                            EmployeeCode = code,
                            Name = dto.Name ?? "Unknown Employee",
                            Department = dto.Department ?? string.Empty,
                            EmpStat = dto.EmpStat ?? string.Empty,
                            Designation = dto.Designation ?? string.Empty,
                            GradeGroup = dto.GradeGroup ?? string.Empty,
                            JobGrade = dto.JobGrade ?? string.Empty,
                            EqGrade = dto.EqGrade ?? string.Empty,
                            Showroom = dto.LocationOutlet ?? string.Empty,
                            ConfirmDate = dto.ConfirmDate ?? string.Empty,
                            JoinDate = ParseDateOrDefault(dto.Doj, DateTime.UtcNow),
                            Dob = ParseDateOrDefault(dto.Dob, new DateTime(1970, 1, 1).ToUniversalTime()),
                            
                            // Default empty values for other required DB schema constraints
                            FatherName = string.Empty,
                            MotherName = string.Empty,
                            PresentAddress = string.Empty,
                            PermanentAddress = string.Empty,
                            Phone = string.Empty,
                            Email = string.Empty,
                            Nationality = string.Empty,
                            Religion = string.Empty,
                            Sex = string.Empty,
                            BloodType = string.Empty,
                            Nid = string.Empty,
                            MaritalStatus = string.Empty,
                            PasswordHash = string.Empty
                        };

                        _context.EmployeeProfiles.Add(newProfile);
                        await _context.SaveChangesAsync(); // save to get newProfile.Id

                        // Create 5 default blank education records
                        var eduLevels = new[] { "SSC", "HSC", "Diploma", "Graduation", "PostGraduation" };
                        foreach (var level in eduLevels)
                        {
                            _context.EmployeeEducations.Add(new EmployeeEducation
                            {
                                EmployeeProfileId = newProfile.Id,
                                Level = level,
                                Institution = string.Empty,
                                BoardOrDivision = string.Empty,
                                GroupOrMajor = string.Empty,
                                PassingYear = string.Empty,
                                Result = string.Empty,
                                Achievement = string.Empty,
                                AchievementFile = string.Empty
                            });
                        }

                        // Create 1 default blank document record
                        _context.EmployeeDocuments.Add(new EmployeeDocument
                        {
                            EmployeeProfileId = newProfile.Id,
                            Resume = string.Empty,
                            NidEmpFront = string.Empty,
                            NidEmpBack = string.Empty,
                            NidNomFront = string.Empty,
                            NidNomBack = string.Empty,
                            Covid = string.Empty,
                            Release = string.Empty,
                            Payslip = string.Empty,
                            Tax = string.Empty
                        });
                    }
                }

                await _context.SaveChangesAsync();
                return Ok(new { Message = "Employee batch import successful." });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Import error: {ex}");
                var detailedError = ex.Message;
                if (ex.InnerException != null)
                {
                    detailedError += " -> " + ex.InnerException.Message;
                }
                return BadRequest(detailedError);
            }
        }

        private static int CalculateCompletion(EmployeeProfile profile, List<EmployeeEducation> educations, EmployeeDocument? document)
        {
            if (string.IsNullOrEmpty(profile.PasswordHash))
            {
                return 0;
            }

            double total = 0;

            // 13 Personal Fields
            int personalCount = 0;
            if (!string.IsNullOrEmpty(profile.Name)) personalCount++;
            if (!string.IsNullOrEmpty(profile.FatherName)) personalCount++;
            if (!string.IsNullOrEmpty(profile.MotherName)) personalCount++;
            if (!string.IsNullOrEmpty(profile.PresentAddress)) personalCount++;
            if (!string.IsNullOrEmpty(profile.PermanentAddress)) personalCount++;
            if (profile.Dob != DateTime.MinValue && profile.Dob != new DateTime(1970, 1, 1).ToUniversalTime() && profile.Dob != new DateTime(1969, 12, 31).ToUniversalTime()) personalCount++;
            if (!string.IsNullOrEmpty(profile.Phone)) personalCount++;
            if (!string.IsNullOrEmpty(profile.Email)) personalCount++;
            if (!string.IsNullOrEmpty(profile.Nationality)) personalCount++;
            if (!string.IsNullOrEmpty(profile.Religion)) personalCount++;
            if (!string.IsNullOrEmpty(profile.Sex)) personalCount++;
            if (!string.IsNullOrEmpty(profile.BloodType)) personalCount++;
            if (!string.IsNullOrEmpty(profile.Nid)) personalCount++;

            total += Math.Round(((double)personalCount / 13) * 40);

            // Education
            if (educations.Any(e => e.Level == "SSC" && !string.IsNullOrEmpty(e.Institution))) total += 8;
            if (educations.Any(e => (e.Level == "HSC" || e.Level == "Diploma") && !string.IsNullOrEmpty(e.Institution))) total += 8;
            if (educations.Any(e => e.Level == "Graduation" && !string.IsNullOrEmpty(e.Institution))) total += 9;

            // Documents
            if (document != null)
            {
                if (!string.IsNullOrEmpty(document.Resume)) total += 5;
                if (!string.IsNullOrEmpty(document.NidEmpFront)) total += 5;
                if (!string.IsNullOrEmpty(document.NidEmpBack)) total += 5;
                if (!string.IsNullOrEmpty(document.NidNomFront)) total += 2.5;
                if (!string.IsNullOrEmpty(document.NidNomBack)) total += 2.5;
                if (!string.IsNullOrEmpty(document.Covid)) total += 5;
            }

            // Account Created / Password Hash
            if (!string.IsNullOrEmpty(profile.PasswordHash)) total += 10;

            return Math.Min(100, (int)Math.Round(total));
        }

        private static DateTime ParseDateOrDefault(string dateStr, DateTime defaultDate)
        {
            if (string.IsNullOrWhiteSpace(dateStr)) return defaultDate;
            if (DateTime.TryParse(dateStr, out var parsedDate))
            {
                return DateTime.SpecifyKind(parsedDate, DateTimeKind.Utc);
            }
            return defaultDate;
        }
    }

    public class EmployeeImportDto
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string EmpStat { get; set; } = string.Empty;
        public string Doj { get; set; } = string.Empty;
        public string GradeGroup { get; set; } = string.Empty;
        public string Designation { get; set; } = string.Empty;
        public string JobGrade { get; set; } = string.Empty;
        public string Dob { get; set; } = string.Empty;
        public string EqGrade { get; set; } = string.Empty;
        public string LocationOutlet { get; set; } = string.Empty;
        public string ConfirmDate { get; set; } = string.Empty;
    }
}
