using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Security.Cryptography;
using System.IO;
using System.Diagnostics;
using Hunt.EPIC.Logging;
namespace CABApplication.Security_Key
{
    public class SecurityKeyManager
    {
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(E650MeterDataReadout).ToString());
        public static List<string> GetSecurityKeys(string meterid, string argstrprivatekey)
        {

            string source = string.Empty;
            string result = string.Empty;
            string errorcode = string.Empty;
            XmlDocument doc = new XmlDocument();
            List<string> SecurityKeyDetails = new List<string>();
            try
            {
               string SecurityKeyFilePath = AppDomain.CurrentDomain.BaseDirectory + "EndDeviceSecurityResponse.Xml";
               if (!File.Exists(SecurityKeyFilePath)) return null;
                
                doc.Load(SecurityKeyFilePath);

                foreach (XmlNode node in doc.ChildNodes)
                {
                    if (node.Name == "ResponseMessageType")
                        foreach (XmlNode node1 in node.ChildNodes)
                        {
                            if (node1.Name == "Header")
                            {
                                foreach (XmlNode node2 in node1.ChildNodes)
                                {
                                    if (node2.Name == "Source")
                                    {
                                        source = node2.InnerText;

                                    }

                                }
                            }
                            if (node1.Name == "Reply")
                            {
                                foreach (XmlNode node2 in node1.ChildNodes)
                                {
                                    if (node2.Name == "Result")
                                    {
                                        result = node2.InnerText;

                                    }
                                    if (node2.Name == "Error")
                                    {
                                        foreach (XmlNode node3 in node2.ChildNodes)
                                        {
                                            if (node3.Name == "code")
                                            {

                                                errorcode = node2.InnerText;
                                            }

                                        }


                                    }

                                }
                            }
                        }
                }
                if (source == "command center" && result != "FAILED" && errorcode == "0.0")
                {
                    var doc2 = XDocument.Load(SecurityKeyFilePath);

                    var itemsList = (from c in doc2.Descendants("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}EndDeviceSecurity")
                                     select new
                                     {
                                         // item = query1.ElementAt(0),
                                         // item = query1.ElementAt(0),
                                         meterid = doc2.Root.Elements("{http://iec.ch/TC57/2011/schema/message}Payload").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}EndDeviceSecurityConfig").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}EndDeviceSecurity").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}Names").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}name"),
                                         llsvalue = doc2.Root.Elements("{http://iec.ch/TC57/2011/schema/message}Payload").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}EndDeviceSecurityConfig").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}EndDeviceSecurity").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}CustomAttributes").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}value"),
                                         secTempGlobalKey = doc2.Root.Elements("{http://iec.ch/TC57/2011/schema/message}Payload").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}EndDeviceSecurityConfig").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}EndDeviceSecurity").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}CustomAttributes").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}value"),
                                         secGlobalKey = doc2.Root.Elements("{http://iec.ch/TC57/2011/schema/message}Payload").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}EndDeviceSecurityConfig").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}EndDeviceSecurity").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}meterGlobalKey"),

                                     }).ToList();

                    int itemindex = 0;
                    int idualkeymultiplier = 1;
                    if (itemsList[0].llsvalue.Count() >= 2 * itemsList.Count) idualkeymultiplier = 2;

                    // Test Code

                    List<string> Filetestdata = new List<string>();

                    foreach (var item in itemsList)
                    {
                        SecurityKeyDetails.Clear();
                       // if (itemsList.ElementAt(itemindex).meterid.ElementAt(itemindex).Value == meterid)
                        {
                            SecurityKeyDetails.Add(itemsList.ElementAt(itemindex).meterid.ElementAt(itemindex).Value);
                            SecurityKeyDetails.Add(itemsList.ElementAt(itemindex).llsvalue.ElementAt(itemindex * idualkeymultiplier).Value);
                            SecurityKeyDetails.Add(itemsList.ElementAt(itemindex).secGlobalKey.ElementAt(itemindex).Value);
                            if (itemsList.ElementAt(itemindex).secTempGlobalKey.Count() > 1)
                                SecurityKeyDetails.Add(itemsList.ElementAt(itemindex).secTempGlobalKey.ElementAt(idualkeymultiplier * itemindex + 1).Value);
                        }
                        Filetestdata.AddRange(SecurityKeyDetails.ToArray());
                        if (argstrprivatekey == "") { logger.Log(LOGLEVELS.Error, "ConfigSettings.GetValue(PrivateKey)--> Can't be Null to Decrypt EndDeviceSecurityResponse"); return null; }

                        List<string> tempkeys = GetDecryptedSecurityKeys(SecurityKeyDetails, argstrprivatekey);
                        Filetestdata.AddRange(tempkeys.ToArray());
                        Filetestdata.Add("**************************************************************");
                        itemindex++;
                    }


                    WriteDebugKeyFile(Filetestdata);
                    
                    itemindex = 0;
                    SecurityKeyDetails.Clear();
                    foreach (var item in itemsList)
                    {
                        if (itemsList.ElementAt(itemindex).meterid.ElementAt(itemindex).Value == meterid)
                        {
                            SecurityKeyDetails.Add(itemsList.ElementAt(itemindex).meterid.ElementAt(itemindex).Value);
                            SecurityKeyDetails.Add(itemsList.ElementAt(itemindex).llsvalue.ElementAt(itemindex * idualkeymultiplier).Value);
                            SecurityKeyDetails.Add(itemsList.ElementAt(itemindex).secGlobalKey.ElementAt(itemindex).Value);
                            if (itemsList.ElementAt(itemindex).secTempGlobalKey.Count() > 1)
                                SecurityKeyDetails.Add(itemsList.ElementAt(itemindex).secTempGlobalKey.ElementAt(idualkeymultiplier * itemindex + 1).Value);

                            break;
                        }

                        itemindex++;
                    }
                    
                    return GetDecryptedSecurityKeys(SecurityKeyDetails, argstrprivatekey);


                }
                return null;

            }
            catch (UnauthorizedAccessException ex)
            {
                //MessageBox.Show("Access permission issue. Please run as administrator.");
                return null;
            }

        }
        [Conditional("DEBUG")]
        private static void WriteDebugKeyFile(List<string> Filetestdata)
        {
            File.WriteAllLines(AppDomain.CurrentDomain.BaseDirectory + "KeysEnlog.txt", Filetestdata.ToArray());
        }
        public static List<string> GetDecryptedSecurityKeys(List<string> arglistData, string argstrPrivateKey)
        {

            try
            {
                // Local varuables Initialised here
                string meterTempGlobalKey = "";
                const int LLSKEYINDEX = 1;
                const int GLOBALKEYINDEX = 2;
                const int TEMPGLOBALKEYINDEX = 3;
                byte[] globalkey = null;
                byte[] tempglobalkey = null;

                
                // Verify input data not null
                if (arglistData == null || arglistData.Count < 3)
                    return null;

                // Get Values
                string meterLLSKey = arglistData[LLSKEYINDEX];
                string meterGlobalKey = arglistData[GLOBALKEYINDEX];

                // RSA objects creation
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                // Load private key
                rsa.FromXmlString(argstrPrivateKey);
                // Check valid temp global key
                if (arglistData.Count > TEMPGLOBALKEYINDEX && arglistData[TEMPGLOBALKEYINDEX] != null && arglistData[TEMPGLOBALKEYINDEX].Length > 1)
                {
                    meterTempGlobalKey = arglistData[TEMPGLOBALKEYINDEX];
                    tempglobalkey = rsa.Decrypt(Convert.FromBase64String(meterTempGlobalKey), false);
                    arglistData[TEMPGLOBALKEYINDEX] = BitConverter.ToString(tempglobalkey);
                    arglistData[TEMPGLOBALKEYINDEX] = arglistData[3].Replace("-", "");
                }

                // Check Global Key
                if (meterGlobalKey != null && meterGlobalKey.Length > 1)
                {
                    globalkey = rsa.Decrypt(Convert.FromBase64String(meterGlobalKey), false);
                    arglistData[GLOBALKEYINDEX] = BitConverter.ToString(globalkey);
                    arglistData[GLOBALKEYINDEX] = arglistData[2].Replace("-", "");
                }

                // Get LLS key
                byte[] llskey = rsa.Decrypt(Convert.FromBase64String(meterLLSKey), false);
                arglistData[1] = new string(System.Text.Encoding.UTF8.GetString(llskey).ToCharArray());

                // Test Code

                // File.WriteAllLines(AppDomain.CurrentDomain.BaseDirectory + "KeysDelog.txt", arglistData.ToArray());

                // Return List
                return arglistData;

            }
            catch (Exception ex)
            {

            }

            return null;
        }



    }
}
