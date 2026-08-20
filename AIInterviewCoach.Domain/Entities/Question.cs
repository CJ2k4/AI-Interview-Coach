namespace AIInterviewCoach.Domain.Entities;

public class Question
{
    public int Id { get; set; }
    
    public int JobRoleId { get; set; }
    public JobRole? JobRole { get; set; }
    
    public string Text { get; set; } = string.Empty;
    public string ExpectedAnswerRubric { get; set; } = string.Empty;
}
