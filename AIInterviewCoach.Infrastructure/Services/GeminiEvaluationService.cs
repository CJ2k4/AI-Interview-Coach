using AIInterviewCoach.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;

namespace AIInterviewCoach.Infrastructure.Services;

public class GeminiEvaluationService : IAiEvaluationService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _model;

    public GeminiEvaluationService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _apiKey = config["Gemini:ApiKey"] ?? string.Empty;
        _model = config["Gemini:Model"] ?? "gemini-1.5-flash";
    }

    public async Task<AiEvaluationResult> EvaluateAnswerAsync(string transcribedText, string rubric)
    {
        var prompt = $@"You are an expert AI interview coach.
Evaluate the candidate's answer based on the provided rubric.
You must return your evaluation in strict JSON format containing exactly two properties:
- ""score"": An integer from 0 to 100 representing the overall quality of the answer.
- ""feedback"": A detailed string providing constructive feedback, highlighting strengths and areas for improvement based on the rubric.

Rubric:
{rubric}

Candidate's Answer:
{transcribedText}";

        var requestBody = new
        {
            contents = new[]
            {
                new
                {
                    parts = new[]
                    {
                        new { text = prompt }
                    }
                }
            },
            generationConfig = new
            {
                responseMimeType = "application/json",
                temperature = 0.3
            }
        };

        var json = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var url = $"https://generativelanguage.googleapis.com/v1beta/models/{_model}:generateContent?key={_apiKey}";
        
        var response = await _httpClient.PostAsync(url, content);
        
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            return new AiEvaluationResult { Score = 0, Feedback = $"Gemini API request failed. {errorContent}" };
        }

        var responseJson = await response.Content.ReadAsStringAsync();
        
        try
        {
            using var doc = JsonDocument.Parse(responseJson);
            var text = doc.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString();
                
            var evaluation = JsonSerializer.Deserialize<AiEvaluationResult>(text ?? "{}", new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return evaluation ?? new AiEvaluationResult { Score = 0, Feedback = "Failed to parse JSON." };
        }
        catch (Exception ex)
        {
            return new AiEvaluationResult { Score = 0, Feedback = $"Error generating evaluation: {ex.Message}" };
        }
    }
}
