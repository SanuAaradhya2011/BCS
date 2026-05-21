/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |		                                All rights reserved to Cabcon Technologies 		                                |
 * | 	                                                                                                            |
 * |		                                Author : Piyush Singh.                                        |
 * |		           	            		                                                                        |
 * | 	    		                                                                                                |
 * |----------------------------------------------------------------------------------------------------------------| */
using CAB.DALC.Data;
using CAB.IECFramework;
using CAB.IECFramework.Entity;
using System.Data;
using System.Collections.Generic;
using LTCTBLL;
using CAB.Entity;
namespace CAB.BLL
{
    public class InstantPowerBLL : IBLL
    {
        InstantPowerDAL instantPowerDAL;
        Dictionary<string, string> instantColumns = new Dictionary<string, string>();

        public InstantPowerBLL()
        {
            instantPowerDAL = new InstantPowerDAL();
        }

        public Dictionary<string,string> CreateInstantDictionary()
        {
            instantColumns.Add("Voltage R Phase", "i.VoltageRPhase");
            instantColumns.Add("Voltage Y Phase", "i.VoltageYPhase");
            instantColumns.Add("Voltage B Phase", "i.VoltageBPhase");
            instantColumns.Add("Current R Phase", "i.CurrentRPhase");
            instantColumns.Add("Current Y Phase", "i.CurrentYPhase");
            instantColumns.Add("Current B Phase", "i.CurrentBPhase");
            instantColumns.Add("Active Power", "i.InstantActivepower");
            instantColumns.Add("Apparent Power", "i.InstantApparentPower");
            instantColumns.Add("Reactive Power (lag)", "i.InstantReactiveLagPower");
            instantColumns.Add("Reactive Power (lead)", "i.InstantReactiveLeadPower");
            instantColumns.Add("Total Power Factor", "i.TotalPowerFactor");
            instantColumns.Add("Power Factor R Phase", "i.PowerFactorRPhase");
            instantColumns.Add("Power Factor Y Phase", "i.PowerFactorYPhase");
            instantColumns.Add("Power Factor B Phase", "i.PowerFactorBPhase");
            instantColumns.Add("Frequency", "i.Frequency");
            if (UtilityDetails.UtilityName == IECUtilityEntity.TNEB)
            {
                instantColumns.Add("InstantActivepowerRPhase", "i.InstantActivepowerRPhase");
                instantColumns.Add("InstantActivepowerYPhase", "i.InstantActivepowerYPhase");
                instantColumns.Add("InstantActivepowerBPhase", "i.InstantActivepowerBPhase");
                instantColumns.Add("InstantReactivepowerRPhase", "i.InstantReactivepowerRPhase");
                instantColumns.Add("InstantReactivepowerYPhase", "i.InstantReactivepowerYPhase");
                instantColumns.Add("InstantReactivepowerBPhase", "i.InstantReactivepowerBPhase");
                instantColumns.Add("InstantApparentpowerRPhase", "i.InstantApparentpowerRPhase");
                instantColumns.Add("InstantApparentpowerYPhase", "i.InstantApparentpowerYPhase");
                instantColumns.Add("InstantApparentpowerBPhase", "i.InstantApparentpowerBNPhase");
            }
            instantColumns.Add("Rising Demand kW", "g.RisingDemandKW");
            instantColumns.Add("Elapsed Time kW", "g.ElapsedTimeKW");
            instantColumns.Add("Rising Demand kVA", "g.RisingDemandKVA");
            instantColumns.Add("ElapsedTime kVA", "g.ElapsedTimeKVA");
            //instantColumns.Add("Total Fundamental Active Energy", "TotalFundamentalActiveEnergy");
            return instantColumns;
        }

        public DataSet GetInstantDataByParameter(string value, List<string> columnList, string reportType)
        {
            DataSet ds = new DataSet();
            List<string> columns = GetDatabaseColumns(columnList);
            if (reportType == "CAB")
                ds = instantPowerDAL.GetInstantDataByFileName(value, columns);
            else if (reportType == "Meter")
                ds = instantPowerDAL.GetInstantDataByMeter(value, columns);

            return ds;
        }

        private List<string> GetDatabaseColumns(List<string> columnList)
        {
            CreateInstantDictionary();
            List<string> columns = new List<string>();
            string tempStr = string.Empty;
            foreach (string key in columnList)
            {
                if (instantColumns.TryGetValue(key, out tempStr))
                    columns.Add(tempStr);
            }
            return columns;
        }

        public string GetReportColumnName(string key)
        {
            string tempStr = string.Empty;
            instantColumns.TryGetValue(key, out tempStr);
            return tempStr;
        }

        public IEntity InsertData(IEntity entity)
        {
            return instantPowerDAL.InsertData(entity);
        }

        public DataSet GetMeterData(int MeterDataId)
        {
            DataSet ds = instantPowerDAL.GetMeterData(MeterDataId);
            return CommonBLL.ConvertRowToColumn(ds);
        }
        public bool DeleteData(long meterDataId)
        {
            return instantPowerDAL.DeleteData(meterDataId);
        }
    }
}
