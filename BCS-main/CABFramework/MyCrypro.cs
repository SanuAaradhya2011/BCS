using System;
using System.IO;
using System.Collections.Generic;
using System.Management;
using System.Security.Cryptography;
using System.Text;
using System.Globalization;
using CAB.Framework.Utility;
using System.Reflection;
using System.Windows.Forms;
using System.Net.NetworkInformation;
namespace CAB.Framework.Utility
{

    public class MyCrypro
    {
        ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BaseBoard"); 
        private const string CRYPTKEY = "RZFKGEHN$GCNV@LXJLKMEKDYYURTKJLE";
        private const string CRYPTIV = "IL#MCFACBX&YKPWGPWPGJNUPL?ZJHBXM";
        public RijndaelManaged _Rijndael = new RijndaelManaged();
        public long App_RunningTime = 0;
        static string copyRightsDetail = string.Empty;
        static string productversion = string.Empty;

        public MyCrypro()
        {
            //  cryptSetting
         
            _Rijndael.Mode = CipherMode.CBC;
            _Rijndael.Padding = PaddingMode.ANSIX923;
            _Rijndael.BlockSize = 256;
            _Rijndael.KeySize = 256;


           
            //_Rijndael.Clear();

        }

        public string GetSystemMACAddress()
        {
            string macAddresses = string.Empty;

            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.NetworkInterfaceType.ToString() == "Ethernet")
                {
                    macAddresses = nic.GetPhysicalAddress().ToString();

                }
            }
            return macAddresses;
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
            CryptoStream cs = new CryptoStream(ms,transform,CryptoStreamMode.Write);
            cs.Write(plainTextBytes,0,plainTextBytes.Length);

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
                string get_Filename = string.Concat(Path.GetDirectoryName(Application.ExecutablePath), "\\EulaCfg.LTB");

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
                            string SysInfo = GetSystemMACAddress();

                            if (GenerateKey(SysInfo) != RecInpData) return "False";
                            else return "OK";


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
                string filePath=string.Concat(Path.GetDirectoryName(Application.ExecutablePath), "\\EulaCfg.LTB");
                FileStream file1 = new FileStream(filePath, FileMode.Create);
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
            catch (Exception )
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

        private void WriteFile(string fileName,string fileContent)
        {
            FileStream file1 = new FileStream(fileName, FileMode.Create);
            StreamWriter wr1 = new StreamWriter(file1);
            wr1.Write(fileContent);
            wr1.Close();
            file1.Close();
        }

        /// <summary>
        /// This function write the application running time in System current directory 
        /// for validating the product registration
        /// </summary>
        /// <param name="CompanyName"></param>
        /// <param name="GetProductID"></param>
        public void SetRegistryInfo(string CompanyName,string GetProductID)
        {
            try
            {

                string get_Filename = string.Concat(Path.GetDirectoryName(Application.ExecutablePath), "\\EulaCfg.LTB");
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
                WriteFile(get_Filename, SysInfo);
            }
            catch (Exception)
            {

            }

        }
        
