using GymManagement.Domain.Entities;

namespace GymManagement.Application.Interfaces
{
    public interface ICoachRepository
    {
        Task<IEnumerable<Coach>> GetAllAsync();
        Task<Coach?> GetByIdAsync(int id);
        Task AddAsync(Coach coach);
        Task UpdateAsync(Coach coach);
        Task DeleteAsync(int id);
    }
}