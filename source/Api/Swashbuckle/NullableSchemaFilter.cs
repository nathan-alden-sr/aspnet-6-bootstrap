using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Company.Product.WebApi.Api.Swashbuckle;

public sealed class NullableSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.MemberInfo?.IsNonNullableReferenceType() != true)
        {
            return;
        }

        schema.Nullable = false;

        if (schema.Items is not null)
        {
            schema.Items.Nullable = false;
        }
    }
}