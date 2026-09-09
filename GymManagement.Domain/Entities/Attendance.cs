namespace GymManagement.Domain.Entities
{
    public class Attendance
    {
        public int AttendanceId { get; set; }
        public int MemberId { get; set; }
        public string MemberName { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string CheckInTime { get; set; } = string.Empty;
        public string CheckOutTime { get; set; } = string.Empty;
        public string Status { get; set; } = "حضر";
    }
}
