using System;
using System.IO;
using System.Text;
using Marlin.Core.Interfaces;

namespace Marlin.Core.Encrypt;

public class HMACSHA256 : ICryptoAlghorithm
{
    private static readonly char[] Padding = { '=' };
    public string AlgorithmName => "HS256";

    public string GetSignature(string input, string secret)
    {
        byte[] result;

        using (var hash256 = new System.Security.Cryptography.HMACSHA256(Encoding.ASCII.GetBytes(secret)))
        {
            using var memoryStream = new MemoryStream(Encoding.Default.GetBytes(input));
            result = hash256.ComputeHash(memoryStream);
        }

        return Convert.ToBase64String(result).TrimEnd(Padding).Replace('+', '-').Replace('/', '_');
    }
}