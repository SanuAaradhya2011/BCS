/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon 								|
 * | 																												|
 * |											Author : Piyush Singh. 	 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| 
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using CAB.BLL;
using CAB.IECChannel.Formatter;
using CAB.Entity;
using CAB.IECFramework.Entity;
using CAB.IECFramework.Utility;
using CAB.UI.Controls;
using CABEntity;
using System.Collections.ObjectModel;
using LTCTBLL;
using CABApplication;
using System.Collections;
using System.Data;
using CAB.Contracts;
using System.Text.RegularExpressions;
using CAB.EntityGenerator;
using CAB.Mapper;
using System.Linq;
using CAB.Parser;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using CABCommunication.Common;
using System.Globalization;

namespace CAB.UI
{   
    public partial class UploadFile : MdiChildForm
    {
        # region Member Vars declarations
       
        string sucessFiles = string.Empty; 
        string newFileString = string.Empty; 
        static string  csvFileName = Application.StartupPath +"\\"+"Log"+ "\\" + "FileStatus.csv";
        CsvFileWriter writer = new CsvFileWriter(csvFileName); 
        private OpenFileDialog openFileDialog;
        private FileUploadMasterBLL fileUploadMasterBLL = null;
        PhasorBLL phasorBLL = null;
        MeterDataBLL meterDataBLL = null;
        InstantPowerBLL instantPowerBLL = null;
        MeterDataHeaderInfoBLL meterDataHeaderInfoBLL = null;
        GeneralBLL generalBLL = null;
        BillingBLL billingBLL = null;
        TariffBLL tariffBLL = null;
        TamperGeneralBLL tamperGeneralBLL = null;
        FraudEnergyBLL fraudEnergyBLL = null;
        ProgrammingBLL programmingBLL = null;
        RTCUpdateBLL rTCUpdateBLL = null;
        LoadSurveyBLL loadSurveyBLL = null;
        TamperCounterBLL tamperCounterBLL = null;
        TamperSnapShotBLL tamperSnapShotBLL = null;
        DTMLoadSurveyBLL dTMLoadSurveyBLL = null;
        DTMDailyProfileBLL dTMDailyProfileBLL = null;
        private GenerateEntity entityGenerator = null;
        private string todData = string.Empty;
        bool fileExists = false;
        private System.Resources.ResourceManager resourceMgr;
        ArrayList uploadFileRequests = new ArrayList();
        public string cmriID;
        
        #endregion

        public UploadFile()
        {
            InitializeComponent();
            InitalizeMemberObjects();
            cmriID = String.Empty;
            resourceMgr = new System.Resources.ResourceManager("CAB.UI.UploadFile", System.Reflection.Assembly.GetExecutingAssembly());
            entityGenerator = new GenerateEntity();
        }
        public UploadFile(string cmriID)
        {
            InitializeComponent();
            InitalizeMemberObjects();
            this.cmriID=cmriID;
            entityGenerator = new GenerateEntity();
            resourceMgr = new System.Resources.ResourceManager("CAB.UI.UploadFile", System.Reflection.Assembly.GetExecutingAssembly());
        }
        private void InitalizeMemberObjects()
        {
            dTMDailyProfileBLL = new DTMDailyProfileBLL();
            dTMLoadSurveyBLL = new DTMLoadSurveyBLL();
            loadSurveyBLL = new LoadSurveyBLL();
            phasorBLL = new PhasorBLL();
            rTCUpdateBLL = new RTCUpdateBLL();
            programmingBLL = new ProgrammingBLL();
            fraudEnergyBLL = new FraudEnergyBLL();
            tamperGeneralBLL = new TamperGeneralBLL();
            meterDataHeaderInfoBLL = new MeterDataHeaderInfoBLL();
            tariffBLL = new TariffBLL();
            billingBLL = new BillingBLL();
            generalBLL = new GeneralBLL();
            meterDataBLL = new MeterDataBLL();
            instantPowerBLL = new InstantPowerBLL();
            fileUploadMasterBLL = new FileUploadMasterBLL();
            tamperCounterBLL = new TamperCounterBLL();
            tamperSnapShotBLL = new TamperSnapShotBLL();
            uploadFileRequests = new ArrayList();            
        }

