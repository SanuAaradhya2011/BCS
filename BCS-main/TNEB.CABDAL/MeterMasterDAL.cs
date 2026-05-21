/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 	 											|
 * | 										    Date   : 25 March 2010												|
 * |----------------------------------------------------------------------------------------------------------------| 
 */
using System.Text;
using CAB.DALC.Data.DataServices;
using CAB.Entity.Base;
using CAB.IECFramework;
using CAB.IECFramework.Entity;
using CAB.IECFramework.Utility;
using CAB.Entity;
using System.Data;
using System.Data.Common;
using System;

namespace CAB.DALC.Data
{
	public class MeterMasterDAL : DALBase
	{
		private string Meter_ID = "Meter_ID";
		private string MeterType_ID = "MeterType_ID";
		private string MeterModel_ID = "MeterModel_ID";
		private string Meter_EMF = "Meter_EMF";
		private string Meter_ContractDemand = "Meter_ContractDemand";
		private string MeterUnit_ID = "MeterUnit_ID";
		private string Meter_CTPrimary = "Meter_CTPrimary";
		private string Meter_CTSecondary = "Meter_CTSecondary";
		private string Meter_PTPrimary = "Meter_PTPrimary";
		private string Meter_PTSecondary = "Meter_PTSecondary";
		private string Meter_InstalledCTPrimary = "Meter_InstalledCTPrimary";
		private string Meter_InstalledCTSecondary = "Meter_InstalledCTSecondary";
		private string Meter_InstalledPTPrimary = "Meter_InstalledPTPrimary";
		private string Meter_InstalledPTSecondary = "Meter_InstalledPTSecondary";
		private string Meter_Phone = "Meter_Phone";
		private string Meter_Status = "Meter_Status";

