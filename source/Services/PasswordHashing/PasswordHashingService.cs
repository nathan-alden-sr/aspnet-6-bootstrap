using System.Security.Cryptography;

namespace Company.Product.WebApi.Services.PasswordHashing;

public sealed class PasswordHashingService : IPasswordHashingService
{
    public (byte[] salt, byte[] hash) HashPassword(
        string password,
        int saltByteCount,
        int hashByteCount,
        int iterations,
        HashAlgorithmName algorithmName)
    {
        var salt = new byte[saltByteCount];

        RandomNumberGenerator.Fill(salt);

        var hash = new byte[hashByteCount];

        Rfc2898DeriveBytes.Pbkdf2(password, salt, hash, iterations, algorithmName);

        return (salt, hash);
    }

    public bool TestPassword(
        string password,
        ReadOnlySpan<byte> salt,
        ReadOnlySpan<byte> expectedHash,
        int iterations,
        HashAlgorithmName algorithmName)
    {
        var actualHash = new byte[expectedHash.Length];

        Rfc2898DeriveBytes.Pbkdf2(password, salt, actualHash, iterations, algorithmName);

        return expectedHash.SequenceEqual(actualHash);
    }
}
