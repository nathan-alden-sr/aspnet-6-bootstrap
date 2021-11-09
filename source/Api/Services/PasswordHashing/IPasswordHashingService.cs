using System.Security.Cryptography;

namespace Company.Product.WebApi.Api.Services.PasswordHashing;

public interface IPasswordHashingService
{
    (byte[] salt, byte[] hash) HashPassword(
        string password,
        int saltByteCount,
        int hashByteCount,
        int iterations,
        HashAlgorithmName algorithmName);

    bool TestPassword(
        string password,
        ReadOnlySpan<byte> salt,
        ReadOnlySpan<byte> expectedHash,
        int iterations,
        HashAlgorithmName algorithmName);
}
