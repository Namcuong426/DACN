using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    // chức vụ
    [Table("position")]
    public class Position : BaseEntity
    {
        [Column("position_name")]
        public string? PositionName { get; set; }  // Tên chức vụ

        [Column("description")]
        public string? Description { get; set; }  // Mô tả
        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
        [Column("abbreviation")]
        public string? Abbreviation { get; set; }  // Viết tắt
    }
}