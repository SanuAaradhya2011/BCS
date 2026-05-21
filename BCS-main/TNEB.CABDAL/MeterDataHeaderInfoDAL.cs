using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using CAB.IECFramework;
using CAB.DALC.Data.DataServices;
using CAB.Entity;
using CAB.IECFramework.Entity;
using System.Data.Common;
using CAB.IECFramework.Utility;

namespace CAB.DALC.Data
{
    public class MeterDataHeaderInfoDAL : DALBase
    {
        private string HeaderInfo_ID = "HeaderInfo_ID";
        private string MeterID = "MeterID";
        private string MeterData_ID = "MeterData_ID";
        private string MD1KWDemandType = "MD1KWDemandType";
        private string MD1KWTimeInterval = "MD1KWTimeInterval";
        private string MD1KWSubInterval = "MD1KWSubInterval";
        private string MD2KVADemandType = "MD2KVADemandType";
        private string MD2KVATimeInterval = "MD2KVATimeInterval";
        private string MD2KVASubInterval = "MD2KVASubInterval";
        private string PFLogic = "PFLogic";
        private string PowerOffDays = "PowerOffDays";
        private string MeterConstant = "MeterConstant";
        private string InternalCTPTRatio = "InternalCTPTRatio";
        private string SoftwareVersion = "SoftwareVersion";
        private string BillingType = "BillingType";
        private string BillingDate = "BillingDate";
        private string BillingHour = "BillingHour";
        private string BillingMinute = "BillingMinute";
        private string NoLoadDuration = "NoLoadDuration";
        private string NoSupplyDuration = "NoSupplyDuration";

