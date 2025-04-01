using EmployeeManagement.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    // phụ cấp
    [Table("allowance")]
    public class Allowance : BaseEntity
    {
        [Column("employee_id")]
        public int EmployeeID { get; set; }  // Nhân viên ID

        [Column("allowance_name")]
        public string? AllowanceName { get; set; }  // Tên phụ cấp

        [Column("amount")]
        public decimal Amount { get; set; }  // Số tiền

        [Column("start_date")]
        public DateTime StartDate { get; set; }  // Ngày cấp

        [Column("end_date")]
        public DateTime EndDate { get; set; }  // Ngày hết hạn

        [Column("description")]
        public string? Description { get; set; }  // Mô tả
    }
    public class AllowanceIndexViewModel
    {
        public List<Allowance> Allowances { get; set; } = new List<Allowance>();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }

    }

}