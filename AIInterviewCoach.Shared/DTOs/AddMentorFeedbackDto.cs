namespace AIInterviewCoach.Shared.DTOs;

public class AddMentorFeedbackDto
{
    public int SessionId { get; set; }
    public string MentorComments { get; set; } = string.Empty;
}