        public override IEntity InsertData(IEntity entity)
		{
            IECMeterMasterEntity meterMasterEntity = null;
			if (entity == null)
                return meterMasterEntity;
            meterMasterEntity = entity as IECMeterMasterEntity;
			bool Flag = false;
			try
			{
				IDataHelper helper = DatabaseFactory.GetHelper();
				StringBuilder builder = new StringBuilder();
				builder.Append("Insert Into meter_master(Meter_ID,MeterType_ID,MeterModel_ID,Meter_EMF," +
					"Meter_ContractDemand,MeterUnit_ID,Meter_CTPrimary,Meter_CTSecondary,Meter_PTPrimary,Meter_PTSecondary," +
					"Meter_InstalledCTPrimary,Meter_InstalledCTSecondary,Meter_InstalledPTPrimary,Meter_InstalledPTSecondary," +
					"Meter_Phone,Meter_Status) values(");
				builder.Append(string.Concat(ParameterName(Meter_ID), ","));
				builder.Append(string.Concat(ParameterName(MeterType_ID), ","));
				builder.Append(string.Concat(ParameterName(MeterModel_ID), ","));
				builder.Append(string.Concat(ParameterName(Meter_EMF), ","));
				builder.Append(string.Concat(ParameterName(Meter_ContractDemand), ","));
				builder.Append(string.Concat(ParameterName(MeterUnit_ID), ","));
				builder.Append(string.Concat(ParameterName(Meter_CTPrimary), ","));
				builder.Append(string.Concat(ParameterName(Meter_CTSecondary), ","));
				builder.Append(string.Concat(ParameterName(Meter_PTPrimary), ","));
				builder.Append(string.Concat(ParameterName(Meter_PTSecondary), ","));
				builder.Append(string.Concat(ParameterName(Meter_InstalledCTPrimary), ","));
				builder.Append(string.Concat(ParameterName(Meter_InstalledCTSecondary), ","));
				builder.Append(string.Concat(ParameterName(Meter_InstalledPTPrimary), ","));
				builder.Append(string.Concat(ParameterName(Meter_InstalledPTSecondary), ","));
				builder.Append(string.Concat(ParameterName(Meter_Phone), ","));
				builder.Append(string.Concat(ParameterName(Meter_Status), ")"));

				DataRequest request = new DataRequest(builder.ToString());
				request.AddParamter(ParameterName(Meter_ID), meterMasterEntity.Meter_ID, DbType.String, 20);
				request.AddParamter(ParameterName(MeterType_ID), meterMasterEntity.MeterType_ID, DbType.Int64);
				request.AddParamter(ParameterName(MeterModel_ID), meterMasterEntity.MeterModel_ID, DbType.Int64);
				request.AddParamter(ParameterName(Meter_EMF), meterMasterEntity.Meter_EMF, DbType.Int64);
				request.AddParamter(ParameterName(Meter_ContractDemand), meterMasterEntity.Meter_ContractDemand, DbType.Double);
				request.AddParamter(ParameterName(MeterUnit_ID), meterMasterEntity.MeterUnit_ID, DbType.Int64);
				request.AddParamter(ParameterName(Meter_CTPrimary), meterMasterEntity.Meter_CTPrimary, DbType.Int64);
				request.AddParamter(ParameterName(Meter_CTSecondary), meterMasterEntity.Meter_CTSecondary, DbType.Int64);
				request.AddParamter(ParameterName(Meter_PTPrimary), meterMasterEntity.Meter_PTPrimary, DbType.Int64);
				request.AddParamter(ParameterName(Meter_PTSecondary), meterMasterEntity.Meter_PTSecondary, DbType.Int64);
				request.AddParamter(ParameterName(Meter_InstalledCTPrimary), meterMasterEntity.Meter_InstalledCTPrimary, DbType.Int64);
				request.AddParamter(ParameterName(Meter_InstalledCTSecondary), meterMasterEntity.Meter_InstalledCTSecondary, DbType.Int64);
				request.AddParamter(ParameterName(Meter_InstalledPTPrimary), meterMasterEntity.Meter_InstalledPTPrimary, DbType.Int64);
				request.AddParamter(ParameterName(Meter_InstalledPTSecondary), meterMasterEntity.Meter_InstalledPTSecondary, DbType.Int64);
				request.AddParamter(ParameterName(Meter_Phone), meterMasterEntity.Meter_Phone, DbType.String, 15);
				request.AddParamter(ParameterName(Meter_Status), meterMasterEntity.Meter_Status, DbType.Int32);
				
				helper.ExecuteNonQuery(request);
				Flag = true;
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data inserted"));
			}
			catch (CABException)
			{
                meterMasterEntity = null;
			}
            return meterMasterEntity;
		}

