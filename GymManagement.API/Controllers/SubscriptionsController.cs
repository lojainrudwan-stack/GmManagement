using GymManagement.Application.Interfaces;
using GymManagement.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using GymManagement.API.Hubs;

namespace GymManagement.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionsController : ControllerBase
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IHubContext<GymHub> _hubContext;

        public SubscriptionsController(ISubscriptionRepository subscriptionRepository, IHubContext<GymHub> hubContext)
        {
            _subscriptionRepository = subscriptionRepository;
            _hubContext = hubContext;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string? search)
        {
            if (!string.IsNullOrWhiteSpace(search))
            {
                var results = await _subscriptionRepository.SearchAsync(search);
                return Ok(results);
            }
            var subscriptions = await _subscriptionRepository.GetAllAsync();
            return Ok(subscriptions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var subscription = await _subscriptionRepository.GetByIdAsync(id);
            if (subscription == null) return NotFound();
            return Ok(subscription);
        }

        [HttpGet("expiring/{days}")]
        public async Task<IActionResult> GetExpiring(int days)
        {
            var subscriptions = await _subscriptionRepository.GetExpiringSoonAsync(days);
            return Ok(subscriptions);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Subscription subscription)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _subscriptionRepository.AddAsync(subscription);
            await _hubContext.Clients.All.SendAsync("SubscriptionAdded", subscription);
            return CreatedAtAction(nameof(GetById), new { id = subscription.SubscriptionId }, subscription);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Subscription subscription)
        {
            if (id != subscription.SubscriptionId) return BadRequest("ID mismatch");
            var existing = await _subscriptionRepository.GetByIdAsync(id);
            if (existing == null) return NotFound();
            await _subscriptionRepository.UpdateAsync(subscription);
            await _hubContext.Clients.All.SendAsync("SubscriptionUpdated", subscription);
            return Ok(subscription);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _subscriptionRepository.GetByIdAsync(id);
            if (existing == null) return NotFound();
            await _subscriptionRepository.DeleteAsync(id);
            await _hubContext.Clients.All.SendAsync("SubscriptionDeleted", id);
            return NoContent();
        }
    }
}