using AutoMapper;
using EmployeeManagement.Constant;
using EmployeeManagement.Context;
using EmployeeManagement.Dto;
using EmployeeManagement.Helper;
using EmployeeManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Services
{
    public class RewardServices : IRewardServices
    {
        private readonly IMapper _mapper;
        private readonly EmployeeManagementContext _context;

        public RewardServices(IMapper mapper, EmployeeManagementContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<GenericResponse<List<RewardDto>>> GetAllRewardAsync(int page, int pageSize, int? searchMonth = null, int? searchYear = null, string? search = null, string? sort = null)
        {
            var response = new GenericResponse<List<RewardDto>>();

            try
            {
                // Fetch the list of rewards from the database
                IQueryable<Reward> query = _context.Rewards.Include(r => r.Employee);

                // Perform search if a search term is provided
                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(r => r.RewardName.Contains(search) || r.Description.Contains(search));
                }
                if (searchMonth != null && searchYear != null)
                {
                    query = query.Where(r => r.RewardDate.Month == searchMonth && r.RewardDate.Year == searchYear);
                }
                else if (searchMonth != null)
                {
                    query = query.Where(r => r.RewardDate.Month == searchMonth);
                }
                else if (searchYear != null)
                {
                    query = query.Where(r => r.RewardDate.Year == searchYear);
                }
                // Perform sorting if sort criteria are provided
                if (!string.IsNullOrEmpty(sort))
                {
                    switch (sort)
                    {
                        case "reward_name_asc":
                            query = query.OrderBy(r => r.RewardName);
                            break;
                        case "reward_name_desc":
                            query = query.OrderByDescending(r => r.RewardName);
                            break;
                        case "reward_date_asc":
                            query = query.OrderBy(r => r.RewardDate);
                            break;
                        case "reward_date_desc":
                            query = query.OrderByDescending(r => r.RewardDate);
                            break;
                        case "name_desc":
                            query = query.OrderByDescending(r => r.Employee.Name);
                            break;
                        case "name_asc":
                            query = query.OrderBy(r => r.Employee.Name);
                            break;
                        default:
                            // Default sorting by reward date descending
                            query = query.OrderByDescending(r => r.RewardDate);
                            break;
                    }
                }

                // Get total count of rewards
                var totalCount = await query.CountAsync();

                // Paginate data
                var rewards = await query.Skip((page - 1) * pageSize)
                                         .Take(pageSize)
                                         .ToListAsync();

                // Calculate total pages
                var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

                // Map the list of rewards to DTOs
                response.Data = rewards.Select(x => new RewardDto
                {
                    Id = x.Id,
                    EmployeeID = x.EmployeeID,
                    RewardName = x.RewardName,
                    RewardDate = x.RewardDate,
                    Description = x.Description,
                    Amount = x.Amount,
                    EmployeeName = x.Employee?.Name,
                    CreatedAt = x.CreatedAt,
                    Email = x.Employee.Email,
                    UpdatedAt = x.UpdatedAt,
                    Username = x.Employee.Username,
                    AmountString = x.Amount.FormatNumberDecimal()

                }).ToList();

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
                response.Message = "Failed to retrieve rewards.";
            }

            return response;
        }

        public async Task<GenericResponseSingle<RewardDto>> GetRewardByIdAsync(int rewardId)
        {
            var response = new GenericResponseSingle<RewardDto>();

            try
            {
                var reward = await _context.Rewards.FindAsync(rewardId);
                if (reward == null)
                {
                    response.Success = false;
                    response.Message = "Reward not found.";
                    return response;
                }

                response.Data = _mapper.Map<RewardDto>(reward);
                response.Success = true;
                response.Message = "Success";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Failed to retrieve reward.";
            }

            return response;
        }

        public async Task<GenericResponseSingle<RewardDto>> CreateRewardAsync(RewardDto rewardDto)
        {
            var response = new GenericResponseSingle<RewardDto>();

            try
            {
                var reward = new Reward
                {
                    RewardName = rewardDto.RewardName,
                    RewardDate = rewardDto.RewardDate,
                    Description = rewardDto.Description,
                    Amount = rewardDto.Amount,
                    EmployeeID = rewardDto.EmployeeID,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                };


                await _context.Rewards.AddAsync(reward);
                await _context.SaveChangesAsync();

                response.Data = _mapper.Map<RewardDto>(reward);
                response.Success = true;
                response.Message = Message.RewardCreated;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = Message.RewardFailed;
            }

            return response;
        }

        public async Task<GenericResponseSingle<RewardDto>> UpdateRewardAsync(int rewardId, RewardDto rewardDto)
        {
            var response = new GenericResponseSingle<RewardDto>();

            try
            {
                var existingReward = await _context.Rewards.FindAsync(rewardId);
                if (existingReward == null)
                {
                    response.Success = false;
                    response.Message = "Reward not found.";
                    return response;
                }

                existingReward.RewardName = rewardDto.RewardName;
                existingReward.RewardDate = rewardDto.RewardDate;
                existingReward.Description = rewardDto.Description;
                existingReward.Amount = rewardDto.Amount;
                existingReward.EmployeeID = rewardDto.EmployeeID;
                existingReward.UpdatedAt = DateTime.Now;

                await _context.SaveChangesAsync();

                response.Data = _mapper.Map<RewardDto>(existingReward);
                response.Success = true;
                response.Message = Message.RewardUpdate;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = Message.RewardUpdateFailed;
            }

            return response;
        }

        public async Task<GenericResponseSingle<bool>> DeleteRewardAsync(int rewardId)
        {
            var response = new GenericResponseSingle<bool>();

            try
            {
                var reward = await _context.Rewards.FindAsync(rewardId);
                if (reward == null)
                {
                    response.Success = false;
                    response.Message = Message.RewardNotFound;
                    return response;
                }

                _context.Rewards.Remove(reward);
                await _context.SaveChangesAsync();

                response.Data = true;
                response.Success = true;
                response.Message = Message.RewardDeleted;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = Message.RewardDeletedFailed;
            }

            return response;
        }

        public async Task<GenericResponseSingle<bool>> DeleteRewardAllAsync()
        {
            var response = new GenericResponseSingle<bool>();

            try
            {
                var rewards = await _context.Rewards.ToListAsync();
                if (!rewards.Any())
                {
                    response.Success = false;
                    response.Message = Message.RewardNotFound;
                    return response;
                }

                _context.Rewards.RemoveRange(rewards);
                await _context.SaveChangesAsync();

                response.Data = true;
                response.Success = true;
                response.Message = Message.RewardDeleted;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = Message.RewardDeletedFailed;
            }

            return response;
        }

        public async Task<GenericResponseSingle<bool>> DeleteRewardByIdsAsync(List<int> rewardIds)
        {
            var response = new GenericResponseSingle<bool>();

            try
            {
                var rewards = await _context.Rewards.Where(r => rewardIds.Contains(r.Id)).ToListAsync();
                if (!rewards.Any())
                {
                    response.Success = false;
                    response.Message = Message.RewardNotFound;
                    return response;
                }

                _context.Rewards.RemoveRange(rewards);
                await _context.SaveChangesAsync();

                response.Data = true;
                response.Success = true;
                response.Message = Message.RewardDeleted;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = Message.RewardDeletedFailed;
            }

            return response;
        }

        public async Task<List<int>> GetListYearsAsync()
        {
            return await _context.Rewards.Select(r => r.RewardDate.Year).Distinct().ToListAsync();
        }
    }

}
