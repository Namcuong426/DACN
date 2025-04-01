using EmployeeManagement.Context;
using EmployeeManagement.Models;
using EmployeeManagement.Enum;
using EmployeeManagement.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagement.Services;
using EmployeeManagement.Dto;

namespace EmployeeManagement.Controllers
{
    [Route("[controller]")]
    public class InsuranceController : Controller
    {
        private readonly EmployeeManagementContext _context;
        private readonly IEmployeeServices _employeeServices;

        public InsuranceController(EmployeeManagementContext context, IEmployeeServices employeeServices)
        {
            _context = context;
            _employeeServices = employeeServices;
        }

        [HttpGet("index")]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, int? searchMonth = null, int? searchYear = null, string? search = null, string? sort = null)
        {
            sort ??= "start_date_desc";
            var isAdmin = User.IsInRole(Role.ADMIN.ToString()) || User.IsInRole(Role.SUPPER_ADMIN.ToString());
            var insuranceQuery = _context.Insurances.AsQueryable();

            // Apply filters if any
            if (searchMonth.HasValue)
                insuranceQuery = insuranceQuery.Where(i => i.StartDate.Month == searchMonth.Value);
            if (searchYear.HasValue)
                insuranceQuery = insuranceQuery.Where(i => i.StartDate.Year == searchYear.Value);
            if (!string.IsNullOrEmpty(search))
                insuranceQuery = insuranceQuery.Where(i => i.InsuranceName.Contains(search));

            // Sorting
            insuranceQuery = sort switch
            {
                "start_date_asc" => insuranceQuery.OrderBy(i => i.StartDate),
                _ => insuranceQuery.OrderByDescending(i => i.StartDate)
            };

            // Pagination
            var totalCount = await insuranceQuery.CountAsync();
            var insurances = await insuranceQuery.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            var employees = await _employeeServices.GetAllEmployeeAsync();

            if (!isAdmin)
            {
                var name = User.Identity.Name;
                var employee = employees.Find(x => x.Username == name);
                insurances = insurances.Where(x => x.EmployeeID == employee.Id).ToList();
                totalCount = insurances.Count;
            }

            var viewModel = new InsuranceIndexViewModel
            {
                Insurances = insurances,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };

            TempData["PageSize"] = pageSize;
            ViewData["employees"] = employees;
            return View(viewModel);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add(Insurance model)
        {
            try
            {
                model.CreatedAt = DateTime.Now;
                _context.Insurances.Add(model);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Insurance added successfully.";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "An error occurred while adding the insurance.";
                return RedirectToAction("Index");
            }
        }

        //[HttpPost("edit/{id}")]
        public async Task<IActionResult> Edit(int id, Insurance model)
        {
            var insurance = await _context.Insurances.FindAsync(id);
            if (insurance == null)
            {
                TempData["ErrorMessage"] = "Insurance not found.";
                return RedirectToAction("Index");
            }

            try
            {
                insurance.InsuranceName = model.InsuranceName;
                insurance.PremiumAmount = model.PremiumAmount;
                insurance.StartDate = model.StartDate;
                insurance.EndDate = model.EndDate;
                insurance.Description = model.Description;
                insurance.UpdatedAt = DateTime.Now;

                _context.Insurances.Update(insurance);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Insurance updated successfully.";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "An error occurred while updating the insurance.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var insurance = await _context.Insurances.FindAsync(id);
            if (insurance == null)
            {
                TempData["ErrorMessage"] = "Insurance not found.";
                return RedirectToAction("Index");
            }

            try
            {
                _context.Insurances.Remove(insurance);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Insurance deleted successfully.";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the insurance.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost("delete-all")]
        public async Task<IActionResult> DeleteAll()
        {
            try
            {
                _context.Insurances.RemoveRange(_context.Insurances);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "All insurances deleted successfully.";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting all insurances.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost("delete-by-ids")]
        public async Task<IActionResult> DeleteByIds([FromBody] List<int> insuranceIds)
        {
            try
            {
                var insurances = await _context.Insurances.Where(i => insuranceIds.Contains(i.Id)).ToListAsync();
                _context.Insurances.RemoveRange(insurances);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Selected insurances deleted successfully.";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting selected insurances.";
                return RedirectToAction("Index");
            }
        }
    }
}
