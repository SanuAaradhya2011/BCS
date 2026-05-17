
#region NameSpaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.Entity;
using CAB.Parser;
using CAB.Parser.Entity;
using CAB.Framework;
using Hunt.EPIC.Logging;
#endregion
namespace Common.EntityMapper
{
    /// <summary>
    /// Maps Anamoly data to entity .
    /// </summary>
    public class Anamoly
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(Anamoly).ToString());
        #endregion

        #region Properties
        #endregion

        #region Constructor
        #endregion

        #region Public Methods
        /// <summary>
        /// Gets general data into general entity 
        /// </summary>
        /// <param name="instantData"></param>
        /// <param name="phasorData"></param>
        /// <param name="fraudEnergyData"></param>
        /// <returns></returns>
        public AnomalyEntity GetMappedEntity(List<ProfileData> anamolyData, DLMS650NamePlateDetailsEntity generalEntity)
        {
           
            AnomalyEntity anamolyEntity = null;           
            List<DataElement> generalRecords = new List<DataElement>();            
            
            try
            {
                    if (anamolyData != null && anamolyData.Count > 0 && anamolyData[0].ListMeterDataPacket.Count > 0)
                    {
                        string value = anamolyData[0].ListMeterDataPacket[0].ListDataElementValue[0].Value;
                   
                    if (!string.IsNullOrEmpty(value))
                        {                      
                        anamolyEntity = new AnomalyEntity();
                            if (generalEntity.MeterModelNo == NamePlateConstants.SmartM_Cipher_1PH.ToString() || generalEntity.MeterModelNo == NamePlateConstants.SmartM_Cipher_HTCT.ToString() || generalEntity.MeterModelNo == NamePlateConstants.SmartM_Cipher_LTCT.ToString() || generalEntity.MeterModelNo == NamePlateConstants.SmartM_Cipher_WCM.ToString())
                            {
                                AnamolySmartmeter(value, anamolyEntity);

                            }
                        else if (generalEntity.MeterModelNo == NamePlateConstants.SapphireS2.ToString() || generalEntity.MeterModelNo == NamePlateConstants.SapphireS2_NetMeter.ToString())
                        {
                                AnamolySapphireS2(value, anamolyEntity);

                            }
                          else
                            {

                            if (value.Substring(0, 1) == "\0")
                            {
                                anamolyEntity.Flash = 0;
                            }
                            else
                            {
                                anamolyEntity.Flash = 1;
                            }
                            if (value.Substring(1, 1) == "\0")
                            {
                                anamolyEntity.EeProm = 0;
                            }
                            else
                            {
                                anamolyEntity.EeProm = 1;
                            }
                            if (value.Substring(2, 1) == "\0")
                            {
                                anamolyEntity.Smps = 0;
                            }
                            else
                            {
                                anamolyEntity.Smps = 1;
                            }
                            if (value.Substring(3, 1) == "\0")
                            {
                                anamolyEntity.Rtc = 0;
                            }
                            else
                            {
                                anamolyEntity.Rtc = 1;
                            }
                            // added for Sapphire new Anamoly design 
                            // expected to thorw error for other meters
                            if (value.Substring(7, 1) == "\0")
                            {
                                anamolyEntity.RTCBattery = 0;
                            }
                            else
                            {
                                anamolyEntity.RTCBattery = 1;
                            }
                            //Adding for Single Phase meters
                            // expected to thorw error for other meters
                            if (value.Substring(8, 1) == "\0")
                            {
                                anamolyEntity.MainBattery = 0;
                            }
                            else
                            {
                                anamolyEntity.MainBattery = 1;
                            }

                            if (value.Substring(9, 1) == "\0")
                            {
                                anamolyEntity.Error = 0;
                            }
                            else
                            {
                                if (Convert.ToInt32(Convert.ToChar(value.Substring(9, 1))) != 0)
                                {
                                    anamolyEntity.Error = Convert.ToInt32(Convert.ToChar(value.Substring(9, 1)));
                                }


                            }



                            // added for single phase Ammenment 3 cahnges  Anamoly design                         
                            if (generalEntity.MeterModelNo == "8")
                            {
                                if (value.Substring(10, 1) == "\0")
                                {
                                    anamolyEntity.RTCBattery = 0;
                                }
                                else
                                {
                                    anamolyEntity.RTCBattery = 1;
                                }
                            }  }
                        }
                    
                }
            }
            catch (Exception ex) //SarkarA //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetMappedEntity(List<ProfileData> anamolyData, DLMS650NamePlateDetailsEntity generalEntity)", ex); 
            }
            return anamolyEntity;
        }

        private void AnamolySmartmeter(string value, AnomalyEntity anamolyEntity)
        {
            //***********Byte 1 reserve for future use
            if (value.Substring(1, 1) == "\0")//byte 2 flash memory
            {
                anamolyEntity.Flash = 0;
            }
            else
            {
                anamolyEntity.Flash = 1;
            }
            if (value.Substring(2, 1) == "\0")//byte 3 EEPROM memory
            {
                anamolyEntity.EeProm = 0;
            }
            else
            {
                anamolyEntity.EeProm = 1;
            }
            //***********Byte 4 reserve for future use

            if (value.Substring(4, 1) == "\0")//byte 5 RTC
            {
                anamolyEntity.Rtc = 0;
            }
            else
            {
                anamolyEntity.Rtc = 1;
            }
            //***********Byte 6,7,8 reserve for future use

            if (value.Substring(8, 1) == "\0")//byte 9 RTC Battery
            {
                anamolyEntity.RTCBattery = 0;
            }
            else
            {
                anamolyEntity.RTCBattery = 1;
            }
            //Adding for Single Phase meters
            // expected to thorw error for other meters
            if (value.Substring(9, 1) == "\0")//byte 10 Main Battery
            {
                anamolyEntity.MainBattery = 0;
            }
            else
            {
                anamolyEntity.MainBattery = 1;
            }
            //***********Byte 11,12 reserve for future use
            if (value.Substring(12, 1) == "\0")//byte 13 Main Battery value
            {
                anamolyEntity.MainBatteryValue = 0;
            }
            else
            {
                anamolyEntity.MainBatteryValue = 1;

            }
            //***********Byte 15,16 reserve for future use
          
        
        }

        private void AnamolySapphireS2(string value, AnomalyEntity anamolyEntity)
        {
            //ASCIIEncoding ascii = new ASCIIEncoding();
            string AnamolyS2Value = string.Empty;
            Byte[] bytes = System.Text.ASCIIEncoding.Default.GetBytes(value);                
            int flash1 = 0; int flash2 = 0;
            int Eprom1 = 0; int Eprom2 = 0;
            int RTCStop = 0;int RTCRemove = 0;
            int RTCBad = 0; 

            for (int i = 0; i < bytes.Length; i++)
            {               
                char[] tempchararr = Convert.ToString(bytes[i], 2).PadRight(8, '0').ToCharArray();
                Array.Reverse(tempchararr);
                AnamolyS2Value += new string(tempchararr); 

            }
            char[] chararr = AnamolyS2Value.ToCharArray();

           
            //***********SapphireS2 Bit 1 means "OK" 0 means "NotOK            
            if (chararr[0] == '0')//bit 0 flash1 
                flash1 = 0;            
            else
                flash1 = 1;

            if (chararr[1] == '0')//bit 1 flash2         
                flash2 = 0;
            else
                flash2 = 1;

            if (flash1==1 && flash2==1)
                anamolyEntity.Flash = 1;
            else
                anamolyEntity.Flash = 0;

            if (chararr[2] == '0')//Bit 2 EPROM1           
                Eprom1 = 0;            
            else
                Eprom1 = 1;

            if (chararr[3] == '0')//bit 3 EPROM2 //not available            
                Eprom2 = 0;
            else
                Eprom2 = 1;

            if (Eprom1 == 1 && Eprom2 == 1)
                anamolyEntity.EeProm = 1;
            else
                anamolyEntity.EeProm = 0;

            if (chararr[4] == '0')//Bit 4 SMPS            
                anamolyEntity.Smps = 0;            
            else            
                anamolyEntity.Smps = 1;            
          
            if (chararr[5] == '0')//Bit 5 RTC Battery Low            
                anamolyEntity.RTCBattery = 0;            
            else
                anamolyEntity.RTCBattery = 1;


            if (chararr[6] == '0')//Bit 6 RTC Stop            
                RTCStop = 0;
            else
                RTCStop = 1;

            if (chararr[7] == '0')//Bit 7 RTC Bad            
                RTCBad = 0;
            else
                RTCBad = 1;

            if (chararr[8] == '0')//Bit 0 RTC Remove            
                RTCRemove = 0;
            else 
                RTCRemove = 1;

            //*** If RTCStop,RTCBad and RTCRemove=1 then RTC status OK****
            if (RTCStop == 1 && RTCBad == 1 && RTCRemove== 1)
                anamolyEntity.Rtc = 1;
            else
                anamolyEntity.Rtc = 0; 
                                  
                anamolyEntity.MainBattery = 1; //By default 1 
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
