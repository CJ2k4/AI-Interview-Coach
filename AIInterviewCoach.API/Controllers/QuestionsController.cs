using AIInterviewCoach.Application.CQRS.Questions.Queries;
using AIInterviewCoach.Shared.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AIInterviewCoach.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class QuestionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public QuestionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("jobRole/{jobRoleId}")]
    public async Task<ActionResult<List<QuestionDto>>> GetByJobRole(int jobRoleId)
    {
        return await _mediator.Send(new GetQuestionsByJobRoleQuery(jobRoleId));
    }

    [HttpPost]
    [Authorize(Roles = "Mentor,Admin")]
    public async Task<ActionResult<int>> Create(CreateQuestionDto dto)
    {
        var id = await _mediator.Send(new AIInterviewCoach.Application.CQRS.Questions.Commands.CreateQuestionCommand(dto));
        return Ok(id);
    }
}
