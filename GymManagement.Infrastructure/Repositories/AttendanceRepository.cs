using GymManagement.Application.Interfaces;
using GymManagement.Domain.Entities;
using GymManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Repositories
{
    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly AppDbContext _context;

        public AttendanceRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Attendance>> GetAllAsync()
        {
            return await _context.Attendances.ToListAsync();
        }

        public async Task<Attendance?> GetByIdAsync(int id)
        {
            return await _context.Attendances.FindAsync(id);
        }

        public async Task AddAsync(Attendance attendance)
        {
            await _context.Attendances.AddAsync(attendance);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Attendance attendance)
        {
            _context.Attendances.Update(attendance);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var att = await _context.Attendances.FindAsync(id);
            if (att != null)
            {
                _context.Attendances.Remove(att);
                await _context.SaveChangesAsync();
            }
        }

        // الدالة الإضافية المطلوبة لتتوافق مع الـ Interface تماماً
        public async Task<int> GetTodayCountAsync()
        {
            var today = DateTime.Today;
            return await _context.Attendances
                .Where(a => a.Date.Date == today)
                .CountAsync();
        }
    }
}