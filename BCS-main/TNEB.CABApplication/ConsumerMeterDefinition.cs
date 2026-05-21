/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon 								|
 * | 																												|
 * |											Author : Piyush Singh. 	 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| 
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CAB.BLL;
using CAB.UI.Controls;
using CAB.Entity;
using CAB.IECFramework.Utility;
using System.Collections;

namespace CAB.UI
{
	public partial class ConsumerMeterDefinition : MdiChildForm
	{
		DataSet dataSet = null;
		//BLL
		ConsumerMeterDefinitionBLL consumerMeterDefinitionBLL;
		ConsumerMeterBLL consumerMeterBLL;
		MeterMasterBLL meterMasterBLL;
		ConsumerMasterBLL consumerMasterBLL;
		SuspectedConsumerBLL suspectedConsumerBLL;
        GSMGroupScheduleBLL gSMGroupScheduleBLL;
		//Entities
		ConsumerMeterEntity consumerMeterEntity;
		MeterMasterEntity meterMasterEntity;
		ConsumerMasterEntity consumerMasterEntity;
		SuspectedConsumerEntity suspectedConsumerEntity;

		public ConsumerMeterDefinition()
		{
			consumerMeterDefinitionBLL = new ConsumerMeterDefinitionBLL();
			consumerMeterBLL = new ConsumerMeterBLL();
			meterMasterBLL = new MeterMasterBLL();
			consumerMasterBLL = new ConsumerMasterBLL();
			suspectedConsumerBLL = new SuspectedConsumerBLL();
            gSMGroupScheduleBLL = new GSMGroupScheduleBLL();

			consumerMeterEntity = new ConsumerMeterEntity();
			meterMasterEntity = new MeterMasterEntity();
			consumerMasterEntity = new ConsumerMasterEntity();
			suspectedConsumerEntity = new SuspectedConsumerEntity();
			dataSet = new DataSet();

			InitializeComponent();
		}

		private void tabPageActive_Enter(object sender, EventArgs e)
		{
			dataSet = null;
			grdActiveMeter.DataSource = null;
			dataSet =  consumerMeterDefinitionBLL.GetActiveMeterData() ;
			if ((dataSet != null) && (dataSet.Tables.Count > 0) && (dataSet.Tables[0].Rows.Count != 0))
			{
				grdActiveMeter.DataSource = dataSet.Tables[0];
                if (grdActiveMeter.Columns.Count > 0)
                    for (int i = 0; i < grdActiveMeter.Columns.Count; i++)
                        grdActiveMeter.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
				ucSearchControl.EnableEdit = true;
				ucSearchControl.EnableDelete = true;
			}
			else
				ucSearchControl.EnableEdit = false;
		}

		private void tabPageDeactive_Enter(object sender, EventArgs e)
		{
			dataSet = null;
			grdInactiveMeter.DataSource = null;
			dataSet = consumerMeterDefinitionBLL.GetInActiveMeterData();
			if ((dataSet != null) && (dataSet.Tables.Count > 0) && (dataSet.Tables[0].Rows.Count != 0))
			{
				if (dataSet.Tables[0].Rows.Count > 0)
				{
					grdInactiveMeter.DataSource = dataSet.Tables[0];
                    if (grdInactiveMeter.Columns.Count > 0)
                        for (int i = 0; i < grdInactiveMeter.Columns.Count; i++)
                            grdInactiveMeter.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
					ucSearchControl.EnableDelete = true;
				}

				ucSearchControl.EnableEdit = false;
			}
			else
			{
				ucSearchControl.EnableEdit = false;
				ucSearchControl.EnableDelete = false;
			}
		}

		private void tabPageFreeConsumer_Enter(object sender, EventArgs e)
		{
			dataSet = null;
			grdFreeConsumer.DataSource = null;
			dataSet = convertDataSet(consumerMeterDefinitionBLL.GetFreeConsumerData());
			if ((dataSet != null) && (dataSet.Tables.Count > 0) && (dataSet.Tables[0].Rows.Count != 0))
			{
				grdFreeConsumer.DataSource = dataSet.Tables[0];
                if (grdFreeConsumer.Columns.Count > 0)
                    for (int i = 0; i < grdFreeConsumer.Columns.Count; i++)
                        grdFreeConsumer.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
				ucSearchControl.EnableDelete = true;
				ucSearchControl.EnableEdit = true;
			}
			else
			{
				ucSearchControl.EnableDelete = false;
				ucSearchControl.EnableEdit = false;
			}
		}

		private void ucSearchControl_OnAddClick(object sender, EventArgs e)
        {
			if (ActivateThisChild("ConsumerMeterDetails") == false)
			{
				ConsumerMeterDetails consumerMeterDetails = new ConsumerMeterDetails();
                consumerMeterDetails.MeterID = "";
                consumerMeterDetails.ConsumerID = "";
                consumerMeterDetails.Mode = "Add";
				consumerMeterDetails.Text = "New Consumer Meter Details";
				consumerMeterDetails.MdiParent = this.MdiParent;
				consumerMeterDetails.Show();
			}
		}

