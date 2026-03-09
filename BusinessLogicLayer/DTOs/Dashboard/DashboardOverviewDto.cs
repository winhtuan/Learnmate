namespace BusinessLogicLayer.DTOs.Dashboard;

public record DashboardOverviewDto(
    int TotalUsers,
    double UserGrowthPercentage,
    int ActiveClasses,
    double ClassGrowthPercentage,
    double AverageAttendance,
    double AttendanceGrowthPercentage,
    double SystemHealth,
    string SystemHealthStatus
);
