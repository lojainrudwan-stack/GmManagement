using GymManagement.Application.Interfaces;
using GymManagement.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using GymManagement.API.Hubs;

namespace GymManagement.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionPlansController : ControllerBase
    {
        private readonly ISubscriptionPlanRepository _subscriptionPlanRepository;
        private readonly IHubContext<GymHub> _hubContext;

        public SubscriptionPlansController(ISubscriptionPlanRepository subscriptionPlanRepository, IHubContext<GymHub> hubContext)
        {
            _subscriptionPlanRepository = subscriptionPlanRepository;
            _hubContext = hubContext;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var plans = await _subscriptionPlanRepository.GetAllAsync();
            return Ok(plans);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var plan = await _subscriptionPlanRepository.GetByIdAsync(id);
            if (plan == null) return NotFound();
            return Ok(plan);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SubscriptionPlan subscriptionPlan)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _subscriptionPlanRepository.AddAsync(subscriptionPlan);
            await _hubContext.Clients.All.SendAsync("PackageAdded", subscriptionPlan);
            return CreatedAtAction(nameof(GetById), new { id = subscriptionPlan.PlanId }, subscriptionPlan);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] SubscriptionPlan subscriptionPlan)
        {
            if (id != subscriptionPlan.PlanId) return BadRequest("ID mismatch");
            var existing = await _subscriptionPlanRepository.GetByIdAsync(id);
            if (existing == null) return NotFound();
            await _subscriptionPlanRepository.UpdateAsync(subscriptionPlan);
            await _hubContext.Clients.All.SendAsync("PackageUpdated", subscriptionPlan);
            return Ok(subscriptionPlan);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _subscriptionPlanRepository.GetByIdAsync(id);
            if (existing == null) return NotFound();
            await _subscriptionPlanRepository.DeleteAsync(id);
            await _hubContext.Clients.All.SendAsync("PackageDeleted", id);
            return NoContent();
        }
    }
}