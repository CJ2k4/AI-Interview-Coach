namespace AIInterviewCoach.Domain.Entities;

public class Feedback
{
    public int Id { get; set; }
    
    public int InterviewSessionId { get; set; }
    public InterviewSession? InterviewSession { get; set; }
    
    public int OverallScore { get; set; }
    public string AIComments { get; set; } = string.Empty;
    public string MentorComments { get; set; } = string.Empty;
}
