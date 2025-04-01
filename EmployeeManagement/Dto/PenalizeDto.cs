namespace EmployeeManagement.Dto
{
    public class PenalizeDto : BaseEntityDto
    {
        public int EmployeeID { get; set; }  // Nhân viên ID
        public string? PenalizeName { get; set; }  // Tên phạt
        public DateTime PenalizeDate { get; set; }  // Ngày phạt
        public string? Description { get; set; }  // Mô tả
        public decimal Amount { get; set; }  // Số tiền
        public string? EmployeeName { get; set; }  // Tên nhân viên
        public string? Email { get; set; }  // Email
        public string? Username { get; set; }
        public string? AmountString { get; set; }  // Số tiền
    }
}
