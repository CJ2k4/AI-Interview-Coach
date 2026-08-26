# AI Interview Coach - Project Walkthrough

This document serves as a living, comprehensive guide to understand how the **AI Interview Coach** project is built. It will be updated automatically at the end of every development phase to reflect the latest architecture, decisions, and codebase additions.

---

## Architecture Overview
This project uses **Clean Architecture** to ensure separation of concerns and maintainability. The `.NET 10` solution (`AIInterviewCoach.sln`) is divided into the following layers:

1. **`AIInterviewCoach.Domain`**: The core of the system. It contains enterprise logic and entities. It has *zero* dependencies on other projects or external frameworks.
2. **`AIInterviewCoach.Application`**: Contains the business use cases (CQRS). It depends on the Domain layer and defines interfaces that the Infrastructure layer will implement.
3. **`AIInterviewCoach.Infrastructure`**: Contains implementation details for external concerns (Data access via Entity Framework Core, Identity, external APIs).
4. **`AIInterviewCoach.Shared`**: Contains Data Transfer Objects (DTOs) and common utilities shared between the backend and the frontend.
5. **`AIInterviewCoach.API`**: The ASP.NET Core Web API project. It acts as the entry point for the backend, wiring up Dependency Injection and routing HTTP requests to the Application layer.
6. **`AIInterviewCoach.UI`**: The Blazor WebAssembly frontend project. It communicates with the backend via the API and provides the user interface.

---

## Development Logs

### ✅ Phase 1: Foundation and Architecture Setup
**Objective:** Establish the core project structure, domain models, and database connectivity.

**What was done:**
1. **Scaffolded Projects**: Created the 6 `.NET 10` class libraries and applications listed in the architecture overview and wired up their project references.
2. **Domain Entities**: Modeled the core data structures in `AIInterviewCoach.Domain/Entities`:
   - `JobRole`: Represents a role a candidate can practice for.
   - `Question`: Represents a question tied to a specific `JobRole`, along with the expected answer rubric.
   - `InterviewSession`: Tracks a candidate's session for a specific `JobRole`.
   - `Answer`: Stores the user's audio/transcript answer and score for a specific `Question`.
   - `Feedback`: The final evaluation generated at the end of an `InterviewSession`.
3. **Data Access (EF Core)**: 
   - Created `ApplicationDbContext` in `AIInterviewCoach.Infrastructure` to manage our database tables using SQLite.
   - Integrated ASP.NET Core Identity by creating an `ApplicationUser` class (extending `IdentityUser`) to handle future authentication needs.
4. **API Configuration**: 
   - Wired up the EF Core context and Identity services in `AIInterviewCoach.API/Program.cs`.
   - Setup the SQLite connection string in `appsettings.json`.
5. **Migrations**: Executed the `InitialCreate` Entity Framework migration, successfully generating the `app.db` local SQLite database file containing all our Identity and Domain tables.

---

### ✅ Phase 2: Core Backend Logic and API
**Objective:** Build out the CQRS architecture and expose RESTful API endpoints.

**What was done:**
1. **Application Interfaces**: 
   - Introduced `IApplicationDbContext` to allow the Application layer to query Domain tables without taking a direct dependency on the Infrastructure layer's EF Core context.
   - Updated `ApplicationDbContext` to implement this new interface.
2. **CQRS with MediatR**:
   - Added `MediatR` and `FluentValidation` to the `AIInterviewCoach.Application` project.
   - Designed generic DTOs (`JobRoleDto`, `CreateJobRoleDto`, `InterviewSessionDto`, `CreateInterviewSessionDto`) in the `Shared` library.
   - Implemented CQRS Commands, Queries, and Handlers for **JobRoles** and **InterviewSessions**.
   - Created a MediatR `ValidationBehavior` Pipeline that automatically intercepts incoming commands and runs FluentValidation checks before the command reaches its handler.
3. **API Controllers**:
   - Registered all Application Services (MediatR, FluentValidation) inside `Program.cs`.
   - Built `JobRolesController` and `InterviewSessionsController` that elegantly route REST HTTP requests (GET, POST) directly to MediatR messages.
   - Configured CORS in `Program.cs` to explicitly allow future UI connections.

---

### ✅ Phase 3: Frontend WebAssembly Setup
**Objective:** Build a premium frontend UI using Blazor WebAssembly and a modern component library.

**What was done:**
1. **Component Library (MudBlazor)**:
   - Integrated `MudBlazor` into the UI project to achieve a premium, modern design aesthetic (Material Design).
   - Configured `index.html` with Roboto fonts and MudBlazor CSS/JS.
   - Updated `_Imports.razor` to include globally required namespaces.
2. **Premium Layout Setup**:
   - Replaced default Bootstrap layout with a custom `MainLayout.razor`.
   - Designed a responsive layout using `MudLayout`, `MudAppBar`, and `MudDrawer`.
   - Implemented a custom vibrant theme and a built-in **Dark Mode** toggle.
3. **API Connectivity**:
   - Configured the UI's `HttpClient` in `Program.cs` to connect directly to the local backend API (`https://localhost:7229/api/`).
4. **Dashboard Pages**:
   - Created a dynamic `Home.razor` landing page for candidates.
   - Built `JobRoles.razor` to fetch available mock interview roles from the backend and present them in interactive `MudCard` elements with hover micro-animations.
   - Built `InterviewSessions.razor` to present the user's interview history in a responsive `MudTable`.

---

### ✅ Phase 4: Authentication & Authorization
**Objective:** Secure the API and Frontend with JWT based authentication.

