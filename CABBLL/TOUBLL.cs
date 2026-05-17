using System.Collections.Generic;
using System.Data;
using CAB.DALC.Data;
using CAB.Framework;
using CAB.Framework.Entity;
/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											BLL calss for TOU data  																						|
 * |											Author : Gopal Krishna Gupya      									|
 * |											Date   : 06-Feb-2013			     								|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| */

namespace CAB.BLL
{
    public class TOUBLL:IBLL
    {
        Dictionary<string, string> touDataColumns = new Dictionary<string, string>();
        private TOUDAL objTOUDAL = null;
        public string fileName = string.Empty;
        public TOUBLL()
        {
            objTOUDAL = new TOUDAL();
        }
        public IEntity InsertData(IEntity entity)
        {
            return objTOUDAL.InsertData(entity);
        }
        public IEntity InsertData(IList<IEntity> entities)
        {
            return objTOUDAL.InsertData(entities);
        }
        public IEntity GetDetailData(int meterDataId)
        {
            return objTOUDAL.GetDetailData(meterDataId);
        }

        public DataSet DetailData(long meterDataID, bool isAnalysis)
        {
            return AddColumns(objTOUDAL.DetailData(meterDataID), isAnalysis);            
        }

        /// <summary>
        /// Add two additional columns in tou table 
        /// </summary>
        /// <param name="inputDataSet"></param>
        /// <returns></returns>
        private DataSet AddColumns(DataSet inputDataSet, bool isAnalysis)
        {
            if (inputDataSet != null && inputDataSet.Tables.Count > 0)
            {
                DataColumn serialNumber = new DataColumn("S. No.",typeof(string));
                DataColumn slotNumber = new DataColumn("Slot No.",typeof(string));
                DataColumn endTime = new DataColumn("Zone End Time(HH:MM)", typeof(string));
                inputDataSet.Tables[0].Columns.Add(serialNumber);
                inputDataSet.Tables[0].Columns.Add(slotNumber);
                inputDataSet.Tables[0].Columns.Add(endTime);
                if (isAnalysis && inputDataSet.Tables[0].Columns.Contains("Season Number"))
                {
                    inputDataSet.Tables[0].Columns.Remove("Season Number");
                }
                serialNumber.SetOrdinal(0);
                slotNumber.SetOrdinal(1);
                endTime.SetOrdinal(3);
                for (int index = 0; index < inputDataSet.Tables[0].Rows.Count; index++)
                {
                    inputDataSet.Tables[0].Rows[index]["S. No."] = index + 1;
                    inputDataSet.Tables[0].Rows[index]["Slot No."] = "Slot No. " + (index + 1);
                    if (index == inputDataSet.Tables[0].Rows.Count - 1)
                    {
                        inputDataSet.Tables[0].Rows[index]["Zone End Time(HH:MM)"] = inputDataSet.Tables[0].Rows[0]["Zone Start Time(HH:MM)"];
                    }
                    else
                    {
                        inputDataSet.Tables[0].Rows[index]["Zone End Time(HH:MM)"] = inputDataSet.Tables[0].Rows[index+1]["Zone Start Time(HH:MM)"];
                    }
                }
            }
            return inputDataSet;
            
        }

        private List<string> GetDatabaseColumns(List<string> columnList)
        {
            CreateTOUDictionary();
            List<string> columns = new List<string>();
            string tempStr = string.Empty;
            foreach (string key in columnList)
            {
                if (touDataColumns.TryGetValue(key, out tempStr))
                    columns.Add(tempStr);
            }
            return columns;
        }

        public Dictionary<string, string> CreateTOUDictionary()
        {
            touDataColumns.Add("START HOUR", "StartHour");
            touDataColumns.Add("START MINUTE", "StartMin");
            touDataColumns.Add("TARIFF", "Tariff");
            return touDataColumns;
        }
      

    }
}
