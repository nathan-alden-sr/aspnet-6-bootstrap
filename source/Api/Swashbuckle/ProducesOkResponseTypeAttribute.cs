using Microsoft.AspNetCore.Mvc;

namespace Company.Product.WebApi.Api.Swashbuckle;

public class ProducesOkResponseTypeAttribute : ProducesResponseTypeAttribute
{
    public ProducesOkResponseTypeAttribute(Type? type = null) : base(StatusCodes.Status200OK)
    {
        if (type is not null)
        {
            Type = type;
        }
    }
}
