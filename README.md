# Sales API - Backend Implementation

## Overview

The 123Vendas system is divided into multiple domains, including Inventory, CRM (Customer), and Sales. As a Sales team developer, you are required to implement a prototype API for managing sales.

Since we follow Domain-Driven Design (DDD), references to entities from other domains should use the **External Identities** pattern, with data denormalization for descriptive attributes.

## Features

The API must provide a complete CRUD implementation for managing sales transactions. It should support:

- Sale ID
- Sale date
- Customer details
- Total sale amount
- Salesperson details
- Products with quantities, unit prices, discounts, and total item prices
- Canceled/Not Canceled status

## Additional Requirements (Optional but Recommended)

Implement event publication for actions like:

- SaleCreated
- SaleUpdated
- SaleCanceled
- ItemCanceled

These events can be logged in the application or integrated with a **Message Broker** (e.g., RabbitMQ or Service Bus), if preferred.

## Business Rules

- Sales with more than 4 identical items: **10% discount**
- Sales with 10 to 20 identical items: **20% discount**
- Cannot sell more than 20 identical items
- Sales with fewer than 4 identical items cannot receive a discount

## Technologies and Best Practices

- **Logging**: Use Serilog
- **Layered Architecture**: API, Domain, and Data separation
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
- .NET SDK 6.0+
- Visual Studio or VS Code
- SQL Server or PostgreSQL
- RabbitMQ (if using event-driven architecture)

### Install Required NuGet Packages
Run the following commands in the project root:
```sh
# Install essential dependencies
dotnet add package Microsoft.EntityFrameworkCore.SqlServer

# Install logging and testing libraries
dotnet add package Serilog.AspNetCore

# Install testing frameworks
dotnet add package xunit

dotnet add package FluentAssertions

dotnet add package Bogus

dotnet add package NSubstitute

dotnet add package TestContainers
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
dotnet run
```

### Running Tests
Execute unit and integration tests:
```sh
dotnet test
```
