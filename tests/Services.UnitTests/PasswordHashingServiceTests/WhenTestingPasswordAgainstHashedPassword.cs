using System.Security.Cryptography;
using Company.Product.WebApi.Services.PasswordHashing;
using FluentAssertions;
using Xunit;

namespace Company.Product.WebApi.Services.UnitTests.PasswordHashingServiceTests;

public sealed class WhenTestingPasswordAgainstHashedPassword
{
    [Fact]
    public void MustReturnTrueIfPasswordsMatch()
    {
        var passwordHashingService = new PasswordHashingService();
        const string password = "password";
        const int iterations = 10;
        var algorithmName = HashAlgorithmName.SHA256;
        var (salt, hash) = passwordHashingService.HashPassword(password, 32, 32, iterations, algorithmName);

        passwordHashingService.TestPassword(password, salt, hash, iterations, algorithmName).Should().BeTrue();
    }

    [Fact]
    public void MustReturnFalseIfPasswordsDoNotMatch()
    {
        var passwordHashingService = new PasswordHashingService();
        const int iterations = 10;
        var algorithmName = HashAlgorithmName.SHA256;
        var (salt, hash) = passwordHashingService.HashPassword("password", 32, 32, iterations, algorithmName);

        passwordHashingService.TestPassword("wrongpassword", salt, hash, iterations, algorithmName).Should().BeFalse();
    }
}
