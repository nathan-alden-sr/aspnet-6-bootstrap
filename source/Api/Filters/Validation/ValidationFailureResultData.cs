namespace Company.Product.WebApi.Api.Filters.Validation;

public class ValidationFailureResultData
{
    public ValidationFailureResultData(string message)
    {
        Message = message;
    }

    public string Message { get; }
}
