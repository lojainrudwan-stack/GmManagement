using GymManagement.Application.Interfaces;
using GymManagement.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using GymManagement.API.Hubs;

namespace GymManagement.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoachesController : ControllerBase
    {
        private readonly ICoachRepository _coachRepository;
        private readonly IHubContext<GymHub> _hubContext;

        public CoachesController(ICoachRepository coachRepository, IHubContext<GymHub> hubContext)
        {
            _coachRepository = coachRepository;
            _hubContext = hubContext;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var coaches = await _coachRepository.GetAllAsync();
            return Ok(coaches);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var coach = await _coachRepository.GetByIdAsync(id);
            if (coach == null) return NotFound();
            return Ok(coach);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Coach coach)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _coachRepository.AddAsync(coach);
            await _hubContext.Clients.All.SendAsync("TrainerAdded", coach);
            return CreatedAtAction(nameof(GetById), new { id = coach.CoachId }, coach);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Coach coach)
        {
            if (id != coach.CoachId) return BadRequest("ID mismatch");
            var existing = await _coachRepository.GetByIdAsync(id);
            if (existing == null) return NotFound();
            await _coachRepository.UpdateAsync(coach);
            await _hubContext.Clients.All.SendAsync("TrainerUpdated", coach);
            return Ok(coach);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _coachRepository.GetByIdAsync(id);
            if (existing == null) return NotFound();
            await _coachRepository.DeleteAsync(id);
            await _hubContext.Clients.All.SendAsync("TrainerDeleted", id);
            return NoContent();
        }
    }
}