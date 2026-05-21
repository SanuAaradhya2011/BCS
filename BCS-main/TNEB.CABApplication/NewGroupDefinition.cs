using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CAB.IECFramework.Utility;
using CAB.Entity;
using CAB.IECFramework.Entity;
using CAB.UI.Controls;
using CAB.BLL;

namespace CAB.UI
{
	public partial class NewGroupDefinition : MdiChildForm
	{
		GroupMasterEntity groupMasterEntity;
		SubGroupMasterEntity subGroupMasterEntity;
		SubGroupMeterMasterEntity subGroupMeterMasterEntity;
		GroupDefinitionBLL groupDefinitionBLL;

		MainForm mainForm = (MainForm)Application.OpenForms["MainForm"];

		public int groupID { get; set; }
		public string groupName { get; set; }
		public string subGroupName { get; set; }
		public string description { get; set; }

		public NewGroupDefinition()
		{
			InitializeComponent();
			groupMasterEntity = new GroupMasterEntity();
			subGroupMasterEntity = new SubGroupMasterEntity();
			subGroupMeterMasterEntity = new SubGroupMeterMasterEntity();
			groupDefinitionBLL = new GroupDefinitionBLL();
		}

		private void lngButton1_Click(object sender, EventArgs e)
		{
			try
			{
				if (listBoxAvailableMeters.SelectedItems.Count == 0) { return; }
				foreach (object item in listBoxAvailableMeters.SelectedItems)
				{
					listBoxSelectedMeters.Items.Add(item);
				}
				foreach (object item in listBoxSelectedMeters.Items)
				{
					listBoxAvailableMeters.Items.Remove(item);
				}
				if (listBoxAvailableMeters.Items.Count >= 1)
				{ listBoxAvailableMeters.SelectedIndex = 0; }
				listBoxAvailableMeters.Focus();
			}
			catch (Exception)
			{
			}
		}

		private void lngButton2_Click(object sender, EventArgs e)
		{
			try
			{
				int count = 0;
				while (count < listBoxAvailableMeters.Items.Count)
				{
					listBoxSelectedMeters.Items.Add(listBoxAvailableMeters.Items[count]);
					count++;
				}
				listBoxAvailableMeters.Items.Clear();
				if (listBoxSelectedMeters.Items.Count > 0) listBoxSelectedMeters.SelectedIndex = 0;
			}
			catch (Exception)
			{
			}
		}

		private void lngButton3_Click(object sender, EventArgs e)
		{
			try
			{
				if (listBoxSelectedMeters.SelectedItems.Count == 0) { return; }
				foreach (object item in listBoxSelectedMeters.SelectedItems)
				{
					listBoxAvailableMeters.Items.Add(item);
				}
				foreach (object item in listBoxAvailableMeters.Items)
				{
					listBoxSelectedMeters.Items.Remove(item);
					if (listBoxAvailableMeters.Items.Count == 0) { return; }
				}
				if (listBoxSelectedMeters.Items.Count >= 1)
				{ listBoxSelectedMeters.SelectedIndex = 0; }
				listBoxSelectedMeters.Focus();
			}
			catch (Exception)
			{
			}
		}

		private void lngButton4_Click(object sender, EventArgs e)
		{
			try
			{
				int count = 0;
				while (count < listBoxSelectedMeters.Items.Count)
				{
					listBoxAvailableMeters.Items.Add(listBoxSelectedMeters.Items[count]);
					count++;
				}
				listBoxSelectedMeters.Items.Clear();
				if (listBoxAvailableMeters.Items.Count > 0) listBoxAvailableMeters.SelectedIndex = 0;
			}
			catch (Exception)
			{
			}
		}

		private bool ValidateData(IEntity entity)
		{
			bool Flag = false;
			subGroupMasterEntity = entity as SubGroupMasterEntity;
			
			if (!ValidationProvider.ValidateData(subGroupMasterEntity.SubGroup_Name.Trim(), ValidationConstant.groupName))
			{
				mainForm.toolStripStatusLabel.Text = "Data Saved Successfully";
				txtSubGroupName.Focus();
				Flag = false;
			}
			else
			{
				Flag = true;
			}
			return Flag;
		}

