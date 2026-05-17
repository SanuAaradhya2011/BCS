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
using CAB.Framework;
using CAB.Framework.Entity;

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
        /// <summary>
        /// This method is use to get the columns list configued for load survey and set the status for NP/NL.
        /// </summary>
        /// <param name="meterDataId"></param>
        /// <returns></returns>
        public bool CanShowNPNL(long meterDataId, string meterType)
        {
            DataSet dSet = new DataSet();
            dSet = GetColumnNames(meterDataId);
            if (dSet != null && dSet.Tables[0].Rows.Count > 0)
            {
                //if (meterType == "1P-2W")
                //{
                //    if (dSet.Tables[0].Rows[0][0].ToString().Contains("blockEnergykWh,blockEnergykVAh"))
                //        return true;
                //    else
                //        return false;
                //}
                //else
                //{
                //    if (dSet.Tables[0].Rows[0][0].ToString().Contains("blockEnergykWh,blockEnergykvarhlag,blockEnergykvarhlead,blockEnergykVAh"))
                //        return true;
                //    else
                //        return false;
                //}
                /* The given parameters blockEnergykvarhlag,blockEnergykvarhlead,blockEnergykVAh in the condition is removed because for NP and NL there is only  blockEnergykWh is compulsory required and other parameters are configurable so its optional */
                if (dSet.Tables[0].Rows[0][0].ToString().Contains("blockEnergykWh"))
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
        public DataSet GetColumnNames(long meterDataId)
        {
            return loadSurveyParameterDAL.GetColumnNames(meterDataId);
        }
        /// <summary>
        /// Gets Column Names of load survey parameters For MeterID
        /// </summary>
        /// <param name="meterId">A meterID can have multiple files (each having different MeterData_ID</param>
        /// <returns></returns>
        public DataSet GetColumnNamesForMeterID(string meterId)
        {
            return loadSurveyParameterDAL.GetColumnNamesForMeterID(meterId);
        }

    }
}
