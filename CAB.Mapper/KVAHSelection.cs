#region NameSpaces 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.Parser;
using CAB.Entity;
#endregion
namespace CAB.Mapper
{
    /// <summary>
    /// Fills kvahselection data 
    /// </summary>
    public class KVAHSelection
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
        /// Used to fill KVAH Selection data .
        /// </summary>
        /// <param name="namePlateData"></param>
        /// <returns></returns>
        public IECkvarSelectionEntity GetData(List<ProfileData> kvahSelectionData)
        {
            IECkvarSelectionEntity kvahSelection = new IECkvarSelectionEntity();
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
            catch (Exception)
            {

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
