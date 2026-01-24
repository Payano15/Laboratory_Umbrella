using System.Security.Cryptography;
using System.Text;

namespace Laboratory.Umbrella.Dominio.Helpers;

public static class SecureChecksumHelper
{

    public static string ComputeHMACSHA256(string data, string secretKey)
    {
        var dataBytes = Encoding.UTF8.GetBytes(data);
        var keyBytes = Encoding.UTF8.GetBytes(secretKey);
        return ComputeHMACSHA256(dataBytes, keyBytes);
    }
    private static string ComputeHMACSHA256(byte[] data, byte[] key)
    {
        using var hmac = new HMACSHA256(key);
        var hash = hmac.ComputeHash(data);
        return Convert.ToHexString(hash).ToLower();
    }
    // For streaming large payloads - most efficient
    public static async Task<string> ComputeHMACSHA256Async(Stream dataStream, byte[] key)
    {
        using var hmac = new HMACSHA256(key);
        var hashBytes = await hmac.ComputeHashAsync(dataStream);
        return Convert.ToHexString(hashBytes).ToLower();
    }
    // Zero-allocation version for hot paths
    public static void ComputeHMACSHA256(ReadOnlySpan<byte> data, ReadOnlySpan<byte> key, Span<byte> destination)
    {
        using var hmac = new HMACSHA256(key.ToArray());
        if (!hmac.TryComputeHash(data, destination, out _))
        {
            throw new InvalidOperationException("Failed to compute hash");
        }
    }
    public static bool Validate(string current, string expected)
    {
        if (!CryptographicOperations.FixedTimeEquals(Convert.FromHexString(current), Convert.FromHexString(expected)))
            return false;

        return true;
    }
}
