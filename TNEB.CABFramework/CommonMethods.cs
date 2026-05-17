using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Xml;
using System.IO;
using System.Windows.Forms;
namespace CAB.IECFramework.Utility
{
   public static class CommonMethods
    {
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
                retValue = valueList.Contains(utilityName);
            }

            return retValue;
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

    }
}
