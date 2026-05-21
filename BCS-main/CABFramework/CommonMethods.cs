using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Windows.Forms;
namespace CAB.Framework.Utility
{
  public static  class CommonMethods
    {
      private static MeterDataTypes METERDATATYPE = MeterDataTypes.LTCT;

      private static string METERTYPE = string.Empty;
      private static int METERMODELNUMBER = 0;

      public static string getDisplayUnit(MeterDataTypes meterDataType)
        {
            if (Enum.Equals(meterDataType,MeterDataTypes.HTCT_MEGA))
            {  
                return "M"; 
            }
            else 
            { 
                return "k"; 
            }
        }

        public static string getDisplayHeaderText(string displayText)
        {
            return string.Format(displayText, getDisplayUnit(METERDATATYPE));
        }

        public static MeterDataTypes MeterDataType
        {
            get { return METERDATATYPE; }
            set { METERDATATYPE = value; }
        }

        public static string MeterType
        {
            get { return METERTYPE; }
            set { METERTYPE = value; }
        }


        public static int MeterModelNumber
        {
            get { return METERMODELNUMBER; }
            set { METERMODELNUMBER = value; }
        }
        
        static XmlDocument doc = new XmlDocument();
        static CommonMethods()
        {
            LoadFeatureConfigFile();

        }

      /// <summary>
      /// Load UtilityFeatures.config file in memory.
      /// </summary>
        static void LoadFeatureConfigFile()
        {
            string fileNames = string.Concat(Path.GetDirectoryName(Application.ExecutablePath)) + "\\" + "UtilityFeatures.config";
            try
            {
                FileInfo info = new FileInfo(fileNames);
                string path = info.FullName.ToString();
                doc.Load(path);
            }
            catch (Exception)
            {

            }
        }

      /// <summary>
      /// Function will return true if functionality is defined for passed utility name in config file 
      /// </summary>
      /// <param name="key"></param>
      /// <param name="utilityName"></param>
      /// <returns></returns>
        public static bool ShowFunctionalityToUtility(string key, string utilityName)
        {
            string keyValue = GetConfigSettingValue(key);
            string[] valueList = keyValue.Split('|');
            bool retValue = false;
            if (valueList.Length > 0)
            {
                retValue= valueList.Contains(utilityName);
            } 
            
            return retValue;
        }

        public static List<string> GetWBTenderFWVersionList()
        {
            List<string> wbTenderFWVersions = new List<string>();
            wbTenderFWVersions.Add("0.24");
            wbTenderFWVersions.Add("0.30");
            wbTenderFWVersions.Add("0.31");
            wbTenderFWVersions.Add("0.32");
            wbTenderFWVersions.Add("0.33");
            wbTenderFWVersions.Add("0.34");
            wbTenderFWVersions.Add("0.35");
            wbTenderFWVersions.Add("0.36");
            wbTenderFWVersions.Add("0.37");
            wbTenderFWVersions.Add("0.38");
            wbTenderFWVersions.Add("0.39");
            wbTenderFWVersions.Add("0.40");
            wbTenderFWVersions.Add("0.41");
            wbTenderFWVersions.Add("0.42");
            wbTenderFWVersions.Add("0.43");
            wbTenderFWVersions.Add("0.44");
            wbTenderFWVersions.Add("0.45");
            return wbTenderFWVersions;
        }
      /// <summary>
      /// Read UtilityFeature.config and search passed key
      /// and return its value
      /// </summary>
      /// <param name="Key"></param>
      /// <returns></returns>
      public static string GetConfigSettingValue(string Key)
        {
            string value = string.Empty;
            if (doc == null)
            {
                return string.Empty;
            }
            try
            {
                XmlNode configuration = null;
                foreach (XmlNode node in doc.ChildNodes)
                {
                    if (node.Name == "configuration")
                    {
                        configuration = node;
                    }
                }
                if (configuration != null)
                {
                    XmlNode settingNode = null;
                    foreach (XmlNode node in configuration.ChildNodes)
                    {
                        if (node.Name == "appSettings")
                        {
                            settingNode = node;
                        }
                    }
                    if (settingNode != null)
                    {
                        XmlNode NumNode = null;
                        foreach (XmlNode node in settingNode.ChildNodes)
                        {
                            if (node.Attributes["key"] != null)
                            {
                                if (string.Equals(node.Attributes["key"].Value, Key, StringComparison.OrdinalIgnoreCase))
                                {
                                    NumNode = node;
                                }
                            }
                        }

                        if (NumNode != null)
                        {
                            XmlAttribute att = NumNode.Attributes["value"];
                            value = att.Value;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            return value;
        }

        public static int GetDisplayProgrammingVariantFromSignature(string signatureData)
        {
            int startIDX = 23;
            int EndIDX = 1;

            if (signatureData.Length >= 60) { startIDX = 25; EndIDX = 1; } //--------For Meters like Net Metering having 64 Byte Signature Info
            else if (signatureData.Length >= 0x1E) { startIDX = 24; EndIDX = 1; } //--------For Meters like HTCT Variants with voltage 63.5V having 30 Byte Signature Info

            if (signatureData.Trim().Length < startIDX + EndIDX) return (int)CAB.Framework.DisplayProgrammingTypes.OneByte;
            else
            {
                int.TryParse(signatureData.Substring(startIDX, EndIDX), out int dispVariant);
                if (dispVariant == (int)CAB.Framework.DisplayProgrammingTypes.TwoByte)
                {
                    CAB.Framework.Utility.ConfigInfo.DisplayProgrammingVariant = CAB.Framework.DisplayProgrammingTypes.TwoByte;
                }
                else
                {
                    CAB.Framework.Utility.ConfigInfo.DisplayProgrammingVariant = CAB.Framework.DisplayProgrammingTypes.OneByte;
                }
                return dispVariant ;
            }
        }

    }
}
