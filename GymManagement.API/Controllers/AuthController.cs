using GymManagement.Application.Interfaces;
using GymManagement.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Presentation.Controllers
{
    /// <summary>
    /// Auth controller for Flutter login / register.
    /// Uses the existing Members table (IMemberRepository) --
    /// no new schema, no migrations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMemberRepository _memberRepository;

        public AuthController(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

        // POST /api/auth/login
        // Body: { "identifier": "phone_or_email", "password": "..." }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Identifier))
                return BadRequest(new { message = "identifier is required" });

            // Find member by phone (identifier)
            var members = await _memberRepository.SearchAsync(request.Identifier);
            var member  = members.FirstOrDefault(m =>
                m.Phone == request.Identifier ||
                m.Name  == request.Identifier);

            if (member == null)
                return Unauthorized(new { message = "المستخدم غير موجود" });

            return Ok(new
            {
                id    = member.MemberId,
                name  = member.Name,
                email = $"{member.Phone}@gym.local",
                phone = member.Phone,
                token = $"member-token-{member.MemberId}"
            });
        }

        // POST /api/auth/register
        // Body: { "name": "", "phone": "", "email": "", "password": "", "gender": "male|female" }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name) ||
                string.IsNullOrWhiteSpace(request.Phone))
                return BadRequest(new { message = "name and phone are required" });

            // Check if phone already registered
            var existing = await _memberRepository.SearchAsync(request.Phone);
            if (existing.Any(m => m.Phone == request.Phone))
                return Conflict(new { message = "الحساب مسجل مسبقا" });

            var member = new Member
            {
                Name             = request.Name,
                Phone            = request.Phone,
                Gender           = request.Gender ?? "male",
                Package          = string.Empty,
                SubscriptionDate = DateTime.UtcNow,
                Status           = "نشط"
            };

            await _memberRepository.AddAsync(member);

            return StatusCode(201, new
            {
                id    = member.MemberId,
                name  = member.Name,
                email = request.Email ?? $"{member.Phone}@gym.local",
                phone = member.Phone,
                token = $"member-token-{member.MemberId}"
            });
        }
    }

    public class LoginRequest
    {
        public string Identifier { get; set; } = string.Empty;
        public string Password   { get; set; } = string.Empty;
    }

    public class RegisterRequest
    {
        public string Name     { get; set; } = string.Empty;
        public string Phone    { get; set; } = string.Empty;
        public string? Email   { get; set; }
        public string Password { get; set; } = string.Empty;
        public string? Gender  { get; set; }
    }
}
