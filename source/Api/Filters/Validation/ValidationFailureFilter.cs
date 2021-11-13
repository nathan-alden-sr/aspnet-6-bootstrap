using Company.Product.WebApi.Api.Results;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Company.Product.WebApi.Api.Filters.Validation;

public class ValidationFailureFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ModelState.IsValid)
        {
            return;
        }

        var hostEnvironment = context.HttpContext.RequestServices.GetRequiredService<IHostEnvironment>();
        var errors =
            context
                .ModelState.Values
                .SelectMany(a => a.Errors)
                .Select(
                    a =>
                        hostEnvironment.IsDevelopment()
                            ? new ValidationFailureWithExceptionsResultData(a.ErrorMessage, a.Exception)
                            : new ValidationFailureResultData(a.ErrorMessage));

        context.Result = ActionResult.BadRequest("Request model is invalid.", errors);
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}
