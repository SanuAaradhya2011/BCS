using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hunt.EPIC.Logging;

namespace GPRSComService.Tasks
{
    class TamperTask : TaskBase
    {
        private const string profileName = "Tamper";
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(GPRSComService.Tasks.InstantaneousTask).ToString());
        private string strTamperScaleBuffer = string.Empty;
        private string strTamperScaleCapture = string.Empty;

        public override void AddResponseToOutputString(string response)
        {
            OutputString = string.Concat(OutputString, string.Format("{0:X2}", response));
        }

        #region ITask Members


        /// <summary>
        /// Override initialize General Task
        /// </summary>
        public override void init()
        {
            try
            {
                base.JobName = profileName;
                base.methodType = typeof(TamperTask);
                fileSeparator = "04";
                base.init();
            }
            catch (Exception ex)
            {
                logger.Log(LOGLEVELS.Error, "initilazation failed for Tamper task.", ex);
            }
        }

        #endregion

        public byte[] ReadTamperProfile(byte atb, byte tamperCompartment)
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
                ////code for Selective Access. If Required can be un
                //if (atb == 0x02)
                //{
                //    if (rdBtnReadLastEvent.Checked == true)
                //    {
                //        HDLCIndex = GlobalObjects.objCOSEMLIB.fGetSelectiveAccessByEntry(HDLCCommand, HDLCIndex, 1, Convert.ToByte(cmbBoxLastFromEvent.Text));

                //    }
                //    else if (rdBtnReadBetweenEvent.Checked == true)
                //    {

                //        HDLCIndex = GlobalObjects.objCOSEMLIB.fGetSelectiveAccessByEntry(HDLCCommand, HDLCIndex, Convert.ToByte(cmbBoxFromEvent.Text), Convert.ToByte(cmbBoxToEvent.Text));
                //    }
                //}
                HDLCIndex = objCOSEMLIB.fGetQueryTamperProfile(HDLCCommand, HDLCIndex, atb, tamperCompartment);
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

        public int ValidateTamperProfile()
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
                    responseList.Add(string.Concat("04", strTemp));

                    IncrementStepCount();
                }
                else if (retValue == 0x07)
                {
                    responseList.Add("04");
                    IncrementStepCount();
                }
            }

            return retValue;
        }

        public byte[] ReadTamperProfileRecursive()
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

        public int ValidateTamperProfileRecursive()
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

        public byte[] ReadTamperScalarProfile(byte atb)
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
                HDLCIndex = objCOSEMLIB.GetQueryTamperScalarProfile(HDLCCommand, HDLCIndex, atb);
                HDLCIndex = PrepareCommandAfterOBISCode(HDLCCommand, HDLCIndex);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            Array.Resize(ref HDLCCommand, HDLCIndex);
            return HDLCCommand;
        }

        public int ValidateScalarCapture()
        {
            int retValue = 0;
            objHDLCLIB.fIncRecieve();

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

                    strTamperScaleCapture = string.Concat(strTamperScaleCapture, strTemp);
                    
                    //Validation for command is Passed. Increment the step count so that next command can be picked.
                    IncrementStepCount();
                }
                else if (retValue == 0x07)
                {
                    responseList.Add(fileSeparator);
                }
            }
            return retValue;
        }

        public int ValidateScalarCaptureRecursive()
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

                    strTamperScaleCapture = string.Concat(strTamperScaleCapture, strTemp);
                }
                else if (retValue == 0x07)
                {
                    responseList.Add(fileSeparator);
                }
            }
            return retValue;
        }

        public int ValidateScalarBuffer()
        {
            int retValue = 0;
            objHDLCLIB.fIncRecieve();

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

                    strTamperScaleBuffer = string.Concat(strTamperScaleBuffer, strTemp);

                    //Validation for command is Passed. Increment the step count so that next command can be picked.
                    IncrementStepCount();
                }
                else if (retValue == 0x07)
                {
                    responseList.Add(fileSeparator);
                }
            }
            return retValue;
        }

        public int ValidateScalarBufferRecursive()
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

                    strTamperScaleBuffer = string.Concat(strTamperScaleBuffer, strTemp);
                   
                }
                else if (retValue == 0x07)
                {
                    responseList.Add(fileSeparator);
                }
            }
            return retValue;
        }

        public byte[] InsertBufferAndCapture()
        {
            byte[] retValue = null;
            try
            {
                responseList.Add(string.Concat(fileSeparator, strTamperScaleCapture));
                responseList.Add(string.Concat(fileSeparator, strTamperScaleBuffer));
                IncreaseTimeout();
                IncrementStepCount();
            }
            catch (Exception ex)
            {
                logger.Log(LOGLEVELS.Error, "Error while calling insertBufferAndCapture", ex);
            }
            return retValue;
        }
    }
}
