/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 									        |
 * |											Date   : 26/03/2010 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| 
 */

using System;
using System.Windows.Forms;
using CAB.BLL;
using CAB.Entity;
using CAB.Framework.Entity;
using CAB.Framework.Utility;
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
            // Re-center when this control becomes visible (e.g. Add/Edit clicked)
            this.VisibleChanged += (s, e) => { if (this.Visible) DeferCenter(); };
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (OnCancelClick != null)
            {
                OnCancelClick(sender, e);
            }
        }

        /// <summary>
        /// This method is used to save the Data in the case of Add & Edit. 
        /// Before Saving the Data It validate that Data and raised the event for chield.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (cmriMasterEntity == null)
            {
                cmriMasterEntity = new CMRIMasterEntity();
            }

            if (this.txtBoxCMRINumber.Text.Trim().Length < 3)
            {
                MessageBox.Show("CMRI Number should not be less than three letters");
                return;
            }

            cmriMasterEntity.CMRI_Number = this.txtBoxCMRINumber.Text.Trim();
            cmriMasterEntity.CMRI_Description = this.txtBoxCMRIDescription.Text.Trim();
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
            this.txtBoxCMRIDescription.Text = cmriMasterEntity.CMRI_Description;
            this.txtBoxCMRIDescription.Focus();
        }

        private void CMRIMasterControl_Load(object sender, EventArgs e)
        {
            this.StatusMessage = "";
            DeferCenter();
        }

        /// <summary>
        /// Re-center whenever the control resizes — deferred so final MDI size is used.
        /// </summary>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            DeferCenter();
        }

        /// <summary>
        /// Queue CenterGroupBox after all pending layout messages have been processed.
        /// This ensures we use the final, settled control size.
        /// </summary>
        private void DeferCenter()
        {
            if (!this.IsHandleCreated) return;
            this.BeginInvoke(new Action(CenterGroupBox));
        }

        private void CenterGroupBox()
        {
            if (groupBoxMRIDefinition == null || !this.IsHandleCreated) return;
            int left = Math.Max(16, (this.Width  - groupBoxMRIDefinition.Width)  / 2);
            int top  = Math.Max(16, (this.Height - groupBoxMRIDefinition.Height) / 2);
            groupBoxMRIDefinition.Location = new System.Drawing.Point(left, top);
        }
    }
}