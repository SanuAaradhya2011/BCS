/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 	 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| 
 */
using System;
using System.Configuration;
using System.IO;
using System.Xml;
using LNG.Framework.Utility.DataBaseType;
using LNG.Framework.Entity;
using System.Security.Cryptography;
using LNG.Framework.Utility.Cryptography;
using System.Data;
using System.Windows.Forms;

namespace LNG.Framework.Utility
{
    /// <summary>
    /// This class is used to keep the value of configuration settings.
    /// </summary>
    public class ConfigInfo
    {
        static string dateFormat = string.Empty;
        static string signatureInfo = string.Empty;
        private const string CompanyCode = "CompanyCode";
        private static string rightId = "111111110";
        private static bool isOnline = false;
        static string signatureDataLength = string.Empty;
        static DisplayProgrammingTypes displayParameterVariant = DisplayProgrammingTypes.OneByte;



        public static bool IsOnline
        {
            get { return isOnline; }
            set { isOnline = value; }
        }

        /// <summary>
        /// This method is used to return the database type.
        /// </summary>
        /// <returns>DBType</returns>
        public static DBType GetDBType()
        {
            DBType dbType = 0;
            string dbtype = ConfigSettings.GetValue("DBTYPE");
            switch (dbtype)
            {
                case "MS_Access":
                    dbType = DBType.MS_Access;
                    break;
                case "Oracle":
                    dbType = DBType.Oracle;
                    break;
                case "MS_SQL":
                    dbType = DBType.MS_SQL;
                    break;
                case "My_SQL":
                    dbType = DBType.My_SQL;
                    break;
            }
            return dbType;
        }

        public static string RightID
        {
            get
            {
                return rightId;
            }
            set
            {
                rightId = value;
            }
        }



