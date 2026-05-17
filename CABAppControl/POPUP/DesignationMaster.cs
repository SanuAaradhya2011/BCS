using System;
using System.Windows.Forms;
using CAB.BLL;
using CAB.Entity;
using CAB.UI.Controls;


namespace CAB.UI.Controls
{
    public partial class DesignationMaster : Form
    {
        private DesignationMasterBLL designationMasterBLL = new DesignationMasterBLL();
        private DesignationMasterEntity designationMasterEntity;
        public DesignationMasterEntity designation
        {
            get;
            set;
        }
        public DesignationMaster()
        {
            InitializeComponent();
        }

        private void DivisionMaster_Load(object sender, EventArgs e)
        {

            this.Text = MessageConstant.GetText("M000037"); 
            this.pDivision.Visible = false;
            LoadGrid();
            designationMasterEntity = designationMasterBLL.GetDetailData(lgcDesignation.GetPrimaryValue()) as DesignationMasterEntity;  
            if (designationMasterEntity == null)
            {
                lngbEdit.Enabled = false;
                lngbDelete.Enabled = false;
                lngbPick.Visible = true;
                designationMasterEntity = null;
            }
        }
        private void LoadGrid()
        {
            lgcDesignation.HiddenColumn = "Designation_ID";
            lgcDesignation.ValueColumn = "Designation_ID"; 
            lgcDesignation.Data = designationMasterBLL.ListDataSet();
            lgcDesignation.SetWidth("SL. No", 70);
            lgcDesignation.SetWidth("Designation Name", 387);
            lgcDesignation.Visible = true;
            pDivision.Visible = false;
			lgcDesignation.RefreshGrid();
        }

	
        private void lngbAdd_Click(object sender, EventArgs e)
        {
            lblStatus.Text = string.Empty;
            this.txtCategory.Text = string.Empty;
            lgcDesignation.Visible = false;
            pDivision.Visible = true;
            designationMasterEntity = new DesignationMasterEntity();
            lngbEdit.Enabled = false;
            lngbDelete.Enabled = false;
            lngbPick.Visible = false;
            txtCategory.Focus();
        }

        private void lngbSave_Click(object sender, EventArgs e)
        {
            string designationN = this.txtCategory.Text.Trim();
            if (designationN.Equals(string.Empty))
            {
                lblStatus.Text = MessageConstant.GetText("M000036"); 
                txtCategory.Focus();
                return;
            }
            if (designationMasterEntity.Designation_ID == 0)
            {
                if ((designationMasterBLL.ValidateDesignation(designationN) as DesignationMasterEntity).Designation_ID != 0)
                {
                    lblStatus.Text = MessageConstant.GetText("M000035");
                    txtCategory.Focus();
                }
                else
                {
                    designationMasterEntity.Designation_Name = designationN;
                    designationMasterBLL.InsertData(designationMasterEntity);
                    RefreshData();
                }
            }
            else
            {
                DesignationMasterEntity designation=designationMasterBLL.ValidateDesignation(designationN) as DesignationMasterEntity;
                if (designation.Designation_ID != designationMasterEntity.Designation_ID && designation.Designation_ID != 0)
                {
                    lblStatus.Text = MessageConstant.GetText("M000035");
                    txtCategory.Focus();
                }
                else
                {
                    designationMasterEntity.Designation_Name = designationN;
                    designationMasterBLL.UpdateData(designationMasterEntity);
                    RefreshData();
                }
            }
        }
        private void RefreshData()
        {
            LoadGrid();
            if (lgcDesignation.GetPrimaryValue().Equals(string.Empty))
            {
                lngbEdit.Enabled = false;
                lngbDelete.Enabled = false;
                lngbPick.Visible = true;
            }
            else
            {
                lngbEdit.Enabled = true;
                lngbDelete.Enabled = true;
                lngbPick.Visible = true;
                lngbAdd.Enabled = true;
            }
            designationMasterEntity = null;
        }
        private void lngbCancel_Click(object sender, EventArgs e)
        {
             RefreshData();
             this.lblStatus.Text = string.Empty;
        }

        private void txtCategory_TextChanged(object sender, EventArgs e)
       {
            lblStatus.Text = string.Empty;
        }

        private void lngbEdit_Click(object sender, EventArgs e)
        { 
            designationMasterEntity=designationMasterBLL.GetDetailData(lgcDesignation.GetPrimaryValue()) as DesignationMasterEntity;
            if (designationMasterEntity == null)
                return;
            lblStatus.Text = string.Empty;
            this.txtCategory.Text = designationMasterEntity.Designation_Name;
            lgcDesignation.Visible = false;
            pDivision.Visible = true;
            lngbAdd.Enabled = false;
            lngbDelete.Enabled = false;
            lngbPick.Visible = false;
            txtCategory.Focus();
        }

        private void lngbDelete_Click(object sender, EventArgs e)
        {
            designationMasterEntity = designationMasterBLL.GetDetailData(lgcDesignation.GetPrimaryValue()) as DesignationMasterEntity;
            if (designationMasterEntity == null)
                return;
            if (CABMessageBox.ShowFilterMessage("M000025", "A000001", MessageBoxButtons.YesNo,MessageBoxIcon.Question).Equals(DialogResult.Yes))
            {
                designationMasterBLL.DeleteData(designationMasterEntity);
                RefreshData();
            }
        }

        private void lngbPick_Click(object sender, EventArgs e)
        {
             this.Close();
        }

        private void lgcCategory_OnGridRowChanged(string msg)
        { 
            this.lblStatus.Text = string.Empty;
        }

        private void lngBtnPick_Click(object sender, EventArgs e)
        {
            designation = designationMasterBLL.GetDetailData (lgcDesignation.GetPrimaryValue()) as DesignationMasterEntity;
            this.Close();
        }

        private void lgcDesignation_OnGridMouseDoubleClick(string KeyValue)
        {
            designation = designationMasterBLL.GetDetailData(lgcDesignation.GetPrimaryValue()) as DesignationMasterEntity;
            this.Close();
        }
    }
}
