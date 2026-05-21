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
using CAB.IECFramework.Utility.DataBaseType;
using CAB.IECFramework.Entity;
using System.Security.Cryptography;
using CAB.IECFramework.Utility.Cryptography;
using System.Data;
using System.Windows.Forms;

namespace CAB.IECFramework.Utility
{
    /// <summary>
    /// This class is used to keep the value of configuration settings.
    /// </summary>
    public class ConfigInfo
    {

        private static string signatureInfo = string.Empty;


        public static string SignatureInfo
        {
            get
            {
                return signatureInfo;
            }

            set
            {
                signatureInfo = value;
            }
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
        public static AppType GetApplicationType()
        {
            AppType appType = 0;
            string dbtype = ConfigSettings.GetValue("ApplicationType");
            switch (dbtype)
            {  
                case "E650_DLMS":
                    appType = AppType.E650_DLMS; // LTCT Meter & DLMS Protocol
                    break;
                case "E650_IEC":
                    appType = AppType.E650_IEC; //LTCT Meter & IEC Protocol
                    break;
                case "E250_DLMS":
                    appType = AppType.E250_DLMS; //Ruby Meter & DLMS Protocol   
                    break;
                case "E250_IEC":
                    appType = AppType.E250_IEC;  //Ruby Meter & IEC Protocol  
                    break;
             case "E150_DLMS":
                    appType = AppType.E150_DLMS; //Starlight Meter & DLMS Protocol   
                    break;
             case "E150_IEC":
                    appType = AppType.E150_IEC; //Starlight Meter & IEC Protocol 
                    break;
             case "ZCE_IEC":
                    appType = AppType.ZCE_IEC;  //ZCE Meter & IEC Protocol
                    break;
            }
            return appType;
        }
        public static long LogID;
        /// <summary>
        /// This method is used to Get the Connection String.
        /// </summary>
        /// <returns>string</returns>
        public static string GetConnectionString()
        {
            return ConfigSettings.GetValue("ConnectionString");
        }


        #region Get TimeDelayInSeconds
        /// <summary>
        /// Function added by Vivek on 18 August 2011.
        /// </summary>
        /// <returns></returns>
        public static string GetTimeDelayInSeconds()
        {
            return ConfigSettings.GetValue("TimeDelayInSeconds");
        }
        #endregion


        public static string RootLog()
        {
            return ConfigSettings.GetValue("RootLog");
        } 
        public static string GetLocation()
        {
            string path=ConfigSettings.GetValue("FileLocation");
            if(string.IsNullOrEmpty(path))
                path=string.Concat(Path.GetDirectoryName(Application.ExecutablePath), @"\CAB Readout\");
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
        public static string SimNumber
        {
            get;
            set;
        }

        public static string CheckOrCreatePath()
        {
            string appPath =  GetLocation();
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
        
        /// <summary>
        /// This method is used to retrieve the version information.
        /// </summary>
        /// <returns>VersionInfo</returns>
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
        /// <summary>
        /// This variable is used to hold the value of Logged user id.
        /// </summary>
        public static int UserInformationID;
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
        public static ApplicationType GetMeterType()
        {
            ApplicationType info = ApplicationType.LTCT3PHASE;
            string mtype = ConfigSettings.GetValue("MeterType");
            if (!string.IsNullOrEmpty(mtype))
            {
                mtype = mtype.Trim();
                if (mtype.Equals("BCS3PHASE"))
                    info = ApplicationType.LTCT3PHASE;
                else if (mtype.Equals("RUBY3PHASE"))
                    info = ApplicationType.RUBY3PHASE;
                else if (mtype.Equals("STARLIGHT1PHASE"))
                    info = ApplicationType.STARLIGHT1PHASE;
            }
            return info;
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
        public static string GetLoadSurvey()
        {
            return ConfigSettings.GetValue("LoadSurveyConfiguration");
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
            return ConfigSettings.GetValue("DateFormat");
        }

        public static string FileNamingConvention()
        {
            return ConfigSettings.GetValue("FileNamingConvention");
        }
        public static double GetRTCTimeFrame()
        {
            string[] timeFrameSetting= ConfigSettings.GetValue("RTCTimeFrame").Split(':');
            return ((Convert.ToDouble(timeFrameSetting[0])) + ((Convert.ToDouble(timeFrameSetting[1]))/60.0));
        }
        public static string GetRTCTimeFrameStringValue()
        {
            return ConfigSettings.GetValue("RTCTimeFrame");
        }

        public static string ActiveMeterDataId
        {
            get;
            set;
        }
        public static string FileUpload_ID
        { get; set; }
        public static bool GraphStatus
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
        // To fetch BCS software version
        public static string GetBCSVersion()
        {
            return ConfigSettings.GetValue("BCSSoftwareVersion");
        }

    }
}