        /// <summary>
        /// This method is used to Get the Connection String.
        /// </summary>
        /// <returns>string</returns>
        public static string GetConnectionString()
        {
            string val = ConfigSettings.GetValue("ConnectionString");
            string appType = ConfigSettings.GetValue("ApplicationType");
            val = val.Replace("DND", appType);
            return val;
        }
        /// <summary>
        /// VBM - 20/03/3013
        /// This method is used to Get the Connection String for stored procedures.
        /// </summary>
        /// <returns>string</returns>
        public static string GetConnectionStringForSp()
        {
            string val = ConfigSettings.GetValue("ConnectionString");
            string dbName = ConfigSettings.GetValue("DataBaseName");
            val = val.Replace("DND", dbName);
            return val;
        }
        public static string RootLog()
        {
            return ConfigSettings.GetValue("RootLog");
        }
        public static int GetFDLTamperDataSize()
        {
            int i = 0;
            try
            {
                i = Convert.ToInt32(ConfigSettings.GetValue("FDLTamperDataSize").Trim());
            }
            catch (Exception)
            {
                throw new Exception("FDLTamperDataSize value is absent or incorrect in configuration file.");
            }
            return i;

        }
        public static int GetFDLGeneralDataSize()
        {
            int generalSize = 0;
            try
            {
                generalSize = Convert.ToInt32(ConfigSettings.GetValue("FDLGeneralDataSize").Trim());
            }
            catch (Exception)
            {
                throw new Exception("FDLGeneralDataSize value is absent or incorrect in configuration file.");
            }
            return generalSize;

        }
        public static int GetFDLInstantDataSize()
        {
            int instantSize = 0;
            try
            {
                instantSize = Convert.ToInt32(ConfigSettings.GetValue("FDLInstantDataSize").Trim());
            }
            catch (Exception)
            {
                throw new Exception("FDLInstantDataSize value is absent or incorrect in configuration file.");
            }
            return instantSize;

        }
        /// <summary>
        /// Company code for MIOS
        /// </summary>
        /// <returns></returns>
        public static string GetCompanyCode()
        {
            string companyCode = string.Empty;
            try
            {
                companyCode = ConfigSettings.GetValue(CompanyCode).Trim();
            }
            catch (Exception)
            {
                throw new Exception("Company code is absent in configuration file.");
            }
            return companyCode;

        }
        public static int GetFDLPhasorDataSize()
        {
            int phasorSize = 0;
            try
            {
                phasorSize = Convert.ToInt32(ConfigSettings.GetValue("FDLPhasorDataSize").Trim());
            }
            catch (Exception)
            {
                throw new Exception("FDLPhasorDataSize value is absent or incorrect in configuration file.");
            }
            return phasorSize;

        }
        public static int GetMidNightDataSize()
        {
            int midNightSize = 0;
            try
            {
                midNightSize = Convert.ToInt32(ConfigSettings.GetValue("FDLMidNightDataSize").Trim());
            }
            catch (Exception)
            {
                throw new Exception("FDLMidNightDataSize value is absent or incorrect in configuration file.");
            }
            return midNightSize;

        }
        public static int GetAnomalyDataSize()
        {
            int anomalySize = 0;
            try
            {
                anomalySize = Convert.ToInt32(ConfigSettings.GetValue("FDLAnomalyDataSize").Trim());
            }
            catch (Exception)
            {
                throw new Exception("FDLAnomalyDataSize value is absent or incorrect in configuration file.");
            }
            return anomalySize;

        }
        /* GKG JVVNL Current TOU Read */
        public static int GetTOUDataSize()
        {
            int touSize = 0;
            try
            {
                touSize = Convert.ToInt32(ConfigSettings.GetValue("FDLTOUDataSize").Trim());
            }
            catch (Exception)
            {
                throw new Exception("FDLTOUDataSize value is absent or incorrect in configuration file.");
            }
            return touSize;

        }
        /* GKG JVVNL Current TOU Read */
        public static int GetFDLLSDataSize()
        {
            int i;
            try
            {
                i = Convert.ToInt32(ConfigSettings.GetValue("FDLLSDataSize").Trim());
            }
            catch (Exception)
            {
                throw new Exception("FDLTamperDataSize value is absent or incorrect in configuration file.");
            }
            return i;
        }
        public static int GetFDLBillingDataSize()
        {
            int i;
            try
            {
                i = Convert.ToInt32(ConfigSettings.GetValue("FDLBillingDataSize").Trim());
            }
            catch (Exception)
            {
                throw new Exception("FDLTamperDataSize value is absent or incorrect in configuration file.");
            }
            return i;
        }
        public static string GetLocation()
        {
            string path = ConfigSettings.GetValue("FileLocation");
            if (string.IsNullOrEmpty(path))
                path = string.Concat(Path.GetDirectoryName(Application.ExecutablePath), @"\CAB Readout\");
            return path;
        }
        public static bool IsGSMConnected()
        {
            string status = ConfigSettings.GetValue("GSMConnected");
            if (status.Equals("True"))
            {
                channelType = ChannelType.GSM;
                return true;
            }
            else
                return false;
        }
        public static string GetTOULocation()
        {
            string path = ConfigSettings.GetValue("TOUFileLocation");
            if (string.IsNullOrEmpty(path))
                path = string.Concat(Path.GetDirectoryName(Application.ExecutablePath), @"\TOU\");
            return path;
        }
        public static string EncryptFile(string strFile)
        {
            Rijndael RijndaelAlg = Rijndael.Create();
            CryptoProvider objMyCrypo = new CryptoProvider();
            return objMyCrypo.EncryptString(strFile);
        }
        public static string CheckOrCreatePath()
        {
            string appPath = GetLocation();
            if (Directory.Exists(appPath))
                return appPath;
            else
                Directory.CreateDirectory(appPath);
            return appPath;
        }
        public static string LocationType()
        {
            return ConfigSettings.GetValue("LocationType");
        }
        public static VersionInfo GetVersion()
        {
            VersionInfo info = VersionInfo.Version1_0;
            string vinfo = ConfigSettings.GetValue("VersionString");
            if (!string.IsNullOrEmpty(vinfo))
            {
                vinfo = vinfo.Trim();
                double versionNumber = double.Parse(vinfo);
                if (versionNumber == 1.0)
                    info = VersionInfo.Version1_0;
                else if (versionNumber == 1.1)
                    info = VersionInfo.Version1_1;
                else if (versionNumber == 1.2)
                    info = VersionInfo.Version1_2;
                else if (versionNumber == 1.3)
                    info = VersionInfo.Version1_3;
                else if (versionNumber == 1.4)
                    info = VersionInfo.Version1_4;
                else if (versionNumber == 1.5)
                    info = VersionInfo.Version1_5;
            }
            return info;
        }
        /// <summaryLogID>
        /// This variable is used to hold the value of Logged user id.
        /// </summary>
        public static int UserInformationID;
        public static long LogID;
        /// <summary>
        /// This variable is used to hold the value of client Machine Name.
        /// </summary>
        public static string MachineName = Environment.MachineName;
        /// <summary>
        /// This variable is used to get the channel type.
        /// </summary>
        private static ChannelType channelType = ChannelType.RS232;
        /// <summary>
        /// This property is used to get & set the channel type.
        /// </summary>
        public static ChannelType ChannelType
        {
            get
            {
                return channelType;
            }
            set { channelType = value; }
        }
        /// <summary>
        /// This method is used to get active meter type.
        /// </summary>
        public static ApplicationType GetApplicationType()
        {
            string appType = string.Empty;
            //try
            //{
            appType = ConfigSettings.GetValue("ApplicationType");
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Please RUN setup with administrator rights !!!");
            //    Application.Exit();

            //}
            appType = appType.Trim();
            if (!string.IsNullOrEmpty(appType))
            {
                if (appType.Equals("IEC_LTCT_650"))
                    return ApplicationType.IEC_LTCT_650;
                else if (appType.Equals("IEC_RUBY_250"))
                    return ApplicationType.IEC_RUBY_250;
                else if (appType.Equals("IEC_STARLIGHT_150"))
                    return ApplicationType.IEC_STARLIGHT_150;
                else if (appType.Equals("IEC_ZCE"))
                    return ApplicationType.IEC_ZCE;
                else if (appType.Equals("DLMS_LTCT_650"))
                    return ApplicationType.DLMS_LTCT_650;
                else if (appType.Equals("DLMS_RUBY_250"))
                    return ApplicationType.DLMS_RUBY_250;
                else if (appType.Equals("DLMS_STARLIGHT_150"))
                    return ApplicationType.DLMS_STARLIGHT_150;
            }
            return ApplicationType.IEC_LTCT_650;
        }

        public static string GetPortName()
        {
            return ConfigSettings.GetValue("PortName");
        }
        public static string GetBaudRate()
        {
            return ConfigSettings.GetValue("BaudRate");
        }
        public static string GetLocalMode()
        {
            return ConfigSettings.GetValue("CommunicationMode");
        }

        /// <summary>
        /// This method is used to find whether encryption of CAB file is enabled or not.
        /// </summary>
        /// <returns>string</returns>
        public static bool IsEncryption()
        {
            string temp = ConfigSettings.GetValue("EncryptFile");
            bool flag = false;
            if (!string.IsNullOrEmpty(temp))
            {
                flag = Convert.ToBoolean(temp);
            }
            return flag;
        }

        public static string DecryptFile(string fileContent)
        {
            Rijndael RijndaelAlg = Rijndael.Create();
            CryptoProvider objMyCrypo = new CryptoProvider();
            return objMyCrypo.DecryptString(fileContent);
        }
        public static bool IsValidCheckSum(string strInput)
        {
            if (strInput == "\0")
                return false;
            int receivedBCC = strInput[strInput.Length - 1];
            CryptoProvider cryptoProvider = new CryptoProvider();
            int calculatedBCC = cryptoProvider.CallBcc(strInput.Substring(0, strInput.Length - 1));
            if (receivedBCC == calculatedBCC)
                return true;
            else
                return false;
        }
        public static string DateFormat()
        {
            if (string.IsNullOrEmpty(dateFormat))
            {
                dateFormat = ConfigSettings.GetValue("DateFormat");
            }
            return dateFormat;
        }

        public static string FileNamingConvention()
        {
            return ConfigSettings.GetValue("FileNamingConvention");
        }
        public static TenderType GetTenderType()
        {
            if (ConfigSettings.GetValue("TenderType").ToString() == "JUSCO")
                return TenderType.JUSCO;
            else if (ConfigSettings.GetValue("TenderType").ToString() == "DGVCL")
                return TenderType.DGVCL;
            else
                return TenderType.UGVCL;
        }
        public static string ActiveMeterDataId
        {
            get;
            set;
        }
        public static string ActiveFileType
        {
            get;
            set;
        }
        public static bool GraphStatus
        {
            get;
            set;
        }
        public static string FirmwareVersion
        {
            get;
            set;
        }
        public static string MeterModel
        {
            get;
            set;
        }
        //User Story 478245. Voltage Rating change to 63.5 V for HK meter
        public static string SignatureDataLength
        {
            get { return signatureDataLength; }
            set { signatureDataLength = value; }
        }

        public static string SignatureInfo
        {
            get {return signatureInfo;}
            set { signatureInfo = value; }
        }
        public static string ActiveFirmwareVersion
        {
            get;
            set;
        }
        public static int EnergyResolution
        {
            get;
            set;
        }
        public static int DemandResolution
        {
            get;
            set;
        }
        public static int MeterIdLength
        {
            get;
            set;
        }
        public static string ActiveMeterType
        {
            get;
            set;
        }
        public static byte BillingTariffCount
        {
            get;
            set;
        }

        public static DataTable AutoNumberedTable(DataTable SourceTable)
        {
            DataTable ResultTable = new DataTable();
            DataColumn AutoNumberColumn = new DataColumn();
            AutoNumberColumn.ColumnName = "SNo";
            AutoNumberColumn.DataType = typeof(int);
            AutoNumberColumn.AutoIncrement = true;
            AutoNumberColumn.AutoIncrementSeed = 1;
            AutoNumberColumn.AutoIncrementStep = 1;
            ResultTable.Columns.Add(AutoNumberColumn);
            ResultTable.Merge(SourceTable);
            return ResultTable;
        }

        // For Meter Accuracy Check
        public static string GetMeterAccuracyCheck()
        {
            return ConfigSettings.GetValue("MeterAccuracyCheck");
        }

        /// <summary>
        /// This method is used to return the database type for GPRS Adapter.
        /// </summary>
        /// <returns>DBType</returns>
        public static DBType GetGPRSAdapterDBType()
        {
            DBType dbType = 0;
            string dbtype = ConfigSettings.GetValue("GPRSDbType");
            switch (dbtype.ToUpper())
            {
                case "MS_ACCESS":
                    dbType = DBType.MS_Access;
                    break;
                case "ORACLE":
                    dbType = DBType.Oracle;
                    break;
                case "SQLSERVER":
                    dbType = DBType.MS_SQL;
                    break;
                case "MY_SQL":
                    dbType = DBType.My_SQL;
                    break;
            }
            return dbType;
        }

        public static DisplayProgrammingTypes DisplayProgrammingVariant
        {
            get { return displayParameterVariant; }
            set { displayParameterVariant = value; }
        } 
    }
}

