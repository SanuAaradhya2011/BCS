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
using System.Collections;
using System.Text;
using System.Data;
using CAB.IECFramework;
using CAB.DALC.Data.DataServices;
using CAB.Entity;
using CAB.IECFramework.Entity;
using CAB.IECFramework.Utility;
using System.Data.Common;


namespace CAB.DALC.Data
{
	public class LoadSurveyDAL : DALBase
	{
		private string FileUpload_ID = "FileUpload_ID";
		private string Meterdata_ID = "Meterdata_ID";
		private string FileName = "FileName";
		private string MeterID = "MeterID";

        private string LoadSurvey_ID = "LoadSurvey_ID";
        private string MeterReadingDatetime = "MeterReadingDatetime";
        private string RPhaseVoltage = "RPhaseVoltage";
        private string YPhaseVoltage = "YPhaseVoltage";
        private string BPhaseVoltage = "BPhaseVoltage";
        private string RPhaseCurrent = "RPhaseCurrent";
        private string YPhaseCurrent = "YPhaseCurrent";
        private string BPhaseCurrent = "BPhaseCurrent";
        private string AvgVoltage = "AvgVoltage";
        private string AvgCurrent = "AvgCurrent";
        private string DemandKVARLead = "DemandKVARLead";
        private string DemandKVA = "DemandKVA";
        private string DemandKW = "DemandKW";
        private string DemandKVARLag = "DemandKVARLag";
        private string PowerFactor = "PowerFactor";
        private string TamperStatus = "TamperStatus";
        private string LoadSurveyDateTime = "LoadSurveyDateTime";
        private string MDIntervalPeriod = "MDIntervalPeriod";
        private string MeterData_ID = "MeterData_ID";
        private const string IsDLMS = "IsDLMS";

