namespace Company.Product.WebApi.Api.Filters.Validation;

public sealed record ValidationFailureExceptionData(
    string Message,
    string? StackTrace,
    ValidationFailureExceptionData? InnerException);
