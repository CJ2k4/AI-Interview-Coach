using AIInterviewCoach.Application.Interfaces;
using AIInterviewCoach.Domain.Entities;
using AIInterviewCoach.Shared.DTOs;
using MediatR;

namespace AIInterviewCoach.Application.CQRS.Questions.Commands;

public record CreateQuestionCommand(CreateQuestionDto Dto) : IRequest<int>;

public class CreateQuestionCommandHandler : IRequestHandler<CreateQuestionCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateQuestionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateQuestionCommand request, CancellationToken cancellationToken)
    {
        var question = new Question
        {
            JobRoleId = request.Dto.JobRoleId,
            Text = request.Dto.Text,
            ExpectedAnswerRubric = request.Dto.ExpectedAnswerRubric
        };

        _context.Questions.Add(question);
        await _context.SaveChangesAsync(cancellationToken);

        return question.Id;
    }
}