        public LoadSurveyDAL()
            : base("meterdata_loadsurvey", "LoadSurvey_ID")
        {
        }
        private DataRequest GetRequest(IEntity entity)
        {
            IECLoadSurveyEntity loadSurveyEntity = entity as IECLoadSurveyEntity;
            StringBuilder builder = new StringBuilder();
            builder.Append("Insert Into meterdata_loadsurvey(MeterReadingDatetime,RPhaseVoltage,YPhaseVoltage,BPhaseVoltage,RPhaseCurrent,YPhaseCurrent,BPhaseCurrent,AvgVoltage,AvgCurrent,DemandKVARLead,DemandKVA,DemandKW,DemandKVARLag,PowerFactor,TamperStatus,LoadSurveyDateTime,MDIntervalPeriod,IsDLMS,MeterData_ID) values(");
            builder.Append(string.Concat(ParameterName(MeterReadingDatetime), ","));
            builder.Append(string.Concat(ParameterName(RPhaseVoltage), ","));
            builder.Append(string.Concat(ParameterName(YPhaseVoltage), ","));
            builder.Append(string.Concat(ParameterName(BPhaseVoltage), ","));
            builder.Append(string.Concat(ParameterName(RPhaseCurrent), ","));
            builder.Append(string.Concat(ParameterName(YPhaseCurrent), ","));
            builder.Append(string.Concat(ParameterName(BPhaseCurrent), ","));
            builder.Append(string.Concat(ParameterName(AvgVoltage), ","));
            builder.Append(string.Concat(ParameterName(AvgCurrent), ","));
            builder.Append(string.Concat(ParameterName(DemandKVARLead), ","));
            builder.Append(string.Concat(ParameterName(DemandKVA), ","));
            builder.Append(string.Concat(ParameterName(DemandKW), ","));
            builder.Append(string.Concat(ParameterName(DemandKVARLag), ","));
            builder.Append(string.Concat(ParameterName(PowerFactor), ","));
            builder.Append(string.Concat(ParameterName(TamperStatus), ","));
            builder.Append(string.Concat(ParameterName(LoadSurveyDateTime), ","));
            builder.Append(string.Concat(ParameterName(MDIntervalPeriod), ","));
            builder.Append(string.Concat(ParameterName(IsDLMS), ","));
            builder.Append(string.Concat(ParameterName(MeterData_ID), ")"));
            DataRequest request = new DataRequest(builder.ToString());
            request.AddParamter(ParameterName(MeterReadingDatetime), loadSurveyEntity.MeterReadingDatetime, DbType.Int64);
            request.AddParamter(ParameterName(RPhaseVoltage), loadSurveyEntity.RPhaseVoltage, DbType.String, 20);
            request.AddParamter(ParameterName(YPhaseVoltage), loadSurveyEntity.YPhaseVoltage, DbType.String, 20);
            request.AddParamter(ParameterName(BPhaseVoltage), loadSurveyEntity.BPhaseVoltage, DbType.String, 20);
            request.AddParamter(ParameterName(RPhaseCurrent), loadSurveyEntity.RPhaseCurrent, DbType.String, 20);
            request.AddParamter(ParameterName(YPhaseCurrent), loadSurveyEntity.YPhaseCurrent, DbType.String, 20);
            request.AddParamter(ParameterName(BPhaseCurrent), loadSurveyEntity.BPhaseCurrent, DbType.String, 20);
            request.AddParamter(ParameterName(AvgVoltage), loadSurveyEntity.AvgVoltage, DbType.String, 20);
            request.AddParamter(ParameterName(AvgCurrent), loadSurveyEntity.AvgCurrent, DbType.String, 20);
            request.AddParamter(ParameterName(DemandKVARLead), loadSurveyEntity.DemandKVARLead, DbType.String, 20);
            request.AddParamter(ParameterName(DemandKVA), loadSurveyEntity.DemandKVA, DbType.String, 20);
            request.AddParamter(ParameterName(DemandKW), loadSurveyEntity.DemandKW, DbType.String, 20);
            request.AddParamter(ParameterName(DemandKVARLag), loadSurveyEntity.DemandKVARLag, DbType.String, 20);
            request.AddParamter(ParameterName(PowerFactor), loadSurveyEntity.PowerFactor, DbType.String, 20);
            request.AddParamter(ParameterName(TamperStatus), loadSurveyEntity.TamperStatus, DbType.String, 20);
            request.AddParamter(ParameterName(LoadSurveyDateTime), loadSurveyEntity.LoadSurveyDateTime, DbType.Int64);
            request.AddParamter(ParameterName(MDIntervalPeriod), loadSurveyEntity.MDIntervalPeriod, DbType.Int16);
            request.AddParamter(ParameterName(IsDLMS), loadSurveyEntity.IsDLMS, DbType.Int16);
            request.AddParamter(ParameterName(MeterData_ID), loadSurveyEntity.MeterData_ID, DbType.String, 20);
            return request;
        }
        public long GetToDate(long meterDataID)
        {
            long meterid = 0;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select Max(LoadSurveyDateTime)  from meterdata_loadsurvey where LoadSurveyDateTime!=0 and ");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataID, DbType.Int64);
                Object data = helper.ExecuteScalar(request);
                if (string.IsNullOrEmpty(Convert.ToString(data)))
                    meterid = 0;
                else
                meterid = Convert.ToInt64(data);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Load Survey data viewed"));
            }
            catch (CABException)
            {
                meterid = 0;
            }
            return meterid;
        }

