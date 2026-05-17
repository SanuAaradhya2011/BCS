using System;
using System.Collections.Generic;
using System.Text;
using CAB.IECFramework;
using CAB.DALC.Data;
using CAB.IECFramework.Entity;
using CAB.Entity;
using System.Data.Common;
using System.Data;

namespace CAB.BLL
{
    public class TamperTypeBLL : IBLL
    {
       private TamperTypeMasterDAL tamperTypeMasterDAL;
        public TamperTypeBLL()
        {
            tamperTypeMasterDAL = new TamperTypeMasterDAL();
        }
        public DataSet ExistOrInsert()
        {
            bool Flag = true;
            DataSet dataSet = tamperTypeMasterDAL.ListDataSet();
            if (dataSet != null)
            {
                if (dataSet.Tables.Count > 0)
                {
                    if (dataSet.Tables[0].Rows.Count > 0)
                        Flag = true;
                    else
                        Flag = false;
                }
                else
                    Flag = false;
            }
            if (!Flag)
                tamperTypeMasterDAL.InsertDefaultTamper();
            dataSet = tamperTypeMasterDAL.ListDataSet();
            return dataSet;
        }

        public DataSet ListDataSet()
        {
            DataTable dt = new CommonBLL().AutoNumberedTable(tamperTypeMasterDAL.ListDataSet().Tables[0]);
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            return ds;
        }

        public DataSet ListDataSetForReports()
        {
            DataTable dt = new CommonBLL().AutoNumberedTable(tamperTypeMasterDAL.ListDataSetForReports().Tables[0]);
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            return ds;
        }
    }
}
