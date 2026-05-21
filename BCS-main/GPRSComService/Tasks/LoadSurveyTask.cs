using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.Framework.Utility;
using CAB.BLL;

namespace GPRSComService.Tasks
{
    class LoadSurveyTask :TaskBase
    {
        private const string profileName = "LoadSurvey";

        /// <summary>
        /// Initialize BillingTask object.
        /// </summary>
        public override void init()
        {
            base.JobName = profileName;
            base.methodType = typeof(LoadSurveyTask);
            fileSeparator = "03";
            base.init();
        }

        public override void AddResponseToOutputString(string response)
        {
            OutputString = string.Concat(OutputString, string.Format("{0:X2}", response));
        }

        public DateTime FromDate { get; set; }

        public DateTime ToDate{ get; set; }

        public LSTypes LSType { get; set; }

        private DateTime meterDateTime = DateTime.MinValue;

        public byte[] ReadLSProfile(byte atb)
        {
            byte[] HDLCCommand = new byte[200];
            byte HDLCIndex = 0;

            try
            {
                HDLCIndex = PrepareCommandBeforeOBISCode(HDLCCommand, HDLCIndex);
                HDLCIndex = objCOSEMLIB.fGetQueryLoadSurveyProfile(HDLCCommand, HDLCIndex, atb);
                
                if (atb == 0x02)
                {

                    if (FromDate> DateTime.MinValue && ToDate > DateTime.MinValue)
                    {
                        HDLCIndex = objCOSEMLIB.fGetSelectiveAccessByEntry(HDLCCommand, HDLCIndex, FromDate, ToDate);
                    }
                }

                HDLCIndex = PrepareCommandAfterOBISCode(HDLCCommand, HDLCIndex);
                objHDLCLIB.fIncRecieve();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            Array.Resize(ref HDLCCommand, HDLCIndex);
            return HDLCCommand;
        }

        public int ValidateReadLSProfile()
        {
            int retValue = 0;
            if (HDLCValidator())
            {
                retValue = objCOSEMLIB.fCheckCOSEMResponse(responseBytes);
                if (retValue == 0x01)
                {
                    int length = objCOSEMLIB.nBlockTotalByteCount;
                    string strTemp = string.Empty;

                    for (int i = 0; i < length; i++)
                    {
                        strTemp = strTemp + String.Format("{0:X2}", objCOSEMLIB.BlockBuffer[i]);
                    }
                    responseList.Add(string.Concat(fileSeparator, strTemp));
                    
                    IncrementStepCount();
                }
                else if (retValue == 0x07)
                {
                    responseList.Add(fileSeparator);
                    IncrementStepCount();
                }
            }
            return retValue;
        }

        public byte[] ReadLSProfileRecursive()
        {
            byte[] HDLCCommand = new byte[200];
            byte HDLCIndex = 0;

            try
            {

                HDLCIndex = PrepareCommandBeforeOBISCode(HDLCCommand, HDLCIndex);
                HDLCIndex = objCOSEMLIB.fGetBlockTransferPacket(HDLCCommand, HDLCIndex);
                HDLCIndex = PrepareCommandAfterOBISCode(HDLCCommand, HDLCIndex);

                objHDLCLIB.fIncRecieve();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            Array.Resize(ref HDLCCommand, HDLCIndex);
            return HDLCCommand;
        }

        public int ValidateLSProfileRecursive()
        {
            int retValue = 0;
            if (HDLCValidator())
            {
                retValue = objCOSEMLIB.fCheckCOSEMResponse(responseBytes);
                if (retValue == 0x01)
                {
                    int length = objCOSEMLIB.nBlockTotalByteCount;
                    string strTemp = string.Empty;
                    for (int i = 0; i < length; i++)
                    {
                        strTemp = strTemp + String.Format("{0:X2}", objCOSEMLIB.BlockBuffer[i]);
                    }
                    responseList.Add(string.Concat(fileSeparator, strTemp));
                }
                else if (retValue == 0x07)
                {
                    responseList.Add(fileSeparator);
                }
            }
            return retValue;
        }

        public byte[] ReadLSScalarProfile(byte atb)
        {
            byte[] HDLCCommand = new byte[200];
            byte HDLCIndex = 0;

            objCOSEMLIB.nBlockIndex = 0;
            objCOSEMLIB.nTotalPacketSize = 0;
            objCOSEMLIB.nBlockNumber = 0;
            objCOSEMLIB.nBlockTotalByteCount = 0;
            try
            {
                HDLCIndex = PrepareCommandBeforeOBISCode(HDLCCommand, HDLCIndex);
                HDLCIndex = objCOSEMLIB.GetQueryLoadSurveyScalarProfile(HDLCCommand, HDLCIndex, atb);
                HDLCIndex = PrepareCommandAfterOBISCode(HDLCCommand, HDLCIndex);
                objHDLCLIB.fIncRecieve();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            Array.Resize(ref HDLCCommand, HDLCIndex);
            return HDLCCommand;
        }

        public byte[] ReadRTC()
        {
            byte[] HDLCCommand = null;
            byte HDLCIndex = 0;
            byte attribute = 2;
            if (LSType == LSTypes.BetweenFromToDate)
            {
                //IF CommandType is bewteen From and To Date. Then no need to read meter RTC
                //skip the task by incrementing the step and push the next command.
                IncrementStepCount();
            }
            else
            {
                HDLCCommand = new byte[200];
                objCOSEMLIB.nBlockIndex = 0;
                objCOSEMLIB.nTotalPacketSize = 0;
                objCOSEMLIB.nBlockNumber = 0;
                objCOSEMLIB.nBlockTotalByteCount = 0;
                try
                {
                    HDLCIndex = PrepareCommandBeforeOBISCode(HDLCCommand, HDLCIndex);
                    HDLCIndex = objCOSEMLIB.GetQueryReadRTC(HDLCCommand, HDLCIndex, attribute);
                    HDLCIndex = PrepareCommandAfterOBISCode(HDLCCommand, HDLCIndex);
                    objHDLCLIB.fIncRecieve();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            Array.Resize(ref HDLCCommand, HDLCIndex);
            return HDLCCommand;
        }

        public bool ValidateRTC()
        {
            bool retValue = false;
            if (HDLCValidator())
            {
               int flag = objCOSEMLIB.fCheckCOSEMResponse(responseBytes);
                if (flag == 0x01)
                {
                    PopulateMeterDate(responseBytes);
                    retValue = true;
                }
            }
            return retValue;
        }

        private void PopulateMeterDate(byte[] responseBytes)
        {
            String strTemp = string.Empty;

            int length = objCOSEMLIB.nBlockTotalByteCount;
            for (int i = 0; i < length; i++)
            {
                strTemp = strTemp + String.Format("{0:X2}", objCOSEMLIB.BlockBuffer[i]);
            }
            meterDateTime = DateUtility.ConvertHexStringToDateTime(strTemp);
            
            if (meterDateTime == DateTime.MinValue)
            {
                meterDateTime = DateTime.Now;
            }
        }

        /// <summary>
        /// Fills the load survey from and to dates according to the load survey type selected
        /// </summary>
        /// <param name="gsmTaskEntity"></param>
        private void FillLoadSurveyFromAndToDate()
        {
            try
            {
                //Get the current RTC of the meter
                ToDate = meterDateTime;
                FromDate = ToDate.AddDays(- Convert.ToInt16(Constants.GetConfigValue(Constants.constMaxLoadSurveyDays)));

                if (LSType == LSTypes.Partial)
                {
                    DLMS650LoadSurveyBLL dlms650LoadSurveyBLL = new DLMS650LoadSurveyBLL();
                    
                    //when load survey type is of load survey partial get last load survey date from db.
                    DateTime lastLoadSurveyDate = dlms650LoadSurveyBLL.GetLastLoadSurveyDataInDbForMeter(MeterId);
                    // if last load survey date stored in db is greater than max load survey date then it can be a load survey from date.
                    if (lastLoadSurveyDate != null && lastLoadSurveyDate > DateTime.MinValue && lastLoadSurveyDate > FromDate)
                    {
                       FromDate = lastLoadSurveyDate;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    
    public enum LSTypes
    {
        Complete,
        Partial,
        BetweenFromToDate
    }
}
