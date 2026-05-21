 /* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 									|
 * | 																												|
 * |											Author : Piyush Singh         									|
 * |											Date   : 05/04/2010 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| */


using System.Management;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System;

namespace LNG.Framework.Utility.Cryptography
{
    public class CryptoProvider
    {
        ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BaseBoard");
        private const string CRYPTKEY = "RZFKGEHN$GCNV@LXJLKMEKDYYURTKJLE";
        private const string CRYPTIV = "IL#MCFACBX&YKPWGPWPGJNUPL?ZJHBXM";
        public RijndaelManaged _Rijndael = new RijndaelManaged();
        public long App_RunningTime = 0;

        public CryptoProvider()
        {
            //  cryptSetting

            _Rijndael.Mode = CipherMode.CBC;
            _Rijndael.Padding = PaddingMode.ANSIX923;
            _Rijndael.BlockSize = 256;
            _Rijndael.KeySize = 256;



            //_Rijndael.Clear();

        }


        /// <summary>
        /// Encrypts a string
        /// </summary>
        /// <param name="decryptedString"></param>
        /// <returns></returns>
        public string EncryptString(string plainText)
        {
            _Rijndael.Key = Encoding.ASCII.GetBytes(CRYPTKEY);
            _Rijndael.IV = Encoding.ASCII.GetBytes(CRYPTIV);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            ICryptoTransform transform = _Rijndael.CreateEncryptor();
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, transform, CryptoStreamMode.Write);
            cs.Write(plainTextBytes, 0, plainTextBytes.Length);

            cs.FlushFinalBlock();

            byte[] cipherText = ms.ToArray();

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
                _Rijndael.Key = Encoding.ASCII.GetBytes(CRYPTKEY);
                _Rijndael.IV = Encoding.ASCII.GetBytes(CRYPTIV);
                byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
                ICryptoTransform transform = _Rijndael.CreateDecryptor();
                MemoryStream ms = new MemoryStream(cipherTextBytes);
                CryptoStream cs = new CryptoStream(ms, transform, CryptoStreamMode.Read);

                byte[] plainTextBuffer = new byte[cipherTextBytes.Length];
                int plainTextLength = cs.Read(plainTextBuffer, 0, cipherTextBytes.Length);

