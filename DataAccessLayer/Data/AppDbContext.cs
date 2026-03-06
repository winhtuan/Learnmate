using System.Reflection;
using BusinessObject.Models;
using BusinessObject.Models.Base;
using DataAccessLayer.Data.Seeding;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    // ── Identity ─────────────────────────────────────────────────────────────
    public DbSet<User> Users => Set<User>();
    public DbSet<TeacherProfile> TeacherProfiles => Set<TeacherProfile>();
    public DbSet<StudentProfile> StudentProfiles => Set<StudentProfile>();

    // ── Class Management ─────────────────────────────────────────────────────
    public DbSet<Class> Classes => Set<Class>();
    public DbSet<ClassMember> ClassMembers => Set<ClassMember>();
    public DbSet<Schedule> Schedules => Set<Schedule>();
    public DbSet<VideoSession> VideoSessions => Set<VideoSession>();
    public DbSet<Material> Materials => Set<Material>();

    // ── Assignment System ────────────────────────────────────────────────────
    public DbSet<Assignment> Assignments => Set<Assignment>();
    public DbSet<AssignmentQuestion> AssignmentQuestions => Set<AssignmentQuestion>();
    public DbSet<AssignmentOption> AssignmentOptions => Set<AssignmentOption>();
    public DbSet<Submission> Submissions => Set<Submission>();
    public DbSet<SubmissionAnswer> SubmissionAnswers => Set<SubmissionAnswer>();
    public DbSet<SubmissionAnswerOption> SubmissionAnswerOptions => Set<SubmissionAnswerOption>();
    public DbSet<Feedback> Feedbacks => Set<Feedback>();

    // ── Rating ───────────────────────────────────────────────────────────────
    public DbSet<TeacherRating> TeacherRatings => Set<TeacherRating>();

    // ── Finance ──────────────────────────────────────────────────────────────
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<Invoice> Invoices => Set<Invoice>();

    // ── System ───────────────────────────────────────────────────────────────
    public DbSet<Notification> Notifications => Set<Notification>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Auto-discover all IEntityTypeConfiguration<T> in this assembly
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        DataSeeder.Seed(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            if (entry.State == EntityState.Added)
                entry.Entity.CreatedAt = now;

            if (entry.State is EntityState.Added or EntityState.Modified)
                entry.Entity.UpdatedAt = now;
        }
        return await base.SaveChangesAsync(cancellationToken);
    }
}
