using GymManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Dashboard.Controllers
{
    public class ReportsController : Controller
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly ISubscriptionPlanRepository _planRepository;

        public ReportsController(
            ISubscriptionRepository subscriptionRepository,
            ISubscriptionPlanRepository planRepository)
        {
            _subscriptionRepository = subscriptionRepository;
            _planRepository = planRepository;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                // جلب البيانات مع حماية لتجنب أي تعليق مفاجئ
                var subscriptions = (await _subscriptionRepository.GetAllAsync())?.ToList() ?? new List<Domain.Entities.Subscription>();
                var plans = (await _planRepository.GetAllAsync())?.ToList() ?? new List<Domain.Entities.SubscriptionPlan>();

                var totalSubs = subscriptions.Count;

                // Package distribution
                var packageGroups = subscriptions
                    .GroupBy(s => s.Package)
                    .Select(g => new { Package = g.Key, Count = g.Count(), Percent = totalSubs > 0 ? (int)Math.Round((double)g.Count() / totalSubs * 100) : 0 })
                    .ToList();

                ViewBag.TotalSubscriptions = totalSubs;
                ViewBag.PackageDistribution = packageGroups;
            }
            catch (Exception)
            {
                // لو حصلت مشكلة في قاعدة البيانات، نعرض قيم افتراضية بدل ما تفصل الصفحة
                ViewBag.TotalSubscriptions = 0;
                ViewBag.PackageDistribution = new List<object>();
            }

            // Monthly stats (افتراضية للعرض)
            var monthly = new[]
            {
                new { Month = "يناير",  Count = 63 },
                new { Month = "فبراير", Count = 72 },
                new { Month = "مارس",   Count = 83 },
                new { Month = "ابريل",  Count = 86 },
                new { Month = "مايو",   Count = 90 },
                new { Month = "يونيو",  Count = 104 },
            };
            ViewBag.MonthlyStats = monthly;

            return View();
        }
    }
}