using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagement.Models
{
    [Table("education_level")]
    public class EducationLevel : BaseEntity
    {
        [Column("education_name")]
        public string? EducationLevelName { get; set; }  // Tên trình độ (e.g., Đại học, cao đăng)

        [Column("description")]
        public string? Description { get; set; }  // Mô tả

        [Column("abbreviation")]
        public string? Abbreviation { get; set; }  // Viết tắt
        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();


    }
}