/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 									|
 * |											           				            								|
 * | 																    											|
 * |----------------------------------------------------------------------------------------------------------------| */
using CAB.DALC.Data;
using CAB.IECFramework;
using CAB.IECFramework.Entity;
using System.Data;
using System;
using System.Collections.Generic;
using LTCTBLL;
using CAB.Entity;
namespace CAB.BLL
{
    public class GeneralBLL : IBLL
    {
        GeneralDAL generalDAL;
        Dictionary<string, string> generalDataColumns = new Dictionary<string, string>();
        public GeneralBLL()
        {
            generalDAL = new GeneralDAL(UtilityDetails.UtilityName);
        }

        public bool DeleteData(long meterDataId)
        {
            return generalDAL.DeleteData(meterDataId);
        }

        public IEntity InsertData(IEntity entity)
        {
            return generalDAL.InsertData(entity);
        }

        public DataSet GetMeterData(int MeterDataId)
        {
            DataSet ds = generalDAL.GetMeterData(MeterDataId);
            return CommonBLL.ConvertRowToColumn(ds);
        }

        public Dictionary<string, string> CreateGeneralDictionary()
        {
            generalDataColumns.Add("Meter DateTime", "g.MeterDateTime");
            generalDataColumns.Add("Firmware Version", "g.FirmwareVersion");
            generalDataColumns.Add("Meter Constant", "g.MeterConstant");
            generalDataColumns.Add("Error Code", "g.ErrorCode");
            generalDataColumns.Add("Cumulative Fundamental Active Energy", "I.TotalFundamentalActiveEnergy");
            generalDataColumns.Add("Cumulative Active Energy", "g.TotalActiveEnergy");
            generalDataColumns.Add("Cumulative Apparent Energy", "b.CumulativeEnergyKVAH");
            generalDataColumns.Add("Cumulative Reactive Energy (Lag)", "b.CumulativeEnergyKVARHLag");
            generalDataColumns.Add("Cumulative Reactive Energy (Lead)", "b.CumulativeEnergyKVARHLead");
            generalDataColumns.Add("Current Month MD1 (kW)", "b.CumulativeMD1");
            generalDataColumns.Add("Current Month MD1 Time Stamp", "b.CumulativeMD1TimeStamp");
            generalDataColumns.Add("Current Month MD2 (kVA)", "b.CumulativeMD2");
            generalDataColumns.Add("Current Month MD2 Time Stamp", "b.CumulativeMD2TimeStamp");
            generalDataColumns.Add("Cumulative MD1 (kW)", "g.CumulativeMD1");
            generalDataColumns.Add("Cumulative MD2 (kVA)", "g.CumulativeMD2");
            generalDataColumns.Add("MD Reset Counter", "g.MDResetCounter");
            generalDataColumns.Add("Readout Counter", "g.ReadoutCounter");
            generalDataColumns.Add("Prog. Counter", "g.ProgrammingCounter");
            generalDataColumns.Add("Total Power On Hours (HH:MM)", "g.TotalPowerOnHours");
            if (UtilityDetails.UtilityName == IECUtilityEntity.TNEB || UtilityDetails.UtilityName == IECUtilityEntity.TNEB1)
            {
                generalDataColumns.Add("Power Off Days", "g.PowerOffDays");
            }
            generalDataColumns.Add("Battery Mode Power On Hour", "g.BateryModePowerOnHour");
            generalDataColumns.Add("Voltage Phase Sequence", "g.VoltagePhaseSequence");
            generalDataColumns.Add("Latest Tamper Occurrence", "g.LatestTamperOccurrenceID");
            generalDataColumns.Add("Occurrence Time", "g.OccurrenceTime");
            generalDataColumns.Add("Latest Tamper Restoration", "g.LatestTamperRestorationID");
            generalDataColumns.Add("Restoration Time", "g.RestorationTime");
            if (UtilityDetails.UtilityName == IECUtilityEntity.TNEB || UtilityDetails.UtilityName == IECUtilityEntity.TNEB1)
            {
                generalDataColumns.Add("Billing Reset Type", "b.BillingResetType");
            }
            //If utility is WBEXPORTVCL. then show additional parameter in selection list
            if (UtilityDetails.UtilityName == IECUtilityEntity.WBEXPORTVCL)
            {
                generalDataColumns.Add("Cumulative Active Energy (E)", "g.CumulativeExportEnergyKWH");
                generalDataColumns.Add("Cumulative Apparent Energy (E)", "g.CumulativeExportEnergyKVAH");
            }
            
            //generalDataColumns.Add("CT Ratio", "CTRatio");
            //generalDataColumns.Add("Current Phase Sequence", "CurrentPhaseSequence");
            //generalDataColumns.Add("Total Fund. Active Energy", "TotalActiveEnergy");
            //generalDataColumns.Add("Cumulative MD3", "CumulativeMD3");
            //generalDataColumns.Add("Demand Rising kW", "RisingDemandKW");
            //generalDataColumns.Add("Elasped Time kW", "ElapsedTimeKW");
            //generalDataColumns.Add("Demand Rising kVA", "RisingDemandKVA");
            //generalDataColumns.Add("Elasped Time kVA", "ElapsedTimeKVA");
            //generalDataColumns.Add("Current Month Power On Hours", "CurrentMonthPowerOnHours");
            //generalDataColumns.Add("CT Ratio Prog. Counters", "CTRatioProgrammingCounter");
            return generalDataColumns;
        }

        public DataSet GetGeneralDataByParameter(string value, List<string> columnList, string reportType)
        {
            DataSet ds = new DataSet();
            List<string> columns = GetDatabaseColumns(columnList);
            if (reportType == "CAB")
                ds = generalDAL.GetGeneralDataByFileName(value, columns);
            else if (reportType == "Meter")
                ds = generalDAL.GetGeneralDataByMeter(value, columns);

            return ds;
        }

        private List<string> GetDatabaseColumns(List<string> columnList)
        {
            CreateGeneralDictionary();
            List<string> columns = new List<string>();
            string tempStr = string.Empty;
            foreach (string key in columnList)
            {
                if (generalDataColumns.TryGetValue(key, out tempStr))
                    columns.Add(tempStr);
            }
            return columns;
        }

        public string GetReportColumnName(string key)
        {
            string tempStr = string.Empty;
            generalDataColumns.TryGetValue(key, out tempStr);
            return tempStr;
        }
    }
}

