namespace AIInterviewCoach.Domain.Entities;

public class JobRole
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Requirements { get; set; } = string.Empty;
    
    public ICollection<Question> Questions { get; set; } = new List<Question>();
    public ICollection<InterviewSession> InterviewSessions { get; set; } = new List<InterviewSession>();
}
