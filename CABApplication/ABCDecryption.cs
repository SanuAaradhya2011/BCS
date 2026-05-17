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
using System.Text.RegularExpressions;
using CABApplication.Reports.Forms;
using CrystalDecisions.Shared;
using Hunt.EPIC.Logging;

namespace CABApplication
{
    public partial class ABCDecryption : MdiChildForm
    {
        public ABCDecryption()
        {
            InitializeComponent();
        }
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(ABCDecryption).ToString());

        string EncyCode = "";
        private ABCCodeDataSet reportXSD = null;
        //=============================
        //Decimal To Binary Conversion Variable Decleration
        const int base10 = 10;
        char[] cHexa = new char[] { 'A', 'B', 'C', 'D', 'E', 'F' };
        int[] iHexaNumeric = new int[] { 10, 11, 12, 13, 14, 15 };
        int[] iHexaIndices = new int[] { 0, 1, 2, 3, 4, 5 };
        const int asciiDiff = 48;
        //============================
        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            if (txt_EnterMID.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please Enter Meter ID", MessageBoxIcon.Stop.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Stop);
                txt_EnterMID.Focus();
                return;
            }
            string BinValue = "";
            string TampValue = "";
            string DcodeByte = "";
            int DecodeValue = 0;

            txtct.Text = ""; txtct.ForeColor = Color.Green;

            DataTable DTable = new DataTable();
            DTable.Columns.Add("Parameter");
            DTable.Columns.Add("Value");
            DataRow Drow;
            int ival = 0;


