using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagement.Models
{
    // Xử phạt
    [Table("penalize")]
    public class Penalize : BaseEntity
    {
        [Column("employee_id")]
        public int EmployeeID { get; set; }  // Nhân viên ID

        [Column("penalize_name")]
        public string? PenalizeName { get; set; }  // Tên phạt

        [Column("penalize_date")]
        public DateTime PenalizeDate { get; set; }  // Ngày phạt

        [Column("description")]
        public string? Description { get; set; }  // Mô tả

        [Column("amount")]
        public decimal Amount { get; set; }  // Số tiền

        public virtual Employee? Employee { get; set; }
    }

}