        public DataSet GetDates(long meterDataID)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select LoadSurveyDateTime from meterdata_loadsurvey where ");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)," and LoadSurveyDateTime!=0 order by LoadSurveyDateTime asc"));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataID, DbType.Int64);
                  dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Load Survey data viewed"));
            }
            catch (CABException)
            {
                dataSet = null;
            }
            return dataSet;
        }

        public long GetFromDate(long meterDataID)
        {
            long meterid = 0;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select Min(LoadSurveyDateTime)  from meterdata_loadsurvey where LoadSurveyDateTime!=0 and ");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataID, DbType.Int64);
                Object data = helper.ExecuteScalar(request);
                if (string.IsNullOrEmpty(Convert.ToString(data)))
                    meterid = 0;
                else
                    meterid = Convert.ToInt64(data);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Load Survey data viewed"));
            }
            catch (CABException)
            {
                meterid = 0;
            }
            return meterid;
        }

        public override IEntity InsertData(IEntity entity)
        {
            IECLoadSurveyEntity loadSurveyEntity = entity as IECLoadSurveyEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper(); 
                helper.ExecuteNonQuery(this.GetRequest(entity));
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Load Survey data added"));
                Flag = true;
            }
            catch (Exception) { }
            if (Flag)
                loadSurveyEntity.LoadSurvey_ID = Int64.Parse(this.GetPK());
            return loadSurveyEntity;
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
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Load Survey data added"));
            }
            catch (Exception) { }
            return null;
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
                builder.Append("Delete from meterdata_loadsurvey where ");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataID, DbType.Int32);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter Data Deleted."));
                Flag = true;
            }
            catch (Exception) { }
            return Flag;
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
			//DataSet ds = null;
			//try
			//{
			//    IDataHelper helper = DatabaseFactory.GetHelper();
			//    StringBuilder builder = new StringBuilder();
			//    //Removed from the List
			//    //M.Meter_Phone as 'Meter Phone',
			//    //if meter_master.Meter_Status = 1  and consumermeter.Status = 1.then the active Meters are selected else if 0 is checked then Inactive meters are selected.
			//    //but the queries for selecting the active and inactive meters are different since the consumer meter and 
			//    //consumer_Master will not contain in the query.

			//    builder.Append("select * from meterdata_loadsurvey where Meterdata_ID = ");
			//    builder.Append(string.Concat(Meterdata_ID, "=", ParameterName(Meterdata_ID)));
			//    DataRequest request = new DataRequest(builder.ToString());
			//    request.AddParamter(ParameterName(Meterdata_ID), meter_ID, DbType.String, 20);
			//    ds = new DataSet();
			//    ds = helper.FillDataSet(request, ds);
			//    if (ds.Tables[0].Rows.Count > 0)
			//        meterMasterEntity = (MeterMasterEntity)RowToEntity(ds.Tables[0].Rows[0]);
			//}
			//catch (CABException)
			//{
			//    ds = null;
			//}
			//return ds;
		}
        public DataSet ListDataSet(long meterDataId, string columnsNames, long fromDate, long toDate)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select distinct loadsurveydatetime ");
                builder.Append(columnsNames);
                builder.Append(",MDIntervalPeriod, TamperStatus,IsDLMS from meterdata_loadsurvey where ");
                builder.Append(string.Concat(Meterdata_ID, "=", ParameterName(Meterdata_ID)," and "));
                builder.Append(string.Concat(LoadSurveyDateTime, ">=", ParameterName("FromDate"), " and "));
                builder.Append(string.Concat(LoadSurveyDateTime, "<=", ParameterName("ToDate")));
                builder.Append(string.Concat(" order by loadsurveydatetime"));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Meterdata_ID), meterDataId, DbType.Int64);
                request.AddParamter(ParameterName("FromDate"), fromDate, DbType.Int64);
                request.AddParamter(ParameterName("ToDate"), toDate, DbType.Int64);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Load Survey data viewed"));
            }
            catch (CABException)
            {
                dataSet = null;
            }
            return dataSet;
        }
		public DataSet ListDataSet(string meterDataID)//string fileName, string meterID)
		{
			DataSet ds = null;
			//string meterDataID = string.Empty;
			try
			{
				IDataHelper helper = DatabaseFactory.GetHelper();
				StringBuilder builder = new StringBuilder();

			//	meterDataID = GetMeterDataID(fileName, meterID);
				builder.Append(string.Concat("Select RPhaseVoltage,YPhaseVoltage,BPhaseVoltage,RPhaseCurrent,YPhaseCurrent,BPhaseCurrent,AvgVoltage,AvgCurrent,DemandKVARLead,DemandKVA,DemandKW,DemandKVARLag,PowerFactor,LoadSurveyDateTime,MDIntervalPeriod from meterdata_loadsurvey where "));
				builder.Append(string.Concat(Meterdata_ID, "=", ParameterName(Meterdata_ID)));
				builder.Append(string.Concat(" order by loadsurveydatetime"));
				DataRequest request = new DataRequest(builder.ToString());
				request.AddParamter(ParameterName(Meterdata_ID), meterDataID, DbType.String, 20);
				ds = new DataSet();
				ds = helper.FillDataSet(request, ds);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Load Survey data viewed"));
				if (ds.Tables[0].Rows.Count > 0)
					return ds;
			}
			catch (CABException)
			{
				ds = null;
			}
			return ds;
		}

		public DataSet ListDataSet(ArrayList columnNames,string fileName, string meterID)
		{
			DataSet ds = null;
			string meterDataID = string.Empty;
			try
			{
				IDataHelper helper = DatabaseFactory.GetHelper();
				StringBuilder builder = new StringBuilder();

				meterDataID = GetMeterDataID(fileName, meterID);

				builder.Append("select  ");
				for (int colCount = 0; colCount < columnNames.Count; colCount++)
				{
					builder.Append(string.Concat(columnNames[colCount].ToString(), ","));
				}
				builder.Append(string.Concat(" loadsurveydatetime from meterdata_loadsurvey where "));
				builder.Append(string.Concat(Meterdata_ID, "=", ParameterName(Meterdata_ID)));
				builder.Append(string.Concat(" order by loadsurveydatetime"));//
				DataRequest request = new DataRequest(builder.ToString());
				request.AddParamter(ParameterName(Meterdata_ID), meterDataID, DbType.String, 20);
				ds = new DataSet();
				ds = helper.FillDataSet(request, ds);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Load Survey data viewed"));
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
		/// Get MeterdataID is used to get the MeterDataID for the FileName and The Meter
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="meterID"></param>
		/// <returns></returns>
		public string GetMeterDataID(string fileName, string meterID)
		{
			string result = string.Empty;

			try
			{
				IDataHelper helper = DatabaseFactory.GetHelper();
				StringBuilder builder = new StringBuilder();
				builder.Append("select b.MeterData_ID from fileupload_master a inner join meterdata b on a.FileUpload_ID = b.FileUpload_ID where ");
				builder.Append(string.Concat("a." + FileName, "=", ParameterName(FileName)," and "));
				builder.Append(string.Concat("b." + MeterID, "=", ParameterName(MeterID)));
				builder.Append(string.Concat(" order by b.MeterData_ID desc"));
				DataRequest request = new DataRequest(builder.ToString());
				request.AddParamter(ParameterName(FileName), fileName, DbType.String, 40);
				request.AddParamter(ParameterName(MeterID), meterID, DbType.String, 20);
				result = Convert.ToString( helper.ExecuteScalar(request));
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter ID retrieved"));
				return result;
			}
			catch (CABException)
			{
				return null;	
			}
		}

		public ArrayList GetColumnsListAvailable(string parameterName, string meterDataID)//string fileName, string meterID)
		{
			DataSet dataSet = new DataSet();
			ArrayList columnArray = new ArrayList();
			//string meterDataID = string.Empty;
			try
			{
				//meterDataID = GetMeterDataID(fileName, meterID);
				if (meterDataID == "")
				{
					return null;
				}
				IDataHelper helper = DatabaseFactory.GetHelper();
				StringBuilder builder = new StringBuilder();
				builder.Append("show columns from meterdata_loadsurvey where field like ");
				if (parameterName == "Demand")
				{
					builder.Append(string.Concat("'" + parameterName + "%' and field not like 'avg%'"));
				}
				else
				{
					builder.Append(string.Concat("'%" + parameterName + "' and field not like 'avg%'"));
				}
				DataRequest request = new DataRequest(builder.ToString());
				helper.FillDataSet(request, dataSet);
				foreach (DataRow drow in dataSet.Tables[0].Rows)
				{
					StringBuilder builder1 = new StringBuilder();
					builder1.Append("Select count(*) from meterdata_loadsurvey where ");
					builder1.Append(string.Concat(drow[0].ToString(), " is not null and "));
					builder1.Append(string.Concat(Meterdata_ID, "=", meterDataID));
					DataRequest request1 = new DataRequest(builder1.ToString());
					object data = helper.ExecuteScalar(request1);
					if (Convert.ToInt16(data.ToString()) > 0)
					{
						columnArray.Add(drow[0].ToString());
					}
				}
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Load Survey data viewed"));
				return columnArray;
			}
			catch (CABException)
			{
				return null;
			}
		}

		public List<string> GetColumnListAvailable(long meterDataID)
		{
			DataSet dataSet = new DataSet();
            List<string> columnArray = new List<string>();
			try
			{
				IDataHelper helper = DatabaseFactory.GetHelper();
				StringBuilder builder = new StringBuilder();
				builder.Append("show columns from meterdata_loadsurvey");
				DataRequest request = new DataRequest(builder.ToString());
				helper.FillDataSet(request, dataSet);

				foreach (DataRow drow in dataSet.Tables[0].Rows)
				{
					StringBuilder builder1 = new StringBuilder();
					builder1.Append("Select count(*) from meterdata_loadsurvey where ");
					builder1.Append(string.Concat(drow[0].ToString(), " is not null and "));
					builder1.Append(string.Concat(Meterdata_ID, "=", meterDataID));
					DataRequest request1 = new DataRequest(builder1.ToString());
					object data = helper.ExecuteScalar(request1);
					if (Convert.ToInt32(data.ToString()) > 0)
					{
						columnArray.Add(drow[0].ToString());
					}
				}
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Load Survey data viewed"));
				return columnArray;
			}
			catch (CABException)
			{
				return null;
			}
		}

		public DataSet ListDataSet(long meterDataID, List<string> columnNames)
		{
			DataSet dataSet = null;
			try
			{
				IDataHelper helper = DatabaseFactory.GetHelper();
				StringBuilder builder = new StringBuilder();
                
                builder.Append("Select m.MeterID ");
                foreach (string columnName in columnNames)
                {
                   builder.Append(string.Concat(", ", "ls.",columnName));
                }
                builder.Append(string.Concat(" from meterdata_loadsurvey as ls inner join meterdata as m on ls.MeterData_ID = m.MeterData_ID where "));                
                //builder.Append(string.Concat("Select A.MeterID,B.RPhaseVoltage,B.YPhaseVoltage,B.BPhaseVoltage,B.RPhaseCurrent,B.YPhaseCurrent,B.BPhaseCurrent,B.AvgVoltage,B.AvgCurrent,B.DemandKVARLead,B.DemandKVA,B.DemandKW,B.DemandKVARLag,B.PowerFactor,B.LoadSurveyDateTime,B.MDIntervalPeriod from meterdata_loadsurvey B Inner Join meterdata A on B.MeterData_ID = A.MeterData_ID where "));
				builder.Append(string.Concat("ls.",Meterdata_ID, "= ", ParameterName(Meterdata_ID), " "));
                builder.Append("order by loadSurveyDateTime asc");
				DataRequest request = new DataRequest(builder.ToString());
				request.AddParamter(ParameterName(Meterdata_ID), meterDataID, DbType.Int32);
				dataSet = new DataSet();
				dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Load Survey data viewed"));
			}
			catch (CABException)
			{
				dataSet = null;
			}
			return dataSet;
		}

        public DataSet GetLoadSurveyDataByFile(string fileName, List<string> columns)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select m.MeterID ");
                foreach (string column in columns)
                {
                    builder.Append(string.Concat(",","ls.", column, " "));
                }
                builder.Append("from meterdata_loadsurvey ls inner join meterdata m on ls.MeterData_ID = m.MeterData_ID ");
                builder.Append("inner join fileupload_master f on m.FileUpload_ID = f.FileUpload_ID where ");
                builder.Append(string.Concat("f.", FileName, "=", ParameterName(FileName)));
                DataRequest request = new DataRequest(builder.ToString());
				request.AddParamter(ParameterName(FileName), fileName, DbType.String, 40);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Load Survey data viewed"));
            }
            catch (CABException)
            {
            }
            return dataSet;
        }

        public DataSet GetLoadSurveyDataByMeter(string meterID, List<string> columns)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select m.MeterID, f.FileName ");
                foreach (string column in columns)
                {
                    builder.Append(string.Concat(",", "ls.", column, " "));
                }
                builder.Append("from meterdata_loadsurvey ls inner join meterdata m on ls.MeterData_ID = m.MeterData_ID ");
                builder.Append("inner join fileupload_master f on m.FileUpload_ID = f.FileUpload_ID where ");
                builder.Append(string.Concat("m.", MeterID, "=", ParameterName(MeterID)));
                DataRequest request = new DataRequest(builder.ToString());
				request.AddParamter(ParameterName(MeterID), meterID, DbType.String, 20);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Load Survey data viewed"));
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
 
    }
}
