using System;
namespace CAB.License.DataStore
{
    /// <summary>
    /// Interface used for data store manager
    /// </summary>
    public interface IDataStoreManager
    {
        void EncryptObjectandWriteToFile(DataStoreInfo dataStoreInfo, string fileName);
        DateTime GetInstallationDate();
        DataStoreInfo ReadDataFromRegistry();
        DataStoreInfo ReadFileandDecryptObject(string fileName);
        bool WriteDatatoRegistry(DataStoreInfo dataStoreInfo);
    }
}
