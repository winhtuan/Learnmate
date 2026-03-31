using Microsoft.AspNetCore.Mvc;
using BusinessObject.Models;
using BusinessObject.Enum;
using DataAccessLayer.Data;
using Microsoft.EntityFrameworkCore;

namespace LearnmateSolution.Controllers;

[ApiController]
[Route("api/test")]
public class TestSeedController(AppDbContext db) : ControllerBase
{
    [HttpGet("seed")]
    public async Task<IActionResult> Seed()
    {
        // 1. Create Teacher
        var teacherEmail = "teacher@learnmate.vn";
        var teacher = await db.Users.FirstOrDefaultAsync(u => u.Email == teacherEmail);
        if (teacher == null)
        {
            teacher = new User
            {
                Email = teacherEmail,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("123"),
                Role = UserRole.TEACHER,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            db.Users.Add(teacher);
            await db.SaveChangesAsync();

            db.TeacherProfiles.Add(new TeacherProfile
            {
                UserId = teacher.Id,
                FullName = "Teacher LearnMate",
                HourlyRate = 250000,
                Subjects = "Math, Science",
                Status = ComplianceStatus.APPROVED,
                Bio = "Experienced tutor for all levels.",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });
        }

        // 2. Create Student
        var studentEmail = "student@learnmate.vn";
        var student = await db.Users.FirstOrDefaultAsync(u => u.Email == studentEmail);
        if (student == null)
        {
            student = new User
            {
                Email = studentEmail,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("123"),
                Role = UserRole.STUDENT,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            db.Users.Add(student);
            await db.SaveChangesAsync();

            db.StudentProfiles.Add(new StudentProfile
            {
                UserId = student.Id,
                FullName = "Student LearnMate",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });
        }

        // 3. Create a Class for the teacher
        var testClass = await db.Classes.FirstOrDefaultAsync(c => c.TeacherId == teacher.Id && c.Name == "Lớp học thử nghiệm VNPay");
        if (testClass == null)
        {
            testClass = new Class
            {
                TeacherId = teacher.Id,
                Name = "Lớp học thử nghiệm VNPay",
                Description = "Lớp học dùng để test luồng thanh toán VNPay",
                Status = ClassStatus.ACTIVE,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            db.Classes.Add(testClass);
            await db.SaveChangesAsync();
        }

        // 4. Create a Booking Request with AWAITING_PAYMENT
        var existingBooking = await db.TutorBookingRequests.FirstOrDefaultAsync(b => b.StudentId == student.Id && b.Status == BookingRequestStatus.AWAITING_PAYMENT);
        if (existingBooking == null)
        {
            var booking = new TutorBookingRequest
            {
                StudentId = student.Id,
                TeacherId = teacher.Id,
                RequestedStartTime = DateTime.UtcNow.AddDays(1),
                RequestedEndTime = DateTime.UtcNow.AddDays(1).AddHours(2),
                Status = BookingRequestStatus.AWAITING_PAYMENT,
                ClassId = testClass.Id, // Student sẽ join class này sau khi thanh toán
                PaymentDeadline = DateTime.UtcNow.AddHours(24),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Note = "Test booking for VNPay payment flow"
            };
            db.TutorBookingRequests.Add(booking);
            await db.SaveChangesAsync();
        }

        return Ok(new { Message = "Seed data successful", Student = studentEmail, Teacher = teacherEmail, Password = "123" });
    }
}
