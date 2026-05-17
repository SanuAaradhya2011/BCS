using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.Framework.Utility;

namespace GPRSComService.Tasks
{
    class BillingTask: TaskBase
    {
        private const string profileName = "Billing"; 

        /// <summary>
        /// Initialize BillingTask object.
        /// </summary>
        public override void init()
        {
            base.JobName = profileName;
            base.methodType = typeof(BillingTask);
            fileSeparator = "02";
            base.init();
        }

        /// <summary>
        /// Overrid the Base class method to provide response formatter
        /// </summary>
        /// <param name="response"></param>
        public override void AddResponseToOutputString(string response)
        {
            OutputString = string.Concat(OutputString, string.Format("{0:X2}", response));
        }

        #region "Communication Methods"

        public byte[] ReadBillingProfile(byte atb)
        {
            byte[] HDLCCommand = new byte[200];
            byte HDLCIndex = 0;

            try
            {
                HDLCIndex= PrepareCommandBeforeOBISCode(HDLCCommand, HDLCIndex);
                HDLCIndex = objCOSEMLIB.fGetQueryBillingProfile(HDLCCommand, HDLCIndex, atb);
               
                //TODO: If Selective is required then use following piece of code

                ////added by gopal for Selective Access By Entry
                //if (atb == 0x02)
                //{
                //    if (rdBtnReadLast.Checked == true)    
                //    {
                //        HDLCIndex = objCOSEMLIB.fGetSelectiveAccessByEntry(HDLCCommand, HDLCIndex, 1, Convert.ToByte(cmbBoxLastFrom.Text));

                //    }
                //    else if (rdBtnReadBetween.Checked == true)
                //    {
                //        HDLCIndex = objCOSEMLIB.fGetSelectiveAccessByEntry(HDLCCommand, HDLCIndex, Convert.ToByte(cmbBoxFrom.Text), Convert.ToByte(cmbBoxTo.Text));
                //    }

                //}

                //added by gopal for Selective Access
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

        public int ValidateBillingProfile()
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
                    responseList.Add(string.Concat("02", strTemp));

                    IncrementStepCount();
                }
                else if (retValue == 0x07)
                {
                    responseList.Add("02");
                }
            }

            return retValue;
        }

        public byte[] ReadBillingProfileRecursive()
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

            Array.Resize(ref HDLCCommand, HDLCIndex );
            return HDLCCommand;
        }

        public byte[] ReadBillingScalarProfile(byte atb)
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
                HDLCIndex = objCOSEMLIB.GetQueryBillingScalarProfile(HDLCCommand, HDLCIndex, atb);
                HDLCIndex = PrepareCommandAfterOBISCode(HDLCCommand, HDLCIndex);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            Array.Resize(ref HDLCCommand, HDLCIndex);
            return HDLCCommand;
        }

        public int ValidateBillingProfileRecursive()
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

        #endregion

    }
}
