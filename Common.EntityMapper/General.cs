#region NameSpaces
using System;
using System.Collections.Generic;
using CAB.Entity;
using CAB.Framework;
using CAB.Parser;
using System.Text;
using CAB.Parser.Entity;
using Hunt.EPIC.Logging;
using CAB.Framework.Utility;
#endregion
namespace Common.EntityMapper
{
    /// <summary>
    /// Maps General data to entity .
    /// </summary>
    public class General
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(General).ToString());
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
        public DLMS650NamePlateDetailsEntity GetMappedEntity(List<ProfileData> generalData)
        {
            DLMS650NamePlateDetailsEntity generalEntity = new DLMS650NamePlateDetailsEntity();

            string defaultValue = "----";
            DataElement dataElement = null;
            List<DataElement> generalRecords = new List<DataElement>();
            if (generalData != null && generalData.Count > 0 && generalData[0].ListMeterDataPacket.Count > 0)
            {
                foreach (ProfileData profileData in generalData)
                {
                    foreach (MeterDataPacket meterDataPacket in profileData.ListMeterDataPacket)
                    {
                        foreach (DataElement element in meterDataPacket.ListDataElementValue)
                        {
                            generalRecords.Add(element);
                        }
                    }
                }

                generalEntity.MeterDataType = "LTCT";

                dataElement = CommonMapper.GetDataElementByDataDefId(generalRecords, 1);
                generalEntity.MeterSerialNumber = dataElement.Value;

                dataElement = CommonMapper.GetDataElementByDataDefId(generalRecords, 2);
                generalEntity.Manufacturername = dataElement.Value;

                dataElement = CommonMapper.GetDataElementByDataDefId(generalRecords, 3);
                generalEntity.FirmwareVersionformeter = dataElement.Value;

                dataElement = CommonMapper.GetDataElementByDataDefId(generalRecords, 4);
                
                switch (dataElement.Value)
                {
                    case "0":
                    case "00":
                        generalEntity.Metertype = MeterType.ThreePhaseThreeWireHTPTCT;
                        generalEntity.MeterConstant = "8000";//add pk_meterconstant
                        break;
                    case "1":
                    case "01":
                        generalEntity.Metertype = MeterType.ThreePhaseFourWire;
                        generalEntity.MeterConstant = "8000";
                        break;
                    case "2":
                    case "02":
                        generalEntity.Metertype = MeterType.ThreePhaseFourWireHTPTCT;
                        generalEntity.MeterConstant = "8000";//add pk_meterconstant
                        break;
                    case "3":
                    case "03":
                        generalEntity.Metertype = MeterType.ThreePhaseFourWireLTCT;
                        generalEntity.MeterConstant = "4000";//add pk_meterconstant
                        
                        break;
                    case "4":
                    case "04":
                            generalEntity.Metertype = MeterType.ThreePhaseFourWireWCM;                        
                        
                        break;
                    case "5":
                    case "05":
                        generalEntity.Metertype = MeterType.OnePhaseTwoWire;
                        generalEntity.MeterConstant = "3200";//add pk_meterconstant
                        break;
                    case "6":
                    case "06":
                        generalEntity.Metertype = MeterType.OnePhaseTwoWire;
                        generalEntity.MeterConstant = "3200";//smart meter 1p
                        break;
                    case "7":
                    case "07":
                        generalEntity.Metertype = MeterType.ThreePhaseFourWire;
                        generalEntity.MeterConstant = "2000";//smart meter WCM
                        break;
                    case "8":
                    case "08":
                        generalEntity.Metertype = MeterType.ThreePhaseFourWire;
                        generalEntity.MeterConstant = "4000";//smart meter LTCT
                        break;
                    default:
                        generalEntity.Metertype = defaultValue;
                        break;

                }
                //generalEntity.Metertype = dataElement.Value == "1" ? MeterType.ThreePhaseFourWire : defaultValue;
                if (generalEntity.Metertype == MeterType.OnePhaseTwoWire)
                {
                    generalEntity.InternalCTratio = defaultValue;
                    generalEntity.InternalPTratio = defaultValue;
                    generalEntity.InternalVTratio = defaultValue;
                }
                else
                {
                    dataElement = CommonMapper.GetDataElementByDataDefId(generalRecords, 5);
                    generalEntity.InternalCTratio = dataElement.Value;

                    dataElement = CommonMapper.GetDataElementByDataDefId(generalRecords, 6);
                    generalEntity.InternalPTratio = dataElement.Value;
                    generalEntity.InternalVTratio = dataElement.Value;
                }
                //category added.
                dataElement = CommonMapper.GetDataElementByDataDefId(generalRecords, 171);
                if (dataElement.Value == "0")
                {
                    generalEntity.Category = defaultValue;
                }
                else
                {
                    generalEntity.Category = dataElement.Value;
                }

                dataElement = CommonMapper.GetDataElementByDataDefId(generalRecords, 7);
                generalEntity.Meteryearofmanufacture = dataElement.Value;

                dataElement = CommonMapper.GetDataElementByDataDefId(generalRecords, 190);
                generalEntity.BasicCurrentRating = "Invalid Rating!";
                generalEntity.CurrentRating = "Invalid Rating!";
                try
                {
                    if (dataElement.Value.Length >= 7)
                    {
                        if (dataElement.Value.Contains("A"))
                        {
                            generalEntity.BasicCurrentRating = dataElement.Value.Trim();
                            generalEntity.CurrentRating = dataElement.Value.Trim();
                        }
                        else
                        {
                            string[] currentRatingRange = dataElement.Value.Split('-');
                            if (currentRatingRange.Length > 1)
                            {
                                generalEntity.BasicCurrentRating = Convert.ToInt32(currentRatingRange[0].Trim()) + "-" + Convert.ToInt32(currentRatingRange[1].Trim()) + " A";
                                generalEntity.CurrentRating = Convert.ToInt32(currentRatingRange[0].Trim()) + "-" + Convert.ToInt32(currentRatingRange[1].Trim()) + " A";
                            }
                        }
                    }
                    if (generalEntity.Metertype == "3P-4W WCM")
                    {

                        if (generalEntity.BasicCurrentRating == "5-30 A")
                        {
                            generalEntity.MeterConstant = "4000 Impluse/kwh";
                        }
                        if (generalEntity.BasicCurrentRating == "10-60 A")
                        {
                            generalEntity.MeterConstant = "2000 Impluse/kwh";
                        }
                        if (generalEntity.BasicCurrentRating == "20-100 A")
                        {
                            generalEntity.MeterConstant = "1000 Impluse/kwh";
                        }
                        if (generalEntity.BasicCurrentRating == "10-80 A")
                        {
                            generalEntity.MeterConstant = "1000 Impluse/kwh";
                        }
                    }
                }
                catch (Exception ex)    //Exception log for catch block
                {
                    logger.Log(LOGLEVELS.Error, "GetMappedEntity(List<ProfileData> generalData)", ex);
                }
                dataElement = CommonMapper.GetDataElementOrNullByDataDefId(generalRecords, 1003);
                generalEntity.ReverseKWh = dataElement.Value;

                dataElement = CommonMapper.GetDataElementOrNullByDataDefId(generalRecords, 2190);
                generalEntity.MeterMonthOfManufacture = dataElement.Value;

                dataElement = CommonMapper.GetDataElementOrNullByDataDefId(generalRecords, 2189);
                generalEntity.AccuracyClass = dataElement.Value;

                //dataElement = CommonMapper.GetDataElementOrNullByDataDefId(generalRecords, 2126);

                //if (dataElement.Value != null)
                //    generalEntity.MeterConstant = dataElement.Value;
                //else
                //    generalEntity.MeterConstant = defaultValue;

                dataElement = CommonMapper.GetDataElementByDataDefId(generalRecords, 2);
                if ((!dataElement.Value.Contains("Cabcon")) && (!dataElement.Value.Contains("LGZ"))) //if not landis gyr /LGZ meter
                {
                    generalEntity.MeterModelNo = NamePlateConstants.NonLandisMeter.ToString();
                    generalEntity.VoltageRating = "---";
                    generalEntity.CurrentRating = "---";
                    generalEntity.MeterConstant = "---";
                    generalEntity.PrimaryMeterConstant = "---";//PGVCL                    
                    generalEntity.BasicCurrentRating = "---";
                }

                dataElement = CommonMapper.GetDataElementByDataDefId(generalRecords, 8);
                MapSignatureData(dataElement.Value, generalEntity);

                if (generalEntity.AccuracyClass == null)//PGVCL
                {
               string meter_modelno = generalEntity.MeterModelNo;
                if (generalEntity.MeterModelNo == null)
                    meter_modelno = ConfigInfo.MeterModel;
                    
                generalEntity.AccuracyClass = GetMeterAccuracyClass(meter_modelno);
                   
                }


                if (generalEntity.MeterModelNo == "35")//PGVCL
                {
                    generalEntity.MeterConstant = "2000 Impluse/kwh";
                }
                if (generalEntity.Metertype == "1P-2W")//PGVCL
                {
                    generalEntity.MeterConstant = "3200 Impluse/kwh";
                }
                if (generalEntity.Metertype == "3P-4W LTCT" || generalEntity.Metertype == "3P-4W HTCT" || generalEntity.MeterModelNo=="34" || generalEntity.MeterModelNo == "36")//PGVCL
                {
                    if (generalEntity.InternalCTratio == "1")
                    {
                        generalEntity.PrimaryMeterConstant ="4000"  + " Impluse/kwh";//generalEntity.MeterConstant
                        generalEntity.MeterConstant = "4000" + " Impluse/kwh";
                    }
                    else
                    {
                        generalEntity.PrimaryMeterConstant = Convert.ToString(Convert.ToInt32("4000") / Convert.ToInt32(generalEntity.InternalCTratio)) + " Impluse/kwh";//PGVCL
                        generalEntity.MeterConstant = "4000" + " Impluse/kwh";
                    }

                }
            }
            return generalEntity;
        }

        public string GetMeterAccuracyClass(string metermodel)
        {
            string MeterAccuracy="1.0 S";
            switch (Int32.Parse(metermodel))
            {
                
                case NamePlateConstants.SapphireLTCT_st:
                case NamePlateConstants.SmartM_Cipher_LTCT:                       
                case NamePlateConstants.Smartmeter_LTCT:                                     
                case NamePlateConstants.TwoTOUltModelValue:                                    
                case NamePlateConstants.WBLTValue:                                     
                case NamePlateConstants.PumaLTE650Value:
                case NamePlateConstants.LTCTCortexValue:
                case NamePlateConstants.SapphireWCM_St:                    
                case NamePlateConstants.PumaHTE650Value:                                  
                case NamePlateConstants.HTCTCortexValue:
                case NamePlateConstants.Smartmeter_HTCT:                              
                case NamePlateConstants.SmartM_Cipher_HTCT:
                case NamePlateConstants.SapphireLTCT:
                    MeterAccuracy = "0.5 S";
                    break;
                default:
                    MeterAccuracy = "1.0 ";
                    break;
            }
            return MeterAccuracy;
        }


        /// <summary>
        /// This Method used to maps signature data 
        /// </summary>
        /// <param name="signatureData"></param>
        /// <param name="generalEntity"></param>
        public void MapSignatureData(string signatureData, DLMS650NamePlateDetailsEntity generalEntity)
        {
             if (signatureData == "Non-CabconMeter" || generalEntity.MeterModelNo == NamePlateConstants.NonLandisMeter.ToString())
            {
                generalEntity.MeterModelNo = NamePlateConstants.NonLandisMeter.ToString();
            }

            else if (!string.IsNullOrEmpty(signatureData) && signatureData.Length > 19)
            {
                int InitialStartupIndex = 0;
                if (signatureData == "**2.21240010060WC4RS" && generalEntity.Metertype == MeterType.OnePhaseTwoWire)
                {
                    //@balGovind: For Single phase smart meter (signature not available)
                    signatureData = "#0.059240005030FS2rB";
                    generalEntity.InternalFirmwareVersion = "0.059";
                    // signatureData = "**2.21240010060SK4RS";
                    // generalEntity.InternalFirmwareVersion = "----";
                    if (signatureData.Contains("#"))
                    {
                        generalEntity.InternalFirmwareVersion = signatureData.Substring(signatureData.IndexOf('#') + 1, 5);
                        InitialStartupIndex = 6;
                    }
                    else
                    {
                        generalEntity.InternalFirmwareVersion = string.Format("{0:0.00}", Convert.ToDecimal((signatureData.Substring(0, 6).Trim('*'))));
                        InitialStartupIndex = 6;
                    }
                }
                else if (signatureData.Contains("#"))
                {
                    generalEntity.InternalFirmwareVersion = signatureData.Substring(signatureData.IndexOf('#') + 1, 6).TrimStart('0');
                    InitialStartupIndex = 8;
                }
                else if (signatureData.Contains("SPS2"))//Sapphire S2 Low cost variant
                {
                    generalEntity.InternalFirmwareVersion = signatureData.Substring(signatureData.LastIndexOf('-') + 1);
                    if (signatureData.Substring(16, 1) == "1")
                        generalEntity.VoltageRating = RefVoltageSmartMeter.Refvolt1;
                    else if (signatureData.Substring(16, 1) == "2")
                        generalEntity.VoltageRating = RefVoltageSmartMeter.Refvolt2;
                    else if (signatureData.Substring(16, 1) == "3")
                        generalEntity.VoltageRating = RefVoltageSmartMeter.Refvolt3;
                    //*****************Current Rating S2 Optima***************
                    Dictionary<string, string> S2rating = SapphireS2Rating.CurrentRatingSapphireS2;
                    string val=string.Empty;
                    S2rating.TryGetValue(signatureData.Substring(20, 1),out val);
                    generalEntity.BasicCurrentRating = val;                   
                    InitialStartupIndex = 4;
                }
                else if (signatureData.Contains("LGZ-SM"))//for smart meter
                {
                    generalEntity.InternalFirmwareVersion = signatureData.Substring(signatureData.LastIndexOf('-') + 1,9);
                //  generalEntity.Metertype = MeterType.ThreePhaseFourWire; 
                    if (signatureData.Substring(14, 1) == "1")
                        generalEntity.VoltageRating = RefVoltageSmartMeter.Refvolt1;
                    else if (signatureData.Substring(14, 1) == "2")
                        generalEntity.VoltageRating = RefVoltageSmartMeter.Refvolt2;
                    else if (signatureData.Substring(14, 1) == "3")
                        generalEntity.VoltageRating = RefVoltageSmartMeter.Refvolt3;
                    if (signatureData.Contains("SM0110") && signatureData.Substring(14, 1) == "1")
                        generalEntity.VoltageRating = RefVoltageSmartMeter.Refvolt2;
                    else if (signatureData.Contains("SM0110") && signatureData.Substring(14, 1) == "2")
                        generalEntity.VoltageRating = RefVoltageSmartMeter.Refvolt1;
                    else if (signatureData.Contains("SM0110") && signatureData.Substring(14, 1) == "3")
                        generalEntity.VoltageRating = RefVoltageSmartMeter.Refvolt3;

                    if (signatureData.Substring(13, 1) == "1")
                        generalEntity.BasicCurrentRating = RefCurrentSmartMeter.ThreePSMLTCT;
                    else if (signatureData.Substring(13, 1) == "2")
                        generalEntity.BasicCurrentRating = RefCurrentSmartMeter.ThreePSMLTCT1;
                    else if (signatureData.Substring(13, 1) == "3")
                        generalEntity.BasicCurrentRating = RefCurrentSmartMeter.ThreePSMWCM;
                    else if (signatureData.Substring(13, 1) == "4")
                        generalEntity.BasicCurrentRating = RefCurrentSmartMeter.ThreePSMWCM1;
                    else if (signatureData.Substring(13, 1) == "5")
                        generalEntity.BasicCurrentRating = RefCurrentSmartMeter.ThreePSMWCM2;
                    else if (signatureData.Substring(13, 1) == "6")
                        generalEntity.BasicCurrentRating = RefCurrentSmartMeter.ThreePSMHTCT;
                    else if (signatureData.Substring(13, 1) == "7")
                        generalEntity.BasicCurrentRating = RefCurrentSmartMeter.ThreePSMHTCT1;
                    else if (signatureData.Substring(13, 1) == "8")
                        generalEntity.BasicCurrentRating = RefCurrentSmartMeter.SinglePSM;
                    InitialStartupIndex = 4;
                }
                else
                {
                    //generalEntity.InternalFirmwareVersion = string.Format("{0:0.00}", Convert.ToDecimal((signatureData.Substring(0, 6).Trim('*'))));
                    string Firmwarestr = signatureData.Substring(0, 6).Trim('*');
                    int number;
                    bool success = int.TryParse(Firmwarestr, out number);
                    if (success)
                    {
                        generalEntity.InternalFirmwareVersion = string.Format("{0:0.00}", Convert.ToDecimal(Firmwarestr));
                        InitialStartupIndex = 6;
                    }
                    else
                    {
                        generalEntity.InternalFirmwareVersion = string.Format("{0:0.00}", Firmwarestr);
                        InitialStartupIndex = 6;                       
                    }
                }
                if (!(signatureData.Contains("LGZ-SM") || signatureData.Contains("LGZ-SPS2")))
                {

                //User Story 478245. Voltage Rating change to 63.5 V for HK meter model
                int LenghtVoltageRating = 3;
                if (signatureData.Length >= 30 && signatureData.Length < 60)
                {
                    LenghtVoltageRating = 4;
                }
                //NET Metering Meter SignatureInfo Length >= 60
                if (signatureData.Length >= 60)
                {
                    LenghtVoltageRating = 5;
                }

                generalEntity.VoltageRating =signatureData.Substring(InitialStartupIndex, LenghtVoltageRating) + " V";


                InitialStartupIndex += LenghtVoltageRating;
                if (generalEntity.Metertype != MeterType.OnePhaseTwoWire)
                {
                    generalEntity.BasicCurrentRating = Convert.ToInt32(signatureData.Substring(InitialStartupIndex, 3)) + "-" + Convert.ToInt32(signatureData.Substring((InitialStartupIndex + 3), 3)) + " A";
                }
                InitialStartupIndex += 6;
                }
                   string meterType = string.Empty;
                   if (signatureData.Contains("LGZ-SM"))
                       meterType = signatureData.Substring(InitialStartupIndex, 6);
                else if (signatureData.Contains("LGZ-SPS2"))
                    meterType = signatureData.Substring(InitialStartupIndex, 6);
                else if (generalEntity.Metertype == MeterType.OnePhaseTwoWire && generalEntity.BasicCurrentRating.ToLower().Contains("invalid"))// Handle single phase 128k meter invalid rating case
                   {
                       // 
                       if (signatureData.LastIndexOf(".") > 5) InitialStartupIndex = 11;
                       else InitialStartupIndex = 9;

                       generalEntity.BasicCurrentRating = Convert.ToInt32(signatureData.Substring(InitialStartupIndex, 3)) + "-" + Convert.ToInt32(signatureData.Substring((InitialStartupIndex + 3), 3)) + " A";
                       InitialStartupIndex += 6;
                       meterType = signatureData.Substring(InitialStartupIndex, 2);
                      
                   }
                   else 
                       meterType = signatureData.Substring(InitialStartupIndex, 2);
                   // meterType = "SM0310";

                
                // Handle single phase 128k meter invalid rating case
                //if (generalEntity.Metertype == MeterType.OnePhaseTwoWire && generalEntity.BasicCurrentRating.ToLower().Contains("invalid"))
                //   generalEntity.BasicCurrentRating = Convert.ToInt32(signatureData.Substring(InitialStartupIndex, 3)) + "-" + Convert.ToInt32(signatureData.Substring((InitialStartupIndex + 3), 3)) + " A";
                //InitialStartupIndex += 6;
                //string meterType = signatureData.Substring(InitialStartupIndex, 2);

                //NET Metering Meter SignatureInfo Length >= 60
                generalEntity.NetMeterVariantInfo = CAB.Framework.MeterVariant.ONE;
                if (signatureData.Length >= 60)
                {
                    InitialStartupIndex += 5;
                    try
                    {
                        byte MeterVariant = Convert.ToByte(signatureData[InitialStartupIndex]);
                        generalEntity.NetMeterVariantInfo = MeterVariant.ToString("X");
                    }
                    catch (Exception ex)    //Exception log for catch block
                    {
                        logger.Log(LOGLEVELS.Error, "MapSignatureData(string signatureData, DLMS650NamePlateDetailsEntity generalEntity)", ex);

                    }
                }

                generalEntity.DisplayProgrammingType = CommonMethods.GetDisplayProgrammingVariantFromSignature(signatureData).ToString();

                if (meterType.ToUpper() == "WC")
                {
                    generalEntity.MeterModelNo = NamePlateConstants.RubyE250Value.ToString();
                }
                //**************** Smart meter Ciphering***********
                else if (meterType == "SM0310")
                {
                    generalEntity.MeterModelNo = NamePlateConstants.SmartM_Cipher_WCM.ToString();
                }
                else if (meterType == "SM0405")
                {
                    generalEntity.MeterModelNo = NamePlateConstants.SmartM_Cipher_LTCT.ToString();
                }
                else if (meterType == "SM0110")
                {
                    generalEntity.MeterModelNo = NamePlateConstants.SmartM_Cipher_1PH.ToString();
                }
                else if (meterType == "SPS201")
                {
                    generalEntity.MeterModelNo = NamePlateConstants.SapphireS2.ToString();
                }
                else if (meterType == "SPS202")
                {
                    generalEntity.MeterModelNo = NamePlateConstants.SapphireS2.ToString();//LGZ-SPS202-S01101-074.001.024C

                    //byte[] Signaturebytes = Encoding.ASCII.GetBytes(signatureData);                    
                    //switch (Signaturebytes[20])
                    //{
                    //    case 0x31: //LTCT
                    //    case 0x32:
                    //        if (Signaturebytes[13] == 0x31)
                    //            generalEntity.MeterModelNo = NamePlateConstants.Sapphire_Netmeter_LTCT.ToString();
                    //        else
                    //            generalEntity.MeterModelNo = NamePlateConstants.SapphireS2.ToString();//S2 ltct need to be define
                    //        break;
                    //    case 0x33: //WCM
                    //    case 0x34: //WCM
                    //    case 0x35: //WCM
                    //    case 0x39:
                    //    case 0x41:
                    //        if (Signaturebytes[13] == 0x31)
                    //            generalEntity.MeterModelNo = NamePlateConstants.Sapphire_Netmeter_WCM.ToString();
                    //        else
                    //            generalEntity.MeterModelNo = NamePlateConstants.SapphireS2.ToString();
                    //        break;
                    //    default:
                    //        break;
                    //}
                    //if (Signaturebytes[20] == 0x01 || Signaturebytes[20] == 0x02)
                    //    //ltct sapphire s2
                    //    if (Signaturebytes[20] == 0x03 || Signaturebytes[20] == 0x04)

                    //        generalEntity.MeterModelNo = NamePlateConstants.Sapphire_Netmeter_WCM.ToString();
                    //    else
                    //        generalEntity.MeterModelNo = NamePlateConstants.SapphireS2.ToString();
                }
                else if (meterType == "uk")    // for two season Ruby6
                {
                    generalEntity.MeterModelNo = NamePlateConstants.Ruby6ukModelValue.ToString();
                }
                else if (meterType.ToUpper() == "UK")
                {
                    generalEntity.MeterModelNo = NamePlateConstants.Ruby6Value.ToString();
                }
                else if (meterType.ToUpper() == "WB")
                {
                    generalEntity.MeterModelNo = NamePlateConstants.WBValue.ToString();
                }
                else if (meterType.ToUpper() == "BW")
                {
                    generalEntity.MeterModelNo = NamePlateConstants.WBLTValue.ToString();
                }
                else if (meterType == "lt")    // for two season LTCT
                {
                    generalEntity.MeterModelNo = NamePlateConstants.TwoTOUltModelValue.ToString();
                }
                else if ((meterType.ToUpper() == "LT"))
                {
                    generalEntity.MeterModelNo = NamePlateConstants.PumaLTE650Value.ToString();
                }
                else if ((meterType == "ST"))
                {
                    generalEntity.MeterModelNo = NamePlateConstants.SapphireLTCT.ToString();
                }

                else if ((meterType == "L0")) //SapphireS2LTCT
                {
                    generalEntity.MeterModelNo = NamePlateConstants.Sapphire_Netmeter_LTCT.ToString();
                }
                else if ((meterType == "st"))
                {
                    generalEntity.MeterModelNo = NamePlateConstants.SapphireLTCT_st.ToString();
                }
                else if ((meterType == "St"))
                {
                    generalEntity.MeterModelNo = NamePlateConstants.SapphireWCM_St.ToString();
                }
                else if (meterType.ToUpper() == "HK" || meterType == "HT")
                {
                    generalEntity.MeterModelNo = NamePlateConstants.PumaHTE650Value.ToString();
                }
                else if (meterType.ToUpper() == "LC" || meterType == "HC")
                {
                    generalEntity.MeterModelNo = NamePlateConstants.LTCTCortexValue.ToString();
                }
                else if (meterType.ToUpper() == "SK")    // for Single Phase
                {
                    generalEntity.MeterModelNo = NamePlateConstants.RubyE150Value.ToString();
                }

                else if (meterType.ToUpper() == "BF")    // for Single Phase BRPL
                {
                    generalEntity.MeterModelNo = NamePlateConstants.BRPL_7Slot.ToString();
                }                    
                  else if (meterType.ToUpper() == "CF")    // For 1P VIM 64K DLMS with FD
                {
                    generalEntity.MeterModelNo = NamePlateConstants.BYPL_FD.ToString();
                }
                else if (meterType.ToUpper() == "CB")    // For 1P VIM 64K DLMS wo FD   //user story 1016689
                {
                    generalEntity.MeterModelNo = NamePlateConstants.BRPL_CBSP.ToString();
                }

                else if (meterType.ToUpper() == "FS")    // for Single Phase
                {
                    generalEntity.MeterModelNo = NamePlateConstants.SM110value.ToString();
                }
                else if (meterType.ToUpper() == "HM")    // for HTCT meters
                {
                    generalEntity.MeterModelNo = NamePlateConstants.PumaHTE650MWValue.ToString();
                    generalEntity.MeterDataType = "HTCT";
                }
                //---Rohit-----------03-March-2016------- for UPCL-----TwoSeason------
                else if (meterType == "sc")    // for Sapphire
                {
                    generalEntity.MeterModelNo = NamePlateConstants.TwoTOUSapphireValue.ToString();
                }
                else if (meterType == "SC")    // for Sapphire
                {
                    generalEntity.MeterModelNo = NamePlateConstants.SapphireValue.ToString();
                }

                else if (meterType == "W0")    // for SapphireS2WCM
                {
                    generalEntity.MeterModelNo = NamePlateConstants.Sapphire_Netmeter_WCM.ToString();
                }
                else if (meterType.ToUpper() == "TN")    // for Sapphire TNEb
                {
                    generalEntity.MeterModelNo = NamePlateConstants.TNValue.ToString();
                }
                //---Rohit-----------21-March-2016------- for VB--1P DLMS---No Season-No Week-----
                else if (meterType == "VB")
                {
                    generalEntity.MeterModelNo = NamePlateConstants.VBSPNoSeasonNoWeek.ToString();
                }
                //******* Meter Model Change Required Here ***********//
                else if (meterType.ToUpper() == "VF")
                {
                    generalEntity.MeterModelNo = NamePlateConstants.VFSPNoSeasonNoWeek.ToString();
                }
                else if (meterType == "sc")    // for Sapphire
                {
                    generalEntity.MeterModelNo = NamePlateConstants.TwoTOUSapphireValue.ToString();
                }
                else if (meterType == "Sc")    // foR 3PH THREE TOU SEASSION
                {
                    generalEntity.MeterModelNo = NamePlateConstants.ThreeTOUWCMValue.ToString();
                }

                else if (meterType == "FU")    // for Smart meter WCM
                {
                    generalEntity.MeterModelNo = NamePlateConstants.Smartmeter_WCM.ToString();
                }
                else if (meterType == "FL")    // for Smart meter LTCT
                {
                    generalEntity.MeterModelNo = NamePlateConstants.Smartmeter_LTCT.ToString();
                }
                else if (meterType == "FH")    // for Smart meter HTCT
                {
                    generalEntity.MeterModelNo = NamePlateConstants.Smartmeter_HTCT.ToString();
                }
                else if (meterType.ToUpper() == "SF")    // for Single Phase DLMS Fast Download
                {
                    generalEntity.MeterModelNo = NamePlateConstants.SFSP.ToString();
                }
                else if (meterType == "SH")    // HTCT kilo variant
                {
                    generalEntity.MeterModelNo = NamePlateConstants.Sapphire_SH.ToString();
                    //generalEntity.MeterDataType = CAB.Framework.Utility.MeterDataTypes.HTCT_KILO.ToString();
                }
                else if (meterType == "SPS2")    // Sapphire S2 Three Phae Low cost meter
                {
                    generalEntity.MeterModelNo = NamePlateConstants.SapphireS2.ToString();
                    
                }
                else if (meterType == "SM")    // HTCT Mega variant
                {
                    generalEntity.MeterModelNo = NamePlateConstants.Sapphire_SM.ToString();
                    generalEntity.MeterDataType = CAB.Framework.Utility.MeterDataTypes.HTCT_MEGA.ToString();
                }
                else if (meterType == "sm")    // HTCT mega variant 2 tou
                {
                    generalEntity.MeterModelNo = NamePlateConstants.Sapphire_sm.ToString();
                    generalEntity.MeterDataType = CAB.Framework.Utility.MeterDataTypes.HTCT_MEGA.ToString();
                }
                else if (meterType == "sh")    // HTCT kilo variant 2 tou
                {
                    generalEntity.MeterModelNo = NamePlateConstants.Sapphire_sh.ToString();
                    //generalEntity.MeterDataType = CAB.Framework.Utility.MeterDataTypes.HTCT_KILO.ToString();
                }
                else if (meterType == "vb")    // Vim Series 2 meter
                {
                    generalEntity.MeterModelNo = NamePlateConstants.VIM_Series2.ToString();

                }
                else if (meterType.ToUpper() == "BF")    // for Single Phase BYPL
                {
                    generalEntity.MeterModelNo = NamePlateConstants.BYPL_7Slot.ToString();
                }

                else if (meterType.ToUpper() == "RF")    // for Single Phase BRPL
                {
                    generalEntity.MeterModelNo = NamePlateConstants.BRPL_7Slot.ToString();
                }

                else
                {
                    generalEntity.MeterModelNo = NamePlateConstants.InvalidModel.ToString();

                }
                //  generalEntity.Metertype = signatureData.Substring(17, 1) == "3" ? MeterType.ThreePhaseThreeWire : MeterType.ThreePhaseFourWire;
                if (generalEntity.Metertype != MeterType.OnePhaseTwoWire && generalEntity.Metertype!=null)
                {
                    string[] currentRatingRange = generalEntity.BasicCurrentRating.Split('-');//PGVCL
                    if (currentRatingRange.Length > 1)
                    {

                        if (generalEntity.MeterModelNo == "34"|| generalEntity.MeterModelNo == "36" || generalEntity.Metertype == "3P-4W LTCT" || generalEntity.Metertype == "3P-4W HTCT")
                        {

                            if (generalEntity.InternalCTratio == "1")
                            {
                                generalEntity.BasicCurrentRating = "3 x" + "-" + "/" + Convert.ToString(Convert.ToInt32(currentRatingRange[0].Trim())) + " A";//PGVCL
                            }
                            else
                            {
                                generalEntity.BasicCurrentRating = "3 x" + Convert.ToInt32(Convert.ToInt32(currentRatingRange[0].Trim().Replace("A", " "))) * (Convert.ToInt32(generalEntity.InternalCTratio)) + " /" + Convert.ToString(Convert.ToInt32(Convert.ToInt32(currentRatingRange[0].Trim()))) + " A";//PGVCL
                            }
                        }
                        else
                        {
                            generalEntity.BasicCurrentRating = "3 x" + generalEntity.BasicCurrentRating;//PGVCL
                        }
                    }
                }
            }
           
            
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
