using System;
using System.Data;
using CAB.BLL;
using CAB.Entity;
using CAB.UI.Controls;
using CAB.IECFramework.Utility;
using System.Windows.Forms;
using System.Drawing;

namespace CAB.UI
{
	public partial class GSMScheduling : MdiChildForm
	{
		private GSMScheduleBLL sgmScheduleBLL;
		GSMScheduleEntity gsmScheduleEntity;
        GSMScheduleInfo ucDetail = null;
        public GSMScheduling()
		{
			InitializeComponent();
            this.Text = "GSM Scheduling";
            sgmScheduleBLL = new GSMScheduleBLL(); 
            ucDetail = new GSMScheduleInfo();
            this.ucDetail.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ucDetail.Location = new System.Drawing.Point(13, 51);
            this.ucDetail.Name = "ucDetail";
            this.ucDetail.Size = new System.Drawing.Size(867, 582); 
            this.ucDetail.TabIndex = 3;
            this.ucDetail.OnCancelClick += new GSMScheduleInfo.CancelClickHandler(this.ucDetail_OnCancelClick);
            this.ucDetail.OnChannelStatusChanged += new GSMScheduleInfo.ChannelStatusChanged(this.Child_OnStatusChanged);
            this.ucDetail.OnSaveClick += new GSMScheduleInfo.SaveClickHandler(this.ucDetail_OnSaveClick);
            this.Controls.Add(ucDetail);
		}
        private void Child_OnStatusChanged(string msg)
        {
            this.StatusMessage = msg;
            Application.DoEvents();
        }
        public override DataSet GetSearchData()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add(new DataColumn("DisplayMember", typeof(System.String)));
            dataTable.Columns.Add(new DataColumn("ValueMember", typeof(System.String)));
            AddNewRow(dataTable, "All", "-");
            AddNewRow(dataTable, "Schedule Name", "TEXT"); 
            AddNewRow(dataTable, "Period", "NONCBO1");
            AddNewRow(dataTable, "Status", "NONCBO2");
            AddNewRow(dataTable, "Creation Date", "DATE"); 
            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(dataTable);
            return dataSet;
        }
        private void GSMScheduling_Load(object sender, EventArgs e)
        {
            LoadGrid();
            ucSearchControl_OnFindNowClick(this, null);
            ucGridControl.SetEqualWidth();
        }

		private void LoadGrid()
		{
            SettingsBLL settingsBLL=new SettingsBLL();
            ucSearchControl.PrimarySearchTypeData = this.GetSearchData();
            ucSearchControl.PrimaryNonComboData = settingsBLL.GetPeriod();
            ucSearchControl.SecondaryNonComboData = settingsBLL.GetStatus();
			ucSearchControl.RefreshList();
		}
       
        private void FillGridWithSearchType()
        {
            DataSet dataSet = null;
            string SearchType = ucSearchControl.PrimarySearchTypeComboData;
            string SearchData = ucSearchControl.SecondarySearchTypeComboData;
            string SearchText = ucSearchControl.SecondarySearchTypeTextData;
            long fromDate = ucSearchControl.SecondarySearchTypeFromDateData;
            long toDate = ucSearchControl.SecondarySearchTypeToDateData;

           
            if (SearchType.Equals("All"))
                dataSet = sgmScheduleBLL.ListDataSet();
            else if (SearchType.Equals("Schedule Name"))
            {
                if (SearchText == string.Empty)
                    dataSet = sgmScheduleBLL.ListDataSet();
                else
                    dataSet = sgmScheduleBLL.GetSearchData("Schedule_Name", SearchText);
            }
              else if (SearchType.Equals("Status"))
            {
                if (SearchData == "-")
                    dataSet = sgmScheduleBLL.ListDataSet();
                else
                    dataSet = sgmScheduleBLL.GetSearchData("Status", Int32.Parse(SearchData));
            }
            else if (SearchType.Equals("Period"))
            {
                if (SearchData == "-")
                    dataSet = sgmScheduleBLL.ListDataSet();
                else
                    dataSet = sgmScheduleBLL.GetSearchData("Schedule_Period", SearchData);
            }
            else if (SearchType.Equals("Creation Date"))
            {
                if (fromDate > toDate)
                {
                    this.StatusMessage="To date can't be less than from date";
                    Application.DoEvents();
                    return;
                } 
                dataSet = sgmScheduleBLL.GetSearchData(DateUtility.ConvertSearchDateTimeToLong(fromDate, "000000"), DateUtility.ConvertSearchDateTimeToLong(toDate, "235959"));
            }
            dataSet = sgmScheduleBLL.ConvertData(dataSet);
            ucSearchControl.RefreshControls(dataSet);
            ucGridControl.Data = dataSet;
            ucGridControl.HiddenColumn = "gsmSchedule_ID";
            ucGridControl.ValueColumn = "gsmSchedule_ID";
            ucGridControl.IsSorting = false;
            ucGridControl.RefreshGrid();  
        }

        private void ucSearchControl_OnFindNowClick(object sender, EventArgs e)
        {
            ucDetail.Visible = false;
            ucGridControl.Visible = true;
            FillGridWithSearchType();
        }

        private void ucSearchControl_OnAddClick(object sender, EventArgs e)
        {
            this.StatusMessage = String.Empty;
            ucSearchControl.ShowAddOption();
            ucGridControl.Visible = false;
            ucSearchControl.Visible = false;
            ucDetail.Visible = true;
            ucDetail.Location = new Point(19, 16);
            ucDetail.ClearData();
        }

        private void ucDetail_OnCancelClick(object sender, EventArgs e)
        { 
            GSMScheduling_Load(this, null);
            ucSearchControl.Visible = true;
            this.StatusMessage = String.Empty;
        }

        private void ucDetail_OnSaveClick(object sender, EventArgs e)
        {
            ucSearchControl.Visible = true;
            GSMScheduling_Load(this, null); 
        }

		private void ucSearchControl_OnSearchClick(object sender, EventArgs e)
		{
            this.StatusMessage = String.Empty;
			ucSearchControl.EnableControls();
            ucSearchControl.HideMainSearch = true;
			this.LoadGrid();
		}

        private void ucSearchControl_OnEditClick(object sender, EventArgs e)
        {
            this.StatusMessage = String.Empty;
            ucDetail.Visible = true;
            ucSearchControl.ShowEditOption();
            ucSearchControl.Visible = false;
            ucDetail.Location = new Point(19, 16);
            ucGridControl.Visible = false;
            LoadEntity();
        }

        private void LoadEntity()
        {
            string pkValue = ucGridControl.GetPrimaryValue();
            if (!string.IsNullOrEmpty(pkValue))
            {
                gsmScheduleEntity = (GSMScheduleEntity)sgmScheduleBLL.GetDetailData(Int32.Parse(pkValue));
                ucDetail.EditData(gsmScheduleEntity);
            }
        }

		private void ucSearchControl_OnDeleteClick(object sender, EventArgs e)
		{
            LoadEntity(); 
            if (MessageBox.Show("Are you sure to delete this record.", "E-250 BCS", MessageBoxButtons.YesNo,MessageBoxIcon.Question).Equals(DialogResult.Yes))
            { 
                sgmScheduleBLL.DeleteData(gsmScheduleEntity);
                this.StatusMessage = "Record deleted successfully.";
                GSMScheduling_Load(this, null);
            }
		} 
         
	}
}