            EncyCode = txtCode.Text.Trim();
            int[] XorByte = new int[] { 4, 3, 7, 2, 1, 5, 6, 3, 4, 2, 5, 1, 7, 5, 2, 4, 1, 6, 3, 6 };
            if (EncyCode.Length == 20)
            {
                for (int a = 0; a < 20; a++)
                {
                    DecodeValue = Convert.ToInt16(EncyCode.Substring(a, 1));
                    if (DecodeValue <= XorByte[a])
                    {
                        DcodeByte = DcodeByte + Convert.ToString((XorByte[a] - DecodeValue));
                    }
                    else
                    {
                        DcodeByte = DcodeByte + Convert.ToString(((10 - DecodeValue) + XorByte[a]));
                    }
                }
                txtmid.Text = DcodeByte.Substring(19, 1) + DcodeByte.Substring(14, 1) + DcodeByte.Substring(6, 1) + DcodeByte.Substring(5, 1) + DcodeByte.Substring(12, 1) + DcodeByte.Substring(1, 1) + DcodeByte.Substring(13, 1) + DcodeByte.Substring(2, 1);
                txtkwh.Text = DcodeByte.Substring(4, 1) + DcodeByte.Substring(11, 1) + DcodeByte.Substring(3, 1) + DcodeByte.Substring(15, 1) + DcodeByte.Substring(10, 1) + DcodeByte.Substring(16, 1) + "." + DcodeByte.Substring(9, 1);
                txtmd.Text = DcodeByte.Substring(17, 1) + DcodeByte.Substring(8, 1) + "." + DcodeByte.Substring(18, 1);
                string MID = txt_EnterMID.Text.Trim();


                while (MID.Length < 8) MID = "0" + MID;

                if (txtmid.Text.Trim() != MID)
                {
                    txtkwh.Text = "";
                    txtmd.Text = "";
                    txtmd.Text = "";
                    MessageBox.Show("Wrong Code or Meter ID !" + "\r\n" + "Please Enter Valid 20 Digit Code or Valid Meter ID", "ABC Code", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    txtCode.Focus();
                    return;
                }

                BinValue = DecimalToBase(Int32.Parse(DcodeByte.Substring(7, 1)), 2) + DecimalToBase(Int32.Parse(DcodeByte.Substring(0, 1)), 2);
               
                //**************** Code added by Deep for all data in grid ***************
                Drow = DTable.NewRow();
                Drow["Parameter"] = "";
                Drow["Value"] = "---";

                Drow["Parameter"] = "Billing (kWh)";
                Drow["Value"] = txtkwh.Text;
                DTable.Rows.Add(Drow);
                Drow = DTable.NewRow();
                Drow["Parameter"] = "Billing MD (kW)";
                Drow["Value"] = txtmd.Text;
                DTable.Rows.Add(Drow);

                for (int i = 8; i >= 1; i--)
                {
                    TampValue = String.Format("{0:Present;;Absent}", Convert.ToInt16(BinValue.Substring(i - 1, 1)));
                    
                    Drow = DTable.NewRow();
                    Drow["Parameter"] = "";
                    Drow["Value"] = "---";
                         
                    
                    
                    if (TampValue == "Present")
                    {
                        if (i == 8)
                        {
                            Drow["Parameter"] = "Earth Tamper";
                            Drow["Value"] = "Present";


                        }
                        if (i == 7)
                        {
                            Drow["Parameter"] = "Reverse Tamper";
                            Drow["Value"] = "Present";

                        }
                        if (i == 6)
                        {
                            Drow["Parameter"] = "Magnet Tamper";
                            Drow["Value"] = "Present";

                        }
                        if (i == 4)
                        {
                            Drow["Parameter"] = "ESD Tamper";
                            Drow["Value"] = "Present";
                            txtct.ForeColor = Color.Red;
                        }
                        if (i == 3)
                        {
                            Drow["Parameter"] = "Neutral Disturbance";
                            Drow["Value"] = "Present";

                        }
                        if (i == 2)
                        {
                            Drow["Parameter"] = "Single Wire";
                            Drow["Value"] = "Present";

                        }
                    }
                    else
                    {

                        if (i == 8)
                        {
                            Drow["Parameter"] = "Earth Tamper";
                            Drow["Value"] = "Absent";

                        }
                        if (i == 7)
                        {
                            Drow["Parameter"] = "Reverse Tamper";
                            Drow["Value"] = "Absent";

                        }
                        if (i == 6)
                        {
                            Drow["Parameter"] = "Magnet Tamper";
                            Drow["Value"] = "Absent";

                        }
                        if (i == 4)
                        {
                            Drow["Parameter"] = "ESD Tamper";
                            Drow["Value"] = "Absent"; ;
                            txtct.ForeColor = Color.Green;
                        }
                        if (i == 3)
                        {
                            Drow["Parameter"] = "Neutral Disturbance";
                            Drow["Value"] = "Absent";

                        }
                        if (i == 2)
                        {
                            Drow["Parameter"] = "Single Wire";
                            Drow["Value"] = "Absent";

                        }
                    }
                                      

                    if (!Drow["Value"].ToString().Contains("--"))
                        DTable.Rows.Add(Drow);
                }

                string temp_Data = txtmid.Text + "\r\n";
                temp_Data += txtkwh.Text + "\r\n";
                temp_Data += txtmd.Text + "\r\n";
                for (int icount = 0; icount < DTable.Rows.Count; icount++)
                    temp_Data += DTable.Rows[icount]["Parameter"].ToString() + "," + DTable.Rows[icount]["Value"].ToString() + "\r\n";

                CreateTempFile(temp_Data);
                dgtamperstatus.Columns.Clear();
                dgtamperstatus.DataSource = DTable;
                dgtamperstatus.Columns[0].Width = 125;
                dgtamperstatus.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                dgtamperstatus.Columns[1].Width = 105;
                dgtamperstatus.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            else
            {
                MessageBox.Show("Please Enter All 20 Digit", MessageBoxIcon.Warning.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCode.Focus();
            }

        }
        /// <summary>
        /// This function converts the specified decimal value to the base pased as parameter. 
        /// </summary>
        /// <param name="iDec"></param>
        /// <param name="numbase"></param>
        /// <returns>The converted string.</returns>
        string DecimalToBase(int iDec, int numbase)
        {
            string strBin = "";
            int[] result = new int[32];
            int MaxBit = 32;
            for (; iDec > 0; iDec /= numbase)
            {
                int rem = iDec % numbase;
                result[--MaxBit] = rem;
            }
            for (int i = 0; i < result.Length; i++)
                if ((int)result.GetValue(i) >= base10)
                    strBin += cHexa[(int)result.GetValue(i) % base10];
                else
                    strBin += result.GetValue(i);
            strBin = strBin.TrimStart(new char[] { '0' });
            if (strBin == "")
            {
                strBin = "0";
            }
            strBin = string.Format("{0:0000}", Convert.ToDouble(strBin));
            return strBin;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Message"></param>
        public void CreateTempFile(string Message)
        {
            try
            {

                String FilePath = AppDomain.CurrentDomain.BaseDirectory + "\\" + "tmpCrypto.txt";
                FileStream FS = new FileStream(FilePath, FileMode.Create);
                StreamWriter SW = new StreamWriter(FS);
                SW.WriteLine(Message);
                SW.Flush();
                SW.Close();
                FS.Close();

            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "CreateTempFile(string Message)", ex);
            }

        }

        /// <summary>
        /// This event is generated when the user enters a character in txtCode1 text box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            /*========================================================
          ** Check The TextBox Enter Value should be Only Numeric
          ** If not numeric then Disply Message and ask for numeric value
             *e.KeyChar != '' for Ctrl + V key press
          *========================================================*/
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != 8 && e.KeyChar != '')
            {
                MessageBox.Show("Please Enter Only Numeric Value", MessageBoxIcon.Warning.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Handled = true;
            }
        }

        /// <summary>
        /// This event is generated on the click of the Clear button to clear the controls on the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            txtCode.Text = "";
            txtmid.Text = "";
            txtkwh.Text = "";
            txtmd.Text = "";
            //txtet.Text = "";
            //txtmt.Text = "";
            //txtrt.Text = "";
            txtct.Text = "";
            //txtswt.Text = "";
            txt_EnterMID.Text = "";
          

            while (dgtamperstatus.Rows.Count > 0)
            {
                dgtamperstatus.Rows.RemoveAt(0);
            }



            //txtnd.Text = "";
            txtCode.Focus();
        }

