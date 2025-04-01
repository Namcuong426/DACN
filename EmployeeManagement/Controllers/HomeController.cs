using EmployeeManagement.Context;
using EmployeeManagement.Models;
using EmployeeManagement.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EmployeeManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly EmployeeManagementContext _context;

        public HomeController(
            ILogger<HomeController> logger,
            EmployeeManagementContext context
            )
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var employees = _context.Employees.ToList().Count();
            var department = _context.Departments.ToList().Count();
            ViewBag.EmployeeCount = employees;
            ViewBag.DepartmentCount = department;
            return View();
        }
        public IActionResult HomePage()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
