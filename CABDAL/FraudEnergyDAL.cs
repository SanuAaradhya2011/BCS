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
using CAB.DALC.Data.DataServices;
using CAB.Entity;
using System.Data.Common;
using CAB.Framework.Entity;
using CAB.Framework;
using Hunt.EPIC.Logging;

namespace CAB.DALC.Data
{

    public class FraudEnergyDAL : DALBase
    {
        private string FraudEnergy_ID = "FraudEnergy_ID";
        private string MagneticInfluenceKWh = "MagneticInfluenceKWh";
        private string MagneticInflueneceKVARhLag = "MagneticInflueneceKVARhLag";
        private string MagneticInflueneceKVARhLead = "MagneticInflueneceKVARhLead";
        private string MagneticInflueneceKVAh = "MagneticInflueneceKVAh";
        private string ReverseEnergyKWh = "ReverseEnergyKWh";
        private string ReverseEnergyKVAh = "ReverseEnergyKVAh";
        private string ReverseEnergyKVARhLag = "ReverseEnergyKVARhLag";
        private string ReverseEnergyKVARhLead = "ReverseEnergyKVARhLead";
        private string THDVoltageRPhase = "THDVoltageRPhase";
        private string THDVoltageYPhase = "THDVoltageYPhase";
        private string THDVoltageBPhase = "THDVoltageBPhase";
        private string THDCurrentRPhase = "THDCurrentRPhase";
        private string THDCurrentYPhase = "THDCurrentYPhase";
        private string THDCurrentBPhase = "THDCurrentBPhase";
        private string MeterData_ID = "MeterData_ID";

        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(FraudEnergyDAL).ToString());
        public FraudEnergyDAL()
            : base("MeterData_FraudEnergy", "FraudEnergy_ID")
        {
        }
 
