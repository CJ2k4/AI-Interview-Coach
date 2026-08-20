using AIInterviewCoach.Shared.DTOs;
using MediatR;
using AIInterviewCoach.Application.Interfaces;
using AIInterviewCoach.Domain.Entities;
using FluentValidation;

namespace AIInterviewCoach.Application.CQRS.JobRoles.Commands;

public record CreateJobRoleCommand(CreateJobRoleDto Dto) : IRequest<int>;

public class CreateJobRoleCommandValidator : AbstractValidator<CreateJobRoleCommand>
{
    public CreateJobRoleCommandValidator()
    {
        RuleFor(v => v.Dto.Title).NotEmpty().MaximumLength(200);
        RuleFor(v => v.Dto.Description).NotEmpty();
    }
}

public class CreateJobRoleCommandHandler : IRequestHandler<CreateJobRoleCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateJobRoleCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateJobRoleCommand request, CancellationToken cancellationToken)
    {
        var entity = new JobRole
        {
            Title = request.Dto.Title,
            Description = request.Dto.Description,
            Requirements = request.Dto.Requirements
        };

        _context.JobRoles.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
