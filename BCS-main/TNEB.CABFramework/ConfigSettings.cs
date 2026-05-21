/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 	 								|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| 
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Windows.Forms;

namespace CAB.IECFramework.Utility
{
    public class ConfigSettings
    {
        public static void ChangeNode(string Key, string KeyValue)
        {
            XmlDocument doc = new XmlDocument();
            string path = "";

            string fileNames = string.Concat(Path.GetDirectoryName(Application.ExecutablePath)) + "\\" + "CABApplication.exe.config";
                FileInfo info = new FileInfo(fileNames);
                path = info.FullName.ToString();
                doc.Load(path);
            
              XmlNode configuration = null;
                foreach (XmlNode node in doc.ChildNodes)
                    if (node.Name == "configuration")
                        configuration = node;

                if (configuration != null)
                {
                    XmlNode settingNode = null;
                    foreach (XmlNode node in configuration.ChildNodes)
                        if (node.Name == "appSettings")
                            settingNode = node;

                    if (settingNode != null)
                    {
                        XmlNode NumNode = null;
                        foreach (XmlNode node in settingNode.ChildNodes)
                        {
                            if (node.Attributes["key"] != null)
                                if (node.Attributes["key"].Value == Key)
                                    NumNode = node;
                        }

                        if (NumNode != null)
                        {
                            XmlAttribute att = NumNode.Attributes["value"];
                            att.Value = KeyValue;
                            doc.Save(path);
                        }
                    }
                }
            }

        public static string GetValue(string Key)
        {
            string value = string.Empty;
            XmlDocument doc = new XmlDocument();
            //string fileNames = string.Concat(Path.GetDirectoryName(Application.ExecutablePath))+"\\"+"CABApplication.exe.config";
            string fileNames =  Application.ExecutablePath + ".config";
             
            FileInfo info = new FileInfo(fileNames);
            string path = info.FullName.ToString();
            doc.Load(path);
                        XmlNode configuration = null;
            foreach (XmlNode node in doc.ChildNodes)
                if (node.Name == "configuration")
                    configuration = node;

            if (configuration != null)
            {
                XmlNode settingNode = null;
                foreach (XmlNode node in configuration.ChildNodes)
                    if (node.Name == "appSettings")
                        settingNode = node;

                if (settingNode != null)
                {
                    XmlNode NumNode = null;
                    foreach (XmlNode node in settingNode.ChildNodes)
                    {
                        if (node.Attributes["key"] != null)
                            if (node.Attributes["key"].Value == Key)
                                NumNode = node;
                    }

                    if (NumNode != null)
                    {
                        XmlAttribute att = NumNode.Attributes["value"];
                        value=att.Value  ; 
                    }
                }
            }
            return value;
        }
    }
}
