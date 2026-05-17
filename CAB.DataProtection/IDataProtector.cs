using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAB.DataProtection
{
    public interface IDataProtector
    {
        byte[] Encrypt(byte[] plainText, byte[] optionalEntropy);
        byte[] Decrypt(byte[] cipherText, byte[] optionalEntropy);
    }
}
