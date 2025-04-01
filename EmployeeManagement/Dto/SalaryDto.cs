namespace EmployeeManagement.Dto
{
    public class SalaryDto : BaseEntityDto
    {
        public int EmployeeID { get; set; }  // Nhân viên ID
        public int Month { get; set; }  // Tháng
        public int Year { get; set; }  // Năm
        public decimal BasicSalary { get; set; }  // Lương cơ bản
        public decimal Bonus { get; set; }  // Tiền thưởng
        public decimal Allowance { get; set; }  // Tiền phụ cấp
        public decimal Penalty { get; set; }  // Tiền phạt
        public decimal SalaryAdvance { get; set; }  // Ứng lương
        public int Insurance { get; set; }  // Tiền bảo hiểm
        public decimal NetSalary { get; set; }  // Lương thực nhận
        public string? EmployeeName { get; set; } // Tên nhân viên
        public string? EmployeeCode { get; set; } // Mã nhân viên
        public int? TotalAttendance { get; set; } // Tổng số ngày công
        public string? BasicSalaryString { get; set; }  // Lương cơ bản
        public string? BonusString { get; set; }  // Tiền thưởng
        public string? AllowanceString { get; set; }  // Tiền phụ cấp
        public string? PenaltyString { get; set; }  // Tiền phạt
        public string? SalaryAdvanceString { get; set; }  // Ứng lương
        public string? NetSalaryString { get; set; }  // Lương thực nhận
        public string? Email { get; set; } // Email
        public string? UserName { get; set; } // Tên đăng nhập

        public int TotalDayInMonth { get; set; } // Tổng số ngày trong tháng
        public int TotalDayOffInMonth { get; set; } // Tổng số ngày nghỉ trong tháng

        public int TotalLeaveWithPermission { get; set; } // Tổng số ngày nghỉ CÓ PHÉP

        public int TotalLeaveWithoutPermission { get; set; } // Tổng số ngày nghỉ KHÔNG PHÉP

        public SalaryDto(decimal BasicSalary, decimal Bonus, decimal Allowance, decimal Penalty, decimal SalaryAdvance, decimal NetSalary)
        {
            this.BasicSalaryString = BasicSalary.ToString("#,##0.##");
            this.BonusString = Bonus.ToString("#,##0.##");
            this.AllowanceString = Allowance.ToString("#,##0.##");
            this.PenaltyString = Penalty.ToString("#,##0.##");
            this.SalaryAdvanceString = SalaryAdvance.ToString("#,##0.##");
            this.NetSalaryString = NetSalary.ToString("#,##0.##");

        }
        public SalaryDto()
        {
            this.BasicSalaryString = BasicSalary.ToString("#,##0.##");
        }
    }
}
