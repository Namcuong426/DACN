using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeManagement.Migrations
{
    /// <inheritdoc />
    public partial class aftertableskill : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_employee_education_level_education_level_id",
                table: "employee");

            migrationBuilder.DropPrimaryKey(
                name: "PK_education_level",
                table: "education_level");

            migrationBuilder.RenameTable(
                name: "education_level",
                newName: "skill");

            migrationBuilder.RenameColumn(
                name: "education_level_name",
                table: "skill",
                newName: "skill_name");

            migrationBuilder.AddColumn<string>(
                name: "abbreviation",
                table: "position",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_skill",
                table: "skill",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_employee_skill_education_level_id",
                table: "employee",
                column: "education_level_id",
                principalTable: "skill",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_employee_skill_education_level_id",
                table: "employee");

            migrationBuilder.DropPrimaryKey(
                name: "PK_skill",
                table: "skill");

            migrationBuilder.DropColumn(
                name: "abbreviation",
                table: "position");

            migrationBuilder.RenameTable(
                name: "skill",
                newName: "education_level");

            migrationBuilder.RenameColumn(
                name: "skill_name",
                table: "education_level",
                newName: "education_level_name");

            migrationBuilder.AddPrimaryKey(
                name: "PK_education_level",
                table: "education_level",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_employee_education_level_education_level_id",
                table: "employee",
                column: "education_level_id",
                principalTable: "education_level",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
