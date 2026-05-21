/*
File Name: FastDownLoading.cs
Created By: Vivek Agrawal
Date : 24/Feb/2012
Purpose: Fast Downloading implementation.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities;
using CABEntity;
using LTCTBLL;
using System.IO;
using CHANNEL.Formatter;
using CHANNEL;
using CAB.Entity;
using CAB.Entity.Base;
using CAB.BLL;
using CAB.Channel.Formatter;
using CAB.Framework.Entity;
using CAB.Framework.Utility;


namespace FastDownloading
{
    /// <summary>
    /// Created By : Vivek Agrawal
    /// Date : 24/Feb/2012
    /// Purpose : This class is responsible for operations liks
    /// a) Validation of user input required for fast downloading.
    /// b) Down Loading the Data.
    /// </summary>
    public class FDLOperations
    {//array to maintain downloaded oprions selected by user.
        /* GKG JVVNL Current TOU Read */
        //private bool[] downLoadOptions = new bool[8];
        private bool[] downLoadOptions = new bool[9];
        /* GKG JVVNL Current TOU Read */
        //meter id input by user.
        private string meterID = string.Empty;
        public delegate void OnFastDownloadingStatusChanged(string statusMessage);
        public event OnFastDownloadingStatusChanged OnfastDownloadingStatusChanged;
        public delegate void OnFDLUploadStatusChanged(string statusMessage);
        public event OnFDLUploadStatusChanged OnfdlUploadStatusChanged;

        public delegate void OnFDLStatusChanged(string statusMessage);
        public event OnFDLStatusChanged onfdlStatusChanged;

        private System.Resources.ResourceManager rmFDLOperations;

        public FDLOperations()
        {
            rmFDLOperations = new System.Resources.ResourceManager("FastDownloading.FDLOperationsResource", System.Reflection.Assembly.GetExecutingAssembly());
        }

        public FDLOperations(bool downLoadTampers, bool downLoadLS, bool downLoadBilling, bool downLoadGeneral, bool downLoadInstant, bool downLoadPhasor, bool downLoadMidNight, string meterID)
        {
            downLoadOptions[0] = downLoadGeneral;
            downLoadOptions[1] = downLoadInstant;
            downLoadOptions[2] = downLoadPhasor;
            downLoadOptions[3] = downLoadTampers;
            downLoadOptions[4] = downLoadLS;
            downLoadOptions[5] = downLoadBilling;
            downLoadOptions[6] = downLoadMidNight;
            if (UtilityDetails.ShowAnamolyParameters && UtilityDetails.PrimaryUtlityName != "TNEB")
            {
                downLoadOptions[7] = downLoadInstant;
            }
            else
            {
                downLoadOptions[7] = false;
            }

            downLoadOptions[8] = false;
            this.meterID = meterID;
            rmFDLOperations = new System.Resources.ResourceManager("FastDownloading.FDLOperationsResource", System.Reflection.Assembly.GetExecutingAssembly());
        }

        #region Uploading of FDL file
        /// <summary>
        /// Created By : Vivek Agrawal
        /// Date : 27/Feb/2012
        /// Purpose : Split the data according to tag and then pass
        /// the data to the relevant class to Upload it.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileText"></param>
        /// <param name="fileUploadID"></param>
        public FDLFileUploadStatuses UploadFDLData(string fileName, string fileText, long fileUploadID)
        {
            ParseFDLData parseFDLData;
            DLMS650FormatterBilling formatterBilling = new DLMS650FormatterBilling();
            BillingGeneralNFDLMSEntity master = new BillingGeneralNFDLMSEntity();
            DLMS650NamePlateDetailsEntity namePlateDetails = null;
            try
            {//Split tag wise data.
                if (fileText.Trim().Length == 0)
                    return FDLFileUploadStatuses.NoDataToUpload;
                /* GKG JVVNL Current TOU Read */
                //string[] tagwiseData = fileText.Split(new string[] { "FDLDATA\\", "\\GENERAL\\", "\\INSTANT\\", "\\PHASOR\\", "\\TAMPERDATA\\", "\\LSDATA\\", "\\BILLINGDATA\\", "\\MIDNIGHT\\", "\\ANOMALY\\" }, StringSplitOptions.None);
                string[] tagwiseData = fileText.Split(new string[] { "FDLDATA\\", "\\GENERAL\\", "\\INSTANT\\", "\\PHASOR\\", "\\TAMPERDATA\\", "\\LSDATA\\", "\\BILLINGDATA\\", "\\MIDNIGHT\\", "\\ANOMALY\\","\\TOU\\" }, StringSplitOptions.None);
                /* GKG JVVNL Current TOU Read */
                int i = 2;

                MeterDataEntity mtrDataEntity = SetMeterDataDetails(tagwiseData[1], fileUploadID);
                if (fileText.IndexOf("GENERAL") > 0)
                {
                    OnfdlUploadStatusChanged(rmFDLOperations.GetString("ParsingGeneralData"));
                    parseFDLData = new ParseFDLGeneralData(tagwiseData[i++],fileText,fileUploadID,mtrDataEntity.MeterData_ID);
                    if (parseFDLData.Parse(out namePlateDetails) == FDLFileParseStatuses.BCCMismatchGeneral)
                    {
                        return FDLFileUploadStatuses.BCCMismatchGeneral;
                    }
                    master.General = (DLMS650NamePlateDetailsEntity)namePlateDetails;
                    tagwiseData[i - 1] = "";

                   

                }

                if (fileText.IndexOf("INSTANT") > 0)
                {
                    OnfdlUploadStatusChanged(rmFDLOperations.GetString("ParsingInstantData"));
                    parseFDLData = new ParseFDLInstantData(tagwiseData[i++], fileText, fileUploadID, mtrDataEntity.MeterData_ID,namePlateDetails.DemandResolution,namePlateDetails.EnergyResolution );
                    parseFDLData.OnfdlParsingStatusChanged += new ParseFDLData.OnFDLParsingStatusChanged(parseFDLData_OnParseStatusChanged);
                    if (parseFDLData.Parse() == FDLFileParseStatuses.BCCMismatchInstantaneous)
                        return FDLFileUploadStatuses.BCCMismatchInstantaneous;
                    tagwiseData[i - 1] = "";
                }
                if (fileText.IndexOf("PHASOR") > 0)
                {
                    parseFDLData = new ParseFDLPhasorData(tagwiseData[i++], fileText, fileUploadID, mtrDataEntity.MeterData_ID);
                    if (parseFDLData.Parse() == FDLFileParseStatuses.BCCMismatchGeneral)
                    {
                        return FDLFileUploadStatuses.BCCMismatchGeneral;
                    }
                    tagwiseData[i - 1] = "";
                }
                
                if (fileText.IndexOf("TAMPERDATA") > 0)
                {//Tamper Data
                    OnfdlUploadStatusChanged(rmFDLOperations.GetString("ParsingTamperData"));
                    parseFDLData = new ParseTamperFDLData(tagwiseData[i++], fileText, fileUploadID, mtrDataEntity.MeterData_ID, namePlateDetails.DemandResolution, namePlateDetails.EnergyResolution);
                    parseFDLData.OnfdlParsingStatusChanged += new ParseFDLData.OnFDLParsingStatusChanged(parseFDLData_OnParseStatusChanged);
                    if (parseFDLData.Parse() == FDLFileParseStatuses.BCCMismatchTamper)
                        return FDLFileUploadStatuses.BCCMismatchTamper;
                    tagwiseData[i - 1] = "";
                    // added to solve bug 74492
                    //OnfdlUploadStatusChanged(rmFDLOperations.GetString("TamperDataParsed"));
                }
                if (fileText.IndexOf("LSDATA") > 0)
                {//Load Survey Data
                    OnfdlUploadStatusChanged(rmFDLOperations.GetString("ParsingLSData"));
                    parseFDLData = new ParseLSFDLData(tagwiseData[i++], fileText, fileUploadID, mtrDataEntity.MeterData_ID, namePlateDetails.DemandResolution, namePlateDetails.EnergyResolution);
                    parseFDLData.OnfdlParsingStatusChanged += new ParseFDLData.OnFDLParsingStatusChanged(parseFDLData_OnParseStatusChanged);
                    if (parseFDLData.Parse() == FDLFileParseStatuses.BCCMismatchLS)
                        return FDLFileUploadStatuses.BCCMismatchLS;
                    tagwiseData[i - 1] = "";
                    // added to solve bug 74492
                    //OnfdlUploadStatusChanged(rmFDLOperations.GetString("LSDataParsed"));
                }
               
                if (fileText.IndexOf("BILLINGDATA") > 0)
                {//billing Data.
                    OnfdlUploadStatusChanged(rmFDLOperations.GetString("ParsingBillingData"));
                    parseFDLData = new ParseFDLBillingData(tagwiseData[i++], fileText, fileUploadID, mtrDataEntity.MeterData_ID, namePlateDetails.DemandResolution, namePlateDetails.EnergyResolution);
                    parseFDLData.OnfdlParsingStatusChanged += new ParseFDLData.OnFDLParsingStatusChanged(parseFDLData_OnParseStatusChanged);
                    if (parseFDLData.Parse() == FDLFileParseStatuses.BCCMismatchBilling)
                        return FDLFileUploadStatuses.BCCMismatchBilling;
                    tagwiseData[i - 1] = "";
                    // added to solve bug 74492
                    //OnfdlUploadStatusChanged(rmFDLOperations.GetString("BillingDataParsed"));
                }
                if (fileText.IndexOf("MIDNIGHT") > 0)
                {//billing Data.
                    //OnfdlUploadStatusChanged(rmFDLOperations.GetString("ParsingBillingData"));
                    parseFDLData = new ParseFDLMidNightData(tagwiseData[i++], fileText, fileUploadID, mtrDataEntity.MeterData_ID, namePlateDetails.DemandResolution, namePlateDetails.EnergyResolution);
                    parseFDLData.OnfdlParsingStatusChanged += new ParseFDLData.OnFDLParsingStatusChanged(parseFDLData_OnParseStatusChanged);
                    if (parseFDLData.Parse() == FDLFileParseStatuses.BCCMismatchBilling)
                        return FDLFileUploadStatuses.BCCMismatchBilling;
                    tagwiseData[i - 1] = "";
                    // added to solve bug 74492
                    //OnfdlUploadStatusChanged(rmFDLOperations.GetString("BillingDataParsed"));
                }
                if (fileText.IndexOf("ANOMALY") > 0)
                {//billing Data.
                    //OnfdlUploadStatusChanged(rmFDLOperations.GetString("ParsingBillingData"));
                    parseFDLData = new ParseFDLAnomalyData(tagwiseData[i++], fileText, fileUploadID, mtrDataEntity.MeterData_ID, namePlateDetails.DemandResolution, namePlateDetails.EnergyResolution);
                    parseFDLData.OnfdlParsingStatusChanged += new ParseFDLData.OnFDLParsingStatusChanged(parseFDLData_OnParseStatusChanged);
                    if (parseFDLData.Parse() == FDLFileParseStatuses.BCCMismatchBilling)
                        return FDLFileUploadStatuses.BCCMismatchBilling;
                    tagwiseData[i - 1] = "";
                    // added to solve bug 74492
                    //OnfdlUploadStatusChanged(rmFDLOperations.GetString("BillingDataParsed"));
                }
                /* GKG JVVNL Current TOU Read */
                if (fileText.IndexOf("TOU") > 0)
                {
                    parseFDLData = new ParseFDLTOUData(tagwiseData[i++], fileText, fileUploadID, mtrDataEntity.MeterData_ID);
                    parseFDLData.OnfdlParsingStatusChanged += new ParseFDLData.OnFDLParsingStatusChanged(parseFDLData_OnParseStatusChanged);
                    if (parseFDLData.Parse() == FDLFileParseStatuses.BCCMismatchBilling)
                        return FDLFileUploadStatuses.BCCMismatchBilling;
                    tagwiseData[i - 1] = "";
                    // added to solve bug 74492
                    //OnfdlUploadStatusChanged(rmFDLOperations.GetString("BillingDataParsed"));
                }
                /* GKG JVVNL Current TOU Read */
                //DLMS650FormatterBilling formatterBilling = new DLMS650FormatterBilling();
                //BillingGeneralNFDLMSEntity master = new BillingGeneralNFDLMSEntity();
                
                //formatterBilling.GetData(tagwiseData[0], master);
                ////if (master.General != null)// && !string.IsNullOrEmpty(mtrDataEntity.MeterData_ID))
                ////{
                ////    DLMS650NamePlateDetailsEntity general = master.General as DLMS650NamePlateDetailsEntity;
                ////    OnfdlUploadStatusChanged("Please wait. Uploading General Data");
                ////    general.MeterData_ID = mtrDataEntity.MeterData_ID;
                ////    new DLMS650GeneralBLL().InsertData(general);
                ////}
                ////if (master.Instant != null)// && !string.IsNullOrEmpty(meterDataEntity.MeterID))
                ////{
                ////    OnfdlUploadStatusChanged("Please wait. Uploading Instant Data");
                ////    DLMS650InstantaneousBLL instantBLL = new DLMS650InstantaneousBLL();
                ////    List<IEntity> entities = new List<IEntity>();
                ////    for (int counter = 0; counter < master.Instant.Count; counter++)
                ////    {
                ////        master.Instant[counter].MeterDataID = mtrDataEntity.MeterData_ID;
                ////        entities.Add(master.Instant[counter]);
                ////    }
                ////    instantBLL.InsertData(entities);
                //}
                return FDLFileUploadStatuses.FileUploadedSuccessfully;
            }
            //catch (InvalidOperationException)
            //{
            //    return FDLFileUploadStatuses.UnableToReadPacketStructureXMLFile;
            //}
            catch (Exception ex)
            {
                return FDLFileUploadStatuses.FileCorrupt;
            }
            finally
            {
                parseFDLData = null;
            }
        }
        #endregion

        #region SetMeterDataDetails
        /// <summary>
        /// Created By : Vivek Agrawal
        /// Date : 27/Feb/2012
        /// Purpose : Set Meter data details in DB &
        /// Generate Meter Data ID.
        /// </summary>
        private MeterDataEntity SetMeterDataDetails( string fileText, long fileUploadID)
        {
            //Get Meter Data ID for this readout.
            MeterDataEntity mtrDataEntity = new MeterDataEntity();
            string[] splitData=fileText.Split(new string[] {"\\METERID\\","\\DATETIME\\" },StringSplitOptions.None);
            mtrDataEntity.MeterID = splitData[1];// fileText.Substring(fileText.IndexOf("METERID\\") + 8, fileText.IndexOf("\\DATETIME\\") - fileText.IndexOf("METERID\\") - 8);
            mtrDataEntity.ReadingDateTime = Convert.ToInt64(splitData[2].Replace("\\",""));//fileText.Substring(fileText.LastIndexOf("\\DATETIME\\") + 10, fileText.IndexOf("\\\\") - fileText.IndexOf("\\DATETIME\\") - 10));
            mtrDataEntity.FileUpload_ID = fileUploadID;
            mtrDataEntity = new MeterDataBLL().InsertData(mtrDataEntity) as MeterDataEntity;
            return mtrDataEntity;
        }
        #endregion

        #region ValidateInput
        /// <summary>
        /// Created By : Vivek Agrawal
        /// Date : 24/Feb/2012
        /// Purpose : Validation of input required for fast downloading.
        /// Returns the status if input is valid or not.
        // </summary>
        /// <returns>FastDownLoadStatus</returns>
        public FastDownLoadStatuses ValidateInput()
        {
            //if (meterID.Length ==0)//Blank Meter ID Validation
            //    return FastDownLoadStatuses.BlankMeterID;
          //  if (meterID.Trim().Length != 8)//Length of Meter ID Validation
             //   return FastDownLoadStatuses.IncorrectMeterID;
            //Validation for selection of any download option.
            if (!(downLoadOptions[0] || downLoadOptions[1] || downLoadOptions[2]))
                return FastDownLoadStatuses.NoOptionToDownLoad;
            return FastDownLoadStatuses.None;
        }
        #endregion

        #region DownloadData
        /// <summary>
        /// Created By : Vivek Agrawal
        /// Date : 24/Feb/2012
        /// Purpose : Down load the data for all the options selected by user.
        /// This method accumulated all the data downloaded.
        /// This method return 
        /// a) a status for download. b) Complete Downloaded data (as out parameter)
        /// </summary>
       /// <param name="completeDownLoadedData"></param>
        /// <returns>FastDownLoadStatus</returns>
        public FastDownLoadStatuses DownloadData(string fastDownloadPort,out string completeDownLoadedData)
        {
            FastDownLoadingBLL fastDownLoadingBLL = new FastDownLoadingBLL(meterID);
            completeDownLoadedData="";
            string downloadedData = "";
            FastDownLoadStatuses fastDownLoadStatus = FastDownLoadStatuses.None;
            //fastDownLoadingBLL.onfdlStatusChangedBLL += new FastDownLoadingBLL.OnFDLStatusChangedBLL(SetDataDownloadStatus);

            //Loop to download data for downloading options.
            for (int i = 0; i < downLoadOptions.Length; i++)
            {
                if (downLoadOptions[i])
                {
                    FastDownLoadOptions fastDownLoadOptions ;
                    //Download data for a option selected by user.
                    switch(i)
                    {
                        case 0:
                            fastDownLoadOptions = FastDownLoadOptions.General;
                            OnfastDownloadingStatusChanged(rmFDLOperations.GetString("GeneralReadingInprogress"));
                            break;
                        case 1:
                            fastDownLoadOptions = FastDownLoadOptions.Instant;
                            OnfastDownloadingStatusChanged(rmFDLOperations.GetString("InstantDataReadingInprogress"));
                            break;
                        case 2:
                            fastDownLoadOptions = FastDownLoadOptions.Phasor;
                            OnfastDownloadingStatusChanged(rmFDLOperations.GetString("PhasorDataReadingInprogress"));
                            break;
                        case 3:
                            fastDownLoadOptions = FastDownLoadOptions.TamperData;
                            OnfastDownloadingStatusChanged(rmFDLOperations.GetString("TamperDataReadingInprogress"));
                            break;
                        case 4:
                            fastDownLoadOptions = FastDownLoadOptions.LSData;
                            OnfastDownloadingStatusChanged(rmFDLOperations.GetString("LSDataReadingInprogress"));
                            break;
                        case 5:
                            fastDownLoadOptions = FastDownLoadOptions.BillingData;
                            OnfastDownloadingStatusChanged(rmFDLOperations.GetString("BillingDataReadingInprogress"));
                            break;
                        case 6:
                            fastDownLoadOptions = FastDownLoadOptions.MidNight;
                            OnfastDownloadingStatusChanged(rmFDLOperations.GetString("MidNightDataReadingInprogress"));
                            break;
                        case 7:
                            fastDownLoadOptions = FastDownLoadOptions.Anomaly;
                            OnfastDownloadingStatusChanged(rmFDLOperations.GetString("AnomalyDataReadingInprogress"));
                            break;
                        /* GKG JVVNL Current TOU Read */
                        case 8:
                            fastDownLoadOptions = FastDownLoadOptions.TOU;
                            OnfastDownloadingStatusChanged(rmFDLOperations.GetString("TOUDataReadingInprogress"));
                            break;
                        /* GKG JVVNL Current TOU Read */
                        default ://case 2:(Billing)
                            fastDownLoadOptions = FastDownLoadOptions.MidNight;
                            OnfastDownloadingStatusChanged(rmFDLOperations.GetString("MidNightDataReadingInprogress"));
                            break;    
                          
                    }
                    // To solve bug 89139, hard coding is removed for serial port selection.
                    fastDownLoadStatus = fastDownLoadingBLL.DownloadData(fastDownloadPort,fastDownLoadOptions, out downloadedData);
                    //if data is downloaded then accumulate the data in with a Tag(for identification).
                    if (fastDownLoadStatus == FastDownLoadStatuses.None && downloadedData != null && downloadedData.Length != 0)
                        completeDownLoadedData += "\\" + fastDownLoadOptions.ToString().ToUpper() + "\\" + downloadedData;
                    // To solve bug 89140. 
                    if (fastDownLoadStatus == FastDownLoadStatuses.ErrorInCommunication)
                    {
                        return fastDownLoadStatus;
                    }
                    // Commented else condition to iterate if all reading options are selected. 1-May-2012.
                    //else //if(fastDownLoadStatus==FastDownLoadStatuses.ErrorInCommunication)
                    //    return fastDownLoadStatus;
                    //else if(fastDownLoadStatus==FastDownLoadStatuses.BuffersizeNotSufficient)

                }
            }
            return FastDownLoadStatuses.None;
        }
        #endregion

        private void parseFDLData_OnParseStatusChanged(string msg)
        {
            OnfdlUploadStatusChanged(msg);
        }
        private void SetDataDownloadStatus(string statusMessage)
        {
           onfdlStatusChanged(statusMessage);
        }

    }
}
