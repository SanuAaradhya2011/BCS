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
	public partial class CMRIScheduleFileNonTneb : MdiChildForm
	{
        public CMRIScheduleFileNonTneb()
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

		private void chk_CTRatio_CheckedChanged(object sender, EventArgs e)
		{
			if (chk_CTRatio.Checked) txt_CTratio.Enabled = true;
			else txt_CTratio.Enabled = false;
		}

		private void chk_SetTOU_CheckedChanged(object sender, EventArgs e)
		{
			if (chk_SetTOU.Checked)
			{
				lblschdfile.Visible = true;
				lblschdfile.Text = "";
				Btn_SelectTouFile.Enabled = true;
			}
			else
			{
				Btn_SelectTouFile.Enabled = false;
				lblschdfile.Visible = false;
				lblschdfile.Text = "";
			}
		}

		private void Btn_SelectTouFile_Click(object sender, EventArgs e)
		{
            this.StatusMessage = string.Empty;
            lblschdfile.Text = "";
			string CfgCommand = GetScheduleFile();
			if (CfgCommand == "") return;
			lblschdfile.Text = CfgCommand;
			string fileExt = string.Empty;
			if (lblschdfile.Text.IndexOf(".") > 0)
			{
				if (!(lblschdfile.Text.Contains(".tou")) && (!(lblschdfile.Text.Contains(".TOU"))))
				{
					this.StatusMessage = "Please Select TOU File From Browse...";
					Application.DoEvents();
					Btn_SelectTouFile.Focus();
					lblschdfile.Text = "";
					return;
				}
			}

			if (lblschdfile.Text.Trim().Length > 0) CfgCommand = VerifyTouFile(lblschdfile.Text);

			if (CfgCommand.Length <= 0) 
			{
				this.StatusMessage = "Please Select TOU File From Browse...";
				Application.DoEvents();
				lblschdfile.Text = "";
				return;
			}
			if (CfgCommand == "1")
			{
				this.StatusMessage = "File is corrupted";
				Application.DoEvents();
				lblschdfile.Text = "";
				return;
			}
		}

		private string GetScheduleFile()
		{
			OpenFileDialog openFile = new OpenFileDialog();
			openFile.Multiselect = false;

			openFile.DefaultExt = "TOU";
			openFile.Filter = "TouConfig(*.TOU)|*.TOU";
			DialogResult result = openFile.ShowDialog();
			string AllSelectedFile = openFile.FileName;

			if (result == DialogResult.OK)
			{
				if (AllSelectedFile.Length > 0) lblschdfile.Text = AllSelectedFile;
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

		private void chk_Default_CheckedChanged(object sender, EventArgs e)
        {
            if (!chk_Default.Checked)
            {
                chk_Kwh.Checked = false;
                chk_KvarhLag.Checked = false;
                chk_KvarhLead.Checked = false;
                chk_Kvah.Checked = false;
                chk_MD1.Checked = false;
                chk_MD2.Checked = false;
            }
            else
            {
                chk_Kwh.Checked = true;
                chk_KvarhLag.Checked = true;
                chk_KvarhLead.Checked = true;
                chk_Kvah.Checked = true;
                chk_MD1.Checked = true;
                chk_MD2.Checked = true;
            }
		}

		private void chk_DailyLog_CheckedChanged(object sender, EventArgs e)
		{
			if (chk_DailyLog.Checked) GRPDailyLog.Enabled = true;
			else GRPDailyLog.Enabled = false;
		}

		private void chk_LPRPara_CheckedChanged(object sender, EventArgs e)
		{
            if (chk_LPRPara.Checked)
            {
                this.StatusMessage = string.Empty;
                GrpLPRPara.Enabled = true;
            }
            else GrpLPRPara.Enabled = false;
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

		private void CmriAuth_btnOK_Click(object sender, EventArgs e)
		{
			string LPRParaCmd = string.Empty;
			string Dailylogcmd = string.Empty;
			string CtRation = string.Empty;
			if (chk_CTRatio.Checked)
			{
				if (!ValidateCTRatio()) return;
				CtRation = Convert.ToInt32(txt_CTratio.Text).ToString("X");
				while (CtRation.Length < 2) CtRation = "0" + CtRation;
				CtRation = "(" + CtRation + ")";

			}
			else
			{
				CtRation = "(" + "00" + ")";
			}
			//------------------Command & Verification For LPR Parameter----------------------
			if (chk_LPRPara.Checked)
			{
				if (!chkLPRParastate()) return;
				LPRParaCmd = "(" + GetDailyLogCmd() + ")";
			}
			else
			{
				LPRParaCmd = "(" + "0000" + ")";
			}
			//-----------Command & Verification For Daily Log---------------------------------
			if (chk_DailyLog.Checked)
			{
				if (!IsValidateDailyLog()) return;
				string Tranrating = Convert.ToInt32(txt_Rating.Text).ToString("X");
				while (Tranrating.Length < 4) Tranrating = "0" + Tranrating;
				string HL = Convert.ToInt32(txt_Highload.Text).ToString("X");
				while (HL.Length < 2) HL = "0" + HL;
				string LL = Convert.ToInt32(txt_LowLoad.Text).ToString("X");
				while (LL.Length < 2) LL = "0" + LL;
				Dailylogcmd = "(" + HL + LL + Tranrating + ")";
			}
			else
			{
				Dailylogcmd = "(" + "00000000" + ")";
			}
			//-------------------------End Of Daily Log----------------------------------------


			try
			{


				string Auth_flg = string.Empty;
				if (chk_SetTOU.Checked)
				{
					if (lblschdfile.Text == "")
					{
						//this.StatusMessage = "Please Select TOU File From Browse..!";
						MessageBox.Show("Please Select TOU File From Browse..!", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
						Btn_SelectTouFile.Focus();
						return;
					}
					if (!File.Exists(lblschdfile.Text))
					{
						//this.StatusMessage = "Selected TOU File Does Not Exist!";
						MessageBox.Show("Selected TOU File Does Not Exist!", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
						Btn_SelectTouFile.Focus();
						return;
					}
				}
				if (listBoxSelectedMeters.Items.Count <= 0)
				{
					//this.StatusMessage = "Select At Lease One Item From List";
					MessageBox.Show("Select At Lease One Item From List", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					if (listBoxAvailableMeters.Items.Count > 0) listBoxAvailableMeters.SelectedIndex = 0;
					return;
				}
				if (!chk_SetRTC.Checked && !chk_SetTamperIcon.Checked && !chk_SetTOU.Checked && !chk_BillingReset.Checked && !chk_CTRatio.Checked && !chk_LPRPara.Checked && !chk_DailyLog.Checked)
				{
					//this.StatusMessage = "Select AT Least One Option";
					MessageBox.Show("Select AT Least One Option", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return;
				}
				/*--------------Data Programm Options---------------------------*/

				if (chk_SetRTC.Checked) Auth_flg += "1";
				else Auth_flg += "0";
				if (chk_SetTamperIcon.Checked) Auth_flg += "1";
				else Auth_flg += "0";
				if (chk_SetTOU.Checked) Auth_flg += "1";
				else Auth_flg += "0";
				if (chk_BillingReset.Checked) Auth_flg += "1";
				else Auth_flg += "0";
				if (chk_CTRatio.Checked) Auth_flg += "1";
				else Auth_flg += "0";
				if (chk_LPRPara.Checked) Auth_flg += "1";
				else Auth_flg += "0";
				if (chk_DailyLog.Checked) Auth_flg += "1";
				else Auth_flg += "0";


				string MeterIDList = GetMeterIDList();

				MeterIDList = "\r\n" + LPRParaCmd + MeterIDList;
				MeterIDList = "\r\n" + Dailylogcmd + MeterIDList;
				MeterIDList = "\r\n" + CtRation + MeterIDList;
				/*Creating Authentication File*/
				int getcreate = CreateScheduleFile("(" + Auth_flg + ")" + MeterIDList);
				if (getcreate == 0)
				{
					this.StatusMessage = "Data Saved Successfully";
					Application.DoEvents();
				}
				else if (getcreate == 2)
				{
					this.StatusMessage = "Schedule File Created";
					Application.DoEvents();
				}
			}
			catch (Exception Ex)
			{
				MessageBox.Show(Ex.ToString());

			}
		}

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

		private bool IsValidateDailyLog()
		{
			try
			{
				if (txt_Highload.Text == "" || Convert.ToInt32(txt_Highload.Text) < 1 || Convert.ToInt32(txt_Highload.Text) > 200)
				{
					//this.StatusMessage = "Range Of High load Threshold Should Be 1-200";
					MessageBox.Show("Range Of High load Threshold Should Be 1-200", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					txt_Highload.Focus();
					return false;
				}
				if (txt_LowLoad.Text == "" || Convert.ToInt32(txt_LowLoad.Text) < 0 || Convert.ToInt32(txt_LowLoad.Text) > 100)
				{
					//this.StatusMessage = "Range Of Low load Threshold Should Be 0-100";
					MessageBox.Show("Range Of Low load Threshold Should Be 0-100", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					txt_LowLoad.Focus();
					return false;
				}
				if (txt_Rating.Text == "" || Convert.ToInt32(txt_Rating.Text) < 1 || Convert.ToInt32(txt_Rating.Text) > 700)
				{
					//this.StatusMessage = "Range Of Transformer Rating Should Be 1-700";
					MessageBox.Show("Range Of Transformer Rating Should Be 1-700", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					txt_Rating.Focus();
					return false;
				}
				return true;
			}
			catch (Exception Ex)
			{
				MessageBox.Show(Ex.ToString());
				return false;
			}
		}

		private bool chkLPRParastate()
		{
			try
			{
				if (chk_MinI.Checked == false && chk_MaxI.Checked == false && chk_MinV.Checked == false && chk_MaxV.Checked == false && chk_MD3.Checked == false && chk_MD2.Checked == false && chk_MD1.Checked == false && chk_Kwh.Checked == false && chk_KvarhLag.Checked == false && chk_KvarhLead.Checked == false && chk_Kvah.Checked == false)
				{
					this.StatusMessage = "Please Select At Least One LPR Parameter";
					Application.DoEvents();
					return false;
				}
				else
				{
					return true;
				}
			}
			catch (Exception)
			{
				return false;
			}
		}

		private string GetDailyLogCmd()
		{
			try
			{

				string CmdResponse1 = "";
				if (chk_Kwh.Checked == true) CmdResponse1 = "1" + CmdResponse1;
				else CmdResponse1 = "0" + CmdResponse1;
				if (chk_KvarhLag.Checked == true) CmdResponse1 = "1" + CmdResponse1;
				else CmdResponse1 = "0" + CmdResponse1;
				if (chk_KvarhLead.Checked == true) CmdResponse1 = "1" + CmdResponse1;
				else CmdResponse1 = "0" + CmdResponse1;
				if (chk_Kvah.Checked == true) CmdResponse1 = "1" + CmdResponse1;
				else CmdResponse1 = "0" + CmdResponse1;
				if (chk_MD1.Checked == true) CmdResponse1 = "1" + CmdResponse1;
				else CmdResponse1 = "0" + CmdResponse1;
				if (chk_MD2.Checked == true) CmdResponse1 = "1" + CmdResponse1;
				else CmdResponse1 = "0" + CmdResponse1;
				if (chk_MD3.Checked == true) CmdResponse1 = "1" + CmdResponse1;
				else CmdResponse1 = "0" + CmdResponse1;
				if (chk_MaxV.Checked == true) CmdResponse1 = "1" + CmdResponse1;
				else CmdResponse1 = "0" + CmdResponse1;

				string CmdResponse2 = "";
				if (chk_MinV.Checked == true) CmdResponse2 = "1" + CmdResponse2;
				else CmdResponse2 = "0" + CmdResponse2;
				if (chk_MaxI.Checked == true) CmdResponse2 = "1" + CmdResponse2;
				else CmdResponse2 = "0" + CmdResponse2;
				if (chk_MinI.Checked == true) CmdResponse2 = "1" + CmdResponse2;
				else CmdResponse2 = "0" + CmdResponse2;

				if (chk_CumFundamentalkWh.Checked == true) CmdResponse2 = "1" + CmdResponse2;
				else CmdResponse2 = "0" + CmdResponse2;

				while (CmdResponse1.Length < 8) CmdResponse1 = "0" + CmdResponse1;
				while (CmdResponse2.Length < 8) CmdResponse2 = "0" + CmdResponse2;

				CmdResponse1 = Convert.ToInt32(CmdResponse1, 2).ToString();
				CmdResponse1 = Convert.ToInt32(CmdResponse1).ToString("X");

				CmdResponse2 = Convert.ToInt32(CmdResponse2, 2).ToString();
				CmdResponse2 = Convert.ToInt32(CmdResponse2).ToString("X");
				while (CmdResponse2.Length < 2) CmdResponse2 = "0" + CmdResponse2;
				while (CmdResponse1.Length < 2) CmdResponse1 = "0" + CmdResponse1;
				return CmdResponse1 + CmdResponse2;
			}
			catch (Exception)
			{
				return "";
			}
		}

		private bool ValidateCTRatio()
		{
			try
			{
				if (txt_CTratio.Text.Trim() == "")
				{
					this.StatusMessage = "Please enter CT Ratio";
					Application.DoEvents();
					//MessageBox.Show("Please enter CT Ratio", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					txt_CTratio.Focus();
					return false;
				}

				System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
				Byte[] bytes = encoding.GetBytes(txt_CTratio.Text);
				foreach (byte b in bytes)
				{
					if (b < 48 || b > 57)
					{
						MessageBox.Show("CTRatio can be only numbers");
						txt_CTratio.Focus();
						return false;
					}
				}

				if (Convert.ToInt16(txt_CTratio.Text) < 1 || Convert.ToInt16(txt_CTratio.Text) > 240)
				{
					this.StatusMessage = "Please enter valid CT Ratio (1 - 240)";
					Application.DoEvents();
					txt_CTratio.Focus();
					return false;
				}
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
		/// <summary>
		/// Create The Schedule File On Selected Path
		/// </summary>
		/// <param name="FileContent"></param>
		/// <returns></returns>
		private int CreateScheduleFile(string FileContent)
		{
			try
			{
				SaveFileDialog FileDialog = new SaveFileDialog();
				string fileName = string.Empty;
				FileDialog.Filter = "Schedule Files (*.Lcd)|*.Lcd";
				FileDialog.FilterIndex = 1;
				if (FileDialog.ShowDialog() == DialogResult.Cancel)
					return 1;
				else
					fileName = FileDialog.FileName;

				if (fileName.Trim().Length > 0)
				{
					this.StatusMessage = "File Saved in  " + fileName;
					Application.DoEvents();
					FileStream file1 = new FileStream(fileName, FileMode.Create);
					StreamWriter wr1 = new StreamWriter(file1);
					/*-------------------Calculate File BCC and Append-------------*/
					string strdata = FileContent.Replace("\r\n", "");
					if (chk_SetTOU.Checked) fileName = "TOUFilePath:" + lblschdfile.Text;
					else fileName = "";
					strdata = StrToHex(strdata + fileName);
					string fileBcc = CalBcc(strdata);
					int BccVal = Convert.ToInt32(fileBcc, 16);
					char mchar = Convert.ToChar(BccVal);
					/*--------------------End Of Bcc Appending---------------------*/
					if (fileName == "") wr1.Write(FileContent + "\r\n" + mchar.ToString());
					else wr1.Write(FileContent + "\r\n" + fileName + "\r\n" + mchar.ToString());

					wr1.Close();
					file1.Close();
					return 0;
				}
				else
				{
					return 1;
				}
			}
			catch (Exception)
			{
				return 2;
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

		private void CmriAuth_btnCancel_Click(object sender, EventArgs e)
		{
			this.StatusMessage = "";
			Application.DoEvents();
			this.Close();
		}

        private void CMRIScheduleFile_Activated(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            Application.DoEvents();
        }

		private void txt_Highload_KeyDown(object sender, KeyEventArgs e)
		{
			if (!((e.KeyCode >= Keys.NumPad0 && e.KeyCode <= Keys.NumPad9) ||
				((e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9) && (!e.Shift)) ||
				  e.KeyCode == Keys.Back ||
				  e.KeyCode == Keys.Delete ||
				  e.KeyCode == Keys.Alt ||
				  e.KeyCode == Keys.Left ||
				  e.KeyCode == Keys.Right ||
				  e.KeyCode == Keys.Shift ||
				  e.KeyCode == Keys.Home ||
				  e.KeyCode == Keys.End))

				e.SuppressKeyPress = true;
		}

		private void txt_LowLoad_KeyDown(object sender, KeyEventArgs e)
		{
			if (!((e.KeyCode >= Keys.NumPad0 && e.KeyCode <= Keys.NumPad9) ||
				((e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9) && (!e.Shift)) ||
				  e.KeyCode == Keys.Back ||
				  e.KeyCode == Keys.Delete ||
				  e.KeyCode == Keys.Alt ||
				  e.KeyCode == Keys.Left ||
				  e.KeyCode == Keys.Right ||
				  e.KeyCode == Keys.Shift ||
				  e.KeyCode == Keys.Home ||
				  e.KeyCode == Keys.End))

				e.SuppressKeyPress = true;
		}

		private void txt_Rating_KeyDown(object sender, KeyEventArgs e)
		{
			if (!((e.KeyCode >= Keys.NumPad0 && e.KeyCode <= Keys.NumPad9) ||
				((e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9) && (!e.Shift)) ||
				  e.KeyCode == Keys.Back ||
				  e.KeyCode == Keys.Delete ||
				  e.KeyCode == Keys.Alt ||
				  e.KeyCode == Keys.Left ||
				  e.KeyCode == Keys.Right ||
				  e.KeyCode == Keys.Shift ||
				  e.KeyCode == Keys.Home ||
				  e.KeyCode == Keys.End))

				e.SuppressKeyPress = true;
		}

        private void chk_SetRTC_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_SetRTC.Checked)
            {
                this.StatusMessage = string.Empty;
            }
        }

        private void chk_SetTamperIcon_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_SetTamperIcon.Checked)
            {
                this.StatusMessage = string.Empty;
            }
        }

        private void chk_BillingReset_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_BillingReset.Checked)
            {
                this.StatusMessage = string.Empty;
            }
        }

        private void CmriAuth_grpAuth_Enter(object sender, EventArgs e)
        {

        }

	}
}
