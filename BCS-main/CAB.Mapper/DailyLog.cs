#region NameSpaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.Entity;
using CAB.Parser;
using CABEntity;
#endregion
namespace CAB.Mapper
{
    /// <summary>
    /// mapping support for daily log configuration
    /// </summary>
    public class DailyLog
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
        /// Used to fill dailylog entity
        /// here data is filled is static as its not configurable.
       /// </summary>
       /// <returns></returns>
        public IECDailyLogEntity GetData()
        {
            IECDailyLogEntity dailyLogEntity = new IECDailyLogEntity();
            try
            {
                
                dailyLogEntity.CumulativeKWh = "1";
                dailyLogEntity.CumulativeKVAh = "1";
                dailyLogEntity.CumulativeKVARhLag = "1";
                dailyLogEntity.CumulativeKVARhLead = "1";
                dailyLogEntity.DailyMD1 = "1";
                dailyLogEntity.DailyMD2 = "1";               

            }
            catch (Exception)
            {

            }
            return dailyLogEntity;

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
