using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using CAB.DALC.Data.DataServices;
using CAB.Entity;
using CAB.Framework;
using CAB.Framework.Entity;
using System.Data.Common;
using CAB.Entity.Base;

namespace CAB.DALC.Data
{
    public class SystemSettingsDAL : DALBase
    {
        private const string System_Setting_ID = "System_Setting_ID";
        private const string Name = "Name";
        private const string Value = "Value";
        public void InsertSystemSettings()
        {
            string[] qry = new string[4];
            //qry[0] = "Insert Into metertype_master(MeterType_Name) values('1P3W')";
            qry[0] = "Insert Into system_settings(Name,Value) values('CMRI_COM_PORT','COM1')";
            qry[1] = "Insert Into system_settings(Name,Value) values('GSM_COM_PORTS','COM1')";
            //qry[1] = "Insert Into system_settings(Name,Value) values('GSM_COM_PORTS','COM2,COM3,COM4,COM5,COM6')";
            qry[2] = "Insert Into system_settings(Name,Value) values('USE_MULTIPLE_PORTS','0')";
            qry[3] = "Insert Into system_settings(Name,Value) values('COM_PORT','COM1')";

            IDataHelper helper = DatabaseFactory.GetHelper();
            for (int i = 0; i < qry.Length; i++)
            {
                DataRequest request = new DataRequest(qry[i]);
                helper.ExecuteNonQuery(request);
            }
        }
        public void UpdateSetting(string name,string value)
        {
            string query = "update system_settings set Value ='" + value + "'" + " where Name = '" + name + "'";
            IDataHelper helper = DatabaseFactory.GetHelper();
            DataRequest request = new DataRequest(query);
            helper.ExecuteNonQuery(request);
        }
        public string GetSettingValue(string name)
        {
            SystemSettingEntity systemSettingEntity = new SystemSettingEntity();
            systemSettingEntity.Value = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select System_Setting_ID,Name,Value from System_Settings where ");
                builder.Append(string.Concat(Name, "=", ParameterName(Name)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Name), name, DbType.String);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds != null &&
                    ds.Tables.Count > 0 &&
                    ds.Tables[0].Rows.Count > 0 &&
                    NotNullAndNotDBNull(ds.Tables[0].Rows[0], Value))
                {
                    systemSettingEntity.Value = ds.Tables[0].Rows[0][Value].ToString();
                }
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("system setting viewed"));
            }
            catch (CABException)
            {
                systemSettingEntity= null;
            }
            return systemSettingEntity.Value;
        }

        public override IEntity InsertData(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public override IEntity InsertData(IList<IEntity> entities)
        {
            throw new NotImplementedException();
        }

        public override bool UpdateData(IEntity entity)
        {
            throw new NotImplementedException();
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
        }

        public override IEntity RowToEntity(DataRow row)
        {
            throw new NotImplementedException();
        }
    }
}
