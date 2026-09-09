using GymManagement.Application.Interfaces;
using GymManagement.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Dashboard.Controllers
{
    public class PackagesController : Controller
    {
        private readonly ISubscriptionPlanRepository _planRepository;

        public PackagesController(ISubscriptionPlanRepository planRepository)
        {
            _planRepository = planRepository;
        }

        // GET: /Packages
        public async Task<IActionResult> Index()
        {
            var plans = await _planRepository.GetAllAsync();
            return View(plans);
        }

        // GET: /Packages/Create
        public IActionResult Create()
        {
            return View(new SubscriptionPlan { Status = "نشط" });
        }

        // POST: /Packages/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SubscriptionPlan plan)
        {
            if (ModelState.IsValid)
            {
                await _planRepository.AddAsync(plan);
                TempData["Success"] = "تم إضافة الباقة بنجاح";
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }

        // GET: /Packages/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var plan = await _planRepository.GetByIdAsync(id);
            if (plan == null) return NotFound();
            return View(plan);
        }

        // POST: /Packages/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SubscriptionPlan plan)
        {
            if (id != plan.PlanId) return BadRequest();
            if (ModelState.IsValid)
            {
                await _planRepository.UpdateAsync(plan);
                TempData["Success"] = "تم تحديث الباقة بنجاح";
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }

        // POST: /Packages/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var plan = await _planRepository.GetByIdAsync(id);
            if (plan == null) return NotFound();
            await _planRepository.DeleteAsync(id);
            TempData["Success"] = "تم حذف الباقة بنجاح";
            return RedirectToAction(nameof(Index));
        }
    }
}
