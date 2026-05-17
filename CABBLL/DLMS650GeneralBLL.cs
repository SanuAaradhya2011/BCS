
/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 									|
 * |											           				            								|
 * | 																    											|
 * |----------------------------------------------------------------------------------------------------------------| */
using CAB.DALC.Data;
using CAB.Framework;
using CAB.Framework.Entity;
using System.Data;
using System.Collections.Generic;
using System;
using CABFramework;
using CAB.Framework.Utility;
using Hunt.EPIC.Logging;

namespace CAB.BLL
{
    public class DLMS650GeneralBLL : IBLL
    {
        private DLMS650GeneralDAL generalDAL;
        private Dictionary<string, string> generalDataColumns = new Dictionary<string, string>();
        private bool isPUMA = false;
        private DLMS650CommonBLL dlmsCommonBLL = null;
        private const string ObisCode = "OBIS Code";
        private const string Value = "Value";
        private const int ErrorValue = -1;
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(DLMS650GeneralBLL).ToString());
        public DLMS650GeneralBLL()
        {
            dlmsCommonBLL = new DLMS650CommonBLL();
            if (UtilityDetails.GetUtilityDetails() == UtilityEntity.Generic)
            {
                isPUMA = true;
            }
            generalDAL = new DLMS650GeneralDAL(isPUMA);
        }
        public Dictionary<string, string> CreateGeneralDictionary()
        {
            //generalDataColumns.Add("Meter Serial Number", "meterSerialNumber");
            generalDataColumns.Add("Manufacturer Name", "manufacturername");
            generalDataColumns.Add("Firmware Version", "firmwareVersionformeter");
            generalDataColumns.Add("Meter Type", "metertype");
            generalDataColumns.Add("Internal CT Ratio", "internalCTratio");
            generalDataColumns.Add("Internal PT Ratio", "internalPTratio");
            generalDataColumns.Add("Year of Manufacture", "meteryearofmanufacture");
            //if (UtilityDetails.ShowMeterModelNo)
            //{
                generalDataColumns.Add("Model - Type", "metermodelno");
            //}
            return generalDataColumns;
        }
        public IEntity InsertData(IEntity entity)
        {

            return generalDAL.InsertData(entity);

        }
        /// <summary>
        /// Fill the corresponding with manipulated CT Ratio according to the meter model
        /// </summary>
        /// <param name="row"></param>
        /// <param name="ctRatioRow"></param>
        /// <param name="tempModelNo"></param>
        /// <param name="tempCTRatio"></param>
        /// <returns></returns>
        public void FillRowWithCTRatio(DataRow ctRatioRow, int tempModelNo, int tempCTRatio, string columnName)
        {
            // for ruby : BhardwajG : Replacing with constant
            if (tempModelNo == NamePlateConstants.RubyE250Value)
            {
                if (tempCTRatio > 1)
                {
                    ctRatioRow[columnName] = "NA";
                }
                else if (tempCTRatio == 1)
                {
                    //ctRatioRow[columnName] = "-- / 5";
                    ctRatioRow[columnName] = "1";
                }
                else
                {
                    ctRatioRow[columnName] = "1";
                }

            }
            //BhardwajG : Replacing with constants
            else if (tempModelNo == NamePlateConstants.PumaLTE650Value || tempModelNo == NamePlateConstants.PumaHTE650Value)
            {
                if (tempCTRatio > 1)
                {
                    ctRatioRow[columnName] = tempCTRatio.ToString() + " ( " + (tempCTRatio * 5).ToString() + " / " + 5 + " )";
                }
                else if (tempCTRatio == 1)
                {
                    ctRatioRow[columnName] = "-- / 5";
                }
                else
                {
                    ctRatioRow[columnName] = "1";
                }
            }
        }
        /// <summary>
        /// Fill the corresponding with manipulated PT Ratio according to the meter model
        /// </summary>
        /// <param name="row"></param>
        /// <param name="ctRatioRow"></param>
        /// <param name="tempModelNo"></param>
        /// <param name="tempCTRatio"></param>
        /// <returns></returns>
        public void FillRowWithPTRatio(DataRow ptRatioRow, int tempModelNo, int tempPTRatio, string columnName)
        {
            // for ruby
            if (tempModelNo == NamePlateConstants.RubyE250Value)
            {
                if (tempPTRatio >= 1)
                {
                    ptRatioRow[columnName] = "--";
                }
                // if pt ratio comes as NA it is saved as 0 in file. If found 0 in files saved as -- in database.
                // in case of -- display 1.
                else
                {
                    ptRatioRow[columnName] = "1";
                }
            }
            //PUMA HT and pt ratio greater than 1
            else if (tempModelNo == NamePlateConstants.PumaHTE650Value)
            {
                //for PUMA HT if PT Ratio > 1 then show formulated value
                if (tempPTRatio > 1)
                {
                    ptRatioRow[columnName] = tempPTRatio + " ( " + (tempPTRatio * 110).ToString() + " / " + 110 + " ) ";
                }
                // PUMA HT and pt ratio equal to 1
                else if (tempPTRatio == 1)
                {
                    ptRatioRow[columnName] = "-- / 110";
                }
                else if (tempPTRatio < 1)
                {
                    ptRatioRow[columnName] = "--";
                }

            }
            //PUMA LT and temp pt ratio greater than 1
            else if (tempModelNo == NamePlateConstants.PumaLTE650Value)
            {
                if (tempPTRatio >= 1)
                {
                    ptRatioRow[columnName] = "--";
                }
                //PUMA LT and temp pt ratio less than 1
                else if (tempPTRatio < 1)
                {
                    ptRatioRow[columnName] = "1";
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="meterModelNo"></param>
        /// <param name="meterModelRow"></param>
        private void FillMeterModelName(int meterModelNo, DataRow meterModelRow, string columnName)
        {
            if (meterModelNo == 1)
            {
                meterModelRow[columnName] = NamePlateConstants.RubyE250;
            }
            else if (meterModelNo == 2)
            {
                meterModelRow[columnName] = NamePlateConstants.PumaLTE650;
            }
            else if (meterModelNo == 3)
            {
                meterModelRow[columnName] = NamePlateConstants.PumaHTE650;
            }
            else if (meterModelNo == NamePlateConstants.LTCTCortexValue)
            {
                meterModelRow[columnName] = NamePlateConstants.LTCTCortex;
            }
            else if (meterModelNo == NamePlateConstants.Ruby6Value)
            {
                meterModelRow[columnName] = NamePlateConstants.Ruby6Val;
            }
            else if (meterModelNo == NamePlateConstants.RubyE350Value)
            {
                meterModelRow[columnName] = NamePlateConstants.E350Val;
            }
            else if (meterModelNo == NamePlateConstants.RubyE150Value)
            {
                meterModelRow[columnName] = NamePlateConstants.E150Val;
            }
            else if (meterModelNo == NamePlateConstants.WBValue)
            {
                meterModelRow[columnName] = NamePlateConstants.WBVal;
            }
            else if (meterModelNo == NamePlateConstants.WBLTValue)
            {
                meterModelRow[columnName] = NamePlateConstants.WBLTVal;
            }
            else if (meterModelNo == NamePlateConstants.PumaHTE650MWValue)
            {
                meterModelRow[columnName] = NamePlateConstants.PumaHTE650MW;
            }
            else if (meterModelNo == NamePlateConstants.SapphireValue)
            {
                meterModelRow[columnName] = NamePlateConstants.Sapphire;
            }
            else if (meterModelNo == NamePlateConstants.Sapphire_Netmeter_WCM)
            {
                meterModelRow[columnName] = NamePlateConstants.Sapphire;
            }
            else if (meterModelNo == NamePlateConstants.Ruby6ukModelValue)
            {
                meterModelRow[columnName] = NamePlateConstants.Ruby6ukModel;
            }
            else if (meterModelNo == NamePlateConstants.TwoTOUltModelValue)
            {
                meterModelRow[columnName] = NamePlateConstants.TwoTOUltModel;
            }
            else if (meterModelNo == NamePlateConstants.ThreeTOUWCMValue) // add 3PH THREE TOU
            {
                meterModelRow[columnName] = NamePlateConstants.Sapphire_WCM;
            }
            //**************** Smart meter Ciphering***********
            else if (meterModelNo == NamePlateConstants.SmartM_Cipher_WCM)
            {
                meterModelRow[columnName] = NamePlateConstants.SmartMeterCipherWCM;
            }
            else if (meterModelNo == NamePlateConstants.SmartM_Cipher_LTCT)
            {
                meterModelRow[columnName] = NamePlateConstants.SmartMeterCipherLTCT;
            }
            else if (meterModelNo == NamePlateConstants.SmartM_Cipher_1PH)
            {
                meterModelRow[columnName] = NamePlateConstants.SmartMeterCipher1PH;
            }

            else if (meterModelNo == NamePlateConstants.TNValue)
            {
                meterModelRow[columnName] = NamePlateConstants.TNModel;
            }
            else if (meterModelNo == NamePlateConstants.TwoTOUSapphireValue)
            {
                meterModelRow[columnName] = NamePlateConstants.Sapphire;
            }
            //---Rohit-----------21-March-2016------- for VB---1p DLMS--No Season-No Week-----
            else if (meterModelNo == NamePlateConstants.VBSPNoSeasonNoWeek)
            {
                meterModelRow[columnName] = NamePlateConstants.E150Val;
            }
            //******* Meter Model Change Required Here ***********//
            else if (meterModelNo == NamePlateConstants.VFSPNoSeasonNoWeek)
            {
                meterModelRow[columnName] = NamePlateConstants.E150Val;
            }
            else if (meterModelNo == NamePlateConstants.SapphireLTCT)
            {
                meterModelRow[columnName] = NamePlateConstants.SapphireSTVal;
            }

            else if (meterModelNo == NamePlateConstants.Sapphire_Netmeter_LTCT)
            {
                meterModelRow[columnName] = NamePlateConstants.SapphireSTVal;
            }
            else if (meterModelNo == NamePlateConstants.SM110value)
            {
                meterModelRow[columnName] = NamePlateConstants.SM110Val;
            }
            //******* Smart meter 3 phase WCM  ***********//
            else if (meterModelNo == NamePlateConstants.Smartmeter_WCM)
            {
                meterModelRow[columnName] = NamePlateConstants.SmartWCM670;
            }
            //******* Smart meter 3 phase LTCT  ***********//
            else if (meterModelNo == NamePlateConstants.Smartmeter_LTCT)
            {
                meterModelRow[columnName] = NamePlateConstants.SmartLTCT670;
            }
            else if (meterModelNo == NamePlateConstants.SM110value)
            {
                meterModelRow[columnName] = NamePlateConstants.SM110Val;
            }
            else if (meterModelNo == NamePlateConstants.SFSP)
            {
                meterModelRow[columnName] = NamePlateConstants.SFSPVal;
            }
            else if (meterModelNo == NamePlateConstants.Sapphire_SH)
            {
                meterModelRow[columnName] = NamePlateConstants.SapphireSHVal;
            }
            else if (meterModelNo == NamePlateConstants.SapphireS2)
            {
                meterModelRow[columnName] = NamePlateConstants.SapphireS2Val;
            }
            else if (meterModelNo == NamePlateConstants.Sapphire_sh)
            {
                meterModelRow[columnName] = NamePlateConstants.SapphireshVal;
            }
            else if (meterModelNo == NamePlateConstants.Sapphire_SM)
            {
                meterModelRow[columnName] = NamePlateConstants.SapphireSMVal;
            }
            else if (meterModelNo == NamePlateConstants.Sapphire_sm)
            {
                meterModelRow[columnName] = NamePlateConstants.SapphiresmVal;
            }
            else if (meterModelNo == NamePlateConstants.VIM_Series2) // Vim series 2 meter
            {
                meterModelRow[columnName] = NamePlateConstants.VIM_Series2;
            }
            else if (meterModelNo == 39) // for brpl 7 slot
            {
                meterModelRow[columnName] = NamePlateConstants.BYPL_7Slot;
            }
            else if (meterModelNo == 40) // for brpl 7 slot
            {
                meterModelRow[columnName] = NamePlateConstants.BRPL_7Slot;
            }
            else if (meterModelNo == 41) // for BYPL FD
            {
                meterModelRow[columnName] = NamePlateConstants.BYPL_FD;
            }
            else if (meterModelNo == 42) // for UPCL
            {
                meterModelRow[columnName] = NamePlateConstants.SapphireWCM_St;
            }
            else if (meterModelNo == 44) // VIM 64K DLMS wo FD 7 slot TOU   //user story 1016689
            {
                meterModelRow[columnName] = NamePlateConstants.BRPL_CBSP;
            }
            else
            {
                meterModelRow[columnName] = NamePlateConstants.InvalidModel;
            }

        }

        private void FillNetMeterModelName(string meterDataID, int meterModelNo, DataRow meterModelRow, string columnName)
        {
            string meterVariant = GetMeterVariantByMeterDataID(meterDataID);
            if (meterModelNo == 1)
            {
                if (meterVariant == CAB.Framework.MeterVariant.THREE || meterVariant == CAB.Framework.MeterVariant.FOUR)
                {
                    meterModelRow[columnName] = NamePlateConstants.RubyE250NET;
                }
                else
                {
                    meterModelRow[columnName] = NamePlateConstants.RubyE250;
                }
            }
            else if (meterModelNo == 2)
            {
                meterModelRow[columnName] = NamePlateConstants.PumaLTE650;
            }
            else if (meterModelNo == 3)
            {
                meterModelRow[columnName] = NamePlateConstants.PumaHTE650;
            }
            else if (meterModelNo == NamePlateConstants.LTCTCortexValue)
            {
                meterModelRow[columnName] = NamePlateConstants.LTCTCortex;
            }
            else if (meterModelNo == NamePlateConstants.Ruby6Value)
            {
                if (meterVariant == CAB.Framework.MeterVariant.THREE || meterVariant == CAB.Framework.MeterVariant.FOUR)
                {
                    meterModelRow[columnName] = NamePlateConstants.Ruby6ValNET;
                }
                else
                {
                    meterModelRow[columnName] = NamePlateConstants.Ruby6Val;
                }
            }
            else if (meterModelNo == NamePlateConstants.RubyE350Value)
            {
                meterModelRow[columnName] = NamePlateConstants.E350Val;
            }
            else if (meterModelNo == NamePlateConstants.RubyE150Value)
            {
                meterModelRow[columnName] = NamePlateConstants.E150Val;
            }
            else if (meterModelNo == NamePlateConstants.WBValue)
            {
                if (meterVariant == CAB.Framework.MeterVariant.THREE || meterVariant == CAB.Framework.MeterVariant.FOUR)
                {
                    meterModelRow[columnName] = NamePlateConstants.WBValNET;
                }
                else
                {
                    meterModelRow[columnName] = NamePlateConstants.WBVal;
                }
            }
            else if (meterModelNo == NamePlateConstants.WBLTValue)
            {
                meterModelRow[columnName] = NamePlateConstants.WBLTVal;
            }
            else if (meterModelNo == NamePlateConstants.PumaHTE650MWValue)
            {
                meterModelRow[columnName] = NamePlateConstants.PumaHTE650MW;
            }
            else if (meterModelNo == NamePlateConstants.SapphireValue || meterModelNo == NamePlateConstants.Sapphire_Netmeter_WCM)
            {
                if (meterVariant == CAB.Framework.MeterVariant.THREE || meterVariant == CAB.Framework.MeterVariant.FOUR)
                {
                    meterModelRow[columnName] = NamePlateConstants.SapphireNET;
                }
                else
                {
                    meterModelRow[columnName] = NamePlateConstants.Sapphire;
                }
            }
            else if (meterModelNo == NamePlateConstants.Ruby6ukModelValue)
            {
                if (meterVariant == CAB.Framework.MeterVariant.THREE || meterVariant == CAB.Framework.MeterVariant.FOUR)
                {
                    meterModelRow[columnName] = NamePlateConstants.Ruby6ukModelNET;
                }
                else
                {
                    meterModelRow[columnName] = NamePlateConstants.Ruby6ukModel;
                }
            }
            else if (meterModelNo == NamePlateConstants.TwoTOUltModelValue)
            {
                meterModelRow[columnName] = NamePlateConstants.TwoTOUltModel;
            }
            else if (meterModelNo == NamePlateConstants.TNValue)
            {
                meterModelRow[columnName] = NamePlateConstants.TNModel;
            }
            else if (meterModelNo == NamePlateConstants.TwoTOUSapphireValue)
            {
                meterModelRow[columnName] = NamePlateConstants.Sapphire;
            }
            //---Rohit-----------21-March-2016------- for VB---1p DLMS--No Season-No Week-----
            else if (meterModelNo == NamePlateConstants.VBSPNoSeasonNoWeek)
            {
                if (meterVariant == CAB.Framework.MeterVariant.THREE || meterVariant == CAB.Framework.MeterVariant.FOUR)
                {
                    meterModelRow[columnName] = NamePlateConstants.E150ValNet;
                }
                else
                {
                    meterModelRow[columnName] = NamePlateConstants.E150Val;                    
                }                
            }
            //******* Meter Model Change Required Here ***********//
            else if (meterModelNo == NamePlateConstants.VFSPNoSeasonNoWeek)
            {
                if (meterVariant == CAB.Framework.MeterVariant.THREE || meterVariant == CAB.Framework.MeterVariant.FOUR)
                {
                    meterModelRow[columnName] = NamePlateConstants.E150ValNet;
                }
                else
                {
                    meterModelRow[columnName] = NamePlateConstants.E150Val;
                }                
            }
            else if (meterModelNo == NamePlateConstants.SapphireLTCT)
            {
                if (meterVariant == CAB.Framework.MeterVariant.THREE || meterVariant == CAB.Framework.MeterVariant.FOUR)
                {
                    meterModelRow[columnName] = NamePlateConstants.SapphireSTValNET;
                }
                else
                {
                    meterModelRow[columnName] = NamePlateConstants.SapphireSTVal;
                }
            }
            else if (meterModelNo == NamePlateConstants.SapphireLTCT_st)
            {
                if (meterVariant == CAB.Framework.MeterVariant.THREE || meterVariant == CAB.Framework.MeterVariant.FOUR)
                {
                    meterModelRow[columnName] = NamePlateConstants.Sapphire_stValNET;
                }
                else
                {
                    meterModelRow[columnName] = NamePlateConstants.Sapphire_stVal;
                }
            }
            else if (meterModelNo == NamePlateConstants.SM110value)
            {
                meterModelRow[columnName] = NamePlateConstants.SM110Val;
            }
            //******* Smart meter 3 phase WCM  ***********//
            else if (meterModelNo == NamePlateConstants.Smartmeter_WCM)
            {
                meterModelRow[columnName] = NamePlateConstants.SmartWCM670;
            }
            //******* Smart meter 3 phase LTCT  ***********//
            else if (meterModelNo == NamePlateConstants.Smartmeter_LTCT)
            {                
                    meterModelRow[columnName] = NamePlateConstants.SmartLTCT670;                
            }
            else if (meterModelNo == NamePlateConstants.SM110value)
            {
                meterModelRow[columnName] = NamePlateConstants.SM110Val;
            }
            else if (meterModelNo == NamePlateConstants.SFSP)
            {
                if (meterVariant == CAB.Framework.MeterVariant.THREE || meterVariant == CAB.Framework.MeterVariant.FOUR)
                {
                    meterModelRow[columnName] = NamePlateConstants.SFSPValNet;
                }
                else
                {
                    meterModelRow[columnName] = NamePlateConstants.SFSPVal;
                } 
            }
            else if (meterModelNo == NamePlateConstants.Sapphire_SH)
            {
                if (meterModelNo == 27)
                {
                    meterModelRow[columnName] = "E650- HTCT";
                }
                else
                {
                    meterModelRow[columnName] = NamePlateConstants.SapphireSHVal;
                }
            }
            else if (meterModelNo == NamePlateConstants.SapphireS2)
            {
                meterModelRow[columnName] = NamePlateConstants.SapphireS2Val;
            }
            else if (meterModelNo == NamePlateConstants.Sapphire_sh)
            {
                meterModelRow[columnName] = NamePlateConstants.SapphireshVal;
            }
            else if (meterModelNo == NamePlateConstants.Sapphire_SM)
            {
                meterModelRow[columnName] = NamePlateConstants.SapphireSMVal;
            }
            else if (meterModelNo == NamePlateConstants.Sapphire_sm)
            {
                meterModelRow[columnName] = NamePlateConstants.SapphiresmVal;
            }
            else if (meterModelNo == NamePlateConstants.SmartM_Cipher_WCM)// Falcon2 WCM
            {
                meterModelRow[columnName] = NamePlateConstants.SmartMeterCipherWCM;
            }
            else if (meterModelNo == NamePlateConstants.SmartM_Cipher_LTCT)// Falcon2 LTCT
            {
                meterModelRow[columnName] = NamePlateConstants.SmartMeterCipherLTCT;
            }
            else if (meterModelNo == NamePlateConstants.SmartM_Cipher_1PH)// Falcon2 1PH
            {
                meterModelRow[columnName] = NamePlateConstants.SmartMeterCipher1PH;
            }
            else if (meterModelNo == NamePlateConstants.BRPL_7Slot)// BYPL 1PH 7 SLOT
            {
                meterModelRow[columnName] = NamePlateConstants.BRPL_7Slot;
            }
            else if (meterModelNo == NamePlateConstants.BYPL_7Slot)// BRPL 1PH 7 SLOT
            {
                meterModelRow[columnName] = NamePlateConstants.BYPL_7Slot;
            }
            else if (meterModelNo == NamePlateConstants.SapphireWCM_St)// UPCL 3TOU
            {
                meterModelRow[columnName] = NamePlateConstants.SapphireWCM_St;
            }
            else if (meterModelNo == NamePlateConstants.VIM_Series2)// Vim series 2 meter
            {
                if (meterVariant == CAB.Framework.MeterVariant.THREE || meterVariant == CAB.Framework.MeterVariant.FOUR)
                {
                    meterModelRow[columnName] = NamePlateConstants.VIMS2Net;
                }
                else
                {
                    meterModelRow[columnName] = NamePlateConstants.VIMS2;
                } 
            }
            //SarkarA code change start 20180122 // Add valid meter model name for model 33 "Sc"
            else if (meterModelNo == NamePlateConstants.ThreeTOUWCMValue)
            {
                meterModelRow[columnName] = NamePlateConstants.Sapphire_WCM;
            }
            
            else if (meterModelNo == NamePlateConstants.NonLandisMeter)
            {
                meterModelRow[columnName] = NamePlateConstants.NonLandis;
            }
            else if (meterModelNo == NamePlateConstants.BYPL_FD)
            {
                meterModelRow[columnName] = NamePlateConstants.VIM64KFD;// For 1P VIM 64K DLMS with FD
            }
            else if (meterModelNo == NamePlateConstants.BRPL_CBSP)  //user story 1016689
            {
                meterModelRow[columnName] = NamePlateConstants.E150Val;// For 1P VIM 64K DLMS wo FD
            }
            else if (meterModelNo == NamePlateConstants.Sapphire_Netmeter_LTCT) 
            {
                meterModelRow[columnName] = NamePlateConstants.WBLTVal;//
            }
            else if (meterModelNo == NamePlateConstants.SapphireS2_NetMeter)  
            {
                meterModelRow[columnName] = NamePlateConstants.E150Val;//
            }



            //SarkarA code change end 20180122
            else
            {
                meterModelRow[columnName] = NamePlateConstants.InvalidModel;
            }

        }


        public DataSet GetMeterData(int meterDataID)
        {
            int tempCTRatio = 0;
            int tempPTRatio = 0;
            int tempModelNo = 0;
            DataRow ctRatioRow = null;
            DataRow ptRatioRow = null;
            DataRow meterModelRow = null;
            DataSet dataSet = dlmsCommonBLL.ConvertGeneralRowToColumn(generalDAL.GetMeterData(meterDataID));
            if (dataSet != null && dataSet.Tables != null && dataSet.Tables.Count > 0)
            {
                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    //set the tempctratio variable with ct ratio or with -1 if the value is not valid
                    if (row[ObisCode].ToString().Trim().Equals(NamePlateConstants.CTRatioObisCode))
                    {
                        ctRatioRow = row;
                        if (!int.TryParse(row[Value].ToString(), out tempCTRatio))
                        {
                            tempCTRatio = ErrorValue;
                        }
                    }
                    //set the tempptratio variable with pt ratio or with -1 if the value is not valid
                    if (row[ObisCode].ToString().Trim().Equals(NamePlateConstants.PTRatioObisCode))
                    {
                        ptRatioRow = row;
                        if (!int.TryParse(row[Value].ToString(), out tempPTRatio))
                        {
                            tempPTRatio = ErrorValue;
                        }
                    }
                    //set the meter model no variable with meter model no or with -1 if the value is not valid
                    if (row[ObisCode].ToString().Trim().Equals(NamePlateConstants.MeterModelNoObisCode))
                    {
                        meterModelRow = row;
                        if (!int.TryParse(row[Value].ToString(), out tempModelNo))
                        {
                            tempModelNo = ErrorValue;
                        }
                    }
                }
                if (UtilityDetails.ShowCTRatioFormula)
                {
                    //Fill the row with ct ratio and model name
                    FillRowWithCTRatio(ctRatioRow, tempModelNo, tempCTRatio, Value);
                }
                if (UtilityDetails.ShowPTRatioFormula)
                {
                    //Fill the row with PT ratio and model name
                    FillRowWithPTRatio(ptRatioRow, tempModelNo, tempPTRatio, Value);
                }
                //if (UtilityDetails.ShowMeterModelNo)
                //{
                    //Fill the row with meter model no
                    FillNetMeterModelName(meterDataID.ToString(),tempModelNo, meterModelRow, Value);
                //}
            }
            return dataSet;
        }
        private List<string> GetDatabaseColumns(List<string> columnList)
        {
            CreateGeneralDictionary();
            List<string> columns = new List<string>();
            string tempStr = string.Empty;
            foreach (string key in columnList)
            {
                if (generalDataColumns.TryGetValue(key, out tempStr))
                    columns.Add(tempStr);
            }
            return columns;
        }
        /// <summary>
        /// Get general profile data by columns selected and on the basis of whether meter model name will be shown or not.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="columnList"></param>
        /// <param name="showModelNo"></param>
        /// <returns></returns>
        public DataSet GetGeneralDataByParameter(string value, List<string> columnList, bool showModelNo)
        {
            int internalCTRatio = 0;
            int internalPTRatio = 0;
           
            DataSet ds = new DataSet();
            List<string> columns = GetDatabaseColumns(columnList);
            ds = generalDAL.dGetGeneralDataByMeter(value, columns);

            if (showModelNo)
            {
                int meterModelNo = generalDAL.GetMeterModelNoByMeterID(value);
                if (meterModelNo > 0)
                {
                    if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            if (meterModelNo > 0)
                            {
                                if (columnList.Contains("Model - Type"))
                                {
                                    FillMeterModelName(meterModelNo, row, "metermodelno");
                                }
                                if (UtilityDetails.ShowCTRatioFormula && columnList.Contains("Internal CT Ratio"))
                                {
                                    if (row["internalctratio"] != DBNull.Value)
                                    {
                                        if (!(int.TryParse(row["internalctratio"].ToString(), out internalCTRatio)))
                                        {
                                            internalCTRatio = ErrorValue;
                                        }
                                        FillRowWithCTRatio(row, meterModelNo, internalCTRatio, "internalctratio");
                                    }
                                }
                                if (UtilityDetails.ShowPTRatioFormula && columnList.Contains("Internal PT Ratio"))
                                {
                                    if (row["internalptratio"] != DBNull.Value)
                                    {
                                        if (!(int.TryParse(row["internalptratio"].ToString(), out internalPTRatio)))
                                        {
                                            internalPTRatio = ErrorValue;
                                        }
                                        FillRowWithPTRatio(row, meterModelNo, internalPTRatio, "internalptratio");
                                    }
                                }
                            }
                        }
                    }
                    
                }
                else
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        if (meterModelNo == -1)
                        {
                            if (columnList.Contains("Model - Type"))
                            {
                                FillMeterModelName(meterModelNo, row, "metermodelno");
                            }
                        }
                    }

                }
            }
            return ds;
        }
        public DataSet GetGeneralDataByFileName(string value, string fileName, List<string> columnList)
        {
            DataSet ds = new DataSet();
            List<string> columns = GetDatabaseColumns(columnList);
            ds = generalDAL.dGetGeneralDataByFileName(value, fileName, columns);
            return ds;
        }
        /// <summary>
        /// Get the display information of General profile
        /// </summary>
        /// <param name="value"></param>
        /// <param name="fileName"></param>
        /// <param name="columnList"></param>
        /// <param name="meterModelNo"></param>
        /// <returns></returns>
        public DataSet GetGeneralDataByFileName(string value, string fileName, List<string> columnList, int meterModelNo)
        {
            DataSet ds = new DataSet();
            List<string> columns = GetDatabaseColumns(columnList);
            ds = generalDAL.dGetGeneralDataByFileName(value, fileName, columns);
            int internalCTRatio = 0;
            int internalPTRatio = 0;
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    if (meterModelNo > 0)
                    {
                        if (columnList.Contains("Model - Type"))
                        {
                            FillMeterModelName(meterModelNo, row, "metermodelno");
                        }
                        if ( UtilityDetails.ShowCTRatioFormula && columnList.Contains("Internal CT Ratio"))
                        {
                            if (row["internalctratio"] != DBNull.Value)
                            {
                                if (!(int.TryParse(row["internalctratio"].ToString(), out internalCTRatio)))
                                {
                                    internalCTRatio = -1;
                                }
                                FillRowWithCTRatio(row, meterModelNo, internalCTRatio, "internalctratio");
                            }
                        }
                        if ( UtilityDetails.ShowCTRatioFormula && columnList.Contains("Internal PT Ratio"))
                        {
                            if (row["internalptratio"] != DBNull.Value)
                            {
                                if (!(int.TryParse(row["internalptratio"].ToString(), out internalPTRatio)))
                                {
                                    internalPTRatio = -1;
                                }
                                FillRowWithPTRatio(row, meterModelNo, internalPTRatio, "internalptratio");
                            }
                        }
                    }
                }
            }
            return ds;
        }
        public bool DeleteData(long meterDataID)
        {
            return generalDAL.DeleteData(meterDataID);
        }
        public MeterDataTypes GetMeterDataType(string meterDataId)
        {
            MeterDataTypes meterType = MeterDataTypes.LTCT;
            try
            {
                DataSet ds = generalDAL.GetMeterDataType(meterDataId);

                if (ds != null && ds.Tables != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["MeterDataType"].ToString()))
                    {
                        meterType = (MeterDataTypes)Enum.Parse(typeof(MeterDataTypes), ds.Tables[0].Rows[0]["MeterDataType"].ToString(), true);
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetMeterDataType(string meterDataId)", ex);
            }
            return meterType;
        }


        public string GetMeterType(string meterDataId)
        {
            string meterType = string.Empty;
            try
            {
                DataSet ds = generalDAL.GetMeterType(meterDataId);

                if (ds != null && ds.Tables != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["metertype"].ToString()))
                    {
                        meterType = ds.Tables[0].Rows[0]["metertype"].ToString();
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetMeterType(string meterDataId)", ex);
            }
            return meterType;
        }



        public decimal GetRollOverValue(string meterDataId)
        {
            decimal rollOverValue = 0;
            int energyResolution = -1;
            try
            {
                DataSet ds = generalDAL.GetEnergyResolution(meterDataId);

                if (ds != null && ds.Tables != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["energyResolution"].ToString()))
                    {
                        energyResolution = Convert.ToInt32(ds.Tables[0].Rows[0]["energyResolution"].ToString());
                        rollOverValue = CalculateRollOverValue(energyResolution);
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetRollOverValue(string meterDataId)", ex);
            }
            return rollOverValue;
        }
        public decimal CalculateRollOverValue(int energyresolution)
        {
            if (energyresolution == 0 || energyresolution == 4)
            {
                return 99999999 + 1;
            }
            else if (energyresolution == 1 || energyresolution == 5)
            {
                return 9999999.9M + 0.1M;
            }
            else if (energyresolution == 2 || energyresolution == 6)
            {
                return 999999.99M + 0.01M;
            }
            else if (energyresolution == 3)
            {
                return 999999.999M + 0.001M;
            }
            else
            {
                return 0;
            }
        }
        public bool UpdateMeterDataType(string meterDataID, string meterType)
        {
            return generalDAL.UpdateMeterDataType(meterDataID, meterType);
        }
        //VBM - Get meterModel no from meter Id
        public int GetMeterModelNoByMeterID(string meterId)
        {
            return generalDAL.GetMeterModelNoByMeterID(meterId);
        }
        //VBM - Get meterModel no from meterdata Id
        public int GetMeterModelNoByMeterDataID(string meterId)
        {
            return generalDAL.GetMeterModelNoByMeterDataID(meterId);
        }
        //Get MeterVariant from MeterDataID
        public string GetMeterVariantByMeterDataID(string meterDataId)
        {
            string meterVariant = string.Empty;
            try
            {
                DataSet ds = generalDAL.GetMeterVariantByMeterDataID(meterDataId);
                meterVariant = ds.Tables[0].Rows[0][0].ToString();
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetMeterVariantByMeterDataID(string meterDataId)", ex);   
            }
           return meterVariant;
        }


        public int GetMeterTypeByMeterID(string meterId)
        {
            return generalDAL.GetMeterTypeNoByMeterID(meterId);
        }
        //VBM - Get fw version  from meterdata Id
        public string GetFirmwareVersionByMeterDataID(string meterDataId)
        {
            return generalDAL.GetFirmwareVersionByMeterDataID(meterDataId);
        }
        /// <summary>
        /// get active meter type by meter ID
        /// </summary>
        /// <param name="meterId"></param>
        /// <returns></returns>
        public string GetActiveMeterTypeByMeterDataID(string meterDataId)
        {
            return generalDAL.GetActiveMeterTypeByMeterDataID(meterDataId);
        }

        /// <summary>
        /// gets all the meter model numbers for existing meters in database.
        /// </summary>
        /// <returns></returns>
         public DataSet GetAllMeterModel()
        {
            return generalDAL.GetAllMeterModel();
        }
        /// <summary>
         /// gets all the meter types for existing meters in database.
        /// </summary>
        /// <returns></returns>
          public DataSet GetAllMeterType()
        {
            return generalDAL.GetAllMeterType();
        }

          public int GetMeterDisplayProgrammingVariantByMeterDataID(string meterDataId)
          {
              return generalDAL.GetMeterDisplayProgrammingVariantByMeterDataID(meterDataId);
          }
    }
}

