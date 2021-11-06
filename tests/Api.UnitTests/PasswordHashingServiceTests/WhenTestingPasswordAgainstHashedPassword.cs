using System.Security.Cryptography;
using FluentAssertions;
using Company.Product.WebApi.Api.Services.PasswordHashing;
using Xunit;

namespace Company.Product.WebApi.Api.UnitTests.PasswordHashingServiceTests;

public sealed class WhenTestingPasswordAgainstHashedPassword
{
    [Fact]
    public void MustReturnTrueIfPasswordsMatch()
    {
        PasswordHashingService passwordHashingService = new();
        const string password = "password";
        const int iterations = 10;
        HashAlgorithmName algorithmName = HashAlgorithmName.SHA256;
        (byte[] salt, byte[] hash) = passwordHashingService.HashPassword(password, 32, 32, iterations, algorithmName);

        passwordHashingService.TestPassword(password, salt, hash, iterations, algorithmName).Should().BeTrue();
    }

    [Fact]
    public void MustReturnFalseIfPasswordsDoNotMatch()
    {
        PasswordHashingService passwordHashingService = new();
        const int iterations = 10;
        HashAlgorithmName algorithmName = HashAlgorithmName.SHA256;
        (byte[] salt, byte[] hash) = passwordHashingService.HashPassword("password", 32, 32, iterations, algorithmName);

        passwordHashingService.TestPassword("wrongpassword", salt, hash, iterations, algorithmName).Should().BeFalse();
    }
}