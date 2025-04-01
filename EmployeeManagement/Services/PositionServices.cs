using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EmployeeManagement.Context;
using EmployeeManagement.Dto;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Services
{
    public class PositionServices : IPositionServices
    {
        private readonly IMapper _mapper;
        private readonly EmployeeManagementContext _context;

        public PositionServices(IMapper mapper, EmployeeManagementContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<List<PositionDto>> GetAllPositionAsync()
        {
            var positions = await _context.Positions.ToListAsync();
            var positionDtos = _mapper.Map<List<PositionDto>>(positions);
            return positionDtos;
        }
    }
}