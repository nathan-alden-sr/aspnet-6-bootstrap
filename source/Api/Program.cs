using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Company.Product.WebApi.Api;
using Company.Product.WebApi.Api.ExceptionHandlers;
using Company.Product.WebApi.Api.Filters.Validation;
using Company.Product.WebApi.Api.Logging;
using Company.Product.WebApi.Api.Swashbuckle;
using Company.Product.WebApi.Common;
using Company.Product.WebApi.Data;
using Company.Product.WebApi.Services;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NodaTime;
using NodaTime.Serialization.SystemTextJson;
using Serilog;
using Swashbuckle.AspNetCore.Filters;
using JsonOptions = Microsoft.AspNetCore.Http.Json.JsonOptions;

// Roslyn incorrectly says the IDE0058 suppression is unnecessary
#pragma warning disable IDE0079
#pragma warning disable IDE0058

var builder = WebApplication.CreateBuilder(args);

/*
 * Helper methods
 */

void ConfigureDbContext<T>(string connectionStringName)
    where T : DbContext
{
    connectionStringName = $"{builder.Environment.EnvironmentName}-{connectionStringName}";

    var connectionString = builder.Configuration.GetConnectionString(connectionStringName);

    if (string.IsNullOrEmpty(connectionString))
    {
        ThrowInvalidOperationException($"A connection string named {connectionStringName} was not found.");
    }

    builder.Services.AddDbContextPool<T>(
        options => options
            .UseNpgsql(connectionString, optionsBuilder => optionsBuilder.UseNodaTime())
            .UseSnakeCaseNamingConvention());
}

/*
 * Secrets
 */

builder.Configuration.AddUserSecrets(Assembly.GetExecutingAssembly());

/*
 * Caching
 */

builder.Services.AddMemoryCache();

/*
 * Registrations
 */

// General-purpose

builder
    .Services
    .AddSingleton<IClock>(SystemClock.Instance)
    .AddScoped<IClockSnapshot, ClockSnapshot>()
    .AddSingleton<IGuidFactory, GuidFactory>();

// Business logic services

// Automatically add all services in the Services namespace that have a matching concrete/interface type
builder.Services.Scan(
    selector => selector
        .FromAssemblyOf<ServicesRootNamespace>()
        .AddClasses(
            filter => filter
                .InNamespaceOf<ServicesRootNamespace>()
                .Where(type => type.Name.EndsWith("Service", StringComparison.Ordinal)))
        .AsMatchingInterface()
        .WithScopedLifetime());

/*
 * Entity Framework Core
 */

ConfigureDbContext<DatabaseContext>("company_product");

/*
 * ASP.NET
 */

// System.Text.Json

// JSON serializer options need to be available both inside and outside of controllers

static void ConfigureJsonSerializerOptions(JsonSerializerOptions options)
{
    options.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
    options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
    options.PropertyNameCaseInsensitive = true;
    options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.WriteIndented = true;
}

builder.Services.Configure<JsonOptions>(options => ConfigureJsonSerializerOptions(options.SerializerOptions));

// Fluent validation

builder.Services.AddFluentValidation(
    options =>
    {
        options.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        options.DisableDataAnnotationsValidation = true;
    });

// Forms

builder.Services.Configure<FormOptions>(options => options.MultipartBodyLengthLimit = 1_000_000);

// Custom 400 responses
builder.Services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);

// Controllers

builder.Services
    .AddControllers(options => options.Filters.Add<ValidationFailureFilter>())
    .AddJsonOptions(options => ConfigureJsonSerializerOptions(options.JsonSerializerOptions))
    .ConfigureApiBehaviorOptions(options => options.SuppressMapClientErrors = true);

// Swagger

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerExamplesFromAssemblies(Assembly.GetExecutingAssembly());
builder.Services.AddSwaggerGen(
    options =>
    {
        options.EnableAnnotations();
        options.ExampleFilters();
        options.SchemaFilter<NullableSchemaFilter>();
        options.SchemaFilter<TitleFilter>();
        options.SwaggerDoc(
            "v1",
            new OpenApiInfo
            {
                Title = "Company Product Web API",
                Version = "v1"
            });
    });

/*
 * Logging
 */

builder.WebHost.UseSerilog(
    (_, configuration) => configuration
        .ReadFrom.Configuration(builder.Configuration)
        .Enrich.With<UtcTimestampEnricher>(),
    true);

/*
 * Kestrel configuration
 */

builder.WebHost.ConfigureKestrel(options => options.AddServerHeader = false);

/*
 * App
 */

var app = builder.Build();

if (app.Environment.IsDeveloper())
{
    app.UseSwagger();
    app.UseSwaggerUI(
        options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Company Product Web API");
            options.DefaultModelExpandDepth(-1);
        });
}

// Custom 500 responses
app.UseExceptionHandler(new ExceptionHandlerOptions { ExceptionHandler = UnhandledExceptionHandler.Handle });

app.UseAuthorization();
app.MapControllers();

app.Run();
