using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities;
using System.IO;
using HexEncodingAPI;
using UtilitiesXml;
using Hunt.EPIC.Logging;
using CAB.Framework.Utility;

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//// ************************* Class Name :    convertion from 2ng to xml for CC                                                                    //////
//// ************************* Purpose:         Convertion class to convert 2NG file in to xml as comptaible with CC ----------//////
//// ************************* Details :        ----------------------------------------------------                           //////
//// ************************* Author :         Mohsin Raza                                                                    //////                                
//// ************************* Date:            16-08-2016                                                                     //////                    
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

namespace XMLCCConvertion
{

    public class MeterGenericData
    {

        public List<byte> RawDlmsData
        {
            get;
            set;
        }

        public string DLMSValidData
        {
            get;
            set;
        }

        public string DLMSLGDData
        {
            get;
            set;
        }
    }

    public class ReadOutToXML
    {
        private string filename;
        private string meterid;
        private string readingtimestamp;
        private string metertype;
        private string configcrc;
        private DateTime readingdatetime;
       
        private List<MeterGenericData> listreadoutdata = new List<MeterGenericData>();
        private List<MeterReadoutData> listmeterdata = new List<MeterReadoutData>();
        private List<MeterScalarData> listscalardata = new List<MeterScalarData>();
        private string DATANODENAME = "MeterData";
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(XMLFileHandle).ToString());

        Dictionary<string, string> dicnodemapper = new Dictionary<string, string>()
        {
                {"Instant","0701005E5B00FF"},
                { "InstantScalar", "0701005E5B03FF"},
                {"Billing", "070100620100FF"},
                { "BillingScalar", "0701005E5B06FF"},
                { "Voltage Related Events", "070000636200FF"},
                { "Current Related Events", "070000636201FF"},
                { "Power Failure Events", "070000636202FF"},
                { "Transaction Related Events", "070000636203FF"},
                { "Other Events Log", "070000636204FF"},
                { "Non Rollover Events Log", "070000636205FF"},
                { "Control Events Log", "070000636206FF"},
                { "Utility Specific Events", "070000636280FF"},/*UtilitySpecefic*/
                { "Digital Input Events", "070000636282FF"},/*DigitalInput*/
                { "SmartTamperScalar", "0701005E5B82FF"},
                { "TamperScalar", "0701005E5B07FF"},
                { "LoadSurvey", "070100630100FF"},
                { "LSScalar", "0701005E5B04FF"},
                { "DailySurvey", "070100630200FF"},
                { "DSScalar", "0701005E5B05FF"},
                { "NamePlate", "0700005E5B0AFF"},
                { "LoadSwitch", "070000636281FF"},
                { "LoadSwitchScalar", "0701005E5B83FF"},

        };

        public  void ConvertStringDataToMeterGenericData(string genericdata)
        {
            try
            {
                List<MeterGenericData> _listMeterGenericData = new List<MeterGenericData>();

                string[] arra = genericdata.Split('\r');

                for (int icount = 0; icount < arra.Count(); icount++)
                {
                    for (int iparam = 0; iparam < dicnodemapper.Count; iparam++)
                    {
                        if (arra[icount].Contains(dicnodemapper.ElementAt(iparam).Value.ToString()))
                        {
                            MeterGenericData _objMeterGenericData = new MeterGenericData();
                            _objMeterGenericData.DLMSValidData = arra[icount].Replace("\n", "");
                            _objMeterGenericData.DLMSLGDData = arra[icount].Replace("\n", "");
                            _listMeterGenericData.Add(_objMeterGenericData);
                        }
                    }
                }
                listreadoutdata = _listMeterGenericData;
               
            }
            catch (Exception ex)
            {
                logger.Log(LOGLEVELS.Error, "ConvertStringDataToMeterGenericData()", ex);
            }

           
        }

        public ReadOutToXML(string _meterid, DateTime readingdt, string strconfigcrc, string readdata)
        {
            meterid = _meterid;
            readingdatetime = readingdt;
            configcrc = strconfigcrc;
            ConvertStringDataToMeterGenericData(readdata);
        }

        public void GetReadoutData(string metersignature)
        {
            //if (!metersignature.Contains("SM"))  return;
            
            GetMeterReadoutData("Instant");
            GetMeterReadoutData("Billing");
            GetMeterReadoutDataTamper("Tamper");
            /*GetMeterReadoutDataTamper("VoltageRelatedEvents");
            GetMeterReadoutDataTamper("CurrentRelatedEvents");
            GetMeterReadoutDataTamper("PowerFailureEvents");
            GetMeterReadoutDataTamper("TransactionRelatedEvents");
            GetMeterReadoutDataTamper("OtherEventsLog");
            GetMeterReadoutDataTamper("NonRolloverEventsLog");
            GetMeterReadoutDataTamper("ControlEventsLog");
            GetMeterReadoutDataTamper("UtilitySpecificEvents");
            GetMeterReadoutDataTamper("DigitalInputEvents");
            GetMeterReadoutDataTamper("EventLogScalarObject");*/
            GetMeterReadoutData("LoadSurvey");
            GetMeterReadoutData("DailySurvey");
            GetMeterReadoutData("LoadSwitch");
            GetMeterReadoutData("NamePlate");
            
        }

