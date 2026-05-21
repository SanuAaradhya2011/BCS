/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh										|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| 
 */

using System.Data;
using CAB.DALC.Data;
using CAB.Framework;
using CAB.Framework.Entity;
using System;

namespace CAB.BLL
{
    public class MeterTypeBLL : IBLL
    {
        MeterTypeMasterDAL meterTypeMasterDAL;

        public MeterTypeBLL()
        {
            meterTypeMasterDAL = new MeterTypeMasterDAL();
        }

        public IEntity InsertData(IEntity entity)
        {
            return meterTypeMasterDAL.InsertData(entity);
        }

        public bool UpdateData(IEntity entity)
        {
            return meterTypeMasterDAL.UpdateData(entity);
        }

        public bool DeleteData(IEntity entity)
        {
            return meterTypeMasterDAL.DeleteData(entity);
        }
        public IEntity GetDetailData(string id)
        {
            if (id.Equals(string.Empty))
                return null;
            return GetDetailData(Convert.ToInt32(id));
        }

        public IEntity GetDetailData(int id)
        {
            return meterTypeMasterDAL.GetDetailData(id);
        }

        public DataSet ListDataSet()
        {
            DataSet dataSet = meterTypeMasterDAL.ListDataSet();
            if (dataSet.Tables.Count < 1)
                return new DataSet();
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("SL. No", typeof(System.Int32)));
            table.Columns.Add(new DataColumn("MeterType_ID", typeof(System.Int32)));
            table.Columns.Add(new DataColumn("Meter Type", typeof(System.String)));
            int slno = 1;
            DataRow newRow;
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                newRow = table.NewRow();
                newRow["SL. No"] = slno;
                newRow["MeterType_ID"] = row["MeterType_ID"];
                newRow["Meter Type"] = row["MeterType_Name"];
                table.Rows.Add(newRow);
                slno++;
            }
            dataSet.Tables.Remove("table");
            dataSet.Tables.Add(table);
            return dataSet;
        }
         
        public IEntity ValidateType(string typeName)
        {
            return meterTypeMasterDAL.GetDetailData(typeName);
        }

        public void InsertDefaultData()
        {
            meterTypeMasterDAL.InsertDefaultMeterType();
        }
    }
}


