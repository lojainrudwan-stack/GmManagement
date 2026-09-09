using GymManagement.Dashboard.Application.DTOs;

namespace GymManagement.Dashboard.Application.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardStatsDto> GetStatsAsync();
        Task<IEnumerable<ExpiringSubscriptionDto>> GetExpiringSoonAsync();
        Task<IEnumerable<MonthlyStatsDto>> GetMonthlyStatsAsync();
    }
}
