/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 							        |
 * |											Date   : 25/03/2010 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| 
 */

using System.Data;
using CAB.DALC.Data;
using CAB.IECFramework;
using CAB.IECFramework.Entity;

namespace CAB.BLL
{
    public class LoadSurveyParameterBLL : IBLL
    {
        LoadSurveyParameterDAL loadSurveyParameterDAL;

        public LoadSurveyParameterBLL()
        {
            loadSurveyParameterDAL = new LoadSurveyParameterDAL();
        }

        public bool DeleteData(long meterDataId)
        {
            return loadSurveyParameterDAL.DeleteData(meterDataId);
        }

        public IEntity InsertData(IEntity entity)
        {
            return loadSurveyParameterDAL.InsertData(entity);
        }

        public IEntity GetColumn(long meterDataId)
        {
            return loadSurveyParameterDAL.GetDetailData(meterDataId);
        }
    }
}
