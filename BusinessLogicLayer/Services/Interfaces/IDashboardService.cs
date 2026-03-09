using BusinessLogicLayer.DTOs.Dashboard;

namespace BusinessLogicLayer.Services.Interfaces;

public interface IDashboardService
{
    Task<DashboardOverviewDto> GetDashboardOverviewAsync();
    Task<IEnumerable<RecentActivityDto>> GetRecentActivitiesAsync(int count = 10);
}
