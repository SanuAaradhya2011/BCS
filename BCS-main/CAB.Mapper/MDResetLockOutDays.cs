#region NameSpaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.Entity;
using CAB.Parser;
#endregion 
namespace CAB.Mapper
{
    /// <summary>
    /// Mapping support for Lockout days
    /// </summary>
    public class MDResetLockOutDays
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
        /// Used to Reset lockout data
        /// </summary>
        /// <param name="namePlateData"></param>
        /// <returns></returns>
        public string GetData(List<ProfileData> resetLockoutData)
        {
            string lockoutData ="0";
            try
            {
                if (resetLockoutData[0].ListMeterDataPacket.Count > 0 && resetLockoutData[0].ListMeterDataPacket[0].ListDataElementValue.Count > 0)
                {
                    string actualData = resetLockoutData[0].ListMeterDataPacket[0].ListDataElementValue[0].Value;
                    lockoutData = ((int.Parse(actualData, System.Globalization.NumberStyles.HexNumber)) / (24 * 4)).ToString();
                }
            }
            catch (Exception)
            {
                
            }
            return lockoutData; 

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
