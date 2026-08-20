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
}
