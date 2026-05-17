using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CAB.Framework;
using CAB.License.DataStore;
namespace CAB.License
{
    /// <summary>
    /// Interface for licenseManager
    /// </summary>
    public interface ILicenseManager
    {
        LicenseStatus GetLicenseStatus();
        void WriteClientInformationToFile(string fileName, DataStoreInfo dataStoreInfo);
        string GenerateKey(string sysInfo);
        DataStoreInfo ReadClientInformationFromFile(string fileName);
        DataStoreInfo ReadClientInformationFromRegistry();
        bool VerifyLicense(string inputLicenseKey);
        bool  MatchKey(string inputLicenseKey);
    }
}
