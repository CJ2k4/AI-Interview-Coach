namespace AIInterviewCoach.Shared.DTOs;

public class CreateInterviewSessionDto
{
    public string UserId { get; set; } = string.Empty;
    public int JobRoleId { get; set; }
}
