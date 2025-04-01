using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Dto
{
    public class GenericResponse<T> : PagingDto
    {
        public T? Data { get; set; }
        public bool Success { get; set; }
        public string? Message { get; set; } = null;
    }
    public class GenericResponseSingle<T>
    {
        public T? Data { get; set; }
        public bool Success { get; set; }
        public string? Message { get; set; } = null;
    }
    public class PagingDto
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
    }
}