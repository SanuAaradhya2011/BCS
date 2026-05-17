/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh.        									|
 * |											           				            								|
 * | 																    											|
 * |----------------------------------------------------------------------------------------------------------------| */
using CAB.DALC.Data;
using CAB.IECFramework;
using CAB.IECFramework.Entity;
using System.Data;
using System.Collections.Generic;

namespace CAB.BLL
{
    public class TamperSnapShotBLL : IBLL
    {
        TamperSnapShotDAL tamperSnapShotDAL;
        

        public TamperSnapShotBLL()
        {
            tamperSnapShotDAL = new TamperSnapShotDAL();
        }
        public bool DeleteData(long meterDataId)
        {
            return tamperSnapShotDAL.DeleteData(meterDataId);
        }
        public IEntity InsertData(IEntity entity)
        {
            return tamperSnapShotDAL.InsertData(entity);
        }
        public IEntity InsertData(List<IEntity> entities)
        {
            return tamperSnapShotDAL.InsertData(entities);
        }
        public DataSet ListData(long meterDataID)
        {
            return tamperSnapShotDAL.ListDataSet(meterDataID);
        }
        public DataSet ListData(long meterDataID,long tamperCode)
        {
            return tamperSnapShotDAL.ListDataSet(meterDataID, tamperCode);
        }
        public DataSet DetailData(long shnapshortID, long meterDataID)
        {
            return tamperSnapShotDAL.DetailData(shnapshortID, meterDataID);
        }

        public Dictionary<string,string> CreateSnapshotDictionary()
        {
            Dictionary<string, string> tamperSnapshotColumns = new Dictionary<string, string>();
            tamperSnapshotColumns.Add("Tamper Occurred Time", "TamperOccurredTime");
            tamperSnapshotColumns.Add("Tamper Restored Time", "TamperRestoredTime");
            tamperSnapshotColumns.Add("R Phase Voltage Occurred", "RVoltageOccurred");
            tamperSnapshotColumns.Add("Y Phase Voltage Occurred", "YVoltageOccurred");
            tamperSnapshotColumns.Add("B Phase Voltage Occurred", "BVoltageOccurred");
            tamperSnapshotColumns.Add("R Phase Current Occurred", "RCurrentOccurred");
            tamperSnapshotColumns.Add("Y Phase Current Occurred", "YCurrentOccurred");
            tamperSnapshotColumns.Add("B Phase Current Occurred", "BCurrentOccurred");
            tamperSnapshotColumns.Add("R Phase PF Occurred", "RPFOccurred");
            tamperSnapshotColumns.Add("Y Phase PF Occurred", "YPFOccurred");
            tamperSnapshotColumns.Add("B Phase PF Occurred", "BPFOccurred");
            tamperSnapshotColumns.Add("Total PF occurred", "TotalPFOccurred");
            tamperSnapshotColumns.Add("kWh Occurred", "kWhOccurred");
            tamperSnapshotColumns.Add("kVAh Occurred", "kVAhOccurred");
            tamperSnapshotColumns.Add("R Phase Voltage Restored", "RVoltageRestored");
            tamperSnapshotColumns.Add("Y Phase Voltage Restored", "YVoltageRestored");
            tamperSnapshotColumns.Add("B Phase Voltage Restored", "BVoltageRestored");
            tamperSnapshotColumns.Add("R Phase Current Restored", "RCurrentRestored");
            tamperSnapshotColumns.Add("Y Phase Current Restored", "YCurrentRestored");
            tamperSnapshotColumns.Add("B Phase Current Restored", "BCurrentRestored");
            tamperSnapshotColumns.Add("R Phase PF Restored", "RPFRestored");
            tamperSnapshotColumns.Add("Y Phase PF Restored", "YPFRestored");
            tamperSnapshotColumns.Add("B Phase PF Restored", "BPFRestored");
            tamperSnapshotColumns.Add("Total PF Restored", "TotalPFRestored");
            tamperSnapshotColumns.Add("kWh Restored", "kWhRestored");
            tamperSnapshotColumns.Add("kVAh Restored", "kVAhRestored");
            return tamperSnapshotColumns;
        }

        public Dictionary<string, string> CreatePowerOnOffDictionary()
        {
            Dictionary<string, string> tamperSnapshotOnOffColumns = new Dictionary<string, string>();
            tamperSnapshotOnOffColumns.Add("Tamper Occurred Time", "TamperOccurredTime");
            tamperSnapshotOnOffColumns.Add("Tamper Restored Time", "TamperRestoredTime");
            return tamperSnapshotOnOffColumns;
        }

        public DataSet GetTamperSnapshotDataByParameter(string value, List<string> columnList, int tamperCode, string reportType)
        {
            DataSet ds = new DataSet();
            List<string> columns = GetDatabaseColumns(columnList);
            if (reportType == "CAB")
                ds = tamperSnapShotDAL.GetTamperSnapshotDataByFile(value, columns, tamperCode);
            else if (reportType == "Meter")
                ds = tamperSnapShotDAL.GetTamperSnapshotDataByMeter(value, columns, tamperCode);

            return ds;
        }

        private List<string> GetDatabaseColumns(List<string> columnList)
        {
            Dictionary<string, string> snapshotColumns = CreateSnapshotDictionary();
            List<string> columns = new List<string>();
            string tempStr = string.Empty;
            foreach (string key in columnList)
            {
                if (snapshotColumns.TryGetValue(key, out tempStr))
                    columns.Add(tempStr);
            }
            return columns;
        }
    }
}