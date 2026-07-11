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
        }
    }
}
