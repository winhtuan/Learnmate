using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.DTOs.Tutor;
using BusinessLogicLayer.Services.Interfaces;
using BusinessObject.Enum;
using BusinessObject.Models;
using DataAccessLayer.Repositories.Interfaces;

namespace BusinessLogicLayer.Services;

public class TutorService(
    ITeacherProfileRepository teacherProfileRepo,
    ITutorBookingRepository bookingRepo,
    ITeacherCourseRepository teacherCourseRepo
) : ITutorService
{
    public async Task<ApiResponse<IReadOnlyList<ClassListingDto>>> GetClassListingsAsync(
        string? subject, decimal? maxRate, CancellationToken ct = default)
    {
        var classes = await teacherCourseRepo.GetActiveClassListingsAsync(subject, maxRate, ct);

        var dtos = classes.Select(c =>
        {
            var teacher = c.Teacher.TeacherProfile;
            var initials = teacher?.FullName.Length >= 2
                ? teacher.FullName[..2].ToUpper()
                : teacher?.FullName.ToUpper() ?? "??";

            return new ClassListingDto(
                ClassId:          c.Id,
                ClassName:        c.Name,
                Description:      c.Description,
                Subject:          c.Subject,
                ScheduleSummary:  FormatScheduleSummary(c.Schedules),
                EnrolledCount:    c.ClassMembers.Count,
                MaxStudents:      c.MaxStudents,
                TeacherId:        c.TeacherId,
                TeacherName:      teacher?.FullName ?? c.Teacher.Email,
                TeacherAvatarUrl: teacher?.AvatarUrl ?? c.Teacher.AvatarUrl ?? $"https://placehold.co/200/e0e7ff/4f46e5?text={Uri.EscapeDataString(initials)}",
                Rating:           (double)(teacher?.RatingAvg ?? 0),
                ReviewCount:      teacher?.TotalRatingCount ?? 0,
                Subjects:         teacher?.Subjects ?? "",
                HourlyRate:       teacher?.HourlyRate ?? 0,
                Bio:              teacher?.Bio,
                Schedules:        c.Schedules
                                    .Where(s => s.Status != ScheduleStatus.CANCELLED)
                                    .Select(s => new ClassScheduleDto(s.StartTime.ToLocalTime(), s.EndTime.ToLocalTime()))
                                    .ToArray(),
                StartDate:        c.StartDate?.ToLocalTime(),
                EndDate:          c.EndDate?.ToLocalTime(),
                TotalSessions:    c.TotalSessions
            );
        }).ToList();

        return ApiResponse<IReadOnlyList<ClassListingDto>>.Ok(dtos);
    }

    private static string FormatScheduleSummary(IEnumerable<Schedule> schedules)
    {
        var active = schedules
            .Where(s => s.Status != ScheduleStatus.CANCELLED)
            .OrderBy(s => s.StartTime)
            .ToList();

        if (active.Count == 0) return "No schedule yet";

        var first = active.First();
        var startTime = first.StartTime.ToLocalTime();
        var endTime = first.EndTime.ToLocalTime();

        // Simple format: "Mon 18:00-20:00" or "Mon +2 more"
        var day = startTime.ToString("ddd");
        var timeRange = $"{startTime:HH:mm} – {endTime:HH:mm}";

        return active.Count > 1
            ? $"{day} {timeRange} (+{active.Count - 1} slots)"
            : $"{day} {timeRange}";
    }

    public async Task<ApiResponse<IReadOnlyList<TutorSummaryDto>>> GetTutorsAsync(
        string? subject, decimal? maxRate, CancellationToken ct = default)
    {
        var profiles = await teacherProfileRepo.GetAllTeachersAsync(subject, maxRate, ct);

        var dtos = profiles.Select(p => new TutorSummaryDto(
            TeacherUserId:    p.UserId,
            FullName:         p.FullName,
            AvatarUrl:        p.AvatarUrl ?? p.User.AvatarUrl,
            Subjects:         p.Subjects,
            HourlyRate:       p.HourlyRate,
            RatingAvg:        (double)p.RatingAvg,
            TotalRatingCount: p.TotalRatingCount,
            Bio:              p.Bio
        )).ToList();

        return ApiResponse<IReadOnlyList<TutorSummaryDto>>.Ok(dtos);
    }

    public async Task<ApiResponse<BookingRequestDto>> CreateBookingRequestAsync(
        long studentId, CreateBookingRequestDto dto, CancellationToken ct = default)
    {
        if (dto.RequestedStartTime >= dto.RequestedEndTime)
            return ApiResponse<BookingRequestDto>.Fail("Start time must be before end time.");

        if (dto.RequestedStartTime <= DateTime.UtcNow)
            return ApiResponse<BookingRequestDto>.Fail("Booking time must be in the future.");

        var request = new TutorBookingRequest
        {
            StudentId            = studentId,
            TeacherId            = dto.TeacherId,
            RequestedStartTime   = dto.RequestedStartTime.ToUniversalTime(),
            RequestedEndTime     = dto.RequestedEndTime.ToUniversalTime(),
            Note                 = dto.Note,
            Status               = BookingRequestStatus.PENDING,
            CreatedAt            = DateTime.UtcNow,
            UpdatedAt            = DateTime.UtcNow,
        };

        var created = await bookingRepo.CreateAsync(request, ct);

        // Reload with navigation properties
        var full = await bookingRepo.GetByIdAsync(created.Id, ct);
        return ApiResponse<BookingRequestDto>.Ok(MapToDto(full!));
    }

    public async Task<ApiResponse<IReadOnlyList<BookingRequestDto>>> GetStudentBookingsAsync(
        long studentId, CancellationToken ct = default)
    {
        var requests = await bookingRepo.GetByStudentIdAsync(studentId, ct);
        var dtos = requests.Select(MapToDto).ToList();
        return ApiResponse<IReadOnlyList<BookingRequestDto>>.Ok(dtos);
    }

    public async Task<ApiResponse<bool>> CancelBookingAsync(
        long bookingId, long studentId, CancellationToken ct = default)
    {
        var request = await bookingRepo.GetByIdAsync(bookingId, ct);

        if (request is null)
            return ApiResponse<bool>.Fail("Booking not found.");

        if (request.StudentId != studentId)
            return ApiResponse<bool>.Fail("You are not authorized to cancel this booking.");

        if (request.Status != BookingRequestStatus.PENDING)
            return ApiResponse<bool>.Fail("Only pending bookings can be cancelled.");

        request.Status    = BookingRequestStatus.CANCELLED;
        request.UpdatedAt = DateTime.UtcNow;

        await bookingRepo.UpdateAsync(request, ct);
        return ApiResponse<bool>.Ok(true);
    }

    private static BookingRequestDto MapToDto(TutorBookingRequest r) => new(
        Id:                      r.Id,
        TeacherId:               r.TeacherId,
        TeacherName:             r.Teacher.TeacherProfile?.FullName ?? r.Teacher.Email,
        TeacherAvatarUrl:        r.Teacher.TeacherProfile?.AvatarUrl ?? r.Teacher.AvatarUrl,
        RequestedStartTimeLocal: r.RequestedStartTime.ToLocalTime(),
        RequestedEndTimeLocal:   r.RequestedEndTime.ToLocalTime(),
        Status:                  r.Status.ToString(),
        Note:                    r.Note,
        CreatedAtLocal:          r.CreatedAt.ToLocalTime()
    );
}
