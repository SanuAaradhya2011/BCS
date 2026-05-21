/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 	 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| 
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using CAB.IECFramework;
using CAB.DALC.Data.DataServices;
using CAB.Entity;
using CAB.IECFramework.Entity;
using System.Data.Common;

namespace CAB.DALC.Data
{
    public class TamperCounterGeneralDAL : DALBase
    {
        private string TamperCounterGeneral_ID = "TamperCounterGeneral_ID";
        private string VoltageImbalanceRPhaseTamperCounter = "VoltageImbalanceRPhaseTamperCounter";
        private string VoltageImbalanceYPhaseTamperCounter = "VoltageImbalanceYPhaseTamperCounter";
        private string VoltageImbalanceBPhaseTamperCounter = "VoltageImbalanceBPhaseTamperCounter";
        private string MissingPotentialRPhaseTamperCounter = "MissingPotentialRPhaseTamperCounter";
        private string MissingPotentialYPhaseTamperCounter = "MissingPotentialYPhaseTamperCounter";
        private string MissingPotentialBPhaseTamperCounter = "MissingPotentialBPhaseTamperCounter";
        private string CTShortTamperCounter = "CTShortTamperCounter";
        private string CTOpenRPhaseTamperCounter = "CTOpenRPhaseTamperCounter";
        private string CTOpenYPhaseTamperCounter = "CTOpenYPhaseTamperCounter";
        private string CTOpenBPhaseTamperCounter = "CTOpenBPhaseTamperCounter"; 
        private string OnePhaseNeutralAbsentTamperCounter = "OnePhaseNeutralAbsentTamperCounter"; 
        private string VoltagePhaseReversalTamperCounter = "VoltagePhaseReversalTamperCounter";
        private string CurrentImbalanceRPhaseTamperCounter = "CurrentImbalanceRPhaseTamperCounter";
        private string CurrentImbalanceYPhaseTamperCounter = "CurrentImbalanceYPhaseTamperCounter";
        private string CurrentImbalanceBPhaseTamperCounter = "CurrentImbalanceBPhaseTamperCounter";
        private string CurrentReversalRPhaseTamperCounter = "CurrentReversalRPhaseTamperCounter";
        private string CurrentReversalYPhaseTamperCounter = "CurrentReversalYPhaseTamperCounter";
        private string CurrentReversalBPhaseTamperCounter = "CurrentReversalBPhaseTamperCounter";
        private string MagneticInfluenceTamperCounter = "MagneticInfluenceTamperCounter";
        private string NeutralDisturbanceTamperCounter = "NeutralDisturbanceTamperCounter";
        private string FrontCoverOpeningTamperCounter = "FrontCoverOpeningTamperCounter"; 
        private string History_ID = "History_ID";
        private string MeterData_ID = "MeterData_ID";
        private string RelatedTo="RelatedTo";
        private string BillingTimeStamp = "BillingTimeStamp";
        private string BillingCounter = "BillingCounter";
        
        public TamperCounterGeneralDAL()
            : base("MeterData_TamperCounterGeneral", "TamperCounterGeneral_ID")
        {
        }
 
