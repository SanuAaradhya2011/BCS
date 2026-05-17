using System;
using System.IO;
using System.Collections.Generic;
using System.Management;
using System.Security.Cryptography;
using System.Text;
using System.Globalization;
using System.Reflection;
using System.Net.NetworkInformation;

namespace CAB.DataProtection
{
    /// <summary>
    /// This class provides key based encryption decryption methods.
    /// </summary>
    public class KeyedDataProtection : IKeyedDataProtection
    {
        #region variables
        ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BaseBoard");
        private const string CRYPTKEY = "RZFKGEHN$GCNV@LXJLKMEKDYYURTKJLE";
        private const string CRYPTIV = "IL#MCFACBX&YKPWGPWPGJNUPL?ZJHBXM";
        public RijndaelManaged rijndael = new RijndaelManaged();
        public long App_RunningTime = 0;
        static string copyRightsDetail = string.Empty;
        static string productversion = string.Empty;
        byte[] plainTextBytes = null;
        byte[] cipherTextBytes = null;
        ICryptoTransform transform = null;
        byte[] plainTextBuffer = null;
        byte[] plainText = null;
        #endregion


        public KeyedDataProtection()
        {
            //  cryptSetting
            rijndael.Mode = CipherMode.CBC;
            rijndael.Padding = PaddingMode.ANSIX923;
            rijndael.BlockSize = 256;
            rijndael.KeySize = 256;
            rijndael.Key = Encoding.ASCII.GetBytes(CRYPTKEY);
            rijndael.IV = Encoding.ASCII.GetBytes(CRYPTIV);
        }

        #region public functions
        /// <summary>
        /// Encrypts a string
        /// </summary>
        /// <param name="decryptedString"></param>
        /// <returns></returns>
        public string EncryptString(string plainText)
        {
            plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            transform = rijndael.CreateEncryptor();
            //create a new object of memory stream to avoid data corruption and memory leaks
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream,transform,CryptoStreamMode.Write);
            cryptoStream.Write(plainTextBytes,0,plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] cipherText = memoryStream.ToArray();
           
            return Convert.ToBase64String(cipherText);
          

           
        }
        /// <summary>
        /// Decrypt a string
        /// </summary>
        /// <param name="encryptedString"></param>
        /// <returns></returns>
        public string DecryptString(string cipherText)
        {
            try
            {
                int plainTextLength; 
                cipherTextBytes = Convert.FromBase64String(cipherText);
                transform = rijndael.CreateDecryptor();
                //create a new object of memory stream to avoid data corruption and memory leaks
                MemoryStream ms = new MemoryStream(cipherTextBytes);
                CryptoStream cs = new CryptoStream(ms, transform, CryptoStreamMode.Read);

                plainTextBuffer = new byte[cipherTextBytes.Length];
                plainTextLength = cs.Read(plainTextBuffer, 0, cipherTextBytes.Length);

                plainText = new byte[plainTextLength];
                Array.Copy(plainTextBuffer, 0, plainText, 0, plainTextLength);
                return Encoding.UTF8.GetString(plainText);
            }
            catch (Exception)
            {
                return string.Empty;
            }


        }
        #endregion
    }

}
