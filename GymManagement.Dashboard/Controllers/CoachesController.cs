using GymManagement.Application.Interfaces;
using GymManagement.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Dashboard.Controllers
{
    public class CoachesController : Controller
    {
        private readonly ICoachRepository _coachRepository;

        private readonly GymManagement.Dashboard.Services.SignalRNotificationService _signalRNotificationService;

        public CoachesController(ICoachRepository coachRepository, GymManagement.Dashboard.Services.SignalRNotificationService signalRNotificationService)
        {
            _coachRepository = coachRepository;
            _signalRNotificationService = signalRNotificationService;
        }

        // GET: /Coaches
        public async Task<IActionResult> Index()
        {
            var coaches = await _coachRepository.GetAllAsync();
            return View(coaches);
        }

        // GET: /Coaches/Create
        public IActionResult Create()
        {
            return View(new Coach { Status = "نشط" });
        }

        // POST: /Coaches/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Coach coach)
        {
            if (ModelState.IsValid)
            {
                await _coachRepository.AddAsync(coach);
                await _signalRNotificationService.NotifyTrainerAdded(coach);
                TempData["Success"] = "تم إضافة المدرب بنجاح";
                return RedirectToAction(nameof(Index));
            }
            return View(coach);
        }

        // GET: /Coaches/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var coach = await _coachRepository.GetByIdAsync(id);
            if (coach == null) return NotFound();
            return View(coach);
        }

        // POST: /Coaches/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Coach coach)
        {
            if (id != coach.CoachId) return BadRequest();
            if (ModelState.IsValid)
            {
                await _coachRepository.UpdateAsync(coach);
                await _signalRNotificationService.NotifyTrainerUpdated(coach);
                TempData["Success"] = "تم تحديث بيانات المدرب بنجاح";
                return RedirectToAction(nameof(Index));
            }
            return View(coach);
        }

        // POST: /Coaches/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var coach = await _coachRepository.GetByIdAsync(id);
            if (coach == null) return NotFound();
            await _coachRepository.DeleteAsync(id);
            await _signalRNotificationService.NotifyTrainerDeleted(id);
            TempData["Success"] = "تم حذف المدرب بنجاح";
            return RedirectToAction(nameof(Index));
        }
    }
}
