using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace transcom_lms_api.Migrations
{
    /// <inheritdoc />
    public partial class AddAssessorRoleConfigurationFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CanConductAudit",
                table: "EmployeeProfiles",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanTakeAssessment",
                table: "EmployeeProfiles",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAssessor",
                table: "EmployeeProfiles",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanConductAudit",
                table: "EmployeeProfiles");

            migrationBuilder.DropColumn(
                name: "CanTakeAssessment",
                table: "EmployeeProfiles");

            migrationBuilder.DropColumn(
                name: "IsAssessor",
                table: "EmployeeProfiles");
        }
    }
}