        public List<MeterScalarData> GetScalarData()
        {
            return null;
        }

        private void FillXmlData(string genericdata)
        {
            try
            {
                List<MeterGenericData> _listMeterGenericData = new List<MeterGenericData>();

                string[] arra = genericdata.Split('\r');

                for (int icount = 0; icount < arra.Count(); icount++)
                {
                    for (int iparam = 0; iparam < dicnodemapper.Count; iparam++)
                    {
                        if (listreadoutdata[icount].DLMSValidData.Contains(dicnodemapper.ElementAt(iparam).Value))
                        {
                            MeterGenericData _objMeterGenericData = new MeterGenericData();
                            _objMeterGenericData.DLMSValidData = arra[icount].Replace("\n", "");
                            _objMeterGenericData.DLMSLGDData = arra[icount].Replace("\n", "");
                            _listMeterGenericData.Add(_objMeterGenericData);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Log(LOGLEVELS.Error, "FillXmlData()", ex);
            }
        }
        
       
        private List<MeterReadoutData> GetMeterReadoutData(string nodename)
        {
            
            MeterReadoutData _objmeterreadoutdata = new MeterReadoutData();
            MeterScalarData _objscalardata = new MeterScalarData();
            MeterobjectData _objmeterobjectdata = new MeterobjectData();
            int icount = 0;
            List<byte[]> _captureobject = new List<byte[]>();
            List<byte[]> _objectdata = new List<byte[]>();
            List<byte[]> _scalarobject = new List<byte[]>();
            List<byte[]> _scalardata = new List<byte[]>();

            try
            {
                for (icount = 0; icount < listreadoutdata.Count; icount++)
                {
                    if (listreadoutdata[icount].DLMSValidData.Contains(dicnodemapper[nodename]))
                    {
                        //int idiscard;
                        //_captureobject.Add(HexEncoding.ConvertHexStringToByteArray(listreadoutdata[icount++].DLMSLGDData.Remove(0, 2)));
                        //_objectdata.Add(HexEncoding.ConvertHexStringToByteArray(listreadoutdata[icount++].DLMSLGDData.Remove(0, 2)));
                        //if (!nodename.ToUpperInvariant().Contains("NAMEPLATE"))
                        //{
                        //    _scalarobject.Add(HexEncoding.ConvertHexStringToByteArray(listreadoutdata[icount++].DLMSLGDData.Remove(0, 2)));
                        //    _scalardata.Add(HexEncoding.ConvertHexStringToByteArray(listreadoutdata[icount++].DLMSLGDData.Remove(0, 2)));
                        //}
                        //break;

                        int idiscard;
                        _captureobject.Add(HexEncoding.ConvertHexStringToByteArray(listreadoutdata[icount++].DLMSValidData.Remove(0, 16)));
                        _objectdata.Add(HexEncoding.ConvertHexStringToByteArray(listreadoutdata[icount++].DLMSValidData.Remove(0, 16)));
                        if (!nodename.ToUpperInvariant().Contains("NAMEPLATE"))
                        {
                            _scalarobject.Add(HexEncoding.ConvertHexStringToByteArray(listreadoutdata[icount++].DLMSValidData.Remove(0, 16)));
                            _scalardata.Add(HexEncoding.ConvertHexStringToByteArray(listreadoutdata[icount++].DLMSValidData.Remove(0, 16)));
                        }
                        break;
                    }
                }

                _objmeterobjectdata.NodeName = nodename;
                _objmeterobjectdata.ObjectData = _captureobject;
                _objmeterobjectdata.DataValue = _objectdata;
                _objscalardata.ScalarObjct = _scalarobject;
                _objscalardata.ScalarData = _scalardata; 
                _objscalardata.ScalarNodeName = nodename;
                _objscalardata.NodeName = new List<string>();
                _objscalardata.NodeName.AddRange(new string[]{"Object", "Data"});
                                
                _objmeterreadoutdata.NodeName = nodename;
                _objmeterreadoutdata.MeterObjectData.Add(_objmeterobjectdata);
                _objmeterreadoutdata.MeterScalarData.Add(_objscalardata);
                listmeterdata.Add(_objmeterreadoutdata);
                listscalardata.Add(_objscalardata);
            }
            catch (Exception ex)
            {
                logger.Log(LOGLEVELS.Error, "GetMeterReadoutData()", ex);
            }
            return null;
        }

        private List<MeterReadoutData> GetMeterReadoutDataTamper(string nodename)
        {

            MeterReadoutData _objmeterreadoutdata = new MeterReadoutData();
            MeterScalarData _objscalardata = new MeterScalarData();
            MeterobjectData _objmeterobjectdata = new MeterobjectData();
            int icount = 0;
            List<byte[]> _captureobject = new List<byte[]>();
            List<byte[]> _objectdata = new List<byte[]>();
            List<byte[]> _scalarobject = new List<byte[]>();
            List<byte[]> _scalardata = new List<byte[]>();
            List<string> _ProfileNamedata = new List<string>();
             
            List<string> TamperParam = new List<string>(){"Voltage Related Events",
                                                          "Current Related Events",
                                                          "Power Failure Events",
                                                          "Transaction Related Events",
                                                          "Other Events Log",
                                                          "Non Rollover Events Log",
                                                          "Control Events Log",
                                                          "Digital Input Events",
                                                          "Utility Specific Events",
                                                          "SmartTamperScalar"
                                                          };

            try
            {
                int tamperCount = 0;

                for (int iparam = 0; iparam < TamperParam.Count; iparam++)
                {
                    for (icount = 0; icount < listreadoutdata.Count; icount++)
                    {
                        if (listreadoutdata[icount].DLMSValidData.Contains(dicnodemapper[TamperParam[iparam]]))//.Contains(dicnodemapper[nodename]))
                        {
                            int idiscard;
                            
                            if (!TamperParam[iparam].Contains("SmartTamperScalar"))
                            _ProfileNamedata.Add(TamperParam[iparam]);
                            // 0701005E5B82FF03
                            // 0701005E5B82FF02
                            string obiskey = listreadoutdata[icount].DLMSValidData.Substring(0, 20);

                            if (obiskey.Contains("0701005E5B82FF03") || obiskey.Contains("0701005E5B82FF02")) //--Tamper Scalar & Unit 
                            {
                                _scalarobject.Add(HexEncoding.ConvertHexStringToByteArray(listreadoutdata[icount++].DLMSLGDData.Remove(0, 16)));
                                _scalardata.Add(HexEncoding.ConvertHexStringToByteArray(listreadoutdata[icount++].DLMSLGDData.Remove(0, 16)));
                            }
                            else //---Tamper Data List
                            {
                                _captureobject.Add(HexEncoding.ConvertHexStringToByteArray(listreadoutdata[icount++].DLMSLGDData.Remove(0, 16)));
                                _objectdata.Add(HexEncoding.ConvertHexStringToByteArray(listreadoutdata[icount++].DLMSLGDData.Remove(0, 16)));
                            }

                        }

                    }

                }

                int nodecnt=0;
                while (nodecnt < _captureobject.Count)
                {
                    MeterobjectData _objmdataList = new MeterobjectData();
                    _objmdataList.NodeName = _ProfileNamedata[nodecnt];
                    _objmdataList.ObjectData.Add(_captureobject[nodecnt]);
                    _objmdataList.DataValue.Add(_objectdata[nodecnt]);
                    _objmeterreadoutdata.MeterObjectData.Add(_objmdataList);
                    nodecnt++;
                }
                _objscalardata.ScalarObjct = _scalarobject;
                _objscalardata.ScalarData = _scalardata;
                _objscalardata.ScalarNodeName = nodename;
                _objscalardata.NodeName = new List<string>();
                _objscalardata.NodeName.AddRange(new string[] { "Object", "Data" });
                _objmeterreadoutdata.NodeName = nodename;

                _objmeterreadoutdata.MeterScalarData.Add(_objscalardata);
                listmeterdata.Add(_objmeterreadoutdata);
                listscalardata.Add(_objscalardata);
            }
            catch (Exception ex)
            {
                logger.Log(LOGLEVELS.Error, "GetMeterReadoutDataTamper()", ex);
            }
            return null;
        }

        public bool GenerateXMLFile(string filename, string filepath, string metersignature)
        {
            try
            {
                filepath = ConfigInfo.CheckOrCreatePath();

                XMLFileHandle _xmlhandler = new XMLFileHandle();
                string xmlFileName = filepath + "\\" + meterid;
                _xmlhandler.FilePath = xmlFileName;
                _xmlhandler.MeterID = meterid;
                _xmlhandler.TimeStamp = String.Format("{0:0000}", readingdatetime.Year) + String.Format("{0:00}", readingdatetime.Month) + String.Format("{0:00}", readingdatetime.Day) + String.Format("{0:00}", readingdatetime.Hour) + String.Format("{0:00}", readingdatetime.Minute) + String.Format("{0:00}", readingdatetime.Second);

                _xmlhandler.MeterType = "RFE350SM310";

                if (metersignature.Contains("SM0110")) _xmlhandler.MeterType = "GSDLMSE350";
                else if (metersignature.Contains("SM0310")) _xmlhandler.MeterType = "RFE350SM310";
                else if (metersignature.Contains("SM0405")) _xmlhandler.MeterType = "RFE670SM405"; 

                //_xmlhandler.MeterType = "GSDLMSE350";
                //1P : GSDLMSE350
                //3P: RFEE350SM310 //RFE350SM310 //WCM
                //3P: RFEE670SM405 //LTCT
                
                _xmlhandler.ConfigCRC = configcrc;// "0";
                _xmlhandler.MeterDataList = listmeterdata;
                _xmlhandler.ScalarDataList = listscalardata;

                bool bresult = _xmlhandler.GenerateXML();

                if (XMLFileHandle.UpdateChecksumFromXml(xmlFileName).Length < 1)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                logger.Log(LOGLEVELS.Error, "GenerateXMLFile()", ex);
                return false;
            }
            return true; ;
        }
    }
}
