using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagement.Models
{
    // lương
    [Table("salary")]
    public class Salary : BaseEntity
    {
        [Column("employee_id")]
        public int EmployeeID { get; set; }  // Nhân viên ID

        [Column("month")]
        public int Month { get; set; }  // Tháng

        [Column("year")]
        public int Year { get; set; }  // Năm

        [Column("basic_salary")]
        public decimal BasicSalary { get; set; }  // Lương cơ bản

        [Column("bonus")]
        public decimal Bonus { get; set; }  // Tiền thưởng

        [Column("allowance")]
        public decimal Allowance { get; set; }  // Tiền phụ cấp

        [Column("insurance")]
        public int Insurance { get; set; }  // Tiền bảo hiểm

        [Column("penalty")]
        public decimal Penalty { get; set; }  // Tiền phạt

        [Column("salary_advance")]
        public decimal SalaryAdvance { get; set; }  // Ứng lương

        [Column("net_salary")]
        public decimal NetSalary { get; set; }  // Lương thực nhận
        [Column("total_attendance")]

        public int? TotalAttendance { get; set; } // Tổng số ngày công
        public virtual Employee? Employee { get; set; }
    }
}