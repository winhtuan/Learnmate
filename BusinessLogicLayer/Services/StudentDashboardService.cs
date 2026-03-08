using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.DTOs.Student;
using BusinessLogicLayer.Services.Interfaces;
using DataAccessLayer.Repositories.Interfaces;

namespace BusinessLogicLayer.Services;

public class StudentDashboardService(
    IStudentProfileRepository studentProfileRepo) : IStudentDashboardService
{
    public async Task<ApiResponse<StudentDashboardDto>> GetDashboardAsync(
        long userId, CancellationToken ct = default)
    {
        var profile = await studentProfileRepo.GetByUserIdAsync(userId);

        var dto = new StudentDashboardDto(
            FullName:    profile?.FullName ?? "Student",
            CurrentTerm: ComputeCurrentTerm(),
            Stats: new StudentStatsDto(
                ActiveClasses:  0,                            // TODO: COUNT class_members WHERE user_id = userId AND status = active
                PendingTasks:   0,                            // TODO: COUNT submissions WHERE user_id = userId AND status = pending
                StudyStreakDays: profile?.StudyStreakDays ?? 0,
                NextSession:    null                          // TODO: next schedule entry for this student
            ),
            WeekSchedule: BuildCurrentWeek(), // computed from real date, no DB needed
            Classes:       [],              // TODO: query enrolled classes via class_members
            Notifications: []               // TODO: query notifications WHERE user_id = userId
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

    // ── Helpers ──────────────────────────────────────────────────────────────

    /// <summary>Returns 5 working days (Mon–Fri) of the current week.</summary>
    private static IReadOnlyList<DayScheduleDto> BuildCurrentWeek()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        // Monday of current week (Sunday = 0 in DayOfWeek, treated as day 7)
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
