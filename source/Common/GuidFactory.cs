namespace Company.Product.WebApi.Common;

public sealed class GuidFactory : IGuidFactory
{
    public Guid CreateRandom() =>
        Guid.NewGuid();
}