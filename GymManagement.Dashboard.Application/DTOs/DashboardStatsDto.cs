namespace GymManagement.Dashboard.Application.DTOs
{
    public class DashboardStatsDto
    {
        public int ActiveMembers { get; set; }
        public int ExpiredSubscriptions { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TodayAttendance { get; set; }
        public double ActiveMembersChangePercent { get; set; }
        public double ExpiredSubscriptionsChangePercent { get; set; }
        public double RevenueChangePercent { get; set; }
        public double AttendanceChangePercent { get; set; }
    }
}
