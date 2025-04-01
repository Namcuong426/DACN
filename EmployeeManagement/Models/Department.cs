using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagement.Models
{
    // phòng ban
    [Table("department")]
    public class Department : BaseEntity
    {
        [Column("department_name")]
        public string? DepartmentName { get; set; }  // Tên phòng ban (e.g., IT)

        [Column("description")]
        public string? Description { get; set; }  // Mô tả

        [Column("abbreviation")]
        public string? Abbreviation { get; set; }  // Viết tắt (e.g., IT)
        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}