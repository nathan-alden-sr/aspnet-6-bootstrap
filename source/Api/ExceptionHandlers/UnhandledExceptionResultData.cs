namespace Company.Product.WebApi.Api.ExceptionHandlers;

public sealed record UnhandledExceptionResultData(string Message, string? StackTrace, UnhandledExceptionResultData? InnerException);