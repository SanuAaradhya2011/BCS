using System;
using System.Text;
using CAB.IECFramework;
using CAB.IECFramework.Entity;
using System.Data;
using CAB.Entity;
using CAB.DALC.Data;
using System.Collections.Generic;
using CAB.IECFramework.Utility;

namespace CAB.BLL
{
    public class GSMConfigBLL : IBLL
    {
        //private ConsumerMeterDAL consumerMeterDAL = new ConsumerMeterDAL();
        private GSMConfigDAL gSMConfigDAL;

        public GSMConfigBLL()
        {
            gSMConfigDAL = new GSMConfigDAL();
        }
        public DataSet ComboList(bool flag)
        {
          return  new DataSet();
            //return consumerMeterDAL.ComboList(true);
        }
        public DataSet MeterIDList(string consumerNumber)
        {
            return new DataSet();
           // return consumerMeterDAL.ListDataSet("CN", consumerNumber);
        }
        public DataSet ListDataSet()
        {
            return gSMConfigDAL.ListDataSet();
        }
        public IEntity GetDetailData(string id)
        {
            return gSMConfigDAL.GetDetailData(id);
        }
        public IEntity GetDetailData(long id)
        {
            return gSMConfigDAL.GetDetailData(id);
        }
        public int GetCount(long id)
        {
            return gSMConfigDAL.GetCount(id);
        }
        public bool UpdateData(IEntity entity)
        {
            return gSMConfigDAL.UpdateData(entity);
        }
    }
}
