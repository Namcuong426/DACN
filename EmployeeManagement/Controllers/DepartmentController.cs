using EmployeeManagement.Dto;
using EmployeeManagement.Services;
using Microsoft.AspNetCore.Mvc;

[Route("[controller]")]
public class DepartmentController : Controller
{
    private readonly IDepartmentServices _departmentService;

    public DepartmentController(IDepartmentServices departmentService)
    {
        _departmentService = departmentService;
    }

    [HttpGet]
    [Route("index")]
    public async Task<IActionResult> Index()
    {
        var departments = await _departmentService.GetAllDepartmentAsync();
        return View(departments);
    }

    [HttpGet("{departmentId}")]
    public async Task<IActionResult> Details(int departmentId)
    {
        var response = await _departmentService.GetDepartmentByIdAsync(departmentId);
        if (!response.Success)
        {
            TempData["ErrorMessage"] = response.Message;
            return RedirectToAction("Index");
        }

        return View(response.Data);
    }

    [HttpGet("create")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(DepartmentDto model)
    {
        if (!ModelState.IsValid)
        {
            TempData["ErrorMessage"] = "Invalid data.";
            return View(model);
        }

        var response = await _departmentService.CreateDepartmentAsync(model);
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

    [HttpGet("edit/{departmentId}")]
    public async Task<IActionResult> Edit(int departmentId)
    {
        var response = await _departmentService.GetDepartmentByIdAsync(departmentId);
        if (!response.Success)
        {
            TempData["ErrorMessage"] = response.Message;
            return RedirectToAction("Index");
        }

        return View(response.Data);
    }

    [HttpPost("edit/{departmentId}")]
    public async Task<IActionResult> Edit(int departmentId, DepartmentDto model)
    {
        if (!ModelState.IsValid)
        {
            TempData["ErrorMessage"] = "Invalid data.";
            return View(model);
        }

        var response = await _departmentService.UpdateDepartmentAsync(departmentId, model);
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
        var response = await _departmentService.DeleteDepartmentAsync(id);
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
}
