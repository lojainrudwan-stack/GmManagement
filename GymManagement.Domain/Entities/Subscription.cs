namespace GymManagement.Domain.Entities
{
    public class Subscription
    {
        public int SubscriptionId { get; set; }
        public string SubscriptionNumber { get; set; } = string.Empty;
        public int MemberId { get; set; }
        public string MemberName { get; set; } = string.Empty;
        public string Package { get; set; } = string.Empty;
        public string? CoachName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = "نشط";
    }
}