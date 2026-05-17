using System;
namespace CAB.DataProtection
{
    /// <summary>
    /// Interface for key based data protection.
    /// </summary>
    public interface IKeyedDataProtection
    {
        string DecryptString(string cipherText);
        string EncryptString(string plainText);
    }
}
