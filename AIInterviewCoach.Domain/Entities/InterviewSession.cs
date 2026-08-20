namespace AIInterviewCoach.Domain.Entities;

public class InterviewSession
{
    public int Id { get; set; }
    
    public string UserId { get; set; } = string.Empty; // References ApplicationUser (Identity)
    
    public int JobRoleId { get; set; }
    public JobRole? JobRole { get; set; }
    
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    
    public string Status { get; set; } = "Scheduled"; // E.g., Scheduled, InProgress, Completed
    
    public ICollection<Answer> Answers { get; set; } = new List<Answer>();
    public Feedback? Feedback { get; set; }
}
