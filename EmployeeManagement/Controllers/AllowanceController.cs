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
    public class AllowanceController : Controller
    {
        private readonly EmployeeManagementContext _context;
        private readonly IEmployeeServices _employeeServices;
        public AllowanceController(EmployeeManagementContext context, IEmployeeServices employeeServices)
        {
            _context = context;
            _employeeServices = employeeServices;
        }

        [HttpGet("index")]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, int? searchMonth = null, int? searchYear = null, string? search = null, string? sort = null)
        {
            sort ??= "start_date_desc";
            var isAdmin = User.IsInRole(Role.ADMIN.ToString()) || User.IsInRole(Role.SUPPER_ADMIN.ToString());
            var allowancesQuery = _context.Allowances.AsQueryable();

            // Apply filters if any
            if (searchMonth.HasValue)
                allowancesQuery = allowancesQuery.Where(a => a.StartDate.Month == searchMonth.Value);
            if (searchYear.HasValue)
                allowancesQuery = allowancesQuery.Where(a => a.StartDate.Year == searchYear.Value);
            if (!string.IsNullOrEmpty(search))
                allowancesQuery = allowancesQuery.Where(a => a.AllowanceName.Contains(search));

            // Sorting
            allowancesQuery = sort switch
            {
                "start_date_asc" => allowancesQuery.OrderBy(a => a.StartDate),
                _ => allowancesQuery.OrderByDescending(a => a.StartDate)
            };

            // Pagination
            var totalCount = await allowancesQuery.CountAsync();
            var allowances = await allowancesQuery.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            var employees = await _employeeServices.GetAllEmployeeAsync();
            if (!isAdmin)
            {
                var name = User.Identity.Name;
                var employeesId = employees.Find(x => x.Username == name);
                allowances = allowances.Where(x => x.EmployeeID == employeesId.Id).ToList();
                totalCount = allowances.Count;
            
            }
            var viewModel = new AllowanceIndexViewModel
            {
                Allowances = allowances,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,

            };
            TempData["PageSize"] = pageSize;
            ViewData["employees"] = employees;
            return View(viewModel);
        }


        [HttpPost("add")]
        public async Task<IActionResult> Add(Allowance model)
        {
            try
            {
                model.CreatedAt = DateTime.Now;
                _context.Allowances.Add(model);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Allowance added successfully.";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "An error occurred while adding the allowance.";
                return RedirectToAction("Index");
            }
        }

   
        public async Task<IActionResult> Edit(int id, Allowance model)
        {
            var allowance = await _context.Allowances.FindAsync(id);
            if (allowance == null)
            {
                TempData["ErrorMessage"] = "Allowance not found.";
                return RedirectToAction("Index");
            }

            try
            {
                allowance.AllowanceName = model.AllowanceName;
                allowance.Amount = model.Amount;
                allowance.StartDate = model.StartDate;
                allowance.EndDate = model.EndDate;
                allowance.Description = model.Description;
                allowance.UpdatedAt = DateTime.Now;

                _context.Allowances.Update(allowance);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Allowance updated successfully.";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "An error occurred while updating the allowance.";
                return RedirectToAction("Index");
            }
        }
        [HttpPost("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var allowance = await _context.Allowances.FindAsync(id);
            if (allowance == null)
            {
                TempData["ErrorMessage"] = "Allowance not found.";
                return RedirectToAction("Index");
            }

            try
            {
                _context.Allowances.Remove(allowance);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Allowance deleted successfully.";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the allowance.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost("delete-all")]
        public async Task<IActionResult> DeleteAll()
        {
            try
            {
                _context.Allowances.RemoveRange(_context.Allowances);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "All allowances deleted successfully.";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting all allowances.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost("delete-by-ids")]
        public async Task<IActionResult> DeleteByIds([FromBody] List<int> allowanceIds)
        {
            try
            {
                var allowances = await _context.Allowances.Where(a => allowanceIds.Contains(a.Id)).ToListAsync();
                _context.Allowances.RemoveRange(allowances);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Selected allowances deleted successfully.";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting selected allowances.";
                return RedirectToAction("Index");
            }
        }
    }
}
