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
    /// Mapping support for DIP
    /// </summary>
    public class MDWithIp
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(MDWithIp).ToString());
        #endregion

        #region Properties
        #endregion

        #region Constructor
        #endregion

        #region Public Methods

        /// <summary>
        /// Used to fill DIP  data .
        /// </summary>
        /// <param name="namePlateData"></param>
        /// <returns></returns>
        public E650MDWithIPEntity GetMappedEntity(List<ProfileData> mdWithIPData)
        {
            E650MDWithIPEntity dipEntity = new E650MDWithIPEntity();
            try
            {
                if (mdWithIPData[0].ListMeterDataPacket.Count > 0 && mdWithIPData[0].ListMeterDataPacket[0].ListDataElementValue.Count > 0)
                {
                    string acutalData = mdWithIPData[0].ListMeterDataPacket[0].ListDataElementValue[0].Value;
                    dipEntity.KWDemandType = dipEntity.KVADemandType = Convert.ToInt32(acutalData.Substring(0, 1)) == 0 ? "Block Demand" : "Sliding Demand";
                    dipEntity.KWInterval = dipEntity.KVAInterval = Int32.Parse(acutalData.Substring(1,3),System.Globalization.NumberStyles.HexNumber)/60;
                    dipEntity.KWSubInterval = dipEntity.KVASubInterval = int.Parse(acutalData.Substring(0, 1), System.Globalization.NumberStyles.HexNumber) * 5;
                }

            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetMappedEntity(List<ProfileData> mdWithIPData)", ex);
            }
            return dipEntity;

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
