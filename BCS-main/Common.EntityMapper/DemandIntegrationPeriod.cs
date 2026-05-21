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
    public class DemandIntegrationPeriod
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(DemandIntegrationPeriod).ToString());
        #endregion

        #region Properties
        #endregion

        #region Constructor
        #endregion

        #region Public Methods

       /// <summary>
        /// Used to fill DIP  data .
       /// </summary>
       /// <param name="demandIntegrationPeriodData"></param>
       /// <param name="isUpload"></param>
       /// <returns></returns>
        public DIPEntity GetData(List<ProfileData> demandIntegrationPeriodData, bool isUpload)
        {
            DIPEntity dipEntity = new DIPEntity();
            try
            {
                if (demandIntegrationPeriodData[0].ListMeterDataPacket.Count > 0 && demandIntegrationPeriodData[0].ListMeterDataPacket[0].ListDataElementValue.Count > 0)
                {
                    string acutalData = string.Empty;
                    if (demandIntegrationPeriodData.Count > 1)
                    {
                        //In case of CDF converter sliding demand will show as a block demand.
                        if (isUpload == false)
                            acutalData = demandIntegrationPeriodData[0].ListMeterDataPacket[0].ListDataElementValue[0].Value;
                        else
                            acutalData = demandIntegrationPeriodData[1].ListMeterDataPacket[0].ListDataElementValue[0].Value;
                    }
                    else
                    {
                        acutalData = demandIntegrationPeriodData[0].ListMeterDataPacket[0].ListDataElementValue[0].Value;
                    }

                    dipEntity.DIPValue = Int32.Parse(acutalData, System.Globalization.NumberStyles.HexNumber);

                }

            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetData(List<ProfileData> demandIntegrationPeriodData, bool isUpload)" , ex);
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
