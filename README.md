# Contact List API

A clean, modular, and testable ASP.NET Core 10 Web API designed for managing personal contacts.Built as a practice project for the Hasin Company hiring task, this application follows Clean Architecture, implements CQRS with MediatR, and leverages both EF Core and Dapper for data access.

---

## Tech Stack

* **.NET 10**
* **ASP.NET Core Web API**
* **C#**
* **MediatR**
* **Dapper**
* **SQL Server**
* **JWT Bearer Authentication**
* **Swagger / OpenAPI**
* **Docker**
* **Clean Architecture principles**
* **CQRS**
* **Repository Pattern**
* **Result Pattern**
* **Value Objects**
* **DDD**
* **Global Exception Handling**

---

## Project Overview

The main purpose of this project is to provide a simple but well-structured **Contact List API**.

The system supports:

* User registration
* User authentication
* JWT token generation
* Creating contacts
* Retrieving contacts
* Retrieving a contact by ID
* Updating contacts
* Soft deleting contacts

Each contact belongs to a specific user, and authenticated users can only access their own contacts.
The `User` domain exists primarily to support authentication, ownership, and authorization of contacts.

---

## Architecture

The solution follows Clean Architecture principles with clear separation of responsibilities.

```text
Solution
│
├── Domain
│   ├── Entities
│   ├── ValueObjects
│   └── Domain Rules
│
├── Application
│   ├── Features
│   ├── DTOs
│   ├── Contracts
│
├── Infrastructure
│   ├── Persistence
│   │   ├── Dapper
│   │   ├── Repositories
│   │   └── Database
│   ├── Security
│   └── External Services
|
|
│── ContactList.DI
│   └── DependencyContainerServices
|
|
|
└── API
    ├── Controllers
    ├── Middleware
    ├── Configuration
    └── Program.cs
```

### Domain

Contains the core business entities and domain rules.

Main entities:

* `User`
* `Contact`

Value Objects:

* `Email`
* `PhoneNumber`

Domain entities use encapsulation through private setters and domain methods instead of exposing unrestricted state mutation.

---

### Application

Contains application use cases and business orchestration.

The project uses **CQRS with MediatR**.

Examples:

```text
RegisterUserCommand
CreateContactCommand
UpdateContactCommand
DeleteContactCommand
```

Application code depends on abstractions rather than database-specific implementations.

---

### Infrastructure

Contains implementation details such as:

* Dapper database access
* Repository implementations
* Database connection management
* JWT token generation
* Persistence models

Database-specific read models are kept inside Infrastructure.

---

### API

The API layer is responsible for:

* HTTP endpoints
* Authentication / Authorization
* Swagger
* Global exception handling
* Dependency Injection
* Application startup

---

# Contact Management

Contact management is the primary functionality of the application.

Each contact belongs to a specific user.

A contact contains information such as:

```text
FirstName
LastName
PhoneNumber
Tag
UserId
```

Supported operations include:

### Create Contact

Creates a new contact for the authenticated user.

### Get Contacts

Returns the contacts belonging to the authenticated user.

### Get Contact

Retrieves a specific contact while validating its ownership.

### Update Contact

Updates an existing contact belonging to the authenticated user.

### Delete Contact

Uses soft deletion instead of physically removing the record.

---

# Authentication

The API uses **JWT Bearer Authentication**.

The authentication flow is:

```text
Register
   ↓
User Created
   ↓
Login
   ↓
Credentials Validated
   ↓
JWT Generated
   ↓
Client
```

The JWT contains claims such as:

```text
sub
nameidentifier
email
username
```

Protected endpoints require:

```http
Authorization: Bearer <token>
```

---

# Authorization and Contact Ownership

A user must be authenticated to access contact endpoints.

Contact queries include the authenticated user's ID.

For example:

```sql
WHERE Id = @Id
  AND UserId = @UserId
  AND IsDeleted = 0
```

This ensures that a user cannot retrieve, modify, or delete another user's contact simply by knowing its ID.

---

# Soft Delete

Contacts are not physically removed from the database.

Instead:

```text
IsDeleted = true
```

is used to mark a contact as deleted.

Normal queries automatically exclude deleted records.

---

# Result Pattern

The application uses a custom Result Pattern for consistent application responses.

For operations that return data:

```csharp
CustomResult<ContactDTO>
```

Example:

```csharp
return CustomResult<ContactDTO>.SuccessResult(contactDto);
```

For operations without a response payload:

```csharp
CustomResult
```

Example:

```csharp
return CustomResult.SuccessResult();
```

This avoids returning meaningless values such as:

```csharp
CustomResult<string>
```

for operations that do not actually return data.

---

# Global Exception Handling

Unhandled exceptions are handled centrally through:

```text
GlobalExceptionMiddleware
```

Instead of adding broad `try/catch` blocks to every handler and repository, exceptions are allowed to propagate to the global exception handler.

The middleware is responsible for:

* Logging unhandled exceptions
* Mapping exceptions to HTTP status codes
* Returning consistent error responses
* Preventing internal exception details from being exposed to clients

