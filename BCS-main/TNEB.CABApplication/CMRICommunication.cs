using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CAB.UI.Controls;
using CAB.IECChannel.ReadOut;
using System.IO;
using CAB.IECChannel.CMRI;
using CAB.IECChannel;
using CAB.IECFramework;
using CAB.IECFramework.Utility;
using LTCTBLL;
using CAB.Entity;
namespace CAB.UI
{
    public partial class CMRICommunication : MdiChildForm
    {
        private CMRIReadout cMRIReadout;
        private LocalCommunication communications;
        private Command command;
        private PrepareCMRI prepareCMRI;
        private string scheduleFileLocation = string.Empty;
        private string touFileName = string.Empty;
        private string scheduleFile = "Scd";
        private string sCDFileCommand = string.Empty;
        private string tOUFileCommand = string.Empty;
        private string fileExtension = string.Empty;
        private string cmriID = String.Empty;
        public CMRICommunication()
        {
            InitializeComponent();
            command = Command.GetInstance();
            cMRIReadout = new CMRIReadout();
            cMRIReadout.OnChannelStatusChanged += new CMRIReadout.ChannelStatusChanged(Channel_OnStatusChanged);
            communications = new LocalCommunication();
            prepareCMRI = new PrepareCMRI();

        }

        private void cmriID_OnValuesSubmission(string cmriID)
        {
            this.cmriID = cmriID;
        }

        string textRead = string.Empty;
        private void btnReadData_Click(object sender, EventArgs e)
        {
            this.StatusMessage = "CMRI Reading.";
            Application.DoEvents();
            this.Cursor = Cursors.WaitCursor;
            cMRIReadout.Channel = communications;
            textRead = cMRIReadout.GetData();
            if (textRead.Trim() != string.Empty)
            {
                if (ReadoutCommon.CalculateFileBcc(textRead) != string.Empty)
                {
                    this.StatusMessage = "CMRI readout successful";
                    Application.DoEvents();
                    SaveData(textRead);
                }
                else
                {
                    this.StatusMessage = "BCC not Matched";
                    Application.DoEvents();
                }
            }
            else
            {
                this.StatusMessage = "TimeOut!";
                Application.DoEvents();
            }
            this.Cursor = Cursors.Default;
        }
        private void Channel_OnStatusChanged(string msg)
        {
            this.StatusMessage = msg;
        }
        private void btnUpdateRTC_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            this.StatusMessage = "RTC Updating...";
            Application.DoEvents();
            cMRIReadout.Channel = communications;
            if (cMRIReadout.UpdateCMRIRTC())
            {
                this.StatusMessage = "RTC Updated";
                Application.DoEvents();
            }
            else
            {
                this.StatusMessage = "TimeOut!";
                Application.DoEvents();
            }
            this.Cursor = Cursors.Default;
        }

        private void btnPrepareCMRI_Click(object sender, EventArgs e)
        {
            if (scheduleFileLocation == string.Empty)
            {
                CABMessageBox.ShowFilterMessage("M000088", "A000001", MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
            if (fileExtension.ToLower().Contains(".lcd"))
            {
                CABMessageBox.ShowFilterMessage("M000088", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            prepareCMRI.Channel = communications;
               sCDFileCommand = CreateSendTouFileCmd(scheduleFileLocation, "Schedule");
            if (sCDFileCommand.Length <= 0)
            {
                CABMessageBox.ShowFilterMessage("M000099", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (sCDFileCommand == "1")
            {
                CABMessageBox.ShowFilterMessage("M000087", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //Assigning the Schedule file for preparing the CMRI
            prepareCMRI.ScheduleFileCommand = sCDFileCommand;

            if (this.touFileName != string.Empty)
            {
                prepareCMRI.TOUFileName = this.touFileName;
                tOUFileCommand = TouFileCmd(this.touFileName, "TOU");
                if (tOUFileCommand != string.Empty)
                {
                    prepareCMRI.TOUFileCommand = tOUFileCommand;
                }
                else
                {
                    this.StatusMessage = "File Not available at the Configured Location";
                    return;
                }
            }
            else
            {
                prepareCMRI.TOUFileName = string.Empty;
            }

            this.Cursor = Cursors.WaitCursor;
            this.StatusMessage = "Preparing CMRI...";
            Application.DoEvents();
            if (prepareCMRI.PrepareCMRIData())
            {
                this.StatusMessage = "CMRI Prepared";
                Application.DoEvents();
            }
            else
            {
                this.StatusMessage = "Time Out!";
                Application.DoEvents();
            }
            scheduleFileLocation = string.Empty;
            this.Cursor = Cursors.Default;
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
                MessageBox.Show("File saved at " + filePath ,"BCS",MessageBoxButtons.OK,MessageBoxIcon.Information) ; //MessageConstant.GetText("M000048");
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
                IsUploaded = uploadFile.Upload(filePath, uploadFile.GetContent(filePath), true);
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            this.Close();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            lblTOUFile.Text = GetScheduleFile(scheduleFile);
            scheduleFileLocation = lblTOUFile.Text.Trim();
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
                if (!File.Exists(get_Filename)) return "";
                StringBuilder FileData = new StringBuilder();
                StreamReader SR = File.OpenText(get_Filename);
                FileStream fs = new FileStream(get_Filename, FileMode.Open, FileAccess.Read);

                while (((fileContent = SR.ReadLine()) != null))
                {
                    if (fileContent.Length > 0)
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
                    SR = File.OpenText(get_Filename);
                    fs = new FileStream(get_Filename, FileMode.Open, FileAccess.Read);

                    while (((fileContent = SR.ReadLine()) != null))
                    {
                        if (fileContent.Length > 0)
                        {
                            filedata += fileContent;
                        }
                    }

                    string CalBcc = filedata.Substring(filedata.Length - 1, 1);
                    if (CalBcc != "\\")
                    {
                        string File_cnt = StrToHex(filedata.Substring(0, filedata.Length - 1));
                        string Bccchar = CalBccFromFile(File_cnt);
                        if (Bccchar == "False") return "1";
                        char mchar = Convert.ToChar(Convert.ToInt32(Bccchar, 16));
                        if (CalBcc != mchar.ToString()) return "1";
                    }
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
                if (UtilityDetails.UtilityName == UtilityEntity.UGVCL || UtilityDetails.UtilityName == UtilityEntity.PVVNL || UtilityDetails.UtilityName == UtilityEntity.JDVVNL)
                {
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
                }

                return touSendCommand;
            }
            catch (Exception)
            {
                return "";
            }
        }

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
            catch (Exception)
            {

                return "False";
            }
        }

        private void btnClearCMRI_Click(object sender, EventArgs e)
        {
            this.StatusMessage = "Clearing CMRI...";
            Application.DoEvents();
            prepareCMRI.Channel = communications;
            if (prepareCMRI.ClearCMRIData())
            {
                this.StatusMessage = "CMRI Cleared";
            }
            else
            {
                this.StatusMessage = "Time Out";
            }
        }

        

        private void CMRICommunication_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.StatusMessage = "";
            this.Cursor = Cursors.Default;
        }

       
    }
}
