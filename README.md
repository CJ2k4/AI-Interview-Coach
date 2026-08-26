# 🎤 AI Interview Coach

Welcome to **AI Interview Coach**, an interactive web application designed to help candidates practice their interview skills with real-time feedback powered by Google's Gemini AI. The platform features an interactive Voice UI for candidates and a complete management dashboard for Mentors/Admins.

## 🚀 Tech Stack
- **Frontend**: Blazor WebAssembly (.NET 10), MudBlazor UI Library
- **Backend**: ASP.NET Core Web API (.NET 10)
- **Database**: Entity Framework Core with SQLite
- **AI Integration**: Google Generative Language API (Gemini 3.5 Flash)

---

## 🛠️ Prerequisites
Before you begin, ensure you have the following installed on your local machine:
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- A valid [Google Gemini API Key](https://aistudio.google.com/app/apikey)

---

## 🏃‍♂️ Getting Started (Local Development)

Follow these steps to clone the project, configure your secrets, and run it locally.

### 1. Clone the Repository
```bash
git clone https://github.com/CJ2k4/AI-Interview-Coach.git
cd AI-Interview-Coach
```

### 2. Configure Your API Key
We use .NET User Secrets to securely store API keys so they are never committed to GitHub.

Navigate to the API folder and set your Google Gemini API key:
```bash
cd AIInterviewCoach.API
dotnet user-secrets init
dotnet user-secrets set "Gemini:ApiKey" "YOUR_API_KEY_HERE"
```

### 3. Setup the Database
Since the SQLite `.db` file is ignored by Git, you'll need to create the database using the existing Entity Framework migrations. Ensure you are still in the `AIInterviewCoach.API` directory:
```bash
# If you don't have the EF Core tools installed globally, run this first:
# dotnet tool install --global dotnet-ef

dotnet ef database update --project ../AIInterviewCoach.Infrastructure --startup-project .
```

### 4. Run the Backend API
Start the backend server (still inside the `AIInterviewCoach.API` folder):
```bash
dotnet run
```
The API will start listening on `http://localhost:5251`.

### 5. Run the Blazor Frontend UI
Open a **new terminal window**, navigate to the UI project, and start the frontend:
```bash
cd AIInterviewCoach.UI
dotnet run
```
The UI will start listening on `http://localhost:5246`. Open this URL in your web browser!

---

## 🛡️ Admin & Mentor Workflow

By default, creating an account with "Is Mentor" checked will place the user in a **Pending** state. They must be approved by an Admin to access mentor privileges.

### How to seed an Admin account:
1. Ensure both the API and UI are running.
2. Open your browser and navigate directly to:
   `http://localhost:5251/api/Auth/seed-admin`
3. This will instantly create an Admin account with:
   - **Email:** `admin@admin.com`
   - **Password:** `Admin@123`
4. Log into the UI using this account to access the **Admin Dashboard**, where you can approve pending Mentors!
