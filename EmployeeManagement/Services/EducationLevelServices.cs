using AutoMapper;
using EmployeeManagement.Context;
using EmployeeManagement.Dto;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Services
{
    public class EducationLevelServices : IEducationLevelServices
    {
        private readonly IMapper _mapper;
        private readonly EmployeeManagementContext _context;

        public EducationLevelServices(IMapper mapper, EmployeeManagementContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<List<EducationLevelDto>> GetAllEducationLevelAsync()
        {
            var educationLevels = await _context.EducationLevels.ToListAsync();
            var educationLevelDtos = _mapper.Map<List<EducationLevelDto>>(educationLevels);
            return educationLevelDtos;
        }
    }
}