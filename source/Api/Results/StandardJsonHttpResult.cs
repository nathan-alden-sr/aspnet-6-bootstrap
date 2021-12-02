using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;

namespace Company.Product.WebApi.Api.Results;

public class StandardJsonHttpResult : HttpResult, IStandardJsonResult<HttpContext>
{
    public StandardJsonHttpResult(
        int? statusCode = null,
        string? message = null,
        IEnumerable<Func<ContextWrapper<HttpContext>, Task>>? formatters = null)
        : base(statusCode, formatters)
    {
        Message = message;
    }

    public string? Message { get; private set; }

    protected sealed override async Task ExecuteResultInternalAsync(HttpContext context)
    {
        ThrowIfNull(context);

        var jsonOptions = context.RequestServices.GetRequiredService<IOptions<JsonOptions>>();

        await context.Response.WriteAsJsonAsync(
            GetStandardJsonAnonymousObject(),
            jsonOptions.Value.SerializerOptions);
    }

    protected virtual object GetStandardJsonAnonymousObject() =>
        new
        {
            Data = (object?)null,
            Message
        };

    public StandardJsonHttpResult<T> WithData<T>(T? data) =>
        new(StatusCode, data, Message, Formatters);

    public StandardJsonHttpResult WithMessage(string? message)
    {
        Message = message;

        return this;
    }
}
