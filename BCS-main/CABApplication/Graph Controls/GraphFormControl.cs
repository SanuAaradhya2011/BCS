using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CAB.BLL;
using CAB.Entity;
using CAB.UI.Controls;
using CAB.Framework.Utility;
using CAB.Framework.Entity;
using CAB.Framework;

namespace CAB.UI.Graphs
{
	public partial class GraphFormControl : UserControl
	{
		LoadSurveyBLL loadSurveyBLL = new LoadSurveyBLL();
		private EnergyGraph energyGraph;
		
		private ArrayList selectedParameterList = new ArrayList();
		private ArrayList voltageParameterList= new ArrayList();
		private ArrayList currentParameterList= new ArrayList();
		private ArrayList demandParameterList= new ArrayList();

		private int intervalPeriod = 0;
		private int totalRec = 0;
		private int currentRecordCount = 0;
		
		private enum ParameterEnum
		{
			Voltage,
			Current,
			Demand
		}

		public string FileName {get;set;}
		public string MeterDataID { get; set; }

		public GraphFormControl()
		{
			InitializeComponent();
		}

		/// <summary>
		/// function to draw the Graph by setting the Property and calling the Energy Graph Control.
		/// </summary>
		/// <param name="parameterList"></param>
		/// <param name="fileName"></param>
		/// <param name="meterID"></param>
		/// <param name="recordCount"></param>

		void DrawGraph(ArrayList parameterList)
		{
			if (cboViewType.SelectedItem != null)
				energyGraph.GraphView = GetGraphView(cboViewType.SelectedItem.ToString());
			else
				energyGraph.GraphView = GraphView.Daily;
			 
			List<string> columnList = new List<string>();
			DataSet dataSet = loadSurveyBLL.LoadPaddingData(this.MeterDataID, energyGraph.GraphView);
			if ((dataSet == null) || (dataSet.Tables.Count == 0) || (dataSet.Tables[0].Rows.Count == 0))
				return;
			energyGraph.graphColorCount = 0;
			energyGraph.GraphDataSet = loadSurveyBLL.SelectColumnsFromPaddedData(dataSet, parameterList);
			totalRec = energyGraph.GraphDataSet.Tables[0].Rows.Count;
			energyGraph.IntervalPeriod= this.intervalPeriod= Convert.ToInt16(dataSet.Tables[0].Rows[0]["MDIntervalPeriod"]);
			energyGraph.CurrentRecord = currentRecordCount = 0;
			if (cboParameters.SelectedItem != null)
				energyGraph.ParameterType = cboParameters.SelectedItem.ToString();
			else
				energyGraph.ParameterType = "Voltage"; 
			if (cboChartType.SelectedItem != null)
				energyGraph.ChartType = GetChartType(cboChartType.SelectedItem.ToString());
			else
				energyGraph.ChartType = ChartType.Line_Chart;
			energyGraph.DisplayChart();
			energyGraph.OnMouseMoveInGraph += new EnergyGraph.GraphMouseMove(energyGraph_OnMouseMoveInGraph);
			panelChart.Controls.Add(energyGraph);
			label1.Text = "Start DateTime  : " + Convert.ToDateTime(energyGraph.GraphDataSet.Tables[0].Rows[1]["loadSurveyDateTime"].ToString()).ToString("dd/MM/yyyy HH:mm");
			label2.Text = "End DateTime  : " + Convert.ToDateTime(energyGraph.GraphDataSet.Tables[0].Rows[energyGraph.GraphDataSet.Tables[0].Rows.Count -1]["loadSurveyDateTime"].ToString()).ToString("dd/MM/yyyy HH:mm");
			label3.Text = "Active Log  : " + energyGraph.GraphDataSet.Tables[0].DefaultView.ToTable(true, "loadSurveyDateTime").Rows.Count;
			label4.Text = "Interval Period  : " + intervalPeriod + " Minutes";
		}

		private void energyGraph_OnMouseMoveInGraph(string dateTime, string Value1, string Value2, string Value3, string Value4)
		{
			this.lblDateTime.Text = dateTime.ToString();
			this.lblRphaseValue.Text = Value1.ToString();
			this.lblYPhaseValue.Text = Value2.ToString();
			this.lblBPhaseValue.Text = Value3.ToString();
			this.lblxPhaseValue.Text = Value4.ToString();
		}

