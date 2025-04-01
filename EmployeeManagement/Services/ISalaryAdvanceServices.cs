using EmployeeManagement.Dto;

namespace EmployeeManagement.Services
{
    public interface ISalaryAdvanceServices
    {
        Task<GenericResponse<(List<SalaryAdvanceDto> SalaryAdvanceDtosEx, List<SalaryAdvanceDto> data)>> GetAllSalaryAdvanceAsync(int page, int pageSize, int? searchMonth = null, int? searchYear = null, string? search = null, string? sort = null);
        Task<GenericResponseSingle<SalaryAdvanceDto>> GetSalaryAdvanceByIdAsync(int SalaryAdvanceId);
        Task<GenericResponseSingle<SalaryAdvanceDto>> CreateSalaryAdvanceAsync(SalaryAdvanceDto SalaryAdvanceDto);
        Task<GenericResponseSingle<SalaryAdvanceDto>> UpdateSalaryAdvanceAsync(int SalaryAdvanceId, SalaryAdvanceDto SalaryAdvanceDto);
        Task<GenericResponseSingle<bool>> DeleteSalaryAdvanceAsync(int SalaryAdvanceId);
        Task<GenericResponseSingle<bool>> DeleteSalaryAdvanceAllAsync();
        Task<GenericResponseSingle<bool>> DeleteSalaryAdvanceByIdsAsync(List<int> SalaryAdvanceIds);
        Task<List<int>> GetListYearsAsync();
    }
}
