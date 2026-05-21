using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.Entity;
using CABEntity;
using CAB.Parser.Entity;
using CAB.Parser;
using Hunt.EPIC.Logging;

namespace Common.EntityMapper
{
    /// <summary>
    /// Mapping Meter Accuracy chek
    /// </summary>
    public class MeterAccuracyCheck
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(MeterAccuracyCheck).ToString());
        #endregion

        #region Properties
        #endregion

        #region Constructor
        #endregion

        #region Public Methods

        /// <summary>
        /// Used to fill meter accuracy check entity 
        /// </summary>
        /// <param name="namePlateData"></param>
        /// <returns></returns>
        public List<MeterAccuracyCheckEntity> GetData(List<ProfileData> fraudEnergyData)
        {
            List<MeterAccuracyCheckEntity> resultData = new List<MeterAccuracyCheckEntity>();
            List<DataElement> meterAccuracyRecords = null;
            MeterAccuracyCheckEntity meterAccuracyEntity = new MeterAccuracyCheckEntity();
            DataElement dataElement = null;
            string value = string.Empty;           
            try
            {
                meterAccuracyRecords = fraudEnergyData[0].ListMeterDataPacket[0].ListDataElementValue;                
               
                ////Reverse energy
                //dataElement = Common.GetDataElementByDataDefId(meterAccuracyRecords, 69);
                //meterAccuracyEntity.ReversekVAh = Convert.ToDecimal(dataElement.Value);
                //dataElement = Common.GetDataElementByDataDefId(meterAccuracyRecords, 70);
                //meterAccuracyEntity.ReversekWh = Convert.ToDecimal(dataElement.Value);

                //dataElement = Common.GetDataElementByDataDefId(meterAccuracyRecords, 78);
                //meterAccuracyEntity.ReversekvarhLag = Convert.ToDecimal(dataElement.Value);
                //dataElement = Common.GetDataElementByDataDefId(meterAccuracyRecords, 79);
                //meterAccuracyEntity.ReversekvarhLead = Convert.ToDecimal(dataElement.Value);

                //Normal 
                dataElement = CommonMapper.GetDataElementByDataDefId(meterAccuracyRecords, 34);
                meterAccuracyEntity.KVAh = dataElement.Value;
                dataElement = CommonMapper.GetDataElementByDataDefId(meterAccuracyRecords, 31);
                meterAccuracyEntity.KWh = dataElement.Value;

                dataElement = CommonMapper.GetDataElementByDataDefId(meterAccuracyRecords, 32);
                meterAccuracyEntity.KvarhLag = dataElement.Value;
                dataElement = CommonMapper.GetDataElementByDataDefId(meterAccuracyRecords, 33);
                meterAccuracyEntity.KvarhLead = dataElement.Value;

                dataElement = CommonMapper.GetDataElementByDataDefId(meterAccuracyRecords, 1079);
                meterAccuracyEntity.ExportKWh = dataElement.Value;
                dataElement = CommonMapper.GetDataElementByDataDefId(meterAccuracyRecords, 1080);
                meterAccuracyEntity.ExportKVAh = dataElement.Value;

                dataElement = CommonMapper.GetDataElementByDataDefId(meterAccuracyRecords, 1137);
                meterAccuracyEntity.ExportKvarhLead = dataElement.Value;
                dataElement = CommonMapper.GetDataElementByDataDefId(meterAccuracyRecords, 1138);
                meterAccuracyEntity.ExportKvarhLag = dataElement.Value;

                resultData.Add(meterAccuracyEntity);

            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetData(List<ProfileData> fraudEnergyData)", ex);
            }
            return resultData;
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