        /// <summary>
        /// This function write the application running time in  System current directory
        ///  for validating the product registration
        /// </summary>
		public void SetWin32Info()
		{
			DateTime Dt = new DateTime();
			try
			{
				foreach (DriveInfo drive in DriveInfo.GetDrives())
				{
					//string get_Filename = drive.Name + @"Windows\System32\E250DLMSSupport.txt";
					string get_Filename = string.Concat(Path.GetDirectoryName(Application.ExecutablePath), "\\support.txt");
					//get_Filename = get_Filename.Replace("\\\\", "\\");
					string File_Comtent = string.Empty;
					string str_StoredTime = "";
					string StoredDays = "00";
					if (File.Exists(get_Filename))
					{
						StreamReader SR = File.OpenText(get_Filename);
						FileStream fs = new FileStream(get_Filename, FileMode.Open, FileAccess.Read);
						while (((File_Comtent = SR.ReadLine()) != null))
						{
							str_StoredTime = File_Comtent;
						}
						str_StoredTime = ConfigInfo.DecryptFile(str_StoredTime);
						SR.Close();
						fs.Close();
						int dayindex = str_StoredTime.IndexOf(",");
						if (dayindex > 0) File_Comtent = str_StoredTime.Substring(0, dayindex);

						string Storeddate = File_Comtent;
						if (Storeddate != null)
						{
							DateTime dt = DateTime.Parse(Storeddate, new CultureInfo("en-GB"));
							File_Comtent = dt.ToString(System.Globalization.DateTimeFormatInfo.CurrentInfo.ShortDatePattern);
						}
						bool isdate = DateTime.TryParse(File_Comtent, out Dt);
						/*----------Check For Valid stored date------------------*/
						if (isdate)
						{
							int daydiff = 0;
							//DateTime endDate = Convert.ToDateTime(File_Comtent).AddDays(30);
							//TimeSpan ts = endDate - Convert.ToDateTime(File_Comtent);


							if (DateTime.Now >= Convert.ToDateTime(File_Comtent))
							{
								TimeSpan ts = DateTime.Now - Convert.ToDateTime(File_Comtent);
								daydiff = ts.Days;
								StoredDays = str_StoredTime.Substring(dayindex + 1);
								StoredDays = (Convert.ToInt16(StoredDays) + daydiff).ToString();

								/*--------------In case for force normal increment of date----------------*/

								// DateTime mdt = Convert.ToDateTime(File_Comtent);
								Dt = DateTime.Now;
								string fileContent = string.Empty;
								if (daydiff <= 0) fileContent = Dt.ToString("dd/MM/yyyy") + "," + StoredDays;
								else if (daydiff >= Convert.ToInt16(StoredDays)) fileContent = Dt.ToString("dd/MM/yyyy") + "," + daydiff.ToString("00");
								else fileContent = Dt.ToString("dd/MM/yyyy") + "," + StoredDays;
								fileContent = ConfigInfo.EncryptFile(fileContent);
								WriteFile(get_Filename, fileContent);
							}
							else
							{
								StoredDays = str_StoredTime.Substring(dayindex + 1);
								Dt = DateTime.Now;
								str_StoredTime = Dt.ToString("dd/MM/yyyy") + "," + StoredDays;
								str_StoredTime = ConfigInfo.EncryptFile(str_StoredTime);
								WriteFile(get_Filename, str_StoredTime);
							}
						}
						else
						{
							/*------------------In Case of Invalid Date Stored in File -----------------*/
							Dt = DateTime.Now;
							str_StoredTime = Dt.ToString("dd/MM/yyyy");// DateTime.Now.Day.ToString("00") + "/" + DateTime.Now.Month.ToString("00") + "/" + DateTime.Now.Year.ToString("{0YYYY}");
							str_StoredTime = str_StoredTime + ",00";
							str_StoredTime = ConfigInfo.EncryptFile(str_StoredTime);
							WriteFile(get_Filename, str_StoredTime);
						}
						break;
					}
					else
					{
						/*----------------------In Case Of New File Creation----------------------------*/
						Dt = DateTime.Now;
						str_StoredTime = Dt.ToString("dd/MM/yyyy");
						str_StoredTime = str_StoredTime + ",00";
						str_StoredTime = ConfigInfo.EncryptFile(str_StoredTime);
						WriteFile(get_Filename, str_StoredTime);
						break;
					}
				}
			}
			catch (Exception)
			{

			}
		}

        /// <summary>
        /// This function Read the application running time in System current directory 
        /// for validating the product registration 
        /// </summary>
        /// <returns></returns>
        public int GetWin32Info()
        {
            int daydiff = 0;
            try
            {
				string str_StoredTime = string.Empty;
				foreach (DriveInfo drive in DriveInfo.GetDrives())
				{
					//string get_Filename = drive.Name + @"Windows\System32\E250DLMSSupport.txt";

					string get_Filename = string.Concat(Path.GetDirectoryName(Application.ExecutablePath), "\\support.txt");
					get_Filename = get_Filename.Replace("\\\\", "\\");
					string File_Comtent = string.Empty;
					
					if (File.Exists(get_Filename))
					{
						StreamReader SR = File.OpenText(get_Filename);
						FileStream fs = new FileStream(get_Filename, FileMode.Open, FileAccess.Read);
						while (((File_Comtent = SR.ReadLine()) != null))
						{
							str_StoredTime = File_Comtent;
						}
						SR.Close();
						fs.Close();
						str_StoredTime = ConfigInfo.DecryptFile(str_StoredTime);
						int dayindex = str_StoredTime.IndexOf(",");
						if (dayindex > 0) str_StoredTime = str_StoredTime.Substring(dayindex + 1);
						break;
					}
				}
				return Convert.ToInt32(str_StoredTime);
            }
            catch (Exception)
            {
                return daydiff;
            }
        }

        /// <summary>
        /// This function is used to read Product version information from  
        ///  Application Assembly that is title in our case.
        /// </summary>
        /// <returns></returns>
        public string ProductVersion()
        {

            Assembly assembly = System.Reflection.Assembly.GetEntryAssembly();
            if (assembly != null)
            {
                object[] customAttributes = assembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if ((customAttributes != null) && (customAttributes.Length > 0))
                {
                    productversion = ((AssemblyTitleAttribute)customAttributes[0]).Title;
                }
                if (string.IsNullOrEmpty(productversion))
                {
                    productversion = string.Empty;
                }
            }
            return productversion;

        }

        /// <summary>
        ///This function is used to read Copyright information from Application Assembly. 
        /// </summary>
        /// <returns></returns>
        public string CopyRightsDetail()
        {
            Assembly assembly = System.Reflection.Assembly.GetEntryAssembly();
            if (assembly != null)
            {
                object[] customAttributes = assembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if ((customAttributes != null) && (customAttributes.Length > 0))
                {
                    copyRightsDetail = ((AssemblyCopyrightAttribute)customAttributes[0]).Copyright;
                }
                if (string.IsNullOrEmpty(copyRightsDetail))
                {
                    copyRightsDetail = string.Empty;
                }
            }
            return copyRightsDetail;

        }

    }
}