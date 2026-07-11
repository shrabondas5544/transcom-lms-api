using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace transcom_lms_api.Migrations
{
    /// <inheritdoc />
    public partial class InitialPostgresCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmployeeProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmployeeCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    FatherName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    MotherName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PresentAddress = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    PermanentAddress = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Dob = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Nationality = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Religion = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Sex = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    BloodType = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    Nid = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    MaritalStatus = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    MarriageDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Passport = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Hobbies = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Facebook = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Instagram = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    XLink = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Linkedin = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Designation = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Showroom = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Division = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Department = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    JoinDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PasswordHash = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeProfiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmployeeProfileId = table.Column<int>(type: "integer", nullable: false),
                    Resume = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    NidEmpFront = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    NidEmpBack = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    NidNomFront = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    NidNomBack = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Covid = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Release = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Payslip = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Tax = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeDocuments_EmployeeProfiles_EmployeeProfileId",
                        column: x => x.EmployeeProfileId,
                        principalTable: "EmployeeProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeEducations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmployeeProfileId = table.Column<int>(type: "integer", nullable: false),
                    Level = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Institution = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    BoardOrDivision = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    GroupOrMajor = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PassingYear = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Result = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Achievement = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    AchievementFile = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeEducations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeEducations_EmployeeProfiles_EmployeeProfileId",
                        column: x => x.EmployeeProfileId,
                        principalTable: "EmployeeProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeDocuments_EmployeeProfileId",
                table: "EmployeeDocuments",
                column: "EmployeeProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeEducations_EmployeeProfileId",
                table: "EmployeeEducations",
                column: "EmployeeProfileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeDocuments");

            migrationBuilder.DropTable(
                name: "EmployeeEducations");

            migrationBuilder.DropTable(
                name: "EmployeeProfiles");
        }
    }
}
