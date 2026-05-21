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
    /// Mapping support for DIP
    /// </summary>
    public class DemandIntegrationPeriod
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
        /// Used to fill DIP  data .
        /// </summary>
        /// <param name="namePlateData"></param>
        /// <returns></returns>
        public MDWithIPEntity GetData(List<ProfileData> mdWithIPData)
        {
            MDWithIPEntity dipEntity = new MDWithIPEntity();
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
            catch (Exception)
            {

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