**What was done:**
1. **API JWT Setup**:
   - Added `Microsoft.AspNetCore.Authentication.JwtBearer` to the API.
   - Configured `appsettings.json` with secure JWT keys.
   - Registered JWT authentication middleware in `Program.cs`.
2. **Auth Controller**:
   - Created `AuthController` with `/register` and `/login` endpoints.
   - Leveraged ASP.NET Core Identity's `UserManager` for creating users and validating passwords.
   - Generated signed JWT tokens containing user claims.
3. **Blazor Auth State**:
   - Built a custom `AuthenticationStateProvider` that decodes the JWT payload from browser local storage.
   - Created a custom `JwtAuthorizationMessageHandler` (`DelegatingHandler`) to automatically append the `Authorization: Bearer <token>` header to all outgoing API requests.
4. **UI Updates**:
   - Built modern `Login.razor` and `Register.razor` pages using `MudBlazor`.
   - Wrapped the application in `<CascadingAuthenticationState>`.
   - Used `<AuthorizeView>` in `MainLayout.razor` to dynamically show "Login/Register" or "Hello, {Name} / Logout".
   - Secured the API `JobRoles` (POST) and `InterviewSessions` endpoints, as well as the UI `InterviewSessions` page, using `[Authorize]`.

---

### ✅ Phase 5: AI & Speech-to-Text Integration
**Objective:** Integrate AI services for mock interviews and evaluation.

**What was done:**
1. **Infrastructure**:
   - Built a robust Interface-driven architecture for file storage, STT (Speech-to-Text), and AI evaluation.
   - Built `LocalFileStorageService` to securely save browser `.webm` audio blobs directly to the backend `wwwroot/uploads` directory.
   - Replaced mock services with real API integrations using the Gemini REST API:
     - **`GeminiSpeechToTextService`**: Connects to `gemini-1.5-flash` via HTTP to natively transcribe `.webm` base64 audio data into text.
     - **`GeminiEvaluationService`**: Connects to `gemini-1.5-flash` using `responseMimeType: application/json` to grade the transcribed answer against the rubric and return a JSON object with `score` and `feedback`.
2. **Application (CQRS)**:
   - Added `SubmitAnswerCommand` to orchestrate the entire process: saving audio -> transcribing -> evaluating -> saving the `Answer` and `Score` entities to Entity Framework Core.
3. **API**:
   - Exposed `AnswersController` that handles `multipart/form-data` uploads safely.
   - Configured `appsettings.json` to store `Gemini:Model`. (API Keys are stored securely in User Secrets).
4. **Blazor UI**:
   - Wrote a custom Javascript Interop file `audioRecorder.js` that taps into the browser's native `navigator.mediaDevices.getUserMedia` and `MediaRecorder` API to capture and stream the microphone input as `Uint8Array` bytes.
   - Designed a polished `AudioRecorder.razor` component with MudBlazor featuring a pulsating visual recording indicator and an indeterminate loading spinner to show background processing while Gemini evaluates the answer.

---

### ✅ Phase 6: Interview Engine & Flow
**Objective**: Tie the components together to create the actual interview experience.

**What was done:**
1. **CQRS & API Updates**:
   - Added `GetQuestionsByJobRoleQuery` to fetch all questions for a specific role.
   - Upgraded `GetInterviewSessionQuery` to use Entity Framework `.Include(s => s.Answers)` so we can eagerly load all recorded answers and scores.
2. **Session Initialization**:
   - Wired up the "Start Interview" button on the `JobRoles` page to create an `InterviewSession` entity via the API and immediately redirect the user to `/interview/{id}`.
3. **Active Interview UI**:
   - Created `ActiveInterview.razor` to act as the primary interview interface.
   - It fetches the session's questions and displays them sequentially, tracking the `_currentQuestionIndex`.
   - It seamlessly integrates the `AudioRecorder.razor` component for capturing answers.
   - Prevents the user from moving to the "Next Question" until their audio is successfully uploaded and graded.
4. **Results Dashboard**:
   - Created `InterviewResults.razor` to display a final overall calculated score.
   - Uses `MudExpansionPanels` to present a beautiful, expandable accordion view of every question asked, alongside the AI's simulated transcription and grading feedback.

---

### ✅ Phase 7: Reporting, Polish, and Deployment
**Objective**: Finalize dashboards, implement Mentor reviews, and add candidate progress tracking.

**What was done:**
1. **Identity & Auth Updates**:
   - Updated `RegisterDto` and `AuthController` to handle an `IsMentor` flag during registration.
   - Ensured the "Mentor" IdentityRole is dynamically created and assigned to users.
   - Updated JWT generation to include `ClaimTypes.Role` so Blazor's UI can conditionally render elements using `<AuthorizeView Roles="Mentor">`.
2. **Backend API & CQRS**:
   - Added `GetAllCompletedSessionsQuery` to fetch all completed interviews across all users for Mentors.
   - Added `AddMentorFeedbackCommand` to allow Mentors to append their own manual comments to an existing `Feedback` entity.
   - Secured Mentor endpoints in `InterviewSessionsController` with `[Authorize(Roles = "Mentor")]`.
   - Updated `InterviewSessionDto` to include `FeedbackDto` so scores are easily accessible in UI lists.
3. **Frontend UI**:
   - Built `MentorDashboard.razor` containing a responsive `MudTable` to display sessions awaiting review.
   - Built `SessionReview.razor` providing Mentors with an accordion view of AI-generated feedback and a text area for manual `MentorComments`.
   - Built `CandidateProgress.razor` incorporating a `MudChart` (Line Chart) to visualize a candidate's `OverallScore` trajectory over multiple practice interviews.
   - Updated `MainLayout.razor` to conditionally show navigation links based on user roles.
