using Company.Product.WebApi.Api.Filters.Validation;

namespace Company.Product.WebApi.Api.Results;

public class HttpResult : Result<HttpContext, HttpResult>
{
    public HttpResult(int? statusCode = null, IEnumerable<Func<ContextWrapper<HttpContext>, Task>>? formatters = null)
        : base(statusCode, formatters)
    {
    }

    public override async Task ExecuteResultAsync(ContextWrapper<HttpContext> context)
    {
        ThrowIfNull(context, nameof(context));

        if (StatusCode is null)
        {
            ThrowInvalidOperationException("Status code is required.");
        }

        context.Response.StatusCode = StatusCode.Value;

        foreach (var formatter in Formatters)
        {
            await formatter(context);
        }

        await ExecuteResultInternalAsync(context.Context);
    }

    public async Task ExecuteResultAsync(HttpContext context)
    {
        ThrowIfNull(context, nameof(context));

        await ExecuteResultAsync(new ContextWrapper<HttpContext>(context));
    }

    public StandardJsonHttpResult AsStandardJson() =>
        new(StatusCode, null, Formatters);

    public StandardJsonHttpResult<TData> AsStandardJson<TData>(TData? data) =>
        new(StatusCode, data, null, Formatters);

    public StandardJsonHttpResult<TData> AsStandardJson<TData>(TData? data, string? message) =>
        new(StatusCode, data, message, Formatters);

    protected virtual Task ExecuteResultInternalAsync(HttpContext context) =>
        Task.CompletedTask;

    // 400 Bad Request

    public static StandardJsonHttpResult<IEnumerable<ValidationFailureResultData>> BadRequest(
        string? message,
        IEnumerable<ValidationFailureResultData>? messages) =>
        new(StatusCodes.Status400BadRequest, messages, message);

    public static StandardJsonHttpResult<IEnumerable<ValidationFailureResultData>> BadRequest(
        IEnumerable<ValidationFailureResultData> messages) =>
        new(StatusCodes.Status400BadRequest, messages);

    public static StandardJsonHttpResult<IEnumerable<ValidationFailureResultData>> BadRequest(
        params ValidationFailureResultData[] messages) =>
        new(StatusCodes.Status400BadRequest, messages);

    public static StandardJsonHttpResult<IEnumerable<ValidationFailureResultData>> BadRequest(
        string? message,
        params string[] messages) =>
        BadRequest(message, messages.Select(a => new ValidationFailureResultData(a)));

    public static StandardJsonHttpResult<IEnumerable<ValidationFailureResultData>> BadRequest(params string[] messages) =>
        BadRequest(messages.Select(a => new ValidationFailureResultData(a)));
}
