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
using CAB.Framework.Utility;
using System.ComponentModel;
using CAB.Framework;
using System.Globalization;
using Hunt.EPIC.Logging;
using System.Linq;

namespace CAB.UI
{
    public partial class frmLoadsurveyGraph : Form
    {
        public long MeterDataId { get; set; }
        public int IntervalPeriod { get; set; }
        public string unitType { get; set; }
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
        private string columnNames = string.Empty;
        private int CurrentRecord;
        private bool value;
        private bool ShowGrids;
        private bool GraphType;
        private bool Viewtype;
        private bool isAverageVoltagePresent;
        //Added to solve bug 71617.
        private const string DATETITLE1 = "Report Date Time: ";
        private const string DATETITLE2 = "\n(dd/MM/yyyy HH:mm)";
        private const string POWERSTATUS = "PowerStatus";
        private const string NORMAL = "Normal";
        private const string NOLOAD = "NL";
        private const string NOPOWER = "NP";
        private const string ZEROHOUR = "00:00";
        //****** Import has been removed done by AET wrongly *********
        private const string DEMANDKW = "Block Demand - kW (1.0.1.29.0.255;3;2)";
        private const string DEMANKKVAHLAG = "Block Demand - kvar - lag (1.0.5.29.0.255;3;2)";
        private const string DEMANDKVAHLEAD = "Block Demand - kvar - lead (1.0.8.29.0.255;3;2)";
        private const string DEMANDKVA = "Block Demand - kVA (1.0.9.29.0.255;3;2)";
        private const string ENEGRYKWH = "Block Energy - kWh (1.0.1.29.0.255;3;2)";
        private const string ENERGYKKVARHLAG = "Block Energy - kvarh - lag (1.0.5.29.0.255;3;2)";
        private const string ENERGYKKVARHLEAD = "Block Energy - kvarh - lead (1.0.8.29.0.255;3;2)";
        private const string ENERGYKVAH = "Block Energy - kVAh (1.0.9.29.0.255;3;2)";
        // SB Code Change Start - 20171204 - Export Energy Display in Load Survey Graph
        private const string ENEGRYKWHEXPORT = "Block Energy - kWh Export (1.0.2.29.0.255;3;2)";
        private const string ENERGYKKVARHLAGEXPORT = "Block Energy - kvarh - lag Q3 (1.0.7.29.0.255;3;2)";
        private const string ENERGYKKVARHLEADEXPORT = "Block Energy - kvarh - lead Q2 (1.0.6.29.0.255;3;2)";
        private const string ENERGYKVAHEXPORT = "Block Energy - kVAh Export (1.0.10.29.0.255;3;2)";
        // SB Code Change End - 20171204 - Export Energy Display in Load Survey Graph
        // SB Code Change Start - 20180129 - Export Demand Display in Load Survey Graph
        private const string DEMANDKWEXPORT = "Block Demand - kW Export (1.0.2.29.0.255;3;2)";
        private const string DEMANDKKVARLAGEXPORT = "Block Demand - kvar - lag Q3 (1.0.7.29.0.255;3;2)";
        private const string DEMANDKKVARLEADEXPORT = "Block Demand - kvar - lead Q2 (1.0.6.29.0.255;3;2)";
        private const string DEMANDKVAEXPORT = "Block Demand - kVA Export (1.0.10.29.0.255;3;2)";
        // SB Code Change End - 20180129 - Export Demand Display in Load Survey Graph
        private const string DATETIME = "Real Time Clock - Date and Time (0.0.1.0.0.255;8;2)";
        private const string VOLTAGEVRN = "Voltage - VRN (1.0.32.27.0.255;3;2)";
        private const string VOLTAGEVYN = "Voltage - VYN (1.0.52.27.0.255;3;2)";
        private const string VOLTAGEVBN = "Voltage - VBN (1.0.72.27.0.255;3;2)";
        private const string AVERAGEVOLTAGE = "Average Voltage (1.0.12.27.0.255;3;2)";
        private const string CURRENTIR = "Current - IR (1.0.31.27.0.255;3;2)";
        private const string CURRENTIY = "Current - IY (1.0.51.27.0.255;3;2)";
        private const string CURRENTIB = "Current - IB (1.0.71.27.0.255;3;2)";
        // SB Code Change Start - 20171214 - Neutral Current in Load Survey Graph
        private const string NEUTRALCURRENT = "NeutralCurrent - (1.0.91.7.0.255;3;2)";
        // SB Code Change End - 20171214 - Neutral Current in Load Survey Graph
        private const string AVERAGECURRENT = "Average Current (1.0.11.27.0.255;3;2)";
        // HTCT Specific
        private const string DEMANDMW = "Block Demand - MW (1.0.1.29.0.255;3;2)";
        private const string DEMANDMVAHLAG = "Block Demand - Mvar - lag (1.0.5.29.0.255;3;2)";
        private const string DEMANDMVAHLEAD = "Block Demand - Mvar - lead (1.0.8.29.0.255;3;2)";
        private const string DEMANDMVA = "Block Demand - MVA (1.0.9.29.0.255;3;2)";
        private const string ENEGRYMWH = "Block Energy - MWh (1.0.1.29.0.255;3;2)";
        private const string ENERGYMVARHLAG = "Block Energy - Mvarh - lag (1.0.5.29.0.255;3;2)";
        private const string ENERGYMVARHLEAD = "Block Energy - Mvarh - lead (1.0.8.29.0.255;3;2)";
        private const string ENERGYMVAH = "Block Energy - MVAh (1.0.9.29.0.255;3;2)";
        // SB Code Change Start - 20171204 - Export Energy Display in Load Survey Graph
        private const string ENEGRYMWHEXPORT = "Block Energy - MWh Export (1.0.2.29.0.255;3;2)";
        private const string ENERGYMVARHLAGEXPORT = "Block Energy - Mvarh - lag Q3 (1.0.7.29.0.255;3;2)";
        private const string ENERGYMVARHLEADEXPORT = "Block Energy - Mvarh - lead Q2 (1.0.6.29.0.255;3;2)";
        private const string ENERGYMVAHEXPORT = "Block Energy - MVAh Export (1.0.10.29.0.255;3;2)";
        // SB Code Change End - 20171204 - Export Energy Display in Load Survey Graph
        // SB Code Change Start - 20180129 - Export Demand Display in Load Survey Graph
        private const string DEMANDMWEXPORT = "Block Demand - MW Export (1.0.2.29.0.255;3;2)";
        private const string DEMANDMVARLAGEXPORT = "Block Demand - Mvar - lag Q3 (1.0.7.29.0.255;3;2)";
        private const string DEMANDMVARLEADEXPORT = "Block Demand - Mvar - lead Q2 (1.0.6.29.0.255;3;2)";
        private const string DEMANDMVAEXPORT = "Block Demand - MVA Export (1.0.10.29.0.255;3;2)";
        // SB Code Change End - 20180129 - Export Demand Display in Load Survey Graph
        private const string POWERFACTOR = "Power Factor";
        private const string TEMPFLAG = "Tamper Flag(0.0.96.10.128.255;1;2)";
        private const string AVGVOLT3PHASE = "Avg. Voltage 3 Phase(1.0.12.27.128.255;3;2)";
        private const string AVGRPHPF = "Avg.R Phase PF(1.0.33.29.0.255;3;2)";
        private const string AVGYPHPF = "Avg.Y Phase PF(1.0.53.29.0.255;3;2)";
        private const string AVGBPHPF = "Avg.B Phase PF(1.0.73.29.0.255;3;2)";
        private const string AVGTOTALPF = "Avg.Total PF(1.0.13.29.0.255;3;2)";

        private const string LSneucurrent = "AvgNeutralCurrent (1.0.91.29.0.255;3;2)";
        private const string LSTHDVR = "THDVR (1.0.32.128.124.255;3;2)";
        private const string LSTHDVY = "THDVY (1.0.52.128.124.255;3;2)";
        private const string LSTHDVB = "THDVB (1.0.72.128.124.255;3;2)";
        private const string LSTHDIR = "THDIR (1.0.31.128.124.255;3;2)";
        private const string LSTHDIY = "THDIY (1.0.51.128.124.255;3;2)";
        private const string LSTHDIB = "THDIB (1.0.71.128.124.255;3;2)";

        private const string FREQUENCY = "Frequency - Hz (1.0.14.27.0.255;3;2)";

        DateTimeFormatInfo dateTimeFormatInfo = new DateTimeFormatInfo();

        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(frmLoadsurveyGraph).ToString());

