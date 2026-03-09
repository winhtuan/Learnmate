using BusinessLogicLayer.DTOs.Class;

namespace LearnmateSolution.Components.Pages.Class.Models;

/// <summary>Shared mapping helpers used across all Class sub-pages.</summary>
internal static class ClassPageHelpers
{
    private static readonly string[] IconBgs    = ["bg-rose-100", "bg-sky-100", "bg-emerald-100", "bg-purple-100"];
    private static readonly string[] IconColors = ["text-rose-600", "text-sky-600", "text-emerald-600", "text-purple-600"];

    public static string IconBg(int i)    => IconBgs[i    % IconBgs.Length];
    public static string IconColor(int i) => IconColors[i % IconColors.Length];

    public static AssignmentItem MapAssignment(ClassAssignmentDto a)
    {
        var (statusLabel, statusColor, statusBg) = a.SubmissionStatus switch
        {
            "graded"      => ($"Graded: {a.Score}", "text-emerald-700", "bg-emerald-100"),
            "submitted"   => ("Submitted",           "text-sky-700",     "bg-sky-100"),
            "in_progress" => ("In Progress",         "text-amber-700",   "bg-amber-100"),
            "missing"     => ("Missing",             "text-rose-700",    "bg-rose-100"),
            _             => ("Not Started",         "text-slate-600",   "bg-slate-100"),
        };
        var dueLabel = a.DueDateLocal is { } d ? FormatDue(d) : "No due date";

        return new AssignmentItem(
            Id:          a.Id,
            Title:       a.Title,
            Due:         dueLabel,
            Metadata:    "",
            Status:      statusLabel,
            StatusColor: statusColor,
            StatusBg:    statusBg,
            Icon:        "edit_note",
            IconBg:      "bg-rose-100",
            IconColor:   "text-rose-600"
        );
    }

    public static string FormatDue(DateTime local)
    {
        var diff = (local.Date - DateTime.Today).Days;
        return diff switch
        {
            0   => "Today",
            1   => "Tomorrow",
            < 0 => $"Was due {local:MMM d}",
            _   => $"In {diff} days"
        };
    }

    public static string FileIcon(string fileType) => fileType.ToLower() switch
    {
        "pdf"                    => "picture_as_pdf",
        "zip"                    => "folder_zip",
        "mp4" or "avi" or "mov" => "videocam",
        _                        => "description"
    };
}
