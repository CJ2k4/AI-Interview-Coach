namespace AIInterviewCoach.Shared.DTOs;

public class FeedbackDto
{
    public int OverallScore { get; set; }
    public string AIComments { get; set; } = string.Empty;
    public string MentorComments { get; set; } = string.Empty;
}
