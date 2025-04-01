using EmployeeManagement.Dto;
using EmployeeManagement.Enum;
using EmployeeManagement.Helper;
using EmployeeManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    [Route("[controller]")]
    public class RewardController : Controller
    {
        private readonly IRewardServices _rewardServices;
        private readonly IEmployeeServices _employeeServices;

        public RewardController(IRewardServices rewardServices, IEmployeeServices employeeServices)
        {
            _rewardServices = rewardServices;
            _employeeServices = employeeServices;
        }

        [HttpGet]
        [Route("index")]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, int? searchMonth = null, int? searchYear = null, string? search = null, string? sort = null)
        {
            var isAdmin = User.IsInRole(Role.ADMIN.ToString()) || User.IsInRole(Role.SUPPER_ADMIN.ToString());
            if (sort == null)
            {
                sort = "reward_date_desc";
            }
            var rewards = await _rewardServices.GetAllRewardAsync(page, pageSize, searchMonth, searchYear, search, sort);
            var employees = await _employeeServices.GetAllEmployeeAsync();
            if (!isAdmin)
            {
                var name = User.Identity.Name;
                rewards.Data = rewards.Data.Where(x => x.Email == name || x.Username == name).ToList();
                rewards.TotalCount = rewards.Data.Count;
                employees = employees.Where(x => x.Email == name || x.Username == name).ToList();
            }
            var years = await _rewardServices.GetListYearsAsync();
            ViewData["Years"] = years;
            TempData["PageSize"] = pageSize;
            ViewData["employees"] = employees;
            var viewDto = new ViewDto<List<RewardDto>>
            {
                GenericResponse = rewards
            };
            return View(viewDto);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add(RewardDto model)
        {
            try
            {
                // covert string to decimal
                model.Amount = model.AmountString.ParseDecimal();
                var response = await _rewardServices.CreateRewardAsync(model);
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
                TempData["ErrorMessage"] = "An error occurred while adding the reward.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _rewardServices.DeleteRewardAsync(id);
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
            var response = await _rewardServices.DeleteRewardAllAsync();
            if (response.Success)
            {
                TempData["SuccessMessage"] = response.Message;
                return RedirectToAction("Index");
            }
            TempData["ErrorMessage"] = response.Message;
            return RedirectToAction("Index");
        }

        [HttpPost("delete-by-ids")]
        public async Task<IActionResult> DeleteByIds([FromBody] List<int> rewardIds)
        {
            var response = await _rewardServices.DeleteRewardByIdsAsync(rewardIds);
            if (response.Success)
            {
                TempData["SuccessMessage"] = response.Message;
                return RedirectToAction("Index");
            }
            TempData["ErrorMessage"] = response.Message;
            return RedirectToAction("Index");
        }

        [HttpPost("edit/{rewardId}")]
        public async Task<IActionResult> Edit(int rewardId, RewardDto rewardDto)
        {
            rewardDto.Amount = rewardDto.AmountString.ParseDecimal();
            var response = await _rewardServices.UpdateRewardAsync(rewardId, rewardDto);
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
