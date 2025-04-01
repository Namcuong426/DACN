using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagement.Dto;

namespace EmployeeManagement.Services
{
    public interface IDepartmentServices
    {
        Task<List<DepartmentDto>> GetAllDepartmentAsync();
        Task<GenericResponseSingle<DepartmentDto>> GetDepartmentByIdAsync(int departmentId);
        Task<GenericResponseSingle<DepartmentDto>> CreateDepartmentAsync(DepartmentDto departmentDto);
        Task<GenericResponseSingle<DepartmentDto>> UpdateDepartmentAsync(int departmentId, DepartmentDto departmentDto);
        Task<GenericResponseSingle<bool>> DeleteDepartmentAsync(int departmentId);
    }
}