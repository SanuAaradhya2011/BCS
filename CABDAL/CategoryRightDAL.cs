using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh    			                     	|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| 
 */
using System.Text;
using CAB.DALC.Data.DataServices;
using CAB.Entity;
using CAB.Framework;
using CAB.Framework.Entity;
using Hunt.EPIC.Logging;

namespace CAB.DALC.Data
{
    public class CategoryRightDAL : DALBase
    {
        private string Category_ID = "Category_ID";
        private string Module_ID = "Module_ID";
        private string DefaultRight = "DefaultRight";
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(CategoryRightDAL).ToString());

        public void InsertDefaultRight()
        {
            ModuleMasterDAL moduleDAL = new ModuleMasterDAL();
            CategoryMasterDAL categoryMasterDAL = new CategoryMasterDAL();
            string[] qry = new string[40];

            qry[0] = "Insert Into categoryright(Category_ID,Module_ID,DefaultRight) values(" + Convert.ToString(categoryMasterDAL.GetCategoryIDByName("Administrator")) + "," + Convert.ToString(moduleDAL.GetModuleIdByName("User Administrator")) + ",1)";
            qry[1] = "Insert Into categoryright(Category_ID,Module_ID,DefaultRight) values(" + Convert.ToString(categoryMasterDAL.GetCategoryIDByName("Administrator")) + "," + Convert.ToString(moduleDAL.GetModuleIdByName("Programming")) + ",1)";
            qry[2] = "Insert Into categoryright(Category_ID,Module_ID,DefaultRight) values(" + Convert.ToString(categoryMasterDAL.GetCategoryIDByName("Administrator")) + "," + Convert.ToString(moduleDAL.GetModuleIdByName("Schedule")) + ",1)";
            qry[3] = "Insert Into categoryright(Category_ID,Module_ID,DefaultRight) values(" + Convert.ToString(categoryMasterDAL.GetCategoryIDByName("Administrator")) + "," + Convert.ToString(moduleDAL.GetModuleIdByName("Data Export/Import")) + ",1)";
            qry[4] = "Insert Into categoryright(Category_ID,Module_ID,DefaultRight) values(" + Convert.ToString(categoryMasterDAL.GetCategoryIDByName("Administrator")) + "," + Convert.ToString(moduleDAL.GetModuleIdByName("Data Archive")) + ",1)";
            qry[5] = "Insert Into categoryright(Category_ID,Module_ID,DefaultRight) values(" + Convert.ToString(categoryMasterDAL.GetCategoryIDByName("Administrator")) + "," + Convert.ToString(moduleDAL.GetModuleIdByName("Data Readout")) + ",1)";
            qry[6] = "Insert Into categoryright(Category_ID,Module_ID,DefaultRight) values(" + Convert.ToString(categoryMasterDAL.GetCategoryIDByName("Administrator")) + "," + Convert.ToString(moduleDAL.GetModuleIdByName("Definition")) + ",1)";
            qry[7] = "Insert Into categoryright(Category_ID,Module_ID,DefaultRight) values(" + Convert.ToString(categoryMasterDAL.GetCategoryIDByName("Administrator")) + "," + Convert.ToString(moduleDAL.GetModuleIdByName("Reports View")) + ",1)";

            qry[8] = "Insert Into categoryright(Category_ID,Module_ID,DefaultRight) values(" + Convert.ToString(categoryMasterDAL.GetCategoryIDByName("Master")) + "," + Convert.ToString(moduleDAL.GetModuleIdByName("User Administrator")) + ",0)";
            qry[9] = "Insert Into categoryright(Category_ID,Module_ID,DefaultRight) values(" + Convert.ToString(categoryMasterDAL.GetCategoryIDByName("Master")) + "," + Convert.ToString(moduleDAL.GetModuleIdByName("Programming")) + ",1)";
            qry[10] = "Insert Into categoryright(Category_ID,Module_ID,DefaultRight) values(" + Convert.ToString(categoryMasterDAL.GetCategoryIDByName("Master")) + "," + Convert.ToString(moduleDAL.GetModuleIdByName("Schedule")) + ",1)";
            qry[11] = "Insert Into categoryright(Category_ID,Module_ID,DefaultRight) values(" + Convert.ToString(categoryMasterDAL.GetCategoryIDByName("Master")) + "," + Convert.ToString(moduleDAL.GetModuleIdByName("Data Export/Import")) + ",1)";
            qry[12] = "Insert Into categoryright(Category_ID,Module_ID,DefaultRight) values(" + Convert.ToString(categoryMasterDAL.GetCategoryIDByName("Master")) + "," + Convert.ToString(moduleDAL.GetModuleIdByName("Data Archive")) + ",1)";
            qry[13] = "Insert Into categoryright(Category_ID,Module_ID,DefaultRight) values(" + Convert.ToString(categoryMasterDAL.GetCategoryIDByName("Master")) + "," + Convert.ToString(moduleDAL.GetModuleIdByName("Data Readout")) + ",1)";
            qry[14] = "Insert Into categoryright(Category_ID,Module_ID,DefaultRight) values(" + Convert.ToString(categoryMasterDAL.GetCategoryIDByName("Master")) + "," + Convert.ToString(moduleDAL.GetModuleIdByName("Definition")) + ",1)";
            qry[15] = "Insert Into categoryright(Category_ID,Module_ID,DefaultRight) values(" + Convert.ToString(categoryMasterDAL.GetCategoryIDByName("Master")) + "," + Convert.ToString(moduleDAL.GetModuleIdByName("Reports View")) + ",1)";

            qry[16] = "Insert Into categoryright(Category_ID,Module_ID,DefaultRight) values(" + Convert.ToString(categoryMasterDAL.GetCategoryIDByName("Utility")) + "," + Convert.ToString(moduleDAL.GetModuleIdByName("User Administrator")) + ",0)";
            qry[17] = "Insert Into categoryright(Category_ID,Module_ID,DefaultRight) values(" + Convert.ToString(categoryMasterDAL.GetCategoryIDByName("Utility")) + "," + Convert.ToString(moduleDAL.GetModuleIdByName("Programming")) + ",1)";
            qry[18] = "Insert Into categoryright(Category_ID,Module_ID,DefaultRight) values(" + Convert.ToString(categoryMasterDAL.GetCategoryIDByName("Utility")) + "," + Convert.ToString(moduleDAL.GetModuleIdByName("Schedule")) + ",1)";
            qry[19] = "Insert Into categoryright(Category_ID,Module_ID,DefaultRight) values(" + Convert.ToString(categoryMasterDAL.GetCategoryIDByName("Utility")) + "," + Convert.ToString(moduleDAL.GetModuleIdByName("Data Export/Import")) + ",1)";
            qry[20] = "Insert Into categoryright(Category_ID,Module_ID,DefaultRight) values(" + Convert.ToString(categoryMasterDAL.GetCategoryIDByName("Utility")) + "," + Convert.ToString(moduleDAL.GetModuleIdByName("Data Archive")) + ",0)";
            qry[21] = "Insert Into categoryright(Category_ID,Module_ID,DefaultRight) values(" + Convert.ToString(categoryMasterDAL.GetCategoryIDByName("Utility")) + "," + Convert.ToString(moduleDAL.GetModuleIdByName("Data Readout")) + ",1)";
            qry[22] = "Insert Into categoryright(Category_ID,Module_ID,DefaultRight) values(" + Convert.ToString(categoryMasterDAL.GetCategoryIDByName("Utility")) + "," + Convert.ToString(moduleDAL.GetModuleIdByName("Definition")) + ",0)";
            qry[23] = "Insert Into categoryright(Category_ID,Module_ID,DefaultRight) values(" + Convert.ToString(categoryMasterDAL.GetCategoryIDByName("Utility")) + "," + Convert.ToString(moduleDAL.GetModuleIdByName("Reports View")) + ",1)";

            qry[24] = "Insert Into categoryright(Category_ID,Module_ID,DefaultRight) values(" + Convert.ToString(categoryMasterDAL.GetCategoryIDByName("Reader")) + "," + Convert.ToString(moduleDAL.GetModuleIdByName("User Administrator")) + ",0)";
            qry[25] = "Insert Into categoryright(Category_ID,Module_ID,DefaultRight) values(" + Convert.ToString(categoryMasterDAL.GetCategoryIDByName("Reader")) + "," + Convert.ToString(moduleDAL.GetModuleIdByName("Programming")) + ",0)";
            qry[26] = "Insert Into categoryright(Category_ID,Module_ID,DefaultRight) values(" + Convert.ToString(categoryMasterDAL.GetCategoryIDByName("Reader")) + "," + Convert.ToString(moduleDAL.GetModuleIdByName("Schedule")) + ",0)";
            qry[27] = "Insert Into categoryright(Category_ID,Module_ID,DefaultRight) values(" + Convert.ToString(categoryMasterDAL.GetCategoryIDByName("Reader")) + "," + Convert.ToString(moduleDAL.GetModuleIdByName("Data Export/Import")) + ",0)";
            qry[28] = "Insert Into categoryright(Category_ID,Module_ID,DefaultRight) values(" + Convert.ToString(categoryMasterDAL.GetCategoryIDByName("Reader")) + "," + Convert.ToString(moduleDAL.GetModuleIdByName("Data Archive")) + ",0)";
            qry[29] = "Insert Into categoryright(Category_ID,Module_ID,DefaultRight) values(" + Convert.ToString(categoryMasterDAL.GetCategoryIDByName("Reader")) + "," + Convert.ToString(moduleDAL.GetModuleIdByName("Data Readout")) + ",1)";
            qry[30] = "Insert Into categoryright(Category_ID,Module_ID,DefaultRight) values(" + Convert.ToString(categoryMasterDAL.GetCategoryIDByName("Reader")) + "," + Convert.ToString(moduleDAL.GetModuleIdByName("Definition")) + ",0)";
            qry[31] = "Insert Into categoryright(Category_ID,Module_ID,DefaultRight) values(" + Convert.ToString(categoryMasterDAL.GetCategoryIDByName("Reader")) + "," + Convert.ToString(moduleDAL.GetModuleIdByName("Reports View")) + ",0)";

            qry[32] = "Insert Into categoryright(Category_ID,Module_ID,DefaultRight) values(" + Convert.ToString(categoryMasterDAL.GetCategoryIDByName("Data store administrator")) + "," + Convert.ToString(moduleDAL.GetModuleIdByName("User Administrator")) + ",0)";
            qry[33] = "Insert Into categoryright(Category_ID,Module_ID,DefaultRight) values(" + Convert.ToString(categoryMasterDAL.GetCategoryIDByName("Data store administrator")) + "," + Convert.ToString(moduleDAL.GetModuleIdByName("Programming")) + ",0)";
            qry[34] = "Insert Into categoryright(Category_ID,Module_ID,DefaultRight) values(" + Convert.ToString(categoryMasterDAL.GetCategoryIDByName("Data store administrator")) + "," + Convert.ToString(moduleDAL.GetModuleIdByName("Schedule")) + ",0)";
            qry[35] = "Insert Into categoryright(Category_ID,Module_ID,DefaultRight) values(" + Convert.ToString(categoryMasterDAL.GetCategoryIDByName("Data store administrator")) + "," + Convert.ToString(moduleDAL.GetModuleIdByName("Data Export/Import")) + ",0)";
            qry[36] = "Insert Into categoryright(Category_ID,Module_ID,DefaultRight) values(" + Convert.ToString(categoryMasterDAL.GetCategoryIDByName("Data store administrator")) + "," + Convert.ToString(moduleDAL.GetModuleIdByName("Data Archive")) + ",1)";
            qry[37] = "Insert Into categoryright(Category_ID,Module_ID,DefaultRight) values(" + Convert.ToString(categoryMasterDAL.GetCategoryIDByName("Data store administrator")) + "," + Convert.ToString(moduleDAL.GetModuleIdByName("Data Readout")) + ",0)";
            qry[38] = "Insert Into categoryright(Category_ID,Module_ID,DefaultRight) values(" + Convert.ToString(categoryMasterDAL.GetCategoryIDByName("Data store administrator")) + "," + Convert.ToString(moduleDAL.GetModuleIdByName("Definition")) + ",0)";
            qry[39] = "Insert Into categoryright(Category_ID,Module_ID,DefaultRight) values(" + Convert.ToString(categoryMasterDAL.GetCategoryIDByName("Data store administrator")) + "," + Convert.ToString(moduleDAL.GetModuleIdByName("Reports View")) + ",0)";
            IDataHelper helper = DatabaseFactory.GetHelper();
            for (int i = 0; i < 40; i++)
            {
                DataRequest request = new DataRequest(qry[i]);
                helper.ExecuteNonQuery(request);
            }
            UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Default rights for every category inserted"));
        }

