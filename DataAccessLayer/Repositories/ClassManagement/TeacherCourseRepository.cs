using DataAccessLayer.Data;
using DataAccessLayer.Repositories.Interfaces;

// using BusinessObject.Models;
// using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories;

public class TeacherCourseRepository : ITeacherCourseRepository
{
    private readonly AppDbContext _context;

    public TeacherCourseRepository(AppDbContext context)
    {
        _context = context;
    }

    // Implement methods here
}
