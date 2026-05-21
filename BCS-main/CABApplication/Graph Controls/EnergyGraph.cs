using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Windows.Forms.DataVisualization.Charting;
using CAB.Framework;
using CAB.Framework.Utility;
using CAB.BLL;
using Hunt.EPIC.Logging;

namespace CAB.UI.Graphs
{
	public partial class EnergyGraph : UserControl
	{
		CalloutAnnotation calloutAnnotation;
		private DataSet _graphDataSet;
		private string _parameterType;
		private GraphView _graphView;
		private ChartType _chartType;
		private int _maxValue = 0;
		private int maxRecord = 0;
		private string seriesName = string.Empty;
		private int _currentRecord;
		private double value;
		private bool _showGrids; 
		private int _intervalPeriod;
		private Color selectedColorRPhase, selectedColorYPhase, selectedColorBPhase, selectedColorDemand;
		public int graphColorCount = 0;
		private string selectedSeriesName = string.Empty;
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(EnergyGraph).ToString());
		//delegate to display the mouse move values to the main User Control
		public delegate void GraphMouseMove(string dateTime, string Value1,string Value2, string Value3,string Value4);
		public event GraphMouseMove OnMouseMoveInGraph;

		public void OnGraphMouseMove(string dateTime, string Value1, string Value2, string Value3, string Value4)
		{
			if (OnMouseMoveInGraph != null)
				OnMouseMoveInGraph(dateTime, Value1, Value2, Value3, Value4);
		}

		/// <summary>
		/// To Get the DataSet from the Graph Form Control ie the Padded Dataset 
		/// Whichever column the values are available in the Load Survey Table
		/// </summary>
		public DataSet GraphDataSet
		{
			get
			{
				return _graphDataSet;
			}
			set
			{
				_graphDataSet = value;
			}
		}

		/// <summary>
		/// Voltage ,Curent or Demand Parameters
		/// </summary>
		public string ParameterType
		{
			get
			{
				return _parameterType;
			}
			set
			{
				_parameterType = value;
			}
		}

		/// <summary>
		/// Bar Chart, Line Chart or Spline Chart
		/// </summary>
		public ChartType ChartType
		{
			get
			{
				return _chartType;
			}
			set
			{
				_chartType = value;
			}
		}

		/// <summary>
		/// Daily, Weekly or Monthly View
		/// </summary>
		public GraphView GraphView
		{
			get
			{
				return _graphView;
			}
			set
			{
				_graphView = value;
			}
		}

		/// <summary>
		/// The Current Records that has to be shown for the current Page
		/// </summary>
		public int CurrentRecord
		{
			get
			{
				return _currentRecord;
			}
			set
			{
				_currentRecord=value;
			}
		}

		/// <summary>
		/// 48 for 30 minutes Interval, 96 for 15 Minutes Interval
		/// </summary>
		public int MaxValue
		{
			get
			{
				return _maxValue;
			}
			set
			{
				_maxValue = value;
			}
		}

		/// <summary>
		/// 30 Minutes Interval or 15 Minutes Interval
		/// </summary>
		public int IntervalPeriod
		{
			get
			{
				return _intervalPeriod;
			}
			set
			{
				_intervalPeriod = value;
			}
		}

		/// <summary>
		/// Whether to show the Grid Lines in the Graph or not
		/// </summary>
		public bool ShowGrids
		{
			get
			{
				return _showGrids;
			}
			set
			{
				_showGrids = value;
			}
		}

		 

		public EnergyGraph()
		{
			InitializeComponent();
			selectedColorRPhase = Color.Red;
			selectedColorYPhase = Color.Yellow;
			selectedColorBPhase = Color.Blue;
			selectedColorDemand = Color.Green;
		}

