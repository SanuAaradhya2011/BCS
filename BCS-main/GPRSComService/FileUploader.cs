using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.BLL;
using CAB.Entity;
using CAB.Channel.Formatter;
using System.IO;
using CAB.Framework.Utility;
using CAB.Framework.Entity;
using System.Data;

namespace GPRSComService
{
    class FileUploader
    {
        /// <summary>
        /// This method is used for saving file content into database.
        /// </summary>
        /// <param name="fileName">Pass the file name.</param>
        /// <param name="fileText">Pass the file text.</param>
        /// <param name="flag">A boolean value</param>
        /// <returns>A boolean value true/false.</returns>
        public bool UploadGPRSFile(string fileName)
        {

            FileUploadMasterBLL fileUploadMasterBLL = new FileUploadMasterBLL();
            FileUploadMasterEntity fileUploadMasterEntity = new FileUploadMasterEntity();
            MeterDataBLL meterDataBLL = new MeterDataBLL();

            try
            {
                bool isUploaded = false;
                string fileText = GetContent(fileName);
                if (string.IsNullOrEmpty(fileText))
                {
                    return isUploaded;
                }

                DLMS650FormatterBilling formatterBilling = new DLMS650FormatterBilling();
                BillingGeneralNFDLMSEntity master = new BillingGeneralNFDLMSEntity();
                fileUploadMasterEntity.UserInformation_ID = ConfigInfo.UserInformationID;
                fileUploadMasterEntity.FileContent = ASCIIEncoding.UTF8.GetBytes(ConfigInfo.EncryptFile(fileText));
                fileUploadMasterEntity.FileName = Path.GetFileName(fileName);
                fileUploadMasterEntity.UploadingDateTime = DateUtility.DateTimeToLong(DateTime.Now);

                FileUploadMasterEntity fileEntity = fileUploadMasterBLL.ValidateFile(fileUploadMasterEntity.FileName) as FileUploadMasterEntity;
                if (fileEntity.FileUpload_ID == 0)
                {
                    fileUploadMasterEntity = fileUploadMasterBLL.InsertData(fileUploadMasterEntity) as FileUploadMasterEntity;
                }
                else
                {
                    fileUploadMasterEntity.FileUpload_ID = fileEntity.FileUpload_ID;
                }

                isUploaded = true;
                formatterBilling.GetData(fileText, master);

                try
                {
                    MeterDataEntity meterDataEntity = master.MeterData as MeterDataEntity;
                    meterDataEntity.FileUpload_ID = fileUploadMasterEntity.FileUpload_ID;
                    if (meterDataEntity != null)
                    {
                        meterDataEntity.FileUpload_ID = fileUploadMasterEntity.FileUpload_ID;
                        meterDataEntity.UploadingDateTime = DateUtility.DateTimeToLong(System.DateTime.Now);
                        if (meterDataEntity.MeterID != null)
                        {
                            if (!meterDataBLL.ValidateData(fileUploadMasterEntity.FileUpload_ID, meterDataEntity.MeterID, meterDataEntity.ReadingDateTime))
                            {
                                meterDataEntity = new MeterDataBLL().InsertData(meterDataEntity) as MeterDataEntity;
                            }
                            else
                            {
                                meterDataEntity = meterDataBLL.GetDetailData(meterDataEntity.MeterID, fileUploadMasterEntity.FileUpload_ID, meterDataEntity.ReadingDateTime) as MeterDataEntity;
                            }
                        }
                    }

                    FileUploadStatus fileStatus = GetFileUploadStatus(meterDataEntity.MeterData_ID);

                    if (master.General != null && !string.IsNullOrEmpty(meterDataEntity.MeterID) && !fileStatus.isGeneralUploaded)
                    {
                        DLMS650NamePlateDetailsEntity general = master.General as DLMS650NamePlateDetailsEntity;
                        general.MeterData_ID = meterDataEntity.MeterData_ID;
                        new DLMS650GeneralBLL().InsertData(general);
                    }
                    if (master.Instant != null && !string.IsNullOrEmpty(meterDataEntity.MeterID) && !fileStatus.isInstantUploaded)
                    {
                        DLMS650InstantaneousBLL instantBLL = new DLMS650InstantaneousBLL();
                        List<IEntity> entities = new List<IEntity>();
                        for (int counter = 0; counter < master.Instant.Count; counter++)
                        {
                            master.Instant[counter].MeterDataID = meterDataEntity.MeterData_ID;
                            entities.Add(master.Instant[counter]);
                        }
                        instantBLL.InsertData(entities);
                    }
                    if (master.Anomaly != null && !string.IsNullOrEmpty(meterDataEntity.MeterID))
                    {
                        AnomalyBLL objAnomalyBLL = new AnomalyBLL();
                        AnomalyEntity entity = master.Anomaly;
                        entity.MeterDataId = meterDataEntity.MeterData_ID;
                        objAnomalyBLL.InsertData(entity);
                    }
                    if (master.LoadSurvey != null && !string.IsNullOrEmpty(meterDataEntity.MeterID) && !fileStatus.isLoadSurveyUploaded)
                    {
                        DLMS650LoadSurveyBLL loadSurveyBLL = new DLMS650LoadSurveyBLL();
                        LoadSurveyParameterBLL loadSurveyParameterBLL = new LoadSurveyParameterBLL();
                        List<IEntity> entities = new List<IEntity>();

                        for (int counter = 0; counter < master.LoadSurvey.Count; counter++)
                        {
                            master.LoadSurvey[counter].MeterData_ID = meterDataEntity.MeterData_ID;
                            entities.Add(master.LoadSurvey[counter]);
                        }

                        loadSurveyBLL.InsertData(entities);
                        master.LSParameterColumns.MeterDataId = meterDataEntity.MeterData_ID;
                        loadSurveyParameterBLL.InsertData(master.LSParameterColumns);
                    }
                    if (master.Billing != null && !string.IsNullOrEmpty(meterDataEntity.MeterID) && !fileStatus.isBillingUploaded)
                    {
                        DLMS650BillingBLL billingBLL = new DLMS650BillingBLL();
                        List<IEntity> entities = new List<IEntity>();
                        for (int counter = 0; counter < master.Billing.Count; counter++)
                        {
                            master.Billing[counter].MeterData_ID = meterDataEntity.MeterData_ID;
                            entities.Add(master.Billing[counter]);
                        }
                        billingBLL.InsertData(entities);
                    }
                    if (master.Tamper != null && !string.IsNullOrEmpty(meterDataEntity.MeterID) && !fileStatus.isTamperUploaded)
                    {
                        DLMS650TamperMasterBLL tamperBLL = new DLMS650TamperMasterBLL();
                        List<IEntity> entities = new List<IEntity>();
                        for (int counter = 0; counter < master.Tamper.Count; counter++)
                        {
                            master.Tamper[counter].MeterData_ID = meterDataEntity.MeterData_ID;
                            entities.Add(master.Tamper[counter]);
                        }
                        tamperBLL.InsertData(entities);
                    }
                    if (master.TOU != null && !string.IsNullOrEmpty(meterDataEntity.MeterID))
                    {
                        TOUBLL objTOUBLL = new TOUBLL();
                        List<IEntity> entities = new List<IEntity>();
                        foreach (TOU tou in master.TOU)
                        {
                            tou.MeterData_ID = meterDataEntity.MeterData_ID;
                            entities.Add(tou);
                        }
                        objTOUBLL.InsertData(entities);
                    }
                }
                catch (Exception ex)
                {
                    isUploaded = false;
                }

                return isUploaded;
            }
            catch (Exception ex)
            {
                fileUploadMasterBLL.DeleteData(fileUploadMasterEntity);
                return false;
            }
        }


        private FileUploadStatus GetFileUploadStatus(long meterDataId)
        {
            MeterDataBLL meterDataBLL = new MeterDataBLL();
            DataSet ds = meterDataBLL.GetFileUploadedStatus((int)meterDataId);
            FileUploadStatus status = new FileUploadStatus();
            if (ds != null && ds.Tables != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                status.isGeneralUploaded = Convert.ToBoolean(ds.Tables[0].Rows[0]["GENERAL"]);
                status.isInstantUploaded = Convert.ToBoolean(ds.Tables[0].Rows[0]["INSTANT"]);
                status.isBillingUploaded = Convert.ToBoolean(ds.Tables[0].Rows[0]["BILLING"]);
            }
            return status;
        }

        /// <summary>
        /// This method is used for getting content of the file.
        /// </summary>
        /// <param name="filePath">Pass path of the file.</param>
        /// <returns>Returns string content of the file.</returns>
        private string GetContent(string filePath)
        {
            StreamReader streamReader = new StreamReader(filePath);
            string fileContent = streamReader.ReadToEnd();
            streamReader.Close();
            return fileContent;
        }
    }
}
