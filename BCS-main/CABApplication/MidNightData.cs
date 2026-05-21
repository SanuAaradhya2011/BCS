using System;
using System.Data;
using System.Windows.Forms;
using CAB.BLL;
using CAB.Entity;
using CAB.Framework.Utility;
using CAB.UI.Controls;
using CAB.DALC.Data;
using Hunt.EPIC.Logging;
namespace CAB.UI
{
    public partial class MidNightData : MdiChildForm
    {
        public DateTime FromDate;
        public DateTime ToDate;
        private DataSet MidNightDataSet;
        public long MeterDataId;
        long fDate, tDate;
        bool isKVAParsed = false;
        bool isKWParsed = false;
        decimal KWValue = 0;
        decimal KVAValue = 0;
        private DLMS650LoadSurveyBLL loadSurveyBLL;
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(MidNightData).ToString());

        public MidNightData()
        {
            InitializeComponent();

            //initializing object
            loadSurveyBLL = new DLMS650LoadSurveyBLL();
            
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
        // Added to solve midnight data difference in fast download and direct read out issue 
        private DataSet ListFileName(long activeMeterDataId)
        {
            return new FileUploadMasterBLL().GetCABFileNameWithMeterDataId(activeMeterDataId);
        }
        
        private void MidNightData_Load(object sender, EventArgs e)
        {
            try
            {
                string fileName = string.Empty;
                this.Text = "MidNight Data";

                //call to BLL
                MeterDataEntity meterDataEntity = new MeterDataBLL().GetDetailData(MeterDataId) as MeterDataEntity;

                if (meterDataEntity != null)
                    lblMeterIdValue.Text = meterDataEntity.MeterID;

                //Parsing date
                fDate = ParseDate(DateUtility.DateTimeToLong(FromDate).ToString(), true);
                tDate = ParseDate(DateUtility.DateTimeToLong(ToDate).ToString(), false);
                string fromDate = CommonBLL.SplitWithOutDateUnit(fDate.ToString());
                string toDate = CommonBLL.SplitWithOutDateUnit(tDate.ToString());
                lblFromDateValue.Text = fromDate.Split(' ')[0];
                lblToDateValue.Text = toDate.Split(' ')[0];

                this.Cursor = Cursors.WaitCursor;
                DataSet fileDataset = new DataSet();
                // Added to solve midnight data difference in fast download and direct read out issue 
                fileDataset = ListFileName(MeterDataId);
                if (fileDataset != null)
                {
                    if (fileDataset.Tables[0].Rows.Count > 0)
                    {
                        fileName = fileDataset.Tables[0].Rows[0][1].ToString();
                    }
                }
                //call to BLL
                MidNightDataSet = loadSurveyBLL.GetMidNightData(fileName, MeterDataId, fDate, tDate);
                if (UtilityDetails.Utility == CAB.Framework.UtilityEntity.Generic)
                {
                    lngMidNightData.Data = MidNightDataSet;
                    lngMidNightData.SetEqualWidth();
                    lngMidNightData.RefreshGrid();
                    return;
                }
                int div = 1;
                DLMS650LoadSurveyDAL ls = new DLMS650LoadSurveyDAL();
                DataSet dSet = ls.ListDataSet(MeterDataId, fDate, tDate,true  ); // Story - 427028 - Load survey data sequence should be in descending order except graph
                int MDInterval = 0;
                if (dSet != null)
                {
                    if (dSet.Tables[0].Rows.Count > 1)
                    {
                        TimeSpan ts = DateUtility.LongToDateTime(Int64.Parse(dSet.Tables[0].Rows[1][0].ToString())) - DateUtility.LongToDateTime(Int64.Parse(dSet.Tables[0].Rows[0][0].ToString()));
                        MDInterval = (int)ts.TotalMinutes;
                        if (MDInterval == 15)
                            div = 4;
                        if (MDInterval == 30)
                            div = 2;
                    }
                    else
                    {
                        div = 1;
                    }
                }

                //check for null condition
                if (MidNightDataSet != null)
                {
                    // Added to calculate demand from energy values.
                    for (int i = 0; i < MidNightDataSet.Tables[0].Rows.Count; i++)
                    {
                        isKWParsed = decimal.TryParse(MidNightDataSet.Tables[0].Rows[i][17].ToString(), out KWValue);
                        isKVAParsed = decimal.TryParse(MidNightDataSet.Tables[0].Rows[i][18].ToString(), out KVAValue);
                        if (isKWParsed)
                        {
                            MidNightDataSet.Tables[0].Rows[i][17] = Convert.ToDecimal(KWValue * div);
                        }
                        else
                        {
                            MidNightDataSet.Tables[0].Rows[i][17] = MidNightDataSet.Tables[0].Rows[i][17].ToString();
                        }
                        if (isKVAParsed)
                        {
                            MidNightDataSet.Tables[0].Rows[i][18] = Convert.ToDecimal(KVAValue * div);
                        }
                        else
                        {
                            MidNightDataSet.Tables[0].Rows[i][18] = MidNightDataSet.Tables[0].Rows[i][18].ToString();
                        }
                    }
                    if (MidNightDataSet.Tables.Count > 0 || MidNightDataSet.Tables[0].Rows.Count > 0)
                    {
                        //filling the grid
                        lngMidNightData.Data = MidNightDataSet;
                        lngMidNightData.SetEqualWidth();
                        lngMidNightData.RefreshGrid();

                    }
                }
                //else
                //{
                //    MessageBox.Show("There is no data for Selected Dates.");
                //}
                this.Cursor = Cursors.Default;







            }
            catch (Exception ex)    //Exception log for catch block
            {
                //MessageBox.Show(ex.Message);
                logger.Log(LOGLEVELS.Error, "MidNightData_Load(object sender, EventArgs e)", ex);
            }
            finally {
                this.Cursor = Cursors.Default;
            }
        }
    }
}