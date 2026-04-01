using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.DTOs.Tutor;

namespace BusinessLogicLayer.Services.Interfaces;

public interface ITutorService
{
    Task<ApiResponse<IReadOnlyList<TutorSummaryDto>>> GetTutorsAsync(
        string? subject, decimal? maxRate, CancellationToken ct = default);

    Task<ApiResponse<BookingRequestDto>> CreateBookingRequestAsync(
        long studentId, CreateBookingRequestDto dto, CancellationToken ct = default);

    Task<ApiResponse<IReadOnlyList<BookingRequestDto>>> GetStudentBookingsAsync(
        long studentId, CancellationToken ct = default);

    Task<ApiResponse<bool>> CancelBookingAsync(
        long bookingId, long studentId, CancellationToken ct = default);

    Task<ApiResponse<IReadOnlyList<ClassListingDto>>> GetClassListingsAsync(
        string? subject, decimal? maxRate, CancellationToken ct = default);
}