		public override bool UpdateData(IEntity entity)
		{
			bool Flag = false;
			try
			{
                IECMeterMasterEntity meterMasterEntity = entity as IECMeterMasterEntity;
				IDataHelper helper = DatabaseFactory.GetHelper();
				StringBuilder builder = new StringBuilder();
				builder.Append("Update meter_master Set ");
				builder.Append(string.Concat(Meter_ID, "=", ParameterName(Meter_ID), ","));
				builder.Append(string.Concat(MeterType_ID, "=", ParameterName(MeterType_ID), ","));
				builder.Append(string.Concat(MeterModel_ID, "=", ParameterName(MeterModel_ID), ","));
				builder.Append(string.Concat(Meter_EMF, "=", ParameterName(Meter_EMF), ","));
				builder.Append(string.Concat(Meter_ContractDemand, "=", ParameterName(Meter_ContractDemand), ","));
				builder.Append(string.Concat(MeterUnit_ID, "=", ParameterName(MeterUnit_ID), ","));
				builder.Append(string.Concat(Meter_CTPrimary, "=", ParameterName(Meter_CTPrimary), ","));
				builder.Append(string.Concat(Meter_CTSecondary, "=", ParameterName(Meter_CTSecondary), ","));
				builder.Append(string.Concat(Meter_PTPrimary, "=", ParameterName(Meter_PTPrimary), ","));
				builder.Append(string.Concat(Meter_PTSecondary, "=", ParameterName(Meter_PTSecondary), ","));
				builder.Append(string.Concat(Meter_InstalledCTPrimary, "=", ParameterName(Meter_InstalledCTPrimary), ","));
				builder.Append(string.Concat(Meter_InstalledCTSecondary, "=", ParameterName(Meter_InstalledCTSecondary), ","));
				builder.Append(string.Concat(Meter_InstalledPTPrimary, "=", ParameterName(Meter_InstalledPTPrimary), ","));
				builder.Append(string.Concat(Meter_InstalledPTSecondary, "=", ParameterName(Meter_InstalledPTSecondary), ","));
				builder.Append(string.Concat(Meter_Phone, "=", ParameterName(Meter_Phone), ","));
				builder.Append(string.Concat(Meter_Status, "=", ParameterName(Meter_Status)));

				builder.Append(string.Concat(" Where ", Meter_ID, "=", ParameterName(Meter_ID)));

				DataRequest request = new DataRequest(builder.ToString());
				request.AddParamter(ParameterName(Meter_ID), meterMasterEntity.Meter_ID, DbType.String, 20);
				request.AddParamter(ParameterName(MeterType_ID), meterMasterEntity.MeterType_ID, DbType.Int64);
				request.AddParamter(ParameterName(MeterModel_ID), meterMasterEntity.MeterModel_ID, DbType.Int64);
				request.AddParamter(ParameterName(Meter_EMF), meterMasterEntity.Meter_EMF, DbType.Int64);
				request.AddParamter(ParameterName(Meter_ContractDemand), meterMasterEntity.Meter_ContractDemand, DbType.Double);
				request.AddParamter(ParameterName(MeterUnit_ID), meterMasterEntity.MeterUnit_ID, DbType.Int64);
				request.AddParamter(ParameterName(Meter_CTPrimary), meterMasterEntity.Meter_CTPrimary, DbType.Int64);
				request.AddParamter(ParameterName(Meter_CTSecondary), meterMasterEntity.Meter_CTSecondary, DbType.Int64);
				request.AddParamter(ParameterName(Meter_PTPrimary), meterMasterEntity.Meter_PTPrimary, DbType.Int64);
				request.AddParamter(ParameterName(Meter_PTSecondary), meterMasterEntity.Meter_PTSecondary, DbType.Int64);
				request.AddParamter(ParameterName(Meter_InstalledCTPrimary), meterMasterEntity.Meter_InstalledCTPrimary, DbType.Int64);
				request.AddParamter(ParameterName(Meter_InstalledCTSecondary), meterMasterEntity.Meter_InstalledCTSecondary, DbType.Int64);
				request.AddParamter(ParameterName(Meter_InstalledPTPrimary), meterMasterEntity.Meter_InstalledPTPrimary, DbType.Int64);
				request.AddParamter(ParameterName(Meter_InstalledPTSecondary), meterMasterEntity.Meter_InstalledPTSecondary, DbType.Int64);
				request.AddParamter(ParameterName(Meter_Phone), meterMasterEntity.Meter_Phone, DbType.String, 15);
				request.AddParamter(ParameterName(Meter_Status), meterMasterEntity.Meter_Status, DbType.Int32);

				helper.ExecuteNonQuery(request);
				Flag = true;
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data updated"));
			}
			catch (CABException)
			{
				Flag = false;
			}
			return Flag;
		}

		public override bool DeleteData(IEntity entity)
		{
			bool Flag = false;
			try
			{
                IECMeterMasterEntity meterMasterEntity = entity as IECMeterMasterEntity;
				IDataHelper helper = DatabaseFactory.GetHelper();
				StringBuilder builder = new StringBuilder();
				builder.Append("Update meter_master Set ");
				builder.Append(string.Concat(Meter_Status, "=", ParameterName(Meter_Status)));
				builder.Append(string.Concat(" Where ", Meter_ID, "=", ParameterName(Meter_ID)));
				DataRequest request = new DataRequest(builder.ToString());
				request.AddParamter(ParameterName(Meter_ID), meterMasterEntity.Meter_ID, DbType.String, 20);
				request.AddParamter(ParameterName(Meter_Status), meterMasterEntity.Meter_Status, DbType.Int32);
				helper.ExecuteNonQuery(request);
				Flag = true;
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data deleted"));
			}
			catch (CABException)
			{
				Flag = false;
			}
			return Flag;
		}

