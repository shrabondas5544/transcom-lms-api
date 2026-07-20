using System;
using System.ComponentModel.DataAnnotations;

namespace transcom_lms_api.Models
{
    /// <summary>
    /// Audit logging table to track employee selfie and coordinates validation attempts.
    /// Used for security review and debugging check-in failures.
    /// </summary>
    public class AttendanceValidationAttempt
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        [Required]
        public DateTime AttemptTime { get; set; } = DateTime.UtcNow;

        [Required]
        public double SubmittedLatitude { get; set; }

        [Required]
        public double SubmittedLongitude { get; set; }

        [Required]
        [MaxLength(500)]
        public string SelfieUrl { get; set; } = string.Empty;

        [Required]
        public bool IsSuccessfulCheckIn { get; set; }
    }
}
