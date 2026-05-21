#region NameSpaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.Entity;
using CAB.Parser;
using Hunt.EPIC.Logging;
#endregion
namespace Common.EntityMapper
{
    /// <summary>
    /// Mapping support for LSIP
    /// </summary>
    public class LSCapturePeriod
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(LSCapturePeriod).ToString());
        #endregion

        #region Properties
        #endregion

        #region Constructor
        #endregion

        #region Public Methods

        /// <summary>
        /// Used to fill LISP  data .
        /// </summary>
        /// <param name="namePlateData"></param>
        /// <returns></returns>
        public LSIPEntity GetData(List<ProfileData> loadSurveyCapturePeriodData)
        {
            LSIPEntity sipEntity = new LSIPEntity();
            try
            {
                if (loadSurveyCapturePeriodData[0].ListMeterDataPacket.Count > 0 && loadSurveyCapturePeriodData[0].ListMeterDataPacket[0].ListDataElementValue.Count > 0)
                {
                    string acutalData = loadSurveyCapturePeriodData[0].ListMeterDataPacket[0].ListDataElementValue[0].Value;
                    sipEntity.LSIPValue = Convert.ToInt32(acutalData);
                }

            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetData(List<ProfileData> loadSurveyCapturePeriodData)", ex);
            }
            return sipEntity;

        }


        #endregion

        #region Protected Methods
        #endregion

        #region Event Handlers

        #endregion

        #region Private Methods


        #endregion
    }
}
