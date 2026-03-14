namespace BusinessLogicLayer.DTOs.Class;

public record ClassVideosDto(
    VideoSessionItemDto? LiveSession,
    IReadOnlyList<VideoSessionItemDto> RecordedSessions
);

public record VideoSessionItemDto(
    long Id,
    long ScheduleId,
    string Title,
    string MeetingUrl,
    string Provider,
    string SessionStatus,
    DateTime StartTimeLocal,
    DateTime? EndTimeLocal
);
