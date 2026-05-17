/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 	 												|
 * | 											Changed By Piyush Singh																	|
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
	public class CategoryMasterBLL : IBLL
	{
        CategoryMasterDAL categoryMasterDAL;

        public CategoryMasterBLL()
        {
            categoryMasterDAL = new CategoryMasterDAL();
        }



        public IEntity InsertData(IEntity entity)
        {
            return categoryMasterDAL.InsertData(entity);
        }

        public bool UpdateData(IEntity entity)
        {
            return categoryMasterDAL.UpdateData(entity);
        }

        public bool DeleteData(IEntity entity)
        {
            return categoryMasterDAL.DeleteData(entity);
        }
        public IEntity GetDetailData(string id)
        {
            if (id.Equals(string.Empty))
                return null;
            return GetDetailData(Convert.ToInt32(id));
        }

        public IEntity GetDetailData(int id)
        {
            return categoryMasterDAL.GetDetailData(id);
        }

        public DataSet ListDataSet()
        {
            DataSet dataSet = categoryMasterDAL.ListDataSet(); 
            if (dataSet.Tables.Count < 1)
                return new DataSet();
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("SL. No", typeof(System.Int32)));
            table.Columns.Add(new DataColumn("Category_ID", typeof(System.Int32)));
            table.Columns.Add(new DataColumn("Category Name", typeof(System.String))); 
            int slno = 1;
            DataRow newRow;
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                newRow = table.NewRow();
                newRow["SL. No"] = slno;
                newRow["Category_ID"] = row["Category_ID"];
                newRow["Category Name"] = row["Category_Name"]; 
                table.Rows.Add(newRow);
                slno++;
            }
            dataSet.Tables.Remove("table");
            dataSet.Tables.Add(table);
            return dataSet;
        }

        public IEntity ValidateCategory(string categoryName)
        {
            return categoryMasterDAL.GetDetailData(categoryName);
        }

        public DataSet GetAllData()
        {
            return categoryMasterDAL.GetAllData();
        }

        public void InsertDefaultData()
        {
            categoryMasterDAL.InsertDefaultCategory();
        }
    }
}
