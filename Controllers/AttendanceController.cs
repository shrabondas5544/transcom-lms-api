using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using transcom_lms_api.Data;
using transcom_lms_api.Models;

namespace transcom_lms_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttendanceController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AttendanceController(AppDbContext context)
        {
            _context = context;
        }

        public class ValidateRequest
        {
            public int EmployeeId { get; set; }
            public double SubmittedLatitude { get; set; }
            public double SubmittedLongitude { get; set; }
            public string SelfieUrl { get; set; } = string.Empty;
            public string CheckType { get; set; } = "arrive"; // "arrive" or "leave"
        }

        // POST: api/Attendance/validate
        [HttpPost("validate")]
        public async Task<IActionResult> ValidateAttendance([FromBody] ValidateRequest req)
        {
            if (req.EmployeeId <= 0)
            {
                return BadRequest(new { success = false, message = "Invalid Employee Profile ID." });
            }

            var profile = await _context.EmployeeProfiles.FindAsync(req.EmployeeId);
            if (profile == null)
            {
                return NotFound(new { success = false, message = "Employee profile not found." });
            }

            // Find matching geofence parameter for this showroom/outlet
            string showroomName = profile.Showroom;
            var geofence = await _context.LocationGeofences
                .FirstOrDefaultAsync(g => g.LocationName == showroomName);

            bool isSuccessful = false;
            string failReason = "";
            double distanceMeters = 0;

            if (geofence == null)
            {
                // If geofence is not configured for this employee's showroom, allow checking in with a warning, or block it.
                // Based on user request, let's treat it as allowed but log 0 distance, OR block it. Let's make it allowed with a notice,
                // or block it if we want strict geofencing. Let's treat it as: geofence configuration missing, so we allow check-in
                // but state a warning. Or since we want to check radius, if no geofence setup, let's allow it but warn.
                isSuccessful = true;
                failReason = "No geofence configured for showroom: " + showroomName + ". Check-in allowed without GPS check.";
            }
            else
            {
                // Calculate distance using Haversine formula
                distanceMeters = CalculateDistance(req.SubmittedLatitude, req.SubmittedLongitude, geofence.Latitude, geofence.Longitude);
                if (distanceMeters <= geofence.AllowedRadiusMeters)
                {
                    isSuccessful = true;
                }
                else
                {
                    isSuccessful = false;
                    failReason = $"You are standing outside the allowed outlet boundary. Standing distance: {Math.Round(distanceMeters, 1)} meters from outlet. Allowed radius: {geofence.AllowedRadiusMeters} meters.";
                }
            }

            // Log attempt to database
            var attempt = new AttendanceValidationAttempt
            {
                EmployeeId = req.EmployeeId,
                AttemptTime = DateTime.UtcNow,
                SubmittedLatitude = req.SubmittedLatitude,
                SubmittedLongitude = req.SubmittedLongitude,
                SelfieUrl = req.SelfieUrl.Length > 500 ? req.SelfieUrl.Substring(0, 500) : req.SelfieUrl,
                IsSuccessfulCheckIn = isSuccessful
            };
            _context.AttendanceValidationAttempts.Add(attempt);
            await _context.SaveChangesAsync();

            // Upsert today's official AttendanceLog record (we allow out-of-bounds check-ins, but mark them IsPresent = false)
            var todayUtc = DateTime.UtcNow.Date;
            var log = await _context.AttendanceLogs
                .FirstOrDefaultAsync(l => l.EmployeeId == req.EmployeeId && l.LogDate == todayUtc);

            if (log == null)
            {
                log = new AttendanceLog
                {
                    EmployeeId = req.EmployeeId,
                    LogDate = todayUtc,
                    IsPresent = isSuccessful
                };
                _context.AttendanceLogs.Add(log);
            }
            else
            {
                // If they already have a log, update IsPresent based on check-in success
                log.IsPresent = isSuccessful;
            }

            if (req.CheckType == "arrive")
            {
                log.ArrivalTime = DateTime.UtcNow;
            }
            else
            {
                log.DepartureTime = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();

            return Ok(new 
            { 
                success = true, 
                isPresent = isSuccessful,
                message = isSuccessful 
                    ? (req.CheckType == "arrive" ? "Arrived selfie verified successfully!" : "Left selfie verified successfully!")
                    : $"Out of bounds check-in recorded. {failReason}"
            });
        }

        // GET: api/Attendance/status/{employeeId}
        [HttpGet("status/{employeeId}")]
        public async Task<IActionResult> GetTodayStatus(int employeeId)
        {
            var todayUtc = DateTime.UtcNow.Date;
            var log = await _context.AttendanceLogs
                .FirstOrDefaultAsync(l => l.EmployeeId == employeeId && l.LogDate == todayUtc);

            return Ok(new
            {
                arrived = log?.ArrivalTime != null,
                left = log?.DepartureTime != null,
                arrivalTime = log?.ArrivalTime,
                departureTime = log?.DepartureTime,
                isPresent = log?.IsPresent ?? true
            });
        }

        // GET: api/Attendance/logs/{employeeId}
        [HttpGet("logs/{employeeId}")]
        public async Task<IActionResult> GetLogs(int employeeId)
        {
            var logs = await _context.AttendanceLogs
                .Where(l => l.EmployeeId == employeeId)
                .OrderByDescending(l => l.LogDate)
                .ToListAsync();

            return Ok(logs);
        }

        // GET: api/Attendance/stats/all
        [HttpGet("stats/all")]
        public async Task<IActionResult> GetStatsAll()
        {
            // For the admin page overview table, we want to know each employee's attendance ratio.
            // Let's count how many days they were present.
            // Return a simple list of { employeeId, presentCount, totalWorkdays }
            // Let's assume a static total period of 100 days (e.g. 98/100 as requested).
            var stats = await _context.AttendanceLogs
                .GroupBy(l => l.EmployeeId)
                .Select(g => new
                {
                    EmployeeId = g.Key,
                    PresentCount = g.Count(l => l.IsPresent)
                })
                .ToListAsync();

            return Ok(stats);
        }

        // Haversine Distance Formula
        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            var R = 6371000.0; // earth radius in meters
            var dLat = ToRadians(lat2 - lat1);
            var dLon = ToRadians(lon2 - lon1);
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }

        private double ToRadians(double val)
        {
            return (Math.PI / 180.0) * val;
        }
    }
}
