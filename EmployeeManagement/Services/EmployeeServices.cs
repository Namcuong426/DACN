using AutoMapper;
using EmployeeManagement.Constant;
using EmployeeManagement.Context;
using EmployeeManagement.Dto;
using EmployeeManagement.Enum;
using EmployeeManagement.Helper;
using EmployeeManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Services
{
    public class EmployeeServices : IEmployeeServices
    {
        private readonly IMapper _mapper;
        private readonly EmployeeManagementContext _context;

        public EmployeeServices(IMapper mapper, EmployeeManagementContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<GenericResponse<List<EmployeeDto>>> GetAllEmployeeAsync(int page, int pageSize, string? search = null, string? sort = null, int? educationLevelId = null, int? departmentId = null, int? positionId = null)
        {
            var response = new GenericResponse<List<EmployeeDto>>();

            try
            {
                // Lấy danh sách môn học từ cơ sở dữ liệu
                IQueryable<Employee> query = _context.Employees.Include(x => x.Department).Include(u => u.Position).Include(k => k.EducationLevel);

                // Thực hiện tìm kiếm nếu có từ khóa search
                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(x => x.Name.Contains(search) || x.Email.Contains(search) || x.EmployeeCode.Contains(search));
                }

                // Thực hiện sắp xếp nếu có trường và hướng sắp xếp được chỉ định
                if (!string.IsNullOrEmpty(sort))
                {
                    switch (sort)
                    {
                        case "name_asc":
                            query = query.OrderBy(x => x.Name);
                            break;
                        case "name_desc":
                            query = query.OrderByDescending(x => x.Name);
                            break;
                        case "created_at_asc":
                            query = query.OrderBy(x => x.CreatedAt);
                            break;
                        case "created_at_desc":
                            query = query.OrderByDescending(x => x.CreatedAt);
                            break;
                        case "updated_at_asc":
                            query = query.OrderBy(x => x.UpdatedAt);
                            break;
                        case "updated_at_desc":
                            query = query.OrderByDescending(x => x.UpdatedAt);
                            break;
                        default:
                            // Mặc định sắp xếp theo ngày cập nhật giảm dần
                            query = query.OrderByDescending(x => x.UpdatedAt);
                            break;
                    }
                }
                if (educationLevelId != null)
                {
                    query = query.Where(x => x.EducationLevelID == educationLevelId);
                }
                if (departmentId != null)
                {
                    query = query.Where(x => x.DepartmentID == departmentId);
                }
                if (positionId != null)
                {
                    query = query.Where(x => x.PositionID == positionId);
                }

                // Lấy tổng số môn học
                var totalCount = await query.CountAsync();

                // Phân trang dữ liệu
                var employee = await query.Skip((page - 1) * pageSize)
                                          .Take(pageSize)
                                          .ToListAsync();

                // Tính toán tổng số trang
                var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

                // Ánh xạ danh sách môn học sang DTO
                response.Data = employee.Select(x => new EmployeeDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    DateOfBirth = x.DateOfBirth,
                    Address = x.Address,
                    PhoneNumber = x.PhoneNumber,
                    BasicSalary = x.BasicSalary,
                    CitizenID = x.CitizenID,
                    DepartmentID = x.DepartmentID,
                    EducationLevelID = x.EducationLevelID,
                    PositionID = x.PositionID,
                    Email = x.Email,
                    ForeignLanguageProficiency = x.ForeignLanguageProficiency,
                    DepartmentName = x.Department.DepartmentName,
                    EducationLevelName = x.EducationLevel.EducationLevelName,
                    PositionName = x.Position.PositionName,
                    Username = x.Username,
                    Password = x.Password,
                    IsAdmin = x.IsAdmin,
                    IsActivated = x.IsActivated,
                    Gender = x.Gender,
                    HireDate = x.HireDate,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt,
                    EmployeeCode = x.EmployeeCode,
                    Avatar = FileHelper.GetFile(x.Avatar),
                    BasicSalaryString = x.BasicSalary.FormatNumberDecimal(),
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
                response.Message = Message.EmployeeFailed;
            }

            return response;
        }


        public async Task<GenericResponseSingle<EmployeeDto>> GetEmployeeByIdAsync(int EmployeeId)
        {
            var response = new GenericResponseSingle<EmployeeDto>();

            try
            {
                var Employee = await _context.Employees.FindAsync(EmployeeId);
                if (Employee == null)
                {
                    response.Success = false;
                    response.Message = Message.EmployeeNotFound;
                    return response;
                }

                response.Data = _mapper.Map<EmployeeDto>(Employee);
                response.Success = true;
                response.Message = "Success";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = Message.EmployeeFailed;
            }

            return response;
        }

        public async Task<GenericResponseSingle<EmployeeDto>> CreateEmployeeAsync(EmployeeDto employeeDto)
        {
            var response = new GenericResponseSingle<EmployeeDto>();

            try
            {

                // Kiểm tra xem nhân viên đã tồn tại chưa
                var isExisted = await _context.Employees.AnyAsync(x =>
                x.CitizenID == employeeDto.CitizenID || x.Email == employeeDto.Email || x.Username == employeeDto.Username);
                if (isExisted)
                {
                    response.Success = false;
                    response.Message = Message.EmployeeExisted;
                    return response;
                }
                if (string.IsNullOrEmpty(employeeDto.Username) || string.IsNullOrEmpty(employeeDto.Password))
                {
                    response.Success = false;
                    response.Message = "Username hoặc Password không được để trống";
                    return response;
                }
                if (!string.IsNullOrEmpty(employeeDto.Username) && !string.IsNullOrEmpty(employeeDto.Password))
                {
                    // Tao tai khoan
                    employeeDto.Password = HashingHelper.ComputeMd5Hash(employeeDto.Password);
                    var account = new Account
                    {
                        Username = employeeDto.Username,
                        Password = employeeDto.Password,
                        Role = (Role)employeeDto.Role,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };

                    _context.Accounts.Add(account);

                }
                employeeDto.IsAdmin = false;
                if (employeeDto.Role == 1 || employeeDto.Role == 2)
                {
                    employeeDto.IsAdmin = true;
                }
                // save avatar
                if (employeeDto.AvatarFile != null && employeeDto.AvatarFile.Length > 0)
                {
                    employeeDto.Avatar = FileHelper.SaveFile(employeeDto.AvatarFile);
                }
                var createdEmployee = _mapper.Map<Employee>(employeeDto);
                response.Data = _mapper.Map<EmployeeDto>(createdEmployee);
                response.Success = true;
                response.Message = Message.EmployeeCreated;
                createdEmployee.UpdatedAt = DateTime.Now;
                createdEmployee.CreatedAt = DateTime.Now;
                createdEmployee.IsActivated = true;

                await _context.Employees.AddAsync(createdEmployee);
                // get DepartmentName
                var department = await _context.Departments.FindAsync(createdEmployee.DepartmentID);
                await _context.SaveChangesAsync();
                if (department != null)
                {
                    createdEmployee.EmployeeCode = "NV" + department.Abbreviation + createdEmployee.Id;
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = Message.EmployeeFailed;
            }

            return response;
        }

        public async Task<GenericResponseSingle<EmployeeDto>> UpdateEmployeeAsync(int EmployeeId, EmployeeDto employeeDto)
        {
            var response = new GenericResponseSingle<EmployeeDto>();

            try
            {
                var existingEmployee = await _context.Employees.Include(x => x.Department).Include(u => u.Position).Include(k => k.EducationLevel).FirstOrDefaultAsync(x => x.Id == EmployeeId);
                if (existingEmployee == null)
                {
                    response.Success = false;
                    response.Message = Message.EmployeeNotFound;
                    return response;
                }
                employeeDto.IsAdmin = false;
                if (employeeDto.Role == 1 || employeeDto.Role == 2)
                {
                    employeeDto.IsAdmin = true;
                }
                existingEmployee.Address = employeeDto.Address;
                existingEmployee.BasicSalary = employeeDto.BasicSalary;
                existingEmployee.CitizenID = employeeDto.CitizenID;
                existingEmployee.DateOfBirth = employeeDto.DateOfBirth;
                existingEmployee.DepartmentID = employeeDto.DepartmentID;
                existingEmployee.EducationLevelID = employeeDto.EducationLevelID;
                existingEmployee.Email = employeeDto.Email;
                existingEmployee.ForeignLanguageProficiency = employeeDto.ForeignLanguageProficiency;
                existingEmployee.EducationLevelID = employeeDto.EducationLevelID;
                existingEmployee.DateOfBirth = employeeDto.DateOfBirth;
                existingEmployee.HireDate = employeeDto.HireDate;
                existingEmployee.Name = employeeDto.Name;
                existingEmployee.PhoneNumber = employeeDto.PhoneNumber;
                existingEmployee.PositionID = employeeDto.PositionID;
                existingEmployee.DepartmentID = employeeDto.DepartmentID;
                existingEmployee.UpdatedAt = DateTime.Now;
                existingEmployee.IsActivated = employeeDto.IsActivated;
                existingEmployee.IsAdmin = employeeDto.IsAdmin;
                existingEmployee.EmployeeCode = "NV" + existingEmployee.Department.Abbreviation + existingEmployee.Id;
                // update account
                var account = await _context.Accounts.FirstOrDefaultAsync(x => x.Username == existingEmployee.Username);
                if (account != null)
                {
                    account.Role = (Role)employeeDto.Role;
                    account.UpdatedAt = DateTime.Now;
                }
                // update avatar
                existingEmployee.Avatar = FileHelper.UpdateFile(employeeDto.AvatarFile, existingEmployee.Avatar);
                await _context.SaveChangesAsync();

                response.Data = _mapper.Map<EmployeeDto>(existingEmployee);
                response.Success = true;
                response.Message = Message.EmployeeUpdate;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = Message.EmployeeUpdateFailed;
            }

            return response;
        }

        public async Task<GenericResponseSingle<bool>> DeleteEmployeeAsync(int EmployeeId)
        {
            var response = new GenericResponseSingle<bool>();

            try
            {
                var isDeleted = await _context.Employees.FindAsync(EmployeeId);

                if (isDeleted == null)
                {
                    response.Success = false;
                    response.Message = Message.EmployeeNotFound;
                    return response;
                }

                _context.Employees.Remove(isDeleted);
                // remove account
                var account = await _context.Accounts.FirstOrDefaultAsync(x => x.Username == isDeleted.Username || isDeleted.Email == x.Username);
                if (account != null)
                {
                    _context.Accounts.Remove(account);
                }
                // remove avatar
                if (!string.IsNullOrEmpty(isDeleted.Avatar))
                {
                    FileHelper.DeleteFile(isDeleted.Avatar);
                }
                await _context.SaveChangesAsync();
                response.Data = true;
                response.Success = true;
                response.Message = Message.EmployeeDeleted;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = Message.EmployeeDeletedFailed;
            }

            return response;
        }

        public async Task<GenericResponseSingle<bool>> DeleteEmployeeAllAsync()
        {
            var response = new GenericResponseSingle<bool>();
            try
            {
                var employee = await _context.Employees.ToListAsync();
                if (employee.Count == 0)
                {
                    response.Success = false;
                    response.Message = Message.EmployeeNotFound;
                    return response;
                }
                // remove accounts
                var employeesEmailOrUserName = employee.Select(x => new
                {
                    x.Email,
                    x.Username
                }).ToList();



                _context.Employees.RemoveRange(employee);
                var accounts = await _context.Accounts.Where(x => employee.Any(o => o.Username == x.Username || o.Email == x.Username)).ToListAsync();
                // remove
                _context.Accounts.RemoveRange(accounts);
                // remove avatar
                foreach (var item in employee)
                {
                    if (!string.IsNullOrEmpty(item.Avatar))
                    {
                        FileHelper.DeleteFile(item.Avatar);
                    }
                }

                await _context.SaveChangesAsync();
                response.Data = true;
                response.Success = true;
                response.Message = Message.EmployeeDeleted;

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = Message.EmployeeDeletedFailed;
            }
            return response;

        }

        public async Task<GenericResponseSingle<bool>> DeleteEmployeeByIdsAsync(List<int> EmployeeIds)
        {
            var response = new GenericResponseSingle<bool>();
            try
            {
                var employee = await _context.Employees.Where(x => EmployeeIds.Contains(x.Id)).ToListAsync();
                if (employee.Count == 0)
                {
                    response.Success = false;
                    response.Message = Message.EmployeeNotFound;
                    return response;
                }
                _context.Employees.RemoveRange(employee);
                // remove account
                var accounts = await _context.Accounts.Where(x => employee.Any(o => o.Username == x.Username || o.Email == x.Username)).ToListAsync();
                // remove
                if (accounts.Any())
                {
                    _context.Accounts.RemoveRange(accounts);
                }
                // remove avatar
                foreach (var item in employee)
                {
                    if (!string.IsNullOrEmpty(item.Avatar))
                    {
                        FileHelper.DeleteFile(item.Avatar);
                    }
                }
                await _context.SaveChangesAsync();
                response.Data = true;
                response.Success = true;
                response.Message = Message.EmployeeDeleted;
                return response;

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = Message.EmployeeDeletedFailed;
                return response;
            }
        }

        public async Task<List<EmployeeViewDto>> GetAllEmployeeAsync()
        {
            var employees = await _context.Employees.Where(u => u.IsActivated == true).Select(x => new EmployeeViewDto
            {
                Id = x.Id,
                Name = x.Name,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                Email = x.Email,
                Username = x.Username,
                Avatar = FileHelper.GetFile(x.Avatar)

            }).ToListAsync();
            return employees;

        }

        public async Task<GenericResponseSingle<EmployeeDto>> GetEmployeeByEmailAsync(string email)
        {
            var response = new GenericResponseSingle<EmployeeDto>();
            try
            {
                var employee = await _context.Employees.FirstOrDefaultAsync(x => x.Email == email || x.Username == email);
                if (employee == null)
                {
                    // check account
                    var account = await _context.Accounts.FirstOrDefaultAsync(x => x.Username == email);
                    if (account == null)
                    {
                        response.Success = false;
                        response.Message = Message.EmployeeNotFound;
                        return response;
                    }
                    response.Success = true;
                    response.Message = "Success";
                    response.Data = new EmployeeDto
                    {
                        Username = account.Username,
                        Name = account.Username,
                        Role = (int)account.Role
                    };
                    return response;

                }
                response.Data = _mapper.Map<EmployeeDto>(employee);
                response.Success = true;
                response.Message = "Success";
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = Message.EmployeeFailed;
                return response;
            }
        }

        public async Task<List<int>> GetListYearsAsync()
        {
            var years = await _context.Employees.Select(x => x.DateOfBirth.Year).Distinct().ToListAsync();
            return years;

        }
    }
}