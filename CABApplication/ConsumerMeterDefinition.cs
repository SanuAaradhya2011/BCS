/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 
 * |											Date   : 25 March 2010
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| 
 */
using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Windows.Forms;
using CAB.BLL;
using CAB.Entity;
using CAB.UI.Controls;
using System.ComponentModel;

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
            if (ucSearchControl.Controls.ContainsKey("btnDeleteAll"))
            {
                ((CAB.UI.Controls.CABButton)ucSearchControl.Controls.Find("btnDeleteAll", false)[0]).Visible = true;
            }
            dataSet = consumerMeterDefinitionBLL.GetActiveMeterData();
            if ((dataSet != null) && (dataSet.Tables.Count > 0) && (dataSet.Tables[0].Rows.Count != 0))
            {
                //To make sure that sorting by consumerId(integer) is done properly
                DataTable cloneTable = dataSet.Tables[0].Clone();
                // Need to change for PED Requirement : Consumer ID aplhaneumeric
                //cloneTable.Columns["Consumer ID"].DataType = typeof(long);
                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    cloneTable.ImportRow(row);
                }
                grdActiveMeter.DataSource = cloneTable;
                //VBM- Sort by consumer  id  # 151117
                grdActiveMeter.Sort(this.grdActiveMeter.Columns["Consumer ID"],
                                    ListSortDirection.Ascending);
                DisableSortingForConsumerGrid(grdActiveMeter);

                ucSearchControl.EnableEdit = true;
                ucSearchControl.EnableDeleteAll = true;
                ucSearchControl.EnableDelete = true;
            }
            else
            {
                ucSearchControl.EnableEdit = false;
                ucSearchControl.EnableDeleteAll = false;
                ucSearchControl.EnableDelete = false;
            }
        }

        private void tabPageDeactive_Enter(object sender, EventArgs e)
        {
            dataSet = null;
            grdInactiveMeter.DataSource = null;
            if ( ucSearchControl.Controls.ContainsKey("btnDeleteAll"))
            {
                ((CAB.UI.Controls.CABButton)ucSearchControl.Controls.Find("btnDeleteAll", false)[0]).Visible = true;
            }
            ucSearchControl.EnableDeleteAll = false;
            dataSet = consumerMeterDefinitionBLL.GetInActiveMeterData();
            if ((dataSet != null) && (dataSet.Tables.Count > 0) && (dataSet.Tables[0].Rows.Count != 0))
            {
                if (dataSet.Tables[0].Rows.Count > 0)
                {
                    grdInactiveMeter.DataSource = dataSet.Tables[0];                    
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
            if (ucSearchControl.Controls.ContainsKey("btnDeleteAll"))
            {
                ((CAB.UI.Controls.CABButton)ucSearchControl.Controls.Find("btnDeleteAll", false)[0]).Visible = true;
            }
            ucSearchControl.EnableDeleteAll = false;
            dataSet = convertDataSet(consumerMeterDefinitionBLL.GetFreeConsumerData());
            if ((dataSet != null) && (dataSet.Tables.Count > 0) && (dataSet.Tables[0].Rows.Count != 0))
            {

                grdFreeConsumer.DataSource = dataSet.Tables[0];               
                ucSearchControl.EnableDelete = true;
                ucSearchControl.EnableEdit = true;
            }
            else
            {
                ucSearchControl.EnableDelete = false;
                ucSearchControl.EnableEdit = false;
            }
        }

       /// <summary>
        ///  Used to disable sorting for all columns except Consumerid column.
       /// </summary>
       /// <param name="consumerGrid"></param>
        private void DisableSortingForConsumerGrid(DataGridView consumerGrid)
        {
            foreach (DataGridViewColumn column in consumerGrid.Columns)
            {
                if (column.Name != "Consumer ID")
                {
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
                }
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
            if (ActivateThisChild("ConsumerMeterDetails") == false)
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
                        consumerMeterDetails.GPRSIMEI = grdActiveMeter.Rows[grdActiveMeter.CurrentRow.Index].Cells["Modem IMEI"].Value.ToString();
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
            if (grdActiveMeter.Rows.Count > 0)
                meterID = grdActiveMeter.Rows[grdActiveMeter.CurrentRow.Index].Cells[1].Value.ToString();
            GSMTaskBLL objGSMTaskBLL = new GSMTaskBLL();
            if (objGSMTaskBLL.DoesActiveTaskExistsForMeterID(meterID))
            {
                MessageBox.Show("Meter ID already assigned in active task(s). Cannot be deleted.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Stop);
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

                    //If GPRS is enabled then only try remove the endpoint from m2m.
                    // to enable syncing of endpoints changes to GPRS Adapter
                    if (!String.IsNullOrEmpty(grdActiveMeter.Rows[grdActiveMeter.CurrentRow.Index].Cells["Modem IMEI"].Value.ToString()))
                    {
                        LandisGyr.AMI.Layers.Endpoint[] endPoints = new LandisGyr.AMI.Layers.Endpoint[1];
                        endPoints[0] = new LandisGyr.AMI.Layers.Endpoint();
                        endPoints[0].Model = new LandisGyr.AMI.Layers.EndpointModel();
                        endPoints[0].Model.Type = LandisGyr.AMI.Layers.EndpointType.GPRS;
                        endPoints[0].SerialNumber = grdActiveMeter.Rows[grdActiveMeter.CurrentRow.Index].Cells["Modem IMEI"].Value.ToString();
                        GPRSCommunication.EndPointOperations.RemoveEndpoints(endPoints);
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

                    //If GPRS is enabled for utility then only try remove the endpoint from m2m.
                    //if (UtilityDetails.ShowGPRSCommunication)
                    //{
                        // to enable syncing of endpoints changes to GPRS Adapter
                        if (!String.IsNullOrEmpty(grdInactiveMeter.Rows[grdInactiveMeter.CurrentRow.Index].Cells["Modem IMEI"].Value.ToString()))
                        {
                            LandisGyr.AMI.Layers.Endpoint[] endPoints = new LandisGyr.AMI.Layers.Endpoint[1];
                            endPoints[0] = new LandisGyr.AMI.Layers.Endpoint();
                            endPoints[0].Model = new LandisGyr.AMI.Layers.EndpointModel();
                            endPoints[0].Model.Type = LandisGyr.AMI.Layers.EndpointType.GPRS;
                            endPoints[0].SerialNumber = grdInactiveMeter.Rows[grdInactiveMeter.CurrentRow.Index].Cells["Modem IMEI"].Value.ToString();
                            GPRSCommunication.EndPointOperations.RemoveEndpoints(endPoints);

                        }
                    //}
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
            foreach (DataGridViewColumn column in grdActiveMeter.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            foreach (DataGridViewColumn column in grdInactiveMeter.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            foreach (DataGridViewColumn column in grdFreeConsumer.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
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

        private void ucSearchControl_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// Delete All Consumers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ucSearchControl_OnDeleteAllClick(object sender, EventArgs e)
        {

            if (grdActiveMeter.Rows.Count > 0)
            {
                this.StatusMessage = string.Empty;
                Application.DoEvents();
                string meterID = string.Empty;
                StringBuilder strMeterIds = new StringBuilder();
                for (int activeMeterRowCount = 0; activeMeterRowCount < grdActiveMeter.Rows.Count; activeMeterRowCount++)
                {
                    meterID = grdActiveMeter.Rows[activeMeterRowCount].Cells["Meter ID"].Value.ToString();
                    GSMTaskBLL objGSMTaskBLL = new GSMTaskBLL();
                    if (objGSMTaskBLL.DoesActiveTaskExistsForMeterID(meterID))
                    {
                        if (strMeterIds.Length == 0)
                        {
                            strMeterIds.Append(meterID);
                        }
                        else
                        {
                            strMeterIds.Append(",");
                            strMeterIds.Append(meterID);
                        }

                    }

                }
                if (strMeterIds.Length > 0)
                {                    
                    if (MessageBox.Show("Meter ID(s)  " + strMeterIds.ToString() + "  already assigned in active task(s).\n" + "Are you sure you want to delete record(s)?", "BCS", MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.No))
                    {
                        return;
                    }
                }

                if (MessageBox.Show("Are you sure you want to delete record(s)?", "BCS", MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.Yes))
                {
                    for (int activeMeterRowCount = 0; activeMeterRowCount < grdActiveMeter.Rows.Count; activeMeterRowCount++)
                    {

                        string consumerId = grdActiveMeter.Rows[activeMeterRowCount].Cells["Consumer ID"].Value.ToString();
                        string MeterId = grdActiveMeter.Rows[activeMeterRowCount].Cells["Meter ID"].Value.ToString();
                        consumerMeterEntity.Consumer_Number = consumerId;
                        consumerMeterEntity.Meter_ID = MeterId;
                        consumerMeterBLL.DeleteData(consumerMeterEntity);
                        meterMasterEntity.Meter_ID = MeterId;
                        meterMasterBLL.DeleteMeterData(meterMasterEntity);
                        suspectedConsumerEntity.Consumer_Number = consumerId;
                        consumerMeterDefinitionBLL.DeleteSuspectedConsumerData(suspectedConsumerEntity);
                        if (!(consumerMeterBLL.GetConsumerAvailable(consumerId)))
                        {
                            consumerMasterEntity.Consumer_Number = consumerId;
                            consumerMasterBLL.DeleteData(consumerMasterEntity);
                        }

                    }
                    tabPageActive_Enter(sender, e);
                }

            }
        }

       
    }
}
        
       

      

       

