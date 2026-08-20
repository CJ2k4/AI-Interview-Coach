namespace AIInterviewCoach.Shared.DTOs;

public class QuestionDto
{
    public int Id { get; set; }
    public int JobRoleId { get; set; }
    public string Text { get; set; } = string.Empty;
    public string ExpectedAnswerRubric { get; set; } = string.Empty;
}
