# My Model Contest Protocol

## Overview

McpServer is a modular C# application designed for robust backend operations, featuring a clean architecture with clear separation of concerns. It leverages MediatR for CQRS, FluentValidation for validation, and supports Azure Functions for serverless execution.

## Project Structure

- **McpServer.Application**  
  Contains application logic, MediatR handlers, commands, queries, and validators.
  - `CustomerHandlers/` — Handlers and validators for customer-related operations.
  - `KernelMemoryHandlers/` — Handlers for document-based Q\&A and memory operations.
  - `Documents/` — Reference documents used by the backend (e.g., for Q\&A).

- **McpServer.Domain**  
  Domain models, interfaces, and business rules.

- **McpServer.Function**  
  Azure Functions endpoints, including tools for document Q\&A.

- **McpServer.Infrastructure**  
  Implementations of domain interfaces, data access, and external integrations.

- **McpServer.WebService**  
  Hosts the HTTP API endpoints, serving as the main web entry point for client applications.

## Key Features

- **CQRS with MediatR**: Decouples request handling and business logic.
- **Validation Pipeline**: Centralized request validation using FluentValidation and MediatR pipeline behaviors.
- **Document Q\&A**: Ask questions about backend documents using AI-powered handlers.
- **Azure Functions Integration**: Exposes backend logic as serverless functions.
- **Extensible Validation**: Enforce business rules and input validation consistently.

## Use Cases

- **Customer Management**: Create, update, and manage customer data via commands and handlers.
- **Document Intelligence**: Query backend documentation using natural language.
- **Serverless APIs**: Deploy business logic as scalable Azure Functions.
- **Web APIs**: Interact with backend services through HTTP endpoints.

## Getting Started

1. **Restore dependencies**  
   `dotnet restore`

2. **Start the application**  
   Configure and run the desired entry point (e.g., Azure Functions host or WebService).

## Contributing

- Follow the project structure and naming conventions.
- Use MediatR and FluentValidation for new handlers and commands.

## License

MIT (or specify your license)