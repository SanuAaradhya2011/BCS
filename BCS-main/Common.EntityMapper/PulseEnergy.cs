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
    /// Mapping support for PulseEnergy
    /// </summary>
    public class PulseEnergy
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(PulseEnergy).ToString());
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
        public PulseEnergyEntity GetData(List<ProfileData> pulseEnergyData)
        {
            PulseEnergyEntity pulseEnergyEntity = new PulseEnergyEntity();
            try
            {
                if (pulseEnergyData[0].ListMeterDataPacket.Count > 0 && pulseEnergyData[0].ListMeterDataPacket[0].ListDataElementValue.Count > 0)
                {
                    pulseEnergyEntity.PulseEnergyValue = pulseEnergyData[0].ListMeterDataPacket[0].ListDataElementValue[0].Value;
                }

            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetData(List<ProfileData> pulseEnergyData)", ex);
            }
            return pulseEnergyEntity;

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
