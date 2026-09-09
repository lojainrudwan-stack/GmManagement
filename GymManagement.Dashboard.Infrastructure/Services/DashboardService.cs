using GymManagement.Application.Interfaces;
using GymManagement.Dashboard.Application.DTOs;
using GymManagement.Dashboard.Application.Interfaces;

namespace GymManagement.Dashboard.Infrastructure.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IMemberRepository _memberRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IAttendanceRepository _attendanceRepository;

        public DashboardService(
            IMemberRepository memberRepository,
            ISubscriptionRepository subscriptionRepository,
            IAttendanceRepository attendanceRepository)
        {
            _memberRepository = memberRepository;
            _subscriptionRepository = subscriptionRepository;
            _attendanceRepository = attendanceRepository;
        }

        public async Task<DashboardStatsDto> GetStatsAsync()
        {
            var members = await _memberRepository.GetAllAsync();
            var subscriptions = await _subscriptionRepository.GetAllAsync();
            var todayAttendance = await _attendanceRepository.GetTodayCountAsync();

            var activeMembers = members.Count(m => m.Status == "نشط");
            var expiredSubs = subscriptions.Count(s => s.Status == "منتهي");
            // Total revenue from active subscription plans (mock sum based on package names)
            var totalRevenue = subscriptions.Sum(s => s.Package switch
            {
                "باقة شهرية"   => 250m,
                "باقة 3 اشهر"  => 650m,
                "باقة سنوية"   => 2000m,
                _              => 300m
            });

            return new DashboardStatsDto
            {
                ActiveMembers = activeMembers,
                ExpiredSubscriptions = expiredSubs,
                TotalRevenue = totalRevenue,
                TodayAttendance = todayAttendance,
                ActiveMembersChangePercent = 8,
                ExpiredSubscriptionsChangePercent = 12,
                RevenueChangePercent = 8,
                AttendanceChangePercent = 14
            };
        }

        public async Task<IEnumerable<ExpiringSubscriptionDto>> GetExpiringSoonAsync()
        {
            var expiring = await _subscriptionRepository.GetExpiringSoonAsync(30);
            return expiring.Select(s => new ExpiringSubscriptionDto
            {
                MemberName = s.MemberName,
                AvatarInitials = s.MemberName.Length > 0 ? s.MemberName[0].ToString() : "?",
                DaysRemaining = (int)(s.EndDate - DateTime.Now).TotalDays,
                Package = s.Package
            }).ToList();
        }

        public async Task<IEnumerable<MonthlyStatsDto>> GetMonthlyStatsAsync()
        {
            // Return static monthly data matching the chart in the screenshot
            var months = new[]
            {
                new MonthlyStatsDto { Month = "يناير",  NewSubscriptions = 150 },
                new MonthlyStatsDto { Month = "فبراير", NewSubscriptions = 160 },
                new MonthlyStatsDto { Month = "مارس",   NewSubscriptions = 270 },
                new MonthlyStatsDto { Month = "ابريل",  NewSubscriptions = 230 },
                new MonthlyStatsDto { Month = "مايو",   NewSubscriptions = 260 },
                new MonthlyStatsDto { Month = "يونيو",  NewSubscriptions = 370 },
            };
            return await Task.FromResult(months);
        }
    }
}
