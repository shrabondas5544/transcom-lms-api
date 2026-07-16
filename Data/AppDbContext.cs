using System;
using Microsoft.EntityFrameworkCore;
using transcom_lms_api.Models;

namespace transcom_lms_api.Data
{
    /// <summary>
    /// Database context responsible for managing database connections and mapping C# model classes to database tables.
    /// </summary>
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Represents the database table containing employee profile records
        public DbSet<EmployeeProfile> EmployeeProfiles { get; set; }
        
        // Represents the database table containing employee education records
        public DbSet<EmployeeEducation> EmployeeEducations { get; set; }

        // Represents the database table containing employee uploaded documents
        public DbSet<EmployeeDocument> EmployeeDocuments { get; set; }

        // Represents the database table containing showroom visit evaluations
        public DbSet<ShowroomVisitEvaluation> ShowroomVisitEvaluations { get; set; }

        // Represents the database table containing location geofences
        public DbSet<LocationGeofence> LocationGeofences { get; set; }

        // Represents the database table containing official accepted attendance records
        public DbSet<AttendanceLog> AttendanceLogs { get; set; }

        // Represents the database table containing attendance selfie validation attempts
        public DbSet<AttendanceValidationAttempt> AttendanceValidationAttempts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure table mappings
            modelBuilder.Entity<EmployeeProfile>()
                .ToTable("EmployeeProfiles");

            modelBuilder.Entity<EmployeeEducation>()
                .ToTable("EmployeeEducations");

            modelBuilder.Entity<EmployeeDocument>()
                .ToTable("EmployeeDocuments");

            modelBuilder.Entity<ShowroomVisitEvaluation>()
                .ToTable("ShowroomVisitEvaluations");

            modelBuilder.Entity<LocationGeofence>()
                .ToTable("LocationGeofences");

            modelBuilder.Entity<LocationGeofence>()
                .HasIndex(l => l.LocationName)
                .IsUnique();

            modelBuilder.Entity<AttendanceLog>()
                .ToTable("AttendanceLogs");

            modelBuilder.Entity<AttendanceLog>()
                .HasIndex(a => new { a.EmployeeId, a.LogDate })
                .IsUnique();

            modelBuilder.Entity<AttendanceValidationAttempt>()
                .ToTable("AttendanceValidationAttempts");
        }
    }
}
