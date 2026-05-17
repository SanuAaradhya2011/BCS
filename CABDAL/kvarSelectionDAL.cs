using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using CAB.DALC.Data.DataServices;
using CAB.Entity;
using CAB.Framework.Entity;
using CAB.Framework;
using Hunt.EPIC.Logging;

namespace CAB.DALC.Data
{
    public class kvarSelectionDAL : DALBase
    {
        private string MeterID = "MeterID";
        private string lagOnly = "lagOnly";
        private string lagandLead = "lagandLead";
        private string FileUploadID = "FileUploadID";
        private string MeterDataID = "MeterData_ID";
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(kvarSelectionDAL).ToString());
        //public kvarSelectionDAL()
        //    : base("meterdata_NamePlateDetail", "NamePlateDetailID")
        //{
        //}

        public override IEntity InsertData(IEntity entity)
        {
            kvarSelectionEntity kvarSelectionEntity = entity as kvarSelectionEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert Into kvarselection(MeterData_ID,lagOnly,lagandLead) values(");               
                builder.Append(string.Concat(ParameterName(MeterDataID), ","));             
                builder.Append(string.Concat(ParameterName(lagOnly), ","));
                builder.Append(string.Concat(ParameterName(lagandLead), ")"));

                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterDataID), kvarSelectionEntity.MeterDataID, DbType.String, 20);               
                request.AddParamter(ParameterName(lagOnly), kvarSelectionEntity.LagOnly, DbType.String, 10);
                request.AddParamter(ParameterName(lagandLead), kvarSelectionEntity.LagandLead, DbType.String, 10);

                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("kvar Selection configuration inserted"));
                Flag = true;
            }
            catch (Exception ex)    //Exception log for catch block 
            {
                logger.Log(LOGLEVELS.Error, "InsertData(IEntity entity)", ex);
            }
            return kvarSelectionEntity;
        }
        public MeterConfigurationsNFEntity GetData( Int64 MeterData_ID)
        {
            MeterConfigurationsNFEntity meterConfigurationsNFEntity = new MeterConfigurationsNFEntity();
            kvarSelectionEntity kvarSelectionEntity = new kvarSelectionEntity();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                DataRequest request = new DataRequest("select * from kvarselection where  MeterData_ID=" + MeterData_ID);
                DataSet iDataset = helper.FillDataSet(request, new DataSet());
                int i = 0;
                if (iDataset.Tables.Count !=0 && iDataset.Tables[0].Rows.Count > 0)
                {//MeterID,KWDemandType,KWInterval,KWSubInterval,KVADemandType,KVAInterval,KVASubInterval
                   // kvarSelectionEntity.MeterDataID = MeterDataID;
                    if (iDataset.Tables[0].Rows[0]["lagandLead"] != null)
                        kvarSelectionEntity.LagandLead = iDataset.Tables[0].Rows[0]["lagandLead"].ToString();
                    if (iDataset.Tables[0].Rows[0]["lagOnly"] != null)
                        kvarSelectionEntity.LagOnly = iDataset.Tables[0].Rows[0]["lagOnly"].ToString();
                }
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("kvarSelection Paramater Read from Db"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetData( Int64 MeterData_ID)", ex);
                return null;
            }
            meterConfigurationsNFEntity.kvarselectionEntity = kvarSelectionEntity;
            return meterConfigurationsNFEntity;
        }

        public bool DeleteData(long meterData_ID)
        {
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete from kvarselection where ");
                builder.Append(string.Concat(MeterDataID, "=", ParameterName(MeterDataID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterDataID), meterData_ID, DbType.Int32);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data deleted"));
                Flag = true;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "DeleteData(long meterData_ID)", ex);
            }
            return Flag;
        }
      
        public override IEntity InsertData(IList<IEntity> entities)
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

        public override IList<IEntity> ListData()
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
    }
}
