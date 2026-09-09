namespace GymManagement.Domain.Entities
{
    public class Member
    {
        public int MemberId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string Package { get; set; } = string.Empty;
        public DateTime SubscriptionDate { get; set; }
        public string Status { get; set; } = "نشط";
    }
}