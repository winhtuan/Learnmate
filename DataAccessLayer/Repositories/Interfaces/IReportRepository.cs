using BusinessObject.Models.System;

namespace DataAccessLayer.Repositories.Interfaces;

public interface IReportRepository
{
    Task<IEnumerable<Report>> GetAllAsync();
    Task<Report?> GetByIdAsync(int id);
    Task<Report> CreateAsync(Report report);
    Task UpdateAsync(Report report);
    Task DeleteAsync(int id);
}
