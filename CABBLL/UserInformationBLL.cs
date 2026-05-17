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
    public class UserInformationBLL : IBLL
    {
        UserInformationDAL userInformationDAL;

        public UserInformationBLL()
        {
            userInformationDAL = new UserInformationDAL();
        }

        public IEntity ValidateUser(IEntity entity)
        {
            UserInformationEntity userInformation = (UserInformationEntity)entity;
            return userInformationDAL.ValidateUser(userInformation);
        }

        public DataSet GetSearchData(string columnName, string value)
        {
            DataSet dataSet = userInformationDAL.ListDataSet(columnName, value);
            return ConvertData(dataSet);
        }

        public DataSet GetSearchData(string columnName, int value)
        {
            DataSet dataSet = userInformationDAL.ListDataSet(columnName, value);
            return ConvertData(dataSet);
        }
        public DataSet GetSearchData()
        {
			DataSet dataSet = userInformationDAL.ListDataSet();
            return ConvertData(dataSet);
        }

        private DataSet ConvertData(DataSet dataSet)
        {
            DesignationMasterBLL designationBLL = new DesignationMasterBLL();
            CategoryMasterBLL categoryBLL = new CategoryMasterBLL();
            if (dataSet.Tables.Count < 1)
                return new DataSet();
            DataTable table = new DataTable(); 
            table.Columns.Add(new DataColumn("SL. Number", typeof(System.Int32)));
            table.Columns.Add(new DataColumn("UserInformation_ID", typeof(System.Int32)));
            table.Columns.Add(new DataColumn("Users Name", typeof(System.String)));
            table.Columns.Add(new DataColumn("Login Id", typeof(System.String)));
            table.Columns.Add(new DataColumn("Category", typeof(System.String)));
            table.Columns.Add(new DataColumn("Designation", typeof(System.String)));
            int slno = 1;
            DataRow newRow;
            foreach (DataRow row in  dataSet.Tables[0].Rows)
            {
                newRow = table.NewRow();
                newRow["SL. Number"] = slno;
                newRow["UserInformation_ID"] = row["UserInformation_ID"];
                newRow["Users Name"] = row["Users_Name"];
                newRow["Login Id"] = row["Login_ID"];
                if (!string.IsNullOrEmpty(Convert.ToString(row["Category_ID"])))
                {
                    int categoryId = Int32.Parse(row["Category_ID"].ToString());
                    CategoryMasterEntity categoryEntity = (CategoryMasterEntity)categoryBLL.GetDetailData(categoryId);
					if(categoryEntity!=null)
                    newRow["Category"] = categoryEntity.Category_Name;
                }
                if (!string.IsNullOrEmpty(Convert.ToString(row["Designation_ID"])))
                {
                    int designationId = Int32.Parse(row["Designation_ID"].ToString());
                    DesignationMasterEntity designationEntity = (DesignationMasterEntity)designationBLL.GetDetailData(designationId);
					if(designationEntity!=null)
                    newRow["Designation"] = designationEntity.Designation_Name;
                }
                table.Rows.Add(newRow);
                slno++;
            }
            dataSet.Tables.Remove("table");
            dataSet.Tables.Add(table);
            return dataSet;
        }
        public int InsertData(IEntity entity)
		{ 
             userInformationDAL.InsertData(entity);
             string id=userInformationDAL.GetPK();
             if (string.IsNullOrEmpty(id))
                 return 0;
             else
                 return Int32.Parse(id);
		}
		public bool UpdateData(IEntity entity)
		{
			UserInformationEntity objUserInformation = (UserInformationEntity)entity;
			return userInformationDAL.UpdateData(objUserInformation);
		}
        public bool ValidateLogin(string loginId)
        {
            return userInformationDAL.ValidateLoginID(loginId);
        }
		public bool UpdatePassword(IEntity entity)
		{
			UserInformationEntity objUserInformation = (UserInformationEntity)entity;
            return userInformationDAL.UpdatePassword(objUserInformation);
		}

		public IEntity GetDetailData(int userId)
		{
			return userInformationDAL.GetDetailData(userId);
		}

		public bool DeleteData(IEntity entity)
		{
			return userInformationDAL.DeleteData(entity);
		}

		public DataSet GetModuleMasterData()
		{
			return userInformationDAL.GetModuleMasterData();
		}

		
    }
}
