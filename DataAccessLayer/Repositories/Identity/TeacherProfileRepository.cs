using DataAccessLayer.Data;
using DataAccessLayer.Repositories.Interfaces;

// using BusinessObject.Models;
// using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories;

public class TeacherProfileRepository : ITeacherProfileRepository
{
    private readonly AppDbContext _context;

    public TeacherProfileRepository(AppDbContext context)
    {
        _context = context;
    }

    // Implement methods here
}
