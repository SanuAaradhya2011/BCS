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
    public class MDWithIPDAL : DALBase
    {
        private string MeterID = "MeterID";
        private string KWDemandType = "KWDemandType";
        private string KWInterval = "KWInterval";
        private string KWSubInterval = "KWSubInterval";
        private string KVADemandType = "KVADemandType";
        private string KVAInterval = "KVAInterval";
        private string KVASubInterval = "KVASubInterval";
        private string FileUploadID = "FileUploadID";
        private string MeterDataID = "MeterData_ID";
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(MDWithIPDAL).ToString());
        //public MDWithIPDAL()
        //    : base("meterconfigurations", "MeterConfigurationID")
        //{
        //}

        public  override IEntity InsertData(IEntity entity)
        {
            E650MDWithIPEntity mdWithIPEntity = entity as E650MDWithIPEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert Into MDWithIP(MeterData_ID,KWDemandType,KWInterval,KWSubInterval,KVADemandType,KVAInterval,KVASubInterval) values(");
               
                builder.Append(string.Concat(ParameterName(MeterDataID), ","));                
                builder.Append(string.Concat(ParameterName(KWDemandType), ","));
                builder.Append(string.Concat(ParameterName(KWInterval), ","));
                builder.Append(string.Concat(ParameterName(KWSubInterval), ","));
                builder.Append(string.Concat(ParameterName(KVADemandType), ","));
                builder.Append(string.Concat(ParameterName(KVAInterval), ","));
                builder.Append(string.Concat(ParameterName(KVASubInterval), ")"));

                DataRequest request = new DataRequest(builder.ToString());

                request.AddParamter(ParameterName(MeterDataID), mdWithIPEntity.MeterDataID, DbType.Int64);                
                request.AddParamter(ParameterName(KWDemandType), mdWithIPEntity.KWDemandType, DbType.String, 10);
                request.AddParamter(ParameterName(KWInterval), mdWithIPEntity.KWInterval, DbType.String, 10);
                request.AddParamter(ParameterName(KWSubInterval), mdWithIPEntity.KWSubInterval, DbType.String, 10);
                request.AddParamter(ParameterName(KVADemandType), mdWithIPEntity.KVADemandType, DbType.String, 10);
                request.AddParamter(ParameterName(KVAInterval), mdWithIPEntity.KVAInterval, DbType.String, 10);
                request.AddParamter(ParameterName(KVASubInterval), mdWithIPEntity.KVASubInterval, DbType.String, 10);

                int row =  helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("MD with IP configuration inserted"));
                Flag = true;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "InsertData(IEntity entity)", ex);
            }
            return mdWithIPEntity;
        }

        public MeterConfigurationsNFEntity GetData( Int64 MeterData_ID)
        {
            MeterConfigurationsNFEntity meterConfigurationsNFEntity = new MeterConfigurationsNFEntity();
            E650MDWithIPEntity mdWithIPEntity = new E650MDWithIPEntity();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                DataRequest request = new DataRequest("select * from MDWithIP where MeterData_ID=" + MeterData_ID);
                DataSet iDataset = helper.FillDataSet(request, new DataSet());
                int i = 0;
                if (iDataset.Tables.Count!=0 && iDataset.Tables[0].Rows.Count > 0)
                {//MeterID,KWDemandType,KWInterval,KWSubInterval,KVADemandType,KVAInterval,KVASubInterval
                   // mdWithIPEntity.MeterID = meterID;
                    mdWithIPEntity.KWDemandType = iDataset.Tables[0].Rows[0]["KWDemandType"].ToString();
                    if (iDataset.Tables[0].Rows[0]["KWInterval"] != null)
                        mdWithIPEntity.KWInterval = Convert.ToInt16(iDataset.Tables[0].Rows[0]["KWInterval"]);
                    if (iDataset.Tables[0].Rows[0]["KWSubInterval"] != null)
                        mdWithIPEntity.KWSubInterval = Convert.ToInt16(iDataset.Tables[0].Rows[0]["KWSubInterval"]);
                    mdWithIPEntity.KVADemandType = iDataset.Tables[0].Rows[0]["KVADemandType"].ToString();
                    if (iDataset.Tables[0].Rows[0]["KVAInterval"] != null)
                        mdWithIPEntity.KVAInterval = Convert.ToInt16(iDataset.Tables[0].Rows[0]["KVAInterval"]);
                    if (iDataset.Tables[0].Rows[0]["KVASubInterval"] != null)
                        mdWithIPEntity.KVASubInterval = Convert.ToInt16(iDataset.Tables[0].Rows[0]["KVASubInterval"]);
                }
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("MDWithIP Paramater Read from Db"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetData( Int64 MeterData_ID)", ex);
                return null;
            }
            meterConfigurationsNFEntity.mdWithIPEntity = mdWithIPEntity;
            return meterConfigurationsNFEntity;
        }

        public bool DeleteData(long meterData_ID)
        {
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete from MDWithIP where ");
                builder.Append(string.Concat(MeterDataID, "=", ParameterName(MeterDataID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterDataID), meterData_ID, DbType.Int32);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data deleted"));
                Flag = true;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, " DeleteData(long meterData_ID)", ex);
            }
            return Flag;
        }

        //public override IEntity InsertData(IEntity entity)
        //{
        //    throw new NotImplementedException();
        //}
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
