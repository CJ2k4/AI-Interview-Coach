using AIInterviewCoach.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AIInterviewCoach.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<JobRole> JobRoles { get; }
    DbSet<Question> Questions { get; }
    DbSet<InterviewSession> InterviewSessions { get; }
    DbSet<Answer> Answers { get; }
    DbSet<Feedback> Feedbacks { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
