using BusinessLogicLayer.DTOs.Teacher.Schedules;

namespace BusinessLogicLayer.Services.Interfaces.Teacher.Schedules;

public interface IVoiceScheduleAnalyzer
{
    Task<CreateScheduleDto> ParseTranscriptAsync(string transcript);
}
