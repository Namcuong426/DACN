using EmployeeManagement.Dto;
using EmployeeManagement.Enum;
using EmployeeManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    [Route("[controller]")]
    public class AttendanceController : Controller
    {
        private readonly IAttendanceServices _AttendanceServices;
        private readonly IEmployeeServices _employeeServices;

        public AttendanceController(IAttendanceServices AttendanceServices, IEmployeeServices employeeServices)
        {
            _AttendanceServices = AttendanceServices;
            _employeeServices = employeeServices;
        }

        [HttpGet]
        [Route("index")]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 5, int? searchMonth = null, int? searchYear = null, string? search = null, string? sort = null)
        {
            var isAdmin = User.IsInRole(Role.ADMIN.ToString()) || User.IsInRole(Role.SUPPER_ADMIN.ToString());
            if (sort == null)
            {
                if (!isAdmin && searchMonth == null && searchYear == null)
                {
                    sort = "attendance_admin";
                }
                else
                {
                    sort = "attendance_date_desc";
                }
            }
            var Attendances = await _AttendanceServices.GetAllAttendanceAsync(page, pageSize, searchMonth, searchYear, search, sort);
            var employees = await _employeeServices.GetAllEmployeeAsync();
            var years = await _AttendanceServices.GetListYearsAsync();
            TempData["PageSize"] = pageSize;
            TempData["Years"] = years;
            if (!isAdmin)
            {
                var name = User.Identity.Name;
                Attendances.Data = Attendances.Data.Where(x => x.Email == name || x.Username == name).ToList();
                Attendances.TotalCount = Attendances.Data.Count;
                employees = employees.Where(x => x.Email == name || x.Username == name).ToList();

            }
            ViewData["employees"] = employees;
            var viewDto = new ViewDto<List<AttendanceDto>>
            {
                GenericResponse = Attendances
            };
            return View(viewDto);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add(AttendanceDto model)
        {
            try
            {
                var response = await _AttendanceServices.CreateAttendanceAsync(model);
                if (response.Success)
                {
                    TempData["SuccessMessage"] = response.Message;
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ErrorMessage"] = response.Message;
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while adding the Attendance.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _AttendanceServices.DeleteAttendanceAsync(id);
            if (response.Success)
            {
                TempData["SuccessMessage"] = response.Message;
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = response.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpPost("delete-all")]
        public async Task<IActionResult> DeleteAll()
        {
            var response = await _AttendanceServices.DeleteAttendanceAllAsync();
            if (response.Success)
            {
                TempData["SuccessMessage"] = response.Message;
                return RedirectToAction("Index");
            }
            TempData["ErrorMessage"] = response.Message;
            return RedirectToAction("Index");
        }

        [HttpPost("delete-by-ids")]
        public async Task<IActionResult> DeleteByIds([FromBody] List<int> attendanceIds)
        {
            var response = await _AttendanceServices.DeleteAttendanceByIdsAsync(attendanceIds);
            if (response.Success)
            {
                TempData["SuccessMessage"] = response.Message;
                return RedirectToAction("Index");
            }
            TempData["ErrorMessage"] = response.Message;
            return RedirectToAction("Index");
        }

        [HttpPost("edit/{attendanceId}")]
        public async Task<IActionResult> Edit(int attendanceId, AttendanceDto attendanceDto)
        {
            var response = await _AttendanceServices.UpdateAttendanceAsync(attendanceId, attendanceDto);
            if (response.Success)
            {
                TempData["SuccessMessage"] = response.Message;
                return RedirectToAction("Index");
            }
            TempData["ErrorMessage"] = response.Message;
            return RedirectToAction("Index");
        }
    }
}
