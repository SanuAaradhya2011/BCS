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
    public class kvarSelectionDAL : DALBase
    {
        private string MeterID = "MeterID";
        private string lagOnly = "lagOnly";
        private string lagandLead = "lagandLead";
        private string FileUploadID = "FileUploadID";
        private string MeterDataID = "MeterData_ID";
        //public kvarSelectionDAL()
        //    : base("meterdata_NamePlateDetail", "NamePlateDetailID")
        //{
        //}

        public IEntity InsertData(IEntity entity, Int64 fileUploadID, Int64 MeterData_ID)
        {
            IECkvarSelectionEntity kvarSelectionEntity = entity as IECkvarSelectionEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert Into kvarselection(MeterID,MeterData_ID,FileUploadID,lagOnly,lagandLead) values(");
                builder.Append(string.Concat(ParameterName(MeterID), ","));
                builder.Append(string.Concat(ParameterName(MeterDataID), ","));
                builder.Append(string.Concat(ParameterName(FileUploadID), ","));
                builder.Append(string.Concat(ParameterName(lagOnly), ","));
                builder.Append(string.Concat(ParameterName(lagandLead), ")"));

                DataRequest request = new DataRequest(builder.ToString());

                request.AddParamter(ParameterName(MeterID), kvarSelectionEntity.MeterID, DbType.String, 20);
                request.AddParamter(ParameterName(MeterDataID), MeterData_ID, DbType.Int64);
                request.AddParamter(ParameterName(FileUploadID), fileUploadID, DbType.Int64);
                request.AddParamter(ParameterName(lagOnly), kvarSelectionEntity.LagOnly, DbType.String, 10);
                request.AddParamter(ParameterName(lagandLead), kvarSelectionEntity.LagandLead, DbType.String, 10);

                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("kvar Selection configuration inserted"));
                Flag = true;
            }
            catch (Exception) 
            { }
            return kvarSelectionEntity;
        }
        public IECMeterConfigurationsNFEntity GetData(string meterID, Int64 fileUploadID, Int64 MeterData_ID)
        {
            IECMeterConfigurationsNFEntity meterConfigurationsNFEntity = new IECMeterConfigurationsNFEntity();
            IECkvarSelectionEntity kvarSelectionEntity = new IECkvarSelectionEntity();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                DataRequest request = new DataRequest("select * from kvarselection where meterid='" + meterID + "' and FileUploadID='" + fileUploadID + "' and MeterData_ID=" + MeterData_ID);
                DataSet iDataset = helper.FillDataSet(request, new DataSet());
                int i = 0;
                if (iDataset.Tables.Count !=0 && iDataset.Tables[0].Rows.Count > 0)
                {//MeterID,KWDemandType,KWInterval,KWSubInterval,KVADemandType,KVAInterval,KVASubInterval
                    kvarSelectionEntity.MeterID = meterID;
                    if (iDataset.Tables[0].Rows[0]["lagandLead"] != null)
                        kvarSelectionEntity.LagandLead = iDataset.Tables[0].Rows[0]["lagandLead"].ToString();
                    if (iDataset.Tables[0].Rows[0]["lagOnly"] != null)
                        kvarSelectionEntity.LagOnly = iDataset.Tables[0].Rows[0]["lagOnly"].ToString();
                }
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("kvarSelection Paramater Read from Db"));
            }
            catch (CABException)
            {
                return null;
            }
            meterConfigurationsNFEntity.kvarselectionEntity = kvarSelectionEntity;
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
