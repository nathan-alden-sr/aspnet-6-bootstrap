using System.Text;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Company.Product.WebApi.Api.Swashbuckle;

public sealed class TitleFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        ThrowIfNull(schema);
        ThrowIfNull(context);

        var stringBuilder = new StringBuilder();

        GetTypeName(context.Type, stringBuilder);

        schema.Title = stringBuilder.ToString();
    }

    private static void GetTypeName(Type type, StringBuilder stringBuilder)
    {
        _ = stringBuilder.Append(type.IsGenericType ? type.Name[..type.Name.LastIndexOf('`')] : type.Name);

        // ReSharper disable once InvertIf
        if (type.IsGenericType)
        {
            _ = stringBuilder.Append('<');

            foreach (var genericArgument in type.GetGenericArguments())
            {
                GetTypeName(genericArgument, stringBuilder);
            }

            _ = stringBuilder.Append('>');
        }
    }
}