---

# Database

The project currently uses **SQL Server** with Dapper for database access.
It was better to call stored procedures for command actions and Query database Views intead of raw T-SQL queries, but for simplifying code base I used raw T-SQL command and queries.
For this project, the application automatically creates the database schema when the API starts using:

```csharp
await dbContext.Database.EnsureCreatedAsync();
```

> This automatic database initialization is intentionally used for this project/demo setup. In production systems, database schema changes should normally be managed through a controlled migration and deployment process.

---

# Configuration

Update the connection string and JWT configuration in:

```text
appsettings.json
```

Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=ContactListDb;User Id=sa;Password=YourPassword;TrustServerCertificate=True;"
  },

  "JwtSettings": {
    "SecretKey": "your-super-secret-key-at-least-32-characters",
    "Issuer": "ContactListApi",
    "Audience": "ContactListClient",
    "ExpiryMinutes": 60
  }
}
```

For real deployments, secrets should not be committed to source control.

Use environment variables, Secret Manager, or a dedicated secret-management solution for sensitive configuration.

---

# Running the Project

There are two main ways to run the application:

1. Run directly from source code
2. Run using Docker

---

## 1. Run From Source Code

### Prerequisites

Install:

* .NET 10 SDK
* SQL Server

Verify the .NET installation:

```bash
dotnet --version
```

Clone the repository:

```bash
git clone <repository-url>
```

Navigate to the solution directory:

```bash
cd <solution-directory>
```

Restore dependencies:

```bash
dotnet restore
```

Build the solution:

```bash
dotnet build
```

Configure the SQL Server connection string in:

```text
appsettings.json
```

Run the API project:

```bash
dotnet run --project <ApiProjectPath>
```

For example:

```bash
dotnet run --project src/Api
```

The API will start on the configured HTTP/HTTPS ports.

Swagger will be available at:

```text
/swagger
```

For example:

```text
https://localhost:xxxx/swagger
```

---

# Running With Visual Studio

Open the solution:

```text
*.sln
```

Then:

1. Restore NuGet packages.
2. Set the API project as the startup project.
3. Verify the SQL Server connection string.
4. Run the application using `F5` or `Ctrl + F5`.

Swagger will be available in the browser.

---

# Running With Docker

### Prerequisites

Install:

* Docker Desktop
* Docker Engine

Verify Docker:

```bash
docker --version
```

---

## Build the Docker Image

From the solution directory:

```bash
docker build -t contact-list-api .
```

If the Dockerfile is located somewhere else:

```bash
docker build -t contact-list-api -f path/to/Dockerfile .
```

---

## Run the Container

```bash
docker run -d \
  --name contact-list-api \
  -p 8080:8080 \
  contact-list-api
```

The API will be available at:

```text
http://localhost:8080
```

Swagger:

```text
http://localhost:8080/swagger
```

---

# Docker With Environment Variables

Configuration can be provided through environment variables.

Example:

```bash
docker run -d \
  --name contact-list-api \
  -p 8080:8080 \
  -e ConnectionStrings__DefaultConnection="Server=host.docker.internal;Database=ContactListDb;User Id=sa;Password=YourPassword;TrustServerCertificate=True;" \
  -e JwtSettings__SecretKey="your-super-secret-key-at-least-32-characters" \
  -e JwtSettings__Issuer="ContactListApi" \
  -e JwtSettings__Audience="ContactListClient" \
  -e JwtSettings__ExpiryMinutes="60" \
  contact-list-api
```

# Development Notes

The project intentionally avoids unnecessary abstractions and patterns.

### Manual Mapping

DTOs, persistence models, and domain entities are mapped explicitly.

Example:

```csharp
var contactDto = new ContactDTO
{
    Id = contact.Id,
    FirstName = contact.FirstName,
    LastName = contact.LastName,
    PhoneNumber = contact.PhoneNumber.Value,
    Tag = contact.Tag,
    UserId = contact.UserId
};
```

No AutoMapper is used.

---

### Dapper

Dapper is used for database operations to provide explicit SQL and control over database queries.

Database-specific read models are kept inside Infrastructure.

---

### Repository Pattern

Application code depends on repository abstractions:

```text
IUserRepository
IContactRepository
```

while implementations remain inside Infrastructure.

---

### Value Objects

Domain-specific values are represented using Value Objects:

```text
Email
PhoneNumber
```

This allows domain validation and behavior to remain close to the values they represent.

---

# Production Considerations

This project is primarily a practical demonstration and is not intended to represent a complete production deployment.

Before production deployment, consider adding or strengthening:

* Database migrations and controlled schema deployment
* Production secret management
* Refresh tokens
* Token revocation strategy
* Role/permission authorization
* Rate limiting
* Request validation
* Structured logging
* Health checks
* Distributed tracing
* Automated unit/integration tests
* Optimistic concurrency
* Database unique constraints
* Transaction management for multi-step use cases
* Production Docker configuration
* CI/CD pipeline
* HTTPS and certificate management

---

# License

This project is intended for educational and demonstration purposes.
