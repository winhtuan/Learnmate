using DataAccessLayer.Data;
using DataAccessLayer.Repositories.Interfaces.Teacher.Profile;
// using BusinessObject.Models;
// using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories.Teacher.Profile;

public class TeacherProfileRepository : ITeacherProfileRepository
{
    private readonly AppDbContext _context;

    public TeacherProfileRepository(AppDbContext context)
    {
        _context = context;
    }

    // Implement methods here
}
