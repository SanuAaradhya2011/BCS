using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;
using CAB.BLL;
using CAB.Channel.Formatter;
using CAB.Channel.ReadOut;
using CAB.Entity;
using CAB.Framework;
using CAB.Framework.Utility;
using CAB.IECChannel;
using CAB.IECChannel.CMRI;
using CAB.MeterData.Upload;
using CAB.Parser;
using CAB.Serialization;
using CAB.UI.Controls;
using CABCommunication.Common;
using CABCommunication.PhysicalLayer;
using CABCommunication.WrapperLayer;
using CABFramework;
using Shell32;
using System.Net.Sockets;
using Hunt.EPIC.Logging;
namespace CAB.UI
{
    public partial class CMRICommunication : MdiChildForm
    {
        private CMRIReadout cMRIReadout;
        private IECLocalCommunication communications;
        private Communication communication;
        private Command command;
        private PrepareCMRI prepareCMRI;
        private string scheduleFileLocation = string.Empty;
        private string touFileName = string.Empty;
        private string scheduleFile = "Scd";
        private string sCDFileCommand = string.Empty;
        private string tOUFileCommand = string.Empty;
        private string fileExtension = string.Empty;
        private string cmriID = String.Empty;
        private Dictionary<byte, string> dumpFiles = new Dictionary<byte, string>();
        private Dictionary<byte, string> fdlFileList = new Dictionary<byte, string>();
        private const string Splitter = "$";
        private const string FixUSModePasswordForCMRILogin = "2222222222222222";
        private const string FileName = "IntermediateFile";
        private Dictionary<string, int> meterLoadList = null;
        private static Serializer serializer = null;
        private static DLMS commandRepository = null;
        private string selectedFile = string.Empty;
        private string DLMSFileName = string.Empty;
        private string IECFileText = string.Empty;
        private int filesCount = 0;
        private int fileIndex = 0;
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(CMRICommunication).ToString());

        public CMRICommunication()
        {
            InitializeComponent();
            command = Command.GetInstance();
            cMRIReadout = new CMRIReadout();
            cMRIReadout.OnChannelStatusChanged += new CMRIReadout.ChannelStatusChanged(Channel_OnStatusChanged);
            communications = new IECLocalCommunication();
            prepareCMRI = new PrepareCMRI();
            ChannelInformation channelInfo = new ChannelInformation();
            channelInfo.CommunicationMode = ConfigSettings.GetValue("ChannelType");
            channelInfo.ComPort = ConfigSettings.GetValue("PortName");
            channelInfo.SecurityMechanism = Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism"));
            channelInfo.Password = channelInfo.SecurityMechanism == 2 ? FixUSModePasswordForCMRILogin : ConfigSettings.GetValue("ModePassword");
            channelInfo.ProtocolType = UtilityDetails.PrimaryUtlityName;
            channelInfo.NoOfRetries = Convert.ToByte(ConfigSettings.GetValue("NoOfRetries"));
            communication = new Communication(channelInfo);

        }

