//using System;
//using Microsoft.Win32;					//used to access the registry
//using System.IO;								//used in isolated storage
//using System.IO.IsolatedStorage;//used for isolated Storage
//using System.Xml;								//used to write and read the xml document.
//using System.Xml.Serialization;	//used to actually write the xml
//using System.Text;
//using System.Configuration;			//used to get the Trial key from the 
//using System.Management;				//used to create the unique machine ID.
//using System.Runtime.Serialization;
//using System.Security.Cryptography;
//using System.Runtime.Serialization.Formatters.Binary;
//using CAB.DataProtection;
//using CAB.License.Framework;
//using CAB.Framework;

//namespace CAB.License.DataStore
//{
//    public class DataStore : IDataStore
//    {
//        static DataStore dataStore;
//        private const string appDataPath = "SOFTWARE\\Cabcon\\AppData";
//        /// <summary>The name of the file written to isolated storage</summary>						
//        protected const string DATA_STORE_NAME = "DataStore.dat";
//        private IsolatedStorageFileStream isfs;
//        private DataStoreInfo storageObject;
//        IsolatedStorageFile isf = IsolatedStorageFile.GetMachineStoreForApplication();
//        public static DataStore Instance()
//        {
//            if (dataStore == null)
//            {
//               dataStore = new DataStore();
//            }
//            return dataStore;
//        }
//        public DataStore()
//        {
//            storageObject = new DataStoreInfo();
//        }
//        public bool StorageFileExists()
//        {
     
//            bool fileExists = false;
//            string[] fileNames = isf.GetFileNames(DATA_STORE_NAME);
//            foreach (string file in fileNames)
//            {
//                if (file == DATA_STORE_NAME)
//                {
//                    fileExists = true;
//                    break;
//                }
//                else
//                {
//                    fileExists = false;
//                }
//            }
//            return fileExists;
//        }
//        private void read()
//        {
//            bool fileExists = false;

//            //load the stored data into the inner class so we have easy access to it.	

//            fileExists = StorageFileExists();         
          

//            DataProtector dataProtector = new DataProtector(DataStoreType.USE_MACHINE_STORE);

//            try
//            {
//                isfs = new IsolatedStorageFileStream(DATA_STORE_NAME,
//                                   FileMode.Open,
//                                   FileAccess.Read,
//                                   isf);
//            }
//            catch
//            {
//                //Error while reading in the storage file
//                throw new Exception("The inner exception was thrown while opening the islated storage file stream.");
//            }

//            //Read the file and put the data in our storage class.
//            try
//            {
//                byte[] byteBuffer = new byte[isfs.Length];

//                while (isfs.Position < isfs.Length)
//                {
//                    byteBuffer[isfs.Position] = (byte)isfs.ReadByte();
//                }


//                byteBuffer = dataProtector.Decrypt(byteBuffer, null);

//                MemoryStream memStream = new MemoryStream(byteBuffer);

//                XmlTextReader xtr = new XmlTextReader(memStream);
//                XmlSerializer xs = new XmlSerializer(typeof(DataStoreInfo));
//                storageObject = (DataStoreInfo)xs.Deserialize(xtr);
//            }
//            catch
//            {
//                //Error while reading in the storage file
//                throw new Exception("The inner exception was thrown while reading data from the isolated storage datastore.");
//            }
//            finally
//            {
//                isfs.Close();
//            }
//        }


//        /// <summary>
//        /// Reads all of the data in from the isolated storage
//        /// and stores it in the <see cref="storage"/> object.  If the isolated
//        /// storage file is not found it attempts to contact the web service to
//        /// initiate the data store.
//        /// </summary>
//        /// <exception cref="DataStoreAppConfigException(string)"/>
//        /// <exception cref="DataStoreUniqueMachineIDException(string)"/>
//        /// <exception cref="DataStoreWriteException(string)"/>
//        /// <exception cref="DataStoreReadException(string)"/>
//        public void ReadDataFromIsolatedStorage()
//        {
//            IsolatedStorageFile isoStore = IsolatedStorageFile.GetMachineStoreForAssembly();
//            bool fileExists = false;

