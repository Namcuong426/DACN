using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagement.Dto;

namespace EmployeeManagement.Services
{
    public interface IPositionServices
    {
        Task<List<PositionDto>> GetAllPositionAsync();
    }
}