		private bool energyGraph_ShowGridLine()
		{
			return false;
		}

		/// <summary>
		/// When the Parameters are Selected Voltage, Current, Demand
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cboParameters_SelectedIndexChanged(object sender, EventArgs e)
		{
			FillCheckedParameterList();
			if (cboParameters.Text == ParameterEnum.Voltage.ToString())
				DrawGraph(voltageParameterList);
			else if (cboParameters.Text == ParameterEnum.Current.ToString())
				DrawGraph(currentParameterList);
			else
				DrawGraph(demandParameterList);
		}

		void FillCheckedParameterList()
		{
			chkLBParameterDisplay.Items.Clear();
			if (cboParameters.Text == ParameterEnum.Voltage.ToString())
			{
				for (int chkCount = 0; chkCount < voltageParameterList.Count; chkCount++)
				{
					chkLBParameterDisplay.Items.Add(voltageParameterList[chkCount].ToString());
					chkLBParameterDisplay.SetItemChecked(chkCount, true);
				}
			}
			else if (cboParameters.Text == ParameterEnum.Current.ToString())
			{
				for (int chkCount = 0; chkCount < currentParameterList.Count; chkCount++)
				{
					chkLBParameterDisplay.Items.Add(currentParameterList[chkCount].ToString());
					chkLBParameterDisplay.SetItemChecked(chkCount, true);
				}
			}
			else
			{
				for (int chkCount = 0; chkCount < demandParameterList.Count; chkCount++)
				{
					chkLBParameterDisplay.Items.Add(demandParameterList[chkCount].ToString());
					chkLBParameterDisplay.SetItemChecked(chkCount, true);
				}
			}
		}

		void graphForSelectedParameters(string parameterName)
		{
			if (parameterName == ParameterEnum.Voltage.ToString())
				DrawGraph(voltageParameterList);
			else if (parameterName == ParameterEnum.Current.ToString())
				DrawGraph(currentParameterList);
			else
				DrawGraph(demandParameterList);
		}

		/// <summary>
		/// change the Parameter Selection to whatever the user selected
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void chkLBParameterDisplay_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.Cursor = Cursors.WaitCursor;
			Application.DoEvents();
			selectedParameterList.Clear();
			for (int listCount = 0; listCount < chkLBParameterDisplay.CheckedItems.Count; listCount++)
			{
				selectedParameterList.Add(GetParameterName(chkLBParameterDisplay.CheckedItems[listCount].ToString()));
			}
			DrawGraph(selectedParameterList);
			this.Cursor = Cursors.Default;
		}

