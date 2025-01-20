# Use official .NET SDK image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy only the necessary project files first (for caching dependencies)
COPY src/Sales.API/*.csproj Sales.API/
COPY src/Sales.Application/*.csproj Sales.Application/
COPY src/Sales.Domain/*.csproj Sales.Domain/
COPY src/Sales.Infrastructure/*.csproj Sales.Infrastructure/

# Restore dependencies
WORKDIR /app/Sales.API
RUN dotnet restore "Sales.API.csproj"

# Copy the entire project after restoring dependencies
COPY . .

# Build and publish the application
RUN dotnet publish "Sales.API.csproj" -c Release -o out

# Use lightweight runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/Sales.API/out .

ENTRYPOINT ["dotnet", "Sales.API.dll"]
