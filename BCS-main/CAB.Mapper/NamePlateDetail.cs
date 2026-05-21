#region Namespaces
using System.Collections.Generic;

using CAB.Entity;
using CAB.Parser;
using CAB.Parser.Entity;
using System;
#endregion

namespace CAB.Mapper
{
    /// <summary>
    /// This class is responsible for mapping operation of NamePlate data .
    /// </summary>
    public class NamePlateDetail
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
        /// Used to prepare NamePlateDetailEntity .
        /// </summary>
        /// <param name="namePlateData"></param>
        /// <returns></returns>
        public List<NamePlateDetailEntity> GetData(List<ProfileData> namePlateData)
        {
            List<NamePlateDetailEntity> resultEntity = new List<NamePlateDetailEntity>();
            NamePlateDetailEntity namePlateDetailEntity = new NamePlateDetailEntity();
            string defaultValue = "----";

            DataElement dataElement = namePlateData[0].ListMeterDataPacket[0].ListDataElementValue[0];


            namePlateDetailEntity.MeterID = dataElement.Value;
            dataElement = namePlateData[5].ListMeterDataPacket[0].ListDataElementValue[0];
            namePlateDetailEntity.ManufacturingDate = dataElement.Value;
            dataElement = namePlateData[3].ListMeterDataPacket[0].ListDataElementValue[0];
            namePlateDetailEntity.MeterType = dataElement.Value == "1" ? "3PH 4W" : defaultValue;

            dataElement = namePlateData[7].ListMeterDataPacket[0].ListDataElementValue[0];
            string signatureData = dataElement.Value;                      
            namePlateDetailEntity.VoltageRating = Convert.ToDecimal(signatureData.Substring(6, 3)).ToString();
            namePlateDetailEntity.CurrentRating = Convert.ToDecimal(signatureData.Substring(9, 3)).ToString()
                + "-" +Convert.ToDecimal(signatureData.Substring(12, 3)).ToString();
            namePlateDetailEntity.MeterConstant = "4000 Imp/kWh; Imp/kvarh";
            
            resultEntity.Add(namePlateDetailEntity);

            return resultEntity;
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
