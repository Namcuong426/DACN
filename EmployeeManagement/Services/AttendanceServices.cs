using AutoMapper;
using EmployeeManagement.Constant;
using EmployeeManagement.Context;
using EmployeeManagement.Dto;
using EmployeeManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Services
{
    public class AttendanceServices : IAttendanceServices
    {
        private readonly IMapper _mapper;
        private readonly EmployeeManagementContext _context;

        public AttendanceServices(IMapper mapper, EmployeeManagementContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<GenericResponse<List<AttendanceDto>>> GetAllAttendanceAsync(int page, int pageSize, int? searchMonth = null, int? searchYear = null, string? search = null, string? sort = null)
        {
            var response = new GenericResponse<List<AttendanceDto>>();

            try
            {
                // Fetch the list of Attendances from the database
                IQueryable<Attendance> query = _context.Attendances.Include(r => r.Employee);

                // Perform search if a search term is provided
                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(r => r.Employee.Name.Contains(search) || r.Employee.Email.Contains(search) || r.Employee.EmployeeCode.Contains(search));
                }
                if (searchMonth != null && searchYear != null)
                {
                    query = query.Where(r => r.Date.Month == searchMonth && r.Date.Year == searchYear);
                }
                else if (searchMonth != null)
                {
                    query = query.Where(r => r.Date.Month == searchMonth);
                }
                else if (searchYear != null)
                {
                    query = query.Where(r => r.Date.Year == searchYear);
                }
                // Perform sorting if sort criteria are provided
                if (!string.IsNullOrEmpty(sort))
                {
                    switch (sort)
                    {
                        case "name_asc":
                            query = query.OrderBy(r => r.Employee.Name);
                            break;
                        case "name_desc":
                            query = query.OrderByDescending(r => r.Employee.Name);
                            break;
                        case "attendance_date_asc":
                            query = query.OrderBy(r => r.Date);
                            break;
                        case "attendance_date_desc":
                            query = query.OrderByDescending(r => r.Date);
                            break;
                        case "attendance_admin":
                            query = query.Where(r => r.Date.Month == DateTime.Now.Month && r.Date.Year == DateTime.Now.Year);
                            break;
                        default:
                            // Default sorting by Attendance date descending
                            query = query.OrderByDescending(r => r.Date);
                            break;
                    }
                }

                // Get total count of Attendances
                var totalCount = await query.CountAsync();

                // Paginate data
                var Attendances = await query.Skip((page - 1) * pageSize)
                                         .Take(pageSize)
                                         .ToListAsync();

                // Calculate total pages
                var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

                // Map the list of Attendances to DTOs
                response.Data = Attendances.Select(x => new AttendanceDto
                {
                    Date = x.Date,
                    Notes = x.Notes,
                    EmployeeID = x.EmployeeID,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt,
                    Id = x.Id,
                    IsAccepted = x.IsAccepted,
                    EmployeeName = x.Employee.Name,
                    EmployeeCode = x.Employee.EmployeeCode,
                    Email = x.Employee.Email,
                    Username = x.Employee.Username

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
                response.Message = "Failed to retrieve Attendances.";
            }

            return response;
        }

        public async Task<GenericResponseSingle<AttendanceDto>> GetAttendanceByIdAsync(int AttendanceId)
        {
            var response = new GenericResponseSingle<AttendanceDto>();

            try
            {
                var Attendance = await _context.Attendances.FindAsync(AttendanceId);
                if (Attendance == null)
                {
                    response.Success = false;
                    response.Message = "Attendance not found.";
                    return response;
                }

                response.Data = _mapper.Map<AttendanceDto>(Attendance);
                response.Success = true;
                response.Message = "Success";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Failed to retrieve Attendance.";
            }

            return response;
        }

        public async Task<GenericResponseSingle<AttendanceDto>> CreateAttendanceAsync(AttendanceDto AttendanceDto)
        {
            var response = new GenericResponseSingle<AttendanceDto>();

            try
            {
                if (AttendanceDto.Date.Date > DateTime.Now.Date)
                {
                    response.Success = false;
                    response.Message = Message.AttendanceDateInvalid;
                    return response;
                }
                // check EmployeeID Attendanced in Date
                var isAttendance = await _context.Attendances.AnyAsync(x => x.IsAccepted == true
                && x.EmployeeID == AttendanceDto.EmployeeID && x.Date == AttendanceDto.Date);
                if (isAttendance)
                {
                    response.Success = false;
                    response.Message = Message.AttendanceExisted;
                    return response;
                }
                var Attendance = new Attendance
                {
                    Date = AttendanceDto.Date,
                    Notes = AttendanceDto.Notes,
                    EmployeeID = AttendanceDto.EmployeeID,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    IsAccepted = true,
                };


                await _context.Attendances.AddAsync(Attendance);
                await _context.SaveChangesAsync();

                response.Data = _mapper.Map<AttendanceDto>(Attendance);
                response.Success = true;
                response.Message = Message.AttendanceCreated;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = Message.AttendanceFailed;
            }

            return response;
        }

        public async Task<GenericResponseSingle<AttendanceDto>> UpdateAttendanceAsync(int AttendanceId, AttendanceDto AttendanceDto)
        {
            var response = new GenericResponseSingle<AttendanceDto>();

            try
            {
                var existingAttendance = await _context.Attendances.FindAsync(AttendanceId);
                if (existingAttendance == null)
                {
                    response.Success = false;
                    response.Message = "Attendance not found.";
                    return response;
                }
                if (existingAttendance.Date == AttendanceDto.Date)
                {
                    response.Success = false;
                    response.Message = Message.AttendanceExisted;
                    return response;
                }
                existingAttendance.Date = AttendanceDto.Date;
                existingAttendance.Notes = AttendanceDto.Notes;
                existingAttendance.EmployeeID = AttendanceDto.EmployeeID;
                existingAttendance.UpdatedAt = DateTime.Now;
                existingAttendance.IsAccepted = AttendanceDto.IsAccepted;

                await _context.SaveChangesAsync();

                response.Data = _mapper.Map<AttendanceDto>(existingAttendance);
                response.Success = true;
                response.Message = Message.AttendanceUpdated;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = Message.AttendanceUpdateFailed;
            }

            return response;
        }

        public async Task<GenericResponseSingle<bool>> DeleteAttendanceAsync(int AttendanceId)
        {
            var response = new GenericResponseSingle<bool>();

            try
            {
                var Attendance = await _context.Attendances.FindAsync(AttendanceId);
                if (Attendance == null)
                {
                    response.Success = false;
                    response.Message = Message.AttendanceNotFound;
                    return response;
                }

                _context.Attendances.Remove(Attendance);
                await _context.SaveChangesAsync();

                response.Data = true;
                response.Success = true;
                response.Message = Message.AttendanceDeleted;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = Message.AttendanceDeletedFailed;
            }

            return response;
        }

        public async Task<GenericResponseSingle<bool>> DeleteAttendanceAllAsync()
        {
            var response = new GenericResponseSingle<bool>();

            try
            {
                var Attendances = await _context.Attendances.ToListAsync();
                if (!Attendances.Any())
                {
                    response.Success = false;
                    response.Message = Message.AttendanceNotFound;
                    return response;
                }

                _context.Attendances.RemoveRange(Attendances);
                await _context.SaveChangesAsync();

                response.Data = true;
                response.Success = true;
                response.Message = Message.AttendanceDeleted;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = Message.AttendanceDeletedFailed;
            }

            return response;
        }

        public async Task<GenericResponseSingle<bool>> DeleteAttendanceByIdsAsync(List<int> AttendanceIds)
        {
            var response = new GenericResponseSingle<bool>();

            try
            {
                var Attendances = await _context.Attendances.Where(r => AttendanceIds.Contains(r.Id)).ToListAsync();
                if (!Attendances.Any())
                {
                    response.Success = false;
                    response.Message = Message.AttendanceNotFound;
                    return response;
                }

                _context.Attendances.RemoveRange(Attendances);
                await _context.SaveChangesAsync();

                response.Data = true;
                response.Success = true;
                response.Message = Message.AttendanceDeleted;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = Message.AttendanceDeletedFailed;
            }

            return response;
        }
        public async Task<List<int>> GetListYearsAsync()
        {
            var years = await _context.Attendances.Select(x => x.Date.Year).Distinct().ToListAsync();
            return years;
        }
    }


}
