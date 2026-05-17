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
     public class InstantPowerDAL : DALBase
    {
         private string InstantPower_ID = "InstantPower_ID";
         private string MeterID = "MeterID";
         private string MeterDateTime = "MeterDateTime";
         private string VoltageRPhase = "VoltageRPhase";
         private string VoltageYPhase = "VoltageYPhase"; 
         private string VoltageBPhase = "VoltageBPhase";
         private string CurrentRPhase = "CurrentRPhase";
         private string CurrentYPhase = "CurrentYPhase";
         private string CurrentBPhase = "CurrentBPhase";
         private string InstantActivepower = "InstantActivepower"; 
         private string InstantReactiveLagPower = "InstantReactiveLagPower";
         private string InstantReactiveLeadPower = "InstantReactiveLeadPower";
         private string InstantApparentPower = "InstantApparentPower";
         private string TotalPowerFactor = "TotalPowerFactor";
         private string PowerFactorRPhase = "PowerFactorRPhase"; 
         private string PowerFactorYPhase = "PowerFactorYPhase";
         private string PowerFactorBPhase = "PowerFactorBPhase"; 
         private string Frequency = "Frequency";
         private string InstantActivepowerRPhase = "InstantActivepowerRPhase";
         private string InstantActivepowerYPhase = "InstantActivepowerYPhase";
         private string InstantActivepowerBPhase = "InstantActivepowerBPhase";
         private string InstantReactivepowerRPhase = "InstantReactivepowerRPhase";
         private string InstantReactivepowerYPhase = "InstantReactivepowerYPhase";
         private string InstantReactivepowerBPhase = "InstantReactivepowerBPhase";
         private string InstantApparentpowerRPhase = "InstantApparentpowerRPhase";
         private string InstantApparentpowerYPhase = "InstantApparentpowerYPhase";
         private string InstantApparentpowerBPhase = "InstantApparentpowerBPhase";
         private string TotalFundamentalActiveEnergy = "TotalFundamentalActiveEnergy";
         private string MeterData_ID = "MeterData_ID";

         private string FileName = "FileName";

        public InstantPowerDAL()
             : base("meterdata_instantpower", "InstantPower_ID")
        {
        }


        public override IEntity InsertData(IEntity entity)
        {
            InstantPowerEntity instantPowerEntity = entity as InstantPowerEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert Into meterdata_instantpower(MeterID,MeterDateTime,VoltageRPhase,VoltageYPhase,VoltageBPhase,CurrentRPhase,CurrentYPhase,CurrentBPhase,InstantActivepower,InstantReactiveLagPower,InstantReactiveLeadPower,InstantApparentPower,TotalPowerFactor,PowerFactorRPhase,PowerFactorYPhase,PowerFactorBPhase,Frequency,TotalFundamentalActiveEnergy,InstantActivepowerRPhase,InstantActivepowerYPhase,InstantActivepowerBPhase,InstantReactivepowerRPhase,InstantReactivepowerYPhase,InstantReactivepowerBPhase,InstantApparentpowerRPhase,InstantApparentpowerYPhase,InstantApparentpowerBPhase,MeterData_ID) values(");
                builder.Append(string.Concat(ParameterName(MeterID), ","));
                builder.Append(string.Concat(ParameterName(MeterDateTime), ","));
                builder.Append(string.Concat(ParameterName(VoltageRPhase), ","));
                builder.Append(string.Concat(ParameterName(VoltageYPhase), ","));
                builder.Append(string.Concat(ParameterName(VoltageBPhase), ","));
                builder.Append(string.Concat(ParameterName(CurrentRPhase), ","));
                builder.Append(string.Concat(ParameterName(CurrentYPhase), ","));
                builder.Append(string.Concat(ParameterName(CurrentBPhase), ","));
                builder.Append(string.Concat(ParameterName(InstantActivepower), ","));
                builder.Append(string.Concat(ParameterName(InstantReactiveLagPower), ","));
                builder.Append(string.Concat(ParameterName(InstantReactiveLeadPower), ","));
                builder.Append(string.Concat(ParameterName(InstantApparentPower), ","));
                builder.Append(string.Concat(ParameterName(TotalPowerFactor), ","));
                builder.Append(string.Concat(ParameterName(PowerFactorRPhase), ","));
                builder.Append(string.Concat(ParameterName(PowerFactorYPhase), ","));
                builder.Append(string.Concat(ParameterName(PowerFactorBPhase), ",")); 
                builder.Append(string.Concat(ParameterName(Frequency), ","));
                builder.Append(string.Concat(ParameterName(TotalFundamentalActiveEnergy), ","));
                builder.Append(string.Concat(ParameterName(InstantActivepowerRPhase), ","));
                builder.Append(string.Concat(ParameterName(InstantActivepowerYPhase), ","));
                builder.Append(string.Concat(ParameterName(InstantActivepowerBPhase), ","));
                builder.Append(string.Concat(ParameterName(InstantReactivepowerRPhase), ","));
                builder.Append(string.Concat(ParameterName(InstantReactivepowerYPhase), ","));
                builder.Append(string.Concat(ParameterName(InstantReactivepowerBPhase), ","));
                builder.Append(string.Concat(ParameterName(InstantApparentpowerRPhase), ","));
                builder.Append(string.Concat(ParameterName(InstantApparentpowerYPhase), ","));
                builder.Append(string.Concat(ParameterName(InstantApparentpowerBPhase), ","));
                builder.Append(string.Concat(ParameterName(MeterData_ID), ")"));
                DataRequest request = new DataRequest(builder.ToString()); 
                request.AddParamter(ParameterName(MeterID), instantPowerEntity.MeterID, DbType.String, 20);
                request.AddParamter(ParameterName(MeterDateTime), instantPowerEntity.MeterDateTime, DbType.Int64);
                request.AddParamter(ParameterName(VoltageRPhase), instantPowerEntity.VoltageRPhase, DbType.String, 40);
                request.AddParamter(ParameterName(VoltageYPhase), instantPowerEntity.VoltageYPhase, DbType.String, 40);
                request.AddParamter(ParameterName(VoltageBPhase), instantPowerEntity.VoltageBPhase, DbType.String, 40);
                request.AddParamter(ParameterName(CurrentRPhase), instantPowerEntity.CurrentRPhase, DbType.String, 40);
                request.AddParamter(ParameterName(CurrentYPhase), instantPowerEntity.CurrentYPhase, DbType.String, 40);
                request.AddParamter(ParameterName(CurrentBPhase), instantPowerEntity.CurrentBPhase, DbType.String, 40);
                request.AddParamter(ParameterName(InstantActivepower), instantPowerEntity.InstantActivepower, DbType.String, 40);
                request.AddParamter(ParameterName(InstantReactiveLagPower), instantPowerEntity.InstantReactiveLagPower, DbType.String, 40);
                request.AddParamter(ParameterName(InstantReactiveLeadPower), instantPowerEntity.InstantReactiveLeadPower, DbType.String, 40);
                request.AddParamter(ParameterName(InstantApparentPower), instantPowerEntity.InstantApparentPower, DbType.String, 40);
                request.AddParamter(ParameterName(TotalPowerFactor), instantPowerEntity.TotalPowerFactor, DbType.String, 40);
                request.AddParamter(ParameterName(PowerFactorRPhase), instantPowerEntity.PowerFactorRPhase, DbType.String, 40);
                request.AddParamter(ParameterName(PowerFactorYPhase), instantPowerEntity.PowerFactorYPhase, DbType.String, 40);
                request.AddParamter(ParameterName(PowerFactorBPhase), instantPowerEntity.PowerFactorBPhase, DbType.String, 40); 
                request.AddParamter(ParameterName(Frequency), instantPowerEntity.Frequency, DbType.String, 40);
                request.AddParamter(ParameterName(TotalFundamentalActiveEnergy), instantPowerEntity.TotalFundamentalActiveEnergy, DbType.String, 40);

                request.AddParamter(ParameterName(InstantActivepowerRPhase), instantPowerEntity.InstantActivepowerRPhase, DbType.String, 40);
                request.AddParamter(ParameterName(InstantActivepowerYPhase), instantPowerEntity.InstantActivepowerYPhase, DbType.String, 40);
                request.AddParamter(ParameterName(InstantActivepowerBPhase), instantPowerEntity.InstantActivepowerBPhase, DbType.String, 40);
                request.AddParamter(ParameterName(InstantReactivepowerRPhase), instantPowerEntity.InstantReactivepowerRPhase, DbType.String, 40);
                request.AddParamter(ParameterName(InstantReactivepowerYPhase), instantPowerEntity.InstantReactivepowerYPhase, DbType.String, 40);
                request.AddParamter(ParameterName(InstantReactivepowerBPhase), instantPowerEntity.InstantReactivepowerBPhase, DbType.String, 40);
                request.AddParamter(ParameterName(InstantApparentpowerRPhase), instantPowerEntity.InstantApparentpowerRPhase, DbType.String, 40);
                request.AddParamter(ParameterName(InstantApparentpowerYPhase), instantPowerEntity.InstantApparentpowerYPhase, DbType.String, 40);
                request.AddParamter(ParameterName(InstantApparentpowerBPhase), instantPowerEntity.InstantApparentpowerBPhase, DbType.String, 40);

                request.AddParamter(ParameterName(MeterData_ID), instantPowerEntity.MeterData_ID, DbType.Int64);

                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data Instant Power added"));
                Flag = true;
            }
            catch (Exception) { }
            if (Flag)
                instantPowerEntity.InstantPower_ID = long.Parse(this.GetPK());
            return instantPowerEntity;
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
                builder.Append("Delete from meterdata_instantpower where ");
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
            InstantPowerEntity instantPowerEntity = entity as InstantPowerEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete from meterdata_instantpower where ");
                builder.Append(string.Concat(InstantPower_ID, "=", ParameterName(InstantPower_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(InstantPower_ID), instantPowerEntity.InstantPower_ID, DbType.UInt32);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data Instant Power deleted"));
                Flag = true;
            }
            catch (Exception) { }
            return Flag;
        }

        public override IEntity GetDetailData(int id)
        {
            InstantPowerEntity instantPowerEntity = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select InstantPower_ID,MeterID,MeterDateTime,VoltageRPhase,VoltageYPhase,VoltageBPhase,CurrentRPhase,CurrentYPhase,CurrentBPhase,InstantActivepower,InstantReactiveLagPower,InstantReactiveLeadPower,InstantApparentPower,TotalPowerFactor,PowerFactorRPhase,PowerFactorYPhase,PowerFactorBPhase,Frequency,TotalFundamentalActiveEnergy,InstantActivepowerRPhase,InstantActivepowerYPhase,InstantActivepowerBPhase,InstantReactivepowerRPhase,InstantReactivepowerYPhase,InstantReactivepowerBPhase,InstantApparentpowerRPhase,InstantApparentpowerYPhase,InstantApparentpowerBPhase,MeterData_ID from meterdata_instantpower where ");
                builder.Append(string.Concat(InstantPower_ID, "=", ParameterName(InstantPower_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(InstantPower_ID), id, DbType.UInt32);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    instantPowerEntity = (InstantPowerEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Instant Power data viewed"));

            }
            catch (CABException)
            { 
            }
            return instantPowerEntity;
        }

		public IEntity GetEntityData(int meterDataID)
		{
			InstantPowerEntity instantPowerEntity = null;
			try
			{
				IDataHelper helper = DatabaseFactory.GetHelper();
				StringBuilder builder = new StringBuilder();
                builder.Append("Select InstantPower_ID,MeterID,MeterDateTime,VoltageRPhase,VoltageYPhase,VoltageBPhase,CurrentRPhase,CurrentYPhase,CurrentBPhase,InstantActivepower,InstantReactiveLagPower,InstantReactiveLeadPower,InstantApparentPower,TotalPowerFactor,PowerFactorRPhase,PowerFactorYPhase,PowerFactorBPhase,Frequency,TotalFundamentalActiveEnergy,InstantActivepowerRPhase,InstantActivepowerYPhase,InstantActivepowerBPhase,InstantReactivepowerRPhase,InstantReactivepowerYPhase,InstantReactivepowerBPhase,InstantApparentpowerRPhase,InstantApparentpowerYPhase,InstantApparentpowerBPhase,MeterData_ID from meterdata_instantpower where ");
				builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
				DataRequest request = new DataRequest(builder.ToString());
				request.AddParamter(ParameterName(MeterData_ID), meterDataID, DbType.UInt32);
				DataSet ds = new DataSet();
				ds = helper.FillDataSet(request, ds);
				if (ds.Tables[0].Rows.Count > 0)
					instantPowerEntity = (InstantPowerEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Instant Power data viewed"));

			}
			catch (CABException)
			{
			}
			return instantPowerEntity;
		}

        public override IList<IEntity> ListData()
        {
            throw new NotImplementedException();
        }

        public DataSet GetMeterData(int meterDataId)
        {
            DataSet dataSet = new DataSet();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select A.VoltageRPhase as 'Voltage R Phase',");
                builder.Append("A.VoltageYPhase as 'Voltage Y Phase',");
                builder.Append("A.VoltageBPhase as 'Voltage B Phase',");
                builder.Append("A.CurrentRPhase as 'Current R Phase',");
                builder.Append("A.CurrentYPhase as 'Current Y Phase',");
                builder.Append("A.CurrentBPhase as 'Current B Phase',");
                builder.Append("A.InstantActivepower as 'Active power',");
                builder.Append("A.InstantApparentPower as 'Apparent power',");
                builder.Append("A.InstantReactiveLagPower as 'Reactive power (Lag)',");
                builder.Append("A.InstantReactiveLeadPower as 'Reactive power (Lead)',");
                builder.Append("A.TotalPowerFactor as 'Total Power Factor',");
                builder.Append("A.PowerFactorRPhase as 'Power Factor R Phase',");
                builder.Append("A.PowerFactorYPhase as 'Power Factor Y Phase',");
                builder.Append("A.PowerFactorBPhase as 'Power Factor B Phase',"); 
                builder.Append("A.Frequency as 'Frequency',");
                builder.Append("A.InstantActivepowerRPhase as 'Instant Active power R Phase',");
                builder.Append("A.InstantActivepowerYPhase as 'Instant Active power Y Phase',");
                builder.Append("A.InstantActivepowerBPhase as 'Instant Active power B Phase',");
                builder.Append("A.InstantReactivepowerRPhase as 'Instant Reactive power R Phase',");
                builder.Append("A.InstantReactivepowerYPhase as 'Instant Reactive power Y Phase',");
                builder.Append("A.InstantReactivepowerBPhase as 'Instant Reactive power B Phase',");
                builder.Append("A.InstantApparentpowerRPhase as 'Instant Apparent power R Phase',");
                builder.Append("A.InstantApparentpowerYPhase as 'Instant Apparent power Y Phase',");
                builder.Append("A.InstantApparentpowerBPhase as 'Instant Apparent power B Phase',");
                 builder.Append("B.RisingDemandKW as 'Rising Demand kW',");
                builder.Append("B.ElapsedTimeKW as 'Elapsed Time kW',");
                builder.Append("B.RisingDemandKVA as 'Rising Demand kVA',");
                builder.Append("B.ElapsedTimeKVA as 'ElapsedTime kVA'");
                builder.Append(" from meterdata_instantpower A, meterdata_general B where ");
                builder.Append("A.MeterData_ID=B.MeterData_ID and A.");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataId, DbType.UInt32);
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Instant Power data viewed"));
            }
            catch (CABException)
            {
                dataSet = null;
            }
            return dataSet;
        }
        public override DataSet ListDataSet()
        {
            DataSet dataSet = null;
            try  
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select InstantPower_ID,MeterID,MeterDateTime,VoltageRPhase,VoltageYPhase,VoltageBPhase,CurrentRPhase,CurrentYPhase,CurrentBPhase,InstantActivepower,InstantReactiveLagPower,InstantReactiveLeadPower,InstantApparentPower,TotalPowerFactor,PowerFactorRPhase,PowerFactorYPhase,PowerFactorBPhase,Frequency,TotalFundamentalActiveEnergy,InstantActivepowerRPhase,InstantActivepowerYPhase,InstantActivepowerBPhase,InstantReactivepowerRPhase,InstantReactivepowerYPhase,InstantReactivepowerBPhase,InstantApparentpowerRPhase,InstantApparentpowerYPhase,InstantApparentpowerBPhase,MeterData_ID from meterdata_instantpower");
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Instant Power data viewed"));
            }
            catch (CABException)
            {
            }
            return dataSet;
        }

        public override IEntity RowToEntity(DataRow row)
        { 
            if (row == null) return null;
            InstantPowerEntity instantPowerEntity = new InstantPowerEntity(); 
            if (NotNullAndNotDBNull(row,InstantPower_ID )) instantPowerEntity.InstantPower_ID = Convert.ToInt32(row[InstantPower_ID]);
            if (NotNullAndNotDBNull(row,MeterDateTime )) instantPowerEntity.MeterDateTime = Convert.ToInt64(row[MeterDateTime]);
            if (NotNullAndNotDBNull(row, MeterID)) instantPowerEntity.MeterID = Convert.ToString(row[MeterID]);
            if (NotNullAndNotDBNull(row,VoltageRPhase )) instantPowerEntity.VoltageRPhase = Convert.ToString(row[VoltageRPhase]);
            if (NotNullAndNotDBNull(row,VoltageYPhase )) instantPowerEntity.VoltageYPhase = Convert.ToString(row[VoltageYPhase]);
            if (NotNullAndNotDBNull(row,VoltageBPhase )) instantPowerEntity.VoltageBPhase = Convert.ToString(row[VoltageBPhase]);
            if (NotNullAndNotDBNull(row,CurrentRPhase )) instantPowerEntity.CurrentRPhase = Convert.ToString(row[CurrentRPhase]);
            if (NotNullAndNotDBNull(row,CurrentYPhase )) instantPowerEntity.CurrentYPhase = Convert.ToString(row[CurrentYPhase]);
            if (NotNullAndNotDBNull(row,CurrentBPhase )) instantPowerEntity.CurrentBPhase = Convert.ToString(row[CurrentBPhase]);
            if (NotNullAndNotDBNull(row,InstantActivepower )) instantPowerEntity.InstantActivepower = Convert.ToString(row[InstantActivepower]);
            if (NotNullAndNotDBNull(row,InstantReactiveLagPower )) instantPowerEntity.InstantReactiveLagPower = Convert.ToString(row[InstantReactiveLagPower]);
            if (NotNullAndNotDBNull(row,InstantReactiveLeadPower )) instantPowerEntity.InstantReactiveLeadPower = Convert.ToString(row[InstantReactiveLeadPower]);
            if (NotNullAndNotDBNull(row,InstantApparentPower )) instantPowerEntity.InstantApparentPower = Convert.ToString(row[InstantApparentPower]);
            if (NotNullAndNotDBNull(row,TotalPowerFactor )) instantPowerEntity.TotalPowerFactor = Convert.ToString(row[TotalPowerFactor]);
            if (NotNullAndNotDBNull(row, PowerFactorRPhase)) instantPowerEntity.PowerFactorRPhase = Convert.ToString(row[PowerFactorRPhase]);
            if (NotNullAndNotDBNull(row,PowerFactorYPhase )) instantPowerEntity.PowerFactorYPhase = Convert.ToString(row[PowerFactorYPhase]);
            if (NotNullAndNotDBNull(row,PowerFactorBPhase )) instantPowerEntity.PowerFactorBPhase = Convert.ToString(row[PowerFactorBPhase]);
            if (NotNullAndNotDBNull(row,Frequency )) instantPowerEntity.Frequency = Convert.ToString(row[Frequency]);
            if (NotNullAndNotDBNull(row,TotalFundamentalActiveEnergy )) instantPowerEntity.TotalFundamentalActiveEnergy = Convert.ToString(row[TotalFundamentalActiveEnergy]);

            if (NotNullAndNotDBNull(row, InstantActivepowerRPhase)) instantPowerEntity.InstantActivepowerRPhase = Convert.ToString(row[InstantActivepowerRPhase]);
            if (NotNullAndNotDBNull(row, InstantActivepowerYPhase)) instantPowerEntity.InstantActivepowerYPhase = Convert.ToString(row[InstantActivepowerYPhase]);
            if (NotNullAndNotDBNull(row, InstantActivepowerBPhase)) instantPowerEntity.InstantActivepowerBPhase = Convert.ToString(row[InstantActivepowerBPhase]);
            if (NotNullAndNotDBNull(row, InstantReactivepowerRPhase)) instantPowerEntity.InstantReactivepowerRPhase = Convert.ToString(row[InstantReactivepowerRPhase]);
            if (NotNullAndNotDBNull(row, InstantReactivepowerYPhase)) instantPowerEntity.InstantReactivepowerYPhase = Convert.ToString(row[InstantReactivepowerYPhase]);
            if (NotNullAndNotDBNull(row, InstantReactivepowerBPhase)) instantPowerEntity.InstantReactivepowerBPhase = Convert.ToString(row[InstantReactivepowerBPhase]);
            if (NotNullAndNotDBNull(row, InstantApparentpowerRPhase)) instantPowerEntity.InstantApparentpowerRPhase = Convert.ToString(row[InstantApparentpowerRPhase]);
            if (NotNullAndNotDBNull(row, InstantApparentpowerYPhase)) instantPowerEntity.InstantApparentpowerYPhase = Convert.ToString(row[InstantApparentpowerYPhase]);
            if (NotNullAndNotDBNull(row, InstantApparentpowerBPhase)) instantPowerEntity.InstantApparentpowerBPhase = Convert.ToString(row[InstantApparentpowerBPhase]);
            
            if (NotNullAndNotDBNull(row,MeterData_ID )) instantPowerEntity.MeterData_ID = Convert.ToInt32(row[MeterData_ID]); 
            return instantPowerEntity;
        }

        public override IEntity InsertData(IList<IEntity> entities)
        {
            throw new NotImplementedException();
        }

        public DataSet GetInstantDataByFileName(string fileName, List<string> columns)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select i.MeterID ");
                foreach (string column in columns)
                {
                    //builder.Append(string.Concat(",", "i.", column, " "));
                    builder.Append(string.Concat(",", column, " "));
                }
                builder.Append("from meterdata_instantpower i inner join meterdata m on i.MeterData_ID = m.MeterData_ID ");
                builder.Append("inner join meterdata_general g on i.MeterData_ID = g.MeterData_ID ");
                builder.Append("inner join fileupload_master f on m.FileUpload_ID = f.FileUpload_ID where ");
                builder.Append(string.Concat("f.", FileName, "=", ParameterName(FileName)));
                DataRequest request = new DataRequest(builder.ToString());
				request.AddParamter(ParameterName(FileName), fileName, DbType.String, 40);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Instant Power data viewed"));
            }
            catch (CABException)
            {
            }
            return dataSet;
        }

        public DataSet GetInstantDataByMeter(string meterID, List<string> columns)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select i.MeterID, f.FileName ");
                foreach (string column in columns)
                {
                    //builder.Append(string.Concat(",", "i.", column, " "));
                    builder.Append(string.Concat(",", column, " "));
                }
                builder.Append("from meterdata_instantpower i inner join meterdata m on i.MeterData_ID = m.MeterData_ID ");
                builder.Append("inner join meterdata_general g on i.MeterData_ID = g.MeterData_ID ");
                builder.Append("inner join fileupload_master f on m.fileUpload_ID = f.fileUpload_ID where ");
                builder.Append(string.Concat("m.", MeterID, "=", ParameterName(MeterID)));
                DataRequest request = new DataRequest(builder.ToString());
				request.AddParamter(ParameterName(MeterID), meterID, DbType.String, 20);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Instant Power data viewed"));
            }
            catch (CABException)
            {
            }
            return dataSet;
        }
    } 
}
