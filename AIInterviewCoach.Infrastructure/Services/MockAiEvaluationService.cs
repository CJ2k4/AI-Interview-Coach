using AIInterviewCoach.Application.Interfaces;

namespace AIInterviewCoach.Infrastructure.Services;

public class MockAiEvaluationService : IAiEvaluationService
{
    public async Task<AiEvaluationResult> EvaluateAnswerAsync(string transcribedText, string rubric)
    {
        // Simulate GPT-4 processing delay
        await Task.Delay(2000);

        return new AiEvaluationResult
        {
            Score = 85,
            Feedback = "Good answer! You touched on the key points, but could have elaborated more on the specific technologies you used."
        };
    }
}
