/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 									        |
 * |											Date   : 06/05/2010 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| 
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using CAB.DALC.Data.DataServices;
using CAB.Entity;
using CAB.IECFramework;
using CAB.IECFramework.Entity;
using System.Data.Common;

namespace CAB.DALC.Data
{
    public class TamperTypeMasterDAL : DALBase
    {
        private string TamperTypeID = "TamperTypeID";
        private string TamperType = "TamperType";

        public void InsertDefaultTamper()
        {
            string[] qry = new string[23];

            qry[0] = "Insert Into TamperType_Master(TamperTypeID,TamperType) values(161,'Voltage Imbalance R Phase Tamper')";
            qry[1] = "Insert Into TamperType_Master(TamperTypeID,TamperType) values(162,'Voltage Imbalance Y Phase Tamper')";
            qry[2] = "Insert Into TamperType_Master(TamperTypeID,TamperType) values(163,'Voltage Imbalance B Phase Tamper')";
            qry[3] = "Insert Into TamperType_Master(TamperTypeID,TamperType) values(164,'Missing Potential R Phase Tamper')";
            qry[4] = "Insert Into TamperType_Master(TamperTypeID,TamperType) values(165,'Missing Potential Y Phase Tamper')";
            qry[5] = "Insert Into TamperType_Master(TamperTypeID,TamperType) values(166,'Missing Potential B Phase Tamper')";       
            qry[6] = "Insert Into TamperType_Master(TamperTypeID,TamperType) values(177,'CT Short Tamper')";
            qry[7] = "Insert Into TamperType_Master(TamperTypeID,TamperType) values(178,'CT Open R Phase Tamper')";
            qry[8] = "Insert Into TamperType_Master(TamperTypeID,TamperType) values(179,'CT Open Y Phase Tamper')";
            qry[9] = "Insert Into TamperType_Master(TamperTypeID,TamperType) values(180,'CT Open B Phase Tamper')";
            qry[10] = "Insert Into TamperType_Master(TamperTypeID,TamperType) values(187,'One Phase Neutral Absent Tamper')";        
            qry[11] = "Insert Into TamperType_Master(TamperTypeID,TamperType) values(167,'Voltage Phase Reversal Tamper')";//TODO: Need Attension its Tamper ID in Doc is 194
            qry[12] = "Insert Into TamperType_Master(TamperTypeID,TamperType) values(195,'Current Imbalance R Phase Tamper')";
            qry[13] = "Insert Into TamperType_Master(TamperTypeID,TamperType) values(196,'Current Imbalance Y Phase Tamper')";
            qry[14] = "Insert Into TamperType_Master(TamperTypeID,TamperType) values(197,'Current Imbalance B Phase Tamper')";
            qry[15] = "Insert Into TamperType_Master(TamperTypeID,TamperType) values(198,'Current Reversal R Phase Tamper')";
            qry[16] = "Insert Into TamperType_Master(TamperTypeID,TamperType) values(199,'Current Reversal Y Phase Tamper')";
            qry[17] = "Insert Into TamperType_Master(TamperTypeID,TamperType) values(200,'Current Reversal B Phase Tamper')";
            qry[18] = "Insert Into TamperType_Master(TamperTypeID,TamperType) values(209,'Magnetic Influence Tamper')";
            qry[19] = "Insert Into TamperType_Master(TamperTypeID,TamperType) values(210,'Neutral Disturbance Tamper')";
            qry[20] = "Insert Into TamperType_Master(TamperTypeID,TamperType) values(211,'Front Cover Opening Tamper')";
            //qry[] = "Insert Into TamperType_Master(TamperTypeID,TamperType) values(212,'Terminal cover opening Tamper')";
            qry[21] = "Insert Into TamperType_Master(TamperTypeID,TamperType) values(226,'Total Tamper')";
            qry[22] = "Insert Into TamperType_Master(TamperTypeID,TamperType) values(225,'Power On/Off')";
        

            IDataHelper helper = DatabaseFactory.GetHelper();
            for (int i = 0; i < 23; i++)
            {
                if (qry[i] != null)
                {
                    DataRequest request = new DataRequest(qry[i]);
                    helper.ExecuteNonQuery(request);
                }
            }
        }

