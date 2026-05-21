///****************************************************************************
//'*
//'*  Projet       : DLMS BCS
//'*
//'*  Component    : Report
//'*
//'*  Module       : Load Survey Graph
//'*
//'*  Environment  : Visual Studio 2008 - C#.net
//'*
//'*------+----------+------------------------------------------------------------
//'*Vers |   Date    |    Programmer and Comments
//'*------+----------+------------------------------------------------------------
//'* 1.00 | 19/11/10 | Gopal Krishna Gupta : creation.
//'*------+----------+------------------------------------------------------------
//'*      |          | XXXXX: Change Details
//'******************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using CAB.BLL;
using CAB.Entity;
using CAB.IECFramework.Utility;
using LTCTBLL;

namespace CAB.UI
{
    public partial class frmLoadsurveyGraph : Form
    {
        public long MeterDataId { get; set; }
        public int IntervalPeriod { get; set; }
        public long FromDate { get; set; }
        public long ToDate { get; set; }
        public ArrayList selectedParameterList = new ArrayList();
        private int recordCount = 0;
        private int currentRecordCount = 0;
        DateTime firstDate, lastDate, nextDate;
        private int numberOfDays;
        public string parameterName;
        private DataSet GraphDataSet;
        private string ParameterType;
        private byte GraphView;
        private byte ChartType;
        private int MaxValue = 0;
        private int maxRecord = 0;
        private string seriesName = string.Empty;
        private int CurrentRecord;
        private bool value;
        private bool ShowGrids;
        private bool GraphType;
        private bool Viewtype;
        public int noLoadIPCount = 0;
        public int noPowerIPCount = 0;
        private const string DATETITLE1 = "Report Date Time: ";
        private const string DATETITLE2 = "\n(dd/MM/yyyy HH:mm)";
        System.Globalization.DateTimeFormatInfo dateInfo = new System.Globalization.DateTimeFormatInfo(); 

