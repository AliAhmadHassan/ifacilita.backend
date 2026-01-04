# iFacilita Backend

Backend platform for automating the property buy/sell process, integrating registries, certifications, seller and buyer data, and electronic document signing. The goal is to reduce manual steps and accelerate real estate transactions.

## Overview
- End-to-end automation of the buy/sell workflow.
- Integrations with registries and legacy systems.
- Orchestration of seller and buyer data.
- Contract generation, inspections, and formalizations.
- Document authentication and signing via DocuSign.
- Around 25 RPA automations for operational tasks.

## Architecture
- Modular solution with dozens of .NET projects organized by domain.
- Layered pattern (API, Service, Repository, Model) for separation of concerns.
- DDD/Clean structure in several modules (Presentation, Application, Domain, Infrastructure, IoC, ExternalServices, Cache).
- Dedicated RPA/Robot components for web automations.

## Technologies and stack
- .NET Core 3.1 / ASP.NET Core Web API.
- MongoDB (MongoDB.Driver) for operational data and logs.
- SQL Server with Dapper + Dapper.FluentMap in core modules.
- Swagger/Swashbuckle for API documentation.
- Observability with Prometheus (prometheus-net) and HealthChecks.
- Structured logging with Serilog and MongoDB sink.
- RPA with Selenium WebDriver + ChromeDriver.
- Anti-captcha via AntiCaptchaAPI with GeeTest support.
- MailKit for email delivery.
- Containerization via Docker.

## Domains and main modules
- Core: orchestration of the main flow, profiles, documents, and business rules.
- DocuSign: document authentication and electronic signing.
- eCartorio / RGI / ITBI / IPTU: integrations with registries and municipal fees.
- Certificates and checks: debts, protests, ESaj, and others.
- Chat, PushNotification, and FileService as supporting services.
- RoboAntiCaptcha and RPA modules for external task automation.

## Observability and reliability
- Health checks for MongoDB and SQL Server.
- Metrics exposed for Prometheus monitoring.
- Centralized logs in MongoDB for execution traceability.

## Repository structure
- `Com.ByteAnalysis.IFacilita.Core.sln`: main solution with all projects.
- Domain folders with APIs, services, repositories, and models.
- `RPA`/`Robot` projects with Selenium-based automation.
- `Dockerfile` for building and publishing services.

---

If you are a recruiter, this repository demonstrates experience with:
- modular architecture and separation of concerns in .NET;
- integration with external systems and complex process automation;
- system observability with metrics, health checks, and structured logs;
- orchestration of RPA automations in real-world environments.
