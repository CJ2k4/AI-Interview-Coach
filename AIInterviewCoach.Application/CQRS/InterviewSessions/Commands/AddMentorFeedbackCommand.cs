using AIInterviewCoach.Shared.DTOs;
using MediatR;
using AIInterviewCoach.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AIInterviewCoach.Application.CQRS.InterviewSessions.Commands;

public record AddMentorFeedbackCommand(AddMentorFeedbackDto Dto) : IRequest<bool>;

public class AddMentorFeedbackCommandHandler : IRequestHandler<AddMentorFeedbackCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public AddMentorFeedbackCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(AddMentorFeedbackCommand request, CancellationToken cancellationToken)
    {
        var session = await _context.InterviewSessions
            .Include(s => s.Feedback)
            .FirstOrDefaultAsync(s => s.Id == request.Dto.SessionId, cancellationToken);

        if (session == null) return false;

        if (session.Feedback == null)
        {
            // If for some reason there is no AI feedback, we create one.
            session.Feedback = new AIInterviewCoach.Domain.Entities.Feedback
            {
                InterviewSessionId = session.Id,
                MentorComments = request.Dto.MentorComments
            };
            _context.Feedbacks.Add(session.Feedback);
        }
        else
        {
            session.Feedback.MentorComments = request.Dto.MentorComments;
        }

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
