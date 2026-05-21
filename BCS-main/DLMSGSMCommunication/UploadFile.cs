using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CAB.BLL;
using CAB.Channel.Formatter;
using CAB.Entity;
using CAB.Framework.Entity;
using CAB.Framework.Utility;
using Utilities;

namespace DLMSGSMCommunication
{
   public  class UploadFile
    {
        MeterDataBLL meterDataBLL = null;
        string meterType = string.Empty;
        private string comPortName;
        public string ComPortName
        {
            get
            {
                return comPortName;
            }
            set
            {
                comPortName = value;
            }
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

        /// <summary>
        /// This method is used for getting total bytes of the file.
        /// </summary>
        /// <param name="fileName">Pass the file name.</param>
        /// <returns>Returns byte array.</returns>
        private byte[] TotalBytes(string fileName)
        {
            FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            long size = stream.Length;
            byte[] data = new byte[size];
            stream.Read(data, 0, (int)size);
            stream.Close();
            return data;
        }

        /// <summary>
        /// This method is used for saving file content into database.
        /// </summary>
        /// <param name="fileName">Pass the file name.</param>
        /// <returns>A boolean value true/false.</returns>
        public bool SaveMeterData(string fileName)
        {
            return Upload(fileName, GetContent(fileName), true);
        }

       /// <summary>
        /// Overload of SaveMeterData(string fileName) 
       /// </summary>
       /// <param name="fileName"></param>
       /// <param name="flag"></param>
       /// <returns></returns>
        public bool SaveMeterData(string fileName, bool flag)
        {
            return Upload(fileName, GetContent(fileName), flag);
        }

        /// <summary>
        /// This method is used for saving file content into database.
        /// </summary>
        /// <param name="fileName">Pass the file name.</param>
        /// <param name="fileText">Pass the file text.</param>
        /// <param name="flag">A boolean value</param>
        /// <returns>A boolean value true/false.</returns>
        private bool Upload(string fileName, string fileText, bool flag)
        {
            FileUploadMasterBLL fileUploadMasterBLL = new FileUploadMasterBLL();
            FileUploadMasterEntity fileUploadMasterEntity = new FileUploadMasterEntity();
            meterDataBLL = new MeterDataBLL();

            try
            {
                bool isUploaded = false;
                if (!isUploaded)
                {
                    if (string.IsNullOrEmpty(fileText))
                    {
                        EventLogging.CallLogDetails("File corrupt.");
                        return isUploaded;
                    }

                    DLMS650FormatterBilling formatterBilling = new DLMS650FormatterBilling();
                    BillingGeneralNFDLMSEntity master = new BillingGeneralNFDLMSEntity();
                    fileUploadMasterEntity.UserInformation_ID = ConfigInfo.UserInformationID;

                    if (flag == true)
                    {
                        fileUploadMasterEntity.FileContent = TotalBytes(fileName);
                        fileUploadMasterEntity.FileName = Path.GetFileName(fileName);
                    }
                    else
                    {
                        fileUploadMasterEntity.FileContent = ASCIIEncoding.UTF8.GetBytes(ConfigInfo.EncryptFile(fileText));
                        fileUploadMasterEntity.FileName = Path.GetFileName(fileName);
                    }
                    fileUploadMasterEntity.UploadingDateTime = DateUtility.DateTimeToLong(DateTime.Now);

                    if (flag == true)
                    {
                        FileUploadMasterEntity fileEntity = fileUploadMasterBLL.ValidateFile(fileUploadMasterEntity.FileName) as FileUploadMasterEntity;
                        if (fileEntity != null)
                        {
                            if (fileEntity.FileUpload_ID != 0)
                            {
                                EventLogging.CallLogDetails("File '" + fileEntity.FileName + "' already exist.");
                                return isUploaded;
                            }
                        }
                        fileUploadMasterEntity = fileUploadMasterBLL.InsertData(fileUploadMasterEntity) as FileUploadMasterEntity;
                        if (fileUploadMasterEntity.FileUpload_ID == 0)
                        {
                            EventLogging.CallLogDetails("Please Contact system administrator. Invalid DB Structure.");
                            return false;
                        }
                    }
                    else
                    {
                        FileUploadMasterEntity fileEntity = fileUploadMasterBLL.ValidateFile(fileUploadMasterEntity.FileName) as FileUploadMasterEntity;
                        if (fileEntity.FileUpload_ID == 0)
                            fileUploadMasterEntity = fileUploadMasterBLL.InsertData(fileUploadMasterEntity) as FileUploadMasterEntity;
                        else
                            fileUploadMasterEntity.FileUpload_ID = fileEntity.FileUpload_ID;
                    }

                    isUploaded = true;
                    formatterBilling.GetData(fileText, master);

                    int cdx = 1;
                    if (master.Billing == null)
                        cdx++;
                    if (master.General == null)
                        cdx++;
                    if (master.LoadSurvey == null)
                        cdx++;
                    if (master.Tamper == null)
                        cdx++;
                    if (master.Instant == null)
                        cdx++;
                    if (cdx > 5)
                    {
                        fileUploadMasterBLL.DeleteData(fileUploadMasterEntity);
                        EventLogging.CallLogDetails("No data in the selected file.");
                        return false;
                    }
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
                                    meterDataEntity = new MeterDataBLL().InsertData(meterDataEntity) as MeterDataEntity;
                                else
                                    meterDataEntity = meterDataBLL.GetDetailData(meterDataEntity.MeterID, fileUploadMasterEntity.FileUpload_ID, meterDataEntity.ReadingDateTime) as MeterDataEntity;
                            }
                        }
                        if (master.General != null && !string.IsNullOrEmpty(meterDataEntity.MeterID))
                        {
                            DLMS650NamePlateDetailsEntity general = master.General as DLMS650NamePlateDetailsEntity;
                            //EventLogging.CallLogDetails("Uploading General Data");
                            general.MeterData_ID = meterDataEntity.MeterData_ID;
                            new DLMS650GeneralBLL().InsertData(general);
                        }
                        if (master.Instant != null && !string.IsNullOrEmpty(meterDataEntity.MeterID))
                        {
                            //EventLogging.CallLogDetails("Uploading Instant Data");
                            DLMS650InstantaneousBLL instantBLL = new DLMS650InstantaneousBLL();
                            List<IEntity> entities = new List<IEntity>();
                            for (int counter = 0; counter < master.Instant.Count; counter++)
                            {
                                master.Instant[counter].MeterDataID = meterDataEntity.MeterData_ID;
                                entities.Add(master.Instant[counter]);
                            }
                            instantBLL.InsertData(entities);
                        }
                        //BhardwajG : Code added for Diagnosis/Anomaly
                        if (master.Anomaly != null && !string.IsNullOrEmpty(meterDataEntity.MeterID))
                        {
                            AnomalyBLL objAnomalyBLL = new AnomalyBLL();
                            AnomalyEntity entity = master.Anomaly;
                            entity.MeterDataId = meterDataEntity.MeterData_ID;
                            objAnomalyBLL.InsertData(entity);
                        }
                        if (master.LoadSurvey != null && !string.IsNullOrEmpty(meterDataEntity.MeterID))
                        {
                            //EventLogging.CallLogDetails("Uploading LoadSurvey Data");
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
                        if (master.Billing != null && !string.IsNullOrEmpty(meterDataEntity.MeterID))
                        {
                            //EventLogging.CallLogDetails("Uploading Billing Data");
                            DLMS650BillingBLL billingBLL = new DLMS650BillingBLL();
                            List<IEntity> entities = new List<IEntity>();
                            for (int counter = 0; counter < master.Billing.Count; counter++)
                            {
                                master.Billing[counter].MeterData_ID = meterDataEntity.MeterData_ID;
                                entities.Add(master.Billing[counter]);
                            }
                            billingBLL.InsertData(entities);
                        }
                        if (master.Tamper != null && !string.IsNullOrEmpty(meterDataEntity.MeterID))
                        {
                            //EventLogging.CallLogDetails("Uploading Tamper Data");
                            DLMS650TamperMasterBLL tamperBLL = new DLMS650TamperMasterBLL();
                            List<IEntity> entities = new List<IEntity>();
                            for (int counter = 0; counter < master.Tamper.Count; counter++)
                            {
                                master.Tamper[counter].MeterData_ID = meterDataEntity.MeterData_ID;
                                entities.Add(master.Tamper[counter]);
                            }
                            tamperBLL.InsertData(entities);
                        }
                        //BhardwajG : insert tou if available
                        if (master.TOU != null && !string.IsNullOrEmpty(meterDataEntity.MeterID))
                        {
                            TOUBLL objTOUBLL = new TOUBLL();
                            //TOUEntity entity = master.TOU;
                            //entity.MeterDataId = meterDataEntity.MeterData_ID;
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
                        EventLogging.CallLogDetails(ex.Message.ToString());
                        isUploaded = false;
                    }
                }
                return isUploaded;
            }
            catch (Exception ex)
            {
                fileUploadMasterBLL.DeleteData(fileUploadMasterEntity);
                EventLogging.CallLogDetails("File corrupted." + ex.Message.ToString());
                return false;
            }
        }
    }
}