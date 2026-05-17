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
using CAB.Entity;

namespace CAB.UI
{
    public partial class GSMReadingStatus : MdiChildForm 
    {
        public GSMReadingStatusBLL gSMReadingStatusBLL;
        private GSMReadingStatusEntity gSMReadingStatusEntity;
        public GSMReadingStatus()
        {
            gSMReadingStatusBLL = new GSMReadingStatusBLL();
            gSMReadingStatusEntity = new GSMReadingStatusEntity();
            InitializeComponent();
        }

        public override DataSet GetSearchData()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add(new DataColumn("DisplayMember", typeof(System.String)));
            dataTable.Columns.Add(new DataColumn("ValueMember", typeof(System.String)));
            AddNewRow(dataTable, "All", "-");
            AddNewRow(dataTable, "Reading Time Stamp", "DATE");
            AddNewRow(dataTable, "File Name", "TEXT");
            AddNewRow(dataTable, "File Path", "TEXT");
            AddNewRow(dataTable, "Status", "NONCBO2");
            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(dataTable);
            return dataSet;
        }

        private void LoadGrid()
        {
            SettingsBLL settingsBLL = new SettingsBLL();
            ucSearchControl.PrimarySearchTypeData = this.GetSearchData();
            ucSearchControl.PrimaryNonComboData = settingsBLL.GetPeriod();
            ucSearchControl.SecondaryNonComboData = settingsBLL.GetStatus();
            ucSearchControl.RefreshList();
        }

        private void GSMReadingStatus_Load(object sender, EventArgs e)
        {
            LoadGrid();
            //ucSearchControl.PrimarySearchTypeData = GetSearchData();
            ucSearchControl.ShowSearchandDelete();
            //ucSearchControl.RefreshList();
            //ucSearchControl.ShowSearchText();
            //ucSearchControl_OnFindNowClick(this, null);
            ucSearchControl_OnFindNowClick(this, null);
            grdGSMReadingStatus.SetEqualWidth();

        }

        private void ucSearchControl_OnFindNowClick(object sender, EventArgs e)
        {
            grdGSMReadingStatus.Visible = true;
            DataSet dataSet = new DataSet();
            string searchType = ucSearchControl.PrimarySearchTypeComboData;
            string searchData = ucSearchControl.SecondarySearchTypeComboData;
            string searchText = ucSearchControl.SecondarySearchTypeTextData;
            long fromDate = ucSearchControl.SecondarySearchTypeFromDateData;
            long toDate = ucSearchControl.SecondarySearchTypeToDateData;

            if (fromDate > toDate)
            {
                this.StatusMessage = "To date can't be less than From Date";
                Application.DoEvents();
                return;
            }
            if (searchType.Equals("All"))
            {
                dataSet = gSMReadingStatusBLL.ConvertData(gSMReadingStatusBLL.ListDataSet());
            }
            else if (searchType.Equals("Reading Time Stamp"))
            {
                dataSet = gSMReadingStatusBLL.GetDateWiseReadingStatus(DateUtility.ConvertSearchDateTimeToLong(fromDate, "000000"), DateUtility.ConvertSearchDateTimeToLong(toDate, "235959"));
            }
            if (searchType.Equals("File Name"))
            {
                dataSet = gSMReadingStatusBLL.GetSearchData("FileName", searchText);
            }
            if (searchType.Equals("File Path"))
            {
                dataSet = gSMReadingStatusBLL.GetSearchData("FilePath", searchText);
            }
            if (searchType.Equals("Status"))
            {
                dataSet = gSMReadingStatusBLL.GetSearchData("Status", Int32.Parse(searchData));
            }


            //dataSet = gSMReadingStatusBLL.ConvertData(dataSet);
            //grdGSMReadingStatus.HiddenColumn = "ID";
            grdGSMReadingStatus.ValueColumn = "ID";
            ucSearchControl.RefreshControls(dataSet);
            grdGSMReadingStatus.Data = dataSet;
            grdGSMReadingStatus.IsSorting = false;
            grdGSMReadingStatus.RefreshGrid(); 
        }

        private void ucSearchControl_OnSearchClick(object sender, EventArgs e)
        {
            this.StatusMessage = String.Empty;
            ucSearchControl.EnableControls();
            ucSearchControl.HideMainSearch = true;
            this.LoadGrid();
        }

        private void LoadEntity()
        {
            string pkValue = grdGSMReadingStatus.GetPrimaryValue();
            if (!string.IsNullOrEmpty(pkValue))
            {
                gSMReadingStatusEntity = (GSMReadingStatusEntity)gSMReadingStatusBLL.GetDetailData(Int32.Parse(pkValue));
            }
        }

        private void ucSearchControl_OnDeleteClick(object sender, EventArgs e)
        {
            LoadEntity();
            if (MessageBox.Show("Are you sure to delete this record.", "E-250 BCS", MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.Yes))
            {
                gSMReadingStatusBLL.DeleteData(gSMReadingStatusEntity);
                this.StatusMessage = "Record deleted successfully.";
                GSMReadingStatus_Load(this, null);
            }

        }
    }
}
