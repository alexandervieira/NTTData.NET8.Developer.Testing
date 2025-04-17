<p align="center" style="font-weight: bold;">
    <!--<img alt="logo" src="https://user-images.githubusercontent.com/5068797/161198565-ac18c5ac-c0d9-4669-9568-b2009e944d77.png#gh-light-mode-only" />
    <img alt="logo" src="https://user-images.githubusercontent.com/5068797/161364257-0c1d81f6-62ac-4192-93f8-836b4ce0fd06.png#gh-dark-mode-only" />-->
    Logo aqui
</p>

## Perfil

Consulte o meu perfil <a href="https://github.com/alexandervieira/alexandervieira/blob/master/README.md">aqui</a>.

<h1 align="center" style="font-weight: bold;">DeveloperEvaluation 💻</h1>

## Índice

- [Começo rápido](#começo-rápido)
- [Descrição](#descrição)
- [Tecnologias](#tecnologias)
- [Arquitetura](#arquitetura)
- [Erros e solicitações de recursos](#erros-e-solicitações-de-recursos)
- [Contribuição](#contribuição)
- [Criador](#criador)
- [Agradecimentos](#agradecimentos)
- [Direitos e licença](#direitos-e-licença)

## Começo rápido

<p align="center" style="font-weight: bold;">
    <!--<img alt="FrontEnd" src="https://user-images.githubusercontent.com/5068797/164293734-a72fbeeb-0965-4413-a624-29e1c56c25df.png" />-->
    Imagem Front-End aqui
</p>

## Descrição

- Descreva seu projeto aqui

## Lista de comandos úteis do GIT

Consulte <a href="https://github.com/alexandervieira/repositorio-base/blob/master/git.md">aqui</a>.

## Galeria de Fotos

Consulte <a href="https://github.com/alexandervieira/NTTData.NET8.Developer.Testing/blob/master/.doc/galery.md">aqui</a>.


## Tecnologias

### Componentes Implementados

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
![Azure][AZURE_BADGE]
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

## Arquitetura

### Arquitetura completa implementando as preocupações mais importantes:

- Hexagonal Architecture
- Clean Code
- Clean Architecture
- DDD - Domain Driven Design (Layers and Domain Model Pattern)
- Domain Events
- Domain Notification
- Domain Validations
- CQRS (Imediate Consistency)
- Retry Pattern
- Circuit Breaker
- Unit of Work
- Repository
- Specification Pattern
- API Gateway / BFF

---

## Estrutura do Projeto
```
src/
├── Ambev.DeveloperEvaluation.Application/    # Application services, commands and queries
│   ├── Catalog/
│   ├── Sales/
│   └── Payments/
├── Ambev.DeveloperEvaluation.Domain/         # Domain entities and interfaces
│   ├── Entities/
│   ├── Interfaces/
│   └── Services/
├── Ambev.DeveloperEvaluation.Infrastructure/ # Infrastructure implementations
│   ├── Data/
│   ├── Repositories/
│   └── Services/
├── Ambev.DeveloperEvaluation.WebApi/         # API Controllers and configurations
├── Ambev.DeveloperEvaluation.Common/         # Shared components and utilities
└── tests/
    ├── UnitTests/
    └── IntegrationTests/
```

### Toda a aplicação é baseada em uma solução única com X API's e uma aplicação web

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
From the solution root directory:

```bash
# Create new migration
dotnet ef migrations add InitialMigration --project src/Ambev.DeveloperEvaluation.ORM --startup-project src/Ambev.DeveloperEvaluation.WebApi

# Apply migrations
dotnet ef database update --project src/Ambev.DeveloperEvaluation.ORM --startup-project src/Ambev.DeveloperEvaluation.WebApi
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

## Docker Support
The solution includes Docker support with multi-stage builds and Docker Compose for local development.

To build and run with Docker:

```bash
# Build Docker images
docker-compose build

# Run all services
docker-compose up -d

# Stop all services
docker-compose down
```

## Erros e solicitações de recursos
Tem um bug ou uma solicitação de recurso? Leia primeiro as [diretrizes do problema](https://reponame/blob/master/CONTRIBUTING.md)  e pesquise os problemas existentes e encerrados. [abra um novo problema](https://github.com/alexandervieira/repositorio-base/issues).

## Contribuição

Por favor, leia nossas [diretrizes de contribuição](https://reponame/blob/master/CONTRIBUTING.md). Estão incluídas instruções para abrir questões, padrões de codificação e notas sobre o desenvolvimento.

## Criador

- <https://github.com/alexandervsilva>

## Agradecimentos

Obrigado por consultar, divulgar ou contribuir.

## Direitos e licença

Código e documentação com copyright 2021 dos autores. Código divulgado sob a [MIT License](https://github.com/alexandervieira/repositorio-base/blob/master/LICENSE).

<h3>Documentações que podem ajudar</h3>

[📝 Como criar um Pull Request](https://www.atlassian.com/br/git/tutorials/making-a-pull-request)

[💾 Commit pattern](https://gist.github.com/joshbuchea/6f47e86d2510bce28f8e7f42ae84c716)