        static CMRICommunication()
        {
            serializer = new Serializer();
            if (commandRepository == null)
            {
                commandRepository = (DLMS)serializer.DeserializeToObject(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "CommandRepository.xml"), typeof(DLMS));
            }
        }

        private void cmriID_OnValuesSubmission(string cmriID)
        {
            this.cmriID = cmriID;
        }
        /// <summary>
        /// enable start timer for different thread
        /// </summary>
        private void EnableStartTimer()
        {
            if (statusStrip.InvokeRequired)
            {
                statusStrip.Invoke(new MethodInvoker(EnableStartTimer));
            }
            else
            {
                StartProgressBarTimer();
            }
        }
        /// <summary>
        /// enable stop timer for different thread
        /// </summary>
        private void EnableStopTimer()
        {
            if (statusStrip.InvokeRequired)
            {
                statusStrip.Invoke(new MethodInvoker(EnableStopTimer));
            }
            else
            {
                StopProgressBarTimer();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void CMRIDataRead()
        {
            try
            {
                filesCount = 0;
                fileIndex = 0;
                string ZIPFilePath = string.Empty;
                string NONDLMSText = string.Empty;
                string portName = ConfigSettings.GetValue("PortName");
                string baudRate = ConfigSettings.GetValue("BaudRate");
                this.StatusMessage = "Reading CMRI Data...";
                //EnableStartTimer();
                StartProgressBarTimer();
                this.Cursor = Cursors.WaitCursor;
                Result result = communication.CheckCMRICommunicationType((byte)CMRICommunicationType.DLMSDumpData, portName, baudRate);

                if (result.ErrorCode == CommunicationErrorType.SuccessForDLMS)
                {

                    SetConnectionDetail(true);
                    Application.DoEvents();
                    Thread.Sleep(4000);

                    //if (baudRate.Contains("38400")) communication.PhysicalChannelDetail.SetBaud(7);
                    ZIPFilePath = DumpCMRIDataByZIPMode();

                    //if (UtilityDetails.IECSupport)
                    //{
                    //    result = communication.CheckCMRICommunicationType((byte)CMRICommunicationType.IECDumpData, portName, baudRate);
                    //    if (result.ErrorCode == CommunicationErrorType.SuccessForDLMS)
                    //    {
                    //        Application.DoEvents();
                    //        Thread.Sleep(4000);
                    //        NONDLMSText = DumpIECCMRIData();
                    //        NONDLMSText = GetIECCMRIData(ZIPFilePath);
                    //    }
                    //}

                    if (!ZIPFilePath.Contains(".2NG"))
                    NONDLMSText = GetIECCMRIData(ZIPFilePath);

                    if (ZIPFilePath == String.Empty && NONDLMSText == String.Empty)
                    {
                        this.StatusMessage = "No data available for readout.";
                    }
                    else
                    {
                        this.Cursor = Cursors.Default;
                        EnableStopTimer();
                        this.StatusMessage = "CMRI readout successful";
                        Application.DoEvents();

                        #region Uplaod DLMS & Non-DLMS

                        filesCount = GetFilesToReadCount(ZIPFilePath, NONDLMSText);

                        if (ZIPFilePath.Contains(".2NG"))
                        {
                            ReadDataAndCreateInstantBillingCMRIFiles(ZIPFilePath, cmriID);
                        }
                        else if (ZIPFilePath != String.Empty)
                        {
                            ReadDataAndCreateDLMSFileFromNativeCMRIFiles(ZIPFilePath, cmriID);
                     
                            //region Uplaod NON DLMS
                            if (UtilityDetails.IECSupport && NONDLMSText != String.Empty)
                            {
                                UploadNonDLMSCMRIFile(NONDLMSText, cmriID);
                            }
                        }
                        
                        #endregion

                        #region Insert CMRI details
                        InsertCMRIDetails(cmriID);
                        #endregion

                        if (ZIPFilePath != String.Empty || (UtilityDetails.IECSupport && NONDLMSText != String.Empty))
                        {
                            this.Cursor = Cursors.Default;
                            this.ListRefresh = true;
                            this.StatusMessage = "File Uploaded Successfully";
                            Application.DoEvents();
                        }
                    }
                }
                else
                {
                    this.StatusMessage = CommonBLL.GetEnumDescription(result.ErrorCode);
                    //this.StatusMessage = "CMRI not able to connect..";
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "CMRIDataRead()", ex);
            }
            finally
            {
                SetConnectionDetail(false);
                StopProgressBarTimer();
                this.Cursor = Cursors.Default;
            }
        }

        private int GetFilesToReadCount(string ZIPFilePath, string NONDLMSText)
        {
            int dlmsFilesCount=0;
            int iecFilesCount=0;

            if (ZIPFilePath.Contains(".2NG"))
            {
                string content = File.ReadAllText(ZIPFilePath);
                if (content.IndexOf('$') != content.LastIndexOf('$'))
                {
                    dlmsFilesCount = content.Split('$').Length;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(ZIPFilePath))
                {
                    dlmsFilesCount = Directory.GetFiles(ZIPFilePath, "*.LGD").Length;
                }
                if (!string.IsNullOrEmpty(NONDLMSText))
                {
                    iecFilesCount = NONDLMSText.Split(new string[] { "NP" }, StringSplitOptions.RemoveEmptyEntries).Length;
                }
            }
            
            return dlmsFilesCount + iecFilesCount;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public string GetIECCMRIData(string filePath)
        {
            string nonDLMSfileData = string.Empty;
            const string keyFile = "\\CABR.CAB";
            try
            {
                string individualFileKeys = null;
                //Get the file having each readout file index, meterid, reading datetime and data.                    
                individualFileKeys = File.ReadAllText(filePath + keyFile);

                if (individualFileKeys.Length > 0)
                {
                    textRead = individualFileKeys;
                }                
                if (textRead.Trim() != string.Empty)
                {
                    if (textRead == "Connection Timeout !")
                    {
                        this.StatusMessage = textRead;
                        Application.DoEvents();
                    }
                    else if (textRead == MessageConstant.GetText("M000038"))
                    {
                        this.StatusMessage = MessageConstant.GetText("M000038") + " Please Restart BCS.";
                    }
                    else if (textRead == "No Data Available.")
                    {
                        //this.StatusMessage = textRead;
                        Application.DoEvents();
                    }
                    else
                    {
                        if (ReadoutCommon.CalculateFileBcc(textRead) != string.Empty)
                        {
                            nonDLMSfileData = textRead;
                        }
                        else
                        {
                            this.StatusMessage = "BCC not Matched";
                            Application.DoEvents();
                        }
                    }
                }
                
                       
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetIECCMRIData(string filePath)", ex);
            }
            finally
            {
                //Delete Files 
                foreach (string file in Directory.GetFiles(filePath, "*.CAB"))
                {
                    File.Delete(file);
                }
                
                this.Cursor = Cursors.Default;
            }
            return nonDLMSfileData;
        }



        /// <summary>
        /// Insert cmri details while dumping file from new cmri
        /// </summary>
        /// <param name="cmriID"></param>
        private void InsertCMRIDetails(string cmriID)
        {
            CMRIMasterBLL cmriMasterBLL = new CMRIMasterBLL();
            CMRIMasterEntity cmriMasterEntity = new CMRIMasterEntity();
            cmriMasterEntity.CMRI_Number = cmriID;
            cmriMasterEntity.CMRI_Description = cmriID;

            if (cmriMasterEntity.CMRI_ID == 0)
            {
                if (!cmriMasterBLL.ValidateCMRI(cmriMasterEntity.CMRI_Number))
                {
                    cmriMasterBLL.InsertData(cmriMasterEntity);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="NONDLMSText"></param>
        public void UploadNonDLMSCMRIFile(string NONDLMSText, string cmriID)
        {
            // Story - 339623
            string fileName=string.Empty;
            string fileNameWithPath = string.Empty;
            string[] readOutsList = null;
            string tmpMtrID = string.Empty;
            FileStream fileStream=null;
            StreamWriter strWriter = null;
            const string strSplitTxt = "NP";

            try
            {  
                // Separate each meter data in the array
                readOutsList = NONDLMSText.Split(new string[] { strSplitTxt }, StringSplitOptions.RemoveEmptyEntries);

                for (int fileNumber = 0; fileNumber < readOutsList.Length; fileNumber++)
                {
                    if (readOutsList[fileNumber].Trim().Length == 0) continue;

                    if (readOutsList[fileNumber].ToUpper().Contains("LGC")) // Story - 347720 - This would be called for single phase Non DLMS
                    {
                        // To calculate Meter ID
                        tmpMtrID = readOutsList[fileNumber].Substring(readOutsList[fileNumber].IndexOf("/") + 1);
                        tmpMtrID = tmpMtrID.Substring(13, 16); // Story - 349654 - 16 digit are fixed in case single pahse non dlms , other than digit/characters, spaces are appended at the end of meter id

                        fileName = tmpMtrID.Trim() + "_" + System.DateTime.Now.ToString("ddMMyyyy") + "_" + System.DateTime.Now.ToString("HHmmss") + ".SLG"; // Append meter id in the File Name
                    }
                    else
                    {
                        // To calculate Meter ID
                        tmpMtrID = readOutsList[fileNumber].Substring(readOutsList[fileNumber].IndexOf("/") + 1);
                        tmpMtrID = tmpMtrID.Substring(4, tmpMtrID.IndexOf("/") - 4);

                        fileName = tmpMtrID.Trim() + "_" + System.DateTime.Now.ToString("ddMMyyyy") + "_" + System.DateTime.Now.ToString("HHmmss")+ ".CAB";  // Append meter id in the File Name
                    }
                    
                    fileNameWithPath = string.Concat(ConfigInfo.CheckOrCreatePath(), "\\", fileName).Replace("\\\\", "\\");

                    fileStream = new FileStream(fileNameWithPath, FileMode.Create);
                    strWriter = new StreamWriter(fileStream);

                    readOutsList[fileNumber] = string.Concat(readOutsList[fileNumber], ReadoutCommon.CalculateFileBcc(readOutsList[fileNumber])); // Appending checksum

                    readOutsList[fileNumber] = ConfigInfo.EncryptFile(readOutsList[fileNumber]);
                    strWriter.Write(readOutsList[fileNumber]);

                    strWriter.Close();
                    fileStream.Close();
                    //Clear File Text
                    IECFileText = string.Empty;

                    bool IsUploaded = false;
                    UploadFile uploadFile = new UploadFile();
                    this.Cursor = Cursors.WaitCursor;
                    //this.StatusMessage = "Uploading " + fileName;
                    this.StatusMessage = string.Format("Uploading {0}   {1} / {2}", fileName, ++fileIndex, filesCount);
                    Application.DoEvents();
                    string resultMessage = string.Empty;
                    ConfigSettings.ChangeNode("SourceOfFile", ((int)CommTypes.CMRI).ToString());

                    if (!string.IsNullOrEmpty(fileNameWithPath))
                    {
                        if (uploadFile.GetIECFileContent(fileNameWithPath).ToUpper().Contains("LGC")) // Story - 347720 - This would be called for single phase Non DLMS
                        {
                            IsUploaded = IsUploaded = uploadFile.UploadSLGFile(fileNameWithPath, uploadFile.GetIECFileContent(fileNameWithPath), true, out resultMessage, cmriID);
                        }
                        else
                        {
                            IsUploaded = IsUploaded = uploadFile.UploadCABFile(fileNameWithPath, uploadFile.GetIECFileContent(fileNameWithPath), true, out resultMessage, cmriID);
                        }
                    }
                }

            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "UploadNonDLMSCMRIFile(string NONDLMSText, string cmriID)", ex);
            }
            finally
            {
                strWriter.Close();
                fileStream.Close();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string DumpCMRIDataByZIPMode()
        {
            string destinationPath = string.Empty;
            CommunicationErrorType resultStatus = CommunicationErrorType.Nothing;

            string fileName = AppDomain.CurrentDomain.BaseDirectory + @"DLMSCommunication\Temp.zip";
            // Support for linux hhu
            // Mohsin Raza - 05-Nov-15
            string targzfileName = AppDomain.CurrentDomain.BaseDirectory + @"DLMSCommunication\Temp.tar.gz";

            Result result;
            bool isConnected = false;
            string baudRate = ConfigSettings.GetValue("BaudRate");

            if (baudRate.Contains("38400"))
                result = communication.OpenSessionCMRI(7);
            else
                result = communication.OpenSession();


            try
            {


                
                if (result.ErrorCode == CommunicationErrorType.Success)
                {
                    // Added to support CMRI ID If not then defult ID "unknown" will be returned.
                    cmriID = communication.GetCMRIID();

                    isConnected = true;
                    ProfileCommand profileCommand = new ProfileCommand(0xFF, "MeterID.EE.FF.00.00.10", 0xFF);
                    profileCommand.MeterID = SoapHexBinary.Parse("FFFFFFFFFFFFFFFF").Value.ToList();
                    result = communication.Send(profileCommand);
                    if (result.ErrorCode == CommunicationErrorType.Success && result.RecieveDataBuffer.Count > 0)
                    {
                        // Find which type of compression is used "PK"
                        //if (result.RecieveDataBuffer[0] == '$' || result.RecieveDataBuffer[0] == 0x19)
                        if (result.RecieveDataBuffer[0] == 0x0d && result.RecieveDataBuffer[1] == 0x0a && (result.RecieveDataBuffer[2] == '$' || result.RecieveDataBuffer[0] == 0x19 || result.RecieveDataBuffer[0] == '>'))
                        {
                            CheckDirectory(string.Concat(AppDomain.CurrentDomain.BaseDirectory, @"CAB Readout\"));
                            fileName = AppDomain.CurrentDomain.BaseDirectory +"CAB Readout\\" + "hhu_" + DateTime.Now.ToString("ddMMyy_HHmmss") + ".2NG";
                            using (BinaryWriter binaryWriter = new BinaryWriter(File.Open(fileName, FileMode.Create)))
                            {
                                for (int i = 0; i < result.RecieveDataLength; i++)
                                {
                                    binaryWriter.Write(result.RecieveDataBuffer[i]);
                                }
                            }                           
                            communication.CloseSession();
                            isConnected = false;
                            destinationPath = fileName;
                        }
                        else if (result.RecieveDataBuffer[0] != 0x50 && result.RecieveDataBuffer[1] != 0x4B)
                        {
                            fileName = targzfileName;
                            //Create ZIP file 
                            CheckDirectory(string.Concat(AppDomain.CurrentDomain.BaseDirectory, @"DLMSCommunication\"));
                            using (BinaryWriter binaryWriter = new BinaryWriter(File.Open(fileName, FileMode.Create)))
                            {
                                for (int i = 0; i < result.RecieveDataLength; i++)
                                {
                                    binaryWriter.Write(result.RecieveDataBuffer[i]);
                                }
                            }
                            communication.CloseSession();
                            isConnected = false;
                            //Unzip Files
                            //destinationPath = UnZipFiles(fileName);                       

                            if (fileName.ToLower().Contains(".tar.gz"))
                            {
                                destinationPath = UnZipTARGZFiles(fileName);
                            }
                            else
                            {
                                destinationPath = UnZipFiles(fileName);
                            }
                        }
                        else
                        {
                            //Create ZIP file 
                            CheckDirectory(string.Concat(AppDomain.CurrentDomain.BaseDirectory, @"DLMSCommunication\"));
                            using (BinaryWriter binaryWriter = new BinaryWriter(File.Open(fileName, FileMode.Create)))
                            {
                                for (int i = 0; i < result.RecieveDataLength; i++)
                                {
                                    binaryWriter.Write(result.RecieveDataBuffer[i]);
                                }
                            }
                            communication.CloseSession();
                            isConnected = false;
                            //Unzip Files
                            //destinationPath = UnZipFiles(fileName);                       

                            if (fileName.ToLower().Contains(".tar.gz"))
                            {
                                destinationPath = UnZipTARGZFiles(fileName);
                            }
                            else
                            {
                                destinationPath = UnZipFiles(fileName);
                            }
                        }
                       

                    }
                    else
                    {
                        resultStatus = result.ErrorCode;
                    }
                }
                else
                {
                    resultStatus = result.ErrorCode;
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "DumpCMRIDataByZIPMode()", ex);
            }
            finally
            {
                if (isConnected)
                {
                    communication.CloseSession();
                }
                //Delete ZIP File 
                if (File.Exists(fileName) && !fileName.Contains(".2NG"))
                {
                    File.Delete(fileName);
                }
            }
            return destinationPath;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string DumpIECCMRIData()
        {
            string fileData = string.Empty;
            cMRIReadout.Channel = communications;
            textRead = cMRIReadout.GetData();
            if (textRead.Trim() != string.Empty)
            {
                if (textRead == "Connection Timeout !")
                {
                    this.StatusMessage = textRead;
                    Application.DoEvents();
                }
                else if (textRead == MessageConstant.GetText("M000038"))
                {
                    this.StatusMessage = MessageConstant.GetText("M000038") + " Please Restart BCS.";
                }
                else if (textRead == "No Data Available.")
                {
                    //this.StatusMessage = textRead;
                    Application.DoEvents();
                }
                else
                {
                    if (ReadoutCommon.CalculateFileBcc(textRead) != string.Empty)
                    {
                        //this.StatusMessage = "CMRI readout successful";
                        //Application.DoEvents();
                        fileData = textRead;//SaveData(textRead);
                    }
                    else
                    {
                        this.StatusMessage = "BCC not Matched";
                        Application.DoEvents();
                    }
                }
            }
            return fileData;
        }
        string textRead = string.Empty;
        private void btnReadData_Click(object sender, EventArgs e)
        {

            CMRIDataRead();
            //try
            // {
            //     DLMSFileName = string.Empty;
            //     IECFileText = string.Empty;
            //     this.StatusMessage = "Reading CMRI Data...";
            //     StartProgressBarTimer();
            //     Application.DoEvents();
            //     this.Cursor = Cursors.WaitCursor;
            //     Result result = communication.CheckCMRICommunicationType((byte)CMRICommunicationType.DLMSDumpData, ConfigSettings.GetValue("PortName"), ConfigSettings.GetValue("BaudRate"));
            //     if (result.ErrorCode == CommunicationErrorType.SuccessForDLMS)
            //     {
            //         SetConnectionDetail(true);
            //         Application.DoEvents();
            //         Thread.Sleep(4000);
            //         CommunicationErrorType resultStatus = ReadCMRIDataWithFastModeCommunication(); //ReadE650CMRIData(); 
            //         if (UtilityDetails.IECSupport)
            //         {
            //             //Generic CMRI with IEC Support
            //             result = communication.CheckCMRICommunicationType((byte)CMRICommunicationType.IECDumpData, ConfigSettings.GetValue("PortName"), ConfigSettings.GetValue("BaudRate"));
            //             if (result.ErrorCode == CommunicationErrorType.SuccessForDLMS)
            //             {
            //                 Application.DoEvents();
            //                 Thread.Sleep(4000);
            //                 ReadIECCMRIData();
            //                 this.Cursor = Cursors.Default;
            //                 //Save/Upload IEC and DLMS files.
            //                 if (!string.IsNullOrEmpty(DLMSFileName) || !string.IsNullOrEmpty(IECFileText))
            //                 {
            //                     this.StatusMessage = "Read out completed.";
            //                     Application.DoEvents();
            //                     SetConnectionDetail(false);
            //                     StopProgressBarTimer();
            //                     SaveDLMSAndIECFiles();
            //                     SaveDLMSAndIECFiles1();
            //                 }
            //                 else if (string.IsNullOrEmpty(DLMSFileName) && string.IsNullOrEmpty(IECFileText))
            //                 {
            //                     this.StatusMessage = "No Data Found.";
            //                 }
            //             }
            //         }
            //         else if (resultStatus == CommunicationErrorType.Success && !string.IsNullOrEmpty(DLMSFileName))
            //         {

            //             this.Cursor = Cursors.Default;
            //             if (File.Exists(DLMSFileName))
            //             {
            //                 this.StatusMessage = "Read out completed.";
            //                 Application.DoEvents();
            //                 SetConnectionDetail(false);
            //                 StopProgressBarTimer();
            //                 SaveE650Data(DLMSFileName);
            //                 // SaveE650Data1(DLMSFileName);
            //             }

            //         }
            //         else if (resultStatus == CommunicationErrorType.SuccessForDLMS && string.IsNullOrEmpty(DLMSFileName) && string.IsNullOrEmpty(IECFileText))
            //         {
            //             this.StatusMessage = "No Data Found.";
            //         }
            //         else
            //         {
            //             this.StatusMessage = CommonBLL.GetEnumDescription(resultStatus);
            //         }

            //     }
            //     else
            //     {
            //         this.StatusMessage = CommonBLL.GetEnumDescription(result.ErrorCode);
            //     }
            // }
            // catch
            // {

            // }
            // finally
            // {
            //     SetConnectionDetail(false);
            //     StopProgressBarTimer();
            //     this.Cursor = Cursors.Default;
            // }
        }
        /// <summary>
        /// Used to read CMRI data using fast mode communication.
        /// Here Method reads data from CMRI file and creates a zip file .
        /// Later on Unzips that file passes unzipped file location to a method that will further do processing on that file.
        /// Returns communicationerror type as success if everything goes fine else returns status as nothing.
        /// </summary>
        /// <returns></returns>
        
        private CommunicationErrorType ReadCMRIDataWithFastModeCommunication()
        {
            CommunicationErrorType resultStatus = CommunicationErrorType.Nothing;
            string fileName = AppDomain.CurrentDomain.BaseDirectory + @"DLMSCommunication\Temp.zip";
            // Support for linux hhu
            // Mohsin Raza - 05-Nov-15
            string targzfileName = AppDomain.CurrentDomain.BaseDirectory + @"DLMSCommunication\Temp.tar.gz";


            Result result;
            bool isConnected = false;
            try
            {
                result = communication.OpenSession();
                if (result.ErrorCode == CommunicationErrorType.Success)
                {
                    isConnected = true;
                    ProfileCommand profileCommand = new ProfileCommand(0xFF, "MeterID.EE.FF.00.00.10", 0xFF);
                    profileCommand.MeterID = SoapHexBinary.Parse("FFFFFFFFFFFFFFFF").Value.ToList();
                    result = communication.Send(profileCommand);
                    if (result.ErrorCode == CommunicationErrorType.Success && result.RecieveDataBuffer.Count > 0)
                    {
                        // Find which type of compression is used "PK"
                        if (result.RecieveDataBuffer[0] != 0x50 && result.RecieveDataBuffer[1] != 0x4B)
                        {
                            fileName = targzfileName;
                        }

                        //Create ZIP file 
                        CheckDirectory(string.Concat(AppDomain.CurrentDomain.BaseDirectory, @"DLMSCommunication\"));
                        using (BinaryWriter binaryWriter = new BinaryWriter(File.Open(fileName, FileMode.Create)))
                        {
                            for (int i = 0; i < result.RecieveDataLength; i++)
                            {
                                binaryWriter.Write(result.RecieveDataBuffer[i]);
                            }
                        }
                        communication.CloseSession();
                        isConnected = false;


                        //Unzip Files
                        //string destinationPath = UnZipFiles(fileName);                     

                        string destinationPath = string.Empty;

                        if (fileName.ToLower().Contains(".tar.gz"))
                        {
                            destinationPath = UnZipTARGZFiles(fileName);
                        }
                        else
                        {
                            destinationPath = UnZipFiles(fileName);
                        }

                        if (!string.IsNullOrEmpty(destinationPath))
                        {
                            //Code To Map CMRI files into 2NG files
                            ReadDataAndCreateDLMSFileFromNativeCMRIFiles(destinationPath, cmriID);
                            resultStatus = CommunicationErrorType.Success;

                        }
                    }
                    else
                    {
                        resultStatus = result.ErrorCode;
                    }
                }
                else
                {
                    resultStatus = result.ErrorCode;
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ReadCMRIDataWithFastModeCommunication()", ex);
            }
            finally
            {
                if (isConnected)
                {
                    communication.CloseSession();
                }
                //Delete ZIP File 
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
            }


            return resultStatus;
        }
        /// <summary>
        /// This function is responsible for reading data from native CMRI files and creating a single DLMS file.
        /// </summary>
        /// <param name="filePath"></param>
        public void ReadDataAndCreateDLMSFileFromNativeCMRIFiles(string filePath, string cmriID)
        {

            string[] cmriNativeFileRecords;
            string classIdOBISCodeAttribute;
            string readingDateTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            string meterId = string.Empty;
            try
            {
                string[] fileCollection = Directory.GetFiles(filePath, "*.LGD");
                //Get the file having each readout file index,meterid and reading datetime.
                string[] keyFile = Directory.GetFiles(filePath, "*.FTP");
                string[] individualFileKeys = null;
                bool isFastMode;
                if (keyFile.Length > 0)
                {
                    individualFileKeys = File.ReadAllLines(keyFile[0]);
                }
                if (fileCollection.Length > 0)
                {
                    List<ProfileId> selectedProfiles = GetSelectedProfilesToReadCMRI();
                    List<ProfileCommand> allProfileCommands = GetProfileCommandEntityFromCMRIRepository();
                    List<ProfileCommand> lstProfileCommands = new List<ProfileCommand>();

                    //if (ConfigInfo.FileNamingConvention() == "Default+MeterID")
                    //{
                    foreach (string file in fileCollection)
                    {
                        isFastMode = false;
                        cmriNativeFileRecords = File.ReadAllLines(file);
                        GetMeterIdAndReadingDateTime(Path.GetFileName(file).Substring(0, 2), individualFileKeys, out meterId, out readingDateTime, out isFastMode);
                        // string fileName = string.Concat(meterId, "_", readingDateTime.Substring(0, 2), readingDateTime.Substring(3, 2), readingDateTime.Substring(6, 4), "_", readingDateTime.Substring(11, 2), readingDateTime.Substring(14, 2), readingDateTime.Substring(17, 2), ".2NG");
                        string fileName = string.Concat(meterId.Trim(), "_", DateTime.Now.ToString("ddMMyyyy HHmmss").Trim().Replace(' ', '_'), ".2NG");
                        string fileNameWithPath = string.Concat(ConfigInfo.CheckOrCreatePath(), "\\", fileName).Replace("\\\\", "\\");

                        FileStream initialFileStream = new FileStream(fileNameWithPath, FileMode.Append);
                        StreamWriter writeToFile = new StreamWriter(initialFileStream);

                        writeToFile.WriteLine(Splitter);
                        //Write meter data length,need to make it dynamic later
                        writeToFile.WriteLine(meterId.Length.ToString("00"));
                        writeToFile.WriteLine(readingDateTime);

                        if (isFastMode)
                        {
                            lstProfileCommands = GetProfileCommandsToRead(allProfileCommands, 2, CommunicationMode.FastDownload);
                        }
                        else
                        {
                            lstProfileCommands = GetProfileCommandsToRead(allProfileCommands, 2, CommunicationMode.Normal);
                        }
                        #region dataConvertions
                        bool isUtilitySpeceficTamperScalarExist = false;
                        foreach (ProfileCommand profileCommand in lstProfileCommands)
                        {
                            foreach (string record in cmriNativeFileRecords)
                            {
                                if (!string.IsNullOrEmpty(record) && record.Length > 2)
                                {
                                    string strTAG = record.Substring(0, 2);
                                    int TAGNumber = 0;
                                    if (strTAG.Contains("A") || strTAG.Contains("B") || strTAG.Contains("C") || strTAG.Contains("D") || strTAG.Contains("E") || strTAG.Contains("F"))
                                        TAGNumber = int.Parse(strTAG, System.Globalization.NumberStyles.AllowHexSpecifier);
                                    else
                                        TAGNumber = Convert.ToInt32(strTAG);


                                    if (profileCommand.TagNumber == 14)
                                    { }
                                    if (profileCommand.TagNumber == TAGNumber)
                                    {
                                        if (profileCommand.TagNumber == 249 || profileCommand.TagNumber == 250)
                                        {
                                            if (record.Length > 6) isUtilitySpeceficTamperScalarExist = true;
                                            else break;
                                        }
                                        if (profileCommand.TagNumber == 32 || profileCommand.TagNumber == 33)
                                        {
                                            if (isUtilitySpeceficTamperScalarExist) break;
                                        }
                                        if (string.IsNullOrEmpty((record.Substring(2))) || (record.Substring(2).Length < 33 && profileCommand.TagNumber == 53))
                                        {

                                        }
                                        else
                                        {
                                            if (isFastMode && profileCommand.TagNumber == 50)
                                            {
                                                classIdOBISCodeAttribute = "0100006001BCFF02";
                                            }                                            
                                            else if (!isFastMode && profileCommand.TagNumber == 88)
                                            {
                                                classIdOBISCodeAttribute = String.Format("{0:X2}", 255)
                                                                           + profileCommand.ObisCode.Replace(".", "").ToUpper().Replace("METERID", "FF")
                                                                           + String.Format("{0:X2}", profileCommand.Attribute);
                                            }
                                            else if (isFastMode)
                                            {
                                                classIdOBISCodeAttribute = String.Format("{0:X2}", profileCommand.ClassId)
                                                                           + profileCommand.ObisCode.Replace(".", "").ToUpper().Replace("METERID", "FF")
                                                                           + String.Format("{0:X2}", profileCommand.Attribute);
                                            }
                                            else if (profileCommand.TagNumber == 98)
                                            {
                                                classIdOBISCodeAttribute = "98";
                                                
                                            }                                          
                                            else
                                            {

                                                if (profileCommand.ClassId != 11)
                                                {
                                                    classIdOBISCodeAttribute = profileCommand.ClassId.ToString("00") + profileCommand.ObisCode.Replace(".", "")
                                                                                                       + String.Format("{0:X2}", profileCommand.Attribute);
                                                }
                                                else
                                                {
                                                    classIdOBISCodeAttribute = profileCommand.ClassId.ToString("X2") + profileCommand.ObisCode.Replace(".", "")
                                                                                                                                                           + String.Format("{0:X2}", profileCommand.Attribute);
                                                }
                                            }

                                            writeToFile.Write(classIdOBISCodeAttribute);
                                            if (!string.IsNullOrEmpty(record.Substring(2)))
                                            {
                                                writeToFile.Write(record.Substring(2));
                                                writeToFile.WriteLine("");
                                            }

                                            

                                        }
                                        break;
                                    }
                                }
                            }

                        }
                        writeToFile.Close();
                        initialFileStream.Close();
                        #endregion

                        //Append Checksum data into file.
                        String strChecksum = GetMD5ChecksumForFile(fileNameWithPath);
                        using (FileStream fileStream = new FileStream(fileNameWithPath, FileMode.Append))
                        {
                            using (StreamWriter writeStream = new StreamWriter(fileStream))
                            {
                                writeStream.WriteLine(strChecksum);
                                DLMSFileName = fileName;
                            }
                        }

                        #region fileUplaod
                        //this.StatusMessage = "Uploading " + fileName + ".";
                        this.StatusMessage = string.Format("Uploading {0}   {1} / {2}", fileName, ++fileIndex, filesCount);
                        Application.DoEvents();
                        UploadFile uploadFile = new UploadFile();
                        string resultMessage = string.Empty;
                        ConfigSettings.ChangeNode("SourceOfFile", ((int)CommTypes.CMRI).ToString());
                        bool IsUploaded = uploadFile.Upload2NGFile(fileNameWithPath, uploadFile.GetContent(fileNameWithPath), true, out resultMessage, cmriID);
                        if (IsUploaded)
                        {
                            this.Cursor = Cursors.Default;
                            this.ListRefresh = true;
                            this.RightStatusMessage = String.Empty;
                            this.StatusMessage = "File Uploaded successfully.";
                            Application.DoEvents();
                        }
                        else
                        {
                            this.Cursor = Cursors.Default;
                            this.RightStatusMessage = String.Empty;
                            this.StatusMessage = resultMessage;
                        }
                        #endregion
                    }
                    //}
                    //else
                    //{
                    //    string fileName = string.Concat(DateTime.Now.ToString("ddMMyyyy HHmmss").Trim().Replace(' ', '_'), ".2NG");
                    //    string fileNameWithPath = string.Concat(ConfigInfo.CheckOrCreatePath(), "\\", fileName).Replace("\\\\", "\\");

                    //    using (FileStream initialFileStream = new FileStream(fileNameWithPath, FileMode.Append))
                    //    {
                    //        using (StreamWriter writeToFile = new StreamWriter(initialFileStream))
                    //        {
                    //            foreach (string file in fileCollection)
                    //            {
                    //                isFastMode = false;
                    //                cmriNativeFileRecords = File.ReadAllLines(file);
                    //                GetMeterIdAndReadingDateTime(Path.GetFileName(file).Substring(0, 2), individualFileKeys, out meterId, out readingDateTime, out isFastMode);

                    //                writeToFile.WriteLine(Splitter);
                    //                //Write meter data length,need to make it dynamic later
                    //                writeToFile.WriteLine(meterId.Length.ToString("00"));
                    //                writeToFile.WriteLine(readingDateTime);
                    //                if (isFastMode)
                    //                {
                    //                    lstProfileCommands = GetProfileCommandsToRead(allProfileCommands, 2, CommunicationMode.FastDownload);
                    //                }
                    //                else
                    //                {
                    //                    lstProfileCommands = GetProfileCommandsToRead(allProfileCommands, 2, CommunicationMode.Normal);
                    //                }
                    //                foreach (ProfileCommand profileCommand in lstProfileCommands)
                    //                {
                    //                    foreach (string record in cmriNativeFileRecords)
                    //                    {

                    //                        if (!string.IsNullOrEmpty(record) && record.Length > 2)
                    //                        {

                    //                            if (profileCommand.TagNumber == Convert.ToInt32(record.Substring(0, 2)))
                    //                            {
                    //                                if (string.IsNullOrEmpty((record.Substring(2))) || (record.Substring(2).Length < 33 && profileCommand.TagNumber == 53))
                    //                                {

                    //                                }
                    //                                else
                    //                                {
                    //                                    if (isFastMode && profileCommand.TagNumber == 50)
                    //                                    {
                    //                                        classIdOBISCodeAttribute = "0100006001BCFF02";
                    //                                    }
                    //                                    else if (!isFastMode && profileCommand.TagNumber == 88)
                    //                                    {
                    //                                        classIdOBISCodeAttribute = String.Format("{0:X2}", 255)
                    //                                                                   + profileCommand.ObisCode.Replace(".", "").ToUpper().Replace("METERID", "FF")
                    //                                                                   + String.Format("{0:X2}", profileCommand.Attribute);
                    //                                    }
                    //                                    else if (isFastMode)
                    //                                    {
                    //                                        classIdOBISCodeAttribute = String.Format("{0:X2}", profileCommand.ClassId)
                    //                                                                   + profileCommand.ObisCode.Replace(".", "").ToUpper().Replace("METERID", "FF")
                    //                                                                   + String.Format("{0:X2}", profileCommand.Attribute);
                    //                                    }
                    //                                    else
                    //                                    {

                    //                                        classIdOBISCodeAttribute = profileCommand.ClassId.ToString("00") + profileCommand.ObisCode.Replace(".", "")
                    //                                                                                           + String.Format("{0:X2}", profileCommand.Attribute);
                    //                                    }

                    //                                    writeToFile.Write(classIdOBISCodeAttribute);
                    //                                    if (!string.IsNullOrEmpty(record.Substring(2)))
                    //                                    {
                    //                                        writeToFile.Write(record.Substring(2));
                    //                                        writeToFile.WriteLine("");
                    //                                    }

                    //                                }
                    //                                break;
                    //                            }
                    //                        }
                    //                    }
                    //                }
                    //            }
                    //        }
                    //    }
                    //    #region fileUplaod
                    //    this.StatusMessage = "Uploading " + fileName + ".";
                    //    Application.DoEvents();
                    //    UploadFile uploadFile = new UploadFile();
                    //    string resultMessage = string.Empty;
                    //    ConfigSettings.ChangeNode("SourceOfFile", ((int)CommTypes.CMRI).ToString());
                    //    uploadFile.Upload2NGFile(fileNameWithPath, uploadFile.GetContent(fileNameWithPath), true, out resultMessage);
                    //    this.StatusMessage = resultMessage;
                    //    #endregion
                    //}
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ReadDataAndCreateDLMSFileFromNativeCMRIFiles(string filePath, string cmriID)", ex);
            }
            finally
            {
                //Delete Files 
                foreach (string file in Directory.GetFiles(filePath, "*.LGD"))
                {
                    File.Delete(file);
                }
                foreach (string file in Directory.GetFiles(filePath, "*.FTP"))
                {
                    File.Delete(file);
                }

                this.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// This function is responsible for reading data from native CMRI files and creating a single DLMS file.
        /// </summary>
        /// <param name="filePath"></param>
        public void ReadDataAndCreateInstantBillingCMRIFiles(string filePath, string cmriID)
        {

            string[] cmriNativeFileRecords;
            string classIdOBISCodeAttribute;
            string readingDateTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            string meterId = string.Empty;
            try
            {
                
                        #region fileUplaod
                        this.StatusMessage = "Uploading Data...";
                        Application.DoEvents();
                        UploadFile uploadFile = new UploadFile();
                        string resultMessage = string.Empty;
                        ConfigSettings.ChangeNode("SourceOfFile", ((int)CommTypes.CMRI).ToString());
                        string content = File.ReadAllText(filePath);                        
                        bool IsUploaded = false;
                        if (content.IndexOf('$') != content.LastIndexOf('$'))
                        {
                            string[] lstFiles = content.Split('$');
                            for (int i = 0; i < lstFiles.Length; i++)
                            {
                                this.StatusMessage = string.Format("Uploading Data...   {0} / {1}", ++fileIndex, filesCount);
                                if (lstFiles[i].Length > 5)
                                {
                                    string filename = AppDomain.CurrentDomain.BaseDirectory + "CAB Readout\\" + "hhu" + i.ToString() + "_" + DateTime.Now.ToString("ddMMyy_HHmmss") + ".2NG";
                                    File.AppendAllText(filename,"$" +lstFiles[i]);
                                    IsUploaded = uploadFile.Upload2NGFile(filename, uploadFile.GetContent(filename), true, out resultMessage, cmriID);
                                }
                            }
                            File.Delete(filePath);
                        }
                        else
                        {
                            this.StatusMessage = "Uploading Data...   1 / 1";
                            IsUploaded = uploadFile.Upload2NGFile(filePath, uploadFile.GetContent(filePath), true, out resultMessage, cmriID);
                        }
                        if (IsUploaded)
                        {
                            this.Cursor = Cursors.Default;
                            this.ListRefresh = true;
                            this.RightStatusMessage = String.Empty;
                            this.StatusMessage = "File Uploaded successfully.";
                            Application.DoEvents();
                        }
                        else
                        {
                            this.Cursor = Cursors.Default;
                            this.RightStatusMessage = String.Empty;
                            this.StatusMessage = resultMessage;
                        }
                        #endregion
                    }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ReadDataAndCreateInstantBillingCMRIFiles(string filePath, string cmriID)", ex);
            }
            finally
            {
                //Delete Files 
                foreach (string file in Directory.GetFiles(filePath, "*.LGD"))
                {
                    File.Delete(file);
                }
                foreach (string file in Directory.GetFiles(filePath, "*.FTP"))
                {
                    File.Delete(file);
                }

                this.Cursor = Cursors.Default;
            }
        }

        //private void ReadDataAndCreateDLMSFileFromNativeCMRIFiles(string filePath)
        //{
        //    bool isFileCreatedSuccessfully = false;
        //    string[] cmriNativeFileRecords;
        //    string classIdOBISCodeAttribute;
        //    CheckDirectory(string.Concat(AppDomain.CurrentDomain.BaseDirectory, @"DLMSCommunication\"));
        //    string fileName = AppDomain.CurrentDomain.BaseDirectory + @"DLMSCommunication\InterMediateFile";
        //    string readingDateTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    string meterId = string.Empty;
        //    try
        //    {
        //        string[] fileCollection = Directory.GetFiles(filePath, "*.LGD");
        //        //Get the file having each readout file index,meterid and reading datetime.
        //        string[] keyFile = Directory.GetFiles(filePath, "*.FTP");
        //        string[] individualFileKeys = null;
        //        bool isFastMode;
        //        if (keyFile.Length > 0)
        //        {
        //            individualFileKeys = File.ReadAllLines(keyFile[0]);
        //        }
        //        if (fileCollection.Length > 0)
        //        {
        //            List<ProfileId> selectedProfiles = GetSelectedProfilesToReadCMRI();
        //            List<ProfileCommand> allProfileCommands = GetProfileCommandEntityFromCMRIRepository();
        //            List<ProfileCommand> lstProfileCommands = new List<ProfileCommand>();
        //            using (FileStream initialFileStream = new FileStream(fileName, FileMode.Append))
        //            {
        //                using (StreamWriter writeToFile = new StreamWriter(initialFileStream))
        //                {
        //                    foreach (string file in fileCollection)
        //                    {
        //                        isFastMode = false;
        //                        cmriNativeFileRecords = File.ReadAllLines(file);
        //                        GetMeterIdAndReadingDateTime(Path.GetFileName(file).Substring(0, 2), individualFileKeys, out meterId, out readingDateTime, out isFastMode);
        //                        writeToFile.WriteLine(Splitter);
        //                        //Write meter data length,need to make it dynamic later
        //                        writeToFile.WriteLine(meterId.Length.ToString("00"));
        //                        writeToFile.WriteLine(readingDateTime);
        //                        if (isFastMode)
        //                        {
        //                            lstProfileCommands = GetProfileCommandsToRead(allProfileCommands, 2, CommunicationMode.FastDownload);
        //                        }
        //                        else
        //                        {
        //                            lstProfileCommands = GetProfileCommandsToRead(allProfileCommands, 2, CommunicationMode.Normal);
        //                        }
        //                        foreach (ProfileCommand profileCommand in lstProfileCommands)
        //                        {
        //                            foreach (string record in cmriNativeFileRecords)
        //                            {

        //                                if (!string.IsNullOrEmpty(record) && record.Length > 2)
        //                                {

        //                                    if (profileCommand.TagNumber == Convert.ToInt32(record.Substring(0, 2)))
        //                                    {
        //                                        if (string.IsNullOrEmpty((record.Substring(2))) || (record.Substring(2).Length < 33 && profileCommand.TagNumber == 53))
        //                                        {

        //                                        }
        //                                        else
        //                                        {
        //                                            if (isFastMode && profileCommand.TagNumber == 50)
        //                                            {
        //                                                classIdOBISCodeAttribute = "0100006001BCFF02";
        //                                            }
        //                                            else if (!isFastMode && profileCommand.TagNumber == 88)
        //                                            {
        //                                                classIdOBISCodeAttribute = String.Format("{0:X2}", 255)
        //                                                                           + profileCommand.ObisCode.Replace(".", "").ToUpper().Replace("METERID", "FF")
        //                                                                           + String.Format("{0:X2}", profileCommand.Attribute);
        //                                            }
        //                                            else if (isFastMode)
        //                                            {
        //                                                classIdOBISCodeAttribute = String.Format("{0:X2}", profileCommand.ClassId)
        //                                                                           + profileCommand.ObisCode.Replace(".", "").ToUpper().Replace("METERID", "FF")
        //                                                                           + String.Format("{0:X2}", profileCommand.Attribute);
        //                                            }
        //                                            else
        //                                            {

        //                                                classIdOBISCodeAttribute = profileCommand.ClassId.ToString("00") + profileCommand.ObisCode.Replace(".", "")
        //                                                                                                   + String.Format("{0:X2}", profileCommand.Attribute);
        //                                            }

        //                                            writeToFile.Write(classIdOBISCodeAttribute);
        //                                            if (!string.IsNullOrEmpty(record.Substring(2)))
        //                                            {
        //                                                writeToFile.Write(record.Substring(2));
        //                                                writeToFile.WriteLine("");
        //                                            }

        //                                        }
        //                                        break;
        //                                    }
        //                                }
        //                            }

        //                        }

        //                    }

        //                }
        //            }
        //            //Append Checksum data into file.
        //            String strChecksum = GetMD5ChecksumForFile(fileName);
        //            using (FileStream fileStream = new FileStream(fileName, FileMode.Append))
        //            {
        //                using (StreamWriter writeStream = new StreamWriter(fileStream))
        //                {
        //                    writeStream.WriteLine(strChecksum);
        //                    DLMSFileName = fileName;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {

        //    }
        //    finally
        //    {
        //        if (File.Exists(fileName) && isFileCreatedSuccessfully)
        //        {
        //            File.Delete(fileName);
        //        }
        //        //Delete Files 
        //        foreach (string file in Directory.GetFiles(filePath, "*.LGD"))
        //        {
        //            File.Delete(file);
        //        }
        //        foreach (string file in Directory.GetFiles(filePath, "*.FTP"))
        //        {
        //            File.Delete(file);
        //        }

        //        this.Cursor = Cursors.Default;
        //    }
        //}


        /// <summary>
        /// Gets meter id and reading date time from content of native CMRI FTP file.
        /// </summary>
        /// <param name="fileIndex"></param>
        /// <param name="recordList"></param>
        /// <returns></returns>
        private void GetMeterIdAndReadingDateTime(string fileIndex, string[] recordList, out string meterId, out  string readingDateTime, out bool isFastMode)
        {
            meterId = null;
            readingDateTime = DateTime.Now.ToString();
            isFastMode = false;
            try
            {
                DLMS650FormatterCommon commom = new DLMS650FormatterCommon();
                string[] recordContent;
                foreach (string record in recordList)
                {
                    if (!string.IsNullOrEmpty(record) && record.Length > 2)
                    {
                        if (fileIndex.Equals(record.Substring(0, 2)))
                        {
                            recordContent = record.Split(',');
                            if (recordContent != null && recordContent.Length > 2)
                            {
                                meterId = commom.ConvertHexToString(recordContent[1].Trim());
                                readingDateTime = DateUtility.LongToStringDateTimeWithSecFormat(Convert.ToInt64(commom.GetDateTimeString(recordContent[2])));
                                if (recordContent[2].Substring(16, 2).ToUpper() == "FD")
                                {
                                    isFastMode = true;
                                }

                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetMeterIdAndReadingDateTime(string fileIndex, string[] recordList, out string meterId, out  string readingDateTime, out bool isFastMode)", ex);
            }
        }

        /// <summary>
        /// Used to Get commands for reading profiles from xml file and deserialize 
        /// that into list of ProFileCommand as return value.
        /// </summary>
        /// <returns></returns>
        private List<ProfileCommand> GetProfileCommandEntityFromCMRIRepository()
        {
            //DLMS profileCommands = (DLMS)new Serializer().DeserializeToObject("CommandRepository.xml", typeof(DLMS));
            DLMS profileCommands = (DLMS)new Serializer().DeserializeToObject("CMRIRepository.xml", typeof(DLMS));

            List<ProfileCommand> lstProfileCommands = new List<ProfileCommand>();
            ProfileCommand profileCommandEntity;
            foreach (DLMSCOMMAND dlmsCommand in profileCommands.Items)
            {
                profileCommandEntity = new ProfileCommand();
                // Updated By Mohsin to increase CMRI Read ID range
                                
                if (dlmsCommand.TAGNO.Contains("A") || dlmsCommand.TAGNO.Contains("B") || dlmsCommand.TAGNO.Contains("C") || dlmsCommand.TAGNO.Contains("D") || dlmsCommand.TAGNO.Contains("E") || dlmsCommand.TAGNO.Contains("F"))
                {
                    profileCommandEntity.TagNumber = int.Parse(dlmsCommand.TAGNO, System.Globalization.NumberStyles.AllowHexSpecifier); //  Convert.ToInt32(dlmsCommand.TAGNO, System.);
                }
                else
                {
                    profileCommandEntity.TagNumber = Convert.ToInt32(dlmsCommand.TAGNO);                    
                }

                
               
                profileCommandEntity.Attribute = Convert.ToByte(dlmsCommand.ATTRIBUTE);
                if (profileCommandEntity.TagNumber == 44)
                { 
                    profileCommandEntity.ClassId = byte.Parse(dlmsCommand.CLASS, System.Globalization.NumberStyles.AllowHexSpecifier); 
                }
                else
                {
                    profileCommandEntity.ClassId = Convert.ToByte(dlmsCommand.CLASS);
                }
                profileCommandEntity.ObisCode = dlmsCommand.OBISCODE;
                profileCommandEntity.MeterModelNumber = Convert.ToByte(dlmsCommand.METERMODEL);
                lstProfileCommands.Add(profileCommandEntity);
            }
            return lstProfileCommands;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lstProfileCommands"></param>
        /// <param name="selectedProfile"></param>
        /// <param name="meterModelNumber"></param>
        /// <returns></returns>
        private List<ProfileCommand> GetProfileCommandsToRead(List<ProfileCommand> lstProfileCommands, int meterModelNumber, CommunicationMode commMode)
        {
            List<ProfileCommand> profileReadCommands = null;
            if (commMode == CommunicationMode.Normal)
            {
                //find normal commands
                profileReadCommands = lstProfileCommands.FindAll(delegate(ProfileCommand profileCommandEntity)
                {
                    return profileCommandEntity.ClassId != 0xFF
                    && (profileCommandEntity.MeterModelNumber == meterModelNumber ||
                    profileCommandEntity.MeterModelNumber == 0);
                });
            }
            else
            {
                //find fast download commands
                profileReadCommands = lstProfileCommands.FindAll(delegate(ProfileCommand profileCommandEntity)
                {
                    return profileCommandEntity.ClassId == 0xFF
                    && (profileCommandEntity.MeterModelNumber == meterModelNumber ||
                    profileCommandEntity.MeterModelNumber == 0);
                });

            }
            return profileReadCommands;
        }
        /// <summary>
        /// Used to UNZIP .ZIP files into a destination folder and returns 
        /// destination folder path.
        /// </summary>
        /// <param name="sourceFilePath"></param>
        /// <returns></returns>
        public string UnZipFiles(string sourceFilePath)
        {

            string destPath = string.Concat(AppDomain.CurrentDomain.BaseDirectory, @"DLMSCommunication\Temp");
            try
            {
                Shell objShell = new Shell();
                //objShell.WindowsSecurity();
                Folder sourceFolder = objShell.NameSpace(sourceFilePath);
                destPath = string.Concat(AppDomain.CurrentDomain.BaseDirectory, @"DLMSCommunication\Temp");
                CheckDirectory(destPath);
                Folder destinationFolder = objShell.NameSpace(destPath);
                Shell32.FolderItems items = sourceFolder.Items();
                destinationFolder.CopyHere(items, 20);

            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "UnZipFiles(string sourceFilePath)", ex);
            }
            return destPath;
        }



        /// <summary>
        /// Used to UNZIP TAR.GZ files into a destination folder and returns 
        /// destination folder path. for Linux HHU
        /// </summary>
        /// <param name="sourceFilePath"></param>
        /// <returns></returns>
        private string UnZipTARGZFiles(string sourceFilePath)
        {

            string destPath = string.Concat(AppDomain.CurrentDomain.BaseDirectory, @"DLMSCommunication\Temp");
            try
            {
                Stream inStream = File.OpenRead(sourceFilePath);
                Stream gzipStream = new GZipInputStream(inStream);
                TarArchive tarArchive = TarArchive.CreateInputTarArchive(gzipStream);
                tarArchive.ExtractContents(destPath);
                tarArchive.Close();
                gzipStream.Close();
                inStream.Close();
            }
            catch (Exception ex)    //Exception log for catch block
            {
                //MessageBox.Show("Unable to Unzip the TAR.GZ");
                MessageBox.Show("Communication Failed !");
                logger.Log(LOGLEVELS.Error, "UnZipTARGZFiles(string sourceFilePath)", ex);
            }
            return destPath;
        }



        /// <summary>
        /// 
        /// </summary>
        private void SaveDLMSAndIECFiles()
        {
            string dlmsFilePath = string.Empty;
            string iecFilePath = string.Empty;
            string newDLMSFileName = string.Empty;
            string newIECFileName = string.Empty;
            string fileText = string.Empty;
            string fileNames = string.Empty;
            string configFilePath = ConfigInfo.CheckOrCreatePath();
            StreamReader streamReader = null;
            FileStream fileStream = null;
            StreamWriter strWriter = null;

            try
            {
                string fileName = System.DateTime.Now.ToString("ddMMyyyyHHmmss");
                if (fileName.Length >= 14)
                {
                    fileName = fileName.Substring(0, 8) + "_" + fileName.Substring(8, 6);
                }
                if (!string.IsNullOrEmpty(DLMSFileName))
                {
                    newDLMSFileName = string.Concat(fileName, ".2NG");
                    dlmsFilePath = string.Concat(configFilePath, "\\", newDLMSFileName).Replace("\\\\", "\\");

                    streamReader = new StreamReader(DLMSFileName);
                    fileText = streamReader.ReadToEnd();
                    streamReader.Close();
                    File.Delete(DLMSFileName);

                    fileStream = new FileStream(dlmsFilePath, FileMode.Create);
                    strWriter = new StreamWriter(fileStream);
                    strWriter.Write(fileText);
                    strWriter.Close();
                    fileStream.Close();

                    fileNames = newDLMSFileName + Symbols.NEWLINE;
                }
                if (!string.IsNullOrEmpty(IECFileText))
                {
                    newIECFileName = string.Concat(fileName, ".CAB");
                    iecFilePath = string.Concat(ConfigInfo.CheckOrCreatePath(), "\\", newIECFileName).Replace("\\\\", "\\");

                    fileStream = new FileStream(iecFilePath, FileMode.Create);
                    strWriter = new StreamWriter(fileStream);
                    IECFileText = ConfigInfo.EncryptFile(IECFileText);
                    strWriter.Write(IECFileText);
                    strWriter.Close();
                    fileStream.Close();
                    //Clear File Text
                    IECFileText = string.Empty;

                    fileNames += newIECFileName + Symbols.NEWLINE;
                }
                MessageBox.Show("Readout File(s) " + Symbols.NEWLINE + fileNames + " are saved at " + configFilePath, "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                bool IsUploaded = false;
                UploadFile uploadFile = new UploadFile();
                this.Cursor = Cursors.WaitCursor;
                this.StatusMessage = "Uploading Readout file";
                Application.DoEvents();
                string resultMessage = string.Empty;
                ConfigSettings.ChangeNode("SourceOfFile", ((int)CommTypes.CMRI).ToString());
                if (!string.IsNullOrEmpty(dlmsFilePath))
                {
                    IsUploaded = uploadFile.Upload2NGFile(dlmsFilePath, uploadFile.GetContent(dlmsFilePath), true, out resultMessage, null);
                }
                if (!string.IsNullOrEmpty(iecFilePath))
                {
                    IsUploaded = IsUploaded = uploadFile.UploadCABFile(iecFilePath, uploadFile.GetIECFileContent(iecFilePath), true, out resultMessage, null);
                }
                //uploadFile.DeleteFile();
                if (IsUploaded)
                {
                    this.ListRefresh = true;
                    this.StatusMessage = "File Uploaded successfully.";
                    Application.DoEvents();
                }
                else
                {
                    this.StatusMessage = resultMessage;
                }

            }
            catch (Exception Ex)    //Exception log for catch block
            {
                MessageBox.Show(Ex.ToString());
                logger.Log(LOGLEVELS.Error, "SaveDLMSAndIECFiles()", Ex);
            }
            finally
            {
                this.Cursor = Cursors.Default;
                this.RightStatusMessage = String.Empty;
            }

        }


        /// <summary>
        /// 
        /// </summary>
        private void SaveDLMSAndIECFiles1()
        {
            string dlmsFilePath = string.Empty;
            string iecFilePath = string.Empty;
            string newDLMSFileName = string.Empty;
            string newIECFileName = string.Empty;
            string fileText = string.Empty;
            string fileNames = string.Empty;
            string configFilePath = ConfigInfo.CheckOrCreatePath();
            StreamReader streamReader = null;
            FileStream fileStream = null;
            StreamWriter strWriter = null;
            string dlmsFilePathData = string.Empty;

            try
            {
                //string fileName = System.DateTime.Now.ToString("ddMMyyyyHHmmss");
                //if (fileName.Length >= 14)
                //{
                //    fileName = fileName.Substring(0, 8) + "_" + fileName.Substring(8, 6);
                //}
                if (!string.IsNullOrEmpty(DLMSFileName))
                {
                    streamReader = new StreamReader(DLMSFileName);
                    fileText = streamReader.ReadToEnd();
                    streamReader.Close();
                    File.Delete(DLMSFileName);

                    string[] allFileContent = fileText.Split('$');
                    if (allFileContent.Length > 0)
                    {
                        for (int i = 1; i < allFileContent.Length; i++)
                        {
                            string fileName = System.DateTime.Now.ToString("ddMMyyyyHHmmss");
                            if (fileName.Length >= 14)
                            {
                                fileName = fileName.Substring(0, 8) + "_" + fileName.Substring(8, 6);
                            }
                            newDLMSFileName = string.Concat(fileName, ".2NG");
                            dlmsFilePath = string.Concat(configFilePath, "\\", newDLMSFileName).Replace("\\\\", "\\");
                            // streamReader = new StreamReader(allFileContent[i]);
                            //  fileText = streamReader.ReadToEnd();
                            // streamReader.Close();
                            //File.Delete(DLMSFileName);

                            fileStream = new FileStream(dlmsFilePath, FileMode.Create);
                            strWriter = new StreamWriter(fileStream);
                            strWriter.Write("$" + allFileContent[i]);
                            strWriter.Close();
                            fileStream.Close();

                            fileNames += newDLMSFileName + Symbols.NEWLINE;
                            dlmsFilePathData += dlmsFilePath + "^";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(IECFileText))
                {
                    string fileName = System.DateTime.Now.ToString("ddMMyyyyHHmmss");
                    if (fileName.Length >= 14)
                    {
                        fileName = fileName.Substring(0, 8) + "_" + fileName.Substring(8, 6);
                    }
                    newIECFileName = string.Concat(fileName, ".CAB");
                    iecFilePath = string.Concat(ConfigInfo.CheckOrCreatePath(), "\\", newIECFileName).Replace("\\\\", "\\");

                    fileStream = new FileStream(iecFilePath, FileMode.Create);
                    strWriter = new StreamWriter(fileStream);
                    IECFileText = ConfigInfo.EncryptFile(IECFileText);
                    strWriter.Write(IECFileText);
                    strWriter.Close();
                    fileStream.Close();
                    //Clear File Text
                    IECFileText = string.Empty;

                    fileNames += newIECFileName + Symbols.NEWLINE;
                }
                MessageBox.Show("Readout File(s) " + Symbols.NEWLINE + fileNames + " are saved at " + configFilePath, "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                bool IsUploaded = false;
                UploadFile uploadFile = new UploadFile();
                this.Cursor = Cursors.WaitCursor;
                this.StatusMessage = "Uploading Readout file";
                Application.DoEvents();
                string resultMessage = string.Empty;
                ConfigSettings.ChangeNode("SourceOfFile", ((int)CommTypes.CMRI).ToString());
                if (!string.IsNullOrEmpty(dlmsFilePath))
                {
                    string[] dataForUpload = dlmsFilePathData.Split('^');
                    for (int i = 0; i < dataForUpload.Length - 1; i++)
                    {
                        IsUploaded = uploadFile.Upload2NGFile(dlmsFilePath, uploadFile.GetContent(dlmsFilePath), true, out resultMessage, null);
                    }

                }
                if (!string.IsNullOrEmpty(iecFilePath))
                {
                    IsUploaded = IsUploaded = uploadFile.UploadCABFile(iecFilePath, uploadFile.GetIECFileContent(iecFilePath), true, out resultMessage, null);
                }
                //uploadFile.DeleteFile();
                if (IsUploaded)
                {
                    this.ListRefresh = true;
                    this.StatusMessage = "File Uploaded successfully.";
                    Application.DoEvents();
                }
                else
                {
                    this.StatusMessage = resultMessage;
                }

            }
            catch (Exception Ex)    //Exception log for catch block
            {
                MessageBox.Show(Ex.ToString());
                logger.Log(LOGLEVELS.Error, "SaveDLMSAndIECFiles1()", Ex);
            }
            finally
            {
                this.Cursor = Cursors.Default;
                this.RightStatusMessage = String.Empty;
            }

        }
        /// <summary>
        ///  Read IEC files from CMRI
        /// </summary>
        private void ReadIECCMRIData()
        {
            cMRIReadout.Channel = communications;
            textRead = cMRIReadout.GetData();
            if (textRead.Trim() != string.Empty)
            {
                if (textRead == "Connection Timeout !")
                {
                    this.StatusMessage = textRead;
                    Application.DoEvents();
                }
                else if (textRead == MessageConstant.GetText("M000038"))
                {
                    this.StatusMessage = MessageConstant.GetText("M000038") + " Please Restart BCS.";
                }
                else if (textRead == "No Data Available.")
                {
                    //this.StatusMessage = textRead;
                    Application.DoEvents();
                }
                else
                {
                    if (ReadoutCommon.CalculateFileBcc(textRead) != string.Empty)
                    {
                        this.StatusMessage = "CMRI readout successful";
                        Application.DoEvents();
                        IECFileText = textRead;//SaveData(textRead);
                    }
                    else
                    {
                        this.StatusMessage = "BCC not Matched";
                        Application.DoEvents();
                    }
                }
            }
        }

        /// <summary>
        /// Diplays the files read from CMRI in a check box list and fills a dictionary
        /// </summary>
        /// <param name="receiveBuffer"></param>
        private void DisplayMeterList(List<byte> receiveBuffer)
        {
            string data = string.Empty;
            int capture_object_definition;
            int counter, lengthCounter = 0;
            int nLength = 0;
            int nByteIndex = 0;
            nByteIndex++;
            capture_object_definition = receiveBuffer[nByteIndex];
            for (counter = 0; counter < capture_object_definition; counter++)
            {
                nByteIndex += 7;
                nLength = receiveBuffer[nByteIndex++];
                // length 06
                data = string.Empty;
                for (lengthCounter = 0; lengthCounter < nLength; lengthCounter++)
                {
                    data = data + Convert.ToChar(receiveBuffer[lengthCounter + nByteIndex]);
                }
                nByteIndex = nByteIndex + (lengthCounter - 1);
                //09 0C 07 DA 0B 1D FF 0B 30 13 FF 80 00 00
                nByteIndex += 3;
                int year = 0;// receivedData[21];
                year = (year | (int)receiveBuffer[nByteIndex++]) << 8;
                year = (year | (int)receiveBuffer[nByteIndex++]);
                int month = receiveBuffer[nByteIndex++];
                int date = receiveBuffer[nByteIndex++];
                int week = receiveBuffer[nByteIndex++];
                int hour = receiveBuffer[nByteIndex++];
                int minute = receiveBuffer[nByteIndex++];
                int second = receiveBuffer[nByteIndex++];
                data = data + " " + date.ToString("d2") + "/" + month.ToString("d2") + "/" + year.ToString("d2") + " " + hour.ToString("d2") + ":" + minute.ToString("d2") + ":" + second.ToString("d2");
                //FDL File
                if (receiveBuffer[nByteIndex] == 253)
                {
                    fdlFileList.Add((byte)counter, data);
                }
                else  //Normal Readout file.
                {
                    dumpFiles.Add((byte)counter, data);
                }

                nByteIndex += 3;
            }
        }

        /// <summary>
        ///  Used to create profileId enums based on profiles
        /// that needs to be read(Selected by user through checkboxes)
        /// </summary>
        /// <returns></returns>
        private List<ProfileId> GetSelectedProfilesToReadCMRI()
        {
            List<ProfileId> selectedProfiles = new List<ProfileId>();
            selectedProfiles.Clear();
            selectedProfiles.Add(ProfileId.NamePlate);
            selectedProfiles.Add(ProfileId.Instant);
            selectedProfiles.Add(ProfileId.Anomaly);
            selectedProfiles.Add(ProfileId.Billing);
            selectedProfiles.Add(ProfileId.LoadSurvey);
            selectedProfiles.Add(ProfileId.Tamper);
            selectedProfiles.Add(ProfileId.Midnight);
            selectedProfiles.Add(ProfileId.Phasor);


            //selectedProfiles.Add(ProfileId.FraudEnergy);
            selectedProfiles.Add(ProfileId.RTC);
            selectedProfiles.Add(ProfileId.BillingType);
            selectedProfiles.Add(ProfileId.DIP);
            selectedProfiles.Add(ProfileId.SIP);

            selectedProfiles.Add(ProfileId.PassiveSeasonProfile);
            selectedProfiles.Add(ProfileId.PassiveWeekProfile);
            selectedProfiles.Add(ProfileId.PassiveDayProfile);
            selectedProfiles.Add(ProfileId.ActiveSeasonProfile);
            selectedProfiles.Add(ProfileId.ActiveWeekProfile);
            selectedProfiles.Add(ProfileId.ActiveDayProfile);
            selectedProfiles.Add(ProfileId.ActivationDate);
            // selectedProfiles.Add(ProfileId.ResetLockOutDays);

            selectedProfiles.Add(ProfileId.KvahSelection);
            selectedProfiles.Add(ProfileId.RS232LockUnlock);
            selectedProfiles.Add(ProfileId.AutoLock);

            selectedProfiles.Add(ProfileId.PushDisplayParameter);
            selectedProfiles.Add(ProfileId.ScrollDisplyParameter);
            selectedProfiles.Add(ProfileId.HighResolutionDisplayParameter);
            //selectedProfiles.Add(ProfileId.DisplayTimeoutParameter); // Story - Hide Display Timeout Parameter
            selectedProfiles.Add(ProfileId.ABCCode);

            return selectedProfiles;
        }
        /// <summary>
        /// Used to Get commands for reading profiles from xml file and deserialize 
        /// that into list of ProFileCommand as return value.
        /// </summary>
        /// <returns></returns>
        private List<ProfileCommand> GetProfileCommandEntity()
        {
            DLMS profileCommands = (DLMS)new Serializer().DeserializeToObject("CommandRepository.xml", typeof(DLMS));
            List<ProfileCommand> lstProfileCommands = new List<ProfileCommand>();
            ProfileCommand profileCommandEntity;
            foreach (DLMSCOMMAND dlmsCommand in profileCommands.Items)
            {
                profileCommandEntity = new ProfileCommand();
                profileCommandEntity.TagNumber = Convert.ToInt32(dlmsCommand.TAGNO);
                profileCommandEntity.Attribute = Convert.ToByte(dlmsCommand.ATTRIBUTE);
                profileCommandEntity.ClassId = Convert.ToByte(dlmsCommand.CLASS);
                profileCommandEntity.ObisCode = dlmsCommand.OBISCODE;
                profileCommandEntity.MeterModelNumber = Convert.ToByte(dlmsCommand.METERMODEL);
                lstProfileCommands.Add(profileCommandEntity);
            }
            return lstProfileCommands;
        }
        /// <summary>
        /// Gets DLMS Command object from command repository using OBIS code, classId , attribute.
        /// </summary>
        /// <param name="obisCode"></param>
        /// <param name="classID"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        private DLMSCOMMAND GetCommandFromCommandRepository(string obisInfo)
        {
            DLMSCOMMAND command = null;
            byte classID = Convert.ToByte(obisInfo.Substring(0, 2), 16);
            string obisCode = obisInfo.Substring(2, 12);
            byte attribute = Convert.ToByte(obisInfo.Substring(14, 2), 16);
            foreach (DLMSCOMMAND dlmsCommand in commandRepository.Items)
            {
                if ((dlmsCommand.OBISCODE.Replace(".", "").ToUpper().Replace("METERID", "FF") == obisCode)
                    && (Convert.ToByte(dlmsCommand.CLASS) == classID)
                    && (Convert.ToByte(dlmsCommand.ATTRIBUTE) == attribute))
                {
                    command = dlmsCommand;
                    break;
                }
            }
            return command;
        }

        /// <summary>
        /// Checks the directory at the specified location, create one if it doesnt exists.
        /// </summary>
        /// <param name="folderName"></param>
        private void CheckDirectory(string folderName)
        {
            try
            {
                if (!Directory.Exists(folderName))
                {
                    Directory.CreateDirectory(folderName);
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "CheckDirectory(string folderName)", ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private CommunicationErrorType ReadE650CMRIData()
        {
            //Read list of files .           
            Result result = communication.OpenSession();
            bool readSuccess = false;
            List<byte> meterId = null;
            if (result.ErrorCode == CommunicationErrorType.Success)
            {

                meterLoadList = new Dictionary<string, int>();
                //Initialize and instantiate the profile command for loading meter list from CMRI
                ProfileCommand profileCommand = new ProfileCommand(0x11, "00.00.41.00.00.FF", 02);
                result = communication.Send(profileCommand);
                if (result.ErrorCode == CommunicationErrorType.Success && result.RecieveDataBuffer.Count > 0)
                {
                    #region LoadFileList
                    meterLoadList.Clear();
                    dumpFiles.Clear();
                    fdlFileList.Clear();
                    //lstCMRIfile.Items.Clear();                
                    DisplayMeterList(result.RecieveDataBuffer);
                    communication.CloseSession();
                    #endregion

                    if (dumpFiles.Count > 0 || fdlFileList.Count > 0)
                    {

                        //Read each file data        
                        string message = string.Empty;
                        string meterID = string.Empty;
                        string readingDateTime = string.Empty;
                        bool isSessionOpen = false;
                        int meterModelNumber = 2;
                        string classIdOBISCodeAttribute = string.Empty;
                        FileStream initialFileStream = null;
                        StreamWriter writeToFile = null;
                        List<ProfileId> selectedProfiles = GetSelectedProfilesToReadCMRI();
                        List<ProfileCommand> lstProfileCommands = GetProfileCommandEntity();
                        //Creates the directory if it doesnt exixts
                        CheckDirectory(string.Concat(AppDomain.CurrentDomain.BaseDirectory, @"DLMSCommunication\"));
                        string fileName = AppDomain.CurrentDomain.BaseDirectory + @"DLMSCommunication\Temp";
                        try
                        {
                            initialFileStream = new FileStream(fileName, FileMode.Append);
                            writeToFile = new StreamWriter(initialFileStream);

                            #region NormlaReadout
                            foreach (string file in dumpFiles.Values)
                            {
                                this.Cursor = Cursors.WaitCursor;
                                // String fileName = string.Empty;
                                int index = 0;
                                //get the corresponding SAP value in dictionary
                                //this.StatusMessage = "Reading CMRI Data...";                               
                                index = dumpFiles.Where(kvp => kvp.Value == file).Select(kvp => kvp.Key).FirstOrDefault();
                                //Try to open session i.e open the port and connect to CMRI with given SAP
                                communication.SetSAP((byte)(index + 2));
                                result = communication.OpenSession();
                                if (result.ErrorCode == CommunicationErrorType.Success)
                                {
                                    isSessionOpen = true;
                                    //fileName = GetFileName(file);
                                    readingDateTime = GetReadingDateTime(file);
                                    if (!string.IsNullOrEmpty(fileName))
                                    {
                                        writeToFile.WriteLine(Splitter);
                                        writeToFile.WriteLine((file.Length - 20).ToString("00"));
                                        writeToFile.WriteLine(readingDateTime);
                                        //Get Signature data
                                        string signatureInfo = communication.GetSignatureData().ToUpper();
                                        //Write signature data in file
                                        writeToFile.WriteLine(GetFormattedSignatureData(signatureInfo));
                                        #region Read selected profiles
                                        foreach (ProfileId selectedProfile in selectedProfiles)
                                        {
                                            Application.DoEvents();
                                            List<ProfileCommand> profileReadCommands = GetProfileCommandsToRead(lstProfileCommands, selectedProfile, meterModelNumber, CommunicationMode.Normal);
                                            for (index = 0; index < profileReadCommands.Count; index++)
                                            {
                                                if (result.ErrorCode == CommunicationErrorType.Success)
                                                {
                                                    profileReadCommands[index].Action = ActionType.READ;
                                                    result = communication.Send(profileReadCommands[index]);
                                                    if (result.ErrorCode == CommunicationErrorType.Success)
                                                    {
                                                        classIdOBISCodeAttribute = String.Format("{0:X2}", profileReadCommands[index].ClassId)
                                                                                   + profileReadCommands[index].ObisCode.Replace(".", "")
                                                                                   + String.Format("{0:X2}", profileReadCommands[index].Attribute);
                                                        if ((result.RecieveDataLength <= 0)
                                                            // Chnaged by Gopal for CMRI support in case of No data found for Configuration parameter
                                                            || (((int)selectedProfile >= 101) && result.RecieveDataLength == 2 && (result.RecieveDataBuffer[0] == 1 && result.RecieveDataBuffer[1] == 0))
                                                            // Chnaged by Gopal for CMRI support in case of No data found for Configuration parameter
                                                            || (result.RecieveDataLength < 33 && selectedProfile == ProfileId.Phasor))
                                                        {

                                                        }
                                                        else
                                                        {
                                                            writeToFile.Write(classIdOBISCodeAttribute);
                                                            for (int i = 0; i < result.RecieveDataLength; i++)
                                                            {
                                                                writeToFile.Write(String.Format("{0:X2}", result.RecieveDataBuffer[i]));
                                                            }
                                                            writeToFile.WriteLine("");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        break;
                                                    }
                                                }
                                            }
                                            if (result.ErrorCode != CommunicationErrorType.Success)
                                            {
                                                break;
                                            }
                                        }
                                        #endregion

                                        if (result.ErrorCode == CommunicationErrorType.Success)
                                        {
                                            communication.CloseSession();
                                            isSessionOpen = false;
                                        }
                                        else
                                        {
                                            this.StatusMessage = CommonBLL.GetEnumDescription(result.ErrorCode);
                                            Application.DoEvents();
                                            break;
                                        }
                                    }

                                }

                            }
                            #endregion

                            #region FDLFileReadout
                            foreach (string file in fdlFileList.Values)
                            {
                                readSuccess = false;
                                this.Cursor = Cursors.WaitCursor;
                                int index = 0;
                                //get the corresponding SAP value in dictionary                                
                                index = index = fdlFileList.Where(kvp => kvp.Value == file).Select(kvp => kvp.Key).FirstOrDefault();
                                //Try to open session i.e open the port and connect to CMRI with given SAP
                                communication.SetSAP((byte)(index + 2));
                                result = communication.OpenSession();
                                if (result.ErrorCode == CommunicationErrorType.Success)
                                {
                                    isSessionOpen = true;
                                    readingDateTime = GetReadingDateTime(file);
                                    meterId = GetMeterId(file);
                                    if (!string.IsNullOrEmpty(fileName))
                                    {
                                        writeToFile.WriteLine(Splitter);
                                        writeToFile.WriteLine((file.Length - 20).ToString("00"));
                                        writeToFile.WriteLine(readingDateTime);
                                        #region Read selected profiles

                                        //VBM - Only Disconnect Device , no port closing , this is Specific demand from CMRI
                                        //bcz CMRI need to be disconnected from DLMS mode before moving on with FD commands.
                                        communication.CloseRemoteSession();

                                        /*Read Signature Data from CMRI in Fast Mode*/
                                        profileCommand = new ProfileCommand(0xFF, "MeterID.EE.A4.00.00.10", 0xFF);
                                        profileCommand.MeterID = meterId;
                                        profileCommand.Action = ActionType.READ;
                                        result = communication.Send(profileCommand);
                                        /*Read Signature Data from CMRI in Fast Mode*/

                                        if (result.ErrorCode == CommunicationErrorType.Success)
                                        {
                                            writeToFile.WriteLine(GetFormattedSignatureData(result));
                                            foreach (ProfileId selectedProfile in selectedProfiles)
                                            {
                                                //As FDL Readout does not support configuration data readout.
                                                if ((int)selectedProfile >= 101)
                                                {
                                                    continue;
                                                }
                                                //this.StatusMessage = CommonBLL.GetEnumDescription(selectedProfile) + " read in progress..";
                                                Application.DoEvents();
                                                List<ProfileCommand> profileReadCommands = GetProfileCommandsToRead(lstProfileCommands, selectedProfile,
                                                                                                       2, CommunicationMode.FastDownload);
                                                for (index = 0; index < profileReadCommands.Count; index++)
                                                {

                                                    profileReadCommands[index].Action = ActionType.READ;
                                                    profileReadCommands[index].MeterID = meterId;
                                                    result = communication.Send(profileReadCommands[index]);
                                                    if (result.ErrorCode == CommunicationErrorType.Success)
                                                    {
                                                        classIdOBISCodeAttribute = String.Format("{0:X2}", profileReadCommands[index].ClassId)
                                                                                   + profileReadCommands[index].ObisCode.Replace(".", "").ToUpper().Replace("METERID", "FF")
                                                                                   + String.Format("{0:X2}", profileReadCommands[index].Attribute);

                                                        if (result.RecieveDataLength > 0)
                                                        {
                                                            readSuccess = true;
                                                            writeToFile.Write(classIdOBISCodeAttribute);
                                                            for (int i = 0; i < result.RecieveDataLength; i++)
                                                            {
                                                                writeToFile.Write(String.Format("{0:X2}", result.RecieveDataBuffer[i]));
                                                            }
                                                            writeToFile.WriteLine("");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        break;
                                                    }

                                                }
                                                if (result.ErrorCode != CommunicationErrorType.Success && !readSuccess)
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                        #endregion
                                        //Disconnect and close port.
                                        communication.CloseSession();
                                        if (result.ErrorCode != CommunicationErrorType.Success && !readSuccess)
                                        {
                                            this.StatusMessage = CommonBLL.GetEnumDescription(result.ErrorCode);
                                            Application.DoEvents();
                                            break;
                                        }
                                    }

                                }
                                else
                                {
                                    this.StatusMessage = CommonBLL.GetEnumDescription(result.ErrorCode);
                                }

                            }
                            #endregion

                            if (result.ErrorCode == CommunicationErrorType.Success)
                            {
                                writeToFile.Close();
                                initialFileStream.Close();
                                String strChecksum = GetMD5ChecksumForFile(fileName);
                                FileStream fileStream = new FileStream(fileName, FileMode.Append);
                                StreamWriter writeStream = new StreamWriter(fileStream);
                                writeStream.WriteLine(strChecksum);
                                writeStream.Close();
                                fileStream.Close();
                                //this.StatusMessage = "Read out completed.";
                                Application.DoEvents();
                                DLMSFileName = fileName;
                                //SaveE650Data(fileName);
                                // isFileCreatedSuccessfully = true;

                            }
                        }
                        catch (Exception ex)    //Exception log for catch block
                        {
                            communication.CloseSession();
                            logger.Log(LOGLEVELS.Error, "ReadE650CMRIData()", ex);
                        }
                        finally
                        {
                            //if (File.Exists(fileName) && isFileCreatedSuccessfully)
                            //{
                            //    File.Delete(fileName);
                            //}
                            if (isSessionOpen)
                            {
                                communication.CloseSession();
                            }
                            if (initialFileStream != null)
                            {
                                initialFileStream.Close();
                            }
                            // this.Cursor = Cursors.Default;
                        }
                    }
                    else if (dumpFiles.Count == 0 && fdlFileList.Count == 0 && !UtilityDetails.IECSupport)
                    {
                        this.StatusMessage = "No readout files found.";
                        Application.DoEvents();
                    }
                }
                else
                {
                    communication.CloseSession();
                    this.StatusMessage = "Failure in Reading CMRI.";
                }
            }
            else if (result.ErrorCode == CommunicationErrorType.InvalidServerAddress)
            {
                this.StatusMessage = "Access Denied.Please Change Mode!";
            }
            else if (result.ErrorCode != CommunicationErrorType.Success)
            {
                if (result.ErrorCode == CommunicationErrorType.ResponseTimeout)
                {
                    this.StatusMessage = "TimeOut!";
                }
                else
                {
                    this.StatusMessage = CommonBLL.GetEnumDescription(result.ErrorCode);
                }
                //MessageBox.Show(CommonBLL.GetEnumDescription(result.ErrorCode), "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return result.ErrorCode;

        }

        /// <summary>
        /// Gets the signature data in file format for FD Mode
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private string GetFormattedSignatureData(Result result)
        {
            string signatureInfo = string.Empty;
            if (result != null && result.ErrorCode == CommunicationErrorType.Success && result.RecieveDataLength != 0)
            {
                for (int buffIndex = 2; buffIndex < result.RecieveDataLength; buffIndex++)
                {
                    signatureInfo = signatureInfo + Convert.ToChar(result.RecieveDataBuffer[buffIndex]).ToString();
                }
                signatureInfo = signatureInfo + "4C";
            }
            string outputSignatureInfo = "0100006001BCFF020914";//322E34393234303031303036305743347253";
            byte[] dataInByteForm = System.Text.Encoding.ASCII.GetBytes(signatureInfo);

            for (int dataIndex = 0; dataIndex < signatureInfo.Length; dataIndex++)
            {
                outputSignatureInfo = outputSignatureInfo + String.Format("{0:X2}", dataInByteForm[dataIndex]);
            }
            return outputSignatureInfo;
        }

        /// <summary>
        /// Gets the signature data in file format for Normal Mode.
        /// </summary>
        /// <param name="signatureInfo"></param>
        /// <returns></returns>
        private string GetFormattedSignatureData(string signatureInfo)
        {
            string outputSignatureInfo = "0100006001BCFF020914";//322E34393234303031303036305743347253";
            byte[] dataInByteForm = SoapHexBinary.Parse(signatureInfo).Value;
            return outputSignatureInfo = outputSignatureInfo + signatureInfo.Substring(4);

        }

        /// <summary>
        /// Save DLMS File Text
        /// </summary>
        /// <param name="fileText"></param>
        public void SaveE650Data(string inputFile)
        {
            StreamReader streamReader = new StreamReader(inputFile);
            string fileText = streamReader.ReadToEnd();
            streamReader.Close();
            File.Delete(inputFile);

            string fileName = ReadoutCommon.GetFileName().Trim();
            bool Flag = false;
        ReConf:
            do
            {
            AMT:
                if (ReadoutCommon.ReadoutMessageBox(ref fileName, DialogType.Common) == DialogResult.OK)
                {
                    if (string.IsNullOrEmpty(fileName))
                    {
                        this.StatusMessage = "File name cann't be empty.";
                        this.RightStatusMessage = "";
                        Application.DoEvents();
                        goto AMT;
                    }
                    if (ReadoutCommon.ValidFileName(fileName))
                        Flag = true;
                }
                else
                {
                    this.StatusMessage = string.Empty;
                    return;
                }
            } while (!Flag);
            if (fileName.Trim().Equals(string.Empty) || Flag == false)
            {
                this.StatusMessage = MessageConstant.GetText("M000047");
                return;
            }
            string filePath = string.Concat(ConfigInfo.CheckOrCreatePath(), "\\", fileName, ".2NG");
            filePath = filePath.Replace("\\\\", "\\");
            if (File.Exists(filePath))
            {
                DialogResult dr = CABMessageBox.ShowFilterMessage("M000046", "A000001", MessageBoxButtons.YesNo);
                if (dr.Equals(DialogResult.No))
                {
                    goto ReConf;
                }
            }
            try
            {
                FileStream file = new FileStream(filePath, FileMode.Create);
                StreamWriter wr1 = new StreamWriter(file);
                //fileText = ConfigInfo.EncryptFile(fileText);
                wr1.Write(fileText);
                wr1.Close();
                file.Close();
                //MessageBox.Show("File saved at " + filePath, "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information); //MessageConstant.GetText("M000048");
                //this.StatusMessage = "Please Enter the CMRI ID";
                //CMRIID cmriID = new CMRIID(false);
                //cmriID.OnValues_Submission += new CMRIID.GetSubmittedValues(cmriID_OnValuesSubmission);
                //cmriID.ShowDialog();
                //if (this.cmriID.Length == 0)
                //    return;
                bool IsUploaded = false;
                UploadFile uploadFile = new UploadFile();
                this.Cursor = Cursors.WaitCursor;
                this.StatusMessage = "Uploading Readout file";
                Application.DoEvents();
                string resultMessage = string.Empty;
                //uploadFile.cmriID = this.cmriID;
                ConfigSettings.ChangeNode("SourceOfFile", ((int)CommTypes.CMRI).ToString());
                IsUploaded = uploadFile.Upload2NGFile(filePath, uploadFile.GetContent(filePath), true, out resultMessage, null);
                //uploadFile.DeleteFile();

                if (IsUploaded)
                {
                    this.Cursor = Cursors.Default;
                    this.ListRefresh = true;
                    this.RightStatusMessage = String.Empty;
                    this.StatusMessage = "File Uploaded successfully.";
                    Application.DoEvents();
                }
                else
                {
                    this.Cursor = Cursors.Default;
                    this.RightStatusMessage = String.Empty;
                    this.StatusMessage = resultMessage;
                }
            }
            catch (Exception Ex)    //Exception log for catch block
            {
                MessageBox.Show(Ex.ToString());
                logger.Log(LOGLEVELS.Error, "SaveE650Data(string inputFile)", Ex);
            }

        }


        /// <summary>
        /// Save DLMS File Text
        /// </summary>
        /// <param name="fileText"></param>
        public void SaveE650Data1(string inputFile)
        {
            StreamReader streamReader = new StreamReader(inputFile);
            string fileText = streamReader.ReadToEnd();
            streamReader.Close();
            File.Delete(inputFile);

            string configFilePath = ConfigInfo.CheckOrCreatePath();
            string fileNames = string.Empty;
            string resultMessage = string.Empty;
            bool IsUploaded = false;
            string dlmsFilePathData = string.Empty;
            string[] allFileContent = fileText.Split('$');
            try
            {
                if (allFileContent.Length > 0)
                {
                    for (int i = 1; i < allFileContent.Length; i++)
                    {
                        string fileName = ReadoutCommon.GetFileName().Trim();
                        //    bool Flag = false;
                        //ReConf:
                        //    do
                        //    {
                        //    AMT:
                        //        if (ReadoutCommon.ReadoutMessageBox(ref fileName, DialogType.Common) == DialogResult.OK)
                        //        {
                        //            if (string.IsNullOrEmpty(fileName))
                        //            {
                        //                this.StatusMessage = "File name cann't be empty.";
                        //                this.RightStatusMessage = "";
                        //                Application.DoEvents();
                        //                goto AMT;
                        //            }
                        //            if (ReadoutCommon.ValidFileName(fileName))
                        //                Flag = true;
                        //        }
                        //        else
                        //        {
                        //            this.StatusMessage = string.Empty;
                        //            return;
                        //        }
                        //    } while (!Flag);
                        //if (fileName.Trim().Equals(string.Empty) || Flag == false)
                        //{
                        //    this.StatusMessage = MessageConstant.GetText("M000047");
                        //    return;
                        //}
                        string filePath = string.Concat(ConfigInfo.CheckOrCreatePath(), "\\", fileName, ".2NG");
                        filePath = filePath.Replace("\\\\", "\\");
                        //if (File.Exists(filePath))
                        //{
                        //    DialogResult dr = CABMessageBox.ShowFilterMessage("M000046", "A000001", MessageBoxButtons.YesNo);
                        //    if (dr.Equals(DialogResult.No))
                        //    {
                        //        goto ReConf;
                        //    }
                        //}

                        FileStream file = new FileStream(filePath, FileMode.Create);
                        StreamWriter wr1 = new StreamWriter(file);
                        //fileText = ConfigInfo.EncryptFile(fileText);
                        wr1.Write("$" + allFileContent[i].ToString());
                        wr1.Close();
                        file.Close();
                        //MessageBox.Show("File saved at " + filePath, "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information); //MessageConstant.GetText("M000048");
                        //this.StatusMessage = "Please Enter the CMRI ID";
                        //CMRIID cmriID = new CMRIID(false);
                        //cmriID.OnValues_Submission += new CMRIID.GetSubmittedValues(cmriID_OnValuesSubmission);
                        //cmriID.ShowDialog();
                        //if (this.cmriID.Length == 0)
                        //    return;
                        fileNames += fileName + ".2NG" + Symbols.NEWLINE;
                        dlmsFilePathData += filePath + "^";
                    }
                    MessageBox.Show("Readout File(s) " + Symbols.NEWLINE + fileNames + " are saved at " + configFilePath, "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    IsUploaded = false;
                    UploadFile uploadFile = new UploadFile();
                    this.Cursor = Cursors.WaitCursor;
                    this.StatusMessage = "Uploading Readout file";
                    Application.DoEvents();
                    //string resultMessage = string.Empty;
                    //uploadFile.cmriID = this.cmriID;
                    ConfigSettings.ChangeNode("SourceOfFile", ((int)CommTypes.CMRI).ToString());

                    //IsUploaded = uploadFile.Upload2NGFile(filePath, uploadFile.GetContent(filePath), true, out resultMessage);
                    string[] dataForUpload = dlmsFilePathData.Split('^');
                    for (int i = 0; i < dataForUpload.Length - 2; i++)
                    {
                        IsUploaded = uploadFile.Upload2NGFile(dataForUpload[i].ToString(), uploadFile.GetContent(dataForUpload[i].ToString()), true, out resultMessage, null);

                    }

                    //uploadFile.DeleteFile();

                    if (IsUploaded)
                    {
                        this.Cursor = Cursors.Default;
                        this.ListRefresh = true;
                        this.RightStatusMessage = String.Empty;
                        this.StatusMessage = "File Uploaded successfully.";
                        Application.DoEvents();
                    }
                    else
                    {
                        this.Cursor = Cursors.Default;
                        this.RightStatusMessage = String.Empty;
                        this.StatusMessage = resultMessage;
                    }

                }
            }
            catch (Exception Ex)    //Exception log for catch block
            {
                MessageBox.Show(Ex.ToString());
                logger.Log(LOGLEVELS.Error, "SaveE650Data1(string inputFile)", Ex);
            }
        }

        /// <summary>
        /// Creates and returns MD5 CheckSum
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        private static string GetMD5ChecksumForFile(string fileName)
        {
            if (fileName == null)
                throw new ArgumentNullException("The 'filename' parameter cannot be null.");

            if (!File.Exists(fileName))
                throw new ArgumentException(string.Format("Filename '{0}' does not exist.", fileName));

            using (FileStream fileStream = new FileStream(fileName, FileMode.Open))
            {
                byte[] hashValue = new MD5CryptoServiceProvider().ComputeHash(fileStream);
                StringBuilder checkSum = new StringBuilder();
                foreach (byte hasByte in hashValue)
                {
                    checkSum.Append(hasByte.ToString("X2"));
                }
                return checkSum.ToString().ToUpper();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lstProfileCommands"></param>
        /// <param name="selectedProfile"></param>
        /// <param name="meterModelNumber"></param>
        /// <returns></returns>
        private List<ProfileCommand> GetProfileCommandsToRead(List<ProfileCommand> lstProfileCommands, ProfileId selectedProfile, int meterModelNumber, CommunicationMode commMode)
        {
            List<ProfileCommand> profileReadCommands = null;
            if (commMode == CommunicationMode.Normal)
            {
                //find normal commands
                profileReadCommands = lstProfileCommands.FindAll(delegate(ProfileCommand profileCommandEntity)
                {
                    return profileCommandEntity.TagNumber == (int)selectedProfile
                    && (profileCommandEntity.ClassId != 0xFF)
                    && (profileCommandEntity.MeterModelNumber == meterModelNumber ||
                    profileCommandEntity.MeterModelNumber == 0);
                });
            }
            else
            {
                //find fast download commands
                profileReadCommands = lstProfileCommands.FindAll(delegate(ProfileCommand profileCommandEntity)
                {
                    return (profileCommandEntity.TagNumber == (int)selectedProfile)
                    && (profileCommandEntity.ClassId == 0xFF)
                    && (profileCommandEntity.MeterModelNumber == meterModelNumber ||
                    profileCommandEntity.MeterModelNumber == 0);
                });

            }
            return profileReadCommands;
        }
        /// <summary>
        /// Get Reading date time
        /// </summary>
        /// <param name="fileNameWithTimeStamp"></param>
        /// <returns></returns>
        private string GetReadingDateTime(string fileNameWithTimeStamp)
        {
            string readinDateTime = string.Empty;
            int meterIDIndex = fileNameWithTimeStamp.Length - 20;
            return readinDateTime = fileNameWithTimeStamp.Substring(meterIDIndex).Trim(' ');
        }
        /// <summary>
        /// Get Reading date time
        /// </summary>
        /// <param name="fileNameWithTimeStamp"></param>
        /// <returns></returns>
        private List<byte> GetMeterId(string fileNameWithTimeStamp)
        {
            List<byte> meterId = new List<byte>();
            string meterIdBuffer = ToHex(fileNameWithTimeStamp.Substring(0, fileNameWithTimeStamp.Length - 20));
            meterId.AddRange(SoapHexBinary.Parse(meterIdBuffer).Value);
            return meterId;
        }
        private void Channel_OnStatusChanged(string msg)
        {
            this.StatusMessage = msg;
        }
        private void btnUpdateRTC_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.StatusMessage = "RTC Updating...";
                Application.DoEvents();
                Result result = communication.CheckCMRICommunicationType((byte)CMRICommunicationType.DLMSUpdateCMRIRTC,
                                                                            ConfigSettings.GetValue("PortName"),
                                                                            ConfigSettings.GetValue("BaudRate"));
                Thread.Sleep(4000);
                Application.DoEvents();
                UpdateCMRIRTCForDLMS();
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "btnUpdateRTC_Click(object sender, EventArgs e)", ex);
            }
            finally
            {
                this.Cursor = Cursors.Default;
                SetConnectionDetail(false);
                StopProgressBarTimer();
            }
        }
        /// <summary>
        /// Update CMRI RTC to support backward compatability.
        /// </summary>
        private void UpdateCMRIRTC(bool isDLMSOnly)
        {
            //Generic CMRI where DLMS RTC update will sufficient for bith IEC and DLMS .
            if (isDLMSOnly)
            {
                Result resultStatus = UpdateRTC();
                if (resultStatus.ErrorCode != CommunicationErrorType.Success)
                {
                    if (resultStatus.ErrorCode == CommunicationErrorType.InvalidServerAddress)
                    {
                        this.StatusMessage = "Access Denied.Please Change the Mode !";
                    }
                    else
                    {
                        this.StatusMessage = CommonBLL.GetEnumDescription(resultStatus.ErrorCode);
                        Application.DoEvents();
                    }
                }
                else
                {
                    this.StatusMessage = "RTC Updated";
                }
            }
            else
            {
                //Legacy CMRI Support where Both DLMS and IEC CMRI RTC update commands required!
                cMRIReadout.Channel = communications;
                string result = cMRIReadout.UpdateCMRIRTC().Trim();
                if (result != string.Empty)
                {
                    if (result == "Sign-On Failure!")
                    {
                        Result resultStatus = UpdateRTC();
                        if (resultStatus.ErrorCode != CommunicationErrorType.Success)
                        {
                            if (resultStatus.ErrorCode == CommunicationErrorType.InvalidServerAddress)
                            {
                                this.StatusMessage = "Access Denied.Please Change the Mode !";
                            }
                            else
                            {
                                this.StatusMessage = CommonBLL.GetEnumDescription(resultStatus.ErrorCode);
                                Application.DoEvents();
                            }
                        }
                        else
                        {
                            this.StatusMessage = "RTC Updated";
                        }
                    }
                    else if (result == MessageConstant.GetText("M000038"))
                    {
                        this.StatusMessage = MessageConstant.GetText("M000038") + " Please Restart BCS.";
                    }
                    else
                    {
                        this.StatusMessage = "TimeOut!";
                        Application.DoEvents();
                    }

                }
                else
                {
                    this.StatusMessage = "RTC Updated";
                    Application.DoEvents();
                }
            }
        }
        /// <summary>
        /// Used to update CMRi RTC using DLMS standerd
        /// </summary>
        private void UpdateCMRIRTCForDLMS()
        {
            Result resultStatus = UpdateRTC();
            if (resultStatus.ErrorCode != CommunicationErrorType.Success)
            {
                if (resultStatus.ErrorCode == CommunicationErrorType.InvalidServerAddress)
                {
                    this.StatusMessage = "Access Denied.Please Change the Mode !";
                }
                else
                {
                    this.StatusMessage = CommonBLL.GetEnumDescription(resultStatus.ErrorCode);
                    Application.DoEvents();
                }
            }
            else
            {
                this.StatusMessage = "RTC Updated";
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrepareCMRI_Click(object sender, EventArgs e)
        {
            try
            {
                if (scheduleFileLocation == string.Empty)
                {
                    CABMessageBox.ShowFilterMessage("M000088", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (fileExtension.ToLower().Contains(".lcd"))
                {
                    CABMessageBox.ShowFilterMessage("M000088", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Result result = communication.CheckCMRICommunicationType((byte)CMRICommunicationType.DLMSPrerpareCMRI, ConfigSettings.GetValue("PortName"), ConfigSettings.GetValue("BaudRate"));
                if (result.ErrorCode == CommunicationErrorType.SuccessForDLMS)
                {
                    SetConnectionDetail(true);
                    //Apply Wait cursor here as we have applied
                    this.Cursor = Cursors.WaitCursor;
                    this.StatusMessage = "Preparing CMRI...";
                    Application.DoEvents();
                    Thread.Sleep(4000);
                    if (DLMSPrepareCMRI())
                    {
                        if (GetFileContent().Contains(BCSConstants.IEC))
                        {
                            result = communication.CheckCMRICommunicationType((byte)CMRICommunicationType.IECPrerpareCMRI, ConfigSettings.GetValue("PortName"), ConfigSettings.GetValue("BaudRate"));
                            if (result.ErrorCode == CommunicationErrorType.SuccessForDLMS)
                            {
                                Application.DoEvents();
                                Thread.Sleep(4000);
                                IECPrepareCMRI();
                            }
                        }
                        else
                        {
                            this.StatusMessage = "CMRI Prepared";
                        }
                    }
                }
                else
                {
                    this.StatusMessage = CommonBLL.GetEnumDescription(result.ErrorCode);
                }

            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "btnPrepareCMRI_Click(object sender, EventArgs e)", ex);
            }
            finally
            {
                SetConnectionDetail(false);
                StopProgressBarTimer();
                scheduleFileLocation = string.Empty;
                this.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// updates protocol , mode and connected/disconnected the right side in status bar  
        /// </summary>
        /// <param name="isConnected"></param>
        private void SetConnectionDetail(bool connected)
        {

            string channelType = ConfigSettings.GetValue("ChannelType");
            string mode;
            if (connected)
            {

                mode = Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism")) == 1 ? BCSConstants.ReaderMode : BCSConstants.MasterMode;
                this.ConnectionDetailStatusMessageAsync = "Connection: " + channelType + "(" + "CMRI" + ")" + ", Port: " + ConfigSettings.GetValue("PortName") + ", Mode: " + mode;

                Application.DoEvents();
            }
            else
            {

                mode = Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism")) == 1 ? BCSConstants.ReaderMode : BCSConstants.MasterMode;
                this.ConnectionDetailStatusMessageAsync = "Connection: " + "Not Connected" + ", Port: " + ConfigSettings.GetValue("PortName") + ", Mode: " + mode;


            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool DLMSPrepareCMRI()
        {
            bool result = false;
            CommunicationErrorType output = PrepareCMRILT();
            if (output != CommunicationErrorType.Success)
            {
                if (output == CommunicationErrorType.InvalidServerAddress)
                {
                    this.StatusMessage = "Access Denied.Please Change the Mode !";
                }
                else
                {
                    this.StatusMessage = CommonBLL.GetEnumDescription(output);
                }
            }
            else
            {
                result = true;
                //this.StatusMessage = "CMRI Prepared";
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool IECPrepareCMRI()
        {
            prepareCMRI.Channel = communications;
            string fileData = GetFileContent();
            fileData = fileData.Substring(fileData.IndexOf(BCSConstants.IEC) + 7);
            sCDFileCommand = CreateSendTouFileCmd(fileData, "Schedule");
            if (sCDFileCommand.Length <= 0)
            {
                CABMessageBox.ShowFilterMessage("M000099", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (sCDFileCommand == "1")
            {
                CABMessageBox.ShowFilterMessage("M000087", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            //Assigning the Schedule file for preparing the CMRI
            prepareCMRI.ScheduleFileCommand = sCDFileCommand;
            /* For CSPDCL genrric BCS We need to implement CMRI Preparetion without using separate TOU file
                As CMRI is not going to change logic from their end so we need to take help of temp file which is just  apalce holder it's not existing 
                 in real */
            prepareCMRI.TOUFileName = "test.tou";
            //tOUFileCommand = TouFileCmd(prepareCMRI.TOUFileName, "TOU");
            tOUFileCommand = GetTOUCommandFromScheduleFile(fileData, "TOU");
            if (tOUFileCommand != string.Empty)
            {
                prepareCMRI.TOUFileCommand = tOUFileCommand;
            }
            else
            {
                this.StatusMessage = "File Not available at the Configured Location";
                return false;
            }

            //this.Cursor = Cursors.WaitCursor;
            //this.StatusMessage = "Preparing CMRI...";
            //Application.DoEvents();
            if (prepareCMRI.PrepareCMRIData())
            {
                this.StatusMessage = "CMRI Prepared";
                Application.DoEvents();
                return true;
            }
            else
            {
                this.StatusMessage = "Time Out!";
                Application.DoEvents();
                return false;

            }
        }


        /// <summary>
        /// Prepare CMRI For Lt meters
        /// </summary>
        /// <returns></returns>
        private CommunicationErrorType PrepareCMRILT()
        {
            bool isConnected = false;
            Result result = new Result();
            try
            {
                if (!string.IsNullOrEmpty(selectedFile))
                {
                    string fileContent = GetFileContent();
                    if (fileContent.Contains("\n"))
                    {
                        string[] fileText = fileContent.Split(new string[] { BCSConstants.IEC }, StringSplitOptions.None);
                        fileText = fileText[0].Split('\n');
                        if (fileText[0].Trim() == "DLMS")
                        {
                            result = UpdateRTC();
                            if (result.ErrorCode == CommunicationErrorType.Success)
                            {
                                isConnected = true;
                                string baudRate = ConfigSettings.GetValue("BaudRate");

                                if (baudRate.Contains("38400"))
                                    result = communication.OpenSessionCMRI(7);
                                else
                                    result = communication.OpenSession();

                                //result = communication.OpenSession();
                                if (result.ErrorCode == CommunicationErrorType.Success)
                                {
                                    result = WriteUSModePassword();
                                    if (result.ErrorCode == CommunicationErrorType.Success)
                                    {
                                        for (int counter = 1; counter < fileText.Length - 1; counter++)
                                        {
                                            if (fileText[counter] != "\r" && fileText[counter] != "\r\n" && fileText[counter].Length >= 16)
                                            {
                                                DLMSCOMMAND command = GetCommandFromCommandRepository(fileText[counter].Substring(0, 16));
                                                string data = fileText[counter].Substring(16);
                                                ProfileCommand profileCommand = new ProfileCommand(Convert.ToByte(command.CLASS),
                                                    command.OBISCODE, Convert.ToByte(command.ATTRIBUTE));
                                                //28/03/2014 For WB CMRI as asked with validation and shiva BCS need to send RTC while preparing CMRI if its available
                                                //IN LCD file , so commenting the code below
                                                //if (command.OBISCODE == "00.00.01.00.00.FF")
                                                //    continue;
                                                if (command.OBISCODE == "00.01.0A.09.00.FF")
                                                {
                                                    profileCommand.Action = ActionType.RESET;
                                                }
                                                else
                                                {
                                                    profileCommand.WriteDataBuffer = SoapHexBinary.Parse(data).Value.ToList<byte>();
                                                    profileCommand.Action = ActionType.WRITEBUFFER;

                                                }
                                                result = communication.Send(profileCommand);
                                                if (result.ErrorCode != CommunicationErrorType.Success)
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            if (result.ErrorCode == CommunicationErrorType.Success)
                            {
                                //MessageBox.Show("CMRI Prepared successfully!", "BCS");
                                //this.StatusMessage = "CMRI Prepared successfully.";
                            }
                            else
                            {
                                this.StatusMessage = "CMRI Preparation failed.";
                                //this.StatusMessage = CommonBLL.GetEnumDescription(result.ErrorCode);
                            }
                            if (isConnected)
                            {
                                result = communication.CloseSession();
                            }

                        }
                        else
                        {
                            this.StatusMessage = "Invalid Configuration File.";
                        }

                    }
                }
                else
                {
                    CABMessageBox.ShowFilterMessage("M000088", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                this.StatusMessage = "File corrupted";
                communication.CloseSession();
                logger.Log(LOGLEVELS.Error, "PrepareCMRILT()", ex);
            }
            finally
            {
                //this.Cursor = Cursors.Default;
            }

            return result.ErrorCode;
        }

        /// <summary>
        /// Used to write US Mode password to CMRI.
        /// So that CMRI will not ask password from user .
        /// </summary>
        /// <returns></returns>
        private Result WriteUSModePassword()
        {
            Result result = null;
            try
            {

                string writeDataBuffer = string.Empty;
                writeDataBuffer = "0910" + ToHex(ConfigSettings.GetValue("ModePassword"));
                //This is an exception as we have RTC tag in xml file in instant profile so we can't put it one more time.
                ProfileCommand command = new ProfileCommand(0x0F, "00.00.28.00.03.FF", 2);
                command.ClassName = "CAB.E650MeterConfiguration.TOUConfiguration,E650MeterConfiguration";
                command.Action = ActionType.WRITE;
                command.WriteDataBuffer = (SoapHexBinary.Parse(writeDataBuffer).Value.ToArray<byte>());
                result = communication.Send(command);


            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "WriteUSModePassword()", ex);
            }
            finally
            {

            }
            return result;
        }
        /// <summary>
        /// Used to convert input string into Hex
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private string ToHex(string input)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in input)
            {
                sb.AppendFormat("{0:X2}", (int)c);
            }
            return sb.ToString().Trim();
        }

        /// <summary>
        /// RTC Update
        /// </summary>
        /// <returns></returns>
        private Result UpdateRTC()
        {
            Result result = null;
            bool isConnected = false;
            try
            {
                //This is an exception as we have RTC tag in xml file in instant profile so we can't put it one more time.
                ProfileCommand rtcCommand = new ProfileCommand(8, "00.00.01.00.00.FF", 2);
                rtcCommand.ClassName = "CAB.E650MeterConfiguration.RTC,E650MeterConfiguration";
                rtcCommand.Action = ActionType.WRITE;
                rtcCommand.WriteDataBuffer = DateTime.Now;
                string baudRate = ConfigSettings.GetValue("BaudRate");

                if (baudRate.Contains("38400"))
                    result = communication.OpenSessionCMRI(7);
                else
                    result = communication.OpenSession();

                //result = communication.OpenSession();
                if (result.ErrorCode == CommunicationErrorType.Success)
                {
                    SetConnectionDetail(true);
                    isConnected = true;
                    result = communication.Send(rtcCommand);
                }

            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "UpdateRTC()", ex);
            }
            finally
            {
                if (isConnected)
                {
                    communication.CloseSession();
                }
            }
            return result;
        }

        /// <summary>
        /// Gets the file content in string format
        /// </summary>
        /// <returns></returns>
        private string GetFileContent()
        {
            string fileContent = string.Empty;
            try
            {
                FileStream fileStream = new FileStream(selectedFile, FileMode.Open, FileAccess.Read);
                StreamReader reader = new StreamReader(fileStream);
                fileContent = reader.ReadToEnd();
                reader.Close();
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetFileContent()", ex);
            }
            return fileContent;
        }

        public void SaveData(string fileText)
        {
            string fileName = ReadoutCommon.GetFileName().Trim();
            bool Flag = false;
            do
            {
            AMT:
                if (ReadoutCommon.ReadoutMessageBox(ref fileName, DialogType.Common) == DialogResult.OK)
                {
                    if (string.IsNullOrEmpty(fileName))
                    {
                        this.StatusMessage = "File name cann't be empty.";
                        this.RightStatusMessage = "";
                        Application.DoEvents();
                        goto AMT;
                    }
                    if (ReadoutCommon.ValidFileName(fileName))
                        Flag = true;
                }
                else
                {
                    this.StatusMessage = string.Empty;
                    return;
                }
            } while (!Flag);
            if (fileName.Trim().Equals(string.Empty) || Flag == false)
            {
                this.StatusMessage = MessageConstant.GetText("M000047");
                return;
            }
            string filePath = string.Concat(ConfigInfo.CheckOrCreatePath(), "\\", fileName, ".CAB");
            filePath = filePath.Replace("\\\\", "\\");
            if (File.Exists(filePath))
            {
                DialogResult dr = CABMessageBox.ShowFilterMessage("M000046", "A000001", MessageBoxButtons.YesNo);
                if (dr.Equals(DialogResult.No))
                {
                    this.StatusMessage = string.Empty;
                    return;
                }
            }
            try
            {
                FileStream file = new FileStream(filePath, FileMode.Create);
                StreamWriter wr1 = new StreamWriter(file);
                fileText = ConfigInfo.EncryptFile(fileText);
                wr1.Write(fileText);
                wr1.Close();
                file.Close();
                //MessageBox.Show("File saved at " + filePath ,"BCS",MessageBoxButtons.OK,MessageBoxIcon.Information) ; //MessageConstant.GetText("M000048");
                //this.StatusMessage = "Please Enter the CMRI ID";
                //CMRIID cmriID = new CMRIID(false);
                //cmriID.OnValues_Submission += new CMRIID.GetSubmittedValues(cmriID_OnValuesSubmission);
                //cmriID.ShowDialog();
                //if (this.cmriID.Length == 0)
                //    return;
                bool IsUploaded = false;
                UploadFile uploadFile = new UploadFile();
                this.Cursor = Cursors.WaitCursor;
                this.StatusMessage = "Uploading Readout file";
                Application.DoEvents();
                string resultMessage = string.Empty;
                // uploadFile.cmriID = this.cmriID;
                ConfigSettings.ChangeNode("SourceOfFile", ((int)CommTypes.CMRI).ToString());
                IsUploaded = uploadFile.UploadCABFile(filePath, uploadFile.GetIECFileContent(filePath), true, out resultMessage, null);
                //uploadFile.DeleteFile();  
                if (IsUploaded)
                {
                    this.Cursor = Cursors.Default;
                    this.ListRefresh = true;
                    this.RightStatusMessage = String.Empty;
                    this.StatusMessage = "File Uploaded successfully.";
                    Application.DoEvents();
                }
                else
                {
                    this.Cursor = Cursors.Default;
                    this.RightStatusMessage = String.Empty;
                    this.StatusMessage = resultMessage;
                }
            }
            catch (Exception Ex)    //Exception log for catch block
            {
                MessageBox.Show(Ex.ToString());
                logger.Log(LOGLEVELS.Error, "SaveData(string fileText)", Ex);
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            this.Close();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            lblTOUFile.Text = GetScheduleFile(scheduleFile);
            selectedFile = scheduleFileLocation = lblTOUFile.Text.Trim();
        }

        private string GetScheduleFile(string filetype)
        {
            string selectedFile = string.Empty;
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Multiselect = false;
            if (filetype == "Scd")
            {
                openFile.DefaultExt = "Scd";
                openFile.Filter = "Schedule(*.Lcd)|*.Lcd";
            }
            else
            {
                openFile.DefaultExt = "TOU";
                openFile.Filter = "TouConfig(*.TOU)|*.TOU";
            }

            DialogResult result = openFile.ShowDialog();
            selectedFile = openFile.FileName;

            if (result == DialogResult.OK)
            {
                if (selectedFile.Length <= 0)
                {
                    this.StatusMessage = "File Is Corrupted!";
                    return "";
                }
            }

            return selectedFile;
        }

        private string CreateSendTouFileCmd(string get_Filename, string task_Id)
        {
            try
            {
                string fileContent = string.Empty;
                string touSendCommand = string.Empty;
                string touFile = string.Empty;
                touFileName = string.Empty;
                // if (!File.Exists(get_Filename)) return "";
                StringBuilder FileData = new StringBuilder();
                StringReader SR = new StringReader(get_Filename);
                //FileStream fs = new FileStream(get_Filename, FileMode.Open, FileAccess.Read);

                while (((fileContent = SR.ReadLine()) != null))
                {
                    //Added  this "&& !fileContent.Contains("<") " condition to make sure that all commands are prepared here except TOU .
                    if (fileContent.Length > 0 && !fileContent.Contains("<"))
                    {
                        if (task_Id == "TOU")
                        {
                            /*--------Verify File BCC--------------*/
                            if (fileContent.Length < 85) return "1";
                            /*--------END Of Verification----------*/

                            fileContent = StrToHex(fileContent);
                            touSendCommand += fileContent;// +"0D0A";
                        }
                        else
                        {
                            if (fileContent.IndexOf("(") >= 0)
                            {
                                if (fileContent.Length > 1)
                                {
                                    fileContent = fileContent.Substring(fileContent.IndexOf("(") + 1);
                                    fileContent = StrToHex(fileContent.Substring(0, fileContent.IndexOf(")")));
                                }
                            }
                            else
                            {
                                if (fileContent.IndexOf("TOUFilePath:") < 0) fileContent = StrToHex(fileContent);
                            }
                            // TouSend_Command += "01" + File_Comtent + "0D0A";
                            if (fileContent.IndexOf("TOUFilePath:") >= 0)
                            {
                                touFileName = fileContent.Substring(fileContent.IndexOf(":") + 1);
                            }
                            else if (fileContent.Length > 1) touSendCommand += fileContent + "0A";
                            fileContent = "";
                        }
                    }
                    fileContent += touSendCommand;
                }
                SR.Close();

                /*--------------Verify Schedule file Bcc---------------*/
                if (task_Id == "Schedule")
                {
                    string filedata = string.Empty;
                    SR = new StringReader(get_Filename);
                    //fs = new FileStream(get_Filename, FileMode.Open, FileAccess.Read);

                    while (((fileContent = SR.ReadLine()) != null))
                    {
                        if (fileContent.Length > 0)
                        {
                            filedata += fileContent;
                        }
                    }

                    //string CalBcc = filedata.Substring(filedata.Length - 1, 1);
                    //if (CalBcc != "\\")
                    //{
                    //    string File_cnt = StrToHex(filedata.Substring(0, filedata.Length - 1));
                    //    string Bccchar = CalBccFromFile(File_cnt);
                    //    if (Bccchar == "False") return "1";
                    //    char mchar = Convert.ToChar(Convert.ToInt32(Bccchar, 16));
                    //    if (CalBcc != mchar.ToString()) return "1";
                    //}
                }
                //touSendCommand += "30313604";

                int strPosition = 0;
                string[] strCmd = new string[1024];
                string ScheduleFileCommand = touSendCommand;
                while (ScheduleFileCommand.Length > 0)
                {
                    if (ScheduleFileCommand.Length >= 1024)
                    {
                        strCmd[strPosition] = ScheduleFileCommand.Substring(0, 1024);
                        ScheduleFileCommand = ScheduleFileCommand.Substring(1024);
                    }
                    else
                    {
                        strCmd[strPosition] = ScheduleFileCommand;
                        ScheduleFileCommand = "";
                    }
                    strPosition++;
                }

                //Code added on 10 Nov by Mohsin.
                if (strPosition < 10)
                    touSendCommand += "3030" + StrToHex(Convert.ToString(strPosition)) + "04";
                else
                    touSendCommand += "30" + StrToHex(Convert.ToString(strPosition)) + "04";
                /*-------------------End Of Verification---------------*/
                // Added for backward compatibility.
                //if (UtilityDetails.UtilityName == UtilityEntity.UGVCL || UtilityDetails.UtilityName == UtilityEntity.PVVNL || UtilityDetails.UtilityName == UtilityEntity.JDVVNL)
                //{
                if (task_Id == "TOU")
                {
                    fileContent = (((touSendCommand.Length / 256) + 1) / 16).ToString() + (((touSendCommand.Length / 256) + 1) % 16).ToString();
                }
                else
                {
                    fileContent = (((touSendCommand.Length / 1024) + 1) / 16).ToString() + (((touSendCommand.Length / 1024) + 1) % 16).ToString();
                }

                if (fileContent.Length <= 1)
                {
                    fileContent += "0" + Convert.ToInt32(fileContent).ToString();
                }
                if (task_Id == "TOU")
                {
                    touSendCommand += "30" + StrToHex(Convert.ToInt32(fileContent).ToString()) + "05";
                }
                else
                {
                    touSendCommand += StrToHex(fileContent) + "04";
                }
                //}

                return touSendCommand;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "CreateSendTouFileCmd(string get_Filename, string task_Id)", ex);
                return "";
                
            }
        }
        /// <summary>
        /// Prepares TOU command from TOU file .
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="taskID"></param>
        /// <returns></returns>
        private string TouFileCmd(string fileName, string taskID)
        {
            string fileContent = string.Empty;
            string touSendCommand = string.Empty;
            StringBuilder fileData = new StringBuilder();
            if (!File.Exists(fileName))
            {
                return string.Empty;
            }
            StreamReader streamReader = File.OpenText(fileName);
            FileStream fStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);

            while (((fileContent = streamReader.ReadLine()) != null))
            {
                if (fileContent.Length > 5)
                {

                    if (taskID == "TOU")
                    {
                        fileContent = StrToHex(fileContent);
                        touSendCommand += fileContent + "0D0A";
                        fileContent = "";
                    }
                    else
                    {
                        fileContent = fileContent.Substring(fileContent.IndexOf("(") + 1);
                        fileContent = StrToHex(fileContent.Substring(0, fileContent.IndexOf(")")));
                        touSendCommand += "01" + fileContent;// +"0D0A";
                        fileContent = "";
                    }
                }
            }
            fileContent = (((touSendCommand.Length / 1024) + 1) / 16).ToString() + (((touSendCommand.Length / 1024) + 1) % 16).ToString();
            if (fileContent.Length <= 1) fileContent += "0" + fileContent;
            touSendCommand += StrToHex(fileContent) + "04";
            streamReader.Close();
            return touSendCommand;
        }
        /// <summary>
        /// Use to Get Command for TOU only from lcd file .
        /// This Metod is clone of  "TouFileCmd()" method except that this method gets tou command from a lcd file.
        /// that has all other commands like RTC,Rsest also .
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="taskID"></param>
        /// <returns></returns>
        private string GetTOUCommandFromScheduleFile(string fileName, string taskID)
        {
            string fileContent = string.Empty;
            string touSendCommand = string.Empty;
            StringBuilder fileData = new StringBuilder();
            //if (!File.Exists(fileName))
            //{
            //    return string.Empty;
            //}
            StringReader streamReader = new StringReader(fileName);
            //FileStream fStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);

            while (((fileContent = streamReader.ReadLine()) != null))
            {
                if (fileContent.Length > 5 && fileContent.Contains("<"))
                {

                    if (taskID == "TOU")
                    {
                        fileContent = StrToHex(fileContent);
                        touSendCommand += fileContent + "0D0A";
                        fileContent = "";
                    }
                    else
                    {
                        fileContent = fileContent.Substring(fileContent.IndexOf("(") + 1);
                        fileContent = StrToHex(fileContent.Substring(0, fileContent.IndexOf(")")));
                        touSendCommand += "01" + fileContent;// +"0D0A";
                        fileContent = "";
                    }
                }
            }
            fileContent = (((touSendCommand.Length / 1024) + 1) / 16).ToString() + (((touSendCommand.Length / 1024) + 1) % 16).ToString();
            if (fileContent.Length <= 1) fileContent += "0" + fileContent;
            touSendCommand += StrToHex(fileContent) + "04";
            streamReader.Close();
            return touSendCommand;
        }

        /************************************************************************************
        *    Event Name    : StrToHex ()
        *    Description   : This Function get a string of AsciiCharactor and return a string 
        *                  : of Hex to the corresponding Ascii Charactor.
        *                     
        *************************************************************************************/
        private string StrToHex(string getStr)
        {
            int indexCount = 0;
            char aSCIIChar;
            int hexChar;
            string tempStr = "";
            char[] hexCharArray = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
            while (indexCount < getStr.Length)
            {
                aSCIIChar = Convert.ToChar(getStr.Substring(indexCount, 1));
                if ((aSCIIChar >= 48) && aSCIIChar <= 57)
                {
                    hexChar = (Convert.ToInt16(aSCIIChar) - 48) + 30;
                    tempStr += hexChar.ToString();
                }
                else
                {
                    if (aSCIIChar != 32)
                    {
                        hexChar = Convert.ToInt16(aSCIIChar);
                        aSCIIChar = hexCharArray[hexChar / 16];
                        tempStr += (hexCharArray[hexChar / 16]).ToString() + (hexCharArray[hexChar % 16]).ToString();
                    }
                    else
                    {
                        tempStr += "20";       //Space
                    }
                }
                indexCount++;
            }
            return tempStr;
        }

        public string CalBccFromFile(string RecInpData)
        {
            try
            {
                char[] _hexChars = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
                int Bcc = 0;
                int countbyt = 0;
                string TempStr = "";
                while (countbyt < RecInpData.Length)
                {

                    TempStr += System.Convert.ToChar(System.Convert.ToUInt32(RecInpData.Substring(countbyt, 2), 16)).ToString(); ;
                    countbyt += 2;
                }
                countbyt = 0;
                System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
                Byte[] bytes = encoding.GetBytes(TempStr);
                foreach (byte b in bytes)
                {
                    if (countbyt <= RecInpData.Length) Bcc = Bcc ^ b;
                    countbyt++;
                }
                string retval = _hexChars[Bcc / 16].ToString() + _hexChars[Bcc % 16].ToString();

                return retval;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "CalBccFromFile(string RecInpData)", ex);
                return "False";
            }
        }

        private void btnClearCMRI_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.StatusMessage = "Clearing CMRI...";
                Application.DoEvents();
                Result result = communication.CheckCMRICommunicationType((byte)CMRICommunicationType.DLMSClearCMRI,
                                                                            ConfigSettings.GetValue("PortName"),
                                                                            ConfigSettings.GetValue("BaudRate"));
                Thread.Sleep(4000);
                Application.DoEvents();
                ClearCMRIForDLMSOnly();
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "btnClearCMRI_Click(object sender, EventArgs e)", ex);
            }
            finally
            {
                SetConnectionDetail(false);
                StopProgressBarTimer();
                this.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// Used For Clear CMRI for those utilities which support DLMS meters only.
        /// </summary>
        private void ClearCMRIForDLMSOnly()
        {
            CommunicationErrorType resultStatus = ClearCMRILT();
            if (resultStatus != CommunicationErrorType.Success)
            {
                if (resultStatus == CommunicationErrorType.InvalidServerAddress)
                {
                    this.StatusMessage = "Access Denied.Please Change the Mode!";
                }
                else
                {
                    this.StatusMessage = CommonBLL.GetEnumDescription(resultStatus);
                    Application.DoEvents();
                }
            }
            else
            {
                this.StatusMessage = "CMRI Cleared";
            }

        }

        /// <summary>
        /// Used For Clear CMRI for those utilities which support DLMS as well IEC meters .
        /// </summary>
        private void ClearCMRIForDLMSAndIEC()
        {
            Thread.Sleep(2000);
            prepareCMRI.Channel = communications;
            string result = prepareCMRI.ClearCMRIData();
            if (result != string.Empty)
            {
                if (result == "Sign-On Failure!")
                {
                    ClearCMRIForDLMSOnly();
                }
                else if (result == MessageConstant.GetText("M000038"))
                {
                    this.StatusMessage = MessageConstant.GetText("M000038") + " Please Restart BCS.";
                }

            }
            else
            {
                this.StatusMessage = "CMRI Cleared";
            }
        }


        /// <summary>
        /// Clear CMRI for LT meters 
        /// </summary>
        /// <returns></returns>
        private CommunicationErrorType ClearCMRILT()
        {
            Result result = new Result();
            bool isConnected = false;
            try
            {
                this.Cursor = Cursors.WaitCursor;
                //This is an exception as we have RTC tag in xml file in instant profile so we can't put it one more time.
                ProfileCommand clearCMRICommand = new ProfileCommand(01, "00.00.60.01.C8.FF ", 02);
                clearCMRICommand.ClassName = "CAB.E650MeterConfiguration.BillingReset,E650MeterConfiguration";
                clearCMRICommand.Action = ActionType.WRITE;
                string baudRate = ConfigSettings.GetValue("BaudRate");

                if (baudRate.Contains("38400"))
                    result = communication.OpenSessionCMRI(7);
                else
                    result = communication.OpenSession();

                //result = communication.OpenSession();
                if (result.ErrorCode == CommunicationErrorType.Success)
                {
                    isConnected = true;
                    SetConnectionDetail(true);
                    result = communication.Send(clearCMRICommand);
                    if (result.ErrorCode == CommunicationErrorType.Success)
                    {
                        this.StatusMessage = "CMRI Cleared Successfully!";
                    }
                }
                if (result.ErrorCode != CommunicationErrorType.Success)
                {
                    this.StatusMessage = CommonBLL.GetEnumDescription(result.ErrorCode);
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ClearCMRILT()", ex);
            }
            finally
            {
                if (isConnected)
                {
                    communication.CloseSession();
                }
                this.Cursor = Cursors.Default;
            }
            return result.ErrorCode;
        }

     
        /// <param name="progressBar"></param>
        /// <param name="statusLabel"></param>
        public void StartProgressBarTimer()
        {
            statusStrip.Visible = true;
            progressBarTimer.Enabled = true;
        }

        /// <param name="progressBar"></param>
        /// <param name="statusLabel"></param>
        public void StopProgressBarTimer()
        {
            statusStrip.Visible = false;
            progressBarTimer.Enabled = false;
        }

        private void CMRICommunication_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.StatusMessage = "";
            this.Cursor = Cursors.Default;
        }
        /// <summary>
        /// progress bar timer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void progressBarTimer_Tick(object sender, EventArgs e)
        {
            if (toolStripProgressBar.Value > toolStripProgressBar.Maximum - 1)
            {
                toolStripProgressBar.Value = 0;
            }
            else
            {
                toolStripProgressBar.Value = toolStripProgressBar.Value + 10;
            }
        }

        private void btnpreparesmarthhu_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] b1;
                Int32 portno = Int32.Parse(ConfigSettings.GetValue("HHUPortNumber"));
                //string fileName = Application.StartupPath + @"\SecurityFiles.zip";//@"\EndDeviceSecurityKey.xml";
                string fileName = AppDomain.CurrentDomain.BaseDirectory + @"meterlist.txt";//@"\EndDeviceSecurityKey.xml";
                if (!File.Exists(fileName))
                {
                    MessageBox.Show("File not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                this.StatusMessage = "Connecting....";
                Application.DoEvents();
                TcpClient client = new TcpClient(ConfigSettings.GetValue("HHUServer"), portno);
                Stream s = client.GetStream();
                this.StatusMessage = "Sending File....";
                b1 = File.ReadAllBytes(fileName);
                s.Write(b1, 0, b1.Length);
                client.Close();
                this.StatusMessage = "File Transferred.";
            }
            catch (Exception ex)    //Exception log for catch block
            {

                MessageBox.Show("Unable To Connect to Host" + "\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.StatusMessage = " ";
                logger.Log(LOGLEVELS.Error, "btnpreparesmarthhu_Click(object sender, EventArgs e)", ex);
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
