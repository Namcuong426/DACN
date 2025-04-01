using AutoMapper;
using EmployeeManagement.Constant;
using EmployeeManagement.Context;
using EmployeeManagement.Dto;
using EmployeeManagement.Helper;
using EmployeeManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Services
{
    public class PenalizeServices : IPenalizeServices
    {
        private readonly IMapper _mapper;
        private readonly EmployeeManagementContext _context;

        public PenalizeServices(IMapper mapper, EmployeeManagementContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<GenericResponse<List<PenalizeDto>>> GetAllPenalizeAsync(int page, int pageSize, int? searchMonth = null, int? searchYear = null, string? search = null, string? sort = null)
        {
            var response = new GenericResponse<List<PenalizeDto>>();

            try
            {
                // Fetch the list of penalizes from the database
                IQueryable<Penalize> query = _context.Penalizes.Include(p => p.Employee);

                // Perform search if a search term is provided
                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(p => p.PenalizeName.Contains(search) || p.Description.Contains(search));
                }
                if (searchMonth != null && searchYear != null)
                {
                    query = query.Where(p => p.PenalizeDate.Month == searchMonth && p.PenalizeDate.Year == searchYear);
                }
                else if (searchMonth != null)
                {
                    query = query.Where(p => p.PenalizeDate.Month == searchMonth);
                }
                else if (searchYear != null)
                {
                    query = query.Where(p => p.PenalizeDate.Year == searchYear);
                }
                // Perform sorting if sort criteria are provided
                if (!string.IsNullOrEmpty(sort))
                {
                    switch (sort)
                    {
                        case "penalize_name_asc":
                            query = query.OrderBy(p => p.PenalizeName);
                            break;
                        case "penalize_name_desc":
                            query = query.OrderByDescending(p => p.PenalizeName);
                            break;
                        case "penalize_date_asc":
                            query = query.OrderBy(p => p.PenalizeDate);
                            break;
                        case "penalize_date_desc":
                            query = query.OrderByDescending(p => p.PenalizeDate);
                            break;
                        case "name_desc":
                            query = query.OrderByDescending(p => p.Employee.Name);
                            break;
                        case "name_asc":
                            query = query.OrderBy(p => p.Employee.Name);
                            break;
                        default:
                            // Default sorting by penalize date descending
                            query = query.OrderByDescending(p => p.PenalizeDate);
                            break;
                    }
                }

                // Get total count of penalizes
                var totalCount = await query.CountAsync();

                // Paginate data
                var penalizes = await query.Skip((page - 1) * pageSize)
                                           .Take(pageSize)
                                           .ToListAsync();

                // Calculate total pages
                var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

                // Map the list of penalizes to DTOs
                response.Data = penalizes.Select(x => new PenalizeDto
                {
                    Id = x.Id,
                    EmployeeID = x.EmployeeID,
                    PenalizeName = x.PenalizeName,
                    PenalizeDate = x.PenalizeDate,
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
                response.Message = "Failed to retrieve penalizes.";
            }

            return response;
        }

        public async Task<GenericResponseSingle<PenalizeDto>> GetPenalizeByIdAsync(int penalizeId)
        {
            var response = new GenericResponseSingle<PenalizeDto>();

            try
            {
                var penalize = await _context.Penalizes.FindAsync(penalizeId);
                if (penalize == null)
                {
                    response.Success = false;
                    response.Message = "Penalize not found.";
                    return response;
                }

                response.Data = _mapper.Map<PenalizeDto>(penalize);
                response.Success = true;
                response.Message = "Success";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Failed to retrieve penalize.";
            }

            return response;
        }

        public async Task<GenericResponseSingle<PenalizeDto>> CreatePenalizeAsync(PenalizeDto penalizeDto)
        {
            var response = new GenericResponseSingle<PenalizeDto>();

            try
            {
                var penalize = new Penalize
                {
                    PenalizeName = penalizeDto.PenalizeName,
                    PenalizeDate = penalizeDto.PenalizeDate,
                    Description = penalizeDto.Description,
                    Amount = penalizeDto.Amount,
                    EmployeeID = penalizeDto.EmployeeID,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                };


                await _context.Penalizes.AddAsync(penalize);
                await _context.SaveChangesAsync();

                response.Data = _mapper.Map<PenalizeDto>(penalize);
                response.Success = true;
                response.Message = Message.PenalizeCreated;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = Message.PenalizeFailed;
            }

            return response;
        }

        public async Task<GenericResponseSingle<PenalizeDto>> UpdatePenalizeAsync(int penalizeId, PenalizeDto penalizeDto)
        {
            var response = new GenericResponseSingle<PenalizeDto>();

            try
            {
                var existingPenalize = await _context.Penalizes.FindAsync(penalizeId);
                if (existingPenalize == null)
                {
                    response.Success = false;
                    response.Message = "Penalize not found.";
                    return response;
                }

                existingPenalize.PenalizeName = penalizeDto.PenalizeName;
                existingPenalize.PenalizeDate = penalizeDto.PenalizeDate;
                existingPenalize.Description = penalizeDto.Description;
                existingPenalize.Amount = penalizeDto.Amount;
                existingPenalize.EmployeeID = penalizeDto.EmployeeID;
                existingPenalize.UpdatedAt = DateTime.Now;

                await _context.SaveChangesAsync();

                response.Data = _mapper.Map<PenalizeDto>(existingPenalize);
                response.Success = true;
                response.Message = Message.PenalizeUpdate;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = Message.PenalizeUpdateFailed;
            }

            return response;
        }

        public async Task<GenericResponseSingle<bool>> DeletePenalizeAsync(int penalizeId)
        {
            var response = new GenericResponseSingle<bool>();

            try
            {
                var penalize = await _context.Penalizes.FindAsync(penalizeId);
                if (penalize == null)
                {
                    response.Success = false;
                    response.Message = Message.PenalizeNotFound;
                    return response;
                }

                _context.Penalizes.Remove(penalize);
                await _context.SaveChangesAsync();

                response.Data = true;
                response.Success = true;
                response.Message = Message.PenalizeDeleted;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = Message.PenalizeDeletedFailed;
            }

            return response;
        }

        public async Task<GenericResponseSingle<bool>> DeleteAllPenalizesAsync()
        {
            var response = new GenericResponseSingle<bool>();

            try
            {
                var penalizes = await _context.Penalizes.ToListAsync();
                if (!penalizes.Any())
                {
                    response.Success = false;
                    response.Message = Message.PenalizeNotFound;
                    return response;
                }

                _context.Penalizes.RemoveRange(penalizes);
                await _context.SaveChangesAsync();

                response.Data = true;
                response.Success = true;
                response.Message = Message.PenalizeDeleted;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = Message.PenalizeDeletedFailed;
            }

            return response;
        }

        public async Task<GenericResponseSingle<bool>> DeletePenalizesByIdsAsync(List<int> penalizeIds)
        {
            var response = new GenericResponseSingle<bool>();

            try
            {
                var penalizes = await _context.Penalizes.Where(p => penalizeIds.Contains(p.Id)).ToListAsync();
                if (!penalizes.Any())
                {
                    response.Success = false;
                    response.Message = Message.PenalizeNotFound;
                    return response;
                }

                _context.Penalizes.RemoveRange(penalizes);
                await _context.SaveChangesAsync();

                response.Data = true;
                response.Success = true;
                response.Message = Message.PenalizeDeleted;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = Message.PenalizeDeletedFailed;
            }

            return response;
        }

        public async Task<List<int>> GetListPenalizeYearsAsync()
        {
            return await _context.Penalizes.Select(p => p.PenalizeDate.Year).Distinct().ToListAsync();
        }
    }

}

