using EmployeeManagement.Dto;

namespace EmployeeManagement.Services
{
    public interface IAttendanceServices
    {
        Task<GenericResponse<List<AttendanceDto>>> GetAllAttendanceAsync(int page, int pageSize, int? searchMonth = null, int? searchYear = null, string? search = null, string? sort = null);
        Task<GenericResponseSingle<AttendanceDto>> GetAttendanceByIdAsync(int AttendanceId);
        Task<GenericResponseSingle<AttendanceDto>> CreateAttendanceAsync(AttendanceDto AttendanceDto);
        Task<GenericResponseSingle<AttendanceDto>> UpdateAttendanceAsync(int AttendanceId, AttendanceDto AttendanceDto);
        Task<GenericResponseSingle<bool>> DeleteAttendanceAsync(int AttendanceId);
        Task<GenericResponseSingle<bool>> DeleteAttendanceAllAsync();
        Task<GenericResponseSingle<bool>> DeleteAttendanceByIdsAsync(List<int> AttendanceIds);
        Task<List<int>> GetListYearsAsync();
    }
}