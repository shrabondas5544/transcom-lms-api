using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using transcom_lms_api.Data;
using transcom_lms_api.Models;

namespace transcom_lms_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Check if an Employee ID exists in the database and whether an account has already been created.
        /// </summary>
        [HttpGet("check-id/{employeeCode}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult CheckEmployeeId(string employeeCode)
        {
            var code = employeeCode.Trim();
            var profile = _context.EmployeeProfiles.FirstOrDefault(p =>
                p.EmployeeCode.ToLower() == code.ToLower());

            if (profile == null)
            {
                return Ok(new CheckIdResponse
                {
                    Exists = false,
                    AccountCreated = false,
                    Name = "",
                    Message = "Employee ID not found in the registry."
                });
            }

            // If PasswordHash is set, account has been created
            var hasAccount = !string.IsNullOrEmpty(profile.PasswordHash);

            return Ok(new CheckIdResponse
            {
                Exists = true,
                AccountCreated = hasAccount,
                Name = profile.Name,
                Message = hasAccount
                    ? "An account has already been created for this Employee ID."
                    : "Employee ID is available. You can create an account."
            });
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Register([FromBody] RegisterRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.EmployeeCode) ||
                string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest("Employee ID, email, and password are required.");
            }

            var code = request.EmployeeCode.Trim();
            var emailNormalized = request.Email.Trim().ToLower();

            // Find the pre-existing profile by EmployeeCode
            var profile = _context.EmployeeProfiles.FirstOrDefault(p =>
                p.EmployeeCode.ToLower() == code.ToLower());

            if (profile == null)
            {
                return BadRequest("Employee ID not found in the registry. Please contact your administrator.");
            }

            // Check if account already created
            if (!string.IsNullOrEmpty(profile.PasswordHash))
            {
                return BadRequest("An account has already been created for this Employee ID.");
            }

            // Check if email is already in use by another profile
            if (_context.EmployeeProfiles.Any(p => p.Email.ToLower() == emailNormalized && p.Id != profile.Id))
            {
                return BadRequest("Email address is already in use by another account.");
            }

            // Securely hash the password
            var passwordHash = HashPassword(request.Password);

            // Update the existing profile with account credentials
            profile.Email = emailNormalized;
            profile.PasswordHash = passwordHash;

            // If education entries don't exist yet, create them
            var existingEdu = _context.EmployeeEducations.Where(e => e.EmployeeProfileId == profile.Id).ToList();
            if (existingEdu.Count == 0)
            {
                var eduLevels = new[] { "SSC", "HSC", "Diploma", "Graduation", "PostGraduation" };
                foreach (var level in eduLevels)
                {
                    _context.EmployeeEducations.Add(new EmployeeEducation
                    {
                        EmployeeProfileId = profile.Id,
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
            }

            // If document entry doesn't exist yet, create it
            var existingDoc = _context.EmployeeDocuments.FirstOrDefault(d => d.EmployeeProfileId == profile.Id);
            if (existingDoc == null)
            {
                _context.EmployeeDocuments.Add(new EmployeeDocument
                {
                    EmployeeProfileId = profile.Id,
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

            _context.SaveChanges();

            return Ok(new { Message = "Account created successfully! Please sign in." });
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Identifier) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest("Credentials are required.");
            }

            var identifier = request.Identifier.Trim().ToLower();
            var hashedPassword = HashPassword(request.Password);

            // Match by Email or EmployeeCode
            var profile = _context.EmployeeProfiles.FirstOrDefault(p =>
                p.Email.ToLower() == identifier || p.EmployeeCode.ToLower() == identifier);

            if (profile == null || profile.PasswordHash != hashedPassword)
            {
                return BadRequest("Invalid email/employee ID or password.");
            }

            // Determine verification status
            var docs = _context.EmployeeDocuments.FirstOrDefault(d => d.EmployeeProfileId == profile.Id);
            var isVerified = !string.IsNullOrEmpty(profile.Nid)
                && docs != null
                && !string.IsNullOrEmpty(docs.NidEmpFront)
                && !string.IsNullOrEmpty(docs.NidEmpBack);

            return Ok(new LoginResponse
            {
                Id = profile.Id,
                EmployeeCode = profile.EmployeeCode,
                Name = profile.Name,
                Email = profile.Email,
                Designation = profile.Designation,
                Showroom = profile.Showroom,
                IsVerified = isVerified
            });
        }

        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }

    public class CheckIdResponse
    {
        public bool Exists { get; set; }
        public bool AccountCreated { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }

    public class RegisterRequest
    {
        public string EmployeeCode { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class LoginRequest
    {
        public string Identifier { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class LoginResponse
    {
        public int Id { get; set; }
        public string EmployeeCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Designation { get; set; } = string.Empty;
        public string Showroom { get; set; } = string.Empty;
        public bool IsVerified { get; set; }
    }
}