        public override IEntity InsertData(IEntity entity)
        {
            CategoryRightEntity categoryRightEntity = null;
            if (entity == null)
                return categoryRightEntity;
            bool Flag = false;
            try
            {
                  categoryRightEntity = entity as CategoryRightEntity;
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert Into categoryright(Category_ID,Module_ID,DefaultRight) values(");
                builder.Append(string.Concat(ParameterName(Category_ID), ","));
                builder.Append(string.Concat(ParameterName(Module_ID), ","));
                builder.Append(string.Concat(ParameterName(DefaultRight), ")"));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Module_ID), categoryRightEntity.Module_ID, DbType.Int32);
                request.AddParamter(ParameterName(Module_ID), categoryRightEntity.Module_ID, DbType.Int32);
                request.AddParamter(ParameterName(DefaultRight), categoryRightEntity.DefaultRight, DbType.Int32);
                helper.ExecuteNonQuery(request);
                Flag = true;
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Default rights inserted"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "InsertData(IEntity entity)", ex);
                categoryRightEntity = null;
            }
            return categoryRightEntity;
        }

        
        public DataSet ListDataSet(int categoryID)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("select Module_ID,DefaultRight from categoryright Where ");
                builder.Append(string.Concat(Category_ID, "=", ParameterName(Category_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Category_ID), categoryID, DbType.String, 20);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                if (dataSet.Tables[0].Rows.Count > 0)
                    return dataSet;
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Default categories for a specified category listed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ListDataSet(int categoryID)", ex);
                dataSet = null;
            }
            return dataSet;
        }
        public override bool UpdateData(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteData(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public override IEntity GetDetailData(int id)
        {
            throw new NotImplementedException();
        }

        public override IList<IEntity> ListData()
        {
            throw new NotImplementedException();
        }

        public override IEntity RowToEntity(DataRow row)
        {
            throw new NotImplementedException();
        }

        public override DataSet ListDataSet()
        {
            throw new NotImplementedException();
        }

        public override IEntity InsertData(IList<IEntity> entities)
        {
            throw new NotImplementedException();
        }
    }
}
