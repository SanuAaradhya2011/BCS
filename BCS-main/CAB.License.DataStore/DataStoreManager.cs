using System;
using Microsoft.Win32;					//used to access the registry
using System.IO;								//used in isolated storage
using System.IO.IsolatedStorage;//used for isolated Storage
using System.Xml;								//used to write and read the xml document.
using System.Xml.Serialization;	//used to actually write the xml
using System.Text;
using System.Configuration;			//used to get the Trial key from the 
			//used to create the unique machine ID.
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Runtime.Serialization.Formatters.Binary;
using CAB.Framework;
using CAB.DataProtection;
using System.Runtime.InteropServices;
namespace CAB.License.DataStore
{
    /// <summary>
    /// This class is used for data storing operations in context of licensing.
    /// </summary>
    public class DataStoreManager : IDataStoreManager
    {
        #region local variables
        private IKeyedDataProtection dataProtector;
        private DataStoreInfo dataStoreInfo = null;
        private RegistryManager registryManager = null;
        [DllImport("msi.dll", CharSet = CharSet.Unicode)]
        static extern Int32 MsiGetProductInfo(string product, string property, [Out] StringBuilder valueBuf, ref Int32 len);
        private bool is64BitSystem = false;
        private string appDataPath = string.Empty;
        #endregion

        #region functions
        public DataStoreManager()
        {
            dataProtector = new KeyedDataProtection();
            registryManager = new RegistryManager(Registry.LocalMachine);
            if (IsWow64BitSystem)
            {
                appDataPath = Resource.APPDATAPATH64;
            }
            else
            {
                appDataPath = Resource.APPDATAPATH;
            }
        }
        /// <summary>
        /// Gets whether the system is of 64 bits or not
        /// </summary>
        /// <returns></returns>
        private bool IsWow64BitSystem
        {
            get
            {
                //If the pointer size is of 8 bytes = 64 bit
                if (IntPtr.Size == 8)
                {
                    is64BitSystem = true;
                }
                return is64BitSystem;
            }
        }
        /// <summary>
        /// This funtion writes the specified object to registry
        /// </summary>
        /// <param name="dataStoreInfo"></param>
        /// <returns></returns>
        public bool WriteDatatoRegistry(DataStoreInfo dataStoreInfo)
        {
            bool isWriteOK = false;
            
            try
            {

                MemoryStream memStream = new MemoryStream();
                XmlSerializer serializer = new XmlSerializer(typeof(DataStoreInfo));
                serializer.Serialize(memStream, dataStoreInfo);

                byte[] byteBuffer = new byte[memStream.Length];
                byteBuffer = memStream.GetBuffer();
                registryManager.WriteKey(appDataPath, Resource.CLIENTINFO, dataProtector.EncryptString(Convert.ToBase64String(byteBuffer)));
                isWriteOK = true;
                return isWriteOK;
            }
            catch (Exception ex)
            {
                return isWriteOK;
            }

        }
        /// <summary>
        /// This funtions writes the specified object to file
        /// </summary>
        /// <param name="dataStoreInfo"></param>
        /// <param name="fileName"></param>
        public void EncryptObjectandWriteToFile(DataStoreInfo dataStoreInfo,string fileName)
        {
            FileStream fileStream = new FileStream(fileName, FileMode.Create);
            StreamWriter streamWriter = new StreamWriter(fileStream);
            MemoryStream memStream = new MemoryStream();
            XmlSerializer serializer = new XmlSerializer(typeof(DataStoreInfo));
            try
            {
                serializer.Serialize(memStream, dataStoreInfo);
                byte[] byteBuffer = new byte[memStream.Length];
                byteBuffer = memStream.GetBuffer();
                streamWriter.Write(dataProtector.EncryptString(Convert.ToBase64String(byteBuffer)));
            }
            catch (Exception ex)
            {
                // do nothing for the moment
            }
            finally
            {
                streamWriter.Close();
                fileStream.Close();
            }
        }
        /// <summary>
        /// This function gets the object from specified file.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public DataStoreInfo ReadFileandDecryptObject(string fileName)
        {
            try
            {
                byte[] byteBuffer = null;
                StreamReader streamReader = new StreamReader(fileName);
                byteBuffer = Convert.FromBase64String(dataProtector.DecryptString(streamReader.ReadToEnd()));
                MemoryStream memStream = new MemoryStream(byteBuffer);
                XmlTextReader xtr = new XmlTextReader(memStream);
                XmlSerializer deserializer = new XmlSerializer(typeof(DataStoreInfo));
                dataStoreInfo = (DataStoreInfo)deserializer.Deserialize(xtr);
                streamReader.Close();
                return dataStoreInfo;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// This function reads data from registry
        /// </summary>
        /// <returns></returns>
        public DataStoreInfo ReadDataFromRegistry()
        {
            try
            {
                byte[] byteBuffer = null;
       
                byteBuffer = Convert.FromBase64String(dataProtector.DecryptString(registryManager.ReadKey(appDataPath,Resource.CLIENTINFO)));
                MemoryStream memStream = new MemoryStream(byteBuffer);
                XmlTextReader xtr = new XmlTextReader(memStream);
                XmlSerializer deserializer = new XmlSerializer(typeof(DataStoreInfo));
                dataStoreInfo = (DataStoreInfo)deserializer.Deserialize(xtr);
                return dataStoreInfo;
            }
            catch (Exception ex)
            {
                DataStoreInfo dataStoreInfo = new DataStoreInfo();
                dataStoreInfo.StartDateTime = GetInstallationDate();
                WriteDatatoRegistry(dataStoreInfo);
                return dataStoreInfo;
            }
        }
        /// <summary>
        /// This functions gets the installation date of BCS by calling com function msiGetProductInfo
        /// </summary>
        /// <returns></returns>
        public DateTime GetInstallationDate()
        {
            Int32 len = 512;
            string dateTimeString = string.Empty;
            DateTime installDate = DateTime.MinValue;
            try
            {
                var builder = new StringBuilder(len);
                MsiGetProductInfo(registryManager.ReadKey(appDataPath, Resource.PRODUCTCODE), Resource.INSTALLDATE, builder, ref len);
                dateTimeString = builder.ToString();
                if (!string.IsNullOrEmpty(dateTimeString))
                {
                    if (dateTimeString.Length >= 8)
                    {
                        installDate = new DateTime(Convert.ToInt32(dateTimeString.Substring(0, 4)), Convert.ToInt32(dateTimeString.Substring(4, 2)), Convert.ToInt32(dateTimeString.Substring(6, 2)));
                    }
                }
            }
            catch (Exception ex)
            {
                return installDate;
            }
            return installDate;
        }

      
        #endregion

    } 
}
