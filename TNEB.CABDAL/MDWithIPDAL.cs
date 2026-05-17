using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using CAB.DALC.Data.DataServices;
using CAB.Entity;
using CAB.IECFramework;
using CAB.IECFramework.Entity;

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
        //public MDWithIPDAL()
        //    : base("meterconfigurations", "MeterConfigurationID")
        //{
        //}

        public IEntity InsertData(IEntity entity, Int64 fileUploadID, Int64 MeterData_ID)
        {
            MDWithIPEntity mdWithIPEntity = entity as MDWithIPEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert Into meterConfigurations(MeterID,MeterData_ID,FileUploadID,KWDemandType,KWInterval,KWSubInterval,KVADemandType,KVAInterval,KVASubInterval) values(");
                builder.Append(string.Concat(ParameterName(MeterID), ","));
                builder.Append(string.Concat(ParameterName(MeterDataID), ","));
                builder.Append(string.Concat(ParameterName(FileUploadID), ","));
                builder.Append(string.Concat(ParameterName(KWDemandType), ","));
                builder.Append(string.Concat(ParameterName(KWInterval), ","));
                builder.Append(string.Concat(ParameterName(KWSubInterval), ","));
                builder.Append(string.Concat(ParameterName(KVADemandType), ","));
                builder.Append(string.Concat(ParameterName(KVAInterval), ","));
                builder.Append(string.Concat(ParameterName(KVASubInterval), ")"));

                DataRequest request = new DataRequest(builder.ToString());

                request.AddParamter(ParameterName(MeterID), mdWithIPEntity.MeterID, DbType.String, 20);
                request.AddParamter(ParameterName(MeterDataID), MeterData_ID, DbType.Int64);
                request.AddParamter(ParameterName(FileUploadID), fileUploadID, DbType.Int64);
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
            catch (Exception) 
            { 
            }
            return mdWithIPEntity;
        }

        public IECMeterConfigurationsNFEntity GetData(string meterID, Int64 fileUploadID, Int64 MeterData_ID)
        {
            IECMeterConfigurationsNFEntity meterConfigurationsNFEntity = new IECMeterConfigurationsNFEntity();
            MDWithIPEntity mdWithIPEntity = new MDWithIPEntity();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                DataRequest request = new DataRequest("select * from meterConfigurations where meterid='" + meterID + "' and FileUploadID='" + fileUploadID + "' and MeterData_ID=" + MeterData_ID);
                DataSet iDataset = helper.FillDataSet(request, new DataSet());
                int i = 0;
                if (iDataset.Tables.Count!=0 && iDataset.Tables[0].Rows.Count > 0)
                {//MeterID,KWDemandType,KWInterval,KWSubInterval,KVADemandType,KVAInterval,KVASubInterval
                    mdWithIPEntity.MeterID = meterID;
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
            catch (CABException)
            {
                return null;
            }
            meterConfigurationsNFEntity.mdWithIPEntity = mdWithIPEntity;
            return meterConfigurationsNFEntity;
        }
        public override IEntity InsertData(IEntity entity)
        {
            throw new NotImplementedException();
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
