#region NameSpaces
using System;
using System.Collections.Generic;
using System.Text;
using CAB.DALC.Data.DataServices;
using CABEntity;
using System.Data;
using CAB.DALC.Data;
using CAB.Framework;
using CAB.Framework.Entity;
using System.Collections.ObjectModel;
using CAB.Entity;
using Hunt.EPIC.Logging;
#endregion

namespace CAB.DALC.Data
{
    public class DisplayParamaterDAL : DALBase
    {
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(DisplayParamaterDAL).ToString());
        /// <summary>
        /// 
        /// </summary>
        /// <param name="collDisplayParamatersDBEntity"></param>
        /// <param name="meterDataID"></param>
        /// <returns></returns>
        public bool InsertData(Collection<DisplayParamatersDBEntity> collDisplayParamatersDBEntity,Int64 meterDataID)
        {
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();

                for (int i = 0; i < collDisplayParamatersDBEntity.Count; i++)
                {
                    builder = new StringBuilder();
                    builder.Append("Insert Into displayparamater(MeterData_ID,DisplayParamaterType,DisplayParamaterName,DisplayParamaterValue) values(");                    
                    builder.Append(string.Concat(ParameterName("MeterData_ID"), ","));                   
                    builder.Append(string.Concat(ParameterName("DisplayParamaterType"), ","));
                    builder.Append(string.Concat(ParameterName("DisplayParamaterName"), ","));
                    builder.Append(string.Concat(ParameterName("DisplayParamaterValue"), ")"));
                    DataRequest request = new DataRequest(builder.ToString());                    
                    request.AddParamter(ParameterName("MeterData_ID"), meterDataID, DbType.Int64);                   
                    request.AddParamter(ParameterName("DisplayParamaterType"), (int)collDisplayParamatersDBEntity[i].displayParamaterType, DbType.Int32);
                    request.AddParamter(ParameterName("DisplayParamaterName"), collDisplayParamatersDBEntity[i].paramaterName, DbType.String);
                    request.AddParamter(ParameterName("DisplayParamaterValue"), collDisplayParamatersDBEntity[i].paramaterValue, DbType.Int32);
                    helper.ExecuteNonQuery(request);
                    UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Display Paramater " + collDisplayParamatersDBEntity[i].paramaterName + " added"));
                }
                return true;
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "InsertData(Collection<DisplayParamatersDBEntity> collDisplayParamatersDBEntity,Int64 meterDataID)", ex);
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="MeterData_ID"></param>
        /// <returns></returns>
        public Collection<Collection<string>> GetData(Int64 MeterData_ID)
        {
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                Collection<Collection<string>> collDisplayParamaters = new Collection<Collection<string>>();
                collDisplayParamaters.Add(new Collection<string>());
                collDisplayParamaters.Add(new Collection<string>());
                collDisplayParamaters.Add(new Collection<string>());
                collDisplayParamaters.Add(new Collection<string>());
                DataRequest request = new DataRequest("select * from displayparamater where  MeterData_ID=" + MeterData_ID);
                DataSet iDataset = helper.FillDataSet(request, new DataSet());
                int i = 0;
                while (iDataset.Tables.Count != 0 && (iDataset.Tables[0].Rows.Count > i))
                {
                    if ((int)DisplayParameterType.PushMode == Convert.ToInt32(iDataset.Tables[0].Rows[i]["DisplayParamaterType"]))
                        collDisplayParamaters[0].Add(iDataset.Tables[0].Rows[i]["DisplayParamaterName"].ToString());
                    else if ((int)DisplayParameterType.ScrollMode == Convert.ToInt32(iDataset.Tables[0].Rows[i]["DisplayParamaterType"]))
                        collDisplayParamaters[1].Add(iDataset.Tables[0].Rows[i]["DisplayParamaterName"].ToString());
                    else if ((int)DisplayParameterType.HighResolutionMode == Convert.ToInt32(iDataset.Tables[0].Rows[i]["DisplayParamaterType"]))
                        collDisplayParamaters[2].Add(iDataset.Tables[0].Rows[i]["DisplayParamaterName"].ToString());
                    else
                        collDisplayParamaters[3].Add(iDataset.Tables[0].Rows[i]["DisplayParamaterName"].ToString() + "/" + iDataset.Tables[0].Rows[i]["DisplayParamaterValue"].ToString());
                    i++;
                }
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Display Paramater Read from Db"));
                return collDisplayParamaters;
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetData(Int64 MeterData_ID)", ex);
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="MeterData_ID"></param>
        /// <returns></returns>
        public bool DeleteAllData(Int64 MeterData_ID)
        {
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                DataRequest request = new DataRequest("Delete from  displayparamater where  MeterData_ID=" + MeterData_ID);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Display Paramater data Deleted"));
                return true;
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "DeleteAllData(Int64 MeterData_ID)", ex);
                return false;
            }
        }
        public override IEntity InsertData(IList<IEntity> entities)
        {
            throw new NotImplementedException();
        }
        public override IList<IEntity> ListData()
        {
            throw new NotImplementedException();
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
        public override DataSet ListDataSet()
        {
            throw new NotImplementedException();
        }
        public override IEntity RowToEntity(DataRow row)
        {
            throw new NotImplementedException();
        }
        public override IEntity InsertData(IEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
