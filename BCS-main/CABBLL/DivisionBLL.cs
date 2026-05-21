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
    public class DivisionBLL : IBLL
    {
        DivisionDAL divisionDAL;

        public DivisionBLL()
        {
            divisionDAL = new DivisionDAL();
        }

        public IEntity InsertData(IEntity entity)
        {
            return divisionDAL.InsertData(entity);
        }

        public bool UpdateData(IEntity entity)
        {
            return divisionDAL.UpdateData(entity);
        }

        public bool DeleteData(IEntity entity)
        {
            return divisionDAL.DeleteData(entity);
        }
        public IEntity GetDetailData(string id)
        {
            if (id.Equals(string.Empty))
                return null;
            return GetDetailData(Convert.ToInt32(id));
        }
        // Added by swati
        public IEntity GetDetailDataConsumer(string id)
        {
            if (id.Equals(string.Empty))
                return null;
            return GetDetailDataConsumer(Convert.ToInt32(id));
        }


        public IEntity GetDetailData(int id)
        {
            return divisionDAL.GetDetailData(id);
        }
        // Added by Swati
        public IEntity GetDetailDataConsumer(int id)
        {
            return divisionDAL.GetDetailDataConsumer(id);
        }
        public DataSet ListDataSet()
        {
            DataSet dataSet = divisionDAL.ListDataSet(); 
            if (dataSet.Tables.Count < 1)
                return new DataSet();
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("SL. No", typeof(System.Int32)));
            table.Columns.Add(new DataColumn("Division_ID", typeof(System.Int32)));
            table.Columns.Add(new DataColumn("Division Name", typeof(System.String)));
            table.Columns.Add(new DataColumn("Region Name", typeof(System.String)));
            table.Columns.Add(new DataColumn("Circle Name", typeof(System.String)));
            int slno = 1;
            DataRow newRow;
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                newRow = table.NewRow();
                newRow["SL. No"] = slno;
                newRow["Division_ID"] = row["Division_ID"];
                newRow["Division Name"] = row["Division_Name"];
                newRow["Region Name"] = row["Region_Name"];
                newRow["Circle Name"] = row["Circle_Name"];
                table.Rows.Add(newRow);
                slno++;
            }
            dataSet.Tables.Remove("table");
            dataSet.Tables.Add(table);
            return dataSet;
        }
        public DataSet ListDataSetDivision(int regionid,int circleid)
        {
            DataSet dataSet = divisionDAL.ListDataSetDivision(regionid,circleid);
            if (dataSet.Tables.Count < 1)
                return new DataSet();
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("SL. No", typeof(System.Int32)));
            table.Columns.Add(new DataColumn("Division_ID", typeof(System.Int32)));
            table.Columns.Add(new DataColumn("Division Name", typeof(System.String)));
            table.Columns.Add(new DataColumn("Region Name", typeof(System.String)));
            table.Columns.Add(new DataColumn("Circle Name", typeof(System.String)));
            int slno = 1;
            DataRow newRow;
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                newRow = table.NewRow();
                newRow["SL. No"] = slno;
                newRow["Division_ID"] = row["Division_ID"];
                newRow["Division Name"] = row["Division_Name"];
                newRow["Region Name"] = row["Region_Name"];
                newRow["Circle Name"] = row["Circle_Name"];
                table.Rows.Add(newRow);
                slno++;
            }
            dataSet.Tables.Remove("table");
            dataSet.Tables.Add(table);
            return dataSet;
        }
        public DataSet GetDivisionDataByCircleID(int circleID)
        {
            return divisionDAL.GetDivisionDataByCircleID(circleID);
        }
        public IEntity ValidateDivision(string divisionName)
        {
            return divisionDAL.GetDetailData(divisionName);
        }
        //20th April 2012
        public bool ValidateDivision(IEntity entity)
        {
            return divisionDAL.ValidateDivision(entity);
        }
        /// <summary>
        /// gets all the data from the Database of division.
        /// </summary>
        /// <returns></returns>
        public DataSet GetDivisionData()
        {
            return divisionDAL.GetDivisionData();
        }
    }
}


