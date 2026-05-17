/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 	 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| 
 */
using System;
using System.Collections.Generic;
using CAB.DALC.Data;
using CAB.Entity;
using System.Text;
using CAB.Framework;
using CAB.Framework.Entity;
using System.Data;

namespace CAB.BLL
{
    public class DesignationMasterBLL : IBLL
    {
        DesignationMasterDAL designationMasterDAL;

        public DesignationMasterBLL()
        {
            designationMasterDAL = new DesignationMasterDAL();
        }

        public IEntity InsertData(IEntity entity)
        {
            return designationMasterDAL.InsertData(entity);
        }

        public bool UpdateData(IEntity entity)
        {
            return designationMasterDAL.UpdateData(entity);
        }

        public bool DeleteData(IEntity entity)
        {
            return designationMasterDAL.DeleteData(entity);
        }
        public IEntity GetDetailData(string id)
        {
            if (id.Equals(string.Empty))
                return null;
            return GetDetailData(Convert.ToInt32(id));
        }

        public IEntity GetDetailData(int id)
        {
            return designationMasterDAL.GetDetailData(id);
        }

        public DataSet ListDataSet()
        {
            DataSet dataSet = designationMasterDAL.ListDataSet();
            if (dataSet.Tables.Count < 1)
                return new DataSet();
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("SL. No", typeof(System.Int32)));
            table.Columns.Add(new DataColumn("Designation_ID", typeof(System.Int32)));
            table.Columns.Add(new DataColumn("Designation Name", typeof(System.String)));
            int slno = 1;
            DataRow newRow;
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                newRow = table.NewRow();
                newRow["SL. No"] = slno;
                newRow["Designation_ID"] = row["Designation_ID"];
                newRow["Designation Name"] = row["Designation_Name"];
                table.Rows.Add(newRow);
                slno++;
            }
            dataSet.Tables.Remove("table");
            dataSet.Tables.Add(table);
            return dataSet;
        }

        public IEntity ValidateDesignation(string designation)
        {
            return designationMasterDAL.GetDetailData(designation);
        }
    }
}