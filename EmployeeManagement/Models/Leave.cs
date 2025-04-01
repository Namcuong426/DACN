using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagement.Models
{
    // Nghỉ phép
    [Table("leave")]
    public class Leave : BaseEntity
    {
       
            [Column("reason_leave")]
            public string? ReasonLeave { get; set; }  // Lý do nghỉ phép

            [Column("date_leave")]
            public DateTime? DateLeave { get; set; }  // Ngày nghỉ phép
        
            [Column("leave_permission")]
            public int LeavePermission { get; set; } // Nghỉ có phép : 1 , Nghỉ không phép : 0 

            [Column("status")]
            public int Status { get; set; }  // Trạng thái : Đang chờ, Từ chối, Chấp thuận

            // 1 đơn nghỉ phép -> 1 nhân viên
            [Column("employee_id")]
            public int EmployeeId { get; set; }
            public virtual Employee Employee { get; set; } = new Employee();
    }
}
