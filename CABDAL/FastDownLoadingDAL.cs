/*
File Name: FastDownLoadingDAL.cs
Created By: Vivek Agrawal
Date : 24/Feb/2012
Purpose: Fast Downloading business layer implementation.
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using Utilities;
using CABEntity;
using SerialCommunication;
using Hunt.EPIC.Logging;


namespace LTCTDAL
{
    /// <summary>
    /// Created By : Vivek Agrawal
    /// Date : 24/Feb/2012
    /// Purpose : This class is responsible for operations like
    /// a) Getting Commands Structures Data.
    /// b) Reading the Data from Meter.
    /// </summary>
    public class FastDownLoadingDAL
    {
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(FastDownLoadingDAL).ToString());
        #region LoadFastDownLoadCommands
        /// <summary>
        /// Created By : Vivek Agrawal
        /// Date : 24/Feb/2012
        /// Purpose : Load the xml file in data structure.
        /// </summary>
        /// <returns>deserializedData</returns>
        public object LoadXMLFileInDataStructure(Type type,string xmlFileName)
        {
            object deserializedData = null;
            XmlSerializer serializer = new XmlSerializer(type);//FastDownLoadCommands
            string appPath = System.AppDomain.CurrentDomain.BaseDirectory;
            TextReader textReader = null;
            try
            {
                textReader = new StreamReader(appPath +xmlFileName);// "FastDownLoadingCommands.xml");
                //Read the Fast Downloading Commands into the data structure(De Serialization)
                deserializedData = serializer.Deserialize(textReader);// as FastDownLoadCommands;
            }
            catch (FileNotFoundException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "LoadXMLFileInDataStructure(Type type,string xmlFileName)", ex);
                throw ex;
            }
            catch (InvalidOperationException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "LoadXMLFileInDataStructure(Type type,string xmlFileName)", ex);
                throw ex;
            }
            finally
            {
                if (textReader != null) { textReader.Close(); textReader.Dispose(); }
            }
            return deserializedData;
        }
        #endregion

        public delegate void OnFDLStatusChangedDAL(string statusMessage);
        public event OnFDLStatusChangedDAL onfdlStatusChangedDAL;

        #region ReadData
        /// <summary>
        /// Created By : Vivek Agrawal
        /// Date : 24/Feb/2012
        /// Purpose : Read data from the port according to the command.
        /// </summary>
        /// <param name="fastDownLoadCmd"></param>
        /// <param name="bufferSize"></param>
        /// <param name="downloadedData"></param>
        /// <returns></returns>
        public FastDownLoadStatuses ReadData(string fastDownloadPort,Byte[] fastDownLoadCmd, int bufferSize, out string downloadedData,FastDownLoadOptions fastDownLoadOptions)
        {
            string hexString = "123456789ABCDEF";
            char[] hexChars = hexString.ToCharArray();
            int bccIndex, lastIndex;
            string bcc = string.Empty;
            string actualData = string.Empty;
            downloadedData = string.Empty;
            //Set the recieve buffer size.
            GlobalObjects.objSerialComm = new SerialCommunication.SerialComm(bufferSize);
            //NOTE : This implementation is under consideration.

            //If this implementation goes fine then hard coding will be removed.            
            //Com port settings.

            // To solve bug 89139, hard coding is removed for serial port selection.
            GlobalObjects.objSerialComm.SetSerialPortSettings(fastDownloadPort, "9600", "None", "8", "1",6000, 5000);
          //  GlobalObjects.objSerialComm.onfdlStatusChanged += new SerialComm.OnFDLStatusChanged(SetDataDownloadStatus);

            GlobalObjects.objSerialComm.OpenPort();
            try
            {//NOTE : This implementation is under consideration.
                bool communicationTimeOut = false;
                /*GKG Changes to maintain dyanmic Id*/
