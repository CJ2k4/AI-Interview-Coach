namespace AIInterviewCoach.Domain.Entities;

public class Answer
{
    public int Id { get; set; }
    
    public int InterviewSessionId { get; set; }
    public InterviewSession? InterviewSession { get; set; }
    
    public int QuestionId { get; set; }
    public Question? Question { get; set; }
    
    public string? AudioUrl { get; set; }
    public string? Transcript { get; set; }
    public int Score { get; set; }
    public string? Feedback { get; set; }
}
