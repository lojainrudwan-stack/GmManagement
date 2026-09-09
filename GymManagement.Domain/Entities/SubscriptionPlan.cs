using System.ComponentModel.DataAnnotations;

namespace GymManagement.Domain.Entities
{
    public class SubscriptionPlan
    {
        [Key]
        public int PlanId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int DurationDays { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; } = "نشط";
    }
}