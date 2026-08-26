using AIInterviewCoach.Application.CQRS.InterviewSessions.Commands;
using AIInterviewCoach.Application.CQRS.InterviewSessions.Queries;
using AIInterviewCoach.Shared.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace AIInterviewCoach.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class InterviewSessionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public InterviewSessionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<List<InterviewSessionDto>>> GetByUserId(string userId)
    {
        return await _mediator.Send(new GetInterviewSessionsQuery(userId));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<InterviewSessionDto>> GetById(int id)
    {
        var session = await _mediator.Send(new GetInterviewSessionQuery(id));
        if (session == null) return NotFound();
        return Ok(session);
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create(CreateInterviewSessionDto dto)
    {
        var id = await _mediator.Send(new CreateInterviewSessionCommand(dto));
        return Ok(id);
    }

    [HttpGet("completed")]
    [Authorize(Roles = "Mentor")]
    public async Task<ActionResult<List<InterviewSessionDto>>> GetAllCompleted()
    {
        return await _mediator.Send(new GetAllCompletedSessionsQuery());
    }

    [HttpPost("feedback")]
    [Authorize(Roles = "Mentor")]
    public async Task<ActionResult> AddMentorFeedback(AddMentorFeedbackDto dto)
    {
        var result = await _mediator.Send(new AddMentorFeedbackCommand(dto));
        if (!result) return NotFound();
        return Ok();
    }
}
