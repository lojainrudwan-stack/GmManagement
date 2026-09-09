using GymManagement.Application.Interfaces;
using GymManagement.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly IMemberRepository _memberRepository;

        public MembersController(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string? search)
        {
            if (!string.IsNullOrWhiteSpace(search))
            {
                var results = await _memberRepository.SearchAsync(search);
                return Ok(results);
            }
            var members = await _memberRepository.GetAllAsync();
            return Ok(members);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var member = await _memberRepository.GetByIdAsync(id);
            if (member == null) return NotFound();
            return Ok(member);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Member member)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _memberRepository.AddAsync(member);
            return CreatedAtAction(nameof(GetById), new { id = member.MemberId }, member);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Member member)
        {
            if (id != member.MemberId) return BadRequest("ID mismatch");
            var existing = await _memberRepository.GetByIdAsync(id);
            if (existing == null) return NotFound();
            await _memberRepository.UpdateAsync(member);
            return Ok(member);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _memberRepository.GetByIdAsync(id);
            if (existing == null) return NotFound();
            await _memberRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}