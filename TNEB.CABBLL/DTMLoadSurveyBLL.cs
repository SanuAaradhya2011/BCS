using System;
using System.Collections.Generic; 
using System.Text;
using CAB.DALC.Data;
using CAB.IECFramework.Entity;
using CAB.IECFramework;
using System.Data;

namespace CAB.BLL
{
    public class DTMLoadSurveyBLL:IBLL
    {
        DTMLoadSurveyDAL dTMLoadSurveyDAL;

        public DTMLoadSurveyBLL()
        {
            dTMLoadSurveyDAL = new DTMLoadSurveyDAL();
        }

        public bool DeleteData(long meterDataId)
        {
            return dTMLoadSurveyDAL.DeleteData(meterDataId);
        }

        public IEntity InsertData(IEntity entity)
        {
            return dTMLoadSurveyDAL.InsertData(entity);
        }
        public IEntity InsertData(List<IEntity> entities)
        {
            return dTMLoadSurveyDAL.InsertData(entities);
        }
        public DataSet ListDataSet(long activeMeterData_ID)
        {
            return dTMLoadSurveyDAL.ListDataSet(activeMeterData_ID);
        }
        public long GetFromDate(long meterDataID)
        {
            return dTMLoadSurveyDAL.GetFromDate(meterDataID);
        }
        public long GetToDate(long meterDataID)
        {
            return dTMLoadSurveyDAL.GetToDate(meterDataID);
        }
        public DataSet ListDataSet(long meterDataId, long fromDate, long toDate)
        {
            return CommonBLL.ConvertDTMLoadSurvey(dTMLoadSurveyDAL.ListDataSet(meterDataId, fromDate, toDate));
        }
    }
} 
