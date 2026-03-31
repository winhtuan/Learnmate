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
        // 1. Get/Create Teacher
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
        }

        var teacherProfile = await db.TeacherProfiles.FirstOrDefaultAsync(p => p.UserId == teacher.Id);
        if (teacherProfile == null)
        {
            teacherProfile = new TeacherProfile { UserId = teacher.Id };
            db.TeacherProfiles.Add(teacherProfile);
        }
        
        teacherProfile.FullName = "Nguyen Van A";
        teacherProfile.HourlyRate = 250000;
        teacherProfile.Subjects = "Math, Science";
        teacherProfile.Status = ComplianceStatus.APPROVED;
        teacherProfile.Bio = "Experienced tutor for all levels.";
        teacherProfile.CreatedAt = DateTime.UtcNow;
        teacherProfile.UpdatedAt = DateTime.UtcNow;
        
        await db.SaveChangesAsync();

        // 2. Get/Create Student
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
        }

        var studentProfile = await db.StudentProfiles.FirstOrDefaultAsync(p => p.UserId == student.Id);
        if (studentProfile == null)
        {
            db.StudentProfiles.Add(new StudentProfile
            {
                UserId = student.Id,
                FullName = "Nguyen Minh Tuan",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });
            await db.SaveChangesAsync();
        }

        // 3. Create a Class for the teacher (Fixing missing Subject)
        var testClass = await db.Classes.FirstOrDefaultAsync(c => c.TeacherId == teacher.Id && c.Name == "Lớp Toán Nâng Cao VNPay");
        if (testClass == null)
        {
            testClass = new Class
            {
                TeacherId = teacher.Id,
                Name = "Lớp Toán Nâng Cao VNPay",
                Description = "Lớp học dùng để test luồng thanh toán VNPay",
                Subject = "Math", // FIX: Added required field
                Status = ClassStatus.ACTIVE,
                MaxStudents = 1,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            db.Classes.Add(testClass);
            await db.SaveChangesAsync();
        }

        // 4. Create a Booking Request with AWAITING_PAYMENT
        // Clear old test bookings to avoid duplicates if re-running
        var oldBookings = await db.TutorBookingRequests
            .Where(b => b.StudentId == student.Id && b.TeacherId == teacher.Id && b.Status == BookingRequestStatus.AWAITING_PAYMENT)
            .ToListAsync();
        if (oldBookings.Any())
        {
             db.TutorBookingRequests.RemoveRange(oldBookings);
             await db.SaveChangesAsync();
        }

        var booking = new TutorBookingRequest
        {
            StudentId = student.Id,
            TeacherId = teacher.Id,
            RequestedStartTime = DateTime.UtcNow.AddDays(1),
            RequestedEndTime = DateTime.UtcNow.AddDays(1).AddHours(2),
            Status = BookingRequestStatus.AWAITING_PAYMENT,
            ClassId = testClass.Id, // Linking to the class we created
            PaymentDeadline = DateTime.UtcNow.AddHours(24),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Note = "Gói học Toán cấp tốc 2h - Thanh toán qua VNPay"
        };
        db.TutorBookingRequests.Add(booking);
        await db.SaveChangesAsync();

        return Ok(new { 
            Message = "Seed data successful", 
            Student = studentEmail, 
            Teacher = teacherEmail, 
            ClassId = testClass.Id,
            BookingId = booking.Id,
            Deadline = booking.PaymentDeadline
        });
    }
}
