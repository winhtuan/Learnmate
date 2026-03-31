using BusinessLogicLayer.Services.Interfaces;
using BusinessObject.Enum;
using BusinessObject.Models.System;
using DataAccessLayer.Repositories.Interfaces;

namespace BusinessLogicLayer.Services;

public class ReportService(IReportRepository reportRepo) : IReportService
{
    public async Task<IEnumerable<Report>> GetReportsAsync()
    {
        return await reportRepo.GetAllAsync();
    }

    public async Task<Report> GenerateReportAsync(string name, ReportCategory category, ReportFormat format)
    {
        var report = new Report
        {
            Name = name,
            Category = category,
            Format = format,
            Status = ReportStatus.Processing, // Initially processing
            RequestedOn = DateTime.UtcNow
        };

        await reportRepo.CreateAsync(report);

        // Simulate async processing (in a real app, this would be a background job)
        _ = Task.Run(async () =>
        {
            await Task.Delay(2000); // Simulate work
            report.Status = ReportStatus.Ready;
            report.FileUrl = $"/exports/{report.Name}.{(format == ReportFormat.CSV ? "csv" : "pdf")}";
            await reportRepo.UpdateAsync(report);
        });

        return report;
    }

    public async Task DeleteReportAsync(int id)
    {
        await reportRepo.DeleteAsync(id);
    }
}
