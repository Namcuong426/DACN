using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagement.Models
{
    // Ứng lương
    [Table("salary_advance")]
    public class SalaryAdvance : BaseEntity
    {
        [Column("employee_id")]
        public int EmployeeID { get; set; }  // Nhân viên ID

        [Column("advance_date")]
        public DateTime AdvanceDate { get; set; }  // Ngày ứng lương

        [Column("advance_amount")]
        public decimal AdvanceAmount { get; set; }  // Số tiền ứng lương

        [Column("description")]
        public string? Description { get; set; }  // Mô tả
        public virtual Employee? Employee { get; set; }
    }
}