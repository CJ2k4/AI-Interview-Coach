using AIInterviewCoach.Shared.DTOs;
using MediatR;
using AIInterviewCoach.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AIInterviewCoach.Application.CQRS.InterviewSessions.Queries;

public record GetInterviewSessionsQuery(string UserId) : IRequest<List<InterviewSessionDto>>;

public class GetInterviewSessionsQueryHandler : IRequestHandler<GetInterviewSessionsQuery, List<InterviewSessionDto>>
{
    private readonly IApplicationDbContext _context;

    public GetInterviewSessionsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<InterviewSessionDto>> Handle(GetInterviewSessionsQuery request, CancellationToken cancellationToken)
    {
        return await _context.InterviewSessions
            .Where(s => s.UserId == request.UserId)
            .Select(s => new InterviewSessionDto
            {
                Id = s.Id,
                UserId = s.UserId,
                JobRoleId = s.JobRoleId,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                Status = s.Status
            })
            .ToListAsync(cancellationToken);
    }
}
