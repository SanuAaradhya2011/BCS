
#region Namespaces
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Common.EntityMapper;
using Hunt.EPIC.Logging;
using CAB.BLL;
using CAB.Channel.Formatter;
using CAB.Entity;
using CAB.EntityGenerator;
using CAB.Framework.Entity;
using CAB.Framework.Utility;
using CAB.IECChannel.Formatter;
using CAB.Parser;
using CABFramework;
using LTCTBLL;
using Utilities;
using CAB.Parser.Entity;


#endregion
namespace CAB.MeterData.Upload
{
    /// <summary>
    /// Uploads the .2ng and .lng file in database
    /// </summary>
    public class UploadFile
    {
        #region Nested Types
        #endregion

        #region Constants and Variables

        private MeterDataBLL meterDataBLL = null;
        private string meterType = string.Empty;
        private string comPortName;
        private TabNameBLL tabNameBll = null;
        private GenerateEntity entityGenerator = null;
        private Dictionary<ReadOutItem, Collection<ReadOutCounterEntity>> readOuts = null;
        private List<long> meterDataIds = null;
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(UploadFile).ToString());
        #endregion

        #region Properties
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
        #endregion

        #region Constructor
        public UploadFile()
        {

            tabNameBll = new TabNameBLL();
            entityGenerator = new GenerateEntity();
            meterDataBLL = new MeterDataBLL();
            readOuts = new Dictionary<ReadOutItem, Collection<ReadOutCounterEntity>>();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Upload CAB  file into the db based on readouts.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileText"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        /// 
        double pf_r, pf_y, pf_b, pf_tot, pf_tot1; // add pradipta
        double pf_r_sin, pf_y_sin, pf_b_sin, pf_tot_sin, pf_tot_sin1, Neu_curr, Neu_curr1; // add pradipta

        double pf_r_cos, pf_y_cos, pf_b_cos;

        string _pf_r_ph, _pf_y_ph, _pf_b_ph;
        string _i_r_ph, _i_y_ph, _i_b_ph, _pf_r, _pf_y, _pf_b;
        string _i_r, _i_y, _i_b;

        double tot_pf_rph, tot_pf_yph, tot_pf_bph;


        public bool UploadCABFile(string fileName, string fileText, bool flag, out string message, string cmriID)
        {
            message = string.Empty;
            bool isUploaded = false;
            if (!string.IsNullOrEmpty(fileText))
            {

                List<string> meterIDsUploaded = new List<string>();
                FileUploadMasterBLL fileUploadMasterBLL = new FileUploadMasterBLL();
                FileUploadMasterEntity fileUploadMasterEntity = new FileUploadMasterEntity();
                try
                {
                    //this.Cursor = Cursors.WaitCursor;

                    //this.StatusMessage = resourceMgr.GetString("Uploading") + resourceMgr.GetString("Filetxt") + Path.GetFileName(fileName);
                    //Application.DoEvents();
                    if (!isUploaded)
                    {
                        if (!FormatterCommon.IsFileNullOrEmpty(fileText))
                        {
                            message = "File Corrupted.";
                            //Application.DoEvents();
                            return isUploaded;
                        }
                        if (!ConfigInfo.IsValidCheckSum(fileText))
                        {
                            message = "BCC Mismatch."; //"BCC mismatched.";
                            //Application.DoEvents();
                            return isUploaded;
                        }

                        fileUploadMasterEntity = new FileUploadMasterEntity();
                        fileUploadMasterEntity.UserInformation_ID = ConfigInfo.UserInformationID;
                        if (flag)
                        {
                            fileUploadMasterEntity.FileContent = TotalBytes(fileName);
                            fileUploadMasterEntity.FileName = Path.GetFileName(fileName);
                            fileUploadMasterEntity.FileSize = GetFileSize(ConfigInfo.EncryptFile(fileText));
                            //for aumatic CMRI association
                            fileUploadMasterEntity.CMRIID = cmriID;

                        }
                        else
                        {
                            fileUploadMasterEntity.FileContent = ASCIIEncoding.UTF8.GetBytes(ConfigInfo.EncryptFile(fileText));
                            fileUploadMasterEntity.FileName = Path.GetFileName(fileName);
                            fileUploadMasterEntity.FileSize = GetFileSize(ConfigInfo.EncryptFile(fileText));
                            //for aumatic CMRI association
                            fileUploadMasterEntity.CMRIID = cmriID;
                        }
                        fileUploadMasterEntity.UploadingDateTime = DateUtility.DateTimeToLong(DateTime.Now);
                        fileUploadMasterEntity.FileType = "NONDLMS";
                        fileUploadMasterEntity.CommType = GetCommType();
                        if (flag)
                        {
                            FileUploadMasterEntity fileEntity = fileUploadMasterBLL.ValidateFile(fileUploadMasterEntity.FileName) as FileUploadMasterEntity;
                            if (fileEntity != null)
                            {
                                if (fileEntity.FileUpload_ID != 0)
                                {


                                    message = "Deleting '" + fileUploadMasterEntity.FileName + "'...";
                                    CommonBLL commonBll = new CommonBLL();
                                    commonBll.DeleteArchieveOperation(fileEntity);
                                }
                            }
                            fileUploadMasterEntity = fileUploadMasterBLL.InsertData(fileUploadMasterEntity) as FileUploadMasterEntity;
                            if (fileUploadMasterEntity.FileUpload_ID == 0)
                            {
                                message = "Please Contact system administrator. Invalid DB Structure.";
                                //Application.DoEvents();
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
                        MeterDataEntity mtrDataEntity = new MeterDataEntity();
                        try
                        {
                            mtrDataEntity.FileUpload_ID = fileUploadMasterEntity.FileUpload_ID;
                            mtrDataEntity.UploadingDateTime = DateUtility.DateTimeToLong(System.DateTime.Now);


                            fileText = fileText.Replace(FormatterConstant.ENTERCARRAGE, string.Empty);
                            fileText = fileText.Replace(FormatterConstant.NEWLINE, string.Empty);
                            //Parse Readouts in file and create a mapping table with meter data id.
                            ParseReadOuts(fileText, mtrDataEntity, fileUploadMasterEntity, cmriID);
                        }
                        catch (Exception ex)    //SarkarA //Exception log for catch block
                        {
                            logger.Log(LOGLEVELS.Error, "UploadCABFile(string fileName, string fileText, bool flag, out string message, string cmriID)", ex);
                            // WriteintoCSVFile(fileUploadMasterEntity, "2");
                        }

                        List<IECBillingGeneralNFEntity> master = new List<IECBillingGeneralNFEntity>();
                        FormatterBilling formatterBilling = new FormatterBilling();
                        formatterBilling.GetData(fileText, master, readOuts);
                        if (master.Count > 0)
                        {
                            List<IECBillingGeneralNFEntity> IECMasterEntity = GetMasterEntity(master[0]);

                            foreach (IECBillingGeneralNFEntity iecMasterEntity in IECMasterEntity)
                            {

                                try
                                {
                                    BillingGeneralNFDLMSEntity masterEntity = new IECToDLMSMapper(false).ConvertIECEntityToDLMSEntity(iecMasterEntity, true);

                                    #region General
                                    if (masterEntity.General != null && !string.IsNullOrEmpty(mtrDataEntity.MeterID))
                                    {
                                        DLMS650NamePlateDetailsEntity general = masterEntity.General;
                                        general.MeterData_ID = masterEntity.MeterDataID;
                                        new DLMS650GeneralBLL().InsertData(general);
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "Gen", true, "General");
                                    }
                                    else
                                    {
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "Gen", false, "General");
                                    }
                                    #endregion

                                    #region Instant
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "InsABC", false, "Instant : ABC Code");
                                    if (masterEntity.Instant != null && !string.IsNullOrEmpty(mtrDataEntity.MeterID))
                                    {
                                        //this.StatusMessage = "Please wait. Uploading Instant Data";
                                        //Application.DoEvents();
                                        DLMS650InstantaneousBLL instantBLL = new DLMS650InstantaneousBLL();
                                        List<IEntity> entities = new List<IEntity>();
                                        for (int counter = 0; counter < masterEntity.Instant.Count; counter++)
                                        {
                                            masterEntity.Instant[counter].MeterDataID = masterEntity.MeterDataID; ;
                                            entities.Add(masterEntity.Instant[counter]);
                                        }
                                        instantBLL.InsertData(entities);
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "InsRea", true, "Instant : Reading");
                                        
                                    }
                                    else
                                    {
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "InsRea", false, "Instant : Reading");
                                        
                                    }
                                    #endregion

                                    #region Anomaly
                                    if (masterEntity.Anomaly != null && !string.IsNullOrEmpty(mtrDataEntity.MeterID))
                                    {
                                        masterEntity.Anomaly.MeterDataId = masterEntity.MeterDataID; ;
                                        new AnomalyBLL().InsertData(masterEntity.Anomaly);
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "InsSel", true, "Instant : Self Diagnostics");
                                    }
                                    else
                                    {
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "InsSel", false, "Instant : Self Diagnostics");
                                    }
                                    #endregion

                                    #region LoadSurvey
                                    if (masterEntity.LoadSurvey != null && !string.IsNullOrEmpty(mtrDataEntity.MeterID) && masterEntity.LoadSurvey.Count > 0)
                                    {
                                        //this.StatusMessage = "Please wait. Uploading LoadSurvey Data";
                                        //Application.DoEvents();
                                        DLMS650LoadSurveyBLL loadSurveyBLL = new DLMS650LoadSurveyBLL();
                                        LoadSurveyParameterBLL loadSurveyParameterBLL = new LoadSurveyParameterBLL();
                                        List<IEntity> entities = new List<IEntity>();

                                        for (int counter = 0; counter < masterEntity.LoadSurvey.Count; counter++)
                                        {
                                            masterEntity.LoadSurvey[counter].MeterData_ID = masterEntity.MeterDataID; ;
                                            entities.Add(masterEntity.LoadSurvey[counter]);
                                        }

                                        loadSurveyBLL.InsertData(entities);
                                        masterEntity.LSParameterColumns.MeterDataId = masterEntity.MeterDataID;
                                        loadSurveyParameterBLL.InsertData(masterEntity.LSParameterColumns);
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "LoaSur", true, "Load Survey");
                                    }
                                    else
                                    {
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "LoaSur", false, "Load Survey");
                                    }
                                    #endregion

                                    #region Billing
                                    if (masterEntity.Billing != null && !string.IsNullOrEmpty(mtrDataEntity.MeterID) && masterEntity.Billing.Count > 0)
                                    {
                                        //this.StatusMessage = "Please wait. Uploading Billing Data";
                                        //Application.DoEvents();
                                        DLMS650BillingBLL billingBLL = new DLMS650BillingBLL();
                                        List<IEntity> entities = new List<IEntity>();
                                        for (int counter = 0; counter < masterEntity.Billing.Count; counter++)
                                        {
                                            masterEntity.Billing[counter].MeterData_ID = masterEntity.MeterDataID; ;
                                            entities.Add(masterEntity.Billing[counter]);
                                        }
                                        billingBLL.InsertData(entities);
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "BilEneTodCon", true, "Billing Parameters : Energy : TOD Consumption");
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "BilEneTodEne", true, "Billing Parameters : Energy : TOD Energy");
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "BilMis", true, "Billing Parameters : Miscellaneous");
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "BilTouCon", true, "Billing Parameters : TOU Configuration");
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "BilDemMax", true, "Billing Parameters : Demand : Maximum Demand");
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "BilDemPowFac", true, "Billing Parameters : Power Factor");
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "BilDemPowOff", true, "Billing Parameters : Power Off Duration");
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "BilDemTod", true, "Billing Parameters : Demand : TOD MD");
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "BilEneCon", true, "Billing Parameters : Energy : Consumption");
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "BilEneMai", true, "Billing Parameters : Energy : Main Energy");
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "BilDemPowOn", true, "Billing Parameters : Power On Duration");
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "BilDemAvgLoaFac", true, "Billing Parameters : Average Load Factor");
                                        //BilDemPowOnOff tab is not needed as POwer ON/Off is handled b by BilDemPowOff and BilDemPowOff tab combined.
                                        //tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "BilDemPowOnOff", true, "Billing Parameters : Power On Off Duration");

                                    }
                                    else
                                    {
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "BilEneTodCon", false, "Billing Parameters : Energy : TOD Consumption");
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "BilEneTodEne", false, "Billing Parameters : Energy : TOD Energy");
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "BilMis", false, "Billing Parameters : Miscellaneous");
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "BilTouCon", false, "Billing Parameters : TOU Configuration");
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "BilDemMax", false, "Billing Parameters : Demand : Maximum Demand");
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "BilDemPowFac", false, "Billing Parameters : Power Factor");
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "BilDemPowOff", false, "Billing Parameters : Power Off Duration");
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "BilDemTod", false, "Billing Parameters : Demand : TOD MD");
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "BilEneCon", false, "Billing Parameters : Energy : Consumption");
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "BilEneMai", false, "Billing Parameters : Energy : Main Energy");
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "BilDemPowOn", false, "Billing Parameters : Power On Duration");
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "BilDemAvgLoaFac", false, "Billing Parameters : Average Load Factor");
                                        //BilDemPowOnOff tab is not needed as POwer ON/Off is handled b by BilDemPowOff and BilDemPowOff tab combined.
                                        //tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "BilDemPowOnOff", false, "Billing Parameters : Power On Off Duration");
                                    }
                                    #endregion

                                    #region Midnight
                                    if (masterEntity.MidnightData != null && !string.IsNullOrEmpty(mtrDataEntity.MeterID) && masterEntity.MidnightData.Count > 0)
                                    {
                                        //this.StatusMessage = "Please wait. Uploading Midnight Data";
                                        //Application.DoEvents();
                                        DLMS650MidnightDataBLL midnightDataBLL = new DLMS650MidnightDataBLL();
                                        MidnightParameterBLL midnightParameterBLL = new MidnightParameterBLL();
                                        List<IEntity> entities = new List<IEntity>();



                                        for (int counter = 0; counter < masterEntity.MidnightData.Count; counter++)
                                        {
                                            masterEntity.MidnightData[counter].MeterData_ID = masterEntity.MeterDataID; ;
                                            entities.Add(masterEntity.MidnightData[counter]);
                                        }
                                        midnightDataBLL.InsertData(entities);
                                        masterEntity.MidnightParameterColumns.MeterDataId = masterEntity.MeterDataID;
                                        midnightParameterBLL.InsertData(masterEntity.MidnightParameterColumns);
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "MidEne", true, "MidNight Energies");
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "DaiEneCon", true, "Daily Energy Consumption");
                                    }
                                    else
                                    {
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "MidEne", false, "MidNight Energies");
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "DaiEneCon", false, "Daily Energy Consumption");
                                    }
                                    #endregion

                                    #region Tamper
                                    if (masterEntity.Tamper != null && !string.IsNullOrEmpty(mtrDataEntity.MeterID) && masterEntity.Tamper.Count > 0)
                                    {
                                        //this.StatusMessage = "Please wait. Uploading Tamper Data";
                                        //Application.DoEvents();
                                        DLMS650TamperMasterBLL tamperBLL = new DLMS650TamperMasterBLL();
                                        TamperParameterBLL tamperParameterBLL = new TamperParameterBLL();
                                        List<IEntity> entities = new List<IEntity>();
                                        for (int counter = 0; counter < masterEntity.Tamper.Count; counter++)
                                        {
                                            masterEntity.Tamper[counter].MeterData_ID = masterEntity.MeterDataID; ;
                                            entities.Add(masterEntity.Tamper[counter]);
                                        }
                                        tamperBLL.InsertData(entities);
                                        masterEntity.TamperParameterColumns.MeterDataId = masterEntity.MeterDataID;
                                        tamperParameterBLL.InsertData(masterEntity.TamperParameterColumns);
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "Tam", true, "Tamper");
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "Tra", true, "Transaction");
                                    }
                                    else
                                    {
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "Tam", false, "Tamper");
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "Tra", false, "Transaction");
                                    }
                                    #endregion

                                    #region Phasor
                                    if (masterEntity.Phasor != null && !string.IsNullOrEmpty(mtrDataEntity.MeterID))
                                    {
                                        //this.StatusMessage = "Please wait. Uploading phasor Data";
                                        //Application.DoEvents();
                                        masterEntity.Phasor.MeterDataId = masterEntity.MeterDataID; ;
                                        new DLMS650PhasorBLL().InsertData(masterEntity.Phasor);
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "Pha", true, "Phasor");
                                    }
                                    else
                                    {
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "Pha", false, "Phasor");
                                    }
                                    #endregion

                                    #region FraudEnergy
                                    if (masterEntity.FraudEnergy != null && !string.IsNullOrEmpty(mtrDataEntity.MeterID))
                                    {
                                        masterEntity.FraudEnergy.MeterData_ID = masterEntity.MeterDataID; ;
                                        new FraudEnergyBLL().InsertData(masterEntity.FraudEnergy);
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "FraEne", true, "Fraud Energy");
                                    }
                                    else
                                    {
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "FraEne", false, "Fraud Energy");
                                    }
                                    #endregion
                                }


                                catch (Exception ex)    //SarkarA //Exception log for catch block
                                {
                                    logger.Log(LOGLEVELS.Error, "UploadCABFile(string fileName, string fileText, bool flag, out string message, string cmriID)", ex);
                                }
                            }
                        }


                    }
                }

                catch (Exception ex)    //SarkarA //Exception log for catch block
                {
                    EventLogging.CallLogDetails("While uploading " + ex.Message);
                    logger.Log(LOGLEVELS.Error, "UploadCABFile(string fileName, string fileText, bool flag, out string message, string cmriID)", ex);
                    message = "File Corrupted.";
                    isUploaded = false;
                }
            }
            return isUploaded;
        }

        /// <summary>
        /// Upload CAB  file into the db based on readouts.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileText"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public bool UploadSLGFile(string fileName, string fileText, bool flag, out string message, string cmriID)
        {
            message = string.Empty;
            bool isUploaded = false;
            if (!string.IsNullOrEmpty(fileText))
            {
                List<string> meterIDsUploaded = new List<string>();
                FileUploadMasterBLL fileUploadMasterBLL = new FileUploadMasterBLL();
                FileUploadMasterEntity fileUploadMasterEntity = new FileUploadMasterEntity();
                Dictionary<string, string> dicOBISandData = null;
                try
                {
                    if (!isUploaded)
                    {
                        if (!FormatterCommon.IsFileNullOrEmpty(fileText))
                        {
                            message = "File Corrupted.";
                            return isUploaded;
                        }
                        if (!ConfigInfo.IsValidCheckSum(fileText))
                        {
                            message = "BCC Mismatch.";
                            return isUploaded;
                        }
                        fileText = fileText.Replace("", "");
                        fileText = fileText.Replace(FormatterConstant.ENTERCARRAGE, string.Empty);
                        fileText = fileText.Replace(FormatterConstant.NEWLINE, string.Empty);

                        string[] arrDataWithOBIS = fileText.Split('/')[3].Split(')');
                        dicOBISandData = new Dictionary<string, string>();
                        for (int v = 0; v < arrDataWithOBIS.Length - 1; v++)
                        {
                            if (arrDataWithOBIS[v].Contains('('))
                            {
                                string[] arrData = arrDataWithOBIS[v].Split('(');
                                dicOBISandData[arrData[0]] = arrData[1].Trim();    //remove space from MeterId  
                            }                       
                        }
                        dicOBISandData["DateTime"] = fileText.Split('/')[2];

                        fileUploadMasterEntity = new FileUploadMasterEntity();
                        fileUploadMasterEntity.UserInformation_ID = ConfigInfo.UserInformationID;
                        if (flag)
                        {
                            fileUploadMasterEntity.FileContent = TotalBytes(fileName);
                            fileUploadMasterEntity.FileName = Path.GetFileName(fileName);
                            fileUploadMasterEntity.FileSize = GetFileSize(ConfigInfo.EncryptFile(fileText));
                            fileUploadMasterEntity.CMRIID = cmriID;
                        }
                        else
                        {
                            fileUploadMasterEntity.FileContent = ASCIIEncoding.UTF8.GetBytes(ConfigInfo.EncryptFile(fileText));
                            fileUploadMasterEntity.FileName = Path.GetFileName(fileName);
                            fileUploadMasterEntity.FileSize = GetFileSize(ConfigInfo.EncryptFile(fileText));
                            fileUploadMasterEntity.CMRIID = cmriID;
                        }
                        fileUploadMasterEntity.UploadingDateTime = DateUtility.DateTimeToLong(DateTime.Now);

                        string data = FormatterCommon.ParseDataFor1Phase(dicOBISandData, "0.9.1");

                        string dateAndTime = DateUtility.GetFormatedDateTme(data);
                        fileUploadMasterEntity.ReadingDateTime = Convert.ToInt64(dateAndTime);
                        fileUploadMasterEntity.FileType = "NONDLMS";
                        fileUploadMasterEntity.CommType = GetCommType();
                        if (flag)
                        {
                            FileUploadMasterEntity fileEntity = fileUploadMasterBLL.ValidateFile(fileUploadMasterEntity.FileName) as FileUploadMasterEntity;
                            if (fileEntity != null)
                            {
                                if (fileEntity.FileUpload_ID != 0)
                                {
                                    message = "Deleting '" + fileUploadMasterEntity.FileName + "'...";
                                    CommonBLL commonBll = new CommonBLL();
                                    commonBll.DeleteArchieveOperation(fileEntity);
                                }
                            }
                            fileUploadMasterEntity = fileUploadMasterBLL.InsertData(fileUploadMasterEntity) as FileUploadMasterEntity;
                            if (fileUploadMasterEntity.FileUpload_ID == 0)
                            {
                                message = "Please Contact system administrator. Invalid DB Structure.";
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
                        MeterDataEntity mtrDataEntity = new MeterDataEntity();

                        try
                        {
                            mtrDataEntity.FileUpload_ID = fileUploadMasterEntity.FileUpload_ID;
                            mtrDataEntity.UploadingDateTime = DateUtility.DateTimeToLong(System.DateTime.Now);
                            mtrDataEntity.ReadingDateTime = Convert.ToInt64(dateAndTime);
                            mtrDataEntity.CMRIID = cmriID;
                            ParseReadOutsForSPhase(fileText, mtrDataEntity, fileUploadMasterEntity, dicOBISandData);
                                }
                        catch (Exception ex)    //SarkarA //Exception log for catch block
                        {
                            logger.Log(LOGLEVELS.Error, "UploadSLGFile(string fileName, string fileText, bool flag, out string message, string cmriID)", ex); // Story - 354382 - Error Message change while uploading SLG file
                        }

                        List<IECBillingGeneralNFEntity> master = new List<IECBillingGeneralNFEntity>();
                            FormatterBilling formatterBilling = new FormatterBilling();
                        formatterBilling.GetDataForSPhase(fileText, master, readOuts, dicOBISandData);
                        if (master.Count > 0)
                        {
                            List<IECBillingGeneralNFEntity> IECMasterEntity = GetMasterEntity(master[0]);

                            foreach (IECBillingGeneralNFEntity iecMasterEntity in IECMasterEntity)
                            {
                                try
                                {
                                    BillingGeneralNFDLMSEntity masterEntity = new IECToDLMSMapper(false).ConvertIECEntityToDLMSEntityForSPhase(iecMasterEntity, true);

                                    #region General
                                    if (masterEntity.General != null && !string.IsNullOrEmpty(mtrDataEntity.MeterID))
                                    {
                                        DLMS650NamePlateDetailsEntity general = masterEntity.General;
                                        general.MeterData_ID = masterEntity.MeterDataID;
                                        new DLMS650GeneralBLL().InsertData(general);
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "Gen", true, "General");
                                    }
                                    else
                                    {
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "Gen", false, "General");
                                    }
                                    #endregion

                                    #region Instant
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "InsABC", false, "Instant : ABC Code");
                                    if (masterEntity.Instant != null && !string.IsNullOrEmpty(mtrDataEntity.MeterID))
                                    {
                                        DLMS650InstantaneousBLL instantBLL = new DLMS650InstantaneousBLL();
                                        List<IEntity> entities = new List<IEntity>();
                                        for (int counter = 0; counter < masterEntity.Instant.Count; counter++)
                                        {
                                            masterEntity.Instant[counter].MeterDataID = masterEntity.MeterDataID; ;
                                            entities.Add(masterEntity.Instant[counter]);
                                        }
                                        instantBLL.InsertData(entities);
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "InsRea", true, "Instant : Reading");
                                    }
                                    else
                                    {
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "InsRea", false, "Instant : Reading");
                                    }
                                    #endregion

                                    #region Anomaly
                                    if (masterEntity.Anomaly != null && !string.IsNullOrEmpty(mtrDataEntity.MeterID))
                                    {
                                        masterEntity.Anomaly.MeterDataId = masterEntity.MeterDataID; ;
                                        new AnomalyBLL().InsertData(masterEntity.Anomaly);
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "InsSel", true, "Instant : Self Diagnostics");
                                    }
                                    else
                                    {
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "InsSel", false, "Instant : Self Diagnostics");
                                    }
                                    //*************-------------- If anamoly data not configured in meter then remove self diagnostic tab in instant**************
                                    if (masterEntity.Anomaly.EeProm == -1 && masterEntity.Anomaly.Rtc == -1)
                                    {
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "InsSel", false, "Instant : Self Diagnostics");
                                    }

                                    #endregion

                                    #region LoadSurvey
                                    if (masterEntity.LoadSurvey != null && !string.IsNullOrEmpty(mtrDataEntity.MeterID) && masterEntity.LoadSurvey.Count > 0)
                                    {
                                        DLMS650LoadSurveyBLL loadSurveyBLL = new DLMS650LoadSurveyBLL();
                                        LoadSurveyParameterBLL loadSurveyParameterBLL = new LoadSurveyParameterBLL();
                                        List<IEntity> entities = new List<IEntity>();

                                        for (int counter = 0; counter < masterEntity.LoadSurvey.Count; counter++)
                                        {
                                            masterEntity.LoadSurvey[counter].MeterData_ID = masterEntity.MeterDataID;
                                            /* Below line of codes is added 
                                             * To Support Bulk insert in Single phase meter 
                                             * and to Handle Graph 2400 data 
                                             * is to be inserted in next day 0000 data.*/
                                           
                                            string datetime = masterEntity.LoadSurvey[counter].RealTimeClockDateandTime.ToString();

                                            if (datetime.Length >= 12 && datetime.Substring(8, 4) == "2400")
                                            {
                                                long dateInLong = 0;
                                                try
                                                {
                                                    DateTime dt = new DateTime(int.Parse(datetime.Substring(0, 4)), int.Parse(datetime.Substring(4, 2)), int.Parse(datetime.Substring(6, 2)), 0, 0, 0);
                                                    dt = dt.AddDays(1);
                                                    string formattedDateTime = string.Format("{0:yyyyMMddHHmmss}", dt);
                                                    dateInLong = Convert.ToInt64(formattedDateTime);
                                                }
                                                catch (Exception ex)
                                                {
                                                }
                                                masterEntity.LoadSurvey[counter].RealTimeClockDateandTime = dateInLong;
                                            }//20171223080000// to support bulk insert in Single phase:::: @Ravi Code
                                            entities.Add(masterEntity.LoadSurvey[counter]);
                                        }
                                       
                                       //loadSurveyBLL.InsertData(entities);
                                        loadSurveyBLL.BatchInsert(entities);
                                        masterEntity.LSParameterColumns.MeterDataId = masterEntity.MeterDataID;
                                        loadSurveyParameterBLL.InsertData(masterEntity.LSParameterColumns);
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "LoaSur", true, "Load Survey");
                                    }
                                    else
                                    {
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "LoaSur", false, "Load Survey");
                                    }
                                    #endregion

                                    #region Billing
                                    if (masterEntity.Billing != null && !string.IsNullOrEmpty(mtrDataEntity.MeterID) && masterEntity.Billing.Count > 0)
                                    {
                                        DLMS650BillingBLL billingBLL = new DLMS650BillingBLL();
                                        List<IEntity> entities = new List<IEntity>();
                                        for (int counter = 0; counter < masterEntity.Billing.Count; counter++)
                                        {
                                            masterEntity.Billing[counter].MeterData_ID = masterEntity.MeterDataID; ;
                                            entities.Add(masterEntity.Billing[counter]);
                                        }
                                        billingBLL.InsertData(entities);
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "BilEneTodCon", true, "Billing Parameters : Energy : TOD Consumption");
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "BilEneTodEne", true, "Billing Parameters : Energy : TOD Energy");
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "BilMis", true, "Billing Parameters : Miscellaneous");
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "BilTouCon", true, "Billing Parameters : TOU Configuration");
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "BilDemMax", true, "Billing Parameters : Demand : Maximum Demand");
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "BilDemPowFac", true, "Billing Parameters : Power Factor");
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "BilDemPowOff", true, "Billing Parameters : Power Off Duration");
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "BilDemTod", true, "Billing Parameters : Demand : TOD MD");
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "BilEneCon", true, "Billing Parameters : Energy : Consumption");
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "BilEneMai", true, "Billing Parameters : Energy : Main Energy");
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "BilDemPowOn", true, "Billing Parameters : Power On Duration");
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "BilDemAvgLoaFac", true, "Billing Parameters : Average Load Factor");                                        

                                    }
                                    else
                                    {
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "BilEneTodCon", false, "Billing Parameters : Energy : TOD Consumption");
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "BilEneTodEne", false, "Billing Parameters : Energy : TOD Energy");
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "BilMis", false, "Billing Parameters : Miscellaneous");
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "BilTouCon", false, "Billing Parameters : TOU Configuration");
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "BilDemMax", false, "Billing Parameters : Demand : Maximum Demand");
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "BilDemPowFac", false, "Billing Parameters : Power Factor");
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "BilDemPowOff", false, "Billing Parameters : Power Off Duration");
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "BilDemTod", false, "Billing Parameters : Demand : TOD MD");
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "BilEneCon", false, "Billing Parameters : Energy : Consumption");
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "BilEneMai", false, "Billing Parameters : Energy : Main Energy");
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "BilDemPowOn", false, "Billing Parameters : Power On Duration");
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "BilDemAvgLoaFac", false, "Billing Parameters : Average Load Factor");
                                    }
                                    #endregion

                                    #region Midnight
                                    if (masterEntity.MidnightData != null && !string.IsNullOrEmpty(mtrDataEntity.MeterID) && masterEntity.MidnightData.Count > 0)
                                    {
                                        //this.StatusMessage = "Please wait. Uploading Midnight Data";
                                        //Application.DoEvents();
                                        DLMS650MidnightDataBLL midnightDataBLL = new DLMS650MidnightDataBLL();
                                        MidnightParameterBLL midnightParameterBLL = new MidnightParameterBLL();
                                        List<IEntity> entities = new List<IEntity>();



                                        for (int counter = 0; counter < masterEntity.MidnightData.Count; counter++)
                                        {
                                            masterEntity.MidnightData[counter].MeterData_ID = masterEntity.MeterDataID; ;
                                            entities.Add(masterEntity.MidnightData[counter]);
                                        }
                                        midnightDataBLL.InsertData(entities);
                                        masterEntity.MidnightParameterColumns.MeterDataId = masterEntity.MeterDataID;
                                        midnightParameterBLL.InsertData(masterEntity.MidnightParameterColumns);
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "MidEne", true, "MidNight Energies");
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "DaiEneCon", true, "Daily Energy Consumption");
                                    }
                                    else
                                    {
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "MidEne", false, "MidNight Energies");
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "DaiEneCon", false, "Daily Energy Consumption");
                                    }
                                    #endregion

                                    #region Tamper
                                    if (masterEntity.Tamper != null && !string.IsNullOrEmpty(mtrDataEntity.MeterID) && masterEntity.Tamper.Count > 0)
                                    {
                                        //this.StatusMessage = "Please wait. Uploading Tamper Data";
                                        //Application.DoEvents();
                                        DLMS650TamperMasterBLL tamperBLL = new DLMS650TamperMasterBLL();
                                        TamperParameterBLL tamperParameterBLL = new TamperParameterBLL();
                                        List<IEntity> entities = new List<IEntity>();
                                        for (int counter = 0; counter < masterEntity.Tamper.Count; counter++)
                                        {
                                            masterEntity.Tamper[counter].MeterData_ID = masterEntity.MeterDataID; ;
                                            entities.Add(masterEntity.Tamper[counter]);
                                        }
                                        tamperBLL.InsertData(entities);
                                        masterEntity.TamperParameterColumns.MeterDataId = masterEntity.MeterDataID;
                                        tamperParameterBLL.InsertData(masterEntity.TamperParameterColumns);
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "Tam", true, "Tamper");
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "Tra", true, "Transaction");
                                    }
                                    else
                                    {
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "Tam", false, "Tamper");
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "Tra", false, "Transaction");
                                    }
                                    #endregion

                                    #region Phasor
                                    if (masterEntity.Phasor != null && !string.IsNullOrEmpty(mtrDataEntity.MeterID))
                                    {
                                        //this.StatusMessage = "Please wait. Uploading phasor Data";
                                        //Application.DoEvents();
                                        masterEntity.Phasor.MeterDataId = masterEntity.MeterDataID; ;
                                        new DLMS650PhasorBLL().InsertData(masterEntity.Phasor);
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "Pha", true, "Phasor");
                                    }
                                    else
                                    {
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "Pha", false, "Phasor");
                                    }
                                    #endregion

                                    #region FraudEnergy
                                    if (masterEntity.FraudEnergy != null && !string.IsNullOrEmpty(mtrDataEntity.MeterID))
                                    {
                                        masterEntity.FraudEnergy.MeterData_ID = masterEntity.MeterDataID; ;
                                        new FraudEnergyBLL().InsertData(masterEntity.FraudEnergy);
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "FraEne", true, "Fraud Energy");
                                    }
                                    else
                                    {
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "FraEne", false, "Fraud Energy");
                                    }
                                    #endregion

                                    // Set Visibility false for 1P IEC meter , only tod is visible in meter configurataion tab
                                    #region Meter Configuration Not Supported IEC

                                    tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "MDWithIP", false, "Meter Configuration : MDWithIP");
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "DspPar", false, "Meter Configuration : Display Parameters"); tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "DIP", false, "Meter Configuration : DIP");
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "KvarSel", false, "Meter Configuration : Kvar Selection");
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "RS232", false, "Meter Configuration : RS232");
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "BillTyp", false, "Meter Configuration : Billing Type");
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "RTC", false, "Meter Configuration : RTC");
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "LSIP", false, "Meter Configuration : LSIP");
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "AutoLck", false, "Meter Configuration : Auto Lock");
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "DaiLog", false, "Meter Configuration : Daily Log");
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "ManBil", false, "Meter Configuration : Manual Billing");
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "SofBil", false, "Meter Configuration : Software Billing");

                                    #endregion

                                    // TOD Parsing for IEC meter is added
                                    #region TOD
                                    if (masterEntity.MeterConfigurations.TODEntity != null && !string.IsNullOrEmpty(masterEntity.MeterData.MeterID))
                                    {
                                        //masterEntity.MeterConfigurations.TODEntity.MeterDataID = masterEntity.MeterData.MeterData_ID;
                                        new TodBLL().InsertData(masterEntity.MeterConfigurations.TODEntity);
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterConfigurations.TODEntity.MeterDataID, "MtrCfg", true, "Meter Configuration ");
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "TOD", true, "Meter Configuration : TOD");

                                    }
                                    else
                                    {
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterDataID, "MtrCfg", false, "Meter Configuration ");
                                    }
                                    #endregion



                                }


                                catch (Exception ex)    //SarkarA //Exception log for catch block
                                {
                                    logger.Log(LOGLEVELS.Error, "UploadSLGFile(string fileName, string fileText, bool flag, out string message, string cmriID)", ex); // Story - 354382 - Error Message change while uploading SLG file
                                }
                            }
                        }


                    }
                }

                catch (Exception ex)    //SarkarA //Exception log for catch block
                {
                    EventLogging.CallLogDetails("While uploading " + ex.Message);
                    logger.Log(LOGLEVELS.Error, "UploadSLGFile(string fileName, string fileText, bool flag, out string message, string cmriID)", ex);// Story - 354382 - Error Message change while uploading SLG file
                    message = "File Corrupted.";
                    isUploaded = false;
                }
            }
            return isUploaded;
        }
        /// <summary>
        /// Upload 2NG file into the db
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileText"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public bool Upload2NGFile(string fileName, string fileText, bool flag, out string message, string cmriID)
        {
            ConfigInfo.DisplayProgrammingVariant = CAB.Framework.DisplayProgrammingTypes.OneByte;
            bool isMeterConfigurationAvailable = false;
            message = string.Empty;
            bool isCMRIFile = false;
            FileUploadMasterBLL fileUploadMasterBLL = new FileUploadMasterBLL();
            FileUploadMasterEntity fileUploadMasterEntity = new FileUploadMasterEntity();
            DLMS650FormatterCommon commom = new DLMS650FormatterCommon();
            bool isUploaded = false;

            try
            {
                if (!isUploaded)
                {
                    if (string.IsNullOrEmpty(fileText))
                    {
                        message = "File Corrupted.";
                        // ////Application.DoEvents();
                        return isUploaded;
                    }

                    //fix Ashish - 10/10/2011 - do not delete this commented code
                    string extension = fileName.Substring(fileName.LastIndexOf("."), 4).ToUpper();

                    if (extension != ".2NG" && extension != ".EXP")
                    {
                        message = "extension of file should be either .2NG or .EXP or .FDL type";
                        //////Application.DoEvents();
                        return isUploaded;
                    }
                    fileUploadMasterEntity.UserInformation_ID = ConfigInfo.UserInformationID;

                    if (flag)
                    {
                        fileUploadMasterEntity.FileContent = TotalBytes(fileName);
                        fileUploadMasterEntity.FileName = Path.GetFileName(fileName);
                        fileUploadMasterEntity.FileSize = GetFileSize(fileText);
                        //for aumatic CMRI association
                        fileUploadMasterEntity.CMRIID = cmriID;
                    }
                    else
                    {
                        fileUploadMasterEntity.FileContent = ASCIIEncoding.UTF8.GetBytes(fileText);
                        fileUploadMasterEntity.FileName = Path.GetFileName(fileName);
                        fileUploadMasterEntity.FileSize = GetFileSize(fileText);
                        //for aumatic CMRI association
                        fileUploadMasterEntity.CMRIID = cmriID;
                    }

                    fileUploadMasterEntity.UploadingDateTime = DateUtility.DateTimeToLong(DateTime.Now);
                    fileUploadMasterEntity.FileType = "DLMS";
                    fileUploadMasterEntity.CommType = GetCommType();

                    if (flag)
                    {
                        FileUploadMasterEntity fileUploadEntity = fileUploadMasterBLL.ValidateFile(fileUploadMasterEntity.FileName) as FileUploadMasterEntity;
                        if (fileUploadEntity != null)
                        {
                            if (fileUploadEntity.FileUpload_ID != 0)
                            {
                                //message = "File '" + fileUploadMasterEntity.FileName + "' already exist.";
                                //  ////Application.DoEvents();
                                message = "Deleting '" + fileUploadMasterEntity.FileName + "'...";
                                CommonBLL commonBll = new CommonBLL();
                                commonBll.DeleteArchieveOperation(fileUploadEntity);
                            }
                        }
                        fileUploadMasterEntity = fileUploadMasterBLL.InsertData(fileUploadMasterEntity) as FileUploadMasterEntity;
                        if (fileUploadMasterEntity.FileUpload_ID == 0)
                        {
                            message = "Please Contact system administrator. Invalid DB Structure.";
                            //  ////Application.DoEvents();
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
                    string[] individualFileContent = fileText.Split('$');

                    if (individualFileContent.Length > 2)
                    {
                        //CMRI File bcz more than one readout data appended in same file 
                        isCMRIFile = true;
                    }

                    string[] strsplbillingdata = null;

                    for (int index = 0; index < individualFileContent.Length; index++)
                    {
                        isMeterConfigurationAvailable = false;
                        if (!string.IsNullOrEmpty(individualFileContent[index]) && individualFileContent[index] != "\r\n")
                        {
                            //For CMRI file more than one file data comes in same file separeted by &
                            //but checksum insterted only at end of file so we need to make sure that while conetent passed to enetity generator 
                            //We insert one blank row to file content to make sure that entity generator works in same way for CMRI or meter read files.
                            if (isCMRIFile && (individualFileContent[individualFileContent.Length - 1] != individualFileContent[index]))
                            {
                                individualFileContent[index] = string.Concat(individualFileContent[index], "\r\n");
                            }
                            try
                            {
                                //Get MeterID length form file and fill in configInfo element that will be used to support dynamic meterId lenth for FD readout.
                                ConfigInfo.MeterIdLength = Convert.ToInt32(individualFileContent[index].Substring(2, 2).Trim('\r').Trim('\n'));

                                DateTime readingDateTime = DateTime.MinValue;
                                if (!DateTime.TryParseExact(individualFileContent[index].Substring(4, 22).Trim('\r').Trim('\n').Replace('-', '/').Replace('.', '/'), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out readingDateTime))
                                {
                                    throw new FormatException("Reading DateTime not in correct format."); 
                                }

                                BillingGeneralNFDLMSEntity masterEntity = GetMasterEntity(individualFileContent[index].Substring(27), true);

                                #region MeterData
                                if (masterEntity.MeterData != null && !string.IsNullOrEmpty(masterEntity.MeterData.MeterID))
                                {
                                    // MeterDataEntity meterDataEntity = new MeterDataEntity();
                                    // meterDataEntity.MeterID = masterEntity.General.MeterSerialNumber;
                                    masterEntity.MeterData.FileUpload_ID = fileUploadMasterEntity.FileUpload_ID;
                                    masterEntity.MeterData.CMRIID = cmriID;

                                    masterEntity.MeterData.ReadingDateTime = DateUtility.DateTimeToLong(readingDateTime);

                                    //meterDataEntity.FileUpload_ID = fileUploadMasterEntity.FileUpload_ID;
                                    masterEntity.MeterData.UploadingDateTime = DateUtility.DateTimeToLong(System.DateTime.Now);

                                    strsplbillingdata = individualFileContent[index].Split('\n');

                                    if (strsplbillingdata != null && strsplbillingdata.Count() > 3)
                                    {

                                        if (strsplbillingdata[4].Substring(0, 2) == "98" && strsplbillingdata[4].Length >= 66)
                                        {
                                            fileUploadMasterEntity.FileType = "CUSTOM";
                                            masterEntity.MeterData.MeterID = commom.ConvertHexToString(strsplbillingdata[4].Substring(6, 14));
                                            DLMS650InstantaneousEntity instantEntity = new DLMS650InstantaneousEntity();
                                            masterEntity.Instant = new List<DLMS650InstantaneousEntity>();
                                            // Filling RTC Datetime

                                            string strmeterdatetime = strsplbillingdata[4].Substring(20, 12);
                                            string strSeconds = strmeterdatetime.Substring(0, 2);
                                            string strMinutes = strmeterdatetime.Substring(2, 2);
                                            string strHours = strmeterdatetime.Substring(4, 2);
                                            string strDate = strmeterdatetime.Substring(6, 2);
                                            string strMonth = strmeterdatetime.Substring(8, 2);
                                            string strYrs = "20" + strmeterdatetime.Substring(10, 2);

                                            // MAP RTC Entity
                                            instantEntity.InstantPower_ID = 0;
                                            instantEntity.InstantPowerObisCode = "0.0.1.0.0.255";
                                            instantEntity.InstantPowerColumnName = "Real Time Clock - Date and Time";
                                            instantEntity.InstantPowerClassID = "8";
                                            instantEntity.InstantPowerAttribute = "2";
                                            instantEntity.InstantPowerDataIndex = 1;
                                            instantEntity.InstantPowerColumnValue = strYrs + strMonth + strDate + strHours + strMinutes + strSeconds;
                                            masterEntity.Instant.Add(instantEntity);

                                            instantEntity = new DLMS650InstantaneousEntity(); 
                                            
                                            // Filling Commulative kWh History 1
                                            string strCumulativeEnergykWhValue1 = strsplbillingdata[4].Substring(32, 16);
                                            strCumulativeEnergykWhValue1 = ReverseString(strCumulativeEnergykWhValue1);
                                            long lkwh1 = 0;
                                            long.TryParse(strCumulativeEnergykWhValue1, NumberStyles.AllowHexSpecifier, null, out lkwh1);
                                            if (lkwh1 < 0) lkwh1 = 0;

                                            // Filling Commulative kWh
                                            string strCumulativeEnergykWhValue = strsplbillingdata[4].Substring(48, 16);
                                            strCumulativeEnergykWhValue = ReverseString(strCumulativeEnergykWhValue);
                                            long lkwh =  0;
                                            long.TryParse(strCumulativeEnergykWhValue, NumberStyles.AllowHexSpecifier, null, out lkwh);
                                            if (lkwh < 0) lkwh = 0;

                                            // Filling Net Commulative kWh
                                            long lNetkwh = 0;
                                            lNetkwh = lkwh - lkwh1;
                                           

                                            // Filling MD kW
                                            /*string strMDValue = strsplbillingdata[4].Substring(48, 8);
                                            strMDValue = ReverseString(strMDValue);
                                            long lkw = 0;
                                            long.TryParse(strMDValue, NumberStyles.AllowHexSpecifier, null, out lkw);*/

                                            // Filling MD kVA History 1
                                            string strMDkVAValueHistory1 = strsplbillingdata[4].Substring(64, 8);
                                            strMDkVAValueHistory1 = ReverseString(strMDkVAValueHistory1);
                                            long lkVAH1 = 0;
                                            long.TryParse(strMDkVAValueHistory1, NumberStyles.AllowHexSpecifier, null, out lkVAH1);
                                            if (lkVAH1 < 0) lkVAH1 = 0;
                                           
                                            // Filling MD KVA History 1 Date Time
                                            strSeconds = "00";
                                            strmeterdatetime = strsplbillingdata[4].Substring(72, 10);
                                            strMinutes = strmeterdatetime.Substring(0, 2);
                                            strHours = strmeterdatetime.Substring(2, 2);
                                            strDate = strmeterdatetime.Substring(4, 2);
                                            strMonth = strmeterdatetime.Substring(6, 2);
                                            strYrs = "20" + strmeterdatetime.Substring(8, 2);
                                            string mdkvatimestamp1 = strYrs + strMonth + strDate + strHours + strMinutes + strSeconds;

                                            // Filling MD kVA current
                                            string strMDkVAValue = strsplbillingdata[4].Substring(82, 8);
                                            strMDkVAValue = ReverseString(strMDkVAValue);
                                            long lkVA = 0;
                                            long.TryParse(strMDkVAValue, NumberStyles.AllowHexSpecifier, null, out lkVA);
                                            if (lkVA < 0) lkVA = 0;

                                            // Filling MD KVA Current Date Time

                                            strmeterdatetime = strsplbillingdata[4].Substring(90, 10);
                                            strMinutes = strmeterdatetime.Substring(0, 2);
                                            strHours = strmeterdatetime.Substring(2, 2);
                                            strDate = strmeterdatetime.Substring(4, 2);
                                            strMonth = strmeterdatetime.Substring(6, 2);
                                            strYrs = "20" + strmeterdatetime.Substring(8, 2);
                                            string mdkvatimestamp = strYrs + strMonth + strDate + strHours + strMinutes + strSeconds;

                                            // Filling Billing Avg PF History 1

                                            string stravgpfbill1 = strsplbillingdata[4].Substring(100, 4);
                                            stravgpfbill1 = ReverseString(stravgpfbill1);
                                            long lavgpf = 0;
                                            long.TryParse(stravgpfbill1, NumberStyles.AllowHexSpecifier, null, out lavgpf);
                                            if (lavgpf < 0) lavgpf = 0;

                                            // Filling Cumulative Tamper Counter

                                            string strtampercounter = strsplbillingdata[4].Substring(104, 4);
                                            strtampercounter = ReverseString(strtampercounter);
                                            long ltampercounter = 0;
                                            long.TryParse(strtampercounter, NumberStyles.AllowHexSpecifier, null, out ltampercounter);
                                            if (ltampercounter < 0) ltampercounter = 0;
                                           
                                            // Filling Billing Cumulative Tamper Counter History 1

                                            string strbillingtampercounter = strsplbillingdata[4].Substring(108, 4);
                                            strbillingtampercounter = ReverseString(strbillingtampercounter);
                                            long lbillingtampercounter = 0;
                                            long.TryParse(strbillingtampercounter, NumberStyles.AllowHexSpecifier, null, out lbillingtampercounter);
                                            if (lbillingtampercounter < 0) lbillingtampercounter = 0;

                                            // MAP COMM KWH HISTORY 1 ENTITY
                                            instantEntity = new DLMS650InstantaneousEntity();
                                            instantEntity.InstantPower_ID = 0;
                                            instantEntity.InstantPowerObisCode = "1.0.1.8.0.255";
                                            instantEntity.InstantPowerColumnName = "Cumulative Energy History 1";
                                            instantEntity.InstantPowerClassID = "3";
                                            instantEntity.InstantPowerAttribute = "2";
                                            instantEntity.InstantPowerDataIndex = 2;
                                            instantEntity.InstantPowerColumnValue = (lkwh1 / 1000000.0).ToString("0.000");//+ "*kWh";
                                            instantEntity.InstantPowerColumnValue = instantEntity.InstantPowerColumnValue.Remove(instantEntity.InstantPowerColumnValue.Length - 1, 1) + "*kWh";

                                            masterEntity.Instant.Add(instantEntity);

                                            // MAP COMM KWH ENTITY
                                            instantEntity = new DLMS650InstantaneousEntity();
                                            instantEntity.InstantPower_ID = 0;
                                            instantEntity.InstantPowerObisCode = "1.0.1.8.0.255";
                                            instantEntity.InstantPowerColumnName = "Cumulative Energy Current";
                                            instantEntity.InstantPowerClassID = "3";
                                            instantEntity.InstantPowerAttribute = "2";
                                            instantEntity.InstantPowerDataIndex = 3;
                                            instantEntity.InstantPowerColumnValue = (lkwh / 1000000.0).ToString("0.000");// +"*kWh";
                                            instantEntity.InstantPowerColumnValue = instantEntity.InstantPowerColumnValue.Remove(instantEntity.InstantPowerColumnValue.Length - 1, 1) + "*kWh";

                                            masterEntity.Instant.Add(instantEntity);

                                            // MAP MD kVA History 1 ENTITY
                                            instantEntity = new DLMS650InstantaneousEntity();
                                            instantEntity.InstantPower_ID = 0;
                                            instantEntity.InstantPowerObisCode = "1.0.9.6.0.255";
                                            instantEntity.InstantPowerColumnName = "Maximum Demand kVA History 1";
                                            instantEntity.InstantPowerClassID = "4";
                                            instantEntity.InstantPowerAttribute = "2";
                                            instantEntity.InstantPowerDataIndex = 4;
                                            instantEntity.InstantPowerColumnValue = (lkVAH1 / 1000000.0).ToString("0.000");// +"*kVA";
                                            instantEntity.InstantPowerColumnValue = instantEntity.InstantPowerColumnValue.Remove(instantEntity.InstantPowerColumnValue.Length - 1, 1) + "*kVA";

                                            masterEntity.Instant.Add(instantEntity);

                                            // MAP MD KVA DATE TIME HISTOR 1 ENTITY
                                            instantEntity = new DLMS650InstantaneousEntity();
                                            instantEntity.InstantPower_ID = 0;
                                            instantEntity.InstantPowerObisCode = "1.0.9.6.0.255";
                                            instantEntity.InstantPowerColumnName = "Maximum Demand kVA History 1 Date Time";
                                            instantEntity.InstantPowerClassID = "4";
                                            instantEntity.InstantPowerAttribute = "5";
                                            instantEntity.InstantPowerDataIndex = 5;
                                            instantEntity.InstantPowerColumnValue = mdkvatimestamp1;
                                            masterEntity.Instant.Add(instantEntity);

                                            // MAP MD kVA Current ENTITY
                                            instantEntity = new DLMS650InstantaneousEntity();
                                            instantEntity.InstantPower_ID = 0;
                                            instantEntity.InstantPowerObisCode = "1.0.9.6.0.255";
                                            instantEntity.InstantPowerColumnName = "Maximum Demand kVA Current";
                                            instantEntity.InstantPowerClassID = "4";
                                            instantEntity.InstantPowerAttribute = "2";
                                            instantEntity.InstantPowerDataIndex = 6;
                                            instantEntity.InstantPowerColumnValue = (lkVA / 1000000.0).ToString("0.000");// +"*kVA";
                                            instantEntity.InstantPowerColumnValue = instantEntity.InstantPowerColumnValue.Remove(instantEntity.InstantPowerColumnValue.Length - 1, 1) + "*kVA";

                                            masterEntity.Instant.Add(instantEntity);

                                            // MAP MD KVA DATE TIME HISTOR 1 ENTITY
                                            instantEntity = new DLMS650InstantaneousEntity();
                                            instantEntity.InstantPower_ID = 0;
                                            instantEntity.InstantPowerObisCode = "1.0.9.6.0.255";
                                            instantEntity.InstantPowerColumnName = "Maximum Demand kVA Date Time";
                                            instantEntity.InstantPowerClassID = "4";
                                            instantEntity.InstantPowerAttribute = "5";
                                            instantEntity.InstantPowerDataIndex = 7;
                                            instantEntity.InstantPowerColumnValue = mdkvatimestamp;
                                            masterEntity.Instant.Add(instantEntity);

                                            // MAP Billing Avg PH History 1
                                            instantEntity = new DLMS650InstantaneousEntity();
                                            instantEntity.InstantPower_ID = 0;
                                            instantEntity.InstantPowerObisCode = "1.0.13.0.0.255";
                                            instantEntity.InstantPowerColumnName = "Billing Avg PF History 1";
                                            instantEntity.InstantPowerClassID = "3";
                                            instantEntity.InstantPowerAttribute = "2";
                                            instantEntity.InstantPowerDataIndex = 8;
                                            instantEntity.InstantPowerColumnValue = (lavgpf / 100.0).ToString("0.00");
                                            masterEntity.Instant.Add(instantEntity);

                                            // MAP Cumulative TC
                                            instantEntity = new DLMS650InstantaneousEntity();
                                            instantEntity.InstantPower_ID = 0;
                                            instantEntity.InstantPowerObisCode = "0.0.94.91.0.255";
                                            instantEntity.InstantPowerColumnName = "Cumulative Tamper Counter";
                                            instantEntity.InstantPowerClassID = "1";
                                            instantEntity.InstantPowerAttribute = "2";
                                            instantEntity.InstantPowerDataIndex = 9;
                                            instantEntity.InstantPowerColumnValue = (ltampercounter).ToString("d2");
                                            masterEntity.Instant.Add(instantEntity);

                                            // MAP Billing Cumulative TC
                                            instantEntity = new DLMS650InstantaneousEntity();
                                            instantEntity.InstantPower_ID = 0;
                                            instantEntity.InstantPowerObisCode = "0.0.96.2.190.255";
                                            instantEntity.InstantPowerColumnName = "Billing Tamper Counter History 1";
                                            instantEntity.InstantPowerClassID = "1";
                                            instantEntity.InstantPowerAttribute = "2";
                                            instantEntity.InstantPowerDataIndex = 10;
                                            instantEntity.InstantPowerColumnValue = (lbillingtampercounter).ToString("d2");
                                            masterEntity.Instant.Add(instantEntity);

                                            // MAP Net Cumulative kWh
                                            instantEntity = new DLMS650InstantaneousEntity();
                                            instantEntity.InstantPower_ID = 0;
                                            instantEntity.InstantPowerObisCode = "1.0.1.8.0.255";
                                            instantEntity.InstantPowerColumnName = "Net Energy";
                                            instantEntity.InstantPowerClassID = "3";
                                            instantEntity.InstantPowerAttribute = "2";
                                            instantEntity.InstantPowerDataIndex = 11;
                                            if (lNetkwh >= 0)
                                            {
                                                instantEntity.InstantPowerColumnValue = (lNetkwh / 1000000.0).ToString("0.000");// +"*kWh";
                                                instantEntity.InstantPowerColumnValue = instantEntity.InstantPowerColumnValue.Remove(instantEntity.InstantPowerColumnValue.Length - 1, 1) + "*kWh";

                                            }
                                            else
                                                instantEntity.InstantPowerColumnValue = "---";

                                            masterEntity.Instant.Add(instantEntity);
                                        }
                                    }


                                    if (!meterDataBLL.ValidateData(fileUploadMasterEntity.FileUpload_ID, masterEntity.MeterData.MeterID, masterEntity.MeterData.ReadingDateTime))
                                        masterEntity.MeterData = new MeterDataBLL().InsertData(masterEntity.MeterData) as MeterDataEntity;
                                    else
                                        masterEntity.MeterData = meterDataBLL.GetDetailData(masterEntity.MeterData.MeterID, fileUploadMasterEntity.FileUpload_ID, masterEntity.MeterData.ReadingDateTime) as MeterDataEntity;

                                }
                                #endregion

                                #region General
                                //new check for Meter Serial no is zero (0) then skip General profile
                                if (masterEntity.General != null && !string.IsNullOrEmpty(masterEntity.MeterData.MeterID) && masterEntity.General.MeterSerialNumber != "0")
                                {
                                    DLMS650NamePlateDetailsEntity general = masterEntity.General;
                                    //////this.StatusMessageAsync = "Please wait. Uploading General Data";
                                    // ////Application.DoEvents();
                                    general.MeterData_ID = masterEntity.MeterData.MeterData_ID;

                                    //VBM - Prefix additional 1 to serial number  
                                    if (UtilityDetails.PrimaryUtlityName == CAB.Framework.UtilityEntity.SHYAMINDUS.ToString())
                                    {
                                        general.MeterSerialNumber = "1" + general.MeterSerialNumber;
                                    }
                                    new DLMS650GeneralBLL().InsertData(general);

                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "Gen", true, "General");
                                }
                                else
                                {
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "Gen", false, "General");
                                }
                                #endregion

                                #region NamePlateProfile
                                if (masterEntity.NamePlateProfile != null && !string.IsNullOrEmpty(masterEntity.MeterData.MeterID))
                                {
                                    DLMS650NamePlateDetailsEntity namePlateProfile = masterEntity.NamePlateProfile;
                                    //////this.StatusMessageAsync = "Please wait. Uploading General Data";
                                    // ////Application.DoEvents();
                                    namePlateProfile.MeterData_ID = masterEntity.MeterData.MeterData_ID;

                                    new DLMS650NamePlateBLL().InsertData(namePlateProfile);

                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "Nam", true, "NamePlate Profile");
                                }
                                else
                                {
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "Nam", false, "NamePlate Profile");
                                }
                                #endregion

                                #region Instant
                                tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "InsABC", false, "Instant : ABC Code");
                                if (masterEntity.Instant != null && masterEntity.Instant.Count > 0 && !string.IsNullOrEmpty(masterEntity.MeterData.MeterID))
                                {

                                    DLMS650InstantaneousBLL instantBLL = new DLMS650InstantaneousBLL();
                                    List<IEntity> entities = new List<IEntity>();
                                    for (int counter = 0; counter < masterEntity.Instant.Count; counter++)
                                    {
                                        masterEntity.Instant[counter].MeterDataID = masterEntity.MeterData.MeterData_ID;
                                        entities.Add(masterEntity.Instant[counter]);
                                    }
                                    
                                    // SB Code Change Start - 20171115
                                   string MeterModelNumber = masterEntity.General.MeterModelNo;
                                   if (MeterModelNumber != "34" && MeterModelNumber != "35" && MeterModelNumber != "36" && MeterModelNumber != "37" && MeterModelNumber != "43")
                                    this.CalculateReactiveCurrent(masterEntity.Instant, masterEntity.MeterData.MeterData_ID, ref entities);
                                    // SB Code Change End - 20171115

                                    // Pradipta add Appranet 20042018
                                    if (masterEntity.General.Metertype == "1P-2W")
                                    {
                                    }
                                    else{
                                        if (MeterModelNumber != "34" && MeterModelNumber != "35" && MeterModelNumber != "36" && MeterModelNumber != "37" && MeterModelNumber != "43")
                                        this.CalculateApparentPower(masterEntity.Instant, masterEntity.MeterData.MeterData_ID, ref entities);
                                    }
                                    instantBLL.InsertData(entities);
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "InsRea", true, "Instant : Reading");
                                }
                                else
                                {
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "InsRea", false, "Instant : Reading");
                                }
                                #endregion

                                #region LoadSurvey
                                if (masterEntity.LoadSurvey != null && masterEntity.LoadSurvey.Count > 0 && !string.IsNullOrEmpty(masterEntity.MeterData.MeterID))
                                {
                                    //////this.StatusMessage = "Please wait. Uploading LoadSurvey Data";
                                    //////Application.DoEvents();
                                    DLMS650LoadSurveyBLL loadSurveyBLL = new DLMS650LoadSurveyBLL();
                                    LoadSurveyParameterBLL loadSurveyParameterBLL = new LoadSurveyParameterBLL();
                                    List<IEntity> entities = new List<IEntity>();

                                    for (int counter = 0; counter < masterEntity.LoadSurvey.Count; counter++)
                                    {
                                        masterEntity.LoadSurvey[counter].MeterData_ID = masterEntity.MeterData.MeterData_ID;
                                        entities.Add(masterEntity.LoadSurvey[counter]);
                                    }
                                  
                                    loadSurveyBLL.BatchInsert(entities);
                                    masterEntity.LSParameterColumns.MeterDataId = masterEntity.MeterData.MeterData_ID;
                                    loadSurveyParameterBLL.InsertData(masterEntity.LSParameterColumns);
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "LoaSur", true, "Load Survey");
                                }
                                else
                                {
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "LoaSur", false, "Load Survey");
                                }
                                #endregion

                                #region Billing
                                if (masterEntity.Billing != null && masterEntity.Billing.Count > 0 && !string.IsNullOrEmpty(masterEntity.MeterData.MeterID))
                                {
                                    //////this.StatusMessage = "Please wait. Uploading Billing Data";
                                    // ////Application.DoEvents();
                                   DLMS650BillingBLL billingBLL = new DLMS650BillingBLL();
                                    BillingParameterBLL billingParameterBLL = new BillingParameterBLL();
                                    List<IEntity> entities = new List<IEntity>();
                                    for (int counter = 0; counter < masterEntity.Billing.Count; counter++)
                                    {
                                        masterEntity.Billing[counter].MeterData_ID = masterEntity.MeterData.MeterData_ID;
                                        entities.Add(masterEntity.Billing[counter]);
                                    }
                                    billingBLL.InsertData(entities);
                                    masterEntity.BillingParameterColumns.MeterDataId = masterEntity.MeterData.MeterData_ID;
                                    billingParameterBLL.InsertData(masterEntity.BillingParameterColumns);
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "BilEneTodCon", true, "Billing Parameters : Energy : TOD Consumption");
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "BilEneTodEne", true, "Billing Parameters : Energy : TOD Energy");
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "BilMis", true, "Billing Parameters : Miscellaneous");
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "BilTouCon", true, "Billing Parameters : TOU Configuration");
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "BilDemMax", true, "Billing Parameters : Demand : Maximum Demand");
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "BilDemPowFac", true, "Billing Parameters : Power Factor");
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "BilDemPowOff", true, "Billing Parameters : Power Off Duration");
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "BilDemTod", true, "Billing Parameters : Demand : TOD MD");
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "BilEneCon", true, "Billing Parameters : Energy : Consumption");
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "BilEneMai", true, "Billing Parameters : Energy : Main Energy");
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "BilDemPowOn", true, "Billing Parameters : Power On Duration");
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "BilDemAvgLoaFac", true, "Billing Parameters : Average Load Factor");
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "BilDemAvgLoad", true, "Billing Parameters : Average Load");
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "BilTouAvgPowFac", true, "Billing Parameters : TOD Average Export PF");//story 1024441 Add TOD Export PF
                                    if (!masterEntity.BillingParameterColumns.ColumnsNames.Contains("CumulativeMDkw"))// && masterEntity.BillingParameterColumns.ColumnsNames != "CumulativeMDkva")

                                        tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "BillCumulativeMD", false, "Billing Parameters : Demand : Cumulative MD");//for all meter
                                    
                                    else
                                        tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "BillCumulativeMD", true, "Billing Parameters : Demand : Cumulative MD");//for smart  meters
                                }
                                else
                                {
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "BilEneTodCon", false, "Billing Parameters : Energy : TOD Consumption");
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "BilEneTodEne", false, "Billing Parameters : Energy : TOD Energy");
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "BilMis", false, "Billing Parameters : Miscellaneous");
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "BilTouCon", false, "Billing Parameters : TOU Configuration");
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "BilDemMax", false, "Billing Parameters : Demand : Maximum Demand");
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "BilDemPowFac", false, "Billing Parameters : Power Factor");
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "BilDemPowOff", false, "Billing Parameters : Power Off Duration");
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "BilDemTod", false, "Billing Parameters : Demand : TOD MD");
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "BilEneCon", false, "Billing Parameters : Energy : Consumption");
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "BilEneMai", false, "Billing Parameters : Energy : Main Energy");
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "BilDemPowOn", false, "Billing Parameters : Power On Duration");
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "BilDemAvgLoaFac", false, "Billing Parameters : Average Load Factor");
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "BilDemAvgLoad", false, "Billing Parameters : Average Load");
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "BilTouAvgPowFac", false, "Billing Parameters : TOD Average Export PF");//story 1024441 Add TOD Export PF
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "BillCumulativeMD", false, "Billing Parameters : Demand : Cumulative MD");
                                }
                                #endregion

                                #region Midnight
                                if (masterEntity.MidnightData != null && masterEntity.MidnightData.Count > 0 && !string.IsNullOrEmpty(masterEntity.MeterData.MeterID))
                                {
                                    // ////this.StatusMessage = "Please wait. Uploading Midnight Data";
                                    // ////Application.DoEvents();
                                    DLMS650MidnightDataBLL midnightDataBLL = new DLMS650MidnightDataBLL();
                                    MidnightParameterBLL midnightParameterBLL = new MidnightParameterBLL();
                                    List<IEntity> entities = new List<IEntity>();

                                    for (int counter = 0; counter < masterEntity.MidnightData.Count; counter++)
                                    {
                                        masterEntity.MidnightData[counter].MeterData_ID = masterEntity.MeterData.MeterData_ID;
                                        entities.Add(masterEntity.MidnightData[counter]);
                                    }
                                    midnightDataBLL.InsertData(entities);
                                    masterEntity.MidnightParameterColumns.MeterDataId = masterEntity.MeterData.MeterData_ID;
                                    midnightParameterBLL.InsertData(masterEntity.MidnightParameterColumns);
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "MidEne", true, "MidNight Energies");
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "DaiEneCon", true, "Daily Energy Consumption");
                                }
                                else
                                {
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "MidEne", false, "MidNight Energies");
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "DaiEneCon", false, "Daily Energy Consumption");
                                }
                                #endregion

                                #region Tamper
                                if (masterEntity.Tamper != null && masterEntity.Tamper.Count > 0 && !string.IsNullOrEmpty(masterEntity.MeterData.MeterID))
                                {
                                    //////this.StatusMessage = "Please wait. Uploading Tamper Data";
                                    //////Application.DoEvents();
                                    DLMS650TamperMasterBLL tamperBLL = new DLMS650TamperMasterBLL();
                                    TamperParameterBLL tamperParameterBLL = new TamperParameterBLL();
                                    List<IEntity> entities = new List<IEntity>();
                                    for (int counter = 0; counter < masterEntity.Tamper.Count; counter++)
                                    {
                                        masterEntity.Tamper[counter].MeterData_ID = masterEntity.MeterData.MeterData_ID;
                                        entities.Add(masterEntity.Tamper[counter]);
                                    }

                                    // SB Code Change Start - 20171115
                                   this.CalculateActiveCurrent(masterEntity.MeterData.MeterData_ID, ref masterEntity, ref entities);
                                    // SB Code Change End - 20171115

                                    tamperBLL.InsertData(entities);
                                    masterEntity.TamperParameterColumns.MeterDataId = masterEntity.MeterData.MeterData_ID;

                                    
                                    //SarkarA code change start 20180110 // limit unnecessary parameters for 1P //20180420 limit parameters for smart model 34, 35, 36, 37 //reenabled on 20180613
                                    if (!(masterEntity.General.Metertype.Equals(CAB.Framework.MeterType.OnePhaseTwoWire) // ||
                                        //masterEntity.General.MeterModelNo.Equals(CAB.Framework.NamePlateConstants.SmartM_Cipher_1PH.ToString()) ||
                                        //masterEntity.General.MeterModelNo.Equals(CAB.Framework.NamePlateConstants.SmartM_Cipher_HTCT.ToString()) ||
                                        //masterEntity.General.MeterModelNo.Equals(CAB.Framework.NamePlateConstants.SmartM_Cipher_LTCT.ToString()) ||
                                       // masterEntity.General.MeterModelNo.Equals(CAB.Framework.NamePlateConstants.SmartM_Cipher_WCM.ToString())
                                        ))
                                    {
                                        if (!(masterEntity.General.MeterModelNo.Equals(CAB.Framework.NamePlateConstants.SmartM_Cipher_1PH.ToString()) ||
                                        masterEntity.General.MeterModelNo.Equals(CAB.Framework.NamePlateConstants.SmartM_Cipher_HTCT.ToString()) ||
                                        masterEntity.General.MeterModelNo.Equals(CAB.Framework.NamePlateConstants.SmartM_Cipher_LTCT.ToString()) ||
                                        masterEntity.General.MeterModelNo.Equals(CAB.Framework.NamePlateConstants.SmartM_Cipher_WCM.ToString())))
                                        {
                                            //add pradipta_neu
                                            masterEntity.TamperParameterColumns.ColumnsNames += ",kWr";//add pradipta_neu
                                            masterEntity.TamperParameterColumns.ColumnsNames += ",kWy";//add pradipta_neu
                                            masterEntity.TamperParameterColumns.ColumnsNames += ",kWb";//add pradipta_neu

                                            masterEntity.TamperParameterColumns.ColumnsNames += ",kVAr";//add pradipta_neu
                                            masterEntity.TamperParameterColumns.ColumnsNames += ",kVAy";//add pradipta_neu
                                            masterEntity.TamperParameterColumns.ColumnsNames += ",kVAb";//add pradipta_neu
                                        }

                                        // SB Code Change Start - 20171116  
                                        masterEntity.TamperParameterColumns.ColumnsNames += ",ActiveCurrentR";
                                        masterEntity.TamperParameterColumns.ColumnsNames += ",ActiveCurrentY";
                                        masterEntity.TamperParameterColumns.ColumnsNames += ",ActiveCurrentB";
                                        
                                        // SB Code Change End - 20171116
                                    }
                                    //SarkarA code change end 20180110

                                    tamperParameterBLL.InsertData(masterEntity.TamperParameterColumns);
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "Tam", true, "Tamper");
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "Tra", true, "Transaction");
                                }
                                else
                                {
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "Tam", false, "Tamper");
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "Tra", false, "Transaction");
                                }
                                #endregion

                                #region Phasor
                                if (masterEntity.Phasor != null && !string.IsNullOrEmpty(masterEntity.MeterData.MeterID))
                                {

                                    masterEntity.Phasor.MeterDataId = masterEntity.MeterData.MeterData_ID;
                                    new DLMS650PhasorBLL().InsertData(masterEntity.Phasor);
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "Pha", true, "Phasor");
                                }
                                else
                                {
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "Pha", false, "Phasor");
                                }
                                #endregion

                                #region Anomaly
                                if (masterEntity.Anomaly != null && !string.IsNullOrEmpty(masterEntity.MeterData.MeterID))
                                {
                                    masterEntity.Anomaly.MeterDataId = masterEntity.MeterData.MeterData_ID;
                                    new AnomalyBLL().InsertData(masterEntity.Anomaly);
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "InsSel", true, "Instant : Self Diagnostics");
                                }
                                else
                                {
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "InsSel", false, "Instant : Self Diagnostics");
                                }
                                #endregion

                                #region FraudEnergy
                                if (masterEntity.FraudEnergy != null && !string.IsNullOrEmpty(masterEntity.MeterData.MeterID))
                                {
                                    masterEntity.FraudEnergy.MeterData_ID = masterEntity.MeterData.MeterData_ID;
                                    new FraudEnergyBLL().InsertData(masterEntity.FraudEnergy);
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "FraEne", true, "Fraud Energy");
                                }
                                else
                                {
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "FraEne", false, "Fraud Energy");
                                }
                                #endregion

                                #region Meter configuration

                                #region MDWithIP
                                if (masterEntity.MeterConfigurations.mdWithIPEntity != null && !string.IsNullOrEmpty(masterEntity.MeterData.MeterID))
                                {
                                    masterEntity.MeterConfigurations.mdWithIPEntity.MeterDataID = masterEntity.MeterData.MeterData_ID;
                                    new MDWithIPBLL().InsertData(masterEntity.MeterConfigurations.mdWithIPEntity);
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "MDWithIP", true, "Meter Configuration : MDWithIP");
                                    isMeterConfigurationAvailable = true;
                                }
                                else
                                {
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "MDWithIP", false, "Meter Configuration : MDWithIP");

                                }
                                #endregion

                                #region DIP
                                if (masterEntity.MeterConfigurations.DIPEntity != null && !string.IsNullOrEmpty(masterEntity.MeterData.MeterID))
                                {
                                    masterEntity.MeterConfigurations.DIPEntity.MeterDataID = masterEntity.MeterData.MeterData_ID;
                                    new DIPBLL().InsertData(masterEntity.MeterConfigurations.DIPEntity);
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "DIP", true, "Meter Configuration : DIP");
                                    isMeterConfigurationAvailable = true;
                                }
                                else
                                {
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "DIP", false, "Meter Configuration : DIP");

                                }
                                #endregion

                                #region KvahSelection
                                if (masterEntity.MeterConfigurations.kvarselectionEntity != null && !string.IsNullOrEmpty(masterEntity.MeterData.MeterID))
                                {
                                    masterEntity.MeterConfigurations.kvarselectionEntity.MeterDataID = masterEntity.MeterData.MeterData_ID;
                                    new kvarSelectionBLL().InsertData(masterEntity.MeterConfigurations.kvarselectionEntity);
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "KvarSel", true, "Meter Configuration : Kvar Selection");
                                    isMeterConfigurationAvailable = true;
                                }
                                else
                                {
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "KvarSel", false, "Meter Configuration : Kvar Selection");

                                }
                                #endregion

                                #region RS232
                                if (masterEntity.MeterConfigurations.RS232Entity != null && !string.IsNullOrEmpty(masterEntity.MeterData.MeterID))
                                {
                                    masterEntity.MeterConfigurations.RS232Entity.MeterDataID = masterEntity.MeterData.MeterData_ID;
                                    new RS232BLL().Insertdata(masterEntity.MeterConfigurations.RS232Entity);
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "RS232", true, "Meter Configuration : RS232");
                                    isMeterConfigurationAvailable = true;
                                }
                                else
                                {
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "RS232", false, "Meter Configuration : RS232");

                                }
                                #endregion

                                #region BillingType
                                if (masterEntity.MeterConfigurations.billingTypeEntity != null && !string.IsNullOrEmpty(masterEntity.MeterData.MeterID))
                                {
                                    masterEntity.MeterConfigurations.billingTypeEntity.MeterDataID = masterEntity.MeterData.MeterData_ID;
                                    new BillingTypeBLL().Insertdata(masterEntity.MeterConfigurations.billingTypeEntity);
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "BillTyp", true, "Meter Configuration : Billing Type");
                                    isMeterConfigurationAvailable = true;
                                }
                                else
                                {
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "BillTyp", false, "Meter Configuration : Billing Type");

                                }
                                #endregion

                                #region RTC
                                if (masterEntity.MeterConfigurations.rtcEnity != null && !string.IsNullOrEmpty(masterEntity.MeterData.MeterID))
                                {
                                    masterEntity.MeterConfigurations.rtcEnity.MeterDataID = masterEntity.MeterData.MeterData_ID;
                                    new RTCBLL().InsertData(masterEntity.MeterConfigurations.rtcEnity);
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "RTC", true, "Meter Configuration : RTC");
                                    isMeterConfigurationAvailable = true;
                                }
                                else
                                {
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "RTC", false, "Meter Configuration : RTC");
                                }
                                #endregion

                                #region LSIP
                                if (masterEntity.MeterConfigurations.LSIPEntity != null && !string.IsNullOrEmpty(masterEntity.MeterData.MeterID))
                                {
                                    masterEntity.MeterConfigurations.LSIPEntity.MeterDataID = masterEntity.MeterData.MeterData_ID;
                                    new LSIPBLL().InsertData(masterEntity.MeterConfigurations.LSIPEntity);
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "LSIP", true, "Meter Configuration : LSIP");
                                    isMeterConfigurationAvailable = true;
                                }
                                else
                                {
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "LSIP", false, "Meter Configuration : LSIP");
                                }
                                #endregion

                                #region AutoLock
                                if (masterEntity.MeterConfigurations.AutoLockEntity != null && !string.IsNullOrEmpty(masterEntity.MeterData.MeterID))
                                {
                                    masterEntity.MeterConfigurations.AutoLockEntity.MeterDataID = masterEntity.MeterData.MeterData_ID;
                                    new AutoLockBLL().Insertdata(masterEntity.MeterConfigurations.AutoLockEntity);
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "AutoLck", true, "Meter Configuration : Auto Lock");
                                    isMeterConfigurationAvailable = true;
                                }
                                else
                                {
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "AutoLck", false, "Meter Configuration : Auto Lock");

                                }
                                #endregion

                                #region DailyLog
                                if (masterEntity.MeterConfigurations.dailyLogEntity != null && !string.IsNullOrEmpty(masterEntity.MeterData.MeterID))
                                {
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "DaiLog", true, "Meter Configuration : Daily Log");
                                    isMeterConfigurationAvailable = true;
                                }
                                else
                                {
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "DaiLog", false, "Meter Configuration : Daily Log");
                                }
                                #endregion

                                #region TOD
                                if (masterEntity.MeterConfigurations.TODEntity != null && !string.IsNullOrEmpty(masterEntity.MeterData.MeterID))
                                {
                                    masterEntity.MeterConfigurations.TODEntity.MeterDataID = masterEntity.MeterData.MeterData_ID;
                                    new TodBLL().InsertData(masterEntity.MeterConfigurations.TODEntity);
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "TOD", true, "Meter Configuration : TOD");
                                    isMeterConfigurationAvailable = true;
                                }
                                else
                                {
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "TOD", false, "Meter Configuration : TOD");
                                }
                                #endregion

                                #region Display Parameter
                                if (masterEntity.MeterConfigurations.DisplayParamater != null
                                    && masterEntity.MeterConfigurations.DisplayParamater.Count > 0 && !string.IsNullOrEmpty(masterEntity.MeterData.MeterID))
                                {
                                    new DisplayParameterBLL().InsertData(masterEntity.MeterConfigurations.DisplayParamater, masterEntity.MeterData.MeterData_ID);
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "DspPar", true, "Meter Configuration : Display Parameters");
                                    isMeterConfigurationAvailable = true;
                                }
                                else
                                {
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "DspPar", false, "Meter Configuration : Display Parameters");

                                }
                                #endregion

                                #region Manual Billing
                                if (masterEntity.MeterConfigurations.ManualBillingEntity != null && !string.IsNullOrEmpty(masterEntity.MeterData.MeterID))
                                {
                                    masterEntity.MeterConfigurations.ManualBillingEntity.MeterDataID = masterEntity.MeterData.MeterData_ID;
                                    new ManualBillingBLL().Insertdata(masterEntity.MeterConfigurations.ManualBillingEntity);
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "ManBil", true, "Meter Configuration : Manual Billing");
                                    isMeterConfigurationAvailable = true;
                                }
                                else
                                {
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "ManBil", false, "Meter Configuration : Manual Billing");

                                }
                                #endregion

                                #region Software Billing
                                if (masterEntity.MeterConfigurations.SoftwareBillingEntity != null && !string.IsNullOrEmpty(masterEntity.MeterData.MeterID))
                                {
                                    masterEntity.MeterConfigurations.SoftwareBillingEntity.MeterDataID = masterEntity.MeterData.MeterData_ID;
                                    new SoftwareBillingBLL().Insertdata(masterEntity.MeterConfigurations.SoftwareBillingEntity);
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "SofBil", true, "Meter Configuration : Software Billing");
                                    isMeterConfigurationAvailable = true;
                                }
                                else
                                {
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "SofBil", false, "Meter Configuration : Software Billing");

                                }
                                #endregion

                                #region CheckMeterConfigAvailable
                                if (isMeterConfigurationAvailable)
                                {
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "MtrCfg", true, "Meter Configuration ");

                                }
                                else
                                {
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "MtrCfg", false, "Meter Configuration ");

                                }
                                #endregion


                                #region DisconnectControl
                                if (masterEntity.MeterConfigurations.DisconnectControlEntity != null && !string.IsNullOrEmpty(masterEntity.MeterData.MeterID))
                                {
                                    masterEntity.MeterConfigurations.DisconnectControlEntity.MeterDataID = masterEntity.MeterData.MeterData_ID;
                                    new DisconnectControlBLL().InsertData(masterEntity.MeterConfigurations.DisconnectControlEntity);
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "DisCon", true, "Meter Configuration : Disconnect Control");
                                    isMeterConfigurationAvailable = true;
                                }
                                else
                                {
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "DisCon", false, "Meter Configuration : Disconnect Control");

                                }
                                #endregion

                                #region LoadControl
                                if (masterEntity.MeterConfigurations.LoadControlEntity != null && !string.IsNullOrEmpty(masterEntity.MeterData.MeterID))
                                {
                                    masterEntity.MeterConfigurations.LoadControlEntity.MeterDataID = masterEntity.MeterData.MeterData_ID;
                                    new LoadControlBLL().InsertData(masterEntity.MeterConfigurations.LoadControlEntity);
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "LoaCon", true, "Meter Configuration : Load Control");
                                    isMeterConfigurationAvailable = true;
                                }
                                else
                                {
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "LoaCon", false, "Meter Configuration : Load Control");
                                }

                                #endregion

                                #region RS485
                                if (masterEntity.MeterConfigurations.RS485Entity != null && !string.IsNullOrEmpty(masterEntity.MeterData.MeterID))
                                {
                                    masterEntity.MeterConfigurations.RS485Entity.MeterDataID = masterEntity.MeterData.MeterData_ID;
                                    new RS485BLL().InsertData(masterEntity.MeterConfigurations.RS485Entity);
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "RS485", true, "Meter Configuration : RS485");
                                    isMeterConfigurationAvailable = true;
                                }
                                else
                                {
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "RS485", false, "Meter Configuration : RS485");

                                }
                                #endregion

                                #region PaymentMode
                                if (masterEntity.MeterConfigurations.PaymentModeEntity!= null && !string.IsNullOrEmpty(masterEntity.MeterData.MeterID))
                                {
                                    masterEntity.MeterConfigurations.PaymentModeEntity.MeterDataID = masterEntity.MeterData.MeterData_ID;
                                    new PaymentModeBLL().InsertData(masterEntity.MeterConfigurations.PaymentModeEntity);
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "PaymentMode", true, "Meter Configuration : PaymentMode");
                                    isMeterConfigurationAvailable = true;
                                }
                                else
                                {
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "PaymentMode", false, "Meter Configuration : PaymentMode");

                                }
                                #endregion

                                #region MeteringMode
                                if (masterEntity.MeterConfigurations.MeteringModeEntity!= null && !string.IsNullOrEmpty(masterEntity.MeterData.MeterID))
                                {
                                    masterEntity.MeterConfigurations.MeteringModeEntity.MeterDataID = masterEntity.MeterData.MeterData_ID;
                                    new MeteringModeBLL().InsertData(masterEntity.MeterConfigurations.MeteringModeEntity);
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "MeteringMode", true, "Meter Configuration : MeteringMode");
                                    isMeterConfigurationAvailable = true;
                                }
                                else
                                {
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "MeteringMode", false, "Meter Configuration : MeteringMode");

                                }
                                #endregion

                                #region LoadLimit
                                if (masterEntity.MeterConfigurations.LoadLimitEntity != null && !string.IsNullOrEmpty(masterEntity.MeterData.MeterID))
                                {
                                    masterEntity.MeterConfigurations.LoadLimitEntity.MeterDataID = masterEntity.MeterData.MeterData_ID;
                                    new LoadLimitBLL().InsertData(masterEntity.MeterConfigurations.LoadLimitEntity);
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "LoadLimit", true, "Meter Configuration : LoadLimit");
                                    isMeterConfigurationAvailable = true;
                                }
                                else
                                {
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "LoadLimit", false, "Meter Configuration : LoadLimit");

                                }
                                #endregion

                                #region SlidingDemand
                                if (masterEntity.MeterConfigurations.SlidingDemandEntity != null && !string.IsNullOrEmpty(masterEntity.MeterData.MeterID))
                                {
                                    masterEntity.MeterConfigurations.SlidingDemandEntity.MeterDataID = masterEntity.MeterData.MeterData_ID;
                                    new SlidingDemandBLL().InsertData(masterEntity.MeterConfigurations.SlidingDemandEntity);
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "SlidingDemand", true, "Meter Configuration : SlidingDemand");
                                    isMeterConfigurationAvailable = true;
                                }
                                else
                                {
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "SlidingDemand", false, "Meter Configuration : SlidingDemand");

                                }
                                #endregion

                                #region Optical lock
                                if (masterEntity.MeterConfigurations.OpticalLockEntity != null && !string.IsNullOrEmpty(masterEntity.MeterData.MeterID))
                                {
                                    masterEntity.MeterConfigurations.OpticalLockEntity.MeterDataID = masterEntity.MeterData.MeterData_ID;
                                    new OpticalLockUnlockBLL().InsertData(masterEntity.MeterConfigurations.OpticalLockEntity);
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "OpticalRJPortLock", true, "Meter Configuration : Port Config");
                                    isMeterConfigurationAvailable = true;
                                }
                                else
                                {
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "OpticalRJPortLock", false, "Meter Configuration : Port Config");

                                }
                                #endregion

                                #region RJlock
                                if (masterEntity.MeterConfigurations.RJLockEntity != null && !string.IsNullOrEmpty(masterEntity.MeterData.MeterID))
                                {
                                    masterEntity.MeterConfigurations.RJLockEntity.MeterDataID = masterEntity.MeterData.MeterData_ID;
                                    new RJLockUnlockBLL().InsertData(masterEntity.MeterConfigurations.RJLockEntity);
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "OpticalRJPortLock", true, "Meter Configuration : Port Config");
                                    isMeterConfigurationAvailable = true;
                                }
                                else
                                {
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "OpticalRJPortLock", false, "Meter Configuration : Port Config");

                                }
                                #endregion

                                #region LoadSwitch

                                if (masterEntity.LoadSwitch != null && !string.IsNullOrEmpty(masterEntity.MeterData.MeterID))
                                {

                                    LoadSwitchBLL loadSwitchBLL = new LoadSwitchBLL();

                                    List<IEntity> entities = new List<IEntity>();

                                    for (int counter = 0; counter < masterEntity.LoadSwitch.Count; counter++)
                                    {
                                        masterEntity.LoadSwitch[counter].MeterData_ID = masterEntity.MeterData.MeterData_ID;
                                        entities.Add(masterEntity.LoadSwitch[counter]);
                                    }
                                    loadSwitchBLL.InsertData(entities);
                                    masterEntity.LoadSwitchParameterColumns.MeterDataId = masterEntity.MeterData.MeterData_ID;
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "LoadSwitch", true, "Load Switch");
                                }
                                else
                                {
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "LoadSwitch", false, "Load Switch");
                                }



                                #endregion

                                #region PulseEnergy
                                if (masterEntity.MeterConfigurations.PulseEnergyEntity != null && !string.IsNullOrEmpty(masterEntity.MeterData.MeterID))
                                {
                                    masterEntity.MeterConfigurations.PulseEnergyEntity.MeterDataID = masterEntity.MeterData.MeterData_ID;
                                    new PulseEnergyBLL().InsertData(masterEntity.MeterConfigurations.PulseEnergyEntity);
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "PulseEnergy", true, "Meter Configuration : PulseEnergy");
                                    isMeterConfigurationAvailable = true;
                                }
                                else
                                {
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "PulseEnergy", false, "Meter Configuration : PulseEnergy");
                                }
                                #endregion
                                #region ManualButtonMDReset
                                if (masterEntity.MeterConfigurations.ManualButtonMDResetEntity != null && !string.IsNullOrEmpty(masterEntity.MeterData.MeterID))
                                {
                                    masterEntity.MeterConfigurations.ManualButtonMDResetEntity.MeterDataID = masterEntity.MeterData.MeterData_ID;
                                    new ManualMDResetBLL().Insertdata(masterEntity.MeterConfigurations.ManualButtonMDResetEntity);
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "ManualMDReset", true, "Meter Configuration : Manual Button MD Reset Entity");
                                    isMeterConfigurationAvailable = true;
                                }
                                else
                                {
                                    tabNameBll.InsertIntoTabName(masterEntity.MeterData.MeterData_ID, "ManualMDReset", false, "Meter Configuration : Manual Button MD Reset Entity");

                                }
                                #endregion



                                #endregion
                            }
                            catch (Exception ex)    //SarkarA //Exception log for catch block
                            {
                                throw;
                            }
                        }
                    }
                }
                                              
                
            }
            catch (Exception ex)    //SarkarA //Exception log for catch block
            {
                //EventLogging.CallLogDetails("While uploading " + ex.Message);
                logger.Log(LOGLEVELS.Error, "Upload2NGFile(string fileName, string fileText, bool flag, out string message, string cmriID)", ex);
                fileUploadMasterBLL.DeleteData(fileUploadMasterEntity);
                message = "File corrupted.";
                ////Application.DoEvents();
                isUploaded = false;
            }
            return isUploaded;
        }

        // SB Code Change Start - 20171115
        /// <summary>
        /// Calculates Active current and instert onto the DB.
        /// </summary>
        /// <param name="instantEntity"></param>
        /// <param name="meterDataId"></param>
        /// <param name="entities"></param>
        private void CalculateActiveCurrent(long meterDataId, ref BillingGeneralNFDLMSEntity masterEntity, ref List<IEntity> entities)
        {
            try
            {
                // Calculate Active Current R
                double dPowerFactorR = 0.0;
                double dMeterCurrentR = 0.0;
                double dPowerFactorY = 0.0;
                double dMeterCurrentY = 0.0;
                double dPowerFactorB = 0.0;
                double dMeterCurrentB = 0.0;
                //----------------If data received from meter then no needs to calculate-----------
                for (int i = 0; i < masterEntity.Tamper.Count; i++)
                {
                    if (masterEntity.Tamper[i].ActiveCurrentR != null) {   return; }
                }
                for (int i = 0; i < masterEntity.Tamper.Count; i++)
                {
                    if (masterEntity.Tamper[i].PowerFactorRphase != null && masterEntity.Tamper[i].CurrentIR != null)
                    {
                        double.TryParse(masterEntity.Tamper[i].PowerFactorRphase.Split('*')[0], out dPowerFactorR);
                        double.TryParse(masterEntity.Tamper[i].CurrentIR.Split('*')[0], out dMeterCurrentR);
                        masterEntity.Tamper[i].ActiveCurrentR = (dMeterCurrentR * dPowerFactorR).ToString("#.000");
                    }

                    if (masterEntity.Tamper[i].PowerFactorYphase != null && masterEntity.Tamper[i].CurrentIY != null)
                    {
                        double.TryParse(masterEntity.Tamper[i].PowerFactorYphase.Split('*')[0], out dPowerFactorY);
                        double.TryParse(masterEntity.Tamper[i].CurrentIY.Split('*')[0], out dMeterCurrentY);
                        masterEntity.Tamper[i].ActiveCurrentY = (dMeterCurrentY * dPowerFactorY).ToString("#.000");
                    }

                    if (masterEntity.Tamper[i].PowerFactorBphase != null && masterEntity.Tamper[i].CurrentIB != null)
                    {
                        double.TryParse(masterEntity.Tamper[i].PowerFactorBphase.Split('*')[0], out dPowerFactorB);
                        double.TryParse(masterEntity.Tamper[i].CurrentIB.Split('*')[0], out dMeterCurrentB);
                        masterEntity.Tamper[i].ActiveCurrentB = (dMeterCurrentB * dPowerFactorB).ToString("#.000");
                    }
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Calculate Reactive current and instert intop DB.
        /// </summary>
        /// <param name="instantEntity">Instance of instant entity.</param>
        /// <param name="meterDataId">Meter data ID.</param>
        /// <param name="entities">Reference of list of entities.</param>
        private void CalculateReactiveCurrent(List<DLMS650InstantaneousEntity> instantEntity, long meterDataId, ref List<IEntity> entities)
        {
            try
            {
                bool bPhaseWiseCurrentExists = false;
                bool bPhaseWisePFExists = false;
                // Calculate Active Current R
                double dPowerFactorR = 0.0;
                double dMeterCurrentR = 0.0;
                for (int i = 0; i < instantEntity.Count; i++)
                {
                    if (instantEntity[i].InstantPowerColumnName.Trim().ToLower().Contains("signed power factor - r phase"))
                    {
                        double.TryParse(instantEntity[i].InstantPowerColumnValue.ToString().Split('*')[0], out dPowerFactorR);
                        bPhaseWisePFExists = true;
                        continue;
                    }
                    if (instantEntity[i].InstantPowerColumnName.Trim().ToLower() == "current - ir")
                    {
                        double.TryParse(instantEntity[i].InstantPowerColumnValue.ToString().Split('*')[0], out dMeterCurrentR);
                        bPhaseWiseCurrentExists = true;
                        continue;
                    }
                }
              
                if ((bPhaseWisePFExists & bPhaseWiseCurrentExists) == true)
                {
                    double dReactiveCurrentR = dMeterCurrentR * (Math.Sin(Math.Acos(dPowerFactorR)));

                    // Add reactive current R row to the grid display
                    DLMS650InstantaneousEntity newEntityR = new DLMS650InstantaneousEntity();
                    newEntityR.InstantPowerColumnName = "Reactive Current - R";
                    newEntityR.InstantPowerColumnValue = dReactiveCurrentR.ToString("#.000") + "*A";//commet sahoo
                    newEntityR.InstantPowerObisCode = "1.0.31.7.129.255";
                    newEntityR.InstantPowerClassID = "3";
                    newEntityR.InstantPowerAttribute = "2";
                    newEntityR.MeterDataID = meterDataId;
                    //instantEntity.Add(newEntity);
                    entities.Add(newEntityR);
                }

                bPhaseWiseCurrentExists = false;
                bPhaseWisePFExists = false;

                // Calculate reactive Current Y
                double dPowerFactorY = 0.0;
                double dMeterCurrentY = 0.0;
                for (int i = 0; i < instantEntity.Count; i++)
                {
                    if (instantEntity[i].InstantPowerColumnName.Trim().ToLower().Contains("signed power factor - y phase"))
                    {
                        double.TryParse(instantEntity[i].InstantPowerColumnValue.ToString().Split('*')[0], out dPowerFactorY);
                        bPhaseWisePFExists = true;
                        continue;
                    }
                    if (instantEntity[i].InstantPowerColumnName.Trim().ToLower() == "current - iy")
                    {
                        double.TryParse(instantEntity[i].InstantPowerColumnValue.ToString().Split('*')[0], out dMeterCurrentY);
                        bPhaseWiseCurrentExists = true;
                        continue;
                    }
                }

                if ((bPhaseWisePFExists & bPhaseWiseCurrentExists) == true)
                {
                    //   double dReactiveCurrentY = dMeterCurrentY * (Math.Sin(this.DegreeToRadian(Math.Acos(dPowerFactorY))));//chang by kheem
                    double dReactiveCurrentY = dMeterCurrentY * (Math.Sin(Math.Acos(dPowerFactorY)));

                    // Add reactive current Y row to the grid display
                    DLMS650InstantaneousEntity newEntityY = new DLMS650InstantaneousEntity();
                    newEntityY.InstantPowerColumnName = "Reactive Current - Y";
                    newEntityY.InstantPowerColumnValue = dReactiveCurrentY.ToString("#.000") + "*A";
                    newEntityY.InstantPowerObisCode = "1.0.51.7.129.255";
                    newEntityY.InstantPowerClassID = "3";
                    newEntityY.InstantPowerAttribute = "2";
                    newEntityY.MeterDataID = meterDataId;
                    //instantEntity.Add(newEntity);
                    entities.Add(newEntityY);
                }

                bPhaseWiseCurrentExists = false;
                bPhaseWisePFExists = false;

                // Calculate Reactive Current B
                double dPowerFactorB = 0.0;
                double dMeterCurrentB = 0.0;
                for (int i = 0; i < instantEntity.Count; i++)
                {
                    if (instantEntity[i].InstantPowerColumnName.Trim().ToLower().Contains("signed power factor - b phase"))
                    {
                        double.TryParse(instantEntity[i].InstantPowerColumnValue.ToString().Split('*')[0], out dPowerFactorB);
                        bPhaseWisePFExists = true;
                        continue;
                    }
                    if (instantEntity[i].InstantPowerColumnName.Trim().ToLower() == "current - ib")
                    {
                        double.TryParse(instantEntity[i].InstantPowerColumnValue.ToString().Split('*')[0], out dMeterCurrentB);
                        bPhaseWiseCurrentExists = true;
                        continue;
                    }
                }

                if ((bPhaseWisePFExists & bPhaseWiseCurrentExists) == true)
                {
                    //double dReactiveCurrentB = dMeterCurrentB * (Math.Sin(this.DegreeToRadian(Math.Acos(dPowerFactorB))));

                    double dReactiveCurrentB = dMeterCurrentB * (Math.Sin(Math.Acos(dPowerFactorB)));

                    // Add reactive current B row to the grid display
                    DLMS650InstantaneousEntity newEntityB = new DLMS650InstantaneousEntity();
                    newEntityB.InstantPowerColumnName = "Reactive Current - B";
                    newEntityB.InstantPowerColumnValue = dReactiveCurrentB.ToString("#.000") + "*A";
                    newEntityB.InstantPowerObisCode = "1.0.71.7.129.255";
                    newEntityB.InstantPowerClassID = "3";
                    newEntityB.InstantPowerAttribute = "2";
                    newEntityB.MeterDataID = meterDataId;
                    //instantEntity.Add(newEntity);
                    entities.Add(newEntityB);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }


        private void CalculateApparentPower(List<DLMS650InstantaneousEntity> instantEntity, long meterDataId, ref List<IEntity> entities)
        {
            try
            {
                bool rPhaseWiseCurrentExists = false;
                bool rPhaseWiseVoltageExists = false;
                //Calculate Appranent RPhase
                double dMeterVoltage = 0.0;
                double dMeterCurrent = 0.0;
               
                    for (int i = 0; i < instantEntity.Count; i++)
                    {
                        if (instantEntity[i].InstantPowerColumnName.Trim().ToLower().Contains("voltage - vrn"))
                        {
                            double.TryParse(instantEntity[i].InstantPowerColumnValue.ToString().Split('*')[0], out dMeterVoltage);
                            rPhaseWiseVoltageExists = true;
                            continue;
                        }
                        if (instantEntity[i].InstantPowerColumnName.Trim().ToLower() == "current - ir")
                        {
                            double.TryParse(instantEntity[i].InstantPowerColumnValue.ToString().Split('*')[0], out dMeterCurrent);
                            rPhaseWiseCurrentExists = true;
                            continue;
                        }
                    }

                    if ((rPhaseWiseVoltageExists & rPhaseWiseCurrentExists) == true)
                    {
                        double dApparentR = (dMeterVoltage * dMeterCurrent) / 1000;

                        //Add reactive current R row to the grid display
                        DLMS650InstantaneousEntity newEntityR = new DLMS650InstantaneousEntity();
                        newEntityR.InstantPowerColumnName = "R-Phase Apparent Power";
                        newEntityR.InstantPowerColumnValue = dApparentR.ToString("0.000") + "*kVA";//commet sahoo
                        newEntityR.InstantPowerObisCode = "1.0.23.7.0.255";
                        newEntityR.InstantPowerClassID = "3";
                        newEntityR.InstantPowerAttribute = "2";
                        newEntityR.MeterDataID = meterDataId;
                        instantEntity.Add(newEntityR);
                        entities.Add(newEntityR);
                    }

                    // Calculate YPhase Apparent Power
                    bool yPhaseWiseCurrentExists = false;
                    bool yPhaseWiseVoltageExists = false;
                    double yMeterVoltage = 0.0;
                    double yMeterCurrent = 0.0;


                    for (int i = 0; i < instantEntity.Count; i++)
                    {
                        if (instantEntity[i].InstantPowerColumnName.Trim().ToLower().Contains("voltage - vyn"))
                        {
                            double.TryParse(instantEntity[i].InstantPowerColumnValue.ToString().Split('*')[0], out yMeterVoltage);
                            yPhaseWiseVoltageExists = true;
                            continue;
                        }
                        if (instantEntity[i].InstantPowerColumnName.Trim().ToLower() == "current - iy")
                        {
                            double.TryParse(instantEntity[i].InstantPowerColumnValue.ToString().Split('*')[0], out yMeterCurrent);
                            yPhaseWiseCurrentExists = true;
                            continue;
                        }
                    }

                    if ((yPhaseWiseVoltageExists & yPhaseWiseCurrentExists) == true)
                    {
                        double dApparentY = (yMeterVoltage * yMeterCurrent) / 1000;

                        //Add reactive current R row to the grid display
                        DLMS650InstantaneousEntity newEntityY = new DLMS650InstantaneousEntity();
                        newEntityY.InstantPowerColumnName = "Y-Phase Apparent Power";
                        newEntityY.InstantPowerColumnValue = dApparentY.ToString("0.000") + "*kVA";//commet sahoo
                        newEntityY.InstantPowerObisCode = "1.0.43.7.0.255"; ;
                        newEntityY.InstantPowerClassID = "3";
                        newEntityY.InstantPowerAttribute = "2";
                        newEntityY.MeterDataID = meterDataId;
                        instantEntity.Add(newEntityY);
                        entities.Add(newEntityY);
                    }

                    // Calculate BPhase Apparent Power
                    bool bPhaseWiseCurrentExists = false;
                    bool bPhaseWiseVoltageExists = false;
                    double bMeterVoltage = 0.0;
                    double bMeterCurrent = 0.0;


                    for (int i = 0; i < instantEntity.Count; i++)
                    {
                        if (instantEntity[i].InstantPowerColumnName.Trim().ToLower().Contains("voltage - vbn"))
                        {
                            double.TryParse(instantEntity[i].InstantPowerColumnValue.ToString().Split('*')[0], out bMeterVoltage);
                            bPhaseWiseVoltageExists = true;
                            continue;
                        }
                        if (instantEntity[i].InstantPowerColumnName.Trim().ToLower() == "current - ib")
                        {
                            double.TryParse(instantEntity[i].InstantPowerColumnValue.ToString().Split('*')[0], out bMeterCurrent);
                            bPhaseWiseCurrentExists = true;
                            continue;
                        }
                    }

                    if ((bPhaseWiseVoltageExists & bPhaseWiseCurrentExists) == true)
                    {
                        double dApparentB = (bMeterVoltage * bMeterCurrent) / 1000;

                        //Add reactive current B row to the grid display
                        DLMS650InstantaneousEntity newEntityB = new DLMS650InstantaneousEntity();
                        newEntityB.InstantPowerColumnName = "B-Phase Apparent Power";
                        newEntityB.InstantPowerColumnValue = dApparentB.ToString("0.000") + "*kVA";//commet sahoo
                        newEntityB.InstantPowerObisCode = "1.0.63.7.0.255"; ;
                        newEntityB.InstantPowerClassID = "3";
                        newEntityB.InstantPowerAttribute = "2";
                        newEntityB.MeterDataID = meterDataId;
                        instantEntity.Add(newEntityB);
                        entities.Add(newEntityB);
                    }

                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       

        /// <summary>
        /// This method is used for getting Size of the file in KB.
        /// </summary>
        /// <param name="fileName">Pass the file name.</param>
        /// <returns>Returns Size in integer.</returns>
        private string GetFileSize(string fileText)
        {
            int length = ASCIIEncoding.ASCII.GetByteCount(fileText);

            string filesize = string.Empty;
            if (length < 1024)
                filesize = Convert.ToString(length) + " bytes";
            else
                filesize = Convert.ToString(length / 1024) + " KB";

            return filesize;
        }
        private string GetMeterModelType(string TextFile)
        {
            string MeterModelType = string.Empty;
            string[] lstLines = TextFile.Split('\n');
            CAB.Framework.Utility.CommonMethods.MeterDataType = MeterDataTypes.LTCT;
            foreach (string item in lstLines)
            {
                if (item.Contains("00006001BCFF") && item.Contains("7374"))
                {
                    MeterModelType = "st";
                    break;
                }
                if (item.Contains("00006001BCFF") && item.Contains("5354"))
                {
                    MeterModelType = "ST";
                    break;
                }
                if (item.Contains("00006001BCFF") && item.Contains("4653"))
                {
                    MeterModelType = "FS";
                    break;
                }
                if (item.Contains("534D30333130"))
                {

                    MeterModelType = "SM0310"; //Smart meter 3 ph WCM.
                    break;
                }
                if (item.Contains("534D30343035"))
                {

                    MeterModelType = "SM0405"; //Smart meter 3 ph LTCT.
                    break;
                }
                if (item.Contains("534D30313130"))
                {

                    MeterModelType = "SM0110"; //Smart meter 1 ph.
                    break;
                }

                if (item.Contains("00006001BCFF") && item.ToUpperInvariant().Contains("534D"))
                {
                    CAB.Framework.Utility.CommonMethods.MeterDataType = MeterDataTypes.HTCT_MEGA;
                    MeterModelType = "SM";
                    break;
                }
                if (item.Contains("00006001BCFF") && item.ToUpperInvariant().Contains("736D"))
                {
                    CAB.Framework.Utility.CommonMethods.MeterDataType = MeterDataTypes.HTCT_MEGA;
                    MeterModelType = "sm";
                    break;
                }
                if (item.Contains("00006001BCFF") && item.Contains("5348"))
                {
                    CAB.Framework.Utility.CommonMethods.MeterDataType = MeterDataTypes.HTCT_KILO;
                    MeterModelType = "SH";
                    break;

                }
                if (item.Contains("00006001BCFF") && item.Contains("7368"))
                {
                    CAB.Framework.Utility.CommonMethods.MeterDataType = MeterDataTypes.HTCT_KILO;
                    MeterModelType = "sh";
                    break;
                }

                //if (item.contains("00006001bcff") && item.contains("535053323031"))
                //{
                //    lng.framework.utility.commonmethods.meterdatatype = meterdatatypes.htct_kilo;
                //    metermodeltype = "sps201";
                //    break;
                //}
                //// Add Meter Model SM/sm for HTCT Mega Variant
                //if (item.Contains("00006001BCFF") && (item.Contains("534D") || item.Contains("736D")))
                //{
                //    CAB.Framework.Utility.CommonMethods.MeterDataType = MeterDataTypes.HTCT_MEGA;
                //    break;
                //}
            }

            return MeterModelType;
        }
       
        private List<ProfileData> GetParsedGetProfileWiseEntityList(string SignatureInfo, string fileText)
        {
            List<ProfileData> allData = null;
            switch (SignatureInfo)
            {
                case "FS":
                    allData = entityGenerator.GetProfileWiseEntityList(fileText, false, 23);
                    break;
                case "SM":
                    allData = entityGenerator.GetProfileWiseEntityList(fileText, false, 28);
                    break;
                case "sm":
                    allData = entityGenerator.GetProfileWiseEntityList(fileText, false, 29);
                    break;
                case "SH":
                    allData = entityGenerator.GetProfileWiseEntityList(fileText, false, 27);
                    break;
                case "sh":
                    allData = entityGenerator.GetProfileWiseEntityList(fileText, false, 30);
                    break;
                default:
                    allData = entityGenerator.GetProfileWiseEntityList(fileText, false);
                    break;
            }

            return allData;
        }


        /// <summary>
        /// Parses input data , creates several profile entity add all entity to master entity .
        /// </summary>
        /// <param name="fileText"></param>
        /// <param name="isUpload">Use to distingush CDF converter Entity</param>
        /// <returns></returns>
        public BillingGeneralNFDLMSEntity GetMasterEntity(string fileText, bool isUpload)
        {
            string MeterModelType = GetMeterModelType(fileText);
            string MeterModelNumber = string.Empty;
            string meterVariant = string.Empty;
            BillingGeneralNFDLMSEntity masterEntity = new BillingGeneralNFDLMSEntity();
            Collection<DisplayParamatersDBEntity> collDisplayParamatersDBEntity = new Collection<DisplayParamatersDBEntity>();
            BillingGeneralNFDLMSEntity entityToHoldTempData = new BillingGeneralNFDLMSEntity();
            List<ProfileData> generalData = new List<ProfileData>();
            List<ProfileData> namePlateData = new List<ProfileData>();
            List<ProfileData> instantData = new List<ProfileData>();
            List<ProfileData> loadSurveyData = new List<ProfileData>();
            List<ProfileData> midnightData = new List<ProfileData>();
            List<ProfileData> phasorData = new List<ProfileData>();
            List<ProfileData> fraudEnergyData = new List<ProfileData>();
            List<ProfileData> tamperData = new List<ProfileData>();
            List<ProfileData> billingData = new List<ProfileData>();
            List<ProfileData> rtcData = new List<ProfileData>();
            List<ProfileData> billingTypeData = new List<ProfileData>();
            List<ProfileData> billingMonthTypeData = new List<ProfileData>(); //[BillingType_Month]
            List<ProfileData> kvahSelectionData = new List<ProfileData>();
            List<ProfileData> rs232LockData = new List<ProfileData>();
            List<ProfileData> meterConfigurationData = new List<ProfileData>();
            List<ProfileData> dipData = new List<ProfileData>();
            List<ProfileData> resetLockOutData = new List<ProfileData>();
            List<ProfileData> pushDisplayParameterData = new List<ProfileData>();
            List<ProfileData> scrollDisplayParameterData = new List<ProfileData>();
            List<ProfileData> highResolutionDisplayParameterData = new List<ProfileData>();
            List<ProfileData> displayTimeoutData = new List<ProfileData>();
            List<ProfileData> autoLockData = new List<ProfileData>();
            List<ProfileData> passiveSeasonProfile = new List<ProfileData>();
            List<ProfileData> passiveWeekProfile = new List<ProfileData>();
            List<ProfileData> passiveDayProfile = new List<ProfileData>();
            List<ProfileData> activeSeasonDayProfile = new List<ProfileData>();
            List<ProfileData> activeWeekDayProfile = new List<ProfileData>();
            List<ProfileData> activeDayProfile = new List<ProfileData>();
            List<ProfileData> activationDate = new List<ProfileData>();
            List<ProfileData> specialDayProfile = new List<ProfileData>();
            List<ProfileData> transactionData = new List<ProfileData>();
            List<ProfileData> DailyLogData = new List<ProfileData>();
            List<ProfileData> CTRatioData = new List<ProfileData>();
            List<ProfileData> PTRatioData = new List<ProfileData>();
            List<ProfileData> lsipData = new List<ProfileData>();
            List<ProfileData> anamolyData = new List<ProfileData>();
            List<ProfileData> dipWithIPData = new List<ProfileData>();
            List<ProfileData> manualBillingData = new List<ProfileData>();
            List<ProfileData> softwareBillingData = new List<ProfileData>();
            List<ProfileData> disconnectControlData = new List<ProfileData>();
            List<ProfileData> loadControlData = new List<ProfileData>();
            List<ProfileData> loadControl1PSmartData = new List<ProfileData>();
            List<ProfileData> RS485Data = new List<ProfileData>();

            List<ProfileData> PaymentModeData = new List<ProfileData>();
            List<ProfileData> MeteringModeData = new List<ProfileData>();
            List<ProfileData> LoadLimitData = new List<ProfileData>();
            List<ProfileData> SlidingDemandData = new List<ProfileData>();
            List<ProfileData> OpticalData = new List<ProfileData>();
            List<ProfileData> RJData = new List<ProfileData>();
            List<ProfileData> LoadSwitchData = new List<ProfileData>();
            List<ProfileData> pulseEnergyData = new List<ProfileData>();
            List<ProfileData> ManualMDResetData = new List<ProfileData>();


            LoadSwitch mapperLoadSwitch = new LoadSwitch();         
            General mapperGeneral = new General();
            Instantaneous mapperInstant = new Instantaneous();
            LoadSurvey mapperLoadSurevy = new LoadSurvey();
            DailyLoadProfile mapperMidnight = new DailyLoadProfile();
            BillingProfile mapperBilling = new BillingProfile();
            Phasor mapperPhasor = new Phasor();
            Tamper mapperTamper = new Tamper();
            KVAHSelection mapperKvar = new KVAHSelection();
            RS232LockUnlock mapperRS232 = new RS232LockUnlock();
            BillingDateTime mapperBillingType = new BillingDateTime();
            RealTimeClock mapperRTC = new RealTimeClock();
            AutoLock mapperAutoLock = new AutoLock();
            Anamoly mapperAnamoly = new Anamoly();
            DisplayParameterAndTimeout mapperDisplayParameter = null;
            //if (MeterModelType == "ST")
            //{
            //    mapperDisplayParameter = new DisplayParameterAndTimeout(MeterModelType);
            //}
            //else
            //{
            //    //mapperDisplayParameter = new DisplayParameterAndTimeout("");
            //    mapperDisplayParameter = new DisplayParameterAndTimeout(MeterModelType);
            //}

            LSCapturePeriod mapperLSCapturePeriod = new LSCapturePeriod();
            DemandIntegrationPeriod mapperDIP = new DemandIntegrationPeriod();
            LoadLimitMapper mapperLoadlimit = new LoadLimitMapper();
            MDWithIp mapperDIPWithIP = new MDWithIp();
            ManualBilling mapperManualBilling = new ManualBilling();
            SoftwareBilling mapperSoftwareBilling = new SoftwareBilling();
            PulseEnergy mapperPulseEnergy = new PulseEnergy();
            ManualMDReset mappermanualmdreset = new ManualMDReset();
            // Specific Check for L&T meters
            // Date : 24-July-17
            // Done By: Mohsin Raza

            fileText = fileText.ToUpper().Replace("010000600100FF02090C", "010000600100FF02090A"); 

            List<ProfileData> allData = GetParsedGetProfileWiseEntityList(MeterModelType,fileText);



            billingData = GetSingleProfileData(allData, (int)ProfileId.Billing);
            generalData = GetSingleProfileData(allData, (int)ProfileId.NamePlate);
            namePlateData = GetSingleProfileData(allData, (int)ProfileId.NamePlateProfile);
            tamperData = GetSingleProfileData(allData, (int)ProfileId.Tamper);
            instantData = GetSingleProfileData(allData, (int)ProfileId.Instant);
            loadSurveyData = GetSingleProfileData(allData, (int)ProfileId.LoadSurvey);
            midnightData = GetSingleProfileData(allData, (int)ProfileId.Midnight);
            phasorData = GetSingleProfileData(allData, (int)ProfileId.Phasor);
            fraudEnergyData = GetSingleProfileData(allData, (int)ProfileId.FraudEnergy);
            anamolyData = GetSingleProfileData(allData, (int)ProfileId.Anomaly);
            kvahSelectionData = GetSingleProfileData(allData, (int)ProfileId.KvahSelection);
            rs232LockData = GetSingleProfileData(allData, (int)ProfileId.RS232LockUnlock);
            billingTypeData = GetSingleProfileData(allData, (int)ProfileId.BillingType);
            billingMonthTypeData = GetSingleProfileData(allData, (int)ProfileId.BillingMonthType); // [BillingType_Month]
            pushDisplayParameterData = GetSingleProfileData(allData, (int)ProfileId.PushDisplayParameter);
            scrollDisplayParameterData = GetSingleProfileData(allData, (int)ProfileId.ScrollDisplyParameter);
            highResolutionDisplayParameterData = GetSingleProfileData(allData, (int)ProfileId.HighResolutionDisplayParameter);
            displayTimeoutData = GetSingleProfileData(allData, (int)ProfileId.DisplayTimeoutParameter);
            dipData = GetSingleProfileData(allData, (int)ProfileId.DIP);
            resetLockOutData = GetSingleProfileData(allData, (int)ProfileId.ResetLockOutDays);
            autoLockData = GetSingleProfileData(allData, (int)ProfileId.AutoLock);
            passiveSeasonProfile = GetSingleProfileData(allData, (int)ProfileId.PassiveSeasonProfile);
            passiveWeekProfile = GetSingleProfileData(allData, (int)ProfileId.PassiveWeekProfile);
            passiveDayProfile = GetSingleProfileData(allData, (int)ProfileId.PassiveDayProfile);
            activeDayProfile = GetSingleProfileData(allData, (int)ProfileId.ActiveDayProfile);
            specialDayProfile = GetSingleProfileData(allData, (int)ProfileId.SpecialDayProfileSmartMeter);
            activeSeasonDayProfile = GetSingleProfileData(allData, (int)ProfileId.ActiveSeasonProfile);
            activeWeekDayProfile = GetSingleProfileData(allData, (int)ProfileId.ActiveWeekProfile);
            activationDate = GetSingleProfileData(allData, (int)ProfileId.ActivationDate);
            rtcData = GetSingleProfileData(allData, (int)ProfileId.RTC);
            CTRatioData = GetSingleProfileData(allData, (int)ProfileId.CTRatio);
            PTRatioData = GetSingleProfileData(allData, (int)ProfileId.PTRatio);
            lsipData = GetSingleProfileData(allData, (int)ProfileId.SIP);
            dipWithIPData = GetSingleProfileData(allData, (int)ProfileId.DIPWithSliding);
            manualBillingData = GetSingleProfileData(allData, (int)ProfileId.ManualBilling);
            softwareBillingData = GetSingleProfileData(allData, (int)ProfileId.SoftwareBilling);
            disconnectControlData = GetSingleProfileData(allData, (int)ProfileId.DisconnectControl);
            loadControlData = GetSingleProfileData(allData, (int)ProfileId.LoadControl);
            loadControl1PSmartData = GetSingleProfileData(allData, (int)ProfileId.LoadControl1PSmartMeter);
            RS485Data = GetSingleProfileData(allData, (int)ProfileId.RS485);
            PaymentModeData = GetSingleProfileData(allData, (int)ProfileId.Paymentmode);
            MeteringModeData = GetSingleProfileData(allData, (int)ProfileId.Meteringmode);
            LoadLimitData = GetSingleProfileData(allData, (int)ProfileId.LoadLimit);
            SlidingDemandData = GetSingleProfileData(allData, (int)ProfileId.Slidingdemand);
            OpticalData = GetSingleProfileData(allData, (int)ProfileId.OpticalLockUnlock);
            RJData = GetSingleProfileData(allData, (int)ProfileId.RJLockUnlock);
            LoadSwitchData = GetSingleProfileData(allData, (int)ProfileId.LoadSwitch);
            pulseEnergyData = GetSingleProfileData(allData, (int)ProfileId.PulseEnergy);
            ManualMDResetData = GetSingleProfileData(allData, (int)ProfileId.ManualButtonMDReset);

            if (generalData != null && generalData.Count > 0)
            {
                masterEntity.General = mapperGeneral.GetMappedEntity(generalData);
                masterEntity.MeterData = new MeterDataEntity();
                masterEntity.MeterData.MeterID = (masterEntity.General.MeterSerialNumber).Trim();
                MeterModelNumber = masterEntity.General.MeterModelNo;
                meterVariant = masterEntity.General.NetMeterVariantInfo;
                ConfigInfo.MeterModel = masterEntity.General.MeterModelNo;
            }

            mapperDisplayParameter = new DisplayParameterAndTimeout(MeterModelType);

            if (namePlateData != null && namePlateData.Count > 0)
            {
                masterEntity.NamePlateProfile = mapperGeneral.GetMappedEntity(namePlateData);
            }
            if (instantData != null && instantData.Count > 0)
            {
                masterEntity.Instant = mapperInstant.GetMappedEntity(instantData, masterEntity.General);
            }


            if (anamolyData != null && anamolyData.Count > 0)
            {
                masterEntity.Anomaly = mapperAnamoly.GetMappedEntity(anamolyData, masterEntity.General);
            }


            if (loadSurveyData != null && loadSurveyData.Count > 0)
            {
                try
                {
                    entityToHoldTempData = mapperLoadSurevy.GetMappedEntity(loadSurveyData, meterVariant);
                    masterEntity.LoadSurvey = entityToHoldTempData.LoadSurvey;
                    masterEntity.LSParameterColumns = entityToHoldTempData.LSParameterColumns;
                }
                catch (Exception ex)    //SarkarA //Exception log for catch block
                {
                    logger.Log(LOGLEVELS.Error, "GetMasterEntity(string fileText, bool isUpload)", ex);

                }
            }

            if (billingData != null && billingData.Count > 0)
            {
                entityToHoldTempData = mapperBilling.GetMappedEntity(billingData, masterEntity.General);
                masterEntity.Billing = entityToHoldTempData.Billing;
                masterEntity.BillingParameterColumns = entityToHoldTempData.BillingParameterColumns;
            }
            if (midnightData != null && midnightData.Count > 0)
            {
                entityToHoldTempData = mapperMidnight.GetMappedEntity(midnightData, meterVariant);
                masterEntity.MidnightData = entityToHoldTempData.MidnightData;
                masterEntity.MidnightParameterColumns = entityToHoldTempData.MidnightParameterColumns;
            }

            if (tamperData != null && tamperData.Count > 0)
            {
                entityToHoldTempData = mapperTamper.GetMappedEntity(tamperData);
                masterEntity.Tamper = entityToHoldTempData.Tamper;
                masterEntity.TamperParameterColumns = entityToHoldTempData.TamperParameterColumns;
            }

            if (phasorData != null && phasorData.Count > 0 && phasorData[0].ListMeterDataPacket.Count > 0)
            {

                masterEntity.Phasor = mapperPhasor.GetMappedEntity(phasorData);
            }

            #region MeterConfigurations
            masterEntity.MeterConfigurations = new MeterConfigurationsNFEntity();
            if (isUpload == true)
            {
                if (passiveDayProfile.Count > 0 && passiveSeasonProfile.Count > 0 && passiveWeekProfile.Count > 0
                 && activationDate.Count > 0 && activeWeekDayProfile.Count > 0 && activeSeasonDayProfile.Count > 0
                 && activeDayProfile.Count > 0 && specialDayProfile.Count > 0)
                {
                    masterEntity.MeterConfigurations.TODEntity = new TODEntity();
                    masterEntity.MeterConfigurations.TODEntity.TODData = "DLMS" + @"\" + passiveSeasonProfile[0].ListMeterDataPacket[0].ListDataElementValue[0].Value
                               + @"\" + passiveWeekProfile[0].ListMeterDataPacket[0].ListDataElementValue[0].Value
                               + @"\" + passiveDayProfile[0].ListMeterDataPacket[0].ListDataElementValue[0].Value
                               + @"\" + activeSeasonDayProfile[0].ListMeterDataPacket[0].ListDataElementValue[0].Value
                               + @"\" + activeWeekDayProfile[0].ListMeterDataPacket[0].ListDataElementValue[0].Value
                               + @"\" + activeDayProfile[0].ListMeterDataPacket[0].ListDataElementValue[0].Value
                               + @"\" + activationDate[0].ListMeterDataPacket[0].ListDataElementValue[0].Value
                               + @"\" + specialDayProfile[0].ListMeterDataPacket[0].ListDataElementValue[0].Value;

                }
                else if (passiveDayProfile.Count > 0 && passiveSeasonProfile.Count > 0 && passiveWeekProfile.Count > 0
                  && activationDate.Count > 0 && activeWeekDayProfile.Count > 0 && activeSeasonDayProfile.Count > 0
                  && activeDayProfile.Count > 0 && specialDayProfile.Count == 0)
                {
                    masterEntity.MeterConfigurations.TODEntity = new TODEntity();
                    masterEntity.MeterConfigurations.TODEntity.TODData = "DLMS" + @"\" + passiveSeasonProfile[0].ListMeterDataPacket[0].ListDataElementValue[0].Value
                               + @"\" + passiveWeekProfile[0].ListMeterDataPacket[0].ListDataElementValue[0].Value
                               + @"\" + passiveDayProfile[0].ListMeterDataPacket[0].ListDataElementValue[0].Value
                               + @"\" + activeSeasonDayProfile[0].ListMeterDataPacket[0].ListDataElementValue[0].Value
                               + @"\" + activeWeekDayProfile[0].ListMeterDataPacket[0].ListDataElementValue[0].Value
                               + @"\" + activeDayProfile[0].ListMeterDataPacket[0].ListDataElementValue[0].Value
                               + @"\" + activationDate[0].ListMeterDataPacket[0].ListDataElementValue[0].Value;


                }
                else if (passiveDayProfile.Count > 0 && activationDate.Count > 0 && activeDayProfile.Count > 0 &&
passiveSeasonProfile.Count == 0 && passiveWeekProfile.Count == 0 && activeSeasonDayProfile.Count == 0 )
                {
                    masterEntity.MeterConfigurations.TODEntity = new TODEntity();
                    masterEntity.MeterConfigurations.TODEntity.TODData = "DLMS" + @"\" + "" + @"\" + ""
                               + @"\" + passiveDayProfile[0].ListMeterDataPacket[0].ListDataElementValue[0].Value
                               + @"\" + ""
                               + @"\" + ""
                               + @"\" + activeDayProfile[0].ListMeterDataPacket[0].ListDataElementValue[0].Value
                               + @"\" + activationDate[0].ListMeterDataPacket[0].ListDataElementValue[0].Value;
                }

                else if (passiveDayProfile.Count > 0 && passiveSeasonProfile.Count > 0 && passiveWeekProfile.Count > 0
                  && activationDate.Count > 0 && activeWeekDayProfile.Count > 0 && activeDayProfile.Count > 0 && specialDayProfile.Count > 0)
                {
                    masterEntity.MeterConfigurations.TODEntity = new TODEntity();
                    masterEntity.MeterConfigurations.TODEntity.TODData = "DLMS" + @"\" + passiveSeasonProfile[0].ListMeterDataPacket[0].ListDataElementValue[0].Value
                               + @"\" + passiveWeekProfile[0].ListMeterDataPacket[0].ListDataElementValue[0].Value
                               + @"\" + passiveDayProfile[0].ListMeterDataPacket[0].ListDataElementValue[0].Value
                               + @"\" + passiveSeasonProfile[0].ListMeterDataPacket[0].ListDataElementValue[0].Value
                               + @"\" + activeWeekDayProfile[0].ListMeterDataPacket[0].ListDataElementValue[0].Value
                               + @"\" + activeDayProfile[0].ListMeterDataPacket[0].ListDataElementValue[0].Value
                               + @"\" + activationDate[0].ListMeterDataPacket[0].ListDataElementValue[0].Value
                               + @"\" + specialDayProfile[0].ListMeterDataPacket[0].ListDataElementValue[0].Value;

                }
            }
            else
            {
                if (activeWeekDayProfile.Count > 0 && activeSeasonDayProfile.Count > 0 && activeDayProfile.Count > 0 && rtcData.Count > 0)
                {

                    string[] touData = new string[4];
                    touData[0] = activeDayProfile[0].ListMeterDataPacket[0].ListDataElementValue[0].Value;
                    touData[1] = activeWeekDayProfile[0].ListMeterDataPacket[0].ListDataElementValue[0].Value;
                    touData[2] = activeSeasonDayProfile[0].ListMeterDataPacket[0].ListDataElementValue[0].Value;
                    touData[3] = rtcData[0].ListMeterDataPacket[0].ListDataElementValue[0].Value;

                    LoadTOUData(touData, masterEntity);
                }
            }

            if (dipData != null && dipData.Count > 0)
            {
                masterEntity.MeterConfigurations.DIPEntity = mapperDIP.GetData(dipData, isUpload);
            }
            if (dipWithIPData != null && dipWithIPData.Count > 0)
            {
                masterEntity.MeterConfigurations.mdWithIPEntity = mapperDIPWithIP.GetMappedEntity(dipWithIPData);
            }
            if (kvahSelectionData != null && kvahSelectionData.Count > 0)
            {
                masterEntity.MeterConfigurations.kvarselectionEntity = mapperKvar.GetData(kvahSelectionData);
            }

            if (rs232LockData != null && rs232LockData.Count > 0)
            {
                masterEntity.MeterConfigurations.RS232Entity = mapperRS232.GetData(rs232LockData);
            }

            if (billingTypeData != null && billingTypeData.Count > 0)
            {
                masterEntity.MeterConfigurations.billingTypeEntity = mapperBillingType.GetData(billingTypeData, billingMonthTypeData); // [BillingType_Month]
            }
            if (lsipData != null && lsipData.Count > 0)
            {
                masterEntity.MeterConfigurations.LSIPEntity = mapperLSCapturePeriod.GetData(lsipData);
            }
            if (autoLockData != null && autoLockData.Count > 0)
            {
                masterEntity.MeterConfigurations.AutoLockEntity = mapperAutoLock.GetData(autoLockData);
            }

            if (rtcData != null && rtcData.Count > 0)
            {
                masterEntity.MeterConfigurations.rtcEnity = mapperRTC.GetData(rtcData);
            }

            if (pulseEnergyData != null && pulseEnergyData.Count > 0)
            {
                masterEntity.MeterConfigurations.PulseEnergyEntity = mapperPulseEnergy.GetData(pulseEnergyData);
            }

            if (pushDisplayParameterData != null && pushDisplayParameterData.Count > 0)
            {
                masterEntity.MeterConfigurations.DisplayParamater = new Collection<DisplayParamatersDBEntity>();
                collDisplayParamatersDBEntity = mapperDisplayParameter.GetData(pushDisplayParameterData, DisplayParameterType.PushMode);
                foreach (DisplayParamatersDBEntity displParameterEntity in collDisplayParamatersDBEntity)
                {
                    masterEntity.MeterConfigurations.DisplayParamater.Add(displParameterEntity);
                }
            }
            if (scrollDisplayParameterData != null && scrollDisplayParameterData.Count > 0)
            {
                collDisplayParamatersDBEntity = mapperDisplayParameter.GetData(scrollDisplayParameterData, DisplayParameterType.ScrollMode);
                foreach (DisplayParamatersDBEntity displParameterEntity in collDisplayParamatersDBEntity)
                {
                    masterEntity.MeterConfigurations.DisplayParamater.Add(displParameterEntity);
                }
            }
            if (highResolutionDisplayParameterData != null && highResolutionDisplayParameterData.Count > 0)
            {
                collDisplayParamatersDBEntity = mapperDisplayParameter.GetData(highResolutionDisplayParameterData, DisplayParameterType.HighResolutionMode);
                foreach (DisplayParamatersDBEntity displParameterEntity in collDisplayParamatersDBEntity)
                {
                    masterEntity.MeterConfigurations.DisplayParamater.Add(displParameterEntity);
                }
            }
            if (displayTimeoutData != null && displayTimeoutData.Count > 0 && masterEntity.MeterConfigurations.DisplayParamater != null)
            {
                collDisplayParamatersDBEntity = mapperDisplayParameter.GetData(displayTimeoutData, DisplayParameterType.DisplayTimeouts);
                foreach (DisplayParamatersDBEntity displParameterEntity in collDisplayParamatersDBEntity)
                {
                    masterEntity.MeterConfigurations.DisplayParamater.Add(displParameterEntity);
                }
            }
            if (manualBillingData != null && manualBillingData.Count > 0)
            {
                masterEntity.MeterConfigurations.ManualBillingEntity = mapperManualBilling.GetData(manualBillingData);
            }
            if (softwareBillingData != null && softwareBillingData.Count > 0)
            {
                masterEntity.MeterConfigurations.SoftwareBillingEntity = mapperSoftwareBilling.GetData(softwareBillingData);
            }
            if (disconnectControlData != null && disconnectControlData.Count > 0)
            {
                masterEntity.MeterConfigurations.DisconnectControlEntity = new CABEntity.DisconnectControlEntity();
                masterEntity.MeterConfigurations.DisconnectControlEntity.DCData = disconnectControlData[0].ListMeterDataPacket[0].ListDataElementValue[0].Value;
            }
            if (loadControlData != null && loadControlData.Count > 0 && loadControlData[0].ListMeterDataPacket != null && loadControlData[0].ListMeterDataPacket.Count > 0 && loadControlData[0].ListMeterDataPacket[0] != null)
            {
                masterEntity.MeterConfigurations.LoadControlEntity = new CABEntity.LoadControlEntity();
                int iter = 0;
                foreach (DataElement item in loadControlData[0].ListMeterDataPacket[0].ListDataElementValue)
                {
                    if (iter == 0)
                    {
                        masterEntity.MeterConfigurations.LoadControlEntity.LCData += item.Value;
                    }
                    else
                    {
                        masterEntity.MeterConfigurations.LoadControlEntity.LCData += "\\" + item.Value;
                    }
                    iter++;
                }
            }
            if (loadControl1PSmartData != null && loadControl1PSmartData.Count > 0 && loadControl1PSmartData[0].ListMeterDataPacket != null && loadControl1PSmartData[0].ListMeterDataPacket.Count > 0 && loadControl1PSmartData[0].ListMeterDataPacket[0] != null)
            {
                masterEntity.MeterConfigurations.LoadControlEntity = new CABEntity.LoadControlEntity();
                int iter = 0;
                foreach (DataElement item in loadControl1PSmartData[0].ListMeterDataPacket[0].ListDataElementValue)
                {
                    if (iter == 0)
                    {
                        masterEntity.MeterConfigurations.LoadControlEntity.LCData += item.Value;
                    }
                    else
                    {
                        masterEntity.MeterConfigurations.LoadControlEntity.LCData += "\\" + item.Value;
                    }
                    iter++;
                }
            }

            if (RS485Data != null && RS485Data.Count > 0)
            {
                masterEntity.MeterConfigurations.RS485Entity = new CABEntity.RS485Entity();
                masterEntity.MeterConfigurations.RS485Entity.DCData = RS485Data[0].ListMeterDataPacket[0].ListDataElementValue[0].Value;
            }
            if (PaymentModeData != null && PaymentModeData.Count > 0)
            {
                masterEntity.MeterConfigurations.PaymentModeEntity = new CABEntity.PaymentModeEntity();
                masterEntity.MeterConfigurations.PaymentModeEntity.PMData = PaymentModeData[0].ListMeterDataPacket[0].ListDataElementValue[0].Value;
            }

            if (MeteringModeData != null && MeteringModeData.Count > 0)
            {
                masterEntity.MeterConfigurations.MeteringModeEntity = new CABEntity.MeteringModeEntity();
                masterEntity.MeterConfigurations.MeteringModeEntity.MMData = MeteringModeData[0].ListMeterDataPacket[0].ListDataElementValue[0].Value;
            }
            if (LoadLimitData != null && LoadLimitData.Count > 0)
            {
                masterEntity.MeterConfigurations.LoadLimitEntity = mapperLoadlimit.GetData(LoadLimitData, isUpload);



            }

            if (SlidingDemandData != null && SlidingDemandData.Count > 0)
            {
                masterEntity.MeterConfigurations.SlidingDemandEntity = new CABEntity.SlidingDemandEntity();
                masterEntity.MeterConfigurations.SlidingDemandEntity.SDData = SlidingDemandData[0].ListMeterDataPacket[0].ListDataElementValue[0].Value;
            }
            if (OpticalData != null && OpticalData.Count > 0)
            {
                masterEntity.MeterConfigurations.OpticalLockEntity = new CABEntity.OpticalLockUnlockEntity();
                masterEntity.MeterConfigurations.OpticalLockEntity.OPData = OpticalData[0].ListMeterDataPacket[0].ListDataElementValue[0].Value;
            }

            if (RJData != null && RJData.Count > 0)
            {
                masterEntity.MeterConfigurations.RJLockEntity = new CABEntity.RJLockUnlockEntity();
                masterEntity.MeterConfigurations.RJLockEntity.RJData = RJData[0].ListMeterDataPacket[0].ListDataElementValue[0].Value;
            }



            //if (dailylo != null && kvahSelectionData.Count > 0)
            //{
            //    masterEntity.MeterConfigurations.kvarselectionEntity = mapperKvar.GetData(kvahSelectionData);
            //}
            #endregion
            if (LoadSwitchData != null && LoadSwitchData.Count > 0)
            {
                entityToHoldTempData = mapperLoadSwitch.GetMappedEntity(LoadSwitchData, meterVariant);
                masterEntity.LoadSwitch = entityToHoldTempData.LoadSwitch;
                masterEntity.LoadSwitchParameterColumns = entityToHoldTempData.LoadSwitchParameterColumns;
            }

            if (ManualMDResetData != null && ManualMDResetData.Count > 0)
            {
                masterEntity.MeterConfigurations.ManualButtonMDResetEntity = mappermanualmdreset.GetData(ManualMDResetData);
            }

            return masterEntity;

        }

        /// <summary>
        /// Wrapper for CDFConverter: returns CAB.Entity.BillingGeneralNFDLMSEntity so
        /// Generic.CABApplication (no direct CAB.Entity reference) can use it without CS0246.
        /// </summary>
        public LNG.Entity.BillingGeneralNFDLMSEntity GetMasterEntityCAB(string fileText, bool isUpload)
        {
            return new IECToDLMSMapper(true).MapEntityToCAB(GetMasterEntity(fileText, isUpload));
        }

        //////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="master"></param>
        public void LoadTOUData(string[] data, BillingGeneralNFDLMSEntity masterEntity)
        {


            if (string.IsNullOrEmpty(data[0]))
            {
                return;
            }
            int currentDayID = 1;
            int weekNumber = 1;
            if (data.Length == 4)
            {
                DateTime meterDateTime = GetDateTime(data[3]);
                weekNumber = GetWeekProfileNumber(data[2], meterDateTime);
                currentDayID = GetCurrentDayProfileNo(data[1], meterDateTime, weekNumber);
            }

            string currentTOUdata = data[0];
            TOUEntity touEntity = new TOUEntity();
            List<TOU> touList = new List<TOU>();
            touEntity.tou = new List<TOU>();

            /*08
             * 0118
             *      0202
             *          1101
             *          010A
             *              0203090400000000090600000A0064FF120001
             *              0203090406000000090600000A0064FF120002
             *              020309040C000000090600000A0064FF120003
             *              0203090412000000090600000A0064FF120004
             *              0203090400000000090600000A0064FF120000
             *              0203090400000000090600000A0064FF1200000203090400000000090600000A0064FF1200000203090400000000090600000A0064FF1200000203090400000000090600000A0064FF1200000203090400000000090600000A0064FF120000*/

            int counter = 0;
            counter = counter + 2;  // for array
            int dayCount = ConvertHexToDecimal(currentTOUdata, counter);
            counter = counter + 2;  // for array length
            byte dayIndex = 0;

            // For Ruby Every Season have 6 day profile so 2nd Week should start from 7th Day Profile 
            if (dayCount > 2) // ruby (24 )
            {
                if (weekNumber == 2)
                {
                    currentDayID = 7;
                }
            }

            while (dayIndex < dayCount)
            {
                counter = counter + 4; // for Structure + length
                counter = counter + 2; //  unsigned
                int dayID = ConvertHexToDecimal(currentTOUdata, counter);
                counter = counter + 2; // day id
                if (dayID == currentDayID)
                {
                    byte dayActionIndex = 0;
                    counter = counter + 2;  // for array
                    int dayActionCount = ConvertHexToDecimal(currentTOUdata, counter);
                    counter = counter + 2;  // for array length

                    while (dayActionIndex < dayActionCount)
                    {
                        TOU touItem = new TOU();
                        counter = counter + 4;      //0203 (struct of 3)
                        counter = counter + 4;      //090400000000 (String of 4 and HH,MM)
                        touItem.StartHour = ConvertHexToDecimal(currentTOUdata, counter);
                        counter = counter + 2;
                        touItem.StartMin = ConvertHexToDecimal(currentTOUdata, counter);
                        counter = counter + 2;
                        counter = counter + 4;

                        counter = counter + 16;  //090600000A0064FF (OBIS code)
                        counter = counter + 4;  //120005 ( tariff in last byte)
                        touItem.Tariff = ConvertHexToDecimal(currentTOUdata, counter);
                        counter = counter + 2;
                        touItem.SeasonNumber = Convert.ToByte(weekNumber);
                        if (touItem.Tariff == 0)
                            break;
                        // touEntity.tou.Add(touItem);
                        touList.Add(touItem);
                        dayActionIndex++;
                    }
                    break;
                }
                else
                {
                    byte dayActionIndex = 0;
                    counter = counter + 2;  // for array
                    int dayActionCount = ConvertHexToDecimal(currentTOUdata, counter);
                    counter = counter + 2;  // for array length

                    while (dayActionIndex < dayActionCount)
                    {
                        counter = counter + 38;
                        dayActionIndex++;
                    }
                }
                dayIndex++;
            }



            masterEntity.TOU = touList;

        }
        /// <summary>
        /// This method converts the Date time values(Hexadecimal format) for a parameter into proper date time string
        /// </summary>
        /// <param name="DateTimeValue"></param>
        /// <returns></returns>
        public DateTime GetDateTime(string DateTimeValue)
        {
            int num = 0;
            int Year = 0;
            int Month = 0;
            int Day = 0;
            int Hour = 0;
            int Minute = 0;
            int Seconds = 0;
            DateTime dateTime;
            try
            {
                // Extracting the year value
                num += 4;
                string data = DateTimeValue.Substring(num, 4);
                Year = Int32.Parse(data, System.Globalization.NumberStyles.HexNumber);
                num += 4;
                // Extracting the month value
                Month = ConvertHexToDecimal(DateTimeValue.Substring(num, 2), 0);
                num += 2;
                // Extracting the Day value
                Day = ConvertHexToDecimal(DateTimeValue.Substring(num, 2), 0);
                num += 4;
                // Extracting the Hour value
                Hour = ConvertHexToDecimal(DateTimeValue.Substring(num, 2), 0);
                num += 2;
                // Extracting the Minutes value
                Minute = ConvertHexToDecimal(DateTimeValue.Substring(num, 2), 0);
                num += 2;
                // Extracting the Seconds value
                Seconds = ConvertHexToDecimal(DateTimeValue.Substring(num, 2), 0);
                num += 2;

                dateTime = new DateTime(Year, Month, Day, Hour, Minute, Seconds);
            }
            catch (Exception ex)    //SarkarA //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetDateTime(string DateTimeValue)", ex);
                dateTime = System.DateTime.Now;
            }

            return dateTime;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="calendarSeasonProfile"></param>
        /// <param name="currentDate"></param>
        /// <returns></returns>
        public int GetWeekProfileNumber(string seasonProfileData, DateTime currentDate)
        {
            int weekProfile = 1;

            //08
            //0104
            //0203
            //090101
            //090CFFFF0101FFFFFFFFFF800000
            //090101
            //0203090101090CFFFF0401FFFFFFFFFF800000090102
            //0203090101090CFFFF0701FFFFFFFFFF800000090103
            //0203090101090CFFFF0A01FFFFFFFFFF800000090104
            try
            {
                int counter = 0;
                counter = counter + 2;  // for array
                int seasonCount = ConvertHexToDecimal(seasonProfileData, counter);
                counter = counter + 2;  // for array length
                byte seasonIndex = 0;

                while (seasonIndex < seasonCount)
                {
                    counter = counter + 4; // for Structure + length
                    counter = counter + 6; // season profile name
                    counter = counter + 8; // for date 

                    int seasonMonth = ConvertHexToDecimal(seasonProfileData, counter);
                    int seasonDay = ConvertHexToDecimal(seasonProfileData, counter + 2);
                    DateTime dateTime = new DateTime(currentDate.Year, seasonMonth, seasonDay);
                    /*GKG : 146685 TOU Tariff issue */
                    //counter = counter + 20; // for date time
                    //counter = counter + 4;
                    //weekProfile = ConvertHexToDecimal(seasonProfileData, counter);
                    /*GKG : 146685 TOU Tariff issue */
                    if (dateTime > currentDate)
                    {
                        break;
                    }
                    /*GKG : 146685 TOU Tariff issue */
                    counter = counter + 20; // for date time
                    counter = counter + 4;
                    weekProfile = ConvertHexToDecimal(seasonProfileData, counter);
                    /*GKG : 146685 TOU Tariff issue */
                    counter = counter + 2;
                    seasonIndex++;

                }
            }
            catch (Exception ex)    //SarkarA //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetWeekProfileNumber(string seasonProfileData, DateTime currentDate)", ex);
                // weekProfile = 1;
            }
            return weekProfile;
        }
        /// <summary>
        /// Gets the current day profile no from calendar's season profile, week profile and current data
        /// </summary>
        /// <param name="calendarSeasonProfile"></param>
        /// <param name="calendarWeekProfile"></param>
        /// <param name="master"></param>
        /// <returns></returns>
        public byte GetCurrentDayProfileNo(string weekProfileData, DateTime currentDate, int currentWeekNumber)
        {
            //080104
            //0208
            //0901011101110111081101111111111111
            //020809010211FF11FF11FF11FF11FF11FF11FF
            //020809010311FF11FF11FF11FF11FF11FF11FF
            //020809010411FF11FF11FF11FF11FF11FF11FF

            byte dayProfileNo = 1;
            try
            {
                int counter = 0;
                counter = counter + 2;  // for array
                int weekCount = ConvertHexToDecimal(weekProfileData, counter);
                counter = counter + 2;  // for array length
                byte weekIndex = 0;

                while (weekIndex <= weekCount)
                {
                    counter = counter + 4; // for Structure + length
                    counter = counter + 4; // for week profile name
                    weekIndex = ConvertHexToDecimal(weekProfileData, counter);
                    counter = counter + 2;
                    if (weekIndex == currentWeekNumber)
                    {
                        int cDay = (int)(currentDate.DayOfWeek - 1);
                        for (int i = 0; i < 7; i++)
                        {
                            counter = counter + 2;  // enum

                            if (i == cDay)
                            {
                                dayProfileNo = ConvertHexToDecimal(weekProfileData, counter);
                                break;
                            }
                            counter = counter + 2;  // day id
                        }

                        break;
                    }
                    else
                    {
                        counter = counter + 28;
                    }
                    weekIndex++;

                }
            }
            catch (Exception ex)    //SarkarA //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetCurrentDayProfileNo(string weekProfileData, DateTime currentDate, int currentWeekNumber)", ex);
                dayProfileNo = 1;
            }

            if (dayProfileNo == 0xff)
            {
                dayProfileNo = 0x01;
            }
            return dayProfileNo;
        }
        public byte ConvertHexToDecimal(string dataInStringFormat, int dataIndex)
        {
            string data = dataInStringFormat.Substring(dataIndex, 2);
            return byte.Parse(data, System.Globalization.NumberStyles.HexNumber);
        }
        ///////////////////////////////////////////////

        /// <summary>
        /// Get DLMS File Content
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public string GetContent(string filePath)
        {
            StreamReader streamReader = new StreamReader(filePath);
            string fileContent = streamReader.ReadToEnd();
            streamReader.Close();
            return fileContent;
        }
        /// <summary>
        /// Gets Content of IEC File
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public string GetIECFileContent(string filePath)
        {
            StreamReader streamReader = new StreamReader(filePath);

            // string fileName = Path.GetFileName(filePath);

            string fileContent = streamReader.ReadToEnd();
            streamReader.Close();
            if (ConfigInfo.IsEncryption())
            {
                fileContent = ConfigInfo.DecryptFile(fileContent);
            }
            if (fileContent.Contains("<F><Gen>"))
            {
                return "NonSupportedFileError";
            }
            //if (fileContent.Contains("NP/"))
            //{
            //    //MessageBox.Show(string.Format(resourceMgr.GetString("FormatNotSupported"), fileName), "BCS", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //    fileContent = string.Empty;
            //}

            return fileContent;
        }
        #endregion

        #region Protected Methods
        #endregion

        #region Event Handlers
        #endregion

        #region Private Methods

        #region GetMeterDataID
        /// <summary>
        /// Search proper meter data id(from the mapping table) for a an item based on its reading date time and meterid.
        /// </summary>
        /// <param name="collectionItemReadOuts"></param>
        /// <param name="meterID"></param>
        /// <param name="readingDateTime"></param>
        /// <returns></returns>
        private Int64 GetMeterDataID(Collection<ReadOutCounterEntity> collectionItemReadOuts, string meterID, Int64 readingDateTime)
        {
            for (int i = 0; i < collectionItemReadOuts.Count; i++)
            {
                if (collectionItemReadOuts[i].meterID.Trim() == meterID.Trim() && collectionItemReadOuts[i].readingDateTime == readingDateTime)
                    return collectionItemReadOuts[i].meterDataID;
            }
            return -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="master"></param>
        /// <returns></returns>
        private List<IECBillingGeneralNFEntity> GetMasterEntity(IECBillingGeneralNFEntity master)
        {
            List<IECBillingGeneralNFEntity> outputEntity = new List<IECBillingGeneralNFEntity>();
            IECBillingGeneralNFEntity masterEntity;

            foreach (long meterDataId in meterDataIds)
            {
                masterEntity = new IECBillingGeneralNFEntity();
                masterEntity.listGeneralData = new List<GeneralData>();
                masterEntity.listLoadSurveyData = new List<LoadSurveyData>();
                masterEntity.listPhasor = new List<IECPhasorEntity>();
                masterEntity.listDTMDailyProfileData = new List<DTMDailyProfileData>();
                masterEntity.listFraudEnergy = new List<IECFraudEnergyEntity>();
                masterEntity.listRTCUpdate = new List<RTCUpdateEntity>();
                masterEntity.listTamper = new List<TamperData>();
                masterEntity.listTransactionData = new List<TransactionData>();
                masterEntity.meterConfigurationDetail = new List<IECMeterConfigurationsNFEntity>();

                if (master.listGeneralData != null)
                {
                    foreach (GeneralData generalData in master.listGeneralData)
                    {
                        if (GetMeterDataID(readOuts[ReadOutItem.General], generalData.MeterData.MeterID, generalData.MeterData.ReadingDateTime) == meterDataId)
                        {
                            generalData.MeterData.MeterData_ID = meterDataId;
                            masterEntity.listGeneralData.Add(generalData);

                        }
                    }
                }
                if (master.listLoadSurveyData != null)
                {
                    foreach (LoadSurveyData loadSurveyData in master.listLoadSurveyData)
                    {
                        if (GetMeterDataID(readOuts[ReadOutItem.LoadSurvey], loadSurveyData.LoadSurveyMeterData.MeterID, loadSurveyData.LoadSurveyMeterData.ReadingDateTime) == meterDataId)
                        {
                            loadSurveyData.LoadSurveyMeterData.MeterData_ID = meterDataId;
                            masterEntity.listLoadSurveyData.Add(loadSurveyData);
                        }
                    }
                }

                if (master.listDTMDailyProfileData != null)
                {
                    foreach (DTMDailyProfileData dailyProfileData in master.listDTMDailyProfileData)
                    {
                        if (GetMeterDataID(readOuts[ReadOutItem.DailyProfile], dailyProfileData.DTMDailyProfileMeterData.MeterID, dailyProfileData.DTMDailyProfileMeterData.ReadingDateTime) == meterDataId)
                        {
                            dailyProfileData.DTMDailyProfileMeterData.MeterData_ID = meterDataId;
                            masterEntity.listDTMDailyProfileData.Add(dailyProfileData);
                        }
                    }
                }
                if (master.listFraudEnergy != null)
                {
                    foreach (IECFraudEnergyEntity fraudEnergy in master.listFraudEnergy)
                    {
                        if (GetMeterDataID(readOuts[ReadOutItem.FraudEnergy], fraudEnergy.MeterID, fraudEnergy.ReadingDateTime) == meterDataId)
                        {
                            fraudEnergy.MeterData_ID = meterDataId;
                            masterEntity.listFraudEnergy.Add(fraudEnergy);
                        }
                    }
                }

                if (master.listPhasor != null)
                {
                    foreach (IECPhasorEntity phasorEntity in master.listPhasor)
                    {
                        if (GetMeterDataID(readOuts[ReadOutItem.Phasor], phasorEntity.MeterID, phasorEntity.ReadingDateTime) == meterDataId)
                        {
                            phasorEntity.MeterData_ID = meterDataId;
                            masterEntity.listPhasor.Add(phasorEntity);
                        }
                    }
                }
                if (master.listTamper != null)
                {
                    foreach (TamperData tamperData in master.listTamper)
                    {
                        if (GetMeterDataID(readOuts[ReadOutItem.Tamper], tamperData.General.MeterID, tamperData.General.ReadingDateTime) == meterDataId)
                        {
                            tamperData.General.MeterData_ID = meterDataId;
                            masterEntity.listTamper.Add(tamperData);
                        }
                    }
                }

                if (master.listTransactionData != null)
                {
                    foreach (TransactionData transactionData in master.listTransactionData)
                    {
                        if (GetMeterDataID(readOuts[ReadOutItem.Transaction], transactionData.meterDataEntity.MeterID, transactionData.meterDataEntity.ReadingDateTime) == meterDataId)
                        {
                            transactionData.meterDataEntity.MeterData_ID = meterDataId;
                            masterEntity.listTransactionData.Add(transactionData);
                        }
                    }
                }

                if (master.listRTCUpdate != null)
                {
                    foreach (RTCUpdateEntity rtcUpdateEntity in master.listRTCUpdate)
                    {
                        if (GetMeterDataID(readOuts[ReadOutItem.RTCUpdate], rtcUpdateEntity.MeterID, rtcUpdateEntity.ReadingDateTime) == meterDataId)
                        {
                            rtcUpdateEntity.MeterData_ID = meterDataId;
                            masterEntity.listRTCUpdate.Add(rtcUpdateEntity);
                        }
                    }
                }

                // Add functinality for Meter Config
                // Mohsin

                if (master.meterConfigurationDetail != null)
                {
                    foreach (IECMeterConfigurationsNFEntity meterConfiguration in master.meterConfigurationDetail)
                    {
                        if (GetMeterDataID(readOuts[ReadOutItem.MeterConfigurations], meterConfiguration.TODEntity.MeterDataID, meterConfiguration.TODEntity.ReadingDateTime) == meterDataId)
                        {
                            meterConfiguration.TODEntity.MeterData_ID = meterDataId;
                            masterEntity.meterConfigurationDetail.Add(meterConfiguration);
                        }
                    }
                }
                masterEntity.MeterDataID = meterDataId;
                outputEntity.Add(masterEntity);

            }

            return outputEntity;
        }
        #endregion


        /// <summary>
        /// This method is used for getting source of file going to be uploaded .
        /// </summary>
        /// <returns></returns>
        private int GetCommType()
        {
            CommTypes commType = CommTypes.Direct;
            int channelType = Convert.ToInt32(ConfigSettings.GetValue("SourceOfFile"));
            if (channelType == (int)CommTypes.GSM)
            {
                commType = CommTypes.GSM;
            }
            else if (channelType == (int)CommTypes.PSTN)
            {
                commType = CommTypes.PSTN;
            }
            else if (channelType == (int)CommTypes.GPRS)
            {
                commType = CommTypes.GPRS;
            }
            else if (channelType == (int)CommTypes.Restore)
            {
                commType = CommTypes.Restore;
            }
            else if (channelType == (int)CommTypes.Scheduling)
            {
                commType = CommTypes.Scheduling;
            }
            else if (channelType == (int)CommTypes.Import)
            {
                commType = CommTypes.Import;
            }
            else if (channelType == (int)CommTypes.Upload)
            {
                commType = CommTypes.Upload;
            }
            else if (channelType == (int)CommTypes.CMRI)
            {
                commType = CommTypes.CMRI;
            }
            else if (channelType == (int)CommTypes.TCP)
            {
                commType = CommTypes.TCP;
            }
            else if (channelType == (int)CommTypes.FTP)
            {
                commType = CommTypes.FTP;
            }
            return (int)commType;
        }
        /// <summary>
        /// Gets list of all profile's data as input and returns the list of desired profile data
        /// </summary>
        /// <param name="allProfileData"></param>
        /// <param name="profileId"></param>
        /// <returns></returns>
        private List<ProfileData> GetSingleProfileData(List<ProfileData> allProfileData, int profileId)
        {
            
            List<ProfileData> resultData = allProfileData.Where(item => item.ProfileId == profileId).ToList() as List<ProfileData>;
            return resultData;
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
                        //EventLogging.CallLogDetails("File corrupt.");
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
                                //EventLogging.CallLogDetails("File '" + fileEntity.FileName + "' already exist.");
                                return isUploaded;
                            }
                        }
                        fileUploadMasterEntity = fileUploadMasterBLL.InsertData(fileUploadMasterEntity) as FileUploadMasterEntity;
                        if (fileUploadMasterEntity.FileUpload_ID == 0)
                        {
                            //EventLogging.CallLogDetails("Please Contact system administrator. Invalid DB Structure.");
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
                        // EventLogging.CallLogDetails("No data in the selected file.");
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
                    catch (Exception ex)    //SarkarA //Exception log for catch block
                    {
                        EventLogging.CallLogDetails(ex.Message.ToString());
                        logger.Log(LOGLEVELS.Error, "Upload(string fileName, string fileText, bool flag)", ex);
                        isUploaded = false;
                    }
                }
                return isUploaded;
            }
            catch (Exception ex)    //SarkarA //Exception log for catch block
            {
                fileUploadMasterBLL.DeleteData(fileUploadMasterEntity);
                EventLogging.CallLogDetails("File corrupted." + ex.Message.ToString());
                logger.Log(LOGLEVELS.Error, "Upload(string fileName, string fileText, bool flag)", ex);
                return false;
            }
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
        /// Parse each readout for Read item tag.
        /// </summary>
        /// <param name="readOut"></param>
        /// <param name="MeterData_ID"></param>
        private void ParseReadOutItems(string readOut, Int64 MeterData_ID)
        {
            string data = readOut.Replace(FormatterConstant.ENTERCARRAGE, string.Empty);
            MatchCollection matches = FormatterCommon.ValidateData(data);
            string[] generalData = new string[matches.Count];
            int counter = 0;
            foreach (Match match in matches)
            {
                GroupCollection groups = match.Groups;
                generalData[counter++] = groups[0].Value;
            }
            string[] GeneralData = FormatterCommon.RemoveDuplicateData(generalData);
            counter = 0;
            while (counter <= GeneralData.GetUpperBound(0)) counter++;
            //Create General Data Mapping
            if (counter > 0 && GeneralData.Length > 0)
                CreateReadOutItemDictionary("RD", readOut, MeterData_ID);
            //Create Fraud Energy Data Mapping
            if (readOut.Contains("MI"))
                CreateReadOutItemDictionary("MI", readOut, MeterData_ID);
            //Create Transactions Data Mapping
            if (readOut.Contains("TR"))
                CreateReadOutItemDictionary("TR", readOut, MeterData_ID);
            //Create RTC Update Data Mapping
            if (readOut.Contains("RU"))
                CreateReadOutItemDictionary("RU", readOut, MeterData_ID);
            //Create Meter Configurations Data Mapping
            if (readOut.Contains("CR"))
            {
                CreateReadOutItemDictionary("CR", readOut, MeterData_ID);
                readOut = readOut.Replace("DP", string.Empty);
                readOut = readOut.Replace("DL", string.Empty);
            }
            //Create Load Survey Data Mapping
            if (readOut.Contains("L/"))
                CreateReadOutItemDictionary("L", readOut, MeterData_ID);
            //Create Tamper Data Mapping
            if (readOut.Contains("TM"))
                CreateReadOutItemDictionary("TM", readOut, MeterData_ID);
            //Create DTM Load Survey Data Mapping
            if (readOut.Contains("SD/"))
                CreateReadOutItemDictionary("SD", readOut, MeterData_ID);
            //Create Header Data Mapping
            if (readOut.Contains("HD/"))
                CreateReadOutItemDictionary("HD", readOut, MeterData_ID);
            //Create Name plate Data Mapping
            if (readOut.Contains("NP"))
            {
                CreateReadOutItemDictionary("NP", readOut, MeterData_ID);
                readOut = readOut.Replace("NP", string.Empty);
            }
            //Create Phasor Data Mapping
            if (readOut.Contains("P/"))
                CreateReadOutItemDictionary("P", readOut, MeterData_ID);
            // TOU in Meter Configuration
            if (readOut.Contains("TU"))
            {
                CreateReadOutItemDictionary("TU", readOut, MeterData_ID);
                readOut = readOut.Replace("TU", string.Empty);
            }
        }

        /// <summary>
        /// Parse each readout for Read item tag.
        /// </summary>
        /// <param name="readOut"></param>
        /// <param name="MeterData_ID"></param>
        private void ParseReadOutItemsForSPhase(string readOut, Int64 MeterData_ID, Dictionary<string, string> dicOBISandData)
        {
            string data = readOut.Replace(FormatterConstant.ENTERCARRAGE, string.Empty);
            MatchCollection matches = FormatterCommon.ValidateData(data);//To be changed
            string[] generalData = new string[matches.Count];
            int counter = 0;
            foreach (Match match in matches)
            {
                GroupCollection groups = match.Groups;
                generalData[counter++] = groups[0].Value;
            }
            string[] GeneralData = FormatterCommon.RemoveDuplicateDataForSPhase(generalData);
            counter = 0;
            while (counter <= GeneralData.GetUpperBound(0)) counter++;
            //Create General Data Mapping
            if (counter > 0 && GeneralData.Length > 0)
                CreateReadOutItemDictionaryForSPhase("RD", readOut, MeterData_ID, dicOBISandData);
            //Create Fraud Energy Data Mapping
            if (readOut.Contains("MI"))
                CreateReadOutItemDictionary("MI", readOut, MeterData_ID);
            //Create Transactions Data Mapping
            if (readOut.Contains("TR"))
                CreateReadOutItemDictionary("TR", readOut, MeterData_ID);
            //Create RTC Update Data Mapping
            if (readOut.Contains("RU"))
                CreateReadOutItemDictionary("RU", readOut, MeterData_ID);
            //Create Meter Configurations Data Mapping
            if (readOut.Contains("CR"))
            {
                CreateReadOutItemDictionary("CR", readOut, MeterData_ID);
                readOut = readOut.Replace("DP", string.Empty);
                readOut = readOut.Replace("DL", string.Empty);
            }
            //Create Load Survey Data Mapping
            if (readOut.Contains("L/"))
                CreateReadOutItemDictionaryForSPhase("L", readOut, MeterData_ID,dicOBISandData);
            if (readOut.Contains("SA/"))
                CreateReadOutItemDictionaryForSPhase("L", readOut, MeterData_ID, dicOBISandData);
            //Create Tamper Data Mapping
            if (readOut.Contains("TM"))
            {
                CreateReadOutItemDictionaryForSPhase("TM", readOut, MeterData_ID, dicOBISandData);
                CreateReadOutItemDictionaryForSPhase("TR", readOut, MeterData_ID, dicOBISandData);
                CreateReadOutItemDictionaryForSPhase("RU", readOut, MeterData_ID, dicOBISandData);
            }
            //Create DTM Load Survey Data Mapping
            if (readOut.Contains("SD/"))
                CreateReadOutItemDictionaryForSPhase("SD", readOut, MeterData_ID,dicOBISandData);
            //Create Header Data Mapping
            if (readOut.Contains("HD/"))
                CreateReadOutItemDictionary("HD", readOut, MeterData_ID);
            //Create Name plate Data Mapping
            if (readOut.Contains("NP"))
            {
                CreateReadOutItemDictionary("NP", readOut, MeterData_ID);
                readOut = readOut.Replace("NP", string.Empty);
            }
            //Create Phasor Data Mapping
            if (readOut.Contains("P/"))
                CreateReadOutItemDictionary("P", readOut, MeterData_ID);

            //Create TU tag for Meter configuration TOD
            if (readOut.Contains("TU/"))
                CreateReadOutItemDictionaryForSPhase("TU", readOut, MeterData_ID, dicOBISandData);
        }
        /// <summary>
        /// Create mapping of readout item with meter data id.
        /// </summary>
        /// <param name="itemTag"></param>
        /// <param name="readOut"></param>
        /// <param name="meterDataID"></param>
        private void CreateReadOutItemDictionary(string itemTag, string readOut, Int64 meterDataID)
        {
            ReadOutItem readOutItem = ReadOutItem.None;
            Collection<ReadOutCounterEntity> collectionItemReadOuts;
            switch (itemTag)
            {
                case "RD"://General
                    readOutItem = ReadOutItem.General;
                    break;
                case "MI":
                    readOutItem = ReadOutItem.FraudEnergy;
                    break;
                case "TR":
                    readOutItem = ReadOutItem.Transaction;
                    break;
                case "RU":
                    readOutItem = ReadOutItem.RTCUpdate;
                    break;
                case "P":
                    readOutItem = ReadOutItem.Phasor;
                    break;
                case "L":
                    readOutItem = ReadOutItem.LoadSurvey;
                    break;
                case "TM":
                    readOutItem = ReadOutItem.Tamper;
                    break;
                case "SD":
                    readOutItem = ReadOutItem.DailyProfile;
                    break;
                case "CR":
                    readOutItem = ReadOutItem.MeterConfigurations;
                    break;
                case "HD":
                    readOutItem = ReadOutItem.HeaderDetails;
                    break;
                case "NP":
                    readOutItem = ReadOutItem.NamePlate;
                    break;
                case "TU":
                    readOutItem = ReadOutItem.MeterConfigurations;
                    break;
            }
            if (!readOuts.ContainsKey(readOutItem))
            {
                collectionItemReadOuts = new Collection<ReadOutCounterEntity>();
                readOuts.Add(readOutItem, collectionItemReadOuts);
            }

            try
            {
                ReadOutCounterEntity readOutCounterEntity = new ReadOutCounterEntity();
                //Get meter id of the ReadOutItem.
                string tmpMtrID = readOut.Substring(readOut.IndexOf(itemTag + "/") + 1 + itemTag.Length);
                readOutCounterEntity.meterID = tmpMtrID.Substring(4, tmpMtrID.IndexOf("/") - 4);
                //Get Reading Date Time of the ReadOutItem.
                string tmpreadingDateTime = tmpMtrID.Substring(tmpMtrID.IndexOf(readOutCounterEntity.meterID) + 1 + readOutCounterEntity.meterID.Length);
                tmpreadingDateTime = tmpreadingDateTime.Substring(0, tmpreadingDateTime.IndexOf("/"));

                //Create Mapping of Reading Date Time & Meter Data ID of the ReadOutItem in dictionary.
                readOutCounterEntity.readingDateTime = Convert.ToInt64(tmpreadingDateTime);
                readOutCounterEntity.meterDataID = meterDataID;

                readOuts[readOutItem].Add(readOutCounterEntity);
            }
            catch (Exception ex)    //SarkarA //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "CreateReadOutItemDictionary(string itemTag, string readOut, Int64 meterDataID)", ex);
            }
        }

        /// <summary>
        /// Create mapping of readout item with meter data id.
        /// </summary>
        /// <param name="itemTag"></param>
        /// <param name="readOut"></param>
        /// <param name="meterDataID"></param>
        private void CreateReadOutItemDictionaryForSPhase(string itemTag, string readOut, Int64 meterDataID, Dictionary<string, string> dicOBISandData)
        {
            ReadOutItem readOutItem = ReadOutItem.None;
            Collection<ReadOutCounterEntity> collectionItemReadOuts;
            switch (itemTag)
            {
                case "RD"://General
                    readOutItem = ReadOutItem.General;
                    break;
                case "MI":
                    readOutItem = ReadOutItem.FraudEnergy;
                    break;
                case "TR":
                    readOutItem = ReadOutItem.Transaction;
                    break;
                case "RU":
                    readOutItem = ReadOutItem.RTCUpdate;
                    break;
                case "P":
                    readOutItem = ReadOutItem.Phasor;
                    break;
                case "L":
                    readOutItem = ReadOutItem.LoadSurvey;
                    break;
                case "TM":
                    readOutItem = ReadOutItem.Tamper;
                    break;
                case "SD":
                    readOutItem = ReadOutItem.DailyProfile;
                    break;
                case "CR":
                    readOutItem = ReadOutItem.MeterConfigurations;
                    break;
                case "HD":
                    readOutItem = ReadOutItem.HeaderDetails;
                    break;
                case "NP":
                    readOutItem = ReadOutItem.NamePlate;
                    break;
                case "TU":
                    readOutItem = ReadOutItem.MeterConfigurations;
                    break;
            }
            if (!readOuts.ContainsKey(readOutItem))
            {
                collectionItemReadOuts = new Collection<ReadOutCounterEntity>();
                readOuts.Add(readOutItem, collectionItemReadOuts);
            }

            try
            {
                ReadOutCounterEntity readOutCounterEntity = new ReadOutCounterEntity();
                readOutCounterEntity.meterID = FormatterCommon.ParseDataFor1Phase(dicOBISandData, "C.1");
                //Get Reading Date Time of the ReadOutItem.
                string tmpreadingDateTime = "0";
                //Create Mapping of Reading Date Time & Meter Data ID of the ReadOutItem in dictionary.
                readOutCounterEntity.readingDateTime = Convert.ToInt64(tmpreadingDateTime);
                readOutCounterEntity.meterDataID = meterDataID;
                readOuts[readOutItem].Add(readOutCounterEntity);
            }
            catch (Exception ex)    //SarkarA //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "CreateReadOutItemDictionaryForSPhase(string itemTag, string readOut, Int64 meterDataID, Dictionary<string, string> dicOBISandData)", ex);
            }
        }
        /// <summary>
        /// Split the File text into the readouts.
        /// Get MeterData ID for each read out.
        /// </summary>
        /// <param name="fileText"></param>
        /// <param name="mtrDataEntity"></param>
        /// <param name="fileUploadMasterEntity"></param>
        private void ParseReadOuts(string fileText, MeterDataEntity mtrDataEntity, FileUploadMasterEntity fileUploadMasterEntity, string cmriID)
        {
            meterDataIds = new List<long>();
            readOuts = new Dictionary<ReadOutItem, Collection<ReadOutCounterEntity>>();
            string strSplitTxt = "NP";

            bool isOldFormatFile = false;
            string[] readOutsList = null;
            readOutsList = fileText.Split(new string[] { strSplitTxt }, StringSplitOptions.RemoveEmptyEntries);

            int i = 0;

            for (; i < readOutsList.Length; i++)
            {
                try
                {
                    if (readOutsList[i].Trim().Length == 0) continue;
                    string tmpMtrID = string.Empty;

                    /*GKG 03/04/2013 137643*/
                    if (isOldFormatFile)
                    {
                        if (i == 0) i++; //to skip the first blank space 
                    }
                    /*GKG 03/04/2013 137643*/
                    tmpMtrID = readOutsList[i].Substring(readOutsList[i].IndexOf("/") + 1);
                    tmpMtrID = tmpMtrID.Substring(4, tmpMtrID.IndexOf("/") - 4);

                    //Get Reading Date Time
                    string tmpreadingDateTime = readOutsList[i].Substring(readOutsList[i].IndexOf(tmpMtrID) + 1 + tmpMtrID.Length);
                    tmpreadingDateTime = tmpreadingDateTime.Substring(0, tmpreadingDateTime.IndexOf("/"));

                    //Insert meter details specific to this read out in DB and 
                    //Get Meter Data ID for this readout.
                    mtrDataEntity.MeterID = tmpMtrID;
                    mtrDataEntity.CMRIID = cmriID;
                    mtrDataEntity.ReadingDateTime = Convert.ToInt64(tmpreadingDateTime);
                    mtrDataEntity = new MeterDataBLL().InsertData(mtrDataEntity) as MeterDataEntity;
                    ParseReadOutItems(readOutsList[i], mtrDataEntity.MeterData_ID);
                    meterDataIds.Add(mtrDataEntity.MeterData_ID);
                }
                catch (Exception ex)    //SarkarA //Exception log for catch block
                {
                    logger.Log(LOGLEVELS.Error, "ParseReadOuts(string fileText, MeterDataEntity mtrDataEntity, FileUploadMasterEntity fileUploadMasterEntity, string cmriID)", ex);
                }
            }
        }

        /// <summary>
        /// Split the File text into the readouts.
        /// Get MeterData ID for each read out.
        /// </summary>
        /// <param name="fileText"></param>
        /// <param name="mtrDataEntity"></param>
        /// <param name="fileUploadMasterEntity"></param>
        private void ParseReadOutsForSPhase(string fileText, MeterDataEntity mtrDataEntity, FileUploadMasterEntity fileUploadMasterEntity, Dictionary<string, string> dicOBISandData)
        {
            meterDataIds = new List<long>();
            readOuts = new Dictionary<ReadOutItem, Collection<ReadOutCounterEntity>>();
            //********** Use /NP for single phase IEC meter when abc code is alphnumeric
            string strSplitTxt = "/NP";
            string[] readOutsList = null;
            readOutsList = fileText.Split(new string[] { strSplitTxt }, StringSplitOptions.RemoveEmptyEntries);
            int i = 0;

            for (; i < readOutsList.Length; i++)
            {
                try
                {
                    if (readOutsList[i].Trim().Length == 0) continue;
                    string tmpMtrID = string.Empty;
                    tmpMtrID = FormatterCommon.ParseDataFor1Phase(dicOBISandData, "C.1");

                    // Meter RTC is showing rather MRI datetime
                    //string tmpMtrID1 = string.Empty;
                    string[] meterArr = readOutsList[i].Split('/');
                    string tmpreadingDateTime = string.Empty;

                    if (meterArr.Length > 2)
                    {
                        tmpreadingDateTime = meterArr[2].ToString();
                    }
                    else
                    {
                        tmpreadingDateTime = "0";
                    }
                    //string tmpMtrID1 = string.Empty;
                    //tmpMtrID1 = readOutsList[i].Substring(readOutsList[i].IndexOf("/") + 1);
                    //tmpMtrID1 = tmpMtrID1.Substring(13, 16);
                    //string tmpreadingDateTime = readOutsList[i].Substring(readOutsList[i].IndexOf(tmpMtrID1) + 1 + tmpMtrID1.Length);
                    //tmpreadingDateTime = tmpreadingDateTime.Substring(0, tmpreadingDateTime.IndexOf("/"));
                    mtrDataEntity.ReadingDateTime = Convert.ToInt64(tmpreadingDateTime);
                    mtrDataEntity.MeterID = tmpMtrID;
                    mtrDataEntity = new MeterDataBLL().InsertData(mtrDataEntity) as MeterDataEntity;
                    ParseReadOutItemsForSPhase(readOutsList[i], mtrDataEntity.MeterData_ID, dicOBISandData);
                    meterDataIds.Add(mtrDataEntity.MeterData_ID);
                }
                catch (Exception ex)    //SarkarA //Exception log for catch block
                {
                    logger.Log(LOGLEVELS.Error, "ParseReadOutsForSPhase(string fileText, MeterDataEntity mtrDataEntity, FileUploadMasterEntity fileUploadMasterEntity, Dictionary<string, string> dicOBISandData)", ex);
                }
            }
        }
        #endregion

        /// <summary>
        /// REVERSE STRING FOR sPECIAL bILLING poARSING
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string ReverseString(string str)
        {
            int count = str.Length - 2;
            string revString = "";
            for (count = str.Length - 2; count >= 0; count -= 2)
            {
                revString += str.Substring(count, 2);
            }
            return revString;
        }
    }
}
