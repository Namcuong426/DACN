using EmployeeManagement.Enum;

namespace EmployeeManagement.Dto
{
    public class AccountDto
    {
        public string? Username { get; set; }  // Tên đăng nhập

        public string? Password { get; set; }  // Mật khẩu
        public Role? Role { get; set; } = Enum.Role.USER;  // Quyền hạn
    }
    public class AccountLoginDto
    {
        public string? Username { get; set; }  // Tên đăng nhập

        public string? Password { get; set; }  // Mật khẩu
    }

}
