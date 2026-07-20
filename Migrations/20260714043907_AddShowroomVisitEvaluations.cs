using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace transcom_lms_api.Migrations
{
    /// <inheritdoc />
    public partial class AddShowroomVisitEvaluations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShowroomVisitEvaluations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmployeeProfileId = table.Column<int>(type: "integer", nullable: false),
                    VisitDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TotalScore = table.Column<double>(type: "double precision", nullable: false),
                    CustomerDealingScore = table.Column<int>(type: "integer", nullable: false),
                    CustomerDealingRemarks = table.Column<string>(type: "text", nullable: false),
                    ProductKnowledgeScore = table.Column<int>(type: "integer", nullable: false),
                    ProductKnowledgeRemarks = table.Column<string>(type: "text", nullable: false),
                    GroomingScore = table.Column<int>(type: "integer", nullable: false),
                    GroomingRemarks = table.Column<string>(type: "text", nullable: false),
                    DemonstrationSkillScore = table.Column<int>(type: "integer", nullable: false),
                    DemonstrationSkillRemarks = table.Column<string>(type: "text", nullable: false),
                    DisciplineScore = table.Column<int>(type: "integer", nullable: false),
                    DisciplineRemarks = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShowroomVisitEvaluations", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShowroomVisitEvaluations");
        }
    }
}