        public override IEntity InsertData(IEntity entity)
        {
            TamperTypeEntity tamperTypeEntity = null;
            if (entity == null)
                return tamperTypeEntity;
              tamperTypeEntity = entity as TamperTypeEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert Into TamperType_Master(TamperType) values(");
                builder.Append(string.Concat(ParameterName(TamperType), ")")); 
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(TamperType), tamperTypeEntity.TamperType, DbType.String, 50); 
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Tamper Type ", tamperTypeEntity.TamperType, " added"));
                Flag = true;
            }
            catch (CABException)
            {
                tamperTypeEntity = null;
            }
            return tamperTypeEntity;
        }

        public override bool UpdateData(IEntity entity)
        {
            bool Flag = false;
            try
            {
                TamperTypeEntity tamperTypeEntity = entity as TamperTypeEntity;
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Update TamperType_Master Set ");
                builder.Append(string.Concat( TamperType, "=", ParameterName(TamperType ))); 
                builder.Append(string.Concat(" Where ", TamperTypeID, "=", ParameterName(TamperTypeID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(TamperTypeID), tamperTypeEntity.TamperTypeID, DbType.Int32);
                request.AddParamter(ParameterName(TamperType), tamperTypeEntity.TamperType, DbType.String, 50); 
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Tamper Type ", tamperTypeEntity.TamperType, " modified"));
                Flag = true;
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
                TamperTypeEntity tamperTypeEntity = entity as TamperTypeEntity;
                if (tamperTypeEntity == null)
                    return false;
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete From TamperType_Master ");
                builder.Append(string.Concat(" Where ", TamperTypeID, "=", ParameterName(TamperTypeID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(TamperTypeID), tamperTypeEntity.TamperTypeID, DbType.Int64);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Tamper Type ", tamperTypeEntity.TamperType, " deleted"));
                Flag = true;
            }
            catch (CABException)
            {
                Flag = false;
            }
            return Flag;
        }

        public override IEntity GetDetailData(int id)
        {
            TamperTypeEntity TamperTypeEntity = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select TamperTypeID,TamperType from TamperType_Master where ");
                builder.Append(string.Concat(TamperTypeID, "=", ParameterName(TamperTypeID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(TamperTypeID), id, DbType.Int64);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    TamperTypeEntity = (TamperTypeEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Tamper type viewed"));
            }
            catch (CABException)
            {
                TamperTypeEntity = null;
            }
            return TamperTypeEntity;
        }

        public IEntity GetDetailData(string tamperType)
        {
            TamperTypeEntity TamperTypeEntity = new TamperTypeEntity(); 
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select TamperTypeID,TamperType from TamperType_Master where ");
                builder.Append(string.Concat(TamperType, "=", ParameterName(TamperType)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(TamperType), tamperType, DbType.String, 10);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    TamperTypeEntity = (TamperTypeEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Tamper type viewed"));
            }
            catch (CABException)
            {
            }
            return TamperTypeEntity;
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
                builder.Append("Select TamperTypeID,TamperType from TamperType_Master order by TamperTypeID");
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Tamper type viewed"));
            }
            catch (CABException)
            {
                dataSet = null;
            }
            return dataSet;
        }

        public DataSet ListDataSetForReports()
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select TamperTypeID,TamperType from TamperType_Master where TamperTypeID != 226 order by TamperTypeID");
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Tamper type viewed"));
            }
            catch (CABException)
            {
                dataSet = null;
            }
            return dataSet;
        }

        public override IEntity RowToEntity(DataRow row)
        {
            if (row == null) return null;
            TamperTypeEntity tamperTypeEntity = new TamperTypeEntity();
            if (NotNullAndNotDBNull(row, TamperTypeID)) tamperTypeEntity.TamperTypeID = Convert.ToInt32(row[TamperTypeID]);
            if (NotNullAndNotDBNull(row, TamperType)) tamperTypeEntity.TamperType = Convert.ToString(row[TamperType]);
            return tamperTypeEntity;
        }

        public override IEntity InsertData(IList<IEntity> entities)
        {
            throw new NotImplementedException();
        }
    }
}



