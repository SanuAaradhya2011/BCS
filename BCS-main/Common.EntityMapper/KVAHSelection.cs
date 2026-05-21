#region NameSpaces 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.Parser;
using CAB.Entity;
using Hunt.EPIC.Logging;
#endregion
namespace Common.EntityMapper
{
    /// <summary>
    /// Fills kvahselection data 
    /// </summary>
    public class KVAHSelection
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(KVAHSelection).ToString());
        #endregion

        #region Properties
        #endregion

        #region Constructor
        #endregion

        #region Public Methods

        /// <summary>
        /// Used to fill KVAH Selection data .
        /// </summary>
        /// <param name="namePlateData"></param>
        /// <returns></returns>
        public kvarSelectionEntity GetData(List<ProfileData> kvahSelectionData)
        {
            kvarSelectionEntity kvahSelection = new kvarSelectionEntity();
            try
            {
                if (kvahSelectionData[0].ListMeterDataPacket.Count > 0 && kvahSelectionData[0].ListMeterDataPacket[0].ListDataElementValue.Count > 0)
                {

                    if (Convert.ToInt16(kvahSelectionData[0].ListMeterDataPacket[0].ListDataElementValue[0].Value) == 0)
                    {
                        kvahSelection.LagOnly = "1";
                        kvahSelection.LagandLead = "0"; 
                    }
                    else if (Convert.ToInt16(kvahSelectionData[0].ListMeterDataPacket[0].ListDataElementValue[0].Value)  == 1)
                    {
                        kvahSelection.LagOnly = "0";
                        kvahSelection.LagandLead = "1";
                    }
                }

            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetData(List<ProfileData> kvahSelectionData)", ex);
            }
            return kvahSelection;

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
