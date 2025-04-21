<p align="center" style="font-weight: bold;">
    <!--<img alt="logo" src="https://user-images.githubusercontent.com/5068797/161198565-ac18c5ac-c0d9-4669-9568-b2009e944d77.png#gh-light-mode-only" />
    <img alt="logo" src="https://user-images.githubusercontent.com/5068797/161364257-0c1d81f6-62ac-4192-93f8-836b4ce0fd06.png#gh-dark-mode-only" />-->
    Logo aqui
</p>

- [Quickstart](#quickstart)
- [Description](#description)
- [Technologies](#technologies)
- [Architecture](#architecture)
- [Bugs and Feature Requests](#bugs-and-feature-requests)
- [Contribution](#contribution)
- [Creator](#creator)
- [Acknowledgements](#acknowledgements)
- [Rights and License](#rights-and-license)

## Quick Start

<p align="center" style="font-weight: bold;">
<!--<img alt="FrontEnd" src="https://user-images.githubusercontent.com/5068797/164293734-a72fbeeb-0965-4413-a624-29e1c56c25df.png" />-->
Front-End Image here
</p>

## Description

- The project was developed with the intention of being a base repository for projects in .NET 8, using the best development practices.
- The project is an example of an e-commerce application, with catalog, sales and payments functionalities.
- The project is a web application, with a RESTful API and a front-end in Vue.js.
- The project is an example of a microservices application, with an event-based architecture.
- The project is an example of a rich domain application, with complex business rules.
- The project is an example of a CQRS application, with separate commands and queries.
- The project is an example of a DDD application, with a layer-based architecture.
- The project is an example of a Clean Architecture application, with an architecture based on ports and adapters.
- The project is an example of a Clean Code application, with clean and readable code. - The project is an example of a SOLID application, with object-oriented design principles.
- The project is an example of a TDD application, with automated tests.

## List of useful GIT commands

See <a href="https://github.com/alexandervieira/repositorio-base/blob/master/git.md">here</a>.

## Photo Gallery

See <a href="https://github.com/alexandervieira/NTTData.NET8.Developer.Testing/blob/master/.doc/galery.md">aqui</a>.

## Notes

### Users used in the tests

{
  "email": "admin@domain.com",
  "password": "Admin@123"
}

{
  "email": "user1@teste.com",
  "password": "Teste@123"
}

{
  "email": "user2@teste.com",
  "password": "Teste@123"
}


{
  "email": "user3@teste.com",
  "password": "Teste@123"
}

## Technologies

### Implemented Components

[JAVASCRIPT__BADGE]: https://img.shields.io/badge/Javascript-000?style=for-the-badge&logo=javascript
[TYPESCRIPT__BADGE]: https://img.shields.io/badge/typescript-D4FAFF?style=for-the-badge&logo=typescript
[EXPRESS__BADGE]: https://img.shields.io/badge/express-005CFE?style=for-the-badge&logo=express
[VUE__BADGE]: https://img.shields.io/badge/VueJS-fff?style=for-the-badge&logo=vue
[NEST__BADGE]: https://img.shields.io/badge/nest-7026b9?style=for-the-badge&logo=nest
[GRAPHQL__BADGE]: https://img.shields.io/badge/GraphQL-e10098?style=for-the-badge&logo=graphql
[JAVA_BADGE]:https://img.shields.io/badge/java-%23ED8B00.svg?style=for-the-badge&logo=openjdk&logoColor=white
[SPRING_BADGE]: https://img.shields.io/badge/spring-%236DB33F.svg?style=for-the-badge&logo=spring&logoColor=white
[MONGO_BADGE]:https://img.shields.io/badge/MongoDB-%234ea94b.svg?style=for-the-badge&logo=mongodb&logoColor=white
[AWS_BADGE]:https://img.shields.io/badge/AWS-%23FF9900.svg?style=for-the-badge&logo=amazon-aws&logoColor=white
[DOTNET_BADGE]:https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white
[AZURE_BADGE]:https://img.shields.io/badge/azure-%230072C6.svg?style=for-the-badge&logo=microsoftazure&logoColor=white
[CSHARP_BADGE]:https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=csharp&logoColor=white
[SWAGGER_BADGE]:https://img.shields.io/badge/-Swagger-%23Clojure?style=for-the-badge&logo=swagger&logoColor=white
[SQLSERVER_BADGE]:https://img.shields.io/badge/Microsoft%20SQL%20Server-CC2927?style=for-the-badge&logo=microsoft%20sql%20server&logoColor=white
[POSTGRESQL_BADGE]:https://img.shields.io/badge/postgres-%23316192.svg?style=for-the-badge&logo=postgresql&logoColor=white
[REDIS_BADGE]:https://img.shields.io/badge/redis-%23DD0031.svg?style=for-the-badge&logo=redis&logoColor=white

![.Net][DOTNET_BADGE]
![C#][CSHARP_BADGE]
![Swagger][SWAGGER_BADGE]
![POSTGRESQL_BADGE]
![REDIS_BADGE]

- .NET 8   
    - ASP.NET WebApi    
    - ASP.NET Identity Core
    - Roles
    - Claims    
    - JWT    
    - EntityFramework Core 8

- Components / Services    
    - RabbitMQ
    - EasyNetQ
    - CQRS
    - MediatR
    - Event Storage
    - Docker
    - Refit
    - Retry com Polly
    - Circuit Breaker
    - Fluent Validation
    - FluentAPI
    - APIGateway BFF
    - SOLID
    - Dapper
    - BackgroundService
    - IAggregateRoot
    - UnitOfWork 
    - ExceptionMiddleware
    - ViewComponents
    - Razor Extensions
    - ValidationProblemDetails (RFC7807)
    - Swagger (Documentação API)
    - xUnit
    - Bogus
    - NSubstitute
    - AutoMoq
    - PostgreSQL
    - MongoDB

- Hosting
    - IIS
    - NGINX
    - Docker (with compose)

## Architecture

### Complete architecture implementing the most important concerns:

- Hexagonal Architecture
- Clean Code
- Clean Architecture
- DDD - Domain Driven Design (Layers and Domain Model Pattern)
- Domain Events
- Domain Notification
- Domain Validations
- CQRS (Immediate Consistency)
- Retry Pattern
- Circuit Breaker
- Unit of Work
- Repository
- Specification Pattern
- API Gateway/BFF

---

## Project Structure
```
src/ 
├── Ambev.DeveloperEvaluation.Application/    # Serviços de aplicação, comandos e consultas 
│   ├── Catalog/ 
│   ├── Sales/ 
│   └── Payments/ 
├── Ambev.DeveloperEvaluation.Domain/         # Entidades e interfaces do domínio 
│   ├── Entities/ 
│   ├── Interfaces/ 
│   └── Services/ 
├── Ambev.DeveloperEvaluation.Infrastructure/ # Implementações de infraestrutura 
│   ├── Data/ 
│   ├── Repositories/ 
│   └── Services/ 
├── Ambev.DeveloperEvaluation.WebApi/         # Controladores e configurações da API 
├── Ambev.DeveloperEvaluation.Common/         # Componentes e utilitários compartilhados 
├── Ambev.DeveloperEvaluation.IoC/            # Configuração de injeção de dependência 
├── Ambev.DeveloperEvaluation.ORM/            # Configuração e mapeamento do banco de dados 
├── Ambev.DeveloperEvaluation.Security/       # Gerenciamento de autenticação e autorização 
└── tests/ 
├── Ambev.DeveloperEvaluation.Unit/       # Testes unitários 
├── Ambev.DeveloperEvaluation.Integration/# Testes de integração 
└── Ambev.DeveloperEvaluation.Functional/ # Testes funcionais

```

### The entire application is based on a single solution with X API's and a web application

<p align="center">
    <!--<img alt="read before" src="https://user-images.githubusercontent.com/5068797/161202409-edcf2f38-0714-4de5-927d-1a02be4501ec.png" />-->
    <img alt="read before" src="https://github.com/alexandervieira/NTTData.NET8.Developer.Testing/blob/master/.doc/arquitetura-developer-evoluation.jpg" />
</p>

## Prerequisites
- .NET 8 SDK
- Docker Desktop
- Visual Studio 2022 or VS Code

## Setup & Running

### 1. Database Setup
Run PostgreSQL, MongoDB and Redis using Docker Compose:

```bash
docker-compose up -d
```

### 2. Database Migrations

Migrations performed automatically when running the application for the first time, using Entity Framework Core.
- Added configuration to create the database and apply migrations automatically.

- From the solution root directory:

```bash
# Create new migration

dotnet ef migrations add InitialMigration --startup-project ../Ambev.DeveloperEvaluation.WebApi --context DefaultContext
dotnet ef migrations add InitialIdentity --startup-project ../Ambev.DeveloperEvaluation.WebApi --context AuthDbContext

# Apply migrations

dotnet ef database update --startup-project ../Ambev.DeveloperEvaluation.WebApi --context DefaultContext
dotnet ef database update --startup-project ../Ambev.DeveloperEvaluation.WebApi --context AuthDbContext

```

### 3. Build and Run
```bash
# Restore dependencies
dotnet restore

# Build solution
dotnet build

# Run API
cd src/Ambev.DeveloperEvaluation.WebApi
dotnet run
```

The API will be available at:
- HTTP: http://localhost:5119
- HTTPS: https://localhost:7181
- Swagger UI: https://localhost:7181/swagger
- IIS: http://localhost:44312
- Docker: http://localhost:8080 or https://localhost:8081

### 4. Running Tests
```bash
# Run all tests
dotnet test

# Run specific test project
dotnet test tests/Ambev.DeveloperEvaluation.UnitTests
dotnet test tests/Ambev.DeveloperEvaluation.IntegrationTests
```

## Architecture Overview

### Domain-Driven Design (DDD)
- Bounded Contexts: Catalog, Sales, Payment
- Rich domain models with business rules
- Domain events for cross-context communication

### CQRS Pattern
- Commands: Handle state changes
- Queries: Read-only operations
- MediatR for command/query dispatching

### Event-Driven Architecture
- Domain events for business operations
- Integration events between bounded contexts
- Event handlers for side effects

### Data Storage
- PostgreSQL: Main transactional database
- MongoDB: Product catalog and search
- Redis: Caching and distributed locking

## Business Rules
- Quantity-based discounts:
  - 4+ items: 10% discount
  - 10-20 items: 20% discount
  - Maximum 20 items per product
  - No discounts for quantities below 4

## API Documentation
Available via Swagger UI when running the application.

## Logging
Logs have been enabled to aid in observability.
- Serilog for structured logging
- Logging to console and file
- Different log levels (Information, Warning, Error)
- Logs directory location: logs/log-yyyymmdd.txt

## Security
- Implemented with ASP.NET Identity, using Roles and Claims

## Docker Support
- The solution includes Docker support with multi-stage builds and Docker Compose for local development.

To build and run with Docker:

```bash
# Build Docker images
docker-compose build

# Run all services
docker-compose up -d

# Stop all services
docker-compose down
```

## Database Synchronization and CAP Theorem

The project adopts an **event-driven** approach to synchronize data between the relational database (PostgreSQL) and the NoSQL database (MongoDB). This approach is analyzed based on the **CAP Theorem**:

### CAP Theorem
The CAP theorem states that in distributed systems, it is impossible to guarantee all three properties simultaneously:
1. **Consistency**: All nodes see the same data at the same time.
2. **Availability**: The system responds to all requests, even in case of failures.
3. **Partition Tolerance**: The system continues to function even if communication between nodes fails.

In this project:
- **Consistency**: Not guaranteed immediately, as synchronization between PostgreSQL and MongoDB is eventual.
- **Availability**: Prioritized, as the system continues to function even if synchronization with MongoDB fails.
- **Partition Tolerance**: Supported, as the system uses events to synchronize data, allowing operations to continue during temporary failures.

### Synchronization Flow
1. **Relational Database as the Source of Truth**:
   - PostgreSQL is treated as the primary data source. Write operations (e.g., product creation) are performed first in the relational database.

2. **Event Publishing**:
   - After creating a product in PostgreSQL, a domain event (`ProductCreatedEvent`) is published to notify the creation of a new product.

3. **Eventual Synchronization**:
   - The `ProductEventHandler` listens to the event and synchronizes with MongoDB by adding the category and product to the NoSQL database.

4. **Failure Handling**:
   - If synchronization with MongoDB fails, an exception is thrown, but the operation in PostgreSQL is not rolled back, ensuring availability.

### EF Core `BeginTransaction` Usage

The `BeginTransactionAsync` method from EF Core is used to manage explicit transactions in the relational database. It ensures that multiple operations in PostgreSQL are executed atomically.

#### How It Works
1. **Transaction Start**:
   - The `BeginTransactionAsync` method starts an explicit transaction in the relational database.

2. **Operation Execution**:
   - All operations performed within the `DbContext` scope are treated as part of a single unit of work.

3. **Commit or Rollback**:
   - If all operations succeed, the transaction is committed.
   - In case of failure, the transaction is rolled back, ensuring consistency in the relational database.

#### Limitations
- The `BeginTransaction` does not cover MongoDB, as it does not support distributed transactions with PostgreSQL. This means synchronization between the databases is eventual and not atomic.


## Bugs and Feature Requests
Have a bug or feature request? Please read the [issue guidelines](https://reponame/blob/master/CONTRIBUTING.md) first and search for existing and closed issues. [open a new issue](https://github.com/alexandervieira/repositorio-base/issues).

## Contributing

Please read our [contributing guidelines](https://reponame/blob/master/CONTRIBUTING.md). Instructions for opening issues, coding standards, and development notes are included.

## Creator

- <https://github.com/alexandervsilva>

## Acknowledgements

Thank you for reviewing, sharing, or contributing.

## Rights and License

Code and documentation copyright 2021 by the authors. Code released under the [MIT License](https://github.com/alexandervieira/repositorio-base/blob/master/LICENSE).

<h3>Documentation that may help</h3>

[📝 How to create a Pull Request](https://www.atlassian.com/br/git/tutorials/making-a-pull-request)

[💾 Commit pattern](https://gist.github.com/joshbuchea/6f47e86d2510bce28f8e7f42ae84c716)