using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GPRSComService.Framework;
using GPRSComService.Tasks;
using System.IO;
using CAB.Framework.Utility;
using System.Reflection;
using GPRSComService.DLMSLIB;
using Utilities;
using LandisGyr.AMI.Layers;
using System.Data;
using System.Xml;
using Hunt.EPIC.Logging;


namespace GPRSComService
{
    class GeneralTask:TaskBase
    {
        private const string profileName = "General";
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(GPRSComService.Tasks.InstantaneousTask).ToString());

        #region ITask Members

        /// <summary>
        /// Override initialize General Task
        /// </summary>
        public override void init()
        {
            try
            {
                base.JobName = profileName;
                base.methodType = typeof(GeneralTask);
                fileSeparator = "05";
                base.init();
            }
            catch (Exception ex)
            {
                logger.Log(LOGLEVELS.Error, "initilazation failed for General task.", ex);
            }
        }

        #endregion

        /// <summary>
        /// Override basetask method to response formatter
        /// </summary>
        /// <param name="response"></param>
        public override void AddResponseToOutputString(string response)
         {
             OutputString = string.Concat(OutputString, string.Format("{0:X2}", response));
         }

        #region Communication Methods

        public byte[] fReadMeterSerialNumberCommand()
        {
            //HDLCLIB objHDLCLIB = new HDLCLIB();
            byte[] HDLCCommand = new byte[200];
            byte HDLCIndex = 0;
            try
            {

                HDLCIndex = PrepareCommandBeforeOBISCode(HDLCCommand, HDLCIndex);
                HDLCIndex = objCOSEMLIB.GetQueryReadMeterID(HDLCCommand, HDLCIndex, 2);
                HDLCIndex = PrepareCommandAfterOBISCode(HDLCCommand, HDLCIndex);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            Array.Resize(ref HDLCCommand, HDLCIndex );
            return HDLCCommand;
        }

        public bool fMeterSerialNumberValidator()
        {
            //HDLCLIB objHDLCLIB = new HDLCLIB(); 
            objHDLCLIB.fIncRecieve();

            if (HDLCValidator() == true)
            {
                int ret = objCOSEMLIB.fCheckCOSEMResponseForGet(responseBytes);
                if (ret == 0x01)
                {
                   return  WriteSerialNumberToResponseList(responseBytes);
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        private bool WriteSerialNumberToResponseList(byte[] responseBytes)
        {
            int idLen = Convert.ToInt16(responseBytes[19]);
            if (idLen < 7 || idLen > 16)
            {
                return false;
            }

            string idLength = Convert.ToString(responseBytes[19]);

            while (idLength.Length < 2)
            {
                idLength = "0" + idLength;
            }

            int index = Convert.ToInt16(responseBytes[19]);
            
            string data = string.Empty;

            for (int i = 20; i <= 20 + (index - 1); i++)
            {
                data += Convert.ToChar(responseBytes[i]).ToString();

            }
            responseList.Add("00" + idLength
                            + data
                            + String.Format("{0:0000}", DateTime.Now.Year)
                            + String.Format("{0:00}", DateTime.Now.Month)
                            + String.Format("{0:00}", DateTime.Now.Day)
                            + String.Format("{0:00}", DateTime.Now.Hour)
                            + String.Format("{0:00}", DateTime.Now.Minute)
                            + String.Format("{0:00}", DateTime.Now.Second)
                            );
            return true;
        }

        public byte[] Initialize_ReadMeterID(int iIndex)
        {
            byte[] HDLCCommand = new byte[200];
            byte HDLCIndex = 0;
            try
            {
                //store value from xml data set
                DataSet OBISLIST = null;
                //define xml data document object
                XmlDataDocument xmlDatadoc = null;
                xmlDatadoc = new XmlDataDocument();
                //serialize the xml data 
                string path = AppDomain.CurrentDomain.BaseDirectory + "Name Plate Details.xml";//SerialPortSettings.Default.ReadOut;//AppDomain.CurrentDomain.BaseDirectory + "DLMSReadOutList.xml";
                xmlDatadoc.DataSet.ReadXml(path);
                //assign memory to dataset object and name it "alerts"
                OBISLIST = new DataSet("OBis List Dataset");
                //deserialize xml data
                OBISLIST = xmlDatadoc.DataSet;
                objCOSEMLIB.ObisQueryDSet = OBISLIST;
                //store value from xml data set

                HDLCIndex = PrepareCommandBeforeOBISCode(HDLCCommand, HDLCIndex);
                HDLCIndex = objCOSEMLIB.GetQuery(HDLCCommand, HDLCIndex, iIndex);
                HDLCIndex = PrepareCommandAfterOBISCode(HDLCCommand, HDLCIndex);

                objHDLCLIB.fIncRecieve();

            }
            catch (Exception ex)
            {
                throw ex;
            }

            Array.Resize(ref HDLCCommand, HDLCIndex );
            return HDLCCommand;
        }

        public bool Initialize_ReadMeterIDValidator()
        {
            int ret;
            if (HDLCValidator() == true)
            {
                ret = objCOSEMLIB.fCheckCOSEMResponseForGet(responseBytes);
                processCosemResponse(ret);
                if (ret == 0x01)
                {
                    if (objHDLCLIB.fCheckFCS(responseBytes))
                    {
                        ProcessResponse(responseBytes);
                        return true;
                    }
                }
                else if (ret == 0x03)
                {
                    return true;
                }
                return false;
            }
            else
            {
                return false;
            }
        }

        private void ProcessResponse(byte[] responseBytes)
        {
            bool isCurrentCommandOfPTRatio = false;
            int length = 0;
            int startIndex = 0;
            String strTemp = string.Empty;
            
            if (currentStep == 6)
                isCurrentCommandOfPTRatio = true;

            if (responseBytes[18] == 0x09 && responseBytes[19] != 12)
            {
                length = responseBytes[19];
                startIndex = 20;
            }
            else if (responseBytes[18] == 0x0A && responseBytes[19] != 12)
            {
                length = responseBytes[19];
                startIndex = 20;
            }
            else if (responseBytes[18] == 0x09 && responseBytes[19] == 12)
            {
                length = responseBytes[19];
                startIndex = 20;
            }
            else if (responseBytes[18] == 0x12)
            {
                length = 2;
                startIndex = 19;
            }
            else if (responseBytes[18] == 0x11)
            {
                length = 1;
                startIndex = 19;
            }
            else if (responseBytes[18] == 0x06 || responseBytes[18] == 0x05)
            {
                length = 4;
                startIndex = 19;
            }
            else if (responseBytes[18] == 0x15)
            {
                length = 8;
                startIndex = 19;

            }
            for (int i = 0; i < length; i++)
            {
                strTemp = strTemp + String.Format("{0:X2}", responseBytes[i + startIndex]);
            }

            if (isCurrentCommandOfPTRatio && String.IsNullOrEmpty(strTemp))
            {
                responseList.Add(string.Concat("05" , strTemp , 0x00));
            }
            else
            {
                responseList.Add(string.Concat("05", strTemp));
            }
        }

        private int processCosemResponse(int ret)
        {
            // This is a workaround as LTCT,HTCT meters will not support PT Ratio.
            //// if access denied and current command is for PT ratio return success.
            //if (ret == 0x03)
            //{
            //    return 0x01;
            //}
            if (ret == 0x01)
            {
                return 0x01; //Success
            }
            else if (ret == 0x0E) //Data block unavailable
            {
                return 0x0E;
            }
            else if (ret == 0x03) //Access denied
            {
                return 0x03;
            }
            else
            {
                return 0x00; //Fail
            }
        }
      
        #endregion

    }
}
