using System.Text;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Company.Product.WebApi.Api.Swashbuckle;

public sealed class TitleFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        ThrowIfNull(schema, nameof(schema));
        ThrowIfNull(context, nameof(context));

        schema.Title = GetTypeName(context.Type, new StringBuilder());
    }

    private static string GetTypeName(Type type, StringBuilder stringBuilder)
    {
        stringBuilder.Append(type.IsGenericType ? type.Name[..type.Name.LastIndexOf('`')] : type.Name);

        // ReSharper disable once InvertIf
        if (type.IsGenericType)
        {
            stringBuilder.Append('<');

            foreach (Type genericArgument in type.GetGenericArguments())
            {
                GetTypeName(genericArgument, stringBuilder);
            }

            stringBuilder.Append('>');
        }

        return stringBuilder.ToString();
    }
}