		private bool CheckSubGroupName(string subGroupName)
		{
			if (groupDefinitionBLL.GetSubGroupName(subGroupName))
				return true;
			return false;
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			if (btnSave.Text == "Update")
			{
				//Check for the Selected Meters in the list and if there are no meters selected then the message is thrown
				if (listBoxSelectedMeters.Items.Count == 0)
				{
					MessageBox.Show("Selected meters cannot be left blank", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					listBoxAvailableMeters.Focus();
					return;
				}

				//To insert the description if the description was changed during the update.
				subGroupMasterEntity.SubGroup_Description = this.txtDescription.Text.Trim();
				subGroupMasterEntity.Group_ID = this.groupID;
				subGroupMasterEntity.SubGroup_Name = this.subGroupName;
				groupDefinitionBLL.UpdateSubGroupMasterData(subGroupMasterEntity);//InsertSubGroupMasterData(subGroupMasterEntity);

				subGroupMeterMasterEntity.GroupAllocation_Date = DateTime.Now;
				//Deleting the older values for the subGroupID and then again inserting
				if (groupDefinitionBLL.DeleteSubGroupMeterMasterValues(this.groupID, this.subGroupName))
				{
					//Getting the already available groupID and then inserting into the subgroupMeter Master table
					int subGroupID = groupDefinitionBLL.GetSubGroupID(this.groupID, this.subGroupName);

					if (subGroupID > 0)
					{
						subGroupMeterMasterEntity.SubGroup_ID = subGroupID;
						List<string> meterDataList = new List<string>();
						for (int count = 0; count < listBoxSelectedMeters.Items.Count; count++)
						{
							meterDataList.Add(listBoxSelectedMeters.Items[count].ToString());
						}

						foreach (string tempStr in meterDataList)
						{
							subGroupMeterMasterEntity.Meter_ID = tempStr;
							groupDefinitionBLL.InsertSubGroupMeterMasterData(subGroupMeterMasterEntity);
						}
					}
				}
				mainForm.toolStripStatusLabel.Text = "Data Updated Successfully";
				this.Close();
			}
			else
			{
				int subGroupID = 0;
				string subGroupName = txtSubGroupName.Text.Trim();
				//Sub Group Name cannot be in the Name ALL
			
				if (string.IsNullOrEmpty(subGroupName))
				{
					MessageBox.Show("SubGroup Name cannot be blank", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning); 
					this.txtSubGroupName.Focus();
					return;
				}
				else if (subGroupName.ToUpper().ToString().Equals("ALL"))
				{
					MessageBox.Show("SubGroup Name All already exists cannot be added", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning); 
					//mainForm.toolStripStatusLabel.Text = "SubGroup Name All already exists cannot be added";

					return;
				}
				else if  (CheckSubGroupName(subGroupName))
				{
					MessageBox.Show("Subgroup Name Already Available", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					//mainForm.toolStripStatusLabel.Text = "Subgroup Name Already Available";
					this.txtSubGroupName.Focus();
					return;
				}
				subGroupMasterEntity.SubGroup_Name = txtSubGroupName.Text.Trim();
				subGroupMasterEntity.SubGroup_Description = txtDescription.Text.Trim();
				subGroupMasterEntity.Group_ID = this.groupID;
				subGroupMeterMasterEntity.GroupAllocation_Date = DateTime.Now;
				SubGroupMasterEntity subGroupEntity = null;

				if (ValidateData(subGroupMasterEntity))
				{
					//Check for the Selected Meters in the list and if there are no meters selected then the message is thrown
					if (listBoxSelectedMeters.Items.Count == 0)
					{
						MessageBox.Show("Selected meters cannot be left blank", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
						//mainForm.toolStripStatusLabel.Text = "Selected meters cannot be left blank";
						listBoxAvailableMeters.Focus();
						return;
					}

					//Insert the values in the SubGroup Master
					if (Convert.ToString(this.groupID) != string.Empty)
					{
						subGroupEntity = groupDefinitionBLL.InsertSubGroupMasterData(subGroupMasterEntity) as SubGroupMasterEntity;
					}

					//Get the SubGroup Id for the Inserted value then the value is used for the subgroup Meter Master table insert
					subGroupID = groupDefinitionBLL.GetSubGroupID(groupID, subGroupMasterEntity.SubGroup_Name);
					if (subGroupID > 0)
					{
						subGroupMeterMasterEntity.SubGroup_ID = subGroupID;
						List<string> meterDataList = new List<string>();
						for (int count = 0; count < listBoxSelectedMeters.Items.Count; count++)
						{
							meterDataList.Add(listBoxSelectedMeters.Items[count].ToString());
						}

						foreach (string tempStr in meterDataList)
						{
							subGroupMeterMasterEntity.Meter_ID = tempStr;
							groupDefinitionBLL.InsertSubGroupMeterMasterData(subGroupMeterMasterEntity);
						}
						mainForm.toolStripStatusLabel.Text = "Data Saved Successfully";
						this.Close();
					}
				}
			}
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private DataSet GetAssignedMeterID(int groupID, string subGroupName)
		{
			return groupDefinitionBLL.GetAssignedMeterID(groupID, subGroupName);
		}

		private void NewGroupDefinition_Load(object sender, EventArgs e)
		{
			MeterDataBLL meterDataBLL = new MeterDataBLL();

			this.txtGroupName.Text = this.groupName;
			txtSubGroupName.Text = this.subGroupName;

			listBoxSelectedMeters.Items.Clear();
			int subGroupID = groupDefinitionBLL.GetSubGroupID(groupID, subGroupName);

			DataSet dSet = new DataSet();

			dSet = meterDataBLL.ListUnAssignedMeterID(subGroupID);//ListAllMeterID();//
			listBoxAvailableMeters.Items.Clear();
			foreach (DataRow drow in dSet.Tables[0].Rows)
			{
				listBoxAvailableMeters.Items.Add(drow[0].ToString());
			}

			DataSet ds = new DataSet();
			ds = GetAssignedMeterID(groupID, subGroupName);
			foreach (DataRow dr in ds.Tables[0].Rows)
			{
				listBoxSelectedMeters.Items.Add(dr[0].ToString());
			}

			this.txtDescription.Text = string.Empty;
			if (this.Text.Contains("Edit"))
			{
				txtSubGroupName.ReadOnly = true;
				this.txtDescription.Text = this.description;
			}
		}
	}
}