using EmployeeManagement.Dto;
using EmployeeManagement.Enum;
using EmployeeManagement.Helper;
using EmployeeManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    [Route("[controller]")]
    public class SalaryAdvanceController : Controller
    {
        private readonly ISalaryAdvanceServices _salaryAdvanceServices;
        private readonly IEmployeeServices _employeeServices;

        public SalaryAdvanceController(ISalaryAdvanceServices salaryAdvanceServices, IEmployeeServices employeeServices)
        {
            _salaryAdvanceServices = salaryAdvanceServices;
            _employeeServices = employeeServices;
        }

        [HttpGet]
        [Route("index")]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, int? searchMonth = null, int? searchYear = null, string? search = null, string? sort = null)
        {
            var isAdmin = User.IsInRole(Role.ADMIN.ToString()) || User.IsInRole(Role.SUPPER_ADMIN.ToString());

            var salaryAdvances = await _salaryAdvanceServices.GetAllSalaryAdvanceAsync(page, pageSize, searchMonth, searchYear, search, sort);
            var employees = await _employeeServices.GetAllEmployeeAsync();
            if (!isAdmin)
            {
                var name = User.Identity.Name;
                var (salaryAdvancesEx, data) = salaryAdvances.Data;
                salaryAdvancesEx = salaryAdvancesEx.Where(x => x.Email == name || x.UserName == name).ToList();
                data = data.Where(x => x.Email == name || x.UserName == name).ToList();
                salaryAdvances.Data = (salaryAdvancesEx, data);

                employees = employees.Where(x => x.Email == name || x.Username == name).ToList();
            }
            var years = await _salaryAdvanceServices.GetListYearsAsync();
            ViewData["Years"] = years;
            TempData["PageSize"] = pageSize;
            ViewData["employees"] = employees;
            var viewDto = new ViewDto<(List<SalaryAdvanceDto> salaryAdvancesEx, List<SalaryAdvanceDto> data)>
            {
                GenericResponse = salaryAdvances
            };

            return View(viewDto);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add(SalaryAdvanceDto model)
        {
            try
            {
                // covert string to decimal
                model.AdvanceAmount = model.AdvanceAmountString.ParseDecimal();
                var response = await _salaryAdvanceServices.CreateSalaryAdvanceAsync(model);
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
                TempData["ErrorMessage"] = "An error occurred while adding the salaryAdvance.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _salaryAdvanceServices.DeleteSalaryAdvanceAsync(id);
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
            var response = await _salaryAdvanceServices.DeleteSalaryAdvanceAllAsync();
            if (response.Success)
            {
                TempData["SuccessMessage"] = response.Message;
                return RedirectToAction("Index");
            }
            TempData["ErrorMessage"] = response.Message;
            return RedirectToAction("Index");
        }

        [HttpPost("delete-by-ids")]
        public async Task<IActionResult> DeleteByIds([FromBody] List<int> salaryAdvanceIds)
        {
            var response = await _salaryAdvanceServices.DeleteSalaryAdvanceByIdsAsync(salaryAdvanceIds);
            if (response.Success)
            {
                TempData["SuccessMessage"] = response.Message;
                return RedirectToAction("Index");
            }
            TempData["ErrorMessage"] = response.Message;
            return RedirectToAction("Index");
        }

        [HttpPost("edit/{salaryAdvanceId}")]
        public async Task<IActionResult> Edit(int salaryAdvanceId, SalaryAdvanceDto salaryAdvanceDto)
        {
            // covert string to decimal
            salaryAdvanceDto.AdvanceAmount = salaryAdvanceDto.AdvanceAmountString.ParseDecimal();
            var response = await _salaryAdvanceServices.UpdateSalaryAdvanceAsync(salaryAdvanceId, salaryAdvanceDto);
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
