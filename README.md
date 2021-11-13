# Bootstrapped ASP.NET Core 6 Web API Solution

This repository contains a bootstrapped ASP.NET Core 6 Web API solution.

## Changes to make after cloning

Be sure to make the following changes once you clone the repository:

1. Set the `<UserSecretsId>` property in `/source/Api/Company.Product.WebApi.Api.csproj` to a unique GUID.
2. Set the `<UserSecretsId>` property in `/source/Api/Company.Product.WebApi.ScheduledTasks.csproj` to a unique GUID.
3. Rename any occurrences of `Company` or `Product` in all file contents and filenames.
4. Carefully look through `/Directory.Build.props` for property values to change. Some values to change are:
    - `<RepositoryRootUrl>`
    - `<VersionPrefix>`
    - `<VersionSuffix>`
    - `<Company>`
    - `<Product>`
    - `<Authors>`
    - `<Copyright>`

## Understand the code

The solution is opinionated about several things. It's important to understand the purpose of each class before using the solution.

## Before running the API application

You must do a few things before successfully running the API application:

- Run the `create-network.ps1` script (see [Scripts](#scripts))
- Run the `run-postgresql.ps1` script (see [Scripts](#scripts))
- Run the `run-seq.ps1` script (see [Scripts](#scripts))
- Run the `add-postgresql-connection-string.ps1` script (see [Scripts](#scripts))
  - Be sure to run the script twice: once with `-Project Api` and once with `-Project ScheduledTasks`

## Notable features

### Custom fluent result API

A common need is to wrap all responses in a response body envelope. This is simple enough within controller action methods but gets more challenging outside of controller contexts (e.g., validation filters and unhandled exception handlers). Additionally, Microsoft inexplicably does not implement `IActionResult` for every HTTP status code.

I wrote a complete replacement for Microsoft's controller helper methods like `Ok()` that I call _results_. With a simple fluent interface, you can construct an enveloped response. For action methods, use the [`ActionResult`](source/Api/Results/ActionResult.cs) class (see [`HealthController.cs`](source/Api/Controllers/Health/HealthController.cs)). For outside of controller contexts where you only have access to `HttpContext`, use the [`HttpResult`](source/Api/Results/HttpResult.cs) class (see [`UnhandledExceptionHandler.cs`](source/Api/ExceptionHandlers/UnhandledExceptionHandler.cs)).

### Swashbuckle customizations

Swashbuckle has some annoying behavior that I was intent on fixing.

Swashbuckle does not correctly honor non-null reference types when the [nullable reference type](https://docs.microsoft.com/en-us/dotnet/csharp/nullable-references) feature is enabled. The [NullableSchemaFilter](source/Api/Swashbuckle/NullableSchemaFilter.cs) class corrects this behavior.

Swashbuckle does not render generic model type names correctly. The [TitleFilter](source/Api/Swashbuckle/TitleFilter.cs) class corrects this behavior.

I wrote a couple of attributes that wrap common response types to reduce boilerplate.

### Scheduled tasks app

The Scheduled Tasks console application can be used to execute jobs at certain times.

### Seq

Seq is an excellent log sink. Both the `Api` and `ScheduledTasks` projects are configured to use Seq as a Serilog sink.

## Frameworks and libraries

| Framework or Library | Purpose |
| -------------------- | ------- |
| [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/) | [Object-relational mapping](https://en.wikipedia.org/wiki/Object%E2%80%93relational_mapping)
| [Fluent Assertions](https://fluentassertions.com/) | Test assertion |
| [Fluent Validation](https://fluentvalidation.net/) | Model validation |
| [Moq](https://github.com/moq/moq) | Mocking |
| [NodaTime](https://nodatime.org/) | Date/time |
| [Quartz.NET](https://www.quartz-scheduler.net/) | Scheduling |
| [Serilog](https://serilog.net/) (console and Seq sinks) | Logging |
| [Swashbuckle](https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-6.0&tabs=visual-studio) | [OpenAPI](https://swagger.io/) documentation |
| [TerraFX](https://github.com/terrafx/terrafx) | Utilities |
| [xUnit](https://xunit.net/) | Test harness |

## Scripts

| Script | Purpose |
| ------ | ------- |
| `/scripts/dev/docker/create-network.ps1` | Creates a Docker network |
| `/scripts/dev/docker/run-postgresql.ps1` | Runs a PostgreSQL Docker container |
| `/scripts/dev/docker/run-seq.ps1` | Runs a Seq SQL Docker container |
| `/scripts/dev/secrets/add-postgresql-connection-string.ps1` | Adds a PostgreSQL connection string using `dotnet user-secrets` |
| `/scripts/dev/security/dotnet-devcerts.ps1` | .NET HTTPS development certificate management |

## Versioning

The application is versioned using a tweaked [versioning scheme from .NET Arcade](https://github.com/dotnet/arcade/blob/main/Documentation/CorePackages/Versioning.md).

## Debugging

The solution supports debugging in either a console application or Docker.
