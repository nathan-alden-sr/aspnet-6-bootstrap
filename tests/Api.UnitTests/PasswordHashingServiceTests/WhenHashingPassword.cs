using System.Security.Cryptography;
using FluentAssertions;
using Company.Product.WebApi.Api.Services.PasswordHashing;
using Xunit;

namespace Company.Product.WebApi.Api.UnitTests.PasswordHashingServiceTests;

public sealed class WhenHashingPassword
{
    [Theory]
    [InlineData(4, 4)]
    [InlineData(8, 4)]
    [InlineData(4, 8)]
    public void MustGenerateSaltAndHashWithSpecifiedLength(int saltByteCount, int hashByteCount)
    {
        PasswordHashingService passwordHashingService = new();
        (byte[] salt, byte[] hash) = passwordHashingService.HashPassword("password", saltByteCount, hashByteCount, 10, HashAlgorithmName.SHA256);

        hash.Should().HaveCount(hashByteCount);
        salt.Should().HaveCount(saltByteCount);
    }
}