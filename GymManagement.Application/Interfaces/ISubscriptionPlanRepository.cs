using GymManagement.Domain.Entities;

namespace GymManagement.Application.Interfaces
{
    public interface ISubscriptionPlanRepository
    {
        Task<IEnumerable<SubscriptionPlan>> GetAllAsync();
        Task<SubscriptionPlan?> GetByIdAsync(int id);
        Task AddAsync(SubscriptionPlan plan);
        Task UpdateAsync(SubscriptionPlan plan);
        Task DeleteAsync(int id);
    }
}