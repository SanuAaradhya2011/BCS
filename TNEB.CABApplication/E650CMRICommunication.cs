#region Namespaces
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using System.Linq;

using CAB.IECFramework.Utility;
using CAB.Parser;
using CAB.Serialization;
using CAB.UI.Controls;
using CABCommunication.Common;
using CABCommunication.WrapperLayer;
using CAB.UI;
using CAB.IECFramework;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Globalization;
using CAB.IECChannel.ReadOut;
using CAB.BLL;
#endregion


namespace CABApplication
{

    /// <summary>
    /// This class provides UI to user for communicating with CMRI
    /// It supports CMRI readout  as well Prepare CMRI operations .
    /// </summary>
    public partial class E650CMRICommunication : MdiChildForm
    {

        #region Nested Types
        #endregion

        #region Constants and Variables   
        private Communication communication = null;
        private Dictionary<string, int> meterLoadList = null;
        private string selectedFile = string.Empty;
        private static Serializer serializer = null;
        private static DLMS commandRepository = null;
        private List<string> dumpFiles = new List<string>();
        private string cmriID = String.Empty;
        private const string Splitter = "$";
        private const string FileName = "IntermediateFile";
        #endregion

        #region Properties
        #endregion

        #region Constructor
        public E650CMRICommunication()
        {
            InitializeComponent();
            //communication = new Communication();
            communication = new Communication(ConfigSettings.GetValue("PortName"),
                                             Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism")),
                                             ConfigSettings.GetValue("ModePassword"));
        
        }
        static E650CMRICommunication()
        {
            serializer = new Serializer();
            if (commandRepository == null)
            {
                commandRepository = (DLMS)serializer.DeserializeToObject(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "CommandRepository.xml"), typeof(DLMS));
            }
        }
        #endregion        

        #region Public Methods
        #endregion

        #region Protected Methods
        #endregion

        #region Event Handlers


