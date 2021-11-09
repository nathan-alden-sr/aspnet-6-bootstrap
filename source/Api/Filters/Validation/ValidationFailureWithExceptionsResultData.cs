namespace Company.Product.WebApi.Api.Filters.Validation;

public sealed class ValidationFailureWithExceptionsResultData : ValidationFailureResultData
{
    public ValidationFailureWithExceptionsResultData(string message, Exception? exception) : base(message)
    {
        Exception = CreateExceptionData(exception);
    }

    public ValidationFailureExceptionData? Exception { get; }

    private static ValidationFailureExceptionData? CreateExceptionData(Exception? exception) =>
        exception is not null
            ? new ValidationFailureExceptionData(
                exception.Message,
                exception.StackTrace,
                CreateExceptionData(exception.InnerException))
            : null;
}