//                if (GlobalObjects.objSerialComm.SendFastDownLoadCmdToPort(fastDownLoadCmd, 13, out communicationTimeOut) == false)
                if (GlobalObjects.objSerialComm.SendFastDownLoadCmdToPort(fastDownLoadCmd, (byte)fastDownLoadCmd.Length, out communicationTimeOut) == false)
                /*GKG Changes to maintain dyanmic Id*/
                {
                    //VBM - make changes for JVVNL - HT to by pass timeoout error in case of Anomaly and TOU , as meter is not supporting 
                    //These two command . This is not a good practice but we have to do it to avoid having two separate logins for HT and LT utility for JVVNL.
                    if (communicationTimeOut && fastDownLoadOptions != FastDownLoadOptions.Anomaly && fastDownLoadOptions != FastDownLoadOptions.TOU)
                        return FastDownLoadStatuses.ErrorInCommunication;
                }
                else
                {

                }
            }
            catch (ArgumentException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "LoadXMLFileInDataStructure(Type type,string xmlFileName)", ex);
                return FastDownLoadStatuses.BuffersizeNotSufficient;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "LoadXMLFileInDataStructure(Type type,string xmlFileName)", ex);
                return FastDownLoadStatuses.ErrorInCommunication;
            }
            finally
            {
                GlobalObjects.objSerialComm.ClosePort();
            }
            //Convert the byte array to string.
            downloadedData = BitConverter.ToString(GlobalObjects.objSerialComm.fastDownLoadBuffer).Replace("-", "");
            
            if (fastDownLoadOptions == FastDownLoadOptions.LSData || fastDownLoadOptions == FastDownLoadOptions.MidNight)
                {
                    // To get the last index of any hex string that will be checksum.
                    bccIndex = downloadedData.LastIndexOfAny(hexChars);
                    // If check sum is non-zero.
                    if (bccIndex % 2 != 0)
                    {
                        if (bccIndex < 0)
                        {
                            bccIndex = 1;
                        }
                        bcc = downloadedData.Substring(bccIndex - 1, 2);                       
                        actualData = downloadedData.Substring(0, bccIndex - 1);
                    }
                    else
                    {
                        bcc = downloadedData.Substring(bccIndex, 2);
                        actualData = downloadedData.Substring(0, bccIndex);
                    }
                    // To get the last index of any hex string in valid data.
                    lastIndex = actualData.LastIndexOfAny(hexChars);
                    if ((lastIndex + 1) % 2 != 0)
                        lastIndex++;
                    // Getting actual data with appended bcc.
                    
                    downloadedData = actualData.Substring(0, lastIndex + 1) + bcc;
                    // Added to solve bug 89140.
                    if(!string.IsNullOrEmpty(downloadedData))
                    {
                        if (!VerifyBCC(downloadedData))
                        {
                            return FastDownLoadStatuses.ErrorInCommunication;
                        }
                    }
                }
                else
                {
                    // Will run in case of tamper and billing data.
                    while ((downloadedData.LastIndexOf("00") == (downloadedData.Length - 2)) && downloadedData != null)
                        downloadedData = downloadedData.Substring(0, downloadedData.Length - 2);
                    // Added to solve bug 89140.
                    if (!string.IsNullOrEmpty(downloadedData))
                    {
                        if (!VerifyBCC(downloadedData))
                        {
                            return FastDownLoadStatuses.ErrorInCommunication;
                        }
                    }
                }  
            return FastDownLoadStatuses.None;
        }
        #endregion
        /// <summary>
        /// This method will calculate the bcc.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private byte CalculateBcc(string data)
        {
            byte bcc = 0;
            int i = 0;
            for (i = 0; i < data.Length; i += 2)
            {
                if (i == 0)
                {
                    bcc = Convert.ToByte(data.Substring(i, 2), 16);
                    continue;
                }//XOR Operation.
                bcc = (byte)(bcc ^ Convert.ToByte(data.Substring(i, 2), 16));
            }
            return bcc;
        }
        /// <summary>
        /// This method will check validation of BCC of the readout data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool VerifyBCC(string tagwiseData)
        {
            byte bccValue;
            bccValue = CalculateBcc(tagwiseData.Substring(0, tagwiseData.Length - 2));
            if (bccValue != Convert.ToByte(tagwiseData.Substring(tagwiseData.Length - 2, 2), 16))
                return false;
            return true;
        }
        private void SetDataDownloadStatus(string statusMessage)
        {
            onfdlStatusChangedDAL(statusMessage);
        }
    }
}
