# Getting Started with Aspire on Windows
> **Current Version:** Aspire 13.2 (as of April 2026)  
> **Official Docs:** [aspire.dev](https://aspire.dev)

---

## What Is Aspire?

Aspire (formerly ".NET Aspire") is Microsoft's open-source framework for building, running, and observing **distributed multi-service applications**. Think of it as a smart orchestrator: instead of manually wiring up Docker containers, environment variables, and service URLs, Aspire does all of that for you with C# code.

Key benefits at a glance:
- **Orchestration** — Define all your services (APIs, databases, frontends) in one place called the **AppHost**
- **Observability** — Built-in dashboard showing logs, traces, and health checks for every service
- **Service Discovery** — Services find each other automatically; no hardcoded ports or URLs
- **Docker-friendly** — Spins up containers (Redis, PostgreSQL, etc.) automatically during development

> Since version 13, Aspire also supports Python and JavaScript/TypeScript first-class, but this guide focuses on C#.

---

## Prerequisites Check

You already have most things installed. Here's your checklist:

| Requirement | Status | Notes |
|---|---|---|
| .NET 8 SDK | ✅ Installed | Aspire can run apps targeting .NET 8+ |
| .NET 10 SDK | ✅ Installed | **Required** for the Aspire CLI (v13+) |
| Visual Studio 2022 | ✅ Installed | Full IDE option |
| VS Code | ✅ Installed | Recommended for Aspire CLI workflow |
| Docker Desktop | ✅ Installed | Required to run container resources |

> ⚠️ **Important:** Make sure Docker Desktop is **running** before you launch any Aspire app.

---

## Step 1 — Install the Aspire CLI

The Aspire CLI is the primary tool for creating and running Aspire projects.

Open **PowerShell** (run as Administrator) and execute:

```powershell
irm https://aspire.dev/install.ps1 | iex
```

This installs the latest stable release of the Aspire CLI globally on your machine.

### Verify the Installation

Close and reopen your terminal, then run:

```powershell
aspire --version
```

You should see output like:

```
13.2.0+abc1234
```

The `+{hash}` suffix is normal — it indicates the exact build commit.

---

## Step 2 — Install the VS Code Extension (Recommended)

For the best VS Code experience, install the Aspire extension:

1. Open VS Code
2. Go to the **Extensions** panel (`Ctrl+Shift+X`)
3. Search for **"Aspire"** and install the official Microsoft extension
4. Also ensure you have the **C#** extension installed (by Microsoft)

This extension adds:
- Aspire-specific commands in the Command Palette
- Syntax highlighting for Aspire project files
- Run/debug integration

---

## Step 3 — Install the Visual Studio 2022 Workload

If you prefer to use Visual Studio 2022 instead of VS Code:

1. Open **Visual Studio Installer**
2. Click **Modify** on your VS 2022 installation
3. Under **Workloads**, ensure **ASP.NET and web development** is checked
4. Under **Individual Components**, search for **".NET Aspire SDK"** and check it
5. Click **Modify** to apply

> Visual Studio 2022 v17.9 or later has built-in Aspire templates and tooling.

---

## Step 4 — Create Your First Aspire Project

Now let's build a simple starter app. Open a terminal (PowerShell or Windows Terminal) and follow these steps.

### 4.1 — Create a Project Folder

```powershell
mkdir C:\Projects\MyFirstAspireApp
cd C:\Projects\MyFirstAspireApp
```

### 4.2 — Create a New Aspire App

Run the interactive CLI:

```powershell
aspire new
```

The CLI will prompt you to select a template. Choose:

```
> starter  (C# — Blazor frontend + API backend)
```

Then follow the prompts:
- **Project name:** `MyFirstAspireApp`
- **Output folder:** Accept the default (current directory)

Aspire downloads the latest templates and generates your solution.

---

## Step 5 — Understand the Project Structure

After creation, your folder will look like this:

```
MyFirstAspireApp/
│
├── MyFirstAspireApp.AppHost/        ← 🧠 THE BRAIN — orchestrates everything
│   ├── Program.cs
│   └── MyFirstAspireApp.AppHost.csproj
│
├── MyFirstAspireApp.ApiService/     ← A simple Web API (ASP.NET Core)
│   ├── Program.cs
│   └── ...
│
├── MyFirstAspireApp.Web/            ← A Blazor frontend
│   ├── Program.cs
│   └── ...
│
└── MyFirstAspireApp.ServiceDefaults/ ← Shared config (telemetry, health checks)
    └── ...
```

### The AppHost — The Most Important File

Open `MyFirstAspireApp.AppHost/Program.cs`. It should look like this:

```csharp
var builder = DistributedApplication.CreateBuilder(args);

// Register the API service as a resource
var apiService = builder.AddProject<Projects.MyFirstAspireApp_ApiService>("apiservice");

// Register the Blazor frontend, telling it where the API lives
builder.AddProject<Projects.MyFirstAspireApp_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
```

**What this code does, line by line:**

| Line | What It Does |
|---|---|
| `AddProject<...>("apiservice")` | Registers the API as a named resource Aspire will start |
| `AddProject<...>("webfrontend")` | Registers the Blazor frontend |
| `.WithExternalHttpEndpoints()` | Exposes the frontend to your browser |
| `.WithReference(apiService)` | Automatically injects the API's URL into the frontend via environment variables |
| `.WaitFor(apiService)` | Tells Aspire: don't start the frontend until the API is healthy |

> 💡 **Key Concept:** You never hardcode `http://localhost:5001` anywhere. Aspire handles all URL wiring automatically.

---

## Step 6 — Run the App

Make sure **Docker Desktop is running**, then in your terminal:

```powershell
aspire run
```

Aspire will:
1. Build all projects
2. Start each service
3. Open the **Aspire Dashboard** in your browser automatically

### What You'll See in the Dashboard

The Aspire Dashboard (at `http://localhost:18888` by default) shows:

- **Resources** tab — all running services with their status (green = healthy)
- **Console** tab — live logs from any service
- **Traces** tab — distributed traces showing requests flowing between services
- **Metrics** tab — performance counters

Click on **webfrontend** in the Resources tab to open the Blazor app in your browser.

---

## Step 7 — Explore the Dashboard

Try these things to understand Aspire's observability:

1. **Click the frontend URL** to open the Blazor app
2. Click the **"Weather"** page in the app — this calls the API
3. Go back to the Dashboard and click **Traces**
4. You'll see a trace showing the full request: Browser → Frontend → API

This is the core value of Aspire: **zero-config distributed tracing out of the box**.

---

## Step 8 — Stop the App

Press `Ctrl+C` in the terminal to stop all services cleanly.

---

## Step 9 — Adding a Redis Cache (Bonus — Uses Docker!)

Since you know Docker, let's add Redis to show how Aspire manages containers automatically.

### 9.1 — Add the Redis Integration Package

In your terminal, from the `AppHost` project directory:

```powershell
cd MyFirstAspireApp.AppHost
aspire add redis
```

The CLI adds the NuGet package for you automatically.

### 9.2 — Update the AppHost

Edit `MyFirstAspireApp.AppHost/Program.cs`:

```csharp
var builder = DistributedApplication.CreateBuilder(args);

// 🆕 Add Redis as a container resource
var cache = builder.AddRedis("cache");

var apiService = builder.AddProject<Projects.MyFirstAspireApp_ApiService>("apiservice")
    .WithReference(cache)   // 🆕 Inject Redis connection into the API
    .WaitFor(cache);        // 🆕 Wait for Redis to be healthy first

builder.AddProject<Projects.MyFirstAspireApp_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
```

### 9.3 — Run Again

```powershell
aspire run
```

Aspire will now automatically:
- Pull the `redis` Docker image (if not already cached)
- Start a Redis container
- Inject the connection string into your API via environment variables
- Show Redis as a resource in the Dashboard

You don't write a single line of Docker Compose YAML.

---

## Quick Reference — Aspire CLI Commands

| Command | What It Does |
|---|---|
| `aspire new` | Create a new Aspire project (interactive) |
| `aspire run` | Build and run the AppHost + all services |
| `aspire add <name>` | Add an integration (redis, postgres, etc.) |
| `aspire update` | Update Aspire packages to the latest version |
| `aspire --version` | Show installed CLI version |
| `aspire cache clear` | Clear the CLI template/package cache |

---

## Key Concepts Summary

| Concept | Simple Explanation |
|---|---|
| **AppHost** | The C# file that describes your entire app: what services exist, how they connect |
| **Resource** | Anything Aspire manages: a .NET project, a Redis container, a PostgreSQL database, etc. |
| **Service Defaults** | A shared library added to every project for telemetry, health checks, and resilience |
| **Dashboard** | A local web UI showing logs, traces, and metrics for all your services |
| **WithReference()** | Tells Aspire to inject connection info from one resource into another automatically |
| **WaitFor()** | Starts services in the correct order, waiting for dependencies to be healthy |

---

## Next Steps

Once you're comfortable with the basics:

- **Add a database:** Try `aspire add postgres` or `aspire add sqlserver`
- **Deploy to Azure:** Run `aspire deploy` to push to Azure Container Apps
- **Official tutorials:** [aspire.dev/get-started/first-app](https://aspire.dev/get-started/first-app/)
- **Aspire dashboard deep dive:** [aspire.dev/dashboard](https://aspire.dev/dashboard/)

---

*Guide based on Aspire 13.2 — April 2026. For the latest, always check [aspire.dev](https://aspire.dev).*