        /// <summary>
        /// Close CMRI window 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancelCMRI_Click(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            this.Close();
        }    
        /// <summary>
        /// Dump and read CMRi files 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReadData_Click(object sender, EventArgs e)
        {
           
            //Read list of files .
            Result result = communication.OpenSession();
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
                    //lstCMRIfile.Items.Clear();                
                    DisplayMeterList(result.RecieveDataBuffer);
                    communication.CloseSession();
                    #endregion


                    //Read each file data           

                    string message = string.Empty;
                    string meterID = string.Empty;
                    string readingDateTime = string.Empty;
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
                        //Iterate over the listed items in load meter list list box
                        foreach (string file in dumpFiles)
                        {
                            this.Cursor = Cursors.WaitCursor;
                            // String fileName = string.Empty;
                            int index = 0;
                            //get the corresponding SAP value in dictionary
                            this.StatusMessage = "Reading : " + file;
                            index = dumpFiles.IndexOf(file);
                            //Try to open session i.e open the port and connect to CMRI with given SAP
                            communication.SetSAP((byte)(index + 2));
                            result = communication.OpenSession();
                            if (result.ErrorCode == CommunicationErrorType.Success)
                            {
                                //fileName = GetFileName(file);
                                readingDateTime = GetReadingDateTime(file);

                                if (!string.IsNullOrEmpty(fileName))
                                {
                                    writeToFile.WriteLine(Splitter);
                                    writeToFile.WriteLine(readingDateTime);
                                    #region Read selected profiles
                                    foreach (ProfileId selectedProfile in selectedProfiles)
                                    {
                                        this.StatusMessage = selectedProfile.ToString() + " read in progress..";
                                        Application.DoEvents();
                                        List<ProfileCommand> profileReadCommands = lstProfileCommands.FindAll(delegate(ProfileCommand profileCommandEntity)
                                        {
                                            return profileCommandEntity.TagNumber == (int)selectedProfile
                                            && (!profileCommandEntity.ObisCode.ToUpper().Contains("METERID"))
                                                && (profileCommandEntity.MeterModelNumber == meterModelNumber ||
                                            profileCommandEntity.MeterModelNumber == 0);
                                        });
                                        for (index = 0; index < profileReadCommands.Count; index++)
                                        {
                                            if (result.ErrorCode == CommunicationErrorType.Success)
                                            {
                                                profileReadCommands[index].Action = ActionType.READ;
                                                result = communication.Send(profileReadCommands[index]);
                                                if (result.ErrorCode == CommunicationErrorType.Success)
                                                {
                                                    classIdOBISCodeAttribute = String.Format("{0:X2}", profileReadCommands[index].ClassId) + profileReadCommands[index].ObisCode.Replace(".", "")
                                                                               + String.Format("{0:X2}", profileReadCommands[index].Attribute);
                                                    writeToFile.Write(classIdOBISCodeAttribute);
                                                    for (int i = 0; i < result.RecieveDataLength; i++)
                                                    {
                                                        writeToFile.Write(String.Format("{0:X2}", result.RecieveDataBuffer[i]));
                                                    }
                                                    writeToFile.WriteLine("");
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

                                    #region ResourceCleanup
                                    if (result.ErrorCode == CommunicationErrorType.Success)
                                    {
                                        communication.CloseSession();
                                        ////writeToFile.Close();
                                        ////initialFileStream.Close();
                                        // String strChecksum = GetMD5ChecksumForFile(fileName);
                                        //FileStream fileStream = new FileStream(fileName, FileMode.Append);
                                        //StreamWriter writeStream = new StreamWriter(fileStream);
                                        //writeStream.WriteLine(strChecksum);
                                        //writeStream.Close();
                                        //fileStream.Close();
                                        //this.StatusMessage = "Read out completed";
                                        //Application.DoEvents();
                                        //SaveData(fileName);
                                    }
                                    #endregion
                                    else
                                    {
                                        MessageBox.Show(CommonBLL.GetEnumDescription(result.ErrorCode), "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }

                            }

                        }
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
                            this.StatusMessage = "Read out completed";
                            Application.DoEvents();
                            SaveData(fileName);
                        }
                    }
                    catch
                    {
                        communication.CloseSession();
                    }
                    finally
                    {
                        if (File.Exists(fileName))
                        {
                            File.Delete(fileName);
                        }
                        communication.CloseSession();
                        this.Cursor = Cursors.Default;
                        if (initialFileStream != null)
                        {
                            initialFileStream.Close();
                        }
                        if (writeToFile != null)
                        {
                            writeToFile.Close();
                        }
                    }
                }
                else
                {
                    communication.CloseSession();
                    MessageBox.Show("Failure in Reading CMRI.", "BCS");
                }
            }
            else if (result.ErrorCode == CommunicationErrorType.InvalidServerAddress)
            {
                MessageBox.Show("Access Denied.Please Change Mode !", "BCS",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            else if (result.ErrorCode != CommunicationErrorType.Success)
            {
                MessageBox.Show(CommonBLL.GetEnumDescription(result.ErrorCode), "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }        

        private void E650CMRICommunication_Load(object sender, EventArgs e)
        {
           
        }

        private void btnCMRICancel_Click(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            this.Close();
        }
       
       
        /// <summary>
        /// Reads the CMRI 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
       
        private void btnPrepareCMRI_Click(object sender, EventArgs e)
        {
            bool isConnected = false;
            try
            {
                this.StatusMessage = string.Empty;
                if (!string.IsNullOrEmpty(selectedFile))
                {
                    this.Cursor = Cursors.WaitCursor;
                    string fileContent = GetFileContent();
                    Result result = null;
                    if (fileContent.Contains("\n"))
                    {
                        string[] fileText = fileContent.Split('\n');
                        if (fileText[0].Trim() == "DLMS")
                        {
                            result = UpdateRTC();
                            if (result.ErrorCode == CommunicationErrorType.Success)
                            {
                                isConnected = true;
                                result = communication.OpenSession();
                                if (result.ErrorCode == CommunicationErrorType.Success)
                                {
                                    this.StatusMessage = "Preparing CMRI..";
                                    Application.DoEvents();
                                    for (int counter = 1; counter < fileText.Length - 2; counter++)
                                    {
                                        this.Cursor = Cursors.WaitCursor;
                                        DLMSCOMMAND command = GetCommandFromCommandRepository(fileText[counter].Substring(0, 16));
                                        string data = fileText[counter].Substring(16);
                                        ProfileCommand profileCommand = new ProfileCommand(Convert.ToByte(command.CLASS),
                                            command.OBISCODE, Convert.ToByte(command.ATTRIBUTE));
                                        if (command.OBISCODE == "00.00.01.00.00.FF")
                                            continue;
                                        if (command.OBISCODE == "00.01.0A.09.00.FF")
                                        {
                                            profileCommand.Action = ActionType.RESET;
                                        }
                                        else
                                        {
                                            profileCommand.WriteDataBuffer = SoapHexBinary.Parse(data).Value.ToList<byte>();
                                            profileCommand.Action = ActionType.WRITEBUFFER;

                                        }
                                        this.StatusMessage = profileCommand.ClassName;
                                        Application.DoEvents();
                                        result = communication.Send(profileCommand);
                                        if (result.ErrorCode != CommunicationErrorType.Success)
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                            if (result.ErrorCode == CommunicationErrorType.Success)
                            {
                                MessageBox.Show("CMRI Prepared successfully!", "BCS");
                                this.StatusMessage = "CMRI Prepared successfully.";
                            }
                            else
                            {
                                this.StatusMessage = "CMRI Preparation failed.";
                                MessageBox.Show(CommonBLL.GetEnumDescription(result.ErrorCode), "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            if (isConnected)
                            {
                                result = communication.CloseSession();
                            }

                        }
                        else
                        {
                            MessageBox.Show("BCS", "cfg file format not supported.", MessageBoxButtons.OK);
                        }

                    }
                }
                else
                {
                    CABMessageBox.ShowFilterMessage("M000088", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("File corrupted");
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            selectedFile = string.Empty;
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Multiselect = false;
            openFile.DefaultExt = "lcd";
            openFile.Filter = "Config(*.lcd)|*.lcd";
            DialogResult result = openFile.ShowDialog();
            if (result == DialogResult.OK)
            {
                selectedFile = openFile.FileName;
            }

        }
        /// <summary>
        /// Update CMRI RTC
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdateRTC_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                Result result = UpdateRTC();
                if (result.ErrorCode == CommunicationErrorType.Success)
                {
                    MessageBox.Show("RTC Updated Successfully !", "BCS");

                }
                else if (result.ErrorCode == CommunicationErrorType.InvalidServerAddress)
                {
                    MessageBox.Show("Access Denied.Please Change Mode !", "BCS");
                }
                else
                {
                    MessageBox.Show(CommonBLL.GetEnumDescription(result.ErrorCode), "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch
            {
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void btnClearCMRI_Click(object sender, EventArgs e)
        {
            Result result = null;
            bool isConnected = false;
            try
            {
                this.Cursor = Cursors.WaitCursor;
                //This is an exception as we have RTC tag in xml file in instant profile so we can't put it one more time.
                ProfileCommand clearCMRICommand = new ProfileCommand(01,"00.00.60.01.C8.FF ",02);
                clearCMRICommand.ClassName = "CAB.E650MeterConfiguration.BillingReset,E650MeterConfiguration";
                clearCMRICommand.Action = ActionType.WRITE;                
                result = communication.OpenSession();
                if (result.ErrorCode == CommunicationErrorType.Success)
                {
                    isConnected = true;
                    result = communication.Send(clearCMRICommand);
                    if (result.ErrorCode == CommunicationErrorType.Success)
                    {
                        MessageBox.Show("CMRI Cleared Successfully!","BCS");
                    }
                }
                if (result.ErrorCode != CommunicationErrorType.Success)
                {
                    MessageBox.Show(CommonBLL.GetEnumDescription(result.ErrorCode), "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error); 
                }
            }
            catch
            {

            }
            finally
            {               
                if (isConnected)
                {
                    communication.CloseSession();
                }
                this.Cursor = Cursors.Default;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmriID"></param>
        private void cmriID_OnValuesSubmission(string cmriID)
        {
            this.cmriID = cmriID;
        }

        private void E650CMRICommunication_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.StatusMessageAsync = "";
        }

        #endregion 

        #region Private Methods

        private Result UpdateRTC()
        {
            Result result =null;
            bool isConnected = false;
            try
            {
                //This is an exception as we have RTC tag in xml file in instant profile so we can't put it one more time.
                ProfileCommand rtcCommand = new ProfileCommand(8, "00.00.01.00.00.FF", 2);
                rtcCommand.ClassName = "CAB.E650MeterConfiguration.RTC,E650MeterConfiguration";
                rtcCommand.Action = ActionType.WRITE;
                rtcCommand.WriteDataBuffer = DateTime.Now;
                result = communication.OpenSession();
                if (result.ErrorCode == CommunicationErrorType.Success)
                {
                    isConnected = true;
                    result = communication.Send(rtcCommand);
                }               

            }
            catch
            {

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
                if (receiveBuffer[nByteIndex] != 253)
                {
                    data = data + " " + date.ToString("d2") + "/" + month.ToString("d2") + "/" + year.ToString("d2") + " " + hour.ToString("d2") + ":" + minute.ToString("d2") + ":" + second.ToString("d2");
                    //lstCMRIfile.Items.Add(data, true);
                    dumpFiles.Add(data);
                  //  btnRead.Enabled = true;
                }
                nByteIndex += 3;
            }
        }
        /// <summary>
        /// Uploads the file in database
        /// </summary>
        /// <param name="strFileName"></param>
        private bool UploadFile(string strFileName)
        {
            bool IsUploaded = false;
            try
            {
                UploadFile uploadFile = new UploadFile();
                this.StatusMessage = "Uploading readout file..";
                Application.DoEvents();
                uploadFile.cmriID = "CMRI";
                //IsUploaded = uploadFile.Upload2NGFile(uploadFile.Get2NGFileContent(strFileName), strFileName);
                uploadFile.DeleteFile();
                if (IsUploaded)
                {
                    this.ListRefresh = true;
                    this.RightStatusMessage = String.Empty;
                    this.StatusMessage = "File Uploaded successfully.";
                    Application.DoEvents();
                }
                else
                {
                    this.RightStatusMessage = String.Empty;
                    this.StatusMessage = uploadFile.StatusMessage;
                    Application.DoEvents();
                }
            }
            catch (Exception ex)
            {
                IsUploaded = false;
            }
            return IsUploaded;
        }

        /// <summary>
        /// Save File Text
        /// </summary>
        /// <param name="fileText"></param>
        public void SaveData(string inputFile)
        {
            StreamReader streamReader = new StreamReader(inputFile);
            string fileText = streamReader.ReadToEnd();
            streamReader.Close();
            File.Delete(inputFile);

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
            string filePath = string.Concat(ConfigInfo.CheckOrCreatePath(), "\\", fileName, ".2NG");
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
                //fileText = ConfigInfo.EncryptFile(fileText);
                wr1.Write(fileText);
                wr1.Close();
                file.Close();
                MessageBox.Show("File saved at " + filePath, "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information); //MessageConstant.GetText("M000048");
                this.StatusMessage = "Please Enter the CMRI ID";
                CMRIID cmriID = new CMRIID(false);
                cmriID.OnValues_Submission += new CMRIID.GetSubmittedValues(cmriID_OnValuesSubmission);
                cmriID.ShowDialog();
                if (this.cmriID.Length == 0)
                    return;
                bool IsUploaded = false;
                UploadFile uploadFile = new UploadFile();
                this.Cursor = Cursors.WaitCursor;
                this.StatusMessage = "Uploading Readout file";
                uploadFile.cmriID = this.cmriID;                               
                IsUploaded = uploadFile.Upload2NGFile(filePath);                    
                uploadFile.DeleteFile();
                if (IsUploaded)
                {
                    this.Cursor = Cursors.Default;
                    this.ListRefresh = true;
                    this.RightStatusMessage = String.Empty;
                    this.StatusMessage = "File Uploaded successfully.";
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }

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
            catch
            { 
            
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="meterID"></param>
        /// <returns></returns>
        private string GetFileName(string message)
        {
            string fileName = string.Empty;
            int meterIDIndex = message.Length - 20;
            if (meterIDIndex >= 7 || meterIDIndex <= 16)
            {
                fileName = AppDomain.CurrentDomain.BaseDirectory + @"DLMSCommunication\" + message.Substring(0, meterIDIndex) + "_" + DateTime.Now.Day.ToString("00") + "_"
                    + DateTime.Now.Month.ToString("00") + "_" + DateTime.Now.Year.ToString("0000") + "_" +
                    DateTime.Now.Hour.ToString("00") + "_" + DateTime.Now.Minute.ToString("00") + "_" +
                    DateTime.Now.Second.ToString("00") + "_" + DateTime.Now.Millisecond.ToString("00") + ".2NG";
            }
            return fileName;
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
        /// Byte buffer of date time
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public  List<byte> GetDateTimeBuffer(DateTime inputData)
        {
            List<byte> buffer = new List<byte>();
            try
            {               
                buffer.Add(Convert.ToByte((inputData.Year & 0xFF00) >> 8));
                buffer.Add(Convert.ToByte(inputData.Year & 0x00FF));
                buffer.Add(Convert.ToByte(inputData.Month));
                buffer.Add(Convert.ToByte(inputData.Day));                
                buffer.Add(Convert.ToByte(inputData.Hour));
                buffer.Add(Convert.ToByte(inputData.Minute));
                buffer.Add(Convert.ToByte(inputData.Second));                

            }
            catch (Exception)
            {

            }
            return buffer;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="meterIDIndex"></param>
        /// <param name="meterID"></param>
        /// <returns></returns>
        private string GetMeterIDAndRTC(string message, int meterIDIndex, string meterID)
        {
            int tyear = Convert.ToInt16(message.Substring(meterIDIndex + 7, 4));
            int tmonth = Convert.ToInt16(message.Substring(meterIDIndex + 4, 2));
            int tdate = Convert.ToInt16(message.Substring(meterIDIndex + 1, 2));
            int thour = Convert.ToInt16(message.Substring(meterIDIndex + 12, 2));
            int tmin = Convert.ToInt16(message.Substring(meterIDIndex + 15, 2));
            int tsec = Convert.ToInt16(message.Substring(meterIDIndex + 18, 2));
            DateTime meterReadingDate = new DateTime(tyear, tmonth, tdate, thour, tmin, tsec);       
            return meterIDIndex.ToString("00") + meterID + meterReadingDate.Year.ToString("0000") + meterReadingDate.Month.ToString("00") +
                meterReadingDate.Day.ToString("00") + meterReadingDate.Hour.ToString("00") + 
                meterReadingDate.Minute.ToString("00")+ meterReadingDate.Second.ToString("00");
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
        ///  Used to create profileId enums based on profiles
        /// that needs to be read(Selected by user through checkboxes)
        /// </summary>
        /// <returns></returns>
        private List<ProfileId> GetSelectedProfilesToReadCMRI()
        {
            List<ProfileId> selectedProfiles = new List<ProfileId>();
            selectedProfiles.Clear();           
            selectedProfiles.Add(ProfileId.NamePlate);
            selectedProfiles.Add(ProfileId.Phasor);
            selectedProfiles.Add(ProfileId.Instant);
            selectedProfiles.Add(ProfileId.Billing);
            selectedProfiles.Add(ProfileId.FraudEnergy);

            selectedProfiles.Add(ProfileId.BillingType);
            selectedProfiles.Add(ProfileId.KvahSelection);
            selectedProfiles.Add(ProfileId.DIP);
            selectedProfiles.Add(ProfileId.ResetLockOutDays);
            selectedProfiles.Add(ProfileId.LoadSurvey);
            selectedProfiles.Add(ProfileId.Tamper);
            selectedProfiles.Add(ProfileId.Midnight);
            selectedProfiles.Add(ProfileId.RS232LockUnlock);
            selectedProfiles.Add(ProfileId.PushDisplayParameter);
            selectedProfiles.Add(ProfileId.ScrollDisplyParameter);
            selectedProfiles.Add(ProfileId.HighResolutionDisplayParameter);
            selectedProfiles.Add(ProfileId.DisplayTimeoutParameter);
            //TOU
            selectedProfiles.Add(ProfileId.PassiveSeasonProfile);
            selectedProfiles.Add(ProfileId.PassiveWeekProfile);
            selectedProfiles.Add(ProfileId.PassiveDayProfile);
            selectedProfiles.Add(ProfileId.ActiveSeasonProfile);
            selectedProfiles.Add(ProfileId.ActiveWeekProfile);
            selectedProfiles.Add(ProfileId.ActiveDayProfile);
            selectedProfiles.Add(ProfileId.ActivationDate);
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
            catch
            {

            }
            return fileContent;
        }
        #endregion

        
            
    }
}