                byte[] plainText = new byte[plainTextLength];
                Array.Copy(plainTextBuffer, 0, plainText, 0, plainTextLength);
                return Encoding.UTF8.GetString(plainText);
            }
            catch (Exception)
            {
                return "";
            }


        }

        /******************************************************************************
        *  Function Name   : EULAVerification
        *  Description     : This function read the Register File to which this product is 
        *                    registered and return the name of organisation
        *                     
        *                     
        *******************************************************************************/
        public string EULAVerification()
        {
            try
            {

                StringBuilder FileData = new StringBuilder();
                string File_Comtent = string.Empty;
                string File_Inputs = string.Empty;
                string get_Filename = AppDomain.CurrentDomain.BaseDirectory + "EulaCfg.LTB";

                if (File.Exists(get_Filename))
                {
                    StreamReader SR = File.OpenText(get_Filename);
                    FileStream fs = new FileStream(get_Filename, FileMode.Open, FileAccess.Read);
                    while (((File_Comtent = SR.ReadLine()) != null))
                    {
                        File_Inputs += File_Comtent;
                    }
                    SR.Close();
                    fs.Close();
                    string RecInpData = DecryptString(File_Inputs);
                    if (RecInpData == "") return "FileError";
                    string BccResponse = CalFileBcc(RecInpData.Substring(0, RecInpData.Length - 1));
                    if (BccResponse != RecInpData.Substring(RecInpData.Length - 1, 1)) return "False";
                    else
                    {
                        int startindex = RecInpData.IndexOf("STARTS");
                        int Endindex = RecInpData.IndexOf("ENDS");
                        RecInpData = RecInpData.Substring(startindex + 6, Endindex - (startindex + 6));
                        RecInpData = RecInpData.Trim();
                        if (RecInpData == "DemoSerialID") return "True";
                        else
                        {
                            foreach (ManagementObject wmi in searcher.Get())
                            {
                                string SysInfo = wmi.GetPropertyValue("SerialNumber").ToString().Trim();
                                SysInfo += wmi.GetPropertyValue("Product").ToString().Trim();
                                if (GenerateKey(SysInfo) != RecInpData) return "False";
                                else return "OK";
                            }

                        }
                        return RecInpData;
                    }

                }
                else
                {
                    DemoRegistration();
                    return "True";
                }

            }


            catch (Exception)
            {
                return "FileError";

            }
        }

        public void DemoRegistration()
        {
            try
            {
                Random r = new Random();
                string randnumber = "";
                while (randnumber.Length < 200) randnumber += r.Next().ToString();
                string SysInfo = randnumber + "STARTS" + "DemoSerialID" + "ENDS" + randnumber;
                string bcc = CalFileBcc(SysInfo);
                SysInfo = EncryptString(SysInfo + bcc);
                FileStream file1 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "EulaCfg.LTB", FileMode.Create);
                StreamWriter wr1 = new StreamWriter(file1);
                wr1.Write(SysInfo);
                wr1.Close();
                file1.Close();


            }
            catch (Exception)
            {

            }

        }
        /******************************************************************************
      *    Function Name    : CalFileBcc
      *    Description      : This function calculate the CheckSum of the string before
      *                       writing it into the file.
      *******************************************************************************/
        public string CalFileBcc(string RecInpData)
        {
            long countbyt = 0;
            long Bcc = 0;
            string strBcc = "";
            try
            {
                System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
                Byte[] bytesBcc = encoding.GetBytes(RecInpData);

                Byte[] bytes = encoding.GetBytes(RecInpData);
                foreach (byte b in bytes)
                {
                    if (countbyt <= RecInpData.Length) Bcc = Bcc ^ b;
                    countbyt++;
                }

                strBcc = Convert.ToChar(Bcc).ToString();
                return strBcc;
            }
            catch (Exception)
            {
                return "";
            }

        }
        public string GenerateKey(string SysInfo)
        {
            try
            {
                string strPcode = "";
                string[] charstr = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
                int charcnt = 0;
                System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding(); // changes the string to ASCII equivalent
                Byte[] bytes = encoding.GetBytes(SysInfo);
                foreach (byte b in bytes)
                {
                    string strtemp = Convert.ToChar(b ^ 0xF).ToString();
                    Byte[] keychar = encoding.GetBytes(strtemp);
                    foreach (byte ky in keychar)
                    {
                        if (ky >= 65 && ky <= 90)
                        {
                            strPcode += strtemp;

                        }
                        else if (ky >= 48 && ky <= 57)
                        {
                            strPcode += strtemp;

                        }
                        else strPcode += charstr[charcnt];

                    }

                    charcnt++;
                }
                return strPcode;
            }
            catch (Exception)
            {
                return "";
            }

        }

        /// <summary>
        /// Calculates the BCC for the input string.
        /// </summary>
        /// <param name="InpStr">String for which BCC needs to be calculated.</param>
        /// <returns>BCC</returns>
        public int CallBcc(string InpStr)
        {

            int Bcc = 0;
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            Byte[] bytes = encoding.GetBytes(InpStr);
            foreach (byte b in bytes)
            {
                Bcc = Bcc ^ b;
            }
            return Bcc;


        }

        /******************************************************************************
   *  Function Name   : SetRegistryInfo
   *  Description     : This function write the application running time in 
   *                    System current directory for validating the product registration
   *                     
                          
   *******************************************************************************/
        public void SetRegistryInfo(string CompanyName, string GetProductID)
        {
            try
            {

                string get_Filename = AppDomain.CurrentDomain.BaseDirectory + "EulaCfg.LTB";
                string File_Comtent = string.Empty;
                Random r = new Random();
                string randnumber = "";
                while (randnumber.Length < 200) randnumber += r.Next().ToString();
                string SysInfo = randnumber + "STARTC" + CompanyName + "ENDC" + randnumber;
                randnumber = "";
                while (randnumber.Length < 200) randnumber += r.Next().ToString();
                SysInfo += "STARTS" + GetProductID + "ENDS" + randnumber;
                randnumber = "";
                while (randnumber.Length < 200) randnumber += r.Next().ToString();
                string bcc = CalFileBcc(SysInfo);
                SysInfo = EncryptString(SysInfo + bcc);


                FileStream file1 = new FileStream(get_Filename, FileMode.Create);
                StreamWriter wr1 = new StreamWriter(file1);
                wr1.Write(SysInfo);
                wr1.Close();
                file1.Close();

            }
            catch (Exception)
            {
            }

        }

    }
}
