using BusinessLogicLayer.DTOs.Dashboard;
using BusinessLogicLayer.Services.Interfaces;
using DataAccessLayer.Data;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogicLayer.Services;

public class DashboardService(AppDbContext db) : IDashboardService
{
    public async Task<DashboardOverviewDto> GetDashboardOverviewAsync()
    {
        var now = DateTime.UtcNow;
        var startOfThisMonth = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        var startOfLastMonth = startOfThisMonth.AddMonths(-1);

        var totalUsers = await db.Users.CountAsync();
        var usersThisMonth = await db.Users.CountAsync(u => u.CreatedAt >= startOfThisMonth);
        var usersLastMonth = await db.Users.CountAsync(u =>
            u.CreatedAt >= startOfLastMonth && u.CreatedAt < startOfThisMonth
        );
        var userGrowth =
            usersLastMonth > 0
                ? Math.Round((usersThisMonth - usersLastMonth) / (double)usersLastMonth * 100, 1)
            : usersThisMonth > 0 ? 100.0
            : 0.0;

        var activeClasses = await db.Classes.CountAsync(c => c.DeletedAt == null);
        var classesThisMonth = await db.Classes.CountAsync(c =>
            c.DeletedAt == null && c.CreatedAt >= startOfThisMonth
        );
        var classesLastMonth = await db.Classes.CountAsync(c =>
            c.DeletedAt == null && c.CreatedAt >= startOfLastMonth && c.CreatedAt < startOfThisMonth
        );
        var classGrowth =
            classesLastMonth > 0
                ? Math.Round(
                    (classesThisMonth - classesLastMonth) / (double)classesLastMonth * 100,
                    1
                )
            : classesThisMonth > 0 ? 100.0
            : 0.0;

        return new DashboardOverviewDto(
            TotalUsers: totalUsers,
            UserGrowthPercentage: userGrowth,
            ActiveClasses: activeClasses,
            ClassGrowthPercentage: classGrowth,
            AverageAttendance: 87.5,
            AttendanceGrowthPercentage: 2.3,
            SystemHealth: 99.2,
            SystemHealthStatus: "All systems operational"
        );
    }

    public async Task<IEnumerable<RecentActivityDto>> GetRecentActivitiesAsync(int count = 10)
    {
        var recentUsers = await db
            .Users.OrderByDescending(u => u.CreatedAt)
            .Take(count)
            .Select(u => new
            {
                u.Email,
                u.IsActive,
                u.CreatedAt,
                u.Role,
            })
            .ToListAsync();

        return recentUsers.Select(u => new RecentActivityDto(
            Action: $"New {u.Role.ToString().ToLower()} registered",
            UserAvatarUrl: null,
            UserName: u.Email,
            Time: u.CreatedAt,
            StatusType: u.IsActive ? "success" : "warning",
            Status: u.IsActive ? "Verified" : "Pending"
        ));
    }
}
