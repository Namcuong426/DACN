using AutoMapper;
using EmployeeManagement.Dto;
using EmployeeManagement.Models;

namespace EmployeeManagement.Helper
{
    public class AutoMap : Profile
    {
        public AutoMap()
        {
            CreateMap<Account, AccountDto>().ReverseMap();
            CreateMap<Employee, EmployeeDto>().ReverseMap();
            CreateMap<Department, DepartmentDto>().ReverseMap();
            CreateMap<EducationLevel, EducationLevelDto>().ReverseMap();
            CreateMap<Position, PositionDto>().ReverseMap();
            CreateMap<Reward, RewardDto>().ReverseMap();
            CreateMap<Attendance, AttendanceDto>().ReverseMap();
            CreateMap<Penalize, PenalizeDto>().ReverseMap();
            CreateMap<Salary, SalaryDto>().ReverseMap();
            CreateMap<SalaryAdvance, SalaryAdvanceDto>().ReverseMap();
            CreateMap<Leave, LeaveDto>().ReverseMap();
        }
    }
}
