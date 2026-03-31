using BusinessLogicLayer.DTOs.Teacher.Dashboard;
using BusinessLogicLayer.DTOs.Teacher.Classes;
using BusinessLogicLayer.Services.Interfaces.Dashboard;
using BusinessObject.Enum;
using DataAccessLayer.Data;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogicLayer.Services.Dashboard;

public class TeacherDashboardService(AppDbContext db) : ITeacherDashboardService
{
    public async Task<TeacherDashboardDataDto> GetDashboardDataAsync(long teacherId, CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        // 1. Tổng lớp học đang hoạt động
        int activeClassesCount = await db.Classes
            .CountAsync(c => c.TeacherId == teacherId 
                        && c.Status == ClassStatus.ACTIVE 
                        && c.DeletedAt == null, ct);

        // 2. Tổng số học sinh trong các lớp của giáo viên này
        // Join Classes -> ClassMembers để lấy các học sinh không trùng lặp (hoặc đếm tổng enrollment tùy yêu cầu)
        int totalStudentsCount = await db.ClassMembers
            .CountAsync(m => m.Class.TeacherId == teacherId 
                        && m.Status == ClassMemberStatus.ACTIVE, ct);

        // 3. Bài tập chưa chấm (Pending Assignments - nếu có trạng thái chấm điểm)
        // Hiện tại giả định là Assignments -> Submissions chưa có score
        int pendingAssignmentsCount = await db.Submissions
            .CountAsync(s => s.Assignment.Class.TeacherId == teacherId 
                        && s.Score == null, ct);

        // 4. Thu nhập tháng này (Monthly Earnings)
        // Lấy HourlyRate từ TeacherProfile của giáo viên này
        var hourlyRate = await db.TeacherProfiles
            .Where(p => p.UserId == teacherId)
            .Select(p => p.HourlyRate)
            .FirstOrDefaultAsync(ct);

        // Giả lập thu nhập tháng này dựa trên HourlyRate và số lượng học sinh đang dạy
        decimal monthlyEarnings = hourlyRate * totalStudentsCount * 0.5m; 

        // 5. Lịch dạy sắp tới (Upcoming Sessions)
        var upcomingSessions = await db.Schedules
            .AsNoTracking()
            .Where(s => s.Class.TeacherId == teacherId 
                    && s.StartTime > now
                    && s.Class.DeletedAt == null)
            .OrderBy(s => s.StartTime)
            .Take(5)
            .Select(s => new UpcomingSessionDto
            {
                ClassId = s.ClassId,
                ClassName = s.Class.Name,
                Subject = s.Class.Subject,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                Type = s.Type.ToString(),
                MeetingLink = s.Class.MeetingLink
            })
            .ToListAsync(ct);

        // 6. Lớp học gần đây (Recent Classes)
        var recentClasses = await db.Classes
            .AsNoTracking()
            .Where(c => c.TeacherId == teacherId && c.DeletedAt == null)
            .OrderByDescending(c => c.CreatedAt)
            .Take(6)
            .Select(c => new TeacherClassListItemDto
            {
                Id = c.Id,
                Name = c.Name,
                Subject = c.Subject,
                Status = c.Status,
                StudentCount = c.ClassMembers.Count(m => m.Status == ClassMemberStatus.ACTIVE),
                MaxStudents = c.MaxStudents,
                ThumbnailUrl = c.ThumbnailUrl,
                CreatedAt = c.CreatedAt
            })
            .ToListAsync(ct);

        return new TeacherDashboardDataDto
        {
            ActiveClassesCount = activeClassesCount,
            TotalStudentsCount = totalStudentsCount,
            PendingAssignmentsCount = pendingAssignmentsCount,
            MonthlyEarnings = monthlyEarnings,
            UpcomingSessions = upcomingSessions,
            RecentClasses = recentClasses
        };
    }
}
