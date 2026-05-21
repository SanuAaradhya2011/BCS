using System;
using System.Collections.Generic;
using System.Text;
using CAB.Framework;
using CAB.Framework.Entity;
using CAB.DALC.Data;
using System.Data;

namespace CAB.BLL
{
    public class AreaMeterBLL : IBLL
    {
        AreaMeterDAL areaMeterDAL;
        public AreaMeterBLL()
        {
            areaMeterDAL = new AreaMeterDAL();
        }
        public void InsertData(List<IEntity> entities)
        {
            areaMeterDAL.InsertData(entities);
        }
        public void UpdateData(IEntity entity)
        {
            areaMeterDAL.UpdateData(entity);
        }
        public bool DeleteMeters(long areaID)
        {
            return areaMeterDAL.DeleteMeters(areaID);
        }
        public bool validateMeter(List<IEntity> entities)
        {
            return areaMeterDAL.ValidateMeter(entities);
        }
        public IEntity GetDetailData(int id)
        {
            return areaMeterDAL.GetDetailData(id);
        }
        public DataSet ListDataSet(long id)
        {
            return areaMeterDAL.ListDataSet(id);
        }
        public DataSet ListUnassignedMeters()
        {
            return areaMeterDAL.ListUnassignedMeters();
        }
        /* VBM - Add record */
        public void InsertData(IEntity entities)
        {
            areaMeterDAL.InsertData(entities);
        }
        /* VBM - Add record */
        /// <summary>
        /// bulk insert
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public void BatchInsert(List<IEntity> entities)
        {
            areaMeterDAL.BatchInsert(entities);
        }
    }
}
