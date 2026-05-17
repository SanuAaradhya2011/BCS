using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.Reflection;
using System.Globalization;
using System.Net.NetworkInformation;
using Microsoft.Win32;
namespace LicenseKeyGenerator
{
    class RegisteryEntry
    {     
        string regSubDirectoryL1 = "Cabcon";
        string regSubDirectoryL2 = "AppData";       

        private string appDataPath = string.Empty;
        public enum RegistryComponentType{RegKey=0, RegDT=1};

        public LicenseFileEntity WriteAppDataInRegistry(LicenseFileEntity objLicEnty)
        {
            try
            {
                RegistryKey key = Registry.LocalMachine.OpenSubKey("Software", true);
                key.CreateSubKey(regSubDirectoryL1);
                key = key.OpenSubKey(regSubDirectoryL1, true);
                key.CreateSubKey(regSubDirectoryL2);
                key = key.OpenSubKey(regSubDirectoryL2, true);
                key.Close();
                return objLicEnty;
            }
            catch (Exception ex)
            {
                if (ex.Message == "Requested registry access is not allowed.") objLicEnty.FileError = "Please run the program as an administrator!";
                else objLicEnty.FileError = ex.ToString();
                return objLicEnty;
            }
        }    
                           



    }
}
