using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using JsonOptions = Microsoft.AspNetCore.Http.Json.JsonOptions;

namespace Company.Product.WebApi.Api.Results;

public class StandardJsonActionResult : ActionResult, IStandardJsonResult<ActionContext>
{
    public StandardJsonActionResult(
        int? statusCode = null,
        string? message = null,
        IEnumerable<Func<ContextWrapper<ActionContext>, Task>>? formatters = null)
        : base(statusCode, formatters)
    {
        Message = message;
    }

    public string? Message { get; private set; }

    protected sealed override async Task ExecuteResultInternalAsync(ActionContext context)
    {
        ThrowIfNull(context);

        var jsonOptions = context.HttpContext.RequestServices.GetRequiredService<IOptions<JsonOptions>>();

        await context.HttpContext.Response.WriteAsJsonAsync(
            GetStandardJsonAnonymousObject(),
            jsonOptions.Value.SerializerOptions);
    }

    protected virtual object GetStandardJsonAnonymousObject() =>
        new
        {
            Data = (object?)null,
            Message
        };

    public StandardJsonActionResult<T> WithData<T>(T? data) =>
        new(StatusCode, data, Message, Formatters);

    public StandardJsonActionResult WithMessage(string? message)
    {
        Message = message;

        return this;
    }
}
