using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using CAB.Entity;
using CAB.IECFramework.Utility;
using CAB.IECChannel.Formatter;
using System.Windows.Forms;
using System.Collections.ObjectModel;
using CABEntity;
using LTCTBLL;
using CAB.BLL;
using CAB.IECFramework.Entity;

namespace CABApplication
{
    public delegate bool AsyncUploadDelegate(string fileToUpload, string fileContent,int requestID);

    class UploadFileThreadContext
    {
        public AsyncUploadDelegate asyncUploadDelegate;
        public string fileName;
        public int requestID;
    }
    
    public class UploadFileController
    {
        FileUploadStatusBLL fileUploadStatusBLL = new FileUploadStatusBLL();    

        public int UploadFile(string fileToUpload)
        {
            AsyncUploadDelegate asyncUploadDelegate = AsynchronousUpload;
            UploadFileThreadContext uploadFileThreadContext = new UploadFileThreadContext();
            uploadFileThreadContext.asyncUploadDelegate = asyncUploadDelegate;
            
            string fileName=fileToUpload.Substring(fileToUpload.LastIndexOf('\\')+1);
            uploadFileThreadContext.fileName = fileName;
            uploadFileThreadContext.requestID = fileUploadStatusBLL.InsertData(fileName, FileUploadStatus.FileUploadingInitiated);
            IAsyncResult asyncResult = asyncUploadDelegate.BeginInvoke(fileToUpload, GetContent(fileToUpload), uploadFileThreadContext.requestID,
                UploadFileCallbackHandler, uploadFileThreadContext);

            return uploadFileThreadContext.requestID;
            
        }
        public void UploadFileCallbackHandler(IAsyncResult result)
        {
            // Extract the reference to the AsyncExampleDelegate instance
            // from the IAsyncResult instance. This allows you to obtain the
            // completion data.
            //AsyncExampleDelegate longRunningMethod =
            //(AsyncExampleDelegate)result.AsyncState;
            UploadFileThreadContext uploadFileThreadContext = (UploadFileThreadContext)result.AsyncState;

            // Obtain the completion data for the asynchronous method.
            bool isUploaded = false;

            try
            {
                isUploaded = uploadFileThreadContext.asyncUploadDelegate.EndInvoke(result);

                if (isUploaded)
                {
                    fileUploadStatusBLL.UpdateData(uploadFileThreadContext.requestID, FileUploadStatus.FileUploadedSuccessfully);
                }
                //else
                //{
                //    if (!fileExists)
                //    {
                //        if (string.IsNullOrEmpty(this.StatusMessage))
                //        {
                //          //  this.StatusMessage = "File is not in proper format.";
                //            Application.DoEvents();
                //        }
                //    }
                //}
            }
            catch
            {
                // Catch and handle those exceptions you would if calling
                // LongRunningMethod directly.
            }
           
        }
        public bool AsynchronousUpload(string fileName, string fileContent,int requestID)
        {
            return false;
            //bool ifmeterEntityUploaded = false;
            //bool flag = true;
            //bool fileExists = false;
            //int requestNumber;
            //MeterDataBLL meterDataBLL = new MeterDataBLL();
            //FileUploadStatusBLL fileUploadStatusBLL = new FileUploadStatusBLL();
            //try
            //{
            //    bool isUploaded = false;
            //    if (!isUploaded)
            //    {
            //        if (!FormatterCommon.IsFileNullOrEmpty(fileContent))
            //        {
            //           // this.StatusMessage = "File corrupt.";
            //            fileUploadStatusBLL.UpdateData(requestID, FileUploadStatus.FileCorrupt);
            //          //  Application.DoEvents();
            //            return isUploaded;
            //        }
            //        if (!ConfigInfo.IsValidCheckSum(fileContent))
            //        {
            //           // this.StatusMessage = "BCC mismatched.";
            //            fileUploadStatusBLL.UpdateData(requestID, FileUploadStatus.BCC_Mismatch);
            //           // Application.DoEvents();
            //            return isUploaded;
            //        }
            //        List<BillingGeneralNFEntity> master = new List<BillingGeneralNFEntity>();
            //        FormatterBilling formatterBilling = new FormatterBilling();
            //        formatterBilling.GetData(fileContent, master);


            //        FileUploadMasterEntity fileUploadMasterEntity = new FileUploadMasterEntity();
            //        fileUploadMasterEntity.UserInformation_ID = ConfigInfo.UserInformationID;
            //        if (flag == true)
            //        {
            //            fileUploadMasterEntity.FileContent = TotalBytes(fileName);
            //            fileUploadMasterEntity.FileName = Path.GetFileName(fileName);
            //        }
            //        else
            //        {
            //            fileUploadMasterEntity.FileContent = ASCIIEncoding.UTF8.GetBytes(ConfigInfo.EncryptFile(fileContent));
            //            fileUploadMasterEntity.FileName = Path.GetFileName(fileName);
            //        }
            //        fileUploadMasterEntity.UploadingDateTime = DateUtility.DateTimeToLong(DateTime.Now);
            //        if (flag == true)
            //        {
            //            FileUploadMasterEntity fileEntity = new FileUploadMasterBLL().ValidateFile(fileUploadMasterEntity.FileName) as FileUploadMasterEntity;
            //            if (fileEntity != null)
            //            {
            //                if (fileEntity.FileUpload_ID != 0)
            //                {
            //                   // this.StatusMessage = "File '" + fileEntity.FileName + "' already exists";
            //                    fileUploadStatusBLL.UpdateData(requestID, FileUploadStatus.FileAlreadyExists);
            //                    Application.DoEvents();
            //                    Application.DoEvents();
            //                    fileExists = true;
            //                    return isUploaded;
            //                }
            //            }
            //            fileUploadMasterEntity = new FileUploadMasterBLL().InsertData(fileUploadMasterEntity) as FileUploadMasterEntity;
            //            if (fileUploadMasterEntity.FileUpload_ID == 0)
            //            {
            //              //  this.StatusMessage = "File is too larger. Please rename it.";
            //                fileUploadStatusBLL.UpdateData(requestID, FileUploadStatus.FileIsTooLarge);
            //                Application.DoEvents();
            //                return false;
            //            }
            //        }
            //        else
            //        {
            //            FileUploadMasterEntity fileEntity = new FileUploadMasterBLL().ValidateFile(fileUploadMasterEntity.FileName) as FileUploadMasterEntity;
            //            if (fileEntity.FileUpload_ID == 0)
            //                fileUploadMasterEntity = new FileUploadMasterBLL().InsertData(fileUploadMasterEntity) as FileUploadMasterEntity;
            //            else
            //                fileUploadMasterEntity.FileUpload_ID = fileEntity.FileUpload_ID;
            //        }
            //        isUploaded = true;
            //        foreach (BillingGeneralNFEntity entity in master)
            //        {
            //            #region Save Data
            //            try
            //            {
            //                MeterDataEntity meterDataEntity = entity.MeterData as MeterDataEntity;
            //                if (meterDataEntity != null)
            //                {
            //                    meterDataEntity.FileUpload_ID = fileUploadMasterEntity.FileUpload_ID;
            //                    meterDataEntity.UploadingDateTime = DateUtility.DateTimeToLong(System.DateTime.Now);
            //                    if (meterDataEntity.MeterID != null)
            //                    {
            //                        if (!meterDataBLL.ValidateData(fileUploadMasterEntity.FileUpload_ID, meterDataEntity.MeterID, meterDataEntity.ReadingDateTime))
            //                            meterDataEntity = meterDataBLL.InsertData(meterDataEntity) as MeterDataEntity;
            //                        else
            //                            meterDataEntity = meterDataBLL.GetDetailData(meterDataEntity.MeterID, fileUploadMasterEntity.FileUpload_ID, meterDataEntity.ReadingDateTime) as MeterDataEntity;
            //                    }
            //                    ifmeterEntityUploaded = true;
            //                }

            //                //Saving General Data
                            
            //                //Instant Power
            //                if (entity.CurrentInstant != null && !string.IsNullOrEmpty(meterDataEntity.MeterID))
            //                {
            //                    //this.StatusMessage = " Uploading Instant Power Data";
            //                    fileUploadStatusBLL.UpdateData(requestID, FileUploadStatus.UploadingInstantPowerData);
            //                    Application.DoEvents();
            //                    InstantPowerEntity billingGeneral = entity.CurrentInstant as InstantPowerEntity;
            //                    if (billingGeneral != null)
            //                    {
            //                        billingGeneral.MeterData_ID = meterDataEntity.MeterData_ID;
            //                        billingGeneral = new InstantPowerBLL().InsertData(billingGeneral) as InstantPowerEntity;
            //                    }
            //                }
            //                //General
            //                if (entity.CurrentGeneral != null && !string.IsNullOrEmpty(meterDataEntity.MeterID))
            //                {
            //                   //this.StatusMessage = " Uploading General Data";
            //                    fileUploadStatusBLL.UpdateData(requestID, FileUploadStatus.UploadingGeneralData);
            //                    Application.DoEvents();
            //                    GeneralEntity generalEntity = entity.CurrentGeneral as GeneralEntity;
            //                    if (generalEntity != null)
            //                    {
            //                        generalEntity.MeterData_ID = meterDataEntity.MeterData_ID;
            //                        generalEntity = new GeneralBLL().InsertData(generalEntity) as GeneralEntity;
            //                    }
            //                }
            //                //General Billing
            //                if (entity.CurrentBilling != null && !string.IsNullOrEmpty(meterDataEntity.MeterID))
            //                {
            //                 //   this.StatusMessage = " Uploading Billing Data";
            //                    fileUploadStatusBLL.UpdateData(requestID, FileUploadStatus.UploadingBillingData);
            //                    Application.DoEvents();
            //                    BillingEntity currentBilling = entity.CurrentBilling as BillingEntity;
            //                    if (currentBilling != null)
            //                    {
            //                        currentBilling.MeterData_ID = meterDataEntity.MeterData_ID;
            //                        currentBilling = new BillingBLL().InsertData(currentBilling) as BillingEntity;
            //                    }
            //                }
            //                //general tariff
            //                if (entity.CurrentTariff != null && !string.IsNullOrEmpty(meterDataEntity.MeterID))
            //                {
            //                 //   this.StatusMessage = " Uploading Tariff Energy Data";
            //                    fileUploadStatusBLL.UpdateData(requestID, FileUploadStatus.UploadingTariffEnergyData);
            //                    Application.DoEvents();
            //                    TariffEntity tariffCurrentEntity = entity.CurrentTariff as TariffEntity;
            //                    if (tariffCurrentEntity != null)
            //                    {
            //                        tariffCurrentEntity.MeterData_ID = meterDataEntity.MeterData_ID;
            //                        tariffCurrentEntity = new TariffBLL().InsertData(tariffCurrentEntity) as TariffEntity;
            //                    }
            //                }
            //                //General Tamper
            //                if (entity.CurrentTamper != null && !string.IsNullOrEmpty(meterDataEntity.MeterID))
            //                {
            //                   // this.StatusMessage = " Uploading Tamper Data";
            //                    fileUploadStatusBLL.UpdateData(requestID, FileUploadStatus.UploadingTamperData);
            //                    Application.DoEvents();
            //                    TamperCounterGeneralEntity currentTamper = entity.CurrentTamper as TamperCounterGeneralEntity;
            //                    if (currentTamper != null)
            //                    {
            //                        currentTamper.MeterData_ID = meterDataEntity.MeterData_ID;
            //                        currentTamper = new TamperGeneralBLL().InsertData(currentTamper) as TamperCounterGeneralEntity;
            //                    }
            //                }
            //                //History Data
            //                //Billing Data
            //                if (entity.HistoryBilling != null && !string.IsNullOrEmpty(meterDataEntity.MeterID))
            //                {
            //                   // this.StatusMessage = " Uploading Billing History Data";
            //                    fileUploadStatusBLL.UpdateData(requestID, FileUploadStatus.UploadingBillingHistoryData);
            //                    Application.DoEvents();
            //                    List<IEntity> entities = new List<IEntity>();
            //                    for (int counter = 0; counter < entity.HistoryBilling.Count; counter++)
            //                    {
            //                        entity.HistoryBilling[counter].MeterData_ID = meterDataEntity.MeterData_ID;
            //                        entities.Add(entity.HistoryBilling[counter]);
            //                    }
            //                    if (entities.Count > 0)
            //                        new BillingBLL().InsertData(entities);
            //                }
            //                //Tariff Data
            //                if (entity.HistoryTariff != null && !string.IsNullOrEmpty(meterDataEntity.MeterID))
            //                {
            //                //    this.StatusMessage = " Uploading Tariff History Data";
            //                    fileUploadStatusBLL.UpdateData(requestID, FileUploadStatus.UploadingTariffHistoryData);
            //                    Application.DoEvents();

            //                    List<IEntity> entities = new List<IEntity>();
            //                    for (int counter = 0; counter < entity.HistoryTariff.Count; counter++)
            //                    {
            //                        entity.HistoryTariff[counter].MeterData_ID = meterDataEntity.MeterData_ID;
            //                        entities.Add(entity.HistoryTariff[counter]);
            //                    }
            //                    if (entities.Count > 0)
            //                        new TariffBLL().InsertData(entities);
            //                }
            //                //Tamper Data
            //                if (entity.HistoryTamper != null && !string.IsNullOrEmpty(meterDataEntity.MeterID))
            //                {
            //                  //  this.StatusMessage = " Uploading Tamper History Data";
            //                    fileUploadStatusBLL.UpdateData(requestID, FileUploadStatus.UploadingTamperHistoryData);
            //                    Application.DoEvents();
            //                    List<IEntity> entities = new List<IEntity>();
            //                    for (int counter = 0; counter < entity.HistoryTamper.Count; counter++)
            //                    {
            //                        entity.HistoryTamper[counter].MeterData_ID = meterDataEntity.MeterData_ID;
            //                        entities.Add(entity.HistoryTamper[counter]);
            //                    }
            //                    if (entities.Count > 0)
            //                        new TamperGeneralBLL().InsertData(entities);
            //                }
            //                //Fraud Energy
            //                if (entity.FraudEnergy != null)
            //                {
            //                    meterDataEntity = new MeterDataEntity();
            //                    if (entity.FraudEnergy.ReadingDateTime != 0)
            //                    {
            //                        meterDataEntity.FileUpload_ID = fileUploadMasterEntity.FileUpload_ID;
            //                        meterDataEntity.UploadingDateTime = DateUtility.DateTimeToLong(System.DateTime.Now);
            //                        meterDataEntity.MeterID = entity.FraudEnergy.MeterID;
            //                        meterDataEntity.ReadingDateTime = entity.FraudEnergy.ReadingDateTime;
            //                        meterDataEntity.CMRIID = entity.FraudEnergy.CMRIID;
            //                        meterDataEntity.CMRIType = entity.FraudEnergy.CMRIType;
            //                        if (meterDataEntity.MeterID != null)
            //                        {
            //                            if (!new MeterDataBLL().ValidateData(fileUploadMasterEntity.FileUpload_ID, meterDataEntity.MeterID, meterDataEntity.ReadingDateTime))
            //                                meterDataEntity = new MeterDataBLL().InsertData(meterDataEntity) as MeterDataEntity;
            //                            else
            //                                meterDataEntity = new MeterDataBLL().GetDetailData(meterDataEntity.MeterID, fileUploadMasterEntity.FileUpload_ID, meterDataEntity.ReadingDateTime) as MeterDataEntity;
            //                        }
            //                    }
            //                   // this.StatusMessage = " Uploading Fraud Energy Data";
            //                    fileUploadStatusBLL.UpdateData(requestID, FileUploadStatus.UploadingFraudEnergyData);
            //                    Application.DoEvents();
            //                    FraudEnergyEntity fraudEnergyEntity = entity.FraudEnergy as FraudEnergyEntity;
            //                    if (fraudEnergyEntity != null && !string.IsNullOrEmpty(meterDataEntity.MeterID))
            //                    {
            //                        fraudEnergyEntity.MeterData_ID = meterDataEntity.MeterData_ID;
            //                        fraudEnergyEntity = new FraudEnergyBLL().InsertData(fraudEnergyEntity) as FraudEnergyEntity;
            //                    }
            //                }
            //                //Transaction
            //                if (entity.Programming != null)
            //                {
            //                    meterDataEntity = new MeterDataEntity();
            //                    for (int counter = 0; counter < entity.Programming.Count; counter++)
            //                    {
            //                        if (entity.Programming[counter].ReadingDateTime != 0)
            //                        {
            //                            meterDataEntity.FileUpload_ID = fileUploadMasterEntity.FileUpload_ID;
            //                            meterDataEntity.UploadingDateTime = DateUtility.DateTimeToLong(System.DateTime.Now);
            //                            meterDataEntity.MeterID = entity.Programming[counter].MeterID;
            //                            meterDataEntity.ReadingDateTime = entity.Programming[counter].ReadingDateTime;
            //                            meterDataEntity.CMRIID = entity.Programming[counter].CMRIID;
            //                            meterDataEntity.CMRIType = entity.Programming[counter].CMRIType;
            //                            if (meterDataEntity.MeterID != null)
            //                            {
            //                                if (!new MeterDataBLL().ValidateData(fileUploadMasterEntity.FileUpload_ID, meterDataEntity.MeterID, meterDataEntity.ReadingDateTime))
            //                                    meterDataEntity = new MeterDataBLL().InsertData(meterDataEntity) as MeterDataEntity;
            //                                else
            //                                    meterDataEntity = new MeterDataBLL().GetDetailData(meterDataEntity.MeterID, fileUploadMasterEntity.FileUpload_ID, meterDataEntity.ReadingDateTime) as MeterDataEntity;
            //                            }
            //                        }
            //                    }
            //                   // this.StatusMessage = " Uploading Transaction Data";
            //                    fileUploadStatusBLL.UpdateData(requestID, FileUploadStatus.UploadingTransactionData);
            //                    Application.DoEvents();
            //                    List<IEntity> entities = new List<IEntity>();
            //                    for (int counter = 0; counter < entity.Programming.Count; counter++)
            //                    {
            //                        entity.Programming[counter].MeterData_ID = meterDataEntity.MeterData_ID;
            //                        entities.Add(entity.Programming[counter]);
            //                    }
            //                    if (entities.Count > 0 && !string.IsNullOrEmpty(meterDataEntity.MeterID))
            //                        new ProgrammingBLL().InsertData(entities);
            //                }
            //                //RTC Update
            //                if (entity.RTCUpdate != null)
            //                {
            //                    meterDataEntity = new MeterDataEntity();
            //                    if (entity.RTCUpdate.ReadingDateTime != 0)
            //                    {

            //                        meterDataEntity.FileUpload_ID = fileUploadMasterEntity.FileUpload_ID;
            //                        meterDataEntity.UploadingDateTime = DateUtility.DateTimeToLong(System.DateTime.Now);
            //                        meterDataEntity.MeterID = entity.RTCUpdate.MeterID;
            //                        meterDataEntity.ReadingDateTime = entity.RTCUpdate.ReadingDateTime;
            //                        meterDataEntity.CMRIID = entity.RTCUpdate.CMRIID;
            //                        meterDataEntity.CMRIType = entity.RTCUpdate.CMRIType;
            //                        if (meterDataEntity.MeterID != null)
            //                        {
            //                            if (!new MeterDataBLL().ValidateData(fileUploadMasterEntity.FileUpload_ID, meterDataEntity.MeterID, meterDataEntity.ReadingDateTime))
            //                                meterDataEntity = new MeterDataBLL().InsertData(meterDataEntity) as MeterDataEntity;
            //                            else
            //                                meterDataEntity = new MeterDataBLL().GetDetailData(meterDataEntity.MeterID, fileUploadMasterEntity.FileUpload_ID, meterDataEntity.ReadingDateTime) as MeterDataEntity;
            //                        }
            //                    }
            //                   // this.StatusMessage = " Uploading Transaction Data";
            //                    Application.DoEvents();
            //                    RTCUpdateEntity rTCUpdateEntity = entity.RTCUpdate as RTCUpdateEntity;
            //                    if (rTCUpdateEntity != null && !string.IsNullOrEmpty(meterDataEntity.MeterID))
            //                    {
            //                        rTCUpdateEntity.MeterData_ID = meterDataEntity.MeterData_ID;
            //                        rTCUpdateEntity = new RTCUpdateBLL().InsertData(rTCUpdateEntity) as RTCUpdateEntity;
            //                    }
            //                }
            //                //Phasor
            //                if (entity.Phasor != null)
            //                {
            //                    meterDataEntity = new MeterDataEntity();
            //                    if (entity.Phasor.ReadingDateTime != 0)
            //                    {
            //                        meterDataEntity.FileUpload_ID = fileUploadMasterEntity.FileUpload_ID;
            //                        meterDataEntity.UploadingDateTime = DateUtility.DateTimeToLong(System.DateTime.Now);
            //                        meterDataEntity.MeterID = entity.Phasor.MeterID;
            //                        meterDataEntity.ReadingDateTime = entity.Phasor.ReadingDateTime;
            //                        meterDataEntity.CMRIID = entity.Phasor.CMRIID;
            //                        meterDataEntity.CMRIType = entity.Phasor.CMRIType;
            //                        if (meterDataEntity.MeterID != null)
            //                        {
            //                            if (!new MeterDataBLL().ValidateData(fileUploadMasterEntity.FileUpload_ID, meterDataEntity.MeterID, meterDataEntity.ReadingDateTime))
            //                                meterDataEntity = new MeterDataBLL().InsertData(meterDataEntity) as MeterDataEntity;
            //                            else
            //                                meterDataEntity = new MeterDataBLL().GetDetailData(meterDataEntity.MeterID, fileUploadMasterEntity.FileUpload_ID, meterDataEntity.ReadingDateTime) as MeterDataEntity;
            //                        }
            //                    }
            //                   // this.StatusMessage = " Uploading Phasor Data";
            //                    fileUploadStatusBLL.UpdateData(requestID, FileUploadStatus.UploadingPhasorData);
            //                    Application.DoEvents();
            //                    PhasorEntity phasorEntity = entity.Phasor as PhasorEntity;
            //                    if (phasorEntity != null && !string.IsNullOrEmpty(meterDataEntity.MeterID))
            //                    {
            //                        phasorEntity.MeterData_ID = meterDataEntity.MeterData_ID;
            //                        phasorEntity = new PhasorBLL().InsertData(phasorEntity) as PhasorEntity;
            //                    }
            //                }
            //                //LoadSurvey
            //                if (entity.LoadSurvey != null)
            //                {
            //                    meterDataEntity = entity.LoadSurveyMeterData as MeterDataEntity;
            //                    if (meterDataEntity != null)
            //                    {
            //                        if (meterDataEntity.ReadingDateTime != 0)
            //                        {
            //                            meterDataEntity.FileUpload_ID = fileUploadMasterEntity.FileUpload_ID;
            //                            meterDataEntity.UploadingDateTime = DateUtility.DateTimeToLong(System.DateTime.Now);
            //                            if (meterDataEntity.MeterID != null)
            //                            {
            //                                if (!new MeterDataBLL().ValidateData(fileUploadMasterEntity.FileUpload_ID, meterDataEntity.MeterID, meterDataEntity.ReadingDateTime))
            //                                    meterDataEntity = new MeterDataBLL().InsertData(meterDataEntity) as MeterDataEntity;
            //                                else
            //                                    meterDataEntity = new MeterDataBLL().GetDetailData(meterDataEntity.MeterID, fileUploadMasterEntity.FileUpload_ID, meterDataEntity.ReadingDateTime) as MeterDataEntity;
            //                            }
            //                        }
            //                    }
            //                    //this.StatusMessage = " Uploading Load Survey Data";
            //                    fileUploadStatusBLL.UpdateData(requestID, FileUploadStatus.UploadingLoadSurveyData);
            //                    Application.DoEvents();
            //                    string parameters = null;
            //                    List<IEntity> entities = new List<IEntity>();
            //                    for (int counter = 0; counter < entity.LoadSurvey.Count; counter++)
            //                    {
            //                        entity.LoadSurvey[counter].MeterData_ID = meterDataEntity.MeterData_ID;
            //                        parameters = entity.LoadSurvey[counter].Parameters;
            //                        entities.Add(entity.LoadSurvey[counter]);
            //                    }
            //                    if (entities.Count > 0 && !string.IsNullOrEmpty(meterDataEntity.MeterID))
            //                    {
            //                        new LoadSurveyBLL().InsertData(entities);
            //                        LoadSurveyParameterBLL loadSurveyParameterBLL = new LoadSurveyParameterBLL();
            //                        LoadSurveyParameterEntity loadSurveyParameterEntity = new LoadSurveyParameterEntity();
            //                        loadSurveyParameterEntity.MeterDataId = meterDataEntity.MeterData_ID;
            //                        loadSurveyParameterEntity.ColumnsNames = parameters;
            //                        loadSurveyParameterBLL.InsertData(loadSurveyParameterEntity);
            //                    }
            //                }
            //                //Tamper
            //                if (entity.Tamper != null)
            //                {
            //                    TamperData tamper = entity.Tamper;
            //                    if (tamper != null)
            //                    {
            //                        meterDataEntity = new MeterDataEntity();
            //                        if (entity.Tamper.General != null)
            //                        {
            //                            if (entity.Tamper.General.ReadingDateTime != 0)
            //                            {
            //                                meterDataEntity.FileUpload_ID = fileUploadMasterEntity.FileUpload_ID;
            //                                meterDataEntity.UploadingDateTime = DateUtility.DateTimeToLong(System.DateTime.Now);
            //                                meterDataEntity.MeterID = entity.Tamper.General.MeterID;
            //                                meterDataEntity.ReadingDateTime = entity.Tamper.General.ReadingDateTime;
            //                                meterDataEntity.CMRIID = entity.Tamper.General.CMRIID;
            //                                meterDataEntity.CMRIType = entity.Tamper.General.CMRIType;
            //                                if (meterDataEntity.MeterID != null)
            //                                {
            //                                    if (!new MeterDataBLL().ValidateData(fileUploadMasterEntity.FileUpload_ID, meterDataEntity.MeterID, meterDataEntity.ReadingDateTime))
            //                                        meterDataEntity = new MeterDataBLL().InsertData(meterDataEntity) as MeterDataEntity;
            //                                    else
            //                                        meterDataEntity = new MeterDataBLL().GetDetailData(meterDataEntity.MeterID, fileUploadMasterEntity.FileUpload_ID, meterDataEntity.ReadingDateTime) as MeterDataEntity;
            //                                }
            //                            }
            //                        }
            //                        //this.StatusMessage = " Uploading Tamper Snapshot Data";
            //                        fileUploadStatusBLL.UpdateData(requestID, FileUploadStatus.UploadingTamperSnapShotData);
            //                        Application.DoEvents();
            //                        TamperCounterGeneralEntity tamperGeneral = tamper.General;
            //                        if (tamperGeneral != null)
            //                        {
            //                            tamperGeneral.MeterData_ID = meterDataEntity.MeterData_ID;
            //                            tamperGeneral = new TamperGeneralBLL().InsertData(tamperGeneral) as TamperCounterGeneralEntity;
            //                            TamperCounterEntity tamperCounter = tamper.Counter;
            //                            tamperCounter.MeterData_ID = meterDataEntity.MeterData_ID;
            //                            tamperCounter.TamperCounterGeneral_ID = tamperGeneral.TamperCounterGeneral_ID;
            //                            new TamperGeneralBLL().InsertData(tamperCounter);
            //                            List<IEntity> entities = new List<IEntity>();
            //                            for (int counter = 0; counter < tamper.Snapshot.Count; counter++)
            //                            {
            //                                tamper.Snapshot[counter].MeterData_ID = meterDataEntity.MeterData_ID;
            //                                entities.Add(tamper.Snapshot[counter]);
            //                            }
            //                            if (entities.Count > 0 && !string.IsNullOrEmpty(meterDataEntity.MeterID))
            //                                new TamperSnapShotBLL().InsertData(entities);
            //                        }
            //                    }
            //                }

            //                if (entity.DTMDailyProfile != null)
            //                {
            //                    meterDataEntity = entity.DTMDailyProfileMeterData as MeterDataEntity;
            //                    if (meterDataEntity != null)
            //                    {
            //                        if (meterDataEntity.ReadingDateTime != 0)
            //                        {
            //                            meterDataEntity.FileUpload_ID = fileUploadMasterEntity.FileUpload_ID;
            //                            meterDataEntity.UploadingDateTime = DateUtility.DateTimeToLong(System.DateTime.Now);
            //                            if (meterDataEntity.MeterID != null)
            //                            {
            //                                if (!new MeterDataBLL().ValidateData(fileUploadMasterEntity.FileUpload_ID, meterDataEntity.MeterID, meterDataEntity.ReadingDateTime))
            //                                    meterDataEntity = new MeterDataBLL().InsertData(meterDataEntity) as MeterDataEntity;
            //                                else
            //                                    meterDataEntity = new MeterDataBLL().GetDetailData(meterDataEntity.MeterID, fileUploadMasterEntity.FileUpload_ID, meterDataEntity.ReadingDateTime) as MeterDataEntity;
            //                            }
            //                        }
            //                    }
            //                    //this.StatusMessage = " Uploading daily Profile Data";
            //                    fileUploadStatusBLL.UpdateData(requestID, FileUploadStatus.UploadingDailyProfileData);
            //                    List<IEntity> entities = new List<IEntity>();
            //                    string dailyProfileParameter = string.Empty;
            //                    for (int counter = 0; counter < entity.DTMDailyProfile.Count; counter++)
            //                    {
            //                        entity.DTMDailyProfile[counter].MeterData_ID = meterDataEntity.MeterData_ID;
            //                        dailyProfileParameter = entity.DTMDailyProfile[counter].Parameters;
            //                        entities.Add(entity.DTMDailyProfile[counter]);
            //                    }
            //                    if (entities.Count > 0 && !string.IsNullOrEmpty(meterDataEntity.MeterID))
            //                    {
            //                        new DTMDailyProfileBLL().InsertData(entities);
            //                        DTMDailyProfileParameterEntity dtmDailyProfileParameterEntity = new DTMDailyProfileParameterEntity();
            //                        dtmDailyProfileParameterEntity.MeterDataId = meterDataEntity.MeterData_ID;
            //                        dtmDailyProfileParameterEntity.ColumnsNames = dailyProfileParameter;
            //                        new DTMDailyProfileParameterBLL().InsertData(dtmDailyProfileParameterEntity);
            //                    }
            //                }

            //                //Meter Data Header Info
            //                if (entity.HeaderInfo != null && !string.IsNullOrEmpty(meterDataEntity.MeterID))
            //                {
            //                    //this.StatusMessage = " Uploading Header Info";
            //                    fileUploadStatusBLL.UpdateData(requestID, FileUploadStatus.UploadingHeaderInfoData);
            //                    Application.DoEvents();
            //                    MeterDataHeaderInfoEntity meterDataHeaderInfoEntity = entity.HeaderInfo as MeterDataHeaderInfoEntity;
            //                    if (meterDataHeaderInfoEntity != null)
            //                    {
            //                        meterDataHeaderInfoEntity.MeterData_ID = meterDataEntity.MeterData_ID;
            //                        meterDataHeaderInfoEntity = new MeterDataHeaderInfoBLL().InsertData(meterDataHeaderInfoEntity) as MeterDataHeaderInfoEntity;
            //                    }
            //                }
            //                //Meter Data Name Plate Detail
            //                if (entity.NamePlateDetail != null && !string.IsNullOrEmpty(meterDataEntity.MeterID))
            //                {
            //                   // this.StatusMessage = " Uploading Name Plate Detail";
            //                    fileUploadStatusBLL.UpdateData(requestID, FileUploadStatus.UploadingNamePlateDetails);
            //                    Application.DoEvents();
            //                    NamePlateDetailEntity namePlateDetailEntity = entity.NamePlateDetail as NamePlateDetailEntity;
            //                    if (namePlateDetailEntity != null)
            //                    {
            //                        namePlateDetailEntity.MeterData_ID = meterDataEntity.MeterData_ID;
            //                        namePlateDetailEntity = new NamePlateDetailBLL().InsertData(namePlateDetailEntity) as NamePlateDetailEntity;
            //                    }
            //                }
            //            }
            //            catch (Exception ex)
            //            {
            //                //MessageBox.Show(ex.ToString());
            //            }
            //            #endregion
            //        }

            //        try
            //        {

            //            string meterID = string.Empty;
            //            # region Meter Configuration Data.
            //            if (fileContent.Contains("<MeterConfigurationData>"))
            //            {

            //                //this.StatusMessage = "Uploading Meter Configuration Data";
            //                fileUploadStatusBLL.UpdateData(requestID, FileUploadStatus.UploadingMeterConfigurations);
            //                Application.DoEvents();

            //                try
            //                {
            //                    int lengthOfRTCText = fileContent.IndexOf("</RTC>") - 5 - fileContent.IndexOf("<RTC>");
            //                    string rtcvalue = fileContent.Substring((fileContent.IndexOf("<RTC>") + 5), lengthOfRTCText);
            //                    meterID = rtcvalue.Substring(5, rtcvalue.IndexOf("\r") - 5);
            //                    new RTCBLL().InsertData(new FormatterConfigurations().ParseRTC(rtcvalue), meterID, fileUploadMasterEntity.FileUpload_ID);
            //                }
            //                catch (Exception e)
            //                {
            //                    //   this.StatusMessage = "Corrupt RTC Data.";
            //                    // Application.DoEvents();
            //                }
            //                try
            //                {
            //                    MDWithIPEntity mdWithIPEntity = new FormatterConfigurations().ParseMDWithIP(fileContent);
            //                    mdWithIPEntity.MeterID = meterID;
            //                    new MDWithIPBLL().InsertData(mdWithIPEntity, fileUploadMasterEntity.FileUpload_ID);
            //                }
            //                catch (Exception e)
            //                {
            //                    //  this.StatusMessage = "Corrupt MD With IP Data.";
            //                    //  Application.DoEvents();
            //                }

            //                try
            //                {
            //                    kvarSelectionEntity kvarSelectionEntity = new FormatterConfigurations().Parsekvarselection(fileContent);
            //                    kvarSelectionEntity.MeterID = meterID;
            //                    new kvarSelectionBLL().InsertData(kvarSelectionEntity, fileUploadMasterEntity.FileUpload_ID);
            //                }
            //                catch (Exception e)
            //                {
            //                    // this.StatusMessage = "Corrupt kVAR Selection Data.";
            //                    // Application.DoEvents();
            //                }

            //                #region Upload Display Parameters

            //                Collection<DisplayParamatersDBEntity> collDisplayParamatersDBEntity = new Collection<DisplayParamatersDBEntity>();
            //                DisplayParamatersDBEntity displayParamatersDBEntity;


            //                try
            //                {
            //                    if (fileContent.Contains("<Push>"))
            //                    {
            //                        int lengthOfdisplayParamaterText = fileContent.IndexOf("</Push>") - 6 - fileContent.IndexOf("<Push>");
            //                        string[] displayParamaters = fileContent.Substring((fileContent.IndexOf("<Push>") + 6), lengthOfdisplayParamaterText).Split('#');
            //                        for (int i = 0; i < displayParamaters.Length; i++)
            //                        {
            //                            displayParamatersDBEntity = new DisplayParamatersDBEntity();
            //                            displayParamatersDBEntity.displayParamaterType = DisplayParameter.PushMode;
            //                            displayParamatersDBEntity.paramaterName = displayParamaters[i];
            //                            displayParamatersDBEntity.paramaterValue = 1;
            //                            collDisplayParamatersDBEntity.Add(displayParamatersDBEntity);
            //                        }

            //                        lengthOfdisplayParamaterText = fileContent.IndexOf("</Scroll>") - 8 - fileContent.IndexOf("<Scroll>");
            //                        displayParamaters = fileContent.Substring((fileContent.IndexOf("<Scroll>") + 8), lengthOfdisplayParamaterText).Split('#');
            //                        for (int i = 0; i < displayParamaters.Length; i++)
            //                        {
            //                            displayParamatersDBEntity = new DisplayParamatersDBEntity();
            //                            displayParamatersDBEntity.displayParamaterType = DisplayParameter.ScrollMode;
            //                            displayParamatersDBEntity.paramaterName = displayParamaters[i];
            //                            displayParamatersDBEntity.paramaterValue = 1;
            //                            collDisplayParamatersDBEntity.Add(displayParamatersDBEntity);
            //                        }

            //                        lengthOfdisplayParamaterText = fileContent.IndexOf("</HighResolution>") - 16 - fileContent.IndexOf("<HighResolution>");
            //                        displayParamaters = fileContent.Substring((fileContent.IndexOf("<HighResolution>") + 16), lengthOfdisplayParamaterText).Split('#');
            //                        for (int i = 0; i < displayParamaters.Length; i++)
            //                        {
            //                            displayParamatersDBEntity = new DisplayParamatersDBEntity();
            //                            displayParamatersDBEntity.displayParamaterType = DisplayParameter.HighResolutionMode;
            //                            displayParamatersDBEntity.paramaterName = displayParamaters[i];
            //                            displayParamatersDBEntity.paramaterValue = 1;
            //                            collDisplayParamatersDBEntity.Add(displayParamatersDBEntity);
            //                        }
            //                        //DisplayTimeOuts

            //                        lengthOfdisplayParamaterText = fileContent.IndexOf("</DisplayTimeOuts>") - 17 - fileContent.IndexOf("<DisplayTimeOuts>");
            //                        string displayTimeOutText = fileContent.Substring(fileContent.IndexOf("<DisplayTimeOuts>") + 17, lengthOfdisplayParamaterText);

            //                        int tmp = Convert.ToInt32(displayTimeOutText.Substring(2, 4), 16);
            //                        //Fill DTO to write in Db.
            //                        displayParamatersDBEntity = new DisplayParamatersDBEntity();
            //                        displayParamatersDBEntity.displayParamaterType = DisplayParameter.DisplayTimeouts;
            //                        displayParamatersDBEntity.paramaterName = "Scroll Time Out";
            //                        displayParamatersDBEntity.paramaterValue = Convert.ToInt32(tmp);
            //                        collDisplayParamatersDBEntity.Add(displayParamatersDBEntity);

            //                        tmp = Convert.ToInt32(displayTimeOutText.Substring(6, 4), 16);
            //                        //Fill DTO to write in Db.
            //                        displayParamatersDBEntity = new DisplayParamatersDBEntity();
            //                        displayParamatersDBEntity.displayParamaterType = DisplayParameter.DisplayTimeouts;
            //                        displayParamatersDBEntity.paramaterName = "Push Time Out";
            //                        displayParamatersDBEntity.paramaterValue = Convert.ToInt32(tmp);
            //                        collDisplayParamatersDBEntity.Add(displayParamatersDBEntity);

            //                        tmp = Convert.ToInt32(displayTimeOutText.Substring(10, 2), 16);
            //                        if (tmp != 0)
            //                        {
            //                            tmp = Convert.ToInt32(displayTimeOutText.Substring(12, 4), 16);
            //                            //Fill DTO to write in Db.
            //                            displayParamatersDBEntity = new DisplayParamatersDBEntity();
            //                            displayParamatersDBEntity.displayParamaterType = DisplayParameter.DisplayTimeouts;
            //                            displayParamatersDBEntity.paramaterName = "Auto Scroll Resume Time";
            //                            displayParamatersDBEntity.paramaterValue = Convert.ToInt32(tmp);
            //                            collDisplayParamatersDBEntity.Add(displayParamatersDBEntity);
            //                        }
            //                        new DisplayParametersBLL().InsertData(collDisplayParamatersDBEntity, meterID, fileUploadMasterEntity.FileUpload_ID);
            //                    }
            //                }
            //                catch (Exception e)
            //                {
            //                    //   this.StatusMessage = "Corrupt Display Parameters Data.";
            //                    //   Application.DoEvents();
            //                }
            //                #endregion


            //                try
            //                {
            //                    string todData = new FormatterConfigurations().ParseTODData(fileContent);
            //                    new TODBLL().InsertData(todData, meterID, fileUploadMasterEntity.FileUpload_ID);
            //                }
            //                catch (Exception e)
            //                {
            //                    //  this.StatusMessage = "Corrupt TOD Data.";
            //                    // Application.DoEvents();
            //                }

            //               // this.StatusMessage = "Meter Configuration Data Uploaded successfully";
            //                Application.DoEvents();
            //            }
            //            #endregion

            //            MeterDataEntity mtrDataEntity = new MeterDataEntity();
            //            if (ifmeterEntityUploaded == false && mtrDataEntity != null && meterID.Length > 0)
            //            {
            //                mtrDataEntity.FileUpload_ID = fileUploadMasterEntity.FileUpload_ID;
            //                mtrDataEntity.UploadingDateTime = DateUtility.DateTimeToLong(System.DateTime.Now);
            //                if (meterID != null)
            //                {
            //                    if (!new MeterDataBLL().ValidateData(fileUploadMasterEntity.FileUpload_ID, meterID, mtrDataEntity.ReadingDateTime))
            //                    {
            //                        mtrDataEntity.MeterID = meterID;
            //                        if (fileContent.Contains("<ReadingDateTime>"))
            //                        {
            //                            string readingDateTime = fileContent.Substring(fileContent.IndexOf("<ReadingDateTime>") + 17, fileContent.IndexOf("</ReadingDateTime>") - fileContent.IndexOf("<ReadingDateTime>") - 17);
            //                            mtrDataEntity.ReadingDateTime = Convert.ToInt64(readingDateTime);
            //                        }

            //                        mtrDataEntity = new MeterDataBLL().InsertData(mtrDataEntity) as MeterDataEntity;
            //                    }
            //                    else
            //                        mtrDataEntity = new MeterDataBLL().GetDetailData(meterID, fileUploadMasterEntity.FileUpload_ID, mtrDataEntity.ReadingDateTime) as MeterDataEntity;
            //                }
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //          //  MessageBox.Show(ex.ToString());
            //        }
            //        //MessageBox.Show("File Uploaded Successfully.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    }
            //    // this.StatusMessage = "";
            //    // Application.DoEvents();
            //    return isUploaded;
            //}
            //catch (Exception)
            //{
            //    return false;
            //}
        }
       
        public  string GetContent(string filePath)
        {
            StreamReader streamReader = new StreamReader(filePath);
            string fileContent = streamReader.ReadToEnd();
            streamReader.Close();
            if (ConfigInfo.IsEncryption())
            {
                fileContent = ConfigInfo.DecryptFile(fileContent);
            }
            return fileContent;
        }
        public  byte[] TotalBytes(string fileName)
        {
            FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            long size = stream.Length;
            byte[] data = new byte[size];
            stream.Read(data, 0, (int)size);
            stream.Close();
            return data;
        }
    }
}
