/*
File Name: FastDownLoadingBLL.cs
Created By: Vivek Agrawal
Date : 24/Feb/2012
Purpose: Fast Downloading business layer implementation.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CABEntity;
using Utilities;
using LTCTDAL;
using CAB.Framework.Utility;

namespace LTCTBLL
{ /// <summary>
    /// Created By : Vivek Agrawal
    /// Date : 25/Feb/2012
    /// Purpose : This class is responsible for operations like
    /// a) Creation of fast download command
    /// b) Down Loading the Data.
    /// </summary>
    public class FastDownLoadingBLL
    {//varible to maintain commands structure.
        FastDownLoadCommands fastDownLoadCommands;
        FastDownLoadingDAL fastDownLoadingDAL;

        public delegate void OnFDLStatusChangedBLL(string statusMessage);
        public event OnFDLStatusChangedBLL onfdlStatusChangedBLL;

        //Array to maintain the Fast Down Load command as bytes.
        /*GKG Changes to maintain dyanmic Id*/
       // byte[] fastDownLoadCmd = new byte[13];
        byte[] fastDownLoadCmd = null;
        int meterIDLength = 8;
        /*GKG Changes to maintain dyanmic Id*/
        public FastDownLoadingBLL(string meterID)
        {
            //Create Meter ID in Hex for the command & set in byte command array.
            SetMeterIDInCommand(meterID);
            fastDownLoadingDAL = new FastDownLoadingDAL();
            fastDownLoadingDAL.onfdlStatusChangedDAL += new FastDownLoadingDAL.OnFDLStatusChangedDAL(SetDataDownloadStatus);

            //Get Structure of various fast downloading commands.
            fastDownLoadCommands = (FastDownLoadCommands)fastDownLoadingDAL.LoadXMLFileInDataStructure(typeof(FastDownLoadCommands), "FastDownLoadingCommands.xml");
        }
        //public FastDownLoadingBLL(string xmlfileName,Type type)
        //{
        //    fastDownLoadingDAL.LoadXMLFileInDataStructure(type, xmlfileName);
        //}

        #region SetMeterIDInCommand
        /// <summary>
        /// Created By : Vivek Agrawal
        /// Date : 24/Feb/2012
        /// Purpose : Create Meter ID in Hex for the command.
        /// </summary>
        private void SetMeterIDInCommand(string meterID)
        {
            /*GKG Changes to maintain dyanmic Id*/
            //string tempStr = "";
            meterIDLength =meterID.Length;
            fastDownLoadCmd = new byte[meterIDLength + 5];    // Five fixed for command size
            /*GKG Changes to maintain dyanmic Id*/
           
            int i = 0;
            foreach (char ch in meterID)
            {
                fastDownLoadCmd[i]=Convert.ToByte(GetHexValue(ch),16);
                i++;
            }
        }
        #endregion

        #region CreateCommand
        /// <summary>
        /// Created By : Vivek Agrawal
        /// Date : 24/Feb/2012
        /// Purpose : Create Hex Command according to download option
        /// </summary>
        /// <param name="fastDownLoadOptions"></param>
        private void CreateCommand(FastDownLoadOptions fastDownLoadOptions)
        {
            for (int i = 0; i < fastDownLoadCommands.Tables[0].Rows.Count; i++)
            {//Select the command structure.
                if (fastDownLoadCommands.Tables[0].Rows[i][0].ToString() == fastDownLoadOptions.ToString())
                {
                    string[] tempCommand = fastDownLoadCommands.Tables[0].Rows[i][1].ToString().Split('|');
                    /*GKG Changes to maintain dyanmic Id*/
                    //Create the command.
                    for (int j = 1; j < tempCommand.Length; j++)
                    {
                        fastDownLoadCmd[j + (meterIDLength - 1)] = Convert.ToByte(tempCommand[j], 16);
                    }
                    break;
                    /*GKG Changes to maintain dyanmic Id*/
                }
            }
        }
        #endregion

        #region Download Data
        /// <summary>
        /// Created By : Vivek Agrawal
        /// Date : 24/Feb/2012
        /// Purpose : Create the command & Download the data as per the option selected.
        /// Downloaded Data is returned as out parameter
        /// </summary>
        /// <param name="fastDownLoadOptions"></param>
        /// <param name="downloadedData"></param>
        /// <returns>FastDownLoadStatuses</returns>
        public FastDownLoadStatuses DownloadData(string fastDownloadPort,FastDownLoadOptions fastDownLoadOptions, out string downloadedData)
        {
            FastDownLoadStatuses fastDownLoadStatus = FastDownLoadStatuses.None;
            //Create the command for the download option.
            CreateCommand(fastDownLoadOptions);
            switch (fastDownLoadOptions)
            
            //NOTE : Size is specified as parameter in Read Data function. This implementation is under consideration.
                    //If this implementation goes fine the Size parameter will not be hard coded.

            {//Download Tamper Data.
                case FastDownLoadOptions.General:
                    fastDownLoadStatus = fastDownLoadingDAL.ReadData(fastDownloadPort,fastDownLoadCmd,ConfigInfo.GetFDLGeneralDataSize(),out downloadedData,fastDownLoadOptions);
                    break;
                case FastDownLoadOptions.Instant:
                    fastDownLoadStatus = fastDownLoadingDAL.ReadData(fastDownloadPort, fastDownLoadCmd, ConfigInfo.GetFDLInstantDataSize(), out downloadedData, fastDownLoadOptions);
                    break;
                case FastDownLoadOptions.Phasor:
                    fastDownLoadStatus = fastDownLoadingDAL.ReadData(fastDownloadPort, fastDownLoadCmd, ConfigInfo.GetFDLPhasorDataSize(), out downloadedData, fastDownLoadOptions);
                    break;
                
                case FastDownLoadOptions.TamperData:
                    // To solve bug 89139, hard coding is removed for serial port selection.
                    fastDownLoadStatus = fastDownLoadingDAL.ReadData(fastDownloadPort,fastDownLoadCmd, ConfigInfo.GetFDLTamperDataSize(), out downloadedData, fastDownLoadOptions);
                    break;
                    //Dwonload LS Data.
                case FastDownLoadOptions.LSData:
                    // To solve bug 89139, hard coding is removed for serial port selection.
                    fastDownLoadStatus = fastDownLoadingDAL.ReadData(fastDownloadPort,fastDownLoadCmd, ConfigInfo.GetFDLLSDataSize(), out downloadedData, fastDownLoadOptions);
                    break;
                
                case FastDownLoadOptions.BillingData:
                    fastDownLoadStatus = fastDownLoadingDAL.ReadData(fastDownloadPort, fastDownLoadCmd, ConfigInfo.GetFDLBillingDataSize(), out downloadedData, fastDownLoadOptions);
                    break;
                case FastDownLoadOptions.MidNight:
                    fastDownLoadStatus = fastDownLoadingDAL.ReadData(fastDownloadPort, fastDownLoadCmd, ConfigInfo.GetMidNightDataSize(), out downloadedData, fastDownLoadOptions);
                    break;
                case FastDownLoadOptions.Anomaly:
                    fastDownLoadStatus = fastDownLoadingDAL.ReadData(fastDownloadPort, fastDownLoadCmd, ConfigInfo.GetAnomalyDataSize(), out downloadedData, fastDownLoadOptions);
                    break;
                /* GKG JVVNL Current TOU Read */
                case FastDownLoadOptions.TOU:
                    fastDownLoadStatus = fastDownLoadingDAL.ReadData(fastDownloadPort, fastDownLoadCmd, ConfigInfo.GetTOUDataSize(), out downloadedData, fastDownLoadOptions);
                    break;
                /* GKG JVVNL Current TOU Read */
                default://Download Billing Data.
                    // To solve bug 89139, hard coding is removed for serial port selection.
                    fastDownLoadStatus = fastDownLoadingDAL.ReadData(fastDownloadPort, fastDownLoadCmd, ConfigInfo.GetFDLBillingDataSize(), out downloadedData, fastDownLoadOptions);
                    break;
                    break;
            }
           // if(fast
            return fastDownLoadStatus;
        }
        #endregion

        #region GetHexValue
        /// <summary>
        /// Created By : Vivek Agrawal
        /// Date : 24/Feb/2012
        /// Purpose : Returns Hex Value for a Character
        /// </summary>
        /// <param name="ch"></param>
        /// <returns>string</returns>
        private string GetHexValue(Char ch)
        {
            return String.Format("{0:x2}", Convert.ToInt32(ch));
        }
        #endregion

        private void SetDataDownloadStatus(string statusMessage)
        {
            onfdlStatusChangedBLL(statusMessage);
        }
    }
}

