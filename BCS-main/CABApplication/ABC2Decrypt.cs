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
    public partial class ABC2Decrypt : MdiChildForm
    {
        public const int DATA_LONG_PORTION_FIRST = 0;
        public const int DATA_LONG_PORTION_SECOND = (DATA_LONG_PORTION_FIRST + 1);
        public const int DATA_LONG_PORTION_THIRD = (DATA_LONG_PORTION_SECOND + 1);
        public const int DATA_LONG_PORTION_FOURTH = (DATA_LONG_PORTION_THIRD + 1);
        public const int TOTAL_DATA_LONG_PORTION = (DATA_LONG_PORTION_FOURTH + 1);
        public const int DATA_ENCRYPTION_BASE = 22;
        public char[] xg_cEncryptBase = new char[DATA_ENCRYPTION_BASE] { 'D', 'F', 'T', 'L', '1', 'H', '7', '3', '8', 'C', '4', '0', 'J', '9', 'A', '2', 'N', '6', 'P', '5', 'E', 'U' };
        string[] g_strEncrypt = new string[TOTAL_DATA_LONG_PORTION];
        string[] g_strDecrypt = new string[TOTAL_DATA_LONG_PORTION];
        public bool g_bDataUpdateFlag = false;
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(ABC2Decrypt).ToString());
        public ABC2Decrypt()
        {
            InitializeComponent();
        }
        
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
            fn_Decrypt();
           
        }

        private void fn_Decrypt()
        {
            try
            {
                if (txtCode.Text.Trim() == string.Empty)
                {
               MessageBox.Show("Please Enter 20 Digit Alphanumeric Value", MessageBoxIcon.Warning.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
               txtCode.Focus();
                    return;
                }
                string txt1="";
                string txt2="";
                string txt3="";
                string txt4="";
                txt1 = txtCode.Text.Substring(0, 5);
                txt2 = txtCode.Text.Substring(5, 5);
                txt3 = txtCode.Text.Substring(10, 5);
                txt4 = txtCode.Text.Substring(15, 5);

                //TextBox[] encriptedText = new TextBox[] { txtCode, txtCode2, txtCode3, txtCode4 };
                string[] encriptedText = new string[] { txt1, txt2, txt3, txt4 }; 
                string l_strDecrypt;
                Int32[] ll_DataValueLong = new Int32[TOTAL_DATA_LONG_PORTION];
                string[] ll_DataValue = new string[TOTAL_DATA_LONG_PORTION];
                              
                for (int l_iIndexDataPortion = DATA_LONG_PORTION_FIRST; l_iIndexDataPortion < TOTAL_DATA_LONG_PORTION; l_iIndexDataPortion++)
                {
                    ll_DataValueLong[l_iIndexDataPortion] = 0;
                    l_strDecrypt = encriptedText[l_iIndexDataPortion].ToUpperInvariant();
                    for (int l_iIndex = 0; l_iIndex < 5; l_iIndex++)
                    {
                        ll_DataValueLong[l_iIndexDataPortion] = ll_DataValueLong[l_iIndexDataPortion] + (Array.IndexOf(xg_cEncryptBase, l_strDecrypt[l_iIndex]) * ((Int32)Math.Pow(DATA_ENCRYPTION_BASE, l_iIndex)));
                    }
                    ll_DataValue[l_iIndexDataPortion] = ll_DataValueLong[l_iIndexDataPortion].ToString().PadLeft(7, '0');
                }
                GVABC.DataSource = null;
                GVABC.RowCount = 0;
                string meterID = Convert.ToString(ll_DataValue[2]).Substring(1);
                meterID += Convert.ToString(ll_DataValue[3]).Substring(3, 2);

                // *******************Validate Meter ID According to 20 Digit Code _Start ******************
                string MID = txt_EnterMID.Text.Trim();
                while (MID.Length < 8) MID = "0" + MID;
                if (meterID != MID)
                {
                    MessageBox.Show("Wrong Code or Meter ID !" + "\r\n" + "Please Enter Valid 20 Digit Code or Valid Meter ID", "ABC Code", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    txtCode.Focus();
                    return;
                }
                // *******************Validate Meter ID According to 20 Digit Code_ End ******************

                //GVABC.Rows.Add();
                //GVABC.Rows[0].Cells[0].Value = "Meter ID";
                //GVABC.Rows[0].Cells[1].Value = meterID;

                string BillingDate = Convert.ToString(ll_DataValue[1]).Substring(0, 1);
                BillingDate += Convert.ToString(ll_DataValue[3]).Substring(1, 1);
                string BillingMonth = Convert.ToString(ll_DataValue[2]).Substring(0, 1);
                BillingMonth += Convert.ToString(ll_DataValue[3]).Substring(2, 1);
                string BillingYear = Convert.ToString(ll_DataValue[3]).Substring(5);
                GVABC.Rows.Add();
                GVABC.Rows[0].Cells[0].Value = "Billing Date";
                GVABC.Rows[0].Cells[1].Value = BillingDate + "/" + BillingMonth + "/" + BillingYear;

                string kWh = Convert.ToString(ll_DataValue[1]).Substring(1);
                GVABC.Rows.Add();
                GVABC.Rows[1].Cells[0].Value = "Billing (kWh)";
                GVABC.Rows[1].Cells[1].Value = kWh;


                string MD = Convert.ToString(ll_DataValue[0]).Substring(0, 2) + "." + Convert.ToString(ll_DataValue[0]).Substring(2, 1);
                GVABC.Rows.Add();
                GVABC.Rows[2].Cells[0].Value = "Billing MD (kW)";
                GVABC.Rows[2].Cells[1].Value = MD;


                string TamperCount = Convert.ToString(ll_DataValue[0]).Substring(3);
                GVABC.Rows.Add();
                GVABC.Rows[3].Cells[0].Value = "Tamper Counts";
                GVABC.Rows[3].Cells[1].Value = TamperCount;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable To Decrypt ABC Type 2 Code: " + ex.Message, MessageBoxIcon.Warning.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                logger.Log(LOGLEVELS.Error, "fn_Decrypt()", ex);
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
                       
            //if (!char.IsDigit(e.KeyChar) && e.KeyChar != 8 && e.KeyChar != '')
            //{
            //    MessageBox.Show("Please Enter Only Numeric Value", MessageBoxIcon.Warning.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    e.Handled = true;
            //}
        }

        public bool IsAlphaNumeric(string inputString)
        {
            Regex r = new Regex("^[a-zA-Z0-9]+$");
            if (r.IsMatch(inputString))
                return true;
            else
                return false;
        }

      

        /// <summary>
        /// This event is generated on the click of the Clear button to clear the controls on the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            txtCode.Text = "";
           txt_EnterMID.Text = "";


            while (GVABC.Rows.Count > 0)
            {
                GVABC.Rows.RemoveAt(0);
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
            if (!Regex.IsMatch(((System.Windows.Forms.TextBox)(sender)).Text, "^[a-zA-Z0-9]+$") && !string.IsNullOrEmpty(((System.Windows.Forms.TextBox)(sender)).Text))
            {
                MessageBox.Show("Please Enter Only Alphanumeric Value", MessageBoxIcon.Warning.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnRpt_Click(object sender, EventArgs e)
        {
            CABApplication.Reports.DLMS_Detailed_Reports.ABCCodetype2 abcReport = new CABApplication.Reports.DLMS_Detailed_Reports.ABCCodetype2();
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
                    DataRow reportRow = reportXSD.Tables["DTABCType2"].NewRow();
                    reportRow["Meter ID"] = txt_EnterMID.Text;
                   
                    foreach (DataGridViewRow dc in GVABC.Rows)
                    {
                        switch (Convert.ToString(dc.Cells[0].Value))
                        {
                            case "Billing Date":
                                reportRow["BillDate"] = dc.Cells[1].Value;
                                break;
                            case "Billing (kWh)":
                                reportRow["Energykwh"] = dc.Cells[1].Value;
                                break;
                            case "Billing MD (kW)":
                                reportRow["Demandkw"] = dc.Cells[1].Value;
                                break;
                            case "Tamper Counts":
                                reportRow["Tampercount"] = dc.Cells[1].Value;
                                break;
                           
                        }
                    }
                    reportRow["Datetime"] = DateTime.Now.ToString();
                    reportXSD.Tables["DTABCType2"].Rows.Add(reportRow);
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

       
    }
}
