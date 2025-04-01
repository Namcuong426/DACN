namespace EmployeeManagement.Dto
{
    public class RewardDto : BaseEntityDto
    {
        public int EmployeeID { get; set; }  // Nhân viên ID
        public string? RewardName { get; set; }  // Tên khen thưởng
        public DateTime RewardDate { get; set; }  // Ngày khen thưởng
        public string? Description { get; set; }  // Mô tả
        public decimal Amount { get; set; }  // Số tiền
        public string? EmployeeName { get; set; }  // Tên nhân viên
        public string? Email { get; set; }  // Email
        public string? Username { get; set; }

        public string? AmountString { get; set; }  // Số tiền
    }
}
