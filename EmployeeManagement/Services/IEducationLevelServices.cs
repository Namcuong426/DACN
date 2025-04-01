using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagement.Dto;

namespace EmployeeManagement.Services
{
    public interface IEducationLevelServices
    {
        Task<List<EducationLevelDto>> GetAllEducationLevelAsync();
    }
}