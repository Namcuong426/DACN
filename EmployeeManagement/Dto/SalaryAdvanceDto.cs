namespace EmployeeManagement.Dto
{
    public class SalaryAdvanceDto : BaseEntityDto
    {
        public int EmployeeID { get; set; }  // Nhân viên ID
        public DateTime AdvanceDate { get; set; }  // Ngày ứng lương
        public decimal AdvanceAmount { get; set; }  // Số tiền ứng lương
        public string? Description { get; set; }  // Mô
        public string? AdvanceAmountString { get; set; }  // Số tiền ứng lương
        public string? EmployeeCode { get; set; }  // Mã nhân viên
        public string? EmployeeName { get; set; }  // Tên nhân viên
    }
}
