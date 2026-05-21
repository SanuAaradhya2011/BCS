//Project   : TNEB - BCS 3 phase
//FILE NAME : DisplayParametes.cs
//Date      : August 4, 2011 
//Author    : Vivek Agrawal
/*Purpose   : DisplayParamaters class is responsible to handle various operations and settings
              regarding reading/writing of parameters in a meter.*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LTCTBLL;
using CAB.Entity;
using System.Xml;
using System.IO;

namespace CAB.UI.Controls
{
    #region Class - DisplayParamaters
    /// <summary>
    ///Code Region Added by Vivek on 3 August 2011 (TNEB Project)
    /// DisplayParamaters class is responsible to handle various operations and settings
    ///regarding reading/writing of parameters in a meter.
    /// </summary>
    public partial class DisplayParamaters : UserControl
    {
        
        #region Binding Paramter list to various datagrids.
        /// <summary>
       /// Binding Paramter list to various datagrids.
       /// </summary> 
        public DisplayParamaters()
        {
            InitializeComponent();
            if (UtilityDetails.UtilityName == IECUtilityEntity.TNEB || UtilityDetails.UtilityName == IECUtilityEntity.TNEB1)
            {
                //Set Parameters values in grids.
                if (dgridPushDisplayParams.Rows.Count == 0)
                    FillDisplayParameters();
            }
            else
                return;
        }
        /// <summary>
        /// Binding Paramter list to various datagrids.
        /// </summary>
        private void FillDisplayParameters()
        {
            XmlDataDocument xmlDatadoc = null;
            DataSet ds = null;
            try
            {//Read the Parameters from the XML file.
                xmlDatadoc = new XmlDataDocument();
                if(UtilityDetails.UtilityName == IECUtilityEntity.TNEB)
                
                xmlDatadoc.DataSet.ReadXml(string.Concat(Path.GetDirectoryName(Application.ExecutablePath), "\\", "DisplayParameters.xml"));
                if(UtilityDetails.UtilityName == IECUtilityEntity.TNEB1)
                    xmlDatadoc.DataSet.ReadXml(string.Concat(Path.GetDirectoryName(Application.ExecutablePath), "\\", "DisplayParameters_0.19.xml"));
                //deserialize xml data
                ds = xmlDatadoc.DataSet;

                //set datasource of gridview
                dgridPushDisplayParams.DataSource=ds.DefaultViewManager;
                dgridScrollDisplayParams.DataSource= ds.DefaultViewManager;
                dgridHighResolution.DataSource=ds.DefaultViewManager;

                //specify grdiview datamember
                dgridPushDisplayParams.DataMember = "PushDisplayParams";
                dgridScrollDisplayParams.DataMember = "ScrollDisplayParams";
                dgridHighResolution.DataMember = "HighResolution";
            }
            catch (Exception ex)
            { 
              MessageBox.Show(ex.Message);
            }
            finally
            {   //dispose and free memory occupied by objects
                ds.Dispose();
            }
                DataGridViewColumn chkboxColumn_PushParameters = new DataGridViewCheckBoxColumn();
                chkboxColumn_PushParameters.Name = "colInclude";
                chkboxColumn_PushParameters.HeaderText = "Include";
                dgridPushDisplayParams.Columns.Add(chkboxColumn_PushParameters);

                DataGridViewColumn chkboxColumn_ScrollParameters = new DataGridViewCheckBoxColumn();
                chkboxColumn_ScrollParameters.Name = "colInclude";
                chkboxColumn_ScrollParameters.HeaderText = "Include";
                dgridScrollDisplayParams.Columns.Add(chkboxColumn_ScrollParameters);
                

                DataGridViewColumn chkboxColumn_HighResolution = new DataGridViewCheckBoxColumn();
                chkboxColumn_HighResolution.Name = "colInclude";
                chkboxColumn_HighResolution.HeaderText = "Include";
                dgridHighResolution.Columns.Add(chkboxColumn_HighResolution);                

                dgridPushDisplayParams.Columns["SNo"].Width = dgridScrollDisplayParams.Columns["SNo"].Width = dgridHighResolution.Columns["SNo"].Width = 60;
                dgridPushDisplayParams.Columns["ID"].Width = dgridScrollDisplayParams.Columns["ID"].Width = dgridHighResolution.Columns["ID"].Width = 60;
                dgridPushDisplayParams.Columns["Description"].Width = dgridScrollDisplayParams.Columns["Description"].Width = dgridHighResolution.Columns["Description"].Width = 270;

                dgridPushDisplayParams.Columns[0].ReadOnly = dgridPushDisplayParams.Columns[1].ReadOnly =
                    dgridScrollDisplayParams.Columns[0].ReadOnly = dgridScrollDisplayParams.Columns[1].ReadOnly =
                    dgridHighResolution.Columns[0].ReadOnly = dgridHighResolution.Columns[1].ReadOnly = true;

                dgridPushDisplayParams.Columns[0].SortMode = dgridScrollDisplayParams.Columns[0].SortMode = dgridHighResolution.Columns[0].SortMode =
                    dgridPushDisplayParams.Columns[1].SortMode = dgridScrollDisplayParams.Columns[1].SortMode = dgridHighResolution.Columns[1].SortMode =
                        dgridPushDisplayParams.Columns["colInclude"].SortMode = dgridScrollDisplayParams.Columns["colInclude"].SortMode = dgridHighResolution.Columns["colInclude"].SortMode 
                    = DataGridViewColumnSortMode.NotSortable;

                dgridPushDisplayParams.Select();

        }
        #endregion

        #region Swapping Selected Row Values with previous/next row values
        /// <summary>
        /// Code Region Added by Vivek on 5 August 2011 (TNEB Project)
        /// Swapping values in selected row and previous row if present in datagrid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpScroll_Click(object sender, EventArgs e)
        {
            DataGridView dgridDisplayParams = GetSelectedDataGridView();
            int SelRow = dgridDisplayParams.CurrentRow.Index;
            if (SelRow > 0)
            {
                bool tempChkboxValue = (dgridDisplayParams[0, SelRow - 1].Value != null && (bool)dgridDisplayParams[0, SelRow - 1].Value) ? true : false;
                dgridDisplayParams.Rows[SelRow - 1].Cells[0].Value = dgridDisplayParams.CurrentRow.Cells[0].Value;
                dgridDisplayParams.CurrentRow.Cells[0].Value = tempChkboxValue;

                String tempDispInfo = dgridDisplayParams.Rows[SelRow - 1].Cells[2].Value.ToString();
                dgridDisplayParams.Rows[SelRow - 1].Cells[2].Value = dgridDisplayParams.CurrentRow.Cells[2].Value;
                dgridDisplayParams.CurrentRow.Cells[2].Value = tempDispInfo;
                dgridDisplayParams.ClearSelection();
                dgridDisplayParams.Rows[SelRow - 1].Cells[1].Selected = true;
            }
        }
        /// <summary>
        /// Code Region Added by Vivek on 5 August 2011 (TNEB Project)
        /// Swapping values in selected row and next row if present in datagrid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDownScroll_Click(object sender, EventArgs e)
        {

            DataGridView dgridDisplayParams = GetSelectedDataGridView();
            int SelRow = dgridDisplayParams.CurrentRow.Index;
            if (SelRow < dgridDisplayParams.Rows.Count - 1)
                {
                    bool tempChkboxValue = (dgridDisplayParams[0, SelRow + 1].Value != null && (bool)dgridDisplayParams[0, SelRow + 1].Value) ? true : false;
                    dgridDisplayParams.Rows[SelRow + 1].Cells[0].Value = dgridDisplayParams.CurrentRow.Cells[0].Value;
                    dgridDisplayParams.CurrentRow.Cells[0].Value = tempChkboxValue;
                    
                    String tempDispInfo = dgridDisplayParams.Rows[SelRow + 1].Cells[2].Value.ToString();
                    dgridDisplayParams.Rows[SelRow + 1].Cells[2].Value = dgridDisplayParams.CurrentRow.Cells[2].Value;
                    dgridDisplayParams.CurrentRow.Cells[2].Value = tempDispInfo;
                    dgridDisplayParams.ClearSelection();
                    dgridDisplayParams.Rows[SelRow + 1].Cells[1].Selected = true;
                }
        }
        #endregion

        #region SelectAll_CheckedChanged
        /// <summary>
        /// Code Region Added by Vivek on 5 August 2011 (TNEB Project)
        /// Handling Check change event on Select all check box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkboxSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            DataGridView dgridDisplayParams = GetSelectedDataGridView();

            for (int i = 0; i < dgridDisplayParams.Rows.Count; i++)
            {
                dgridDisplayParams.Rows[i].Cells["colInclude"].Value = chkboxSelectAll.Checked;
            }
            dgridDisplayParams.EndEdit();
        }
        #endregion

        #region Event handler for Check/uncheck of any chekbox in any grid 
        /// <summary>
        /// Code Region Added by Vivek on 5 August 2011 (TNEB Project)
        /// Called when ever checkbox is checked/unchecked in any grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_CheckBoxChange(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                DataGridView dataGridView = GetSelectedDataGridView();
                if (e.ColumnIndex == 0)
                {
                    dataGridView.EndEdit();
                    chkboxSelectAll.CheckedChanged -= chkboxSelectAll_CheckedChanged;
                    if (!(bool)dataGridView.CurrentCell.Value)
                        chkboxSelectAll.Checked = false;
                    else
                    {
                        bool IfAllRowsSelected = true;
                        for (int i = 0; i < dataGridView.Rows.Count; i++)
                        {
                            DataGridViewCheckBoxCell cell = dataGridView[0, i] as DataGridViewCheckBoxCell;
                            if (cell.Value == null || (bool)cell.Value == false)
                            { IfAllRowsSelected = false; break; }
                        }
                        chkboxSelectAll.Checked = IfAllRowsSelected;
                    }
                    this.chkboxSelectAll.CheckedChanged += chkboxSelectAll_CheckedChanged;
                }
            }
        }
        #endregion

        #region SetSelectAllCheckBoxStatus
        /// <summary>
        /// Code Region Added by Vivek on 4 August 2011 (TNEB Project)
        /// Set the status of SelectAll Check box based on checked/unchecked status
        /// of the checkboxes in the selected Grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetSelectAllCheckBoxStatus(object sender, EventArgs e)
        {
            btnDownScroll.Visible = true;
            btnUpScroll.Visible = true;
            chkboxSelectAll.Visible = true;

            DataGridView dataGridView = GetSelectedDataGridView();
            bool IfAllRowsSelected = true;
            for (int i = 0; i < dataGridView.Rows.Count; i++)
            {
                DataGridViewCheckBoxCell cell = dataGridView[0, i] as DataGridViewCheckBoxCell;
                if (cell==null || cell.Value == null || (bool)cell.Value == false)
                { IfAllRowsSelected = false; break; }
            }
            this.chkboxSelectAll.CheckedChanged -= chkboxSelectAll_CheckedChanged;
            chkboxSelectAll.Checked = IfAllRowsSelected;
            this.chkboxSelectAll.CheckedChanged += chkboxSelectAll_CheckedChanged;
        }
        #endregion

        #region GetSelectedDataGridView
        /// <summary>
        /// Return DataGridview object of the data in Current Grid.
        /// </summary>
        /// <returns></returns>
        private DataGridView GetSelectedDataGridView()
        {
            if (tabControlDisplayParams.SelectedIndex == 0)
            {
                this.tabPushButton.Enter -= new System.EventHandler(this.SetSelectAllCheckBoxStatus);
                dgridPushDisplayParams.Select();
                this.tabPushButton.Enter += new System.EventHandler(this.SetSelectAllCheckBoxStatus);
                return dgridPushDisplayParams;
            }
            else if (tabControlDisplayParams.SelectedIndex == 1)
            {
                this.tabScrollButton.Enter -= new System.EventHandler(this.SetSelectAllCheckBoxStatus);
                dgridScrollDisplayParams.Select();
                this.tabScrollButton.Enter += new System.EventHandler(this.SetSelectAllCheckBoxStatus);
                return dgridScrollDisplayParams;
            }
            else if (tabControlDisplayParams.SelectedIndex == 2)
            {
                this.tabHighResolution.Enter -= new System.EventHandler(this.SetSelectAllCheckBoxStatus);
                dgridHighResolution.Select();
                this.tabHighResolution.Enter += new System.EventHandler(this.SetSelectAllCheckBoxStatus);
                return dgridHighResolution;
            }
            return new DataGridView();
        }
        #endregion

       

        private void chkBoxAutoScrollTime_CheckedChanged(object sender, EventArgs e)
        {
            txtAutoScrollTime.Enabled = chkBoxAutoScrollTime.Checked;
            if (!txtAutoScrollTime.Enabled)
                txtAutoScrollTime.Text = "";
        }

        private void tabDisplayTimeouts_Enter(object sender, EventArgs e)
        {
            btnDownScroll.Visible = false;
            btnUpScroll.Visible = false;

            this.chkboxSelectAll.CheckedChanged -= chkboxSelectAll_CheckedChanged;
            chkboxSelectAll.Checked = false;            
            chkboxSelectAll.Visible = false;
            this.chkboxSelectAll.CheckedChanged += chkboxSelectAll_CheckedChanged;
        }

        private void dgridPushDisplayParams_Enter(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel13_Paint(object sender, PaintEventArgs e)
        {

        }
    }
    #endregion
}
