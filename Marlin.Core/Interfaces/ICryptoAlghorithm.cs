namespace Marlin.Core.Interfaces;

public interface ICryptoAlghorithm
{
    string GetSignature(string input, string secret);
}