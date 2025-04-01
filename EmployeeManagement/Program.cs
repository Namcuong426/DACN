using EmployeeManagement.Context;
using EmployeeManagement.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddCors(p => p.AddPolicy("corspolicy", build =>
{
    build.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

var connectionString = builder.Configuration.GetConnectionString("EmployeeManagement");

builder.Services.AddDbContext<EmployeeManagementContext>(x => x.UseSqlServer(connectionString));
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<IEmployeeServices, EmployeeServices>();
builder.Services.AddScoped<IDepartmentServices, DepartmentServices>();
builder.Services.AddScoped<IPositionServices, PositionServices>();
builder.Services.AddScoped<IEducationLevelServices, EducationLevelServices>();
builder.Services.AddScoped<IRewardServices, RewardServices>();
builder.Services.AddScoped<IAttendanceServices, AttendanceServices>();
builder.Services.AddScoped<IPenalizeServices, PenalizeServices>();
builder.Services.AddScoped<ISalaryServices, SalaryServices>();
builder.Services.AddScoped<ISalaryAdvanceServices, SalaryAdvanceServices>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<EmployeeServices>();
builder.Services.AddScoped<ILeaveService, LeaveService>();
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
builder.Services.AddAuthentication(
    CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(option =>
    {
        //option.LoginPath = "/Account/Login";

    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
// Middleware to redirect to login page if not authenticated
app.Use(async (context, next) =>
{
    // N?u ng??i dùng không ???c xác th?c
    if (!context.User.Identity.IsAuthenticated)
    {
        // Cho phép truy c?p trang ch? và trang ??ng ký
        if (context.Request.Path.StartsWithSegments("/Account/Login") ||
            context.Request.Path.StartsWithSegments("/Account/Register") ||
            context.Request.Path.StartsWithSegments("/Home/HomePage"))
        {
            await next.Invoke();
            return;
        }
        else
        {
            // Chuy?n h??ng ng??i dùng không xác th?c t?i trang ??ng nh?p
            context.Response.Redirect("/Home/HomePage");
            return;
        }
    }
    else
    {
        // Cho phép truy c?p n?u ng??i dùng ?ã xác th?c
        await next.Invoke();
    }
});
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=HomePage}/{id?}");

app.Run();
