using AIInterviewCoach.Application.CQRS.Answers.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AIInterviewCoach.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AnswersController : ControllerBase
{
    private readonly IMediator _mediator;

    public AnswersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<int>> SubmitAnswer([FromForm] int interviewSessionId, [FromForm] int questionId, IFormFile audioFile)
    {
        if (audioFile == null || audioFile.Length == 0)
        {
            return BadRequest("Audio file is required.");
        }

        using var stream = audioFile.OpenReadStream();
        
        var command = new SubmitAnswerCommand
        {
            InterviewSessionId = interviewSessionId,
            QuestionId = questionId,
            AudioStream = stream,
            FileName = audioFile.FileName,
            ContentType = audioFile.ContentType
        };

        var answerId = await _mediator.Send(command);
        return Ok(answerId);
    }
}
