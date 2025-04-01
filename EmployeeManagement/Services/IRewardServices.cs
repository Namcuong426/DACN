using EmployeeManagement.Dto;

namespace EmployeeManagement.Services
{
    public interface IRewardServices
    {
        Task<GenericResponse<List<RewardDto>>> GetAllRewardAsync(int page, int pageSize, int? searchMonth = null, int? searchYear = null, string? search = null, string? sort = null);
        Task<GenericResponseSingle<RewardDto>> GetRewardByIdAsync(int rewardId);
        Task<GenericResponseSingle<RewardDto>> CreateRewardAsync(RewardDto rewardDto);
        Task<GenericResponseSingle<RewardDto>> UpdateRewardAsync(int rewardId, RewardDto rewardDto);
        Task<GenericResponseSingle<bool>> DeleteRewardAsync(int rewardId);
        Task<GenericResponseSingle<bool>> DeleteRewardAllAsync();
        Task<GenericResponseSingle<bool>> DeleteRewardByIdsAsync(List<int> rewardIds);
        public Task<List<int>> GetListYearsAsync();
    }
}
