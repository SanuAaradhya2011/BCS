/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon 								|
 * | 																												|
 * |											Author : Piyush Singh. 	 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| 
 */

using System;
using System.Windows.Forms;
using CAB.BLL;
using CAB.Entity;
using CAB.IECFramework.Entity;
using CAB.IECFramework.Utility;
using CAB.UI.Controls;

namespace CAB.UI
{
    public partial class CMRIMasterControl : UserControl
    { 
        public delegate void ControlStatusChanged(string msg);
        public event ControlStatusChanged OnControlStatusChanged;
        private string message;
        public string StatusMessage
        {
            get
            {
                return message;
            }
            set
            {
                message = value;
                if (OnControlStatusChanged != null)
                {
                    OnControlStatusChanged(message);
                }
            }
        }
        public delegate void CancelClickHandler(object sender, EventArgs e);
        public event CancelClickHandler OnCancelClick;
        public delegate void SaveClickHandler(object sender, EventArgs e);
        public event SaveClickHandler OnSaveClick;

        private CMRIMasterBLL cmriMasterBLL;
        private CMRIMasterEntity cmriMasterEntity;

        public CMRIMasterControl()
        {
            InitializeComponent();
            cmriMasterBLL = new CMRIMasterBLL();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (OnCancelClick != null)
            {
                OnCancelClick(sender, e);
            }
        }
        private bool ValidateCMRINumber(string cmriNumber)
        { 
            decimal deccmriNumber;
            if (decimal.TryParse(cmriNumber, out deccmriNumber))
            {
                if (deccmriNumber == 0)
                    return false;
                else
                    return true;
            }
            return true;
        }
        /// <summary>
        /// This method is used to save the Data in the case of Add & Edit. 
        /// Before Saving the Data It validate that Data and raised the event for chield.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            string cmriType = "Analogic";
            if (cmriMasterEntity == null)
            {
                
                cmriMasterEntity = new CMRIMasterEntity();
            }
            if (rdSands.Checked)
                cmriType = "SANDS";
            if (!ValidateCMRINumber(txtBoxCMRINumber.Text.Trim()))
            {
                this.txtBoxCMRINumber.Text = "";
                this.txtBoxCMRINumber.Focus();
                this.StatusMessage = "CMRI Number can not be zero";
                return;
            }
            cmriMasterEntity.CMRI_Number = this.txtBoxCMRINumber.Text.Trim();
            cmriMasterEntity.CMRI_Description = this.txtBoxCMRIDescription.Text.Trim();
            cmriMasterEntity.CMRIType = cmriType;
            
            if (ValidateData(cmriMasterEntity))
            {
                if (cmriMasterEntity.CMRI_ID == 0)
                {
                    if (!cmriMasterBLL.ValidateCMRI(cmriMasterEntity.CMRI_Number))
                    {
                        cmriMasterBLL.InsertData(cmriMasterEntity);
						this.StatusMessage = "Data Saved Successfully";
                        //CABMessageBox.ShowFilterMessage("M000021", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        CABMessageBox.ShowFilterMessage("L000012|M000011", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtBoxCMRINumber.Focus();
                        return;
                    }
                }
                else
                {
                    cmriMasterBLL.UpdateData(cmriMasterEntity);
					this.StatusMessage = "Data Updated Successfully";
                    //CABMessageBox.ShowFilterMessage("M000092", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                if (OnSaveClick != null)
                {
                    OnSaveClick(sender, e);
                }
            }
        }

        private bool ValidateData(CMRIMasterEntity entity)
        {
            bool Flag = false;
            if (string.IsNullOrEmpty(entity.CMRI_Number))
            {
                CABMessageBox.ShowFilterMessage("L000012|M000001", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtBoxCMRINumber.Focus();
                Flag = false;
            }
            else if (!ValidationProvider.ValidateData(entity.CMRI_Number, ValidationConstant.consumerID))
            {
                CABMessageBox.ShowFilterMessage("L000012|M000012", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtBoxCMRINumber.Focus();
                Flag = false;
            }
            else if (string.IsNullOrEmpty(entity.CMRI_Description))
            {
                CABMessageBox.ShowFilterMessage("L000013|M000001", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtBoxCMRIDescription.Focus();
                Flag = false;
            }

            else
            {
                Flag = true;
            }

            return Flag;
        }

        internal void ClearData()
        {
            cmriMasterEntity = null;
            this.txtBoxCMRINumber.Text = string.Empty;
            this.txtBoxCMRIDescription.Text = string.Empty;
            this.txtBoxCMRINumber.Enabled = true;
            this.txtBoxCMRINumber.Focus();
        }

        internal void EditData(IEntity entity)
        {
			this.StatusMessage = "";
            if (entity == null)
            {
                return;
            }
            cmriMasterEntity = entity as CMRIMasterEntity;
            this.txtBoxCMRINumber.Text = cmriMasterEntity.CMRI_Number;
            this.txtBoxCMRINumber.Enabled = false;
            if (cmriMasterEntity.CMRIType == "Analogic")
                rdAnalogs.Checked = true;
            else
                rdSands.Checked = true;
            this.txtBoxCMRIDescription.Text = cmriMasterEntity.CMRI_Description;
            this.txtBoxCMRIDescription.Focus();
        }

		private void CMRIMasterControl_Load(object sender, EventArgs e)
		{
			this.StatusMessage = "";
            rdAnalogs.Checked = true;

		}
    }
}