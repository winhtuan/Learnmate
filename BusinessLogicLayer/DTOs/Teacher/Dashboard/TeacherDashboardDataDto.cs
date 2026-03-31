using BusinessLogicLayer.DTOs.Teacher.Classes;

namespace BusinessLogicLayer.DTOs.Teacher.Dashboard;

public class TeacherDashboardDataDto
{
    // Tổng hợp các con số (Stats)
    public int ActiveClassesCount { get; set; }
    public int TotalStudentsCount { get; set; }
    public int PendingAssignmentsCount { get; set; }
    public decimal MonthlyEarnings { get; set; }
    
    // Lớp học sắp tới (Upcoming Sessions)
    public List<UpcomingSessionDto> UpcomingSessions { get; set; } = [];
    
    // Yêu cầu đăng ký mới (New Join Requests - nếu có tính năng này)
    public int NewJoinRequestsCount { get; set; }
    
    // Lớp học gần đây (Recent Classes)
    public List<TeacherClassListItemDto> RecentClasses { get; set; } = [];
}

public class UpcomingSessionDto
{
    public long ClassId { get; set; }
    public string ClassName { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Type { get; set; } = string.Empty; // ONLINE/OFFLINE
    public string? MeetingLink { get; set; }
}
