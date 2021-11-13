using Company.Product.WebApi.Api.Results;
using Microsoft.AspNetCore.Diagnostics;

namespace Company.Product.WebApi.Api.ExceptionHandlers;

public static class UnhandledExceptionHandler
{
    public static async Task Handle(HttpContext context)
    {
        ThrowIfNull(context, nameof(context));

        var hostEnvironment = context.RequestServices.GetRequiredService<IHostEnvironment>();
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
            HttpResult
                .InternalServerError()
                .AsStandardJson(
                    hostEnvironment.IsDevelopment()
                        ? CreateUnhandledExceptionResponse(exceptionHandlerFeature.Error)
                        : null,
                    "An unhandled exception has occurred.");

        await result.ExecuteResultAsync(context);
    }
}
