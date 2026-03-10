using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.DTOs.Student;
using BusinessLogicLayer.Services.Interfaces;
using DataAccessLayer.Repositories.Interfaces;

namespace BusinessLogicLayer.Services;

public class StudentDashboardService(
    IStudentProfileRepository studentProfileRepo,
    IClassMemberRepository classMemberRepo,
    INotificationRepository notificationRepo,
    IScheduleRepository scheduleRepo
) : IStudentDashboardService
{
    public async Task<ApiResponse<StudentDashboardDto>> GetDashboardAsync(
        long userId,
        CancellationToken ct = default
    )
    {
        // Compute current week bounds in UTC
        var today = DateOnly.FromDateTime(DateTime.Today);
        // Sunday → show upcoming Mon-Sun (next week); other days → show current Mon-Sun
        var daysFromMonday = today.DayOfWeek == DayOfWeek.Sunday ? -1 : (int)today.DayOfWeek - 1;
        var monday = today.AddDays(-daysFromMonday);
        var sunday = monday.AddDays(6);
        var weekStartUtc = monday.ToDateTime(TimeOnly.MinValue).ToUniversalTime();
        var weekEndUtc = sunday.ToDateTime(TimeOnly.MaxValue).ToUniversalTime();

        // Sequential — DbContext is not thread-safe; parallel queries on the same context throw.
        var profile = await studentProfileRepo.GetByUserIdAsync(userId);
        var members = await classMemberRepo.GetEnrolledWithClassAsync(userId, ct);
        var notifications = await notificationRepo.GetRecentForUserAsync(userId, limit: 10, ct);
        var schedules = await scheduleRepo.GetWeeklyForStudentAsync(
            userId,
            weekStartUtc,
            weekEndUtc,
            ct
        );

        var nowUtc = DateTime.UtcNow;
        var nextSchedule = schedules.FirstOrDefault(s => s.StartTime > nowUtc);

        var dto = new StudentDashboardDto(
            FullName: profile?.FullName ?? "Student",
            CurrentTerm: ComputeCurrentTerm(),
            Stats: new StudentStatsDto(
                ActiveClasses: members.Count,
                PendingTasks: 0, // TODO: COUNT submissions WHERE status = pending
                StudyStreakDays: profile?.StudyStreakDays ?? 0,
                NextSession: nextSchedule is null
                    ? null
                    : new NextSessionDto(
                        ClassName: nextSchedule.Class.Name,
                        Room: "",
                        StartTimeLocal: nextSchedule.StartTime.ToLocalTime()
                    )
            ),
            WeekSchedule: BuildWeekWithSchedules(monday, schedules),
            Classes: MapClasses(members),
            Notifications: MapNotifications(notifications)
        );

        return ApiResponse<StudentDashboardDto>.Ok(dto);
    }

    public async Task<ApiResponse<StudentProfileHeaderDto>> GetProfileHeaderAsync(
        long userId,
        CancellationToken ct = default
    )
    {
        var profile = await studentProfileRepo.GetByUserIdAsync(userId);

        return ApiResponse<StudentProfileHeaderDto>.Ok(
            new StudentProfileHeaderDto(
                FullName: profile?.FullName ?? "Student",
                AvatarUrl: profile?.User.AvatarUrl,
                GradeLevel: profile?.GradeLevel
            )
        );
    }

    // ── Mapping ───────────────────────────────────────────────────────────────

    private static IReadOnlyList<EnrolledClassDto> MapClasses(
        IReadOnlyList<BusinessObject.Models.ClassMember> members
    )
    {
        return members
            .Select(m =>
            {
                var nextAssignment = m.Class.Assignments.FirstOrDefault();

                return new EnrolledClassDto(
                    ClassId: m.ClassId,
                    ClassName: m.Class.Name,
                    SubjectColor: "", // no color column — UI uses index-based fallback
                    ProgressPercent: 0, // TODO: compute from submissions
                    NextTaskTitle: nextAssignment?.Title,
                    NextTaskDue: nextAssignment?.DueDate is DateTime d
                        ? DateOnly.FromDateTime(d)
                        : null
                );
            })
            .ToList();
    }

    private static IReadOnlyList<NotificationDto> MapNotifications(
        IReadOnlyList<BusinessObject.Models.Notification> notifications
    ) =>
        notifications
            .Select(n => new NotificationDto(
                Id: n.Id,
                Message: n.Content,
                CreatedAt: n.CreatedAt,
                IsRead: n.IsRead
            ))
            .ToList();

    // ── Helpers ───────────────────────────────────────────────────────────────

    private static IReadOnlyList<DayScheduleDto> BuildWeekWithSchedules(
        DateOnly monday,
        IReadOnlyList<BusinessObject.Models.Schedule> schedules
    )
    {
        // Pre-convert UTC→local once per schedule to avoid repeated ToLocalTime() calls
        var localSchedules = schedules
            .Select(s =>
                (
                    s.Class.Name,
                    LocalStart: s.StartTime.ToLocalTime(),
                    LocalEnd: s.EndTime.ToLocalTime()
                )
            )
            .ToList();

        var byDate = localSchedules
            .GroupBy(s => DateOnly.FromDateTime(s.LocalStart))
            .ToDictionary(g => g.Key, g => g.ToList());

        return Enumerable
            .Range(0, 7)
            .Select(i =>
            {
                var date = monday.AddDays(i);
                var day = byDate.TryGetValue(date, out var list) ? list : [];
                return new DayScheduleDto(
                    date,
                    day.Select(s => new ScheduledClassDto(
                            ClassName: s.Name,
                            Room: "",
                            StartTime: TimeOnly.FromDateTime(s.LocalStart),
                            EndTime: TimeOnly.FromDateTime(s.LocalEnd)
                        ))
                        .ToList()
                );
            })
            .Where(d => d.Classes.Count > 0)
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
            _ => $"Winter {today.Year - 1}–{today.Year}",
        };
    }
}
