using Company.Product.WebApi.Api.Results;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;

namespace Company.Product.WebApi.Api.ExceptionHandlers;

public static class UnhandledExceptionHandler
{
    public static async Task Handle(HttpContext context)
    {
        ThrowIfNull(context, nameof(context));

        var hostEnvironment = context.RequestServices.GetRequiredService<IHostEnvironment>();
        var jsonOptions = context.RequestServices.GetRequiredService<IOptions<JsonOptions>>();
        var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();

        AssertNotNull(exceptionHandlerFeature);

        static UnhandledExceptionResultData? CreateUnhandledExceptionResponse(Exception? exception) =>
            exception is not null
                ? new UnhandledExceptionResultData(
                    exception.Message,
                    exception.StackTrace,
                    CreateUnhandledExceptionResponse(exception.InnerException))
                : null;

        var result =
            Result
                .InternalServerError()
                .AsStandardJson(
                    hostEnvironment.IsDevelopment()
                        ? CreateUnhandledExceptionResponse(exceptionHandlerFeature.Error)
                        : null,
                    "An unhandled exception has occurred.");

        Assert(result.StatusCode is not null);

        context.Response.StatusCode = result.StatusCode.Value;

        await context.Response.WriteAsJsonAsync(new { result.Data, result.Message }, jsonOptions.Value.SerializerOptions);
    }
}
