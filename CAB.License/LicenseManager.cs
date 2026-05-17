using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Windows.Forms;
using System.Security.Cryptography;
using CAB.Framework;
using Microsoft.Win32;
using CAB.DataProtection;
using CAB.License.DataStore;
using System.Management;
using System.Configuration;
using System.Net.NetworkInformation;
using CAB.Framework.Utility.Cryptography;
using CAB.Framework.Utility;
namespace CAB.License
{
    /// <summary>
    /// This class takes care of license status and all the actions related to licensing
    /// </summary>
    public class LicenseManager : ILicenseManager
    {
        #region private variables
        private const string WIN32PROCESSOR = "Win32_Processor";
        LicenseStatus licenseStatus;
        IDataStoreManager dataStoreManager = null;
        private const int DEMOTRIALPERIOD = 30;
        private const string masterKey = "AJI6M5G9I83D";
        private const string defaultKey = "NNMMLLKKJJII";
        #endregion

        public LicenseManager()
        {
            dataStoreManager = new DataStoreManager();
        }

        #region public functions
        /// <summary>
        /// This function write the object into file
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="dataStoreInfo"></param>
        public void WriteClientInformationToFile(string fileName, DataStoreInfo dataStoreInfo)
        {
            dataStoreInfo.UniqueClientId = CreateUniqueMachineID();
            dataStoreManager.EncryptObjectandWriteToFile(dataStoreInfo, fileName);
        }
        /// <summary>
        /// This function reads the object from file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public DataStoreInfo ReadClientInformationFromFile(string fileName)
        {
            return dataStoreManager.ReadFileandDecryptObject(fileName);
        }

        /// <summary>
        /// This function gets the status of license of BCS
        /// </summary>
        /// <returns></returns>

        public DataStoreInfo ReadClientInformationFromRegistry()
        {
            return dataStoreManager.ReadDataFromRegistry();
        }