		public void DisplayChart()
		{
			try
			{
				int totalXAxisCoumn = 0;
				if (IntervalPeriod == 30)
					totalXAxisCoumn = 48;
				else
					totalXAxisCoumn = 96;  
				if ((GraphDataSet == null) || (GraphDataSet.Tables.Count == 0))
					return;
				DateTime currentRecDate = new DateTime();
				maxRecord = GraphDataSet.Tables[0].Rows.Count;
				if (maxRecord <= 0) return;
				DataTable graphTable = SortAndPrepareColumn(GraphDataSet, totalXAxisCoumn);
				if (graphTable == null)
					return;
				if (graphTable.Rows.Count == 0)
					return; 
				graphChart.ChartAreas.Clear();
				graphChart.ChartAreas.Add("ChartArea1");
				graphChart.Series.Clear(); 
				
				if (GraphView == GraphView.Daily)
				{ 
					graphChart.ChartAreas["ChartArea1"].AxisX.Minimum = 0;
					graphChart.ChartAreas["ChartArea1"].AxisX.Maximum = totalXAxisCoumn; 
					for (int colCount = 0; colCount < GraphDataSet.Tables[0].Columns.Count - 1; colCount++)
					{ 
						seriesName = GraphDataSet.Tables[0].Columns[colCount].ToString();
						graphChart.Series.Add(seriesName); 
						SetChartType(graphChart); 
						graphChart.Series[seriesName].BorderWidth = 3; 
						foreach (DataRow dRow in graphTable.Rows)
						{
							string xAxisValue = Convert.ToDateTime(dRow["loadSurveyDateTime"].ToString()).ToString("dd/MM/yyyy HH:mm");
							double yAxisValue = Convert.ToDouble(dRow[colCount].ToString());
							graphChart.Series[seriesName].Points.AddXY(xAxisValue, yAxisValue);
						}
					}
				}


				else if (GraphView == GraphView.Weekly)
				{
					for (int colCount = 0; colCount < graphTable.Columns.Count-1 ; colCount++)
					{
						seriesName = graphTable.Columns[colCount].ToString();
						graphChart.Series.Add(seriesName);
						SetChartType(graphChart);
						graphChart.Series[seriesName].BorderWidth = 3;
						foreach (DataRow dRow in graphTable.Rows)
						{
							if (double.TryParse(dRow[colCount].ToString(), out value))
							{
								DateTime xAxisValue = Convert.ToDateTime(dRow["loadSurveyDateTime"].ToString());
								double yAxisValue = Convert.ToDouble(dRow[colCount].ToString());
								graphChart.Series[seriesName].Points.AddXY(xAxisValue, yAxisValue);
							}
						}
					}
				}

				else if (GraphView == GraphView.Monthly)
				{  
					for (int colCount = 0; colCount < graphTable.Columns.Count-1 ; colCount++)
					{
						seriesName = graphTable.Columns[colCount].ToString();
						graphChart.Series.Add(seriesName); 
						SetChartType(graphChart); 
						graphChart.Series[seriesName].BorderWidth = 1; 
						foreach (DataRow dRow in graphTable.Rows)
						{
							if (double.TryParse(dRow[colCount].ToString(), out value))
							{ 
								currentRecDate = Convert.ToDateTime(dRow["loadSurveyDateTime"].ToString());
								DateTime xAxisValue = Convert.ToDateTime(dRow["loadSurveyDateTime"].ToString());
								double yAxisValue = Convert.ToDouble(dRow[colCount].ToString());
								graphChart.Series[seriesName].Points.AddXY(xAxisValue, yAxisValue);
							}
						}
					}
				} 
				graphChart.ChartAreas["ChartArea1"].BackColor = Color.FromArgb(64, 165, 191, 228);
				graphChart.ChartAreas["ChartArea1"].BackGradientStyle = GradientStyle.TopBottom;
				graphChart.ChartAreas["ChartArea1"].BackSecondaryColor = Color.White;
				graphChart.BackColor = Color.FromArgb(211, 223, 240);
				graphChart.BorderlineColor = Color.FromArgb(26, 59, 105);
				graphChart.BorderlineDashStyle = ChartDashStyle.Solid; 
				graphChart.ChartAreas["ChartArea1"].CursorX.Interval = 1;
				graphChart.ChartAreas["ChartArea1"].CursorY.Interval = 1; 
				graphChart.ChartAreas["ChartArea1"].CursorX.IsUserEnabled = true;
				graphChart.ChartAreas["ChartArea1"].CursorY.IsUserEnabled = true;

				if (GraphView == GraphView.Weekly)
				{ 
					//graphChart.ChartAreas["ChartArea1"].AxisX.Interval = 672;
					graphChart.ChartAreas["ChartArea1"].AxisX.IntervalType = DateTimeIntervalType.Auto;
					graphChart.ChartAreas["ChartArea1"].AxisX.LabelStyle.Format = "{0:ddd,d/MM}"; 
				}
				else if (GraphView == GraphView.Monthly)
				{
					int val =  new LoadSurveyBLL().GetEndDay(currentRecDate);
					graphChart.ChartAreas["ChartArea1"].AxisX.Interval = val-1;
					graphChart.ChartAreas["ChartArea1"].AxisX.IntervalType = DateTimeIntervalType.Days;
					graphChart.ChartAreas["ChartArea1"].AxisX.IntervalOffset = 1;
					graphChart.ChartAreas["ChartArea1"].AxisX.IntervalOffsetType = DateTimeIntervalType.Days;
					graphChart.ChartAreas["ChartArea1"].AxisX.LabelStyle.Format = "{0:ddd,d/MM}";//"{0:dd/MM/yyyy}";  
				}
				else
				{
					graphChart.ChartAreas["ChartArea1"].AxisX.MinorGrid.Enabled = true;
					int val=0;
					if (IntervalPeriod == 15)
						val = 4;
					else
						val = 2;
					graphChart.ChartAreas["ChartArea1"].AxisX.LabelStyle.Interval = val;
					graphChart.ChartAreas["ChartArea1"].AxisX.MajorGrid.Interval = val;
					graphChart.ChartAreas["ChartArea1"].AxisX.MajorTickMark.Interval = val; 
					graphChart.ChartAreas["ChartArea1"].AxisY.MinorGrid.Interval = 1; 
					graphChart.ChartAreas["ChartArea1"].AxisY.MinorTickMark.Interval = 1;
					graphChart.ChartAreas["ChartArea1"].AxisX.MinorGrid.LineDashStyle = ChartDashStyle.NotSet;
					graphChart.ChartAreas["ChartArea1"].AxisY.MinorGrid.LineDashStyle = ChartDashStyle.NotSet; 
					graphChart.ChartAreas["ChartArea1"].AxisX.LabelAutoFitStyle = LabelAutoFitStyles.LabelsAngleStep90;
				}

				graphChart.ChartAreas["ChartArea1"].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
				graphChart.ChartAreas["ChartArea1"].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
				if (graphColorCount == 0) InitialColorSelection(graphChart);
				else SetColorForSeries(graphChart);
				graphColorCount++;
				graphChart.ChartAreas["ChartArea1"].AxisX.Title = "Load Survey DateTime";
				graphChart.ChartAreas["ChartArea1"].AxisX.TitleAlignment = StringAlignment.Center; 
				graphChart.ChartAreas["ChartArea1"].AxisY.Title = ParameterType;
				graphChart.ChartAreas["ChartArea1"].AxisY.TitleAlignment = StringAlignment.Center;
				graphChart.ChartAreas["ChartArea1"].AxisY.TextOrientation = TextOrientation.Stacked; 
				graphChart.Legends[0].Alignment = StringAlignment.Center;
				graphChart.Legends[0].Docking = Docking.Top; 
				graphChart.Titles["ChartTitle"].Text = ParameterType;
				graphChart.Titles["ChartTitle"].ForeColor = Color.Brown;
				graphChart.Titles["ChartTitle"].Font = new Font("Times New Roman", 16, FontStyle.Bold);
				graphChart.Titles["ChartTitle"].Alignment = ContentAlignment.TopCenter;
			}
            catch (Exception ex)    //Exception log for catch block
			{
				MessageBox.Show(ex.Message);
                logger.Log(LOGLEVELS.Error, "DisplayChart()", ex);
			}
		}

