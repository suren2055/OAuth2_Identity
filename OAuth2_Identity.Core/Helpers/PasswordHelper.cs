using System.Security.Cryptography;
using System.Text;

namespace OAuth2_Identity.Core.Helpers;

public static class PasswordHelper
{
    public static bool CompareHashes(byte[] hash1, byte[] hash2)
    {
        return hash1.SequenceEqual(hash2);
    }

    public static string Hash(string password, string salt)
    {
        var rawData = string.Concat(password, salt);
        using var sha256Hash = SHA256.Create();
        var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

        var builder = new StringBuilder();
        foreach (var t in bytes)
            builder.Append(t.ToString("x2"));

        return builder.ToString();
    }

    public static string Salt(string rawData)
    {
        var rng = new RNGCryptoServiceProvider();
        byte[] userBytes;
        string salt;
        userBytes = Encoding.ASCII.GetBytes(rawData);
        long xored = 0x00;

        foreach (int x in userBytes)
            xored = xored ^ x;

        var rand = new Random(Convert.ToInt32(xored));
        salt = rand.Next().ToString();
        salt += rand.Next().ToString();
        salt += rand.Next().ToString();
        salt += rand.Next().ToString();
        return salt;
    }
}