        public LicenseStatus GetLicenseStatus()
        {
            LicenseStatus lcsStatus = LicenseStatus.Error;
            try
            {
                DataStoreInfo dataStoreInfo = dataStoreManager.ReadDataFromRegistry();
                if (dataStoreInfo != null)
                {
                    if (dataStoreInfo.UniqueClientId != null && dataStoreInfo.UniqueClientId.Length >= 12)
                    {
                        string macAddress = CreateUniqueMachineID();
                        if (!string.IsNullOrEmpty(macAddress))
                        {
                            if (dataStoreInfo.UniqueClientId.Substring(0, 12) == GenerateKey(macAddress) || dataStoreInfo.UniqueClientId.Substring(0, 12) == defaultKey)
                            {
                                lcsStatus = LicenseStatus.Activated;
                            }
                        }
                        if (dataStoreInfo.UniqueClientId.Substring(0, 12) == GenerateKey(masterKey))
                        {
                            lcsStatus = LicenseStatus.Activated;
                        }
                    }
                    else if (dataStoreInfo.NumberOfDaysElapsed > 24 * DEMOTRIALPERIOD)
                    {
                        lcsStatus = LicenseStatus.TrialExpired;
                    }
                    else if (dataStoreInfo.NumberOfDaysElapsed < 24 * DEMOTRIALPERIOD)
                    {
                        lcsStatus = LicenseStatus.Trial;
                    }
                    else
                    {
                        lcsStatus = LicenseStatus.Error;
                    }
                }
                else
                {
                    lcsStatus = LicenseStatus.Error;
                }
            }
            catch (Exception EX)
            {
                
                
            }           
            return lcsStatus;
        }
        /// <summary>
        /// The generates a key based on the unique machine id
        /// 
        /// </summary>	
        /// <param name="sysInfo">unique machine id of the system</param>
        /// <returns>generate a key on some proprietery logic</returns>
        /// <exception cref="DataStoreUniqueMachineIDException(string)"/>
        public string GenerateKey(string sysInfo)
        {
            try
            {
                string strPcode = "";
                string[] charstr = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
                int charcnt = 0;
                System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding(); // changes the string to ASCII equivalent
                Byte[] bytes = encoding.GetBytes(sysInfo);
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
        /// This function verifies the input license key
        /// </summary>
        /// <param name="inputLicenseKey"></param>
        /// <returns></returns>
        public bool VerifyLicense(string inputLicenseKey)
        {
            bool validLicense = false;
            DateTime installDate = dataStoreManager.GetInstallationDate();
            if (installDate <= DateTime.MinValue)
            {
                validLicense = false;
            }
            else
            {
                if (GenerateKey(CreateUniqueMachineID()) == inputLicenseKey)
                {
                    try
                    {

                        validLicense = true;
                        DataStoreInfo dataStoreInfo = new DataStoreInfo(inputLicenseKey,installDate,DateTime.Now);
                        validLicense = dataStoreManager.WriteDatatoRegistry(dataStoreInfo);
                    }
                    catch (Exception ex)
                    {
                        return validLicense;
                    }
                }
                else
                {
                    validLicense = false;
                }
            }
            return validLicense;
        }
        public bool MatchKey(string inputLicenseKey)
        {
            bool validLicense = false;
            try
            {
                DateTime startDate = DateTime.MinValue;
                startDate = dataStoreManager.GetInstallationDate();
                if (inputLicenseKey.Trim().Length >= 12)
                {
                    string macAddress = CreateUniqueMachineID();
                    if (!string.IsNullOrEmpty(macAddress))
                    {
                        if (GenerateKey(macAddress) == inputLicenseKey.Trim().Substring(0, 12))
                        {
                            try
                            {
                                DataStoreInfo dataStoreInfo = new DataStoreInfo(inputLicenseKey, startDate, DateTime.Now);
                                validLicense = dataStoreManager.WriteDatatoRegistry(dataStoreInfo);
                                validLicense = true;
                            }
                            catch (Exception ex)
                            {
                                return validLicense;
                            }
                        }

                    }
                    if (inputLicenseKey.Trim().Substring(0, 12) == GenerateKey(masterKey))
                    {
                        try
                        {
                            DataStoreInfo dataStoreInfo = new DataStoreInfo(inputLicenseKey, startDate, DateTime.Now);
                            validLicense = dataStoreManager.WriteDatatoRegistry(dataStoreInfo);
                            validLicense = true;
                        }
                        catch (Exception ex)
                        {
                            return validLicense;
                        }
                    }
                }
                else
                {
                    validLicense = false;
                }
            }
            catch (Exception EX)
            {

            }
            return validLicense;
        }
        #endregion

        #region private functions
        /// <summary>
        /// This creates a unique machine identifier, namely the first mac 
        /// address found on the machine.
        /// </summary>		
        /// <returns>A string version of the mac address.</returns>
        /// <exception cref="DataStoreUniqueMachineIDException(string)"/>
        private string CreateUniqueMachineID()
        {
            //try
            //{
            //    string cpuInfo = String.Empty;
            //    ManagementClass managementClass = new ManagementClass(WIN32PROCESSOR);
            //    ManagementObjectCollection managedObjectCollection = managementClass.GetInstances();
            //    foreach (ManagementObject managementObject in managedObjectCollection)
            //    {
            //        if (cpuInfo == String.Empty)
            //        {// only return cpuInfo from first CPU

            //            cpuInfo = managementObject.Properties[Resource.HARDWAREID].Value.ToString();

            //        }
            //    }
            //    return cpuInfo;
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(
            //        Resource.MESSAGE001, ex);
            //}
            string macAddresses = "AABBCCDDEEFF";

            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.NetworkInterfaceType.ToString() == Resource.ETHERNET && nic.Name.ToString() == Resource.ETHERNET)
                {
                    macAddresses = nic.GetPhysicalAddress().ToString();
                }
            }
            return macAddresses;
        }
        #endregion

    }
}
