using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace transcom_lms_api.Migrations
{
    /// <inheritdoc />
    public partial class AddImportFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConfirmDate",
                table: "EmployeeProfiles",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EmpStat",
                table: "EmployeeProfiles",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EqGrade",
                table: "EmployeeProfiles",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GradeGroup",
                table: "EmployeeProfiles",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "JobGrade",
                table: "EmployeeProfiles",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConfirmDate",
                table: "EmployeeProfiles");

            migrationBuilder.DropColumn(
                name: "EmpStat",
                table: "EmployeeProfiles");

            migrationBuilder.DropColumn(
                name: "EqGrade",
                table: "EmployeeProfiles");

            migrationBuilder.DropColumn(
                name: "GradeGroup",
                table: "EmployeeProfiles");

            migrationBuilder.DropColumn(
                name: "JobGrade",
                table: "EmployeeProfiles");
        }
    }
}
