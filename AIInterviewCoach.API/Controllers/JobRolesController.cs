using AIInterviewCoach.Application.CQRS.JobRoles.Commands;
using AIInterviewCoach.Application.CQRS.JobRoles.Queries;
using AIInterviewCoach.Shared.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace AIInterviewCoach.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JobRolesController : ControllerBase
{
    private readonly IMediator _mediator;

    public JobRolesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<JobRoleDto>>> Get()
    {
        return await _mediator.Send(new GetJobRolesQuery());
    }

    [HttpPost]
    [Authorize(Roles = "Mentor,Admin")]
    public async Task<ActionResult<int>> Create(CreateJobRoleDto dto)
    {
        var id = await _mediator.Send(new CreateJobRoleCommand(dto));
        return Ok(id);
    }
}
