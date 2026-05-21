/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon 								|
 * | 																												|
 * |											Author : Piyush Singh. 	 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| 
 */

using System;
using System.Data;
using CAB.BLL;
using CAB.Entity;
using CAB.UI.Controls;
using System.Drawing;
using System.Windows.Forms;

namespace CAB.UI
{
    public partial class CMRIMasterForm : MdiChildForm
    {
        private CMRIMasterBLL cmriMasterBLL;
        private CMRIMasterEntity cMRIMasterEntity;
        private AreaBLL areaBLL;
     
        public CMRIMasterForm()
		{
			InitializeComponent();
            cmriMasterBLL = new CMRIMasterBLL();
            ucGridControl.ValueColumn = ucGridControl.HiddenColumn = "CMRI_ID";
		}

        private void CMRIMasterForm_Load(object sender, EventArgs e)
        {
            ucSearchControl.SearchRequire = false;
            FillGridWithSearchType();
        }

        private void FillGridWithSearchType()
        {
            ucGridControl.Visible = true;
            ucDetail.Visible = false;
            DataSet dataSet = cmriMasterBLL.ListDataSet();
            ucSearchControl.RefreshControls(dataSet);
            ucGridControl.Data = dataSet;
            ucGridControl.IsEqual = true;
            ucGridControl.SetEqualWidth();
            ucGridControl.RefreshGrid();
        } 

        private void ucSearchControl_OnAddClick(object sender, EventArgs e)
        {
            ucDetail.Visible = true;
			ucDetail.groupBoxMRIDefinition.Text = "New CMRI Definition"; 
			//this.Text = "New CMRI Definition";
            ucDetail.Location = new Point(18, 14);
            ucGridControl.Visible = false;
            ucSearchControl.Visible = false;
            ucDetail.ClearData();
        }

        private void ucDetail_OnCancelClick(object sender, EventArgs e)
        { 
            CMRIMasterForm_Load(this, null);
            ucSearchControl.Visible = true;
        }

        private void ucDetail_OnSaveClick(object sender, EventArgs e)
        {
            ucSearchControl.Visible = true;
            CMRIMasterForm_Load(this, null);
        } 

		private void ucSearchControl_OnEditClick(object sender, EventArgs e)
		{
			ucDetail.Visible = true;
			ucDetail.groupBoxMRIDefinition.Text = "Edit CMRI Definition"; 
			//this.Text = "Edit CMRI Definition";
            ucDetail.Location = new Point(18, 14);
            ucSearchControl.EnableAdd = ucGridControl.Visible = false;
            ucSearchControl.Visible = false;
            LoadEntity();
		}

        private void LoadEntity()
        {
            string pkValue = ucGridControl.GetPrimaryValue();
            DataSet ds = new DataSet();
            if (!string.IsNullOrEmpty(pkValue))
            {
                cMRIMasterEntity = (CMRIMasterEntity)cmriMasterBLL.GetDetailData(Int32.Parse(pkValue));
                ucDetail.EditData(cMRIMasterEntity);
            }
        }

        private bool ValidateDelete()
        {
            areaBLL = new AreaBLL();
            AreaEntity areaEntity=new AreaEntity();
            string pkValue = ucGridControl.GetPrimaryValue();
            if (!string.IsNullOrEmpty(pkValue))
            {
                areaEntity = (AreaEntity)areaBLL.GetDataforCMRI(Int32.Parse(pkValue));
                if (areaEntity == null)
                {
                    return true;
                }
                else
                {
                    this.StatusMessage = "Selected CMRI allocated to an area, cannot be deleted";
                    return false;
                }
            }
            return false;
        }

		private void ucSearchControl_OnDeleteClick(object sender, EventArgs e)
        {
            if (!ValidateDelete())
                return;
            this.StatusMessage = string.Empty;
            Application.DoEvents();
            if (CABMessageBox.ShowFilterMessage("M000093", "A000001", MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.No))
            {
                return;
            }
            LoadEntity(); 
            cmriMasterBLL.DeleteData(cMRIMasterEntity);
            CMRIMasterForm_Load(this, null);
		}

        private void CMRIMasterForm_Activated(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            Application.DoEvents();
        }

        private void ucDetail_OnControlStatusChanged(string msg)
        {
            this.StatusMessage = msg;
            Application.DoEvents();
        }

        private void ucDetail_Load(object sender, EventArgs e)
        {

        }

        private void CMRIMasterForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.StatusMessage = string.Empty;
            this.RightStatusMessage = string.Empty;
        }

        //CMRIMasterControl cmriMasterControl = null;
        //CMRIMasterEntity cmriMasterEntity;


        ////public CMRIMasterForm()
        //{
        //    InitializeComponent();
        //    cmriMasterBLL = new CMRIMasterBLL();
        //    cmriMasterControl = new CMRIMasterControl();
        //    //cmriMasterControl.OnSaveClick += new CMRIMasterControl.SaveClickHandler(cmriMasterControl_OnSaveClick);
        //}
        //private void lngscCMRIMaster_OnAddClick(object sender, EventArgs e)
        //{
        //    //add user control CMRIMaster onto panel panelCMRIMaster
        //    panelCMRIMaster.Controls.Remove(dataGridCMRIMaster);
        //    panelCMRIMaster.Controls.Add(cmriMasterControl);

        //    lngscCMRIMaster.EnableButton = false;  //CAB Search Control
        //    lngscCMRIMaster.EnableButtons();
        //    cmriMasterControl.Visible = true;
        //    dataGridCMRIMaster.Visible = false;
        //    cmriMasterControl.ClearData();
        //}

        //private void lngscCMRIMaster_OnDeleteClick(object sender, EventArgs e)
        //{
        //    cmriMasterEntity.CMRI_ID = 1;
        //    cmriMasterBLL.DeleteData(cmriMasterEntity);
        //}

        //private void lngscCMRIMaster_OnEditClick(object sender, EventArgs e)
        //{
        //    panelCMRIMaster.Controls.Remove(dataGridCMRIMaster);
        //    panelCMRIMaster.Controls.Add(cmriMasterControl);
        //    cmriMasterControl.EditData(cmriMasterEntity);
        //}


        //private void cmriMasterControl_OnSaveClick(object sender, EventArgs e)
        //{
        //    this.lngscCMRIMaster_OnCancelClick(this, null);
        //}


        //private void lngscCMRIMaster_OnCancelClick(object sender, EventArgs e)
        //{
        //    //this.lngscUser_OnSearchClick(this, null);
        //    //ucuserInformation.Visible = false;
        //    //grdUserUserInformation.Visible = true;
        //    //this.lngscUser_OnFindNowClick(this, null);
        //}

        //private void CMRIMasterForm_Load(object sender, EventArgs e)
        //{
        //    DisplayCMRIMasterData();
        //}

        //private void DisplayCMRIMasterData()
        //{
        //     cmriMasterBLL
        //}
    }

}
