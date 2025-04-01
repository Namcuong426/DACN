using EmployeeManagement.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagement.Models
{
    // tài khoản
    [Table("account")]
    public class Account : BaseEntity
    {
        [Column("username")]
        public string? Username { get; set; }  // Tên đăng nhập

        [Column("password")]
        public string? Password { get; set; }  // Mật khẩu

        [Column("role")]
        public Role? Role { get; set; } = Enum.Role.USER;  // Quyền hạn
    }
}