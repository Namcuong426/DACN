using AutoMapper;
using EmployeeManagement.Constant;
using EmployeeManagement.Context;
using EmployeeManagement.Dto;
using EmployeeManagement.Enum;
using EmployeeManagement.Helper;
using EmployeeManagement.Models;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace EmployeeManagement.Services
{
    public class SalaryServices : ISalaryServices
    {
        private readonly IMapper _mapper;
        private readonly EmployeeManagementContext _context;

        public SalaryServices(IMapper mapper, EmployeeManagementContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<GenericResponse<(List<SalaryDto> salaryDtosEx, List<SalaryDto> data)>> GetAllSalaryAsync(int page, int pageSize, int? searchMonth = null, int? searchYear = null, string? search = null, string? sort = null)
        {
            var response = new GenericResponse<(List<SalaryDto>, List<SalaryDto>)>();

            try
            {
                // Fetch the list of Salarys from the database
                IQueryable<Salary> query = _context.Salaries.Include(x => x.Employee).Include(r => r.Employee);

                // Perform search if a search term is provided
                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(r => r.Employee.EmployeeCode.Contains(search) || r.Employee.Name.Contains(search));
                }
                if (searchMonth != null && searchYear != null)
                {
                    query = query.Where(r => r.Month == searchMonth && r.Year == searchYear);
                }
                else if (searchMonth != null)
                {
                    query = query.Where(r => r.Month == searchMonth);
                }
                else if (searchYear != null)
                {
                    query = query.Where(r => r.Year == searchYear);
                }
                // Perform sorting if sort criteria are provided
                if (!string.IsNullOrEmpty(sort))
                {
                    switch (sort)
                    {
                        case "salary_name_asc":
                            query = query.OrderBy(r => r.Employee.Name);
                            break;
                        case "salary_name_desc":
                            query = query.OrderByDescending(r => r.Employee.Name);
                            break;
                        case "salary_month_asc":
                            query = query.OrderBy(r => r.Month);
                            break;
                        case "salary_month_desc":
                            query = query.OrderByDescending(r => r.Month);
                            break;
                        case "salary_year_asc":
                            query = query.OrderBy(r => r.Year);
                            break;
                        case "salary_year_desc":
                            query = query.OrderByDescending(r => r.Year);
                            break;
                        default:
                            // Default sorting by reward date descending
                            query = query.OrderByDescending(r => r.Month).ThenByDescending(x => x.Year);
                            break;
                    }
                }

                // Get total count of Salarys
                var totalCount = await query.CountAsync();

                // Paginate data

                var salaryDtosEx = await query.ToListAsync();
                var Salarys = await query.Skip((page - 1) * pageSize)
                                         .Take(pageSize)
                                         .ToListAsync();

                // Calculate total pages
                var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

                // Map the list of Salarys to DTOs
                response.Data = (salaryDtosEx.Select(x => new SalaryDto
                {
                    Allowance = x.Allowance,
                    BasicSalary = x.BasicSalary,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt,
                    Bonus = x.Bonus,
                    EmployeeCode = x.Employee.EmployeeCode,
                    EmployeeID = x.EmployeeID,
                    EmployeeName = x.Employee.Name,
                    Id = x.Id,
                    Month = x.Month,
                    NetSalary = x.NetSalary,
                    Penalty = x.Penalty,
                    SalaryAdvance = x.SalaryAdvance,
                    Year = x.Year,
                    TotalAttendance = x.TotalAttendance,
                    Email = x.Employee.Email,
                    UserName = x.Employee.Username,
                    BasicSalaryString = x.BasicSalary.FormatNumberDecimal(),
                    AllowanceString = x.Allowance.FormatNumberDecimal(),
                    BonusString = x.Bonus.FormatNumberDecimal(),
                    PenaltyString = x.Penalty.FormatNumberDecimal(),
                    SalaryAdvanceString = x.SalaryAdvance.FormatNumberDecimal(),
                    NetSalaryString = x.NetSalary.FormatNumberDecimal(),
                    TotalDayInMonth = DateTime.DaysInMonth(x.Year, x.Month),
                    TotalDayOffInMonth = DateTime.DaysInMonth(x.Year, x.Month) - x.TotalAttendance ?? 0
                }).ToList(),
                    Salarys.Select(

                        x => new SalaryDto
                        {
                            Allowance = x.Allowance,
                            BasicSalary = x.BasicSalary,
                            CreatedAt = x.CreatedAt,
                            UpdatedAt = x.UpdatedAt,
                            Bonus = x.Bonus,
                            EmployeeCode = x.Employee.EmployeeCode,
                            EmployeeID = x.EmployeeID,
                            EmployeeName = x.Employee.Name,
                            Id = x.Id,
                            Month = x.Month,
                            NetSalary = x.NetSalary,
                            Penalty = x.Penalty,
                            SalaryAdvance = x.SalaryAdvance,
                            Year = x.Year,
                            TotalAttendance = x.TotalAttendance,
                            Email = x.Employee.Email,
                            UserName = x.Employee.Username,
                            BasicSalaryString = x.BasicSalary.FormatNumberDecimal(),
                            AllowanceString = x.Allowance.FormatNumberDecimal(),
                            BonusString = x.Bonus.FormatNumberDecimal(),
                            PenaltyString = x.Penalty.FormatNumberDecimal(),
                            SalaryAdvanceString = x.SalaryAdvance.FormatNumberDecimal(),
                            NetSalaryString = x.NetSalary.FormatNumberDecimal(),
                            TotalDayInMonth = DateTime.DaysInMonth(x.Year, x.Month),
                            TotalDayOffInMonth = DateTime.DaysInMonth(x.Year, x.Month) - x.TotalAttendance ?? 0,
                            TotalLeaveWithPermission = HandleTotalLeaveWithPermission(x.EmployeeID, x.Month, x.Year, true),
                            TotalLeaveWithoutPermission = HandleTotalLeaveWithPermission(x.EmployeeID, x.Month, x.Year, false)
                        }
                        ).ToList());
                response.Page = page;
                response.PageSize = pageSize;
                response.TotalCount = totalCount;
                response.TotalPages = totalPages;
                response.Success = true;
                response.Message = "Success";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Failed to retrieve Salarys.";
            }

            return response;
        }

        public async Task<GenericResponseSingle<SalaryDto>> GetSalaryByIdAsync(int SalaryId)
        {
            var response = new GenericResponseSingle<SalaryDto>();

            try
            {
                var Salary = await _context.Salaries.FindAsync(SalaryId);
                if (Salary == null)
                {
                    response.Success = false;
                    response.Message = "Salary not found.";
                    return response;
                }

                response.Data = _mapper.Map<SalaryDto>(Salary);
                response.Data.BasicSalaryString = Salary.BasicSalary.ToString("#,##0.##");
                response.Success = true;
                response.Message = "Success";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Failed to retrieve Salary.";
            }

            return response;
        }

        public async Task<GenericResponseSingle<SalaryDto>> CreateSalaryAsync(SalaryDto SalaryDto)
        {
            var response = new GenericResponseSingle<SalaryDto>();

            try
            {
                // Check if the Salary already exists
                var existingSalary = await _context.Salaries.FirstOrDefaultAsync(r => r.EmployeeID == SalaryDto.EmployeeID && r.Month == SalaryDto.Month && r.Year == SalaryDto.Year);
                if (existingSalary != null)
                {
                    response.Success = false;
                    response.Message = Message.SalaryExisted;
                    return response;
                }

                var Salary = new Salary
                {
                    Allowance = SalaryDto.Allowance,
                    BasicSalary = SalaryDto.BasicSalary,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    Bonus = SalaryDto.Bonus,
                    EmployeeID = SalaryDto.EmployeeID,
                    Month = SalaryDto.Month,
                    NetSalary = SalaryDto.NetSalary,
                    Penalty = SalaryDto.Penalty,
                    SalaryAdvance = SalaryDto.SalaryAdvance,
                    Year = SalaryDto.Year,
                    TotalAttendance = SalaryDto.TotalAttendance
                };

                // Lương cơ bản / 26 ngày * số ngày công + tiền phụ cấp + tiền thưởng - tiền phạt - ứng lương [Đã xử lý ở phía client]
                if (SalaryDto.Allowance != 0)
                {
                    SalaryDto.NetSalary = SalaryDto.NetSalary + SalaryDto.Allowance;
                    SalaryDto.NetSalary = SalaryDto.NetSalary - ((SalaryDto.NetSalary + Salary.Insurance) / 100);
                }
                //Salary.NetSalary = SalaryDto.BasicSalary / 26 * (26 - SalaryDto.TotalLeaveWithoutPermission) 
                //    + SalaryDto.Allowance + SalaryDto.Bonus - SalaryDto.Penalty - SalaryDto.SalaryAdvance - ((SalaryDto.NetSalary + Salary.Insurance) / 100);

                await _context.Salaries.AddAsync(Salary);
                await _context.SaveChangesAsync();

                response.Data = _mapper.Map<SalaryDto>(Salary);
                response.Success = true;
                response.Message = Message.SalaryCreated;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = Message.SalaryFailed;
            }

            return response;
        }

        public async Task<GenericResponseSingle<SalaryDto>> UpdateSalaryAsync(int SalaryId, SalaryDto SalaryDto)
        {
            var response = new GenericResponseSingle<SalaryDto>();

            try
            {
                var existingSalary = await _context.Salaries.FindAsync(SalaryId);
                if (existingSalary == null)
                {
                    response.Success = false;
                    response.Message = "Salary not found.";
                    return response;
                }



                await _context.SaveChangesAsync();

                response.Data = _mapper.Map<SalaryDto>(existingSalary);
                response.Success = true;
                response.Message = Message.SalaryUpdate;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = Message.SalaryUpdateFailed;
            }

            return response;
        }

        public async Task<GenericResponseSingle<bool>> DeleteSalaryAsync(int SalaryId)
        {
            var response = new GenericResponseSingle<bool>();

            try
            {
                var Salary = await _context.Salaries.FindAsync(SalaryId);
                if (Salary == null)
                {
                    response.Success = false;
                    response.Message = Message.SalaryNotFound;
                    return response;
                }

                _context.Salaries.Remove(Salary);
                await _context.SaveChangesAsync();

                response.Data = true;
                response.Success = true;
                response.Message = Message.SalaryDeleted;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = Message.SalaryDeletedFailed;
            }

            return response;
        }

        public async Task<GenericResponseSingle<bool>> DeleteSalaryAllAsync()
        {
            var response = new GenericResponseSingle<bool>();

            try
            {
                var Salarys = await _context.Salaries.ToListAsync();
                if (!Salarys.Any())
                {
                    response.Success = false;
                    response.Message = Message.SalaryNotFound;
                    return response;
                }

                _context.Salaries.RemoveRange(Salarys);
                await _context.SaveChangesAsync();

                response.Data = true;
                response.Success = true;
                response.Message = Message.SalaryDeleted;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = Message.SalaryDeletedFailed;
            }

            return response;
        }

        public async Task<GenericResponseSingle<bool>> DeleteSalaryByIdsAsync(List<int> SalaryIds)
        {
            var response = new GenericResponseSingle<bool>();

            try
            {
                var Salarys = await _context.Salaries.Where(r => SalaryIds.Contains(r.Id)).ToListAsync();
                if (!Salarys.Any())
                {
                    response.Success = false;
                    response.Message = Message.SalaryNotFound;
                    return response;
                }

                _context.Salaries.RemoveRange(Salarys);
                await _context.SaveChangesAsync();

                response.Data = true;
                response.Success = true;
                response.Message = Message.SalaryDeleted;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = Message.SalaryDeletedFailed;
            }

            return response;
        }

        public async Task<List<int>> GetListYearsAsync()
        {
            return await _context.Salaries.Select(r => r.Year).Distinct().ToListAsync();
        }

        public async Task<SalaryDto> GetSalaryByEmployeeIdAsync(int employeeId, int month, int year)
        {
            DateTime startOfMonth = new DateTime(year, month, 1);
            DateTime endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);
            var employee = await _context.Employees.FindAsync(employeeId);
            var allowances = _context.Allowances
               .Where(a => a.EmployeeID == employeeId && a.StartDate <= endOfMonth && a.EndDate >= startOfMonth)
               .ToList();

            var bonus = await _context.Rewards.Where(x => x.EmployeeID == employeeId
            && x.RewardDate.Month == month && x.RewardDate.Year == year).ToListAsync();
            var penalty = await _context.Penalizes.Where(x => x.EmployeeID == employeeId && x.PenalizeDate.Month == month && x.PenalizeDate.Year == year).ToListAsync();
            var salaryAdvance = await _context.SalaryAdvances.Where(x => x.EmployeeID == employeeId && x.AdvanceDate.Month == month && x.AdvanceDate.Year == year).ToListAsync();
            var totalAttendance = await _context.Attendances.Where(x => x.EmployeeID == employeeId && x.Date.Month == month && x.Date.Year == year && x.IsAccepted == true).CountAsync();
            decimal salaryAdvanceSum = 0;
            decimal allowanceSum = 0;
            decimal bonusSum = 0;
            decimal penaltySum = 0;

            if (penalty.Any())
            {
                penaltySum = penalty.Sum(x => x.Amount);
            }
            if (salaryAdvance.Any())
            {
                salaryAdvanceSum = salaryAdvance.Sum(x => x.AdvanceAmount);
            }
            if (bonus.Any())
            {
                bonusSum = bonus.Sum(x => x.Amount);
            }
            if (allowances.Any())
            {
                allowanceSum = allowances.Sum(x => x.Amount);
            }

            var salary = new SalaryDto
            {
                EmployeeName = employee.Name,
                EmployeeCode = employee.EmployeeCode,
                EmployeeID = employee.Id,
                Allowance = allowanceSum,
                Bonus = bonusSum,
                Penalty = penaltySum,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Month = DateTime.Now.Month,
                Year = DateTime.Now.Year,
                SalaryAdvance = salaryAdvanceSum,
                TotalAttendance = totalAttendance,
                BasicSalaryString = employee.BasicSalary.FormatNumberDecimal(),
                BasicSalary = employee.BasicSalary,
                AllowanceString = allowanceSum.FormatNumberDecimal(),
                BonusString = bonusSum.FormatNumberDecimal(),
                PenaltyString = penaltySum.FormatNumberDecimal(),
                SalaryAdvanceString = salaryAdvanceSum.FormatNumberDecimal(),
                TotalDayInMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month),
                TotalDayOffInMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month) - totalAttendance,
                TotalLeaveWithPermission = HandleTotalLeaveWithPermission(employeeId, month, year, true),
                TotalLeaveWithoutPermission = HandleTotalLeaveWithPermission(employeeId, month, year, false)

            };
            // Lương cơ bản / 26 ngày * số ngày công + tiền phụ cấp + tiền thưởng - tiền phạt - ứng lương - bao hiem - phu cap
            var daysInMonth = DateTime.DaysInMonth(year, month);
            var daysNotWeekend = daysInMonth - 4;
            var netSalary = (employee.BasicSalary / daysNotWeekend) * totalAttendance + allowanceSum + bonusSum - penaltySum - salaryAdvanceSum;
            // decimal 2 số sau dấu phẩy
            salary.NetSalary = Math.Round(netSalary, 2);
            salary.NetSalaryString = salary.NetSalary.FormatNumber().Replace(",", ".");
            return salary;
        }

        private int HandleTotalLeaveWithPermission(int empId, int month, int year, bool flag)
        {
            if(flag)
            {
                return  _context.Leaves.Where(x => x.EmployeeId == empId 
                            && x.DateLeave.Value.Month == month 
                            && x.DateLeave.Value.Year == year 
                            && x.LeavePermission == (int)LeavePermissionEnum.Yes 
                            && x.Status == (int)LeaveStatusEnum.Success)
                .Count();

            }else
            {
                return _context.Leaves.Where(x => x.EmployeeId == empId
                            && x.DateLeave.Value.Month == month
                            && x.DateLeave.Value.Year == year
                            && x.LeavePermission == (int)LeavePermissionEnum.No
                            && x.Status == (int)LeaveStatusEnum.Success)
                .Count();
            }
        }
        public MemoryStream ExportEmployeeSalariesToExcel(List<SalaryDto> employeeSalaries)
        {
            var stream = new MemoryStream();
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add($"Lương Nhân Viên");

                // Add column headers
                worksheet.Cells[1, 1].Value = "Mã nhân viên";
                worksheet.Cells[1, 2].Value = "Tháng";
                worksheet.Cells[1, 3].Value = "Năm";
                worksheet.Cells[1, 4].Value = "Lương cơ bản";
                worksheet.Cells[1, 5].Value = "Tiền thưởng";
                worksheet.Cells[1, 6].Value = "Tiền phụ cấp";
                worksheet.Cells[1, 7].Value = "Tiền phạt";
                worksheet.Cells[1, 8].Value = "Ứng lương";
                worksheet.Cells[1, 9].Value = "Lương thực nhận";
                worksheet.Cells[1, 10].Value = "Tên nhân viên";
                worksheet.Cells[1, 11].Value = "Mã nhân viên";
                worksheet.Cells[1, 12].Value = "Số ngày công";
                worksheet.Cells[1, 13].Value = "Lương cơ bản";
                worksheet.Cells[1, 14].Value = "Tiền thưởng";
                worksheet.Cells[1, 15].Value = "Tiền phụ cấp";
                worksheet.Cells[1, 16].Value = "Tiền phạt";
                worksheet.Cells[1, 17].Value = "Ứng lương";
                worksheet.Cells[1, 18].Value = "Lương thực nhận";
                worksheet.Cells[1, 19].Value = "Email";
                worksheet.Cells[1, 20].Value = "Tên đăng nhập";
                worksheet.Cells[1, 21].Value = "Số ngày trong tháng";
                worksheet.Cells[1, 22].Value = "Số ngày nghỉ trong tháng";

                // Add data rows
                for (int i = 0; i < employeeSalaries.Count; i++)
                {
                    var emp = employeeSalaries[i];
                    worksheet.Cells[i + 2, 1].Value = emp.EmployeeID;
                    worksheet.Cells[i + 2, 2].Value = emp.Month;
                    worksheet.Cells[i + 2, 3].Value = emp.Year;
                    worksheet.Cells[i + 2, 4].Value = emp.BasicSalary;
                    worksheet.Cells[i + 2, 5].Value = emp.Bonus;
                    worksheet.Cells[i + 2, 6].Value = emp.Allowance;
                    worksheet.Cells[i + 2, 7].Value = emp.Penalty;
                    worksheet.Cells[i + 2, 8].Value = emp.SalaryAdvance;
                    worksheet.Cells[i + 2, 9].Value = emp.NetSalary;
                    worksheet.Cells[i + 2, 10].Value = emp.EmployeeName;
                    worksheet.Cells[i + 2, 11].Value = emp.EmployeeCode;
                    worksheet.Cells[i + 2, 12].Value = emp.TotalAttendance;
                    worksheet.Cells[i + 2, 13].Value = emp.BasicSalaryString;
                    worksheet.Cells[i + 2, 14].Value = emp.BonusString;
                    worksheet.Cells[i + 2, 15].Value = emp.AllowanceString;
                    worksheet.Cells[i + 2, 16].Value = emp.PenaltyString;
                    worksheet.Cells[i + 2, 17].Value = emp.SalaryAdvanceString;
                    worksheet.Cells[i + 2, 18].Value = emp.NetSalaryString;
                    worksheet.Cells[i + 2, 19].Value = emp.Email;
                    worksheet.Cells[i + 2, 20].Value = emp.UserName;
                    worksheet.Cells[i + 2, 21].Value = emp.TotalDayInMonth;
                    worksheet.Cells[i + 2, 22].Value = emp.TotalDayOffInMonth;
                }

                package.SaveAs(stream);
            }
            stream.Position = 0;
            return stream;
        }
    }

}
