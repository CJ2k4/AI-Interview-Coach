namespace AIInterviewCoach.Shared.DTOs;

public class CreateQuestionDto
{
    public int JobRoleId { get; set; }
    public string Text { get; set; } = string.Empty;
    public string ExpectedAnswerRubric { get; set; } = string.Empty;
}
