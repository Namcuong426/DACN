using AutoMapper;
using EmployeeManagement.Context;
using EmployeeManagement.Dto;
using EmployeeManagement.Enum;
using EmployeeManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Services
{
    public class LeaveService(EmployeeManagementContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor) : ILeaveService
    {
        private readonly EmployeeManagementContext _context = context;
        private readonly IMapper _mapper = mapper;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private const int MAX_LEAVE_IN_YEAR = 12;

        public async Task<(IList<LeaveDto>, int countLeaveRemaining)> GetAllLeaveService()
        {
            var leaves = await _context.Leaves.Include(x => x.Employee).ToListAsync();
            
            var leaveDtos = _mapper.Map<List<LeaveDto>>(leaves).OrderBy(x => x.UserName).ToList();

            var empName = _httpContextAccessor.HttpContext?.User?.FindFirst("EmployeeName").Value;
            var countLeaveSuccess = leaveDtos.Count(x => x.LeavePermission == (int)LeavePermissionEnum.Yes && x.Status == (int)LeaveStatusEnum.Success
                        && x.EmployeeName == empName);

            int cLr = MAX_LEAVE_IN_YEAR - countLeaveSuccess;

            if (leaveDtos.Count != 0)
            {
                for(int i = 0; i < leaveDtos.Count; i++)
                {
                    leaveDtos[i].UserName = leaves[i].Employee.Name;
                }
            }
            return (leaveDtos, cLr);
        }

        public async Task<GenericResponseSingle<LeaveDto>> CreateLeaveAsync(LeaveDto leaveDto)
        {
            var response = new GenericResponseSingle<LeaveDto>();
            try
            {
                // Không được tạo trùng ngày nghỉ
                var leaveExists = await _context.Leaves.CountAsync(x => x.DateLeave.Value.Date == DateTime.Now.Date && x.EmployeeId == leaveDto.EmployeeId);
                if (leaveExists > 0)
                {
                    response.Data = leaveDto;
                    response.Success = false;
                    response.Message = "This leave already exists.";
                    return response;
                }

                var leaveEntity = _mapper.Map<Leave>(leaveDto);
                leaveEntity.CreatedAt = DateTime.Now;
                leaveEntity.UpdatedAt = DateTime.Now;
                leaveEntity.DateLeave = leaveDto.DateLeave;
                
                leaveEntity.Status = (int)LeaveStatusEnum.Pending;
                
                // Tối đa 12 ngày nghỉ phép/ năm
                var countLeaveEmp = await _context.Leaves.Where(x => x.EmployeeId == leaveDto.EmployeeId && x.LeavePermission == (int)LeavePermissionEnum.Yes).CountAsync();
                if (countLeaveEmp > MAX_LEAVE_IN_YEAR)
                {
                    response.Data = _mapper.Map<LeaveDto>(leaveEntity);
                    response.Success = false;
                    response.Message = "Annual leave is over.";
                    return response;
                }else
                {
                    var employee = _context.Employees.FirstOrDefault(e => e.Id == leaveDto.EmployeeId);
                    if (employee is not null)
                        leaveEntity.Employee = employee;
                    await _context.Leaves.AddAsync(leaveEntity);
                    await _context.SaveChangesAsync();

                    response.Data = _mapper.Map<LeaveDto>(leaveEntity);
                    response.Success = true;
                    response.Message = "Leave created successfully.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Failed to create leave. " + ex.Message;
            }

            return response;
        }

        public async Task<GenericResponseSingle<LeaveDto>> UpdateLeaveAsync(int leaveId, LeaveDto leaveDto)
        {
            var response = new GenericResponseSingle<LeaveDto>();

            try
            {
                var existingLeave = await _context.Leaves.FindAsync(leaveId);
                if (existingLeave == null)
                {
                    response.Success = false;
                    response.Message = "Leave not found.";
                    return response;
                }

                _mapper.Map(leaveDto, existingLeave);
                existingLeave.UpdatedAt = DateTime.Now;
                existingLeave.DateLeave = leaveDto.DateLeave;
                var employee = _context.Employees.FirstOrDefault(e => e.Id == leaveDto.EmployeeId);
                if (employee is not null)
                    existingLeave.Employee = employee;
                await _context.SaveChangesAsync();

                response.Data = _mapper.Map<LeaveDto>(existingLeave);
                response.Success = true;
                response.Message = "Leave updated successfully.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Failed to update leave.";
            }

            return response;
        }

        public async Task<GenericResponseSingle<bool>> DeleteLeaveAsync(int leaveId)
        {
            var response = new GenericResponseSingle<bool>();

            try
            {
                var leave = await _context.Leaves.FindAsync(leaveId);
                if (leave == null)
                {
                    response.Success = false;
                    response.Message = "Leave not found.";
                    return response;
                }

                _context.Leaves.Remove(leave);
                await _context.SaveChangesAsync();

                response.Data = true;
                response.Success = true;
                response.Message = "Leave deleted successfully.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Failed to delete leave. " + ex.Message;
            }

            return response;
        }

        public async Task<GenericResponseSingle<LeaveDto>> ChangeStatusLeave(LeaveDto model, int leaveId)
        {
            var response = new GenericResponseSingle<LeaveDto>();

            try
            {
                var existingLeave = await _context.Leaves.FindAsync(leaveId);
                if (existingLeave == null)
                {
                    response.Success = false;
                    response.Message = "Leave not found.";
                    return response;
                }

                _mapper.Map(model, existingLeave);
                existingLeave.UpdatedAt = DateTime.Now;
                existingLeave.DateLeave = DateTime.Now;
                existingLeave.ReasonLeave = model.ReasonLeave;
                existingLeave.LeavePermission = model.LeavePermission;


                /*
                 * Handle Ngày nghỉ phép liên quan tới chấm công
                 * Chọn nghỉ : Có phép hoặc Không phép
                    Nghỉ có phép => Chỉ trừ 1 ngày nghỉ, Không trừ ngày công
                    Nghỉ không phép => Trừ 1 ngày công
                */
                var month = existingLeave.DateLeave.GetValueOrDefault().Month;
                var day = existingLeave.DateLeave.GetValueOrDefault().Day;
                if (existingLeave.Status == (int)LeaveStatusEnum.Success) {
                    // Nghỉ có phép => Chỉ trừ 1 ngày nghỉ, Không trừ ngày công => Bên tính lương đã count ra theo IsAccepted
                    // Nghỉ không phép => Trừ 1 ngày công
                    if (existingLeave.LeavePermission == (int)LeavePermissionEnum.No)
                    {
                        var attendenceEntity = await _context.Attendances.Where(x => x.EmployeeID == existingLeave.EmployeeId && x.Date.Month == month && x.Date.Day == day && x.IsAccepted == true).FirstOrDefaultAsync();
                        if(attendenceEntity is not null)
                        {
                            attendenceEntity.IsAccepted = false;
                        }
                    }
                }
                var employee = _context.Employees.FirstOrDefault(e => e.Id == model.EmployeeId);
                if (employee is not null)
                    existingLeave.Employee = employee;
                await _context.SaveChangesAsync();

                response.Data = _mapper.Map<LeaveDto>(existingLeave);
                response.Success = true;
                response.Message = "Status Leave updated successfully.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Failed to update status leave. " + ex.Message;
            }

            return response;
        }

        public async Task<GenericResponseSingle<StatisticLeaveDto>> StatisticsLeaveAsync(StatisticLeaveDto leaveDto)
        {
            var response = new GenericResponseSingle<StatisticLeaveDto>();

            try
            {
               var countLeaveWithPermission = await _context.Leaves.Where(x => x.EmployeeId == leaveDto.EmployeeId && x.LeavePermission == (int)LeavePermissionEnum.Yes && x.Status == (int)LeaveStatusEnum.Success)
                    .CountAsync();

                var countLeaveWithoutPermission = await _context.Leaves.Where(x => x.EmployeeId == leaveDto.EmployeeId && x.LeavePermission == (int)LeavePermissionEnum.No && x.Status == (int)LeaveStatusEnum.Success)
                    .CountAsync();

                var countResponse = new StatisticLeaveDto
                {
                    CountLeaveWithPermission = countLeaveWithPermission,
                    CountLeaveWithoutPermission = countLeaveWithoutPermission
                };

                response.Data = _mapper.Map<StatisticLeaveDto>(countResponse);
                response.Success = true;
                response.Message = "Status Leave updated successfully.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Failed to update status leave. " + ex.Message;
            }

            return response;
        }
    }
}
