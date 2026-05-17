#region Namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CAB.IECChannel.Formatter;
using CAB.Parser;
using CAB.Parser.Entity;
using CAB.Entity;
using CAB.IECFramework.Utility;
#endregion
namespace CAB.Mapper
{
    /// <summary>
    /// Class will map DLMS transactions into IEC RTC updates. 
    /// </summary>
    public class RTCUpdate
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        private const string RTCdlmsid = "151";
        private const string defaultValue = "";
        #endregion

        #region Properties
        #endregion

        #region Constructor
        #endregion

        #region Public Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tamperProfileData"></param>
        /// <returns></returns>
        public List<RTCUpdateEntity> GetData(List<ProfileData> tamperProfileData)
        {
            List<RTCUpdateEntity> entityList = new List<RTCUpdateEntity>();
            ProfileData profileData = GetRTCUpdates(tamperProfileData);
            RTCUpdateEntity rtcEntity = null;
            int totalRTCUpdates = profileData.ListMeterDataPacket.Count > 10 ? 10 : profileData.ListMeterDataPacket.Count;
            //Code to make sure that 10 recent records will inserted in database only .
            if (profileData.ListMeterDataPacket.Count > 10)
            {
                int difference = profileData.ListMeterDataPacket.Count - 10;
                profileData.ListMeterDataPacket.RemoveRange(0, difference);
               
            }
            for (int rtcCounter = 0; rtcCounter < profileData.ListMeterDataPacket.Count;rtcCounter++ )
            {
                
                MeterDataPacket meterDataPacket = profileData.ListMeterDataPacket[rtcCounter];
                switch (rtcCounter)
                {
                    case 0:
                        rtcEntity = new RTCUpdateEntity();
                        rtcEntity.CurrentRTC1 = GetDateTimeStamp(meterDataPacket.ReadingDate);
                        rtcEntity.PreviousRTC1 = defaultValue;
                        break;
                    case 1:
                        rtcEntity.CurrentRTC2 = GetDateTimeStamp(meterDataPacket.ReadingDate);
                        rtcEntity.PreviousRTC2 = defaultValue;
                        break;
                    case 2:
                        rtcEntity.CurrentRTC3 = GetDateTimeStamp(meterDataPacket.ReadingDate);
                        rtcEntity.PreviousRTC3 = defaultValue;
                        break;
                    case 3:
                        rtcEntity.CurrentRTC4 = GetDateTimeStamp(meterDataPacket.ReadingDate);
                        rtcEntity.PreviousRTC4 = defaultValue;
                        break;
                    case 4:
                        rtcEntity.CurrentRTC5 = GetDateTimeStamp(meterDataPacket.ReadingDate);
                        rtcEntity.PreviousRTC5 = defaultValue;
                        break;
                    case 5:
                        rtcEntity.CurrentRTC6 = GetDateTimeStamp(meterDataPacket.ReadingDate);
                        rtcEntity.PreviousRTC6 = defaultValue;
                        break;
                    case 6:
                        rtcEntity.CurrentRTC7 = GetDateTimeStamp(meterDataPacket.ReadingDate);
                        rtcEntity.PreviousRTC7 = defaultValue;
                        break;
                    case 7:
                        rtcEntity.CurrentRTC8 = GetDateTimeStamp(meterDataPacket.ReadingDate);
                        rtcEntity.PreviousRTC8 = defaultValue;
                        break;
                    case 8:
                        rtcEntity.CurrentRTC9 = GetDateTimeStamp(meterDataPacket.ReadingDate);
                        rtcEntity.PreviousRTC9 = defaultValue;
                        break;
                    case 9:
                        rtcEntity.CurrentRTC10 = GetDateTimeStamp(meterDataPacket.ReadingDate);
                        rtcEntity.PreviousRTC10 = defaultValue;
                        break;
                }
            }
            if (rtcEntity != null)
            {
                rtcEntity.TotalRTCUpdates = totalRTCUpdates.ToString();
                entityList.Add(rtcEntity);
            }
            return entityList;  
        }

        #endregion

        #region Protected Methods
        #endregion

        #region Event Handlers
        #endregion

        #region Private Methods
        /// <summary>
        /// Returns ProfileData object containing only transaction data
        /// </summary>
        /// <param name="packet"></param>
        /// <returns></returns>
        private ProfileData GetRTCUpdates(List<ProfileData> tamperData)
        {
            ProfileData profileData = new ProfileData();
            profileData.ListMeterDataPacket = new List<MeterDataPacket>();
            profileData.ProfileId = tamperData[3].ProfileId;
            foreach (MeterDataPacket packet in tamperData[3].ListMeterDataPacket)
            {
                if (RTCdlmsid.Trim()== packet.ListDataElementValue[0].Value.Trim())
                {
                    profileData.ListMeterDataPacket.Add(packet);
                }
            }
            var list = (from packet in profileData.ListMeterDataPacket select packet).
                OrderBy<MeterDataPacket, DateTime>(item => item.ReadingDate);
            profileData.ListMeterDataPacket = list.ToList<MeterDataPacket>();

            return profileData;
        }
        private string GetDateTimeStamp(DateTime dateTime)
        {
           return FormatterCommon.GetTimeStamp(DateUtility.DateTimeToLongInDDMMYYYYMMHH(dateTime),0);
        }
        #endregion

    }
}
