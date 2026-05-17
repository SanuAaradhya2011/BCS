/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon 								|
 * | 																												|
 * |											Author : Dhananjay Prasad Verma. 	 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| 
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using CAB.Framework;
using CAB.DALC.Data.DataServices;
using CAB.Entity;
using CAB.Framework.Entity;
using System.Data.Common;

namespace CAB.DALC.Data
{
     public class EnergyDAL : DALBase
    {   
        private string Energy_ID = "Energy_ID";
        private string TotalFundamentalActiveEnergy = "TotalFundamentalActiveEnergy";
        private string TotalActiveEnergy = "TotalActiveEnergy";
        private string TotalReactiveLagEnergy = "TotalReactiveLagEnergy";
         private string TotalReactiveLeadEnergy = "TotalReactiveLeadEnergy";
        private string TotalApparentEnergy = "TotalApparentEnergy";
        public EnergyDAL()
            : base("MeterData_Energy", "Energy_ID")
        {
        }

        public override bool InsertData(IEntity entity)
        {
            throw new NotImplementedException();
        }
        public override IEntity InsertData(IEntity entity, DbTransaction transaction, DbConnection connection)
        {
            EnergyEntity energyEntity = entity as EnergyEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert Into MeterData_Energy(TotalFundamentalActiveEnergy,TotalActiveEnergy,TotalReactiveLagEnergy,TotalReactiveLeadEnergy,TotalApparentEnergy) values(");
                builder.Append(string.Concat(ParameterName(TotalFundamentalActiveEnergy), ","));
                builder.Append(string.Concat(ParameterName(TotalActiveEnergy), ",")); 
                builder.Append(string.Concat(ParameterName(TotalReactiveLagEnergy), ","));
                builder.Append(string.Concat(ParameterName(TotalReactiveLeadEnergy), ","));
                builder.Append(string.Concat(ParameterName(TotalApparentEnergy), ")"));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(TotalFundamentalActiveEnergy), energyEntity.TotalFundamentalActiveEnergy, DbType.String, 40);
                request.AddParamter(ParameterName(TotalActiveEnergy), energyEntity.TotalActiveEnergy, DbType.String, 40);
                request.AddParamter(ParameterName(TotalReactiveLagEnergy), energyEntity.TotalReactiveLagEnergy, DbType.String, 40);
                request.AddParamter(ParameterName(TotalReactiveLeadEnergy), energyEntity.TotalReactiveLeadEnergy, DbType.String, 40);
                request.AddParamter(ParameterName(TotalApparentEnergy), energyEntity.TotalApparentEnergy, DbType.String, 40); 
                helper.ExecuteNonQuery(request, transaction, connection);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter Data Energy Added."));
                Flag = true;
            }
            catch (Exception) { }
            if (Flag)
                energyEntity.Energy_ID = long.Parse(this.GetPK());
            return energyEntity;
        } 
        public override bool UpdateData(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteData(IEntity entity)
        {
            EnergyEntity energyEntity = entity as EnergyEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete from MeterData_Energy where ");
                builder.Append(string.Concat(Energy_ID, "=", ParameterName(Energy_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Energy_ID), energyEntity.Energy_ID, DbType.UInt32);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter Data Energy Deleted."));
                Flag = true;
            }
            catch (Exception) { }
            return Flag;
        }

        public override IEntity GetDetailData(int id)
        {
            EnergyEntity energyEntity = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select Energy_ID,TotalFundamentalActiveEnergy,TotalActiveEnergy,TotalReactiveLagEnergy,TotalReactiveLeadEnergy,TotalApparentEnergy from MeterData_Energy where ");
                builder.Append(string.Concat(Energy_ID, "=", ParameterName(Energy_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Energy_ID), id, DbType.UInt32);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    energyEntity = (EnergyEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter Data Energy viewed."));

            }
            catch (CABException)
            { 
            }
            return energyEntity;
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
                builder.Append("Select Energy_ID,TotalFundamentalActiveEnergy,TotalActiveEnergy,TotalReactiveLagEnergy,TotalReactiveLeadEnergy,TotalApparentEnergy from MeterData_Energy ");
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter Data Energy viewed."));
            }
            catch (CABException)
            {
            }
            return dataSet;
        }

        public override IEntity RowToEntity(DataRow row)
        { 
            if (row == null) return null;
            EnergyEntity energyEntity = new EnergyEntity();
            if (NotNullAndNotDBNull(row, Energy_ID)) energyEntity.Energy_ID = Convert.ToInt32(row[Energy_ID]);
            if (NotNullAndNotDBNull(row, TotalFundamentalActiveEnergy)) energyEntity.TotalFundamentalActiveEnergy = Convert.ToString(row[TotalFundamentalActiveEnergy]);
            if (NotNullAndNotDBNull(row, TotalActiveEnergy)) energyEntity.TotalActiveEnergy = Convert.ToString(row[TotalActiveEnergy]);
            if (NotNullAndNotDBNull(row, TotalReactiveLagEnergy)) energyEntity.TotalReactiveLagEnergy = Convert.ToString(row[TotalReactiveLagEnergy]);
            if (NotNullAndNotDBNull(row, TotalReactiveLeadEnergy)) energyEntity.TotalReactiveLeadEnergy = Convert.ToString(row[TotalReactiveLeadEnergy]);
            if (NotNullAndNotDBNull(row, TotalApparentEnergy)) energyEntity.TotalApparentEnergy = Convert.ToString(row[TotalApparentEnergy]);
            return energyEntity;
        } 
    }
}
