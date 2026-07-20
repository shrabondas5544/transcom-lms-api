using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace transcom_lms_api.Migrations
{
    /// <inheritdoc />
    public partial class AddAvatarToProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AvatarImage",
                table: "EmployeeProfiles",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "AvatarScale",
                table: "EmployeeProfiles",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "AvatarX",
                table: "EmployeeProfiles",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "AvatarY",
                table: "EmployeeProfiles",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvatarImage",
                table: "EmployeeProfiles");

            migrationBuilder.DropColumn(
                name: "AvatarScale",
                table: "EmployeeProfiles");

            migrationBuilder.DropColumn(
                name: "AvatarX",
                table: "EmployeeProfiles");

            migrationBuilder.DropColumn(
                name: "AvatarY",
                table: "EmployeeProfiles");
        }
    }
}
