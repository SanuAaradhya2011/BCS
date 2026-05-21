#region NameSpaces
using System;
using System.Collections.Generic;
using CAB.Parser;
using System.Globalization;
#endregion
namespace CAB.Mapper
{
    /// <summary>
    /// Used for mapping parsed RTC data to RTCEntity entity
    /// </summary>
    public class RealTimeClock
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
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
        public string GetData(List<ProfileData> rtcData)
        {
            string resultData = string.Empty;
            try
            {
                resultData = rtcData[0].ListMeterDataPacket[0].ReadingDate.ToString("dd/MM/yyyy HH:mm:ss");                
            }
            catch(Exception)
            {

            }
           return resultData;
           
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
