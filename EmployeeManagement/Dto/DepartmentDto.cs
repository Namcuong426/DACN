using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Dto
{
    public class DepartmentDto : BaseEntityDto
    {
        public string? DepartmentName { get; set; }  // Tên phòng ban (e.g., IT)
        public string? Description { get; set; }  // Mô tả
        public string? Abbreviation { get; set; }  // Viết tắt (e.g., IT)
    }
}