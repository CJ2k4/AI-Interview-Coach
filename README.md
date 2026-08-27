# AI Interview Coach

AI Interview Coach is a comprehensive, modern web application designed to help candidates practice and improve their interview skills through AI-driven audio interviews, while allowing mentors and admins to manage content and provide personalized feedback.

Built with a **.NET 10 Web API** (using Clean Architecture and CQRS) on the backend and a **Blazor WebAssembly** (MudBlazor) application on the frontend, this project integrates directly with Google's Gemini AI to transcribe audio responses and evaluate them against custom grading rubrics.

## 🚀 Key Features

### For Candidates
*   **Audio-Based Interviews**: Start an interview for specific job roles. Record your answers using your microphone directly in the browser.
*   **Instant AI Evaluation**: Upon finishing the interview, your audio is transcribed using Gemini's multimodal capabilities, and an AI agent evaluates your answers against a predefined grading rubric.
*   **Detailed Results & Feedback**: View your overall interview score and drill down into each question to read the transcription, the exact score received, and constructive AI feedback on how to improve.
*   **Progress Tracking**: A dedicated "My Progress" dashboard automatically visualizes your overall interview scores over time on a line chart so you can track your improvement.
*   **Resume Capability**: Interviews are robust—you can safely pause or accidentally close your tab, and later hit "Resume Interview" to pick up exactly where you left off.

### For Mentors
*   **Mentor Dashboard**: Review a feed of all completed candidate interviews.
*   **Manual Reviews**: Drill into specific sessions to review the AI's transcriptions, scores, and feedback.
*   **Add Mentor Feedback**: Provide human-in-the-loop feedback to override or supplement the AI's grading, leaving specific personalized comments for the candidate.

### For Administrators
*   **Job Role Management**: Create and manage job roles (e.g., Frontend Developer, Backend Developer), including descriptions and requirements.
*   **Question Bank Management**: Add specific questions to each job role.
*   **Custom Grading Rubrics**: For every question, define a specific "Expected Answer Rubric" that guides the AI on exactly what to listen for and how strictly to grade the answer.
*   **User Management**: Approve pending mentor registrations from the Admin Dashboard to grant them access to candidate reviews.

## 🛠 Tech Stack

**Frontend (Client)**
*   Blazor WebAssembly (.NET 10)
*   MudBlazor Component Library (Material Design)
*   Browser MediaRecorder API for audio capture

**Backend (Server)**
*   ASP.NET Core Web API (.NET 10)
*   Clean Architecture (Domain, Application, Infrastructure, API layers)
*   MediatR for CQRS (Command Query Responsibility Segregation) pattern
*   Entity Framework Core (SQLite for easy development)
*   ASP.NET Core Identity for Authentication & JWT tokens

**AI & Integrations**
*   **Gemini AI Multimodal**: `GeminiSpeechToTextService` to accurately transcribe candidate audio responses.
*   **Gemini AI LLM**: `GeminiEvaluationService` to prompt the LLM to act as a harsh but fair interviewer, parsing transcripts and rubrics into structured JSON scores.
*   **Local File Storage**: Saves audio blobs to the server for processing and later mentor review.

## 🏃 Getting Started

### Prerequisites
*   .NET 10 SDK
*   A Gemini API Key (Add to `appsettings.json` under `GeminiSettings:ApiKey`)

### Running the Application

1. **Start the API Server**:
   ```bash
   cd AIInterviewCoach.API
   dotnet run
   ```
2. **Start the Blazor UI**:
   ```bash
   cd AIInterviewCoach.UI
   dotnet run
   ```
3. Open your browser to `http://localhost:5246`.

### Default Accounts
*   **Admin Seeder**: Navigate to `http://localhost:5251/api/Auth/seed-admin` in your browser to generate the default admin account:
    *   Email: `admin@admin.com`
    *   Password: `Admin@123`
