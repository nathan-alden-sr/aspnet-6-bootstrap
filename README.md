# Bootstrapped ASP.NET Core 6 Web API Solution

This repository contains a bootstrapped ASP.NET Core 6 Web API solution.

## Changes to make after cloning

Be sure to make the following changes once you clone the repository:

1. Set the `<UserSecretsId>` property in `/source/Api/Company.Product.WebApi.Api.csproj` to a GUID unique to your project.
2. Rename any occurrences of `Company` or `Product` in all file contents and filenames.
3. Carefully look through `/Directory.Build.props` for property values to change. Some values to change are:
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
- Run the `add-connection-string.ps1` script (see [Scripts](#scripts))

## Notable features

### Custom fluent result API

Microsoft inexplicably does not implement `IActionResult` for every HTTP status code. Additionally, developers sometimes want to implement "response body envelopes" where all response bodies follow a certain JSON schema. In this repository, response body envelopes are implemented by the `StandardJsonResult` and `StandardJsonResult<TData>` classes. A fluent API makes it easy to compose these response body envelopes. See the [`HealthController`](source/Api/Controllers/Health/HealthController.cs) class for an example.

## Frameworks and libraries

| Framework or Library | Purpose |
| -------------------- | ------- |
| [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/) | [Object-relational mapping](https://en.wikipedia.org/wiki/Object%E2%80%93relational_mapping)
| [Swashbuckle](https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-6.0&tabs=visual-studio) | [OpenAPI](https://swagger.io/) documentation |
| [NodaTime](https://nodatime.org/) | Date/time |
| [Fluent Validation](https://fluentvalidation.net/) | Model validation |
| [Serilog](https://serilog.net/) (console sink) | Logging |
| [TerraFX](https://github.com/terrafx/terrafx) | Utilities |
| [xUnit](https://xunit.net/) | Test harness |
| [Fluent Assertions](https://fluentassertions.com/) | Test assertion |
| [Moq](https://github.com/moq/moq) | Mocking |

## Scripts

| Script | Purpose |
| ------ | ------- |
| `/scripts/dev/docker/create-network.ps1` | Creates a Docker network |
| `/scripts/dev/docker/run-postgresql.ps1` | Runs a PostgreSQL Docker container |
| `/scripts/dev/secrets/add-connection-string.ps1` | Adds a PostgreSQL connection string using `dotnet user-secrets` |

## Versioning

The application is versioned using a tweaked [versioning scheme from .NET Arcade](https://github.com/dotnet/arcade/blob/main/Documentation/CorePackages/Versioning.md).

## Debugging

The solution supports debugging in either a console application or Docker.
