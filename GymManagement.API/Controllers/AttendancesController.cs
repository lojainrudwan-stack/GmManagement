using GymManagement.Application.Interfaces;
using GymManagement.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendancesController : ControllerBase
    {
        private readonly IAttendanceRepository _attendanceRepository;

        public AttendancesController(IAttendanceRepository attendanceRepository)
        {
            _attendanceRepository = attendanceRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var attendances = await _attendanceRepository.GetAllAsync();
            return Ok(attendances);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var attendance = await _attendanceRepository.GetByIdAsync(id);
            if (attendance == null) return NotFound();
            return Ok(attendance);
        }

        [HttpGet("today/count")]
        public async Task<IActionResult> GetTodayCount()
        {
            var count = await _attendanceRepository.GetTodayCountAsync();
            return Ok(new { count });
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Attendance attendance)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _attendanceRepository.AddAsync(attendance);
            return CreatedAtAction(nameof(GetById), new { id = attendance.AttendanceId }, attendance);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Attendance attendance)
        {
            if (id != attendance.AttendanceId) return BadRequest("ID mismatch");
            var existing = await _attendanceRepository.GetByIdAsync(id);
            if (existing == null) return NotFound();
            await _attendanceRepository.UpdateAsync(attendance);
            return Ok(attendance);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _attendanceRepository.GetByIdAsync(id);
            if (existing == null) return NotFound();
            await _attendanceRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