		/// <summary>
		/// Setting the Chart Type Property to the Chart Type selected value
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cboChartType_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (energyGraph != null)
			{
				this.Cursor = Cursors.WaitCursor;
				Application.DoEvents();
				energyGraph.ChartType = GetChartType(cboChartType.SelectedItem.ToString());
				energyGraph.DisplayChart();
				this.Cursor = Cursors.Default;
			}
		}

		/// <summary>
		/// Setting the View Type where the Daily, Weekly, Monthly Reports can be Viewed.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cboViewType_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (energyGraph != null)
			{
				this.Cursor = Cursors.WaitCursor;
				Application.DoEvents();
				FillCheckedParameterList();
				energyGraph.GraphView = GetGraphViewType(cboViewType.SelectedItem.ToString());
				graphForSelectedParameters(cboParameters.Text);
				this.Cursor = Cursors.Default;
			}
		}

		/// <summary>
		/// when the Previous button is pressed
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnPrevious_Click(object sender, EventArgs e)
		{
			if (energyGraph.CurrentRecord == 0)
				return;
			energyGraph.CurrentRecord = energyGraph.CurrentRecord - 1;
			string chartType = cboViewType.SelectedText;

			if (chartType.Equals("Daily"))
				energyGraph.GraphView = GraphView.Daily;
			else if (chartType.Equals("Weekly"))
				energyGraph.GraphView = GraphView.Weekly;
			else if (chartType.Equals("Monthly"))
				energyGraph.GraphView = GraphView.Monthly;
			if (energyGraph != null)
				energyGraph.DisplayChart();
		}

		/// <summary>
		/// when the Next button is pressed
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnNext_Click(object sender, EventArgs e)
		{ 
			int rec=0;			
			string chartType = cboViewType.SelectedItem.ToString();
			if (intervalPeriod == 15 && chartType.Equals("Daily"))
			{
				rec = (energyGraph.CurrentRecord * 96) + 96;
				energyGraph.GraphView = GraphView.Daily; 
			}
			if (intervalPeriod == 30 && chartType.Equals("Daily"))
			{
				rec = (energyGraph.CurrentRecord * 48)+48;
				energyGraph.GraphView = GraphView.Daily; 
			}
			if (intervalPeriod == 15 && chartType.Equals("Weekly"))
			{
				rec = (energyGraph.CurrentRecord * 96 * 7) + 96;
				energyGraph.GraphView = GraphView.Weekly; 
			}
			if (intervalPeriod == 30 && chartType.Equals("Weekly"))
			{
				rec = (energyGraph.CurrentRecord * 48 * 7) + 48;
				energyGraph.GraphView = GraphView.Weekly; 
			}
			if (intervalPeriod == 15 && chartType.Equals("Monthly"))
			{
				rec = (energyGraph.CurrentRecord * 96 * 30)+96;
				energyGraph.GraphView = GraphView.Monthly; 
			}
			if (intervalPeriod == 30 && chartType.Equals("Monthly"))
			{
				rec = (energyGraph.CurrentRecord * 48 * 30)+48;
				energyGraph.GraphView = GraphView.Monthly; 
			}
			if (rec < totalRec)
			{
				energyGraph.CurrentRecord = energyGraph.CurrentRecord + 1;
			}
			if (energyGraph != null)
				energyGraph.DisplayChart();
		}

		/// <summary>
		/// method to get the enum value of the graph view type as Daily,Weekly,Monthly
		/// </summary>
		/// <param name="viewType"></param>
		/// <returns></returns>
		private GraphView GetGraphView(string viewType)
		{
			if (viewType == "Daily")
				return GraphView.Daily;
			else if (viewType == "Weekly")
				return GraphView.Weekly;
			else
				return GraphView.Monthly;
		}

		/// <summary>
		/// Method to get the enum value of the Chart Type as Bar Chart or Line Chart
		/// </summary>
		/// <param name="chartType"></param>
		/// <returns></returns>
		private ChartType GetChartType(string chartType)
		{
			if (chartType == "Bar Chart")
				return ChartType.Bar_Chart;
			else
				return ChartType.Line_Chart;
		}

		/// <summary>
		/// Method to get the Graph View Type for Daily, Weekly, Monthly
		/// </summary>
		/// <param name="vewType"></param>
		/// <returns></returns>

		private GraphView GetGraphViewType(string vewType)
		{
			if (vewType == "Daily")
				return GraphView.Daily;
			else if (vewType == "Weekly")
				return GraphView.Weekly;
			else
				return GraphView.Monthly;
		}

		/// <summary>
		/// Method to get the Parameter Name that is available in the Checked List Box  
		/// </summary>
		/// <param name="parameterName"></param>
		/// <returns></returns>
		private string GetParameterName(string parameterName)
		{
			if (parameterName.Equals("R Phase Voltage"))
				return "RPhaseVoltage";
			else if (parameterName.Equals("Y Phase Voltage"))
				return "YPhaseVoltage";
			else if (parameterName.Equals("B Phase Voltage"))
				return "BPhaseVoltage";
			else if (parameterName.Equals("R Phase Current"))
				return "RPhaseCurrent";
			else if (parameterName.Equals("Y Phase Current"))
				return "YPhaseCurrent";
			else if (parameterName.Equals("B Phase Current"))
				return "BPhaseCurrent";
			else if (parameterName.Equals("Demand KVAR Lead"))
				return "DemandKVARLead";
			else if (parameterName.Equals("Demand KVA"))
				return "DemandKVA";
			else if (parameterName.Equals("Demand KW"))
				return "DemandKW";
			else if (parameterName.Equals("Demand KVAR Lag"))
				return "DemandKVARLag";
			return null;
		}

		/// <summary>
		/// Get the Record Count of the dataset that is obtained passing the dataset
		/// </summary>
		/// <param name="dSet"></param>
		/// <returns></returns>
		private int GetRecordCount(DataSet dataSet)
		{
			if (dataSet == null)
				return 0;
			if (dataSet.Tables.Count == 0)
				return 0;
			if (dataSet.Tables[0].Rows.Count == 0)
				return 0;
			return dataSet.Tables[0].Rows.Count;
		}

		private int GetNumberOfDaysInMonth(DataSet dSet,int currentRecordCount)
		{
			DateTime dTime = DateUtility.LongToDateTime(Convert.ToInt64(dSet.Tables[0].Rows[currentRecordCount + 1]["loadSurveyDateTime"].ToString()));
			return System.DateTime.DaysInMonth(dTime.Year,dTime.Month);
		}

		/// <summary>
		/// Set the chart Type and view Type of the Chart and set to index 0 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>

		private void GraphFormControl_Load(object sender, EventArgs e)
		{
			DefaultSettings();
			GetParameterList();
			if (energyGraph == null)
				energyGraph = new EnergyGraph();
			ConfigInfo.GraphStatus = chkShowGridLines.Checked;
			cboParameters.SelectedIndex = 0; 
			cboChartType.SelectedIndex = 0;
			cboViewType.SelectedIndex = 0; 
			energyGraph.CurrentRecord = 0;
		}

		private void GetParameterList()
		{
			this.MeterDataID = ConfigInfo.ActiveMeterDataId;

			foreach (string enumParameterName in Enum.GetNames(typeof(ParameterEnum)))
			{
				ArrayList parameterList = loadSurveyBLL.GetColumnsListAvailable(enumParameterName, this.MeterDataID);
				if (parameterList == null)
					return;
				for (int pCount = 0; pCount < parameterList.Count; pCount++)
				{
					if (parameterList[pCount].ToString() == "RPhaseVoltage")
						voltageParameterList.Add("R Phase Voltage");
					else if (parameterList[pCount].ToString() == "YPhaseVoltage")
						voltageParameterList.Add("Y Phase Voltage");
					else if (parameterList[pCount].ToString() == "BPhaseVoltage")
						voltageParameterList.Add("B Phase Voltage");
					else if (parameterList[pCount].ToString() == "RPhaseCurrent")
						currentParameterList.Add("R Phase Current");
					else if (parameterList[pCount].ToString() == "YPhaseCurrent")
						currentParameterList.Add("Y Phase Current");
					else if (parameterList[pCount].ToString() == "BPhaseCurrent")
						currentParameterList.Add("B Phase Current");
					else if (parameterList[pCount].ToString() == "DemandKVA")
						demandParameterList.Add("Demand KVA");
					else if (parameterList[pCount].ToString() == "DemandKW")
						demandParameterList.Add("Demand KW");
					else if (parameterList[pCount].ToString() == "DemandKVARLag")
						demandParameterList.Add("Demand KVAR Lag");
					else if (parameterList[pCount].ToString() == "DemandKVARLead")
						demandParameterList.Add("Demand KVAR Lead");
				}
			}
		}

		void DefaultSettings()
		{
			lblDateTime.Text = string.Empty;
			lblRphaseValue.Text = string.Empty;
			label1.Text = string.Empty;
			label2.Text = string.Empty;
			label3.Text = string.Empty;
			label4.Text = string.Empty;
		}

		/// <summary>
		/// Used for print Preview the Chart
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnPrintPreview_Click(object sender, EventArgs e)
		{
			if (energyGraph != null)
				energyGraph.graphChart.Printing.PrintPreview();
		}

		/// <summary>
		/// Event To Print the Chart
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnPrint_Click(object sender, EventArgs e)
		{
			if (energyGraph != null)
				energyGraph.graphChart.Printing.Print(true);
		}

		private void chkShowGridLines_CheckedChanged(object sender, EventArgs e)
		{
			ConfigInfo.GraphStatus = chkShowGridLines.Checked;
			//if (chkShowGridLines.Checked == true)
			//{
			//    if (energyGraph != null)
			//    {
			//        energyGraph.graphChart.ChartAreas["ChartArea1"].AxisX.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
			//        energyGraph.graphChart.ChartAreas["ChartArea1"].AxisY.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
			//    }
			//    else
			//    {
			//        energyGraph.graphChart.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false; // = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
			//        energyGraph.graphChart.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = false; // = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
			//    }
			//}
		}
	}
}
