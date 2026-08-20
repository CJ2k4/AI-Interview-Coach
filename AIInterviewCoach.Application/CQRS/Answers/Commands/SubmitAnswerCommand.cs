using AIInterviewCoach.Application.Interfaces;
using AIInterviewCoach.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AIInterviewCoach.Application.CQRS.Answers.Commands;

public class SubmitAnswerCommand : IRequest<int>
{
    public int InterviewSessionId { get; set; }
    public int QuestionId { get; set; }
    public Stream AudioStream { get; set; } = Stream.Null;
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
}

public class SubmitAnswerCommandHandler : IRequestHandler<SubmitAnswerCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileStorageService _fileStorageService;
    private readonly ISpeechToTextService _sttService;
    private readonly IAiEvaluationService _aiService;

    public SubmitAnswerCommandHandler(
        IApplicationDbContext context,
        IFileStorageService fileStorageService,
        ISpeechToTextService sttService,
        IAiEvaluationService aiService)
    {
        _context = context;
        _fileStorageService = fileStorageService;
        _sttService = sttService;
        _aiService = aiService;
    }

    public async Task<int> Handle(SubmitAnswerCommand request, CancellationToken cancellationToken)
    {
        var question = await _context.Questions.FindAsync(new object[] { request.QuestionId }, cancellationToken);
        if (question == null) throw new Exception("Question not found");

        var audioUrl = await _fileStorageService.SaveFileAsync(request.AudioStream, request.FileName, request.ContentType);

        var transcript = await _sttService.TranscribeAudioAsync(audioUrl);

        var evaluation = await _aiService.EvaluateAnswerAsync(transcript, question.ExpectedAnswerRubric);

        var answer = new Answer
        {
            InterviewSessionId = request.InterviewSessionId,
            QuestionId = request.QuestionId,
            AudioUrl = audioUrl,
            Transcript = transcript,
            Score = evaluation.Score,
            Feedback = evaluation.Feedback
        };

        _context.Answers.Add(answer);
        await _context.SaveChangesAsync(cancellationToken);

        return answer.Id;
    }
}
