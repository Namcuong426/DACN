using EmployeeManagement.Dto;

namespace EmployeeManagement.Services
{
    public interface ILeaveService
    {
        Task<(IList<LeaveDto>, int countLeaveRemaining)> GetAllLeaveService();

        Task<GenericResponseSingle<LeaveDto>> CreateLeaveAsync(LeaveDto dto);

        Task<GenericResponseSingle<LeaveDto>> UpdateLeaveAsync(int leaveId, LeaveDto leaveDto);

        Task<GenericResponseSingle<bool>> DeleteLeaveAsync(int id);

        Task<GenericResponseSingle<LeaveDto>> ChangeStatusLeave(LeaveDto model, int leaveId);

        Task<GenericResponseSingle<StatisticLeaveDto>> StatisticsLeaveAsync(StatisticLeaveDto leaveDto);
    }
}
