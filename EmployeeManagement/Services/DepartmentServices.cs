using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EmployeeManagement.Context;
using EmployeeManagement.Dto;
using EmployeeManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Services
{
    public class DepartmentServices : IDepartmentServices
    {
        private readonly IMapper _mapper;
        private readonly EmployeeManagementContext _context;

        public DepartmentServices(IMapper mapper, EmployeeManagementContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<List<DepartmentDto>> GetAllDepartmentAsync()
        {
            var departments = await _context.Departments.ToListAsync();
            var departmentDtos = _mapper.Map<List<DepartmentDto>>(departments);
            return departmentDtos;
        }
        public async Task<GenericResponseSingle<DepartmentDto>> GetDepartmentByIdAsync(int departmentId)
        {
            var response = new GenericResponseSingle<DepartmentDto>();

            try
            {
                var department = await _context.Departments.Include(d => d.Employees)
                                                           .FirstOrDefaultAsync(d => d.Id == departmentId);
                if (department == null)
                {
                    response.Success = false;
                    response.Message = "Department not found.";
                    return response;
                }

                response.Data = _mapper.Map<DepartmentDto>(department);
                response.Success = true;
                response.Message = "Success";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Failed to retrieve department.";
            }

            return response;
        }

        public async Task<GenericResponseSingle<DepartmentDto>> CreateDepartmentAsync(DepartmentDto departmentDto)
        {
            var response = new GenericResponseSingle<DepartmentDto>();

            try
            {
                var department = _mapper.Map<Department>(departmentDto);
                department.CreatedAt = DateTime.Now;
                department.UpdatedAt = DateTime.Now;

                await _context.Departments.AddAsync(department);
                await _context.SaveChangesAsync();

                response.Data = _mapper.Map<DepartmentDto>(department);
                response.Success = true;
                response.Message = "Department created successfully.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Failed to create department.";
            }

            return response;
        }

        public async Task<GenericResponseSingle<DepartmentDto>> UpdateDepartmentAsync(int departmentId, DepartmentDto departmentDto)
        {
            var response = new GenericResponseSingle<DepartmentDto>();

            try
            {
                var existingDepartment = await _context.Departments.FindAsync(departmentId);
                if (existingDepartment == null)
                {
                    response.Success = false;
                    response.Message = "Department not found.";
                    return response;
                }

                _mapper.Map(departmentDto, existingDepartment);
                existingDepartment.UpdatedAt = DateTime.Now;

                await _context.SaveChangesAsync();

                response.Data = _mapper.Map<DepartmentDto>(existingDepartment);
                response.Success = true;
                response.Message = "Department updated successfully.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Failed to update department.";
            }

            return response;
        }

        public async Task<GenericResponseSingle<bool>> DeleteDepartmentAsync(int departmentId)
        {
            var response = new GenericResponseSingle<bool>();

            try
            {
                var department = await _context.Departments.FindAsync(departmentId);
                if (department == null)
                {
                    response.Success = false;
                    response.Message = "Department not found.";
                    return response;
                }

                _context.Departments.Remove(department);
                await _context.SaveChangesAsync();

                response.Data = true;
                response.Success = true;
                response.Message = "Department deleted successfully.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Failed to delete department.";
            }

            return response;
        }
    }
}