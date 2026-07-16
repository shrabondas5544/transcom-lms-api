using System;
using System.ComponentModel.DataAnnotations;

namespace transcom_lms_api.Models
{
    /// <summary>
    /// Represents the geofencing details configured for a showroom or office location.
    /// Used to validate if employees are within range during attendance logging.
    /// </summary>
    public class LocationGeofence
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string LocationName { get; set; } = string.Empty;

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        [Required]
        public int AllowedRadiusMeters { get; set; } = 200;
    }
}
