using System;
using System.Data;
using CAB.IECFramework;
using CAB.IECFramework.Utility;
using System.Collections.Generic;
using CAB.DALC.Data;

namespace CAB.BLL
{
    public class DBGenerationBLL : IBLL
    {
        DBGenerationDAL dbGenerationDAL;

        public DBGenerationBLL()
        {
            dbGenerationDAL = new DBGenerationDAL();
        }

        public bool CreateCABAppDatabase()
        {
            string PortNumber = ConfigSettings.GetValue("PortNumber");
            ConfigSettings.ChangeNode("ConnectionString", "server=127.0.0.1;user=root;database=rubyapp;port=" + PortNumber + ";password=Password12;");
            if (ConfigSettings.GetValue("ConnectionString").IndexOf("rubyapp") > 0)
            {
                dbGenerationDAL.DropAllTable();
                ConfigSettings.ChangeNode("ConnectionString", "server=127.0.0.1;user=root;port=" + PortNumber + ";password=Password12;");
            }
            return dbGenerationDAL.CreateCABAppDatabase();
        }
    }
}
