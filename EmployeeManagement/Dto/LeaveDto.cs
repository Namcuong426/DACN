namespace EmployeeManagement.Dto
{
    public class LeaveDto : BaseEntityDto
    {

        public string? ReasonLeave { get; set; }  // Lý do nghỉ phép

        public DateTime? DateLeave { get; set; }  // Ngày nghỉ phép

        public int LeavePermission { get; set; } // Có phép., Không phép

        public int Status { get; set; }  // Trạng thái : Đang chờ, Từ chối, Chấp thuận

        public int EmployeeId { get; set; }

        public string? EmployeeName { get; set; }
    }
}
