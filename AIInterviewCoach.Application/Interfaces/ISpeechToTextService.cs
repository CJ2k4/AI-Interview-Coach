namespace AIInterviewCoach.Application.Interfaces;

public interface ISpeechToTextService
{
    Task<string> TranscribeAudioAsync(string fileUrl);
}