        public frmLoadsurveyGraph()
        {
            InitializeComponent();
            //FromDate = 20101022003000;
            //ToDate = 20101112120000;
            //IntervalPeriod = 30;
            //MeterDataId = 2;
            dateInfo.ShortDatePattern = ConfigInfo.DateFormat();// "dd/MM/yyyy"; 
        }
        private String fGetSeriesName(String series)
        {
            if (series == "RPhaseCurrent")
                return "R Phase Current";
            else if (series == "YPhaseCurrent")
                return "Y Phase Current";
            else if (series == "BPhaseCurrent")
                return "B Phase Current";
            else if (series == "RPhaseVoltage")
                return "R Phase Voltage";
            else if (series == "YPhaseVoltage")
                return "Y Phase Voltage";
            else if (series == "BPhaseVoltage")
                return "B Phase Voltage";
            else if (series == "DemandKW")
                return "Demand kW";
            else if (series == "DemandKVARLag")
                return "Demand kvar Lag";
            else if (series == "DemandKVARLead")
                return "Demand kvar Lead";
            else if (series == "DemandKVA")
                return "Demand kVA";
            else if (series == "EnergyKWh")
                return "Energy kWh";
            else if (series == "EnergyKVARhLag")
                return "Energy kvarh Lag";
            else if (series == "EnergyKVARhLead")
                return "Energy kvarh Lead";
            else if (series == "EnergyKVAh")
                return "Energy kVAh";
            else
                return series;

        }
        private void SetColorForSeries()
        {
            //Setting the Color of the series
            for (int count = 0; count < graphChart.Series.Count; count++)
            {
                if (count == 0)
                    graphChart.Series[0].Color = clrSeries1.BackColor;// Color.Red;
                else if (count == 1)
                    graphChart.Series[1].Color = clrSeries2.BackColor;// Color.Yellow;
                else if (count == 2)
                    graphChart.Series[2].Color = clrSeries3.BackColor;// Color.Yellow;
                else if (count == 3)
                    graphChart.Series[3].Color = clrSeries4.BackColor;// Color.Yellow;


            }
        }
        private DataTable AutoNumberedTable(DataTable SourceTable)
        {

            DataTable ResultTable = new DataTable();

            DataColumn AutoNumberColumn = new DataColumn();

            AutoNumberColumn.ColumnName = "ID";

            AutoNumberColumn.DataType = typeof(int);

            AutoNumberColumn.AutoIncrement = true;

            AutoNumberColumn.AutoIncrementSeed = 1;

            AutoNumberColumn.AutoIncrementStep = 1;

            ResultTable.Columns.Add(AutoNumberColumn);

            ResultTable.Merge(SourceTable);

            return ResultTable;

        }
        private DataTable DailySortedColumn(DataSet dset, int recordCount)
        {
            if (dset == null)
            {
                return null;
            }
            else if (dset.Tables[0].Rows.Count == 0)
            {
                return null;
            }

            DataTable filteredTable = new DataTable();
            filteredTable = dset.Tables[0].Clone();
            DataTable dataTable = new DataTable();
            dataTable = AutoNumberedTable(dset.Tables[0]);
            string stringExpression = string.Empty;
            if (maxRecord <= MaxValue)
            {
                stringExpression = "ID <= " + maxRecord;
            }
            else
            {
                stringExpression = "ID > " + (recordCount) + " and " + "ID <= " + (recordCount + MaxValue);
            }
            string stringSort = "ID ASC";
            DataRow[] selectedRows = dataTable.Select(stringExpression, stringSort);

            return ConvertDailyDataRowToDataTable(selectedRows, dataTable);
        }
        private DataTable ConvertDailyDataRowToDataTable(DataRow[] dataRow, DataTable dTable)
        {
            DataTable resultTable = new DataTable();
            DataColumn dColumn;
            DataRow dr;

            for (int count = 1; count < dTable.Columns.Count - 1; count++)
            {
                dColumn = new DataColumn(dTable.Columns[count].ColumnName);
                dColumn.DataType = System.Type.GetType("System.Double");
                resultTable.Columns.Add(dColumn);
            }

            dColumn = new DataColumn("loadSurveyDateTime");
            resultTable.Columns.Add(dColumn);

            foreach (DataRow drow in dataRow)
            {
                dr = resultTable.NewRow();
                for (int i = 1; i < dTable.Columns.Count; i++)
                {
                    dr[resultTable.Columns[i - 1].ColumnName] = drow[dTable.Columns[i].ColumnName];
                }
                resultTable.Rows.Add(dr);
            }
            return resultTable;
        }
        private void SetChartType()
        {
            if (ChartType == 0)
            {
                graphChart.Series[seriesName].ChartType = SeriesChartType.Line;
            }
            else if (ChartType == 1)
            {
                graphChart.Series[seriesName].ChartType = SeriesChartType.Column;
            }
            else if (ChartType == 2)
            {
                graphChart.Series[seriesName].ChartType = SeriesChartType.SplineArea;
            }
            else if (ChartType == 3)
            {
                graphChart.Series[seriesName].ChartType = SeriesChartType.StepLine;

            }
            else if (ChartType == 4)
            {
                graphChart.Series[seriesName].ChartType = SeriesChartType.Pie;

            }

        }
        public void DisplayChart()
        {
            try
            {
                int totalcolcount = GraphDataSet.Tables[0].Columns.Count - 1;
                if (GraphType == true && totalcolcount > 1)  //seperate
                {
                    maxRecord = GraphDataSet.Tables[0].Rows.Count;
                    if (maxRecord <= 0)
                        return;

                    DataTable graphTable = new DataTable();
                    graphTable = DailySortedColumn(GraphDataSet, CurrentRecord);

                    if (totalcolcount <= 1)
                        return;

                    graphChart.ChartAreas.Clear();
                    graphChart.Legends.Clear();
                    graphChart.Series.Clear();

                    System.Windows.Forms.DataVisualization.Charting.ElementPosition newelement;
                    if (totalcolcount == 2)
                    {
                        graphChart.ChartAreas.Add("AREA0");
                        newelement = new System.Windows.Forms.DataVisualization.Charting.ElementPosition(0, 8, 45, 92);
                        graphChart.ChartAreas["AREA0"].Position = newelement;

                        graphChart.ChartAreas.Add("AREA1");
                        newelement = new System.Windows.Forms.DataVisualization.Charting.ElementPosition(48, 8, 45, 92);
                        graphChart.ChartAreas["AREA1"].Position = newelement;

                    }
                    else if (totalcolcount >= 3)
                    {
                        graphChart.ChartAreas.Add("AREA0");
                        newelement = new System.Windows.Forms.DataVisualization.Charting.ElementPosition(0, 8, 45, 45);
                        graphChart.ChartAreas["AREA0"].Position = newelement;

                        graphChart.ChartAreas.Add("AREA1");
                        newelement = new System.Windows.Forms.DataVisualization.Charting.ElementPosition(48, 8, 45, 45);
                        graphChart.ChartAreas["AREA1"].Position = newelement;

                        graphChart.ChartAreas.Add("AREA2");
                        newelement = new System.Windows.Forms.DataVisualization.Charting.ElementPosition(0, 52, 45, 45);
                        graphChart.ChartAreas["AREA2"].Position = newelement;

                        graphChart.ChartAreas.Add("AREA3");
                        newelement = new System.Windows.Forms.DataVisualization.Charting.ElementPosition(48, 52, 45, 45);
                        graphChart.ChartAreas["AREA3"].Position = newelement;
                    }

                    //checking the column count and adding the series for each column
                    //count - 1 is used because last column is date time column
                    for (int colCount = 0; colCount < totalcolcount; colCount++)
                    {

                        // taking the series name from the column name
                        seriesName = fGetSeriesName(GraphDataSet.Tables[0].Columns[colCount].ToString());

                        graphChart.Series.Add(seriesName);

                        graphChart.Series[seriesName].ChartArea = "AREA" + colCount;

                        //Setting the Chart Type for the Series
                        SetChartType();
                        //Setting the border width of the line 
                        graphChart.Series[seriesName].BorderWidth = 1;

                        foreach (DataRow dRow in graphTable.Rows)
                        {
                            //Dont Delete Very important
                            //************************************************************************************************************
                            //////DateTime xAxisValue = Convert.ToDateTime(dRow["loadSurveyDateTime"].ToString());
                            //************************************************************************************************************
                            string xAxisValue = "";
                            double yAxisValue = 0;

                            if (GraphView == 0)
                            {
                                xAxisValue = ConvertStringTodate(dRow["loadSurveyDateTime"].ToString()).ToString("HH:mm");
                                yAxisValue = Convert.ToDouble(dRow[colCount].ToString());
                            }
                            else if (GraphView == 1)
                            {
                                xAxisValue = ConvertStringTodate(dRow["loadSurveyDateTime"].ToString()).ToString("HH:mm") + Convert.ToString('\n') + ConvertStringTodate(dRow["loadSurveyDateTime"].ToString()).ToString("ddd") + Convert.ToString('\n') + ConvertStringTodate(dRow["loadSurveyDateTime"].ToString()).ToString("dd-MMM-yy");// +"\n" + Convert.ToDateTime(dRow["loadSurveyDateTime"].ToString()).ToString("dd/MM/yyyy"); ;
                                yAxisValue = Convert.ToDouble(dRow[colCount].ToString());
                            }
                            else if (GraphView == 2)
                            {
                                xAxisValue = ConvertStringTodate(dRow["loadSurveyDateTime"].ToString()).ToString("HH:mm") + Convert.ToString('\n') + ConvertStringTodate(dRow["loadSurveyDateTime"].ToString()).ToString("dd-MMM-yy") + Convert.ToString('\n') + ConvertStringTodate(dRow["loadSurveyDateTime"].ToString()).ToString("ddd");
                                yAxisValue = Convert.ToDouble(dRow[colCount].ToString());
                            }
                            if (yAxisValue < 0)
                                yAxisValue = 0;
                            graphChart.Series[seriesName].Points.AddXY(xAxisValue, yAxisValue);
                        }

                        //}
                        //Setting the Chart Area Back Color and other Settings
                        graphChart.ChartAreas["AREA" + colCount].AxisX.ScaleView.Zoomable = true;
                        graphChart.ChartAreas["AREA" + colCount].BackColor = Color.FromArgb(64, 165, 191, 228);
                        graphChart.ChartAreas["AREA" + colCount].BackGradientStyle = GradientStyle.TopBottom;
                        graphChart.ChartAreas["AREA" + colCount].BackSecondaryColor = Color.White;
                        graphChart.BackColor = Color.FromArgb(211, 223, 240);
                        graphChart.BorderlineColor = Color.FromArgb(26, 59, 105);
                        graphChart.BorderlineDashStyle = ChartDashStyle.Solid;

                        //Enabling the Cursor Positions
                        graphChart.ChartAreas["AREA" + colCount].CursorX.IsUserEnabled = true;
                        graphChart.ChartAreas["AREA" + colCount].CursorX.Position = 0;
                        graphChart.ChartAreas["AREA" + colCount].CursorX.LineColor = Color.FromName("Black");
                        //graphChart.ChartAreas["AREA" + colCount].CursorY.IsUserEnabled = true;

                        graphChart.ChartAreas["AREA" + colCount].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
                        graphChart.ChartAreas["AREA" + colCount].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot;

                        //Dont Delete Very important
                        if (ShowGrids == true)
                        {
                            //************************************************************************************************************
                            graphChart.ChartAreas["AREA" + colCount].AxisX.MajorGrid.Enabled = true;
                            graphChart.ChartAreas["AREA" + colCount].AxisY.MajorGrid.Enabled = true;
                            //************************************************************************************************************
                        }
                        else
                        {
                            graphChart.ChartAreas["AREA" + colCount].AxisX.MajorGrid.Enabled = false;
                            graphChart.ChartAreas["AREA" + colCount].AxisY.MajorGrid.Enabled = false;
                        }

                        if (GraphView == 0)
                        {
                            if (IntervalPeriod == 15)
                            {
                                graphChart.ChartAreas["AREA" + colCount].AxisX.LabelStyle.Interval = 4;
                                graphChart.ChartAreas["AREA" + colCount].AxisX.MajorGrid.Interval = 4;
                            }
                            else if (IntervalPeriod == 30)
                            {
                                graphChart.ChartAreas["AREA" + colCount].AxisX.LabelStyle.Interval = 2;
                                graphChart.ChartAreas["AREA" + colCount].AxisX.MajorGrid.Interval = 2;
                            }
                            else if (IntervalPeriod == 60)
                            {
                                graphChart.ChartAreas["AREA" + colCount].AxisX.LabelStyle.Interval = 1;
                                graphChart.ChartAreas["AREA" + colCount].AxisX.MajorGrid.Interval = 1;
                            }

                            graphChart.ChartAreas["AREA" + colCount].AxisX.MajorTickMark.Interval = 1;

                            graphChart.ChartAreas["AREA" + colCount].AxisX.LabelAutoFitStyle = LabelAutoFitStyles.LabelsAngleStep90;

                        }
                        else if (GraphView == 1)
                        {
                            if (IntervalPeriod == 15)
                            {
                                graphChart.ChartAreas["AREA" + colCount].AxisX.LabelStyle.Interval = 4 * 12;
                                graphChart.ChartAreas["AREA" + colCount].AxisX.MajorGrid.Interval = 4 * 12;
                                graphChart.ChartAreas["AREA" + colCount].AxisX.MajorTickMark.Interval = 4 * 12;
                            }
                            else if (IntervalPeriod == 30)
                            {
                                graphChart.ChartAreas["AREA" + colCount].AxisX.LabelStyle.Interval = 2 * 12;
                                graphChart.ChartAreas["AREA" + colCount].AxisX.MajorGrid.Interval = 2 * 12;
                                graphChart.ChartAreas["AREA" + colCount].AxisX.MajorTickMark.Interval = 2 * 12;

                            }
                            else if (IntervalPeriod == 60)
                            {
                                graphChart.ChartAreas["AREA" + colCount].AxisX.LabelStyle.Interval = 1 * 12;
                                graphChart.ChartAreas["AREA" + colCount].AxisX.MajorGrid.Interval = 1 * 12;
                                graphChart.ChartAreas["AREA" + colCount].AxisX.MajorTickMark.Interval = 1 * 12;
                            }
                        }
                        else if (GraphView == 2)
                        {
                            if (IntervalPeriod == 15)
                            {
                                graphChart.ChartAreas["AREA" + colCount].AxisX.LabelStyle.Interval = 4 * 24;
                                graphChart.ChartAreas["AREA" + colCount].AxisX.MajorGrid.Interval = 4 * 24;
                                graphChart.ChartAreas["AREA" + colCount].AxisX.MajorTickMark.Interval = 4 * 24;
                            }
                            else if (IntervalPeriod == 30)
                            {
                                graphChart.ChartAreas["AREA" + colCount].AxisX.LabelStyle.Interval = 2 * 24;
                                graphChart.ChartAreas["AREA" + colCount].AxisX.MajorGrid.Interval = 2 * 24;
                                graphChart.ChartAreas["AREA" + colCount].AxisX.MajorTickMark.Interval = 2 * 24;

                            }
                            else if (IntervalPeriod == 60)
                            {
                                graphChart.ChartAreas["AREA" + colCount].AxisX.LabelStyle.Interval = 1 * 24;
                                graphChart.ChartAreas["AREA" + colCount].AxisX.MajorGrid.Interval = 1 * 24;
                                graphChart.ChartAreas["AREA" + colCount].AxisX.MajorTickMark.Interval = 1 * 24;
                            }
                        }
                        SetColorForSeries();

                        // Assigning the Chart Title for X - Axis
                        graphChart.ChartAreas["AREA" + colCount].AxisX.Title = tslabelDate.Text;
                        graphChart.ChartAreas["AREA" + colCount].AxisX.TitleAlignment = StringAlignment.Center;

                        // Assigning the Chart Title for Y - Axis
                        graphChart.ChartAreas["AREA" + colCount].AxisY.Title = seriesName;
                        graphChart.ChartAreas["AREA" + colCount].AxisY.TitleAlignment = StringAlignment.Center;
                        graphChart.ChartAreas["AREA" + colCount].AxisY.TextOrientation = TextOrientation.Stacked;


                        graphChart.ChartAreas["AREA" + colCount].BorderDashStyle = ChartDashStyle.Solid;
                        graphChart.ChartAreas["AREA" + colCount].BorderWidth = 3;

                        graphChart.ChartAreas["AREA" + colCount].Area3DStyle.Enable3D = Viewtype;

                    }
                    if (totalcolcount == 3)     /// dummy Row
                    {

                        // taking the series name from the column name
                        seriesName = "Blank";
                        graphChart.Series.Add(seriesName);

                        graphChart.Series[seriesName].ChartArea = "AREA3";

                        //Setting the Chart Type for the Series
                        SetChartType();
                        //Setting the border width of the line 
                        graphChart.Series[seriesName].BorderWidth = 1;

                        foreach (DataRow dRow in graphTable.Rows)
                        {
                            //Dont Delete Very important
                            //************************************************************************************************************
                            //////DateTime xAxisValue = Convert.ToDateTime(dRow["loadSurveyDateTime"].ToString());
                            //************************************************************************************************************
                            string xAxisValue = "";
                            double yAxisValue = 0;

                            if (GraphView == 0)
                            {
                                xAxisValue = ConvertStringTodate(dRow["loadSurveyDateTime"].ToString()).ToString("HH:mm");
                                yAxisValue = Convert.ToDouble(0);
                            }
                            else if (GraphView == 1)
                            {
                                xAxisValue = ConvertStringTodate(dRow["loadSurveyDateTime"].ToString()).ToString("HH:mm") + Convert.ToString('\n') + ConvertStringTodate(dRow["loadSurveyDateTime"].ToString()).ToString("ddd") + Convert.ToString('\n') + ConvertStringTodate(dRow["loadSurveyDateTime"].ToString()).ToString("dd-MMM-yy");// +"\n" + Convert.ToDateTime(dRow["loadSurveyDateTime"].ToString()).ToString("dd/MM/yyyy"); ;
                                yAxisValue = Convert.ToDouble(0);
                            }
                            else if (GraphView == 2)
                            {
                                xAxisValue = ConvertStringTodate(dRow["loadSurveyDateTime"].ToString()).ToString("HH:mm") + Convert.ToString('\n') + ConvertStringTodate(dRow["loadSurveyDateTime"].ToString()).ToString("dd-MMM-yy") + Convert.ToString('\n') + ConvertStringTodate(dRow["loadSurveyDateTime"].ToString()).ToString("ddd");
                                yAxisValue = Convert.ToDouble(0);
                            }
                            if (yAxisValue < 0)
                                yAxisValue = 0;
                            graphChart.Series[seriesName].Points.AddXY(xAxisValue, yAxisValue);
                        }

                        //}
                        //Setting the Chart Area Back Color and other Settings
                        graphChart.ChartAreas["AREA3"].AxisX.ScaleView.Zoomable = true;
                        graphChart.ChartAreas["AREA3"].BackColor = Color.FromArgb(64, 165, 191, 228);
                        graphChart.ChartAreas["AREA3"].BackGradientStyle = GradientStyle.TopBottom;
                        graphChart.ChartAreas["AREA3"].BackSecondaryColor = Color.White;
                        graphChart.BackColor = Color.FromArgb(211, 223, 240);
                        graphChart.BorderlineColor = Color.FromArgb(26, 59, 105);
                        graphChart.BorderlineDashStyle = ChartDashStyle.Solid;

                        //Enabling the Cursor Positions
                        graphChart.ChartAreas["AREA3"].CursorX.IsUserEnabled = true;
                        graphChart.ChartAreas["AREA3"].CursorX.Position = 0;
                        graphChart.ChartAreas["AREA3"].CursorX.LineColor = Color.FromName("Black");
                        //graphChart.ChartAreas["AREA3"].CursorY.IsUserEnabled = true;

                        graphChart.ChartAreas["AREA3"].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
                        graphChart.ChartAreas["AREA3"].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot;

                        //Dont Delete Very important
                        if (ShowGrids == true)
                        {
                            //************************************************************************************************************
                            graphChart.ChartAreas["AREA3"].AxisX.MajorGrid.Enabled = true;
                            graphChart.ChartAreas["AREA3"].AxisY.MajorGrid.Enabled = true;
                            //************************************************************************************************************
                        }
                        else
                        {
                            graphChart.ChartAreas["AREA3"].AxisX.MajorGrid.Enabled = false;
                            graphChart.ChartAreas["AREA3"].AxisY.MajorGrid.Enabled = false;
                        }

                        if (GraphView == 0)
                        {
                            if (IntervalPeriod == 15)
                            {
                                graphChart.ChartAreas["AREA3"].AxisX.LabelStyle.Interval = 4;
                                graphChart.ChartAreas["AREA3"].AxisX.MajorGrid.Interval = 4;
                            }
                            else if (IntervalPeriod == 30)
                            {
                                graphChart.ChartAreas["AREA3"].AxisX.LabelStyle.Interval = 2;
                                graphChart.ChartAreas["AREA3"].AxisX.MajorGrid.Interval = 2;
                            }
                            else if (IntervalPeriod == 60)
                            {
                                graphChart.ChartAreas["AREA3"].AxisX.LabelStyle.Interval = 1;
                                graphChart.ChartAreas["AREA3"].AxisX.MajorGrid.Interval = 1;
                            }

                            graphChart.ChartAreas["AREA3"].AxisX.MajorTickMark.Interval = 1;

                            graphChart.ChartAreas["AREA3"].AxisX.LabelAutoFitStyle = LabelAutoFitStyles.LabelsAngleStep90;

                        }
                        else if (GraphView == 1)
                        {
                            if (IntervalPeriod == 15)
                            {
                                graphChart.ChartAreas["AREA3"].AxisX.LabelStyle.Interval = 4 * 12;
                                graphChart.ChartAreas["AREA3"].AxisX.MajorGrid.Interval = 4 * 12;
                                graphChart.ChartAreas["AREA3"].AxisX.MajorTickMark.Interval = 4 * 12;
                            }
                            else if (IntervalPeriod == 30)
                            {
                                graphChart.ChartAreas["AREA3"].AxisX.LabelStyle.Interval = 2 * 12;
                                graphChart.ChartAreas["AREA3"].AxisX.MajorGrid.Interval = 2 * 12;
                                graphChart.ChartAreas["AREA3"].AxisX.MajorTickMark.Interval = 2 * 12;

                            }
                            else if (IntervalPeriod == 60)
                            {
                                graphChart.ChartAreas["AREA3"].AxisX.LabelStyle.Interval = 1 * 12;
                                graphChart.ChartAreas["AREA3"].AxisX.MajorGrid.Interval = 1 * 12;
                                graphChart.ChartAreas["AREA3"].AxisX.MajorTickMark.Interval = 1 * 12;
                            }
                        }
                        else if (GraphView == 2)
                        {
                            if (IntervalPeriod == 15)
                            {
                                graphChart.ChartAreas["AREA3"].AxisX.LabelStyle.Interval = 4 * 24;
                                graphChart.ChartAreas["AREA3"].AxisX.MajorGrid.Interval = 4 * 24;
                                graphChart.ChartAreas["AREA3"].AxisX.MajorTickMark.Interval = 4 * 24;
                            }
                            else if (IntervalPeriod == 30)
                            {
                                graphChart.ChartAreas["AREA3"].AxisX.LabelStyle.Interval = 2 * 24;
                                graphChart.ChartAreas["AREA3"].AxisX.MajorGrid.Interval = 2 * 24;
                                graphChart.ChartAreas["AREA3"].AxisX.MajorTickMark.Interval = 2 * 24;

                            }
                            else if (IntervalPeriod == 60)
                            {
                                graphChart.ChartAreas["AREA3"].AxisX.LabelStyle.Interval = 1 * 24;
                                graphChart.ChartAreas["AREA3"].AxisX.MajorGrid.Interval = 1 * 24;
                                graphChart.ChartAreas["AREA3"].AxisX.MajorTickMark.Interval = 1 * 24;
                            }
                        }
                        //SetColorForSeries(graphChart);

                        // Assigning the Chart Title for X - Axis
                        graphChart.ChartAreas["AREA3"].AxisX.Title = tslabelDate.Text;
                        graphChart.ChartAreas["AREA3"].AxisX.TitleAlignment = StringAlignment.Center;

                        // Assigning the Chart Title for Y - Axis
                        graphChart.ChartAreas["AREA3"].AxisY.Title = "No Parameter";
                        graphChart.ChartAreas["AREA3"].AxisY.TitleAlignment = StringAlignment.Center;
                        graphChart.ChartAreas["AREA3"].AxisY.TextOrientation = TextOrientation.Stacked;

                        graphChart.ChartAreas["AREA3"].Area3DStyle.Enable3D = Viewtype;

                        graphChart.ChartAreas["AREA3"].BorderDashStyle = ChartDashStyle.Solid;
                        graphChart.ChartAreas["AREA3"].BorderWidth = 3;

                    }
                    graphChart.Legends.Clear();
                    graphChart.Legends.Add("aa");
                    graphChart.Legends[0].Alignment = StringAlignment.Center;
                    graphChart.Legends[0].Docking = Docking.Top;
                }
                else       ///composite
                {
                    //Clear the Chart Area
                    graphChart.ChartAreas.Clear();
                    graphChart.ChartAreas.Add("ChartArea1");
                    graphChart.Series.Clear();

                    maxRecord = GraphDataSet.Tables[0].Rows.Count;
                    if (maxRecord <= 0)
                        return;


                    //Getting the Sorted table after sorting the record Columns
                    DataTable graphTable = new DataTable();
                    graphTable = DailySortedColumn(GraphDataSet, CurrentRecord);

                    //checking the column count and adding the series for each column
                    //count - 1 is used because last column is date time column
                    for (int colCount = 0; colCount < GraphDataSet.Tables[0].Columns.Count - 1; colCount++)
                    {
                        // taking the series name from the column name
                        seriesName = fGetSeriesName(GraphDataSet.Tables[0].Columns[colCount].ToString());

                        graphChart.Series.Add(seriesName);

                        //Setting the Chart Type for the Series
                        SetChartType();
                        //Setting the border width of the line 
                        graphChart.Series[seriesName].BorderWidth = 1;



                        int p = 0;
                        foreach (DataRow dRow in graphTable.Rows)
                        {
                            //Dont Delete Very important
                            //************************************************************************************************************
                            //////DateTime xAxisValue = Convert.ToDateTime(dRow["loadSurveyDateTime"].ToString());
                            //************************************************************************************************************
                            string xAxisValue = "";
                            double yAxisValue = 0;

                            if (GraphView == 0)
                            {
                                xAxisValue = ConvertStringTodate(dRow["loadSurveyDateTime"].ToString()).ToString("HH:mm");
                                yAxisValue = Convert.ToDouble(dRow[colCount].ToString());
                            }
                            else if (GraphView == 1)
                            {
                                xAxisValue = ConvertStringTodate(dRow["loadSurveyDateTime"].ToString()).ToString("HH:mm") + Convert.ToString('\n') + ConvertStringTodate(dRow["loadSurveyDateTime"].ToString()).ToString("ddd") + Convert.ToString('\n') + ConvertStringTodate(dRow["loadSurveyDateTime"].ToString()).ToString("dd-MMM-yy");
                                yAxisValue = Convert.ToDouble(dRow[colCount].ToString());
                            }
                            else if (GraphView == 2)
                            {
                                xAxisValue = ConvertStringTodate(dRow["loadSurveyDateTime"].ToString()).ToString("HH:mm") + Convert.ToString('\n') + ConvertStringTodate(dRow["loadSurveyDateTime"].ToString()).ToString("dd-MMM-yy") + Convert.ToString('\n') + ConvertStringTodate(dRow["loadSurveyDateTime"].ToString()).ToString("ddd");
                                yAxisValue = Convert.ToDouble(dRow[colCount].ToString());
                            }
                            if (yAxisValue < 0)
                                yAxisValue = 0;


                            graphChart.Series[seriesName].Points.AddXY(xAxisValue, yAxisValue);

                            //graphChart.Series[seriesName].Points[0].AxisLabel = "First Point";
                            //graphChart.Series[seriesName].Points[1].AxisLabel = "First Point";

                            p++;
                        }
                    }

                    graphChart.ChartAreas["ChartArea1"].BackColor = Color.FromArgb(64, 165, 191, 228);
                    graphChart.ChartAreas["ChartArea1"].BackGradientStyle = GradientStyle.TopBottom;
                    graphChart.ChartAreas["ChartArea1"].BackSecondaryColor = Color.White;
                    graphChart.BackColor = Color.FromArgb(211, 223, 240);
                    graphChart.BorderlineColor = Color.FromArgb(26, 59, 105);
                    graphChart.BorderlineDashStyle = ChartDashStyle.Solid;

                    //Enabling the Cursor Positions
                    graphChart.ChartAreas["ChartArea1"].CursorX.IsUserEnabled = true;
                    graphChart.ChartAreas["ChartArea1"].CursorX.Position = 0;
                    graphChart.ChartAreas["ChartArea1"].CursorX.LineColor = Color.FromName("Black");
                    //graphChart.ChartAreas["ChartArea1"].CursorY.IsUserEnabled = true;


                    graphChart.ChartAreas["ChartArea1"].AxisY.IsStartedFromZero = true;

                    graphChart.ChartAreas["ChartArea1"].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
                    graphChart.ChartAreas["ChartArea1"].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot;

                    //Dont Delete Very important
                    if (ShowGrids == true)
                    {
                        //************************************************************************************************************
                        graphChart.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = true;
                        graphChart.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = true;
                        //************************************************************************************************************
                    }
                    else
                    {
                        graphChart.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;
                        graphChart.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = false;
                    }

                    if (GraphView == 0)
                    {
                        if (IntervalPeriod == 15)
                        {
                            graphChart.ChartAreas["ChartArea1"].AxisX.LabelStyle.Interval = 4;
                            graphChart.ChartAreas["ChartArea1"].AxisX.MajorGrid.Interval = 4;
                        }
                        else if (IntervalPeriod == 30)
                        {
                            graphChart.ChartAreas["ChartArea1"].AxisX.LabelStyle.Interval = 2;
                            graphChart.ChartAreas["ChartArea1"].AxisX.MajorGrid.Interval = 2;
                        }
                        else if (IntervalPeriod == 60)
                        {
                            graphChart.ChartAreas["ChartArea1"].AxisX.LabelStyle.Interval = 1;
                            graphChart.ChartAreas["ChartArea1"].AxisX.MajorGrid.Interval = 1;
                        }

                        graphChart.ChartAreas["ChartArea1"].AxisX.MajorTickMark.Interval = 1;

                        graphChart.ChartAreas["ChartArea1"].AxisX.LabelAutoFitStyle = LabelAutoFitStyles.LabelsAngleStep90;

                    }
                    else if (GraphView == 1)
                    {
                        if (IntervalPeriod == 15)
                        {
                            graphChart.ChartAreas["ChartArea1"].AxisX.LabelStyle.Interval = 4 * 12;
                            graphChart.ChartAreas["ChartArea1"].AxisX.MajorGrid.Interval = 4 * 12;
                            graphChart.ChartAreas["ChartArea1"].AxisX.MajorTickMark.Interval = 4 * 12;
                        }
                        else if (IntervalPeriod == 30)
                        {
                            graphChart.ChartAreas["ChartArea1"].AxisX.LabelStyle.Interval = 2 * 12;
                            graphChart.ChartAreas["ChartArea1"].AxisX.MajorGrid.Interval = 2 * 12;
                            graphChart.ChartAreas["ChartArea1"].AxisX.MajorTickMark.Interval = 2 * 12;

                        }
                        else if (IntervalPeriod == 60)
                        {
                            graphChart.ChartAreas["ChartArea1"].AxisX.LabelStyle.Interval = 1 * 12;
                            graphChart.ChartAreas["ChartArea1"].AxisX.MajorGrid.Interval = 1 * 12;
                            graphChart.ChartAreas["ChartArea1"].AxisX.MajorTickMark.Interval = 1 * 12;
                        }
                    }
                    else if (GraphView == 2)
                    {
                        if (IntervalPeriod == 15)
                        {
                            graphChart.ChartAreas["ChartArea1"].AxisX.LabelStyle.Interval = 4 * 24;
                            graphChart.ChartAreas["ChartArea1"].AxisX.MajorGrid.Interval = 4 * 24;
                            graphChart.ChartAreas["ChartArea1"].AxisX.MajorTickMark.Interval = 4 * 24;
                        }
                        else if (IntervalPeriod == 30)
                        {
                            graphChart.ChartAreas["ChartArea1"].AxisX.LabelStyle.Interval = 2 * 24;
                            graphChart.ChartAreas["ChartArea1"].AxisX.MajorGrid.Interval = 2 * 24;
                            graphChart.ChartAreas["ChartArea1"].AxisX.MajorTickMark.Interval = 2 * 24;

                        }
                        else if (IntervalPeriod == 60)
                        {
                            graphChart.ChartAreas["ChartArea1"].AxisX.LabelStyle.Interval = 1 * 24;
                            graphChart.ChartAreas["ChartArea1"].AxisX.MajorGrid.Interval = 1 * 24;
                            graphChart.ChartAreas["ChartArea1"].AxisX.MajorTickMark.Interval = 1 * 24;
                        }
                    }
                    SetColorForSeries();
                    if (GraphView == 0)
                    {
                        // return;
                        graphChart.Legends.Clear();
                        graphChart.Legends.Add("aa");
                        graphChart.Legends[0].Alignment = StringAlignment.Center;
                        graphChart.Legends[0].Docking = Docking.Top;

                        //--------------------------Custome----------------------------
                        int statindexNL = 0;
                        int statindexNP = 0;
                        int[] nopower = new int[3000];
                        int[] noLoad = new int[3000];
                        for (int x = 0; x < 3000; x++)
                        {
                            noLoad[x] = nopower[x]=-1;
                        }
                        int rcnt = 0;
                        DateTime frmDt = DateUtility.LongToDateTime(FromDate);
                        foreach (DataRow dRow in graphTable.Rows)
                        {
                            //dateInfo
                            if (Convert.ToDateTime(dRow[4].ToString().Trim(),dateInfo) < frmDt)
                            { rcnt++; continue; }

                            // added by vivek
                            if (IsNoLoadRecord(dRow))
                            {
                                noLoad[statindexNL++] = rcnt;
                            }
                            // Addition ended

                            //if (dRow[0].ToString() == "0")
                            //{
                            //    //if (rcnt == 0)
                            //    //    rcnt++; 
                            //    noLoad[statindexNL++] = rcnt;
                            //}
                            if (dRow[0].ToString().StartsWith("-")) nopower[statindexNP++] = rcnt;
                            rcnt++;
                        }
                        //---------------------------No Load------------------------
                        double startpoint = 0.00;
                        double Endpoint = 0.00;
                        statindexNL = 0;
                        int ismultioff = 0;
                        int diffflg = 1;
                        while (noLoad[statindexNL] != -1 || noLoad[statindexNL + 1] != -1)
                        {
                            ismultioff = 0;
                            diffflg = 2;
                            if (noLoad[statindexNL + 1] != -1) diffflg = noLoad[statindexNL + 1] - noLoad[statindexNL];
                            startpoint = noLoad[statindexNL];
                            while (noLoad[statindexNL] + 1 == noLoad[statindexNL + 1])
                            {
                                if (noLoad[statindexNL] + diffflg == noLoad[statindexNL + 1])
                                    Endpoint = noLoad[statindexNL + 1];
                                statindexNL++;
                                ismultioff++;
                            }

                            if (ismultioff <= 0)
                            {
                                if (diffflg <= 2) Endpoint = startpoint + diffflg;
                                else if (statindexNL == 0) Endpoint = startpoint + 2;
                                else Endpoint = startpoint + 2;
                                graphChart.ChartAreas[0].AxisX.CustomLabels.Add(startpoint, Endpoint, "NL", 1, LabelMarkStyle.None);
                            }
                            else graphChart.ChartAreas[0].AxisX.CustomLabels.Add(startpoint + diffflg, Endpoint + diffflg, "NL", 1, LabelMarkStyle.LineSideMark);
                            statindexNL++;

                            int lblcnt = 0;

                            while (lblcnt < graphChart.ChartAreas[0].AxisX.CustomLabels.Count)
                            {
                                if (graphChart.ChartAreas[0].AxisX.CustomLabels[lblcnt].Text == "NL") graphChart.ChartAreas[0].AxisX.CustomLabels[lblcnt].MarkColor = System.Drawing.Color.Red;
                                lblcnt++;
                            }
                        }
                        //---------------------------No Power------------------------

                        startpoint = 0.00;
                        Endpoint = 0.00;
                        statindexNP = 0;
                        ismultioff = 0;
                        diffflg = 1;
                        while (nopower[statindexNP] != -1 || nopower[statindexNP + 1] != -1)
                        {
                            ismultioff = 0;
                            diffflg = 2;
                            if (nopower[statindexNP + 1] != -1) diffflg = nopower[statindexNP + 1] - nopower[statindexNP];
                            startpoint = nopower[statindexNP];
                            while (nopower[statindexNP] + 1 == nopower[statindexNP + 1])
                            {
                                if (nopower[statindexNP] + diffflg == nopower[statindexNP + 1])
                                    Endpoint = nopower[statindexNP + 1];
                                statindexNP++;
                                ismultioff++;
                            }

                            if (ismultioff <= 0)
                            {
                                if (diffflg <= 2) Endpoint = startpoint + diffflg;
                                else if (statindexNP == 0) Endpoint = startpoint + 2;//(nopower[statindexNP]);   
                                else Endpoint = Endpoint = startpoint + 2;
                                graphChart.ChartAreas[0].AxisX.CustomLabels.Add(startpoint, Endpoint, "NP", 1, LabelMarkStyle.None);
                            }
                            else graphChart.ChartAreas[0].AxisX.CustomLabels.Add(startpoint + diffflg, Endpoint + diffflg, "NP", 1, LabelMarkStyle.LineSideMark);
                            statindexNP++;
                            int lblcnt = 0;
                            while (lblcnt < graphChart.ChartAreas[0].AxisX.CustomLabels.Count)
                            {
                                if (graphChart.ChartAreas[0].AxisX.CustomLabels[lblcnt].Text == "NP") graphChart.ChartAreas[0].AxisX.CustomLabels[lblcnt].MarkColor = System.Drawing.Color.Green;
                                lblcnt++;
                            }
                        }
                    }
                    //------------------End---------------------
                    graphChart.Legends.Clear();
                    graphChart.Legends.Add("aa");
                    graphChart.Legends[0].Alignment = StringAlignment.Center;
                    graphChart.Legends[0].Docking = Docking.Top;


                    // Assigning the Chart Title for X - Axis
                    graphChart.ChartAreas["ChartArea1"].AxisX.Title = tslabelDate.Text;
                    graphChart.ChartAreas["ChartArea1"].AxisX.TitleAlignment = StringAlignment.Center;

                    // Assigning the Chart Title for Y - Axis
                    graphChart.ChartAreas["ChartArea1"].AxisY.Title = ParameterType;
                    graphChart.ChartAreas["ChartArea1"].AxisY.TitleAlignment = StringAlignment.Center;
                    graphChart.ChartAreas["ChartArea1"].AxisY.TextOrientation = TextOrientation.Stacked;

                    graphChart.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = Viewtype;
                    graphChart.Focus();

                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }
        // added by vivek 
        private bool IsNoLoadRecord(DataRow dr)
        {
            for(int i=0;i<dr.Table.Columns.Count;i++)
            {
                if(
                    dr.Table.Columns[i].ColumnName.ToLower().Contains("demand")
                    || dr.Table.Columns[i].ColumnName.ToLower().Contains("energy"))
                {
                    if (dr[i] !=null && Convert.ToDecimal(dr[i]) != 0)
                        return false;
                }
            }
            return true;
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
        public DataSet LoadPaddingData(ArrayList parameterList)//string fileName, string meterID)
        {
            long fDate = ParseDate(Convert.ToString(FromDate), true);
            long tDate = ParseDate(Convert.ToString(ToDate), false);
            DataSet dataSet = new DataSet();
            dataSet = new LoadSurveyBLL().ListDataSetForGraph(MeterDataId, FromDate, ToDate, parameterName);
            return ConvertParameterToDataSet(dataSet, parameterList);
        }
        //DateTime GetStartdate(String strdate)
        //{
        //    string[] val = strdate.Split(' ');
        //    int Year = Convert.ToInt32(val[0].Substring(6, 4));
        //    int Month = Convert.ToInt32(val[0].Substring(3, 2));
        //    int Day = Convert.ToInt32(val[0].Substring(0, 2));
        //    int Hour = 0;
        //    int min = 0;
        //    if (GraphView == 1)
        //    {
        //        DateTime dt = new DateTime(Year, Month, Day, Hour, min, 0);
        //        while (true)
        //        {
        //            String dd = dt.DayOfWeek.ToString().ToUpper();
        //            if (dd == "MONDAY")
        //                return dt;
        //            else
        //                dt = dt.AddDays(-1);
        //        }
        //    }
        //    else if (GraphView == 2)
        //    {
        //        Day = 1;
        //    }

        //    return new DateTime(Year, Month, Day, Hour, min, 0);

        //}

        DateTime GetStartdate(String strdate)
        {
            string[] val = strdate.Split(' ');

            int Year = Convert.ToInt32(val[0].Substring(6, 4));
            int Month;// = Convert.ToInt32(val[0].Substring(3, 2));
            int Day ;//= Convert.ToInt32(val[0].Substring(0, 2));

            if (ConfigInfo.DateFormat() == "dd/MM/yyyy")
            {
                Month = Convert.ToInt32(val[0].Substring(3, 2));
                Day = Convert.ToInt32(val[0].Substring(0, 2));
            }
            else
            {
                Month = Convert.ToInt32(val[0].Substring(0, 2));
                Day = Convert.ToInt32(val[0].Substring(3, 2));
            }

            if (Month > 12)
            {
                int temp = Month;
                Month = Day;
                Day = temp;
            }


            int Hour = 0;
            int min = 0;
            if (GraphView == 1)
            {
                DateTime dt = new DateTime(Year, Month, Day, Hour, min, 0);
                while (true)
                {
                    String dd = dt.DayOfWeek.ToString().ToUpper();
                    if (dd == "MONDAY")
                        return dt;
                    else
                        dt = dt.AddDays(-1);
                }
            }
            else if (GraphView == 2)
            {
                Day = 1;
            }

            return new DateTime(Year, Month, Day, Hour, min, 0);

        }

        private DataSet ConvertParameterToDataSet(DataSet dataSet, ArrayList parameterlist)
        {
            DataTable table = new DataTable();
            for (int i = 0; i < parameterlist.Count; i++)
                table.Columns.Add(new DataColumn(parameterlist[i].ToString(), typeof(System.String)));

            DateTime firstDate, lastDate, nextDate;
            if (dataSet == null)
            {
                MessageBox.Show("No data available.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }
            if (dataSet.Tables.Count == 0)
            {
                MessageBox.Show("No data available.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }
            if (dataSet.Tables[0].Rows.Count == 0)
            {
                MessageBox.Show("No data available.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }
            firstDate = GetStartdate(dataSet.Tables[0].Rows[0][0].ToString());
            lastDate = ConvertStringTodate(dataSet.Tables[0].Rows[0][0].ToString());

            nextDate = firstDate.AddMinutes(IntervalPeriod);

            if (firstDate != lastDate)
            {
                while (true)
                {
                    if (nextDate == lastDate)
                    {
                        break;
                    }
                    else
                    {
                        String defaultval = "0.00";
                        DataRow newrow = table.NewRow();
                        table.Rows.Add(newrow);

                        for (int listCount = 0; listCount < parameterlist.Count; listCount++)
                        {
                            if (parameterlist[listCount].ToString() == "Voltage R Phase")
                            {
                                newrow["Voltage R Phase"] = defaultval;
                            }
                            else if (parameterlist[listCount].ToString() == "Voltage Y Phase")
                            {
                                newrow["Voltage Y Phase"] = defaultval;
                            }
                            else if (parameterlist[listCount].ToString() == "Voltage B Phase")
                            {
                                newrow["Voltage B Phase"] = defaultval;
                            }
                            else if (parameterlist[listCount].ToString() == "Average Voltage")
                            {
                                newrow["Average Voltage"] = defaultval;
                            }
                            else if (parameterlist[listCount].ToString() == "Current R Phase")
                            {
                                newrow["Current R Phase"] = defaultval;
                            }
                            else if (parameterlist[listCount].ToString() == "Current Y Phase")
                            {
                                newrow["Current Y Phase"] = defaultval;
                            }
                            else if (parameterlist[listCount].ToString() == "Current B Phase")
                            {
                                newrow["Current B Phase"] = defaultval;
                            }
                            else if (parameterlist[listCount].ToString() == "Average Current")
                            {
                                newrow["Average Current"] = defaultval;
                            }
                            else if (parameterlist[listCount].ToString() == "Demand kW")
                            {
                                newrow["Demand kW"] = defaultval;
                            }
                            else if (parameterlist[listCount].ToString() == "Demand kvar (lag)")
                            {
                                newrow["Demand kvar (lag)"] = defaultval;
                            }
                            else if (parameterlist[listCount].ToString() == "Demand kvar (lead)")
                            {
                                newrow["Demand kvar (lead)"] = defaultval;
                            }
                            else if (parameterlist[listCount].ToString() == "Demand kVA")
                            {
                                newrow["Demand kVA"] = defaultval;
                            }
                            else if (parameterlist[listCount].ToString() == "Energy kWh")
                            {
                                newrow["Energy kWh"] = defaultval;
                            }
                            else if (parameterlist[listCount].ToString() == "Energy kvarh (lag)")
                            {
                                newrow["Energy kvarh (lag)"] = defaultval;
                            }
                            else if (parameterlist[listCount].ToString() == "Energy kvarh (lead)")
                            {
                                newrow["Energy kvarh (lead)"] = defaultval;
                            }
                            else if (parameterlist[listCount].ToString() == "Energy kVAh")
                            {
                                newrow["Energy kVAh"] = defaultval;
                            }
                            else if (parameterlist[listCount].ToString() == "LoadSurveyDateTime")
                            {
                                newrow["LoadSurveyDateTime"] = DateUtility.LongToStringDateTimeFormat(DateUtility.DateTimeToLong(nextDate));
                            }
                        }

                        firstDate = nextDate;
                        nextDate = firstDate.AddMinutes(IntervalPeriod);
                    }
                }
            }
            foreach (DataRow dr in dataSet.Tables[0].Rows)
            {
                DataRow newrow = table.NewRow();
                table.Rows.Add(newrow);
                for (int listCount = 0; listCount < parameterlist.Count; listCount++)
                {
                    if (parameterlist[listCount].ToString() == "Voltage R Phase")
                    {
                        newrow["Voltage R Phase"] = Convert.ToString(dr["Voltage R Phase"]);
                    }
                    else if (parameterlist[listCount].ToString() == "Voltage Y Phase")
                    {
                        newrow["Voltage Y Phase"] = Convert.ToString(dr["Voltage Y Phase"]);
                    }
                    else if (parameterlist[listCount].ToString() == "Voltage B Phase")
                    {
                        newrow["Voltage B Phase"] = Convert.ToString(dr["Voltage B Phase"]);
                    }
                    else if (parameterlist[listCount].ToString() == "Average Voltage")
                    {
                        newrow["Average Voltage"] = Convert.ToString(dr["Average Voltage"]);
                    }
                    else if (parameterlist[listCount].ToString() == "Current R Phase")
                    {
                        newrow["Current R Phase"] = Convert.ToString(dr["Current R Phase"]);
                    }
                    else if (parameterlist[listCount].ToString() == "Current Y Phase")
                    {
                        newrow["Current Y Phase"] = Convert.ToString(dr["Current Y Phase"]);
                    }
                    else if (parameterlist[listCount].ToString() == "Current B Phase")
                    {
                        newrow["Current B Phase"] = Convert.ToString(dr["Current B Phase"]);
                    }
                    else if (parameterlist[listCount].ToString() == "Average Current")
                    {
                        newrow["Average Current"] = Convert.ToString(dr["Average Current"]);
                    }
                    else if (parameterlist[listCount].ToString() == "Demand kW")
                    {
                        newrow["Demand kW"] = Convert.ToString(dr["Demand kW"]);
                    }
                    else if (parameterlist[listCount].ToString() == "Demand kvar (lag)")
                    {
                        newrow["Demand kvar (lag)"] = Convert.ToString(dr["Demand kvar (lag)"]);
                    }

                    else if (parameterlist[listCount].ToString() == "Demand kvar (lead)")
                    {
                        newrow["Demand kvar (lead)"] = Convert.ToString(dr["Demand kvar (lead)"]);
                    }
                    else if (parameterlist[listCount].ToString() == "Demand kVA")
                    {
                        newrow["Demand kVA"] = Convert.ToString(dr["Demand kVA"]);
                    }

                    else if (parameterlist[listCount].ToString() == "Energy kWh")
                    {
                        newrow["Energy kWh"] = Convert.ToString(dr["Energy kWh"]);
                    }
                    else if (parameterlist[listCount].ToString() == "Energy kvarh (lag)")
                    {
                        newrow["Energy kvarh (lag)"] = Convert.ToString(dr["Energy kvarh (lag)"]);
                    }
                    else if (parameterlist[listCount].ToString() == "Energy kvarh (lead)")
                    {
                        newrow["Energy kvarh (lead)"] = Convert.ToString(dr["Energy kvarh (lead)"]);
                    }
                    else if (parameterlist[listCount].ToString() == "Energy kVAh")
                    {
                        newrow["Energy kVAh"] = Convert.ToString(dr["Energy kVAh"]);
                    }
                    else if (parameterlist[listCount].ToString() == "LoadSurveyDateTime")
                    {
                        newrow["LoadSurveyDateTime"] = Convert.ToString(dr[0]);
                    }
                }
            }
            DataSet ds = new DataSet();
            ds.Tables.Add(table);

            return ds;
        }

        //DateTime ConvertStringTodate(String strdate)
        //{
        //    string[] val = strdate.Split(' ');
        //    int Year = Convert.ToInt32(val[0].Substring(6, 4));
        //    int Month = Convert.ToInt32(val[0].Substring(3, 2));
        //    int Day = Convert.ToInt32(val[0].Substring(0, 2));
        //    int Hour = Convert.ToInt32(val[1]);
        //    int min = Convert.ToInt32(val[3]);
        //    return new DateTime(Year, Month, Day, Hour, min, 0);

        //}

        DateTime ConvertStringTodate(String strdate)
        {
            string[] val = strdate.Split(' ');
            int Year = Convert.ToInt32(val[0].Substring(6, 4));
            int Month;// = Convert.ToInt32(val[0].Substring(3, 2));
            int Day;// = Convert.ToInt32(val[0].Substring(0, 2));
            int Hour = Convert.ToInt32(val[1]);
            int min = Convert.ToInt32(val[3]);
            DateTime dt = new DateTime();

            if (ConfigInfo.DateFormat() == "dd/MM/yyyy")
            {
                Month = Convert.ToInt32(val[0].Substring(3, 2));
                Day = Convert.ToInt32(val[0].Substring(0, 2));
            }
            else
            {
                Month = Convert.ToInt32(val[0].Substring(0, 2));
                Day = Convert.ToInt32(val[0].Substring(3, 2));
            }

            if (Month > 12)
            {
                int temp = Month;
                Month = Day;
                Day = temp;

                string strDate = Day.ToString() + "/" + Month.ToString() + "/" + Year.ToString() + " " + Hour.ToString() + ":" + min.ToString();
                dt = Convert.ToDateTime(strDate, new System.Globalization.CultureInfo("en-GB"));

            }
            else
            {
                dt = new DateTime(Year, Month, Day, Hour, min, 0);
            }

            return dt;

            
            //return new DateTime(Year, Month, Day, Hour, min, 0);

        }

        void DrawGraph(ArrayList ParameterList)
        {
            //Getting the Padded DataSet for the first time when the graph is selected
            DataSet dataSet = null;
            List<string> columnList = new List<string>();

            //Old Code Commented
            dataSet = LoadPaddingData(ParameterList);//this.FileName, this.MeterID);
            if ((dataSet != null) && (dataSet.Tables.Count != 0) && (dataSet.Tables[0].Rows.Count != 0))
            {
                GraphDataSet = dataSet;
            }
            else
            {
                return;
            }

            firstDate = ConvertStringTodate(dataSet.Tables[0].Rows[0]["LoadSurveyDateTime"].ToString());
            lastDate = ConvertStringTodate(dataSet.Tables[0].Rows[dataSet.Tables[0].Rows.Count-1]["LoadSurveyDateTime"].ToString());
            TimeSpan interval = lastDate.Subtract(firstDate);

            if (GraphView == 2)
            {
                numberOfDays = System.DateTime.DaysInMonth(firstDate.Year, firstDate.Month);

            }
            //Setting the MaxValue and the Interval Period
            if (IntervalPeriod == 15)//(Convert.ToInt16(dataSet.Tables[0].Rows[0]["MDIntervalPeriod"]) == 30) 
                MaxValue = 96;
            else if (IntervalPeriod == 30)//(Convert.ToInt16(dataSet.Tables[0].Rows[0]["MDIntervalPeriod"]) == 30) 
                MaxValue = 48;
            else if (IntervalPeriod == 60)//(Convert.ToInt16(dataSet.Tables[0].Rows[0]["MDIntervalPeriod"]) == 30)
                MaxValue = 24;
            else
                return;
            //Daily is 25 records per page view,336 for weekly Reports and 336* 4 for Monthly if interval is 30 minutes and records are 48 per day


            if (GraphView == 1)
                MaxValue = MaxValue * 7;
            else if (GraphView == 2)
                MaxValue = MaxValue * numberOfDays;

            if (GraphView == 0)
            {
                tslabelDate.Text = String.Format("{0:00}", firstDate.Day) + "/" + String.Format("{0:00}", firstDate.Month) + "/" + String.Format("{0:0000}", firstDate.Year);
                nextDate = firstDate;
            }
            else if (GraphView == 1)
            {
                tslabelDate.Text = String.Format("{0:00}", firstDate.Day) + "/" + String.Format("{0:00}", firstDate.Month) + "/" + String.Format("{0:0000}", firstDate.Year);
                nextDate = firstDate;
                lastDate = nextDate.AddDays(7);
                tslabelDate.Text = tslabelDate.Text + "\n - \n" + String.Format("{0:00}", lastDate.Day) + "/" + String.Format("{0:00}", lastDate.Month) + "/" + String.Format("{0:0000}", lastDate.Year);
            }
            else if (GraphView == 2)
            {
                tslabelDate.Text = String.Format("{0:00}", firstDate.Day) + "/" + String.Format("{0:00}", firstDate.Month) + "/" + String.Format("{0:0000}", firstDate.Year);
                nextDate = firstDate;
                lastDate = nextDate.AddDays(numberOfDays);
                tslabelDate.Text = tslabelDate.Text + "\n - \n" + String.Format("{0:00}", lastDate.Day) + "/" + String.Format("{0:00}", lastDate.Month) + "/" + String.Format("{0:0000}", lastDate.Year);

            }
            DisplayChart();
        }
        private void frmLoadsurveyGraph_Load(object sender, EventArgs e)
        {
            value = false;
            cboParameters.SelectedIndex = 2;
            toolStripPrintType.SelectedIndex = 0;
            ParameterType = cboParameters.Text;
            GraphView = 0;
            ChartType = 0;
            ShowGrids = false;
            GraphType = false;
            CurrentRecord = 0;
            Viewtype = false;
            currentRecordCount = 0;
            txtNoLoadIPCnt.Text = noLoadIPCount.ToString();
            txtNoPowerIPCnt.Text = noPowerIPCount.ToString();
            if (UtilityDetails.UtilityName == UtilityEntity.UGVCL || UtilityDetails.UtilityName == UtilityEntity.PVVNL || UtilityDetails.UtilityName != UtilityEntity.JDVVNL)
            {
                label6.Visible = false;
                label7.Visible = false;
                txtNoLoadIPCnt.Visible = false;
                txtNoPowerIPCnt.Visible = false;
            }
            
        }
        private void tsDaily_Click(object sender, EventArgs e)
        {
            this.txtViewType.Text = "Daily";
            GraphView = 0;
            CurrentRecord = 0;
            currentRecordCount = 0;
            DrawGraph(selectedParameterList);

        }
        private void tsWeekly_Click(object sender, EventArgs e)
        {
            this.txtViewType.Text = "Weekly";
            GraphView = 1;
            CurrentRecord = 0;
            currentRecordCount = 0;
            DrawGraph(selectedParameterList);

        }
        private void tsMonthly_Click(object sender, EventArgs e)
        {
            this.txtViewType.Text = "Monthly";
            GraphView = 2;
            CurrentRecord = 0;
            currentRecordCount = 0;
            DrawGraph(selectedParameterList);

        }
        private void tsgrid_Click(object sender, EventArgs e)
        {
            if (GraphDataSet == null)
                return;
            if (GraphDataSet.Tables.Count == 0)
                return;
            if (GraphDataSet.Tables[0].Rows.Count == 0)
                return;
            if (ShowGrids == false)
            {

                ShowGrids = true;
                DisplayChart();
            }
            else
            {

                ShowGrids = false;
                DisplayChart();
            }
        }
        private void tsPre_Click(object sender, EventArgs e)
        {
            if (GraphDataSet == null)
                return;
            if (GraphDataSet.Tables.Count == 0)
                return;
            if (GraphDataSet.Tables[0].Rows.Count == 0)
                return;
            PreviousPage();
        }
        private void PreviousPage()
        {
            if (GraphView == 0)
            {
                if (currentRecordCount >= MaxValue)
                {
                    nextDate = nextDate.AddDays(-1);
                    tslabelDate.Text = String.Format("{0:00}", nextDate.Day) + "/" + String.Format("{0:00}", nextDate.Month) + "/" + String.Format("{0:0000}", nextDate.Year);
                    currentRecordCount = currentRecordCount - MaxValue;
                    CurrentRecord = currentRecordCount;
                    DisplayChart();

                }
            }
            else if (GraphView == 1)
            {
                if (currentRecordCount >= (MaxValue))
                {
                    nextDate = nextDate.AddDays(-7);
                    tslabelDate.Text = String.Format("{0:00}", nextDate.Day) + "/" + String.Format("{0:00}", nextDate.Month) + "/" + String.Format("{0:0000}", nextDate.Year);
                    lastDate = nextDate.AddDays(7);
                    tslabelDate.Text = tslabelDate.Text + "\n - \n" + String.Format("{0:00}", lastDate.Day) + "/" + String.Format("{0:00}", lastDate.Month) + "/" + String.Format("{0:0000}", lastDate.Year);
                    currentRecordCount = currentRecordCount - (MaxValue);
                    CurrentRecord = currentRecordCount;
                    DisplayChart();
                }
            }
            else if (GraphView == 2)
            {
                if (currentRecordCount >= (MaxValue))//* 7 * 4))
                {
                    if (nextDate.Month != 1)
                        numberOfDays = System.DateTime.DaysInMonth(nextDate.Year, nextDate.Month - 1);
                    else
                        numberOfDays = 31;

                    nextDate = nextDate.AddDays(-numberOfDays);
                    tslabelDate.Text = String.Format("{0:00}", nextDate.Day) + "/" + String.Format("{0:00}", nextDate.Month) + "/" + String.Format("{0:0000}", nextDate.Year);
                    lastDate = nextDate.AddDays(numberOfDays);
                    tslabelDate.Text = tslabelDate.Text + "\n - \n" + String.Format("{0:00}", lastDate.Day) + "/" + String.Format("{0:00}", lastDate.Month) + "/" + String.Format("{0:0000}", lastDate.Year);
                    currentRecordCount = currentRecordCount - (MaxValue);
                    CurrentRecord = currentRecordCount;
                    DisplayChart();
                }
            }
        }
        private void tsNext_Click(object sender, EventArgs e)
        {
            if (GraphDataSet == null)
                return;
            if (GraphDataSet.Tables.Count == 0)
                return;
            if (GraphDataSet.Tables[0].Rows.Count == 0)
                return;
            NextPage();

        }
        private void NextPage()
        {
            //Get the Record Count for the dataset that is generated
            recordCount = GetRecordCount(GraphDataSet);
            if (GraphView == 0)
            {
                if (recordCount > currentRecordCount + MaxValue)
                {
                    nextDate = nextDate.AddDays(1);
                    tslabelDate.Text = String.Format("{0:00}", nextDate.Day) + "/" + String.Format("{0:00}", nextDate.Month) + "/" + String.Format("{0:0000}", nextDate.Year);
                    currentRecordCount = currentRecordCount + MaxValue;
                    CurrentRecord = currentRecordCount;
                    DisplayChart();

                }
            }
            else if (GraphView == 1)
            {
                if (recordCount > (currentRecordCount + (MaxValue)))
                {
                    nextDate = nextDate.AddDays(7);
                    tslabelDate.Text = String.Format("{0:00}", nextDate.Day) + "/" + String.Format("{0:00}", nextDate.Month) + "/" + String.Format("{0:0000}", nextDate.Year);
                    lastDate = nextDate.AddDays(7);
                    tslabelDate.Text = tslabelDate.Text + "\n - \n" + String.Format("{0:00}", lastDate.Day) + "/" + String.Format("{0:00}", lastDate.Month) + "/" + String.Format("{0:0000}", lastDate.Year);
                    currentRecordCount = currentRecordCount + (MaxValue);
                    CurrentRecord = currentRecordCount;
                    DisplayChart();
                }
            }
            else if (GraphView == 2)
            {
                if (recordCount > (currentRecordCount + (MaxValue)))
                {
                    numberOfDays = System.DateTime.DaysInMonth(nextDate.Year, nextDate.Month);
                    nextDate = nextDate.AddDays(numberOfDays);
                    tslabelDate.Text = String.Format("{0:00}", nextDate.Day) + "/" + String.Format("{0:00}", nextDate.Month) + "/" + String.Format("{0:0000}", nextDate.Year);
                    numberOfDays = System.DateTime.DaysInMonth(nextDate.Year, nextDate.Month);
                    lastDate = nextDate.AddDays(numberOfDays);
                    tslabelDate.Text = tslabelDate.Text + "\n - \n" + String.Format("{0:00}", lastDate.Day) + "/" + String.Format("{0:00}", lastDate.Month) + "/" + String.Format("{0:0000}", lastDate.Year);
                    currentRecordCount = currentRecordCount + (MaxValue);
                    CurrentRecord = currentRecordCount;
                    DisplayChart();

                }
            }
        }
        private int GetRecordCount(DataSet dSet)
        {
            if (dSet != null)
            {
                if (dSet.Tables[0].Rows.Count > 0)
                {
                    return dSet.Tables[0].Rows.Count;
                }
            }
            return 0;
        }
        private void tsFirst_Click(object sender, EventArgs e)
        {
            if (GraphDataSet == null)
                return;
            if (GraphDataSet.Tables.Count == 0)
                return;
            if (GraphDataSet.Tables[0].Rows.Count == 0)
                return;
            currentRecordCount = 0;
            CurrentRecord = 0;
            if (GraphView == 0)
            {
                nextDate = firstDate;
                tslabelDate.Text = String.Format("{0:00}", nextDate.Day) + "/" + String.Format("{0:00}", nextDate.Month) + "/" + String.Format("{0:0000}", nextDate.Year);
            }
            else if (GraphView == 1)
            {
                tslabelDate.Text = String.Format("{0:00}", firstDate.Day) + "/" + String.Format("{0:00}", firstDate.Month) + "/" + String.Format("{0:0000}", firstDate.Year);
                nextDate = firstDate;
                lastDate = nextDate.AddDays(7);
                tslabelDate.Text = tslabelDate.Text + "\n - \n" + String.Format("{0:00}", lastDate.Day) + "/" + String.Format("{0:00}", lastDate.Month) + "/" + String.Format("{0:0000}", lastDate.Year);
            }
            else if (GraphView == 2)
            {
                tslabelDate.Text = String.Format("{0:00}", firstDate.Day) + "/" + String.Format("{0:00}", firstDate.Month) + "/" + String.Format("{0:0000}", firstDate.Year);
                nextDate = firstDate;
                lastDate = nextDate.AddDays(numberOfDays);
                tslabelDate.Text = tslabelDate.Text + "\n - \n" + String.Format("{0:00}", lastDate.Day) + "/" + String.Format("{0:00}", lastDate.Month) + "/" + String.Format("{0:0000}", lastDate.Year);
            }
            DisplayChart();
        }
        private void tsSeperate_Click(object sender, EventArgs e)
        {
            if (GraphDataSet == null)
                return;
            if (GraphDataSet.Tables.Count == 0)
                return;
            if (GraphDataSet.Tables[0].Rows.Count == 0)
                return;
            GraphType = true;
            DisplayChart();
        }
        private void tsTogether_Click(object sender, EventArgs e)
        {
            if (GraphDataSet == null)
                return;
            if (GraphDataSet.Tables.Count == 0)
                return;
            if (GraphDataSet.Tables[0].Rows.Count == 0)
                return;
            GraphType = false;
            DisplayChart();
        }
        private void tsLast_Click(object sender, EventArgs e)
        {
            if (GraphDataSet == null)
                return;
            if (GraphDataSet.Tables.Count == 0)
                return;
            if (GraphDataSet.Tables[0].Rows.Count == 0)
                return;
            //Get the Record Count for the dataset that is generated
            recordCount = GetRecordCount(GraphDataSet);

            if (GraphView == 0)
            {
                nextDate = ConvertStringTodate(Convert.ToString(GraphDataSet.Tables[0].Rows[recordCount - 1]["LoadSurveyDateTime"]));
                tslabelDate.Text = String.Format("{0:00}", nextDate.Day) + "/" + String.Format("{0:00}", nextDate.Month) + "/" + String.Format("{0:0000}", nextDate.Year);
                currentRecordCount = recordCount - (recordCount % MaxValue);

                CurrentRecord = currentRecordCount;
                DisplayChart();

            }
            else if (GraphView == 1)
            {
                currentRecordCount = recordCount - (recordCount % (MaxValue));

                CurrentRecord = currentRecordCount;
                DisplayChart();


            }
            else if (GraphView == 2)
            {
                currentRecordCount = recordCount - (recordCount % (MaxValue));

                CurrentRecord = currentRecordCount;
                DisplayChart();


            }
        }
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (GraphType == true)
            {
                for (int i = 0; i < GraphDataSet.Tables[0].Columns.Count - 1; i++)
                {
                    graphChart.ChartAreas["AREA" + i].AxisX.ScaleView.Zoom(0, MaxValue);
                    graphChart.ChartAreas["AREA" + i].AxisX.ScaleView.Zoomable = true;
                    graphChart.ChartAreas["AREA" + i].AxisX.ScrollBar.IsPositionedInside = true;
                }
            }
            else
            {
                graphChart.ChartAreas["ChartArea1"].AxisX.ScaleView.Zoom(0, MaxValue);
                graphChart.ChartAreas["ChartArea1"].AxisX.ScaleView.Zoomable = true;
                graphChart.ChartAreas["ChartArea1"].AxisX.ScrollBar.IsPositionedInside = true;
            }
        }
        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (GraphType == true)
            {
                for (int i = 0; i < GraphDataSet.Tables[0].Columns.Count - 1; i++)
                {
                    graphChart.ChartAreas["AREA" + i].AxisX.ScaleView.Zoom(0, MaxValue / 2);
                    graphChart.ChartAreas["AREA" + i].AxisX.ScaleView.Zoomable = true;
                    graphChart.ChartAreas["AREA" + i].AxisX.ScrollBar.IsPositionedInside = true;
                }
            }
            else
            {
                graphChart.ChartAreas["ChartArea1"].AxisX.ScaleView.Zoom(0, MaxValue / 2);
                graphChart.ChartAreas["ChartArea1"].AxisX.ScaleView.Zoomable = true;
                graphChart.ChartAreas["ChartArea1"].AxisX.ScrollBar.IsPositionedInside = true;
            }
        }
        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            if (GraphType == true)
            {
                for (int i = 0; i < GraphDataSet.Tables[0].Columns.Count - 1; i++)
                {
                    graphChart.ChartAreas["AREA" + i].AxisX.ScaleView.Zoom(0, MaxValue / 4);
                    graphChart.ChartAreas["AREA" + i].AxisX.ScaleView.Zoomable = true;
                    graphChart.ChartAreas["AREA" + i].AxisX.ScrollBar.IsPositionedInside = true;
                }
            }
            else
            {
                graphChart.ChartAreas["ChartArea1"].AxisX.ScaleView.Zoom(0, MaxValue / 4);
                graphChart.ChartAreas["ChartArea1"].AxisX.ScaleView.Zoomable = true;
                graphChart.ChartAreas["ChartArea1"].AxisX.ScrollBar.IsPositionedInside = true;
            }
        }
        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            if (GraphType == true)
            {
                for (int i = 0; i < GraphDataSet.Tables[0].Columns.Count - 1; i++)
                {
                    graphChart.ChartAreas["AREA" + i].AxisX.ScaleView.Zoom(0, MaxValue / 8);
                    graphChart.ChartAreas["AREA" + i].AxisX.ScaleView.Zoomable = true;
                    graphChart.ChartAreas["AREA" + i].AxisX.ScrollBar.IsPositionedInside = true;
                }
            }
            else
            {
                graphChart.ChartAreas["ChartArea1"].AxisX.ScaleView.Zoom(0, MaxValue / 8);
                graphChart.ChartAreas["ChartArea1"].AxisX.ScaleView.Zoomable = true;
                graphChart.ChartAreas["ChartArea1"].AxisX.ScrollBar.IsPositionedInside = true;
            }
        }
        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            if (GraphType == true)
            {
                for (int i = 0; i < GraphDataSet.Tables[0].Columns.Count - 1; i++)
                {
                    graphChart.ChartAreas["AREA" + i].AxisX.ScaleView.Zoom(0, MaxValue / 16);
                    graphChart.ChartAreas["AREA" + i].AxisX.ScaleView.Zoomable = true;
                    graphChart.ChartAreas["AREA" + i].AxisX.ScrollBar.IsPositionedInside = true;
                }
            }
            else
            {
                graphChart.ChartAreas["ChartArea1"].AxisX.ScaleView.Zoom(0, MaxValue / 16);
                graphChart.ChartAreas["ChartArea1"].AxisX.ScaleView.Zoomable = true;
                graphChart.ChartAreas["ChartArea1"].AxisX.ScrollBar.IsPositionedInside = true;
            }
        }
        private void splineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChartType = 2;  // 0- Line, 1 - Bar, 2 - Spline
            DisplayChart();
        }
        private void pieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChartType = 4;  // 0- Line, 1 - Bar, 2 - Spline
            DisplayChart();
        }
        private void LoadParameter()
        {
            bool res = false;
            int j = 0;
            for (int i = 0; i < chkLBParameterDisplay.Items.Count; i++)
            {
                if (chkLBParameterDisplay.GetItemCheckState(i) == System.Windows.Forms.CheckState.Checked)
                {
                    res = true;
                    j++;
                }

            }

            if (j <= 3)
            {
                lblSeries4.Visible = false;
                txtSeries4.Visible = false;
                clrSeries4.Visible = false;
            }
            else
            {
                lblSeries4.Visible = true;
                txtSeries4.Visible = true;
                clrSeries4.Visible = true;
            }
            if (j <= 2)
            {
                lblSeries3.Visible = false;
                txtSeries3.Visible = false;
                clrSeries3.Visible = false;
            }
            else
            {
                lblSeries3.Visible = true;
                txtSeries3.Visible = true;
                clrSeries3.Visible = true;
            }
            if (j <= 1)
            {
                lblSeries2.Visible = false;
                txtSeries2.Visible = false;
                clrSeries2.Visible = false;
            }
            else
            {
                lblSeries2.Visible = true;
                txtSeries2.Visible = true;
                clrSeries2.Visible = true;
            }
            if (j <= 0)
            {
                lblSeries1.Visible = false;
                txtSeries1.Visible = false;
                clrSeries1.Visible = false;
            }
            else
            {
                lblSeries1.Visible = true;
                txtSeries1.Visible = true;
                clrSeries1.Visible = true;
            }
            if (res == false)
            {
                graphChart.Series.Clear();
                //MessageBox.Show("Please select any parameter from the list.");
                return;
            }

            txtParamTime.Text = "";
            txtSeries1.Text = "";
            txtSeries2.Text = "";
            txtSeries3.Text = "";
            txtSeries4.Text = "";
            ////lblSeries1.Text = "";
            ////lblSeries2.Text = "";
            ////lblSeries3.Text = "";
            ////lblSeries4.Text = "";
            if (parameterName == "Voltage")
            {
                selectedParameterList.Clear();

                if (chkLBParameterDisplay.GetItemCheckState(0) == System.Windows.Forms.CheckState.Checked)
                {
                    selectedParameterList.Add("Voltage R Phase");
                }
                if (chkLBParameterDisplay.GetItemCheckState(1) == System.Windows.Forms.CheckState.Checked)
                {
                    selectedParameterList.Add("Voltage Y Phase");
                }
                if (chkLBParameterDisplay.GetItemCheckState(2) == System.Windows.Forms.CheckState.Checked)
                {
                    selectedParameterList.Add("Voltage B Phase");
                }
                if (chkLBParameterDisplay.GetItemCheckState(3) == System.Windows.Forms.CheckState.Checked)
                {
                    selectedParameterList.Add("Average Voltage");
                }
            }
            else if (parameterName == "Current")
            {
                selectedParameterList.Clear();
                if (chkLBParameterDisplay.GetItemCheckState(0) == System.Windows.Forms.CheckState.Checked)
                {
                    selectedParameterList.Add("Current R Phase");
                }
                if (chkLBParameterDisplay.GetItemCheckState(1) == System.Windows.Forms.CheckState.Checked)
                {
                    selectedParameterList.Add("Current Y Phase");
                }
                if (chkLBParameterDisplay.GetItemCheckState(2) == System.Windows.Forms.CheckState.Checked)
                {
                    selectedParameterList.Add("Current B Phase");
                }
                if (chkLBParameterDisplay.GetItemCheckState(3) == System.Windows.Forms.CheckState.Checked)
                {
                    selectedParameterList.Add("Average Current");
                }
            }
            else if (parameterName == "Demand")
            {
                selectedParameterList.Clear();
                if (chkLBParameterDisplay.GetItemCheckState(0) == System.Windows.Forms.CheckState.Checked)
                {
                    selectedParameterList.Add("Demand kW");
                }
                if (chkLBParameterDisplay.GetItemCheckState(1) == System.Windows.Forms.CheckState.Checked)
                {
                    selectedParameterList.Add("Demand kvar (lag)");
                }
                if (chkLBParameterDisplay.GetItemCheckState(2) == System.Windows.Forms.CheckState.Checked)
                {
                    selectedParameterList.Add("Demand kvar (lead)");
                }
                if (chkLBParameterDisplay.GetItemCheckState(3) == System.Windows.Forms.CheckState.Checked)
                {
                    selectedParameterList.Add("Demand kVA");
                }
            }
            else
            {
                selectedParameterList.Clear();
                if (chkLBParameterDisplay.GetItemCheckState(0) == System.Windows.Forms.CheckState.Checked)
                {
                    selectedParameterList.Add("Energy kWh");
                }
                if (chkLBParameterDisplay.GetItemCheckState(1) == System.Windows.Forms.CheckState.Checked)
                {
                    selectedParameterList.Add("Energy kvarh (lag)");
                }
                if (chkLBParameterDisplay.GetItemCheckState(2) == System.Windows.Forms.CheckState.Checked)
                {
                    selectedParameterList.Add("Energy kvarh (lead)");

                }
                if (chkLBParameterDisplay.GetItemCheckState(3) == System.Windows.Forms.CheckState.Checked)
                {
                    selectedParameterList.Add("Energy kVAh");

                }
            }
            selectedParameterList.Add("LoadSurveyDateTime");
            ParameterType = cboParameters.Text;
            DrawGraph(selectedParameterList);
            value = true;
            for (int i = 0; i < graphChart.Series.Count; i++)
            {
                if (i == 0)
                {
                    lblSeries1.Text = graphChart.Series[i].Name;
                }
                else if (i == 1)
                {
                    lblSeries2.Text = graphChart.Series[i].Name;
                }
                else if (i == 2)
                {
                    lblSeries3.Text = graphChart.Series[i].Name;
                }
                else if (i == 3)
                {
                    lblSeries4.Text = graphChart.Series[i].Name;
                }
            }
            SetColorForSeries();

        }
        private void cboParameters_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Clear the checklistBox
            chkLBParameterDisplay.Items.Clear();
            txtParamTime.Text = "";
            txtSeries1.Text = "";
            txtSeries2.Text = "";
            txtSeries3.Text = "";
            txtSeries4.Text = "";
            //Setting the Parameter Name of the selected Parameter Combo
            parameterName = cboParameters.Text.Trim();
            //Get the column names for the parameter list which are filled without null values
            if (parameterName == "Voltage")
            {
                chkLBParameterDisplay.Items.Add("R Phase Voltage");
                this.chkLBParameterDisplay.SetItemChecked(0, true);
                lblSeries1.Text = "R Phase Voltage";
                chkLBParameterDisplay.Items.Add("Y Phase Voltage");
                this.chkLBParameterDisplay.SetItemChecked(1, true);
                lblSeries2.Text = "Y Phase Voltage";
                chkLBParameterDisplay.Items.Add("B Phase Voltage");
                this.chkLBParameterDisplay.SetItemChecked(2, true);
                lblSeries3.Text = "B Phase Voltage";
                chkLBParameterDisplay.Items.Add("Average Voltage");
                this.chkLBParameterDisplay.SetItemChecked(2, true);
                lblSeries4.Text = "Average Voltage";
                currentRecordCount = 0;
            }
            else if (parameterName == "Current")
            {
                chkLBParameterDisplay.Items.Add("R Phase Current");
                this.chkLBParameterDisplay.SetItemChecked(0, true);
                lblSeries1.Text = "R Phase Current";
                chkLBParameterDisplay.Items.Add("Y Phase Current");
                this.chkLBParameterDisplay.SetItemChecked(1, true);
                lblSeries2.Text = "Y Phase Current";
                chkLBParameterDisplay.Items.Add("B Phase Current");
                this.chkLBParameterDisplay.SetItemChecked(2, true);
                lblSeries3.Text = "B Phase Current";
                chkLBParameterDisplay.Items.Add("Average Current");
                this.chkLBParameterDisplay.SetItemChecked(2, true);
                lblSeries4.Text = "Average Current";
                currentRecordCount = 0;
            }
            else if (parameterName == "Demand")
            {
                chkLBParameterDisplay.Items.Add("Demand kW");
                this.chkLBParameterDisplay.SetItemChecked(0, true);
                lblSeries1.Text = "Demand kW";
                chkLBParameterDisplay.Items.Add("Demand kvar Lag");
                this.chkLBParameterDisplay.SetItemChecked(1, true);
                lblSeries2.Text = "Demand kvar Lag";
                chkLBParameterDisplay.Items.Add("Demand kvar Lead");
                this.chkLBParameterDisplay.SetItemChecked(2, true);
                lblSeries3.Text = "Demand kvar Lead";
                chkLBParameterDisplay.Items.Add("Demand kVA");
                this.chkLBParameterDisplay.SetItemChecked(3, true);
                lblSeries4.Text = "Demand kVA";
                currentRecordCount = 0;
            }
            else
            {
                chkLBParameterDisplay.Items.Add("Energy kWh");
                this.chkLBParameterDisplay.SetItemChecked(0, true);
                lblSeries1.Text = "Energy kWh";
                chkLBParameterDisplay.Items.Add("Energy kvarh Lag");
                this.chkLBParameterDisplay.SetItemChecked(1, true);
                lblSeries2.Text = "Energy kvarh Lag";
                chkLBParameterDisplay.Items.Add("Energy kvarh Lead");
                this.chkLBParameterDisplay.SetItemChecked(2, true);
                lblSeries3.Text = "Energy kvarh Lead";
                chkLBParameterDisplay.Items.Add("Energy kVAh");
                this.chkLBParameterDisplay.SetItemChecked(3, true);
                lblSeries4.Text = "Energy kVAh";
                currentRecordCount = 0;
            }
            LoadParameter();
        }
        private void graphChart_MouseMove(object sender, MouseEventArgs e)
        {

            HitTestResult result = graphChart.HitTest(e.X, e.Y);
            if ((result.PointIndex >= 0) && (result.Series != null))
            {
                if (result.ChartElementType == ChartElementType.DataPoint)
                {
                    for (int i = 0; i < graphChart.Series.Count; i++)
                    {
                        DataPoint point = graphChart.Series[i].Points[result.PointIndex];
                        if (i == 0)
                        {
                            lblSeries1.Text = graphChart.Series[i].Name;
                            txtSeries1.Text = point.YValues[0].ToString();
                        }
                        else if (i == 1)
                        {
                            lblSeries2.Text = graphChart.Series[i].Name;
                            txtSeries2.Text = point.YValues[0].ToString();
                        }
                        else if (i == 2)
                        {
                            lblSeries3.Text = graphChart.Series[i].Name;
                            txtSeries3.Text = point.YValues[0].ToString();
                        }
                        else if (i == 3)
                        {
                            lblSeries4.Text = graphChart.Series[i].Name;
                            txtSeries4.Text = point.YValues[0].ToString();
                        }
                        if (GraphView == 0)
                        {
                            txtParamTime.Text = tslabelDate.Text + " " + point.AxisLabel;
                        }
                        else if (GraphView == 1)
                        {

                            string[] val = point.AxisLabel.Split('\n');
                            txtParamTime.Text = val[2] + " " + val[1] + " " + val[0];
                        }
                        else if (GraphView == 2)
                        {

                            string[] val = point.AxisLabel.Split('\n');
                            txtParamTime.Text = val[2] + " " + val[1] + " " + val[0];
                        }

                    }

                }
            }

        }
        private void chkLBParameterDisplay_SelectedValueChanged(object sender, EventArgs e)
        {
            if (value == true)
                LoadParameter();
        }
        private void clrSeries1_Click(object sender, EventArgs e)
        {
            if (GraphDataSet == null)
                return;
            if (GraphDataSet.Tables.Count == 0)
                return;
            if (GraphDataSet.Tables[0].Rows.Count == 0)
                return;
            if (colorDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                clrSeries1.BackColor = colorDialog1.Color;
                DisplayChart();
            }
        }
        private void clrSeries2_Click(object sender, EventArgs e)
        {
            if (GraphDataSet == null)
                return;
            if (GraphDataSet.Tables.Count == 0)
                return;
            if (GraphDataSet.Tables[0].Rows.Count == 0)
                return;
            if (colorDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                clrSeries2.BackColor = colorDialog1.Color;
                DisplayChart();
            }
        }
        private void clrSeries3_Click(object sender, EventArgs e)
        {
            if (GraphDataSet == null)
                return;
            if (GraphDataSet.Tables.Count == 0)
                return;
            if (GraphDataSet.Tables[0].Rows.Count == 0)
                return;
            if (colorDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                clrSeries3.BackColor = colorDialog1.Color;
                DisplayChart();
            }
        }
        private void clrSeries4_Click(object sender, EventArgs e)
        {
            if (GraphDataSet == null)
                return;
            if (GraphDataSet.Tables.Count == 0)
                return;
            if (GraphDataSet.Tables[0].Rows.Count == 0)
                return;
            if (colorDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                clrSeries4.BackColor = colorDialog1.Color;
                DisplayChart();
            }
        }
        private void toolStripButton2_Click(object sender, EventArgs e)
        {

            if (GraphDataSet == null)
                return;
            if (GraphDataSet.Tables.Count == 0)
                return;
            if (GraphDataSet.Tables[0].Rows.Count == 0)
                return;
            ChartType = 0;  // 0- Line, 1 - Bar, 2 - Spline
            DisplayChart();
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (GraphDataSet == null)
                return;
            if (GraphDataSet.Tables.Count == 0)
                return;
            if (GraphDataSet.Tables[0].Rows.Count == 0)
                return;
            ChartType = 1;  // 0- Line, 1 - Bar, 2 - Spline
            DisplayChart();
        }
        private void tsExport_Click(object sender, EventArgs e)
        {
            SaveDialog();
        }
        private void SaveDialog()
        {
            SaveFileDialog SaveFileDialog1 = new SaveFileDialog();
            SaveFileDialog1.Filter = "Bitmap (*.bmp)|*.bmp|JPEG (*.jpg)|*.jpg|EMF (*.emf)|*.emf|PNG (*.png)|*.png|GIF (*.gif)|*.gif|TIFF (*.tif)|*.tif";
            SaveFileDialog1.FilterIndex = 2;
            SaveFileDialog1.RestoreDirectory = true;

            DialogResult result = SaveFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                System.Windows.Forms.DataVisualization.Charting.ChartImageFormat format = new ChartImageFormat();
                if (SaveFileDialog1.FileName.EndsWith("jpg"))
                    format = ChartImageFormat.Jpeg;
                else if (SaveFileDialog1.FileName.EndsWith("emf"))
                    format = ChartImageFormat.Emf;
                else if (SaveFileDialog1.FileName.EndsWith("gif"))
                    format = ChartImageFormat.Gif;
                else if (SaveFileDialog1.FileName.EndsWith("png"))
                    format = ChartImageFormat.Png;
                else if (SaveFileDialog1.FileName.EndsWith("tif"))
                    format = ChartImageFormat.Tiff;

                graphChart.SaveImage(SaveFileDialog1.FileName, format);
            }
        }
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            try
            {

                // Set new print document with custom page printing event handler
                graphChart.Printing.PrintDocument = new PrintDocument();
                graphChart.Printing.PrintDocument.PrintPage += new PrintPageEventHandler(pd_PrintPage);


                // Set Low printer resolution
                foreach (PrinterResolution pr in graphChart.Printing.PrintDocument.PrinterSettings.PrinterResolutions)
                {
                    if (pr.Kind == PrinterResolutionKind.Low)
                    {
                        graphChart.Printing.PrintDocument.DefaultPageSettings.PrinterResolution = pr;
                        break;
                    }
                }


                // Print preview chart
                graphChart.Printing.PrintPreview();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Chart Control for .NET Framework", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        private int printingPageIndex = 0;
        private void pd_PrintPage(object sender, PrintPageEventArgs ev)
        {
            if (toolStripPrintType.SelectedIndex == 1)
            {
                // Print more pages
                ev.HasMorePages = true;

                // Calculate chart position rectangle
                Rectangle chartPosition = new Rectangle(100, 300, graphChart.Size.Width, graphChart.Size.Height);

                // Align chart position on the page
                float chartWidthScale = ((float)ev.MarginBounds.Width) / ((float)chartPosition.Width);
                float chartHeightScale = ((float)ev.MarginBounds.Height) / ((float)chartPosition.Height);
                chartPosition.Width = (int)(chartPosition.Width * Math.Min(chartWidthScale, chartHeightScale));
                chartPosition.Height = (int)(chartPosition.Height * Math.Min(chartWidthScale, chartHeightScale));
                if (GraphType == false)
                {
                    // Check if chart view was already set
                    if (double.IsNaN(graphChart.ChartAreas[0].AxisX.ScaleView.Position))
                    {
                        // Reset page index
                        printingPageIndex = 0;

                        // Set view
                        graphChart.ChartAreas[0].AxisX.ScaleView.Position = graphChart.ChartAreas[0].AxisX.Minimum;
                        graphChart.ChartAreas[0].AxisX.ScaleView.Size = 2;
                        //                graphChart.ChartAreas[0].AxisX.ScaleView.SizeType = DateTimeIntervalType.Months;
                    }

                    // Set chart title
                    ++printingPageIndex;
                    graphChart.Text = "Chart Page Number " + printingPageIndex.ToString();

                    // Draw chart on the printer graphisc
                    graphChart.Printing.PrintPaint(ev.Graphics, chartPosition);

                    // Scroll to the next view (2 months)
                    double currentPosition = graphChart.ChartAreas[0].AxisX.ScaleView.Position;

                    graphChart.ChartAreas[0].AxisX.ScaleView.Scroll(System.Windows.Forms.DataVisualization.Charting.ScrollType.LargeIncrement);


                    // Check if position was scrolled
                    if (currentPosition >= (graphChart.ChartAreas[0].AxisX.ScaleView.Position - 1.0))
                    {
                        // No more pages
                        ev.HasMorePages = false;

                        // Restore view state
                        graphChart.ChartAreas[0].AxisX.ScaleView.Position = double.NaN;
                        graphChart.ChartAreas[0].AxisX.ScaleView.Size = double.NaN;

                        // Remove chart title
                        //            graphChart.Text = "";
                    }
                }
                else
                {
                    int totalcolcount = GraphDataSet.Tables[0].Columns.Count - 1;
                    // Check if chart view was already set
                    if (double.IsNaN(graphChart.ChartAreas[0].AxisX.ScaleView.Position))
                    {
                        // Reset page index
                        printingPageIndex = 0;

                        for (int i = 0; i < totalcolcount; i++)
                        {
                            // Set view
                            graphChart.ChartAreas[i].AxisX.ScaleView.Position = graphChart.ChartAreas[i].AxisX.Minimum;
                            graphChart.ChartAreas[i].AxisX.ScaleView.Size = 2;
                            //                graphChart.ChartAreas[0].AxisX.ScaleView.SizeType = DateTimeIntervalType.Months;
                        }
                    }

                    // Set chart title
                    ++printingPageIndex;
                    graphChart.Text = "Chart Page Number " + printingPageIndex.ToString();

                    // Draw chart on the printer graphisc
                    graphChart.Printing.PrintPaint(ev.Graphics, chartPosition);

                    for (int i = 0; i < totalcolcount; i++)
                    {
                        // Scroll to the next view (2 months)
                        double currentPosition = graphChart.ChartAreas[i].AxisX.ScaleView.Position;
                        //MessageBox.Show(System.Windows.Forms.DataVisualization.Charting.ScrollType.LargeIncrement.ToString());
                        graphChart.ChartAreas[i].AxisX.ScaleView.Scroll(System.Windows.Forms.DataVisualization.Charting.ScrollType.LargeIncrement);

                        // Check if position was scrolled
                        if (currentPosition >= (graphChart.ChartAreas[i].AxisX.ScaleView.Position - 1.0))
                        {
                            // No more pages
                            ev.HasMorePages = false;

                            // Restore view state
                            graphChart.ChartAreas[i].AxisX.ScaleView.Position = double.NaN;
                            graphChart.ChartAreas[i].AxisX.ScaleView.Size = double.NaN;

                            // Remove chart title
                            //            graphChart.Text = "";
                        }
                    }
                }
            }
            else
            {
                // Calculate chart position rectangle
                Rectangle chartPosition = new Rectangle(100, 300, graphChart.Size.Width, graphChart.Size.Height);

                // Align chart position on the page
                float chartWidthScale = ((float)ev.MarginBounds.Width) / ((float)chartPosition.Width);
                float chartHeightScale = ((float)ev.MarginBounds.Height) / ((float)chartPosition.Height);
                chartPosition.Width = (int)(chartPosition.Width * Math.Min(chartWidthScale, chartHeightScale));
                chartPosition.Height = (int)(chartPosition.Height * Math.Min(chartWidthScale, chartHeightScale));
                // Draw chart on the printer graphisc
                graphChart.Printing.PrintPaint(ev.Graphics, chartPosition);
                printingPageIndex = 1;
            }
            // Calculate title string position
            Rectangle titlePosition = new Rectangle(100, 100, ev.MarginBounds.Width, ev.MarginBounds.Height);
            Font fontTitle = new Font("Arial", 10);
            MeterDataEntity meterDataEntity = new MeterDataBLL().GetDetailData(MeterDataId) as MeterDataEntity;
            string chartTitle = "Meter ID : " + meterDataEntity.MeterID;
            SizeF titleSize = ev.Graphics.MeasureString(chartTitle, fontTitle);
            titlePosition.Height = (int)titleSize.Height;

            // Draw charts title
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Near;
            ev.Graphics.DrawString(chartTitle, fontTitle, Brushes.Black, titlePosition, format);


            Rectangle titlePosition1 = new Rectangle(100, 1000, ev.MarginBounds.Width, ev.MarginBounds.Height);
            chartTitle = "Cabcon.";
            titleSize = ev.Graphics.MeasureString(chartTitle, fontTitle);
            titlePosition.Height = (int)titleSize.Height;

            // Draw charts title

            format.Alignment = StringAlignment.Near;
            ev.Graphics.DrawString(chartTitle, fontTitle, Brushes.Black, titlePosition1, format);

            Rectangle titlePosition2 = new Rectangle(400, 1000, ev.MarginBounds.Width, ev.MarginBounds.Height);
            chartTitle = "(" + printingPageIndex + ")";
            titleSize = ev.Graphics.MeasureString(chartTitle, fontTitle);
            titlePosition.Height = (int)titleSize.Height;

            // Draw charts title

            format.Alignment = StringAlignment.Near;
            ev.Graphics.DrawString(chartTitle, fontTitle, Brushes.Black, titlePosition2, format);


            Rectangle titlePosition3 = new Rectangle(100, 1000, ev.MarginBounds.Width, ev.MarginBounds.Height);
            //Changed on 1st march 2012 as per the bug report
            string chartDate = String.Concat( DateTime.Now.Day.ToString("00"), "/", DateTime.Now.Month.ToString("00"), "/", DateTime.Now.Year.ToString("0000")," ", DateTime.Now.Hour.ToString("00"), ":",DateTime.Now.Minute.ToString("00"));
            chartTitle = DATETITLE1 + chartDate;
            titleSize = ev.Graphics.MeasureString(chartTitle, fontTitle);
            titlePosition.Height = (int)titleSize.Height;

            // Draw charts title

            format.Alignment = StringAlignment.Far;
            ev.Graphics.DrawString(chartTitle, fontTitle, Brushes.Black, titlePosition3, format);

            //added on 1st march 2012 as per the bug report
            Rectangle titlePosition4 = new Rectangle(0, 1000, ev.MarginBounds.Width, ev.MarginBounds.Height);
            chartTitle = DATETITLE2;
            titleSize = ev.Graphics.MeasureString(chartTitle, fontTitle);
            titlePosition.Height = (int)titleSize.Height;
            format.Alignment = StringAlignment.Far;
            ev.Graphics.DrawString(chartTitle, fontTitle, Brushes.Black, titlePosition4, format);
            //added on 1st march 2012 as per the bug report
           
        }
        private void tsPrint_Click(object sender, EventArgs e)
        {
            try
            {

                // Set new print document with custom page printing event handler
                graphChart.Printing.PrintDocument = new PrintDocument();
                graphChart.Printing.PrintDocument.PrintPage += new PrintPageEventHandler(pd_PrintPage);


                // Set Low printer resolution
                foreach (PrinterResolution pr in graphChart.Printing.PrintDocument.PrinterSettings.PrinterResolutions)
                {
                    if (pr.Kind == PrinterResolutionKind.Low)
                    {
                        graphChart.Printing.PrintDocument.DefaultPageSettings.PrinterResolution = pr;
                        break;
                    }
                }


                // Print preview chart
                graphChart.Printing.Print(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Chart Control for .NET Framework", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }


        }
        private void toolStripChartType_Click(object sender, EventArgs e)
        {
            if (GraphDataSet == null)
                return;
            if (GraphDataSet.Tables.Count == 0)
                return;
            if (GraphDataSet.Tables[0].Rows.Count == 0)
                return;
            ChartType = 3;  // 0- Line, 1 - Bar, 2 - Spline
            DisplayChart();
        }
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            if (GraphDataSet == null)
                return;
            if (GraphDataSet.Tables.Count == 0)
                return;
            if (GraphDataSet.Tables[0].Rows.Count == 0)
                return;
            if (Viewtype == true)
            {
                Viewtype = false;
            }
            else
            {
                Viewtype = true;
            }
            DisplayChart();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }



        private void DisplayInfo(Double Position)
        {
            for (int i = 0; i < graphChart.Series.Count; i++)
            {
                DataPoint point = graphChart.Series[i].Points[Convert.ToInt32(Position)];
                if (i == 0)
                {
                    lblSeries1.Text = graphChart.Series[i].Name;
                    txtSeries1.Text = point.YValues[0].ToString();
                }
                else if (i == 1)
                {
                    lblSeries2.Text = graphChart.Series[i].Name;
                    txtSeries2.Text = point.YValues[0].ToString();
                }
                else if (i == 2)
                {
                    lblSeries3.Text = graphChart.Series[i].Name;
                    txtSeries3.Text = point.YValues[0].ToString();
                }
                else if (i == 3)
                {
                    lblSeries4.Text = graphChart.Series[i].Name;
                    txtSeries4.Text = point.YValues[0].ToString();
                }
                if (GraphView == 0)
                {
                    txtParamTime.Text = tslabelDate.Text + " " + point.AxisLabel;
                }
                else if (GraphView == 1)
                {

                    string[] val = point.AxisLabel.Split('\n');
                    txtParamTime.Text = val[2] + " " + val[1] + " " + val[0];
                }
                else if (GraphView == 2)
                {

                    string[] val = point.AxisLabel.Split('\n');
                    txtParamTime.Text = val[2] + " " + val[1] + " " + val[0];
                }

            }
        }
        private void graphChart_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                bool fwd = false;
                if (e.KeyValue == 37)
                {
                    if (graphChart.ChartAreas[0].CursorX.Position == 1)
                    {
                        //Previous page
                        PreviousPage();
                        graphChart.ChartAreas[0].CursorX.Position = MaxValue;
                        return;
                    }
                    double x = graphChart.ChartAreas[0].CursorX.Position - 1;
                    if (fwd == true)
                    {
                        x = x - 1;
                        fwd = false;
                    }

                    if (GraphType == false)
                    {

                        DisplayInfo(x - 1);
                    }
                    else
                    {
                        graphChart.ChartAreas[0].CursorX.SetCursorPosition(graphChart.ChartAreas[0].CursorX.Position);
                        graphChart.ChartAreas[1].CursorX.SetCursorPosition(graphChart.ChartAreas[0].CursorX.Position);
                        graphChart.ChartAreas[2].CursorX.SetCursorPosition(graphChart.ChartAreas[0].CursorX.Position);
                        graphChart.ChartAreas[3].CursorX.SetCursorPosition(graphChart.ChartAreas[0].CursorX.Position);
                        DisplayInfo(graphChart.ChartAreas[0].CursorX.Position);
                    }
                    graphChart.ChartAreas[0].CursorX.SetCursorPosition(x);

                }
                else if (e.KeyValue == 39)
                {

                    if (graphChart.ChartAreas[0].CursorX.Position == MaxValue)
                    {
                        //next Page

                        NextPage();
                        graphChart.ChartAreas[0].CursorX.Position = 0;
                        return;
                    }
                    double x = graphChart.ChartAreas[0].CursorX.Position;


                    if (GraphType == false)
                    {

                        DisplayInfo(x);

                    }
                    else
                    {

                        graphChart.ChartAreas[0].CursorX.SetCursorPosition(graphChart.ChartAreas[0].CursorX.Position);
                        graphChart.ChartAreas[1].CursorX.SetCursorPosition(graphChart.ChartAreas[0].CursorX.Position);
                        graphChart.ChartAreas[2].CursorX.SetCursorPosition(graphChart.ChartAreas[0].CursorX.Position);
                        graphChart.ChartAreas[3].CursorX.SetCursorPosition(graphChart.ChartAreas[0].CursorX.Position);
                        DisplayInfo(graphChart.ChartAreas[0].CursorX.Position);
                    }
                    fwd = true;
                    graphChart.ChartAreas[0].CursorX.SetCursorPosition(x + 1);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Start / end of data reached.", "Cabcon", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }


    }
}
