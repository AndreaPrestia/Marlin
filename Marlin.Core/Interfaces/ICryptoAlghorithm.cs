namespace Marlin.Core.Interfaces;

public interface ICryptoAlghorithm
{
    string AlgorithmName { get; }
    string GetSignature(string input, string secret);
}