        private void btnexit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtCode_TextChanged(object sender, EventArgs e)
        {
            //when user paste ABC code
            if (!Regex.IsMatch(((System.Windows.Forms.TextBox)(sender)).Text, @"^\d+$") && !string.IsNullOrEmpty(((System.Windows.Forms.TextBox)(sender)).Text))
            {
                MessageBox.Show("Please Enter Only Numeric Value", MessageBoxIcon.Warning.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnRpt_Click(object sender, EventArgs e)
        {
            CABApplication.Reports.DLMS_Detailed_Reports.ABCCode abcReport = new CABApplication.Reports.DLMS_Detailed_Reports.ABCCode();
            reportXSD = new ABCCodeDataSet();


            try
            {
                #region Save dailog
                SaveFileDialog DialogSave = new SaveFileDialog();
                DialogSave.DefaultExt = "pdf";
                DialogSave.Filter = "PDF file (*.pdf)|*.pdf";
                DialogSave.AddExtension = true;
                DialogSave.RestoreDirectory = true;
                DialogSave.FileName = System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute + System.DateTime.Now.Second + ".pdf";
                DialogSave.Title = "Where do you want to save the file?";
                DialogSave.RestoreDirectory = true;

                #endregion

                if (DialogSave.ShowDialog() == DialogResult.OK)
                {

                    #region Fill data
                    DataRow reportRow = reportXSD.Tables["DTABCCode"].NewRow();
                    reportRow["Meter ID"] = txtmid.Text;
                    reportRow["Billing kWh"] = txtkwh.Text;
                    reportRow["Billing MD"] = txtmd.Text;
                    foreach (DataGridViewRow dc in dgtamperstatus.Rows)
                    {
                        switch (Convert.ToString(dc.Cells[0].Value))
                        {
                            case "Earth Tamper":
                                reportRow["Earth Tamper"] = dc.Cells[1].Value;
                                break;
                            case "Reverse Tamper":
                                reportRow["Reverse Tamper"] = dc.Cells[1].Value;
                                break;
                            case "Magnet Tamper":
                                reportRow["Magnet Tamper"] = dc.Cells[1].Value;
                                break;
                            case "ESD Tamper":
                                reportRow["ESD Tamper"] = dc.Cells[1].Value;
                                break;
                            case "Neutral Disturbance":
                                reportRow["Neu. Disturbance "] = dc.Cells[1].Value;
                                break;
                            case "Single Wire":
                                reportRow["Single Wire Tamper"] = dc.Cells[1].Value;
                                break;
                        }
                    }
                    reportRow["Datetime"] = DateTime.Now.ToString();
                    reportXSD.Tables["DTABCCode"].Rows.Add(reportRow);
                    abcReport.SetDataSource(reportXSD);
                    #endregion

                    #region Export Options
                    ExportOptions exportOptions = new ExportOptions();
                    DiskFileDestinationOptions diskFileDestinationOptions = new DiskFileDestinationOptions();
                    diskFileDestinationOptions.DiskFileName = DialogSave.FileName;
                    exportOptions.ExportDestinationOptions = diskFileDestinationOptions;
                    exportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    exportOptions.ExportFormatOptions = new PdfRtfWordFormatOptions();
                    exportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    exportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    #endregion
                    //Export
                    abcReport.Export(exportOptions);
                    MessageBox.Show("File Exported Successfully...");
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show("Error occured !!! \n  Please contact to Administrator.");
                logger.Log(LOGLEVELS.Error, "btnRpt_Click(object sender, EventArgs e)", ex);
            }

        }

        private void ABCDecryption_Load(object sender, EventArgs e)
        {

        }
    }
}
