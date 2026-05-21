/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 									        |
 * |											Date   : 25/03/2010 												|
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
    public class CMRIMasterDAL : DALBase
    {
        private string CMRI_ID = "CMRI_ID";
        private string CMRI_Number = "CMRI_Number";
        private string CMRI_Type = "CMRIType";
        private string CMRI_Description = "CMRI_Description";



        public override IEntity InsertData(IEntity entity)
        {
            IECCMRIMasterEntity cmriMasterEntity = null;
            if (entity == null)
                return cmriMasterEntity;
            cmriMasterEntity = entity as IECCMRIMasterEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert Into CMRI_Master(" + CMRI_Number + "," + CMRI_Type + "," + CMRI_Description + ") values(");
                builder.Append(string.Concat(ParameterName(CMRI_Number), ","));
                builder.Append(string.Concat(ParameterName(CMRI_Type),","));
                builder.Append(string.Concat(ParameterName(CMRI_Description), ")")); 
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(CMRI_Number), cmriMasterEntity.CMRI_Number, DbType.String, 35);
                request.AddParamter(ParameterName(CMRI_Type), cmriMasterEntity.CMRIType, DbType.String, 1);
                request.AddParamter(ParameterName(CMRI_Description), cmriMasterEntity.CMRI_Description, DbType.String, 50);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("CMRI ", cmriMasterEntity.CMRI_Number, " added"));
                Flag = true;
            }
            catch (CABException)
            {
                cmriMasterEntity = null;
            }
            return cmriMasterEntity;
        }

        public override bool UpdateData(IEntity entity)
        {
            bool Flag = false;
            try
            {
                IECCMRIMasterEntity cmriMasterEntity = entity as IECCMRIMasterEntity;
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Update CMRI_Master Set ");
                builder.Append(string.Concat(CMRI_Type,"=",ParameterName(CMRI_Type)) + ",");
                builder.Append(string.Concat(CMRI_Description, "=", ParameterName(CMRI_Description)));
                builder.Append(string.Concat(" Where ", CMRI_ID, "=", ParameterName(CMRI_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(CMRI_Type), cmriMasterEntity.CMRIType, DbType.String);
                request.AddParamter(ParameterName(CMRI_ID), cmriMasterEntity.CMRI_ID, DbType.Int64);
                request.AddParamter(ParameterName(CMRI_Description), cmriMasterEntity.CMRI_Description, DbType.String, 50);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("CMRI ", cmriMasterEntity .CMRI_Number, " modified"));
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
                IECCMRIMasterEntity cmriMasterEntity = entity as IECCMRIMasterEntity;
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete From CMRI_Master ");
                builder.Append(string.Concat(" Where ", CMRI_ID, "=", ParameterName(CMRI_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(CMRI_ID), cmriMasterEntity.CMRI_ID, DbType.Int64);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("CMRI ", cmriMasterEntity.CMRI_Number, " deleted"));
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
            IECCMRIMasterEntity cmriMasterEntity = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select CMRI_ID,CMRI_Number,CMRIType,CMRI_Description from CMRI_Master where ");
                builder.Append(string.Concat(CMRI_ID, "=", ParameterName(CMRI_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(CMRI_ID), id, DbType.Int64);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    cmriMasterEntity = (IECCMRIMasterEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("CMRI list viewed"));
            }
            catch (CABException)
            {
                cmriMasterEntity = null;
            }
            return cmriMasterEntity;
        }

        public bool GetDetailData(string cmriNumber)
        {
            bool Flag =false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select CMRI_ID,CMRI_Number,CMRIType,CMRI_Description from CMRI_Master where ");
                builder.Append(string.Concat(CMRI_Number, "=", ParameterName(CMRI_Number)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(CMRI_Number), cmriNumber, DbType.String, 16);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    Flag= true;
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Details of a specified CMRI viewed"));
            }
            catch (CABException)
            { 
            }
            return Flag;
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
                builder.Append("Select CMRI_ID,CMRI_Number as 'CMRI Number',CMRIType as 'CMRI Type',CMRI_Description as 'CMRI Description' from CMRI_Master");
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("CMRI list viewed"));
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
            IECCMRIMasterEntity cmriMasterEntity = new IECCMRIMasterEntity();
            if (NotNullAndNotDBNull(row, CMRI_ID)) cmriMasterEntity.CMRI_ID = Convert.ToInt64(row[CMRI_ID]);
            if (NotNullAndNotDBNull(row, CMRI_Number)) cmriMasterEntity.CMRI_Number = Convert.ToString(row[CMRI_Number]);
            if (NotNullAndNotDBNull(row, CMRI_Number)) cmriMasterEntity.CMRIType = Convert.ToString(row[CMRI_Type]);
            if (NotNullAndNotDBNull(row, CMRI_Description)) cmriMasterEntity.CMRI_Description = Convert.ToString(row[CMRI_Description]);
            return cmriMasterEntity;
        }

    

        public override IEntity InsertData(IList<IEntity> entities)
        {
            throw new NotImplementedException();
        }
        public DataSet ComboList()
        { 
        
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                //builder.Append("Select distinct concat(CMRIID ,'(',CMRIType,')') as 'DisplayMember', CMRIID as 'ValueMember' from meterdata ");
                //builder.Append("where CMRIType not in ('BCS') order by UploadingDateTime desc");
                builder.Append("Select distinct CMRIID from meterdata ");
                builder.Append("order by UploadingDateTime desc");
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("CMRI Details Viewed"));
            }
            catch (CABException)
            {
            }
            return dataSet;
      
        }
        
    }
}
