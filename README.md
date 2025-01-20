# Sales API - Backend Implementation

## Overview

The 123Vendas system is divided into multiple domains, including Inventory, CRM (Customer), and Sales. As a Sales team developer, you are required to implement a prototype API for managing sales.

Since we follow Domain-Driven Design (DDD), references to entities from other domains should use the **External Identities** pattern, with data denormalization for descriptive attributes.

## Features

- REST API using .NET Core 8 with JWT Authentication
- Backend for managing sales transactions
- Integration with external identity providers

## .Net Core 8 Layer Model (Universal Reference Architecture)

```plaintext
.github/                  # GitHub Actions configuration or other workflow settings
.vscode/                  # Visual Studio Code-specific configuration
src/                      # Source code folder
    Sales.API/            # API Layer (Controllers, Middlewares, Filters)
    Sales.Application/    # Application Layer (Use Cases, DTOs, Interfaces)
    Sales.Domain/         # Domain Layer (Entities, Aggregates, Domain Services)
    Sales.Infrastructure/ # Infrastructure Layer (Repositories, Database Context, External Integrations)
    Sales.Tests/          # Unit and Integration tests
docs/                     # Project documentation
scripts/                  # Automation scripts
.env                      # Environment variables file
docker-compose.yml        # Docker Compose file for multi-container configuration
Dockerfile                # Dockerfile for image building
README.md                 # Project description
```

## Technologies and Best Practices

- **Logging**: Use Serilog
- **Layered Architecture**: API, Application, Domain, Infrastructure separation
- **Git Workflow**: Implement Git Flow
- **Commit Practices**: Follow Semantic Commit standards
- **REST API Principles**: Implement RESTful API
- **Code Quality**: Clean Code, SOLID principles, DRY, YAGNI
- **Best Practices**: Object Calisthenics

## Testing Requirements

- **Unit Testing**
  - Xunit
  - FluentAssertions
  - Bogus
  - NSubstitute
- **Integration Testing** (Recommended)
  - Testing Containers

## Installation and Setup

### Prerequisites

Ensure you have the following installed:

- .NET SDK 8.0+
- PostgreSQL 15.3+
- MongoDB 1.14.1+
- Docker (for containerized deployment)
- Visual Studio Code

### Install Dependencies

Run the following commands in the project root:

```sh
# Restore NuGet packages
dotnet restore
```

### Database Configuration

Update `appsettings.json` with your database connection:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=your_server;Database=SalesDB;User Id=your_user;Password=your_password;"
  }
}
```

### Running the Application

Use the following command to run the API:

```sh
dotnet run --project src/Sales.API
```

### Running Tests

Execute unit and integration tests:

```sh
dotnet test
```

## Docker Setup

### Build and Run with Docker Compose

Run the following command to start the services:

```sh
docker-compose up --build
```

### Docker Compose Configuration

```yaml
version: '3.9'
services:
  app:
    build: .
    ports:
      - "5000:5000"
    volumes:
      - .:/app
    environment:
      - DATABASE_URL=postgresql://postgres:postgres@db:5432/sales_database
  db:
    image: postgres:15.3
    ports:
      - "5432:5432"
    environment:
      POSTGRES_USER: postgres
      POSTGRES_PASSWORD: postgres
      POSTGRES_DB: sales_database
  mongo:
    image: mongo
    ports:
      - "27017:27017"
    environment:
      MONGO_INITDB_ROOT_USERNAME: root
      MONGO_INITDB_ROOT_PASSWORD: root
```

### Swagger API Documentation

Access the API documentation at:

```plaintext
http://localhost:5000/swagger
```

## Multi-Cloud, On-Premises, and Data Center Environment List

- Development: `environment-dev`
- Staging: `environment-hml`
- QA: `environment-qa`
- Production: `environment-prod`
