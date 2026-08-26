using AIInterviewCoach.Application.Interfaces;
using AIInterviewCoach.Shared.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AIInterviewCoach.Application.CQRS.InterviewSessions.Queries;

public record GetInterviewSessionQuery(int Id) : IRequest<InterviewSessionDto?>;

public class GetInterviewSessionQueryHandler : IRequestHandler<GetInterviewSessionQuery, InterviewSessionDto?>
{
    private readonly IApplicationDbContext _context;

    public GetInterviewSessionQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<InterviewSessionDto?> Handle(GetInterviewSessionQuery request, CancellationToken cancellationToken)
    {
        var session = await _context.InterviewSessions
            .Include(s => s.Answers)
            .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

        if (session == null) return null;

        return new InterviewSessionDto
        {
            Id = session.Id,
            UserId = session.UserId,
            JobRoleId = session.JobRoleId,
            StartTime = session.StartTime,
            EndTime = session.EndTime,
            Status = session.Status,
            Answers = session.Answers.Select(a => new AnswerDto
            {
                Id = a.Id,
                QuestionId = a.QuestionId,
                Transcript = a.Transcript,
                Score = a.Score,
                AudioUrl = a.AudioUrl,
                Feedback = a.Feedback
            }).ToList(),
            Feedback = session.Feedback != null ? new FeedbackDto
            {
                OverallScore = session.Feedback.OverallScore,
                AIComments = session.Feedback.AIComments,
                MentorComments = session.Feedback.MentorComments
            } : null
        };
    }
}