		private void ucSearchControl_OnEditClick(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            Application.DoEvents();
			if (tabControlConsumerMeter.SelectedTab == this.tabPageActive)
			{
				if (grdActiveMeter.Rows.Count > 0)
				{
					string consumer_ID = grdActiveMeter.Rows[grdActiveMeter.CurrentRow.Index].Cells[0].Value.ToString();
					string meter_ID = grdActiveMeter.Rows[grdActiveMeter.CurrentRow.Index].Cells[1].Value.ToString();
					ConsumerMeterDetails consumerMeterDetails = new ConsumerMeterDetails();
                    consumerMeterDetails.MeterID = meter_ID;
                    consumerMeterDetails.ConsumerID = consumer_ID;
                    consumerMeterDetails.Mode = "Edit";
					consumerMeterDetails.Text = "Edit Consumer Meter Details";
					consumerMeterDetails.MdiParent = this.MdiParent;
					consumerMeterDetails.Show();
				}
			}
			else if (tabControlConsumerMeter.SelectedTab == this.tabPageFreeConsumer)
			{
				if (grdFreeConsumer.Rows.Count > 0)
				{
					string consumer_ID = grdFreeConsumer.Rows[grdFreeConsumer.CurrentRow.Index].Cells[0].Value.ToString();
					ConsumerMeterDetails consumerMeterDetails = new ConsumerMeterDetails();
                    consumerMeterDetails.MeterID = "";
                    consumerMeterDetails.ConsumerID = consumer_ID;
                    consumerMeterDetails.Mode = "FreeConsumer";
					consumerMeterDetails.MdiParent = this.MdiParent;
					consumerMeterDetails.Show();
				}
			}
		}

		private void ucSearchControl_OnDeleteClick(object sender, EventArgs e)
		{
			if (tabControlConsumerMeter.SelectedTab == this.tabPageActive)
			{
				if (grdActiveMeter.Rows.Count == 0) return;
			}
			else if (tabControlConsumerMeter.SelectedTab == this.tabPageDeactive)
			{
				if (grdInactiveMeter.Rows.Count == 0) return;
			}
			else
			{
				if (grdFreeConsumer.Rows.Count == 0) return;
			}

            this.StatusMessage = string.Empty;
            Application.DoEvents();
            string meterID = "0";
            if(grdActiveMeter.Rows.Count>0)
              meterID = grdActiveMeter.Rows[grdActiveMeter.CurrentRow.Index].Cells[1].Value.ToString();
            DataSet dSet = gSMGroupScheduleBLL.GetSearchData("Meter_ID", meterID);
            if (dSet.Tables[0].Rows.Count > 0)
            {
                MessageBox.Show("Meter ID already assigned in GSM Schedule.Cannot be deleted.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            if (CABMessageBox.ShowFilterMessage("M000093", "A000001", MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.No))
            {
                return;
            }
			if (tabControlConsumerMeter.SelectedTab == this.tabPageActive)
			{
				if (grdActiveMeter.Rows.Count > 0)
				{
					string consumer_ID = grdActiveMeter.Rows[grdActiveMeter.CurrentRow.Index].Cells[0].Value.ToString();
					consumerMeterEntity.Consumer_Number = consumer_ID;
					consumerMeterEntity.Meter_ID = meterID;
					consumerMeterBLL.DeleteData(consumerMeterEntity);
					meterMasterEntity.Meter_ID = meterID;
					meterMasterBLL.DeleteData(meterMasterEntity);
					suspectedConsumerEntity.Consumer_Number = consumer_ID;
					consumerMeterDefinitionBLL.DeleteSuspectedConsumerData(suspectedConsumerEntity); 
					if (!(consumerMeterBLL.GetConsumerAvailable(consumer_ID)))
					{
						consumerMasterEntity.Consumer_Number = consumer_ID;
						consumerMasterBLL.DeleteData(consumerMasterEntity);
					}

					tabPageActive_Enter(sender, e);
				}
			}
			else if (tabControlConsumerMeter.SelectedTab == this.tabPageDeactive)
			{
				if (grdInactiveMeter.Rows.Count > 0)
				{
					string meter_ID = grdInactiveMeter.Rows[grdInactiveMeter.CurrentRow.Index].Cells[0].Value.ToString();
					meterMasterEntity.Meter_ID = meter_ID;
                    meterMasterEntity.Meter_Status = 0;
					meterMasterBLL.DeleteData(meterMasterEntity);
					meterMasterBLL.DeleteMeterData(meterMasterEntity);
					tabPageDeactive_Enter(sender, e);
				}
			}
			else
			{
				if (grdFreeConsumer.Rows.Count > 0)
				{
					string consumer_ID = grdFreeConsumer.Rows[grdFreeConsumer.CurrentRow.Index].Cells[0].Value.ToString();
					consumerMasterEntity.Consumer_Number = consumer_ID;
					//Delete the consumer Number from the suspected Consumer and then delete from the consumer Master
					suspectedConsumerBLL.DeleteData(suspectedConsumerEntity);
					consumerMasterBLL.DeleteData(consumerMasterEntity);
					tabPageFreeConsumer_Enter(sender, e);
				}
			}
		}

        private DataSet convertDataSet(DataSet dSet)
        {
            Hashtable htable = new Hashtable();
            ArrayList duplicateList = new ArrayList();
            foreach (DataRow dr in dSet.Tables[0].Rows)
            {
                if (htable.Contains(dr["Consumer Name"]))
                    duplicateList.Add(dr);
                else
                    htable.Add(dr["Consumer Name"], string.Empty);
            }
            foreach (DataRow dr in duplicateList)
                dSet.Tables[0].Rows.Remove(dr);
            dSet.AcceptChanges();
            return dSet;
        }

        private void ConsumerMeterDefinition_Activated(object sender, EventArgs e)
        {  
            this.StatusMessage = string.Empty;
            Application.DoEvents();
            DataSet dataset = consumerMeterDefinitionBLL.GetActiveMeterData();
            if (dataset.Tables[0].Rows.Count == 0)
            {
                ucSearchControl.ShowAddOption();
            }
			else
			{
				ucSearchControl.EnableMainControls();
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

		private void ucDetail_OnControlStatusChanged(string msg)
		{
			this.StatusMessage = msg;
			Application.DoEvents();
		}
	}
}
        
       

      

       

