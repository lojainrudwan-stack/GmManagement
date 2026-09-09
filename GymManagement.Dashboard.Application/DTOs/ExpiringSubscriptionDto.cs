namespace GymManagement.Dashboard.Application.DTOs
{
    public class ExpiringSubscriptionDto
    {
        public string MemberName { get; set; } = string.Empty;
        public string AvatarInitials { get; set; } = string.Empty;
        public int DaysRemaining { get; set; }
        public string Package { get; set; } = string.Empty;
    }
}
