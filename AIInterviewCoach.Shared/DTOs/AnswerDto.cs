namespace AIInterviewCoach.Shared.DTOs;

public class AnswerDto
{
    public int Id { get; set; }
    public int QuestionId { get; set; }
    public string? Transcript { get; set; }
    public int Score { get; set; }
    public string? AudioUrl { get; set; }
    public string? Feedback { get; set; }
}
