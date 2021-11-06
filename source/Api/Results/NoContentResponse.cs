using Microsoft.AspNetCore.Mvc;

namespace Company.Product.WebApi.Api.Results;

public class NoContentResponse : ActionResult
{
    private readonly Action<ActionContext>? _formatter;

    public NoContentResponse(Action<ActionContext>? formatter = null)
    {
        _formatter = formatter;
    }

    public override void ExecuteResult(ActionContext context)
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status204NoContent;

        _formatter?.Invoke(context);

        base.ExecuteResult(context);
    }

    public override async Task ExecuteResultAsync(ActionContext context)
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status204NoContent;

        _formatter?.Invoke(context);

        await base.ExecuteResultAsync(context);
    }
}