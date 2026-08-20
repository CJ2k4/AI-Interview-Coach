using AIInterviewCoach.Shared.DTOs;
using MediatR;
using AIInterviewCoach.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AIInterviewCoach.Application.CQRS.JobRoles.Queries;

public record GetJobRolesQuery : IRequest<List<JobRoleDto>>;

public class GetJobRolesQueryHandler : IRequestHandler<GetJobRolesQuery, List<JobRoleDto>>
{
    private readonly IApplicationDbContext _context;

    public GetJobRolesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<JobRoleDto>> Handle(GetJobRolesQuery request, CancellationToken cancellationToken)
    {
        return await _context.JobRoles
            .Select(j => new JobRoleDto
            {
                Id = j.Id,
                Title = j.Title,
                Description = j.Description,
                Requirements = j.Requirements
            })
            .ToListAsync(cancellationToken);
    }
}
