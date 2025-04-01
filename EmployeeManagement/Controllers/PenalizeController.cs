using EmployeeManagement.Dto;
using EmployeeManagement.Enum;
using EmployeeManagement.Helper;
using EmployeeManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    [Route("[controller]")]
    public class PenalizeController : Controller
    {
        private readonly IPenalizeServices _penalizeServices;
        private readonly IEmployeeServices _employeeServices;

        public PenalizeController(IPenalizeServices penalizeServices, IEmployeeServices employeeServices)
        {
            _penalizeServices = penalizeServices;
            _employeeServices = employeeServices;
        }

        [HttpGet]
        [Route("index")]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, int? searchMonth = null, int? searchYear = null, string? search = null, string? sort = null)
        {
            var isAdmin = User.IsInRole(Role.ADMIN.ToString()) || User.IsInRole(Role.SUPPER_ADMIN.ToString());
            if (sort == null)
            {
                sort = "penalize_date_desc";
            }
            var penalizes = await _penalizeServices.GetAllPenalizeAsync(page, pageSize, searchMonth, searchYear, search, sort);
            var employees = await _employeeServices.GetAllEmployeeAsync();
            if (!isAdmin)
            {
                var name = User.Identity.Name;
                penalizes.Data = penalizes.Data.Where(x => x.Email == name || x.Username == name).ToList();
                penalizes.TotalCount = penalizes.Data.Count;
                employees = employees.Where(x => x.Email == name || x.Username == name).ToList();
            }
            var years = await _penalizeServices.GetListPenalizeYearsAsync();
            ViewData["Years"] = years;
            TempData["PageSize"] = pageSize;
            ViewData["employees"] = employees;
            var viewDto = new ViewDto<List<PenalizeDto>>
            {
                GenericResponse = penalizes
            };
            return View(viewDto);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add(PenalizeDto model)
        {
            try
            {
                // covert string to decimal
                model.Amount = model.AmountString.ParseDecimal();
                var response = await _penalizeServices.CreatePenalizeAsync(model);
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
                TempData["ErrorMessage"] = "An error occurred while adding the penalize.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _penalizeServices.DeletePenalizeAsync(id);
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
            var response = await _penalizeServices.DeleteAllPenalizesAsync();
            if (response.Success)
            {
                TempData["SuccessMessage"] = response.Message;
                return RedirectToAction("Index");
            }
            TempData["ErrorMessage"] = response.Message;
            return RedirectToAction("Index");
        }

        [HttpPost("delete-by-ids")]
        public async Task<IActionResult> DeleteByIds([FromBody] List<int> penalizeIds)
        {
            var response = await _penalizeServices.DeletePenalizesByIdsAsync(penalizeIds);
            if (response.Success)
            {
                TempData["SuccessMessage"] = response.Message;
                return RedirectToAction("Index");
            }
            TempData["ErrorMessage"] = response.Message;
            return RedirectToAction("Index");
        }

        [HttpPost("edit/{penalizeId}")]
        public async Task<IActionResult> Edit(int penalizeId, PenalizeDto penalizeDto)
        {
            // covert string to decimal
            penalizeDto.Amount = penalizeDto.AmountString.ParseDecimal();
            var response = await _penalizeServices.UpdatePenalizeAsync(penalizeId, penalizeDto);
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
