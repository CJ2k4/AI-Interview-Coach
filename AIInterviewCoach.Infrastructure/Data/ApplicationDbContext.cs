using AIInterviewCoach.Domain.Entities;
using AIInterviewCoach.Application.Interfaces;
using AIInterviewCoach.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AIInterviewCoach.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<JobRole> JobRoles { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<InterviewSession> InterviewSessions { get; set; }
    public DbSet<Answer> Answers { get; set; }
    public DbSet<Feedback> Feedbacks { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}
