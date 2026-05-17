#region Namespaces
using System.Collections.Generic;

using CAB.Entity;
using CAB.Parser;
using CAB.Parser.Entity;
using System;
using CABEntity;
#endregion
namespace CAB.Mapper
{
    public class HeaderInfo
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
        public List<MeterDataHeaderInfoEntity> GetData(List<ProfileData> headerInfoData, List<ProfileData> instantData, List<ProfileData> fraudEnergyData,
                                                        List<ProfileData> namePlateData, MDWithIPEntity dipData, BillingResetEntity billingTypeData,
                                                        IECkvarSelectionEntity kvahSelection, IECAutoLockEntity autoLockEntity)
        {
            List<MeterDataHeaderInfoEntity> resultEntity = new List<MeterDataHeaderInfoEntity>();
            List<DataElement> instantRecords = null;
            List<DataElement> fraudEnergyRecords = null;
            DataElement dataElement = null;
            string defaultValue = "---------";
            TimeSpan timeSpan;                       
            MeterDataHeaderInfoEntity headerEntity = new MeterDataHeaderInfoEntity();
            headerEntity.NoSupplyDuration = defaultValue;
            headerEntity.NoLoadDuration = defaultValue;
            if (instantData != null && instantData.Count > 0 && instantData[0].ListMeterDataPacket.Count > 0)
            {
                instantRecords = instantData[0].ListMeterDataPacket[0].ListDataElementValue;

                dataElement = namePlateData[6].ListMeterDataPacket[0].ListDataElementValue[0];
                headerEntity.InternalCTPTRatio = dataElement.Value;
                dataElement = namePlateData[7].ListMeterDataPacket[0].ListDataElementValue[0];
                headerEntity.SoftwareVersion = dataElement.Value.Substring(0, 6).Replace("*", "");

                dataElement = Common.GetDataElementByDataDefId(instantRecords, 26);
                timeSpan = TimeSpan.FromMinutes(Convert.ToDouble(dataElement.Value));
                headerEntity.PowerOffDays = timeSpan.Days.ToString();
                if (dataElement.Value != "0")
                {
                    //No Supply duration                    
                    headerEntity.NoSupplyDuration = ((int)(timeSpan.TotalHours)).ToString("00") + ":" + timeSpan.Minutes.ToString("00");
                }
            }
            
            //No Load duration.
            if (fraudEnergyData != null && fraudEnergyData.Count > 0 && fraudEnergyData[0].ListMeterDataPacket.Count > 0)
            {                
                fraudEnergyRecords = fraudEnergyData[0].ListMeterDataPacket[0].ListDataElementValue;
                dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 64);
                if (dataElement.Value != "0")
                {
                    timeSpan = TimeSpan.FromMinutes(Convert.ToDouble(dataElement.Value));
                    headerEntity.NoLoadDuration = ((int)(timeSpan.TotalHours)).ToString("00") + ":" + timeSpan.Minutes.ToString("00");
                }

            }           

            if (dipData != null)
            {

                headerEntity.MD2KVADemandType = dipData.KVADemandType;
                headerEntity.MD2KVATimeInterval = dipData.KVAInterval.ToString();
                headerEntity.MD2KVASubInterval = dipData.KVASubInterval.ToString();
                headerEntity.MD1KWDemandType = dipData.KWDemandType;
                headerEntity.MD1KWTimeInterval = dipData.KWInterval.ToString();
                headerEntity.MD1KWSubInterval = dipData.KWSubInterval.ToString();
            }
            if (billingTypeData != null)
            {

                if (autoLockEntity != null && autoLockEntity.AutoLockStatus.ToUpper() == "LOCKED")
                {
                    headerEntity.BillingType = "Disabled";
                    headerEntity.BillingHour = defaultValue;
                    headerEntity.BillingMinute = defaultValue;
                    headerEntity.BillingDate = defaultValue;
                }
                else if (billingTypeData.ModeOfBilling == IECBillingMode.EndofMonth)
                {
                    headerEntity.BillingType = "End of Month";
                    headerEntity.BillingHour = defaultValue;
                    headerEntity.BillingMinute = defaultValue;
                    headerEntity.BillingDate = defaultValue;
                }
                else
                {
                    headerEntity.BillingType = "User Defined - Monthly";
                    headerEntity.BillingHour = billingTypeData.Hours;
                    headerEntity.BillingMinute = billingTypeData.Minutes;
                    headerEntity.BillingDate = billingTypeData.Day;
                }


            }
            if (kvahSelection != null)
            {
                headerEntity.PFLogic = kvahSelection.LagOnly == "1" ? "Lead as Unity" : "Lead as Lead";
            }


            resultEntity.Add(headerEntity);
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
