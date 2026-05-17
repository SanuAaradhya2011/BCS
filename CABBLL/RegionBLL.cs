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
    public class RegionBLL : IBLL
    {
        RegionDAL regionDAL;

        public RegionBLL()
        {
            regionDAL = new RegionDAL();
        }

        public IEntity InsertData(IEntity entity)
        {
            return regionDAL.InsertData(entity);
        }

        public bool UpdateData(IEntity entity)
        {
            return regionDAL.UpdateData(entity);
        }

        public bool DeleteData(IEntity entity)
        {
            return regionDAL.DeleteData(entity);
        }

        public IEntity GetDetailData(string id)
        {
            if (id.Equals(string.Empty))
                return null;
            return GetDetailData(Convert.ToInt32(id));
        }
        // Added by Swati
        public IEntity GetDetailDataConsumerMeter(string id)
        {
            if (id.Equals(string.Empty))
                return null;
            return GetDetailDataConsumerMeter(Convert.ToInt32(id));
        }
        #region Check circle existence
        // Added by Swati
        public IEntity GetDetailDataCircle(string  id)
        {
            if (id.Equals(string.Empty))
                return null;
            return GetDetailDataByCircle(Convert.ToInt32(id));
        }
        public IEntity GetDetailDataByCircle(int id)
        {
            return regionDAL.GetDetailDataForCircle(id);
        }
        #endregion

        public IEntity GetDetailData(int id)
        {
            return regionDAL.GetDetailData(id);
        }
        //Added by Swati
        public IEntity GetDetailDataConsumerMeter(int id)
        {
            return regionDAL.GetDetailDataByConsumerMeter(id);
        }


        public DataSet ListDataSet()
        {
            DataSet dataSet = regionDAL.ListDataSet(); 
            if (dataSet.Tables.Count < 1)
                return new DataSet();
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("SL. No", typeof(System.Int32)));
            table.Columns.Add(new DataColumn("Region_ID", typeof(System.Int32)));
            table.Columns.Add(new DataColumn("Region Name", typeof(System.String))); 
            int slno = 1;
            DataRow newRow;
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                newRow = table.NewRow();
                newRow["SL. No"] = slno;
                newRow["Region_ID"] = row["Region_ID"];
                newRow["Region Name"] = row["Region_Name"]; 
                table.Rows.Add(newRow);
                slno++;
            }
            dataSet.Tables.Remove("table");
            dataSet.Tables.Add(table);
            return dataSet;
        }

        public IEntity ValidateRegion(string regionName)
        {
            return regionDAL.GetDetailData(regionName);
        }
        //20th April 2012
        public bool ValidateRegion(IEntity entity)
        {
            return regionDAL.ValidateRegion(entity);
        }
        /// <summary>
        /// gets all the region data
        /// </summary>
        /// <returns></returns>
        public DataSet GetRegionData()
        {
            return regionDAL.GetRegionData();
        }

    }
}

