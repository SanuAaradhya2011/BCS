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
using System.Diagnostics;

namespace LNG.Framework.Utility
{
    public class ConfigSettings
    {
        //Thread Safe
        private static object LockObject = new object();

        public static void ChangeNode(string Key, string KeyValue)
        {
            lock (LockObject)
            {
                XmlDocument doc = new XmlDocument();
                string path = "";
                try
                {
                    string fileNames = Application.ExecutablePath + ".config";
                    //string fileNames = string.Concat(Path.GetDirectoryName(Application.ExecutablePath)) + "\\" + "CABApplication.exe.config";
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
                catch (UnauthorizedAccessException ex)
                {
                    MessageBox.Show("Access permission issue. Please run as administrator.");
                    Process.GetCurrentProcess().Kill();
                }
            }
        }

        public static string GetValue(string Key)
        {
            lock (LockObject)
            {
                string value = string.Empty;
                XmlDocument doc = new XmlDocument();
                //string fileNames = string.Concat(Path.GetDirectoryName(Application.ExecutablePath))+"\\"+"CABApplication.exe.config";
                try
                {
                    string fileNames = Application.ExecutablePath + ".config";
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
                                value = att.Value;
                            }
                        }
                    }
                }
                catch (UnauthorizedAccessException ex)
                {
                    MessageBox.Show("Access permission issue. Please run as administrator.");
                    Process.GetCurrentProcess().Kill();
                }
                return value;
            }
        }
    }
}