        public MeterDataHeaderInfoDAL()
            : base("meterdata_headerinfo", "HeaderInfo_ID")
        {
        }
        public override IEntity InsertData(IEntity entity)
        {
            MeterDataHeaderInfoEntity meterDataHeaderInfoEntity = entity as MeterDataHeaderInfoEntity;
            bool Flag = false;
            try
            {             
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert Into meterdata_headerinfo(MD1KWDemandType,MD1KWTimeInterval,MD1KWSubInterval,MD2KVADemandType,MD2KVATimeInterval,MD2KVASubInterval,PFLogic,PowerOffDays,NoLoadDuration,NoSupplyDuration,MeterConstant,InternalCTPTRatio,SoftwareVersion,BillingType,BillingDate,BillingHour,BillingMinute,MeterData_ID) values(");
                builder.Append(string.Concat(ParameterName(MD1KWDemandType), ","));
                builder.Append(string.Concat(ParameterName(MD1KWTimeInterval), ","));    
                builder.Append(string.Concat(ParameterName(MD1KWSubInterval), ","));
                builder.Append(string.Concat(ParameterName(MD2KVADemandType), ","));
                builder.Append(string.Concat(ParameterName(MD2KVATimeInterval), ","));
                builder.Append(string.Concat(ParameterName(MD2KVASubInterval), ","));
                builder.Append(string.Concat(ParameterName(PFLogic), ","));
                builder.Append(string.Concat(ParameterName(PowerOffDays), ","));
                builder.Append(string.Concat(ParameterName(NoLoadDuration), ","));
                builder.Append(string.Concat(ParameterName(NoSupplyDuration), ","));
                builder.Append(string.Concat(ParameterName(MeterConstant), ","));  
                builder.Append(string.Concat(ParameterName(InternalCTPTRatio), ","));
                builder.Append(string.Concat(ParameterName(SoftwareVersion), ","));
                builder.Append(string.Concat(ParameterName(BillingType), ",")); 
                builder.Append(string.Concat(ParameterName(BillingDate), ","));
                builder.Append(string.Concat(ParameterName(BillingHour), ","));
                builder.Append(string.Concat(ParameterName(BillingMinute), ","));
                builder.Append(string.Concat(ParameterName(MeterData_ID), ")"));
                DataRequest request = new DataRequest(builder.ToString());                                                 
                request.AddParamter(ParameterName(MD1KWDemandType), meterDataHeaderInfoEntity.MD1KWDemandType, DbType.String, 50);
                request.AddParamter(ParameterName(MD1KWTimeInterval), meterDataHeaderInfoEntity.MD1KWTimeInterval, DbType.String, 50);
                request.AddParamter(ParameterName(MD1KWSubInterval), meterDataHeaderInfoEntity.MD1KWSubInterval, DbType.String, 50);
                request.AddParamter(ParameterName(MD2KVADemandType), meterDataHeaderInfoEntity.MD2KVADemandType, DbType.String, 50);
                request.AddParamter(ParameterName(MD2KVATimeInterval), meterDataHeaderInfoEntity.MD2KVATimeInterval, DbType.String, 50);
                request.AddParamter(ParameterName(MD2KVASubInterval), meterDataHeaderInfoEntity.MD2KVASubInterval, DbType.String, 50);
                request.AddParamter(ParameterName(PFLogic), meterDataHeaderInfoEntity.PFLogic, DbType.String, 50);
                request.AddParamter(ParameterName(PowerOffDays), meterDataHeaderInfoEntity.PowerOffDays, DbType.String, 50);
                request.AddParamter(ParameterName(NoLoadDuration), meterDataHeaderInfoEntity.NoLoadDuration, DbType.String, 50);
                request.AddParamter(ParameterName(NoSupplyDuration), meterDataHeaderInfoEntity.NoSupplyDuration, DbType.String, 50);
                request.AddParamter(ParameterName(MeterConstant), meterDataHeaderInfoEntity.MeterConstant, DbType.String, 50);
                request.AddParamter(ParameterName(InternalCTPTRatio), meterDataHeaderInfoEntity.InternalCTPTRatio, DbType.String, 50);
                request.AddParamter(ParameterName(SoftwareVersion), meterDataHeaderInfoEntity.SoftwareVersion, DbType.String, 50);
                request.AddParamter(ParameterName(BillingType), meterDataHeaderInfoEntity.BillingType, DbType.String, 50);
                request.AddParamter(ParameterName(BillingDate), meterDataHeaderInfoEntity.BillingDate, DbType.String, 50);
                request.AddParamter(ParameterName(BillingHour), meterDataHeaderInfoEntity.BillingHour, DbType.String, 50);
                request.AddParamter(ParameterName(BillingMinute), meterDataHeaderInfoEntity.BillingMinute, DbType.String, 50);
                request.AddParamter(ParameterName(MeterData_ID), meterDataHeaderInfoEntity.MeterData_ID, DbType.Int64);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data header information added"));
                Flag = true;
            }
            catch (Exception) { }
            if (Flag)
                meterDataHeaderInfoEntity.HeaderInfo_ID = long.Parse(this.GetPK());
            return meterDataHeaderInfoEntity;
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

        public bool DeleteData(string meterDataID)
        {
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete from meterdata_headerinfo where ");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataID, DbType.String);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data header information deleted"));
                Flag = true;
            }
            catch (Exception) { }
            return Flag;
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
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select * from meterdata_headerinfo ");
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data header information viewed"));
            }
            catch (CABException)
            {
            }
            return dataSet;
        }

        public override IEntity RowToEntity(DataRow row)
        {
            throw new NotImplementedException();
        }


        public DataSet GetMeterdataHeaderInfo(long activeMeterData_ID)
        {
            DataSet dataSet = new DataSet();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                DataRequest request = null;
                builder.Append("Select MD1KWDemandType as 'MD(KW) Demand Type',MD1KWTimeInterval as 'MD(KW) Time Interval',MD1KWSubInterval as 'MD(KW) Sub Interval',MD2KVADemandType as 'MD(KVA) Demand Type',MD2KVATimeInterval as 'MD(KVA) Time Interval',MD2KVASubInterval as 'MD(KVA) Sub Interval',BillingType as 'Type of Billing',BillingDate as 'Billing Date',BillingHour as 'Billing Hour',BillingMinute as 'Billing Minute',PFLogic as 'Power Factor Logic',PowerOffDays as 'Power Off Days',InternalCTPTRatio as 'Internal CT/PT Ratio',SoftwareVersion as 'Software Version' from meterdata_headerinfo");
                builder.Append(string.Concat(" where ", MeterData_ID, " = ", ParameterName(MeterData_ID)));

                request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), activeMeterData_ID, DbType.Int64);
               
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data header information viewed"));
            }
            catch (CABException)
            {
            }
            return dataSet;
        }

       
        
        public DataSet GetMeterdataHeaderInfo(string meterID)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                DataRequest request = null;

                builder.Append("Select a.HeaderInfo_ID,a.MD1KWDemandType,a.MD1KWTimeInterval,a.MD1KWSubInterval,a.MD2KVADemandType,a.MD2KVATimeInterval,a.MD2KVASubInterval,a.PowerOffDays,a.MeterConstant,a.PFLogic,a.BillingType,a.BillingDate,a.BillingHour,a.BillingMinute,a.InternalCTPTRatio,a.SoftwareVersion,b.MeterData_ID ,b.MeterID from meterdata_headerinfo a, meterdata b where a.MeterData_ID = b.MeterData_ID");
                builder.Append(string.Concat(" and b.", MeterID, " = ", ParameterName(MeterID)));
                builder.Append(" order by b.ReadingDateTime");
                request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterID), meterID, DbType.String, 50);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data header information viewed"));
            }
            catch (CABException)
            {
            }
            return dataSet;
        }
    }
}
