using Company.Product.WebApi.Api.Filters.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Company.Product.WebApi.Api.Results;

public class ActionResult : Result<ActionContext, ActionResult>, IActionResult
{
    public ActionResult(int? statusCode = null, IEnumerable<Func<ContextWrapper<ActionContext>, Task>>? formatters = null)
        : base(statusCode, formatters)
    {
    }

    public async Task ExecuteResultAsync(ActionContext context)
    {
        ThrowIfNull(context, nameof(context));

        await ExecuteResultAsync(new ContextWrapper<ActionContext>(context));
    }

    public override async Task ExecuteResultAsync(ContextWrapper<ActionContext> context)
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

    public StandardJsonActionResult AsStandardJson() =>
        new(StatusCode, null, Formatters);

    public StandardJsonActionResult<TData> AsStandardJson<TData>(TData? data) =>
        new(StatusCode, data, null, Formatters);

    public StandardJsonActionResult<TData> AsStandardJson<TData>(TData? data, string? message) =>
        new(StatusCode, data, message, Formatters);

    protected virtual Task ExecuteResultInternalAsync(ActionContext context) =>
        Task.CompletedTask;

    // 201 Created

    public static ActionResult CreatedAtAction(string? actionName, string? controllerName, object? routeValues) =>
        new ActionResult(StatusCodes.Status201Created)
            .AddFormatter(
                context =>
                {
                    var request = context.Request;
                    var urlHelper =
                        context.RequestServices.GetRequiredService<IUrlHelperFactory>().GetUrlHelper(context.Context);
                    var url =
                        urlHelper.Action(actionName, controllerName, routeValues, request.Scheme, request.Host.ToUriComponent());

                    if (string.IsNullOrEmpty(url))
                    {
                        ThrowInvalidOperationException("No routes matched the supplied values.");
                    }

                    context.Response.Headers.Location = url;

                    return Task.CompletedTask;
                });

    public static ActionResult CreatedAtAction(string? actionName, object? routeValues) =>
        CreatedAtAction(actionName, null, routeValues);

    public static ActionResult CreatedAtAction(string? actionName) =>
        CreatedAtAction(actionName, null, null);

    public static ActionResult CreatedAtRoute(string? routeName, object? routeValues) =>
        new ActionResult(StatusCodes.Status201Created)
            .AddFormatter(
                context =>
                {
                    ThrowIfNull(context, nameof(context));

                    var urlHelper =
                        context.RequestServices.GetRequiredService<IUrlHelperFactory>().GetUrlHelper(context.Context);
                    var url = urlHelper.Link(routeName, routeValues);

                    if (string.IsNullOrEmpty(url))
                    {
                        ThrowInvalidOperationException("No routes matched the supplied values.");
                    }

                    context.Response.Headers.Location = url;

                    return Task.CompletedTask;
                });

    public static ActionResult CreatedAtRoute(string? routeName) =>
        CreatedAtRoute(routeName, null);

    public static ActionResult CreatedAtRoute(object? routeValues) =>
        CreatedAtRoute(null, routeValues);

    // 202 Accepted

    public static ActionResult AcceptedAtAction(string? actionName, string? controllerName, object? routeValues) =>
        new ActionResult(StatusCodes.Status202Accepted)
            .AddFormatter(
                context =>
                {
                    var request = context.Request;
                    var urlHelper =
                        context.RequestServices.GetRequiredService<IUrlHelperFactory>().GetUrlHelper(context.Context);
                    var url =
                        urlHelper.Action(actionName, controllerName, routeValues, request.Scheme, request.Host.ToUriComponent());

                    if (string.IsNullOrEmpty(url))
                    {
                        ThrowInvalidOperationException("No routes matched the supplied values.");
                    }

                    context.Response.Headers.Location = url;

                    return Task.CompletedTask;
                });

    public static ActionResult AcceptedAtAction(string? actionName, object? routeValues) =>
        AcceptedAtAction(actionName, null, routeValues);

    public static ActionResult AcceptedAtAction(string? actionName) =>
        AcceptedAtAction(actionName, null, null);

