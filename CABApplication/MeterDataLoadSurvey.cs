using System;
using System.Data;
using System.Windows.Forms;
using CAB.BLL;
using CAB.Entity;
using CAB.Framework;
using CAB.Framework.Utility;
using CAB.UI.Controls;

namespace CAB.UI
{
    public partial class MeterDataLoadSurvey : MdiChildForm
    {
        public DateTime FromDate;
        public DateTime ToDate;
        private DataSet LoadSurveyData;
        public long MeterDataId;
        long fDate, tDate;
        private ApplicationType types;
        LoadSurveyEntity loadSurveyEntity;
        public MeterDataLoadSurvey()
        {
            InitializeComponent();
            types = ConfigInfo.GetApplicationType();
            loadSurveyEntity = new LoadSurveyEntity();
        }

        private long ParseDate(string dateTime, bool start)
        {
            string val = dateTime.Substring(0, 8);
            if (start)
                val = val + "000000";
            else
                val = val + "235959";
            return long.Parse(val);
        }

        private void MeterDataLoadSurvey_Load(object sender, EventArgs e)
        {
            this.Text = "Load Survey";
            toolStripStatusLabel1.Visible = false;
            MeterDataEntity meterDataEntity = new MeterDataBLL().GetDetailData(MeterDataId) as MeterDataEntity;
            if (meterDataEntity != null)
                lblMeterIdValue.Text = meterDataEntity.MeterID;
            fDate = ParseDate(DateUtility.DateTimeToLong(FromDate).ToString(), true);
            tDate = ParseDate(DateUtility.DateTimeToLong(ToDate).ToString(), false);
            string fromDate = CommonBLL.SplitWithOutDateUnit(fDate.ToString());
            string toDate = CommonBLL.SplitWithOutDateUnit(tDate.ToString());
            lblFromDateValue.Text = fromDate.Split(' ')[0];
            lblToDateValue.Text = toDate.Split(' ')[0];
            lblIntprd.Text = "Integration Period : 0";
            if (types.Equals(ApplicationType.IEC_LTCT_650))
                cboDemandEnergy.SelectedIndex = 0;
            else
                cboDemandEnergy.SelectedIndex = 1;
            rdbNoPadding.Checked = true;
        }

        private void cboDemandEnergy_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool flag = false;
            
            if((ToDate - FromDate).Days > 1825 && !rdbNoPadding.Checked)
			{
                MessageBox.Show("Padding Disabled due to very large Time Span (greater than 6 months)", "BCS Load Survey", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    rdbNoPadding.Checked = true;
            }

            if (types.Equals(ApplicationType.DLMS_LTCT_650))
            {
                lblPF.Visible = false;
                this.Cursor = Cursors.WaitCursor;
                DLMS650LoadSurveyBLL dLMS650LoadSurveyBLL = new DLMS650LoadSurveyBLL();
                //LoadSurveyData = dLMS650LoadSurveyBLL.ListDataSet(MeterDataId, fDate, tDate, cboDemandEnergy.SelectedItem.ToString());                
                if (rdbNoPadding.Checked)
                {
                    LoadSurveyData = dLMS650LoadSurveyBLL.ListDataSetColumnWise(MeterDataId, fDate, tDate, cboDemandEnergy.SelectedItem.ToString(), false);
                }
                else
                {
                    LoadSurveyData = dLMS650LoadSurveyBLL.ListDataSetColumnWise(MeterDataId, fDate, tDate, cboDemandEnergy.SelectedItem.ToString(), true);
                }
                if (LoadSurveyData != null)
                {
                    if (LoadSurveyData.Tables != null)
                    {
                        if (LoadSurveyData.Tables[0].Rows.Count == 1 && cboDemandEnergy.SelectedItem.ToString() == "Demand" && ConfigInfo.ActiveFileType == "DLMS")
                        {
                            this.Cursor = Cursors.Default;
                            lngLoadSurvey.Data = null;
                            return;
                        }
                        //lngLoadSurvey.Data = SortDataSet(LoadSurveyData);
                        lngLoadSurvey.Data = LoadSurveyData; // Story - 427028 - data should be in descending order

                        lngLoadSurvey.SetEqualWidth();
                    }
                    //To handle negative Integeration Period
                    lblIntprd.Text = "Integration Period : " + dLMS650LoadSurveyBLL.MDInterval.ToString().Replace("-", "") + " Minutes";
                    // Added to set the decriptions to tamper status.
                    foreach (DataColumn col in lngLoadSurvey.Data.Tables[0].Columns)
                    {
                        if (col.ToString().Contains("Tamper Status (0.0.96.1.152.255;1;2)"))
                        {
                            if (ConfigInfo.ActiveMeterType == "1P-2W")
                            {
                                panelTamperSinglePhase.Visible = true;
                            }
                            else
                            {
                                panelTamperType.Visible = true;
                            }
                            break;
                        }
                        else
                        {
                            panelTamperType.Visible = false;
                            panelTamperSinglePhase.Visible = false;
                        }
                    }
                    lngLoadSurvey.RefreshGrid();
                }
                else
                {
                    // Added to solve bug 72902.
                    if (dLMS650LoadSurveyBLL.IntegrationPeriodStatus)
                    {
                        //MessageBox.Show("Data Corrupt", "BCS");
                        toolStripStatusLabel1.Visible = true;
                        toolStripStatusLabel1.Text = "Data Corrupt";
                        Application.DoEvents();
                    }
                    else
                    {
                        cboDemandEnergy.SelectedIndex = 1;
                    }
                }
                this.Cursor = Cursors.Default;
            }
        }

        private DataSet SortDataSet(DataSet dset)
        {
            if (dset == null)
                return null;
            if (dset.Tables.Count == 0)
                return null;
            if (dset.Tables[0].Rows.Count == 0)
                return null;
            DataTable table = new DataTable();
            foreach (DataColumn col in dset.Tables[0].Columns)
                table.Columns.Add(Convert.ToString(col.ColumnName));
            for (int i = dset.Tables[0].Rows.Count - 1; i >= 0; i--)
            {
                DataRow dr = table.NewRow();
                for (int j = 0; j < dset.Tables[0].Columns.Count; j++)
                    dr[j] = dset.Tables[0].Rows[i][j];
                table.Rows.Add(dr);
            }
            DataSet dsetSorted = new DataSet();
            dsetSorted.Tables.Add(table);
            return dsetSorted;
        }

        private void rdbNoPadding_CheckedChanged(object sender, EventArgs e)
        {
            cboDemandEnergy_SelectedIndexChanged(sender, e);
        }

        private void rdbWithPadding_CheckedChanged(object sender, EventArgs e)
        {
            cboDemandEnergy_SelectedIndexChanged(sender, e);
        }

    }
}