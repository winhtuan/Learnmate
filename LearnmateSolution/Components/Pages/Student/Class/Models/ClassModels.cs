namespace LearnmateSolution.Components.Pages.Student.Class.Models;

public record ClassItem(
    long Id, string Code, string Title, string Professor, string Description,
    string? NextClass, string? Image, string? ButtonText,
    bool HasWarning = false, bool IsUpcoming = false);

public record ClassInfo(
    string Code, string Status, string Name, string Description, string BackgroundImage);

public record TabItem(string Label, string Path, string? Badge = null, bool Active = false);

public record TaskItem(
    int Id, string Title, string Due, string Status,
    string Icon, string IconBg, string IconColor, bool Locked = false);

public record AssignmentItem(
    long Id, string Title, string Due, string Metadata, string Status,
    string StatusColor, string StatusBg, string Icon, string IconBg, string IconColor,
    bool Locked = false, bool Highlighted = false, bool Completed = false,
    string DueLabel = "Due", string DueColor = "text-slate-900");

public record AssignmentResource(
    long Id, string Title, string Size, string Icon,
    string IconBg, string IconColor, string Href);

public record AssignmentInstructions(string Text, List<string> Items);

public record AssignmentDetail(
    long Id, long ClassId, string Title, string Type, string DueDate, string Points,
    string SubmissionStatus,
    AssignmentInstructions Instructions, List<AssignmentResource> Resources,
    string UploadText, string UploadSubtext,
    string? FeedbackComment = null,
    string? FeedbackScoreDisplay = null);

public record InstructorInfo(string Name, string Department, string Avatar);

public record ResourceItem(
    long Id, string Title, string Type, string Metadata,
    string Icon, string IconBg, string IconColor, string Href);

public record MaterialFolder(
    long Id, string Name, int FileCount, string Size,
    string Icon, string IconColor, string IconBg);

public record MaterialFile(
    long Id, string Name, string Type, string Date, string Size, string FileUrl,
    string Icon, string IconColor, string IconBg);

public record CalendarEvent(string Type, string? Time, string Title);

public record CalendarDay(
    int Day, bool IsPrevMonth = false, bool IsNextMonth = false,
    bool IsToday = false, List<CalendarEvent>? Events = null);

public record EventTypeItem(string Type, string Label, string Color);

public record LiveSessionHost(string Name, string Avatar);

public record LiveSession(
    string Status, string Date, string Title, string Description,
    LiveSessionHost Host, int StudentsWaiting, string MeetingUrl = "#");

public record VideoLecture(
    long Id, string Title, string Duration, string Date, string Unit,
    string MeetingUrl,
    string GradientFrom, string GradientTo, string AccentGradient);
