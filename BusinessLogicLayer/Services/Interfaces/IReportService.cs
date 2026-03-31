using BusinessObject.Models.System;
using BusinessObject.Enum;

namespace BusinessLogicLayer.Services.Interfaces;

public interface IReportService
{
    Task<IEnumerable<Report>> GetReportsAsync();
    Task<Report> GenerateReportAsync(string name, ReportCategory category, ReportFormat format);
    Task DeleteReportAsync(int id);
}
