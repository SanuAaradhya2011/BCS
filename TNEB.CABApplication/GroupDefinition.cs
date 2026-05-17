using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CAB.UI.Controls;
using CAB.BLL;
using CAB.IECFramework.Utility;

namespace CAB.UI
{
	public partial class GroupDefinition : MdiChildForm
	{
		public GroupDefinition()
		{
			InitializeComponent();
		}

		private Dictionary<int, string> groupDefinitionList;
		private int groupID;
		private string groupName = string.Empty;
		private string subGroupName = string.Empty;
		private string description = string.Empty;
		GroupDefinitionBLL groupDefinitionBLL = new GroupDefinitionBLL();
		DataSet dSet = null;

		private void ucSearchControl_OnAddClick(object sender, EventArgs e)
		{
			this.StatusMessage = string.Empty;
			Application.DoEvents();
			if (tvGroup.SelectedNode == null)
			{
				CABMessageBox.ShowFilterMessage("M000090", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}
			else
			{
				//getting the Key value for the selected treeview 
				foreach (KeyValuePair<int, string> kvp in groupDefinitionList)
				{
					if (kvp.Value == tvGroup.SelectedNode.Text)
					{
						groupID = kvp.Key;
						groupName = kvp.Value;
					}
				}
			}
			if (ActivateThisChild("NewGroupDefinition") == false)
			{
				NewGroupDefinition newGroupDefinition = new NewGroupDefinition();
				//newGroupDefinition.On_StatusChanged += new IsStatusChanged(this.MainForm_OnStatusChanged);
				newGroupDefinition.groupID = groupID;
				newGroupDefinition.groupName = groupName;
				newGroupDefinition.Text = "New Group Definition";
				newGroupDefinition.MdiParent = this.MdiParent;
				newGroupDefinition.Show();
			}
		}

		private void ucSearchControl_OnEditClick(object sender, EventArgs e)
		{
			this.StatusMessage = string.Empty;
			Application.DoEvents();
			if (tvGroup.SelectedNode == null)
			{
				CABMessageBox.ShowFilterMessage("M000090", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}
			else
			{
				if (tvGroup.SelectedNode.Parent == null) return;
				//getting the Key value for the selected treeview 
				foreach (KeyValuePair<int, string> kvp in groupDefinitionList)
				{
					if (tvGroup.SelectedNode.Parent.Text != null)
					{
						if (kvp.Value == tvGroup.SelectedNode.Parent.Text)
						{
							groupID = kvp.Key;
							groupName = kvp.Value;
						}
					}
				}
				if (tvGroup.SelectedNode != null)
				{
					if (tvGroup.SelectedNode.Text != null)
					{
						subGroupName = tvGroup.SelectedNode.Text;
					}
					description = groupDefinitionBLL.GetDescriptionForSubGroupName(subGroupName);
				}
			}

			if (ActivateThisChild("NewGroupDefinition") == false)
			{
				NewGroupDefinition newGroupDefinition = new NewGroupDefinition();
				newGroupDefinition.groupID = groupID;
				newGroupDefinition.groupName = groupName;
				newGroupDefinition.subGroupName = subGroupName;
				newGroupDefinition.description = description;
				newGroupDefinition.Text = "Edit Group Definition";
				newGroupDefinition.btnSave.Text = "Update";
				newGroupDefinition.MdiParent = this.MdiParent;
				newGroupDefinition.Show();
			}
		}

		private void ucSearchControl_OnDeleteClick(object sender, EventArgs e)
		{
			this.StatusMessage = string.Empty;
			Application.DoEvents();

			if (tvGroup.SelectedNode == null)
			{
				CABMessageBox.ShowFilterMessage("M000090", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				tvGroup.Focus();
				return;
			}
			else if (tvGroup.SelectedNode.Text == "All")
			{
				CABMessageBox.ShowFilterMessage("M000096", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				tvGroup.Focus();
				return;
			}
			else
			{
				if (tvGroup.SelectedNode.Parent == null) return;
				if (CABMessageBox.ShowMessage("M000025", "A000001", MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.No)) return;

				foreach (KeyValuePair<int, string> kvp in groupDefinitionList)
				{
					if (kvp.Value == tvGroup.SelectedNode.Parent.Text)
					{
						this.groupID = kvp.Key;
						this.groupName = kvp.Value;
					}
				}
				if (this.groupID != 0)
				{
					subGroupName = tvGroup.SelectedNode.Text;

					if (groupDefinitionBLL.DeleteSubGroupMeterMasterValues(this.groupID, subGroupName))
					{
						int subGroupID = groupDefinitionBLL.GetSubGroupID(this.groupID, subGroupName);
						if (groupDefinitionBLL.DeletegroupMasterValues(subGroupID))
						{
							this.StatusMessage = "Data Deleted Successfully";
							Application.DoEvents();
							FillTheTree();
						}
					}
				}
			}
		}

		private void GroupDefinition_Load(object sender, EventArgs e)
		{
			this.StatusMessage = string.Empty;
			Application.DoEvents();
			groupDefinitionBLL.CheckForAllInSuspectedConsumers();
			groupDefinitionList = new Dictionary<int, string>();
			dSet = new DataSet();
			dSet = groupDefinitionBLL.GetAllGroupMasterData();
			foreach (DataRow drow in dSet.Tables[0].Rows)
			{
				groupDefinitionList.Add(Convert.ToInt16(drow[0].ToString()), drow[1].ToString());
				tvGroup.Nodes.Add(drow[1].ToString());
			}
			FillTheTree();
		}

		private void FillTheTree()
		{
			int treeCount = 0;
			DataSet dSet = new DataSet();

			foreach (KeyValuePair<int, string> kvp in groupDefinitionList)
			{
				//Adding the root node and then adding the child nodes to the parent
				TreeNode rootNode = tvGroup.Nodes[treeCount++];
				foreach (TreeNode tn in rootNode.Nodes)
				{
					rootNode.Nodes.Remove(tn);
				}
				groupID = kvp.Key;
				dSet = GetGroupNameList(this.groupID);
				if (dSet == null)
				{
					return;
				}
				else if (dSet.Tables.Count == 0)
				{
					return;
				}
				rootNode.Nodes.Clear();
				foreach (DataRow drow in dSet.Tables[0].Rows)
				{
					rootNode.Nodes.Add(drow[0].ToString());
				}
			}
		}

		private DataSet GetGroupNameList(int groupID)
		{
			return groupDefinitionBLL.GetGroupNameList(groupID);
		}

		private void GroupDefinition_Activated(object sender, EventArgs e)
		{
			this.StatusMessage = "";
			Application.DoEvents();

			if (tvGroup.Nodes != null)
			{
				FillTheTree();
			}
		}

		private void tvGroup_AfterSelect(object sender, TreeViewEventArgs e)
		{
			if (tvGroup.SelectedNode != null)
			{
				if (tvGroup.SelectedNode.Parent == null)
				{
					ucSearchControl.EnableEdit = false;
					ucSearchControl.EnableDelete = false;
					lngGCGroupDefinition.Data = null;
					return;
				}

				foreach (KeyValuePair<int, string> kvp in groupDefinitionList)
				{
					if (tvGroup.SelectedNode.Parent == null) return;
					if (kvp.Value == tvGroup.SelectedNode.Parent.Text)
					{
						groupID = kvp.Key;
						groupName = kvp.Value;
					}
				}

				if (tvGroup.SelectedNode.Parent.Text != "")
				{
					subGroupName = tvGroup.SelectedNode.Text;
				}
				else
				{
					ucSearchControl.EnableEdit = false;
					ucSearchControl.EnableDelete = false;
				}
				if (tvGroup.SelectedNode.Text != "All")
				{
					ucSearchControl.EnableEdit = true;
					ucSearchControl.EnableDelete = true;
					lngGCGroupDefinition.Data = convertDataSet(groupDefinitionBLL.GetAllGroupMeterValues(groupID, subGroupName));
                    lngGCGroupDefinition.IsSorting = false;
                }
				else
				{
					ucSearchControl.EnableEdit = false;
					ucSearchControl.EnableDelete = false;
					lngGCGroupDefinition.Data = convertDataSet(groupDefinitionBLL.GetAllSuspectedMeterValues());
                    lngGCGroupDefinition.IsSorting = false;
                }
			}
		}

		private DataSet convertDataSet(DataSet dSet)
		{
			if (dSet == null || dSet.Tables.Count == 0 || dSet.Tables[0].Rows.Count == 0)
			{
				return null;
			}
			DataColumn dCol = new DataColumn("Installation Date");
			dCol.DataType = System.Type.GetType("System.String");

			dSet.Tables[0].Columns.Add(dCol);
			foreach (DataRow dr in dSet.Tables[0].Rows)
			{
				string dateFormatted = DateUtility.LongToStringDateFormat(Convert.ToInt64(dr["Meter Allocation Date"]));
				dr["Installation Date"] = dateFormatted;
			}
			dSet.Tables[0].Columns.Remove("Meter Allocation Date");
			dSet.AcceptChanges();
			return dSet;
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
	}
}
