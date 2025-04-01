using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Dto
{
    public class PositionDto : BaseEntityDto
    {
        public string? PositionName { get; set; }

        public string? Description { get; set; }  // Mô tả
    }
}