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
    public class CircleBLL : IBLL
    {
        CircleDAL circleDAL;

        public CircleBLL()
        {
            circleDAL = new CircleDAL();
        }

        public IEntity InsertData(IEntity entity)
        {
            return circleDAL.InsertData(entity);
        }

        public bool UpdateData(IEntity entity)
        {
            return circleDAL.UpdateData(entity);
        }

        public bool DeleteData(IEntity entity)
        {
            return circleDAL.DeleteData(entity);
        }

        public IEntity GetDetailData(string id)
        {
            if (id.Equals(string.Empty))
                return null;
            return GetDetailData(Convert.ToInt32(id));
        }
        #region Check division existence
        public IEntity GetDetailDataDivision(string id)
        {
            if (id.Equals(string.Empty))
                return null;
            return GetDetailDataByDivision(Convert.ToInt32(id));
        }
        public IEntity GetDetailDataByDivision(int id)
        {
            return circleDAL.GetDetailDataByDivision(id);

        }
        #endregion
        //Added by Swati
        public IEntity GetDetailDataConsumer(string id)
        {
            if (id.Equals(string.Empty))
                return null;
            return GetDetailDataConsumer(Convert.ToInt32(id));
        }
        public DataSet GetCircleDetailData(int regionID)
        {
            return circleDAL.GetCircleDetailData(regionID);
        }
        public IEntity GetDetailData(int id)
        { 
            return circleDAL.GetDetailData(Convert.ToInt32(id));
        }
        //Added by Swati
        public IEntity GetDetailDataConsumer(int id)
        {
            return circleDAL.GetDetailDataConsumer(Convert.ToInt32(id));
        }
        public DataSet ListDataSet()
        {
            DataSet dataSet = circleDAL.ListDataSet();
            if (dataSet.Tables.Count < 1)
                return new DataSet();
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("SL. No", typeof(System.Int32)));
            table.Columns.Add(new DataColumn("Circle_ID", typeof(System.Int32)));
            table.Columns.Add(new DataColumn("Circle Name", typeof(System.String)));
            table.Columns.Add(new DataColumn("Region Name", typeof(System.String)));
            int slno = 1;
            DataRow newRow;
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                newRow = table.NewRow();
                newRow["SL. No"] = slno;
                newRow["Circle_ID"] = row["Circle_ID"];
                newRow["Circle Name"] = row["Circle_Name"];
                newRow["Region Name"] = row["Region_Name"];
                table.Rows.Add(newRow);
                slno++;
            }
            dataSet.Tables.Remove("table");
            dataSet.Tables.Add(table);
            return dataSet;
        }
        // Added by swati
        public DataSet ListDataSetCircle(int regionid)
        {
            DataSet dataSet = circleDAL.ListDataSetCircle(regionid);
            if (dataSet.Tables.Count < 1)
                return new DataSet();
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("SL. No", typeof(System.Int32)));
            table.Columns.Add(new DataColumn("Circle_ID", typeof(System.Int32)));
            table.Columns.Add(new DataColumn("Circle Name", typeof(System.String)));
            table.Columns.Add(new DataColumn("Region Name", typeof(System.String)));
            int slno = 1;
            DataRow newRow;
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                newRow = table.NewRow();
                newRow["SL. No"] = slno;
                newRow["Circle_ID"] = row["Circle_ID"];
                newRow["Circle Name"] = row["Circle_Name"];
                newRow["Region Name"] = row["Region_Name"];
                table.Rows.Add(newRow);
                slno++;
            }
            dataSet.Tables.Remove("table");
            dataSet.Tables.Add(table);
            return dataSet;
        }

        public IEntity ValidateCircle(string circleName)
        {
            return circleDAL.GetDetailData(circleName);
        }
        //20th April 2012
        public bool ValidateCircle(IEntity entity)
        {
            return circleDAL.ValidateCircle(entity);
        }
        /// <summary>
        /// gets all the data from the Database of circle. 
        /// </summary>
        /// <returns></returns>
         public DataSet GetCircleData()
        {
            return circleDAL.GetCircleData();
        }

    }
}



