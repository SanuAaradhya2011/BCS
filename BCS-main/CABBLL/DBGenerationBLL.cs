using System;
using System.Data;
using CAB.Framework;
using CAB.Framework.Utility;
using System.Collections.Generic;
using CAB.DALC.Data;

namespace CAB.BLL
{
    public class DBGenerationBLL : IBLL
    {
        DBGenerationDAL dbGenerationDAL;
        private bool isMPKWCL = false;
        UtilityEntity localUtilityEntity;
        public DBGenerationBLL()
        {
            dbGenerationDAL = new DBGenerationDAL();
        }
        public DBGenerationBLL(UtilityEntity utilityEntity)
        {
            dbGenerationDAL = new DBGenerationDAL(utilityEntity);
            
        }
        public bool CreateCABAppDatabase()
        {

            string PortNumber = ConfigSettings.GetValue("PortNumber");
            //string appType = ConfigSettings.GetValue("ApplicationType"); 
            //if (ConfigInfo.GetConnectionString().IndexOf(appType) > 0)
            //{

            dbGenerationDAL.DeleteAllData();
            dbGenerationDAL.DropAllTable();
                ConfigSettings.ChangeNode("ConnectionString", "server=127.0.0.1;user=root;port=" + PortNumber + ";password=Password12;");
            //}
            return dbGenerationDAL.CreateCABAppDatabase();
        }
    }
}
