using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagement.Models
{
    // nhan vien
    [Table("employee")]
    public class Employee : BaseEntity
    {
        [Column("name")]
        public string? Name { get; set; }  // Tên

        [Column("date_of_birth")]
        public DateTime DateOfBirth { get; set; }  // Ngày sinh

        [Column("gender")]
        public string? Gender { get; set; }  // Giới tính

        [Column("avatar")]
        public string? Avatar { get; set; }  // Ảnh đại diện

        [Column("address")]
        public string? Address { get; set; }  // Địa chỉ
        [Column("employee_code")]

        public string? EmployeeCode { get; set; } // Mã nhân viên

        [Column("phone_number")]
        public string? PhoneNumber { get; set; }  // Số điện thoại

        [Column("email")]
        public string? Email { get; set; }  // Email

        [Column("hire_date")]
        public DateTime HireDate { get; set; }  // Ngày vào làm

        [Column("foreign_language_proficiency")]
        public bool ForeignLanguageProficiency { get; set; }  // Trình độ ngoại ngữ

        [Column("citizen_id")]
        public string? CitizenID { get; set; }  // CCCD

        [Column("department_id")]
        public int DepartmentID { get; set; }  // Phòng ban ID

        [Column("education_level_id")]
        public int EducationLevelID { get; set; }  // Trình độ ID

        [Column("position_id")]
        public int PositionID { get; set; }  // Chức vụ ID

        [Column("basic_salary")]
        public decimal BasicSalary { get; set; }  // Lương cơ bản
        public string? Username { get; set; } // Tên đăng nhập
        public string? Password { get; set; } // Mật khẩu
        public bool? IsAdmin { get; set; } // Quyền admin
        public bool? IsActivated { get; set; } // Trạng thái kích hoạt
        public virtual Department? Department { get; set; }
        public virtual EducationLevel? EducationLevel { get; set; }
        public virtual Position? Position { get; set; }

        // 1 nhân viên => nhiều đơn nghỉ phép
        public virtual ICollection<Leave> Leaves { get; set; } = [];
    }
}