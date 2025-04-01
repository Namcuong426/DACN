namespace EmployeeManagement.Dto
{
    public class StatisticLeaveDto : LeaveDto
    {
        public int CountLeaveWithPermission { get; set; } // ngày nghỉ có phép
         
        public int CountLeaveWithoutPermission { get; set; } // ngày nghỉ không phép

    }
}
