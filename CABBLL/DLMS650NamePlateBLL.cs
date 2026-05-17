#region NameSpaces
using System.Collections.Generic;
using System.Data;
using CAB.DALC.Data;
using CAB.Framework;
using CAB.Framework.Entity;
#endregion

namespace CAB.BLL
{
    public class DLMS650NamePlateBLL : IBLL
    {

        #region Nested Types
        #endregion

        #region Constants and Variables
        private DLMS650NamePlateDAL nameplateDAL;
        private Dictionary<string, string> nameplateDataColumns = new Dictionary<string, string>();
        private bool isPUMA = false;
        private DLMS650CommonBLL dlmsCommonBLL = null;
        private const string ObisCode = "OBIS Code";
        private const string Value = "Value";
        private const int ErrorValue = -1;
        #endregion

        #region Properties
        #endregion

        #region Constructor
        public DLMS650NamePlateBLL()
        {
            dlmsCommonBLL = new DLMS650CommonBLL();
            if (UtilityDetails.GetUtilityDetails() == UtilityEntity.Generic)
            {
                isPUMA = true;
            }
            nameplateDAL = new DLMS650NamePlateDAL(isPUMA);
        }
        #endregion

        #region Public Methods
        public Dictionary<string, string> CreateGeneralDictionary()
        {
            nameplateDataColumns.Add("Manufacturer Name", "manufacturername");
            nameplateDataColumns.Add("Firmware Version", "firmwareVersionformeter");
            nameplateDataColumns.Add("Meter Type", "metertype");
            nameplateDataColumns.Add("Internal CT Ratio", "internalCTratio");
            nameplateDataColumns.Add("Internal PT Ratio", "internalPTratio");
            nameplateDataColumns.Add("Year of Manufacture", "meteryearofmanufacture");
            nameplateDataColumns.Add("Model - Type", "metermodelno");
            return nameplateDataColumns;
        }

        public DataSet GetMeterData(int meterDataID)
        {
            DataSet dataSet = dlmsCommonBLL.ConvertGeneralRowToColumn(nameplateDAL.GetMeterData(meterDataID));
            return dataSet;
        }

        public IEntity InsertData(IEntity entity)
        {
            return nameplateDAL.InsertData(entity);
        }

        public bool DeleteData(long meterDataId)
        {
            return nameplateDAL.DeleteData(meterDataId);
        }
        #endregion

        #region Protected Methods
        #endregion

        #region Event Handlers

        #endregion

        #region Private Methods
        /// <summary>
        /// Fill the corresponding with manipulated CT Ratio according to the meter model
        /// </summary>
        /// <param name="row"></param>
        /// <param name="ctRatioRow"></param>
        /// <param name="tempModelNo"></param>
        /// <param name="tempCTRatio"></param>
        /// <returns></returns>
        private void FillRowWithCTRatio(DataRow ctRatioRow, int tempModelNo, int tempCTRatio, string columnName)
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
        private void FillRowWithPTRatio(DataRow ptRatioRow, int tempModelNo, int tempPTRatio, string columnName)
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
            else if (meterModelNo == NamePlateConstants.SFSP)
            {
                meterModelRow[columnName] = NamePlateConstants.SFSPVal;
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
                meterModelRow[columnName] = NamePlateConstants.Sapphire_WCM;
            }
            else if (meterModelNo == NamePlateConstants.Ruby6ukModelValue)
            {
                meterModelRow[columnName] = NamePlateConstants.Ruby6ukModel;
            }
            else if (meterModelNo == NamePlateConstants.TwoTOUltModelValue)
            {
                meterModelRow[columnName] = NamePlateConstants.TwoTOUltModel;
            }
            else if (meterModelNo == NamePlateConstants.TNValue)
            {
                meterModelRow[columnName] = NamePlateConstants.TNModel;
            }
            //******* Smart meter 3 phase WCM ****************//
            else if (meterModelNo == NamePlateConstants.Smartmeter_WCM )
            {
                meterModelRow[columnName] = NamePlateConstants.SmartWCM670;
            }
            //******* Smart meter 3 phase LTCT ****************//
            else if (meterModelNo == NamePlateConstants.Smartmeter_LTCT)
            {
                meterModelRow[columnName] = NamePlateConstants.SmartLTCT670;
            }
            else
            {
                meterModelRow[columnName] = NamePlateConstants.InvalidModel;
            }

        }
        #endregion
       
    }
}