		private void InitialColorSelection(Chart graphChart)
		{ 
			for (int count = 0; count < graphChart.Series.Count; count++)
			{
				if (graphChart.Series[count].Name.Contains("R Phase") || graphChart.Series[count].Name.Equals("Demand KVAR Lead"))
					graphChart.Series[count].Color = selectedColorRPhase;
				 if (graphChart.Series[count].Name.Contains("Y Phase") || graphChart.Series[count].Name.Equals("Demand KVA"))
					graphChart.Series[count].Color = selectedColorYPhase;
				 if (graphChart.Series[count].Name.Contains("B Phase") || graphChart.Series[count].Name.Equals("Demand KW"))
					graphChart.Series[count].Color = selectedColorBPhase; 
				 if (graphChart.Series[count].Name.Equals("Demand KVAR Lag"))
					graphChart.Series[count].Color = selectedColorDemand;
			}
		}


		/// <summary>
		/// Setting the Color for the Series that has been displayed
		/// </summary>
		/// <param name="graphChart"></param>
		private void SetColorForSeries(Chart graphChart)
		{
			for (int count = 0; count < graphChart.Series.Count; count++)
			{
				string val = graphChart.Series[count].Name.Replace(" ", ""); ;
				if (val.Contains("RPhase") || val.Contains("DemandKVARLead"))
					graphChart.Series[count].Color = selectedColorRPhase;
				if (val.Contains("YPhase") || val.Contains("DemandKVA"))
					graphChart.Series[count].Color = selectedColorYPhase;
				if (val.Contains("BPhase") || val.Contains("DemandKW"))
					graphChart.Series[count].Color = selectedColorBPhase;
				if (val.Contains("DemandKVARLag"))
					graphChart.Series[count].Color = selectedColorDemand;
			}
			Application.DoEvents();
		}

