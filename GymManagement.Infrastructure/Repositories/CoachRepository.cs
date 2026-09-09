using GymManagement.Application.Interfaces;
using GymManagement.Domain.Entities;
using GymManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Repositories
{
    public class CoachRepository : ICoachRepository
    {
        private readonly AppDbContext _context;

        public CoachRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Coach>> GetAllAsync()
        {
            return await _context.Coaches.ToListAsync();
        }

        public async Task<Coach?> GetByIdAsync(int id)
        {
            return await _context.Coaches.FindAsync(id);
        }

        public async Task AddAsync(Coach coach)
        {
            await _context.Coaches.AddAsync(coach);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Coach coach)
        {
            _context.Coaches.Update(coach);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var coach = await _context.Coaches.FindAsync(id);
            if (coach != null)
            {
                _context.Coaches.Remove(coach);
                await _context.SaveChangesAsync();
            }
        }
    }
}