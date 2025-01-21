# 📌 Sales API - Backend Implementation

## 🏆 Overview

The **123Sales** system is divided into multiple domains, including **Inventory, CRM (Customer), and Sales**. As a Sales team developer, you need to implement a **prototype API** for managing sales.

Following **Domain-Driven Design (DDD)** principles, all references to entities from other domains should follow the **External Identities** pattern, with **data denormalization** for descriptive attributes.

---

## 🎯 Business Rules

- **Progressive Discount**:
  - **Purchases of more than 4 identical items** receive a **10% discount**.
  - **Purchases between 10 and 20 identical items** receive a **20% discount**.
  - **Sales of more than 20 identical items are not allowed**.
  - **Purchases below 4 items** cannot have a discount.

---

## ⚙️ API Architecture (Layered Model)

The API follows a **well-defined modular architecture**, based on **Clean Architecture** and **DDD** principles:

```plaintext
.github/                  # GitHub Actions configuration and workflows
.vscode/                  # Visual Studio Code-specific configurations
src/                      # Main source code folder
    Sales.API/            # API Layer (Controllers, Middlewares, Filters)
    Sales.Application/    # Application Layer (Use Cases, DTOs, Interfaces)
    Sales.Domain/         # Domain Layer (Entities, Aggregates, Domain Services)
    Sales.Infrastructure/ # Infrastructure Layer (Repositories, Database, External Integrations)
    Sales.Tests/          # Unit and Integration tests
docs/                     # Project documentation
scripts/                  # Automation scripts
.env                      # Environment variables file
docker-compose.yml        # Docker Compose for multi-container configuration
Dockerfile                # Dockerfile for building the API
README.md                 # Project description
```

---

## 🚀 Technologies Used

The API leverages the following technologies and best practices:

### 📌 Backend and Infrastructure:
- **.NET Core 8** for API development
- **PostgreSQL 15.3** as the relational database
- **MongoDB 1.14.1** as the NoSQL database
- **Docker & Docker Compose** for containerization and deployment

### 🔍 Security and Authentication:
- **JWT Authentication** for secure authentication
- **Serilog** for structured logging

### 📌 Best Development Practices:
- **Layered Architecture**: API, Application, Domain, Infrastructure
- **Git Flow Workflow** for structured branch management
- **Semantic Commit Messages** for clean versioning
- **Code Principles**: REST API, Clean Code, SOLID, DRY, YAGNI, Object Calisthenics

### 🧪 Automated Testing:
- **XUnit** for unit tests
- **FluentAssertions** for fluent assertions
- **Bogus** for data generation
- **NSubstitute** for mocking
- **Test Containers** (optional) for integration testing using containers

---

## 📦 Installation and Configuration

### 1️⃣ **Prerequisites**
Before starting, make sure you have the following software installed:

- **.NET SDK 8.0+**
- **PostgreSQL 15.3+**
- **MongoDB 1.14.1+**
- **Docker** (for containerized deployment)
- **Visual Studio Code or Rider** (optional)

### 2️⃣ **Install Dependencies**
Run the following commands to install the required packages:

```sh
# Install logging with Serilog
dotnet add package Serilog.AspNetCore

# Install testing frameworks
dotnet add package xunit
dotnet add package FluentAssertions
dotnet add package Bogus
dotnet add package NSubstitute
dotnet add package Microsoft.NET.Test.Sdk

# Install test containers
dotnet add package TestContainers
```

### 3️⃣ **Database Configuration**
Edit the \`appsettings.json\` file with your PostgreSQL database settings:

```json
{
  "ConnectionStrings": {
    "SalesDB": "Host=localhost;Port=5432;Database=SalesDB;Username=postgres;Password=postgres;"
  }
}
```

---

## 🐳 Docker Setup

### **1️⃣ Verify Dependencies**
Before running the API in Docker, verify if .NET restore works:

```sh
dotnet restore src/Sales.API/Sales.API.csproj
```

If necessary, clear the cache and try again:

```sh
dotnet nuget locals all --clear
dotnet restore src/Sales.API/Sales.API.csproj
```

### **2️⃣ Build and Run the API in Docker**
```sh
docker build -t sales-api .
docker run -d -p 5000:5000 --name sales-container sales-api
```

To validate the build:
```sh
docker run --rm -it sales-api ls -R /app
```

If an error occurs, rebuild without cache:
```sh
docker system prune -af
docker build --no-cache -t sales-api .
```

### **3️⃣ Docker Compose Configuration**
Create a \`docker-compose.yml\` file:

```yaml
version: '3.9'
services:
  app:
    build: .
    ports:
      - "5000:5000"
    environment:
      - DATABASE_URL=postgresql://postgres:postgres@db:5432/SalesDB
  db:
    image: postgres:15.3
    ports:
      - "5432:5432"
    environment:
      POSTGRES_USER: postgres
      POSTGRES_PASSWORD: postgres
      POSTGRES_DB: SalesDB
  mongo:
    image: mongo
    ports:
      - "27017:27017"
    environment:
      MONGO_INITDB_ROOT_USERNAME: root
      MONGO_INITDB_ROOT_PASSWORD: root
```

To start the containers:
```sh
docker-compose up -d
```

---

## 🛠️ Automated Testing

Run unit and integration tests:
```sh
dotnet test
```

---

## 🔗 API Access

### **Swagger**
Access the Swagger documentation here:
[Swagger Local](http://localhost:5000/index.html)

- Localhost: [Swagger local machine](http://localhost:8080/swagger-ui/index.html)
  ![img](docs/__img/img.png)

---

## 📌 Environments and Deployment

The system can be deployed in multiple environments:

- **Development**: `environment-dev`
- **Staging**: `environment-hml`
- **QA**: `environment-qa`
- **Production**: `environment-prod`

---

## 📢 Conclusion

This README provides a **detailed overview** of the **Sales API**, from **architecture to practical execution**. Now you have a solid base to start development, testing, and deployment 🚀.

If you need any modifications or improvements, let me know! 😉
