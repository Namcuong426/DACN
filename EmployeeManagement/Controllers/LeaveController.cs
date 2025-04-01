using EmployeeManagement.Dto;
using EmployeeManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    [Route("[controller]")]
    public class LeaveController : Controller
    {
        private readonly ILeaveService _leaveService;

        public LeaveController(ILeaveService leaveService)
        {
            _leaveService = leaveService;
        }

        [HttpGet]
        [Route("index")]
        public async Task<IActionResult> Index()
        {
            var (leaves, countLeaveRemaining) = await _leaveService.GetAllLeaveService();
            ViewData["CountLeaveRemaining"] = countLeaveRemaining;
            return View(leaves);

        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateLeave(LeaveDto model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Invalid data.";
                return View(model);
            }

            var response = await _leaveService.CreateLeaveAsync(model);
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

        [HttpPost("edit/{leaveId}")]
        public async Task<IActionResult> Edit(int leaveId, LeaveDto model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Invalid data.";
                return View(model);
            }

            var response = await _leaveService.UpdateLeaveAsync(leaveId, model);
            if (response.Success)
            {
                TempData["SuccessMessage"] = response.Message;
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = response.Message;
                return View(model);
            }
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _leaveService.DeleteLeaveAsync(id);
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

        [HttpPost("change-status-leave/{leaveId}")]
        public async Task<IActionResult> ChangeStatusLeave(LeaveDto model, int leaveId)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Invalid data.";
                return View(model);
            }

            var response = await _leaveService.ChangeStatusLeave(model, leaveId);
            if (response.Success)
            {
                TempData["SuccessMessage"] = response.Message;
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = response.Message;
                return View(model);
            }
        }

        [HttpPost("statisticsLeave")]
        public async Task<IActionResult> StatisticsLeave(StatisticLeaveDto model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Invalid data.";
                return View(model);
            }

            var response = await _leaveService.StatisticsLeaveAsync(model);
            if (response.Success)
            {
                ViewBag.StatisticLeave = response.Data;
                return Ok(response);
            }
            else
            {
                TempData["ErrorMessage"] = response.Message;
                return View(model);
            }
        }
    }
}
