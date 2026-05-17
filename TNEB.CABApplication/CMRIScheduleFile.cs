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

namespace CAB.UI
{
    public partial class CMRIScheduleFile : MdiChildForm
    {
        public CMRIScheduleFile()
        {
            InitializeComponent();
        }

        MeterDataBLL meterDataBLL = new MeterDataBLL();

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
            catch (Exception)
            {
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
            catch (Exception)
            {
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
            catch (Exception)
            {
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
            catch (Exception)
            {
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
            catch (Exception)
            {
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
            catch (Exception)
            {
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
            catch (Exception)
            {
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
                    MeterIdList += "\r\n" + listBoxSelectedMeters.Items[itmcnt++].ToString();
                }
                return MeterIdList;
            }
            catch
            {
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
                    this.StatusMessage = "File Saved in  " + fileName;
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
            catch (Exception)
            {
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
            catch (Exception)
            {
                return "False";
            }
        }

        private void CMRIScheduleFile_Load(object sender, EventArgs e)
        {
            this.StatusMessage = "";
            Application.DoEvents();
            DataSet dSet = new DataSet();
            dSet = meterDataBLL.ListAllMeterIDValues();
            foreach (DataRow drow in dSet.Tables[0].Rows)
            {
                listBoxAvailableMeters.Items.Add(drow["MeterID"].ToString());
            }
        }

        private void CMRIScheduleFile_Activated(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            Application.DoEvents();
        }

        private void btnCMRISchedule_Click(object sender, EventArgs e)
        {
            if (lblcfgfile.Text == "")
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
            if (Convert.ToInt16(txtLSDays.Text) < 1 || Convert.ToInt16(txtLSDays.Text) >90)
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

            string MeterIDList = GetMeterIDList();
            //string scheduleFileData = txtLSDays.Text + "\r\n " + txtClearCMRI.Text + MeterIDList;

            //FileStream fsConfig = new FileStream(lblcfgfile.Text, FileMode.Append);
            //StreamWriter wr1 = new StreamWriter(fsConfig);
            //wr1.Write(scheduleFileData);
            //wr1.Close();
            //fsConfig.Close();
            StringBuilder scheduleFileData = new StringBuilder();
            StreamReader reader = new StreamReader(lblcfgfile.Text);
            scheduleFileData.Append(reader.ReadToEnd());
            if (scheduleFileData.ToString().Contains("DLMS")) //DLMS Schedule file
            {
                string[] meterIdList = MeterIDList.Split('\n');
                string meterId = string.Empty;
                string meterIdCommand = "110000410000FF0201" + (meterIdList.Length - 1).ToString("00");
                for(int meter = 1;meter<meterIdList.Length;meter++)
                {
                    meterId = meterIdList[meter].Replace("\r","");
                    meterIdCommand += "02021200" + String.Format("{0:X2}", meter) + "09" +
                        String.Format("{0:X2}", meterId.Length) + ToHex(meterId); 
                }
                //writte meter id
                 scheduleFileData.AppendLine(meterIdCommand);
                //Write LS Days
                meterIdCommand = "0100006001CAFF021200"+  String.Format("{0:X2}", Convert.ToByte(txtLSDays.Text));
                scheduleFileData.AppendLine(meterIdCommand);
                //Write password
                meterIdCommand = "0100006001C9FF0209" + String.Format("{0:X2}", txtClearCMRI.Text.Length) + ToHex(txtClearCMRI.Text);
                scheduleFileData.AppendLine(meterIdCommand);                              
              
            }
            else
            {
                scheduleFileData.AppendLine(txtLSDays.Text);
                scheduleFileData.Append(txtClearCMRI.Text);
                scheduleFileData.Append(MeterIDList);
            }
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
            catch (Exception)
            {
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
    }
}
