using DataAccessLayer.Data;
using DataAccessLayer.Repositories.Interfaces.Teacher.Courses;
// using BusinessObject.Models;
// using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories.Teacher.Courses;

public class TeacherCourseRepository : ITeacherCourseRepository
{
    private readonly AppDbContext _context;

    public TeacherCourseRepository(AppDbContext context)
    {
        _context = context;
    }

    // Implement methods here
}
