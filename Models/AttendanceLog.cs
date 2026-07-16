using System;
using System.ComponentModel.DataAnnotations;

namespace transcom_lms_api.Models
{
    /// <summary>
    /// Represents the official accepted attendance records for employees.
    /// Uses composite unique key constraint on EmployeeId and LogDate.
    /// </summary>
    public class AttendanceLog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        [Required]
        public DateTime LogDate { get; set; } = DateTime.UtcNow.Date;

        public DateTime? ArrivalTime { get; set; }

        public DateTime? DepartureTime { get; set; }

        [Required]
        public bool IsPresent { get; set; } = true;
    }
}
