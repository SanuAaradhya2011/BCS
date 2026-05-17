/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Deep. 	 												|
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
using CAB.Framework;
using CAB.Framework.Entity;
using CAB.Framework.Utility;
using Hunt.EPIC.Logging;

namespace CAB.DALC.Data
{

    public class LoadSwitchDAL : DALBase
    {
        //private string ColumnsNames = "ColumnsNames";
        private string MeterData_ID = "MeterData_ID";

        private string RTC = "RTC";
        private string ControlEventConnectDisconnect = "ControlEventConnectDisconnect";
        private string Switchoperationreason = "Switchoperationreason";
        private string Cumulativeenergykwh = "Cumulativeenergykwh";
        private string CumulativeenergykwhTZ1 = "CumulativeenergykwhTZ1";
        private string CumulativeenergykwhTZ2 = "CumulativeenergykwhTZ2";
        private string CumulativeenergykwhTZ3 = "CumulativeenergykwhTZ3";
        private string CumulativeenergykwhTZ4 = "CumulativeenergykwhTZ4";
        private string CumulativeenergykwhTZ5 = "CumulativeenergykwhTZ5";
        private string CumulativeenergykwhTZ6 = "CumulativeenergykwhTZ6";
        private string CumulativeenergykwhTZ7 = "CumulativeenergykwhTZ7";
        private string CumulativeenergykwhTZ8 = "CumulativeenergykwhTZ8";
        private string Cumulativeenergykvah = "Cumulativeenergykvah";
        private string CumulativeenergykvahTZ1 = "CumulativeenergykvahTZ1";
        private string CumulativeenergykvahTZ2 = "CumulativeenergykvahTZ2";
        private string CumulativeenergykvahTZ3 = "CumulativeenergykvahTZ3";
        private string CumulativeenergykvahTZ4 = "CumulativeenergykvahTZ4";
        private string CumulativeenergykvahTZ5 = "CumulativeenergykvahTZ5";
        private string CumulativeenergykvahTZ6 = "CumulativeenergykvahTZ6";
        private string CumulativeenergykvahTZ7 = "CumulativeenergykvahTZ7";
        private string CumulativeenergykvahTZ8 = "CumulativeenergykvahTZ8";

        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(LoadSwitchDAL).ToString());
        public LoadSwitchDAL()
            : base("MeterData_ID", "MeterData_ID")
        {
        }
       private DataRequest GetRequest(IEntity entity)
        {
             if (entity == null)
              return null;
            LoadSwitchEntity loadswitchEntity = entity as LoadSwitchEntity;
            StringBuilder builder = new StringBuilder();
            bool Flag = false;
          
            //try
            //{
                IDataHelper helper = DatabaseFactory.GetHelper();
                builder.Append("Insert Into meterdata_loadswitch(MeterData_Id,Controleventconnectdisconnect,RTC,Switchoperationreason,Cumulativeenergykwh,CumulativeenergykwhTZ1," +
                "CumulativeenergykwhTZ2,CumulativeenergykwhTZ3,CumulativeenergykwhTZ4,CumulativeenergykwhTZ5,CumulativeenergykwhTZ6," +
                "CumulativeenergykwhTZ7,CumulativeenergykwhTZ8,Cumulativeenergykvah,CumulativeenergykvahTZ1,CumulativeenergykvahTZ2,CumulativeenergykvahTZ3,CumulativeenergykvahTZ4,CumulativeenergykvahTZ5,CumulativeenergykvahTZ6,CumulativeenergykvahTZ7,CumulativeenergykvahTZ8) values(");
                builder.Append(string.Concat(ParameterName(MeterData_ID), ","));
                builder.Append(string.Concat(ParameterName(ControlEventConnectDisconnect), ","));
                builder.Append(string.Concat(ParameterName(RTC), ","));
                builder.Append(string.Concat(ParameterName(Switchoperationreason), ","));
                builder.Append(string.Concat(ParameterName(Cumulativeenergykwh), ","));
                builder.Append(string.Concat(ParameterName(CumulativeenergykwhTZ1), ","));
                builder.Append(string.Concat(ParameterName(CumulativeenergykwhTZ2), ","));
                builder.Append(string.Concat(ParameterName(CumulativeenergykwhTZ3), ","));
                builder.Append(string.Concat(ParameterName(CumulativeenergykwhTZ4), ","));
                builder.Append(string.Concat(ParameterName(CumulativeenergykwhTZ5), ","));
                builder.Append(string.Concat(ParameterName(CumulativeenergykwhTZ6), ","));
                builder.Append(string.Concat(ParameterName(CumulativeenergykwhTZ7), ","));
                builder.Append(string.Concat(ParameterName(CumulativeenergykwhTZ8), ","));
                builder.Append(string.Concat(ParameterName(Cumulativeenergykvah), ","));
                builder.Append(string.Concat(ParameterName(CumulativeenergykvahTZ1), ","));
                builder.Append(string.Concat(ParameterName(CumulativeenergykvahTZ2), ","));
                builder.Append(string.Concat(ParameterName(CumulativeenergykvahTZ3), ","));
                builder.Append(string.Concat(ParameterName(CumulativeenergykvahTZ4), ","));
                builder.Append(string.Concat(ParameterName(CumulativeenergykvahTZ5), ","));
                builder.Append(string.Concat(ParameterName(CumulativeenergykvahTZ6), ","));
                builder.Append(string.Concat(ParameterName(CumulativeenergykvahTZ7), ","));
                builder.Append(string.Concat(ParameterName(CumulativeenergykvahTZ8), ")"));
                

                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), loadswitchEntity.MeterData_ID, DbType.Int64); 
                request.AddParamter(ParameterName(ControlEventConnectDisconnect), loadswitchEntity.ControlEventConnectDisconnect , DbType.String,40 );
                request.AddParamter(ParameterName(RTC), loadswitchEntity.RealTimeClock, DbType.Int64);
                request.AddParamter(ParameterName(Switchoperationreason), loadswitchEntity.ReasonSwitchOperation, DbType.String, 40);
                request.AddParamter(ParameterName(Cumulativeenergykwh), loadswitchEntity.CumulativeEnergykwh, DbType.String, 40);
                request.AddParamter(ParameterName(CumulativeenergykwhTZ1), loadswitchEntity.CumulativeEnergykwhTZ1, DbType.String, 40);
                request.AddParamter(ParameterName(CumulativeenergykwhTZ2), loadswitchEntity.CumulativeEnergykwhTZ2, DbType.String, 40);
                request.AddParamter(ParameterName(CumulativeenergykwhTZ3), loadswitchEntity.CumulativeEnergykwhTZ3, DbType.String, 40);
                request.AddParamter(ParameterName(CumulativeenergykwhTZ4), loadswitchEntity.CumulativeEnergykwhTZ4, DbType.String, 40);
                request.AddParamter(ParameterName(CumulativeenergykwhTZ5), loadswitchEntity.CumulativeEnergykwhTZ5, DbType.String, 40);
                request.AddParamter(ParameterName(CumulativeenergykwhTZ6), loadswitchEntity.CumulativeEnergykwhTZ6, DbType.String, 40);
                request.AddParamter(ParameterName(CumulativeenergykwhTZ7), loadswitchEntity.CumulativeEnergykwhTZ7, DbType.String, 40);
                request.AddParamter(ParameterName(CumulativeenergykwhTZ8), loadswitchEntity.CumulativeEnergykwhTZ8, DbType.String, 40);
                request.AddParamter(ParameterName(Cumulativeenergykvah), loadswitchEntity.CumulativeEnergykvah, DbType.String, 40);
                request.AddParamter(ParameterName(CumulativeenergykvahTZ1), loadswitchEntity.CumulativeEnergykvahTZ1, DbType.String, 40);
                request.AddParamter(ParameterName(CumulativeenergykvahTZ2), loadswitchEntity.CumulativeEnergykvahTZ2, DbType.String, 40);
                request.AddParamter(ParameterName(CumulativeenergykvahTZ3), loadswitchEntity.CumulativeEnergykvahTZ3, DbType.String, 40);
                request.AddParamter(ParameterName(CumulativeenergykvahTZ4), loadswitchEntity.CumulativeEnergykvahTZ4, DbType.String, 40);
                request.AddParamter(ParameterName(CumulativeenergykvahTZ5), loadswitchEntity.CumulativeEnergykvahTZ5, DbType.String, 40);
                request.AddParamter(ParameterName(CumulativeenergykvahTZ6), loadswitchEntity.CumulativeEnergykvahTZ6, DbType.String, 40);
                request.AddParamter(ParameterName(CumulativeenergykvahTZ7), loadswitchEntity.CumulativeEnergykvahTZ7, DbType.String, 40);
                request.AddParamter(ParameterName(CumulativeenergykvahTZ8), loadswitchEntity.CumulativeEnergykvahTZ8, DbType.String, 40);
                 

            //}
            //catch (Exception ex)    //Exception log for catch block 
            //{
            //    logger.Log(LOGLEVELS.Error, "InsertData(IEntity entity)", ex);
            //}
            return request;
        }

        
        public DataSet GetMeterData(int meterDataID)
        {

            DataSet dataSet = new DataSet();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select A.Controleventconnectdisconnect,A.RTC,A.Switchoperationreason,A.Cumulativeenergykwh,A.CumulativeenergykwhTZ1,A.CumulativeenergykwhTZ2,A.CumulativeenergykwhTZ3,A.CumulativeenergykwhTZ4,A.CumulativeenergykwhTZ5,A.CumulativeenergykwhTZ6,A.CumulativeenergykwhTZ7,A.CumulativeenergykwhTZ8,A.Cumulativeenergykvah,A.CumulativeenergykvahTZ1,A.CumulativeenergykvahTZ2,A.CumulativeenergykvahTZ3,A.CumulativeenergykvahTZ4,A.CumulativeenergykvahTZ5,A.CumulativeenergykvahTZ6,A.CumulativeenergykvahTZ7,A.CumulativeenergykvahTZ8");
                builder.Append(" from meterdata_loadswitch A, MeterData B where ");
                builder.Append("A.MeterData_ID=B.MeterData_ID and A.");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataID, DbType.Int64);
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Loadswitch data viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetMeterData(int meterDataID)", ex);
                dataSet = null;
            }
            return dataSet;
        }

        public override IEntity InsertData(IEntity entity)
        {
            return null;
        }
       
        public override bool UpdateData(IEntity entity)
        {
            return false;
        }
        public override bool DeleteData(IEntity entity)
        {
                 return false;
        }
        public override IEntity GetDetailData(int id)
        {
            return null;
        }
        public override IList<IEntity> ListData()
        {
            return null;
        }
        public override DataSet ListDataSet()
        {
            return null;
        }
        public override IEntity RowToEntity(DataRow row)
        {
            return null;
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
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "IEntity InsertData(IList<IEntity> entities)", ex);
            }
            return null;
        }
        

       
      
    }
}
