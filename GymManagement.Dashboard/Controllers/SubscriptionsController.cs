using GymManagement.Application.Interfaces;
using GymManagement.Dashboard.Application.Interfaces;
using GymManagement.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Dashboard.Controllers
{
    public class SubscriptionsController : Controller
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IMemberRepository _memberRepository;
        private readonly ISubscriptionPlanRepository _planRepository;
        private readonly GymManagement.Dashboard.Services.SignalRNotificationService _signalRNotificationService;

        public SubscriptionsController(
            ISubscriptionRepository subscriptionRepository,
            IMemberRepository memberRepository,
            ISubscriptionPlanRepository planRepository,
            GymManagement.Dashboard.Services.SignalRNotificationService signalRNotificationService)
        {
            _subscriptionRepository = subscriptionRepository;
            _signalRNotificationService = signalRNotificationService;
            _memberRepository = memberRepository;
            _planRepository = planRepository;
        }

        // GET: /Subscriptions
        public async Task<IActionResult> Index(string? search)
        {
            ViewBag.Search = search;

            // تم ففك التعليق لجلب البيانات من قاعدة البيانات بناءً على طلبك
            IEnumerable<Subscription> subscriptions;
            if (!string.IsNullOrWhiteSpace(search))
                subscriptions = await _subscriptionRepository.SearchAsync(search);
            else
                subscriptions = await _subscriptionRepository.GetAllAsync();

            return View(subscriptions);
        }

        // GET: /Subscriptions/Create
        public async Task<IActionResult> Create()
        {
            // فك التعليق لجلب الأعضاء والباقات
            ViewBag.Members = await _memberRepository.GetAllAsync();
            ViewBag.Plans = await _planRepository.GetAllAsync();

            return View(new Subscription { StartDate = DateTime.Today, EndDate = DateTime.Today.AddMonths(1), Status = "نشط" });
        }

        // POST: /Subscriptions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Subscription subscription)
        {
            if (ModelState.IsValid)
            {
                var member = await _memberRepository.GetByIdAsync(subscription.MemberId);
                if (member != null) subscription.MemberName = member.Name;

                await _subscriptionRepository.AddAsync(subscription);
                await _signalRNotificationService.NotifySubscriptionAdded(subscription);
                TempData["Success"] = "تم إضافة الاشتراك بنجاح";
                return RedirectToAction(nameof(Index));
            }
            return View(subscription);
        }

        // GET: /Subscriptions/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var subscription = await _subscriptionRepository.GetByIdAsync(id);
            if (subscription == null) return NotFound();

            ViewBag.Members = await _memberRepository.GetAllAsync();
            ViewBag.Plans = await _planRepository.GetAllAsync();
            return View(subscription);
        }

        // POST: /Subscriptions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Subscription subscription)
        {
            if (id != subscription.SubscriptionId) return BadRequest();

            if (ModelState.IsValid)
            {
                await _subscriptionRepository.UpdateAsync(subscription);
                await _signalRNotificationService.NotifySubscriptionUpdated(subscription);
                TempData["Success"] = "تم تحديث الاشتراك بنجاح";
                return RedirectToAction(nameof(Index));
            }
            return View(subscription);
        }

        // POST: /Subscriptions/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _subscriptionRepository.DeleteAsync(id);
            await _signalRNotificationService.NotifySubscriptionDeleted(id);
            TempData["Success"] = "تم حذف الاشتراك بنجاح";
            return RedirectToAction(nameof(Index));
        }
    }
}
