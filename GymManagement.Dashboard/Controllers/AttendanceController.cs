using GymManagement.Application.Interfaces;
using GymManagement.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Dashboard.Controllers
{
    public class AttendanceController : Controller
    {
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly IMemberRepository _memberRepository;

        public AttendanceController(IAttendanceRepository attendanceRepository, IMemberRepository memberRepository)
        {
            _attendanceRepository = attendanceRepository;
            _memberRepository = memberRepository;
        }

        // GET: /Attendance
        public async Task<IActionResult> Index()
        {
            var attendances = await _attendanceRepository.GetAllAsync();
            return View(attendances);
        }

        // GET: /Attendance/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Members = await _memberRepository.GetAllAsync();
            return View(new Attendance { Date = DateTime.Today, Status = "حضر" });
        }

        // POST: /Attendance/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Attendance attendance)
        {
            if (ModelState.IsValid)
            {
                var member = await _memberRepository.GetByIdAsync(attendance.MemberId);
                if (member != null) attendance.MemberName = member.Name;
                await _attendanceRepository.AddAsync(attendance);
                TempData["Success"] = "تم تسجيل الحضور بنجاح";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Members = await _memberRepository.GetAllAsync();
            return View(attendance);
        }

        // GET: /Attendance/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var attendance = await _attendanceRepository.GetByIdAsync(id);
            if (attendance == null) return NotFound();
            ViewBag.Members = await _memberRepository.GetAllAsync();
            return View(attendance);
        }

        // POST: /Attendance/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Attendance attendance)
        {
            if (id != attendance.AttendanceId) return BadRequest();
            if (ModelState.IsValid)
            {
                var member = await _memberRepository.GetByIdAsync(attendance.MemberId);
                if (member != null) attendance.MemberName = member.Name;
                await _attendanceRepository.UpdateAsync(attendance);
                TempData["Success"] = "تم تحديث سجل الحضور بنجاح";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Members = await _memberRepository.GetAllAsync();
            return View(attendance);
        }

        // POST: /Attendance/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var att = await _attendanceRepository.GetByIdAsync(id);
            if (att == null) return NotFound();
            await _attendanceRepository.DeleteAsync(id);
            TempData["Success"] = "تم حذف سجل الحضور بنجاح";
            return RedirectToAction(nameof(Index));
        }
    }
}
