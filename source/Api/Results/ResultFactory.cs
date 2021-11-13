using Microsoft.AspNetCore.Mvc;

namespace Company.Product.WebApi.Api.Results;

public static class ResultFactory
{
    public static TResult Create<TContext, TResult>(
        int? statusCode = null,
        IEnumerable<Func<ContextWrapper<TContext>, Task>>? formatters = null)
        where TContext : class
        where TResult : Result<TContext, TResult>
    {
        if (typeof(TContext) == typeof(ActionContext))
        {
            return (TResult)(object)new ActionResult(
                statusCode,
                formatters?.Select(a => (Func<ContextWrapper<ActionContext>, Task>)a));
        }

        if (typeof(TContext) == typeof(HttpContext))
        {
            return (TResult)(object)new HttpResult(
                statusCode,
                formatters?.Select(a => (Func<ContextWrapper<HttpContext>, Task>)a));
        }

        ThrowArgumentOutOfRangeException(
            $"Context type may only be {nameof(ActionContext)} or {nameof(HttpContext)}.",
            typeof(TContext),
            nameof(TContext));

        return default;
    }
}
