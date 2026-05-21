using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using CAB.Framework;
namespace CAB.DataProtection
{
  
    public class DataProtector : COMDataProtector,IDataProtector
    {
        public DataProtector(DataStoreType dataStoreType)
        {
            this.store = dataStoreType;
        }
       
        private const int CRYPTPROTECT_UI_FORBIDDEN = 0x1;
        private const int CRYPTPROTECT_LOCAL_MACHINE = 0x4;
        private const string UNABLETOALLOCATEPLAINTEXT = "Unable to allocate plaintext buffer.";
        private const string EXCEPTIONMARSHALDATA = "Exception marshalling data. ";
        private const string UNABLETOALLOCATEENTROPY = "Unable to allocate entropy data buffer.";
        private const string EXCEPTIONENTROPYMARSHALDATA = "Exception entropy marshalling data. ";
        private const string ENCRYPTIONFAILED = "Encryption failed. ";
        private const string EXCEPTIONENCRYPTING = "Exception encrypting. ";
        private const string UNABLETOALLOCATECIPHERTEXT = "Unable to allocate cipherText buffer.";
        private const string DECRYPTIONFAILED = "Decryption failed. ";
        private const string EXCEPTIONDECRYPTING = "Exception decrypting. ";
        private const string FAILEDTOFORMATMESSAGE = "Failed to format message for error code ";
        private const string ENDMESSAGE = ". ";
        private DataStoreType store;
         /// <summary>
        /// Encrypts an array of bytes.
        /// </summary>
        /// <param name="plainText">The byte array to be encrypted.</param>
        /// <param name="optionalEntropy">Optionally used.  Not used in this implementation.</param>
        /// <returns>The encrypted byte array</returns>
        public byte[] Encrypt(byte[] plainText, byte[] optionalEntropy)
        {
            bool retVal = false;
            int dwFlags;
            DATA_BLOB plainTextBlob = new DATA_BLOB();
            DATA_BLOB cipherTextBlob = new DATA_BLOB();
            DATA_BLOB entropyBlob = new DATA_BLOB();

            CRYPTPROTECT_PROMPTSTRUCT prompt = new CRYPTPROTECT_PROMPTSTRUCT();
            InitPromptstruct(ref prompt);

            
            try
            {
                try
                {
                    int bytesSize = plainText.Length;
                    plainTextBlob.pbData = Marshal.AllocHGlobal(bytesSize);
                    if (IntPtr.Zero == plainTextBlob.pbData)
                    {
                        throw new Exception(UNABLETOALLOCATEPLAINTEXT);
                    }
                    plainTextBlob.cbData = bytesSize;
                    Marshal.Copy(plainText, 0, plainTextBlob.pbData, bytesSize);
                }
                catch (Exception ex)
                {
                    throw new Exception(EXCEPTIONMARSHALDATA + ex.Message);
                }
                if (DataStoreType.USE_MACHINE_STORE == store)
                {//Using the machine store, should be providing entropy.
                    dwFlags = CRYPTPROTECT_LOCAL_MACHINE | CRYPTPROTECT_UI_FORBIDDEN;
                    //Check to see if the entropy is null
                    if (null == optionalEntropy)
                    {//Allocate something       
                        optionalEntropy = new byte[0];
                    }
                    try
                    {
                        int bytesSize = optionalEntropy.Length;
                        entropyBlob.pbData = Marshal.AllocHGlobal(optionalEntropy.Length); ;
                        if (IntPtr.Zero == entropyBlob.pbData)
                        {
                            throw new Exception(UNABLETOALLOCATEENTROPY);
                        }
                        Marshal.Copy(optionalEntropy, 0, entropyBlob.pbData, bytesSize);
                        entropyBlob.cbData = bytesSize;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(EXCEPTIONENTROPYMARSHALDATA +
                                            ex.Message);
                    }
                }
                else
                {//Using the user store
                    dwFlags = CRYPTPROTECT_UI_FORBIDDEN;
                }
                retVal = CryptProtectData(ref plainTextBlob, "", ref entropyBlob,
                                          IntPtr.Zero, ref prompt, dwFlags,
                                          ref cipherTextBlob);
                if (false == retVal)
                {
                    throw new Exception(ENCRYPTIONFAILED +
                                        GetErrorMessage(Marshal.GetLastWin32Error()));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(EXCEPTIONENCRYPTING + ex.Message);
            }
            byte[] cipherText = new byte[cipherTextBlob.cbData];
            Marshal.Copy(cipherTextBlob.pbData, cipherText, 0, cipherTextBlob.cbData);
            return cipherText;
        }

        /// <summary>
        /// Decrypts an array of bytes
        /// </summary>
        /// <param name="cipherText">The byte array to decrypt.</param>
        /// <param name="optionalEntropy">Optional.  Not used in this implementation.</param>
        /// <returns>The unencrypted byte array.</returns>
        public byte[] Decrypt(byte[] cipherText, byte[] optionalEntropy)
        {
            bool retVal = false;
            DATA_BLOB plainTextBlob = new DATA_BLOB();
            DATA_BLOB cipherBlob = new DATA_BLOB();
            CRYPTPROTECT_PROMPTSTRUCT prompt = new CRYPTPROTECT_PROMPTSTRUCT();
            InitPromptstruct(ref prompt);
            try
            {
                try
                {
                    int cipherTextSize = cipherText.Length;
                    cipherBlob.pbData = Marshal.AllocHGlobal(cipherTextSize);
                    if (IntPtr.Zero == cipherBlob.pbData)
                    {
                        throw new Exception(UNABLETOALLOCATECIPHERTEXT);
                    }
                    cipherBlob.cbData = cipherTextSize;
                    Marshal.Copy(cipherText, 0, cipherBlob.pbData, cipherBlob.cbData);
                }
                catch (Exception ex)
                {
                    throw new Exception(EXCEPTIONMARSHALDATA + ex.Message);
                }
                DATA_BLOB entropyBlob = new DATA_BLOB();
                int dwFlags;
                if (DataStoreType.USE_MACHINE_STORE == store)
                {//Using the machine store, should be providing entropy.
                    dwFlags = CRYPTPROTECT_LOCAL_MACHINE | CRYPTPROTECT_UI_FORBIDDEN;
                    //Check to see if the entropy is null
                    if (null == optionalEntropy)
                    {//Allocate something
                        optionalEntropy = new byte[0];
                    }
                    try
                    {
                        int bytesSize = optionalEntropy.Length;
                        entropyBlob.pbData = Marshal.AllocHGlobal(bytesSize);
                        if (IntPtr.Zero == entropyBlob.pbData)
                        {
                            throw new Exception(UNABLETOALLOCATEENTROPY);
                        }
                        entropyBlob.cbData = bytesSize;
                        Marshal.Copy(optionalEntropy, 0, entropyBlob.pbData, bytesSize);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(EXCEPTIONENTROPYMARSHALDATA +
                                            ex.Message);
                    }
                }
                else
                {//Using the user store
                    dwFlags = CRYPTPROTECT_UI_FORBIDDEN;
                }
                retVal = CryptUnprotectData(ref cipherBlob, null, ref entropyBlob,
                                            IntPtr.Zero, ref prompt, dwFlags,
                                            ref plainTextBlob);
                if (false == retVal)
                {
                    throw new Exception(DECRYPTIONFAILED +
                                          GetErrorMessage(Marshal.GetLastWin32Error()));
                }
                //Free the blob and entropy.
                if (IntPtr.Zero != cipherBlob.pbData)
                {
                    Marshal.FreeHGlobal(cipherBlob.pbData);
                }
                if (IntPtr.Zero != entropyBlob.pbData)
                {
                    Marshal.FreeHGlobal(entropyBlob.pbData);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(EXCEPTIONDECRYPTING + ex.Message);
            }
            byte[] plainText = new byte[plainTextBlob.cbData];
            Marshal.Copy(plainTextBlob.pbData, plainText, 0, plainTextBlob.cbData);
            return plainText;
        }

        private void InitPromptstruct(ref CRYPTPROTECT_PROMPTSTRUCT ps)
        {
            ps.cbSize = Marshal.SizeOf(typeof(CRYPTPROTECT_PROMPTSTRUCT));
            ps.dwPromptFlags = 0;
            ps.hwndApp = NullPtr;
            ps.szPrompt = null;
        }

        private unsafe static String GetErrorMessage(int errorCode)
        {
            int FORMAT_MESSAGE_ALLOCATE_BUFFER = 0x00000100;
            int FORMAT_MESSAGE_IGNORE_INSERTS = 0x00000200;
            int FORMAT_MESSAGE_FROM_SYSTEM = 0x00001000;
            int messageSize = 255;
            String lpMsgBuf = "";
            int dwFlags = FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM |
                          FORMAT_MESSAGE_IGNORE_INSERTS;
            IntPtr ptrlpSource = new IntPtr();
            IntPtr prtArguments = new IntPtr();
            int retVal = FormatMessage(dwFlags, ref ptrlpSource, errorCode, 0,
                                       ref lpMsgBuf, messageSize, &prtArguments);
            if (0 == retVal)
            {
                throw new Exception(FAILEDTOFORMATMESSAGE +
                                    errorCode + ENDMESSAGE);
            }
            return lpMsgBuf;
        }
    }
}
