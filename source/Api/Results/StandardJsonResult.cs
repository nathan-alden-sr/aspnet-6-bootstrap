using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using JsonOptions = Microsoft.AspNetCore.Http.Json.JsonOptions;

namespace Company.Product.WebApi.Api.Results;

public class StandardJsonResult : Result, IStandardJsonResult
{
    public StandardJsonResult(int? statusCode = null, string? message = null, IEnumerable<Action<ActionContext>>? formatters = null) : base(statusCode)
    {
        Message = message;
    }

    public string? Message { get; private set; }

    protected sealed override async Task ExecuteResultInternalAsync(ActionContext context)
    {
        ThrowIfNull(context, nameof(context));

        var jsonOptions = context.HttpContext.RequestServices.GetRequiredService<IOptions<JsonOptions>>();

        await context.HttpContext.Response.WriteAsJsonAsync(GetStandardJsonAnonymousObject(), jsonOptions.Value.SerializerOptions);
    }

    protected virtual object GetStandardJsonAnonymousObject() =>
        new
        {
            Data = (object?)null,
            Message
        };

    public StandardJsonResult<T> WithData<T>(T? data) =>
        new(StatusCode, data, Message, Formatters);

    public StandardJsonResult WithMessage(string? message)
    {
        Message = message;

        return this;
    }
}