		private void SetColorForSeries(Chart graphChart, Color selectedColor)
		{
			if (selectedSeriesName.Contains("R Phase") || selectedSeriesName.Contains("Demand KVA"))
				graphChart.Series[0].Color = selectedColorRPhase = selectedColor;
			if (selectedSeriesName.Contains("Y Phase") || selectedSeriesName.Contains("Demand KW"))
				graphChart.Series[0].Color = selectedColorYPhase = selectedColor;
			if (selectedSeriesName.Contains("B Phase") || selectedSeriesName.Contains("Demand KVAR Lag"))
				graphChart.Series[0].Color = selectedColorBPhase = selectedColor;
			if (selectedSeriesName.Contains("Demand KVAR Lead"))
				graphChart.Series[0].Color = selectedColorDemand = selectedColor; 
		}

		/// <summary>
		/// Setting the Chart type for the series
		/// </summary>
		/// <param name="graphChart"></param>
		private void SetChartType(Chart graphChart)
		{
			if (ChartType == ChartType.Line_Chart)
			{
				graphChart.Series[seriesName].ChartType = SeriesChartType.Line;
			}
			else if (ChartType == ChartType.Bar_Chart)
			{
				graphChart.Series[seriesName].ChartType = SeriesChartType.Column;
			}
		}

		/// <summary>
		/// returns the Daily Sorted Column for the DataSet and Record Count Passed
		/// </summary>
		/// <param name="dset"></param>
		/// <param name="recordCount"></param>
		/// <returns></returns>
		private DataTable SortAndPrepareColumn(DataSet dset, int totalRecord)
		{
			if (dset == null) 
				return null; 
			else if (dset.Tables[0].Rows.Count == 0) 
				return null; 
			DataTable filteredTable = new DataTable();
			filteredTable = dset.Tables[0].Clone();
			DataTable dataTable = new DataTable();
			dataTable = AutoNumberedTable(dset.Tables[0]);
			int RecNoStartPos = 0;
			int RecNoEndPos = 0;
			if (IntervalPeriod == 15 && GraphView == GraphView.Daily)
			{
				RecNoStartPos = (CurrentRecord * 96);
				RecNoEndPos = RecNoStartPos + 96;
			}
			if (IntervalPeriod == 30 && GraphView == GraphView.Daily)
			{
				RecNoStartPos = (CurrentRecord * 48);
				RecNoEndPos = RecNoStartPos + 48;
			}
			if (IntervalPeriod == 15 && GraphView == GraphView.Weekly)
			{
				RecNoStartPos = (CurrentRecord * 96 * 7);
				RecNoEndPos = RecNoStartPos + (96*7);
			}
			if (IntervalPeriod == 30 && GraphView == GraphView.Weekly)
			{
				RecNoStartPos = (CurrentRecord * 48 * 7);
				RecNoEndPos = RecNoStartPos + (48 * 7);
			}
			if (IntervalPeriod == 15 && GraphView == GraphView.Monthly)
			{
				RecNoStartPos = (CurrentRecord * 96 * 30);
				RecNoEndPos = RecNoStartPos + (96 * 30);
			}
			if (IntervalPeriod == 30 && GraphView == GraphView.Monthly)
			{
				RecNoStartPos = (CurrentRecord * 48 * 30);
				RecNoEndPos = RecNoStartPos + (48 * 30);
			}
			string stringExpression = string.Empty;
			stringExpression = "ID >" + RecNoStartPos + " and " + "ID <= " + RecNoEndPos;
			 string stringSort = "ID ASC";
			DataRow[] selectedRows = dataTable.Select(stringExpression, stringSort); 
			DataTable tempTable= ConvertDailyDataRowToDataTable(selectedRows, dataTable); 
			return tempTable;
		} 
		 

