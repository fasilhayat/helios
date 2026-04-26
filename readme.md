# Helios

Helios is a distributed application playground built on .NET Aspire. It serves as a foundation for exploring service orchestration, infrastructure composition, and resilient system design.

The project is intentionally minimal at its core, but structured to evolve into a more advanced architecture including caching, messaging, and reliability patterns.

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

* **AppHost**
  The orchestration layer that defines and runs the distributed application

* **API (ASP.NET Core)**
  A simple service used as the primary interaction point

* **Redis**
  Provisioned via Aspire and injected as a dependency for caching scenarios

---

## Project Structure

```
helios/
  src/
    Api/
    AspirePlayground.AppHost/
    AspirePlayground.ServiceDefaults/
  AspirePlayground.sln
  Makefile
```

---

## Getting Started

### Prerequisites

* .NET 8 SDK
* Docker (required for Aspire-managed containers)
* GNU Make (or compatible environment on Windows)

---

### Run the system

From the repository root:

```
make run
```

This will:

* Start the Aspire AppHost
* Provision Redis
* Launch the API
* Open the Aspire dashboard

---

## Aspire Dashboard

Once running, the dashboard provides:

* Service graph visualization
* Logs per service
* Environment and configuration inspection

This is the primary interface for understanding how services interact.

---

## Development Workflow

Common commands:

```
make build
make clean
make restore
make run
```

---

## Next Steps

Planned evolution of Helios includes:

* Redis-backed caching in the API
* Structured logging and tracing across services
* Integration with Akka.NET for reliable message delivery
* AOP-based resiliency (retries, backoff, fault handling)
* Expansion toward a service-oriented architecture

---

## Notes

Helios is not intended as a production system. It is a controlled environment for exploring architectural patterns and validating ideas before applying them in real-world systems.

---

## License

TBD