		public bool DeleteMeterData(IEntity entity)
		{
			bool Flag = false;
			try
			{
                IECMeterMasterEntity meterMasterEntity = entity as IECMeterMasterEntity;
				IDataHelper helper = DatabaseFactory.GetHelper();
				StringBuilder builder = new StringBuilder();
				builder.Append("Delete from meter_master ");
				builder.Append(string.Concat(" Where ", Meter_ID, "=", ParameterName(Meter_ID)));
				DataRequest request = new DataRequest(builder.ToString());
				request.AddParamter(ParameterName(Meter_ID), meterMasterEntity.Meter_ID, DbType.String,20);
				helper.ExecuteNonQuery(request);
				Flag = true;
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data deleted"));
			}
			catch (CABException)
			{
				Flag = false;
			}
			return Flag;
		}

		public override IEntity GetDetailData(int id)
		{
            IECMeterMasterEntity meterMasterEntity = null;
			try
			{
				IDataHelper helper = DatabaseFactory.GetHelper();
				StringBuilder builder = new StringBuilder();
                builder.Append("Select Meter_ID,MeterType_ID,MeterModel_ID,Meter_EMF,Meter_Phone,Meter_ContractDemand,MeterUnit_ID," +
				"Meter_CTPrimary,Meter_CTSecondary,Meter_PTPrimary,Meter_PTSecondary,Meter_InstalledCTPrimary," + 
				"Meter_InstalledCTSecondary,Meter_InstalledPTPrimary,Meter_InstalledPTSecondary,Meter_Status from meter_master where ");
				builder.Append(string.Concat(Meter_ID, "=", ParameterName(Meter_ID)));
				DataRequest request = new DataRequest(builder.ToString());
				request.AddParamter(ParameterName(Meter_ID), id, DbType.String, 20);
				DataSet ds = new DataSet();
				ds = helper.FillDataSet(request, ds);
				if (ds.Tables[0].Rows.Count > 0)
                    meterMasterEntity = (IECMeterMasterEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data viewed"));
			}
			catch (CABException)
			{
				meterMasterEntity = null;
			}
			return meterMasterEntity;
		}
		public IEntity GetDetailData(string meter_ID,int status)
		{
            IECMeterMasterEntity meterMasterEntity = null;
			try
			{
				IDataHelper helper = DatabaseFactory.GetHelper();
				StringBuilder builder = new StringBuilder();
                builder.Append("Select Meter_ID,MeterType_ID,MeterModel_ID,Meter_EMF,Meter_Phone,Meter_ContractDemand,MeterUnit_ID, " +
				"Meter_CTPrimary,Meter_CTSecondary,Meter_PTPrimary,Meter_PTSecondary,Meter_InstalledCTPrimary,Meter_InstalledCTSecondary, " +
				"Meter_InstalledPTPrimary,Meter_InstalledPTSecondary,Meter_Status from meter_master where ");
				builder.Append(string.Concat(Meter_ID, "=", ParameterName(Meter_ID)) + " and ");
				builder.Append(string.Concat(Meter_Status, "=", ParameterName(Meter_Status)));
				DataRequest request = new DataRequest(builder.ToString());
				request.AddParamter(ParameterName(Meter_ID), meter_ID, DbType.String, 20);
				request.AddParamter(ParameterName(Meter_Status), status, DbType.Int32);
				DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if(ds!=null)
				if (ds.Tables[0].Rows.Count > 0)
                    meterMasterEntity = (IECMeterMasterEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data viewed"));
			}
			catch (CABException)
			{
				meterMasterEntity = null;
			}
			return meterMasterEntity;
		}

		public IEntity GetDetailInactiveMeterData(string meter_ID)
		{
            IECMeterMasterEntity meterMasterEntity = null;
			try
			{
				IDataHelper helper = DatabaseFactory.GetHelper();
				StringBuilder builder = new StringBuilder();
				builder.Append("select M.Meter_ID,MT.MeterType_Name,MM.MeterModel_Name,M.Meter_EMF,M.Meter_Phone,M.Meter_ContractDemand, " + 
							"M.Meter_CTPrimary,M.Meter_CTSecondary,M.Meter_PTPrimary,M.Meter_PTSecondary,M.Meter_InstalledCTPrimary,M.Meter_InstalledCTSecondary, " +
							"M.Meter_InstalledPTPrimary,M.Meter_InstalledPTSecondary,M.Meter_Status " +
							"from meter_master M Inner Join metertype_master MT on M.MeterType_ID = MT.MeterType_ID " +
							"Inner Join metermodel_master MM on M.MeterModel_ID = MM.MeterModel_ID where M.Meter_Status = 0 and M.");
				builder.Append(string.Concat(Meter_ID, "=", ParameterName(Meter_ID)));
				DataRequest request = new DataRequest(builder.ToString());
				request.AddParamter(ParameterName(Meter_ID), meter_ID, DbType.String, 20);
				DataSet ds = new DataSet();
				ds = helper.FillDataSet(request, ds);
				if (ds.Tables[0].Rows.Count > 0)
                    meterMasterEntity = (IECMeterMasterEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Data for a specified inactive meter viewed"));
			}
			catch (CABException)
			{
				meterMasterEntity = null;
			}
			return meterMasterEntity;
		}

		public override System.Collections.Generic.IList<IEntity> ListData()
		{
			throw new System.NotImplementedException();
		}

		/// <summary>
		/// To retrieve the values of active Meters
		/// </summary>
		/// <returns></returns>

		public override DataSet ListDataSet()
		{
			DataSet ds = null;
			try
			{
				IDataHelper helper = DatabaseFactory.GetHelper();
				StringBuilder builder = new StringBuilder();
				
				//if meter_master.Meter_Status = 1  and consumermeter.Status = 1.then the active Meters are selected else if 0 is checked then Inactive meters are selected.
				//but the queries for selecting the active and inactive meters are different since the consumer meter and 
				//consumer_Master will not contain in the query.

				builder.Append("select CM.Consumer_Number as 'Consumer ID',CM.Meter_ID as 'Meter ID',C.Consumer_Name as 'Consumer Name',CT.ConsumerType_Name as 'Consumer Type',C.Consumer_Phone as 'Consumer Phone',C.Consumer_HNumber as 'Consumer House No.', " +
							"C.Consumer_Street as 'Consumer Street',C.Consumer_City as 'Consumer City',C.Consumer_Email as 'Consumer Email',MT.MeterType_Name as 'Meter Type',MM.MeterModel_Name as 'Meter Model',M.Meter_EMF as 'Meter EMF',M.Meter_Phone as 'Meter SIM No.' , M.Meter_ContractDemand as 'Meter Contract Demand', " +
							"MU.MeterUnit_Type as 'Meter Unit',M.Meter_CTPrimary as 'Meter CT Primary',M.Meter_CTSecondary as 'Meter CT Secondary',M.Meter_PTPrimary as 'Meter PT Primary',M.Meter_PTSecondary as 'Meter PT Secondary',M.Meter_InstalledCTPrimary as 'Installed CT Primary', " +
							"M.Meter_InstalledCTSecondary as 'Installed CT Secondary',M.Meter_InstalledPTPrimary as 'Installed PT Primary',M.Meter_InstalledPTSecondary as 'Installed PT Secondary',M.Meter_Status as 'Meter Status',CM.Meter_AllocationDate as 'Meter Allocation Date',CM.Meter_Location as 'Location' " +
							"from consumer_master C inner join consumermeter CM on C.Consumer_Number = CM.Consumer_Number " +
							"inner join meter_master M on CM.Meter_ID = M.Meter_ID " +
							"inner join metermodel_master MM on M.MeterModel_ID = MM.MeterModel_ID " +
							"inner join meterunit_master MU on M.MeterUnit_ID = MU.MeterUnit_ID " +
							"inner join metertype_master MT on M.MeterType_ID = MT.MeterType_ID " +
							"inner join consumertype_master CT on C.ConsumerType_ID = CT.ConsumerType_ID where M.Meter_Status = 1 and CM.status = 1");
				DataRequest request = new DataRequest(builder.ToString());
				ds = new DataSet();
				ds = helper.FillDataSet(request, ds);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data viewed"));
				if (ds.Tables[0].Rows.Count > 0)
					return ds;
			}
			catch (CABException)
			{
				ds = null;
			}
			return ds;
		}

		/// <summary>
		/// To retrieve Inactive Meters alone
		/// </summary>
		/// <returns></returns>
		public DataSet ListInactiveMeterDataSet()
		{
			DataSet ds = null;
			try
			{
				IDataHelper helper = DatabaseFactory.GetHelper();
				StringBuilder builder = new StringBuilder();
				//if Meter_Status = 0 then the Inactive Meters are selected.Here the consumer Meter table and consumer_master tables are not included.
				builder.Append("select M.Meter_ID as 'Meter ID',MT.MeterType_Name as 'Meter Type',MM.MeterModel_Name as 'Meter Model',M.Meter_EMF as 'Meter EMF',M.Meter_Phone as 'Meter SIM No.' ,M.Meter_ContractDemand as 'Meter Contract Demand', " +
							"MU.MeterUnit_Type as 'Meter Unit',M.Meter_CTPrimary as 'Meter CT Primary',M.Meter_CTSecondary as 'Meter CT Secondary',M.Meter_PTPrimary as 'Meter PT Primary',M.Meter_PTSecondary as 'Meter PT Secondary',M.Meter_InstalledCTPrimary as 'Installed CT Primary', " +
							"M.Meter_InstalledCTSecondary as 'Installed CT Secondary',M.Meter_InstalledPTPrimary as 'Installed PT Primary',M.Meter_InstalledPTSecondary as 'Installed PT Secondary',M.Meter_Status as 'Meter Status' " +
							"from meter_master M inner join metermodel_master MM on M.MeterModel_ID = MM.MeterModel_ID " +
							"inner join meterunit_master MU on M.MeterUnit_ID = MU.MeterUnit_ID " +
							"inner join metertype_master MT on M.MeterType_ID = MT.MeterType_ID where M.Meter_Status = 0");
				DataRequest request = new DataRequest(builder.ToString());
				ds = new DataSet();
				ds = helper.FillDataSet(request, ds);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Data for inactive meters viewed"));
				if (ds.Tables[0].Rows.Count > 0)
					return ds;
			}
			catch (CABException)
			{
				ds = null;
			}
			return ds;
		}

		public DataSet ListInactiveMeterID()
		{
			DataSet ds = null;
			try
			{
				IDataHelper helper = DatabaseFactory.GetHelper();
				StringBuilder builder = new StringBuilder();
				//if Meter_Status = 0 then the Inactive Meters are selected.Here the consumer Meter table and consumer_master tables are not included.
				builder.Append("select Meter_ID from meter_master where Meter_Status = 0");
				DataRequest request = new DataRequest(builder.ToString());
				ds = new DataSet();
				ds = helper.FillDataSet(request, ds);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter ID for inactive meters retrieved"));
				if (ds.Tables[0].Rows.Count > 0)
					return ds;
			}
			catch (CABException)
			{
				ds = null;
			}
			return ds;
		}

		/// <summary>
		/// To retrieve Free Consumers alone
		/// </summary>
		/// <returns></returns>
		public DataSet ListFreeConsumersDataSet()
		{
			DataSet ds = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                //if Meter_Status = 0 then the Inactive Meters are selected.Here the consumer Meter table and consumer_master tables are not included.
                builder.Append("Select distinct D.Consumer_Number as 'Consumer ID',D.Consumer_Name as 'Consumer Name',CT.ConsumerType_Name as 'Consumer Type',D.Consumer_Phone as 'Consumer Phone',D.Consumer_HNumber as 'Consumer House No.',D.Consumer_Street as 'Consumer Street', " +
                            "D.Consumer_City as 'Consumer City',D.Consumer_Email as 'Consumer Email' from Consumermeter CM " +
                            "inner join consumer_master D on CM.Consumer_Number = D.Consumer_Number " +
                            "inner join ConsumerType_master CT on D.ConsumerType_ID = CT.ConsumerType_ID where CM.Consumer_Number Not in (Select distinct Consumer_Number from Consumermeter where Status = 1)");
                //builder.Append("Select distinct D.Consumer_Number as 'Consumer ID',D.Consumer_Name as 'Consumer Name',");
                //builder.Append("CT.ConsumerType_Name as 'Consumer Type',D.Consumer_Phone as 'Consumer Phone',");
                //builder.Append("D.Consumer_HNumber as 'Consumer House No.',D.Consumer_Street as 'Consumer Street',");
                //builder.Append("D.Consumer_City as 'Consumer City',D.Consumer_Email as 'Consumer Email' from Consumermeter CM,");
                //builder.Append("consumer_master D,ConsumerType_master CT where CM.Consumer_Number = D.Consumer_Number ");
                //builder.Append("and D.ConsumerType_ID = CT.ConsumerType_ID and  CM.Status = 0");
                DataRequest request = new DataRequest(builder.ToString());
                ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("List of free consumers retrieved"));
            }
            catch (CABException)
            {
                ds = null;
            }
			return ds;
		}

		public override IEntity RowToEntity(DataRow row)
		{
			if (row == null) return null;
            IECMeterMasterEntity meterMasterEntity = new IECMeterMasterEntity();
			if (NotNullAndNotDBNull(row, Meter_ID)) meterMasterEntity.Meter_ID = Convert.ToString(row[Meter_ID]);
			if (NotNullAndNotDBNull(row, MeterType_ID)) meterMasterEntity.MeterType_ID = Convert.ToInt32(row[MeterType_ID]);
			if (NotNullAndNotDBNull(row, MeterModel_ID)) meterMasterEntity.MeterModel_ID = Convert.ToInt32(row[MeterModel_ID]);
			if (NotNullAndNotDBNull(row, Meter_EMF)) meterMasterEntity.Meter_EMF = Convert.ToInt32(row[Meter_EMF]);
			if (NotNullAndNotDBNull(row, Meter_ContractDemand)) meterMasterEntity.Meter_ContractDemand = Convert.ToDouble(row[Meter_ContractDemand]);
			if (NotNullAndNotDBNull(row, MeterUnit_ID)) meterMasterEntity.MeterUnit_ID = Convert.ToInt32(row[MeterUnit_ID]);
			if (NotNullAndNotDBNull(row, Meter_CTPrimary)) meterMasterEntity.Meter_CTPrimary = Convert.ToInt32(row[Meter_CTPrimary]);
			if (NotNullAndNotDBNull(row, Meter_CTSecondary)) meterMasterEntity.Meter_CTSecondary = Convert.ToInt32(row[Meter_CTSecondary]);
			if (NotNullAndNotDBNull(row, Meter_PTPrimary)) meterMasterEntity.Meter_PTPrimary = Convert.ToInt32(row[Meter_PTPrimary]);
			if (NotNullAndNotDBNull(row, Meter_PTSecondary)) meterMasterEntity.Meter_PTSecondary = Convert.ToInt32(row[Meter_PTSecondary]);
			if (NotNullAndNotDBNull(row, Meter_InstalledCTPrimary)) meterMasterEntity.Meter_InstalledCTPrimary = Convert.ToInt32(row[Meter_InstalledCTPrimary]);
			if (NotNullAndNotDBNull(row, Meter_InstalledCTSecondary)) meterMasterEntity.Meter_InstalledCTSecondary = Convert.ToInt32(row[Meter_InstalledCTSecondary]);
			if (NotNullAndNotDBNull(row, Meter_InstalledPTPrimary)) meterMasterEntity.Meter_InstalledPTPrimary = Convert.ToInt32(row[Meter_InstalledPTPrimary]);
			if (NotNullAndNotDBNull(row, Meter_InstalledPTSecondary)) meterMasterEntity.Meter_InstalledPTSecondary = Convert.ToInt32(row[Meter_InstalledPTSecondary]);
			if (NotNullAndNotDBNull(row, Meter_Phone)) meterMasterEntity.Meter_Phone = Convert.ToString(row[Meter_Phone]);
			if (NotNullAndNotDBNull(row, Meter_Status)) meterMasterEntity.Meter_Status = Convert.ToInt32(row[Meter_Status]);
			return meterMasterEntity;
		}

        
		public bool ValidateMeterNumber(IEntity entity)
		{
			bool Flag = false;
			try
			{
                IECMeterMasterEntity meterMasterEntity = entity as IECMeterMasterEntity;
				IDataHelper helper = DatabaseFactory.GetHelper();
				StringBuilder builder = new StringBuilder();
				builder.Append("Select count(*) from meter_master");
				builder.Append(string.Concat(" Where ", Meter_ID, "=", ParameterName(Meter_ID)));
				DataRequest request = new DataRequest(builder.ToString());
				request.AddParamter(ParameterName(Meter_ID), meterMasterEntity.Meter_ID, DbType.String, 20);
				object data = helper.ExecuteScalar(request);
				if (Convert.ToInt64(data.ToString()) > 0)
				{
					Flag = true;
				}
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Data for a specified Meter ID retrieved"));
			}
			catch (CABException)
			{
				Flag = false;
			}
			return Flag;
		}

        //added on 12 May 2010

        /// <summary>
        /// To retrieve all the existing Meter ID's
        /// </summary>
        /// <returns></returns>
        public DataSet ListMeterID()
        {
            DataSet ds = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                //if Meter_Status = 0 then the Inactive Meters are selected.Here the consumer Meter table and consumer_master tables are not included.
                builder.Append("select Meter_ID from meter_master union select MeterID from meterdata");
                DataRequest request = new DataRequest(builder.ToString());
                ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("List of Meter ID's retrieved"));
                if (ds.Tables[0].Rows.Count > 0)
                    return ds;
            }
            catch (CABException)
            {
                ds = null;
            }
            return ds;
        }

