#region NameSpaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.Entity;
using CABEntity;
using CAB.Parser;
using Hunt.EPIC.Logging;
#endregion
namespace Common.EntityMapper
{
    /// <summary>
    /// Mapping support for DIP
    /// </summary>
    public class LoadLimitMapper
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(LoadLimitMapper).ToString());
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
        public LoadLimitEntity GetData(List<ProfileData> loadlimitData, bool isUpload)
        {
            
            LoadLimitEntity LoadlimitEntity = new LoadLimitEntity();
            try
            {
                if (loadlimitData[0].ListMeterDataPacket.Count > 0 && loadlimitData[0].ListMeterDataPacket[0].ListDataElementValue.Count > 0)
                {
                    string acutalData = string.Empty;
                    if (loadlimitData.Count > 1)
                    {
                        if (isUpload == false)
                            acutalData = loadlimitData[0].ListMeterDataPacket[0].ListDataElementValue[0].Value;
                        else
                            acutalData = loadlimitData[1].ListMeterDataPacket[0].ListDataElementValue[0].Value;
                    }
                    else
                    {
                        acutalData = loadlimitData[0].ListMeterDataPacket[0].ListDataElementValue[0].Value;
                    }

                    LoadlimitEntity.LLValue = Int32.Parse(acutalData, System.Globalization.NumberStyles.HexNumber);

                }

            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetData(List<ProfileData> loadlimitData, bool isUpload)", ex);
            }
            return LoadlimitEntity;

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
