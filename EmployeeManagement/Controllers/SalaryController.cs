using EmployeeManagement.Context;
using EmployeeManagement.Dto;
using EmployeeManagement.Enum;
using EmployeeManagement.Helper;
using EmployeeManagement.Models;
using EmployeeManagement.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Controllers
{
    [Route("[controller]")]
    public class SalaryController : Controller
    {
        private readonly ISalaryServices _salaryServices;
        private readonly IEmployeeServices _employeeServices;
        private readonly EmployeeManagementContext _context;
        public SalaryController(ISalaryServices salaryServices, IEmployeeServices employeeServices,EmployeeManagementContext context)
        {
            _salaryServices = salaryServices;
            _employeeServices = employeeServices;
            _context = context;
        }

        [HttpGet]
        [Route("index")]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, int? searchMonth = null, int? searchYear = null, string? search = null, string? sort = null)
        {
            var isAdmin = User.IsInRole(Role.ADMIN.ToString()) || User.IsInRole(Role.SUPPER_ADMIN.ToString());

            var salarys = await _salaryServices.GetAllSalaryAsync(page, pageSize, searchMonth, searchYear, search, sort);
            var employees = await _employeeServices.GetAllEmployeeAsync();
            if (!isAdmin)
            {
                var name = User.Identity.Name;
                var (salarysEx, data) = salarys.Data;
                salarysEx = salarysEx.Where(x => x.Email == name || x.UserName == name).ToList();
                data = data.Where(x => x.Email == name || x.UserName == name).ToList();
                salarys.Data = (salarysEx, data);

                employees = employees.Where(x => x.Email == name || x.Username == name).ToList();
            }
            var years = await _salaryServices.GetListYearsAsync();
            ViewData["Years"] = years;
            TempData["PageSize"] = pageSize;
            ViewData["employees"] = employees;
            ViewData["searchMonth"] = searchMonth;
            var viewDto = new ViewDto<(List<SalaryDto> salarysEx, List<SalaryDto> data)>
            {
                GenericResponse = salarys
            };

            return View(viewDto);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add(SalaryDto model)
        {
            try
            {
                // covert string to decimal
                model.BasicSalary = model.BasicSalaryString.ParseDecimal();
                model.Bonus = model.BonusString.ParseDecimal();
                model.Penalty = model.PenaltyString.ParseDecimal();
                model.Allowance = model.AllowanceString.ParseDecimal();
                model.NetSalary = model.NetSalaryString.ParseDecimal();
                model.SalaryAdvance = model.SalaryAdvanceString.ParseDecimal();
                var response = await _salaryServices.CreateSalaryAsync(model);
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
                TempData["ErrorMessage"] = "An error occurred while adding the salary.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _salaryServices.DeleteSalaryAsync(id);
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
            var response = await _salaryServices.DeleteSalaryAllAsync();
            if (response.Success)
            {
                TempData["SuccessMessage"] = response.Message;
                return RedirectToAction("Index");
            }
            TempData["ErrorMessage"] = response.Message;
            return RedirectToAction("Index");
        }

        [HttpPost("delete-by-ids")]
        public async Task<IActionResult> DeleteByIds([FromBody] List<int> salaryIds)
        {
            var response = await _salaryServices.DeleteSalaryByIdsAsync(salaryIds);
            if (response.Success)
            {
                TempData["SuccessMessage"] = response.Message;
                return RedirectToAction("Index");
            }
            TempData["ErrorMessage"] = response.Message;
            return RedirectToAction("Index");
        }

        [HttpPost("edit/{salaryId}")]
        public async Task<IActionResult> Edit(int salaryId, SalaryDto salaryDto)
        {
            // covert string to decimal
            salaryDto.BasicSalary = salaryDto.BasicSalaryString.ParseDecimal();
            salaryDto.Bonus = salaryDto.BonusString.ParseDecimal();
            salaryDto.Penalty = salaryDto.PenaltyString.ParseDecimal();
            salaryDto.Allowance = salaryDto.AllowanceString.ParseDecimal();
            salaryDto.NetSalary = salaryDto.NetSalaryString.ParseDecimal();
            salaryDto.SalaryAdvance = salaryDto.SalaryAdvanceString.ParseDecimal();
            var response = await _salaryServices.UpdateSalaryAsync(salaryId, salaryDto);
            if (response.Success)
            {
                TempData["SuccessMessage"] = response.Message;
                return RedirectToAction("Index");
            }
            TempData["ErrorMessage"] = response.Message;
            return RedirectToAction("Index");
        }
        [HttpGet("GetEmployeeSalary")]
        public async Task<IActionResult> GetEmployeeSalary(int employeeId, int month, int year)
        {
            var salary = await _salaryServices.GetSalaryByEmployeeIdAsync(employeeId, month, year);
            var allow = await _context.Allowances.Where(x => x.EmployeeID == employeeId).ToListAsync();
            var insurances = await _context.Insurances.Where(x => x.EmployeeID == employeeId).ToListAsync();
            if (allow.Count != 0)
            {
               salary.Allowance = allow.Sum(x => x.Amount);
                salary.AllowanceString = salary.Allowance.FormatNumberDecimal();
            }
            if (insurances.Count != 0)
            {
                salary.Insurance = insurances.Sum(x => x.PremiumAmount);
            }
            if (salary == null)
            {
                return NotFound();
            }
            return Ok(salary);
        }
        [HttpPost("ExportToExcel")]
        public IActionResult ExportToExcel([FromBody] List<SalaryDto> salaryDtos)
        {
            var stream = _salaryServices.ExportEmployeeSalariesToExcel(salaryDtos);
            var fileName = "LuongNhanVien.xlsx";
            var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            return File(stream, contentType, fileName);
        }
    }
}
