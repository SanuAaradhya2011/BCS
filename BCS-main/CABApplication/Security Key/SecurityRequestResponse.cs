using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LandisGyr.GSIS.CIM2ndEd.Service;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using LandisGyr.GSIS.CIM2ndEd;
using System.Xml.Linq;
using System.Security.Cryptography;
using Hunt.EPIC.Logging;


namespace App_1Phase.Security
{
    public class SecurityRequestResponse
    {
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(SecurityRequestResponse).ToString());
      
        /// <summary>
        /// This method initiate request in xml for security key (g;obal and lls key) from CC and response will be saved in xml for HHU meter communication in secure mode
        /// </summary>
        /// <param name="xmlfilename"></param>
        /// <param name="argstrBaseEndPointConfigurationName"></param>
        /// <param name="argstrRemoteAddress"></param>
        /// <param name="argstrusername"></param>
        /// <param name="argstrpwd"></param>
        /// <returns></returns>
        /// 
       
        public static bool GenerateResponseXml(string argstrBaseEndPointConfigurationName, string argstrRemoteAddress, string argstrusername, string argstrpwd)
        {
            bool myretval = false;
            CIM2ndEditionCallbackProxyImpl _CIM2ndEditionEndPoint = new CIM2ndEditionCallbackProxyImpl();

            try
            {

                //c.Open("CIM2ndEditionService", "https://indelvr374.ap.bm.net/GSIS_CIM2ndEdition/CIM2ndEd.svc", SoapClientCredentialType.Basic);
                _CIM2ndEditionEndPoint.Open(argstrBaseEndPointConfigurationName, argstrRemoteAddress, SoapClientCredentialType.Basic);
                string filePath = AppDomain.CurrentDomain.BaseDirectory + "EndDeviceSecurityRequest.xml";
                string filePathresponse = AppDomain.CurrentDomain.BaseDirectory + "EndDeviceSecurityResponse.xml";
                TextReader fs = new StreamReader(filePath);
                XmlReader reader = XmlReader.Create(fs);
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(LandisGyr.GSIS.CIM2ndEd.RequestMessageType));
                var requestdata = (LandisGyr.GSIS.CIM2ndEd.RequestMessageType)xmlSerializer.Deserialize(reader);
                RequestRequest r = new RequestRequest();
                r.RequestMessage = requestdata;
                fs.Close();
                var res = _CIM2ndEditionEndPoint.Request(r, SoapClientCredentialType.Basic, argstrusername, argstrpwd);

                if (res.ResponseMessage.Reply.Result == ReplyTypeResult.FAILED)
                {
                    logger.Log(LOGLEVELS.Error, "GenerateResponseXml", new Exception(res.ResponseMessage.Reply.Error[0].details.ToString()));
                    return false;
                }

                XmlSerializer xsSubmit = new XmlSerializer(typeof(LandisGyr.GSIS.CIM2ndEd.ResponseMessageType));
                StringWriter sww = new StringWriter();
                XmlWriter writer = XmlWriter.Create(sww);
                xsSubmit.Serialize(writer, res.ResponseMessage);
                var xml = sww.ToString();

                //---------------------Service Call-------------------------
                StringReader strReader = null;
                XmlSerializer serializer = null;
                XmlTextReader xmlReader = null;

                strReader = new StringReader(xml);
                serializer = new XmlSerializer(typeof(RequestMessageType));
                xmlReader = new XmlTextReader(strReader);

                //-----------------------Save File--------------------------
                var xmldocument = XDocument.Parse(xml);

                xmldocument.Save(filePathresponse);

                myretval = true;
            }
            catch (Exception ex)
            {
                logger.Log(LOGLEVELS.Error, "GenerateResponseXml", ex);
            }
            finally
            {
                _CIM2ndEditionEndPoint.Close();
            }
            return myretval;
        }

        /// <summary>
        /// This method generates request xml file for CC and return private key for decryption
        /// </summary>
        /// <param name="configparaList"></param>
        /// <param name="xmlfilename"></param>
        /// <returns></returns>
        public static string CreateRequestXml(List<string> configparaList)
        {
            string StrPgmDesc = string.Empty;
            string strprivatekeyInfo = "";
            
            try
            {

                // creation of RequestMessageType object
                RequestMessageType objxsd = new RequestMessageType();
                Header objheader = new Header();
                // Fill Header Information
                objheader.Verb = "get";
                objheader.Noun = "EndDeviceSecurityConfig";
                objheader.Revision = 2.0M;
                objheader.Timestamp = System.DateTime.Now;
                objheader.Source = "HHU";
                objheader.MessageID = Guid.NewGuid().ToString();// "7427076d-4dde-47ad-a8f9-83880bcca114";
                objheader.CorrelationID = "D9F521CB-406F-4F29-AEC3-2F17C8755B2D";
                objxsd.Header = objheader;

                // Instant of Request
                Request _request = new Request();

                // Instant of GetEndDeviceSecurityConfig 
                GetEndDeviceSecurityConfig _getendsecurityconfig = new GetEndDeviceSecurityConfig();

                // Instant of EndDeviceSecurity to fill Names List
                _getendsecurityconfig.EndDeviceSecurity = new GetEndDeviceSecurityConfigEndDeviceSecurity();

                // List for Names
                List<GetEndDeviceSecurityConfigEndDeviceSecurityNames> _collectionNames = new List<GetEndDeviceSecurityConfigEndDeviceSecurityNames>();

                int commdCount = 0;

                // Fill list for Names
                while (commdCount < configparaList.Count)
                {
                    string paraContents = configparaList[commdCount];
                    GetEndDeviceSecurityConfigEndDeviceSecurityNames _names = new GetEndDeviceSecurityConfigEndDeviceSecurityNames();
                    _names.name = configparaList[commdCount];// "JSM01033";
                    _names.NameType = new GetEndDeviceSecurityConfigEndDeviceSecurityNamesNameType();
                    _names.NameType.description = "description";
                    _names.NameType.name = "MeterID";
                    _collectionNames.Add(_names);
                    commdCount++;
                }

                _getendsecurityconfig.EndDeviceSecurity.Names = _collectionNames.ToArray();

                // Key Information
                GetEndDeviceSecurityConfigEndDeviceSecurityKeyEncryptionKey objKeyInfo = new GetEndDeviceSecurityConfigEndDeviceSecurityKeyEncryptionKey();

                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();

               // string strpublickey = rsa.ToXmlString(false);

                string strpublickey = "<RSAKeyValue><Modulus>lFGi2sK5nvmVZXN9qO9LGsMQr8n+62eZBd2jyLhW9p1L95E/kzoevtM1x/P4idiQhkVqtHHLJM9bwoKtx5iayBoWQgSoMCiZ/i8ofudo+72019/XE9eAoxQi1ig/mdo609hQCF1YUKK/JrwV5QplrL0mWx7OJeUwgu2gapwWu+k=</Modulus><Exponent>AQAB</Exponent> <P>wZUNitf6c0PDLdqIGYVUlMN/ncWbH0/4GKlyooQFf+iR4mswMZbUvXy9PGc0PBsFNjYzARZGW5mDvMCrimO5uw==</P> <Q>xCRj4LLFdjI+m7erHo6B7t/I6//fZNcDDu9InqRFwqcxyxZfBd5Ov6V4sLEuYEStmI8sXyNDddbCDNLHJixEqw==</Q><DP>fQHU4fFA0UhT9Ptm4hwgl4R7l5Ww65KHot1hoqLgrk2wT2dqkstVDBxOU4BN0fac8fokC7KnsqU61hwRamel2w==</DP><DQ>uzn5VN2Q+5hOhxm1cD+b68cV5aCjP4C2XKUwbq5kIMC0GSXLorBn/ywWnqMin6YvBvdz5YSlunA7Xz4790Y+bQ==</DQ><InverseQ>XzzjJhjMAkJ9XZr0w4+9rn9/fphZlWdMrdnCLFvMmdQ7pZqUTAGk6Qs2+MZWMhGHwwp+LOUF9TjYZltAJ3KN7w==</InverseQ> <D>aXrlZs61U+oL5WqNI2eK8i1n4Jy3PpMesJ0/ra/rqNeU/yu9Gudqlit/RASt7NqnbdZQXKPQ5QzTJTHqhNhyumco2vEJjppY7VcR3CGFXEQ/2MBNI82zJUmDrj5KxQn4DjdjP6VCOwlU461RfUDh3y04q9CNv7IMNW8+CdLmsEE=</D></RSAKeyValue>";

                string[] keyInfo = strpublickey.Split(new string[] { "<RSAKeyValue><Modulus>", "</Modulus>" }, StringSplitOptions.RemoveEmptyEntries);

                string strModulas = keyInfo[0];
                string strexponent = keyInfo[1].Split(new string[] { "<Exponent>", "</Exponent>" }, StringSplitOptions.RemoveEmptyEntries)[0];
                //strprivatekeyInfo = rsa.ToXmlString(true);
                strprivatekeyInfo = "<RSAKeyValue><Modulus>lFGi2sK5nvmVZXN9qO9LGsMQr8n+62eZBd2jyLhW9p1L95E/kzoevtM1x/P4idiQhkVqtHHLJM9bwoKtx5iayBoWQgSoMCiZ/i8ofudo+72019/XE9eAoxQi1ig/mdo609hQCF1YUKK/JrwV5QplrL0mWx7OJeUwgu2gapwWu+k=</Modulus> <Exponent>AQAB</Exponent> <P>wZUNitf6c0PDLdqIGYVUlMN/ncWbH0/4GKlyooQFf+iR4mswMZbUvXy9PGc0PBsFNjYzARZGW5mDvMCrimO5uw==</P> <Q>xCRj4LLFdjI+m7erHo6B7t/I6//fZNcDDu9InqRFwqcxyxZfBd5Ov6V4sLEuYEStmI8sXyNDddbCDNLHJixEqw==</Q><DP>fQHU4fFA0UhT9Ptm4hwgl4R7l5Ww65KHot1hoqLgrk2wT2dqkstVDBxOU4BN0fac8fokC7KnsqU61hwRamel2w==</DP> <DQ>uzn5VN2Q+5hOhxm1cD+b68cV5aCjP4C2XKUwbq5kIMC0GSXLorBn/ywWnqMin6YvBvdz5YSlunA7Xz4790Y+bQ==</DQ> <InverseQ>XzzjJhjMAkJ9XZr0w4+9rn9/fphZlWdMrdnCLFvMmdQ7pZqUTAGk6Qs2+MZWMhGHwwp+LOUF9TjYZltAJ3KN7w==</InverseQ> <D>aXrlZs61U+oL5WqNI2eK8i1n4Jy3PpMesJ0/ra/rqNeU/yu9Gudqlit/RASt7NqnbdZQXKPQ5QzTJTHqhNhyumco2vEJjppY7VcR3CGFXEQ/2MBNI82zJUmDrj5KxQn4DjdjP6VCOwlU461RfUDh3y04q9CNv7IMNW8+CdLmsEE=</D></RSAKeyValue>";

                objKeyInfo.modulus = strModulas;// Convert.ToBase64String(rsap.Modulus);// rsap.Modulus;
                objKeyInfo.exponent = strexponent;

                _getendsecurityconfig.EndDeviceSecurity.KeyEncryptionKey = objKeyInfo;

                _request.GetEndDeviceSecurityConfig = _getendsecurityconfig;

                // Update Request and Header 
                objxsd.Request = _request;
                objxsd.Header = objheader;

                XmlSerializer xsSubmit = new XmlSerializer(typeof(RequestMessageType));
                StringWriter sww = new StringWriter();
                XmlWriter writer = XmlWriter.Create(sww);
                xsSubmit.Serialize(writer, objxsd);
                var xml = sww.ToString();

                //---------------------Service Call-------------------------
                StringReader strReader = null;
                XmlSerializer serializer = null;
                XmlTextReader xmlReader = null;
                Object objxmlData = null;

                strReader = new StringReader(xml);
                serializer = new XmlSerializer(typeof(RequestMessageType));
                xmlReader = new XmlTextReader(strReader);
                objxmlData = serializer.Deserialize(xmlReader);

                //-----------------------Save File--------------------------
                var xmldocument = XDocument.Parse(xml);
                xmldocument.Save(AppDomain.CurrentDomain.BaseDirectory + "EndDeviceSecurityRequest.xml");
               
            }
            catch (Exception ex)
            {
                logger.Log(LOGLEVELS.Error, "CreateRequestXml", ex);
                strprivatekeyInfo = "";
            }

            return strprivatekeyInfo;

        }
              
    }
}
