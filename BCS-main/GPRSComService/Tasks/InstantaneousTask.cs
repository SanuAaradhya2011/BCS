using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.Framework.Utility;
using System.Reflection;
using Hunt.EPIC.Logging;

namespace GPRSComService.Tasks
{
    class InstantaneousTask: TaskBase
    {
        private const string taskName = "Instantaneous"; 
        string strResponse = string.Empty;
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(GPRSComService.Tasks.InstantaneousTask).ToString());

        public override void init()
        {
            try
            {
                base.JobName = taskName;
                base.methodType = typeof(InstantaneousTask);
                fileSeparator = "01";
                base.init();
            }
            catch (Exception ex)
            {
                logger.Log(LOGLEVELS.Error, "initilazation failed for Instant task.", ex);
            }
        }

        public override void AddResponseToOutputString(string response)
        {
            OutputString = string.Concat(OutputString, string.Format("{0:X2}", response));
        }

        public byte[] ReadInstantaneousProfile(byte atb)
        {
            logger.Log(LOGLEVELS.Info, string.Format("Preparaing command fReadInstantaneous MeterId:{0}, ModemId:{1}, TaskName{2}",MeterId, IMEINumber,JobName));
            byte[] HDLCCommand = new byte[200];
            byte HDLCIndex = 0;
            strResponse = string.Empty;

            objCOSEMLIB.nBlockIndex = 0;
            objCOSEMLIB.nTotalPacketSize = 0;
            objCOSEMLIB.nBlockNumber = 0;
            objCOSEMLIB.nBlockTotalByteCount = 0;

            try
            {
                HDLCIndex = PrepareCommandBeforeOBISCode(HDLCCommand, HDLCIndex);
                HDLCIndex = objCOSEMLIB.GetQueryInstantProfile(HDLCCommand, HDLCIndex, atb);
                HDLCIndex = PrepareCommandAfterOBISCode(HDLCCommand, HDLCIndex);
            }
            catch (Exception ex)
            {
                logger.Log(LOGLEVELS.Error, "Error while generating command for fReadInstantaneous method", ex); 
            }
            Array.Resize(ref HDLCCommand, HDLCIndex );
            return HDLCCommand;
               
        }

        public int ValidateInstantaneousProfile()
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