    public static ActionResult AcceptedAtRoute(string? routeName, object? routeValues) =>
        new ActionResult(StatusCodes.Status202Accepted)
            .AddFormatter(
                context =>
                {
                    ThrowIfNull(context, nameof(context));

                    var urlHelper =
                        context.RequestServices.GetRequiredService<IUrlHelperFactory>().GetUrlHelper(context.Context);
                    var url = urlHelper.Link(routeName, routeValues);

                    if (string.IsNullOrEmpty(url))
                    {
                        ThrowInvalidOperationException("No routes matched the supplied values.");
                    }

                    context.Response.Headers.Location = url;

                    return Task.CompletedTask;
                });

    public static ActionResult AcceptedAtRoute(string? routeName) =>
        AcceptedAtRoute(routeName, null);

    public static ActionResult AcceptedAtRoute(object? routeValues) =>
        AcceptedAtRoute(null, routeValues);

    // 300 Multiple Choices

    public static ActionResult MultipleChoicesWithPreferredAction(
        string? actionName,
        string? controllerName,
        object? routeValues) =>
        new ActionResult(StatusCodes.Status300MultipleChoices)
            .AddFormatter(
                context =>
                {
                    var request = context.Request;
                    var urlHelper =
                        context.RequestServices.GetRequiredService<IUrlHelperFactory>().GetUrlHelper(context.Context);
                    var url =
                        urlHelper.Action(actionName, controllerName, routeValues, request.Scheme, request.Host.ToUriComponent());

                    if (string.IsNullOrEmpty(url))
                    {
                        ThrowInvalidOperationException("No routes matched the supplied values.");
                    }

                    context.Response.Headers.Location = url;

                    return Task.CompletedTask;
                });

    public static ActionResult MultipleChoicesWithPreferredAction(string? actionName, object? routeValues) =>
        MultipleChoicesWithPreferredAction(actionName, null, routeValues);

    public static ActionResult MultipleChoicesWithPreferredAction(string? actionName) =>
        MultipleChoicesWithPreferredAction(actionName, null, null);

    public static ActionResult MultipleChoicesWithPreferredRoute(string? routeName, object? routeValues) =>
        new ActionResult(StatusCodes.Status300MultipleChoices)
            .AddFormatter(
                context =>
                {
                    ThrowIfNull(context, nameof(context));

                    var urlHelper =
                        context.RequestServices.GetRequiredService<IUrlHelperFactory>().GetUrlHelper(context.Context);
                    var url = urlHelper.Link(routeName, routeValues);

                    if (string.IsNullOrEmpty(url))
                    {
                        ThrowInvalidOperationException("No routes matched the supplied values.");
                    }

                    context.Response.Headers.Location = url;

                    return Task.CompletedTask;
                });

    public static ActionResult MultipleChoicesWithPreferredRoute(string? routeName) =>
        MultipleChoicesWithPreferredRoute(routeName, null);

    public static ActionResult MultipleChoicesWithPreferredRoute(object? routeValues) =>
        MultipleChoicesWithPreferredRoute(null, routeValues);

    // 400 Bad Request

    public static StandardJsonActionResult<IEnumerable<ValidationFailureResultData>> BadRequest(
        string? message,
        IEnumerable<ValidationFailureResultData>? messages) =>
        new(StatusCodes.Status400BadRequest, messages, message);

    public static StandardJsonActionResult<IEnumerable<ValidationFailureResultData>> BadRequest(
        IEnumerable<ValidationFailureResultData> messages) =>
        new(StatusCodes.Status400BadRequest, messages);

    public static StandardJsonActionResult<IEnumerable<ValidationFailureResultData>> BadRequest(
        params ValidationFailureResultData[] messages) =>
        new(StatusCodes.Status400BadRequest, messages);

    public static StandardJsonActionResult<IEnumerable<ValidationFailureResultData>> BadRequest(
        string? message,
        params string[] messages) =>
        BadRequest(message, messages.Select(a => new ValidationFailureResultData(a)));

    public static StandardJsonActionResult<IEnumerable<ValidationFailureResultData>> BadRequest(params string[] messages) =>
        BadRequest(messages.Select(a => new ValidationFailureResultData(a)));
}
