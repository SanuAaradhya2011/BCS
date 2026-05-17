using System;
using System.Windows.Forms;
using CAB.BLL;
using CAB.Entity;
using CAB.UI.Controls;
using System.Collections.Generic;
using Hunt.EPIC.Logging;

namespace CAB.UI
{
    public partial class ManageGroups : MdiChildForm
    {
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(ManageGroups).ToString());
        GSMGroupBLL groupBLL = null;
        GSMTaskBLL gsmTaskBLL = null;
        public ManageGroups()
        {
            InitializeComponent();
            groupBLL = new GSMGroupBLL();
            gsmTaskBLL = new GSMTaskBLL();
        }

        private void ManageGroups_Load(object sender, EventArgs e)
        { }

        private void SetManageGroupsGrid()
        {
            grdManageGroups.HiddenColumn = "Group Type";
            
            grdManageGroups.Data = groupBLL.ListGroupData();
            grdManageGroups.SetWidth("Group ID", 75); 
            grdManageGroups.SetWidth("Group Name", 225);
            grdManageGroups.SetWidth("CommunicationType", 210); 
            
            
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                int groupID = 0;
                DataGridView dataGridView = (DataGridView)grdManageGroups.Controls["grdData"] as DataGridView;
                if (dataGridView != null)
                {
                    if (dataGridView.Rows.Count > 0)
                    {
                        if (int.TryParse(dataGridView.Rows[dataGridView.SelectedRows[0].Index].Cells[0].Value.ToString(), out groupID))
                        {
                            GSMGroupEntity gsmGroupEntity = (GSMGroupEntity)groupBLL.GetGroupDatabyGroupID(groupID) as GSMGroupEntity;
                            frmGSMGrouping gsmGrouping = new frmGSMGrouping();
                            gsmGrouping.Mode = "Edit";
                            gsmGrouping.GSMGroup = (GSMGroupEntity)groupBLL.GetGroupDatabyGroupID(groupID);
                            gsmGrouping.MdiParent = this.MdiParent;
                            gsmGrouping.Show();
                        }
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "btnEdit_Click(object sender, EventArgs e)", ex);
            }
        }

        public Boolean ActivateThisChild(String formName)
        {
            this.StatusMessage = string.Empty;
            int i;
            Boolean formSetToMdi = false;
            for (i = 0; i < this.MdiParent.MdiChildren.Length; i++)
            {
                if (this.MdiParent.MdiChildren[i].Name == formName)
                {
                    this.MdiParent.MdiChildren[i].Activate();
                    this.MdiParent.MdiChildren[i].Visible = true;
                    formSetToMdi = true;
                }
            }

            if (i == 0 || formSetToMdi == false)
                return false;
            else
                return true;
        }

        private void lngAddButton_Click(object sender, EventArgs e)
        {
            if (ActivateThisChild("frmGSMGrouping") == false)
            {
                frmGSMGrouping gsmGrouping = new frmGSMGrouping();
                gsmGrouping.Mode = "Add";
                gsmGrouping.Text = "Create Group ";
                gsmGrouping.MdiParent = this.MdiParent;
                gsmGrouping.Show();
            }
        }

        private void ManageGroups_Activated(object sender, EventArgs e)
        {
            SetManageGroupsGrid();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int groupID = 0;
            DataGridView dataGridView = (DataGridView)grdManageGroups.Controls["grdData"] as DataGridView;
            if (dataGridView.SelectedRows.Count > 0)
            {
                if (!CABMessageBox.ShowFilterMessage("M000120", "A000001", MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.No))
                {

                    if (int.TryParse(dataGridView.Rows[dataGridView.SelectedRows[0].Index].Cells[0].Value.ToString(), out groupID))
                    {
                        string result = groupBLL.DeleteGroup(groupID);

                        if (result == "Success")
                        {
                            SetManageGroupsGrid();
                            //Delete All tasks associated with deleted group including completed tasks.
                            List<GSMTaskEntity> colGSMTaskEntity = gsmTaskBLL.GetTasksByGroupId(groupID);
                            gsmTaskBLL.deleteGSMTasks(colGSMTaskEntity);
                            gsmTaskBLL.DeleteCompletedTasks(colGSMTaskEntity,groupID);
                            
                        }
                        else if (result == "Exist")
                        {
                            MessageBox.Show("This group is currently running under one or more tasks, hence cannot be deleted.");
                        }
                    }


                }
            }
            else
            {
                MessageBox.Show(CABApplication.Properties.Resources.SELECTAGROUPTODELETE, CABApplication.Properties.Resources.BCS);
            }


            
          
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}