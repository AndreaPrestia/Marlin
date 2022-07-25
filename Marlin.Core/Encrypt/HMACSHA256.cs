using System;
using System.IO;
using System.Text;
using Marlin.Core.Interfaces;

namespace Marlin.Core.Encrypt;

public class HMACSHA256 : ICryptoAlghorithm
{
    public string GetSignature(string input, string secret)
    {
        byte[] result = null;
        
        using (var hash256 = new System.Security.Cryptography.HMACSHA256(Encoding.ASCII.GetBytes(secret)))
        {
            using (var memoryStream = new MemoryStream(Encoding.ASCII.GetBytes(input)))
            {
                result = hash256.ComputeHash(memoryStream);
            }
        }
        
        return Convert.ToBase64String(result);
    }
}