		/// <summary>
		/// Convert the Daily DataRow in to a DataTable column Starts with the count 1
		/// </summary>
		/// <param name="dataRow"></param>
		/// <param name="dTable"></param>
		/// <returns></returns>
		private DataTable ConvertDailyDataRowToDataTable(DataRow[] dataRow, DataTable dTable)
		{
			DataTable resultTable = new DataTable();
			int count = 0;
			foreach (DataColumn col in dTable.Columns)
			{
				if (count != 0)
				{
					if (!col.ColumnName.Equals("loadSurveyDateTime"))
						resultTable.Columns.Add(new DataColumn(col.ColumnName, Type.GetType("System.Double")));
					else
						resultTable.Columns.Add(new DataColumn(col.ColumnName, Type.GetType("System.String")));
				}
				count++;
			}
			foreach (DataRow drow in dataRow)
			{
				DataRow dr = resultTable.NewRow();
				for (int i = 1; i < dTable.Columns.Count; i++)
				{
					dr[resultTable.Columns[i - 1].ColumnName] = drow[dTable.Columns[i].ColumnName];
				}
				resultTable.Rows.Add(dr);
			}
			return resultTable;
		}
 

		/// <summary>
		/// Mouse Down Event is used to display the X- Axis Values and the Y- Axis Values 
		/// when the mouse is clicked down
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void graphChart_MouseDown(object sender, MouseEventArgs e)
		{
			HitTestResult result = graphChart.HitTest(e.X, e.Y);
			calloutAnnotation = new CalloutAnnotation();
			if ((result.PointIndex >= 0) && (result.Series != null))
			{
				selectedSeriesName = result.Series.Name; 
				DataPoint dp = graphChart.Series[result.Series.Name].Points[result.PointIndex];
				calloutAnnotation.AnchorDataPoint = dp;
				if (GraphView == GraphView.Weekly)
					calloutAnnotation.Text = String.Format("DT : {0:dd/MM/yyyy HH:mm}, Value : #VALY", DateTime.FromOADate(dp.XValue));
				else if (GraphView == GraphView.Daily)
					calloutAnnotation.Text = string.Format("DT : #VALX , Value : #VALY", dp.XValue, dp.YValues[0]);
				calloutAnnotation.BackColor = Color.FromArgb(255, 255, 128);
				calloutAnnotation.CalloutStyle = CalloutStyle.RoundedRectangle;
				calloutAnnotation.Visible = true;
				graphChart.Annotations.Add(calloutAnnotation);
			}
		}

		/// <summary>
		/// MouseUp Event for removing the Annotation that is popped up when Mouse Down event occurred
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void graphChart_MouseUp(object sender, MouseEventArgs e)
		{
			if (calloutAnnotation != null)
			{
				calloutAnnotation.Visible = false;
			}
		}

