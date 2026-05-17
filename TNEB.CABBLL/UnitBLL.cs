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
using CAB.IECFramework;
using CAB.IECFramework.Entity;
using System;

namespace CAB.BLL
{
    public class UnitBLL : IBLL
    {
        UnitDAL unitDAL;

        public UnitBLL()
        {
            unitDAL = new UnitDAL();
        }

        public IEntity InsertData(IEntity entity)
        {
            return unitDAL.InsertData(entity);
        }

        public bool UpdateData(IEntity entity)
        {
            return unitDAL.UpdateData(entity);
        }

        public bool DeleteData(IEntity entity)
        {
            return unitDAL.DeleteData(entity);
        }
        public IEntity GetDetailData(string id)
        {
            if (id.Equals(string.Empty))
                return null;
            return GetDetailData(Convert.ToInt32(id));
        }

        public IEntity GetDetailData(int id)
        {
            return unitDAL.GetDetailData(id);
        }

        public DataSet ListDataSet()
        {
            DataSet dataSet = unitDAL.ListDataSet(); 
            if (dataSet.Tables.Count < 1)
                return new DataSet();
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("SL. No", typeof(System.Int32)));
            table.Columns.Add(new DataColumn("MeterUnit_ID", typeof(System.Int32)));
            table.Columns.Add(new DataColumn("Unit Name", typeof(System.String))); 
            int slno = 1;
            DataRow newRow;
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                newRow = table.NewRow();
                newRow["SL. No"] = slno;
                newRow["MeterUnit_ID"] = row["MeterUnit_ID"];
                newRow["Unit Name"] = row["MeterUnit_Type"]; 
                table.Rows.Add(newRow);
                slno++;
            }
            dataSet.Tables.Remove("table");
            dataSet.Tables.Add(table);
            return dataSet;
        }

        public IEntity ValidateUnit(string unitName)
        {
            return unitDAL.GetDetailData(unitName);
        }
    }
}



