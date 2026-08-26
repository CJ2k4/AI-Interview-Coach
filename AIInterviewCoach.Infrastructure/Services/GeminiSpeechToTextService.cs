using AIInterviewCoach.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;

namespace AIInterviewCoach.Infrastructure.Services;

public class GeminiSpeechToTextService : ISpeechToTextService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _model;
    private readonly string _wwwrootPath;

    public GeminiSpeechToTextService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _apiKey = config["Gemini:ApiKey"] ?? string.Empty;
        _model = config["Gemini:Model"] ?? "gemini-1.5-flash";
        _wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
    }

    public async Task<string> TranscribeAudioAsync(string fileUrl)
    {
        if (string.IsNullOrEmpty(fileUrl)) return string.Empty;

        var localPath = Path.Combine(_wwwrootPath, fileUrl.TrimStart('/'));
        if (!File.Exists(localPath))
        {
            throw new FileNotFoundException("Audio file not found", localPath);
        }

        var fileBytes = await File.ReadAllBytesAsync(localPath);
        var base64Data = Convert.ToBase64String(fileBytes);

        var requestBody = new
        {
            contents = new[]
            {
                new
                {
                    parts = new object[]
                    {
                        new { text = "Please transcribe this audio exactly as spoken. Return only the transcription, without any extra text or formatting." },
                        new { inlineData = new { mimeType = "audio/webm", data = base64Data } }
                    }
                }
            }
        };

        var json = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var url = $"https://generativelanguage.googleapis.com/v1beta/models/{_model}:generateContent?key={_apiKey}";
        
        var response = await _httpClient.PostAsync(url, content);
        
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Gemini API error: {errorContent}");
        }

        var responseJson = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(responseJson);
        
        try
        {
            var text = doc.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString();
                
            return text?.Trim() ?? string.Empty;
        }
        catch
        {
            return string.Empty;
        }
    }
}