                    IncrementStepCount();
                }
                else if (retValue == 0x07)
                {
                    responseList.Add(fileSeparator);
                }
            }

            return retValue;
        }

        public int ValidateRecursiveInstananeousData()
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

        public byte[] ReadRecursiveInstananeousData()
        {
            logger.Log(LOGLEVELS.Info, "Preparaing command for method fReadInstananeousBulkData");
            byte[] HDLCCommand = new byte[200];
            byte HDLCIndex = 0;
            strResponse = string.Empty;
            try
            {
                HDLCIndex = PrepareCommandBeforeOBISCode(HDLCCommand, HDLCIndex);
                HDLCIndex = objCOSEMLIB.fGetBlockTransferPacket(HDLCCommand, HDLCIndex);
                HDLCIndex = PrepareCommandAfterOBISCode(HDLCCommand, HDLCIndex);

                objHDLCLIB.fIncRecieve();
            }
            catch(Exception ex)
            {
                logger.Log(LOGLEVELS.Error, "Error while generating command for fReadInstananeousBulkData method", ex); 
            }
            Array.Resize(ref HDLCCommand, HDLCIndex );
            return HDLCCommand;
        }
       
        public byte[] ReadCumulativeKW(byte atb)
        {
            byte[] HDLCCommand = new byte[200];
            byte HDLCIndex = 0;
            strResponse = string.Empty;

            objCOSEMLIB.nBlockIndex = 0;
            objCOSEMLIB.nTotalPacketSize = 0;
            objCOSEMLIB.nBlockNumber = 0;
            objCOSEMLIB.nBlockTotalByteCount = 0;

            try
            {
                HDLCIndex = PrepareCommandBeforeOBISCode(HDLCCommand, HDLCIndex);
                HDLCIndex = objCOSEMLIB.GetCumulativeKW(HDLCCommand, HDLCIndex, atb);
                HDLCIndex = PrepareCommandAfterOBISCode(HDLCCommand, HDLCIndex); 
                Array.Resize(ref HDLCCommand, HDLCIndex);
                objHDLCLIB.fIncRecieve();//Setting Response Command type

            }
            catch (Exception ex)
            {
                logger.Log(LOGLEVELS.Error, "Error while executing method fReadCumulativeKW", ex); 
            }
            return HDLCCommand;
        }

        public bool ValidateCumumativeKW()
        {
            logger.Log(LOGLEVELS.Info, "Execution of method fReadCumumativeKWValidator started."); 
            bool retValue = false;
            try
            {
                if (HDLCValidator())
                {
                    int ret = objCOSEMLIB.fCheckCOSEMResponseForGet(responseBytes);
                    if (ret == 0x01)
                    {
                        String strTemp = string.Empty;

                        int length = objCOSEMLIB.nBlockTotalByteCount;
                        int startIndex = 0;
                        if (responseBytes[18] == 0x06)
                        {
                            length = 4;
                            startIndex = 19;
                            for (int i = 0; i < length; i++)
                            {
                                strTemp = strTemp + String.Format("{0:X2}", responseBytes[i + startIndex]);
                            }
                            responseList.Add(string.Concat("01", strTemp));
                            retValue = true;
                        }
                    }
                    else if (ret == 0x07)
                    {
                        responseList.Add(string.Concat("01", "00000000"));
                        retValue = true;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Log(LOGLEVELS.Error, "Error while executing method fReadCumumativeKWValidator", ex); 
            }
            return retValue;
        }

        public byte[] ReadCumulativeKVA(byte atb)
        {
            byte[] HDLCCommand = new byte[200];
            byte HDLCIndex = 0;
            strResponse = string.Empty;
            logger.Log(LOGLEVELS.Info, "Execution of method fReadCumulativeKVA started."); 

            objCOSEMLIB.nBlockIndex = 0;
            objCOSEMLIB.nTotalPacketSize = 0;
            objCOSEMLIB.nBlockNumber = 0;
            objCOSEMLIB.nBlockTotalByteCount = 0;

            try
            {
                HDLCIndex = PrepareCommandBeforeOBISCode(HDLCCommand, HDLCIndex);
                HDLCIndex = objCOSEMLIB.GetCumulativeKVA(HDLCCommand, HDLCIndex, atb);
                HDLCIndex = PrepareCommandAfterOBISCode(HDLCCommand, HDLCIndex); 
                Array.Resize(ref HDLCCommand, HDLCIndex);
                objHDLCLIB.fIncRecieve();//Setting Response Command type

            }
            catch (Exception ex)
            {
                logger.Log(LOGLEVELS.Error, "Error while executing method fReadCumulativeKVA", ex); 
            }
           
            return HDLCCommand;
        }

        public bool ValidateCumulativeKVA()
        {
            bool retValue = false;
            logger.Log(LOGLEVELS.Info, "Execution of method fReadCumulativeKVAValidator started.");
            try
            {
                if (HDLCValidator())
                {
                    int ret = objCOSEMLIB.fCheckCOSEMResponseForGet(responseBytes);
                    if (ret == 0x01)
                    {
                        String strTemp = string.Empty;

                        int length = objCOSEMLIB.nBlockTotalByteCount;
                        int startIndex = 0;
                        if (responseBytes[18] == 0x06)
                        {
                            length = 4;
                            startIndex = 19;
                            for (int i = 0; i < length; i++)
                            {
                                strTemp = strTemp + String.Format("{0:X2}", responseBytes[i + startIndex]);
                            }
                            responseList.Add(string.Concat("01", strTemp));
                            retValue = true;
                        }
                    }
                    else if (ret == 0x07)
                    {
                        responseList.Add(string.Concat("01", "00000000"));
                        retValue = true;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Log(LOGLEVELS.Error, "Error while executing method fReadCumulativeKVAValidator", ex); 
            }
            return retValue;
        }

        public byte[] InstantScalerProfile(byte atb)
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
                HDLCIndex = objCOSEMLIB.GetQueryInstantScalarProfile(HDLCCommand, HDLCIndex, atb);
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


        /// <summary>
        /// Get command byte for Cumulative Scalar KW
        /// </summary>
        /// <param name="atb"></param>
        /// <returns></returns>
        public byte[] ReadCumulativeScalarKW(byte atb)
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
                HDLCIndex = objCOSEMLIB.GetQueryCumulativeScalarProfileKW(HDLCCommand, HDLCIndex, atb);
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

        /// <summary>
        /// Get command byte for Cumulative KVA Scalar
        /// </summary>
        /// <param name="atb"></param>
        /// <returns></returns>
        public byte[] ReadCumulativeScalarKVA(byte atb)
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
                HDLCIndex = objCOSEMLIB.GetQueryCumulativeScalarProfileKVA(HDLCCommand, HDLCIndex, atb);
               HDLCIndex=  PrepareCommandAfterOBISCode(HDLCCommand, HDLCIndex);
              //  objHDLCLIB.fIncRecieve();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            Array.Resize(ref HDLCCommand, HDLCIndex);
            return HDLCCommand;
        }
    }
}
