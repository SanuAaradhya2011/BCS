using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using CAB.Framework.Utility;
using CAB.Framework.Utility.Cryptography;
using System.Text.RegularExpressions;

namespace CAB.Framework.Utility
{
    public static class FileHandler
    {
        /// <summary>
        /// Return the file content. If key "EncryptFile = true" in app.config then it will return decrypted file. 
        /// </summary>
        /// <param name="filePath">The path of the file where it is stored.</param>
        /// <returns>File content</returns>
        public static string ReadFile(string filePath)
        {
            StreamReader streamReader = new StreamReader(filePath);
            string fileContent = streamReader.ReadToEnd();
            streamReader.Close();
            if (ConfigInfo.IsEncryptionEnabled() == true)
            {
                fileContent = DecryptFile(fileContent);
            }
            return fileContent;
        }

        /// <summary>
        /// Return true if file checksum is valid otherwise false.
        /// </summary>
        /// <param name="strInput">String for which BCC needs to be calculated and matched.</param>
        /// <returns>True if checksum is valid otherwise false.</returns>
        private static bool FileChecksumValid(string strInput)
        {
            CryptoProvider cryptoProvider = new CryptoProvider();

            if (strInput == "\0")
            {
                return false;
            }

            int calculatedBCC = cryptoProvider.CallBcc(strInput.Substring(0, strInput.Length - 1));
            int receivedBCC = strInput[strInput.Length - 1];

            if (receivedBCC == calculatedBCC)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Encrypt a file based on Rijndael algorithm with a secure key.
        /// </summary>
        /// <param name="plaintext">The plaintext to be encrypted.</param>
        /// <returns>Encrypted ciphertext.</returns>
        public static string EncryptFile(string plaintext)
        {
            CryptoProvider cryptoProvider = new CryptoProvider();
            return cryptoProvider.EncryptString(plaintext);
        }
        
        /// <summary>
        /// Decrypt a file based on Rijndael algorithm with a secure key.
        /// </summary>
        /// <param name="ciphertext">The ciphertext to be decrypted.</param>
        /// <returns>Decrypted plaintext.</returns>
        public static string DecryptFile(string ciphertext)
        {
            CryptoProvider cryptoProvider = new CryptoProvider();
            return cryptoProvider.DecryptString(ciphertext); 
        }
        
        /// <summary>
        /// Validate file by matching checksum.
        /// </summary>
        /// <param name="filePath">The path of the file where it is stored.</param>
        /// <returns>True if the file is valid otherwise false.</returns>
        public static bool ValidateFile(string filePath)
        {
            bool isFileValid = false;
            string strfileContent = ReadFile(filePath);
            
            if (FileChecksumValid(strfileContent) == true)
            {
                isFileValid = true;
            }
            
            return isFileValid;
        }

        /// <summary>
        /// Checks whether file has only null values.
        /// </summary>
        /// <param name="filePath">The path of the file where it is stored.</param>
        /// <returns>True if file contains only null values otherwise false.</returns>
        public static bool CheckNullFile(string filePath)
        {
            const string regexValidFile = @"(\x01[\w\W](.*?)\x03[\w\W]\x04)";
            string fileContent = ReadFile(filePath);

            MatchCollection matches = Regex.Matches(fileContent, regexValidFile, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.Multiline);
            if (matches.Count != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}