        private enum Series
        {
            [DescriptionAttribute("R Phase Current")]
            RPhaseCurrent,
            [DescriptionAttribute("Y Phase Current")]
            YPhaseCurrent,
            [DescriptionAttribute("B Phase Current")]
            BPhaseCurrent,
            // SB Code Change Start - 20171214 - Neutral Current in Load Survey Graph
            [DescriptionAttribute("Neutral Current")]
            NeutralCurrent,
            // SB Code Change End - 20171214 - Neutral Current in Load Survey Graph
            [DescriptionAttribute("Average Voltage")]
            AverageVoltage,
            [DescriptionAttribute("R Phase Voltage")]
            RPhaseVoltage,
            [DescriptionAttribute("B Phase Voltage")]
            BPhaseVoltage,
            [DescriptionAttribute("Y Phase Voltage")]
            YPhaseVoltage,
            [DescriptionAttribute("Demand kW")]
            DemandKW,
            [DescriptionAttribute("Demand kvar Lag")]
            DemandKVARLag,
            [DescriptionAttribute("Demand kvar Lead")]
            DemandKVARLead,
            [DescriptionAttribute("Demand kVA")]
            DemandKVA,
            // SB Code Change Start - 20180129 - Export Demand Display in Load Survey Graph
            [DescriptionAttribute("kW Export")]
            DemandKWExport,
            [DescriptionAttribute("kvar Lag Export")]
            DemandKVARLagExport,
            [DescriptionAttribute("kvar Lead Export")]
            DemandKVARLeadExport,
            [DescriptionAttribute("kVA Export")]
            DemandKVAExport,
            // SB Code Change End - 20180129 - Export Demand Display in Load Survey Graph
            [DescriptionAttribute("Energy kWh")]
            EnergyKWh,
            [DescriptionAttribute("Energy kvarh Lag")]
            EnergyKVARhLag,
            [DescriptionAttribute("Energy kvarh Lead")]
            EnergyKVARhLead,
            [DescriptionAttribute("Energy kVAh")]
            EnergyKVAh,
            // SB Code Change Start - 20171204 - Export Energy Display in Load Survey Graph
            [DescriptionAttribute("kWh Export")]
            EnergyKWhExport,
            [DescriptionAttribute("kvarh Lag Export")]
            EnergyKVARhLagExport,
            [DescriptionAttribute("kvarh Lead Export")]
            EnergyKVARhLeadExport,
            [DescriptionAttribute("kVAh Export")]
            EnergyKVAhExport,
            // SB Code Change End - 20171204 - Export Energy Display in Load Survey Graph
            [DescriptionAttribute("Demand MW")]
            DemandMW,
            [DescriptionAttribute("Demand Mvar Lag")]
            DemandMVARLag,
            [DescriptionAttribute("Demand Mvar Lead")]
            DemandMVARLead,
            [DescriptionAttribute("Demand MVA")]
            DemandMVA,
            // SB Code Change Start - 20180129 - Export Demand Display in Load Survey Graph
            [DescriptionAttribute("MW Export")]
            DemandMWExport,
            [DescriptionAttribute("Mvar Lag Export")]
            DemandMVARLagExport,
            [DescriptionAttribute("Mvar Lead Export")]
            DemandMVARLeadExport,
            [DescriptionAttribute("MVA Export")]
            DemandMVAExport,
            // SB Code Change End - 20180129 - Export Demand Display in Load Survey Graph
            [DescriptionAttribute("Energy MWh")]
            EnergyMWh,
            [DescriptionAttribute("Energy Mvarh Lag")]
            EnergyMVARhLag,
            [DescriptionAttribute("Energy Mvarh Lead")]
            EnergyMVARhLead,
            [DescriptionAttribute("Energy MVAh")]
            EnergyMVAh,
            // SB Code Change Start - 20171204 - Export Energy Display in Load Survey Graph
            [DescriptionAttribute("MWh Export")]
            EnergyMWhExport,
            [DescriptionAttribute("Mvarh Lag Export")]
            EnergyMVARhLagExport,
            [DescriptionAttribute("Mvarh Lead Export")]
            EnergyMVARhLeadExport,
            [DescriptionAttribute("MVAh Export")]
            EnergyMVAhExport,
            // SB Code Change End - 20171204 - Export Energy Display in Load Survey Graph
            [DescriptionAttribute("Power Factor")]
            PowerFactor,
            [DescriptionAttribute("Frequency")]
            Frequency,
            [DescriptionAttribute("Average Neutral Current")]
            AverageNeutralCurrent
        }
        public frmLoadsurveyGraph()
        {
            InitializeComponent();
            dateTimeFormatInfo.ShortDatePattern = "dd/MM/yyyy";
            dateTimeFormatInfo.DateSeparator = "/";
            unitType = CommonMethods.MeterDataType == MeterDataTypes.HTCT ? "M" : "k";
            //FromDate = 20101022003000;
            //ToDate = 20101112120000;
            //IntervalPeriod = 30;
            //MeterDataId = 2;
        }
        /// <summary>
        /// This function is used to get colour of the series.
        /// </summary>
        /// <param name="series"></param>
        /// <returns>Color</returns>
        private Color fGetSeriesColor(string series)
        {
            if (series.Equals(EnumUtil.stringValueOf(Series.RPhaseCurrent)))
                return Color.Red;
            else if (series.Equals(EnumUtil.stringValueOf(Series.YPhaseCurrent)))
                return Color.Yellow;
            else if (series.Equals(EnumUtil.stringValueOf(Series.BPhaseCurrent)))
                return Color.Blue;
            // SB Code Change Start - 20171214 - Neutral Current in Load Survey Graph
            else if (series.Equals(EnumUtil.stringValueOf(Series.NeutralCurrent)))
                return Color.Green;
            // SB Code Change End - 20171214 - Neutral Current in Load Survey Graph
            else if (series.Equals(EnumUtil.stringValueOf(Series.RPhaseVoltage)))
                return Color.Red;
            else if (series.Equals(EnumUtil.stringValueOf(Series.YPhaseVoltage)))
                return Color.Yellow;
            else if (series.Equals(EnumUtil.stringValueOf(Series.BPhaseVoltage)))
                return Color.Blue;
            else if (series.Equals(EnumUtil.stringValueOf(Series.DemandKW)) || series.Equals(EnumUtil.stringValueOf(Series.DemandMW)) || series.Equals(EnumUtil.stringValueOf(Series.DemandKWExport)) || series.Equals(EnumUtil.stringValueOf(Series.DemandMWExport))) // SB Code Change Start/End - 20180129 - Export Demand Display in Load Survey Graph - Added condition for Export Demand
                return Color.Red;
            else if (series.Equals(EnumUtil.stringValueOf(Series.DemandKVARLag)) || series.Equals(EnumUtil.stringValueOf(Series.DemandMVARLag)) || series.Equals(EnumUtil.stringValueOf(Series.DemandKVARLagExport)) || series.Equals(EnumUtil.stringValueOf(Series.DemandMVARLagExport))) // SB Code Change Start/End - 20180129 - Export Demand Display in Load Survey Graph - Added condition for Export Demand
                return Color.Yellow;
            else if (series.Equals(EnumUtil.stringValueOf(Series.DemandKVARLead)) || series.Equals(EnumUtil.stringValueOf(Series.DemandMVARLead)) || series.Equals(EnumUtil.stringValueOf(Series.DemandKVARLeadExport)) || series.Equals(EnumUtil.stringValueOf(Series.DemandMVARLeadExport))) // SB Code Change Start/End - 20180129 - Export Demand Display in Load Survey Graph - Added condition for Export Demand
                return Color.Blue;
            else if (series.Equals(EnumUtil.stringValueOf(Series.DemandKVA)) || series.Equals(EnumUtil.stringValueOf(Series.DemandMVA)) || series.Equals(EnumUtil.stringValueOf(Series.DemandKVAExport)) || series.Equals(EnumUtil.stringValueOf(Series.DemandMVAExport))) // SB Code Change Start/End - 20180129 - Export Demand Display in Load Survey Graph - Added condition for Export Demand
                return Color.Green;
            else if (series.Equals(EnumUtil.stringValueOf(Series.EnergyKWh)) || series.Equals(EnumUtil.stringValueOf(Series.EnergyMWh)) || series.Equals(EnumUtil.stringValueOf(Series.EnergyKWhExport)) || series.Equals(EnumUtil.stringValueOf(Series.EnergyMWhExport))) // SB Code Change Start/End - 20171205 - Export Energy Display in Load Survey Graph - Added condition for Export Energy
                return Color.Red;
            else if (series.Equals(EnumUtil.stringValueOf(Series.EnergyKVARhLag)) || series.Equals(EnumUtil.stringValueOf(Series.EnergyKVARhLagExport))) // SB Code Change Start/End - 20171205 - Export Energy Display in Load Survey Graph - Added condition for Export Energy
                return Color.Yellow;
            else if (series.Equals(EnumUtil.stringValueOf(Series.EnergyKVARhLead)) || series.Equals(EnumUtil.stringValueOf(Series.EnergyMVARhLead)) || series.Equals(EnumUtil.stringValueOf(Series.EnergyKVARhLeadExport)) || series.Equals(EnumUtil.stringValueOf(Series.EnergyMVARhLeadExport))) // SB Code Change Start/End - 20171205 - Export Energy Display in Load Survey Graph - Added condition for Export Energy
                return Color.Blue;
            else if (series.Equals(EnumUtil.stringValueOf(Series.EnergyKVAh)) || series.Equals(EnumUtil.stringValueOf(Series.EnergyMVAh)) || series.Equals(EnumUtil.stringValueOf(Series.EnergyKVAhExport)) || series.Equals(EnumUtil.stringValueOf(Series.EnergyMVAhExport))) // SB Code Change Start/End - 20171205 - Export Energy Display in Load Survey Graph - Added condition for Export Energy
                return Color.Green;
            else if (series.Equals(EnumUtil.stringValueOf(Series.PowerFactor)))
                return Color.Red;
            else if (series.Equals(EnumUtil.stringValueOf(Series.AverageVoltage).Replace(" ", "")) && isAverageVoltagePresent)
                return Color.Green;
            else if (series.Equals(EnumUtil.stringValueOf(Series.AverageNeutralCurrent)))
                return Color.Green;
            else
                return Color.Red;
        }
        private String fGetSeriesName(String series)
        {
            if (series == "RPhaseCurrent")
                return "R Phase Current";
            else if (series == "YPhaseCurrent")
                return "Y Phase Current";
            else if (series == "BPhaseCurrent")
                return "B Phase Current";
            // SB Code Change Start - 20171214 - Neutral Current in Load Survey Graph
            else if (series == "NeutralCurrent")
                return "Neutral Current";
            // SB Code Change End - 20171214 - Neutral Current in Load Survey Graph
            else if (series == "AvgNeutralCurrent")
                return "Average Neutral Current";
            else if (series == "RPhaseVoltage")
                return "R Phase Voltage";
            else if (series == "YPhaseVoltage")
                return "Y Phase Voltage";
            else if (series == "BPhaseVoltage")
                return "B Phase Voltage";
            else if (series == "DemandKW")
                return string.Format("Demand {0}W", unitType);
            else if (series == "DemandKVARLag")
                return string.Format("Demand {0}var Lag", unitType);
            else if (series == "DemandKVARLead")
                return string.Format("Demand {0}var Lead", unitType);
            else if (series == "DemandKVA")
                return string.Format("Demand {0}VA", unitType);
            // SB Code Change Start - 20180129 - Export Demand Display in Load Survey Graph
            else if (series == "DemandKWExport")
                return string.Format("{0}W Export", unitType);
            else if (series == "DemandKVARLagExport")
                return string.Format("{0}var Lag Export", unitType);
            else if (series == "DemandKVARLeadExport")
                return string.Format("{0}var Lead Export", unitType);
            else if (series == "DemandKVAExport")
                return string.Format("{0}VA Export", unitType);
            // SB Code Change End - 20180129 - Export Demand Display in Load Survey Graph
            else if (series == "EnergyKWh")
                return string.Format("Energy {0}Wh", unitType);
            else if (series == "EnergyKVARhLag")
                return string.Format("Energy {0}varh Lag", unitType);
            else if (series == "EnergyKVARhLead")
                return string.Format("Energy {0}varh Lead", unitType);
            else if (series == "EnergyKVAh")
                return string.Format("Energy kVAh");
            // SB Code Change Start - 20171204 - Export Energy Display in Load Survey Graph
            else if (series == "EnergyKWhExport")
                return string.Format("{0}Wh Export", unitType);
            else if (series == "EnergyKVARhLagExport")
                return string.Format("{0}varh Lag Export", unitType);
            else if (series == "EnergyKVARhLeadExport")
                return string.Format("{0}varh Lead Export", unitType);
            else if (series == "EnergyKVAhExport")
                return string.Format("kVAh Export");
            // SB Code Change End - 20171204 - Export Energy Display in Load Survey Graph
            // HTCT Specific
            else if (series == "DemandMW")
                return string.Format("Demand MW");
            else if (series == "DemandMVARLag")
                return string.Format("Demand Mvar Lag");
            else if (series == "DemandMVARLead")
                return string.Format("Demand Mvar Lead");
            else if (series == "DemandMVA")
                return string.Format("Demand MVA");
            // SB Code Change Start - 20180129 - Export Demand Display in Load Survey Graph
            else if (series == "DemandMWExport")
                return string.Format("Demand MW Export", unitType);
            else if (series == "DemandMVARLagExport")
                return string.Format("Demand Mvar Lag Export", unitType);
            else if (series == "DemandMVARLeadExport")
                return string.Format("Demand Mvar Lead Export", unitType);
            else if (series == "DemandMVAExport")
                return string.Format("Demand MVA Export");
            // SB Code Change End - 20180129 - Export Demand Display in Load Survey Graph
            else if (series == "EnergyMWh")
                return string.Format("Energy MWh");
            else if (series == "EnergyMVARhLag")
                return string.Format("Energy Mvarh Lag");
            else if (series == "EnergyMVARhLead")
                return string.Format("Energy Mvarh Lead");
            else if (series == "EnergyMVAh")
                return string.Format("Energy MVAh");
            // SB Code Change Start - 20171204 - Export Energy Display in Load Survey Graph
            else if (series == "EnergyMWhExport")
                return string.Format("Energy MWh Export");
            else if (series == "EnergyMVARhLagExport")
                return string.Format("Energy Mvarh Lag Export");
            else if (series == "EnergyMVARhLeadExport")
                return string.Format("Energy Mvarh Lead Export");
            else if (series == "EnergyMVAhExport")
                return string.Format("Energy MVAh Export");
            // SB Code Change End - 20171204 - Export Energy Display in Load Survey Graph
            else if (series == "PowerFactor")
                return "Power Factor";
            else
                return series;
        }
        private void SetColorForSeries()
        {
            //Setting the Color of the series
            for (int count = 0; count < graphChart.Series.Count; count++)
            {
                if (count == 0)
                {
                    // Added to solve bug 83429, Color coding issue.
                    graphChart.Series[0].Color = fGetSeriesColor(graphChart.Series[0].Name);

                }// Color.Red;
                else if (count == 1)
                {
                    graphChart.Series[1].Color = fGetSeriesColor(graphChart.Series[1].Name);// Color.Yellow;
                }
                else if (count == 2)
                {
                    graphChart.Series[2].Color = fGetSeriesColor(graphChart.Series[2].Name);// Color.Yellow;
                }
                else if (count == 3)
                {
                    graphChart.Series[3].Color = fGetSeriesColor(graphChart.Series[3].Name);// Color.Yellow;
                }


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

            for (int count = 1; count < dTable.Columns.Count - 2; count++)
            {
                dColumn = new DataColumn(dTable.Columns[count].ColumnName);
                // dColumn.DataType = System.Type.GetType("System.Double");
                resultTable.Columns.Add(dColumn);
            }

            dColumn = new DataColumn("loadSurveyDateTime");
            resultTable.Columns.Add(dColumn);
            dColumn = new DataColumn(POWERSTATUS);
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
                int totalcolcount = GraphDataSet.Tables[0].Columns.Count - 2;
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

                    else if (totalcolcount > 3)
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
                    // Added one more condition if three parameters are selected.
                    else if (totalcolcount == 3)
                    {
                        graphChart.ChartAreas.Add("AREA0");
                        newelement = new System.Windows.Forms.DataVisualization.Charting.ElementPosition(0, 8, 45, 45);
                        graphChart.ChartAreas["AREA0"].Position = newelement;

                        graphChart.ChartAreas.Add("AREA1");
                        newelement = new System.Windows.Forms.DataVisualization.Charting.ElementPosition(48, 8, 45, 45);
                        graphChart.ChartAreas["AREA1"].Position = newelement;

                        graphChart.ChartAreas.Add("AREA2");
                        newelement = new System.Windows.Forms.DataVisualization.Charting.ElementPosition(25, 52, 45, 45);
                        graphChart.ChartAreas["AREA2"].Position = newelement;
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
                    // Commented dummy row region if 3 parameters are selected . to remove blank or no parameter graph.
                    # region Dummy row
                    //if (totalcolcount == 3)     /// dummy Row
                    //{

                    //    // taking the series name from the column name
                    //    seriesName = "Blank";
                    //    graphChart.Series.Add(seriesName);

                    //    graphChart.Series[seriesName].ChartArea = "AREA3";

                    //    //Setting the Chart Type for the Series
                    //    SetChartType();
                    //    //Setting the border width of the line 
                    //    graphChart.Series[seriesName].BorderWidth = 1;

                    //    foreach (DataRow dRow in graphTable.Rows)
                    //    {
                    //        //Dont Delete Very important
                    //        //************************************************************************************************************
                    //        //////DateTime xAxisValue = Convert.ToDateTime(dRow["loadSurveyDateTime"].ToString());
                    //        //************************************************************************************************************
                    //        string xAxisValue = "";
                    //        double yAxisValue = 0;

                    //        if (GraphView == 0)
                    //        {
                    //            xAxisValue = ConvertStringTodate(dRow["loadSurveyDateTime"].ToString()).ToString("HH:mm");
                    //            yAxisValue = Convert.ToDouble(0);
                    //        }
                    //        else if (GraphView == 1)
                    //        {
                    //            xAxisValue = ConvertStringTodate(dRow["loadSurveyDateTime"].ToString()).ToString("HH:mm") + Convert.ToString('\n') + ConvertStringTodate(dRow["loadSurveyDateTime"].ToString()).ToString("ddd") + Convert.ToString('\n') + ConvertStringTodate(dRow["loadSurveyDateTime"].ToString()).ToString("dd-MMM-yy");// +"\n" + Convert.ToDateTime(dRow["loadSurveyDateTime"].ToString()).ToString("dd/MM/yyyy"); ;
                    //            yAxisValue = Convert.ToDouble(0);
                    //        }
                    //        else if (GraphView == 2)
                    //        {
                    //            xAxisValue = ConvertStringTodate(dRow["loadSurveyDateTime"].ToString()).ToString("HH:mm") + Convert.ToString('\n') + ConvertStringTodate(dRow["loadSurveyDateTime"].ToString()).ToString("dd-MMM-yy") + Convert.ToString('\n') + ConvertStringTodate(dRow["loadSurveyDateTime"].ToString()).ToString("ddd");
                    //            yAxisValue = Convert.ToDouble(0);
                    //        }
                    //        if (yAxisValue < 0)
                    //            yAxisValue = 0;
                    //        graphChart.Series[seriesName].Points.AddXY(xAxisValue, yAxisValue);
                    //    }

                    //    //}
                    //    //Setting the Chart Area Back Color and other Settings
                    //    graphChart.ChartAreas["AREA3"].AxisX.ScaleView.Zoomable = true;
                    //    graphChart.ChartAreas["AREA3"].BackColor = Color.FromArgb(64, 165, 191, 228);
                    //    graphChart.ChartAreas["AREA3"].BackGradientStyle = GradientStyle.TopBottom;
                    //    graphChart.ChartAreas["AREA3"].BackSecondaryColor = Color.White;
                    //    graphChart.BackColor = Color.FromArgb(211, 223, 240);
                    //    graphChart.BorderlineColor = Color.FromArgb(26, 59, 105);
                    //    graphChart.BorderlineDashStyle = ChartDashStyle.Solid;

                    //    //Enabling the Cursor Positions
                    //    graphChart.ChartAreas["AREA3"].CursorX.IsUserEnabled = true;
                    //    graphChart.ChartAreas["AREA3"].CursorX.Position = 0;
                    //    graphChart.ChartAreas["AREA3"].CursorX.LineColor = Color.FromName("Black");
                    //    //graphChart.ChartAreas["AREA3"].CursorY.IsUserEnabled = true;

                    //    graphChart.ChartAreas["AREA3"].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
                    //    graphChart.ChartAreas["AREA3"].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot;

                    //    //Dont Delete Very important
                    //    if (ShowGrids == true)
                    //    {
                    //        //************************************************************************************************************
                    //        graphChart.ChartAreas["AREA3"].AxisX.MajorGrid.Enabled = true;
                    //        graphChart.ChartAreas["AREA3"].AxisY.MajorGrid.Enabled = true;
                    //        //************************************************************************************************************
                    //    }
                    //    else
                    //    {
                    //        graphChart.ChartAreas["AREA3"].AxisX.MajorGrid.Enabled = false;
                    //        graphChart.ChartAreas["AREA3"].AxisY.MajorGrid.Enabled = false;
                    //    }

                    //    if (GraphView == 0)
                    //    {
                    //        if (IntervalPeriod == 15)
                    //        {
                    //            graphChart.ChartAreas["AREA3"].AxisX.LabelStyle.Interval = 4;
                    //            graphChart.ChartAreas["AREA3"].AxisX.MajorGrid.Interval = 4;
                    //        }
                    //        else if (IntervalPeriod == 30)
                    //        {
                    //            graphChart.ChartAreas["AREA3"].AxisX.LabelStyle.Interval = 2;
                    //            graphChart.ChartAreas["AREA3"].AxisX.MajorGrid.Interval = 2;
                    //        }
                    //        else if (IntervalPeriod == 60)
                    //        {
                    //            graphChart.ChartAreas["AREA3"].AxisX.LabelStyle.Interval = 1;
                    //            graphChart.ChartAreas["AREA3"].AxisX.MajorGrid.Interval = 1;
                    //        }

                    //        graphChart.ChartAreas["AREA3"].AxisX.MajorTickMark.Interval = 1;

                    //        graphChart.ChartAreas["AREA3"].AxisX.LabelAutoFitStyle = LabelAutoFitStyles.LabelsAngleStep90;

                    //    }
                    //    else if (GraphView == 1)
                    //    {
                    //        if (IntervalPeriod == 15)
                    //        {
                    //            graphChart.ChartAreas["AREA3"].AxisX.LabelStyle.Interval = 4 * 12;
                    //            graphChart.ChartAreas["AREA3"].AxisX.MajorGrid.Interval = 4 * 12;
                    //            graphChart.ChartAreas["AREA3"].AxisX.MajorTickMark.Interval = 4 * 12;
                    //        }
                    //        else if (IntervalPeriod == 30)
                    //        {
                    //            graphChart.ChartAreas["AREA3"].AxisX.LabelStyle.Interval = 2 * 12;
                    //            graphChart.ChartAreas["AREA3"].AxisX.MajorGrid.Interval = 2 * 12;
                    //            graphChart.ChartAreas["AREA3"].AxisX.MajorTickMark.Interval = 2 * 12;

                    //        }
                    //        else if (IntervalPeriod == 60)
                    //        {
                    //            graphChart.ChartAreas["AREA3"].AxisX.LabelStyle.Interval = 1 * 12;
                    //            graphChart.ChartAreas["AREA3"].AxisX.MajorGrid.Interval = 1 * 12;
                    //            graphChart.ChartAreas["AREA3"].AxisX.MajorTickMark.Interval = 1 * 12;
                    //        }
                    //    }
                    //    else if (GraphView == 2)
                    //    {
                    //        if (IntervalPeriod == 15)
                    //        {
                    //            graphChart.ChartAreas["AREA3"].AxisX.LabelStyle.Interval = 4 * 24;
                    //            graphChart.ChartAreas["AREA3"].AxisX.MajorGrid.Interval = 4 * 24;
                    //            graphChart.ChartAreas["AREA3"].AxisX.MajorTickMark.Interval = 4 * 24;
                    //        }
                    //        else if (IntervalPeriod == 30)
                    //        {
                    //            graphChart.ChartAreas["AREA3"].AxisX.LabelStyle.Interval = 2 * 24;
                    //            graphChart.ChartAreas["AREA3"].AxisX.MajorGrid.Interval = 2 * 24;
                    //            graphChart.ChartAreas["AREA3"].AxisX.MajorTickMark.Interval = 2 * 24;

                    //        }
                    //        else if (IntervalPeriod == 60)
                    //        {
                    //            graphChart.ChartAreas["AREA3"].AxisX.LabelStyle.Interval = 1 * 24;
                    //            graphChart.ChartAreas["AREA3"].AxisX.MajorGrid.Interval = 1 * 24;
                    //            graphChart.ChartAreas["AREA3"].AxisX.MajorTickMark.Interval = 1 * 24;
                    //        }
                    //    }
                    //    //SetColorForSeries(graphChart);

                    //    // Assigning the Chart Title for X - Axis
                    //    graphChart.ChartAreas["AREA3"].AxisX.Title = tslabelDate.Text;
                    //    graphChart.ChartAreas["AREA3"].AxisX.TitleAlignment = StringAlignment.Center;

                    //    // Assigning the Chart Title for Y - Axis
                    //    graphChart.ChartAreas["AREA3"].AxisY.Title = "No Parameter";
                    //    graphChart.ChartAreas["AREA3"].AxisY.TitleAlignment = StringAlignment.Center;
                    //    graphChart.ChartAreas["AREA3"].AxisY.TextOrientation = TextOrientation.Stacked;

                    //    graphChart.ChartAreas["AREA3"].Area3DStyle.Enable3D = Viewtype;

                    //    graphChart.ChartAreas["AREA3"].BorderDashStyle = ChartDashStyle.Solid;
                    //    graphChart.ChartAreas["AREA3"].BorderWidth = 3;

                    //}
                    # endregion
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
                    for (int colCount = 0; colCount < GraphDataSet.Tables[0].Columns.Count - 2; colCount++)
                    {
                        // taking the series name from the column name
                        seriesName = fGetSeriesName(GraphDataSet.Tables[0].Columns[colCount].ToString());

                        graphChart.Series.Add(seriesName);

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
                        // to solve bug 92008
                        int[] nopower = new int[5760];
                        int[] noLoad = new int[5760];
                        int rcnt = 0;
                        //foreach (DataRow dRow in graphTable.Rows)
                        //{
                        //    if (dRow[0].ToString() == "0") noLoad[statindexNL++] = rcnt;
                        //    if (dRow[0].ToString().StartsWith("-")) nopower[statindexNP++] = rcnt;
                        //    rcnt++;
                        //}
                        foreach (DataRow dRow in graphTable.Rows)
                        {
                            if (dRow[POWERSTATUS].ToString() == NOLOAD)
                            {
                                noLoad[statindexNL++] = rcnt;
                            }
                            if (dRow[POWERSTATUS].ToString() == NOPOWER)
                            {
                                nopower[statindexNP++] = rcnt;
                            }
                            rcnt++;
                        }
                        //---------------------------No Load------------------------
                        double startpoint = 0.00;
                        double Endpoint = 0.00;
                        statindexNL = 0;
                        int ismultioff = 0;
                        int diffflg = 1;
                        while (noLoad[statindexNL] != 0 || noLoad[statindexNL + 1] != 0)
                        {
                            ismultioff = 0;
                            diffflg = 2;
                            if (noLoad[statindexNL + 1] != 0) diffflg = noLoad[statindexNL + 1] - noLoad[statindexNL];
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
                                // Fix by Swati . Changed 'L' to 'NL'.
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
                        while (nopower[statindexNP] != 0 || nopower[statindexNP + 1] != 0)
                        {
                            ismultioff = 0;
                            diffflg = 2;
                            if (nopower[statindexNP + 1] != 0) diffflg = nopower[statindexNP + 1] - nopower[statindexNP];
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
                                // Fix by Swati . Changed 'P' to 'NP'.
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
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show(ex.Message);
                logger.Log(LOGLEVELS.Error, "DisplayChart()", ex);
            }
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
            DLMS650LoadSurveyBLL dlms650LoadSurveyBLL = new DLMS650LoadSurveyBLL();
            //long fDate = ParseDate(DateUtility.DateTimeToLong(FromDate), true);
            //long tDate = ParseDate(Convert.ToString(ToDate), true);
            long fDate = ParseDate(FromDate.ToString(), true);
            long tDate = ParseDate(ToDate.ToString(), false);

            DataSet dataSet = new DataSet();
            if (parameterName == "Energy")
                dataSet = dlms650LoadSurveyBLL.ListDataSet(MeterDataId, FromDate, ToDate, "ENERGY", true); // Story - 427028 - Load survey data sequence should be in descending order except graph
            // SB Code Change Start - 20171204 - Export Energy Display in Load Survey Graph
            else if (parameterName == "Energy (Export)")
                dataSet = dlms650LoadSurveyBLL.ListDataSet(MeterDataId, FromDate, ToDate, "ENERGYEXPORT", true);
            // SB Code Change End - 20171204 - Export Energy Display in Load Survey Graph
            else if (parameterName == "Power Factor")
            {
                dataSet = dlms650LoadSurveyBLL.ListDataSet(MeterDataId, FromDate, ToDate, "PowerFactor", true);
            }
            else if (parameterName == "Frequency")
            {
                dataSet = dlms650LoadSurveyBLL.ListDataSet(MeterDataId, FromDate, ToDate, "Frequency", true);
            }
            //// SB Code Change End - 20180129 - Export Demand Display in Load Survey Graph
            //else if (parameterName == "Demand (Export)")
            //{
            //    dataSet = dlms650LoadSurveyBLL.ListDataSet(MeterDataId, FromDate, ToDate, "DEMANDEXPORT", true);
            //}
            //// SB Code Change End - 20180129 - Export Demand Display in Load Survey Graph
            else
                dataSet = dlms650LoadSurveyBLL.ListDataSet(MeterDataId, FromDate, ToDate, "Demand", true);
            // Added to solve bug 72902 for Graphical view 16th April 2012.
            if (dataSet == null)
            {
                if (dlms650LoadSurveyBLL.IntegrationPeriodStatus)
                {
                    MessageBox.Show("Data Corrupt", "BCS");
                    Application.DoEvents();
                    return null;
                }
            }
            if (dataSet != null && dataSet.Tables[0] != null && dataSet.Tables[0].Rows.Count > 0)
            {

                if (Convert.ToString(dataSet.Tables[0].Rows[0]["Real Time Clock - Date and Time (0.0.1.0.0.255;8;2)"]).Substring(10, 8).Trim() == "00 : 00")
                    dataSet.Tables[0].Rows[0].Delete();

            }

            return ConvertParameterToDataSet(dataSet, parameterList);
        }
        DateTime GetStartdate(String strdate)
        {
            string[] val = strdate.Split(' ');

            int Year = Convert.ToInt32(val[0].Substring(6, 4));
            //int Month = Convert.ToInt32(val[0].Substring(3, 2));
            //int Day = Convert.ToInt32(val[0].Substring(0, 2));
            int Month;
            int Day;
            int Hour = 0;
            int min = 0;
            //  if (this.IntervalPeriod == 60)
            //{
            //    Hour = 1;
            //    min = 0;
            //}
            //else
            //{
            //    Hour = 0;
            //    min = this.IntervalPeriod;
            //}

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
        private bool CanShowNPNL()
        {
            LoadSurveyParameterBLL loadSurveyParameterBLL = new LoadSurveyParameterBLL();
            bool canShowStatus = loadSurveyParameterBLL.CanShowNPNL(MeterDataId, ConfigInfo.ActiveMeterType);
            if (canShowStatus)
                return true;
            else
                return false;
        }
        private DataSet ConvertParameterToDataSet(DataSet dataSet, ArrayList parameterlist)
        {
            DataTable table = new DataTable();
            for (int i = 0; i < parameterlist.Count; i++)
                table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(parameterlist[i].ToString()), typeof(System.String)));
            table.Columns.Add(POWERSTATUS);
            decimal yCurrent = 0;
            decimal bCurrent = 0;
            decimal rCurrent = 0;
            decimal rVoltage = 0;
            decimal yVoltage = 0;
            decimal bVoltage = 0;
            decimal demandkW = 0;
            decimal demandkVA = 0;
            decimal demandkVALag = 0;
            decimal demandkVALead = 0;

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

            bool canShowStatus = CanShowNPNL();
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
                            if (parameterlist[listCount].ToString() == "RPhaseVoltage")
                            {
                                newrow["RPhaseVoltage"] = defaultval;
                            }
                            else if (parameterlist[listCount].ToString() == "YPhaseVoltage")
                            {
                                newrow["YPhaseVoltage"] = defaultval;
                            }
                            else if (parameterlist[listCount].ToString() == "BPhaseVoltage")
                            {
                                newrow["BPhaseVoltage"] = defaultval;
                            }
                            else if (parameterlist[listCount].ToString() == "AverageVoltage")
                            {
                                newrow["AverageVoltage"] = defaultval;
                            }

                            else if (parameterlist[listCount].ToString() == "RPhaseCurrent")
                            {
                                newrow["RPhaseCurrent"] = defaultval;
                            }
                            else if (parameterlist[listCount].ToString() == "YPhaseCurrent")
                            {
                                newrow["YPhaseCurrent"] = defaultval;
                            }
                            else if (parameterlist[listCount].ToString() == "BPhaseCurrent")
                            {
                                newrow["BPhaseCurrent"] = defaultval;
                            }
                            else if (parameterlist[listCount].ToString() == "AverageCurrent")
                            {
                                newrow["AverageCurrent"] = defaultval;
                            }
                            else if (parameterlist[listCount].ToString() == "DemandKW")
                            {
                                newrow["DemandKW"] = defaultval;
                            }
                            else if (parameterlist[listCount].ToString() == "DemandKVARLag")
                            {
                                newrow["DemandKVARLag"] = defaultval;
                            }
                            else if (parameterlist[listCount].ToString() == "DemandKVARLead")
                            {
                                newrow["DemandKVARLead"] = defaultval;
                            }
                            else if (parameterlist[listCount].ToString() == "DemandKVA")
                            {
                                newrow["DemandKVA"] = defaultval;
                            }
                            else if (parameterlist[listCount].ToString() == "EnergyKWh")
                            {
                                newrow["EnergyKWh"] = defaultval;
                            }
                            else if (parameterlist[listCount].ToString() == "EnergyKVARhLag")
                            {
                                newrow["EnergyKVARhLag"] = defaultval;
                            }
                            else if (parameterlist[listCount].ToString() == "EnergyKVARhLead")
                            {
                                newrow["EnergyKVARhLead"] = defaultval;
                            }
                            else if (parameterlist[listCount].ToString() == "EnergyKVAh")
                            {
                                newrow["EnergyKVAh"] = defaultval;
                            }
                            else if (parameterlist[listCount].ToString() == "LoadSurveyDateTime")
                            {
                                newrow["LoadSurveyDateTime"] = DateUtility.LongToStringDateTimeFormat(DateUtility.DateTimeToLong(nextDate));
                            }
                            //HTCT Specific
                            else if (parameterlist[listCount].ToString() == "DemandMW")
                            {
                                newrow["DemandMW"] = defaultval;
                            }
                            else if (parameterlist[listCount].ToString() == "DemandMVARLag")
                            {
                                newrow["DemandMVARLag"] = defaultval;
                            }
                            else if (parameterlist[listCount].ToString() == "DemandMVARLead")
                            {
                                newrow["DemandMVARLead"] = defaultval;
                            }
                            else if (parameterlist[listCount].ToString() == "DemandMVA")
                            {
                                newrow["DemandMVA"] = defaultval;
                            }
                            else if (parameterlist[listCount].ToString() == "EnergyMWh")
                            {
                                newrow["EnergyMWh"] = defaultval;
                            }
                            else if (parameterlist[listCount].ToString() == "EnergyMVARhLag")
                            {
                                newrow["EnergyMVARhLag"] = defaultval;
                            }
                            else if (parameterlist[listCount].ToString() == "EnergyMVARhLead")
                            {
                                newrow["EnergyMVARhLead"] = defaultval;
                            }
                            else if (parameterlist[listCount].ToString() == "EnergyMVAh")
                            {
                                newrow["EnergyMVAh"] = defaultval;
                            }
                            else if (parameterlist[listCount].ToString() == "PowerFactor")
                            {
                                newrow["PowerFactor"] = defaultval;
                            }
                            else if (parameterlist[listCount].ToString() == "Tamper Flag")
                            {
                                newrow["Tamper Flag"] = defaultval;
                            }
                            else if (parameterlist[listCount].ToString() == "Avg. Voltage 3 Phase")
                            {
                                newrow["Avg. Voltage 3 Phase"] = defaultval;
                            }
                            else if (parameterlist[listCount].ToString() == "Avg.R Phase PF")
                            {
                                newrow["Avg.R Phase PF"] = defaultval;
                            }
                            else if (parameterlist[listCount].ToString() == "Avg.Y Phase PF")
                            {
                                newrow["Avg.Y Phase PF"] = defaultval;
                            }
                            else if (parameterlist[listCount].ToString() == "Avg.B Phase PF")
                            {
                                newrow["Avg.B Phase PF"] = defaultval;
                            }
                            else if (parameterlist[listCount].ToString() == "Avg.Total PF")
                            {
                                newrow["Avg.Total PF"] = defaultval;
                            }
                            //SarkarA code change start 20180323 // add Demand & Energy Export, neutral current params
                            else if (parameterlist[listCount].ToString() == "DemandKWExport")
                            {
                                newrow["DemandKWExport"] = defaultval;
                            }
                            else if (parameterlist[listCount].ToString() == "DemandKVAExport")
                            {
                                newrow["DemandKVAExport"] = defaultval;
                            }
                            else if (parameterlist[listCount].ToString() == "DemandKVARLagExport")
                            {
                                newrow["DemandKVARLagExport"] = defaultval;
                            }
                            else if (parameterlist[listCount].ToString() == "DemandKVARLeadExport")
                            {
                                newrow["DemandKVARLeadExport"] = defaultval;
                            }
                            else if (parameterlist[listCount].ToString() == "EnergyKWhExport")
                            {
                                newrow["EnergyKWhExport"] = defaultval;
                            }
                            else if (parameterlist[listCount].ToString() == "EnergyKVAhExport")
                            {
                                newrow["EnergyKVAhExport"] = defaultval;
                            }
                            else if (parameterlist[listCount].ToString() == "EnergyKVARhLagExport")
                            {
                                newrow["EnergyKVARhLagExport"] = defaultval;
                            }
                            else if (parameterlist[listCount].ToString() == "EnergyKVARhLeadExport")
                            {
                                newrow["EnergyKVARhLeadExport"] = defaultval;
                            }
                            else if (parameterlist[listCount].ToString() == "NeutralCurrent")
                            {
                                newrow["NeutralCurrent"] = defaultval;
                            }
                            else if (parameterlist[listCount].ToString() == "Frequency")
                            {
                                newrow["Frequency"] = defaultval;
                            }
                            else if (parameterlist[listCount].ToString() == "AvgNeutralCurrent")
                            {
                                newrow["AvgNeutralCurrent"] = defaultval;
                            }
                            //SarkarA code change end 20180332 // add Demand & Energy Export params
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
                if (ConfigInfo.ActiveMeterType == "1P-2W")
                {
                    decimal.TryParse(Convert.ToString(dr[8]), out rVoltage);
                    decimal.TryParse(Convert.ToString(dr[4]), out rCurrent);
                    decimal.TryParse(Convert.ToString(dr[9]), out demandkW);
                    decimal.TryParse(Convert.ToString(dr[12]), out demandkVA);
                }
                else
                {
                    decimal.TryParse(Convert.ToString(dr[5]), out rVoltage);
                    decimal.TryParse(Convert.ToString(dr[6]), out yVoltage);
                    decimal.TryParse(Convert.ToString(dr[7]), out bVoltage);
                    decimal.TryParse(Convert.ToString(dr[1]), out rCurrent);
                    decimal.TryParse(Convert.ToString(dr[2]), out yCurrent);
                    decimal.TryParse(Convert.ToString(dr[3]), out bCurrent);
                    decimal.TryParse(Convert.ToString(dr[9]), out demandkW);
                    decimal.TryParse(Convert.ToString(dr[10]), out demandkVALag);
                    decimal.TryParse(Convert.ToString(dr[11]), out demandkVALead);
                    decimal.TryParse(Convert.ToString(dr[12]), out demandkVA);
                }



                for (int listCount = 0; listCount < parameterlist.Count; listCount++)
                {
                    if (parameterlist[listCount].ToString() == "RPhaseVoltage")
                    {
                        newrow["RPhaseVoltage"] = Convert.ToString(dr[VOLTAGEVRN]);
                    }
                    else if (parameterlist[listCount].ToString() == "YPhaseVoltage")
                    {
                        newrow["YPhaseVoltage"] = Convert.ToString(dr[VOLTAGEVYN]);
                    }
                    else if (parameterlist[listCount].ToString() == "BPhaseVoltage")
                    {
                        newrow["BPhaseVoltage"] = Convert.ToString(dr[VOLTAGEVBN]);
                    }
                    else if (parameterlist[listCount].ToString() == "AverageVoltage")
                    {
                        newrow["AverageVoltage"] = Convert.ToString(dr[AVERAGEVOLTAGE]);
                    }
                    else if (parameterlist[listCount].ToString() == "RPhaseCurrent")
                    {
                        newrow["RPhaseCurrent"] = Convert.ToString(dr[CURRENTIR]);
                    }
                    else if (parameterlist[listCount].ToString() == "YPhaseCurrent")
                    {
                        newrow["YPhaseCurrent"] = Convert.ToString(dr[CURRENTIY]);
                    }
                    else if (parameterlist[listCount].ToString() == "BPhaseCurrent")
                    {
                        newrow["BPhaseCurrent"] = Convert.ToString(dr[CURRENTIB]);
                    }
                    // SB Code Change Start - 20171214 - Neutral Current in Load Survey Graph
                    else if (parameterlist[listCount].ToString() == "NeutralCurrent")
                    {
                        newrow["NeutralCurrent"] = Convert.ToString(dr[NEUTRALCURRENT]);
                    }
                    // SB Code Change End - 20171214 - Neutral Current in Load Survey Graph
                    else if (parameterlist[listCount].ToString() == "AverageCurrent")
                    {
                        newrow["AverageCurrent"] = Convert.ToString(dr[AVERAGECURRENT]);
                    }
                    else if (parameterlist[listCount].ToString() == "DemandKW")
                    {
                        newrow["DemandKW"] = Convert.ToString(dr[DEMANDKW]);
                    }
                    else if (parameterlist[listCount].ToString() == "DemandKVARLag")
                    {
                        newrow["DemandKVARLag"] = Convert.ToString(dr[DEMANKKVAHLAG]);
                    }
                    else if (parameterlist[listCount].ToString() == "DemandKVARLead")
                    {
                        newrow["DemandKVARLead"] = Convert.ToString(dr[DEMANDKVAHLEAD]);
                    }
                    else if (parameterlist[listCount].ToString() == "DemandKVA")
                    {
                        newrow["DemandKVA"] = Convert.ToString(dr[DEMANDKVA]);
                    }
                    // SB Code Change Start - 20180129 - Export Demand Display in Load Survey Graph
                    else if (parameterlist[listCount].ToString() == "DemandKWExport")
                    {
                        newrow["DemandKWExport"] = Convert.ToString(dr[DEMANDKWEXPORT]);
                    }
                    else if (parameterlist[listCount].ToString() == "DemandKVARLagExport")
                    {
                        newrow["DemandKVARLagExport"] = Convert.ToString(dr[DEMANDKKVARLAGEXPORT]);
                    }
                    else if (parameterlist[listCount].ToString() == "DemandKVARLeadExport")
                    {
                        newrow["DemandKVARLeadExport"] = Convert.ToString(dr[DEMANDKKVARLEADEXPORT]);
                    }
                    else if (parameterlist[listCount].ToString() == "DemandKVAExport")
                    {
                        newrow["DemandKVAExport"] = Convert.ToString(dr[DEMANDKVAEXPORT]);
                    }
                    // SB Code Change End - 20180129 - Export Demand Display in Load Survey Graph
                    else if (parameterlist[listCount].ToString() == "EnergyKWh")
                    {
                        newrow["EnergyKWh"] = Convert.ToString(dr[ENEGRYKWH]);
                    }
                    else if (parameterlist[listCount].ToString() == "EnergyKVARhLag")
                    {
                        newrow["EnergyKVARhLag"] = Convert.ToString(dr[ENERGYKKVARHLAG]);
                    }
                    else if (parameterlist[listCount].ToString() == "EnergyKVARhLead")
                    {
                        newrow["EnergyKVARhLead"] = Convert.ToString(dr[ENERGYKKVARHLEAD]);
                    }
                    else if (parameterlist[listCount].ToString() == "EnergyKVAh")
                    {
                        newrow["EnergyKVAh"] = Convert.ToString(dr[ENERGYKVAH]);
                    }
                    // SB Code Change Start - 20171204 - Export Energy Display in Load Survey Graph
                    else if (parameterlist[listCount].ToString() == "EnergyKWhExport")
                    {
                        newrow["EnergyKWhExport"] = Convert.ToString(dr[ENEGRYKWHEXPORT]);
                    }
                    else if (parameterlist[listCount].ToString() == "EnergyKVARhLagExport")
                    {
                        newrow["EnergyKVARhLagExport"] = Convert.ToString(dr[ENERGYKKVARHLAGEXPORT]);
                    }
                    else if (parameterlist[listCount].ToString() == "EnergyKVARhLeadExport")
                    {
                        newrow["EnergyKVARhLeadExport"] = Convert.ToString(dr[ENERGYKKVARHLEADEXPORT]);
                    }
                    else if (parameterlist[listCount].ToString() == "EnergyKVAhExport")
                    {
                        newrow["EnergyKVAhExport"] = Convert.ToString(dr[ENERGYKVAHEXPORT]);
                    }
                    // SB Code Change End - 20171204 - Export Energy Display in Load Survey Graph
                    else if (parameterlist[listCount].ToString() == "LoadSurveyDateTime")
                    {
                        newrow["LoadSurveyDateTime"] = Convert.ToString(dr[DATETIME]);
                    }
                    // HTCT Specific
                    else if (parameterlist[listCount].ToString() == "DemandMW")
                    {
                        newrow["DemandMW"] = Convert.ToString(dr[DEMANDMW]);
                    }
                    else if (parameterlist[listCount].ToString() == "DemandMVARLag")
                    {
                        newrow["DemandMVARLag"] = Convert.ToString(dr[DEMANDMVAHLAG]);
                    }
                    else if (parameterlist[listCount].ToString() == "DemandMVARLead")
                    {
                        newrow["DemandMVARLead"] = Convert.ToString(dr[DEMANDMVAHLEAD]);
                    }
                    else if (parameterlist[listCount].ToString() == "DemandMVA")
                    {
                        newrow["DemandMVA"] = Convert.ToString(dr[DEMANDMVA]);
                    }
                    // SB Code Change Start - 20180129 - Export Demand Display in Load Survey Graph
                    else if (parameterlist[listCount].ToString() == "DemandMWExport")
                    {
                        newrow["DemandMWExport"] = Convert.ToString(dr[DEMANDMWEXPORT]);
                    }
                    else if (parameterlist[listCount].ToString() == "DemandMVARLagExport")
                    {
                        newrow["DemandMVARLagExport"] = Convert.ToString(dr[DEMANDMVARLAGEXPORT]);
                    }
                    else if (parameterlist[listCount].ToString() == "DemandMVARLeadExport")
                    {
                        newrow["DemandMVARLeadExport"] = Convert.ToString(dr[DEMANDMVARLEADEXPORT]);
                    }
                    else if (parameterlist[listCount].ToString() == "DemandMVExport")
                    {
                        newrow["DemandMVAExport"] = Convert.ToString(dr[DEMANDMVAEXPORT]);
                    }
                    // SB Code Change End - 20180129 - Export Demand Display in Load Survey Graph
                    else if (parameterlist[listCount].ToString() == "EnergyMWh")
                    {
                        newrow["EnergyMWh"] = Convert.ToString(dr[ENEGRYMWH]);
                    }
                    else if (parameterlist[listCount].ToString() == "EnergyMVARhLag")
                    {
                        newrow["EnergyMVARhLag"] = Convert.ToString(dr[ENERGYMVARHLAG]);
                    }
                    else if (parameterlist[listCount].ToString() == "EnergyMVARhLead")
                    {
                        newrow["EnergyMVARhLead"] = Convert.ToString(dr[ENERGYMVARHLEAD]);
                    }
                    else if (parameterlist[listCount].ToString() == "EnergyMVAh")
                    {
                        newrow["EnergyMVAh"] = Convert.ToString(dr[ENERGYMVAH]);
                    }
                    // SB Code Change Start - 20171204 - Export Energy Display in Load Survey Graph
                    else if (parameterlist[listCount].ToString() == "EnergyMWhExport")
                    {
                        newrow["EnergyMWhExport"] = Convert.ToString(dr[ENEGRYMWHEXPORT]);
                    }
                    else if (parameterlist[listCount].ToString() == "EnergyMVARhLagExport")
                    {
                        newrow["EnergyMVARhLagExport"] = Convert.ToString(dr[ENERGYMVARHLAGEXPORT]);
                    }
                    else if (parameterlist[listCount].ToString() == "EnergyMVARhLeadExport")
                    {
                        newrow["EnergyMVARhLeadExport"] = Convert.ToString(dr[ENERGYMVARHLEADEXPORT]);
                    }
                    else if (parameterlist[listCount].ToString() == "EnergyMVAhExport")
                    {
                        newrow["EnergyMVAhExport"] = Convert.ToString(dr[ENERGYMVAHEXPORT]);
                    }
                    // SB Code Change End - 20171204 - Export Energy Display in Load Survey Graph
                    else if (parameterlist[listCount].ToString() == "PowerFactor")
                    {
                        newrow["PowerFactor"] = Convert.ToString(dr[POWERFACTOR]);
                    }
                    else if (parameterlist[listCount].ToString() == "Tamper Flag")
                    {
                        newrow["Tamper Flag"] = Convert.ToString(dr[TEMPFLAG]);
                    }
                    else if (parameterlist[listCount].ToString() == "Avg. Voltage 3 Phase")
                    {
                        newrow["Avg. Voltage 3 Phase"] = Convert.ToString(dr[AVGVOLT3PHASE]);
                    }
                    else if (parameterlist[listCount].ToString() == "Avg.R Phase PF")
                    {
                        newrow["Avg.R Phase PF"] = Convert.ToString(dr[AVGRPHPF]);
                    }
                    else if (parameterlist[listCount].ToString() == "Avg.Y Phase PF")
                    {
                        newrow["Avg.Y Phase PF"] = Convert.ToString(dr[AVGYPHPF]);
                    }
                    else if (parameterlist[listCount].ToString() == "Avg.B Phase PF")
                    {
                        newrow["Avg.B Phase PF"] = Convert.ToString(dr[AVGBPHPF]);
                    }
                    else if (parameterlist[listCount].ToString() == "Avg.Total PF")
                    {
                        newrow["Avg.Total PF"] = Convert.ToString(dr[AVGTOTALPF]);
                    }

                    else if (parameterlist[listCount].ToString() == "AvgNeutralCurrent")
                    {
                        newrow["AvgNeutralCurrent"] = Convert.ToString(dr[LSneucurrent]);
                    }
                    else if (parameterlist[listCount].ToString() == "THDVR")
                    {
                        newrow["THDVR"] = Convert.ToString(dr[LSTHDVR]);
                    }
                    else if (parameterlist[listCount].ToString() == "THDVY")
                    {
                        newrow["THDVY"] = Convert.ToString(dr[LSTHDVY]);
                    }
                    else if (parameterlist[listCount].ToString() == "THDVB")
                    {
                        newrow["THDVB"] = Convert.ToString(dr[LSTHDVB]);
                    }
                    else if (parameterlist[listCount].ToString() == "THDIR")
                    {
                        newrow["THDIR"] = Convert.ToString(dr[LSTHDIR]);
                    }
                    else if (parameterlist[listCount].ToString() == "THDIY")
                    {
                        newrow["THDIY"] = Convert.ToString(dr[LSTHDIY]);
                    }
                    else if (parameterlist[listCount].ToString() == "THDIB")
                    {
                        newrow["THDIB"] = Convert.ToString(dr[LSTHDIB]);
                    }
                    else if (parameterlist[listCount].ToString() == "Frequency")
                    {
                        newrow["Frequency"] = Convert.ToString(dr[FREQUENCY]);
                    }

                    if (canShowStatus)
                    {
                        if (ConfigInfo.ActiveMeterType == "1P-2W")
                        {
                            if ((demandkW <= 0) && (demandkVA <= 0))
                            {
                                if ((rVoltage <= 0) && (rCurrent <= 0))
                                {
                                    newrow[POWERSTATUS] = NOPOWER;
                                }
                                else
                                {
                                    newrow[POWERSTATUS] = NOLOAD;
                                }
                            }
                            else
                            {
                                newrow[POWERSTATUS] = NORMAL;
                            }
                        }
                        else
                        {
                            if ((demandkW <= 0) && (demandkVA <= 0) && (demandkVALag <= 0) && (demandkVALead <= 0))
                            {
                                if ((rVoltage <= 0) && (yVoltage <= 0) && (bVoltage <= 0) && (rCurrent <= 0) && (yCurrent <= 0) && (bCurrent <= 0))
                                {
                                    newrow[POWERSTATUS] = NOPOWER;
                                }
                                else
                                {
                                    newrow[POWERSTATUS] = NOLOAD;
                                }
                            }
                            else
                            {
                                newrow[POWERSTATUS] = NORMAL;
                            }
                        }
                    }
                    else
                    {
                        newrow[POWERSTATUS] = NORMAL;
                    }
                }
            }
            DataSet ds = new DataSet();
            ds.Tables.Add(table);

            return ds;
        }

        DateTime ConvertStringTodate(String strdate)
        {
            string[] val = strdate.Split(' ');
            int Year = Convert.ToInt32(val[0].Substring(6, 4));
            int Month = Convert.ToInt32(val[0].Substring(3, 2));
            int Day = Convert.ToInt32(val[0].Substring(0, 2));
            int Hour = Convert.ToInt32(val[1]);
            int min = Convert.ToInt32(val[3]);
            return new DateTime(Year, Month, Day, Hour, min, 0);

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
            lastDate = ConvertStringTodate(dataSet.Tables[0].Rows[dataSet.Tables[0].Rows.Count - 1]["LoadSurveyDateTime"].ToString());
            //lastDate = ConvertStringTodate(dataSet.Tables[0].Rows[1]["LoadSurveyDateTime"].ToString());
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
            ///
            if (ConfigInfo.ActiveMeterType == "1P-2W" && ConfigInfo.ActiveFileType == "NONDLMS")
            {
                cboParameters.Items.Remove("Power Factor");
                // SB Code Change Start - Export Energy Display in Load Survey Graph
                cboParameters.Items.Remove("Energy (Export)");
                // SB Code Change End - Export Energy Display in Load Survey Graph
                // SB Code Change Start - Export Demand Display in Load Survey Graph
                cboParameters.Items.Remove("Demand (Export)");
                // SB Code Change End - Export Demand Display in Load Survey Graph
            }
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

            MeterDataEntity meterDataEntity = new MeterDataBLL().GetDetailData(MeterDataId) as MeterDataEntity;
            txtMeterID.Text = meterDataEntity.MeterID;
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
            int monthNumber = 0;
            if (GraphView == 0)
            {
                if (currentRecordCount >= MaxValue)
                {
                    //nextDate = nextDate.AddDays(-1);
                    //tslabelDate.Text = String.Format("{0:00}", nextDate.Day) + "/" + String.Format("{0:00}", nextDate.Month) + "/" + String.Format("{0:0000}", nextDate.Year);
                    currentRecordCount = currentRecordCount - MaxValue;
                    CurrentRecord = currentRecordCount;
                    // DisplayChart();
                    nextDate = Convert.ToDateTime(GraphDataSet.Tables[0].Rows[CurrentRecord]["LoadSurveyDateTime"].ToString(), new System.Globalization.CultureInfo("en-GB", false));
                    tslabelDate.Text = String.Format("{0:00}", nextDate.Day) + "/" + String.Format("{0:00}", nextDate.Month) + "/" + String.Format("{0:0000}", nextDate.Year);
                    //VBM - To solve x-axis date issue while clicking on previous button.
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
                    monthNumber = nextDate.Month;
                    if (nextDate.Month > 1)
                        monthNumber = monthNumber - 1;
                    numberOfDays = System.DateTime.DaysInMonth(nextDate.Year, monthNumber);
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
            int monthNumber = 0;
            //Get the Record Count for the dataset that is generated
            recordCount = GetRecordCount(GraphDataSet);
            if (GraphView == 0)
            {
                if (recordCount > currentRecordCount + MaxValue)
                {
                    //nextDate = nextDate.AddDays(1);

                    currentRecordCount = currentRecordCount + MaxValue;
                    CurrentRecord = currentRecordCount;
                    // nextDate = Convert.ToDateTime(GraphDataSet.Tables[0].Rows[CurrentRecord]["LoadSurveyDateTime"].ToString());
                    //tslabelDate.Text = String.Format("{0:00}", nextDate.Day) + "/" + String.Format("{0:00}", nextDate.Month) + "/" + String.Format("{0:0000}", nextDate.Year);
                    nextDate = Convert.ToDateTime(GraphDataSet.Tables[0].Rows[CurrentRecord]["LoadSurveyDateTime"].ToString(), new System.Globalization.CultureInfo("en-GB", false));
                    tslabelDate.Text = String.Format("{0:00}", nextDate.Day) + "/" + String.Format("{0:00}", nextDate.Month) + "/" + String.Format("{0:0000}", nextDate.Year);
                    //tslabelDate.Text = GraphDataSet.Tables[0].Rows[CurrentRecord]["LoadSurveyDateTime"].ToString();
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
                //if ( nextDate.Month == 12 )
                //    monthNumber = 1;
                //else
                //    monthNumber = nextDate.Month;

                if (recordCount > (currentRecordCount + (MaxValue)))
                {
                    //numberOfDays = System.DateTime.DaysInMonth(nextDate.Year, monthNumber);
                    //nextDate = nextDate.AddDays(numberOfDays);
                    nextDate = nextDate.AddMonths(1);
                    tslabelDate.Text = String.Format("{0:00}", nextDate.Day) + "/" + String.Format("{0:00}", nextDate.Month) + "/" + String.Format("{0:0000}", nextDate.Year);
                    //if ( monthNumber == 1)
                    //    numberOfDays = System.DateTime.DaysInMonth(nextDate.Year, 1);
                    //else
                    //    numberOfDays = System.DateTime.DaysInMonth(nextDate.Year, monthNumber + 1);

                    lastDate = nextDate.AddMonths(1);
                    //lastDate = nextDate.AddDays(numberOfDays);
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
                /*GKG : 148275 Graph issue 09/05/2013 */
                if (nextDate.Hour == 0x00 && nextDate.Minute == 0x00)
                {
                    nextDate = nextDate.AddDays(-1);
                }
                /*GKG : 148275 Graph issue 09/05/2013 */
                tslabelDate.Text = String.Format("{0:00}", nextDate.Day) + "/" + String.Format("{0:00}", nextDate.Month) + "/" + String.Format("{0:0000}", nextDate.Year);
                currentRecordCount = recordCount - (recordCount % MaxValue);
                // Added to solve 92006. Added this case for rtc backforce condition.
                if (currentRecordCount == recordCount)
                {
                    currentRecordCount = recordCount - MaxValue;
                }
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
                nextDate = ConvertStringTodate(Convert.ToString(GraphDataSet.Tables[0].Rows[recordCount - 1]["LoadSurveyDateTime"]));

                tslabelDate.Text = String.Format("{0:00}", 1) + "/" + String.Format("{0:00}", nextDate.Month) + "/" + String.Format("{0:0000}", nextDate.Year);
                nextDate = Convert.ToDateTime(tslabelDate.Text);
                currentRecordCount = recordCount - (recordCount % (MaxValue));

                CurrentRecord = currentRecordCount;
                DisplayChart();
                lastDate = nextDate.AddMonths(1);
                tslabelDate.Text = tslabelDate.Text + "\n - \n" + String.Format("{0:00}", lastDate.Day) + "/" + String.Format("{0:00}", lastDate.Month) + "/" + String.Format("{0:0000}", lastDate.Year);


            }
        }
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            try
            {
                if (GraphType == true)
                {
                    for (int i = 0; i < GraphDataSet.Tables[0].Columns.Count - 2; i++)
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
            catch (Exception ex)    //Exception log for catch block
            {
                // to avoid craqsh for power factor in zoom mode in seprate graph type
                logger.Log(LOGLEVELS.Error, "toolStripMenuItem2_Click(object sender, EventArgs e)", ex);
            }
        }
        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            try
            {
                if (GraphType == true)
                {
                    for (int i = 0; i < GraphDataSet.Tables[0].Columns.Count - 2; i++)
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
            catch (Exception ex)    //Exception log for catch block
            {
                // to avoid craqsh for power factor in zoom mode in seprate graph type
                logger.Log(LOGLEVELS.Error, "toolStripMenuItem3_Click(object sender, EventArgs e)", ex);
            }
        }
        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            try
            {
                if (GraphType == true)
                {
                    for (int i = 0; i < GraphDataSet.Tables[0].Columns.Count - 2; i++)
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
            catch (Exception ex)    //Exception log for catch block
            {
                // to avoid craqsh for power factor in zoom mode in seprate graph type
                logger.Log(LOGLEVELS.Error, "toolStripMenuItem4_Click(object sender, EventArgs e)", ex);
            }
        }
        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            try
            {
                if (GraphType == true)
                {
                    for (int i = 0; i < GraphDataSet.Tables[0].Columns.Count - 2; i++)
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
            catch (Exception ex)    //Exception log for catch block
            {
                // to avoid craqsh for power factor in zoom mode in seprate graph type
                logger.Log(LOGLEVELS.Error, "toolStripMenuItem5_Click(object sender, EventArgs e)", ex);
            }
        }
        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            try
            {
                if (GraphType == true)
                {
                    for (int i = 0; i < GraphDataSet.Tables[0].Columns.Count - 2; i++)
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
            catch (Exception ex)    //Exception log for catch block
            {
                // to avoid craqsh for power factor in zoom mode in seprate graph type
                logger.Log(LOGLEVELS.Error, "toolStripMenuItem6_Click(object sender, EventArgs e)", ex);
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

            // To determine whether meter is HTCT or not. 
            int meterModelNumber = new DLMS650GeneralBLL().GetMeterModelNoByMeterDataID(ConfigInfo.ActiveMeterDataId);

            if (parameterName == "Voltage")
            {
                selectedParameterList.Clear();
                if (ConfigInfo.ActiveMeterType == "1P-2W")
                {
                    if (chkLBParameterDisplay.GetItemCheckState(0) == System.Windows.Forms.CheckState.Checked)
                    {
                        selectedParameterList.Add("AverageVoltage");
                    }
                }
                else
                {
                    if (chkLBParameterDisplay.GetItemCheckState(0) == System.Windows.Forms.CheckState.Checked)
                    {
                        selectedParameterList.Add("RPhaseVoltage");
                    }
                    if (chkLBParameterDisplay.GetItemCheckState(1) == System.Windows.Forms.CheckState.Checked)
                    {
                        selectedParameterList.Add("YPhaseVoltage");


                    }
                    if (chkLBParameterDisplay.GetItemCheckState(2) == System.Windows.Forms.CheckState.Checked)
                    {
                        selectedParameterList.Add("BPhaseVoltage");
                    }

                    if (isAverageVoltagePresent && chkLBParameterDisplay.GetItemCheckState(3) == System.Windows.Forms.CheckState.Checked)    //added for CSPDCL 3PH Containing Avg Voltage
                    {
                        selectedParameterList.Add("AverageVoltage");
                    }
                }
            }
            else if (parameterName == "Current")
            {
                selectedParameterList.Clear();
                if (ConfigInfo.ActiveMeterType == "1P-2W")
                {
                    if (chkLBParameterDisplay.GetItemCheckState(0) == System.Windows.Forms.CheckState.Checked)
                    {
                        selectedParameterList.Add("AverageCurrent");
                        if(columnNames.Contains("NEUTRALCURRENT") && chkLBParameterDisplay.GetItemCheckState(1) == System.Windows.Forms.CheckState.Checked)
                        {
                            selectedParameterList.Add("AvgNeutralCurrent");
                        }
                    }
                }
                else
                {
                    if (chkLBParameterDisplay.GetItemCheckState(0) == System.Windows.Forms.CheckState.Checked)
                    {
                        selectedParameterList.Add("RPhaseCurrent");
                    }
                    if (chkLBParameterDisplay.GetItemCheckState(1) == System.Windows.Forms.CheckState.Checked)
                    {
                        selectedParameterList.Add("YPhaseCurrent");
                    }
                    if (chkLBParameterDisplay.GetItemCheckState(2) == System.Windows.Forms.CheckState.Checked)
                    {
                        selectedParameterList.Add("BPhaseCurrent");
                    }
                    // SB Code Change Start - 20171214 - Neutral Current in Load Survey Graph
                    if (chkLBParameterDisplay.GetItemCheckState(3) == System.Windows.Forms.CheckState.Checked)
                    {
                        selectedParameterList.Add("NeutralCurrent");
                    }
                    // SB Code Change End - 20171214 - Neutral Current in Load Survey Graph
                }
            }
            else if (parameterName == "Demand")
            {
                selectedParameterList.Clear();
                if (chkLBParameterDisplay.GetItemCheckState(0) == System.Windows.Forms.CheckState.Checked)
                {
                    if (meterModelNumber == 10 || meterModelNumber == 28 || meterModelNumber == 29)
                        selectedParameterList.Add("DemandMW");
                    else
                        selectedParameterList.Add("DemandKW");
                }
                if (chkLBParameterDisplay.GetItemCheckState(1) == System.Windows.Forms.CheckState.Checked)
                {
                    if (meterModelNumber == 10 || meterModelNumber == 28 || meterModelNumber == 29)
                        selectedParameterList.Add("DemandMVARLag");
                    else
                        selectedParameterList.Add("DemandKVARLag");
                }
                if (chkLBParameterDisplay.GetItemCheckState(2) == System.Windows.Forms.CheckState.Checked)
                {
                    if (meterModelNumber == 10 || meterModelNumber == 28 || meterModelNumber == 29)
                        selectedParameterList.Add("DemandMVARLead");
                    else
                        selectedParameterList.Add("DemandKVARLead");
                }
                if (chkLBParameterDisplay.GetItemCheckState(3) == System.Windows.Forms.CheckState.Checked)
                {
                    if (meterModelNumber == 10 || meterModelNumber == 28 || meterModelNumber == 29)
                        selectedParameterList.Add("DemandMVA");
                    else
                        selectedParameterList.Add("DemandKVA");
                }
            }
            else if (parameterName == "Power Factor")
            {
                selectedParameterList.Clear();
                if (chkLBParameterDisplay.GetItemCheckState(0) == System.Windows.Forms.CheckState.Checked)
                {
                    selectedParameterList.Add("PowerFactor");
                }

            }
            else if (parameterName == "Frequency")
            {
                selectedParameterList.Clear();
                if (chkLBParameterDisplay.GetItemCheckState(0) == System.Windows.Forms.CheckState.Checked)
                {
                    selectedParameterList.Add("Frequency");
                }

            }
            // SB Code Change Start - 20171204 - Export Energy Display in Load Survey Graph
            else if (parameterName == "Energy (Export)")
            {
                selectedParameterList.Clear();
                if (chkLBParameterDisplay.GetItemCheckState(0) == System.Windows.Forms.CheckState.Checked)
                {
                    if (meterModelNumber == 10 || meterModelNumber == 28 || meterModelNumber == 29)
                        selectedParameterList.Add("EnergyMWhExport");
                    else
                        selectedParameterList.Add("EnergyKWhExport");
                }
                if (chkLBParameterDisplay.GetItemCheckState(1) == System.Windows.Forms.CheckState.Checked)
                {
                    if (meterModelNumber == 10 || meterModelNumber == 28 || meterModelNumber == 29)
                        selectedParameterList.Add("EnergyMVARhLagExport");
                    else
                        selectedParameterList.Add("EnergyKVARhLagExport");
                }
                if (chkLBParameterDisplay.GetItemCheckState(2) == System.Windows.Forms.CheckState.Checked)
                {
                    if (meterModelNumber == 10 || meterModelNumber == 28 || meterModelNumber == 29)
                        selectedParameterList.Add("EnergyMVARhLeadExport");
                    else
                        selectedParameterList.Add("EnergyKVARhLeadExport");
                }
                if (chkLBParameterDisplay.GetItemCheckState(3) == System.Windows.Forms.CheckState.Checked)
                {
                    if (meterModelNumber == 10 || meterModelNumber == 28 || meterModelNumber == 29)
                        selectedParameterList.Add("EnergyMVAhExport");
                    else
                        selectedParameterList.Add("EnergyKVAhExport");
                }
            }
            // SB Code Change End - 20171204 - Export Energy Display in Load Survey Graph

                // SB Code Change Start - 20180129 - Export Demand Display in Load Survey Graph
            else if (parameterName == "Demand (Export)")
            {
                selectedParameterList.Clear();
                if (chkLBParameterDisplay.GetItemCheckState(0) == System.Windows.Forms.CheckState.Checked)
                {
                    if (meterModelNumber == 10 || meterModelNumber == 28 || meterModelNumber == 29)
                        selectedParameterList.Add("DemandMWExport");
                    else
                        selectedParameterList.Add("DemandKWExport");
                }
                if (chkLBParameterDisplay.GetItemCheckState(1) == System.Windows.Forms.CheckState.Checked)
                {
                    if (meterModelNumber == 10 || meterModelNumber == 28 || meterModelNumber == 29)
                        selectedParameterList.Add("DemandMVARLagExport");
                    else
                        selectedParameterList.Add("DemandKVARLagExport");
                }
                if (chkLBParameterDisplay.GetItemCheckState(2) == System.Windows.Forms.CheckState.Checked)
                {
                    if (meterModelNumber == 10 || meterModelNumber == 28 || meterModelNumber == 29)
                        selectedParameterList.Add("DemandMVARLeadExport");
                    else
                        selectedParameterList.Add("DemandKVARLeadExport");
                }
                if (chkLBParameterDisplay.GetItemCheckState(3) == System.Windows.Forms.CheckState.Checked)
                {
                    if (meterModelNumber == 10 || meterModelNumber == 28 || meterModelNumber == 29)
                        selectedParameterList.Add("DemandMVAExport");
                    else
                        selectedParameterList.Add("DemandKVAExport");
                }
            }
            // SB Code Change End - 20180129 - Export Demand Display in Load Survey Graph
            else
            {
                selectedParameterList.Clear();
                if (chkLBParameterDisplay.GetItemCheckState(0) == System.Windows.Forms.CheckState.Checked)
                {
                    if (meterModelNumber == 10 || meterModelNumber == 28 || meterModelNumber == 29)
                        selectedParameterList.Add("EnergyMWh");
                    else
                        selectedParameterList.Add("EnergyKWh");
                }
                if (chkLBParameterDisplay.GetItemCheckState(1) == System.Windows.Forms.CheckState.Checked)
                {
                    if (meterModelNumber == 10 || meterModelNumber == 28 || meterModelNumber == 29)
                        selectedParameterList.Add("EnergyMVARhLag");
                    else
                        selectedParameterList.Add("EnergyKVARhLag");
                }
                if (chkLBParameterDisplay.GetItemCheckState(2) == System.Windows.Forms.CheckState.Checked)
                {
                    if (meterModelNumber == 10 || meterModelNumber == 28 || meterModelNumber == 29)
                        selectedParameterList.Add("EnergyMVARhLead");
                    else
                        selectedParameterList.Add("EnergyKVARhLead");
                }
                if (chkLBParameterDisplay.GetItemCheckState(3) == System.Windows.Forms.CheckState.Checked)
                {
                    if (meterModelNumber == 10 || meterModelNumber == 28 || meterModelNumber == 29)
                        selectedParameterList.Add("EnergyMVAh");
                    else
                        selectedParameterList.Add("EnergyKVAh");
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
                    // Added to solve bug 83429 .Color coding issue.
                    clrSeries1.BackColor = fGetSeriesColor(graphChart.Series[i].Name);
                }
                else if (i == 1)
                {
                    lblSeries2.Text = graphChart.Series[i].Name;
                    clrSeries2.BackColor = fGetSeriesColor(graphChart.Series[i].Name);
                }
                else if (i == 2)
                {
                    lblSeries3.Text = graphChart.Series[i].Name;
                    clrSeries3.BackColor = fGetSeriesColor(graphChart.Series[i].Name);
                }
                else if (i == 3)
                {
                    lblSeries4.Text = graphChart.Series[i].Name;
                    clrSeries4.BackColor = fGetSeriesColor(graphChart.Series[i].Name);
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

            LoadSurveyParameterBLL loadSurveyParameterBLL = new LoadSurveyParameterBLL();
            DataSet dSet = loadSurveyParameterBLL.GetColumnNames(MeterDataId);
            if (dSet != null && dSet.Tables.Count > 0 && dSet.Tables[0].Rows.Count > 0)
            {
                columnNames = dSet.Tables[0].Rows[0]["ColumnsNames"].ToString().ToUpper();
                if (!((columnNames.Contains("RPHASECURRENT")) || (columnNames.Contains("YPHASECURRENT"))
                    || (columnNames.Contains("BPHASECURRENT")) || (columnNames.Contains("AVERAGECURRENT") || (columnNames.Contains("AVGNEUTRALCURRENT")))))
                {
                    cboParameters.Items.Remove("Current");
                }
                if (!((columnNames.Contains("RPHASEVOLTAGE")) || (columnNames.Contains("YPHASEVOLTAGE"))
                    || (columnNames.Contains("BPHASEVOLTAGE")) || (columnNames.Contains("AVERAGEVOLTAGE"))))
                {
                    cboParameters.Items.Remove("Voltage");
                }
                //SarkarA code change start 20180323 // remove export for non-net meter
                if (!((columnNames.Contains("BLOCKENERGYKVAHEXPORT")) || (columnNames.Contains("BLOCKENERGYKWHEXPORT"))))
                {
                    cboParameters.Items.Remove("Demand (Export)");
                    cboParameters.Items.Remove("Energy (Export)");
                }
                //SarkarA cod change end 20180323 
                if (!(columnNames.Contains("FREQUENCY")))
                {
                    cboParameters.Items.Remove("Frequency");
                }
                if (columnNames.Contains("AVERAGEVOLTAGE"))
                {
                    isAverageVoltagePresent = true;
                }
            }
            //Get the column names for the parameter list which are filled without null values
            if (parameterName == "Voltage")
            {
                if (ConfigInfo.ActiveMeterType == "1P-2W")
                {
                    chkLBParameterDisplay.Items.Add("Average Voltage");
                    this.chkLBParameterDisplay.SetItemChecked(0, true);
                    lblSeries1.Text = "Average Voltage";
                    // Added to update the currentRecordCount if parameters is changed.
                    currentRecordCount = 0;
                    //Added to solve bug 87497. Current record should be parameters selection changed to start drawing graph from starting record.
                    CurrentRecord = 0;
                }
                else
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
                    //lblSeries4.Text = "Series 4";
                    if (isAverageVoltagePresent)    //added for CSPDCL 3PH Containing Avg Voltage
                    {
                        chkLBParameterDisplay.Items.Add("Average Voltage");
                        this.chkLBParameterDisplay.SetItemChecked(3, true);
                        lblSeries4.Text = "Average Voltage";
                    }
                    // Added to update the currentRecordCount if parameters is changed.
                    currentRecordCount = 0;
                    //Added to solve bug 87497. Current record should be parameters selection changed to start drawing graph from starting record.
                    CurrentRecord = 0;
                }
            }
            else if (parameterName == "Current")
            {
                if (ConfigInfo.ActiveMeterType == "1P-2W")
                {
                    chkLBParameterDisplay.Items.Add("Average Current");
                    this.chkLBParameterDisplay.SetItemChecked(0, true);
                    lblSeries1.Text = "Average Current";

                    if(columnNames.Contains("AVGNEUTRALCURRENT"))
                    {
                        chkLBParameterDisplay.Items.Add("Average Neutral Current");
                        this.chkLBParameterDisplay.SetItemChecked(1, true);
                        lblSeries2.Text = "Neutral Current";
                    }
                    // Added to update the currentRecordCount if parameters is changed.
                    currentRecordCount = 0;
                    //Added to solve bug 87497. Current record should be parameters selection changed to start drawing graph from starting record.
                    CurrentRecord = 0;
                }
                else
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
                    // SB Code Change Start - 20171214 - Neutral Current in Load Survey Graph
                    chkLBParameterDisplay.Items.Add("Neutral Current");
                    this.chkLBParameterDisplay.SetItemChecked(3, true);
                    lblSeries4.Text = "Neutral Current";
                    // SB Code Change End - 20171214 - Neutral Current in Load Survey Graph

                    lblSeries4.Text = "Series 4";
                    // Added to update the currentRecordCount if parameters is changed.
                    currentRecordCount = 0;
                    //Added to solve bug 87497. Current record should be parameters selection changed to start drawing graph from starting record.
                    CurrentRecord = 0;
                }
            }
            else if (parameterName == "Demand")
            {

                chkLBParameterDisplay.Items.Add(CommonMethods.getDisplayHeaderText("Demand {0}W"));
                // chkLBParameterDisplay.Items.Add(string.Format("Demand {0}W", unitType));
                this.chkLBParameterDisplay.SetItemChecked(0, true);
                lblSeries1.Text = CommonMethods.getDisplayHeaderText("Demand {0}W");// "Demand kW";
                chkLBParameterDisplay.Items.Add(CommonMethods.getDisplayHeaderText("Demand {0}var Lag"));
                //chkLBParameterDisplay.Items.Add(string.Format("Demand {0}var Lag", unitType));
                if (ConfigInfo.ActiveMeterType == "1P-2W")
                {
                    this.chkLBParameterDisplay.SetItemChecked(1, false);
                }
                else
                {
                    this.chkLBParameterDisplay.SetItemChecked(1, true);
                }
                lblSeries2.Text = CommonMethods.getDisplayHeaderText("Demand {0}var Lag");//"Demand kvar Lag";
                chkLBParameterDisplay.Items.Add(CommonMethods.getDisplayHeaderText("Demand {0}var Lead"));
                //chkLBParameterDisplay.Items.Add(string.Format("Demand {0}var Lead", unitType));
                if (ConfigInfo.ActiveMeterType == "1P-2W")
                {
                    this.chkLBParameterDisplay.SetItemChecked(2, false);
                }
                else
                {
                    this.chkLBParameterDisplay.SetItemChecked(2, true);
                }
                lblSeries3.Text = CommonMethods.getDisplayHeaderText("Demand {0}var Lead"); //"Demand kvar Lead";
                chkLBParameterDisplay.Items.Add(CommonMethods.getDisplayHeaderText("Demand {0}VA"));
                // chkLBParameterDisplay.Items.Add(string.Format("Demand {0}VA", unitType));
                this.chkLBParameterDisplay.SetItemChecked(3, true);
                lblSeries4.Text = CommonMethods.getDisplayHeaderText("Demand {0}VA"); //"Demand kVA";
                // Added to update the currentRecordCount if parameters is changed.
                currentRecordCount = 0;
                //Added to solve bug 87497. Current record should be parameters selection changed to start drawing graph from starting record.
                CurrentRecord = 0;

            }
            // SB Code Change Start - 20180129 - Export Demand Display in Load Survey Graph
            else if (parameterName == "Demand (Export)")
            {

                chkLBParameterDisplay.Items.Add(CommonMethods.getDisplayHeaderText("Demand {0}W Export"));
                // chkLBParameterDisplay.Items.Add(string.Format("Demand {0}W", unitType));
                this.chkLBParameterDisplay.SetItemChecked(0, true);
                lblSeries1.Text = CommonMethods.getDisplayHeaderText("Demand {0}W Export");// "Demand kW";
                chkLBParameterDisplay.Items.Add(CommonMethods.getDisplayHeaderText("Demand {0}var Lag Export"));
                //chkLBParameterDisplay.Items.Add(string.Format("Demand {0}var Lag", unitType));
                this.chkLBParameterDisplay.SetItemChecked(1, true);
                lblSeries2.Text = CommonMethods.getDisplayHeaderText("Demand {0}var Lag Export");//"Demand kvar Lag";
                chkLBParameterDisplay.Items.Add(CommonMethods.getDisplayHeaderText("Demand {0}var Lead Export"));
                //chkLBParameterDisplay.Items.Add(string.Format("Demand {0}var Lead", unitType));
                this.chkLBParameterDisplay.SetItemChecked(2, true);
                lblSeries3.Text = CommonMethods.getDisplayHeaderText("Demand {0}var Lead Export"); //"Demand kvar Lead";
                chkLBParameterDisplay.Items.Add(CommonMethods.getDisplayHeaderText("Demand {0}VA Export"));
                // chkLBParameterDisplay.Items.Add(string.Format("Demand {0}VA", unitType));
                this.chkLBParameterDisplay.SetItemChecked(3, true);
                lblSeries4.Text = CommonMethods.getDisplayHeaderText("Demand {0}VA Export"); //"Demand kVA";
                // Added to update the currentRecordCount if parameters is changed.
                currentRecordCount = 0;
                //Added to solve bug 87497. Current record should be parameters selection changed to start drawing graph from starting record.
                CurrentRecord = 0;

            }
            // SB Code Change End - 20180129 - Export Demand Display in Load Survey Graph
            else if (parameterName == "Power Factor")
            {
                chkLBParameterDisplay.Items.Add("Power Factor");
                this.chkLBParameterDisplay.SetItemChecked(0, true);
                lblSeries1.Text = "Power Factor";
                // Added to update the currentRecordCount if parameters is changed.
                currentRecordCount = 0;
                //Added to solve bug 87497. Current record should be parameters selection changed to start drawing graph from starting record.
                CurrentRecord = 0;

            }
            else if (parameterName == "Frequency")
            {
                chkLBParameterDisplay.Items.Add("Frequency");
                this.chkLBParameterDisplay.SetItemChecked(0, true);
                lblSeries1.Text = "Frequency";
                currentRecordCount = 0;
                CurrentRecord = 0;
            }
            // SB Code Change Start - 20171201 - Export Energy Display in Load Survey Graph
            else if (parameterName == "Energy (Export)")
            {
                chkLBParameterDisplay.Items.Add(CommonMethods.getDisplayHeaderText("Energy {0}Wh Export"));
                this.chkLBParameterDisplay.SetItemChecked(0, true);
                lblSeries1.Text = CommonMethods.getDisplayHeaderText("Energy (E) {0}Wh"); //"Energy kWh";
                chkLBParameterDisplay.Items.Add(CommonMethods.getDisplayHeaderText("Energy {0}varh Lag Export"));
                this.chkLBParameterDisplay.SetItemChecked(1, true);

                lblSeries2.Text = CommonMethods.getDisplayHeaderText("Energy (E) {0}varh Lag");// "Energy kvarh Lag";
                chkLBParameterDisplay.Items.Add(CommonMethods.getDisplayHeaderText("Energy {0}varh Lead Export"));
                this.chkLBParameterDisplay.SetItemChecked(2, true);

                lblSeries3.Text = CommonMethods.getDisplayHeaderText("Energy {0}varh Lead Export");// "Energy kvarh Lead";
                chkLBParameterDisplay.Items.Add(CommonMethods.getDisplayHeaderText("Energy (E) {0}VAh Export"));
                this.chkLBParameterDisplay.SetItemChecked(3, true);
                lblSeries4.Text = CommonMethods.getDisplayHeaderText("Energy {0}VAh Export"); //"Energy kVAh";
                // Added to update the currentRecordCount if parameters is changed.
                currentRecordCount = 0;
                //Added to solve bug 87497. Current record should be parameters selection changed to start drawing graph from starting record.
                CurrentRecord = 0;
            }
            // SB Code Change End - 20171201 - Export Energy Display in Load Survey Graph
            else
            {
                chkLBParameterDisplay.Items.Add(CommonMethods.getDisplayHeaderText("Energy {0}Wh"));
                //chkLBParameterDisplay.Items.Add(string.Format("Energy {0}Wh", unitType));
                this.chkLBParameterDisplay.SetItemChecked(0, true);
                lblSeries1.Text = CommonMethods.getDisplayHeaderText("Energy {0}Wh"); //"Energy kWh";
                chkLBParameterDisplay.Items.Add(CommonMethods.getDisplayHeaderText("Energy {0}varh Lag"));
                //chkLBParameterDisplay.Items.Add(string.Format("Energy {0}varh Lag", unitType));
                if (ConfigInfo.ActiveMeterType == "1P-2W")
                {
                    this.chkLBParameterDisplay.SetItemChecked(1, false);
                }
                else
                {
                    this.chkLBParameterDisplay.SetItemChecked(1, true);
                }
                lblSeries2.Text = CommonMethods.getDisplayHeaderText("Energy {0}varh Lag");// "Energy kvarh Lag";
                chkLBParameterDisplay.Items.Add(CommonMethods.getDisplayHeaderText("Energy {0}varh Lead"));
                // chkLBParameterDisplay.Items.Add(string.Format("Energy {0}varh Lead", unitType));
                if (ConfigInfo.ActiveMeterType == "1P-2W")
                {
                    this.chkLBParameterDisplay.SetItemChecked(2, false);
                }
                else
                {
                    this.chkLBParameterDisplay.SetItemChecked(2, true);
                }
                lblSeries3.Text = CommonMethods.getDisplayHeaderText("Energy {0}varh Lead");// "Energy kvarh Lead";
                chkLBParameterDisplay.Items.Add(CommonMethods.getDisplayHeaderText("Energy {0}VAh"));
                //chkLBParameterDisplay.Items.Add(string.Format("Energy {0}VAh", unitType));
                this.chkLBParameterDisplay.SetItemChecked(3, true);
                lblSeries4.Text = CommonMethods.getDisplayHeaderText("Energy {0}VAh"); //"Energy kVAh";
                // Added to update the currentRecordCount if parameters is changed.
                currentRecordCount = 0;
                //Added to solve bug 87497. Current record should be parameters selection changed to start drawing graph from starting record.
                CurrentRecord = 0;
            }
            LoadParameter();
        }
        private void graphChart_MouseMove(object sender, MouseEventArgs e)
        {
            string resolution = GetResolution();
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
                            txtSeries1.Text = point.YValues[0].ToString(resolution);
                        }
                        else if (i == 1)
                        {
                            lblSeries2.Text = graphChart.Series[i].Name;
                            txtSeries2.Text = point.YValues[0].ToString(resolution);
                        }
                        else if (i == 2)
                        {
                            lblSeries3.Text = graphChart.Series[i].Name;
                            txtSeries3.Text = point.YValues[0].ToString(resolution);
                        }
                        else if (i == 3)
                        {
                            lblSeries4.Text = graphChart.Series[i].Name;
                            txtSeries4.Text = point.YValues[0].ToString(resolution);
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
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show(this, ex.Message, "Chart Control for .NET Framework", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                logger.Log(LOGLEVELS.Error, "toolStripButton3_Click(object sender, EventArgs e)", ex);
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
            Font fontTitle = new Font("Arial", 12);
            MeterDataEntity meterDataEntity = new MeterDataBLL().GetDetailData(MeterDataId) as MeterDataEntity;
            string chartTitle = "Meter ID : " + meterDataEntity.MeterID;
            SizeF titleSize = ev.Graphics.MeasureString(chartTitle, fontTitle);
            titlePosition.Height = (int)titleSize.Height;

            // Draw charts title
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Near;
            ev.Graphics.DrawString(chartTitle, fontTitle, Brushes.Black, titlePosition, format);


            Rectangle titlePosition1 = new Rectangle(100, 1000, ev.MarginBounds.Width, ev.MarginBounds.Height);
            chartTitle = "Cabcon Ltd.";
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
            //Added to solve bug 71617.
            string chartDate = String.Concat(DateTime.Now.Day.ToString("00"), "/", DateTime.Now.Month.ToString("00"), "/", DateTime.Now.Year.ToString("0000"), " ", DateTime.Now.Hour.ToString("00"), ":", DateTime.Now.Minute.ToString("00"));
            chartTitle = DATETITLE1 + chartDate;
            //chartTitle = DateTime.Now.ToString();
            titleSize = ev.Graphics.MeasureString(chartTitle, fontTitle);
            titlePosition.Height = (int)titleSize.Height;

            // Draw charts title

            format.Alignment = StringAlignment.Far;
            ev.Graphics.DrawString(chartTitle, fontTitle, Brushes.Black, titlePosition3, format);

            //added to solve bug 71617.
            Rectangle titlePosition4 = new Rectangle(0, 1000, ev.MarginBounds.Width, ev.MarginBounds.Height);
            chartTitle = DATETITLE2;
            titleSize = ev.Graphics.MeasureString(chartTitle, fontTitle);
            titlePosition.Height = (int)titleSize.Height;
            format.Alignment = StringAlignment.Far;
            ev.Graphics.DrawString(chartTitle, fontTitle, Brushes.Black, titlePosition4, format);
            //added to solve bug 71617.

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
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show(this, ex.Message, "Chart Control for .NET Framework", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                logger.Log(LOGLEVELS.Error, "tsPrint_Click(object sender, EventArgs e)", ex);
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


        /// <summary>
        /// This function is used to calculate display date time .
        /// </summary>
        /// <param name="point"></param>
        private void DisplayDatetime(DataPoint point)
        {
            string dateTime = tslabelDate.Text;
            if (point.AxisLabel != ZEROHOUR)
            {
                txtParamTime.Text = Convert.ToDateTime(dateTime, dateTimeFormatInfo).ToString(dateTimeFormatInfo.ShortDatePattern) + " " + point.AxisLabel;
            }
            else
            {
                DateTime tempdateTime = Convert.ToDateTime(dateTime, dateTimeFormatInfo).AddDays(1);
                txtParamTime.Text = Convert.ToDateTime(tempdateTime, dateTimeFormatInfo).ToString(dateTimeFormatInfo.ShortDatePattern) + " " + point.AxisLabel;
            }
        }
        /// <summary>
        /// VBM - Get Resolution for Demand/Energy and Voltage/Current
        /// </summary>
        /// <returns></returns>
        private string GetResolution()
        {
            string resolution = string.Empty;
            if (cboParameters.SelectedItem.ToString() == "Demand" || cboParameters.SelectedItem.ToString() == "Demand (Export)" || cboParameters.SelectedItem.ToString() == "Energy" || cboParameters.SelectedItem.ToString() == "Energy (Export)") // SB Code Change Start/End - Export Energy Display in Load Survey Graph - Added "Energy (Export)" condition to this line. Export Demand Display in Load Survey Graph - Added "Demand (Export)" condition to this line. 
                resolution = "0.000";
            else if (cboParameters.SelectedItem.ToString() == "Voltage" || cboParameters.SelectedItem.ToString() == "Current")
                resolution = "0.00";
            else if (cboParameters.SelectedItem.ToString() == "Power Factor")
                resolution = "0.0000";
            return resolution;
        }

        private void DisplayInfo(Double Position)
        {
            string resolution = GetResolution();
            for (int i = 0; i < graphChart.Series.Count; i++)
            {
                DataPoint point = graphChart.Series[i].Points[Convert.ToInt32(Position)];
                if (i == 0)
                {
                    lblSeries1.Text = graphChart.Series[i].Name;
                    txtSeries1.Text = point.YValues[0].ToString(resolution);
                }
                else if (i == 1)
                {
                    lblSeries2.Text = graphChart.Series[i].Name;
                    txtSeries2.Text = point.YValues[0].ToString(resolution);
                }
                else if (i == 2)
                {
                    lblSeries3.Text = graphChart.Series[i].Name;
                    txtSeries3.Text = point.YValues[0].ToString(resolution);
                }
                else if (i == 3)
                {
                    lblSeries4.Text = graphChart.Series[i].Name;
                    txtSeries4.Text = point.YValues[0].ToString(resolution);
                }
                if (GraphView == 0)
                {
                    DisplayDatetime(point);

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
                double x;
                bool fwd = false;
                if (e.KeyValue == 37)
                {
                    if (graphChart.ChartAreas[0].CursorX.Position == 1 || graphChart.ChartAreas[0].CursorX.Position == 0)
                    {
                        //Previous page
                        PreviousPage();
                        graphChart.ChartAreas[0].CursorX.Position = MaxValue;
                        x = graphChart.ChartAreas[0].CursorX.Position - 1;
                        DisplayInfo(x);
                        return;
                    }
                    x = graphChart.ChartAreas[0].CursorX.Position - 1;
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
                    x = graphChart.ChartAreas[0].CursorX.Position;


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
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show(this, "Start / end of data reached.", "Cabcon", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                logger.Log(LOGLEVELS.Error, "graphChart_KeyUp(object sender, KeyEventArgs e)", ex);
            }

        }
        // Added to solve bug 83429. Parameter values should be updated on mouse click.

        private void graphChart_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    bool fwd = false;
                    if (graphChart.ChartAreas[0].CursorX.Position > 0)
                    {
                        double x = graphChart.ChartAreas[0].CursorX.Position;
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

                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show(this, "End of data reached.", "Cabcon", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                logger.Log(LOGLEVELS.Error, "graphChart_MouseUp(object sender, MouseEventArgs e)", ex);
            }
        }
    }
}