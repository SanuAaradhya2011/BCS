
 /* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 	 								|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| 
 */
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
     public class BillingDAL : DALBase
    {
         private string Billing_ID = "Billing_ID"; 
         private string BillingResetType = "BillingResetType"; 
         private string CumulativeEnergyKWH = "CumulativeEnergyKWH";
         private string CumulativeEnergyKVARHLag = "CumulativeEnergyKVARHLag";
         private string CumulativeEnergyKVARHLead = "CumulativeEnergyKVARHLead"; 
         private string CumulativeEnergyKVAH = "CumulativeEnergyKVAH";
         private string CumulativeMD1 = "CumulativeMD1";
         private string CumulativeMD1TimeStamp = "CumulativeMD1TimeStamp"; 
         private string CumulativeMD2 = "CumulativeMD2";
         private string CumulativeMD2TimeStamp = "CumulativeMD2TimeStamp";   
         private string AveragePowerFactor = "AveragePowerFactor";
         private string PowerOnHours = "PowerOnHours";
         private string LoadFactor = "LoadFactor"; 
         private string MeterData_ID = "MeterData_ID";
         private string History_ID = "History_ID";
         private string FileName = "FileName";
         private string RelatedTo = "RelatedTo";
         private const string CumulativeExportEnergyKWH = "CumulativeExportEnergyKWH";
         private const string CumulativeExportEnergyKVAH = "CumulativeExportEnergyKVAH";
         private string MeterID = "MeterID";
         private bool isWBExportVCL = false;
         public BillingDAL()
             : base("meterdata_billing", "Billing_ID")
        {
        }
         public BillingDAL(CAB.Entity.IECUtilityEntity utilityEntity)
             : base("meterdata_billing", "Billing_ID")
         {
             if (utilityEntity == CAB.Entity.IECUtilityEntity.WBEXPORTVCL)
             {
                 isWBExportVCL = true;
             }
         }
         
        public DataSet GetCTRatio(int meterDataId)
        {
            DataSet dataSet = new DataSet();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                //builder.Append("Select History_ID,BillingResetType from meterdata_billing where DataIndex < 13 and ");// Story - 365971 - 13 billing for Power ON Hours
                builder.Append("Select History_ID,BillingResetType from meterdata_billing where DataIndex < 61 and ");// Story - 581355 - 60 months billing for Nepal 1P VIM Tender requirement
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataId, DbType.UInt32);
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("CT Ratio for a specified meter viewed."));
            }
            catch (CABException)
            {
                dataSet = null;
            }
            return dataSet;
        }
        public DataSet GetPowerOnHour(int meterDataId)
        {
            DataSet dataSet = new DataSet();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                //builder.Append("Select History_ID,PowerOnHours from meterdata_billing where DataIndex < 13 and ");// Story - 365971 - 13 billing for Power ON Hours
                builder.Append("Select History_ID,PowerOnHours from meterdata_billing where DataIndex < 61 and ");// Story - 581355 - 60 months billing for Nepal 1P VIM Tender requirement
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataId, DbType.UInt32);
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Billing data viewed."));
            }
            catch (CABException)
            {
                dataSet = null;
            }
            return dataSet;
        }
        public DataSet GetAveragePowerFactor(int meterDataId)
        {
            DataSet dataSet = new DataSet();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                //builder.Append("Select History_ID,AveragePowerFactor from meterdata_billing where DataIndex < 13 and ");// Story - 365971 - 13 billing for Power ON Hours
                builder.Append("Select History_ID,AveragePowerFactor from meterdata_billing where DataIndex < 61 and ");// Story - 581355 - 60 months billing for Nepal 1P VIM Tender requirement
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataId, DbType.UInt32);
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Avg power factor for a specified meter viewed."));
            }
            catch (CABException)
            {
                dataSet = null;
            }
            return dataSet;
        }
        public DataSet GetLoadFactor(int meterDataId)
        {
            DataSet dataSet = new DataSet();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                //builder.Append("Select History_ID,LoadFactor from meterdata_billing where DataIndex < 13 and ");// Story - 365971 - 13 billing for Power ON Hours
                builder.Append("Select History_ID,LoadFactor from meterdata_billing where DataIndex < 61 and ");// Story - 581355 - 60 months billing for Nepal 1P VIM Tender requirement
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataId, DbType.UInt32);
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Load Factor for a specified meter viewed."));
            }
            catch (CABException)
            {
                dataSet = null;
            }
            return dataSet;
        }
        public DataSet GetCumulativeEnergy(int meterDataId)
        {
            DataSet dataSet = new DataSet();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select A.History_ID,"); 
                builder.Append("B.BillingTimeStamp,");
                builder.Append("A.CumulativeEnergyKWH,"); //B.TotalActiveEnergy
                builder.Append("A.CumulativeEnergyKVAH,");
                builder.Append("A.CumulativeEnergyKVARHLag,");
                builder.Append("A.CumulativeEnergyKVARHLead");
                if (isWBExportVCL)
                {
                    builder.Append(",A.CumulativeExportEnergyKWH,A.CumulativeExportEnergyKVAH");
                }
                //builder.Append(" from meterdata_billing A,meterdata_tampercountergeneral B where A.DataIndex < 13 and A.MeterData_ID=B.MeterData_ID and A.History_ID=B.History_ID and B.RelatedTo in ('B','G') and ");// Story - 365971 - 13 billing for Power ON Hours
                builder.Append(" from meterdata_billing A,meterdata_tampercountergeneral B where A.DataIndex < 61 and A.MeterData_ID=B.MeterData_ID and A.History_ID=B.History_ID and B.RelatedTo in ('B','G') and ");// Story - 581355 - 60 months billing for Nepal 1P VIM Tender requirement
                builder.Append(string.Concat("A.",MeterData_ID, "=", ParameterName(MeterData_ID))); 
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataId, DbType.UInt32); 
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Cumulative energy for a specified meter viewed."));
            }
            catch (CABException)
            {
                dataSet = null;
            }
            return dataSet;
        }

         // For TNEB Billing Report
        public DataSet GetCumulativeEnergyTNEB(int meterDataId)
        {
            DataSet dataSet = new DataSet();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select A.History_ID,");
                builder.Append("B.BillingTimeStamp,");
                builder.Append("A.CumulativeEnergyKWH,"); //B.TotalActiveEnergy
                builder.Append("A.CumulativeEnergyKVAH,");
                builder.Append("A.CumulativeEnergyKVARHLag,");
                builder.Append("A.CumulativeEnergyKVARHLead,A.CumulativeMD1,A.CumulativeMD1TimeStamp,A.CumulativeMD2,A.CumulativeMD2TimeStamp,A.AveragePowerFactor");
                //builder.Append(" from meterdata_billing A,meterdata_tampercountergeneral B where A.DataIndex < 13 and A.MeterData_ID=B.MeterData_ID and A.History_ID=B.History_ID and B.RelatedTo in ('B','G') and A.History_ID <> 0 and B.BillingTimeStamp <> 0 and  "); // Story - 365971 - 13 billing for Power ON Hours
                builder.Append(" from meterdata_billing A,meterdata_tampercountergeneral B where A.DataIndex < 61 and A.MeterData_ID=B.MeterData_ID and A.History_ID=B.History_ID and B.RelatedTo in ('B','G') and A.History_ID <> 0 and B.BillingTimeStamp <> 0 and  "); // Story - 581355 - 60 months billing for Nepal 1P VIM Tender requirement
                builder.Append(string.Concat("A.", MeterData_ID, "=", ParameterName(MeterData_ID)));
                //builder.Append(" order by B.BillingTimeStamp desc");
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataId, DbType.UInt32);
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Cumulative energies for a specified meter viewed."));
            }
            catch (CABException)
            {
                dataSet = null;
            }
            return dataSet;
        }
        public DataSet GetMaximumDemand(int meterDataId)
        {
            DataSet dataSet = new DataSet();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select A.History_ID,"); 
                builder.Append("B.BillingTimeStamp,");
                 builder.Append("A.CumulativeMD1,");
                builder.Append("A.CumulativeMD1TimeStamp,");
                builder.Append("A.CumulativeMD2,");
                builder.Append("A.CumulativeMD2TimeStamp"); 
                builder.Append(" from meterdata_billing A,meterdata_tampercountergeneral B where A.DataIndex < 13 and A.MeterData_ID=B.MeterData_ID and A.History_ID=B.History_ID and B.RelatedTo in ('B','G') and ");
                builder.Append(string.Concat("A.",MeterData_ID, "=", ParameterName(MeterData_ID))); 
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataId, DbType.UInt32); 
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("MD for a specified meter viewed."));
            }
            catch (CABException)
            {
                dataSet = null;
            }
            return dataSet;
        }
       
        public override bool UpdateData(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public bool DeleteData(long meterDataID)
        {
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete from meterdata_billing where ");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataID, DbType.Int32);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Billing data for a specified meter deleted."));
                Flag = true;
            }
            catch (Exception) { }
            return Flag;
        }

        public override bool DeleteData(IEntity entity)
        {
            IECBillingEntity billingEntity = entity as IECBillingEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete from meterdata_billing where ");
                builder.Append(string.Concat(Billing_ID, "=", ParameterName(Billing_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Billing_ID), billingEntity.Billing_ID, DbType.UInt32);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Billing data deleted."));
                Flag = true;
            }
            catch (Exception) { }
            return Flag;
        }

        public override IEntity GetDetailData(int id)
        {
            IECBillingEntity billingEntity = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                //builder.Append("Select Billing_ID,BillingResetType,CumulativeEnergyKWH,CumulativeEnergyKVARHLag,CumulativeEnergyKVARHLead,CumulativeEnergyKVAH,CumulativeMD1,CumulativeMD1TimeStamp,CumulativeMD2,CumulativeMD2TimeStamp,AveragePowerFactor,PowerOnHours,LoadFactor,MeterData_ID,History_ID from meterdata_billing where DataIndex < 13 and ");// Story - 365971 - 13 billing for Power ON Hours
                builder.Append("Select Billing_ID,BillingResetType,CumulativeEnergyKWH,CumulativeEnergyKVARHLag,CumulativeEnergyKVARHLead,CumulativeEnergyKVAH,CumulativeMD1,CumulativeMD1TimeStamp,CumulativeMD2,CumulativeMD2TimeStamp,AveragePowerFactor,PowerOnHours,LoadFactor,MeterData_ID,History_ID from meterdata_billing where DataIndex < 61 and ");// Story - 581355 - 60 months billing for Nepal 1P VIM Tender requirement
                builder.Append(string.Concat(Billing_ID, "=", ParameterName(Billing_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Billing_ID), id, DbType.UInt32);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    billingEntity = (IECBillingEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Billing data viewed."));

            }
            catch (CABException)
            { 
            }
            return billingEntity;
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
                //builder.Append("Select Billing_ID,BillingResetType,CumulativeEnergyKWH,CumulativeEnergyKVARHLag,CumulativeEnergyKVARHLead,CumulativeEnergyKVAH,CumulativeMD1,CumulativeMD1TimeStamp,CumulativeMD2,CumulativeMD2TimeStamp,AveragePowerFactor,PowerOnHours,LoadFactor,MeterData_ID,History_ID from meterdata_billing where DataIndex < 13 and ");// Story - 365971 - 13 billing for Power ON Hours
                builder.Append("Select Billing_ID,BillingResetType,CumulativeEnergyKWH,CumulativeEnergyKVARHLag,CumulativeEnergyKVARHLead,CumulativeEnergyKVAH,CumulativeMD1,CumulativeMD1TimeStamp,CumulativeMD2,CumulativeMD2TimeStamp,AveragePowerFactor,PowerOnHours,LoadFactor,MeterData_ID,History_ID from meterdata_billing where DataIndex < 61 and ");// Story - 581355 - 60 months billing for Nepal 1P VIM Tender requirement
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Billing data viewed."));
            }
            catch (CABException)
            {
            }
            return dataSet;
        }

		 //Piyush Singh Included for the listing of all the hitory data for exporting the billing values according to the meterdata_ID

		public DataSet ListDataSet(int meterDataID)
		{
			DataSet dataSet = null;
			try
			{
				IDataHelper helper = DatabaseFactory.GetHelper();
				StringBuilder builder = new StringBuilder();
				//builder.Append("Select A.MeterID,B.History_ID,B.Billing_ID,B.BillingResetType,B.CumulativeEnergyKWH,B.CumulativeEnergyKVARHLag,B.CumulativeEnergyKVARHLead,B.CumulativeEnergyKVAH,B.CumulativeMD1,B.CumulativeMD1TimeStamp,B.CumulativeMD2,B.CumulativeMD2TimeStamp,B.AveragePowerFactor,B.PowerOnHours,B.LoadFactor,B.MeterData_ID from meterdata_billing B Inner Join meterdata A on B.MeterData_ID = A.MeterData_ID where B.DataIndex < 13 and ");
                builder.Append("Select A.MeterID,B.History_ID,B.Billing_ID,B.BillingResetType,B.CumulativeEnergyKWH,B.CumulativeEnergyKVARHLag,B.CumulativeEnergyKVARHLead,B.CumulativeEnergyKVAH,B.CumulativeMD1,B.CumulativeMD1TimeStamp,B.CumulativeMD2,B.CumulativeMD2TimeStamp,B.AveragePowerFactor,B.PowerOnHours,B.LoadFactor,B.MeterData_ID from meterdata_billing B Inner Join meterdata A on B.MeterData_ID = A.MeterData_ID where B.DataIndex < 61 and ");// Story - 581355 - 60 months billing for Nepal 1P VIM Tender requirement
				builder.Append(string.Concat("B.",MeterData_ID, "=", ParameterName(MeterData_ID)));
				DataRequest request = new DataRequest(builder.ToString());
				request.AddParamter(ParameterName(MeterData_ID), meterDataID, DbType.UInt32);
				dataSet = new DataSet();
				dataSet = helper.FillDataSet(request, dataSet);
				UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Billing data Retrieved."));
			}
			catch (CABException)
			{
			}
			return dataSet;
		}

        public override IEntity RowToEntity(DataRow row)
        {
            if (row == null) return null;
            IECBillingEntity billingEntity = new IECBillingEntity(); 
            if (NotNullAndNotDBNull(row,Billing_ID )) billingEntity.Billing_ID = Convert.ToInt32(row[Billing_ID]);
            if (NotNullAndNotDBNull(row, BillingResetType)) billingEntity.BillingResetType = Convert.ToString(row[BillingResetType]);
            if (NotNullAndNotDBNull(row,CumulativeEnergyKWH  )) billingEntity.CumulativeEnergyKWH  = Convert.ToString(row[CumulativeEnergyKWH ]);
            if (NotNullAndNotDBNull(row,CumulativeEnergyKVARHLag )) billingEntity.CumulativeEnergyKVARHLag = Convert.ToString(row[CumulativeEnergyKVARHLag]);
            if (NotNullAndNotDBNull(row,CumulativeEnergyKVARHLead )) billingEntity.CumulativeEnergyKVARHLead = Convert.ToString(row[CumulativeEnergyKVARHLead]);
            if (NotNullAndNotDBNull(row,CumulativeEnergyKVAH )) billingEntity.CumulativeEnergyKVAH = Convert.ToString(row[CumulativeEnergyKVAH]);
            if (NotNullAndNotDBNull(row,CumulativeMD1 )) billingEntity. CumulativeMD1= Convert.ToString(row[CumulativeMD1]);
            if (NotNullAndNotDBNull(row, CumulativeMD1TimeStamp )) billingEntity. CumulativeMD1TimeStamp = Convert.ToString(row[CumulativeMD1TimeStamp ]);
            if (NotNullAndNotDBNull(row,CumulativeMD2  )) billingEntity.CumulativeMD2  = Convert.ToString(row[CumulativeMD2 ]);
            if (NotNullAndNotDBNull(row, CumulativeMD2TimeStamp )) billingEntity.CumulativeMD2TimeStamp  = Convert.ToString(row[CumulativeMD2TimeStamp ]); 
            if (NotNullAndNotDBNull(row,AveragePowerFactor )) billingEntity.AveragePowerFactor = Convert.ToString(row[AveragePowerFactor]);
            if (NotNullAndNotDBNull(row,PowerOnHours )) billingEntity.PowerOnHours = Convert.ToString(row[PowerOnHours]);
            if (NotNullAndNotDBNull(row, LoadFactor)) billingEntity. LoadFactor= Convert.ToString(row[LoadFactor]);
            if (NotNullAndNotDBNull(row, MeterData_ID)) billingEntity.MeterData_ID = Convert.ToInt32(row[MeterData_ID]);
            if (NotNullAndNotDBNull(row, History_ID)) billingEntity.History_ID = Convert.ToInt32(row[History_ID]);
            return billingEntity;
        }

        private DataRequest GetRequest(IEntity entity)
        {
            IECBillingEntity billingEntity = entity as IECBillingEntity;
            StringBuilder builder = new StringBuilder();
            builder.Append("Insert Into meterdata_billing(BillingResetType,CumulativeEnergyKWH,CumulativeEnergyKVARHLag,CumulativeEnergyKVARHLead,CumulativeEnergyKVAH,");
            if(isWBExportVCL)
            {
               builder.Append("CumulativeExportEnergyKWH,CumulativeExportEnergyKVAH,");
            }
            builder.Append("CumulativeMD1,CumulativeMD1TimeStamp,CumulativeMD2,CumulativeMD2TimeStamp,AveragePowerFactor,PowerOnHours,LoadFactor,MeterData_ID,History_ID) values(");
            builder.Append(string.Concat(ParameterName(BillingResetType), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergyKWH), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergyKVARHLag), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergyKVARHLead), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergyKVAH), ","));
            if (isWBExportVCL)
            {
                builder.Append(string.Concat(ParameterName(CumulativeExportEnergyKWH), ","));
                builder.Append(string.Concat(ParameterName(CumulativeExportEnergyKVAH), ","));
            }
            builder.Append(string.Concat(ParameterName(CumulativeMD1), ","));
            builder.Append(string.Concat(ParameterName(CumulativeMD1TimeStamp), ","));
            builder.Append(string.Concat(ParameterName(CumulativeMD2), ","));
            builder.Append(string.Concat(ParameterName(CumulativeMD2TimeStamp), ",")); 
            builder.Append(string.Concat(ParameterName(AveragePowerFactor), ","));
            builder.Append(string.Concat(ParameterName(PowerOnHours), ","));
            builder.Append(string.Concat(ParameterName(LoadFactor), ","));
            builder.Append(string.Concat(ParameterName(MeterData_ID), ","));
            builder.Append(string.Concat(ParameterName(History_ID), ")"));
            DataRequest request = new DataRequest(builder.ToString());
            request.AddParamter(ParameterName(BillingResetType), billingEntity.BillingResetType, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergyKWH), billingEntity.CumulativeEnergyKWH, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergyKVARHLag), billingEntity.CumulativeEnergyKVARHLag, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergyKVARHLead), billingEntity.CumulativeEnergyKVARHLead, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergyKVAH), billingEntity.CumulativeEnergyKVAH, DbType.String, 40);
            if (isWBExportVCL)
            {
                request.AddParamter(ParameterName(CumulativeExportEnergyKWH), billingEntity.CumulativeExportEnergyKWH, DbType.String, 40);
                request.AddParamter(ParameterName(CumulativeExportEnergyKVAH), billingEntity.CumulativeExportEnergyKVAH, DbType.String, 40);

            }
            request.AddParamter(ParameterName(CumulativeMD1), billingEntity.CumulativeMD1, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeMD1TimeStamp), billingEntity.CumulativeMD1TimeStamp, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeMD2), billingEntity.CumulativeMD2, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeMD2TimeStamp), billingEntity.CumulativeMD2TimeStamp, DbType.String, 40); 
            request.AddParamter(ParameterName(AveragePowerFactor), billingEntity.AveragePowerFactor, DbType.String, 40);
            request.AddParamter(ParameterName(PowerOnHours), billingEntity.PowerOnHours, DbType.String, 40);
            request.AddParamter(ParameterName(LoadFactor), billingEntity.LoadFactor, DbType.String, 40);
            request.AddParamter(ParameterName(MeterData_ID), billingEntity.MeterData_ID, DbType.Int32);
            request.AddParamter(ParameterName(History_ID), billingEntity.History_ID, DbType.Int16);
            return request;
        }
        public override IEntity InsertData(IEntity entity)
        {
            IECBillingEntity billingEntity = entity as IECBillingEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                DataRequest request = this.GetRequest(entity); 
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("New billing data added"));
                Flag = true;
            }
            catch (Exception) { }
            if (Flag)
                billingEntity.Billing_ID = long.Parse(this.GetPK());
            return billingEntity;
        }

        public override IEntity InsertData(IList<IEntity> entities)
        {
            List<DataRequest> requests = new List<DataRequest>();
            foreach (IEntity entity in entities) 
                requests.Add(this.GetRequest(entity));
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                helper.ExecuteNonQuery(requests);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("New billing data added")); 
            }
            catch (Exception) { }
            return null;
        }

        public DataSet GetBillingDataByFile(string fileName, List<string> columns)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select m.MeterID ");
                foreach (string column in columns)
                {
                    builder.Append(string.Concat(",", column, " "));
                }
                builder.Append("from meterdata_billing b inner join meterdata m on b.MeterData_ID = m.MeterData_ID ");
                builder.Append("inner join meterdata_tariffinformation ti on m.MeterData_ID = ti.MeterData_ID ");
                builder.Append("and b.History_ID = ti.HistoryID ");
                builder.Append("inner join meterdata_tampercountergeneral tc on m.MeterData_ID = tc.MeterData_ID ");
                builder.Append("and b.History_ID = tc.History_ID and ti.HistoryID = tc.History_ID ");
                //builder.Append("inner join fileupload_master f on m.FileUpload_ID = f.FileUpload_ID where b.DataIndex < 13 and ");// Story - 365971 - 13 billing for Power ON Hours
                builder.Append("inner join fileupload_master f on m.FileUpload_ID = f.FileUpload_ID where b.DataIndex < 61 and ");// Story - 581355 - 60 months billing for Nepal 1P VIM Tender requirement
                builder.Append(string.Concat("f.", FileName, "=", ParameterName(FileName), " ", "and "));
                builder.Append(string.Concat("(tc.RelatedTo", "=", "'G' ", "or "));
                builder.Append(string.Concat("tc.RelatedTo", "=", "'B')"));

                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(FileName), fileName, DbType.String);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Billing data for a specified file viewed"));
            }
            catch (CABException)
            {
            }
            return dataSet;
        }

        public DataSet GetBillingDataByMeter(string meterID, List<string> columns)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select m.MeterID, f.FileName ");
                foreach (string column in columns)
                {
                    builder.Append(string.Concat(",", column, " "));
                }
                builder.Append("from meterdata_billing b inner join meterdata m on b.MeterData_ID = m.MeterData_ID ");
                builder.Append("inner join meterdata_tariffinformation ti on m.MeterData_ID = ti.MeterData_ID ");
                builder.Append("and b.History_ID = ti.HistoryID ");
                builder.Append("inner join meterdata_tampercountergeneral tc on m.MeterData_ID = tc.MeterData_ID ");
                builder.Append("and b.History_ID = tc.History_ID and ti.HistoryID = tc.History_ID ");
                //builder.Append("inner join fileupload_master f on m.FileUpload_ID = f.FileUpload_ID where b.DataIndex < 13 and ");// Story - 365971 - 13 billing for Power ON Hours
                builder.Append("inner join fileupload_master f on m.FileUpload_ID = f.FileUpload_ID where b.DataIndex < 61 and ");// Story - 581355 - 60 months billing for Nepal 1P VIM Tender requirement
                builder.Append(string.Concat("m.", MeterID, "=", ParameterName(MeterID), " ", "and "));
                builder.Append(string.Concat("(tc.RelatedTo", "=", "'B'", " " ,"or "));
                builder.Append(string.Concat("tc.RelatedTo", "=", "'G'", ")"));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterID), meterID, DbType.String);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Billing data for a specified meter viewed"));
            }
            catch (CABException)
            {
            }
            return dataSet;
        }


    } 
}

