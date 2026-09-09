using GymManagement.Application.Interfaces;
using GymManagement.Dashboard.Application.Interfaces;
using GymManagement.Dashboard.Infrastructure.Services;
using GymManagement.Infrastructure.Data;
using GymManagement.Infrastructure.Repositories; // ���� �� ��� ��� namespace ����� ���� ��� Repositories ����
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add MVC with views
builder.Services.AddControllersWithViews();

// Register DbContext for Database connection
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register Repositories required by DashboardService and Controllers
builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddScoped<IAttendanceRepository, AttendanceRepository>();
builder.Services.AddScoped<ISubscriptionPlanRepository, SubscriptionPlanRepository>();
builder.Services.AddScoped<ICoachRepository, CoachRepository>(); // <-- �� ����� ��� ����� ��� ����� ���� ��������

// Register dashboard-specific services
builder.Services.AddSingleton<GymManagement.Dashboard.Services.SignalRNotificationService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
