# My Model Context Protocol

## Overview

McpServer is a modular C# application designed for robust backend operations, featuring a clean architecture with clear separation of concerns. It leverages MediatR for CQRS, FluentValidation for validation.

## Project Structure

- **McpServer.Application**  
  Contains application logic, MediatR handlers, commands, queries, and validators.
  - `Handlers/` — Handlers and validators for customer and document operations.
  - `Documents/` — Reference documents used by the backend (e.g., for Q&A).

- **McpServer.Domain**  
  Domain models, interfaces, and business rules.

- **McpServer.Infrastructure**  
  Implementations of domain interfaces, data access, and external integrations.

- **McpServer.Presentation**  
  Main McpServer application entry point. This layer hosts the server process and exposes the application's functionality to external consumers, such as web clients or other services.

## Key Features

- **CQRS with MediatR**: Decouples request handling and business logic.
- **Validation Pipeline**: Centralized request validation using FluentValidation and MediatR pipeline behaviors.
- **Document Q\&A**: Ask questions about backend documents using AI-powered handlers.
- **Extensible Validation**: Enforce business rules and input validation consistently.

## Use Cases

- **Customer Management**: Create, update, and manage customer data via commands and handlers.
- **Document Intelligence**: Query backend documentation using natural language.

## Getting Started

1. **Restore dependencies**  
   `dotnet restore`

2. **Start the application**  
   Configure and run the desired entry point.

## Contributing

- Follow the project structure and naming conventions.
- Use MediatR and FluentValidation for new handlers and commands.
