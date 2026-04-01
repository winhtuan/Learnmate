using BusinessObject.Enum;

namespace BusinessLogicLayer.DTOs.Teacher.Schedules;

public class TeacherScheduleClassDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public ClassStatus Status { get; set; }
    public int EnrolledStudents { get; set; }
    public int? TotalSessions { get; set; }
}

public class TeacherScheduleDto
{
    public long Id { get; set; }
    public long ClassId { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public ScheduleType Type { get; set; }
    public ScheduleStatus Status { get; set; }
    public bool IsTrial { get; set; }
    public int AttendanceCount { get; set; }
    public int TotalStudents { get; set; }
}

public class CreateScheduleDto
{
    public long ClassId { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime StartTime { get; set; } = DateTime.Today.AddHours(9);
    public DateTime EndTime { get; set; } = DateTime.Today.AddHours(11);
    public ScheduleType Type { get; set; } = ScheduleType.REGULAR;
    public ScheduleStatus Status { get; set; } = ScheduleStatus.SCHEDULED;
    public bool IsTrial { get; set; }
}

public class UpdateScheduleDto
{
    public string Title { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public ScheduleType Type { get; set; }
    public ScheduleStatus Status { get; set; }
    public bool IsTrial { get; set; }
}

public class StudentAttendanceDto
{
    public long AttendanceId { get; set; } // 0 if not attended yet
    public long StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string StudentEmail { get; set; } = string.Empty;
    public bool IsPresent { get; set; }
    public string? Notes { get; set; }
}
