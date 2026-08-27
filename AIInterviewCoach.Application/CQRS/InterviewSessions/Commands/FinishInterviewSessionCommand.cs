using MediatR;
using AIInterviewCoach.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using AIInterviewCoach.Domain.Entities;

namespace AIInterviewCoach.Application.CQRS.InterviewSessions.Commands;

public record FinishInterviewSessionCommand(int SessionId) : IRequest<bool>;

public class FinishInterviewSessionCommandHandler : IRequestHandler<FinishInterviewSessionCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public FinishInterviewSessionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(FinishInterviewSessionCommand request, CancellationToken cancellationToken)
    {
        var session = await _context.InterviewSessions
            .Include(s => s.Answers)
            .Include(s => s.Feedback)
            .FirstOrDefaultAsync(s => s.Id == request.SessionId, cancellationToken);

        if (session == null) return false;

        session.Status = "Completed";
        session.EndTime = DateTime.UtcNow;

        int overallScore = 0;
        if (session.Answers.Any())
        {
            overallScore = (int)session.Answers.Average(a => a.Score);
        }

        if (session.Feedback == null)
        {
            session.Feedback = new Feedback
            {
                InterviewSessionId = session.Id,
                OverallScore = overallScore,
                AIComments = "Interview completed. Score calculated based on answers."
            };
            _context.Feedbacks.Add(session.Feedback);
        }
        else
        {
            session.Feedback.OverallScore = overallScore;
        }

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
