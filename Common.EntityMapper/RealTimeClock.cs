#region NameSpaces
using System;
using System.Collections.Generic;
using CAB.Parser;
using System.Globalization;
using CABEntity;
using Hunt.EPIC.Logging;
#endregion
namespace Common.EntityMapper
{
    /// <summary>
    /// Used for mapping parsed RTC data to RTCEntity entity
    /// </summary>
    public class RealTimeClock
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(RealTimeClock).ToString());
        #endregion

        #region Properties
        #endregion

        #region Constructor
        #endregion

        #region Public Methods

        /// <summary>
        /// Used to fill RTC entity
        /// </summary>
        /// <param name="rtcData"></param>
        /// <returns></returns>
        public RTCEntity GetData(List<ProfileData> rtcData)
        {
            RTCEntity rtcEntity = new RTCEntity();
            try
            {
                if (rtcData[0].ListMeterDataPacket.Count > 0
                && rtcData[0].ListMeterDataPacket[0].ListDataElementValue.Count > 0)
                {
                    rtcEntity.RTC = rtcData[0].ListMeterDataPacket[0].ListDataElementValue[0].Value;
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetData(List<ProfileData> rtcData)", ex);
            }
           return rtcEntity;
           
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
