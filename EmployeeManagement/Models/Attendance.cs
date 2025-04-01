using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagement.Models
{
    // chấm công
    [Table("attendance")]
    public class Attendance : BaseEntity
    {
        [Column("employee_id")]
        public int EmployeeID { get; set; }  // Nhân viên ID

        [Column("date")]
        public DateTime Date { get; set; }  // Ngày
        [Column("is_accepted")]
        public bool? IsAccepted { get; set; } = false;  // Đã chấm công 

        [Column("notes")]
        public string? Notes { get; set; }  // Ghi chú
        public virtual Employee? Employee { get; set; }
    }
}