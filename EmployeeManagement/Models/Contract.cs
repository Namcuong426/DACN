using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagement.Models
{
    // hợp đồng
    [Table("contract")]
    public class Contract : BaseEntity
    {
        [Column("employee_id")]

        public int EmployeeID { get; set; }  // Nhân viên ID

        [Column("contract_type")]
        public string? ContractType { get; set; }  // Loại hợp đồng

        [Column("start_date")]
        public DateTime StartDate { get; set; }  // Ngày bắt đầu

        [Column("end_date")]
        public DateTime EndDate { get; set; }  // Ngày kết thúc

        [Column("contract_content")]
        public string? ContractContent { get; set; }  // Nội dung hợp đồng

        [Column("notes")]
        public string? Notes { get; set; }  // Ghi chú
        public virtual Employee? Employee { get; set; }

    }
}