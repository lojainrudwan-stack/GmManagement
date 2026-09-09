using GymManagement.Application.Interfaces;
using GymManagement.Domain.Entities;
using GymManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Repositories
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly AppDbContext _context;

        public SubscriptionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Subscription>> GetAllAsync()
        {
            // تم إضافة AsNoTracking لتجنب قفل جدول قاعدة البيانات والتعليق
            return await _context.Subscriptions.AsNoTracking().ToListAsync();
        }

        public async Task<Subscription?> GetByIdAsync(int id)
        {
            return await _context.Subscriptions.FindAsync(id);
        }

        public async Task AddAsync(Subscription subscription)
        {
            await _context.Subscriptions.AddAsync(subscription);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Subscription subscription)
        {
            _context.Subscriptions.Update(subscription);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var sub = await _context.Subscriptions.FindAsync(id);
            if (sub != null)
            {
                _context.Subscriptions.Remove(sub);
                await _context.SaveChangesAsync();
            }
        }

        // الدوال الإضافية لتتوافق مع الـ Interface تماماً
        public async Task<IEnumerable<Subscription>> SearchAsync(string query)
        {
            return await _context.Subscriptions
                .AsNoTracking()
                .Where(s => s.MemberName.Contains(query) || s.Status.Contains(query))
                .ToListAsync();
        }

        public async Task<IEnumerable<Subscription>> GetExpiringSoonAsync(int days)
        {
            var targetDate = DateTime.Today.AddDays(days);
            return await _context.Subscriptions
                .AsNoTracking()
                .Where(s => s.EndDate <= targetDate && s.EndDate >= DateTime.Today)
                .ToListAsync();
        }
    }
}