using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Collections;
using CAB.UI.Controls;
using CAB.BLL;
using CAB.IECFramework.Utility;
using CAB.Entity;
using LTCTBLL;



namespace CAB.UI
{
    public partial class MeterDataLoadSurvey : MdiChildForm 
    {
        LoadSurveyBLL loadSurveyBLL=new LoadSurveyBLL();
        public DateTime FromDate;
        public DateTime ToDate;
        private DataSet LoadSurveyData;
        private string MeterId;
        public long MeterDataId;
        ArrayList filteredColumsn;
        public MeterDataLoadSurvey()
        {
            InitializeComponent();
        }
        private DataTable SortedDataTable
        {
            get;
            set;
        }
        private long ParseDate(string dateTime, bool start)
        {
            string val=dateTime.Substring(0,8);
            if (start)
                val = val + "000000";
            else
                val = val + "233000";
            return long.Parse(val);
        }

        private DataSet SortDataSet(DataSet dset, string column)
        {
            if (dset == null)
                return null;
            if (dset.Tables.Count == 0)
                return null;
            if (dset.Tables[0].Rows.Count == 0)
                return null;

            DataTable table = new DataTable();
            foreach (DataColumn col in dset.Tables[0].Columns)
            {
                if (Convert.ToString(col.ColumnName) != "MDIntervalPeriod")
                    table.Columns.Add(Convert.ToString(col.ColumnName));
            }

            for (int i = dset.Tables[0].Rows.Count - 1; i >= 0; i--)
            {
                DataRow dr = table.NewRow();
                int innerCounter = 0;
                for (int j = 0; j < dset.Tables[0].Columns.Count; j++)
                {
                    if (dset.Tables[0].Columns[j].Caption.Equals("MDIntervalPeriod"))
                        continue;
                    dr[innerCounter] = dset.Tables[0].Rows[i][j];
                    innerCounter++;
                }
                table.Rows.Add(dr);
            }

            DataSet dsetSorted = new DataSet();
            dsetSorted.Tables.Add(table);
            return dsetSorted;
        }

        private void MeterDataLoadSurvey_Load(object sender, EventArgs e)
        {
            this.Text = "Load Survey"; 
            comboBox1.SelectedIndex = 0;
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

            string types = comboBox1.SelectedItem.ToString().Trim();
            SetGrid(types, lngLoadSurvey,1);
         
        }

        private bool ChkNoLoad(DataRow dr)
        {
            for(int i=0;i<filteredColumsn.Count;i++)
            {
                if (!(dr[filteredColumsn[i].ToString()] != null && Convert.ToDouble(dr[filteredColumsn[i].ToString()]) == 0.0))
                    return false;
            }
            return true;
        }
        private void SetDemandOrEnergyColumns(DataTable dt)
        {
            IEnumerator enumerator = dt.Columns.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (enumerator.Current.ToString().Contains(comboBox1.SelectedIndex == 0 ? "Demand" : "Energy"))
                    filteredColumsn.Add(enumerator.Current.ToString());
            }
                
        }
        private void SetNoLoadNoPowerRecords(ref DataSet ls_ds)
        {
            bool PowerOffInIP = false;
            double tempColValue; 
            for (int i = 0; i < ls_ds.Tables[0].Rows.Count; i++)
            {
                if (ChkNoLoad(ls_ds.Tables[0].Rows[i]))
                {
                    for (int x = 0; x < filteredColumsn.Count; x++)
                        ls_ds.Tables[0].Rows[i][filteredColumsn[x].ToString()] = LSIPType.NL.ToString();
                }// noLoadCount++;
                PowerOffInIP = true;
                for (int j = 1; j < ls_ds.Tables[0].Columns.Count; j++)
                {
                    if ((ls_ds.Tables[0].Rows[i][j] == null))
                    {
                        PowerOffInIP = false;
                        break;
                    }
                    if (Double.TryParse(ls_ds.Tables[0].Rows[i][j].ToString(), out tempColValue))
                    {
                        if (tempColValue != -1)
                        {
                            PowerOffInIP = false;
                            break;
                        }
                    }
                }
                if (PowerOffInIP)
                {
                    for (int j = 1; j < ls_ds.Tables[0].Columns.Count; j++)
                    {
                        if (ls_ds.Tables[0].Rows[i][j] != null && Double.TryParse(ls_ds.Tables[0].Rows[i][j].ToString(), out tempColValue))
                        {
                            if (tempColValue == -1)
                                ls_ds.Tables[0].Rows[i][j] = LSIPType.NP.ToString();
                        }
                    }
                }
            }
        }

