using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using CAB.BLL;
using CAB.Framework;
using CAB.UI.Controls;
namespace CAB.UI
{
    public partial class SimSelectForm : Form
    {
        #region Private variables
        MeterMasterBLL meterMasterBLL = null;
        private static string simNumber = string.Empty;
        private string commType = string.Empty;
        private readonly Int32 IMEILength = 15;
        #endregion

        #region Constructer
        public SimSelectForm(string communictionType)
        {
            InitializeComponent();
            commType = communictionType;
            meterMasterBLL = new MeterMasterBLL();
            FillMeterIdSerialNumber();

            //If communication type is GPRS then open  Meter selection form
            if (commType == CommunicationType.GPRS.ToString())
            {
                groupBoxSelectSim.Text = "Select IMEI Number";
                this.Text = "Meter/IMEI Selection";
                lngLabel1.Text = "Modem IMEI Number:";
                txtBoxMeterSIM.MaxLength = IMEILength;
            }

        }
        #endregion
        #region Property
        public static string SimNumber
        {
            get
            {
                return simNumber;
            }
            set
            {
                simNumber = value;
            }
        }
        #endregion
        #region Methods
        private void FillMeterIdSerialNumber()
        {

            DataSet dsMeterIdSimNumber = meterMasterBLL.GetMeterIdAndSimNumber(this.commType);
            if (dsMeterIdSimNumber != null && dsMeterIdSimNumber.Tables != null && dsMeterIdSimNumber.Tables.Count > 0)
            {
                dgvMeterIdAndSim.AutoGenerateColumns = true;
                dgvMeterIdAndSim.DataSource = dsMeterIdSimNumber.Tables[0];
                DataGridViewColumn dgvColumn = new DataGridViewCheckBoxColumn();
                dgvColumn.Name = "Select";
                dgvColumn.HeaderText = "Select";
                if (!dgvMeterIdAndSim.Columns.Contains("Select"))
                {
                    dgvMeterIdAndSim.Columns.Insert(dgvMeterIdAndSim.Columns.Count, dgvColumn);
                }
                dgvMeterIdAndSim.Columns["Meter Id"].ReadOnly = true;
                dgvMeterIdAndSim.Columns["Meter Id"].Width = dgvMeterIdAndSim.Columns["Meter Id"].Width + 20;
                dgvMeterIdAndSim.Columns["Select"].Width = dgvMeterIdAndSim.Columns["Select"].Width - 40;

                //For GPRS sim number selection is not available.
                if (this.commType != CommunicationType.GPRS.ToString())
                {
                    dgvMeterIdAndSim.Columns["Sim Number"].ReadOnly = true;
                }
                    dgvMeterIdAndSim.AllowUserToAddRows = false;
            }
            else
            {
                lblNoData.Visible = true;
                dgvMeterIdAndSim.Visible = false;
                if (dgvMeterIdAndSim.Columns.Contains("Select"))
                {
                    dgvMeterIdAndSim.Columns.Remove("Select");
                }
                dgvMeterIdAndSim.DataSource = null;
            }
        }
        #endregion

        private void btnOK_Click(object sender, EventArgs e)
        {
          
            //If communication type is GPRS then use following reqular expression for validation.
            if (commType == CommunicationType.GPRS.ToString())
            {
                if (new Regex(string.Concat("^[0-9]{", IMEILength, "}$"), RegexOptions.Compiled).Match(txtBoxMeterSIM.Text).Success)
                {
                    SimNumber = txtBoxMeterSIM.Text;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                   
                }
                else
                {
                    CABMessageBox.ShowFilterMessage("M000116", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtBoxMeterSIM.Focus();
                }

            }
            else
            {
                if (ValidateSimNumber())
                {
                    SimNumber = txtBoxMeterSIM.Text;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
        }



        /// <summary>
        /// Validate SIM Number 
        /// </summary>
        /// <returns></returns>
        private bool ValidateSimNumber()
        {
            bool flag = true;
            long simNumber = 0;
            if (txtBoxMeterSIM.Text.Trim() == string.Empty)
            {
                CABMessageBox.ShowFilterMessage("M000115", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtBoxMeterSIM.Focus();
                flag = false;
            }
            // raise error if the number can not be parsed to a 64 bit signed integer
            else if (!long.TryParse(txtBoxMeterSIM.Text, out simNumber))
            {
                CABMessageBox.ShowFilterMessage("M000007|L000058|M000101", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtBoxMeterSIM.Focus();
                flag = false;
            }
            // raise error if the number is not of 10 length
            else if (txtBoxMeterSIM.Text.Trim().Length != 10)
            {
                CABMessageBox.ShowFilterMessage("M000100", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtBoxMeterSIM.Focus();
                flag = false;
            }
            return flag;

        }


        private void GetCheckedTamperIds()
        {
            if (dgvMeterIdAndSim != null)
            {
                foreach (DataGridViewRow row in dgvMeterIdAndSim.Rows)
                {
                    row.Cells["Select"].Value = false;
                }
            }


        }

        private void dgvMeterIdAndSim_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                bool isChecked = (Boolean)dgvMeterIdAndSim[0, e.RowIndex].EditedFormattedValue;
                if (isChecked)
                {
                    //IF communication type is GPRS
                    if (commType == CommunicationType.GPRS.ToString())
                    {
                        txtBoxMeterSIM.Text = dgvMeterIdAndSim[2, e.RowIndex].Value.ToString();
                    }
                    else
                    {
                        txtBoxMeterSIM.Text = dgvMeterIdAndSim[2, e.RowIndex].Value.ToString();
                        txtBoxMeterSIM.Text = txtBoxMeterSIM.Text.Substring(1, txtBoxMeterSIM.Text.Length - 1);

                    }
                    GetCheckedTamperIds();
                    dgvMeterIdAndSim[0, e.RowIndex].Value = true;

                }
                else
                {
                    txtBoxMeterSIM.Text = "";
                }
            }
        }

    }
}
