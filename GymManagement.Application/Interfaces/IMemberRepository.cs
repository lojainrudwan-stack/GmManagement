using GymManagement.Domain.Entities;

namespace GymManagement.Application.Interfaces
{
    public interface IMemberRepository
    {
        Task<IEnumerable<Member>> GetAllAsync();
        Task<Member?> GetByIdAsync(int id);
        Task AddAsync(Member member);
        Task UpdateAsync(Member member);
        Task DeleteAsync(int id);
        Task<IEnumerable<Member>> SearchAsync(string query);
    }
}