        private void SetGrid(string types, CABGridControl grdControl, int tabNo)
        {
            MeterDataEntity meterDataEntity = new MeterDataBLL().GetDetailData(MeterDataId) as MeterDataEntity;
            if (meterDataEntity != null)
                lblMeterIdValue.Text = meterDataEntity.MeterID;
            long fDate = ParseDate(DateUtility.DateTimeToLong(FromDate).ToString(), true);
            long tDate = ParseDate(DateUtility.DateTimeToLong(ToDate).ToString(), false);
            string fromDate = CommonBLL.SplitWithOutDateUnit(fDate.ToString());
            string toDate = CommonBLL.SplitWithOutDateUnit(tDate.ToString());
            lblFromDateValue.Text = fromDate.Split(' ')[0];
            lblToDateValue.Text = toDate.Split(' ')[0];
            bool flag = false;
            lblIntprd.Text = "Integration Period : 0";
            LoadSurveyData = loadSurveyBLL.ListDataSet(MeterDataId, fDate, tDate, types);
            
            if (LoadSurveyData != null)
            {
                if (UtilityDetails.UtilityName == UtilityEntity.UGVCL || UtilityDetails.UtilityName == UtilityEntity.PVVNL || UtilityDetails.UtilityName == UtilityEntity.JDVVNL)
                    LoadSurveyData.Tables[0].Columns.Remove("TamperStatus"); 

                foreach (DataRow row in LoadSurveyData.Tables[0].Rows)
                {
                    // Added to fix the integartion period coming as -1 in analysis report
                    if (row["MDIntervalPeriod"].ToString() != "-1")
                    {
                        lblIntprd.Text = "Integration Period : " + row["MDIntervalPeriod"].ToString() + " Min.";
                        flag = true;
                        break;
                    }
                    //if (col.ColumnName.Equals("MDIntervalPeriod") )
                    //{
                    //    flag = true;
                    //    break;
                    //}
                }
                //lblIntprd.Text = "Integration Period : " + Convert.ToString(LoadSurveyData.Tables[0].Rows[0]["MDIntervalPeriod"]) + " Minutes";
                // This is a fix for inbuilt DataGrid bug in .net framework
                DataGridView view = (DataGridView)grdControl.Controls["grdData"];
                
          
               
                if(view!=null)
                    view.DataSource = null;
                if (tabNo == 1)
                {
                    DataSet ls_ds = SortDataSet(LoadSurveyData, "Date Time");
                    filteredColumsn=new ArrayList();
                    SetDemandOrEnergyColumns(ls_ds.Tables[0]);
                    SetNoLoadNoPowerRecords(ref ls_ds);
                    grdControl.Data = ls_ds;
                    grdControl.SetWidth("Date Time", 150);
                }
                else if (tabNo == 2)
                {
                    grdControl.Data = SortDataSet(loadSurveyBLL.GetMaxMinDayLoadsurvey(MeterDataId, fDate, tDate, types),"Date");
                    grdControl.SetEqualWidth();
                    if (view != null)
                    {
                        if (view.Columns.Count > 1)
                            view.Columns[0].Width = 75;
                    }
                    grdControl.SetWidth("Date", 150);
                    grdControl.SetWidth("Max Demand kW", 150);
                    grdControl.SetWidth("Min Demand kW", 150);
                }
                
                grdControl.HSrollBar();
                grdControl.IsSorting = false;
            
                //lngLoadSurvey.SetWidth("Date Time", 140);
                //lngLoadSurvey.RefreshGrid();
                //if (flag)
                //{ 
                //    lngLoadSurvey.HiddenColumn = "MDIntervalPeriod";
                //} 
            }
        }
        private void tabPage2_Click(object sender, EventArgs e)
        {
            
           
        }

        private void tabPage2_Enter(object sender, EventArgs e)
        {
            comboBox2.SelectedIndex = 0;
            string types = comboBox2.SelectedItem.ToString().Trim();
            SetGrid(types, lngDDGridControl, 2);
           
            //lngDDGridControl.Data = dataSet;
           
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string types = comboBox2.SelectedItem.ToString().Trim();
            SetGrid(types,lngDDGridControl, 2);
        }

        private void lngDDGridControl_Load(object sender, EventArgs e)
        {

        }

        private void lngLoadSurvey_OnCABGridCellFormat(DataGridViewCellFormattingEventArgs value)
        {
            if (value.Value.ToString().Trim() == LSIPType.NP.ToString())
                value.CellStyle.ForeColor = Color.Green;
            else if (value.Value.ToString().Trim() == LSIPType.NL.ToString())
                value.CellStyle.ForeColor = Color.Red;
            
        }
    }
}
