using System;
using System.Collections.Generic;
using System.Text;
using CAB.Framework;
using CAB.Framework.Entity;
using CAB.DALC.Data;
using System.Data;

namespace CAB.BLL
{
    public class AreaBLL : IBLL
    {
        private AreaDAL areaDAL = null;
        public AreaBLL()
        {
            areaDAL = new AreaDAL();
        }
        public IEntity InsertData(IEntity entity)
        {
            return areaDAL.InsertData(entity, true);
        }
        public bool UpdateData(IEntity entity)
        {
            return areaDAL.UpdateData(entity);
        }
        public IEntity GetDetailData(int id)
        {
            return areaDAL.GetDetailData(id);
        }
        public IEntity GetDataforCMRI(int id)
        {
            return areaDAL.GetDataforCMRI(id);
        }
        public DataSet ListDataSet()
        {
          return  areaDAL.ListDataSet();
        }
        public DataSet ListDetails()
        {
            return areaDAL.ListDetails();
        }
        public bool ValidateData(IEntity entity)
        {
            return areaDAL.ValidateData(entity);
        }
        public bool DeleteData(long id)
        {
            return areaDAL.DeleteData(id);
        }
        /* VBM - Get Area ID */
        public IEntity ValidateData(int divisionId, int circleId, int regionId)
        {
            return areaDAL.GetDetailData(divisionId, circleId, regionId);
        }
        /* VBM - Get Area ID */
        /// <summary>
        /// gets all the data in area table
        /// </summary>
        /// <returns></returns>
        public DataSet GetAreaData()
        {
            return areaDAL.GetAreaData();
        }
        
    }
}
