namespace EmployeeManagement.Dto
{
    public class EmployeeDto : BaseEntityDto
    {
        public string? Name { get; set; }  // Tên
        public DateTime DateOfBirth { get; set; }  // Ngày sinh
        public string? Gender { get; set; }  // Giới tính
        public string? Address { get; set; }  // Địa chỉ
        public string? PhoneNumber { get; set; }  // Số điện thoại
        public string? Email { get; set; }  // Email
        public DateTime HireDate { get; set; }  // Ngày vào làm
        public bool ForeignLanguageProficiency { get; set; }  // Trình độ ngoại ngữ
        public string? CitizenID { get; set; }  // CCCD
        public int DepartmentID { get; set; }  // Phòng ban ID
        public int EducationLevelID { get; set; }  // Trình độ ID
        public int PositionID { get; set; }  // Chức vụ ID
        public decimal BasicSalary { get; set; }  // Lương cơ bản
        public string? Username { get; set; } // Tên đăng nhập
        public string? Password { get; set; } // Mật khẩu
        public bool? IsAdmin { get; set; } // Quyền admin
        public bool? IsActivated { get; set; } // Trạng thái kích hoạt

        public string? PositionName { get; set; }
        public string? DepartmentName { get; set; }
        public string? EducationLevelName { get; set; }
        public int Role { get; set; }
        public int Status { get; set; }
        public string? EmployeeCode { get; set; }
        public string? EmployeeName { get; set; }
        public int EmployeeID { get; set; }  // Nhân viên ID
        public DateTime Date { get; set; }  // Ngày
        public string? Notes { get; set; }  // Ghi chú
        public bool? IsAccepted { get; set; } = true;  // Đã chấm công

        public string? Avatar { get; set; }  // Ảnh đại diện
        public IFormFile? AvatarFile { get; set; } = null;

        public string? BasicSalaryString { get; set; }  // Lương cơ bản
    }
    public class EmployeeViewDto() : BaseEntityDto
    {
        public string? Name { get; set; }  // Tên
        public string? Email { get; set; }  // Email
        public string? Username { get; set; }
        public string? Avatar { get; set; }
    }
}