		/// <summary>
		/// When the Mouse is moved then the X and Y Axis values are displayed in the left side of the form
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void graphChart_MouseMove(object sender, MouseEventArgs e)
		{
			string dateTime = string.Empty;
			string value1 = string.Empty;
			string value2 = string.Empty;
			string value3 = string.Empty;
			string value4 = string.Empty;
			HitTestResult result = graphChart.HitTest(e.X, e.Y);
			int counter = 0;
			if ((result.PointIndex >= 0) && (result.Series != null))
			{
				foreach (Series series in graphChart.Series)
				{
					foreach (DataPoint point in graphChart.Series[series.Name].Points)
					{
						point.BackSecondaryColor = Color.Black;
						point.BackHatchStyle = ChartHatchStyle.None;
						point.BorderWidth = 1;
					}
					DataPoint points = graphChart.Series[series.Name].Points[result.PointIndex];
					if (result.ChartElementType == ChartElementType.DataPoint && counter == 0)
					{
						if (GraphView == GraphView.Daily)
							dateTime = points.AxisLabel.ToString();
						else
							dateTime = string.Format("{0:dd/MM/yyyy HH:mm}", DateTime.FromOADate(points.XValue));
						if (ParameterType == "Voltage")
							value1 = "R Phase  " + points.YValues[0].ToString() + " V";
						if (ParameterType == "Current")
							value1 = "R Phase  " + points.YValues[0].ToString() + " A";
						if (ParameterType == "Demand")
							value1 = "Demand  " + points.YValues[0].ToString() + " kVA";
					}
					if (result.ChartElementType == ChartElementType.DataPoint && counter == 1)
					{
						if (ParameterType == "Voltage")
							value2 = "Y Phase  " + points.YValues[0].ToString() + " V";
						if (ParameterType == "Current")
							value2 = "Y Phase  " + points.YValues[0].ToString() + " A";
						if (ParameterType == "Demand")
							value2 = "Demand  " + points.YValues[0].ToString() + " kW";
					}
					if (result.ChartElementType == ChartElementType.DataPoint && counter == 2)
					{
						if (ParameterType == "Voltage")
							value3 = "B Phase  " + points.YValues[0].ToString() + " V";
						if (ParameterType == "Current")
							value3 = "B Phase  " + points.YValues[0].ToString() + " A";
						if (ParameterType == "Demand")
							value3 = "Demand  " + points.YValues[0].ToString() + " kVAr Lag";
					}
					if (result.ChartElementType == ChartElementType.DataPoint && counter == 3)
					{
						if (ParameterType == "Voltage")
							value4 = points.YValues[0].ToString() + " V";
						if (ParameterType == "Current")
							value4 = points.YValues[0].ToString() + " A";
						if (ParameterType == "Demand")
							value4 = "Demand  " + points.YValues[0].ToString() + " kVAr Lead";
					}
					points.BackSecondaryColor = Color.Silver;
					points.BackHatchStyle = ChartHatchStyle.Percent60;
					points.BorderWidth = 5;
					counter++;
				}
			}
			OnGraphMouseMove(dateTime, value1, value2, value3, value4);
		}

		/// <summary>
		/// To Generate the Auto Number column Name ID for sorting and filtering 
		/// </summary>
		/// <param name="SourceTable"></param>
		/// <returns></returns>
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

		private void btnColor_Click(object sender, EventArgs e)
		{
			ColorDialog colorDialog = new ColorDialog();
			if (colorDialog.ShowDialog() == DialogResult.OK)
			{
				if (selectedSeriesName.Contains("R Phase") || selectedSeriesName.Contains("Demand KVA"))
					selectedColorRPhase = colorDialog.Color;
				 if (selectedSeriesName.Contains("Y Phase") || selectedSeriesName.Contains("Demand KW"))
					selectedColorYPhase = colorDialog.Color;
				 if (selectedSeriesName.Contains("B Phase") || selectedSeriesName.Contains("Demand KVAR Lag"))
					selectedColorBPhase = colorDialog.Color;
				 if (selectedSeriesName.Contains("Demand KVAR Lead"))
					selectedColorDemand = colorDialog.Color;
				DisplayChart();
			}
		} 
	}
}