//            string[] fileNames = isoStore.GetFileNames(DATA_STORE_NAME);
//            foreach (string file in fileNames)
//            {
//                if (file == DATA_STORE_NAME)
//                {
//                    fileExists = true;
//                }
//            }


//            if (!fileExists)
//            {
//                //Logger.Log("DataStore : ReadDataFromIsolatedStorage : No DataStore- Contacting Server");

//                DataStoreInfo dataStoreInfo = new DataStoreInfo(CreateUniqueMachineID());

//                WriteDataToIsolatedStorage();

//            }
//            else
//            {

//                try
//                {
//                    read();                   
//                    DataStoreInfo dataStoreInfo = new DataStoreInfo(CreateUniqueMachineID());
//                    WriteDataToIsolatedStorage();
                 
//                }
//                catch (Exception ex)
//                {
//                    //Logger.Log("DataStore : ReadDataFromIsolatedStorage : Error reading store : " + ex.Message);
//                    DataStoreInfo dataStoreInfo = new DataStoreInfo(CreateUniqueMachineID());

//                    WriteDataToIsolatedStorage();
//                }

//            }
//        }
//        /// <summary>
//        /// This creates a unique machine identifier, namely the first mac 
//        /// address found on the machine.
//        /// </summary>		
//        /// <returns>A string version of the mac address.</returns>
//        /// <exception cref="DataStoreUniqueMachineIDException(string)"/>
//        private string CreateUniqueMachineID()
//        {


//            try
//            {
//                string cpuInfo = String.Empty;
//                ManagementClass mc = new ManagementClass("Win32_Processor");
//                ManagementObjectCollection moc = mc.GetInstances();
//                foreach (ManagementObject mo in moc)
//                {
//                    if (cpuInfo == String.Empty)
//                    {// only return cpuInfo from first CPU

//                        cpuInfo = mo.Properties["ProcessorId"].Value.ToString();

//                    }
//                }
//                return cpuInfo;
//            }
//            catch (Exception ex)
//            {
//                throw new Exception(
//                    "Error while calculating the unique machine ID.", ex);
//            }
//        }
//        /// <summary>
//        ///  Writes the storage object out to isolated storage
//        ///  filename specified in: <see cref="DATA_STORE_NAME"/>
//        /// </summary>
//        /// <exception cref="DataStoreWriteException(string)"/>		
//        public void WriteDataToIsolatedStorage()
//        {
//            try
//            {
//                 MemoryStream memStream = new MemoryStream();
//                 DataStoreInfo dataStoreInfo = new DataStoreInfo(CreateUniqueMachineID());
//                 storageObject = dataStoreInfo;
//                XmlSerializer xs = new XmlSerializer(typeof(DataStoreInfo));
//                xs.Serialize(memStream, storageObject);

//                DataProtector dataProtector = new DataProtector(DataStoreType.USE_MACHINE_STORE);
//                byte[] byteBuffer = new byte[memStream.Length];

//                byteBuffer = memStream.GetBuffer();

//                byteBuffer = dataProtector.Encrypt(byteBuffer, null);



//                IsolatedStorageFile isf = IsolatedStorageFile.GetMachineStoreForAssembly();
//                IsolatedStorageFileStream isfstream = new
//                    IsolatedStorageFileStream(DATA_STORE_NAME,
//                    FileMode.Create,
//                    FileAccess.Write,
//                    isf);

//                isfstream.Write(byteBuffer, 0, byteBuffer.Length);

//                isfstream.Close();

//            }
//            catch(Exception ex)
//            {
//                throw new Exception("The inner exception was thrown while writing data to isolated storage." + ex.Message);
//            }
//        }
      
//    }
//}
