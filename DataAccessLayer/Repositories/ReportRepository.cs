using BusinessObject.Models.System;
using DataAccessLayer.Data;
using DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories;

public class ReportRepository(AppDbContext context) : IReportRepository
{
    public async Task<IEnumerable<Report>> GetAllAsync()
    {
        return await context.Reports
            .OrderByDescending(r => r.RequestedOn)
            .ToListAsync();
    }

    public async Task<Report?> GetByIdAsync(int id)
    {
        return await context.Reports.FindAsync(id);
    }

    public async Task<Report> CreateAsync(Report report)
    {
        await context.Reports.AddAsync(report);
        await context.SaveChangesAsync();
        return report;
    }

    public async Task UpdateAsync(Report report)
    {
        context.Reports.Update(report);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var report = await context.Reports.FindAsync(id);
        if (report != null)
        {
            context.Reports.Remove(report);
            await context.SaveChangesAsync();
        }
    }
}
