namespace LearnmateSolution.Components.Pages.Student.FindTutor.Models;

public sealed record TutorListItem(
    long Id,
    string Name,
    string AvatarUrl,
    bool IsVerified,
    bool IsSuperTutor,
    double Rating,
    int ReviewCount,
    int StudentCount,
    string Bio,
    string[] Subjects,
    decimal HourlyRate,
    AvailabilityStatus Availability,
    string? AvailabilityLabel,
    string Location,
    string ResponseTime,
    int TotalLessons,
    string FullBio,
    string[] AvailableSlots
);

public enum AvailabilityStatus { AvailableToday, NextSlot, FullyBooked }
