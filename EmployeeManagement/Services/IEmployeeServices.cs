using EmployeeManagement.Dto;

namespace EmployeeManagement.Services
{
    public interface IEmployeeServices
    {
        Task<GenericResponse<List<EmployeeDto>>> GetAllEmployeeAsync(int page, int pageSize
            , string? search = null, string? sort = null, int? educationLevelId = null,
            int? departmentId = null,
            int? positionId = null);
        Task<GenericResponseSingle<EmployeeDto>> GetEmployeeByIdAsync(int EmployeeId);
        Task<GenericResponseSingle<EmployeeDto>> CreateEmployeeAsync(EmployeeDto EmployeeDto);
        Task<GenericResponseSingle<EmployeeDto>> UpdateEmployeeAsync(int EmployeeId, EmployeeDto EmployeeDto);
        Task<GenericResponseSingle<bool>> DeleteEmployeeAsync(int EmployeeId);
        // DeleteEmployeeAllAsync 
        Task<GenericResponseSingle<bool>> DeleteEmployeeAllAsync();
        // DeleteEmployeeByIdsAsync
        Task<GenericResponseSingle<bool>> DeleteEmployeeByIdsAsync(List<int> EmployeeIds);

        Task<List<EmployeeViewDto>> GetAllEmployeeAsync();
        Task<GenericResponseSingle<EmployeeDto>> GetEmployeeByEmailAsync(string email);

        // Get List Years contain Employee
        Task<List<int>> GetListYearsAsync();
    }
}