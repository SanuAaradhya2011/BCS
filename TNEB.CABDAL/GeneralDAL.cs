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
     public class GeneralDAL : DALBase
    {
         private string General_ID = "General_ID";
         private string MeterID = "MeterID";
         private string MeterDateTime = "MeterDateTime";
         private string ErrorCode = "ErrorCode";
         private string MeterConstant = "MeterConstant"; 
         private string FirmwareVersion = "FirmwareVersion";
         private string VoltagePhaseSequence = "VoltagePhaseSequence";
         private string TotalActiveEnergy = "TotalActiveEnergy"; 
         private string CumulativeMD1 = "CumulativeMD1";
         private string CumulativeMD2 = "CumulativeMD2";
         private string RisingDemandKW = "RisingDemandKW";
         private string ElapsedTimeKW = "ElapsedTimeKW"; 
         private string RisingDemandKVA = "RisingDemandKVA";
         private string ElapsedTimeKVA = "ElapsedTimeKVA";
         private string TotalPowerOnHours = "TotalPowerOnHours";
         private string CurrentMonthPowerOnHours = "CurrentMonthPowerOnHours";
         private string   MDResetCounter= "MDResetCounter";
         private string  ReadoutCounter = "ReadoutCounter";
         private string  ProgrammingCounter = "ProgrammingCounter";
         private string  LatestTamperOccurrenceID = "LatestTamperOccurrenceID";
         private string  OccurrenceTime = "OccurrenceTime";
         private string  LatestTamperRestorationID = "LatestTamperRestorationID";
         private string  BateryModePowerOnHour = "BateryModePowerOnHour";
         private string  RestorationTime = "RestorationTime"; 
         private string MeterData_ID = "MeterData_ID";
         private string FileName = "FileName";
         private string PowerOffDays = "PowerOffDays";
         private const string CumulativeExportEnergyKWH = "CumulativeExportEnergyKWH";
         private const string CumulativeExportEnergyKVAH = "CumulativeExportEnergyKVAH";
         private const string IMPORT = "";
         private const string EXPORT = " (E)";
         private bool isWBExportVCL = false;
         /* GKG 6/03/2013 138268 PVVNL Bug*/
         private bool isPVVNL = false;
         /* GKG 6/03/2013 138268 PVVNL Bug*/
         public GeneralDAL()
             : base("meterdata_general", "General_ID")
        {
        }
         public GeneralDAL(CAB.Entity.IECUtilityEntity utilityEntity)
             : base("meterdata_general", "General_ID")
         {
             if (utilityEntity == CAB.Entity.IECUtilityEntity.WBEXPORTVCL)
             {
                 isWBExportVCL = true;
             }
             /* GKG 6/03/2013 138268 PVVNL Bug*/
             else if (utilityEntity == CAB.Entity.IECUtilityEntity.PVVNL)
             {
                 isPVVNL = true;
             }
             /* GKG 6/03/2013 138268 PVVNL Bug*/
         }
         public override IEntity InsertData(IEntity entity)
        {
            GeneralEntity generalEntity = entity as GeneralEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert Into meterdata_general(MeterID,MeterDateTime,ErrorCode,MeterConstant,FirmwareVersion,VoltagePhaseSequence,TotalActiveEnergy,");
                if (isWBExportVCL)
                {
                    builder.Append("CumulativeExportEnergyKWH,CumulativeExportEnergyKVAH,");
                }
                builder.Append("CumulativeMD1,CumulativeMD2,RisingDemandKW,ElapsedTimeKW,RisingDemandKVA,ElapsedTimeKVA,TotalPowerOnHours,CurrentMonthPowerOnHours,MDResetCounter,ReadoutCounter,ProgrammingCounter,LatestTamperOccurrenceID,OccurrenceTime,LatestTamperRestorationID,BateryModePowerOnHour,RestorationTime,MeterData_ID,PowerOffDays) values(");
                builder.Append(string.Concat(ParameterName(MeterID), ","));
                builder.Append(string.Concat(ParameterName(MeterDateTime), ","));
                builder.Append(string.Concat(ParameterName(ErrorCode), ","));
                builder.Append(string.Concat(ParameterName(MeterConstant), ","));
                builder.Append(string.Concat(ParameterName(FirmwareVersion), ",")); 
                builder.Append(string.Concat(ParameterName(VoltagePhaseSequence), ",")); 
                builder.Append(string.Concat(ParameterName(TotalActiveEnergy), ","));
                if (isWBExportVCL)
                {
                    builder.Append(string.Concat(ParameterName(CumulativeExportEnergyKWH), ","));
                    builder.Append(string.Concat(ParameterName(CumulativeExportEnergyKVAH), ","));
                }
                builder.Append(string.Concat(ParameterName(CumulativeMD1), ","));
                builder.Append(string.Concat(ParameterName(CumulativeMD2), ",")); 
                builder.Append(string.Concat(ParameterName(RisingDemandKW), ","));
                builder.Append(string.Concat(ParameterName(ElapsedTimeKW), ","));
                builder.Append(string.Concat(ParameterName(RisingDemandKVA), ","));
                builder.Append(string.Concat(ParameterName(ElapsedTimeKVA), ","));
                builder.Append(string.Concat(ParameterName(TotalPowerOnHours), ","));
                builder.Append(string.Concat(ParameterName(CurrentMonthPowerOnHours), ",")); 
                builder.Append(string.Concat(ParameterName(MDResetCounter), ","));
                builder.Append(string.Concat(ParameterName(ReadoutCounter), ","));
                builder.Append(string.Concat(ParameterName(ProgrammingCounter), ",")); 
                builder.Append(string.Concat(ParameterName(LatestTamperOccurrenceID), ","));
                builder.Append(string.Concat(ParameterName(OccurrenceTime), ","));
                builder.Append(string.Concat(ParameterName(LatestTamperRestorationID), ","));
                builder.Append(string.Concat(ParameterName(BateryModePowerOnHour), ","));
                builder.Append(string.Concat(ParameterName(RestorationTime), ",")); 
                builder.Append(string.Concat(ParameterName(MeterData_ID), ","));
                builder.Append(string.Concat(ParameterName(PowerOffDays), ")"));
                DataRequest request = new DataRequest(builder.ToString()); 
                request.AddParamter(ParameterName(MeterID), generalEntity.MeterID, DbType.String, 20);
                request.AddParamter(ParameterName(MeterDateTime), generalEntity.MeterDateTime, DbType.Int64);
                request.AddParamter(ParameterName(ErrorCode), generalEntity.ErrorCode, DbType.String, 40);
                request.AddParamter(ParameterName(MeterConstant), generalEntity.MeterConstant, DbType.String, 40);
                request.AddParamter(ParameterName(FirmwareVersion), generalEntity.FirmwareVersion, DbType.String, 40); 
                request.AddParamter(ParameterName(VoltagePhaseSequence), generalEntity.VoltagePhaseSequence, DbType.String, 40); 
                request.AddParamter(ParameterName(TotalActiveEnergy), generalEntity.TotalActiveEnergy, DbType.String, 40);
                if(isWBExportVCL)
                {
                    request.AddParamter(ParameterName(CumulativeExportEnergyKWH),generalEntity.CumulativeExportEnergyKWH,DbType.String,40);
                    request.AddParamter(ParameterName(CumulativeExportEnergyKVAH), generalEntity.CumulativeExportEnergyKVAH, DbType.String, 40);
                }
                request.AddParamter(ParameterName(CumulativeMD1), generalEntity.CumulativeMD1, DbType.String, 40);
                request.AddParamter(ParameterName(CumulativeMD2), generalEntity.CumulativeMD2, DbType.String, 40); 
                request.AddParamter(ParameterName(RisingDemandKW), generalEntity.RisingDemandKW, DbType.String, 40);
                request.AddParamter(ParameterName(ElapsedTimeKW), generalEntity.ElapsedTimeKW, DbType.String, 40);
                request.AddParamter(ParameterName(RisingDemandKVA), generalEntity.RisingDemandKVA, DbType.String, 40);
                request.AddParamter(ParameterName(ElapsedTimeKVA), generalEntity.ElapsedTimeKVA, DbType.String, 40);
                request.AddParamter(ParameterName(TotalPowerOnHours), generalEntity.TotalPowerOnHours, DbType.String, 40);
                request.AddParamter(ParameterName(CurrentMonthPowerOnHours), generalEntity.CurrentMonthPowerOnHours, DbType.String, 40); 
                request.AddParamter(ParameterName(MDResetCounter), generalEntity.MDResetCounter, DbType.String, 40);
                request.AddParamter(ParameterName(ReadoutCounter), generalEntity.ReadoutCounter, DbType.String, 40);
                request.AddParamter(ParameterName(ProgrammingCounter), generalEntity.ProgrammingCounter, DbType.String, 40); 
                request.AddParamter(ParameterName(LatestTamperOccurrenceID), generalEntity.LatestTamperOccurrenceID, DbType.String, 40);
                request.AddParamter(ParameterName(OccurrenceTime), generalEntity.OccurrenceTime, DbType.String, 40);
                request.AddParamter(ParameterName(LatestTamperRestorationID), generalEntity.LatestTamperRestorationID, DbType.String, 40);
                request.AddParamter(ParameterName(BateryModePowerOnHour), generalEntity.BateryModePowerOnHour, DbType.String, 40);
                request.AddParamter(ParameterName(RestorationTime), generalEntity.RestorationTime, DbType.String, 40); 
                request.AddParamter(ParameterName(MeterData_ID), generalEntity.MeterData_ID, DbType.Int64);
                request.AddParamter(ParameterName(PowerOffDays), generalEntity.PowerOffDays, DbType.String, 40); 
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Data for Power added"));
                Flag = true;
            }
            catch (Exception) { }
            if (Flag)
                generalEntity.General_ID = long.Parse(this.GetPK());
            return generalEntity;
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
                builder.Append("Delete from meterdata_general where ");
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
            GeneralEntity generalEntity = entity as GeneralEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete from meterdata_general where ");
                builder.Append(string.Concat(General_ID, "=", ParameterName(General_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(General_ID), generalEntity.General_ID, DbType.UInt32);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Data for Power deleted added")); 
                Flag = true;
            }
            catch (Exception) { }
            return Flag;
        }

        public DataSet GetMeterData(int meterDataId)
        {
            DataSet dataSet = new DataSet();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select A.MeterID as 'Meter Id',");
                builder.Append("B.MeterDateTime as 'Meter DateTime',");
               
                /* GKG 6/03/2013 138268 PVVNL Bug*/
                if (isPVVNL)
                {
                    builder.Append("B.FirmwareVersion as 'Firmware Version',");
                    builder.Append("B.MeterConstant as 'Meter Constant',");
                }
                /* GKG 6/03/2013 138268 PVVNL Bug*/

                /* GKG 21/01/2013 TANGEDCO ISSUE*/
                //builder.Append("B.FirmwareVersion as 'Firmware Version',");
                /* GKG 21/01/2013 TANGEDCO ISSUE*/
                //builder.Append("B.MeterConstant as 'Meter Constant',");
                builder.Append("B.ErrorCode as 'Error Code',");
                builder.Append("D.TotalFundamentalActiveEnergy as 'Cumulative Fundamental Active Energy',");
                builder.Append("B.TotalActiveEnergy as 'Cumulative Active Energy" + IMPORT +  "',");
                builder.Append("C.CumulativeEnergyKVAH as 'Cumulative Apparent Energy" + IMPORT + "',");

                builder.Append("C.CumulativeEnergyKVARHLag as 'Cumulative Reactive Energy (Lag)',");
                builder.Append("C.CumulativeEnergyKVARHLead as 'Cumulative Reactive Energy (Lead)',");
                if (isWBExportVCL)
                {
                    builder.Append("B.CumulativeExportEnergyKWH as 'Cumulative Active Energy" + EXPORT +  "',");
                    builder.Append("B.CumulativeExportEnergyKVAH as 'Cumulative Apparent Energy" + EXPORT +  "',");

                }
                builder.Append("C.CumulativeMD1 as 'Current Month MD1',");
                builder.Append("C.CumulativeMD1TimeStamp as 'Current Month MD1 Time Stamp',");
                builder.Append("C.CumulativeMD2 as 'Current Month MD2',");
                builder.Append("C.CumulativeMD2TimeStamp as 'Current Month MD2 Time Stamp',"); 
                builder.Append("B.CumulativeMD1 as 'Cumulative MD1',");
                builder.Append("B.CumulativeMD2 as 'Cumulative MD2',"); 
                builder.Append("B.MDResetCounter as 'MD Reset Counter',");
                builder.Append("B.ReadoutCounter as 'Readout Counter',");
                builder.Append("B.ProgrammingCounter as 'Programming Counter',");
                builder.Append("B.TotalPowerOnHours as 'Total Power On Hours',");
                builder.Append("B.BateryModePowerOnHour as 'Battery Mode Power On Hour',");
                builder.Append("B.VoltagePhaseSequence as 'Voltage Phase Sequence',");
                builder.Append("B.LatestTamperOccurrenceID as 'Latest Tamper Occurrence',");
                builder.Append("B.OccurrenceTime as 'Occurrence Time',");
                builder.Append("B.LatestTamperRestorationID as 'Latest Tamper Restoration',");
                builder.Append("B.RestorationTime as 'Restoration Time'");

                //Changed on 9th march 2012 as per validation report. Power off Days is not picked from the db now.
                //builder.Append("B.PowerOffDays as 'Power Off Days'");
                builder.Append(" from meterdata A, meterdata_general B, meterdata_billing C, meterdata_instantpower D where ");
                builder.Append("A.MeterData_ID=B.MeterData_ID and A.MeterData_ID=C.MeterData_ID and C.History_ID=0 and  A.MeterData_ID=D.MeterData_ID and A.");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataId, DbType.UInt32);
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("General data viewed")); 
            }
            catch (CABException)
            {
                dataSet = null;
            }
            return dataSet;
        }
        public override IEntity GetDetailData(int id)
        {
            GeneralEntity generalEntity = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select General_ID,MeterID,MeterDateTime,ErrorCode,MeterConstant,FirmwareVersion,,VoltagePhaseSequence,TotalActiveEnergy,CumulativeMD1,CumulativeMD2,RisingDemandKW,ElapsedTimeKW,RisingDemandKVA,ElapsedTimeKVA,TotalPowerOnHours,CurrentMonthPowerOnHours,MDResetCounter,ReadoutCounter,ProgrammingCounter,LatestTamperOccurrenceID,OccurrenceTime,LatestTamperRestorationID,BateryModePowerOnHour,RestorationTime,MeterData_ID,PowerOffDays from meterdata_general where ");
                builder.Append(string.Concat(General_ID, "=", ParameterName(General_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(General_ID), id, DbType.UInt32);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    generalEntity = (GeneralEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Data for Power viewed"));
            }
            catch (CABException)
            { 
            }
            return generalEntity;
        }

		public IEntity GetEntityData(int meterDataID)
		{
			GeneralEntity generalEntity = null;
			try
			{
				IDataHelper helper = DatabaseFactory.GetHelper();
				StringBuilder builder = new StringBuilder();
				builder.Append("Select General_ID,MeterID,MeterDateTime,ErrorCode,MeterConstant,FirmwareVersion,,VoltagePhaseSequence,TotalActiveEnergy,CumulativeMD1,CumulativeMD2,RisingDemandKW,ElapsedTimeKW,RisingDemandKVA,ElapsedTimeKVA,TotalPowerOnHours,CurrentMonthPowerOnHours,MDResetCounter,ReadoutCounter,ProgrammingCounter,LatestTamperOccurrenceID,OccurrenceTime,LatestTamperRestorationID,BateryModePowerOnHour,RestorationTime,MeterData_ID from meterdata_general where ");
				builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
				DataRequest request = new DataRequest(builder.ToString());
				request.AddParamter(ParameterName(MeterData_ID), meterDataID, DbType.UInt32);
				DataSet ds = new DataSet();
				ds = helper.FillDataSet(request, ds);
				if (ds.Tables[0].Rows.Count > 0)
					generalEntity = (GeneralEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Data for Power viewed"));
            }
			catch (CABException)
			{
			}
			return generalEntity;
		}

		public List<string> GetColumnList(string tableName)
		{
			List<string> columnList = new List<string>();
			DataSet dataSet = null;
			IDataHelper helper = DatabaseFactory.GetHelper();
			StringBuilder builder = new StringBuilder();
			builder.Append("show columns from ");
			builder.Append(tableName);
			DataRequest request = new DataRequest(builder.ToString());
			dataSet = new DataSet();
			dataSet = helper.FillDataSet(request, dataSet);
			foreach (DataRow drow in dataSet.Tables[0].Rows)
			{
				columnList.Add(drow[0].ToString());
			}
			return columnList;

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
                builder.Append("Select General_ID,MeterID,MeterDateTime,ErrorCode,MeterConstant,FirmwareVersion,,VoltagePhaseSequence,,TotalActiveEnergy,CumulativeMD1,CumulativeMD2,RisingDemandKW,ElapsedTimeKW,RisingDemandKVA,ElapsedTimeKVA,TotalPowerOnHours,CurrentMonthPowerOnHours,MDResetCounter,ReadoutCounter,ProgrammingCounter,LatestTamperOccurrenceID,OccurrenceTime,LatestTamperRestorationID,BateryModePowerOnHour,RestorationTime,MeterData_ID from meterdata_general");
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Data for Power viewed"));
            }
            catch (CABException)
            {
            }
            return dataSet;
        }

        public override IEntity RowToEntity(DataRow row)
        {
            if (row == null) return null;
            GeneralEntity generalEntity = new GeneralEntity();
            if (NotNullAndNotDBNull(row, General_ID)) generalEntity.General_ID = Convert.ToInt32(row[General_ID]);
            if (NotNullAndNotDBNull(row, MeterDateTime)) generalEntity.MeterDateTime = Convert.ToInt64(row[MeterDateTime]);
            if (NotNullAndNotDBNull(row, MeterID)) generalEntity.MeterID = Convert.ToString(row[MeterID]);
            if (NotNullAndNotDBNull(row, ErrorCode)) generalEntity.ErrorCode = Convert.ToString(row[ErrorCode]);
            if (NotNullAndNotDBNull(row, MeterConstant)) generalEntity.MeterConstant = Convert.ToString(row[MeterConstant]);
            if (NotNullAndNotDBNull(row, FirmwareVersion)) generalEntity.FirmwareVersion = Convert.ToString(row[FirmwareVersion]);
            if (NotNullAndNotDBNull(row, VoltagePhaseSequence)) generalEntity.VoltagePhaseSequence = Convert.ToString(row[VoltagePhaseSequence]);
            if (NotNullAndNotDBNull(row, TotalActiveEnergy)) generalEntity.TotalActiveEnergy = Convert.ToString(row[TotalActiveEnergy]);
            if (NotNullAndNotDBNull(row, CumulativeMD1)) generalEntity.CumulativeMD1 = Convert.ToString(row[CumulativeMD1]);
            if (NotNullAndNotDBNull(row, CumulativeMD2)) generalEntity.CumulativeMD2 = Convert.ToString(row[CumulativeMD2]);
            if (NotNullAndNotDBNull(row, RisingDemandKW)) generalEntity.RisingDemandKW = Convert.ToString(row[RisingDemandKW]);
            if (NotNullAndNotDBNull(row, ElapsedTimeKW)) generalEntity.ElapsedTimeKW = Convert.ToString(row[ElapsedTimeKW]);
            if (NotNullAndNotDBNull(row, RisingDemandKVA)) generalEntity.RisingDemandKVA = Convert.ToString(row[RisingDemandKVA]);
            if (NotNullAndNotDBNull(row, ElapsedTimeKVA)) generalEntity.ElapsedTimeKVA = Convert.ToString(row[ElapsedTimeKVA]);
            if (NotNullAndNotDBNull(row, TotalPowerOnHours)) generalEntity.TotalPowerOnHours = Convert.ToString(row[TotalPowerOnHours]);
            if (NotNullAndNotDBNull(row, CurrentMonthPowerOnHours)) generalEntity.CurrentMonthPowerOnHours = Convert.ToString(row[CurrentMonthPowerOnHours]);
            if (NotNullAndNotDBNull(row, MDResetCounter)) generalEntity.MDResetCounter = Convert.ToString(row[MDResetCounter]);
            if (NotNullAndNotDBNull(row, ReadoutCounter)) generalEntity.ReadoutCounter = Convert.ToString(row[ReadoutCounter]);
            if (NotNullAndNotDBNull(row, ProgrammingCounter)) generalEntity.ProgrammingCounter = Convert.ToString(row[ProgrammingCounter]);
            if (NotNullAndNotDBNull(row, LatestTamperOccurrenceID)) generalEntity.LatestTamperOccurrenceID = Convert.ToString(row[LatestTamperOccurrenceID]);
            if (NotNullAndNotDBNull(row, OccurrenceTime)) generalEntity.OccurrenceTime = Convert.ToString(row[OccurrenceTime]);
            if (NotNullAndNotDBNull(row, LatestTamperRestorationID)) generalEntity.LatestTamperRestorationID = Convert.ToString(row[LatestTamperRestorationID]);
            if (NotNullAndNotDBNull(row, BateryModePowerOnHour)) generalEntity.BateryModePowerOnHour = Convert.ToString(row[BateryModePowerOnHour]);
            if (NotNullAndNotDBNull(row, RestorationTime)) generalEntity.RestorationTime = Convert.ToString(row[RestorationTime]);
            if (NotNullAndNotDBNull(row, MeterData_ID)) generalEntity.MeterData_ID = Convert.ToInt32(row[MeterData_ID]);
            return generalEntity;
        }

        public override IEntity InsertData(IList<IEntity> entities)
        {
            throw new NotImplementedException();
        }

        public DataSet GetGeneralDataByFileName(string fileName, List<string> columns)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select g.MeterID ");
                foreach(string column in columns)
                { 
                    builder.Append(string.Concat(",", column, " "));
                }
                builder.Append("from meterdata_general g inner join meterdata m on g.MeterData_ID = m.MeterData_ID ");
                builder.Append("inner join meterdata_billing b on g.MeterData_ID = b.MeterData_ID and b.History_ID = 0 ");
                builder.Append("inner join meterdata_instantpower i on g.MeterData_ID = i.MeterData_ID ");
                builder.Append("inner join fileupload_master f on m.FileUpload_ID = f.FileUpload_ID where ");
                builder.Append(string.Concat("f.", FileName, "=", ParameterName(FileName)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(FileName), fileName, DbType.String);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Data for Power viewed"));
            }
            catch (CABException)
            {
            }
            return dataSet;
        }

        public DataSet GetGeneralDataByMeter(string meterID, List<string> columns)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select g.MeterID, f.FileName ");
                foreach (string column in columns)
                { 
                    builder.Append(string.Concat(",", column, " "));
                } 
                builder.Append("from meterdata_general g inner join meterdata m on g.MeterData_ID = m.MeterData_ID ");
                builder.Append("inner join meterdata_billing b on g.MeterData_ID = b.MeterData_ID and b.History_ID = 0 ");
                builder.Append("inner join meterdata_instantpower i on g.MeterData_ID = i.MeterData_ID ");
                builder.Append("inner join fileupload_master f on m.fileUpload_ID = f.fileUpload_ID where ");
                builder.Append(string.Concat("m.", MeterID, "=", ParameterName(MeterID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterID), meterID, DbType.String);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Data for Power viewed"));
            }
            catch (CABException)
            {
            }
            return dataSet;
        }
    } 
}

