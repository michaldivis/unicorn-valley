<img src="assets/icon.png" width="200">

# Unicorn Valley

What is this?
> What would happen if you used every "buzzword" design pattern in a single project?\
> That's what this is...

This is a solution an example of a ASP.NET Core Minimal API and a console client following the principles of Clean Architecture, Domain Driver Design, an a few other principles and patterns.

I've made it a challenge to use as many of the "buzzword" patterns as possible. As a result, the usage of some of these might be considered an overkill and unnecesary. But here we are...

## Used principles & patterns

- Clean Architecture
- [Domain Driver Design](https://en.wikipedia.org/wiki/Domain-driven_design)
- [CQRS](https://learn.microsoft.com/en-us/azure/architecture/patterns/cqrs) using [MediatR](https://github.com/jbogard/MediatR)
- Command & Query validation using [<img src="assets/icons/fluent-validation.png" style="vertical-align:middle" height=30>](https://docs.fluentvalidation.net/en/latest/)
- [REPR](https://deviq.com/design-patterns/repr-design-pattern) (Request-Endpoint-Response) using [<img src="assets/icons/fast-endpoints.svg" style="vertical-align:middle" height=30>](https://fast-endpoints.com/) 
- [Domain Events](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/domain-events-design-implementation)
- Result objects (instead of throwing exceptions) using [<img src="assets/icons/fluentresults.png" style="vertical-align:middle" height=30> FluentResults](https://github.com/altmann/FluentResults)
- [Repository pattern](https://dotnettutorials.net/lesson/repository-design-pattern-csharp/#:~:text=What%20is%20the%20Repository%20Design,for%20accessing%20the%20domain%20objects.&text=In%20the%20above%20design%2C%20now,Framework%20data%20context%20class%20directly.)
- [Unit of work](https://dotnettutorials.net/lesson/unit-of-work-csharp-mvc/#:~:text=The%20Unit%20of%20Work%20pattern,or%20fail%20as%20one%20unit.) pattern
- [Value objects](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/implement-value-objects) to represent things like a username or an e-mail address using [ValueOf](https://github.com/mcintyre321/ValueOf)
- using the [Problem details](https://www.rfc-editor.org/rfc/rfc7807) standard for API error responses
- auto-generating API client code using [<img src="assets/icons/refit.png" style="vertical-align:middle" height=30> Refit](https://github.com/reactiveui/refit)

## Project structure

**Core**
- **Domain**: entities, enums, interfaces, domain errors, value objects, types and logic specific to the domain layer
- **Application**: This layer contains all application logic. It is dependent on the domain layer, but has no dependencies on any other layer or project. This layer only contains commands, queries, and their handlers.
- **Infrastructure**: This layer contains classes for accessing external resources such as file systems, web services, smtp, and so on. In this particular case, it contains the EF Core db context and repository implementations.
- **WebAPI**: This layer is a REST API. This layer depends on both the Application and Infrastructure layers, however, the dependency on Infrastructure is only to support dependency injection. Therefore only Program.cs should reference Infrastructure.

**Client**
- **ApiClient**: API client library. Contains API contracts and the Refit API interface. This library can be used by any client app (console/WPF)
- **ConsoleClient**: A console client. Uses the *ApiClient* library to call the API.

## DDD Entities & Aggregate root
This project has a rich domain model. The domain models are responsible for all (or most of) the domain logic.

Example: [Meeting.cs](src/Domain/Entities/Meeting.cs)

## CQRS & validation
The Application layer contains commands and queries. Both commands and queries can be validated using FluentValidation.

Example command: [CreateUserCommand.cs](src/Application/Users/Commands/CreateUserCommand.cs)\
Example command validator: [CreateUserCommandValidator.cs](src/Application/Users/Commands/CreateUserCommandValidator.cs)\
Example command handler: [CreateUserCommandHandler.cs](src/Application/Users/Commands/CreateUserCommandHandler.cs)

## REPR using Fast Endpoints
The minimal API is implemented using the [Fast Endpoints](https://fast-endpoints.com/) library. There's one file per endpoint. 

Example: [Create User Endpoint.cs](src/WebAPI/Endpoints/Users/Create.cs).

## Result objects
I'm using result objects to return complex results (success/failure + success/error messages).

A domain method, such a the `Create` user method will return a `Result<User>`. This result object will either contain a valid `User` instance of an error message.

The result object is (in case of an error) later converted into an API problem details response [here](src/WebAPI/Utils/ResponseUtils.cs).

The same result object is also logged (to a rich, structured log) [here](src/WebAPI/Services/ResultHandler.cs).