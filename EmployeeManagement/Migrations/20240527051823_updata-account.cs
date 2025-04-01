using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeManagement.Migrations
{
    /// <inheritdoc />
    public partial class updataaccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_account_employee_EmployeeId",
                table: "account");

            migrationBuilder.DropIndex(
                name: "IX_account_EmployeeId",
                table: "account");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "account");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "account",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_account_EmployeeId",
                table: "account",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_account_employee_EmployeeId",
                table: "account",
                column: "EmployeeId",
                principalTable: "employee",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
