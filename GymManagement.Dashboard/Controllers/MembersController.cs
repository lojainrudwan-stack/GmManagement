using GymManagement.Application.Interfaces;
using GymManagement.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Dashboard.Controllers
{
    public class MembersController : Controller
    {
        private readonly IMemberRepository _memberRepository;

        public MembersController(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

        // GET: /Members
        public async Task<IActionResult> Index(string? search)
        {
            ViewBag.Search = search;
            IEnumerable<Member> members;
            if (!string.IsNullOrWhiteSpace(search))
                members = await _memberRepository.SearchAsync(search);
            else
                members = await _memberRepository.GetAllAsync();
            return View(members);
        }

        // GET: /Members/Create
        public IActionResult Create()
        {
            return View(new Member { SubscriptionDate = DateTime.Today, Status = "نشط" });
        }

        // POST: /Members/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Member member)
        {
            if (ModelState.IsValid)
            {
                await _memberRepository.AddAsync(member);
                TempData["Success"] = "تم إضافة المشترك بنجاح";
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        // GET: /Members/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var member = await _memberRepository.GetByIdAsync(id);
            if (member == null) return NotFound();
            return View(member);
        }

        // POST: /Members/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Member member)
        {
            if (id != member.MemberId) return BadRequest();
            if (ModelState.IsValid)
            {
                await _memberRepository.UpdateAsync(member);
                TempData["Success"] = "تم تحديث بيانات المشترك بنجاح";
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        // POST: /Members/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var member = await _memberRepository.GetByIdAsync(id);
            if (member == null) return NotFound();
            await _memberRepository.DeleteAsync(id);
            TempData["Success"] = "تم حذف المشترك بنجاح";
            return RedirectToAction(nameof(Index));
        }
    }
}
