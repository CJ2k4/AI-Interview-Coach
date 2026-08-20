namespace AIInterviewCoach.Application.Interfaces;

public class AiEvaluationResult
{
    public int Score { get; set; }
    public string Feedback { get; set; } = string.Empty;
}

public interface IAiEvaluationService
{
    Task<AiEvaluationResult> EvaluateAnswerAsync(string transcribedText, string rubric);
}
