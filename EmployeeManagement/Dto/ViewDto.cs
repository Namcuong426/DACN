using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Dto
{
    public class ViewDto<T>
    {
        public GenericResponse<T> GenericResponse { get; set; }
    }
}