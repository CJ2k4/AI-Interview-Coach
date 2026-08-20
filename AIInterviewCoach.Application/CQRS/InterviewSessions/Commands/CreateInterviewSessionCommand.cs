using AIInterviewCoach.Shared.DTOs;
using MediatR;
using AIInterviewCoach.Application.Interfaces;
using AIInterviewCoach.Domain.Entities;
using FluentValidation;

namespace AIInterviewCoach.Application.CQRS.InterviewSessions.Commands;

public record CreateInterviewSessionCommand(CreateInterviewSessionDto Dto) : IRequest<int>;

public class CreateInterviewSessionCommandValidator : AbstractValidator<CreateInterviewSessionCommand>
{
    public CreateInterviewSessionCommandValidator()
    {
        RuleFor(v => v.Dto.UserId).NotEmpty();
        RuleFor(v => v.Dto.JobRoleId).GreaterThan(0);
    }
}

public class CreateInterviewSessionCommandHandler : IRequestHandler<CreateInterviewSessionCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateInterviewSessionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateInterviewSessionCommand request, CancellationToken cancellationToken)
    {
        var entity = new InterviewSession
        {
            UserId = request.Dto.UserId,
            JobRoleId = request.Dto.JobRoleId,
            StartTime = DateTime.UtcNow,
            Status = "Scheduled"
        };

        _context.InterviewSessions.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
