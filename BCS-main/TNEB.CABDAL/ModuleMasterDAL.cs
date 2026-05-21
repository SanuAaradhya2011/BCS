using System.Text;
using CAB.DALC.Data.DataServices;
using CAB.Entity.Base;
using CAB.IECFramework;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using CAB.Entity;
using System;
using CAB.IECFramework.Entity;
using System.Data.Common;

namespace CAB.DALC.Data
{
	public class ModuleMasterDAL : DALBase
	{
        private string Module_ID = "Module_ID";
        private string Module_Name = "Module_Name";
		public DataSet GetAllData()
		{
			DataSet dSet = null;
			try
			{
				dSet = new DataSet();
				IDataHelper helper = DatabaseFactory.GetHelper();
				StringBuilder builder = new StringBuilder();
				builder.Append("Select * from module_master");
				DataRequest request = new DataRequest(builder.ToString());
				helper.FillDataSet(request, dSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("List of various modules viewed"));
			}
			catch (CABException)
			{
				dSet = null;
			}
			return dSet;
		}
        public void DeleteAllData()
        {
            string[] qry = new string[49];
            qry[0] = "delete from areamaster";
            qry[1] = "delete from areameter_master";
            qry[2] = "delete from asciiexportsettings";
            qry[3] = "delete from category_master";
            qry[4] = "delete from categoryright";
            qry[5] = "delete from circle_master";
            qry[6] = "delete from consumer_master";
            qry[7] = "delete from consumerexportsettings";
            qry[8] = "delete from consumermeter";
            qry[9] = "delete from consumertype_master";
            qry[10] = "delete from designation_master";
            qry[11] = "delete from division_master";
            qry[12] = "delete from dtmdailyprofileparameter";
            qry[13] = "delete from exceptionlog";
            qry[14] = "delete from fileupload_master";
            qry[15] = "delete from group_master";
            qry[16] = "delete from history_master";
            qry[17] = "delete from loadsurveyparameter";
            qry[18] = "delete from meter_master";
            qry[19] = "delete from meterdata";
            qry[20] = "delete from meterdata_billing";
            qry[21] = "delete from meterdata_dtmdailyprofile";
            qry[22] = "delete from meterdata_dtmloadsurvey";
            qry[23] = "delete from meterdata_fraudenergy";
            qry[24] = "delete from meterdata_general";
            qry[25] = "delete from meterdata_instantpower";
            qry[26] = "delete from meterdata_loadsurvey";
            qry[27] = "delete from meterdata_phasor";
            qry[28] = "delete from meterdata_powerfactor";
            qry[29] = "delete from meterdata_programming";
            qry[30] = "delete from meterdata_rtcupdate";
            qry[31] = "delete from meterdata_tampercounter";
            qry[32] = "delete from meterdata_tampercountergeneral";
            qry[33] = "delete from meterdata_tampersnapshot";
            qry[34] = "delete from meterdata_tariffinformation";
            qry[35] = "delete from metermodel_master";
            qry[36] = "delete from metertype_master";
            qry[37] = "delete from meterunit_master";
            qry[38] = "delete from module_master";
            qry[39] = "delete from modulecategory_master";
            qry[40] = "delete from rcdmeter_master";
            qry[41] = "delete from region_master";
            qry[42] = "delete from subgroup_master";
            qry[43] = "delete from subgroupmeter_master";
            qry[44] = "delete from suspectedconsumer";
            qry[45] = "delete from tampertype_master";
            qry[46] = "delete from userinformation";
            qry[47] = "delete from userlogactivity";
            qry[48] = "delete from userrights"; 
            IDataHelper helper = DatabaseFactory.GetHelper();
            for (int i = 0; i < 8; i++)
            {
                DataRequest request = new DataRequest(qry[i]);
                helper.ExecuteNonQuery(request);
            }
        }
        public void InsertDefaultModule()
        {
            string[] qry = new string[8];
			qry[0] = "Insert Into module_master(Module_Name) values('User Administrator')";
			qry[1] = "Insert Into module_master(Module_Name) values('Programming')";
			qry[2] = "Insert Into module_master(Module_Name) values('Reports View')";
			qry[3] = "Insert Into module_master(Module_Name) values('Definition')";
			qry[4] = "Insert Into module_master(Module_Name) values('Schedule')";
			qry[5] = "Insert Into module_master(Module_Name) values('Data Export/Import')";
			qry[6] = "Insert Into module_master(Module_Name) values('Data Readout')";
			qry[7] = "Insert Into module_master(Module_Name) values('Data Archive')";

            IDataHelper helper = DatabaseFactory.GetHelper();
            for (int i = 0; i < 8; i++)
            {
                DataRequest request = new DataRequest(qry[i]);
                helper.ExecuteNonQuery(request);
            }
            UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Default modules inserted"));
        }

        public int GetModuleIdByName(string moduleName)
        {
            int moduleId = 0;   
            try
            {                          
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select Module_ID from module_master where ");
                builder.Append(string.Concat(Module_Name,"=",ParameterName(Module_Name)));   
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Module_Name), moduleName, DbType.String,35);
                object obj = helper.ExecuteScalar(request);
                moduleId = Convert.ToInt32(obj);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Module ID retrieved"));
            }
            catch (CABException)
            {
                moduleId=0;
            }
            return moduleId;
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
