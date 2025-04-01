using EmployeeManagement.Dto;

namespace EmployeeManagement.Services
{
    public interface ISalaryServices
    {
        Task<GenericResponse<(List<SalaryDto> salaryDtosEx, List<SalaryDto> data)>> GetAllSalaryAsync(int page, int pageSize, int? searchMonth = null, int? searchYear = null, string? search = null, string? sort = null);
        Task<GenericResponseSingle<SalaryDto>> GetSalaryByIdAsync(int SalaryId);
        Task<GenericResponseSingle<SalaryDto>> CreateSalaryAsync(SalaryDto SalaryDto);
        Task<GenericResponseSingle<SalaryDto>> UpdateSalaryAsync(int SalaryId, SalaryDto SalaryDto);
        Task<GenericResponseSingle<bool>> DeleteSalaryAsync(int SalaryId);
        Task<GenericResponseSingle<bool>> DeleteSalaryAllAsync();
        Task<GenericResponseSingle<bool>> DeleteSalaryByIdsAsync(List<int> SalaryIds);
        public Task<List<int>> GetListYearsAsync();
        Task<SalaryDto> GetSalaryByEmployeeIdAsync(int employeeId, int month, int year);
        MemoryStream ExportEmployeeSalariesToExcel(List<SalaryDto> employeeSalaries);

    }
}
