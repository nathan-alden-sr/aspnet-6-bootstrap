using Company.Product.WebApi.Api.Filters.Validation;
using Company.Product.WebApi.Api.Results;
using Microsoft.AspNetCore.Mvc;

namespace Company.Product.WebApi.Api.Swashbuckle;

public class ProducesBadRequestResponseTypeAttribute : ProducesResponseTypeAttribute
{
    public ProducesBadRequestResponseTypeAttribute() : base(StatusCodes.Status400BadRequest)
    {
        Type = typeof(IStandardJsonResult<IEnumerable<ValidationFailureResultData>>);
    }
}
