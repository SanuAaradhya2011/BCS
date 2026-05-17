using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Hunt.EPIC.Logging;

namespace CABApplication.Reports.Forms
{
    public partial class NetReportCheckListPopUp : Form
    {
        #region Private_Member_Variable
        int maxColumns = 0;
        DataTable dtSelectedDataTable = null;
        DataTable dtSourceDataTable = null;
        int prevIndex = 0;
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(NetReportCheckListPopUp).ToString());
        #endregion



        #region Constructor

        public NetReportCheckListPopUp()
        {
            InitializeComponent();
        }


        public NetReportCheckListPopUp(List<string> lstClmNames, string HeaderText, int MaxColumns)
        {
            InitializeComponent();
            try
            {
                maxColumns = MaxColumns;
                this.Text = HeaderText;
                dtSourceDataTable = new DataTable();
                foreach (string item in lstClmNames)
                {
                    dtSourceDataTable.Columns.Add(item);
                }
                FillCheckListBoxByDataTableColumns();
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "NetReportCheckListPopUp(List<string> lstClmNames, string HeaderText, int MaxColumns)", ex);
            }
        }


        public NetReportCheckListPopUp(DataTable dtCheckList, string HeaderText, int MaxColumns)
        {
            InitializeComponent();
            try
            {
                maxColumns = MaxColumns;
                this.Text = HeaderText;
                dtSourceDataTable = dtCheckList;
                FillCheckListBoxByDataTableColumns();                
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "NetReportCheckListPopUp(DataTable dtCheckList, string HeaderText, int MaxColumns)", ex);
            }
        }



        public NetReportCheckListPopUp(DataTable dtCheckList, string HeaderText, int MaxColumns, bool IsRemoveEmptyColumns)
        {
            InitializeComponent();
            try
            {
                maxColumns = MaxColumns;
                this.Text = HeaderText;
                switch (IsRemoveEmptyColumns)
                {
                    case true:
                        dtSourceDataTable = RemoveEmptyColumnsFromDataTable(dtCheckList);
                        break;
                    case false:
                        dtSourceDataTable = dtCheckList;
                        break;
                    default:
                        dtSourceDataTable = dtCheckList;
                        break;
                }
                
                FillCheckListBoxByDataTableColumns();
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "NetReportCheckListPopUp(DataTable dtCheckList, string HeaderText, int MaxColumns, bool IsRemoveEmptyColumns)", ex);
            }
        }


        #endregion

        #region Private_Member_Methods


        private DataTable RemoveEmptyColumnsFromDataTable(DataTable table)
        {
            DataTable dtResult = null;
            try
            {
                if (table != null && table.Rows.Count > 0)
                {
                    dtResult = table.Clone();
                    foreach (DataColumn column in table.Columns)
                    {
                        if (table.AsEnumerable().All(dr => dr.IsNull(column.ColumnName)) || table.AsEnumerable().All(dr => dr[column.ColumnName].Equals(string.Empty)))
                            dtResult.Columns.Remove(column.ColumnName);
                    }

                    foreach (DataRow dr in table.Rows)
                    {
                        DataRow drr = dtResult.NewRow();
                        foreach (DataColumn item in dtResult.Columns)
                        {
                            drr[item.ColumnName] = dr[item.ColumnName];
                        }
                        dtResult.Rows.Add(drr);
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {

                logger.Log(LOGLEVELS.Error, "RemoveEmptyColumnsFromDataTable(DataTable table)", ex);
            }
            return dtResult;
        }

        private void FillCheckListBoxByDataTableColumns()
        {
            try
            {
                if (chkSelectedList != null && chkSelectedList.Items != null)
                {
                    chkSelectedList.Items.Clear();
                    if (dtSourceDataTable != null)
                    {
                        foreach (DataColumn item in dtSourceDataTable.Columns)
                        {
                            chkSelectedList.Items.Add(item.ColumnName, false);
                        }
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {

                logger.Log(LOGLEVELS.Error, "FillCheckListBoxByDataTableColumns()", ex);
            }
        }


        private void SelectDeselectCheckBoxList(bool flag)
        {
            try
            {
                if (chkSelectedList != null && chkSelectedList.Items != null)
                {
                    for (int i = 0; i < chkSelectedList.Items.Count; i++)
                    {
                        chkSelectedList.SetItemChecked(i, flag);
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "SelectDeselectCheckBoxList(bool flag)", ex);

            }
        }


        private bool ValidateCheckList()
        {
            bool IsValid = true;
            try
            {
                if (chkSelectedList != null && chkSelectedList.CheckedItems != null)
                {
                    if (chkSelectedList.CheckedItems.Count == 0)
                    {
                        MessageBox.Show("Please select atleast one parameter for report.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        IsValid = false;
                    }
                    else if (chkSelectedList.CheckedItems.Count > maxColumns)
                    {
                        MessageBox.Show("Selected parameters can not exceed maxLimit (" + maxColumns + ")", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        IsValid = false;
                    }
                    else
                    {

                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ValidateCheckList()", ex);

            }
            return IsValid;
        }


        private void FillDataTableWithSelectedCheckList()
        {

            try
            {
                if (chkSelectedList != null && chkSelectedList.CheckedItems != null)
                {
                    dtSelectedDataTable = new DataTable();
                    foreach (object item in chkSelectedList.CheckedItems)
                    {
                        dtSelectedDataTable.Columns.Add(item.ToString());
                    }

                    foreach (DataRow itemrow in dtSourceDataTable.Rows)
                    {
                        DataRow row = dtSelectedDataTable.NewRow();
                        foreach (DataColumn itemcolumn in dtSelectedDataTable.Columns)
                        {
                            row[itemcolumn.ColumnName] = itemrow[itemcolumn.ColumnName];
                        }
                        dtSelectedDataTable.Rows.Add(row);
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {

                logger.Log(LOGLEVELS.Error, "FillDataTableWithSelectedCheckList()", ex);
            }

        }



        #endregion

        #region Member_Events

        private void NetReportCheckListPopUp_Load(object sender, EventArgs e)
        {
            try
            {
                checkBoxSelectAll.Checked = true;
                // Next button get automatically click in case selected parameters are less then max coulmn supported in New Dynamic Report
                if (chkSelectedList.Items.Count < maxColumns)
                {
                    SaveFunctionality();
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "NetReportCheckListPopUp_Load(object sender, EventArgs e)", ex);
                
            }
        }


        private void checkBoxSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                SelectDeselectCheckBoxList(checkBoxSelectAll.Checked);
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "checkBoxSelectAll_CheckedChanged(object sender, EventArgs e)", ex);
                
            }            
        }


        private void SaveFunctionality()
        {
            try
            {
                if (ValidateCheckList())
                {
                    FillDataTableWithSelectedCheckList();
                    this.Close();
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "SaveFunctionality()", ex);
                
            }

        }

        private void btnSaveChanges_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFunctionality();
            }
            catch (Exception ex)    //Exception log for catch block
            {

                logger.Log(LOGLEVELS.Error, "btnSaveChanges_Click(object sender, EventArgs e)", ex);
            }            
        }

        private void chkSelectedList_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (prevIndex != chkSelectedList.SelectedIndex)
                {
                    bool flag = chkSelectedList.GetItemChecked(chkSelectedList.SelectedIndex);
                    chkSelectedList.SetItemChecked(chkSelectedList.SelectedIndex, !flag);
                }
                prevIndex = chkSelectedList.SelectedIndex;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "chkSelectedList_MouseClick(object sender, MouseEventArgs e)", ex);

            }
        }


        #endregion

        #region Public_Member_Methods

        public DataTable GetSelectedDataTable()
        {
            return dtSelectedDataTable;
        }   


        #endregion





    }
}
