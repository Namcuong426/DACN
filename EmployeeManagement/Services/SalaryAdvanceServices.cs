using AutoMapper;
using EmployeeManagement.Constant;
using EmployeeManagement.Context;
using EmployeeManagement.Dto;
using EmployeeManagement.Helper;
using EmployeeManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Services
{
    public class SalaryAdvanceServices : ISalaryAdvanceServices
    {
        private readonly IMapper _mapper;
        private readonly EmployeeManagementContext _context;

        public SalaryAdvanceServices(IMapper mapper, EmployeeManagementContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<GenericResponse<(List<SalaryAdvanceDto> SalaryAdvanceDtosEx, List<SalaryAdvanceDto> data)>> GetAllSalaryAdvanceAsync(int page, int pageSize, int? searchMonth = null, int? searchYear = null, string? search = null, string? sort = null)
        {
            var response = new GenericResponse<(List<SalaryAdvanceDto>, List<SalaryAdvanceDto>)>();

            try
            {
                // Fetch the list of SalaryAdvances from the database
                IQueryable<SalaryAdvance> query = _context.SalaryAdvances.Include(x => x.Employee).Include(r => r.Employee);

                // Perform search if a search term is provided
                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(r => r.Employee.EmployeeCode.Contains(search) || r.Employee.Name.Contains(search));
                }
                if (searchMonth != null && searchYear != null)
                {
                    query = query.Where(r => r.AdvanceDate.Month == searchMonth && r.AdvanceDate.Year == searchYear);
                }
                else if (searchMonth != null)
                {
                    query = query.Where(r => r.AdvanceDate.Month == searchMonth);
                }
                else if (searchYear != null)
                {
                    query = query.Where(r => r.AdvanceDate.Year == searchYear);
                }
                // Perform sorting if sort criteria are provided
                if (!string.IsNullOrEmpty(sort))
                {
                    switch (sort)
                    {
                        case "SalaryAdvance_name_asc":
                            query = query.OrderBy(r => r.Employee.Name);
                            break;
                        case "SalaryAdvance_name_desc":
                            query = query.OrderByDescending(r => r.Employee.Name);
                            break;
                        case "SalaryAdvance_month_asc":
                            query = query.OrderBy(r => r.AdvanceDate.Month);
                            break;
                        case "SalaryAdvance_month_desc":
                            query = query.OrderByDescending(r => r.AdvanceDate.Month);
                            break;
                        case "SalaryAdvance_year_asc":
                            query = query.OrderBy(r => r.AdvanceDate.Year);
                            break;
                        case "SalaryAdvance_year_desc":
                            query = query.OrderByDescending(r => r.AdvanceDate.Year);
                            break;
                        default:
                            // Default sorting by reward date descending
                            query = query.OrderByDescending(r => r.AdvanceDate.Month).ThenByDescending(x => x.AdvanceDate.Year);
                            break;
                    }
                }

                // Get total count of SalaryAdvances
                var totalCount = await query.CountAsync();

                // Paginate data

                var SalaryAdvanceDtosPdf = await query.ToListAsync();
                var SalaryAdvances = await query.Skip((page - 1) * pageSize)
                                         .Take(pageSize)
                                         .ToListAsync();

                // Calculate total pages
                var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
                // Map the list of SalaryAdvances to DTOs
                response.Data = (SalaryAdvanceDtosPdf.Select(x => new SalaryAdvanceDto
                {
                    AdvanceDate = x.AdvanceDate,
                    AdvanceAmount = x.AdvanceAmount,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt,
                    Description = x.Description,
                    Email = x.Employee.Email,
                    Id = x.Id,
                    EmployeeID = x.EmployeeID,
                    UserName = x.Employee.Username,
                    EmployeeCode = x.Employee.EmployeeCode,
                    EmployeeName = x.Employee.Name,
                    AdvanceAmountString = x.AdvanceAmount.FormatNumberDecimal()
                }).ToList(),
                    SalaryAdvances.Select(

                        x => new SalaryAdvanceDto
                        {
                            AdvanceDate = x.AdvanceDate,
                            AdvanceAmount = x.AdvanceAmount,
                            CreatedAt = x.CreatedAt,
                            UpdatedAt = x.UpdatedAt,
                            Description = x.Description,
                            Email = x.Employee.Email,
                            Id = x.Id,
                            EmployeeID = x.EmployeeID,
                            UserName = x.Employee.Username,
                            EmployeeCode = x.Employee.EmployeeCode,
                            EmployeeName = x.Employee.Name,
                            AdvanceAmountString = x.AdvanceAmount.FormatNumberDecimal()
                        }
                        ).ToList());
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
                response.Message = "Failed to retrieve SalaryAdvances.";
            }

            return response;
        }

        public async Task<GenericResponseSingle<SalaryAdvanceDto>> GetSalaryAdvanceByIdAsync(int SalaryAdvanceId)
        {
            var response = new GenericResponseSingle<SalaryAdvanceDto>();

            try
            {
                var SalaryAdvance = await _context.SalaryAdvances.FindAsync(SalaryAdvanceId);
                if (SalaryAdvance == null)
                {
                    response.Success = false;
                    response.Message = "SalaryAdvance not found.";
                    return response;
                }

                response.Data = _mapper.Map<SalaryAdvanceDto>(SalaryAdvance);
                response.Success = true;
                response.Message = "Success";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Failed to retrieve SalaryAdvance.";
            }

            return response;
        }

        public async Task<GenericResponseSingle<SalaryAdvanceDto>> CreateSalaryAdvanceAsync(SalaryAdvanceDto SalaryAdvanceDto)
        {
            var response = new GenericResponseSingle<SalaryAdvanceDto>();

            try
            {
                // Check if the SalaryAdvance already exists
                var existingSalaryAdvance = await _context.SalaryAdvances.FirstOrDefaultAsync(r => r.EmployeeID == SalaryAdvanceDto.EmployeeID && r.AdvanceDate.Month == SalaryAdvanceDto.AdvanceDate.Month && r.AdvanceDate.Year == SalaryAdvanceDto.AdvanceDate.Year);
                if (existingSalaryAdvance != null)
                {
                    response.Success = false;
                    response.Message = Message.SalaryAdvanceExisted;
                    return response;
                }
                // check amount advance > 0
                if (SalaryAdvanceDto.AdvanceAmount <= 0)
                {
                    response.Success = false;
                    response.Message = Message.SalaryAdvanceAmountInvalid;
                    return response;
                }
                var salary = await _context.Salaries.FirstOrDefaultAsync(r => r.EmployeeID == SalaryAdvanceDto.EmployeeID && r.Month == SalaryAdvanceDto.AdvanceDate.Month && r.Year == SalaryAdvanceDto.AdvanceDate.Year);
                if (salary != null)
                {
                    response.Success = false;
                    response.Message = Message.SalaryAdvanceFailedExistSalary;
                    return response;
                }

                var salaryAdvance = new SalaryAdvance
                {
                    EmployeeID = SalaryAdvanceDto.EmployeeID,
                    AdvanceDate = SalaryAdvanceDto.AdvanceDate,
                    AdvanceAmount = SalaryAdvanceDto.AdvanceAmount,
                    Description = SalaryAdvanceDto.Description,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,


                };

                await _context.SalaryAdvances.AddAsync(salaryAdvance);
                await _context.SaveChangesAsync();

                response.Data = _mapper.Map<SalaryAdvanceDto>(salaryAdvance);
                response.Success = true;
                response.Message = Message.SalaryAdvanceCreated;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = Message.SalaryAdvanceFailed;
            }

            return response;
        }

        public async Task<GenericResponseSingle<SalaryAdvanceDto>> UpdateSalaryAdvanceAsync(int SalaryAdvanceId, SalaryAdvanceDto SalaryAdvanceDto)
        {
            var response = new GenericResponseSingle<SalaryAdvanceDto>();

            try
            {
                var existingSalaryAdvance = await _context.SalaryAdvances.FindAsync(SalaryAdvanceId);
                if (existingSalaryAdvance == null)
                {
                    response.Success = false;
                    response.Message = "SalaryAdvance not found.";
                    return response;
                }
                if (SalaryAdvanceDto.AdvanceAmount <= 0)
                {
                    response.Success = false;
                    response.Message = Message.SalaryAdvanceAmountInvalid;
                    return response;
                }
                var salary = await _context.Salaries.FirstOrDefaultAsync(r => r.EmployeeID == SalaryAdvanceDto.EmployeeID && r.Month == SalaryAdvanceDto.AdvanceDate.Month && r.Year == SalaryAdvanceDto.AdvanceDate.Year);
                if (salary != null)
                {
                    response.Success = false;
                    response.Message = Message.SalaryAdvanceFailedExistSalary;
                    return response;
                }

                existingSalaryAdvance.AdvanceDate = SalaryAdvanceDto.AdvanceDate;
                existingSalaryAdvance.AdvanceAmount = SalaryAdvanceDto.AdvanceAmount;
                existingSalaryAdvance.Description = SalaryAdvanceDto.Description;
                existingSalaryAdvance.UpdatedAt = DateTime.Now;


                await _context.SaveChangesAsync();

                response.Data = _mapper.Map<SalaryAdvanceDto>(existingSalaryAdvance);
                response.Data.AdvanceAmountString = existingSalaryAdvance.AdvanceAmount.FormatNumberDecimal();
                response.Success = true;
                response.Message = Message.SalaryAdvanceUpdate;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = Message.SalaryAdvanceUpdateFailed;
            }

            return response;
        }

        public async Task<GenericResponseSingle<bool>> DeleteSalaryAdvanceAsync(int SalaryAdvanceId)
        {
            var response = new GenericResponseSingle<bool>();

            try
            {
                var SalaryAdvance = await _context.SalaryAdvances.FindAsync(SalaryAdvanceId);
                if (SalaryAdvance == null)
                {
                    response.Success = false;
                    response.Message = Message.SalaryAdvanceNotFound;
                    return response;
                }

                _context.SalaryAdvances.Remove(SalaryAdvance);
                await _context.SaveChangesAsync();

                response.Data = true;
                response.Success = true;
                response.Message = Message.SalaryAdvanceDeleted;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = Message.SalaryAdvanceDeletedFailed;
            }

            return response;
        }

        public async Task<GenericResponseSingle<bool>> DeleteSalaryAdvanceAllAsync()
        {
            var response = new GenericResponseSingle<bool>();

            try
            {
                var SalaryAdvances = await _context.SalaryAdvances.ToListAsync();
                if (!SalaryAdvances.Any())
                {
                    response.Success = false;
                    response.Message = Message.SalaryAdvanceNotFound;
                    return response;
                }

                _context.SalaryAdvances.RemoveRange(SalaryAdvances);
                await _context.SaveChangesAsync();

                response.Data = true;
                response.Success = true;
                response.Message = Message.SalaryAdvanceDeleted;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = Message.SalaryAdvanceDeletedFailed;
            }

            return response;
        }

        public async Task<GenericResponseSingle<bool>> DeleteSalaryAdvanceByIdsAsync(List<int> SalaryAdvanceIds)
        {
            var response = new GenericResponseSingle<bool>();

            try
            {
                var SalaryAdvances = await _context.SalaryAdvances.Where(r => SalaryAdvanceIds.Contains(r.Id)).ToListAsync();
                if (!SalaryAdvances.Any())
                {
                    response.Success = false;
                    response.Message = Message.SalaryAdvanceNotFound;
                    return response;
                }

                _context.SalaryAdvances.RemoveRange(SalaryAdvances);
                await _context.SaveChangesAsync();

                response.Data = true;
                response.Success = true;
                response.Message = Message.SalaryAdvanceDeleted;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = Message.SalaryAdvanceDeletedFailed;
            }

            return response;
        }

        public async Task<List<int>> GetListYearsAsync()
        {
            return await _context.SalaryAdvances.Select(r => r.AdvanceDate.Year).Distinct().ToListAsync();
        }
    }

}
