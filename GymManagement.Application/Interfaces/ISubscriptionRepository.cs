using GymManagement.Domain.Entities;

namespace GymManagement.Application.Interfaces
{
    public interface ISubscriptionRepository
    {
        Task<IEnumerable<Subscription>> GetAllAsync();
        Task<Subscription?> GetByIdAsync(int id);
        Task AddAsync(Subscription subscription);
        Task UpdateAsync(Subscription subscription);
        Task DeleteAsync(int id);
        Task<IEnumerable<Subscription>> SearchAsync(string query);
        Task<IEnumerable<Subscription>> GetExpiringSoonAsync(int days);
    }
}