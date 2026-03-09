namespace BusinessLogicLayer.DTOs.Dashboard;

public record RecentActivityDto(
    string Action,
    string? UserAvatarUrl,
    string UserName,
    DateTime Time,
    string StatusType,  // Bootstrap color: "success" | "warning" | "danger"
    string Status       // Display label
);
