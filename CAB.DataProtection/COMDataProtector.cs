using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
namespace CAB.DataProtection
{

  
    public class COMDataProtector
    {
        static protected IntPtr NullPtr = ((IntPtr)((int)(0)));
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
         protected struct CRYPTPROTECT_PROMPTSTRUCT
        {
            public int cbSize;
            public int dwPromptFlags;
            public IntPtr hwndApp;
            public String szPrompt;
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        protected struct DATA_BLOB
        {
            public int cbData;
            public IntPtr pbData;
        }
     
        [DllImport("Crypt32.dll", SetLastError = true,
        CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        protected static extern bool CryptProtectData(
                                          ref DATA_BLOB pDataIn,
                                          String szDataDescr,
                                          ref DATA_BLOB pOptionalEntropy,
                                          IntPtr pvReserved,
                                          ref CRYPTPROTECT_PROMPTSTRUCT pPromptStruct,
                                          int dwFlags,
                                          ref DATA_BLOB pDataOut);

        [DllImport("Crypt32.dll", SetLastError = true,
                    CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        protected static extern bool CryptUnprotectData(
                                          ref DATA_BLOB pDataIn,
                                          String szDataDescr,
                                          ref DATA_BLOB pOptionalEntropy,
                                          IntPtr pvReserved,
                                          ref CRYPTPROTECT_PROMPTSTRUCT pPromptStruct,
                                          int dwFlags,
                                          ref DATA_BLOB pDataOut);

        [DllImport("kernel32.dll",
                    CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        protected unsafe static extern int FormatMessage(int dwFlags,
                                                       ref IntPtr lpSource,
                                                       int dwMessageId,
                                                       int dwLanguageId,
                                                       ref String lpBuffer, int nSize,
                                                       IntPtr* Arguments);
       
     }
   
}
