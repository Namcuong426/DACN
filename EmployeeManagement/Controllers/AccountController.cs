using EmployeeManagement.Context;
using EmployeeManagement.Dto;
using EmployeeManagement.Helper;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EmployeeManagement.Controllers
{
    public class AccountController : Controller
    {
        private readonly EmployeeManagementContext _context;
        public AccountController(EmployeeManagementContext context)
        {
            _context = context;
        }
        public IActionResult Register()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(AccountDto model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = _context.Accounts.FirstOrDefault(a => a.Username == model.Username);
                if (existingUser == null)
                {
                    // Hash the password before saving
                    model.Password = HashingHelper.ComputeMd5Hash(model.Password);
                    var account = new Account
                    {
                        Username = model.Username,
                        Password = model.Password,
                        Role = Enum.Role.SUPPER_ADMIN,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };

                    _context.Accounts.Add(account);

                    await _context.SaveChangesAsync();
                    return RedirectToAction("index", "home");
                }
                ModelState.AddModelError("", "Username already exists");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(AccountLoginDto model)
        {
            TempData["ErrorMessage"] = "";
            if (ModelState.IsValid)
            {
                var hashedPassword = HashingHelper.ComputeMd5Hash(model.Password);
                var user = _context.Accounts.FirstOrDefault(a => a.Username == model.Username && a.Password == hashedPassword);
                if (user != null)
                {
                    // check employee
                    var employee = _context.Employees.FirstOrDefault(e => e.Email == model.Username || e.Username == model.Username);
                    var claims = new List<Claim>();
                    if (employee is not null)
                    {
                        if (employee.IsActivated == false)
                        {
                            TempData["ErrorMessage"] = "Tài khoản của bạn đã bị khóa";
                            return View(model);
                        }
                        // Create claims for the user
                        claims = new List<Claim>
                        {
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim("EmployeeId", employee.Id.ToString()),
                        new Claim("EmployeeName", employee.Name.ToString()),
                        new Claim(ClaimTypes.Role, user.Role.ToString())  // Assuming you have a Role property
                    };

                    }
                    else
                    {
                        claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim("EmployeeId",  user.Id.ToString()),
                        new Claim("EmployeeName",  user.Username.ToString()),
                        new Claim(ClaimTypes.Role,  user.Role.ToString())  // Assuming you have a Role property
                    };
                    }


                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true
                    };

                    // Sign in the user
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                    return RedirectToAction("Index", "Home");

                }
                else
                {
                    TempData["ErrorMessage"] = "Email hoặc mật khẩu không đúng";
                }


            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
