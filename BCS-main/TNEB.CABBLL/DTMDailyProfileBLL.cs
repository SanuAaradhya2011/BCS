using System;
using System.Collections.Generic; 
using System.Text;
using CAB.DALC.Data;
using CAB.IECFramework.Entity;
using CAB.IECFramework;
using System.Data;
using CAB.Entity;
using LTCTBLL;

namespace CAB.BLL
{
    public class DTMDailyProfileBLL :IBLL
    {
        DTMDailyProfileDAL dTMDailyProfileDAL;
        private Dictionary<string, string> dtmDailyReportColumns = new Dictionary<string, string>();
        private Dictionary<string, string> dtmDailyDatabaseColumns = new Dictionary<string, string>();

        public DTMDailyProfileBLL()
        {
            dTMDailyProfileDAL = new DTMDailyProfileDAL();
            InitializeDailyReportColumns();
            InitializeDailyDatabaseColumns();
        }

        public bool DeleteData(long meterDataId)
        {
            return dTMDailyProfileDAL.DeleteData(meterDataId);
        }

        private void InitializeDailyReportColumns()
        {
            dtmDailyReportColumns.Add("Cumulative kWh", "Cumulative kWh");
            dtmDailyReportColumns.Add("Cumulative kVArh Lag", "Cumulative kVArh Lag");
            dtmDailyReportColumns.Add("Cumulative kVArh Lead", "Cumulative kVArh Lead");
            dtmDailyReportColumns.Add("Cumulative kVAh", "Cumulative kVAh");
            dtmDailyReportColumns.Add("Daily MD1 (kW)", "Daily MD1 (kW)");
            dtmDailyReportColumns.Add("MD1 TimeStamp", "MD1 TimeStamp");
            dtmDailyReportColumns.Add("Daily MD2 (kVA)", "Daily MD2 (kVA)");
            dtmDailyReportColumns.Add("MD2 TimeStamp", "MD2 TimeStamp");
            dtmDailyReportColumns.Add("Daily MD3", "Daily MD3");
            dtmDailyReportColumns.Add("MD3 TimeStamp", "MD3 TimeStamp");
            dtmDailyReportColumns.Add("Max Avg Voltage", "Max Avg Voltage");
            dtmDailyReportColumns.Add("Min Avg Voltage", "Min Avg Voltage");
            dtmDailyReportColumns.Add("Max Avg Current", "Max Avg Current");
            dtmDailyReportColumns.Add("Min Avg Current", "Min Avg Current");
            dtmDailyReportColumns.Add("Cumulative Fundamental kWh", "Cumulative Fundamental kWh");
            if (UtilityDetails.ShowPowerOnHours)
            {
                dtmDailyReportColumns.Add("Power On Hours", "Power On Hours (HH)");
            }
        }

        private void InitializeDailyDatabaseColumns()
        {
            dtmDailyDatabaseColumns.Add("Cumulative kWh", "CumulativekWh");
            dtmDailyDatabaseColumns.Add("Cumulative kVArh Lag", "CumulativekVArh_lag");
            dtmDailyDatabaseColumns.Add("Cumulative kVArh Lead", "CumulativekVArh_lead");
            dtmDailyDatabaseColumns.Add("Cumulative kVAh", "CumulativekVAh");
            dtmDailyDatabaseColumns.Add("Daily MD1 (kW)", "DailyMD1");
            dtmDailyDatabaseColumns.Add("MD1 TimeStamp", "MD1TimeStamp");
            dtmDailyDatabaseColumns.Add("Daily MD2 (kVA)", "DailyMD2");
            dtmDailyDatabaseColumns.Add("MD2 TimeStamp", "MD2TimeStamp");
            dtmDailyDatabaseColumns.Add("Daily MD3", "DailyMD3");
            dtmDailyDatabaseColumns.Add("MD3 TimeStamp", "MD3TimeStamp");
            dtmDailyDatabaseColumns.Add("Max Avg Voltage", "MaxAvgVoltage");
            dtmDailyDatabaseColumns.Add("Min Avg Voltage", "MinAvgVoltage");
            dtmDailyDatabaseColumns.Add("Max Avg Current", "MaxAvgCurrent");
            dtmDailyDatabaseColumns.Add("Min Avg Current", "MinAvgCurrent");
            dtmDailyDatabaseColumns.Add("Cumulative Fundamental kWh", "CumulativeFundamentalKWh");
            if (UtilityDetails.ShowPowerOnHours)
            {
                dtmDailyDatabaseColumns.Add("Power On Hours", "PowerOnHours");
            }
            //DailyProfileDate, CumulativeFundamentalKwh, AvailableDays, MaximumDays, MeterData_ID
        }

        public IEntity InsertData(IEntity entity)
        {
            return dTMDailyProfileDAL.InsertData(entity);
        }
        public IEntity InsertData(List<IEntity> entities)
        {
            return dTMDailyProfileDAL.InsertData(entities);
        }
        public DataSet ListData(long meterData_ID)
        {
            IECDTMDailyProfileParameterEntity entity = new DTMDailyProfileParameterBLL().GetColumn(meterData_ID) as IECDTMDailyProfileParameterEntity;
            if (entity == null)
                return null;
            else if (string.IsNullOrEmpty(entity.ColumnsNames))
                return null;
            else
                return dTMDailyProfileDAL.ListData(entity);
        }
        public DataSet GetDailyProfileByParameter(long activeMeterData_ID, List<string> columnList)
        {
            List<string> columns = new List<string>();
            string tempStr= string.Empty;
            foreach (string column in columnList)
            {
                if (dtmDailyDatabaseColumns.TryGetValue(column,out tempStr))
                    columns.Add(tempStr);
            }
            return dTMDailyProfileDAL.GetDailyProfileByParameter(activeMeterData_ID, columns);
        }

        public string GetReportColumnName(string key)
        {
            string tempStr = string.Empty;
            dtmDailyReportColumns.TryGetValue(key, out tempStr);
            return tempStr;
        }
    }
}
