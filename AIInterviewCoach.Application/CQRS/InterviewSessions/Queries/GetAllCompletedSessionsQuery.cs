using AIInterviewCoach.Shared.DTOs;
using MediatR;
using AIInterviewCoach.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AIInterviewCoach.Application.CQRS.InterviewSessions.Queries;

public record GetAllCompletedSessionsQuery() : IRequest<List<InterviewSessionDto>>;

public class GetAllCompletedSessionsQueryHandler : IRequestHandler<GetAllCompletedSessionsQuery, List<InterviewSessionDto>>
{
    private readonly IApplicationDbContext _context;

    public GetAllCompletedSessionsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<InterviewSessionDto>> Handle(GetAllCompletedSessionsQuery request, CancellationToken cancellationToken)
    {
        return await _context.InterviewSessions
            .Where(s => s.Status == "Completed")
            .Include(s => s.Feedback)
            .Select(s => new InterviewSessionDto
            {
                Id = s.Id,
                UserId = s.UserId,
                JobRoleId = s.JobRoleId,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                Status = s.Status,
                Feedback = s.Feedback != null ? new FeedbackDto
                {
                    OverallScore = s.Feedback.OverallScore,
                    AIComments = s.Feedback.AIComments,
                    MentorComments = s.Feedback.MentorComments
                } : null
            })
            .OrderByDescending(s => s.EndTime)
            .ToListAsync(cancellationToken);
    }
}
