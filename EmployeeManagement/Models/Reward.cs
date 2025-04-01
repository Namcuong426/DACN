using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagement.Models
{
    // khen thưởng
    [Table("reward")]
    public class Reward : BaseEntity
    {
        [Column("employee_id")]
        public int EmployeeID { get; set; }  // Nhân viên ID

        [Column("reward_name")]
        public string? RewardName { get; set; }  // Tên khen thưởng

        [Column("reward_date")]
        public DateTime RewardDate { get; set; }  // Ngày khen thưởng

        [Column("description")]
        public string? Description { get; set; }  // Mô tả

        [Column("amount")]
        public decimal Amount { get; set; }  // Số tiền
        public virtual Employee? Employee { get; set; }
    }
}