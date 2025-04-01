namespace EmployeeManagement.Dto
{
    public class AttendanceDto : BaseEntityDto
    {
        public int EmployeeID { get; set; }  // Nhân viên ID
        public DateTime Date { get; set; }  // Ngày
        public string? Notes { get; set; }  // Ghi chú
        public bool? IsAccepted { get; set; } = true;  // Đã chấm công
        public string? EmployeeName { get; set; }  // Tên nhân viên
        public string? EmployeeCode { get; set; } // Mã nhân viên
        public string? Email { get; set; }  // Email
        public string? Username { get; set; }
    }
}