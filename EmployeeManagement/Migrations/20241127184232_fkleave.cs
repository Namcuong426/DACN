using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeManagement.Migrations
{
    /// <inheritdoc />
    public partial class fkleave : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_leave_employee_EmployeeId",
                table: "leave");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "leave",
                newName: "employee_id");

            migrationBuilder.RenameIndex(
                name: "IX_leave_EmployeeId",
                table: "leave",
                newName: "IX_leave_employee_id");

            migrationBuilder.AlterColumn<int>(
                name: "employee_id",
                table: "leave",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_leave_employee_employee_id",
                table: "leave",
                column: "employee_id",
                principalTable: "employee",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_leave_employee_employee_id",
                table: "leave");

            migrationBuilder.RenameColumn(
                name: "employee_id",
                table: "leave",
                newName: "EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_leave_employee_id",
                table: "leave",
                newName: "IX_leave_EmployeeId");

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                table: "leave",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_leave_employee_EmployeeId",
                table: "leave",
                column: "EmployeeId",
                principalTable: "employee",
                principalColumn: "id");
        }
    }
}
