using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.DTOs.Student;
using BusinessLogicLayer.Services.Interfaces;
using DataAccessLayer.Repositories.Interfaces;

namespace BusinessLogicLayer.Services;

public class StudentDashboardService(
    IStudentProfileRepository studentProfileRepo,
    IClassMemberRepository    classMemberRepo,
    INotificationRepository   notificationRepo) : IStudentDashboardService
{
    public async Task<ApiResponse<StudentDashboardDto>> GetDashboardAsync(
        long userId, CancellationToken ct = default)
    {
        // Sequential — DbContext is not thread-safe; parallel queries on the same context throw.
        var profile       = await studentProfileRepo.GetByUserIdAsync(userId);
        var members       = await classMemberRepo.GetEnrolledWithClassAsync(userId, ct);
        var notifications = await notificationRepo.GetRecentForUserAsync(userId, limit: 10, ct);

        var dto = new StudentDashboardDto(
            FullName:    profile?.FullName ?? "Student",
            CurrentTerm: ComputeCurrentTerm(),
            Stats: new StudentStatsDto(
                ActiveClasses:   members.Count,
                PendingTasks:    0,           // TODO: COUNT submissions WHERE status = pending
                StudyStreakDays: profile?.StudyStreakDays ?? 0,
                NextSession:     null         // TODO: next schedule entry for this student
            ),
            WeekSchedule:  BuildCurrentWeek(),
            Classes:       MapClasses(members),
            Notifications: MapNotifications(notifications)
        );

        return ApiResponse<StudentDashboardDto>.Ok(dto);
    }

    public async Task<ApiResponse<StudentProfileHeaderDto>> GetProfileHeaderAsync(
        long userId, CancellationToken ct = default)
    {
        var profile = await studentProfileRepo.GetByUserIdAsync(userId);

        return ApiResponse<StudentProfileHeaderDto>.Ok(new StudentProfileHeaderDto(
            FullName:   profile?.FullName   ?? "Student",
            AvatarUrl:  profile?.AvatarUrl,
            GradeLevel: profile?.GradeLevel
        ));
    }

    // ── Mapping ───────────────────────────────────────────────────────────────

    private static IReadOnlyList<EnrolledClassDto> MapClasses(
        IReadOnlyList<BusinessObject.Models.ClassMember> members)
    {
        return members.Select(m =>
        {
            // Repo đã .OrderBy().Take(1) — lấy phần tử đầu trực tiếp
            var nextAssignment = m.Class.Assignments.FirstOrDefault();

            return new EnrolledClassDto(
                ClassId:       m.ClassId,
                ClassName:     m.Class.Name,
                SubjectColor:  "",   // no color column — UI uses index-based fallback
                ProgressPercent: 0,  // TODO: compute from submissions
                NextTaskTitle: nextAssignment?.Title,
                NextTaskDue:   nextAssignment?.DueDate is DateTime d
                               ? DateOnly.FromDateTime(d)
                               : null
            );
        }).ToList();
    }

    private static IReadOnlyList<NotificationDto> MapNotifications(
        IReadOnlyList<BusinessObject.Models.Notification> notifications) =>
        notifications.Select(n => new NotificationDto(
            Id:        n.Id,
            Message:   n.Content,
            CreatedAt: n.CreatedAt,
            IsRead:    n.IsRead
        )).ToList();

    // ── Helpers ───────────────────────────────────────────────────────────────

    private static IReadOnlyList<DayScheduleDto> BuildCurrentWeek()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var daysFromMonday = today.DayOfWeek == DayOfWeek.Sunday ? 6 : (int)today.DayOfWeek - 1;
        var monday = today.AddDays(-daysFromMonday);

        return Enumerable.Range(0, 5)
            .Select(i => new DayScheduleDto(monday.AddDays(i), []))
            .ToArray();
    }

    private static string ComputeCurrentTerm()
    {
        var today = DateTime.Today;
        return today.Month switch
        {
            >= 9 => $"Fall {today.Year}–{today.Year + 1}",
            >= 6 => $"Summer {today.Year}",
            >= 2 => $"Spring {today.Year}",
            _    => $"Winter {today.Year - 1}–{today.Year}"
        };
    }
}
