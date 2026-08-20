using AIInterviewCoach.Application.Interfaces;
using AIInterviewCoach.Shared.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AIInterviewCoach.Application.CQRS.Questions.Queries;

public record GetQuestionsByJobRoleQuery(int JobRoleId) : IRequest<List<QuestionDto>>;

public class GetQuestionsByJobRoleQueryHandler : IRequestHandler<GetQuestionsByJobRoleQuery, List<QuestionDto>>
{
    private readonly IApplicationDbContext _context;

    public GetQuestionsByJobRoleQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<QuestionDto>> Handle(GetQuestionsByJobRoleQuery request, CancellationToken cancellationToken)
    {
        return await _context.Questions
            .Where(q => q.JobRoleId == request.JobRoleId)
            .Select(q => new QuestionDto
            {
                Id = q.Id,
                JobRoleId = q.JobRoleId,
                Text = q.Text,
                ExpectedAnswerRubric = q.ExpectedAnswerRubric
            })
            .ToListAsync(cancellationToken);
    }
}
