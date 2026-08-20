namespace AIInterviewCoach.Shared.DTOs;

public class InterviewSessionDto
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int JobRoleId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public string Status { get; set; } = string.Empty;
    public List<AnswerDto> Answers { get; set; } = new();
}