         public override IEntity InsertData(IEntity entity)
        {
            FraudEnergyEntity fraudEnergyEntity = null;
            if (entity == null)
                return fraudEnergyEntity;
             fraudEnergyEntity = entity as FraudEnergyEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert Into MeterData_FraudEnergy(MagneticInfluenceKWh,MagneticInflueneceKVARhLag,MagneticInflueneceKVARhLead,MagneticInflueneceKVAh,ReverseEnergyKWh,ReverseEnergyKVAh,"+
                "ReverseEnergyKVARhLag,ReverseEnergyKVARhLead,THDVoltageRPhase,THDVoltageYPhase,THDVoltageBPhase,"+
                "THDCurrentRPhase,THDCurrentYPhase,THDCurrentBPhase,MeterData_ID) values(");
                builder.Append(string.Concat(ParameterName(MagneticInfluenceKWh), ","));
                builder.Append(string.Concat(ParameterName(MagneticInflueneceKVARhLag), ","));
                builder.Append(string.Concat(ParameterName(MagneticInflueneceKVARhLead), ","));
                builder.Append(string.Concat(ParameterName(MagneticInflueneceKVAh), ","));
                builder.Append(string.Concat(ParameterName(ReverseEnergyKWh), ","));
                builder.Append(string.Concat(ParameterName(ReverseEnergyKVAh), ","));
                builder.Append(string.Concat(ParameterName(ReverseEnergyKVARhLag), ","));
                builder.Append(string.Concat(ParameterName(ReverseEnergyKVARhLead), ","));
                builder.Append(string.Concat(ParameterName(THDVoltageRPhase), ","));
                builder.Append(string.Concat(ParameterName(THDVoltageYPhase), ","));
                builder.Append(string.Concat(ParameterName(THDVoltageBPhase), ","));
                builder.Append(string.Concat(ParameterName(THDCurrentRPhase), ","));
                builder.Append(string.Concat(ParameterName(THDCurrentYPhase), ","));
                builder.Append(string.Concat(ParameterName(THDCurrentBPhase), ","));
                builder.Append(string.Concat(ParameterName(MeterData_ID), ")"));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MagneticInfluenceKWh), fraudEnergyEntity.MagneticInfluenceKWh, DbType.String, 20);
                request.AddParamter(ParameterName(MagneticInflueneceKVARhLag), fraudEnergyEntity.MagneticInflueneceKVARhLag, DbType.String, 20);
                request.AddParamter(ParameterName(MagneticInflueneceKVARhLead), fraudEnergyEntity.MagneticInflueneceKVARhLead, DbType.String, 20);
                request.AddParamter(ParameterName(MagneticInflueneceKVAh), fraudEnergyEntity.MagneticInflueneceKVAh, DbType.String, 20);
                request.AddParamter(ParameterName(ReverseEnergyKWh), fraudEnergyEntity.ReverseEnergyKWh, DbType.String, 20);
                request.AddParamter(ParameterName(ReverseEnergyKVAh), fraudEnergyEntity.ReverseEnergyKVAh, DbType.String, 20);
                request.AddParamter(ParameterName(ReverseEnergyKVARhLag), fraudEnergyEntity.ReverseEnergyKVARhLag, DbType.String, 20);
                request.AddParamter(ParameterName(ReverseEnergyKVARhLead), fraudEnergyEntity.ReverseEnergyKVARhLead, DbType.String, 20);
                request.AddParamter(ParameterName(THDVoltageRPhase), fraudEnergyEntity.THDVoltageRPhase, DbType.String, 20);
                request.AddParamter(ParameterName(THDVoltageYPhase), fraudEnergyEntity.THDVoltageYPhase, DbType.String, 20);
                request.AddParamter(ParameterName(THDVoltageBPhase), fraudEnergyEntity.THDVoltageBPhase, DbType.String, 20);
                request.AddParamter(ParameterName(THDCurrentRPhase), fraudEnergyEntity.THDCurrentRPhase, DbType.String, 20);
                request.AddParamter(ParameterName(THDCurrentYPhase), fraudEnergyEntity.THDCurrentYPhase, DbType.String, 20);
                request.AddParamter(ParameterName(THDCurrentBPhase), fraudEnergyEntity.THDCurrentBPhase, DbType.String, 20);
                request.AddParamter(ParameterName(MeterData_ID), fraudEnergyEntity.MeterData_ID, DbType.Int32);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Fraud Energy data added"));
                Flag = true;
            }
            catch (Exception ex)    //Exception log for catch block 
            {
                logger.Log(LOGLEVELS.Error, "InsertData(IEntity entity)", ex);
            }
            if (Flag)
                fraudEnergyEntity.FraudEnergy_ID = long.Parse(this.GetPK());
            return fraudEnergyEntity;
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
                builder.Append("Delete from MeterData_FraudEnergy where ");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataID, DbType.Int32);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data deleted"));
                Flag = true;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "DeleteData(long meterDataID)", ex);
            }
            return Flag;
        }

        public override bool DeleteData(IEntity entity)
        {
            FraudEnergyEntity fraudEnergyEntity = entity as FraudEnergyEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete from MeterData_FraudEnergy where ");
                builder.Append(string.Concat(FraudEnergy_ID, "=", ParameterName(FraudEnergy_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(FraudEnergy_ID), fraudEnergyEntity.FraudEnergy_ID, DbType.UInt32);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Fraud Energy data deleted"));
                Flag = true;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "DeleteData(IEntity entity)", ex);
            }
            return Flag;
        }
        public IEntity GetFraudEnergy(long meterDataId)
        {
            FraudEnergyEntity fraudEnergyEntity = null;
            DataSet ds = new DataSet();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select FraudEnergy_ID,MagneticInfluenceKWh,MagneticInflueneceKVARhLag,MagneticInflueneceKVARhLead,MagneticInflueneceKVAh,"
                +"ReverseEnergyKWh,ReverseEnergyKVAh,ReverseEnergyKVARhLag,ReverseEnergyKVARhLead,THDVoltageRPhase,"+
                "THDVoltageYPhase,THDVoltageBPhase,THDCurrentRPhase,THDCurrentYPhase,THDCurrentBPhase,MeterData_ID from MeterData_FraudEnergy where ");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataId, DbType.Int64);             
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    fraudEnergyEntity = (FraudEnergyEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Fraud Energy data viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetFraudEnergy(long meterDataId)", ex);
            }
            return fraudEnergyEntity;
        }
        /// <summary>
        /// Get Fraud energy as data set for analysis report
        /// </summary>
        /// <param name="meterDataId"></param>
        /// <returns></returns>
        public DataSet GetFraudEnergyDataSet(long meterDataId)
        {
            //FraudEnergyEntity fraudEnergyEntity = null;
            DataSet ds = new DataSet();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select FraudEnergy_ID,MagneticInfluenceKWh as 'Magnetic Influence Active Energy',MagneticInflueneceKVARhLag as 'Magnetic Influenece Reactive Energy (Lag)',MagneticInflueneceKVARhLead as 'Magnetic Influenece Reactive Energy (Lead)',MagneticInflueneceKVAh as 'Magnetic Influenece Apparent Energy',"
                + "ReverseEnergyKWh as 'Reverse Active Energy',ReverseEnergyKVAh as 'Reverse Apparent Energy',"+
                //"ReverseEnergyKVARhLag as 'Reverse Reactive Energy (Lag)',ReverseEnergyKVARhLead as 'Reverse Reactive Energy (Lead)',THDVoltageRPhase as 'THD Voltage R Phase'," +
               // "THDVoltageYPhase as 'THD Voltage Y Phase',THDVoltageBPhase as 'THD Voltage B Phase',THDCurrentRPhase as 'THD Current R Phase',THDCurrentYPhase as 'THD Current Y Phase',THDCurrentBPhase as 'THD Current B Phase',"+
                "MeterData_ID from MeterData_FraudEnergy where ");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataId, DbType.Int64);
                ds = helper.FillDataSet(request, ds);
                //if (ds.Tables[0].Rows.Count > 0)
                //fraudEnergyEntity = (FraudEnergyEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Fraud Energy data viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetFraudEnergyDataSet(long meterDataId)", ex);
            }
            return ds;
        }
        public override IEntity GetDetailData(int id)
        {
            FraudEnergyEntity fraudEnergyEntity = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select FraudEnergy_ID,MagneticInfluenceKWh,MagneticInflueneceKVARhLag,MagneticInflueneceKVARhLead,MagneticInflueneceKVAh,"
                    +"ReverseEnergyKWh,ReverseEnergyKVAh,ReverseEnergyKVARhLag,ReverseEnergyKVARhLead,"
                    +"THDVoltageRPhase,THDVoltageYPhase,THDVoltageBPhase,THDCurrentRPhase,THDCurrentYPhase,THDCurrentBPhase,"
                    +"MeterData_ID from MeterData_FraudEnergy where ");
                builder.Append(string.Concat(FraudEnergy_ID, "=", ParameterName(FraudEnergy_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(FraudEnergy_ID), id, DbType.UInt32);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    fraudEnergyEntity = (FraudEnergyEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Fraud Energy data viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetDetailData(int id)", ex);
            }
            return fraudEnergyEntity;
        }

		public DataSet ListDataSet(int meterDataID)
		{
			DataSet ds = null;
			try
			{
				IDataHelper helper = DatabaseFactory.GetHelper();
				StringBuilder builder = new StringBuilder();
                builder.Append("Select A.MeterID,B.FraudEnergy_ID,B.MagneticInfluenceKWh,B.MagneticInflueneceKVARhLag,B.MagneticInflueneceKVARhLead,B.MagneticInflueneceKVAh,"
                    +"B.ReverseEnergyKWh,B.ReverseEnergyKVAh,B.ReverseEnergyKVARhLag,B.ReverseEnergyKVARhLead,"
                    + "THDVoltageRPhase,THDVoltageYPhase,THDVoltageBPhase,THDCurrentRPhase,THDCurrentYPhase,THDCurrentBPhase,"
                    +"B.MeterData_ID from MeterData_FraudEnergy B Inner Join meterdata A on A.MeterData_ID = B.MeterData_ID where ");
				builder.Append(string.Concat("B.",MeterData_ID, "=", ParameterName(MeterData_ID)));
				DataRequest request = new DataRequest(builder.ToString());
				request.AddParamter(ParameterName(MeterData_ID), meterDataID, DbType.UInt32);
				ds = new DataSet();
				ds = helper.FillDataSet(request, ds);
				if (ds.Tables[0].Rows.Count > 0)
					return ds;
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Fraud Energy data viewed"));
            }
			catch (CABException ex)    //Exception log for catch block
			{
                logger.Log(LOGLEVELS.Error, "ListDataSet(int meterDataID)", ex);
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
                builder.Append("Select FraudEnergy_ID,MagneticInfluenceKWh,MagneticInflueneceKVARhLag,MagneticInflueneceKVARhLead,MagneticInflueneceKVAh,"
                    +"ReverseEnergyKWh,ReverseEnergyKVAh,ReverseEnergyKVARhLag,ReverseEnergyKVARhLead,"
                    + "THDVoltageRPhase,THDVoltageYPhase,THDVoltageBPhase,THDCurrentRPhase,THDCurrentYPhase,THDCurrentBPhase,"
                    +"MeterData_ID from MeterData_FraudEnergy ");
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Fraud Energy data viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ListDataSet()", ex);
            }
            return dataSet;
        }

        public override IEntity RowToEntity(DataRow row)
        {
            if (row == null) return null;
            FraudEnergyEntity fraudEnergyEntity = new FraudEnergyEntity();
            if (NotNullAndNotDBNull(row, FraudEnergy_ID)) fraudEnergyEntity.FraudEnergy_ID = Convert.ToInt32(row[FraudEnergy_ID]);
            if (NotNullAndNotDBNull(row, MagneticInfluenceKWh)) fraudEnergyEntity.MagneticInfluenceKWh = Convert.ToString(row[MagneticInfluenceKWh]);
            if (NotNullAndNotDBNull(row, MagneticInflueneceKVARhLag)) fraudEnergyEntity.MagneticInflueneceKVARhLag = Convert.ToString(row[MagneticInflueneceKVARhLag]);
            if (NotNullAndNotDBNull(row, MagneticInflueneceKVARhLead)) fraudEnergyEntity.MagneticInflueneceKVARhLead = Convert.ToString(row[MagneticInflueneceKVARhLead]);
            if (NotNullAndNotDBNull(row, MagneticInflueneceKVAh)) fraudEnergyEntity.MagneticInflueneceKVAh = Convert.ToString(row[MagneticInflueneceKVAh]);
            if (NotNullAndNotDBNull(row, ReverseEnergyKWh)) fraudEnergyEntity.ReverseEnergyKWh = Convert.ToString(row[ReverseEnergyKWh]);
            if (NotNullAndNotDBNull(row, ReverseEnergyKVAh)) fraudEnergyEntity.ReverseEnergyKVAh = Convert.ToString(row[ReverseEnergyKVAh]);
            if (NotNullAndNotDBNull(row, ReverseEnergyKVARhLag)) fraudEnergyEntity.ReverseEnergyKVARhLag = Convert.ToString(row[ReverseEnergyKVARhLag]);
            if (NotNullAndNotDBNull(row, ReverseEnergyKVARhLead)) fraudEnergyEntity.ReverseEnergyKVARhLead = Convert.ToString(row[ReverseEnergyKVARhLead]);
            if (NotNullAndNotDBNull(row, THDVoltageRPhase)) fraudEnergyEntity.THDVoltageRPhase = Convert.ToString(row[THDVoltageRPhase]);
            if (NotNullAndNotDBNull(row, THDVoltageYPhase)) fraudEnergyEntity.THDVoltageYPhase = Convert.ToString(row[THDVoltageYPhase]);
            if (NotNullAndNotDBNull(row, THDVoltageBPhase)) fraudEnergyEntity.THDVoltageBPhase = Convert.ToString(row[THDVoltageBPhase]);
            if (NotNullAndNotDBNull(row, THDCurrentRPhase)) fraudEnergyEntity.THDCurrentRPhase = Convert.ToString(row[THDCurrentRPhase]);
            if (NotNullAndNotDBNull(row, THDCurrentYPhase)) fraudEnergyEntity.THDCurrentYPhase = Convert.ToString(row[THDCurrentYPhase]);
            if (NotNullAndNotDBNull(row, THDCurrentBPhase)) fraudEnergyEntity.THDCurrentBPhase = Convert.ToString(row[THDCurrentBPhase]);
            if (NotNullAndNotDBNull(row, MeterData_ID)) fraudEnergyEntity.MeterData_ID = Convert.ToInt32(row[MeterData_ID]);
            return fraudEnergyEntity;
        }

        public override IEntity InsertData(IList<IEntity> entities)
        {
            throw new NotImplementedException();
        }
    }
}