		public DataSet ListUnAssignedAreaMeterID()
		{
			DataSet ds = null;
			try
			{
				IDataHelper helper = DatabaseFactory.GetHelper();
				StringBuilder builder = new StringBuilder();
				//if Meter_Status = 0 then the Inactive Meters are selected.Here the consumer Meter table and consumer_master tables are not included.
				builder.Append("select Meter_ID from meter_master where Meter_ID not in (select Meter_ID from areameter_master) ");
				builder.Append("union select MeterID from meterdata where MeterID not in (select Meter_ID from areameter_master)");

				DataRequest request = new DataRequest(builder.ToString());
				ds = new DataSet();
				ds = helper.FillDataSet(request, ds);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("List of unassigned Meter ID's retrieved"));
				if (ds.Tables[0].Rows.Count > 0)
					return ds;
			}
			catch (CABException)
			{
				ds = null;
			}
			return ds;
		}

        //added on 12 May 2010


        public override IEntity InsertData(System.Collections.Generic.IList<IEntity> entities)
        {
            throw new NotImplementedException();
        }

        public int GetEMF(long meterDataID)
        {
            int val = 1;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("select A.Meter_emf from meter_master A,meterdata B Where A.Meter_ID=B.MeterID and B.MeterData_ID=");
                builder.Append(ParameterName("MeterData_ID")); 
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName("MeterData_ID"), meterDataID, DbType.Int64);  
                object obj = helper.ExecuteScalar(request);
                if (obj != null)
                    val =Convert.ToInt32( obj);
                    
            }
            catch (CABException)
            {
                val = 1;
            }
            return val;
        }
    }
}
