using GymManagement.Dashboard.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Dashboard.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDashboardService _dashboardService;

        public HomeController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                // ›ﬂ «· ⁄·Ìﬁ ÊÃ·» «·»Ì«‰«  „⁄ Õ„«Ì… «·’›Õ… »‹ try-catch · Ã‰» √Ì  ⁄·Ìﬁ
                var stats = await _dashboardService.GetStatsAsync();
                var expiring = await _dashboardService.GetExpiringSoonAsync();
                var monthly = await _dashboardService.GetMonthlyStatsAsync();

                ViewBag.Stats = stats;
                ViewBag.Expiring = expiring.ToList();
                ViewBag.Monthly = monthly.ToList();
            }
            catch (Exception)
            {
                // ›Ì Õ«· Õ’· √Ì  √ŒÌ— √Ê Œÿ√ »ﬁ«⁄œ… «·»Ì«‰« °  › Õ «·’›Õ… »»Ì«‰«  ›«—€… »œ·« „‰ «· ⁄·Ìﬁ
                ViewBag.Stats = null;
                ViewBag.Expiring = new List<object>();
                ViewBag.Monthly = new List<object>();
            }

            return View();
        }
    }
}