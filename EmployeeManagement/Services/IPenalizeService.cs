using EmployeeManagement.Dto;

namespace EmployeeManagement.Services
{
    public interface IPenalizeServices
    {
        Task<GenericResponse<List<PenalizeDto>>> GetAllPenalizeAsync(int page, int pageSize, int? searchMonth = null, int? searchYear = null, string? search = null, string? sort = null);
        Task<GenericResponseSingle<PenalizeDto>> GetPenalizeByIdAsync(int penalizeId);
        Task<GenericResponseSingle<PenalizeDto>> CreatePenalizeAsync(PenalizeDto penalizeDto);
        Task<GenericResponseSingle<PenalizeDto>> UpdatePenalizeAsync(int penalizeId, PenalizeDto penalizeDto);
        Task<GenericResponseSingle<bool>> DeletePenalizeAsync(int penalizeId);
        Task<GenericResponseSingle<bool>> DeleteAllPenalizesAsync();
        Task<GenericResponseSingle<bool>>DeletePenalizesByIdsAsync(List<int> penalizeIds);
        public Task<List<int>> GetListPenalizeYearsAsync();
    }
}
