namespace GymManagement.Domain
{
    public class Member
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Gender { get; set; }
        public string? Package { get; set; }
        public DateTime SubscriptionDate { get; set; }
        public string? Status { get; set; }
    }
}