namespace GymManagement.Domain.Entities
{
    public class Coach
    {
        public int CoachId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Status { get; set; } = "نشط";
    }
}