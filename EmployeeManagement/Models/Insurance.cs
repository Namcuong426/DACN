using EmployeeManagement.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    // Bảo hiểm
    [Table("insurance")]
    public class Insurance : BaseEntity
    {
        [Column("employee_id")]
        public int EmployeeID { get; set; }  // Nhân viên ID

        [Column("insurance_name")]
        public string? InsuranceName { get; set; }  // Tên bảo hiểm

        [Column("premium_amount")]
        public int PremiumAmount { get; set; }  // Phí bảo hiểm

        [Column("start_date")]
        public DateTime StartDate { get; set; }  // Ngày bắt đầu bảo hiểm

        [Column("end_date")]
        public DateTime EndDate { get; set; }  // Ngày hết hạn bảo hiểm

        [Column("description")]
        public string? Description { get; set; }  // Mô tả
    }

    public class InsuranceIndexViewModel
    {
        public List<Insurance> Insurances { get; set; } = new List<Insurance>();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