        private void cmriID_OnValuesSubmission(string cmriID)
        {
            this.cmriID = cmriID;
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            fileExists = false;
            this.Cursor = Cursors.WaitCursor;
            if (txtBoxFileName.Text.Trim() == "")
            {
                CABMessageBox.ShowFilterMessage("M000098", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Cursor = Cursors.Default;
                return;
            }
            if (txtBoxFileName.Text.Length == 0)
            {
                CABMessageBox.ShowFilterMessage("D000001", "A000001|D000002", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Cursor = Cursors.Default;
                return;
            }
            CMRIID cmriID = new CMRIID(true);
            cmriID.OnValues_Submission += new CMRIID.GetSubmittedValues(cmriID_OnValuesSubmission);
            cmriID.ShowDialog();
            if (this.cmriID.Length == 0)
            {
                this.Cursor = Cursors.Default;
                return;
            }
            UploadCABFiles();
            this.Cursor = Cursors.Default;
        }        

        private void UploadCABFiles()
        {
            bool IsUploaded = false;
            string fileText = string.Empty;            
            foreach (string fileName in openFileDialog.FileNames)
            {
                if (Path.GetExtension(fileName) == ".2NG")
                {
                  
                  IsUploaded = Upload2NGFile(fileName);                   
                    
                }
                else
                {
                    fileText = string.Empty;
                    fileText = GetContent(fileName);
                    if (fileText == string.Empty)
                    {
                        return;
                    }
                    else
                    {

                        IsUploaded = Upload(fileName, fileText, true);
                    }
                }
                DeleteFile();  
            }
            
            if (IsUploaded)
            {
                this.StatusMessageAsync = resourceMgr.GetString("FileUploadedSuccess");// "File Uploaded successfully.";
                Application.DoEvents();
            }
            else
            {
                if (!fileExists)
                {
                    if (string.IsNullOrEmpty(this.StatusMessage))
                    {
                        this.StatusMessageAsync = resourceMgr.GetString("ImproperFileFormat");// "File is not in proper format.";
                        Application.DoEvents();
                    }
                }
            }
            
            }

        private void UploadCABFiles2()
        {
            if (txtBoxFileName.Text.Trim() == "")
            {
                CABMessageBox.ShowFilterMessage("M000098", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtBoxFileName.Text.Length == 0)
            {
                CABMessageBox.ShowFilterMessage("D000001", "A000001|D000002", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            UploadFileController uploadFileController = new UploadFileController();
            for (int x = 0; x < openFileDialog.FileNames.Length; x++)
            {
                uploadFileRequests.Add(uploadFileController.UploadFile(openFileDialog.FileNames[x]));

            }
            if (openFileDialog.FileNames.Length > 0) timerFileStatus.Start();
            this.ListRefresh = true;
            //btnChkUploadSatus_Click(null, null);
            //this.StatusMessage = "File Uploading Initiated...";
            // MessageBox.Show("Upload started");

        }

        private Dictionary<ReadOutItem, Collection<ReadOutCounterEntity>> readOuts = new Dictionary<ReadOutItem, Collection<ReadOutCounterEntity>>();

        #region ParseReadOuts
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileText"></param>
        /// <param name="strSplitTxt"></param>
        /// <param name="includeTag">true to include tag, false to not</param>
        /// <returns></returns>
        private string[] SplitData(string fileText, string strSplitTxt, bool includeTag)
        {
            string[] readOutsList = fileText.Split(new string[] { strSplitTxt }, StringSplitOptions.RemoveEmptyEntries);
            if (includeTag)
            {
                for (int fileIndex = 0; fileIndex < readOutsList.Length; fileIndex++)
                {
                    readOutsList[fileIndex] = strSplitTxt + readOutsList[fileIndex];
                }
            }
            return readOutsList;
        }

        /// <summary>
        /// Split the File text into the readouts.
        /// Get MeterData ID for each read out.
        /// </summary>
        /// <param name="fileText"></param>
        /// <param name="mtrDataEntity"></param>
        /// <param name="fileUploadMasterEntity"></param>
        private void ParseReadOuts(string fileText, MeterDataEntity mtrDataEntity, FileUploadMasterEntity fileUploadMasterEntity)
        {
            readOuts = new Dictionary<ReadOutItem, Collection<ReadOutCounterEntity>>();
            //Split the File text into the readouts.
            string strSplitTxt = string.Empty;
            switch (UtilityDetails.UtilityName)
            {
                case UtilityEntity.TNEB: strSplitTxt = "HD/";
                                        break;
                case UtilityEntity.UGVCL   : strSplitTxt = "NP";
                                        break;
                case UtilityEntity.JDVVNL: strSplitTxt = "NP";
                                        break;
                case UtilityEntity.PVVNL: strSplitTxt = "NP";
                                        break;
                case UtilityEntity.WBEXPORTVCL: strSplitTxt = "NP";
                                        break;
                default: strSplitTxt = "HD/";
                                        break;
            }
            /*GKG 03/04/2013 137643*/
            /// This code never worked for back ward compatibilty.
            /// Now i am doing patch work and it now works for old PVVNL files 
            /// where NP tag is not coming.
            //string[] readOutsList = fileText.Split(new string[] { strSplitTxt }, StringSplitOptions.RemoveEmptyEntries);
            bool isOldFormatFile = false;
            string[] readOutsList = null;
            if (UtilityDetails.UtilityName == UtilityEntity.PVVNL)
            {
                if (fileText.Contains("NP"))
                {
                    readOutsList = fileText.Split(new string[] { strSplitTxt }, StringSplitOptions.RemoveEmptyEntries);
                    isOldFormatFile = false;
                }
                else if (fileText.Contains("RD"))
                {
                    isOldFormatFile = true;
                    readOutsList = SplitData(fileText, "RD", true);
                }
                else
                {
                    readOutsList = fileText.Split(new string[] { strSplitTxt }, StringSplitOptions.RemoveEmptyEntries);
                    isOldFormatFile = false;
                }
            }
            else
            {
                readOutsList = fileText.Split(new string[] { strSplitTxt }, StringSplitOptions.RemoveEmptyEntries);
            }
            /*GKG 03/04/2013 137643*/
            int i = 0;
                //Parse Each Read out to Set ReadOutCounterEntity in the dictionary.
                for (; i < readOutsList.Length; i++)
                {
                    if (readOutsList[i].Trim().Length == 0) continue;
                    string tmpMtrID=string.Empty;

                    if (UtilityDetails.UtilityName == UtilityEntity.TNEB || UtilityDetails.UtilityName == UtilityEntity.Any)
                    {
                        if (i == 0) i++;
                        readOutsList[i] = "HD/" + readOutsList[i];
                        //Get Meter ID
                        tmpMtrID = readOutsList[i].Substring(readOutsList[i].IndexOf("HD/") + 3);
                        tmpMtrID = tmpMtrID.Substring(4, tmpMtrID.IndexOf("/") - 4);
                    }
                    else
                    {
                        /*GKG 03/04/2013 137643*/
                        if (isOldFormatFile)
                        {
                            if (i == 0) i++; //to skip the first blank space 
                        }
                        /*GKG 03/04/2013 137643*/
                        tmpMtrID = readOutsList[i].Substring(readOutsList[i].IndexOf("/")+1);
                        tmpMtrID = tmpMtrID.Substring(4, tmpMtrID.IndexOf("/") - 4);
                    }
                    //Get Reading Date Time
                    string tmpreadingDateTime = readOutsList[i].Substring(readOutsList[i].IndexOf(tmpMtrID) + 1 + tmpMtrID.Length);
                    tmpreadingDateTime = tmpreadingDateTime.Substring(0, tmpreadingDateTime.IndexOf("/"));

                    //Insert meter details specific to this read out in DB and 
                    //Get Meter Data ID for this readout.
                    mtrDataEntity.MeterID = tmpMtrID;
                    mtrDataEntity.ReadingDateTime = Convert.ToInt64(tmpreadingDateTime);
                    mtrDataEntity = new MeterDataBLL().InsertData(mtrDataEntity) as MeterDataEntity;
                    //Parse the readout items and store there mapping with meter data id and reading date time.
                     /*GKG 03/04/2013 137643*/
                    if (isOldFormatFile)
                    {
                        ParseReadOutItemsOldFormat(readOutsList[i], mtrDataEntity.MeterData_ID);
                    }
                    else
                    {
                        ParseReadOutItems(readOutsList[i], mtrDataEntity.MeterData_ID);
                    }
                    /*GKG 03/04/2013 137643*/
                }
        }
        #endregion

        #region ParseReadOutItems
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
            if (counter > 0 && GeneralData.Length >0)
                CreateReadOutItemDictionary("RD", readOut, MeterData_ID);
            //Create Fraud Energy Data Mapping
            if(readOut.Contains("MI"))
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
        }
        /// <summary>
        /// Parse each readout for Read item tag.
        /// </summary>
        /// <param name="readOut"></param>
        /// <param name="MeterData_ID"></param>
        private void ParseReadOutItemsOldFormat(string readOut, Int64 MeterData_ID)
        {
            //Create General Data Mapping
            if (readOut.Contains("RD") )
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
        }
        #endregion

        #region CreateReadOutItemDictionary
        /// <summary>
        /// Create mapping of readout item with meter data id.
        /// </summary>
        /// <param name="itemTag"></param>
        /// <param name="readOut"></param>
        /// <param name="meterDataID"></param>
        private void CreateReadOutItemDictionary(string itemTag,string readOut,Int64 meterDataID)
        {
            ReadOutItem readOutItem=ReadOutItem.None;
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
            }
            if (!readOuts.ContainsKey(readOutItem))
            {
                collectionItemReadOuts = new Collection<ReadOutCounterEntity>();
                readOuts.Add(readOutItem,collectionItemReadOuts);
            }
           
            ReadOutCounterEntity readOutCounterEntity = new ReadOutCounterEntity();
            //Get meter id of the ReadOutItem.
            string tmpMtrID = readOut.Substring(readOut.IndexOf(itemTag+"/")+1 + itemTag.Length);
            readOutCounterEntity.meterID = tmpMtrID.Substring(4, tmpMtrID.IndexOf("/") - 4);
            //Get Reading Date Time of the ReadOutItem.
            string tmpreadingDateTime = tmpMtrID.Substring(tmpMtrID.IndexOf(readOutCounterEntity.meterID) + 1 + readOutCounterEntity.meterID.Length);
            tmpreadingDateTime = tmpreadingDateTime.Substring(0, tmpreadingDateTime.IndexOf("/"));

            //Create Mapping of Reading Date Time & Meter Data ID of the ReadOutItem in dictionary.
            readOutCounterEntity.readingDateTime = Convert.ToInt64(tmpreadingDateTime);
            readOutCounterEntity.meterDataID = meterDataID;
            
            readOuts[readOutItem].Add(readOutCounterEntity);
        }
        #endregion
        
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
                if (collectionItemReadOuts[i].meterID == meterID && collectionItemReadOuts[i].readingDateTime == readingDateTime)
                    return collectionItemReadOuts[i].meterDataID;
            }
            return -1;
        }
        #endregion


        private void DeleFile(string fileName)
        {
            MeterDataBLL meterDataBLL = new MeterDataBLL();
            InstantPowerBLL instantPowerBLL = new InstantPowerBLL();
            GeneralBLL generalBLL = new GeneralBLL();
            BillingBLL billingBLL = new BillingBLL();
            TariffBLL tariffBLL = new TariffBLL();
            TamperGeneralBLL tamperGeneralBLL = new TamperGeneralBLL();
            FraudEnergyBLL fraudEnergyBLL = new FraudEnergyBLL();
            ProgrammingBLL programmingBLL = new ProgrammingBLL();
            RTCUpdateBLL rTCUpdateBLL = new RTCUpdateBLL();
            PhasorBLL phasorBLL = new PhasorBLL();
            LoadSurveyBLL loadSurveyBLL = new LoadSurveyBLL();
            LoadSurveyParameterBLL loadSurveyParameterBLL = new LoadSurveyParameterBLL();
            TamperSnapShotBLL tamperSnapShotBLL = new TamperSnapShotBLL();
            TamperCounterBLL tamperCounterBLL = new TamperCounterBLL();
            DTMLoadSurveyBLL dTMLoadSurveyBLL = new DTMLoadSurveyBLL();
            DTMDailyProfileBLL dTMDailyProfileBLL = new DTMDailyProfileBLL();
            DTMDailyProfileParameterBLL dTMDailyProfileParameterBLL = new DTMDailyProfileParameterBLL();
            MeterDataHeaderInfoBLL meterDataHeaderInfoBLL = new MeterDataHeaderInfoBLL();
            FileUploadMasterBLL fileUploadMasterBLL = new FileUploadMasterBLL();
            CAB.Entity.FileUploadMasterEntity fileUploadMasterEntity = fileUploadMasterBLL.ValidateFile(fileName) as CAB.Entity.FileUploadMasterEntity;
            long meterDataId = meterDataBLL.GetMeterDataID(fileUploadMasterEntity.FileUpload_ID);
            fileUploadMasterBLL.DeleteData(fileUploadMasterEntity);
            meterDataBLL.DeleteData(meterDataId);
            instantPowerBLL.DeleteData(meterDataId);
            generalBLL.DeleteData(meterDataId);
            billingBLL.DeleteData(meterDataId);
            tariffBLL.DeleteData(meterDataId);
            tamperGeneralBLL.DeleteData(meterDataId);
            fraudEnergyBLL.DeleteData(meterDataId);
            programmingBLL.DeleteData(meterDataId);
            rTCUpdateBLL.DeleteData(meterDataId);
            phasorBLL.DeleteData(meterDataId);
            loadSurveyBLL.DeleteData(meterDataId);
            loadSurveyParameterBLL.DeleteData(meterDataId);
            tamperSnapShotBLL.DeleteData(meterDataId);
            tamperCounterBLL.DeleteData(meterDataId);
            dTMLoadSurveyBLL.DeleteData(meterDataId);
            dTMDailyProfileBLL.DeleteData(meterDataId);
            dTMDailyProfileParameterBLL.DeleteData(meterDataId);
            meterDataHeaderInfoBLL.DeleteData(Convert.ToString(meterDataId));
            meterDataBLL.DeleteDataBasedOnFileID(fileUploadMasterEntity.FileUpload_ID);
        }
       
        #region Upload File

        /// <summary>
        /// Used to Upload 2NG file by inserting file data into database tables.
        /// </summary>
        /// <param name="fileName"></param> input file name to be uploaded 
        /// <returns></returns> true if file uploaded successfully else false
        public bool Upload2NGFile(string fileName)
        {
            List<string> meterIDsUploaded = new List<string>();
            FileUploadMasterEntity fileUploadMasterEntity = new FileUploadMasterEntity();
            MeterDataEntity mtrDataEntity = new MeterDataEntity();
            DateTime readingDateTime = DateTime.Now;
            bool isUploaded = false;
            try
            {               
                this.Cursor = Cursors.WaitCursor;                
                this.StatusMessageAsync = resourceMgr.GetString("Uploading") + resourceMgr.GetString("Filetxt") + Path.GetFileName(fileName);
                Application.DoEvents();
                if (!isUploaded)
                {
                    if (false) //Validate data handled later if required
                    {
                        this.StatusMessageAsync = resourceMgr.GetString("CorruptFile");
                        Application.DoEvents();
                    }
                    else
                    {
                        if (false) // Checksum code written later
                        {
                            this.StatusMessageAsync = resourceMgr.GetString("BccError"); //"BCC mismatched.";
                            Application.DoEvents();
                        }
                        else
                        {
                            fileUploadMasterEntity = new FileUploadMasterEntity();
                            fileUploadMasterEntity.UserInformation_ID = ConfigInfo.UserInformationID;
                            fileUploadMasterEntity.FileContent = TotalBytes(fileName);
                            fileUploadMasterEntity.FileName = Path.GetFileName(fileName);
                            fileUploadMasterEntity.UploadingDateTime = DateUtility.DateTimeToLong(DateTime.Now);
                            fileUploadMasterEntity.FileType = "DLMS";
                            //Insert data to file upload master
                            FileUploadMasterEntity fileEntity = fileUploadMasterBLL.ValidateFile(fileUploadMasterEntity.FileName) as FileUploadMasterEntity;
                            if (fileEntity.FileUpload_ID !=  0)
                            {
                                if (fileEntity.FileUpload_ID != 0)
                                {
                                    fileExists = true;
                                    this.StatusMessageAsync = "File '" + fileEntity.FileName + "' already exist.";
                                    Application.DoEvents();

                                }
                            }
                            else
                            {
                                fileUploadMasterEntity = fileUploadMasterBLL.InsertData(fileUploadMasterEntity) as FileUploadMasterEntity;
                                if (fileUploadMasterEntity.FileUpload_ID == 0)
                                {
                                    this.StatusMessageAsync = "Please Contact system administrator. Invalid DB Structure.";
                                    Application.DoEvents();
                                    return false;
                                }                                                                     

                                List<BillingGeneralNFEntity> master = new List<BillingGeneralNFEntity>();                                                          
                                string fileContent = Get2NGFileContent(fileName);
                                string[] individualFileContent = fileContent.Split('$');
                                foreach (string content in individualFileContent)
                                {
                                    if (!string.IsNullOrEmpty(content) && content != "\r\n")
                                    {
                                        readingDateTime = DateTime.ParseExact(content.Substring(0, 22).Trim('\r').Trim('\n'), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                                        BillingGeneralNFEntity outputEntity = GetFinalProfileEntity(content.Substring(23));

                                        try
                                        {
                                            #region Meter Data and  Name Plate Detail
                                            if (outputEntity.listNamePlateDetail != null)
                                            {

                                                mtrDataEntity.FileUpload_ID = fileUploadMasterEntity.FileUpload_ID;
                                                mtrDataEntity.UploadingDateTime = DateUtility.DateTimeToLong(System.DateTime.Now);
                                                mtrDataEntity.CMRIID = this.cmriID;/////////////newly added
                                                mtrDataEntity.MeterID = outputEntity.listNamePlateDetail[0].MeterID;
                                                mtrDataEntity.ReadingDateTime = DateUtility.DateTimeToLong(readingDateTime);
                                                mtrDataEntity = new MeterDataBLL().InsertData(mtrDataEntity) as MeterDataEntity;

                                                this.StatusMessageAsync = resourceMgr.GetString("UploadingNamePlateDetail");// " Uploading Name Plate Detail";
                                                Application.DoEvents();
                                                NamePlateDetailEntity namePlateDetailEntity = outputEntity.listNamePlateDetail[0];

                                                namePlateDetailEntity.MeterData_ID = mtrDataEntity.MeterData_ID;

                                                namePlateDetailEntity = new NamePlateDetailBLL().InsertData(namePlateDetailEntity) as NamePlateDetailEntity;
                                            }
                                            #endregion

                                            #region HeaderInfo
                                            if (outputEntity.listHeaderInfo != null)
                                            {
                                                this.StatusMessageAsync = resourceMgr.GetString("UploadingHeaderInfo");
                                                Application.DoEvents();
                                                MeterDataHeaderInfoEntity headerInfoEntity = outputEntity.listHeaderInfo[0];
                                                headerInfoEntity.meterId = outputEntity.listNamePlateDetail[0].MeterID;
                                                headerInfoEntity.MeterData_ID = mtrDataEntity.MeterData_ID;
                                                headerInfoEntity = meterDataHeaderInfoBLL.InsertData(headerInfoEntity) as MeterDataHeaderInfoEntity;
                                            }
                                            #endregion

                                            #region Instnat
                                            if (outputEntity.listGeneralData != null && outputEntity.listGeneralData.Count > 0
                                                && outputEntity.listGeneralData[0].CurrentInstant != null)
                                            {
                                                this.StatusMessageAsync = resourceMgr.GetString("UploadingInstantPowerData");
                                                Application.DoEvents();
                                                InstantPowerEntity instantPowerEntity = outputEntity.listGeneralData[0].CurrentInstant;
                                                instantPowerEntity.MeterID = outputEntity.listNamePlateDetail[0].MeterID;
                                                instantPowerEntity.MeterData_ID = mtrDataEntity.MeterData_ID;
                                                instantPowerEntity = instantPowerBLL.InsertData(instantPowerEntity) as InstantPowerEntity;
                                            }
                                            #endregion

                                            #region General
                                            if (outputEntity.listGeneralData != null)
                                            {
                                                GeneralEntity generalEntity = outputEntity.listGeneralData[0].CurrentGeneral;
                                                generalEntity.MeterID = outputEntity.listNamePlateDetail[0].MeterID;
                                                generalEntity.MeterData_ID = mtrDataEntity.MeterData_ID;
                                                generalEntity = generalBLL.InsertData(generalEntity) as GeneralEntity;
                                            }
                                            #endregion

                                            #region Current Billing
                                            if (outputEntity.listGeneralData != null && outputEntity.listGeneralData.Count > 0)
                                            {
                                                BillingEntity billingEntity = outputEntity.listGeneralData[0].CurrentBilling;
                                                billingEntity.MeterData_ID = mtrDataEntity.MeterData_ID;
                                                billingEntity = billingBLL.InsertData(billingEntity) as BillingEntity;
                                            }
                                            #endregion

                                            #region Billing History
                                            if (outputEntity.listGeneralData != null && outputEntity.listGeneralData.Count > 0
                                                && outputEntity.listGeneralData[0].listHistoryBilling != null)
                                            {
                                                this.StatusMessageAsync = resourceMgr.GetString("UploadingBillingHistoryData");
                                                Application.DoEvents();
                                                List<IEntity> entities = new List<IEntity>();
                                                for (int counter = 1; counter < outputEntity.listGeneralData[0].listHistoryBilling.Count; counter++)
                                                {
                                                    if (outputEntity.listGeneralData[0].listHistoryBilling[counter] != null)
                                                    {
                                                        outputEntity.listGeneralData[0].listHistoryBilling[counter].MeterData_ID = mtrDataEntity.MeterData_ID;
                                                        entities.Add(outputEntity.listGeneralData[0].listHistoryBilling[counter]);
                                                    }
                                                }
                                                if (entities.Count > 0)
                                                {
                                                    billingBLL.InsertData(entities);
                                                }
                                            }

                                            #endregion

                                            #region Current Tarrif
                                            if (outputEntity.listGeneralData != null && outputEntity.listGeneralData.Count > 0)
                                            {
                                                TariffEntity tarrifEntity = outputEntity.listGeneralData[0].CurrentTariff;
                                                tarrifEntity.HistoryID = 0;
                                                tarrifEntity.MeterData_ID = mtrDataEntity.MeterData_ID;
                                                tarrifEntity = tariffBLL.InsertData(tarrifEntity) as TariffEntity;
                                            }
                                            #endregion

                                            #region Tarrif History
                                            if (outputEntity.listGeneralData != null && outputEntity.listGeneralData.Count > 0
                                                && outputEntity.listGeneralData[0].listHistoryTariff != null)
                                            {
                                                this.StatusMessageAsync = resourceMgr.GetString("UploadingTariffHistoryData"); //" Uploading Tariff History Data";
                                                Application.DoEvents();
                                                List<IEntity> entities = new List<IEntity>();

                                                for (int counter = 0; counter < outputEntity.listGeneralData[0].listHistoryTariff.Count; counter++)
                                                {
                                                    if (outputEntity.listGeneralData[0].listHistoryTariff[counter] != null)
                                                    {
                                                        outputEntity.listGeneralData[0].listHistoryTariff[counter].MeterData_ID = mtrDataEntity.MeterData_ID;
                                                        entities.Add(outputEntity.listGeneralData[0].listHistoryTariff[counter]);
                                                    }
                                                }
                                                if (entities.Count > 0)
                                                {
                                                    tariffBLL.InsertData(entities);
                                                }
                                            }
                                            #endregion

                                            #region Current Tamper
                                            if (outputEntity.listGeneralData != null && outputEntity.listGeneralData.Count > 0)
                                            {
                                                TamperCounterGeneralEntity tamperCounterEntity = outputEntity.listGeneralData[0].CurrentTamper;
                                                tamperCounterEntity.MeterData_ID = mtrDataEntity.MeterData_ID;
                                                tamperCounterEntity = tamperGeneralBLL.InsertData(tamperCounterEntity) as TamperCounterGeneralEntity;
                                            }
                                            #endregion

                                            #region Billing History Tamper
                                            if (outputEntity.listGeneralData != null && outputEntity.listGeneralData.Count > 0
                                                && outputEntity.listGeneralData[0].listHistoryTamper != null)
                                            {
                                                this.StatusMessageAsync = resourceMgr.GetString("UploadingTamperHistoryData");
                                                Application.DoEvents();
                                                List<IEntity> entities = new List<IEntity>();
                                                for (int counter = 1; counter < outputEntity.listGeneralData[0].listHistoryTamper.Count; counter++)
                                                {
                                                    if (outputEntity.listGeneralData[0].listHistoryTamper[counter] != null)
                                                    {
                                                        outputEntity.listGeneralData[0].listHistoryTamper[counter].MeterData_ID = mtrDataEntity.MeterData_ID;
                                                        entities.Add(outputEntity.listGeneralData[0].listHistoryTamper[counter]);
                                                    }
                                                }
                                                if (entities.Count > 0)
                                                {
                                                    tamperGeneralBLL.InsertData(entities);
                                                }
                                            }

                                            #endregion

                                            #region LoadSurvey
                                            if (outputEntity.listLoadSurveyData != null && outputEntity.listLoadSurveyData.Count > 0)
                                            {
                                                this.StatusMessageAsync = resourceMgr.GetString("UploadingLoadSurveyData");
                                                Application.DoEvents();
                                                List<IEntity> entities = new List<IEntity>();
                                                foreach (LoadSurveyEntity loadSurveyEntity in outputEntity.listLoadSurveyData[0].LoadSurvey)
                                                {
                                                    loadSurveyEntity.MeterData_ID = mtrDataEntity.MeterData_ID;
                                                    loadSurveyEntity.MeterID = outputEntity.listNamePlateDetail[0].MeterID;
                                                    //set to 1 for DLMS meters
                                                    loadSurveyEntity.IsDLMS = 1;
                                                    entities.Add(loadSurveyEntity);
                                                }

                                                loadSurveyBLL.InsertData(entities);
                                                LoadSurveyParameterBLL loadSurveyParameterBLL = new LoadSurveyParameterBLL();
                                                LoadSurveyParameterEntity loadSurveyParameterEntity = new LoadSurveyParameterEntity();
                                                loadSurveyParameterEntity.MeterDataId = mtrDataEntity.MeterData_ID;
                                                if (outputEntity.listLoadSurveyData.Count > 0 && outputEntity.listLoadSurveyData[0].LoadSurvey.Count > 0)
                                                {
                                                    loadSurveyParameterEntity.ColumnsNames = outputEntity.listLoadSurveyData[0].LoadSurvey[0].Parameters;
                                                    loadSurveyParameterBLL.InsertData(loadSurveyParameterEntity);
                                                }
                                            }
                                            #endregion

                                            #region Midnight
                                            if (outputEntity.listDTMDailyProfileData != null && outputEntity.listDTMDailyProfileData.Count > 0
                                                && outputEntity.listDTMDailyProfileData[0].DTMDailyProfile.Count > 0)
                                            {
                                                this.StatusMessageAsync = resourceMgr.GetString("UploadingdailyProfileData");
                                                Application.DoEvents();
                                                List<IEntity> entities = new List<IEntity>();
                                                foreach (DTMDailyProfileEntity midnightEntity in outputEntity.listDTMDailyProfileData[0].DTMDailyProfile)
                                                {
                                                    midnightEntity.MeterData_ID = mtrDataEntity.MeterData_ID;
                                                    midnightEntity.MeterID = outputEntity.listNamePlateDetail[0].MeterID;
                                                    entities.Add(midnightEntity);
                                                }

                                                dTMDailyProfileBLL.InsertData(entities);
                                                DTMDailyProfileParameterBLL midnightParameterBLL = new DTMDailyProfileParameterBLL();
                                                DTMDailyProfileParameterEntity midnightParameterEntity = new DTMDailyProfileParameterEntity();
                                                midnightParameterEntity.MeterDataId = mtrDataEntity.MeterData_ID;
                                                midnightParameterEntity.ColumnsNames = outputEntity.listDTMDailyProfileData[0].DTMDailyProfile[0].Parameters;
                                                midnightParameterBLL.InsertData(midnightParameterEntity);
                                            }
                                            #endregion

                                            #region Phasor
                                            if (outputEntity.listPhasor != null && outputEntity.listPhasor.Count > 0)
                                            {
                                                this.StatusMessageAsync = resourceMgr.GetString("UploadingPhasorData");
                                                Application.DoEvents();
                                                PhasorEntity phasorEntity = outputEntity.listPhasor[0] as PhasorEntity;

                                                phasorEntity.MeterData_ID = mtrDataEntity.MeterData_ID;
                                                phasorEntity.MeterID = outputEntity.listNamePlateDetail[0].MeterID;
                                                PhasorBLL phasorBLL = new PhasorBLL();
                                                phasorEntity = phasorBLL.InsertData(phasorEntity) as PhasorEntity;

                                            }
                                            #endregion

                                            #region FraudEnergy
                                            if (outputEntity.listFraudEnergy != null && outputEntity.listFraudEnergy.Count > 0)
                                            {
                                                this.StatusMessageAsync = resourceMgr.GetString("UploadingFraudEnergyData");
                                                Application.DoEvents();
                                                FraudEnergyEntity farudEnergyEntity = outputEntity.listFraudEnergy[0] as FraudEnergyEntity;

                                                farudEnergyEntity.MeterData_ID = mtrDataEntity.MeterData_ID;
                                                farudEnergyEntity.MeterID = outputEntity.listNamePlateDetail[0].MeterID;
                                                FraudEnergyBLL farudEnergyrBLL = new FraudEnergyBLL();
                                                farudEnergyEntity = farudEnergyrBLL.InsertData(farudEnergyEntity) as FraudEnergyEntity;

                                            }
                                            #endregion

                                            #region Transaction
                                            if (outputEntity.listTransactionData != null && outputEntity.listTransactionData.Count > 0)
                                            {
                                                this.StatusMessageAsync = resourceMgr.GetString("UploadingTransactionData");
                                                Application.DoEvents();
                                                TransactionData transactionData = outputEntity.listTransactionData[0] as TransactionData;
                                                List<IEntity> entities = new List<IEntity>();
                                                foreach (ProgrammingEntity programmingEntity in outputEntity.listTransactionData[0].programmingData)
                                                {
                                                    programmingEntity.MeterData_ID = mtrDataEntity.MeterData_ID;
                                                    programmingEntity.MeterID = outputEntity.listNamePlateDetail[0].MeterID;
                                                    entities.Add(programmingEntity);
                                                }
                                                programmingBLL.InsertData(entities);

                                            }
                                            #endregion

                                            #region Tamper
                                            if (outputEntity.listTamper != null && outputEntity.listTamper.Count > 0)
                                            {
                                                this.StatusMessageAsync = resourceMgr.GetString("UploadingTamperData");
                                                Application.DoEvents();
                                                TamperData tamperData = outputEntity.listTamper[0] as TamperData;
                                                List<IEntity> entities = new List<IEntity>();
                                                foreach (TamperSnapshotEntity entity in outputEntity.listTamper[0].Snapshot)
                                                {
                                                    entity.MeterData_ID = mtrDataEntity.MeterData_ID;
                                                    entities.Add(entity);
                                                }
                                                outputEntity.listTamper[0].General.MeterData_ID = mtrDataEntity.MeterData_ID;
                                                tamperSnapShotBLL.InsertData(entities);
                                            }
                                            if (outputEntity.listTamper != null && outputEntity.listTamper.Count > 0)
                                            {
                                                this.StatusMessageAsync = resourceMgr.GetString("UploadingTamperData");
                                                Application.DoEvents();
                                                TamperData tamperData = outputEntity.listTamper[0] as TamperData;
                                                List<IEntity> entities = new List<IEntity>();
                                                outputEntity.listTamper[0].General.MeterData_ID = mtrDataEntity.MeterData_ID;
                                                entities.Add(outputEntity.listTamper[0].General);
                                                //outputEntity.listTamper[0].General.MeterData_ID = mtrDataEntity.MeterData_ID;
                                                tamperGeneralBLL.InsertData(entities);
                                            }
                                            if (outputEntity.listTamper != null && outputEntity.listTamper.Count > 0)
                                            {
                                                this.StatusMessageAsync = resourceMgr.GetString("UploadingTamperData");
                                                Application.DoEvents();
                                                TamperData tamperData = outputEntity.listTamper[0] as TamperData;
                                                outputEntity.listTamper[0].Counter.MeterData_ID = mtrDataEntity.MeterData_ID;
                                                tamperCounterBLL.InsertData(outputEntity.listTamper[0].Counter);
                                            }

                                            #endregion

                                            #region RTC Updates
                                            if (outputEntity.listRTCUpdate != null && outputEntity.listRTCUpdate.Count > 0)
                                            {
                                                this.StatusMessageAsync = resourceMgr.GetString("UploadingTamperData");
                                                Application.DoEvents();
                                                RTCUpdateEntity rtcUpdateEntity = outputEntity.listRTCUpdate[0] as RTCUpdateEntity;
                                                rtcUpdateEntity.MeterData_ID = mtrDataEntity.MeterData_ID;
                                                rtcUpdateEntity.MeterID = mtrDataEntity.MeterID;
                                                rTCUpdateBLL.InsertData(rtcUpdateEntity);
                                            }
                                            #endregion

                                            #region MeterConfiguration

                                            if (outputEntity.meterConfigurationDetail != null && outputEntity.meterConfigurationDetail.Count > 0)
                                            {
                                                #region RTC
                                                if (!string.IsNullOrEmpty(outputEntity.meterConfigurationDetail[0].RTC))
                                                {
                                                    new RTCBLL().InsertData(outputEntity.meterConfigurationDetail[0].RTC, outputEntity.listNamePlateDetail[0].MeterID,
                                                                         fileUploadMasterEntity.FileUpload_ID, mtrDataEntity.MeterData_ID);
                                                }
                                                #endregion

                                                #region BillingType
                                                if (outputEntity.meterConfigurationDetail[0].billingresetentity != null)
                                                {
                                                    BillingResetEntity billingType = outputEntity.meterConfigurationDetail[0].billingresetentity;
                                                    billingType.MeterID = outputEntity.listNamePlateDetail[0].MeterID;
                                                    new BillingResetBLL().Insertdata(billingType, fileUploadMasterEntity.FileUpload_ID, mtrDataEntity.MeterData_ID);
                                                }

                                                #endregion

                                                #region KvahSelection
                                                if (outputEntity.meterConfigurationDetail[0].kvarselectionEntity != null)
                                                {
                                                    kvarSelectionEntity kvahSelection = outputEntity.meterConfigurationDetail[0].kvarselectionEntity;
                                                    kvahSelection.MeterID = outputEntity.listNamePlateDetail[0].MeterID;
                                                    new kvarSelectionBLL().InsertData(kvahSelection, fileUploadMasterEntity.FileUpload_ID, mtrDataEntity.MeterData_ID);
                                                }
                                                #endregion

                                                #region RS232LockUnlock
                                                if (outputEntity.meterConfigurationDetail[0].RS232Entity != null)
                                                {
                                                    RS232LockEntity rs232LockEntity = outputEntity.meterConfigurationDetail[0].RS232Entity;
                                                    rs232LockEntity.MeterID = outputEntity.listNamePlateDetail[0].MeterID;
                                                    new RS232BLL().Insertdata(rs232LockEntity, fileUploadMasterEntity.FileUpload_ID, mtrDataEntity.MeterData_ID);
                                                }
                                                #endregion

                                                #region MD with IP
                                                if (outputEntity.meterConfigurationDetail[0].mdWithIPEntity != null)
                                                {
                                                    MDWithIPEntity dipEntity = outputEntity.meterConfigurationDetail[0].mdWithIPEntity;
                                                    dipEntity.MeterID = outputEntity.listNamePlateDetail[0].MeterID;
                                                    new MDWithIPBLL().InsertData(dipEntity, fileUploadMasterEntity.FileUpload_ID, mtrDataEntity.MeterData_ID);
                                                }
                                                #endregion

                                                #region DailyLog
                                                if (outputEntity.meterConfigurationDetail[0].dailylogentity != null)
                                                {
                                                    DailyLogEntity dailyLogEntity = outputEntity.meterConfigurationDetail[0].dailylogentity;
                                                    dailyLogEntity.MeterID = outputEntity.listNamePlateDetail[0].MeterID;
                                                    new DailyLogBLL().Insertdata(dailyLogEntity, fileUploadMasterEntity.FileUpload_ID, mtrDataEntity.MeterData_ID);
                                                }
                                                #endregion

                                                #region Display Parameter
                                                if (outputEntity.meterConfigurationDetail[0].DisplayParamater != null
                                                    && outputEntity.meterConfigurationDetail[0].DisplayParamater.Count > 0)
                                                {
                                                    new DisplayParametersBLL().InsertData(outputEntity.meterConfigurationDetail[0].DisplayParamater, outputEntity.listNamePlateDetail[0].MeterID
                                                        , fileUploadMasterEntity.FileUpload_ID, mtrDataEntity.MeterData_ID);
                                                }
                                                #endregion

                                                #region AutoLock
                                                if (outputEntity.meterConfigurationDetail[0].AutoLockEntity != null)
                                                {
                                                    AutoLockEntity autoLockEntity = outputEntity.meterConfigurationDetail[0].AutoLockEntity;
                                                    autoLockEntity.MeterID = outputEntity.listNamePlateDetail[0].MeterID;
                                                    new AutoLockBLL().Insertdata(autoLockEntity, fileUploadMasterEntity.FileUpload_ID, mtrDataEntity.MeterData_ID);
                                                }
                                                #endregion
                                                #region TOD
                                                if (!string.IsNullOrEmpty(todData) && todData.ToUpper().Contains("DLMS"))
                                                {
                                                    new TODBLL().InsertData(todData, outputEntity.listNamePlateDetail[0].MeterID, fileUploadMasterEntity.FileUpload_ID, mtrDataEntity.MeterData_ID);
                                                }
                                                #endregion
                                            }

                                            #endregion

                                        }
                                        catch (Exception ex)
                                        {
                                            MessageBox.Show(ex.ToString());
                                        }
                                    }
                                }
                                this.StatusMessageAsync = resourceMgr.GetString("Filetxt") + Path.GetFileName(fileName) + resourceMgr.GetString("UploadedSuccessfully");
                                Application.DoEvents();
                                this.ListRefreshAsync = true;
                                isUploaded = true;                                
                            }
                        }
                    }

                }
                
            }
            catch (Exception ex)
            {            
                fileUploadMasterBLL.DeleteData(fileUploadMasterEntity);
                this.StatusMessageAsync = "File corrupted.";
                Application.DoEvents();                
            }
            return isUploaded;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileText"></param>
        /// <returns></returns>
        private BillingGeneralNFEntity GetFinalProfileEntity(string fileText)
        {
            GeneralData generalData = new GeneralData();
            GeneralEntity generalEntity = new GeneralEntity();
            Collection<DisplayParamatersDBEntity> collDisplayParamatersDBEntity = new Collection<DisplayParamatersDBEntity>();
            
            MeterDataHeaderInfoEntity headerEntity = new MeterDataHeaderInfoEntity();
            BillingGeneralNFEntity billingGeneralNFEntity = new BillingGeneralNFEntity();
            billingGeneralNFEntity.meterConfigurationDetail = new List<MeterConfigurationsNFEntity>();
            MeterConfigurationsNFEntity meterConfigurationEntity = new MeterConfigurationsNFEntity();
            List<ProfileData> namePlateData = new List<ProfileData>();
            List<ProfileData> instantData = new List<ProfileData>();
            List<ProfileData> loadSurveyData = new List<ProfileData>();
            List<ProfileData> midnightData = new List<ProfileData>();
            List<ProfileData> phasorData = new List<ProfileData>();
            List<ProfileData> fraudEnergyData = new List<ProfileData>();            
            List<ProfileData> tamperData = new List<ProfileData>();
            List<ProfileData> billingData = new List<ProfileData>();
            List<ProfileData> rtcData = new List<ProfileData>();
            List<ProfileData> sipData = new List<ProfileData>();
            List<ProfileData> billingTypeData = new List<ProfileData>();
            List<ProfileData> kvahSelectionData = new List<ProfileData>();
            List<ProfileData> rs232LockData = new List<ProfileData>();       
            List<ProfileData> meterConfigurationData = new List<ProfileData>();
            List<ProfileData> demandWithIPData = new List<ProfileData>();
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
            meterConfigurationEntity.DisplayParamater = new Collection<DisplayParamatersDBEntity>();

            List<ProfileData> transactionData = new List<ProfileData>();
            NamePlateDetail mapperNamePlateDetail = new NamePlateDetail();
            HeaderInfo mapperHeaderInfo = new HeaderInfo();
            Instant mapperInstant = new Instant();
            LoadSurvey mapperLoadSurevy = new LoadSurvey();
            Midnight mapperMidnight= new Midnight();
            Phasor mapperPhasor = new Phasor();
            Tamper mapperTamper = new Tamper();
            FraudEnergy mapperFarudEnergy = new FraudEnergy();           
            RealTimeClock  mapperRTC = new RealTimeClock();
            LoadSurveyCapturePeriod mapperSIP = new LoadSurveyCapturePeriod();
            KVAHSelection mapperKVAHSelection = new KVAHSelection();
            RS232LockUnlock mapperRS232LockUnlock = new RS232LockUnlock();            
            Transactions mapperTransaction = new Transactions();
            RTCUpdate mapperRTCUpdates = new RTCUpdate();
            General mapperGeneral = new General();
            Billing mapperBilling = new Billing();
            BillingDateTime mapperBillingType = new BillingDateTime();
            DisplayParameterAndTimeout mapperDisplayParameter = new DisplayParameterAndTimeout();
            DemandIntegrationPeriod mapperDIP = new DemandIntegrationPeriod();
            MDResetLockOutDays mapperResetLockOut = new MDResetLockOutDays();
            DailyLog mapperDailyLod = new DailyLog();
            AutoLock mapperAutoLock = new AutoLock();

            billingGeneralNFEntity.listGeneralData = new List<GeneralData>();
            List<ProfileData> allData = entityGenerator.GetProfileWiseEntityList(fileText,false);

            billingData = GetProfileSpecificList(allData, (int)ProfileId.Billing);
            namePlateData = GetProfileSpecificList(allData, (int)ProfileId.NamePlate);
            tamperData = GetProfileSpecificList(allData, (int)ProfileId.Tamper);
            instantData = GetProfileSpecificList(allData, (int)ProfileId.Instant);
            loadSurveyData = GetProfileSpecificList(allData, (int)ProfileId.LoadSurvey);
            midnightData = GetProfileSpecificList(allData, (int)ProfileId.Midnight);            
            phasorData = GetProfileSpecificList(allData, (int)ProfileId.Phasor);
            fraudEnergyData = GetProfileSpecificList(allData, (int)ProfileId.FraudEnergy);
            sipData = GetProfileSpecificList(allData, (int)ProfileId.SIP);
            kvahSelectionData = GetProfileSpecificList(allData, (int)ProfileId.KvahSelection);
            rs232LockData = GetProfileSpecificList(allData, (int)ProfileId.RS232LockUnlock);
            billingTypeData = GetProfileSpecificList(allData, (int)ProfileId.BillingType);
            pushDisplayParameterData = GetProfileSpecificList(allData, (int)ProfileId.PushDisplayParameter);
            scrollDisplayParameterData = GetProfileSpecificList(allData, (int)ProfileId.ScrollDisplyParameter);
            highResolutionDisplayParameterData = GetProfileSpecificList(allData, (int)ProfileId.HighResolutionDisplayParameter);
            displayTimeoutData = GetProfileSpecificList(allData, (int)ProfileId.DisplayTimeoutParameter);
            demandWithIPData = GetProfileSpecificList(allData, (int)ProfileId.DIP);
            resetLockOutData = GetProfileSpecificList(allData, (int)ProfileId.ResetLockOutDays);
            autoLockData = GetProfileSpecificList(allData, (int)ProfileId.AutoLock);
            passiveSeasonProfile = GetProfileSpecificList(allData, (int)ProfileId.PassiveSeasonProfile);
            passiveWeekProfile = GetProfileSpecificList(allData, (int)ProfileId.PassiveWeekProfile);
            passiveDayProfile = GetProfileSpecificList(allData, (int)ProfileId.PassiveDayProfile);
            activeDayProfile = GetProfileSpecificList(allData, (int)ProfileId.ActiveDayProfile);
            activeSeasonDayProfile = GetProfileSpecificList(allData, (int)ProfileId.ActiveSeasonProfile);
            activeWeekDayProfile = GetProfileSpecificList(allData, (int)ProfileId.ActiveWeekProfile);
            activationDate = GetProfileSpecificList(allData, (int)ProfileId.ActivationDate);
            todData = string.Empty;
            #region MeterConfiguration

            meterConfigurationEntity.RTC = mapperRTC.GetData(instantData);

            meterConfigurationEntity.dailylogentity = mapperDailyLod.GetData();

            if (kvahSelectionData != null && kvahSelectionData.Count > 0) //KVAH Selection
            {
                meterConfigurationEntity.kvarselectionEntity = mapperKVAHSelection.GetData(kvahSelectionData);
            }
            if (passiveDayProfile.Count > 0 && passiveSeasonProfile.Count > 0 && passiveWeekProfile.Count > 0
               && activationDate.Count > 0 && activeWeekDayProfile.Count > 0 && activeSeasonDayProfile.Count > 0
               && activeDayProfile.Count > 0)
            {
                todData = "DLMS" + @"\" + passiveSeasonProfile[0].ListMeterDataPacket[0].ListDataElementValue[0].Value
                           + @"\" + passiveWeekProfile[0].ListMeterDataPacket[0].ListDataElementValue[0].Value
                           + @"\" + passiveDayProfile[0].ListMeterDataPacket[0].ListDataElementValue[0].Value
                           + @"\" + activeSeasonDayProfile[0].ListMeterDataPacket[0].ListDataElementValue[0].Value
                           + @"\" + activeWeekDayProfile[0].ListMeterDataPacket[0].ListDataElementValue[0].Value
                           + @"\" + activeDayProfile[0].ListMeterDataPacket[0].ListDataElementValue[0].Value
                           + @"\" + activationDate[0].ListMeterDataPacket[0].ListDataElementValue[0].Value;
            }
            if (rs232LockData != null && rs232LockData.Count > 0) //RS232 Selection
            {
                meterConfigurationEntity.RS232Entity = mapperRS232LockUnlock.GetData(rs232LockData);
            }
            if (billingTypeData != null && billingTypeData.Count > 0) //RS232 Selection
            {
                meterConfigurationEntity.billingresetentity = mapperBillingType.GetData(billingTypeData);
            }
            if (demandWithIPData != null && demandWithIPData.Count > 0)
            {
                meterConfigurationEntity.mdWithIPEntity = mapperDIP.GetData(demandWithIPData);
            }
            if (pushDisplayParameterData != null && pushDisplayParameterData.Count > 0)
            {
               collDisplayParamatersDBEntity =  mapperDisplayParameter.GetData(pushDisplayParameterData,DisplayParameter.PushMode);
               foreach (DisplayParamatersDBEntity displParameterEntity in collDisplayParamatersDBEntity)
               {
                 meterConfigurationEntity.DisplayParamater.Add(displParameterEntity);
               }
            }
            if (scrollDisplayParameterData != null && scrollDisplayParameterData.Count > 0)
            {
                collDisplayParamatersDBEntity = mapperDisplayParameter.GetData(scrollDisplayParameterData,DisplayParameter.ScrollMode);
                foreach (DisplayParamatersDBEntity displParameterEntity in collDisplayParamatersDBEntity)
                {
                  meterConfigurationEntity.DisplayParamater.Add(displParameterEntity);
                }
            }
            if (highResolutionDisplayParameterData != null && highResolutionDisplayParameterData.Count > 0)
            {
                collDisplayParamatersDBEntity = mapperDisplayParameter.GetData(highResolutionDisplayParameterData,DisplayParameter.HighResolutionMode);
                foreach (DisplayParamatersDBEntity displParameterEntity in collDisplayParamatersDBEntity)
                {
                  meterConfigurationEntity.DisplayParamater.Add(displParameterEntity);
                }
            }
            if (displayTimeoutData != null && displayTimeoutData.Count > 0)
            {
              collDisplayParamatersDBEntity = mapperDisplayParameter.GetData(displayTimeoutData,DisplayParameter.DisplayTimeouts);
              foreach (DisplayParamatersDBEntity displParameterEntity in collDisplayParamatersDBEntity)
               {
                 meterConfigurationEntity.DisplayParamater.Add(displParameterEntity);
               }
            }
            if (resetLockOutData != null && resetLockOutData.Count > 0)
            {
                meterConfigurationEntity.billingresetentity.ResetLockOutDays = mapperResetLockOut.GetData(resetLockOutData);
            }
            if (autoLockData != null && autoLockData.Count > 0)
            {
                meterConfigurationEntity.AutoLockEntity = mapperAutoLock.GetData(autoLockData);
            }
            billingGeneralNFEntity.meterConfigurationDetail.Add(meterConfigurationEntity);
            #endregion

            //Fill NamePlate , HeaderInfo Entity 
            billingGeneralNFEntity.listNamePlateDetail = mapperNamePlateDetail.GetData(namePlateData);
            billingGeneralNFEntity.listHeaderInfo = mapperHeaderInfo.GetData(namePlateData, instantData,fraudEnergyData, namePlateData,
                                                   meterConfigurationEntity.mdWithIPEntity, meterConfigurationEntity.billingresetentity,
                                                   meterConfigurationEntity.kvarselectionEntity, meterConfigurationEntity.AutoLockEntity);// Dummy method 
            if (tamperData.Count > 0) //tamper
            {
                billingGeneralNFEntity.listTamper = mapperTamper.GetData(tamperData);
                billingGeneralNFEntity.listTransactionData = mapperTransaction.GetData(tamperData);
                billingGeneralNFEntity.listRTCUpdate = mapperRTCUpdates.GetData(tamperData);
            }
            //Fill general  and instant Entity         
            if (instantData.Count > 0)
            {
                generalData.CurrentInstant = mapperInstant.GetData(instantData,phasorData,fraudEnergyData);
            }
            //General 
            generalData.CurrentGeneral = mapperGeneral.GetData(instantData, phasorData,fraudEnergyData);                  
            billingGeneralNFEntity.listGeneralData.Add(generalData);

            //Billing 
            if (billingData != null && billingData.Count > 0)
            {
                List<BillingEntity> billingEntity = mapperBilling.GetData(billingData, instantData,fraudEnergyData, 1).Cast<BillingEntity>().ToList();
                List<TariffEntity> tariffEntity = mapperBilling.GetData(billingData,instantData,fraudEnergyData, 2).Cast<TariffEntity>().ToList();
                List<TamperCounterGeneralEntity> tamperCounterEntity = mapperBilling.GetData(billingData, instantData, fraudEnergyData, 3).Cast<TamperCounterGeneralEntity>().ToList();
                generalData.CurrentBilling = billingEntity[0];                
                generalData.listHistoryBilling = billingEntity;
                generalData.CurrentTariff = tariffEntity[0];               
                generalData.listHistoryTariff = tariffEntity;
                //Temp code to make sure that billing energy gets loaded 
                generalData.CurrentTamper = tamperCounterEntity[0];
                generalData.listHistoryTamper = tamperCounterEntity;
            }

            if (loadSurveyData.Count > 0) //loadsurvey
            {
                billingGeneralNFEntity.listLoadSurveyData = mapperLoadSurevy.GetData(loadSurveyData, meterConfigurationEntity.mdWithIPEntity);
            }
            if (midnightData.Count > 0) //midnight
            {
                billingGeneralNFEntity.listDTMDailyProfileData = mapperMidnight.GetData(midnightData);
            }            

            if (phasorData.Count > 0) //phasor
            {
                billingGeneralNFEntity.listPhasor = mapperPhasor.GetData(phasorData);
            }

            if (fraudEnergyData.Count > 0) //FraudEnergy
            {
                billingGeneralNFEntity.listFraudEnergy = mapperFarudEnergy.GetData(fraudEnergyData);
            }           
            //TOD
              
            return billingGeneralNFEntity;

        }
        /// <summary>
        /// Temp method
        /// </summary>
        /// <returns></returns>
        //private List<TamperCounterGeneralEntity> GetTamperCounterEntity(int billingCount)
        //{
        //    List<TamperCounterGeneralEntity> listTamperCounterEntity = new List<TamperCounterGeneralEntity>();
        //    TamperCounterGeneralEntity tamperCounterEntity = null;
        //    for (int index = 0; index < billingCount; index++)
        //    {
        //       tamperCounterEntity = new TamperCounterGeneralEntity();
        //       tamperCounterEntity.BillingCounter = 0;
        //       tamperCounterEntity.History_ID = index;
        //       tamperCounterEntity.RelatedTo = index == 0 ? "G" : "B";
        //       tamperCounterEntity.CTOpenBPhaseTamperCounter = 0;
        //       tamperCounterEntity.CTOpenRPhaseTamperCounter = 0;
        //       tamperCounterEntity.CTOpenYPhaseTamperCounter = 0;
        //       tamperCounterEntity.CTShortTamperCounter= 0;
        //       tamperCounterEntity.CurrentImbalanceBPhaseTamperCounter = 0;
        //       tamperCounterEntity.CurrentImbalanceRPhaseTamperCounter = 0;
        //       tamperCounterEntity.CurrentImbalanceYPhaseTamperCounter = 0;
        //       tamperCounterEntity.CurrentPhaseReversalTamperCounter = 0;
        //       tamperCounterEntity.CurrentReversalBPhaseTamperCounter = 0;
        //       tamperCounterEntity.CurrentReversalRPhaseTamperCounter = 0;
        //       tamperCounterEntity.CurrentReversalYPhaseTamperCounter = 0;
        //       tamperCounterEntity.CurrentWithoutVoltageBPhaseTamperCounter = 0;
        //       tamperCounterEntity.CurrentWithoutVoltageRPhaseTamperCounter = 0;
        //       tamperCounterEntity.CurrentWithoutVoltageYPhaseTamperCounter = 0;
        //       tamperCounterEntity.FrontCoverOpeningTamperCounter = 0;
        //       tamperCounterEntity.HighOverVoltageBPhaseTamperCounter = 0;
        //       tamperCounterEntity.HighOverVoltageRPhaseTamperCounter = 0;
        //       tamperCounterEntity.HighOverVoltageYPhaseTamperCounter = 0;
        //       tamperCounterEntity.LowPowerFactorBPhaseTamperCounter = 0;
        //       tamperCounterEntity.LowPowerFactorRPhaseTamperCounter = 0;
        //       tamperCounterEntity.LowPowerFactorYPhaseTamperCounter = 0;
        //       tamperCounterEntity.LowUnderVoltageBPhaseTamperCounter = 0;
        //       tamperCounterEntity.LowUnderVoltageRPhaseTamperCounter = 0;
        //       tamperCounterEntity.LowUnderVoltageYPhaseTamperCounter = 0;
        //       tamperCounterEntity.MagneticInfluenceTamperCounter = 0;
        //       tamperCounterEntity.MissingPotentialBPhaseTamperCounter = 0;
        //       tamperCounterEntity.MissingPotentialRPhaseTamperCounter = 0;
        //       tamperCounterEntity.MissingPotentialYPhaseTamperCounter = 0;
        //       tamperCounterEntity.NeutralDisturbanceTamperCounter = 0;
        //       tamperCounterEntity.OnePhaseNeutralAbsentTamperCounter = 0;
        //       tamperCounterEntity.ReadingDateTime=0;
        //       tamperCounterEntity.TerminalCoverOpeningTamperCounter = 0;
        //       tamperCounterEntity.VoltagePhaseReversalTamperCounter = 0;
        //       tamperCounterEntity.VoltageImbalanceBPhaseTamperCounter = 0;
        //       tamperCounterEntity.VoltageImbalanceRPhaseTamperCounter = 0;
        //       tamperCounterEntity.VoltageImbalanceYPhaseTamperCounter = 0;

        //       tamperCounterEntity.BillingTimeStamp = 0;

        //       listTamperCounterEntity.Add(tamperCounterEntity);
        //    }

        //    return listTamperCounterEntity;
        //}
        

        /// <summary>
        /// Gets list of all profile's data as input and returns the list of desired profile data
        /// </summary>
        /// <param name="allProfileData"></param>
        /// <param name="profileId"></param>
        /// <returns></returns>
        private List<ProfileData>  GetProfileSpecificList(List<ProfileData> allProfileData, int profileId)
        {
            List<ProfileData> resultData = allProfileData.Where(item => item.ProfileId == profileId).ToList() as List<ProfileData>;
            return resultData;
        }

        /// <summary>
        /// Upload the file into the db based on readouts.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileText"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public bool Upload(string fileName, string fileText, bool flag)
        {
            List<string> meterIDsUploaded = new List<string>();
            FileUploadMasterEntity fileUploadMasterEntity = new FileUploadMasterEntity(); 
            try
            {
               
                this.Cursor = Cursors.WaitCursor;
                bool isUploaded = false;
                this.StatusMessage = resourceMgr.GetString("Uploading") + resourceMgr.GetString("Filetxt") + Path.GetFileName(fileName);
                Application.DoEvents();
                if (!isUploaded)
                {
                    if (!FormatterCommon.IsFileNullOrEmpty(fileText))
                    {
                        this.StatusMessage = resourceMgr.GetString("CorruptFile");
                        Application.DoEvents();
                        return isUploaded;
                    }
                    if (!ConfigInfo.IsValidCheckSum(fileText))
                    {
                        this.StatusMessage = resourceMgr.GetString("BccError"); //"BCC mismatched.";
                        Application.DoEvents();
                        return isUploaded;
                    }
                    for (int retryCount = 0; retryCount < 3; retryCount++)
                    {
                       #region Check File Specific Details & Get File Upload ID if file not exists.
                    
                  
                    fileUploadMasterEntity = new FileUploadMasterEntity();
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
                    fileUploadMasterEntity.FileType = "NONDLMS";
                    if (flag == true)
                    {
                        FileUploadMasterEntity fileEntity = fileUploadMasterBLL.ValidateFile(fileUploadMasterEntity.FileName) as FileUploadMasterEntity;
                       
                        // Need to Add logic for status.                        
                         bool fileStatus = false;                       
                         if (System.IO.File.Exists(csvFileName))
                         {
                             StreamReader sr = new StreamReader(csvFileName);
                             CsvFileReader csv = new CsvFileReader(sr);
                             CsvRow row = new CsvRow();
                             while (csv.ReadRow(row))
                             {                                 
                                     if (row[3] != "Status")
                                     {
                                         if (row[2] == fileUploadMasterEntity.FileName)
                                         {   
                                             fileStatus = true;                                                                                         
                                             break;
                                         }
                                     }                                 
                             }
                             sr.Close(); 
                         }

                         if (fileStatus)
                         {
                             if(fileEntity.FileName!=null) 
                             DeleFile(fileEntity.FileName);
                         }
                         else
                         {
                             if ((fileEntity != null))
                             {
                                 if (fileEntity.FileUpload_ID != 0)
                                 {
                                     this.StatusMessage = "File '" + fileEntity.FileName + "' already exists";
                                     Application.DoEvents();
                                     Application.DoEvents();
                                     fileExists = true;
                                     return isUploaded;
                                 }
                             }
                         }
                                               
                        //go for importe if the status not 1(partial),2(unsucessfull).                       
                        fileUploadMasterEntity = fileUploadMasterBLL.InsertData(fileUploadMasterEntity) as FileUploadMasterEntity;
                        WriteintoCSVFile(fileUploadMasterEntity, "4");  

                        if (fileUploadMasterEntity.FileUpload_ID == 0)
                        {
                            this.StatusMessage = resourceMgr.GetString("FileIsLarge");// "File is too larger. Please rename it.";
                            Application.DoEvents();
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

                        //Added by Abhay
                    try
                    {                       
                        MeterDataEntity mtrDataEntity = new MeterDataEntity();
                        mtrDataEntity.FileUpload_ID = fileUploadMasterEntity.FileUpload_ID;
                        mtrDataEntity.UploadingDateTime = DateUtility.DateTimeToLong(System.DateTime.Now);
                        mtrDataEntity.CMRIID = this.cmriID;/////////////newly added
                       #endregion

                        fileText = fileText.Replace(FormatterConstant.ENTERCARRAGE, string.Empty);
                        fileText = fileText.Replace(FormatterConstant.NEWLINE, string.Empty);
                        //Parse Readouts in file and create a mapping table with meter data id.
                        ParseReadOuts(fileText, mtrDataEntity, fileUploadMasterEntity);                        
                    }
                    catch (Exception ex)
                    {
                        WriteintoCSVFile(fileUploadMasterEntity, "2");     
                    }
                    
                    List<BillingGeneralNFEntity> master = new List<BillingGeneralNFEntity>();
                    FormatterBilling formatterBilling = new FormatterBilling();
                    formatterBilling.GetData(fileText, master, readOuts);
                
                    foreach (BillingGeneralNFEntity entity in master)
                    {
                        #region Save Data
                        try
                        {
                            MeterDataEntity meterDataEntity = new MeterDataEntity();
                            MeterDataEntity mtrData = new MeterDataEntity();
                            try
                            {
                               // throw new Exception("test"); 
                                # region Meter Data Header Info
                               
                                if (entity.listHeaderInfo != null)
                                {
                                    //if (meterDataEntity != null && !string.IsNullOrEmpty(meterDataEntity.MeterID))
                                    //{
                                    //    this.StatusMessage = " Uploading Header Info";
                                    //    Application.DoEvents();

                                    //    MeterDataHeaderInfoEntity meterDataHeaderInfoEntity = entity.HeaderInfo as MeterDataHeaderInfoEntity;
                                    //    if (meterDataHeaderInfoEntity != null)
                                    //    {
                                    //        //meterDataHeaderInfoEntity.MeterData_ID = meterDataEntity.MeterData_ID;
                                    //        meterDataHeaderInfoEntity.MeterData_ID =GetMeterDataID(readOuts[ReadOutItem.HeaderDetails], meterDataEntity.MeterID, meterDataEntity.ReadingDateTime);
                                    //        meterDataHeaderInfoEntity = new MeterDataHeaderInfoBLL().InsertData(meterDataHeaderInfoEntity) as MeterDataHeaderInfoEntity;
                                    //    }
                                    //}

                                    this.StatusMessage = resourceMgr.GetString("UploadingHeaderInfo"); //" Uploading Header Info";
                                    Application.DoEvents();
                                    for (int r = 0; r < entity.listHeaderInfo.Count; r++)
                                    {
                                        if (entity.listHeaderInfo[r].meterId != null)
                                        {
                                            mtrData = new MeterDataEntity();
                                            mtrData.MeterID = entity.listHeaderInfo[r].meterId;
                                            mtrData.ReadingDateTime = entity.listHeaderInfo[r].readingDateTime;
                                            mtrData.FileUpload_ID = fileUploadMasterEntity.FileUpload_ID;
                                            mtrData.UploadingDateTime = DateUtility.DateTimeToLong(System.DateTime.Now);


                                            //if (!meterDataBLL.ValidateData(fileUploadMasterEntity.FileUpload_ID, mtrData.MeterID, mtrData.ReadingDateTime))
                                            //{
                                            //    mtrData = new MeterDataBLL().InsertData(mtrData) as MeterDataEntity;
                                            //}
                                            //else
                                            //    mtrData = meterDataBLL.GetDetailData(mtrData.MeterID, fileUploadMasterEntity.FileUpload_ID, mtrData.ReadingDateTime) as MeterDataEntity;
                                            mtrData.MeterData_ID = GetMeterDataID(readOuts[ReadOutItem.HeaderDetails], mtrData.MeterID, mtrData.ReadingDateTime);
                                            MeterDataHeaderInfoEntity meterDataHeaderInfoEntity = entity.listHeaderInfo[r] as MeterDataHeaderInfoEntity;
                                            meterDataHeaderInfoEntity.NoSupplyDuration = "---------";
                                            meterDataHeaderInfoEntity.NoLoadDuration = "---------";

                                            if (meterDataHeaderInfoEntity != null)
                                            {
                                                meterDataHeaderInfoEntity.MeterData_ID = mtrData.MeterData_ID;
                                                meterDataHeaderInfoEntity = new MeterDataHeaderInfoBLL().InsertData(meterDataHeaderInfoEntity) as MeterDataHeaderInfoEntity;
                                            }
                                        }
                                    }
                                }
                                #endregion
                            }
                            catch (Exception ex)
                            {
                                WriteintoCSVFile(fileUploadMasterEntity, "2");    
                            
                            }

                            try
                            {
                                #region Meter Data Name Plate Detail
                                if (entity.listNamePlateDetail != null)
                                {
                                    this.StatusMessage = resourceMgr.GetString("UploadingNamePlateDetail");// " Uploading Name Plate Detail";
                                    Application.DoEvents();

                                    for (int r = 0; r < entity.listNamePlateDetail.Count; r++)
                                    {
                                        mtrData = new MeterDataEntity();
                                        mtrData.MeterID = entity.listHeaderInfo[r].meterId;
                                        mtrData.ReadingDateTime = entity.listNamePlateDetail[r].ReadingDateTime;
                                        mtrData.FileUpload_ID = fileUploadMasterEntity.FileUpload_ID;
                                        mtrData.UploadingDateTime = DateUtility.DateTimeToLong(System.DateTime.Now);


                                        NamePlateDetailEntity namePlateDetailEntity = entity.listNamePlateDetail[r] as NamePlateDetailEntity;
                                        if (namePlateDetailEntity != null)
                                        {
                                            //if (meterDataEntity == null || string.IsNullOrEmpty(meterDataEntity.MeterID))
                                            //{
                                            namePlateDetailEntity.MeterData_ID = GetMeterDataID(readOuts[ReadOutItem.NamePlate], mtrData.MeterID, mtrData.ReadingDateTime);
                                            //}
                                            //else
                                            //    namePlateDetailEntity.MeterData_ID = GetMeterDataID(readOuts[ReadOutItem.HeaderDetails], meterDataEntity.MeterID, meterDataEntity.ReadingDateTime);
                                            namePlateDetailEntity = new NamePlateDetailBLL().InsertData(namePlateDetailEntity) as NamePlateDetailEntity;
                                        }
                                    }
                                }
                                #endregion
                            }
                            catch (Exception ex)
                            {
                                WriteintoCSVFile(fileUploadMasterEntity, "2");    
                                
                            }

                            try
                            {
                                # region Saving General Data

                                for (int r = 0; r < entity.listGeneralData.Count; r++)
                                {
                                    GeneralData generalDataItem = entity.listGeneralData[r];

                                    #region Instant Power //General
                                    if (generalDataItem.CurrentInstant != null)// && !string.IsNullOrEmpty(meterDataEntity.MeterID))
                                    {
                                        this.StatusMessage = resourceMgr.GetString("UploadingInstantPowerData");// " Uploading Instant Power Data";
                                        Application.DoEvents();
                                        InstantPowerEntity billingGeneral = generalDataItem.CurrentInstant as InstantPowerEntity;
                                        if (billingGeneral != null)
                                        {
                                            billingGeneral.MeterData_ID = GetMeterDataID(readOuts[ReadOutItem.General], generalDataItem.MeterData.MeterID, generalDataItem.MeterData.ReadingDateTime);
                                            billingGeneral = instantPowerBLL.InsertData(billingGeneral) as InstantPowerEntity;
                                        }
                                    }
                                    #endregion

                                    #region General
                                    if (generalDataItem.CurrentGeneral != null)// && !string.IsNullOrEmpty(meterDataEntity.MeterID))
                                    {
                                        this.StatusMessage = resourceMgr.GetString("UploadingGeneralData"); //" Uploading General Data";
                                        Application.DoEvents();
                                        GeneralEntity generalEntity = generalDataItem.CurrentGeneral as GeneralEntity;
                                        if (generalEntity != null)
                                        {
                                            generalEntity.MeterData_ID = GetMeterDataID(readOuts[ReadOutItem.General], generalDataItem.MeterData.MeterID, generalDataItem.MeterData.ReadingDateTime);
                                            generalEntity = generalBLL.InsertData(generalEntity) as GeneralEntity;
                                        }
                                    }
                                    #endregion

                                    #region General Billing
                                    if (generalDataItem.CurrentBilling != null)// && !string.IsNullOrEmpty(meterDataEntity.MeterID))
                                    {
                                        this.StatusMessage = resourceMgr.GetString("UploadingBillingData"); //" Uploading Billing Data";
                                        Application.DoEvents();
                                        BillingEntity currentBilling = generalDataItem.CurrentBilling as BillingEntity;
                                        if (currentBilling != null)
                                        {
                                            currentBilling.MeterData_ID = GetMeterDataID(readOuts[ReadOutItem.General], generalDataItem.MeterData.MeterID, generalDataItem.MeterData.ReadingDateTime);
                                            currentBilling = billingBLL.InsertData(currentBilling) as BillingEntity;
                                        }
                                    }
                                    #endregion

                                    #region general tariff
                                    if (generalDataItem.CurrentTariff != null)// && !string.IsNullOrEmpty(meterDataEntity.MeterID))
                                    {
                                        this.StatusMessage = resourceMgr.GetString("UploadingTariffEnergyData");// " Uploading Tariff Energy Data";
                                        Application.DoEvents();
                                        TariffEntity tariffCurrentEntity = generalDataItem.CurrentTariff as TariffEntity;
                                        if (tariffCurrentEntity != null)
                                        {
                                            tariffCurrentEntity.MeterData_ID = GetMeterDataID(readOuts[ReadOutItem.General], generalDataItem.MeterData.MeterID, generalDataItem.MeterData.ReadingDateTime);
                                            tariffCurrentEntity = tariffBLL.InsertData(tariffCurrentEntity) as TariffEntity;
                                        }
                                    }
                                    #endregion

                                    #region General Tamper
                                    if (generalDataItem.CurrentTamper != null)// && !string.IsNullOrEmpty(meterDataEntity.MeterID))
                                    {
                                        this.StatusMessage = resourceMgr.GetString("UploadingTamperData");// " Uploading Tamper Data";
                                        Application.DoEvents();
                                        TamperCounterGeneralEntity currentTamper = generalDataItem.CurrentTamper as TamperCounterGeneralEntity;
                                        if (currentTamper != null)
                                        {
                                            currentTamper.MeterData_ID = GetMeterDataID(readOuts[ReadOutItem.General], generalDataItem.MeterData.MeterID, generalDataItem.MeterData.ReadingDateTime);
                                            currentTamper = tamperGeneralBLL.InsertData(currentTamper) as TamperCounterGeneralEntity;
                                        }
                                    }
                                    #endregion

                                    #region Billing Data //General History Billing
                                    //Billing Data
                                    if (generalDataItem.listHistoryBilling != null)// && !string.IsNullOrEmpty(meterDataEntity.MeterID))
                                    {
                                        this.StatusMessage = resourceMgr.GetString("UploadingBillingHistoryData");// " Uploading Billing History Data";
                                        Application.DoEvents();
                                        List<IEntity> entities = new List<IEntity>();
                                        for (int counter = 0; counter < generalDataItem.listHistoryBilling.Count; counter++)
                                        {
                                            if (generalDataItem.listHistoryBilling[counter] != null)
                                            {
                                                generalDataItem.listHistoryBilling[counter].MeterData_ID = GetMeterDataID(readOuts[ReadOutItem.General], generalDataItem.MeterData.MeterID, generalDataItem.MeterData.ReadingDateTime);
                                                entities.Add(generalDataItem.listHistoryBilling[counter]);
                                            }
                                        }
                                        if (entities.Count > 0)
                                            billingBLL.InsertData(entities);
                                    }
                                    #endregion

                                    #region Tariff Data //General History Tariff
                                    if (generalDataItem.listHistoryTariff != null)// && !string.IsNullOrEmpty(meterDataEntity.MeterID))
                                    {
                                        this.StatusMessage = resourceMgr.GetString("UploadingTariffHistoryData"); //" Uploading Tariff History Data";
                                        Application.DoEvents();
                                        List<IEntity> entities = new List<IEntity>();

                                        for (int counter = 0; counter < generalDataItem.listHistoryTariff.Count; counter++)
                                        {
                                            if (generalDataItem.listHistoryTariff[counter] != null)
                                            {
                                                generalDataItem.listHistoryTariff[counter].MeterData_ID = GetMeterDataID(readOuts[ReadOutItem.General], generalDataItem.MeterData.MeterID, generalDataItem.MeterData.ReadingDateTime);
                                                entities.Add(generalDataItem.listHistoryTariff[counter]);
                                            }
                                        }
                                        if (entities.Count > 0)
                                            tariffBLL.InsertData(entities);
                                    }
                                    #endregion

                                    #region Tamper Data //General History Tamper
                                    if (generalDataItem.listHistoryTamper != null)// && !string.IsNullOrEmpty(meterDataEntity.MeterID))
                                    {
                                        this.StatusMessage = resourceMgr.GetString("UploadingTamperHistoryData");// " Uploading Tamper History Data";
                                        Application.DoEvents();
                                        List<IEntity> entities = new List<IEntity>();
                                        for (int counter = 0; counter < generalDataItem.listHistoryTamper.Count; counter++)
                                        {
                                            if (generalDataItem.listHistoryTamper[counter] != null)
                                            {
                                                generalDataItem.listHistoryTamper[counter].MeterData_ID = GetMeterDataID(readOuts[ReadOutItem.General], generalDataItem.MeterData.MeterID, generalDataItem.MeterData.ReadingDateTime);
                                                entities.Add(generalDataItem.listHistoryTamper[counter]);
                                            }
                                        }
                                        if (entities.Count > 0)
                                            tamperGeneralBLL.InsertData(entities);
                                    }
                                    #endregion

                                }

                                #endregion
                            }
                            catch (Exception ex)
                            {
                                WriteintoCSVFile(fileUploadMasterEntity, "2");                                    
                            }

                            try
                            {
                                #region Fraud Energy //MI
                                if (entity.listFraudEnergy != null && entity.listFraudEnergy.Count > 0)
                                {
                                    this.StatusMessage = resourceMgr.GetString("UploadingFraudEnergyData");// " Uploading Fraud Energy Data";
                                    Application.DoEvents();
                                    for (int r = 0; r < entity.listFraudEnergy.Count; r++)
                                    {
                                        meterDataEntity = new MeterDataEntity();
                                        if (entity.listFraudEnergy[r].ReadingDateTime != 0)
                                        {
                                            meterDataEntity.FileUpload_ID = fileUploadMasterEntity.FileUpload_ID;
                                            meterDataEntity.UploadingDateTime = DateUtility.DateTimeToLong(System.DateTime.Now);
                                            meterDataEntity.MeterID = entity.listFraudEnergy[r].MeterID;
                                            meterDataEntity.ReadingDateTime = entity.listFraudEnergy[r].ReadingDateTime;
                                            meterDataEntity.CMRIID = entity.listFraudEnergy[r].CMRIID;
                                            meterDataEntity.CMRIType = entity.listFraudEnergy[r].CMRIType;
                                            //if (meterDataEntity.MeterID != null)
                                            //{
                                            //    if (!new MeterDataBLL().ValidateData(fileUploadMasterEntity.FileUpload_ID, meterDataEntity.MeterID, meterDataEntity.ReadingDateTime))
                                            //        meterDataEntity = new MeterDataBLL().InsertData(meterDataEntity) as MeterDataEntity;
                                            //    else
                                            //        meterDataEntity = meterDataBLL.GetDetailData(meterDataEntity.MeterID, fileUploadMasterEntity.FileUpload_ID, meterDataEntity.ReadingDateTime) as MeterDataEntity;
                                            //}
                                        }

                                        FraudEnergyEntity fraudEnergyEntity = entity.listFraudEnergy[r] as FraudEnergyEntity;
                                        if (fraudEnergyEntity != null && !string.IsNullOrEmpty(meterDataEntity.MeterID))
                                        {
                                            //fraudEnergyEntity.MeterData_ID = meterDataEntity.MeterData_ID;
                                            fraudEnergyEntity.MeterData_ID = GetMeterDataID(readOuts[ReadOutItem.FraudEnergy], meterDataEntity.MeterID, meterDataEntity.ReadingDateTime);
                                            /*Added as these are coming in TNEB PUMA */
                                            fraudEnergyEntity.ReverseEnergyKVARhLag = "----";
                                            fraudEnergyEntity.ReverseEnergyKVARhLead = "----";
                                            fraudEnergyEntity.THDCurrentRPhase = "----";
                                            fraudEnergyEntity.THDCurrentYPhase = "----";
                                            fraudEnergyEntity.THDCurrentBPhase = "----";
                                            fraudEnergyEntity.THDVoltageRPhase = "----";
                                            fraudEnergyEntity.THDVoltageYPhase = "----";
                                            fraudEnergyEntity.THDVoltageBPhase = "----";
                                            /*Added as these are coming in TNEB PUMA */

                                            fraudEnergyEntity = fraudEnergyBLL.InsertData(fraudEnergyEntity) as FraudEnergyEntity;
                                        }
                                    }
                                }
                                #endregion
                            }
                            catch (Exception ex)
                            {
                                WriteintoCSVFile(fileUploadMasterEntity, "2");    
                            }

                            try
                            {
                                #region Transaction //TR
                                if (entity.listTransactionData != null && entity.listTransactionData.Count > 0)
                                {
                                    this.StatusMessage = resourceMgr.GetString("UploadingTransactionData");// " Uploading Transaction Data";
                                    Application.DoEvents();
                                    List<IEntity> entities = new List<IEntity>();
                                    for (int counter = 0; counter < entity.listTransactionData.Count; counter++)
                                    {
                                        if (entity.listTransactionData[counter].programmingData.Count > 0)
                                        {
                                            meterDataEntity = new MeterDataEntity();

                                            meterDataEntity.FileUpload_ID = fileUploadMasterEntity.FileUpload_ID;
                                            meterDataEntity.UploadingDateTime = DateUtility.DateTimeToLong(System.DateTime.Now);
                                            meterDataEntity.MeterID = entity.listTransactionData[counter].meterDataEntity.MeterID;
                                            meterDataEntity.ReadingDateTime = entity.listTransactionData[counter].meterDataEntity.ReadingDateTime;
                                            meterDataEntity.CMRIID = entity.listTransactionData[counter].meterDataEntity.CMRIID;
                                            meterDataEntity.CMRIType = entity.listTransactionData[counter].meterDataEntity.CMRIType;

                                            entity.listTransactionData[counter].meterDataEntity.MeterData_ID = GetMeterDataID(readOuts[ReadOutItem.Transaction], meterDataEntity.MeterID, meterDataEntity.ReadingDateTime);

                                            for (int x = 0; x < entity.listTransactionData[counter].programmingData.Count; x++)
                                            {
                                                entity.listTransactionData[counter].programmingData[x].MeterData_ID = entity.listTransactionData[counter].meterDataEntity.MeterData_ID;
                                                entities.Add(entity.listTransactionData[counter].programmingData[x]);
                                            }
                                        }
                                    }
                                    if (entities.Count > 0)//&& !string.IsNullOrEmpty(meterDataEntity.MeterID))
                                        programmingBLL.InsertData(entities);
                                }
                                #endregion
                            }
                            catch (Exception ex)
                            {
                                WriteintoCSVFile(fileUploadMasterEntity, "2");                                    
                            }

                            try
                            {
                                #region RTC Update //RU
                                if (entity.listRTCUpdate != null && entity.listRTCUpdate.Count > 0)
                                {
                                    this.StatusMessage = resourceMgr.GetString("UploadingRTCUpdateData"); //" Uploading RTC Update Data";
                                    Application.DoEvents();
                                    for (int r = 0; r < entity.listRTCUpdate.Count; r++)
                                    {
                                        meterDataEntity = new MeterDataEntity();
                                        if (entity.listRTCUpdate[r].ReadingDateTime != 0)
                                        {
                                            meterDataEntity.FileUpload_ID = fileUploadMasterEntity.FileUpload_ID;
                                            meterDataEntity.UploadingDateTime = DateUtility.DateTimeToLong(System.DateTime.Now);
                                            meterDataEntity.MeterID = entity.listRTCUpdate[r].MeterID;
                                            meterDataEntity.ReadingDateTime = entity.listRTCUpdate[r].ReadingDateTime;
                                            meterDataEntity.CMRIID = entity.listRTCUpdate[r].CMRIID;
                                            meterDataEntity.CMRIType = entity.listRTCUpdate[r].CMRIType;
                                            //if (meterDataEntity.MeterID != null)
                                            //{
                                            //    if (!new MeterDataBLL().ValidateData(fileUploadMasterEntity.FileUpload_ID, meterDataEntity.MeterID, meterDataEntity.ReadingDateTime))
                                            //        meterDataEntity = new MeterDataBLL().InsertData(meterDataEntity) as MeterDataEntity;
                                            //    else
                                            //        meterDataEntity = meterDataBLL.GetDetailData(meterDataEntity.MeterID, fileUploadMasterEntity.FileUpload_ID, meterDataEntity.ReadingDateTime) as MeterDataEntity;
                                            //}
                                        }
                                        RTCUpdateEntity rTCUpdateEntity = entity.listRTCUpdate[r] as RTCUpdateEntity;
                                        if (rTCUpdateEntity != null)// && !string.IsNullOrEmpty(meterDataEntity.MeterID))
                                        {
                                            //rTCUpdateEntity.MeterData_ID = meterDataEntity.MeterData_ID;
                                            rTCUpdateEntity.MeterData_ID = GetMeterDataID(readOuts[ReadOutItem.RTCUpdate], meterDataEntity.MeterID, meterDataEntity.ReadingDateTime);
                                            rTCUpdateEntity = rTCUpdateBLL.InsertData(rTCUpdateEntity) as RTCUpdateEntity;
                                        }
                                    }
                                }
                                #endregion
                            }
                            catch (Exception ex)
                            {
                                WriteintoCSVFile(fileUploadMasterEntity, "2");    
                            }

                            try
                            {
                                #region Phasor //P
                                if (entity.listPhasor != null && entity.listPhasor.Count > 0)
                                {
                                    this.StatusMessage = resourceMgr.GetString("UploadingPhasorData");// " Uploading Phasor Data";
                                    Application.DoEvents();
                                    for (int r = 0; r < entity.listPhasor.Count; r++)
                                    {
                                        meterDataEntity = new MeterDataEntity();
                                        if (entity.listPhasor[r].ReadingDateTime != 0)
                                        {
                                            meterDataEntity.FileUpload_ID = fileUploadMasterEntity.FileUpload_ID;
                                            meterDataEntity.UploadingDateTime = DateUtility.DateTimeToLong(System.DateTime.Now);
                                            meterDataEntity.MeterID = entity.listPhasor[r].MeterID;
                                            meterDataEntity.ReadingDateTime = entity.listPhasor[r].ReadingDateTime;
                                            meterDataEntity.CMRIID = entity.listPhasor[r].CMRIID;
                                            meterDataEntity.CMRIType = entity.listPhasor[r].CMRIType;
                                            //if (meterDataEntity.MeterID != null)
                                            //{
                                            //    if (!new MeterDataBLL().ValidateData(fileUploadMasterEntity.FileUpload_ID, meterDataEntity.MeterID, meterDataEntity.ReadingDateTime))
                                            //        meterDataEntity = new MeterDataBLL().InsertData(meterDataEntity) as MeterDataEntity;
                                            //    else
                                            //        meterDataEntity = meterDataBLL.GetDetailData(meterDataEntity.MeterID, fileUploadMasterEntity.FileUpload_ID, meterDataEntity.ReadingDateTime) as MeterDataEntity;
                                            //}
                                        }
                                        PhasorEntity phasorEntity = entity.listPhasor[r] as PhasorEntity;
                                        if (phasorEntity != null && !string.IsNullOrEmpty(meterDataEntity.MeterID))
                                        {
                                            phasorEntity.MeterData_ID = GetMeterDataID(readOuts[ReadOutItem.Phasor], meterDataEntity.MeterID, meterDataEntity.ReadingDateTime);
                                            phasorEntity = phasorBLL.InsertData(phasorEntity) as PhasorEntity;
                                        }
                                    }
                                }
                                #endregion
                            }
                            catch (Exception ex)
                            {
                                WriteintoCSVFile(fileUploadMasterEntity, "2");    
                            }

                            try
                            {
                                #region LoadSurvey //L
                                if (entity.listLoadSurveyData != null && entity.listLoadSurveyData.Count > 0)
                                {
                                    //meterDataEntity = entity.LoadSurveyMeterData as MeterDataEntity;
                                    //if (meterDataEntity != null)
                                    //{
                                    //    if (meterDataEntity.ReadingDateTime != 0)
                                    //    {
                                    //        meterDataEntity.FileUpload_ID = fileUploadMasterEntity.FileUpload_ID;
                                    //        meterDataEntity.UploadingDateTime = DateUtility.DateTimeToLong(System.DateTime.Now);
                                    //        if (meterDataEntity.MeterID != null)
                                    //        {
                                    //            if (!new MeterDataBLL().ValidateData(fileUploadMasterEntity.FileUpload_ID, meterDataEntity.MeterID, meterDataEntity.ReadingDateTime))
                                    //                meterDataEntity = new MeterDataBLL().InsertData(meterDataEntity) as MeterDataEntity;
                                    //            else
                                    //                meterDataEntity = meterDataBLL.GetDetailData(meterDataEntity.MeterID, fileUploadMasterEntity.FileUpload_ID, meterDataEntity.ReadingDateTime) as MeterDataEntity;
                                    //        }
                                    //    }
                                    //}

                                    this.StatusMessage = resourceMgr.GetString("UploadingLoadSurveyData"); //" Uploading Load Survey Data";
                                    Application.DoEvents();
                                    string parameters = null;
                                    List<IEntity> entities = new List<IEntity>();

                                    Int64 meterData_IdLs;
                                    for (int r = 0; r < entity.listLoadSurveyData.Count; r++)
                                    {
                                        parameters = null;
                                        meterData_IdLs = -1;
                                        entities = new List<IEntity>();
                                        if (entity.listLoadSurveyData[r].LoadSurvey.Count > 0)
                                        {
                                            meterData_IdLs = GetMeterDataID(readOuts[ReadOutItem.LoadSurvey], entity.listLoadSurveyData[r].LoadSurveyMeterData.MeterID, entity.listLoadSurveyData[r].LoadSurveyMeterData.ReadingDateTime);
                                            for (int counter = 0; counter < entity.listLoadSurveyData[r].LoadSurvey.Count; counter++)
                                            {
                                                entity.listLoadSurveyData[r].LoadSurvey[counter].MeterData_ID = meterData_IdLs;
                                                //entity.LoadSurvey[counter].MeterData_ID = meterDataEntity.MeterData_ID;
                                                parameters = entity.listLoadSurveyData[r].LoadSurvey[counter].Parameters;
                                                entities.Add(entity.listLoadSurveyData[r].LoadSurvey[counter]);
                                            }
                                        }
                                        if (entities.Count > 0)
                                        {
                                            loadSurveyBLL.InsertData(entities);
                                            LoadSurveyParameterBLL loadSurveyParameterBLL = new LoadSurveyParameterBLL();
                                            LoadSurveyParameterEntity loadSurveyParameterEntity = new LoadSurveyParameterEntity();
                                            loadSurveyParameterEntity.MeterDataId = meterData_IdLs;// meterDataEntity.MeterData_ID;
                                            loadSurveyParameterEntity.ColumnsNames = parameters;
                                            loadSurveyParameterBLL.InsertData(loadSurveyParameterEntity);
                                        }
                                    }


                                }
                                #endregion
                            }
                            catch (Exception ex)
                            {
                                WriteintoCSVFile(fileUploadMasterEntity, "2");                                    
                            }

                            try
                            {
                                #region Tamper //TM
                                if (entity.listTamper != null && entity.listTamper.Count > 0)
                                {
                                    // TamperData tamper = entity.Tamper;
                                    //if (tamper != null)
                                    //{
                                    // meterDataEntity = new MeterDataEntity();
                                    //if (entity.Tamper.General != null)
                                    //{
                                    //    if (entity.Tamper.General.ReadingDateTime != 0)
                                    //    {
                                    //        meterDataEntity.FileUpload_ID = fileUploadMasterEntity.FileUpload_ID;
                                    //        meterDataEntity.UploadingDateTime = DateUtility.DateTimeToLong(System.DateTime.Now);
                                    //        meterDataEntity.MeterID = entity.Tamper.General.MeterID;
                                    //        meterDataEntity.ReadingDateTime = entity.Tamper.General.ReadingDateTime;
                                    //        meterDataEntity.CMRIID = entity.Tamper.General.CMRIID;
                                    //        meterDataEntity.CMRIType = entity.Tamper.General.CMRIType;
                                    //        if (meterDataEntity.MeterID != null)
                                    //        {
                                    //            if (!new MeterDataBLL().ValidateData(fileUploadMasterEntity.FileUpload_ID, meterDataEntity.MeterID, meterDataEntity.ReadingDateTime))
                                    //                meterDataEntity = new MeterDataBLL().InsertData(meterDataEntity) as MeterDataEntity;
                                    //            else
                                    //                meterDataEntity = meterDataBLL.GetDetailData(meterDataEntity.MeterID, fileUploadMasterEntity.FileUpload_ID, meterDataEntity.ReadingDateTime) as MeterDataEntity;
                                    //        }
                                    //    }
                                    //}
                                    this.StatusMessage = resourceMgr.GetString("UploadingTamperSnapshotData"); //" Uploading Tamper Snapshot Data";
                                    Application.DoEvents();

                                    // Int64 meterData_IdTamp;
                                    for (int r = 0; r < entity.listTamper.Count; r++)
                                    {
                                        TamperCounterGeneralEntity tamperGeneral = entity.listTamper[r].General;// tamper.General;
                                        //   meterData_IdTamp = -1;
                                        if (tamperGeneral != null)
                                        {
                                            tamperGeneral.MeterData_ID = GetMeterDataID(readOuts[ReadOutItem.Tamper], entity.listTamper[r].General.MeterID, entity.listTamper[r].General.ReadingDateTime);
                                            tamperGeneral = tamperGeneralBLL.InsertData(tamperGeneral) as TamperCounterGeneralEntity;
                                            TamperCounterEntity tamperCounter = entity.listTamper[r].Counter;
                                            tamperCounter.MeterData_ID = tamperGeneral.MeterData_ID;//GetMeterDataID(readOuts[ReadOutItem.Tamper], meterDataEntity.MeterID, meterDataEntity.ReadingDateTime);
                                            tamperCounter.TamperCounterGeneral_ID = tamperGeneral.TamperCounterGeneral_ID;
                                            tamperCounterBLL.InsertData(tamperCounter);
                                            List<IEntity> entities = new List<IEntity>();
                                            for (int counter = 0; counter < entity.listTamper[r].Snapshot.Count; counter++)
                                            {
                                                entity.listTamper[r].Snapshot[counter].MeterData_ID = tamperGeneral.MeterData_ID;// meterDataEntity.MeterData_ID;
                                                entities.Add(entity.listTamper[r].Snapshot[counter]);
                                            }
                                            if (entities.Count > 0)// && !string.IsNullOrEmpty(tamperGeneral.MeterData_ID))
                                                tamperSnapShotBLL.InsertData(entities);
                                        }
                                    }
                                    //}
                                }
                                #endregion
                            }
                            catch (Exception ex)
                            {
                                WriteintoCSVFile(fileUploadMasterEntity, "2");                                    
                            }

                            try
                            {
                                #region DTMDailyProfile //SD
                                if (entity.listDTMDailyProfileData != null && entity.listDTMDailyProfileData.Count > 0)
                                {
                                    //meterDataEntity = entity.DTMDailyProfileMeterData as MeterDataEntity;
                                    //if (meterDataEntity != null)
                                    //{
                                    //    if (meterDataEntity.ReadingDateTime != 0)
                                    //    {
                                    //        meterDataEntity.FileUpload_ID = fileUploadMasterEntity.FileUpload_ID;
                                    //        meterDataEntity.UploadingDateTime = DateUtility.DateTimeToLong(System.DateTime.Now);
                                    //        if (meterDataEntity.MeterID != null)
                                    //        {
                                    //            if (!new MeterDataBLL().ValidateData(fileUploadMasterEntity.FileUpload_ID, meterDataEntity.MeterID, meterDataEntity.ReadingDateTime))
                                    //                meterDataEntity = new MeterDataBLL().InsertData(meterDataEntity) as MeterDataEntity;
                                    //            else
                                    //                meterDataEntity = meterDataBLL.GetDetailData(meterDataEntity.MeterID, fileUploadMasterEntity.FileUpload_ID, meterDataEntity.ReadingDateTime) as MeterDataEntity;
                                    //        }
                                    //    }
                                    //}
                                    this.StatusMessage = resourceMgr.GetString("UploadingdailyProfileData"); //" Uploading daily Profile Data";
                                    Application.DoEvents();
                                    List<IEntity> entities = new List<IEntity>();
                                    string dailyProfileParameter = string.Empty;
                                    Int64 meterData_IdDP;
                                    for (int r = 0; r < entity.listDTMDailyProfileData.Count; r++)
                                    {
                                        meterData_IdDP = GetMeterDataID(readOuts[ReadOutItem.DailyProfile], entity.listDTMDailyProfileData[r].DTMDailyProfileMeterData.MeterID, entity.listDTMDailyProfileData[r].DTMDailyProfileMeterData.ReadingDateTime);
                                        entities = new List<IEntity>();

                                        for (int counter = 0; counter < entity.listDTMDailyProfileData[r].DTMDailyProfile.Count; counter++)
                                        {
                                            entity.listDTMDailyProfileData[r].DTMDailyProfile[counter].MeterData_ID = meterData_IdDP;// meterDataEntity.MeterData_ID;
                                            dailyProfileParameter = entity.listDTMDailyProfileData[r].DTMDailyProfile[counter].Parameters;
                                            entities.Add(entity.listDTMDailyProfileData[r].DTMDailyProfile[counter]);
                                        }
                                        if (entities.Count > 0)// && !string.IsNullOrEmpty(meterDataEntity.MeterID))
                                        {
                                            dTMDailyProfileBLL.InsertData(entities);
                                            DTMDailyProfileParameterEntity dtmDailyProfileParameterEntity = new DTMDailyProfileParameterEntity();
                                            dtmDailyProfileParameterEntity.MeterDataId = meterData_IdDP;// meterDataEntity.MeterData_ID;
                                            dtmDailyProfileParameterEntity.ColumnsNames = dailyProfileParameter;
                                            new DTMDailyProfileParameterBLL().InsertData(dtmDailyProfileParameterEntity);
                                        }
                                    }
                                }
                                #endregion
                            }
                            catch (Exception ex)
                            {

                                WriteintoCSVFile(fileUploadMasterEntity, "2");    
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                        #endregion
                    }
                    #region Meter Configurations 
                    try
                    {
                        string meterID = string.Empty;
                        string tmpResult = string.Empty;
                        string readingDateTime = string.Empty;
                        Int64 meterDataID = -1;
                        FormatterConfigurations FormatterConfigurations = new FormatterConfigurations();
                        //Get Meter Config Readouts.
                        string[] meterWiseData = fileText.Split(new string[] { "CR" }, StringSplitOptions.None);

                        # region Meter Configuration Data.
                        //Parse each meter config readout.
                        for (int cnt = 1; cnt < meterWiseData.Length; cnt++)
                        {
                            fileText = meterWiseData[cnt];
                            fileText = "CR" + fileText;
                                
                                #region Upload RTC
                            if (fileText.Contains("RTC"))
                            {
                                //                                meterID = rtcvalue.Substring(0, rtcvalue.Length-2);
                                fileText = fileText.Replace("\r\n", string.Empty);
                                fileText = fileText.Replace("\n", string.Empty);
                                fileText = fileText.Replace("\a", string.Empty);
                                fileText = fileText.Replace("\b", string.Empty);

                                meterID = fileText.Substring(fileText.IndexOf("CR/") + 3);
                                meterID = meterID.Substring(4, meterID.IndexOf("/") - 4);

                                if(fileText.Contains("Invalid RTC."))
                                {
                                    this.StatusMessage = "Invalid RTC.";
                                }
                              
                                readingDateTime = fileText.Substring(fileText.IndexOf(meterID) + 1 + meterID.Length);
                                readingDateTime = readingDateTime.Substring(0, readingDateTime.IndexOf("/"));

                                meterDataID=GetMeterDataID(readOuts[ReadOutItem.MeterConfigurations], meterID, Convert.ToInt64(readingDateTime));
                               
                                // meterID = responseOutput.Substring(5, responseOutput.IndexOf("\r") - 5);
                                this.StatusMessage = resourceMgr.GetString("UploadingMeterConfigurationData");// "Uploading Meter Configuration Data";
                                Application.DoEvents();


                                try
                                {//  -->/RTC//290911101242
                                    tmpResult = fileText.Substring(fileText.IndexOf("/RTC//") + 6);
                                    string rtcvalue = tmpResult.Substring(0, tmpResult.IndexOf(Convert.ToChar(3)));
                                    new RTCBLL().InsertData(new FormatterConfigurations().ParseRTC(rtcvalue), meterID, fileUploadMasterEntity.FileUpload_ID, meterDataID);
                                }
                                catch (Exception e)
                                {
                                    //   this.StatusMessage = "Corrupt RTC Data.";
                                    // Application.DoEvents();
                                }
                            #endregion

                                #region Upload MDWithIP
                                try
                                {//-->/MD/(010130000201300000000000)
                                    tmpResult = fileText.Substring(fileText.IndexOf("/MD/") + 5);
                                    string mdWithIPValue = tmpResult.Substring(0, tmpResult.IndexOf(Convert.ToChar(3)));

                                    MDWithIPEntity mdWithIPEntity = new FormatterConfigurations().ParseMDWithIP(mdWithIPValue);
                                    mdWithIPEntity.MeterID = meterID;
                                    new MDWithIPBLL().InsertData(mdWithIPEntity, fileUploadMasterEntity.FileUpload_ID, meterDataID);
                                }
                                catch (Exception e)
                                {
                                    //  this.StatusMessage = "Corrupt MD With IP Data.";
                                    //  Application.DoEvents();
                                }
                                #endregion

                                #region Upload KVAHSelection
                                try
                                { //-->/KV/(01)

                                    tmpResult = fileText.Substring(fileText.IndexOf("/KV/") + 5);
                                    string kVarSelVal = tmpResult.Substring(0, tmpResult.IndexOf(Convert.ToChar(3)));

                                    kvarSelectionEntity kvarSelectionEntity = FormatterConfigurations.Parsekvarselection(kVarSelVal);
                                    kvarSelectionEntity.MeterID = meterID;
                                    new kvarSelectionBLL().InsertData(kvarSelectionEntity, fileUploadMasterEntity.FileUpload_ID, meterDataID);
                                }
                                catch (Exception e)
                                {
                                    // this.StatusMessage = "Corrupt kVAR Selection Data.";
                                    // Application.DoEvents();
                                }
                                #endregion

                                #region Upload Daily Log Data
                                try
                                { //-->/DL/(09)

                                    tmpResult = fileText.Substring(fileText.IndexOf("/DL/") + 5);
                                    string dailyLogData = tmpResult.Substring(0, tmpResult.IndexOf(Convert.ToChar(3)));

                                    DailyLogEntity dailyLogEntity = FormatterConfigurations.ParseDailyLogData(dailyLogData);
                                    dailyLogEntity.MeterID = meterID;
                                    new DailyLogBLL().Insertdata(dailyLogEntity, fileUploadMasterEntity.FileUpload_ID, meterDataID);
                                }
                                catch (Exception e)
                                {
                                    // this.StatusMessage = "Corrupt kVAR Selection Data.";
                                    // Application.DoEvents();
                                }
                                #endregion

                                #region Upload Billing Reset Data
                                try
                                {  //-->/BT/(01160000)
                                    //-->/BM/(01)
                                    //-->/LOD/(01)
                                    BillingResetEntity billingResetEntity = new BillingResetEntity();
                                    billingResetEntity.MeterID = meterID;

                                    tmpResult = fileText.Substring(fileText.IndexOf("/BT/") + 5);
                                    string modeOfBillingData = tmpResult.Substring(0, tmpResult.IndexOf(Convert.ToChar(3)));
                                    FormatterConfigurations.ParseModeOfBilling(modeOfBillingData, billingResetEntity);

                                    tmpResult = fileText.Substring(fileText.IndexOf("/BM/") + 5);
                                    string billPeriodData = tmpResult.Substring(0, tmpResult.IndexOf(Convert.ToChar(3)));
                                    FormatterConfigurations.ParseBillingPeriod(billPeriodData, billingResetEntity);

                                    tmpResult = fileText.Substring(fileText.IndexOf("/LOD/") + 6);
                                    string LockOutDays = tmpResult.Substring(0, tmpResult.IndexOf(Convert.ToChar(3)));
                                    FormatterConfigurations.ParseLockOutDays(LockOutDays, billingResetEntity);

                                    new BillingResetBLL().Insertdata(billingResetEntity, fileUploadMasterEntity.FileUpload_ID, meterDataID);
                                }
                                catch (Exception e)
                                {
                                    // this.StatusMessage = "Corrupt kVAR Selection Data.";
                                    // Application.DoEvents();
                                }
                                #endregion

                                #region Upload Display Parameters

                                Collection<DisplayParamatersDBEntity> collDisplayParamatersDBEntity = new Collection<DisplayParamatersDBEntity>();
                                DisplayParamatersDBEntity displayParamatersDBEntity;


                                try
                                {
                                    if (fileText.Contains("DP"))
                                    { //-->/DP/(777704)
                                        //-->(6A6B6C6D7476776E88898A33868384858B8C8E8D870F1011121314151617181C1D1F20211E22232425262728292A2B2C192D2E2F3031323436397D5B3A3B3C3D)y
                                        //-->(3F4144454647484A4C4F5253545556571A1B58595C5D5E5F606162636465666768690102030405070950517C5A7E7F8081820A0B0C0D0E)
                                        //-->(01020304050A0B0C0D0E0F1011121314151617181C1D1F20211E22232425262728292A2B2C192D2E2F3031323436397D5B3A3B3C3D3F4144454647484A4C4F52)
                                        //-->(53545556571A1B58595C5D5E5F6061626A6465666768696A6B6C6D7476776E88898A33868384858B8C8E8D877C817E7F80638207095051)
                                        //-->(787A7B79)
                                        Collection<Collection<string>> displayParametersToSelect = new Collection<Collection<string>>();
                                        tmpResult = fileText.Substring(fileText.IndexOf("/DP/") + 4);
                                        string displayParamsCount = tmpResult.Substring(tmpResult.IndexOf("("), tmpResult.IndexOf(Convert.ToChar(3)) - 1);
                                        //displayParamsCount = displayParamsCount.Substring(displayParamsCount.IndexOf("("), displayParamsCount.IndexOf(")") - displayParamsCount.IndexOf("("));
                                        int[] displayParmaterCountByType = FormatterConfigurations.SplitDisplayParamaterCount(displayParamsCount);

                                        tmpResult = tmpResult.Replace(displayParamsCount, string.Empty);
                                        tmpResult = tmpResult.Substring(tmpResult.IndexOf("("));


                                        #region Read Scroll Parameter--SCROLL
                                        string pushParams = "";
                                        int index = 0;
                                        string tmpStr = "";
                                        if (displayParmaterCountByType[0] < 64)
                                        {
                                            pushParams = tmpResult.Substring(0, tmpResult.IndexOf(')') + 1);
                                            index = tmpResult.IndexOf(pushParams);

                                            //for (int i = 0; i < tmpResult.Length; i++)
                                            //{
                                            //    if (i >= index && i <= (index + pushParams.Length))
                                            //        continue;
                                            //    tmpStr = tmpStr + tmpResult[i];
                                            //}
                                            tmpResult = tmpResult.Remove(index, pushParams.Length + 1); ;
                                            //tmpResult = tmpResult.Replace(pushParams, string.Empty);
                                        }
                                        else
                                        {
                                            pushParams = tmpResult.Substring(0, tmpResult.IndexOf(')'));
                                            index = tmpResult.IndexOf(pushParams);
                                            //for (int i = 0; i < tmpResult.Length; i++)
                                            //{
                                            //    if (i >= index && i <= (index + pushParams.Length))
                                            //        continue;
                                            //    tmpStr = tmpStr + tmpResult[i];
                                            //}

                                            tmpResult = tmpResult.Remove(index, pushParams.Length + 1); ;

                                            //tmpResult = tmpResult.Replace(pushParams, string.Empty);
                                            tmpResult = tmpResult.Substring(tmpResult.IndexOf("(") + 1);
                                            pushParams += tmpResult.Substring(0, tmpResult.IndexOf(')') + 1);
                                            tmpResult = tmpResult.Substring(tmpResult.IndexOf("("));
                                        }
                                        //Collection<string> parametersToSelect = FormatterConfigurations.ParsePushModeParameters(pushParams, displayParmaterCountByType[0]);

                                        Collection<string> parametersToSelect = FormatterConfigurations.ParseScrollModeParameters(pushParams, displayParmaterCountByType[0]);

                                        //displayParametersToSelect.Add(parametersToSelect);
                                        #endregion

                                        //string[] displayParamaters = fileText.Substring((fileText.IndexOf("<Push>") + 6), lengthOfdisplayParamaterText).Split('#');
                                        for (int i = 0; i < displayParmaterCountByType[0]; i++)
                                        {
                                            displayParamatersDBEntity = new DisplayParamatersDBEntity();
                                            displayParamatersDBEntity.displayParamaterType = DisplayParameter.ScrollMode;
                                            displayParamatersDBEntity.paramaterName = parametersToSelect[i];
                                            displayParamatersDBEntity.paramaterValue = 1;
                                            collDisplayParamatersDBEntity.Add(displayParamatersDBEntity);
                                        }
                                        #region Read Push Parameter---PUSH
                                        string scrollParams = "";
                                        tmpStr = "";
                                        tmpResult = tmpResult.Substring(tmpResult.IndexOf("("));
                                        if (displayParmaterCountByType[1] < 64)
                                        {
                                            scrollParams = tmpResult.Substring(0, tmpResult.IndexOf(')') + 1);
                                            index = tmpResult.IndexOf(scrollParams);
                                            //for (int i = 0; i < tmpResult.Length; i++)
                                            //{
                                            //    if (i >= index && i <= (index + scrollParams.Length))
                                            //        continue;
                                            //    tmpStr = tmpStr + tmpResult[i];
                                            //}
                                            tmpResult = tmpResult.Remove(index, scrollParams.Length + 1);
                                            //tmpResult = tmpResult.Replace(scrollParams, string.Empty);
                                        }
                                        else
                                        {
                                            scrollParams = tmpResult.Substring(0, tmpResult.IndexOf(')'));
                                            index = tmpResult.IndexOf(scrollParams);
                                            //for (int i = 0; i < tmpResult.Length; i++)
                                            //{
                                            //    if (i >= index && i <= (index + scrollParams.Length))
                                            //        continue;
                                            //    tmpStr = tmpStr + tmpResult[i];
                                            //}
                                            tmpResult = tmpResult.Remove(index, scrollParams.Length + 1);
                                            //tmpResult = tmpResult.Replace(scrollParams, string.Empty);
                                            tmpResult = tmpResult.Substring(tmpResult.IndexOf("(") + 1);
                                            scrollParams += tmpResult.Substring(0, tmpResult.IndexOf(')') + 1);
                                        }
                                        tmpResult = tmpResult.Substring(tmpResult.IndexOf("("));
                                        parametersToSelect = FormatterConfigurations.ParsePushModeParameters(scrollParams, displayParmaterCountByType[1]);
                                        //displayParametersToSelect.Add(parametersToSelect);
                                        #endregion

                                        //string[] displayParamaters = fileText.Substring((fileText.IndexOf("<Push>") + 6), lengthOfdisplayParamaterText).Split('#');
                                        for (int i = 0; i < displayParmaterCountByType[1]; i++)
                                        {
                                            displayParamatersDBEntity = new DisplayParamatersDBEntity();
                                            displayParamatersDBEntity.displayParamaterType = DisplayParameter.PushMode;
                                            displayParamatersDBEntity.paramaterName = parametersToSelect[i];
                                            displayParamatersDBEntity.paramaterValue = 1;
                                            collDisplayParamatersDBEntity.Add(displayParamatersDBEntity);
                                        }
                                        string highResolutionParams = tmpResult.Substring(0, tmpResult.IndexOf(')') + 1);
                                        tmpResult = tmpResult.Replace(highResolutionParams, string.Empty);
                                        tmpResult = tmpResult.Substring(tmpResult.IndexOf("("));
                                        parametersToSelect = new FormatterConfigurations().ParseHighResolutionModeParameters(highResolutionParams, displayParmaterCountByType[2]);

                                        for (int i = 0; i < displayParmaterCountByType[2]; i++)
                                        {
                                            displayParamatersDBEntity = new DisplayParamatersDBEntity();
                                            displayParamatersDBEntity.displayParamaterType = DisplayParameter.HighResolutionMode;
                                            displayParamatersDBEntity.paramaterName = parametersToSelect[i];
                                            displayParamatersDBEntity.paramaterValue = 1;
                                            collDisplayParamatersDBEntity.Add(displayParamatersDBEntity);
                                        }

                                        ////DisplayTimeOuts

                                        string displayTimeOutText = tmpResult.Substring(0, tmpResult.IndexOf(")") + 1);

                                        int tmp = Convert.ToInt32(displayTimeOutText.Substring(1, 4), 16);
                                        ////Fill DTO to write in Db.
                                        displayParamatersDBEntity = new DisplayParamatersDBEntity();
                                        displayParamatersDBEntity.displayParamaterType = DisplayParameter.DisplayTimeouts;
                                        displayParamatersDBEntity.paramaterName = "Scroll Time Out";
                                        displayParamatersDBEntity.paramaterValue = Convert.ToInt32(tmp);
                                        collDisplayParamatersDBEntity.Add(displayParamatersDBEntity);

                                        tmp = Convert.ToInt32(displayTimeOutText.Substring(5, 4), 16);
                                        ////Fill DTO to write in Db.
                                        displayParamatersDBEntity = new DisplayParamatersDBEntity();
                                        displayParamatersDBEntity.displayParamaterType = DisplayParameter.DisplayTimeouts;
                                        displayParamatersDBEntity.paramaterName = "Push Time Out";
                                        displayParamatersDBEntity.paramaterValue = Convert.ToInt32(tmp);
                                        collDisplayParamatersDBEntity.Add(displayParamatersDBEntity);

                                        tmp = Convert.ToInt32(displayTimeOutText.Substring(9, 2), 16);
                                        if (tmp != 0)
                                        {
                                            tmp = Convert.ToInt32(displayTimeOutText.Substring(11, 4), 16);
                                            //Fill DTO to write in Db.
                                            displayParamatersDBEntity = new DisplayParamatersDBEntity();
                                            displayParamatersDBEntity.displayParamaterType = DisplayParameter.DisplayTimeouts;
                                            displayParamatersDBEntity.paramaterName = "Auto Scroll Resume Time";
                                            displayParamatersDBEntity.paramaterValue = Convert.ToInt32(tmp);
                                            collDisplayParamatersDBEntity.Add(displayParamatersDBEntity);
                                        }
                                        new DisplayParametersBLL().InsertData(collDisplayParamatersDBEntity, meterID, fileUploadMasterEntity.FileUpload_ID, meterDataID);
                                    }
                                }
                                catch (Exception e)
                                {
                                    //   this.StatusMessage = "Corrupt Display Parameters Data.";
                                    //   Application.DoEvents();
                                }
                                #endregion

                                # region UploadRS232LockUnlock
                                try
                                {
                                    tmpResult = fileText.Substring(fileText.IndexOf("/RS232/") + 9, 2);

                                    RS232LockEntity rs232LockEntity = new RS232LockEntity();
                                    if (tmpResult == "00")
                                        rs232LockEntity.LockStatus = "Locked";
                                    else
                                        rs232LockEntity.LockStatus = "NotLocked";
                                    rs232LockEntity.MeterID = meterID;
                                    new RS232BLL().Insertdata(rs232LockEntity, fileUploadMasterEntity.FileUpload_ID, meterDataID);
                                }
                                catch (Exception e)
                                {
                                }

                                # endregion

                                #region Upload TOD
                                try
                                {
                                    //tmpResult = fileText.Substring(fileText.IndexOf("/TU/"), fileText.IndexOf("/RS232/") - fileText.IndexOf("/TU/"));
                                    tmpResult = fileText.Substring(fileText.IndexOf("/TU/"));
                                    if (tmpResult.IndexOf("/RTC//") >= 0)
                                    {
                                        tmpResult = tmpResult.Substring(0, tmpResult.IndexOf("/RTC//"));
                                    }
                                    if (tmpResult.IndexOf("/MD/") >= 0)
                                    {
                                        tmpResult = tmpResult.Substring(0, tmpResult.IndexOf("/MD/"));
                                    }
                                    if (tmpResult.IndexOf("/KV/") >= 0)
                                    {
                                        tmpResult = tmpResult.Substring(0, tmpResult.IndexOf("/KV/"));
                                    }
                                    if (tmpResult.IndexOf("/DP/") >= 0)
                                    {
                                        tmpResult = tmpResult.Substring(0, tmpResult.IndexOf("/DP/"));
                                    }
                                    if (tmpResult.IndexOf("/DL//") >= 0)
                                    {
                                        tmpResult = tmpResult.Substring(0, tmpResult.IndexOf("/DL/"));
                                    }
                                    if (tmpResult.IndexOf("/BM/") >= 0)
                                    {
                                        tmpResult = tmpResult.Substring(0, tmpResult.IndexOf("/BM/"));
                                    }
                                    if (tmpResult.IndexOf("/BT/") >= 0)
                                    {
                                        tmpResult = tmpResult.Substring(0, tmpResult.IndexOf("/BT/"));
                                    }
                                    if (tmpResult.IndexOf("/LOD/") >= 0)
                                    {
                                        tmpResult = tmpResult.Substring(0, tmpResult.IndexOf("/LOD/"));
                                    }
                                    if (tmpResult.IndexOf("/RS232/") >= 0)
                                    {
                                        tmpResult = tmpResult.Substring(0, tmpResult.IndexOf("/RS232/"));
                                    }
                                    else
                                    {

                                    }
                                    if (tmpResult.IndexOf("<S1D1") > 0)
                                    {
                                        #region Parse CMRI CAB File

                                        StringBuilder todDB = new StringBuilder("/TU/");
                                        string tmpStr = string.Empty;
                                        string[] tmpStrArr;
                                        ArrayList SDs = new ArrayList();
                                        int currentSD = 0;
                                        int indexOf28;
                                        bool ifFUAppended = false;
                                        bool isSDTag = false;
                                        string tuSTR = "";
                                        int cntBracket = 0;
                                        string sd = "";
                                        try
                                        {
                                            while (true)
                                            {
                                                indexOf28 = tmpResult.IndexOf("28");
                                                if (!ifFUAppended && (indexOf28 > tmpResult.IndexOf("/FU/")))
                                                {
                                                    tuSTR = todDB.ToString();
                                                    cntBracket = 0;
                                                    todDB = new StringBuilder();
                                                    foreach (char chr in tuSTR)
                                                    {
                                                        if (chr == ')')
                                                            cntBracket++;

                                                        todDB = todDB.Append(chr);

                                                        if (cntBracket == 6 && currentSD < SDs.Count)
                                                        {
                                                            todDB = todDB.Append(SDs[currentSD].ToString());
                                                            cntBracket = 0;
                                                            currentSD++;
                                                        }
                                                    }

                                                    todDB = todDB.Append("/FU/");
                                                    ifFUAppended = true;
                                                    SDs = new ArrayList();
                                                    currentSD = 0;
                                                }
                                                if (indexOf28 >= 0)
                                                {
                                                    if (tmpResult.IndexOf("<SD") != -1 && tmpResult.IndexOf("<SD") < indexOf28)
                                                    {
                                                        isSDTag = true;
                                                    }
                                                    tmpStr = tmpResult.Substring(indexOf28 + 2, (tmpResult.IndexOf("29") - indexOf28 - 2));
                                                    tmpStrArr = tmpStr.Split(' ');

                                                    if (!isSDTag)
                                                        todDB = todDB.Append("(");
                                                    else
                                                        sd = "(";
                                                    for (int n = 0; n < tmpStrArr.Length; n++)
                                                    {
                                                        if (tmpStrArr[n].Trim().Length == 0)
                                                            continue;
                                                        if (!isSDTag)
                                                            todDB = todDB.Append((char)Int32.Parse(tmpStrArr[n].Trim(), System.Globalization.NumberStyles.HexNumber));
                                                        else
                                                            sd = sd + ((char)Int32.Parse(tmpStrArr[n].Trim(), System.Globalization.NumberStyles.HexNumber));
                                                    }
                                                    if (!isSDTag)
                                                        todDB = todDB.Append(")");
                                                    else
                                                        sd = sd + (")");

                                                    tmpResult = tmpResult.Remove(indexOf28, (tmpResult.IndexOf("29") - indexOf28) + 2);
                                                    if (isSDTag)
                                                    {
                                                        tmpResult = tmpResult.Remove(tmpResult.IndexOf("<SD"), 3);
                                                        tmpResult = tmpResult.Remove(tmpResult.IndexOf("</SD"), 3);
                                                    }

                                                    if (isSDTag)
                                                    { SDs.Add(sd); isSDTag = false; }
                                                }
                                                else
                                                    break;
                                            }
                                        }
                                        catch (Exception eee)
                                        {

                                        }
                                        string fuStr = "";
                                        tuSTR = todDB.ToString();
                                        fuStr = tuSTR.Substring(tuSTR.IndexOf("/FU/"));
                                        cntBracket = 0;
                                        todDB = new StringBuilder(tuSTR.Replace(fuStr, ""));
                                        foreach (char chr in fuStr)
                                        {
                                            if (chr == ')')
                                                cntBracket++;

                                            todDB = todDB.Append(chr);

                                            if (cntBracket == 6 && currentSD < SDs.Count)
                                            {
                                                todDB = todDB.Append(SDs[currentSD].ToString());
                                                cntBracket = 0;
                                                currentSD++;
                                            }
                                        }
                                        #endregion

                                        new TODBLL().InsertData(todDB.ToString(), meterID, fileUploadMasterEntity.FileUpload_ID, meterDataID);
                                    }
                                    else
                                    {
                                        new TODBLL().InsertData(tmpResult.ToString(), meterID, fileUploadMasterEntity.FileUpload_ID, meterDataID);
                                    }
                                }
                                catch (Exception e)
                                {
                                    //  this.StatusMessage = "Corrupt TOD Data.";
                                    // Application.DoEvents();
                                }
                                #endregion


                                this.StatusMessage = resourceMgr.GetString("MeterConfigurationDataUploaded"); // "Meter Configuration Data Uploaded successfully";
                                Application.DoEvents();
                            }

                        }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                    #endregion

                    this.StatusMessage = resourceMgr.GetString("Filetxt") + Path.GetFileName(fileName) + resourceMgr.GetString("UploadedSuccessfully");
                    Application.DoEvents();
                    //MessageBox.Show( resourceMgr.GetString("FileUploadedSuccess"), "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.ListRefresh = true;
                    if (isUploaded)
                    {
                        if (sucessFiles != string.Empty)
                        {
                            sucessFiles = sucessFiles + "," + fileUploadMasterEntity.FileName;
                        }
                        else
                        {
                            sucessFiles = fileUploadMasterEntity.FileName;
                        }
                        break;
                    }
                }
                }
                //WriteintoCSVFile(fileUploadMasterEntity, "1");    
                return isUploaded;
            }
            catch (Exception)
            {
                WriteintoCSVFile(fileUploadMasterEntity, "3");     
                return false;
            }
        }       

        private  void WriteintoCSVFile(FileUploadMasterEntity fileUploadMasterEntity,string status)
        {

            bool writeFlag = true; 
            if (newFileString != string.Empty)
            {
                newFileString = newFileString + "," + fileUploadMasterEntity.FileUpload_ID.ToString() + "*" + fileUploadMasterEntity.UploadingDateTime.ToString() + "*" + fileUploadMasterEntity.FileName.ToString() + "*" + status;
            }
            else
            {
                newFileString = fileUploadMasterEntity.FileUpload_ID.ToString() + "*" + fileUploadMasterEntity.UploadingDateTime.ToString() + "*" + fileUploadMasterEntity.FileName.ToString() + "*" + status;
            }            
           
            try
            {
                if (File.Exists(csvFileName))
                {
                    StreamReader sr = new StreamReader(csvFileName);
                    CsvFileReader csv = new CsvFileReader(sr);
                    CsvRow row = new CsvRow();
                    while (csv.ReadRow(row))
                    {   
                            if (row[2] == fileUploadMasterEntity.FileName && row[3] == "4")
                            {
                                writeFlag = false;
                                break;
                            }                        
                    }
                    sr.Close();
                }

                if (writeFlag)
                {
                    writer = new CsvFileWriter(csvFileName);
                    CsvRow writeRow = new CsvRow();
                    writeRow.Add(fileUploadMasterEntity.FileUpload_ID.ToString());
                    writeRow.Add(fileUploadMasterEntity.UploadingDateTime.ToString());
                    writeRow.Add(fileUploadMasterEntity.FileName.ToString());
                    writeRow.Add(status);
                    writer.WriteRow(writeRow);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteFile()
        {
            try
            {
                string oldFileDetails = string.Empty; 
                if (File.Exists(csvFileName))
                {   
                        StreamReader sr = new StreamReader(csvFileName);
                        CsvFileReader csv = new CsvFileReader(sr);
                        CsvRow row = new CsvRow();
                        string[] allRow=sucessFiles.Split(new char[]{','});
                       
                        while (csv.ReadRow(row))
                        {   
                                if (row[0] != "FileUpload_ID")
                                {
                                    foreach (string singleRow in allRow)
                                    {
                                        if (row[2] != singleRow && row[3] != "1")
                                        {
                                            if (oldFileDetails != string.Empty)
                                            {
                                                oldFileDetails = oldFileDetails + "?" + row[0] + "," + row[1] + "," + row[2] + "," + row[3];
                                            }
                                            else
                                            {
                                                oldFileDetails = row[0] + "," + row[1] + "," + row[2] + "," + row[3];
                                            }
                                        }                                        
                                    }                                   
                                }                              
                            
                        }
                        sr.Close();

                        File.Delete(csvFileName);  
                }
                if (newFileString != string.Empty)
                {                   
                    writer = new CsvFileWriter(csvFileName);
                    CsvRow row = new CsvRow();

                    string[] newrow = newFileString.Split(new char[] { ',' });
                    foreach (string s in newrow)
                    {
                        row = new CsvRow();
                        string[] addrow = s.Split(new char[] { '*' });
                        row.Add(addrow[0]);
                        row.Add(addrow[1]);
                        row.Add(addrow[2]);
                        row.Add(addrow[3]);  
                        if(addrow[3]!="4") 
                        writer.WriteRow(row);                        
                    }                   
                }
                if (oldFileDetails != string.Empty)
                {
                    writer = new CsvFileWriter(csvFileName);
                    CsvRow row = new CsvRow();

                    string[] newrow = oldFileDetails.Split(new char[] { '?' });
                    foreach (string t in newrow)
                    {
                        string[] newrow1 = t.Split(new char[] { ',' });                                             
                         row = new CsvRow();
                         row.Add(newrow1[0]);
                         row.Add(newrow1[1]);
                         row.Add(newrow1[2]);
                         row.Add(newrow1[3]);                           
                         writer.WriteRow(row);
                       
                    }                    
                }
                MainForm mainForm = (MainForm)this.ParentForm;
                if (mainForm != null)
                {
                    if (CsvFileReader.ReadPendingFiles() != string.Empty)
                    {
                        /* GKG 26/02/2013 Fetaure is disabled*/
                        //mainForm.SetPendingFiles = CsvFileReader.ReadPendingFiles();
                        //mainForm.SetPendingFilesVisible = true;
                        mainForm.SetPendingFilesVisible = false;
                        /* GKG 26/02/2013 Fetaure is disabled*/
                    }
                    else
                    {
                        mainForm.SetPendingFilesVisible = false;
                    }                  
                   mainForm.Show();
                }
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
  
        }
        #endregion
        
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
        /// Gets file content as string from input file.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public string Get2NGFileContent(string filePath)
        {
            string fileContent = string.Empty;
            if (File.Exists(filePath))
            {
                StreamReader streamReader = new StreamReader(filePath);
                fileContent = streamReader.ReadToEnd();
                streamReader.Close();
            }
            return fileContent;
        }

        public string GetContent(string filePath)
        {
            StreamReader streamReader = new StreamReader(filePath);

            string fileName = Path.GetFileName(filePath);

            string fileContent = streamReader.ReadToEnd();
            streamReader.Close();
            if (ConfigInfo.IsEncryption())
            {
                fileContent = ConfigInfo.DecryptFile(fileContent);
            }
            if (UtilityDetails.UtilityName == UtilityEntity.UGVCL || UtilityDetails.UtilityName == UtilityEntity.JDVVNL || UtilityDetails.UtilityName == UtilityEntity.PVVNL || UtilityDetails.UtilityName == UtilityEntity.WBEXPORTVCL)
            {
                if (fileContent.Contains("NP/"))
                {
                    MessageBox.Show(string.Format(resourceMgr.GetString("FormatNotSupported"),fileName), "BCS", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    fileContent = string.Empty;
                }
            }
            else if (UtilityDetails.UtilityName == UtilityEntity.TNEB)
            {
                if (!fileContent.Contains("NP/"))
                {
                    MessageBox.Show(string.Format(resourceMgr.GetString("FormatNotSupported"),fileName), "BCS", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    fileContent = string.Empty;
                }
            }
            return fileContent;
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            Application.DoEvents();
            this.Close();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.DefaultExt = "CAB";
            openFileDialog.Filter = "Readout(*.CAB;*.2NG)|*.CAB;*.2NG";

            DialogResult result = openFileDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                txtBoxFileName.Text = string.Empty;
                foreach (string fileName in openFileDialog.SafeFileNames)
                {
                    txtBoxFileName.Text += fileName + " ; ";
                }
            }
        }

        private void UploadFile_Activated(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            Application.DoEvents();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void timerFileStatus_Tick(object sender, EventArgs e)
        {
            if (uploadFileRequests.Count > 0)
            {
                DataSet ds = new FileUploadStatusBLL().GetFileUploadStatus(uploadFileRequests);
                dataGridStatus.DataSource = ds.Tables[0];
            }
        }

        private void UploadFile_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.StatusMessage = "";
        }       
    }
}
