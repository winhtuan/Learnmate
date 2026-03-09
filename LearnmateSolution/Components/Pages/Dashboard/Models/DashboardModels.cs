namespace LearnmateSolution.Components.Pages.Dashboard.Models;

public record NextSessionInfo(DateTime? StartTimeLocal, string ClassName, string Room);
public record WelcomeBannerStats(string ActiveClasses, string PendingTasks, string StudyStreak, NextSessionInfo NextSession);

public record ScheduleClass(string Time, string Name, string Room, string Color);
public record ScheduleDay(string Day, string Date, bool IsToday, List<ScheduleClass> Classes);

public record CourseItem(
    string Icon,
    string IconBg,
    string IconColor,
    string Title,
    int Progress,
    string ProgressColor,
    string NextTask,
    string? DueDate
);

public record QuickAction(string Icon, string Label);

public record NotificationItem(string Color, string Text, string Time);
