
/* |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 									        |
 * |											Date   : 25/03/2010 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| 
 */
using System;
using System.Data;
using System.Text;
using ExceptionServices.Data;
using CAB.Entity;
using CAB.IECFramework;
using CAB.IECFramework.Entity;
namespace CAB.DALC.Data
{
    public class LoadSurveyParameterDAL : DALBase
    {
        private string ColumnsNames = "ColumnsNames";
        private string MeterData_ID = "MeterData_ID";

        public override IEntity InsertData(IEntity entity)
        {
            IECLoadSurveyParameterEntity loadSurveyParameterEntity = null;
            if (entity == null)
                return loadSurveyParameterEntity;
            loadSurveyParameterEntity = entity as IECLoadSurveyParameterEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert Into LoadSurveyParameter(ColumnsNames,MeterData_ID) values(");
                builder.Append(string.Concat(ParameterName(ColumnsNames), ","));
                builder.Append(string.Concat(ParameterName(MeterData_ID), ")"));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(ColumnsNames), loadSurveyParameterEntity.ColumnsNames, DbType.String, 1000);
                request.AddParamter(ParameterName(MeterData_ID), loadSurveyParameterEntity.MeterDataId, DbType.Int64);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Load Survey data added"));
                Flag = true;
            }
            catch (CABException)
            {
                loadSurveyParameterEntity = null;  
            }
            return loadSurveyParameterEntity;
        }
        public override bool UpdateData(IEntity entity)
        {
            throw new System.NotImplementedException();
        }

        public override bool DeleteData(IEntity entity)
        {
            throw new System.NotImplementedException();
        }
        public bool DeleteData(long meterDataID)
        {
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete from LoadSurveyParameter where ");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataID, DbType.Int32);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data deleted"));
                Flag = true;
            }
            catch (Exception) { }
            return Flag;
        }
        public  IEntity GetDetailData(long id)
        {
            IECLoadSurveyParameterEntity loadSurveyParameterEntity = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select ColumnsNames,MeterData_ID from LoadSurveyParameter where ");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), id, DbType.UInt32);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    loadSurveyParameterEntity = (IECLoadSurveyParameterEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data RTC Update record viewed"));

            }
            catch (CABException)
            {
            }
            return loadSurveyParameterEntity;
        }

        public override System.Collections.Generic.IList<IEntity> ListData()
        {
            throw new System.NotImplementedException();
        }

        public override DataSet ListDataSet()
        {
            throw new System.NotImplementedException();
        }

        public override IEntity RowToEntity(DataRow row)
        {
            if (row == null) return null;
            IECLoadSurveyParameterEntity loadSurveyParameterEntity = new IECLoadSurveyParameterEntity();
            if (NotNullAndNotDBNull(row, MeterData_ID)) loadSurveyParameterEntity.MeterDataId = Convert.ToInt32(row[MeterData_ID]);
            if (NotNullAndNotDBNull(row, ColumnsNames)) loadSurveyParameterEntity.ColumnsNames = Convert.ToString(row[ColumnsNames]);
            return loadSurveyParameterEntity;
        }

        public override IEntity GetDetailData(int id)
        {
            throw new NotImplementedException();
        }

        public override IEntity InsertData(System.Collections.Generic.IList<IEntity> entities)
        {
            throw new NotImplementedException();
        }
    }
}