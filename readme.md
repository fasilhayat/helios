# Helios

Helios is a distributed application playground built on [.NET Aspire](https://aspire.dev). It serves as a foundation for exploring service orchestration, infrastructure composition, and resilient system design.

The project is intentionally minimal at its core, but structured to evolve into a more advanced architecture including caching, messaging, and reliability patterns.

---

## Readiness Score: 3 / 10

| Area | Status | Notes |
|---|---|---|
| Runnable locally | ✅ | Works via `make run` |
| API + Swagger UI | ✅ | `/weatherforecast` endpoint with Swagger |
| Infrastructure (Redis) | ✅ | Provisioned via Aspire |
| Aspire Dashboard | ✅ | Logs, traces, health checks |
| Aspire Getting Started Guide | ✅ | See `docs/` |
| Tests | ❌ | No test projects |
| CI/CD pipeline | ❌ | No workflows defined |
| Real business logic | ❌ | Placeholder weather endpoint only |
| License | ❌ | Not defined |
| Production readiness | ❌ | Intentionally a playground |

> Helios is a **learning and experimentation environment**, not a production system. The score reflects that intentional scope.

---

## Purpose

Helios exists to:

* Understand how .NET Aspire composes distributed systems
* Experiment with service-to-service communication
* Introduce infrastructure (e.g. Redis) as first-class dependencies
* Explore observability through the Aspire dashboard
* Serve as a foundation for advanced patterns such as AOP-based resiliency and Akka.NET integration

---

## Current Architecture

The system currently consists of:

* **AppHost** — Orchestration layer that defines and wires all services and resources
* **API (ASP.NET Core)** — Minimal web API with a `/weatherforecast` endpoint and Swagger UI
* **Redis** — Provisioned as a container by Aspire, injected into the API as a named reference
* **ServiceDefaults** — Shared library for telemetry, health checks, and resilience configuration

```
helios/
├── aspire-playground/
│   └── src/
│       ├── Api/                             ← ASP.NET Core API
│       ├── AspirePlayground.AppHost/        ← Aspire orchestration entry point
│       ├── AspirePlayground.ServiceDefaults/ ← Shared service configuration
│       ├── AspirePlayground.sln
│       └── Makefile
└── docs/
    └── aspire-getting-started-guide.md
```

---

## Getting Started

### Prerequisites

| Requirement | Notes |
|---|---|
| .NET 8 SDK | Required for service projects |
| .NET 10 SDK | Required by the Aspire CLI (v13+) |
| Docker Desktop | Required — Aspire provisions Redis as a container |
| GNU Make | Or run `dotnet` commands directly (see below) |

> ⚠️ Make sure **Docker Desktop is running** before launching the app.

---

### Run the system

From the `aspire-playground/src/` directory:

```bash
make run
```

Or directly with the .NET CLI:

```bash
dotnet run --project aspire-playground/src/AspirePlayground.AppHost/AspirePlayground.AppHost.csproj
```

This will:

* Start the Aspire AppHost
* Pull and start a Redis container
* Launch the API
* Open the Aspire Dashboard in your browser

---

## Aspire Dashboard

Once running, the dashboard (default: `http://localhost:18888`) provides:

* **Resources** — all running services and their health status
* **Console** — live logs per service
* **Traces** — distributed traces across services
* **Metrics** — performance counters

---

## API

The API exposes:

| Endpoint | Method | Description |
|---|---|---|
| `/weatherforecast` | GET | Returns 5 random weather forecasts |
| `/swagger` | GET | Swagger UI (development only) |

---

## Development Workflow

From `aspire-playground/src/`:

```bash
make build      # Build the solution
make restore    # Restore NuGet packages
make run        # Start the full Aspire application
make clean      # Clean build artifacts
```

---

## Next Steps

Planned evolution of Helios includes:

* Redis-backed caching in the API
* Structured logging and tracing across services
* Integration with Akka.NET for reliable message delivery
* AOP-based resiliency (retries, backoff, fault handling)
* Expansion toward a service-oriented architecture
* CI/CD pipeline with GitHub Actions

---

## Docs

* [`docs/aspire-getting-started-guide.md`](docs/aspire-getting-started-guide.md) — Full guide to getting started with .NET Aspire on Windows

---

## Notes

Helios is not intended as a production system. It is a controlled environment for exploring architectural patterns and validating ideas before applying them in real-world systems.

---

## License

TBD
