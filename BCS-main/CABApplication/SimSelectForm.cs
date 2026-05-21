using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using CAB.BLL;
using CAB.Framework;
using CAB.UI.Controls;
using System.Collections.Generic;
namespace CAB.UI
{
    public partial class SimSelectForm : Form
    {
        #region Private variables
        MeterMasterBLL meterMasterBLL = null;
        private static string simNumber = string.Empty;
        private string commType = string.Empty;
        public List<string> simNumberList;
      
        #endregion

        #region Constructer
        public SimSelectForm(string communictionType)
        {
            InitializeComponent();
            commType = communictionType;
            meterMasterBLL = new MeterMasterBLL();
            FillMeterIdSerialNumber();
            simNumberList = new List<string>();
           

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
            bool isSuccess = false;
            if (txtBoxMeterSIM.Text.Trim() != string.Empty)
            {
                if (ValidateSimNumber())
                {
                    SimNumber = txtBoxMeterSIM.Text;
                    simNumberList.Clear();
                    simNumberList.Add(txtBoxMeterSIM.Text);
                    isSuccess = true;
                }
            }
            else 
            {
                if (dgvMeterIdAndSim != null)   
                {
                    simNumberList.Clear();
                    for (int i = 0; i < dgvMeterIdAndSim.RowCount; i++)
                    {
                        DataGridViewCheckBoxCell chk1 = dgvMeterIdAndSim.Rows[i].Cells["Select"] as DataGridViewCheckBoxCell;
                        if (Convert.ToBoolean(chk1.Value) == true)
                        {
                            simNumberList.Add(dgvMeterIdAndSim[2, i].Value.ToString());
                        }

                    }
                    isSuccess = true;
                }
            }
            if (simNumberList.Count == 0 && txtBoxMeterSIM.Text.Trim() == string.Empty)
            {
                isSuccess = false;
                MessageBox.Show("Please select/enter atleast one sim number.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (isSuccess)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
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
            if (!long.TryParse(txtBoxMeterSIM.Text, out simNumber))
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


           

       
        private void txtBoxMeterSIM_TextChanged(object sender, EventArgs e)
        {
            if (txtBoxMeterSIM.Text.Trim() == string.Empty)
            {
                dgvMeterIdAndSim.Enabled = true;
            }
            else
            {
                dgvMeterIdAndSim.Enabled = false;
            }
        }

    }
}