        public DataSet GetTamperCounter(int meterDataId, string tamperName)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select ");
                builder.Append(tamperName);
                builder.Append(" from MeterData_TamperCounterGeneral where ");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)," and RelatedTo in ('G','B')"));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataId, DbType.UInt32);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Tamper counter viewed")); 
            }
            catch (Exception) { dataSet = null; }
            return dataSet;
        }
        public object GetTamperCount(int meterDataId, string tamperName)
        {
            object obj = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select ");
                builder.Append(tamperName);
                builder.Append(" from MeterData_TamperCounterGeneral where relatedTo='T' and ");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataId, DbType.UInt32);
                  obj = helper.ExecuteScalar(request);
                  UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Tamper counter viewed")); 
            }
            catch (Exception) { obj = null; }
            return obj;
        }
        private DataRequest GetRequest(IEntity entity)
        {
            TamperCounterGeneralEntity tamperCounterGeneralEntity = entity as TamperCounterGeneralEntity;
            StringBuilder builder = new StringBuilder();
            builder.Append("Insert Into MeterData_TamperCounterGeneral(VoltageImbalanceRPhaseTamperCounter,VoltageImbalanceYPhaseTamperCounter,VoltageImbalanceBPhaseTamperCounter,");
            builder.Append("MissingPotentialRPhaseTamperCounter, MissingPotentialYPhaseTamperCounter, MissingPotentialBPhaseTamperCounter,CTShortTamperCounter,");
            builder.Append("CTOpenRPhaseTamperCounter,CTOpenYPhaseTamperCounter,CTOpenBPhaseTamperCounter,OnePhaseNeutralAbsentTamperCounter,VoltagePhaseReversalTamperCounter,");
            builder.Append("CurrentImbalanceRPhaseTamperCounter,CurrentImbalanceYPhaseTamperCounter,CurrentImbalanceBPhaseTamperCounter,CurrentReversalRPhaseTamperCounter,");
            builder.Append("CurrentReversalYPhaseTamperCounter,CurrentReversalBPhaseTamperCounter,MagneticInfluenceTamperCounter,NeutralDisturbanceTamperCounter,");
            builder.Append("FrontCoverOpeningTamperCounter,History_ID,MeterData_ID,RelatedTo,BillingTimeStamp,BillingCounter) values(");
            builder.Append(string.Concat(ParameterName(VoltageImbalanceRPhaseTamperCounter), ","));
            builder.Append(string.Concat(ParameterName(VoltageImbalanceYPhaseTamperCounter), ","));
            builder.Append(string.Concat(ParameterName(VoltageImbalanceBPhaseTamperCounter), ","));
            builder.Append(string.Concat(ParameterName(MissingPotentialRPhaseTamperCounter), ","));
            builder.Append(string.Concat(ParameterName(MissingPotentialYPhaseTamperCounter), ","));
            builder.Append(string.Concat(ParameterName(MissingPotentialBPhaseTamperCounter), ","));
            builder.Append(string.Concat(ParameterName(CTShortTamperCounter), ","));
            builder.Append(string.Concat(ParameterName(CTOpenRPhaseTamperCounter), ","));
            builder.Append(string.Concat(ParameterName(CTOpenYPhaseTamperCounter), ","));
            builder.Append(string.Concat(ParameterName(CTOpenBPhaseTamperCounter), ","));
               builder.Append(string.Concat(ParameterName(OnePhaseNeutralAbsentTamperCounter), ","));
             builder.Append(string.Concat(ParameterName(VoltagePhaseReversalTamperCounter), ","));
            builder.Append(string.Concat(ParameterName(CurrentImbalanceRPhaseTamperCounter), ","));
            builder.Append(string.Concat(ParameterName(CurrentImbalanceYPhaseTamperCounter), ","));
            builder.Append(string.Concat(ParameterName(CurrentImbalanceBPhaseTamperCounter), ","));
            builder.Append(string.Concat(ParameterName(CurrentReversalRPhaseTamperCounter), ","));
            builder.Append(string.Concat(ParameterName(CurrentReversalYPhaseTamperCounter), ","));
            builder.Append(string.Concat(ParameterName(CurrentReversalBPhaseTamperCounter), ","));
            builder.Append(string.Concat(ParameterName(MagneticInfluenceTamperCounter), ","));
            builder.Append(string.Concat(ParameterName(NeutralDisturbanceTamperCounter), ","));
            builder.Append(string.Concat(ParameterName(FrontCoverOpeningTamperCounter), ",")); 
            builder.Append(string.Concat(ParameterName(History_ID), ","));
            builder.Append(string.Concat(ParameterName(MeterData_ID), ","));
            builder.Append(string.Concat(ParameterName(RelatedTo), ","));
            builder.Append(string.Concat(ParameterName(BillingTimeStamp), ","));
            builder.Append(string.Concat(ParameterName(BillingCounter), ")"));
            DataRequest request = new DataRequest(builder.ToString());
            request.AddParamter(ParameterName(VoltageImbalanceRPhaseTamperCounter), tamperCounterGeneralEntity.VoltageImbalanceRPhaseTamperCounter, DbType.Int32);
            request.AddParamter(ParameterName(VoltageImbalanceYPhaseTamperCounter), tamperCounterGeneralEntity.VoltageImbalanceYPhaseTamperCounter, DbType.Int32);
            request.AddParamter(ParameterName(VoltageImbalanceBPhaseTamperCounter), tamperCounterGeneralEntity.VoltageImbalanceBPhaseTamperCounter, DbType.Int32);
            request.AddParamter(ParameterName(MissingPotentialRPhaseTamperCounter), tamperCounterGeneralEntity.MissingPotentialRPhaseTamperCounter, DbType.Int32);
            request.AddParamter(ParameterName(MissingPotentialYPhaseTamperCounter), tamperCounterGeneralEntity.MissingPotentialYPhaseTamperCounter, DbType.Int32);
            request.AddParamter(ParameterName(MissingPotentialBPhaseTamperCounter), tamperCounterGeneralEntity.MissingPotentialBPhaseTamperCounter, DbType.Int32);
              request.AddParamter(ParameterName(CTShortTamperCounter), tamperCounterGeneralEntity.CTShortTamperCounter, DbType.Int32);
            request.AddParamter(ParameterName(CTOpenRPhaseTamperCounter), tamperCounterGeneralEntity.CTOpenRPhaseTamperCounter, DbType.Int32);
            request.AddParamter(ParameterName(CTOpenYPhaseTamperCounter), tamperCounterGeneralEntity.CTOpenYPhaseTamperCounter, DbType.Int32);
            request.AddParamter(ParameterName(CTOpenBPhaseTamperCounter), tamperCounterGeneralEntity.CTOpenBPhaseTamperCounter, DbType.Int32);
              request.AddParamter(ParameterName(OnePhaseNeutralAbsentTamperCounter), tamperCounterGeneralEntity.OnePhaseNeutralAbsentTamperCounter, DbType.Int32);
            request.AddParamter(ParameterName(VoltagePhaseReversalTamperCounter), tamperCounterGeneralEntity.VoltagePhaseReversalTamperCounter, DbType.Int32);
            request.AddParamter(ParameterName(CurrentImbalanceRPhaseTamperCounter), tamperCounterGeneralEntity.CurrentImbalanceRPhaseTamperCounter, DbType.Int32);
            request.AddParamter(ParameterName(CurrentImbalanceYPhaseTamperCounter), tamperCounterGeneralEntity.CurrentImbalanceYPhaseTamperCounter, DbType.Int32);
            request.AddParamter(ParameterName(CurrentImbalanceBPhaseTamperCounter), tamperCounterGeneralEntity.CurrentImbalanceBPhaseTamperCounter, DbType.Int32);
            request.AddParamter(ParameterName(CurrentReversalRPhaseTamperCounter), tamperCounterGeneralEntity.CurrentReversalRPhaseTamperCounter, DbType.Int32);
            request.AddParamter(ParameterName(CurrentReversalYPhaseTamperCounter), tamperCounterGeneralEntity.CurrentReversalYPhaseTamperCounter, DbType.Int32);
            request.AddParamter(ParameterName(CurrentReversalBPhaseTamperCounter), tamperCounterGeneralEntity.CurrentReversalBPhaseTamperCounter, DbType.Int32);
            request.AddParamter(ParameterName(MagneticInfluenceTamperCounter), tamperCounterGeneralEntity.MagneticInfluenceTamperCounter, DbType.Int32);
            request.AddParamter(ParameterName(NeutralDisturbanceTamperCounter), tamperCounterGeneralEntity.NeutralDisturbanceTamperCounter, DbType.Int32);
            request.AddParamter(ParameterName(FrontCoverOpeningTamperCounter), tamperCounterGeneralEntity.FrontCoverOpeningTamperCounter, DbType.Int32); 
            request.AddParamter(ParameterName(History_ID), tamperCounterGeneralEntity.History_ID, DbType.Int64);
            request.AddParamter(ParameterName(MeterData_ID), tamperCounterGeneralEntity.MeterData_ID, DbType.Int64);
            request.AddParamter(ParameterName(RelatedTo), tamperCounterGeneralEntity.RelatedTo, DbType.String, 2);
            request.AddParamter(ParameterName(BillingTimeStamp), tamperCounterGeneralEntity.BillingTimeStamp, DbType.Int64);
            request.AddParamter(ParameterName(BillingCounter), tamperCounterGeneralEntity.BillingCounter, DbType.Int64);
            
            return request;
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
                builder.Append("Delete from MeterData_TamperCounterGeneral where ");
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

        public override bool DeleteData(IEntity entity)
        {
            TamperCounterGeneralEntity tamperCounterGeneralEntity = entity as TamperCounterGeneralEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete from MeterData_TamperCounterGeneral where ");
                builder.Append(string.Concat(TamperCounterGeneral_ID, "=", ParameterName(TamperCounterGeneral_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(TamperCounterGeneral_ID), tamperCounterGeneralEntity.TamperCounterGeneral_ID, DbType.UInt32);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data Tamper Counter General deleted"));
                Flag = true;
            }
            catch (Exception) { }
            return Flag;
        }

        public override IEntity GetDetailData(int id)
        {
            TamperCounterGeneralEntity tamperCounterGeneralEntity = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select TamperCounterGeneral_ID,VoltageImbalanceRPhaseTamperCounter,VoltageImbalanceYPhaseTamperCounter,VoltageImbalanceBPhaseTamperCounter,MissingPotentialRPhaseTamperCounter,MissingPotentialYPhaseTamperCounter,MissingPotentialBPhaseTamperCounter,LowUnderVoltageRPhaseTamperCounter,LowUnderVoltageYPhaseTamperCounter,LowUnderVoltageBPhaseTamperCounter,HighOverVoltageRPhaseTamperCounter,HighOverVoltageYPhaseTamperCounter,HighOverVoltageBPhaseTamperCounter,CTShortTamperCounter,CTOpenRPhaseTamperCounter,CTOpenYPhaseTamperCounter,CTOpenBPhaseTamperCounter,CurrentWithoutVoltageRPhaseTamperCounter,CurrentWithoutVoltageYPhaseTamperCounter,CurrentWithoutVoltageBPhaseTamperCounter,LowPowerFactorRPhaseTamperCounter,LowPowerFactorYPhaseTamperCounter,LowPowerFactorBPhaseTamperCounter,OnePhaseNeutralAbsentTamperCounter,CurrentPhaseReversalTamperCounter,VoltagePhaseReversalTamperCounter,CurrentImbalanceRPhaseTamperCounter,CurrentImbalanceYPhaseTamperCounter,CurrentImbalanceBPhaseTamperCounter from MeterData_TamperCounterGeneral where ");
                builder.Append(string.Concat(TamperCounterGeneral_ID, "=", ParameterName(TamperCounterGeneral_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(TamperCounterGeneral_ID), id, DbType.UInt32);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    tamperCounterGeneralEntity = (TamperCounterGeneralEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data Tamper Counter General viewed"));
            }
            catch (CABException)
            {
            }
            return tamperCounterGeneralEntity;
        }

		//The Entity Data for the tamper counter General Entity
		public DataSet ListDataSet(int meterDataID)
		{
			DataSet ds = null;
			try
			{
				IDataHelper helper = DatabaseFactory.GetHelper();
				StringBuilder builder = new StringBuilder();
                builder.Append("Select A.MeterID,B.TamperCounterGeneral_ID,B.VoltageImbalanceRPhaseTamperCounter,B.VoltageImbalanceYPhaseTamperCounter,B.VoltageImbalanceBPhaseTamperCounter,B.MissingPotentialRPhaseTamperCounter,B.MissingPotentialYPhaseTamperCounter,B.MissingPotentialBPhaseTamperCounter,B.LowUnderVoltageRPhaseTamperCounter,B.LowUnderVoltageYPhaseTamperCounter,B.LowUnderVoltageBPhaseTamperCounter,B.HighOverVoltageRPhaseTamperCounter,B.HighOverVoltageYPhaseTamperCounter,B.HighOverVoltageBPhaseTamperCounter,B.CTShortTamperCounter,B.CTOpenRPhaseTamperCounter,B.CTOpenYPhaseTamperCounter,B.CTOpenBPhaseTamperCounter,B.CurrentWithoutVoltageRPhaseTamperCounter,B.CurrentWithoutVoltageYPhaseTamperCounter,B.CurrentWithoutVoltageBPhaseTamperCounter,B.LowPowerFactorRPhaseTamperCounter,B.LowPowerFactorYPhaseTamperCounter,B.LowPowerFactorBPhaseTamperCounter,B.OnePhaseNeutralAbsentTamperCounter,B.CurrentPhaseReversalTamperCounter,B.VoltagePhaseReversalTamperCounter,B.CurrentImbalanceRPhaseTamperCounter,B.CurrentImbalanceYPhaseTamperCounter,B.CurrentImbalanceBPhaseTamperCounter,B.CurrentReversalRPhaseTamperCounter,B.CurrentReversalYPhaseTamperCounter,B.CurrentReversalBPhaseTamperCounter,B.MagneticInfluenceTamperCounter,B.NeutralDisturbanceTamperCounter,B.FrontCoverOpeningTamperCounter,B.History_ID,B.MeterData_ID,B.BillingTimeStamp,B.BillingCounter,B.RelatedTo from MeterData_TamperCounterGeneral B Inner Join meterdata A on B.MeterData_ID = A.MeterData_ID where ");
				builder.Append(string.Concat("B.",MeterData_ID, "=", ParameterName(MeterData_ID)));
				DataRequest request = new DataRequest(builder.ToString());
				request.AddParamter(ParameterName(MeterData_ID), meterDataID, DbType.UInt32);
				ds = new DataSet();
				ds = helper.FillDataSet(request, ds);
				if (ds.Tables[0].Rows.Count > 0)
					return ds;
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data Tamper Counter General viewed"));
            }
			catch (CABException)
			{
			}
			return ds;
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
                builder.Append("Select TamperCounterGeneral_ID,VoltageImbalanceRPhaseTamperCounter,VoltageImbalanceYPhaseTamperCounter,VoltageImbalanceBPhaseTamperCounter,MissingPotentialRPhaseTamperCounter,MissingPotentialYPhaseTamperCounter,MissingPotentialBPhaseTamperCounter,LowUnderVoltageRPhaseTamperCounter,LowUnderVoltageYPhaseTamperCounter,LowUnderVoltageBPhaseTamperCounter,HighOverVoltageRPhaseTamperCounter,HighOverVoltageYPhaseTamperCounter,HighOverVoltageBPhaseTamperCounter,CTShortTamperCounter,CTOpenRPhaseTamperCounter,CTOpenYPhaseTamperCounter,CTOpenBPhaseTamperCounter,CurrentWithoutVoltageRPhaseTamperCounter,CurrentWithoutVoltageYPhaseTamperCounter,CurrentWithoutVoltageBPhaseTamperCounter,LowPowerFactorRPhaseTamperCounter,LowPowerFactorYPhaseTamperCounter,LowPowerFactorBPhaseTamperCounter,OnePhaseNeutralAbsentTamperCounter,CurrentPhaseReversalTamperCounter,VoltagePhaseReversalTamperCounter,CurrentImbalanceRPhaseTamperCounter,CurrentImbalanceYPhaseTamperCounter,MeterData_ID,RelatedTo,BillingTimeStamp,BillingCounter from MeterData_TamperCounterGeneral ");
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data Tamper Counter General viewed"));
            }
            catch (CABException)
            {
            }
            return dataSet;
        }

        public override IEntity RowToEntity(DataRow row)
        {
            if (row == null) return null;
            TamperCounterGeneralEntity tamperCounterGeneralEntity = new TamperCounterGeneralEntity();
            if (NotNullAndNotDBNull(row, TamperCounterGeneral_ID)) tamperCounterGeneralEntity.TamperCounterGeneral_ID = Convert.ToInt64(row[TamperCounterGeneral_ID]);
            if (NotNullAndNotDBNull(row, VoltageImbalanceRPhaseTamperCounter)) tamperCounterGeneralEntity.VoltageImbalanceRPhaseTamperCounter = Convert.ToInt32(row[VoltageImbalanceRPhaseTamperCounter]);
            if (NotNullAndNotDBNull(row, VoltageImbalanceYPhaseTamperCounter)) tamperCounterGeneralEntity.VoltageImbalanceYPhaseTamperCounter = Convert.ToInt32(row[VoltageImbalanceYPhaseTamperCounter]);
            if (NotNullAndNotDBNull(row, VoltageImbalanceBPhaseTamperCounter)) tamperCounterGeneralEntity.VoltageImbalanceBPhaseTamperCounter = Convert.ToInt32(row[VoltageImbalanceBPhaseTamperCounter]);
            if (NotNullAndNotDBNull(row, MissingPotentialRPhaseTamperCounter)) tamperCounterGeneralEntity.MissingPotentialRPhaseTamperCounter = Convert.ToInt32(row[MissingPotentialRPhaseTamperCounter]);
            if (NotNullAndNotDBNull(row, MissingPotentialYPhaseTamperCounter)) tamperCounterGeneralEntity.MissingPotentialYPhaseTamperCounter = Convert.ToInt32(row[MissingPotentialYPhaseTamperCounter]);
            if (NotNullAndNotDBNull(row, MissingPotentialBPhaseTamperCounter)) tamperCounterGeneralEntity.MissingPotentialBPhaseTamperCounter = Convert.ToInt32(row[MissingPotentialBPhaseTamperCounter]);
                  if (NotNullAndNotDBNull(row, CTShortTamperCounter)) tamperCounterGeneralEntity.CTShortTamperCounter = Convert.ToInt32(row[CTShortTamperCounter]);
            if (NotNullAndNotDBNull(row, CTOpenRPhaseTamperCounter)) tamperCounterGeneralEntity.CTOpenRPhaseTamperCounter = Convert.ToInt32(row[CTOpenRPhaseTamperCounter]);
            if (NotNullAndNotDBNull(row, CTOpenYPhaseTamperCounter)) tamperCounterGeneralEntity.CTOpenYPhaseTamperCounter = Convert.ToInt32(row[CTOpenYPhaseTamperCounter]);
            if (NotNullAndNotDBNull(row, CTOpenBPhaseTamperCounter)) tamperCounterGeneralEntity.CTOpenBPhaseTamperCounter = Convert.ToInt32(row[CTOpenBPhaseTamperCounter]);
                if (NotNullAndNotDBNull(row, OnePhaseNeutralAbsentTamperCounter)) tamperCounterGeneralEntity.OnePhaseNeutralAbsentTamperCounter = Convert.ToInt32(row[OnePhaseNeutralAbsentTamperCounter]);
             if (NotNullAndNotDBNull(row, VoltagePhaseReversalTamperCounter)) tamperCounterGeneralEntity.VoltagePhaseReversalTamperCounter = Convert.ToInt32(row[VoltagePhaseReversalTamperCounter]);
            if (NotNullAndNotDBNull(row, CurrentImbalanceRPhaseTamperCounter)) tamperCounterGeneralEntity.CurrentImbalanceRPhaseTamperCounter = Convert.ToInt32(row[CurrentImbalanceRPhaseTamperCounter]);
            if (NotNullAndNotDBNull(row, CurrentImbalanceYPhaseTamperCounter)) tamperCounterGeneralEntity.CurrentImbalanceYPhaseTamperCounter = Convert.ToInt32(row[CurrentImbalanceYPhaseTamperCounter]);
            if (NotNullAndNotDBNull(row, CurrentImbalanceBPhaseTamperCounter)) tamperCounterGeneralEntity.CurrentImbalanceBPhaseTamperCounter = Convert.ToInt32(row[CurrentImbalanceBPhaseTamperCounter]);
            if (NotNullAndNotDBNull(row, CurrentReversalRPhaseTamperCounter)) tamperCounterGeneralEntity.CurrentReversalRPhaseTamperCounter = Convert.ToInt32(row[CurrentReversalRPhaseTamperCounter]);
            if (NotNullAndNotDBNull(row, CurrentReversalYPhaseTamperCounter)) tamperCounterGeneralEntity.CurrentReversalYPhaseTamperCounter = Convert.ToInt32(row[CurrentReversalYPhaseTamperCounter]);
            if (NotNullAndNotDBNull(row, CurrentReversalBPhaseTamperCounter)) tamperCounterGeneralEntity.CurrentReversalBPhaseTamperCounter = Convert.ToInt32(row[CurrentReversalBPhaseTamperCounter]);
            if (NotNullAndNotDBNull(row, MagneticInfluenceTamperCounter)) tamperCounterGeneralEntity.MagneticInfluenceTamperCounter = Convert.ToInt32(row[MagneticInfluenceTamperCounter]);
            if (NotNullAndNotDBNull(row, NeutralDisturbanceTamperCounter)) tamperCounterGeneralEntity.NeutralDisturbanceTamperCounter = Convert.ToInt32(row[NeutralDisturbanceTamperCounter]);
            if (NotNullAndNotDBNull(row, FrontCoverOpeningTamperCounter)) tamperCounterGeneralEntity.FrontCoverOpeningTamperCounter = Convert.ToInt32(row[FrontCoverOpeningTamperCounter]);
            if (NotNullAndNotDBNull(row, History_ID)) tamperCounterGeneralEntity.History_ID = Convert.ToInt64(row[History_ID]);
            if (NotNullAndNotDBNull(row, MeterData_ID)) tamperCounterGeneralEntity.MeterData_ID = Convert.ToInt64(row[MeterData_ID]);
            if (NotNullAndNotDBNull(row, BillingTimeStamp)) tamperCounterGeneralEntity.BillingTimeStamp = Convert.ToInt64(row[BillingTimeStamp]);
            if (NotNullAndNotDBNull(row, BillingCounter)) tamperCounterGeneralEntity.BillingCounter = Convert.ToInt64(row[BillingCounter]);
            if (NotNullAndNotDBNull(row, RelatedTo)) tamperCounterGeneralEntity.RelatedTo = Convert.ToString(row[RelatedTo]);
          
            return tamperCounterGeneralEntity;
        }
        public override IEntity InsertData(IEntity entity)
        {
            TamperCounterGeneralEntity tamperCounterGeneralEntity = entity as TamperCounterGeneralEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                helper.ExecuteNonQuery(this.GetRequest(entity));
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data Tamper Counter General added"));
                Flag = true;
            }
            catch (Exception) { }
            if (Flag)
                tamperCounterGeneralEntity.TamperCounterGeneral_ID = long.Parse(this.GetPK());
            return tamperCounterGeneralEntity;
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
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data Tamper Counter General added"));
            }
            catch (Exception) { }
            return null;
        }
    }
}
