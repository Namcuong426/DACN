using EmployeeManagement.Dto;
using EmployeeManagement.Enum;
using EmployeeManagement.Helper;
using EmployeeManagement.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace EmployeeManagement.Controllers
{
    [Route("[controller]")]
    public class EmployeeController : Controller
    {
        private readonly ILogger<EmployeeController> _logger;
        private readonly IDepartmentServices _departmentServices;
        private readonly IEmployeeServices _employeeServices;
        private readonly IPositionServices _positionServices;
        private readonly IEducationLevelServices _educationLevelServices;
        private readonly IWebHostEnvironment _environment;
        public EmployeeController(ILogger<EmployeeController> logger, IEmployeeServices employeeServices,
        IDepartmentServices departmentServices,
        IPositionServices positionServices,
        IWebHostEnvironment environment,
        IEducationLevelServices educationLevelServices)
        {
            _logger = logger;
            _employeeServices = employeeServices;
            _departmentServices = departmentServices;
            _positionServices = positionServices;
            _educationLevelServices = educationLevelServices;
            _environment = environment;
        }
        [HttpGet]
        [Route("index")]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10,
            string? search = null, string? sort = null,
            int? educationLevelId = null,
            int? departmentId = null,
            int? positionId = null
            )
        {
            if (sort == null)
            {
                sort = "updated_at_desc";
            }
            var isAdmin = User.IsInRole(Role.ADMIN.ToString()) || User.IsInRole(Role.SUPPER_ADMIN.ToString());
            var employee = await _employeeServices.GetAllEmployeeAsync(page, pageSize,
                search, sort, educationLevelId, departmentId, positionId);
            var departments = await _departmentServices.GetAllDepartmentAsync();
            var positions = await _positionServices.GetAllPositionAsync();
            var educationLevels = await _educationLevelServices.GetAllEducationLevelAsync();
            ViewData["departments"] = departments;
            ViewData["positions"] = positions;
            ViewData["educationLevels"] = educationLevels;
            TempData["PageSize"] = pageSize;
            var employees = await _employeeServices.GetAllEmployeeAsync();
            if (!isAdmin)
            {
                employees.Where(x => x.Email == User.Identity.Name).ToList();
                employee.Data = employee.Data.Where(x => x.Email == User.Identity.Name).ToList();
                var viewDtoInfor = new ViewDto<List<EmployeeDto>>
                {
                    GenericResponse = employee
                };
                TempData["viewDtoInfor"] = JsonSerializer.Serialize(viewDtoInfor);
                return RedirectToAction("InforEmployee",viewDtoInfor);
            }
            ViewData["employees"] = employees;

            var viewDto = new ViewDto<List<EmployeeDto>>
            {
                GenericResponse = employee
            };
            return View(viewDto);
        }
        [HttpGet]
        [Route("InforEmployee")]
        public async Task<IActionResult> InforEmployee()
        {
            // Retrieve and deserialize from TempData using System.Text.Json
            if (TempData["viewDtoInfor"] != null)
            {
                var viewDtoInfor = JsonSerializer.Deserialize<ViewDto<List<EmployeeDto>>>(TempData["viewDtoInfor"].ToString());

                var employees = await _employeeServices.GetAllEmployeeAsync();
                var departments = await _departmentServices.GetAllDepartmentAsync();
                var positions = await _positionServices.GetAllPositionAsync();
                var educationLevels = await _educationLevelServices.GetAllEducationLevelAsync();
                ViewData["departments"] = departments;
                ViewData["positions"] = positions;
                ViewData["educationLevels"] = educationLevels;
                ViewData["employees"] = employees;

                return View(viewDtoInfor);
            }

            // Handle the case where TempData does not contain the expected data
            return RedirectToAction("Index");
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add(EmployeeDto model)
        {
            try
            {
                // covert string to decimal
                model.BasicSalary = model.BasicSalaryString.ParseDecimal();
                var response = await _employeeServices.CreateEmployeeAsync(model);
                if (response.Success)
                {
                    TempData["SuccessMessage"] = response.Message;
                    return RedirectToAction("Index");

                }
                else
                {
                    // Trường hợp không thêm mới thành công
                    TempData["ErrorMessage"] = response.Message;
                    return RedirectToAction("Index");
                }


            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }
        [HttpPost("delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _employeeServices.DeleteEmployeeAsync(id);
            if (response.Success)
            {
                TempData["SuccessMessage"] = response.Message;
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = response.Message;
                return RedirectToAction("Index"); // Hoặc chuyển hướng đến trang khác tùy ý
            }
        }
        [HttpPost("delete-all")]
        public async Task<IActionResult> DeleteAll()
        {
            var response = await _employeeServices.DeleteEmployeeAllAsync();
            if (response.Success)
            {
                TempData["SuccessMessage"] = response.Message;
                return RedirectToAction("Index");
            }
            TempData["ErrorMessage"] = response.Message;
            return RedirectToAction("Index");
        }
        [HttpPost("delete-by-ids")]
        public async Task<IActionResult> DeleteByIds([FromBody] List<int> employeeIds)
        {
            var response = await _employeeServices.DeleteEmployeeByIdsAsync(employeeIds);
            if (response.Success)
            {
                TempData["SuccessMessage"] = response.Message;
                return RedirectToAction("Index");
            }
            TempData["ErrorMessage"] = response.Message;
            return RedirectToAction("Index");
        }
        [HttpPost("edit/{employeeId}")]
        public async Task<IActionResult> Edit(int employeeId, EmployeeDto employeeDto)
        {
            // covert string to decimal
            employeeDto.BasicSalary = employeeDto.BasicSalaryString.ParseDecimal();
            var response = await _employeeServices.UpdateEmployeeAsync(employeeId, employeeDto);
            if (response.Success)
            {
                TempData["SuccessMessage"] = response.Message;
                return RedirectToAction("Index");
            }
            TempData["ErrorMessage"] = response.Message;
            return RedirectToAction("Index");
        }
        [HttpGet("uploads/{fileName}")]
        public IActionResult GetImage(string fileName)
        {
            var imagePath = Path.Combine(_environment.WebRootPath, "images", fileName);

            if (System.IO.File.Exists(imagePath))
            {
                var imageFileStream = System.IO.File.OpenRead(imagePath);
                return File(imageFileStream, "image/jpeg"); // Adjust content type based on your file type
            }
            else
            {
                var defaultImagePath = Path.Combine(_environment.WebRootPath, "images", "no-images.jpg");
                var imageFileStream = System.IO.File.OpenRead(defaultImagePath);
                return File(imageFileStream, "image/jpeg"); // Adjust content type based on your file type
            }

        }
    }

}