using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using CAB.UI.Controls;
using CAB.BLL;
using CAB.Framework;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Security.Cryptography;
using Hunt.EPIC.Logging;
using CAB.IECFramework.Utility;
using Shell32;
using System.Threading;
using LandisGyr.GSIS.CIM2ndEd.Service;
using LandisGyr.GSIS.CIM2ndEd;


namespace CAB.UI
{
    public partial class CMRIScheduleFile : MdiChildForm
    {
        public CMRIScheduleFile()
        {
            InitializeComponent();
            SecurityFileFlag = false;
        }

        MeterDataBLL meterDataBLL = new MeterDataBLL();
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(CMRIScheduleFile).ToString());
        public bool SecurityFileFlag
        {
            get;
            set;
        }

        Dictionary<string, string> dicMeterLPR = null;
        private void btnPushMove_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBoxAvailableMeters.SelectedItems.Count == 0) { return; }
                foreach (object item in listBoxAvailableMeters.SelectedItems)
                {
                    listBoxSelectedMeters.Items.Add(item);
                }
                foreach (object item in listBoxSelectedMeters.Items)
                {
                    listBoxAvailableMeters.Items.Remove(item);
                }
                if (listBoxAvailableMeters.Items.Count >= 1)
                { listBoxAvailableMeters.SelectedIndex = 0; }
                listBoxAvailableMeters.Focus();
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "btnPushMove_Click(object sender, EventArgs e)", ex);
            }
        }

        private void btnPushMoveAll_Click(object sender, EventArgs e)
        {
            try
            {
                int count = 0;
                while (count < listBoxAvailableMeters.Items.Count)
                {
                    listBoxSelectedMeters.Items.Add(listBoxAvailableMeters.Items[count]);
                    count++;
                }
                listBoxAvailableMeters.Items.Clear();
                if (listBoxSelectedMeters.Items.Count > 0) listBoxSelectedMeters.SelectedIndex = 0;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "btnPushMoveAll_Click(object sender, EventArgs e)", ex);
            }
        }

        private void btnPushRemove_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBoxSelectedMeters.SelectedItems.Count == 0) { return; }
                foreach (object item in listBoxSelectedMeters.SelectedItems)
                {
                    listBoxAvailableMeters.Items.Add(item);
                }
                foreach (object item in listBoxAvailableMeters.Items)
                {
                    listBoxSelectedMeters.Items.Remove(item);
                    if (listBoxAvailableMeters.Items.Count == 0) { return; }
                }
                if (listBoxSelectedMeters.Items.Count >= 1)
                { listBoxSelectedMeters.SelectedIndex = 0; }
                listBoxSelectedMeters.Focus();
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "btnPushRemove_Click(object sender, EventArgs e)", ex);
            }
        }

        private void btnPushRemoveAll_Click(object sender, EventArgs e)
        {
            try
            {
                int count = 0;
                while (count < listBoxSelectedMeters.Items.Count)
                {
                    listBoxAvailableMeters.Items.Add(listBoxSelectedMeters.Items[count]);
                    count++;
                }
                listBoxSelectedMeters.Items.Clear();
                if (listBoxAvailableMeters.Items.Count > 0) listBoxAvailableMeters.SelectedIndex = 0;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "btnPushRemoveAll_Click(object sender, EventArgs e)", ex);
            }
        }

        private string GetScheduleFile()
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Multiselect = false;

            openFile.DefaultExt = "cfg";
            openFile.Filter = "Config(*.cfg)|*.cfg";
            DialogResult result = openFile.ShowDialog();
            string AllSelectedFile = openFile.FileName;

            if (result == DialogResult.OK)
            {
                if (AllSelectedFile.Length > 0) lblcfgfile.Text = AllSelectedFile;
                else
                {
                    this.StatusMessage = "File is corrupted";
                    Application.DoEvents();
                    return "";
                }
            }
            else
            {
                //do nothing
            }
            return AllSelectedFile;
        }

        private string VerifyTouFile(string fileName)
        {
            try
            {
                string tmpCount = string.Empty;
                string fileContent = string.Empty;
                string touSendCommand = string.Empty;
                string touFile = string.Empty;
                if (!File.Exists(fileName)) return "";
                StringBuilder fileData = new StringBuilder();
                StreamReader streamReader = File.OpenText(fileName);
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);

                while (((tmpCount = streamReader.ReadLine()) != null))
                {
                    fileContent += tmpCount;
                }

                streamReader.Close();
                if (fileContent.Length > 0)
                {
                    if (fileContent.Length < 237) return "1";
                    else return "0";
                }
                return "1";
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "VerifyTouFile(string fileName)", ex);
                return "";
                
            }
        }

        public string CalDataBcc(string RecInpData)
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
                logger.Log(LOGLEVELS.Error, "CalDataBcc(string RecInpData)", ex);
                return "False";
                
            }
        }

        private string StrToHexForDTM(string GetStr)
        {
            int indecount = 0;
            int intmod = 0;
            int intDiv = 0;
            string strval = "";
            char[] _hexChars = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
            indecount = Convert.ToInt16(GetStr);

            intmod = (indecount % 16);
            intDiv = (indecount / 16);
            if (intDiv <= 9)
            {
                strval = (intDiv + 30).ToString();
            }
            else
            {
                int index1 = Convert.ToUInt16(_hexChars[intDiv]);
                strval = (_hexChars[index1 / 16]).ToString() + (_hexChars[index1 % 16]).ToString();
            }
            if (intmod <= 9)
            {
                strval += (intmod + 30).ToString();
            }
            else
            {
                int index2 = Convert.ToUInt16(_hexChars[intmod]);
                strval += (_hexChars[index2 / 16]).ToString() + (_hexChars[index2 % 16]).ToString();
            }
            return strval;
        }

        public string StrToHex(string GetStr)
        {
            string tempstr = "";
            try
            {
                int indecount = 0;
                char AsciiCh;
                int chrascii;
                char[] _hexChars = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
                while (indecount < GetStr.Length)
                {
                    AsciiCh = Convert.ToChar(GetStr.Substring(indecount, 1));
                    if ((AsciiCh >= 48) && AsciiCh <= 57)
                    {
                        chrascii = (Convert.ToInt16(AsciiCh) - 48) + 30;
                        tempstr += chrascii.ToString();
                    }
                    else
                    {
                        if (AsciiCh != 32)
                        {
                            chrascii = Convert.ToInt16(AsciiCh);
                            AsciiCh = _hexChars[chrascii / 16];
                            tempstr += (_hexChars[chrascii / 16]).ToString() + (_hexChars[chrascii % 16]).ToString();
                        }
                        else
                        {
                            tempstr += "20";       //Space
                        }
                    }
                    indecount++;
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "StrToHex(string GetStr)", ex);
                return "";
                
            }
            return tempstr;
        }

        //private void CmriAuth_btnOK_Click(object sender, EventArgs e)
        //{
        //    string LPRParaCmd = string.Empty;
        //    string Dailylogcmd = string.Empty;
        //    string CtRation = string.Empty;
        //    if (chk_CTRatio.Checked)
        //    {
        //        if (!ValidateCTRatio()) return;
        //        CtRation = Convert.ToInt32(txt_CTratio.Text).ToString("X");
        //        while (CtRation.Length < 2) CtRation = "0" + CtRation;
        //        CtRation = "(" + CtRation + ")";

        //    }
        //    else
        //    {
        //        CtRation = "(" + "00" + ")";
        //    }
        //    //------------------Command & Verification For LPR Parameter----------------------
        //    if (chk_LPRPara.Checked)
        //    {
        //        if (!chkLPRParastate()) return;
        //        LPRParaCmd = "(" + GetDailyLogCmd() + ")";
        //    }
        //    else
        //    {
        //        LPRParaCmd = "(" + "0000" + ")";
        //    }
        //    //-----------Command & Verification For Daily Log---------------------------------
        //    if (chk_DailyLog.Checked)
        //    {
        //        if (!IsValidateDailyLog()) return;
        //        string Tranrating = Convert.ToInt32(txt_Rating.Text).ToString("X");
        //        while (Tranrating.Length < 4) Tranrating = "0" + Tranrating;
        //        string HL = Convert.ToInt32(txt_Highload.Text).ToString("X");
        //        while (HL.Length < 2) HL = "0" + HL;
        //        string LL = Convert.ToInt32(txt_LowLoad.Text).ToString("X");
        //        while (LL.Length < 2) LL = "0" + LL;
        //        Dailylogcmd = "(" + HL + LL + Tranrating + ")";
        //    }
        //    else
        //    {
        //        Dailylogcmd = "(" + "00000000" + ")";
        //    }
        //    //-------------------------End Of Daily Log----------------------------------------


        //    try
        //    {


        //        string Auth_flg = string.Empty;
        //        if (chk_SetTOU.Checked)
        //        {
        //            if (lblschdfile.Text == "")
        //            {
        //                //this.StatusMessage = "Please Select TOU File From Browse..!";
        //                MessageBox.Show("Please Select TOU File From Browse..!", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //                Btn_SelectTouFile.Focus();
        //                return;
        //            }
        //            if (!File.Exists(lblschdfile.Text))
        //            {
        //                //this.StatusMessage = "Selected TOU File Does Not Exist!";
        //                MessageBox.Show("Selected TOU File Does Not Exist!", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //                Btn_SelectTouFile.Focus();
        //                return;
        //            }
        //        }
        //        if (listBoxSelectedMeters.Items.Count <= 0)
        //        {
        //            //this.StatusMessage = "Select At Lease One Item From List";
        //            MessageBox.Show("Select At Lease One Item From List", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //            if (listBoxAvailableMeters.Items.Count > 0) listBoxAvailableMeters.SelectedIndex = 0;
        //            return;
        //        }
        //        if (!chk_SetRTC.Checked && !chk_SetTamperIcon.Checked && !chk_SetTOU.Checked && !chk_BillingReset.Checked && !chk_CTRatio.Checked && !chk_LPRPara.Checked && !chk_DailyLog.Checked)
        //        {
        //            //this.StatusMessage = "Select AT Least One Option";
        //            MessageBox.Show("Select AT Least One Option", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //            return;
        //        }
        //        /*--------------Data Programm Options---------------------------*/

        //        if (chk_SetRTC.Checked) Auth_flg += "1";
        //        else Auth_flg += "0";
        //        if (chk_SetTamperIcon.Checked) Auth_flg += "1";
        //        else Auth_flg += "0";
        //        if (chk_SetTOU.Checked) Auth_flg += "1";
        //        else Auth_flg += "0";
        //        if (chk_BillingReset.Checked) Auth_flg += "1";
        //        else Auth_flg += "0";
        //        if (chk_CTRatio.Checked) Auth_flg += "1";
        //        else Auth_flg += "0";
        //        if (chk_LPRPara.Checked) Auth_flg += "1";
        //        else Auth_flg += "0";
        //        if (chk_DailyLog.Checked) Auth_flg += "1";
        //        else Auth_flg += "0";


        //        string MeterIDList = GetMeterIDList();

        //        MeterIDList = "\r\n" + LPRParaCmd + MeterIDList;
        //        MeterIDList = "\r\n" + Dailylogcmd + MeterIDList;
        //        MeterIDList = "\r\n" + CtRation + MeterIDList;
        //        /*Creating Authentication File*/
        //        int getcreate = CreateScheduleFile("(" + Auth_flg + ")" + MeterIDList);
        //        if (getcreate == 0)
        //        {
        //            this.StatusMessage = "Data Saved Successfully";
        //            Application.DoEvents();
        //        }
        //        else if (getcreate == 2)
        //        {
        //            this.StatusMessage = "Schedule File Created";
        //            Application.DoEvents();
        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        MessageBox.Show(Ex.ToString());

        //    }
        //}

        private string GetMeterIDList()
        {
            string MeterIdList = string.Empty;
            DataTable DTable = new DataTable();
            try
            {
                int itmcnt = 0;
                while (itmcnt < listBoxSelectedMeters.Items.Count)
                {
                    MeterIdList += "\r\n" + listBoxSelectedMeters.Items[itmcnt++].ToString().Trim();
                }
                return MeterIdList;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetMeterIDList()", ex);
                return "";
                
            }
        }



        /// <summary>
        /// Create The Schedule File On Selected Path
        /// </summary>
        /// <param name="FileContent"></param>
        /// <returns></returns>
        private bool CreateScheduleFile(string fileContent)
        {
            try
            {
                SaveFileDialog FileDialog = new SaveFileDialog();
                string fileName = string.Empty;
                FileDialog.Filter = "Schedule Files (*.Lcd)|*.Lcd";
                FileDialog.FilterIndex = 1;
                if (FileDialog.ShowDialog() == DialogResult.Cancel)
                    return false;
                else
                    fileName = FileDialog.FileName;
                if (fileName.Trim().Length > 0)
                {
                    this.StatusMessage = "File Saving in  " + fileName + "...";
                    Application.DoEvents();
                    FileStream file1 = new FileStream(fileName, FileMode.Create);
                    StreamWriter wr1 = new StreamWriter(file1);
                    /*-------------------Calculate File BCC and Append-------------*/
                    string strdata = fileContent.Replace("\r\n", "");
                    //fileName = "cfgFilePath:" + lblcfgfile.Text;
                    strdata = StrToHex(strdata);
                    string fileBcc = CalBcc(strdata);
                    int BccVal = Convert.ToInt32(fileBcc, 16);
                    char mchar;
                    if (BccVal == 9 || BccVal == 10 || BccVal == 32)
                        mchar = '\\';
                    else
                        mchar = Convert.ToChar(BccVal);
                    /*--------------------End Of Bcc Appending---------------------*/
                    if (fileName == "") wr1.Write(fileContent + "\r\n" + mchar.ToString());
                    else wr1.Write(fileContent + "\r\n" + mchar.ToString());

                    wr1.Close();
                    file1.Close();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "CreateScheduleFile(string fileContent)", ex);
                return false;
                
            }
        }
        public string CalBcc(string RecInpData)
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
                logger.Log(LOGLEVELS.Error, "CalBcc(string RecInpData)", ex);
                return "False";
                
            }
        }

        private void CMRIScheduleFile_Load(object sender, EventArgs e)
        {
            this.StatusMessage = "";
            txtusername.Text = ConfigSettings.GetValue("CCUserName");
            txtpwd.Text = ConfigSettings.GetValue("CCUserPassword");
            scdfileoperationpanel.Visible = !SecurityFileFlag;
            securityfilepanel.Visible = SecurityFileFlag;

            if (SecurityFileFlag)
            {
                CmriAuth_grpAuth.Text = "Security File";
                this.Text = CmriAuth_grpAuth.Text;
            }

            Application.DoEvents();
            DataSet dSet = new DataSet();
            //dSet = meterDataBLL.ListAllMeterIDValues();
            dSet = meterDataBLL.ListAllMeterIDLPRIDValues();
            dicMeterLPR = new Dictionary<string, string>();
            foreach (DataRow drow in dSet.Tables[0].Rows)
            {
                if (!dicMeterLPR.ContainsKey(Convert.ToString(drow["MeterID"])))
                {
                    dicMeterLPR.Add(Convert.ToString(drow["MeterID"]), Convert.ToString(drow["Meter_GPRSModem_IMEI"]));
                    listBoxAvailableMeters.Items.Add(drow["MeterID"].ToString());
                }
            }
        }
        
        private void CMRIScheduleFile_Activated(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            Application.DoEvents();
        }

        private void btnCMRISchedule_Click(object sender, EventArgs e)
        {
            string MeterIDList = string.Empty;
            string oneLineText = string.Empty;
            StringBuilder scheduleFileData = new StringBuilder();
            StringBuilder nonDLMSData = new StringBuilder();
            StreamReader reader = null;

            if (listBoxSelectedMeters.Items.Count <= 0)
            {
                MessageBox.Show("Please select a Meter ID", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                if (listBoxAvailableMeters.Items.Count > 0) listBoxAvailableMeters.SelectedIndex = 0;
                return;
            }

            if (!chkisLPRSchedule.Checked)
            {
                //Fix Defect #224435
                if (lblcfgfile.Text == "" || lblcfgfile.Text == "cfg File Path")
                {
                    MessageBox.Show("Please select a configuration file", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    btnSelectcfgfile.Focus();
                    return;
                }
                if (!File.Exists(lblcfgfile.Text))
                {
                    MessageBox.Show("Selected configuration file does not exist", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    btnSelectcfgfile.Focus();
                    return;
                }
                if (txtLSDays.Text == "")
                {
                    MessageBox.Show("Please enter the no. of days for Load Survey readout", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    btnSelectcfgfile.Focus();
                    return;
                }
                if (Convert.ToInt16(txtLSDays.Text) < 1 || Convert.ToInt16(txtLSDays.Text) > 90)
                {
                    MessageBox.Show("Invalid entry for the no. of days for Load Survey readout", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    btnSelectcfgfile.Focus();
                    return;
                }
                if (txtClearCMRI.Text == "")
                {
                    MessageBox.Show("Please enter the Clear CMRI Password", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    btnSelectcfgfile.Focus();
                    return;
                }
                if (listBoxSelectedMeters.Items.Count <= 0)
                {
                    MessageBox.Show("Please select a Meter ID", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    if (listBoxAvailableMeters.Items.Count > 0) listBoxAvailableMeters.SelectedIndex = 0;
                    return;
                }

                /*string MeterIDList = GetMeterIDList();
                string oneLineText = string.Empty;
                StringBuilder scheduleFileData = new StringBuilder();
                StringBuilder nonDLMSData = new StringBuilder();*/
                reader = new StreamReader(lblcfgfile.Text);

                #region AppendDLMSConfigSettings
                while ((oneLineText = reader.ReadLine()) != null)
                {
                    if (oneLineText == BCSConstants.IEC)
                    {
                        break;
                    }
                    else
                    {
                        scheduleFileData.AppendLine(oneLineText);
                    }
                }
            }
            else
            {
                scheduleFileData.AppendLine("DLMS");
                scheduleFileData.AppendLine("170000160000FF0912001E");
            }

            MeterIDList = GetMeterIDList();
            string[] meterIdList = MeterIDList.Split('\n');
            string meterId = string.Empty;
            string LPRId = string.Empty;
            string meterIdCommand = "";
            meterIdCommand = "110000410000FF0201" + (meterIdList.Length - 1).ToString("00");

            // Support for LPR PED
            if (chkisLPRSchedule.Checked) meterIdCommand = "110000410000FF0201" + (meterIdList.Length - 1).ToString("0000");

            for (int meter = 1; meter < meterIdList.Length; meter++)
            {
                meterId = meterIdList[meter].Replace("\r", "").Trim();
                //LPRId = dicMeterLPR[meterId].Trim();//---------This Condition is replaced by below dictionary search query
                string[] keysByValue = dicMeterLPR.Where(x => x.Key == meterId).Select(pair => pair.Value).ToArray();
                if (keysByValue.Length > 0) LPRId = keysByValue[0];
                else LPRId = ""; 
                if (LPRId.Length > 0)
                {
                    meterId = meterId + "," + LPRId;
                }

                // Support for LPR PED
                if (chkisLPRSchedule.Checked)
                {
                    meterIdCommand += "020212" + String.Format("{0:X4}", meter) + "09" +
                        String.Format("{0:X2}", (meterId.Length)) + ToHex(meterId);
                }
                else
                {
                    meterIdCommand += "02021200" + String.Format("{0:X2}", meter) + "09" +
                        String.Format("{0:X2}", (meterId.Length)) + ToHex(meterId);
                }
            }
            //writte meter id
            scheduleFileData.AppendLine(meterIdCommand);
            //Write LS Days
            meterIdCommand = "0100006001CAFF021200" + String.Format("{0:X2}", Convert.ToByte(txtLSDays.Text));
            scheduleFileData.AppendLine(meterIdCommand);
            //Write password
            meterIdCommand = "0100006001C9FF0209" + String.Format("{0:X2}", txtClearCMRI.Text.Length) + ToHex(txtClearCMRI.Text);
            scheduleFileData.AppendLine(meterIdCommand);
                #endregion

            #region AppendIECSettings
            if (oneLineText != null && !chkisLPRSchedule.Checked)
            {
                nonDLMSData.Append(BCSConstants.IEC);
                while ((oneLineText = reader.ReadLine()) != null)
                {
                    nonDLMSData.AppendLine(oneLineText);
                }
                int touIndex = nonDLMSData.ToString().IndexOf('<');
                if (touIndex > 0)
                {
                    nonDLMSData.Insert(touIndex - 1, MeterIDList.Substring(1).Trim() + "\r");
                }
                else
                {
                    nonDLMSData.Append(MeterIDList.Trim());
                }
                scheduleFileData.Append(nonDLMSData.ToString());
            }
            else
            {
                scheduleFileData.Append("NONDLMS(0000000)\r\n(00)");
            }
            #endregion

            if (reader != null)
                reader.Close();

            /*Creating Authentication File*/
            if (CreateScheduleFile(scheduleFileData.ToString()))
            {
                this.StatusMessage = "Schedule File Created Successfully";
                Application.DoEvents();
            }
        }

        private  string ToHex(string input)
        {
            //foreach (char ch in meterID)
            //{
            //    Buffer[nBufferIndex++] = Convert.ToByte(ch);
            //}

            StringBuilder sb = new StringBuilder();
            foreach (char c in input)
                sb.AppendFormat("{0:X2}", (int)c);
            return sb.ToString().Trim();
        }


        private void btnScheduleCancel_Click(object sender, EventArgs e)
        {
            this.StatusMessage = "";
            Application.DoEvents();
            this.Close();
        }

        private string VerifycfgFile(string fileName)
        {
            try
            {
                string tmpCount = string.Empty;
                string fileContent = string.Empty;
                if (!File.Exists(fileName)) return "";
                StringBuilder fileData = new StringBuilder();
                StreamReader streamReader = File.OpenText(fileName);
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);

                while (((tmpCount = streamReader.ReadLine()) != null))
                {
                    fileContent += tmpCount;
                }

                streamReader.Close();
                if (fileContent.Length > 0)
                {
                    if (fileContent.Length <= 0) return "1";
                    else return "0";
                }
                return "1";
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "VerifycfgFile(string fileName)", ex);
                return "";
                
            }
        }

        private void btnSelectcfgfile_Click(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            lblcfgfile.Text = "";
            string cfgCommand = GetScheduleFile();
            if (cfgCommand == "") return;
            lblcfgfile.Text = cfgCommand;
            string fileExt = string.Empty;
            if (lblcfgfile.Text.IndexOf(".") > 0)
            {
                if (!(lblcfgfile.Text.Contains(".cfg")) && (!(lblcfgfile.Text.Contains(".CFG"))))
                {
                    this.StatusMessage = "Please select the configuration file";
                    Application.DoEvents();
                    btnSelectcfgfile.Focus();
                    lblcfgfile.Text = "";
                    return;
                }
            }

            if (lblcfgfile.Text.Trim().Length > 0) cfgCommand = VerifycfgFile(lblcfgfile.Text);

            if (cfgCommand.Length <= 0)
            {
                this.StatusMessage = "Please select the configuration file";
                Application.DoEvents();
                lblcfgfile.Text = "";
                return;
            }
            if (cfgCommand == "1")
            {
                this.StatusMessage = "File corrupted!";
                Application.DoEvents();
                lblcfgfile.Text = "";
                return;
            }
        }

        private void txtClearCMRI_KeyPress(object sender, KeyPressEventArgs e)
        {
            int ascii = Convert.ToInt16(e.KeyChar);
            if (!((ascii >= 97 && ascii <= 122) || (ascii >= 65 && ascii <= 90) || (ascii >= 48 && ascii <= 57) || (ascii == 8)))
            { e.Handled = true; }
        }

        private void txtLSDays_KeyPress(object sender, KeyPressEventArgs e)
        {
            int ascii = Convert.ToInt16(e.KeyChar);
            if (!((ascii >= 48 && ascii <= 57) || (ascii == 8)))
            { e.Handled = true; }
        }

        private void CMRIScheduleFile_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.StatusMessage = "";
            this.RightStatusMessage = "";
        }

        private void chkisLPRSchedule_CheckedChanged(object sender, EventArgs e)
        {
            btnSelectcfgfile.Enabled = !chkisLPRSchedule.Checked;
        }

        private void btnsecurityfile_Click(object sender, EventArgs e)
        {
            if (listBoxSelectedMeters == null || listBoxSelectedMeters.Items.Count <= 0)
            {
                MessageBox.Show("Meter ID not available", "Cabcon", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            try
            {
            
            ConfigSettings.ChangeNode("CCUserName", txtusername.Text);
            ConfigSettings.ChangeNode("CCUserPassword", txtpwd.Text);

            List<string> mymeterlist = new List<string>();

            for(int icount = 0; icount < listBoxSelectedMeters.Items.Count; icount++)
                mymeterlist.Add(listBoxSelectedMeters.Items[icount].ToString());

            File.WriteAllLines(AppDomain.CurrentDomain.BaseDirectory + "meterlist.txt", mymeterlist.ToArray()); 

           /* StringWriter sww = new StringWriter();
            XmlWriter writer = XmlWriter.Create(sww);
            RequestMessageType objendDevSecu = new RequestMessageType();
            XmlSerializer xsSubmit = new XmlSerializer(typeof(RequestMessageType));
            string SecurityRequestFileName =  @"\EndDeviceSecurityRequest.xml";//RequestTest_1.xmlEndDeviceSecurityRequest
            
            //System.IO.Directory.CreateDirectory(Application.StartupPath);

           
           
            try
            {

                //-------------------------File Path to Save---------------------------------
                List<string> MatcheddataList = new List<string>();
               
                foreach (var item in listBoxSelectedMeters.Items)
                {
                    MatcheddataList.Add(item.ToString());
                }

                if (CreateModXml(MatcheddataList, SecurityRequestFileName) == false)
                {
                    MessageBox.Show("Unable To Export *.XML File !" + "\n" , "Cabcon", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                GenerateResponseXml(SecurityRequestFileName, txtusername.Text, txtpwd.Text);*/
            
                MessageBox.Show(" File Exported Successfully", "Cabcon", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)    //Exception log for catch block
            {
               MessageBox.Show("Unable To Export  File !" + "\n" + ex.Message, "Cabcon", MessageBoxButtons.OK, MessageBoxIcon.Error);
               logger.Log(LOGLEVELS.Error, "btnsecurityfile_Click(object sender, EventArgs e)", ex);
            }
        }

        private bool GenerateResponseXml(string xmlfilename, string argusername, string argpwd)
        {
           
            CIM2ndEditionCallbackProxyImpl c = new CIM2ndEditionCallbackProxyImpl();
            c.Open("CIM2ndEditionService", "https://indelvr374.ap.bm.net/GSIS_CIM2ndEdition/CIM2ndEd.svc", SoapClientCredentialType.Basic);
            // string filePath = @"C:\Users\BholaA\Desktop\RequestTest_1.xml";
            string filePath = AppDomain.CurrentDomain.BaseDirectory + @"SecurityFiles\EndDeviceSecurityRequest.xml";//@"C:\Cabcon Files\Configuration Files\EndDeviceSecurityRequest.xml";
            string filePathresponse = AppDomain.CurrentDomain.BaseDirectory + @"SecurityFiles\EndDeviceSecurityResponse.xml"; //@"C:\Cabcon Files\Configuration Files\EndDeviceSecurityresponse.xml";
            TextReader fs = new StreamReader(filePath);
            XmlReader reader = XmlReader.Create(fs);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(LandisGyr.GSIS.CIM2ndEd.RequestMessageType));
            var requestdata = (LandisGyr.GSIS.CIM2ndEd.RequestMessageType)xmlSerializer.Deserialize(reader);
            RequestRequest r = new RequestRequest();
            r.RequestMessage = requestdata;
            var res = c.Request(r, SoapClientCredentialType.Basic, "Mohsin", "Mohsin@123");
            c.Close();

            XmlSerializer xsSubmit = new XmlSerializer(typeof(LandisGyr.GSIS.CIM2ndEd.ResponseMessageType));
            StringWriter sww = new StringWriter();
            XmlWriter writer = XmlWriter.Create(sww);
            xsSubmit.Serialize(writer, res.ResponseMessage);
            var xml = sww.ToString();

            //---------------------Service Call-------------------------
            StringReader strReader = null;
            XmlSerializer serializer = null;
            XmlTextReader xmlReader = null;
            Object objxmlData = null;

            strReader = new StringReader(xml);
            serializer = new XmlSerializer(typeof(RequestMessageType));
            xmlReader = new XmlTextReader(strReader);
            //  objxmlData = serializer.Deserialize(xmlReader);

            //-----------------------Save File--------------------------
            var xmldocument = XDocument.Parse(xml);

            xmldocument.Save(filePathresponse);
                       
            return false;
        }

        private bool CreateXml(List<string> configparaList, string xmlfilename)
        {
            bool isSuccess = true;
            //string StrPgmDesc = string.Empty;
            //try
            //{


            //    RequestMessageType objxsd = new RequestMessageType();
            //    Header objheader = new Header();
            //    objheader.Verb = "get";
            //    //objheader.Noun = "GetEndDeviceSecurityConfig";//EndDeviceSecurityConfig
            //    objheader.Noun = "EndDeviceSecurityConfig";
            //    objheader.Revision = 2.0M;
            //    objheader.Timestamp = System.DateTime.Now;
            //    objheader.Source = "HHU";
            //    objheader.MessageID = Guid.NewGuid().ToString();// "7427076d-4dde-47ad-a8f9-83880bcca114";
            //    objheader.CorrelationID = "D9F521CB-406F-4F29-AEC3-2F17C8755B2D";
            //    objxsd.Header = objheader;

            //    List<GetEndDeviceSecurityConfigEndDeviceSecurityNames> objNameCollection = new List<GetEndDeviceSecurityConfigEndDeviceSecurityNames>();
            //    List<GetEndDeviceSecurityConfigEndDeviceSecurity> objNameCollectionx = new List<GetEndDeviceSecurityConfigEndDeviceSecurity>();
            //    GetEndDeviceSecurityConfigEndDeviceSecurity opsx = new GetEndDeviceSecurityConfigEndDeviceSecurity();
                
            //    GetEndDeviceSecurityConfigEndDeviceSecurityNames ops = new GetEndDeviceSecurityConfigEndDeviceSecurityNames();



            //    GetEndDeviceSecurityConfigEndDeviceSecurityNamesNameType objNameType = new GetEndDeviceSecurityConfigEndDeviceSecurityNamesNameType();
            //    int commdCount = 0;
            //    while (commdCount < configparaList.Count)
            //    {
            //        string paraContents = configparaList[commdCount];

            //        ops = new GetEndDeviceSecurityConfigEndDeviceSecurityNames();
            //        opsx = new GetEndDeviceSecurityConfigEndDeviceSecurity();

            //        objNameType = new GetEndDeviceSecurityConfigEndDeviceSecurityNamesNameType();

            //        opsx.Names.name = "00000004";

            //        ops.name = "00000004";//configparaList[commdCount];
            //       // objNameType.description = ops.name + "description";
            //       // objNameType.name = ops.name + "name";
            //        objNameType.description =  "description";
            //        objNameType.name =  "MeterID";
            //        ops.NameType = objNameType;
            //        objNameCollection.Add(ops);
            //        commdCount++;
            //    }
            //    GetEndDeviceSecurityConfigEndDeviceSecurityKeyEncryptionKey objKeyInfo = new GetEndDeviceSecurityConfigEndDeviceSecurityKeyEncryptionKey();
            //    Request oblPayload = new Request();
            //    //-----------------------RSA to Get Packet Encryption Exponent & Modulus-------------------------------------

            //    //string publicKey = "a383a2916281721498ff28226f851613bab6f89eb0536e9f237e158596d3b012e5707eba9f2a2963faca63fcb10f5f246c1f587ee6e8f895fd848f2da5aba9d71af4dd8d06e99ff3729631626ed3f3202e56962957c0110a99d2b3893feb148291e09b54fe7df121751fb8bb589576542321b4f548be06b9845ebc6bbef1427741c00b632c05854146b597fdef5a89ace1556a769c5eaff8fc0589e7ad4adb2e2a929969c77f395b2f5a276a9389d1f43c061c9459a65b77bcd581c107aa8424223a0b44ee52582362cc96b90eea071a0dda5e9cb8fd5c31240dc1c9169a629ecec31751069f0c7ccc1c175230390eea071a0dda5e9cb8fd5c31240dc1c9169a";
            //    //string exponant = "098765";
            //    string publicKey = "a383a2916281721498ff28226f851613bab6f89eb0536e9f237e158596d3b012e5707eba9f2a2963faca63fcb10f5f246c1f587ee6e8f895fd848f2da5aba9d71af4dd8d06e99ff3729631626ed3f3202e56962957c0110a99d2b3893feb148291e09b54fe7df121751fb8bb589576542321b4f548be06b9845ebc6bbef1427";
            //    string exponant = "098";
            //    using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            //    {
            //        RSAParameters rsap = new RSAParameters
            //        {
            //            Modulus = Encoding.UTF8.GetBytes(Convert.ToBase64String(Encoding.UTF8.GetBytes(publicKey))),
            //            Exponent = Encoding.UTF8.GetBytes(Convert.ToBase64String(Encoding.UTF8.GetBytes(exponant)))
            //        };
            //        objKeyInfo.exponent = Convert.ToBase64String(Encoding.UTF8.GetBytes(exponant));// rsap.Exponent;
            //        objKeyInfo.modulus = Convert.ToBase64String(Encoding.UTF8.GetBytes(publicKey));// rsap.Modulus;
            //    }
            //    //-----------------------------------------------------------------------------------------------------------
            //    GetEndDeviceSecurityConfigEndDeviceSecurity objeds = new GetEndDeviceSecurityConfigEndDeviceSecurity();
            //    objeds.Names = objNameCollection.ToArray();
            //    objeds.KeyEncryptionKey = objKeyInfo;

            //    GetEndDeviceSecurityConfig objCong = new GetEndDeviceSecurityConfig();
            //    objCong.EndDeviceSecurity = objeds;

            //    oblPayload.GetEndDeviceSecurityConfig = objCong;
            //    objxsd.Request = oblPayload;

            //    XmlSerializer xsSubmit = new XmlSerializer(typeof(RequestMessage));
            //    StringWriter sww = new StringWriter();
            //    XmlWriter writer = XmlWriter.Create(sww);
            //    xsSubmit.Serialize(writer, objxsd);
            //    var xml = sww.ToString();

            //    //---------------------Service Call-------------------------
            //    StringReader strReader = null;
            //    XmlSerializer serializer = null;
            //    XmlTextReader xmlReader = null;
            //    Object objxmlData = null;

            //    strReader = new StringReader(xml);
            //    serializer = new XmlSerializer(typeof(RequestMessage));
            //    xmlReader = new XmlTextReader(strReader);
            //    objxmlData = serializer.Deserialize(xmlReader);


            //    //-----------------------Save File--------------------------
            //    var xmldocument = XDocument.Parse(xml);
            //    xmldocument.Save(xmlfilename);


            //}
            //catch (Exception ex)
            //{
            //    isSuccess = false;
            //}
            return isSuccess;

        }


        private bool CreateModXml(List<string> configparaList, string xmlfilename)
        {
            bool isSuccess = true;
            string StrPgmDesc = string.Empty;
            const string strprivatekeyfilename = @"\securitykey.txt";

            try
            {

                // creation of RequestMessageType object
                RequestMessageType objxsd = new RequestMessageType();
                Header objheader = new Header();
                // Fill Header Information
                objheader.Verb = "get";
                objheader.Noun = "EndDeviceSecurityConfig";
                objheader.Revision = 2.0M;
                objheader.Timestamp = System.DateTime.Now;
                objheader.Source = "HHU";
                objheader.MessageID = Guid.NewGuid().ToString();// "7427076d-4dde-47ad-a8f9-83880bcca114";
                objheader.CorrelationID = "D9F521CB-406F-4F29-AEC3-2F17C8755B2D";
                objxsd.Header = objheader;

                // Instant of Request
                Request _request = new Request();

                // Instant of GetEndDeviceSecurityConfig 
                GetEndDeviceSecurityConfig _getendsecurityconfig = new GetEndDeviceSecurityConfig();

                // Instant of EndDeviceSecurity to fill Names List
                _getendsecurityconfig.EndDeviceSecurity = new GetEndDeviceSecurityConfigEndDeviceSecurity();

                // List for Names
                List<GetEndDeviceSecurityConfigEndDeviceSecurityNames> _collectionNames = new List<GetEndDeviceSecurityConfigEndDeviceSecurityNames>();

                int commdCount = 0;

                // Fill list for Names
                while (commdCount < configparaList.Count)
                {
                    string paraContents = configparaList[commdCount];
                     GetEndDeviceSecurityConfigEndDeviceSecurityNames _names = new GetEndDeviceSecurityConfigEndDeviceSecurityNames();
                     _names.name = "JSM01033";
                    _names.NameType = new GetEndDeviceSecurityConfigEndDeviceSecurityNamesNameType();
                    _names.NameType.description = "description";
                    _names.NameType.name = "MeterID";
                    _collectionNames.Add(_names);
                    commdCount++;
                }

                _getendsecurityconfig.EndDeviceSecurity.Names = _collectionNames.ToArray();

                // Key Information
                GetEndDeviceSecurityConfigEndDeviceSecurityKeyEncryptionKey objKeyInfo = new GetEndDeviceSecurityConfigEndDeviceSecurityKeyEncryptionKey();

                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                                 
                string strpublickey = rsa.ToXmlString(false);
                
                string[] keyInfo = strpublickey.Split(new string[] { "<RSAKeyValue><Modulus>", "</Modulus>" }, StringSplitOptions.RemoveEmptyEntries);
                
                string strModulas = keyInfo[0];
                string strexponent = keyInfo[1].Split(new string[] { "<Exponent>", "</Exponent>" }, StringSplitOptions.RemoveEmptyEntries)[0];
                string strprivatekeyInfo = rsa.ToXmlString(true);
               
                objKeyInfo.modulus = strModulas;// Convert.ToBase64String(rsap.Modulus);// rsap.Modulus;
                objKeyInfo.exponent = strexponent;

                _getendsecurityconfig.EndDeviceSecurity.KeyEncryptionKey = objKeyInfo;

                _request.GetEndDeviceSecurityConfig = _getendsecurityconfig;

                // Update Request and Header 
                objxsd.Request = _request;
                objxsd.Header = objheader;

                XmlSerializer xsSubmit = new XmlSerializer(typeof(RequestMessageType));
                StringWriter sww = new StringWriter();
                XmlWriter writer = XmlWriter.Create(sww);
                xsSubmit.Serialize(writer, objxsd);
                var xml = sww.ToString();

                //---------------------Service Call-------------------------
                StringReader strReader = null;
                XmlSerializer serializer = null;
                XmlTextReader xmlReader = null;
                Object objxmlData = null;

                strReader = new StringReader(xml);
                serializer = new XmlSerializer(typeof(RequestMessageType));
                xmlReader = new XmlTextReader(strReader);
                objxmlData = serializer.Deserialize(xmlReader);

                //-----------------------Save File--------------------------
                var xmldocument = XDocument.Parse(xml);
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"SecurityFiles");
                xmldocument.Save(AppDomain.CurrentDomain.BaseDirectory + @"SecurityFiles" + xmlfilename);
               // xmldocument.Save(Application.StartupPath + xmlfilename);
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"SecurityFiles" + strprivatekeyfilename, strprivatekeyInfo);
                //isSuccess = ZipFiles();
                //Directory.Delete(AppDomain.CurrentDomain.BaseDirectory + @"SecurityFiles", false);
            }
            catch (Exception ex)    //Exception log for catch block
            {
                isSuccess = false;
                logger.Log(LOGLEVELS.Error, "CreateModXml(List<string> configparaList, string xmlfilename)", ex);
            }
            return isSuccess;

        }
              

        private void btnclose_Click(object sender, EventArgs e)
        {
           this.Close();
        }

        /// <summary>
        /// Used to ZIP .ZIP files into a destination folder and returns 
        /// destination folder path.
        /// </summary>
        /// <param name="sourceFilePath"></param>
        /// <returns></returns>
        private bool ZipFiles()
        {
            byte[] startBuffer = {
            80,
            75,
            5,
            6,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0
        };
            // Data for an empty zip file .

            try
            {
                File.Delete(AppDomain.CurrentDomain.BaseDirectory + @"SecurityFiles.zip");

                File.WriteAllBytes(AppDomain.CurrentDomain.BaseDirectory + @"SecurityFiles.zip", startBuffer);

                //We have successfully made the empty zip file .

                //2) Use the Shell32 to zip your files .
                // Declare new shell class
                Shell32.Shell sc = new Shell32.Shell();
                //Declare the folder which contains the files you want to zip .
                Shell32.Folder input = sc.NameSpace(AppDomain.CurrentDomain.BaseDirectory + @"SecurityFiles");
                //Declare  your created empty zip file as folder  .
                Shell32.Folder output = sc.NameSpace(AppDomain.CurrentDomain.BaseDirectory + @"SecurityFiles.zip");
                //Copy the files into the empty zip file using the CopyHere command .
                output.CopyHere(input, 4);
                // delete the original foldder 
                Thread.Sleep(1000);

                Directory.Delete(AppDomain.CurrentDomain.BaseDirectory + @"SecurityFiles", true);
                return true;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ZipFiles()", ex);
            }
            return false;
        }

        private void btnsecuritykey_Click(object sender, EventArgs e)
        {
            try
            {
                this.Enabled = false;

                lbstatus.Text = "Please Wait....";

                Application.DoEvents();
                //GetMeterListFromScd();

                if (listBoxSelectedMeters == null || listBoxSelectedMeters.Items.Count == 0)
                {
                    lbstatus.Text = "Unable to retrive security keys";
                    logger.Log(LOGLEVELS.Error, "btnsecuritykey_Click", new Exception("No meterid available"));
                    return;
                }

                if (txtusername.TextLength < 1 || txtpwd.TextLength < 1)
                {
                    lbstatus.Text = "User Name / Password can not be blank";
                    return;
                }

                string strprivatekey = App_1Phase.Security.SecurityRequestResponse.CreateRequestXml(listBoxSelectedMeters.Items.OfType<string>().ToList());

                if (strprivatekey == null || strprivatekey.Length < 1)
                {
                    lbstatus.Text = "Unable to retrive security keys";
                    logger.Log(LOGLEVELS.Error, "btnsecuritykey_Click", new Exception("private keys not found"));
                    return;
                }



                if (App_1Phase.Security.SecurityRequestResponse.GenerateResponseXml(ConfigSettings.GetValue("EndPointConfigurationName"), ConfigSettings.GetValue("RemoteAddress"), txtusername.Text, txtpwd.Text) == true)
                {
                    lbstatus.Text = "Security keys retrive successfully";
                }
                else
                {
                    lbstatus.Text = "Unable to retrive security keys";
                }

                ConfigSettings.ChangeNode("PrivateKey", strprivatekey);
             
            }
            catch (Exception)
            {
                lbstatus.Text = "Unable to retrive security keys";
            }
            finally
            {
                this.Enabled = true;
            }


        }

    }
}
