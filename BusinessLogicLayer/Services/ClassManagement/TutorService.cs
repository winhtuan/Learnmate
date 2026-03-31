using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.DTOs.Tutor;
using BusinessLogicLayer.Services.Interfaces;
using BusinessObject.Enum;
using BusinessObject.Models;
using DataAccessLayer.Repositories.Interfaces;

namespace BusinessLogicLayer.Services;

public class TutorService(
    ITeacherProfileRepository teacherProfileRepo,
    ITutorBookingRepository bookingRepo
) : ITutorService
{
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
