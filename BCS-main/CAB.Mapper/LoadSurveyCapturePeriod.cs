#region NameSpaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.Parser;
#endregion
namespace CAB.Mapper
{
    /// <summary>
    /// Used for mmaping SIP 
    /// </summary>
    public class LoadSurveyCapturePeriod
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
        /// Used to Fill SIP data 
        /// </summary>
        /// <param name="namePlateData"></param>
        /// <returns></returns>
        public int GetData(List<ProfileData> loadSurveyCaptureData)
        {
            int resultData =0;
            try
            {
                if (loadSurveyCaptureData.Count > 0 && loadSurveyCaptureData[0].ListMeterDataPacket.Count > 0 && loadSurveyCaptureData[0].ListMeterDataPacket[0].ListDataElementValue.Count > 0)
                {
                    resultData = Convert.ToInt32(loadSurveyCaptureData[0].ListMeterDataPacket[0].ListDataElementValue[0].Value);
                }

            }
            catch (Exception)
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
