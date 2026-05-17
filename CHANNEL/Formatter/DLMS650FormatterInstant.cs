/* 
 * |---------------------------------------------------------------------------------------------------------------|
 * |All rights reserved to Cabcon Technologies                                                                             |
 * |                                                                                                               |
 * |Author : Piyush Singh.                                                                               |
 * |                                                                                                               |
 * |                                                                                                               |
 * |---------------------------------------------------------------------------------------------------------------| */

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using CAB.Entity;
using CAB.BLL;
using CAB.Framework.Utility;
using CAB.Framework;
namespace CAB.Channel.Formatter
{
    public class DLMS650FormatterInstant : ReadBase
    {
        DLMS650StructureInfoBLL structureInfoBLL;
        DLMS650StructureUnitInfoBLL structureUnitInfoBLL;
        private DLMS650FormatterCommon common;
        string[] InstantOBISCode, InstantOBISCodeDataValue, InstantScaleOBISCode, InstantScaleOBISCodeDataValue, InstantScaleOBISCodeDataUnitValue;
        int[] InstantScaleClassID, InstantScaleAttribute, InstantClassID, InstantAttribute, InstantDataIndex;
        int structure, structureLength, dataLength, datatype, totByte;
        bool isPUMA = false;
        string utility = string.Empty;
        bool isMPKWCL = false;
        bool isHTCT = false;
        public DLMS650FormatterInstant()
        {
            structureInfoBLL = new DLMS650StructureInfoBLL();
            structureUnitInfoBLL = new DLMS650StructureUnitInfoBLL();
            common = new DLMS650FormatterCommon();
            // Check utility
            if (UtilityEntity.Generic == UtilityDetails.Utility)
            {
                isPUMA = true;
            }
            if (UtilityEntity.MPKWCL == UtilityDetails.Utility)
            {
                isMPKWCL = true;
            }

        }
        public DLMS650FormatterInstant(UtilityEntity utilityEntity)
        {
            structureInfoBLL = new DLMS650StructureInfoBLL();
            structureUnitInfoBLL = new DLMS650StructureUnitInfoBLL();
            common = new DLMS650FormatterCommon();
            if (UtilityEntity.Generic == utilityEntity)
            {
                isPUMA = true;
            }
            if (UtilityEntity.MPKWCL == utilityEntity)
            {
                isMPKWCL = true;
            }
        }
        public DLMS650FormatterInstant(bool IsPUMA)
        {
            structureInfoBLL = new DLMS650StructureInfoBLL();
            structureUnitInfoBLL = new DLMS650StructureUnitInfoBLL();
            common = new DLMS650FormatterCommon();

            this.isPUMA = IsPUMA;

        }
        public void LoadInstantData(string[] data, BillingGeneralNFDLMSEntity master)
        {

            int counter;
            bool flag = false;
            for (counter = 0; counter < data.Length; counter++)
            {
                if (string.IsNullOrEmpty(data[counter]) && counter != 4 && counter != 5 && counter != 6 && counter != 7)
                {
                    flag = false;
                    break;
                }
                else
                    flag = true;
            }
            if (!flag)
            {
                this.StatusMessage = "Instantaneous data not found.";
                Application.DoEvents();
                return;
            }
            this.StatusMessage = "Uploading instantaneous data......";
            Application.DoEvents();
            string captureObject = data[0];
            //captureObject = captureObject.Substring(2, captureObject.Length - 2);
            string captureObjectData = data[1];
            //captureObjectData = captureObjectData.Substring(2, captureObjectData.Length - 2);
            string captureObjectScalerUnit = data[2];
            //captureObjectScalerUnit = captureObjectScalerUnit.Substring(2, captureObjectScalerUnit.Length - 2);
            string captureObjectDataScalerUnit = data[3];
            //captureObjectDataScalerUnit = captureObjectDataScalerUnit.Substring(2, captureObjectDataScalerUnit.Length - 2);

            int index = 0;
            int array = common.ConvertHexToDecimal(captureObject.Substring(index, 2)); index += 2;
            int arrayLength = common.ConvertHexToDecimal(captureObject.Substring(index, 2)); index += 2;
            //BhardwajG : Do not enter for parsing cumulative demand is the data is null or empty
            if (data.Length > 4 && !string.IsNullOrEmpty(data[4]) && !string.IsNullOrEmpty(data[5]) && !string.IsNullOrEmpty(data[6]) && !string.IsNullOrEmpty(data[7]))
            {
                string cuKWvalue = data[4];
                string cuKWscalar = data[5];
                string cuKVAvalue = data[6];
                string cuKVAscalar = data[7];

                InstantOBISCode = new string[arrayLength + 2];
                InstantOBISCodeDataValue = new string[arrayLength + 2];
                InstantClassID = new int[arrayLength + 2];
                InstantAttribute = new int[arrayLength + 2];
            }
            else
            {
                InstantOBISCode = new string[arrayLength];
                InstantOBISCodeDataValue = new string[arrayLength];
                InstantClassID = new int[arrayLength];
                InstantAttribute = new int[arrayLength];
            }

            InstantDataIndex = new int[arrayLength];
            StructureInfoEntity structureInfoEntity = null;
            counter = 0;

            #region Reading Capture Objects
            while (counter < arrayLength)
            {
                structure = common.ConvertHexToDecimal(captureObject.Substring(index, 2)); index += 2;
                structureLength = common.ConvertHexToDecimal(captureObject.Substring(index, 2)); index += 2;

                datatype = common.ConvertHexToDecimal(captureObject.Substring(index, 2)); index += 2;
                totByte = 0;
                //Class ID
                structureInfoEntity = structureInfoBLL.GetDetailData(datatype) as StructureInfoEntity;
                if (structureInfoEntity != null)
                {
                    totByte = structureInfoEntity.ValueInByte * 2;
                    InstantClassID[counter] = common.ConvertHexToDecimal(captureObject.Substring(index, totByte));
                    index += totByte;
                }
                //OBIS Code
                datatype = common.ConvertHexToDecimal(captureObject.Substring(index, 2)); index += 2;
                dataLength = common.ConvertHexToDecimal(captureObject.Substring(index, 2)); index += 2;
                InstantOBISCode[counter] = common.GetOBISCode(captureObject.Substring(index, (dataLength * 2))); index += (dataLength * 2);
                //Attribute
                datatype = common.ConvertHexToDecimal(captureObject.Substring(index, 2)); index += 2;
                structureInfoEntity = structureInfoBLL.GetDetailData(datatype) as StructureInfoEntity;
                if (structureInfoEntity != null)
                {
                    totByte = structureInfoEntity.ValueInByte * 2;
                    InstantAttribute[counter] = common.ConvertHexToDecimal(captureObject.Substring(index, totByte));
                    index += totByte;
                }
                //DataIndex
                datatype = common.ConvertHexToDecimal(captureObject.Substring(index, 2)); index += 2;
                structureInfoEntity = structureInfoBLL.GetDetailData(datatype) as StructureInfoEntity;
                if (structureInfoEntity != null)
                {
                    totByte = structureInfoEntity.ValueInByte * 2;
                    InstantDataIndex[counter] = common.ConvertHexToDecimal(captureObject.Substring(index, totByte));
                    index += totByte;
                }
                counter++;
            }
            #endregion

            #region Reading Capture Data
            counter = 0;
            index = 0;
            array = common.ConvertHexToDecimal(captureObject.Substring(index, 2)); index += 2;
            arrayLength = common.ConvertHexToDecimal(captureObject.Substring(index, 2)); index += 6;
            while (counter < arrayLength)
            {
                datatype = common.ConvertHexToDecimal(captureObjectData.Substring(index, 2)); index += 2;
                dataLength = 0;
                totByte = 0;
                if (datatype == 9 || datatype == 10)
                {
                    dataLength = common.ConvertHexToDecimal(captureObjectData.Substring(index, 2)); index += 2;
                    InstantOBISCodeDataValue[counter] = captureObjectData.Substring(index, (dataLength * 2));
                    index += (dataLength * 2);
                }
                else
                {
                    structureInfoEntity = structureInfoBLL.GetDetailData(datatype) as StructureInfoEntity;
                    if (structureInfoEntity != null)
                    {
                        totByte = structureInfoEntity.ValueInByte * 2;
                        InstantOBISCodeDataValue[counter] = common.ConvertHexToDecimal(captureObjectData.Substring(index, totByte)).ToString();
                        index += totByte;
                    }
                }
                counter++;
            }
            #endregion

            #region capture Scaler Object
            counter = 0;
            index = 0;
            array = common.ConvertHexToDecimal(captureObjectScalerUnit.Substring(index, 2)); index += 2;
            arrayLength = common.ConvertHexToDecimal(captureObjectScalerUnit.Substring(index, 2)); index += 2;

            if (data.Length > 4 && data[4] != null && data[5] != null && data[6] != null && data[7] != null)
            {
                InstantScaleAttribute = new int[arrayLength + 2];
                InstantScaleClassID = new int[arrayLength + 2];
                InstantScaleOBISCodeDataUnitValue = new string[arrayLength + 2];
                InstantScaleOBISCode = new string[arrayLength + 2];
                InstantScaleOBISCodeDataValue = new string[arrayLength + 2];
            }
            else
            {
                InstantScaleAttribute = new int[arrayLength];
                InstantScaleClassID = new int[arrayLength];
                InstantScaleOBISCode = new string[arrayLength];
                InstantScaleOBISCodeDataUnitValue = new string[arrayLength];
                InstantScaleOBISCodeDataValue = new string[arrayLength];
            }

            while (counter < arrayLength)
            {
                structure = common.ConvertHexToDecimal(captureObjectScalerUnit.Substring(index, 2)); index += 2;
                structureLength = common.ConvertHexToDecimal(captureObjectScalerUnit.Substring(index, 2)); index += 2;
                datatype = common.ConvertHexToDecimal(captureObjectScalerUnit.Substring(index, 2)); index += 2;

                totByte = 0;

                //Class ID
                structureInfoEntity = structureInfoBLL.GetDetailData(datatype) as StructureInfoEntity;
                if (structureInfoEntity != null)
                {
                    totByte = structureInfoEntity.ValueInByte * 2;
                    InstantScaleClassID[counter] = common.ConvertHexToDecimal(captureObjectScalerUnit.Substring(index, totByte));
                    index += totByte;
                }
                //OBIS Code
                datatype = common.ConvertHexToDecimal(captureObjectScalerUnit.Substring(index, 2)); index += 2;
                dataLength = common.ConvertHexToDecimal(captureObjectScalerUnit.Substring(index, 2)); index += 2;
                InstantScaleOBISCode[counter] = common.GetOBISCode(captureObjectScalerUnit.Substring(index, (dataLength * 2))); index += (dataLength * 2);
                //Attribute
                datatype = common.ConvertHexToDecimal(captureObjectScalerUnit.Substring(index, 2)); index += 2;
                structureInfoEntity = structureInfoBLL.GetDetailData(datatype) as StructureInfoEntity;
                if (structureInfoEntity != null)
                {
                    totByte = structureInfoEntity.ValueInByte * 2;
                    InstantScaleAttribute[counter] = common.ConvertHexToDecimal(captureObjectScalerUnit.Substring(index, totByte));
                    index += totByte;
                }
                //DataIndex
                datatype = common.ConvertHexToDecimal(captureObjectScalerUnit.Substring(index, 2)); index += 2;
                structureInfoEntity = structureInfoBLL.GetDetailData(datatype) as StructureInfoEntity;
                if (structureInfoEntity != null)
                {
                    totByte = structureInfoEntity.ValueInByte * 2;
                    index += totByte;
                }
                counter++;
            }
            #endregion

            #region Reading Capture Scalar Unit Data
            counter = 0;
            index = 0;
            array = common.ConvertHexToDecimal(captureObjectDataScalerUnit.Substring(index, 2));
            index += 2;

            arrayLength = common.ConvertHexToDecimal(captureObjectDataScalerUnit.Substring(index, 2));
            // for single entry meter
            if (arrayLength == 1)
            {
                arrayLength = common.ConvertHexToDecimal(captureObjectDataScalerUnit.Substring(index + 4, 2));
                index += 4;
            }
            index += 6;
            while (counter < arrayLength)
            {
                datatype = common.ConvertHexToDecimal(captureObjectDataScalerUnit.Substring(index, 2)); index += 2;
                if (datatype == 9 || datatype == 10)
                {
                    dataLength = common.ConvertHexToDecimal(captureObjectDataScalerUnit.Substring(index, 2)); index += 2;
                    InstantScaleOBISCodeDataValue[counter] = captureObjectDataScalerUnit.Substring(index, (dataLength * 2));
                    index += (dataLength * 2);
                }
                else
                {
                    structureInfoEntity = structureInfoBLL.GetDetailData(datatype) as StructureInfoEntity;
                    if (structureInfoEntity != null)
                    {
                        totByte = structureInfoEntity.ValueInByte * 2;
                        InstantScaleOBISCodeDataValue[counter] = common.ConvertHexToDecimal(captureObjectDataScalerUnit.Substring(index, totByte)).ToString();
                        index += totByte;
                    }
                }
                datatype = common.ConvertHexToDecimal(captureObjectDataScalerUnit.Substring(index, 2)); index += 2;
                if (datatype == 9 || datatype == 10)
                {
                    dataLength = common.ConvertHexToDecimal(captureObjectDataScalerUnit.Substring(index, 2)); index += 2;
                    InstantScaleOBISCodeDataValue[counter] = captureObjectDataScalerUnit.Substring(index, (dataLength * 2));
                    index += (dataLength * 2);
                }
                else
                {
                    structureInfoEntity = structureInfoBLL.GetDetailData(datatype) as StructureInfoEntity;
                    if (structureInfoEntity != null)
                    {
                        totByte = structureInfoEntity.ValueInByte * 2;
                        InstantScaleOBISCodeDataUnitValue[counter] = common.ConvertHexToDecimal(captureObjectDataScalerUnit.Substring(index, totByte)).ToString();
                        index += totByte;
                    }
                }
                index += 4;
                counter++;
            }
            #endregion

            //added PUMA
            #region Cumulative
            //BhardwajG : Do not enter for parsing cumulative demand if the data is null or empty
            if (data.Length > 4 && !string.IsNullOrEmpty(data[4]) && !string.IsNullOrEmpty(data[5]) && !string.IsNullOrEmpty(data[6]) && !string.IsNullOrEmpty(data[7]))
            {
                //kW
                InstantOBISCode[InstantOBISCode.Length - 2] = "0.0.96.0.149.255";
                InstantClassID[InstantClassID.Length - 2] = 4;
                InstantAttribute[InstantAttribute.Length - 2] = 2;
                InstantOBISCodeDataValue[InstantOBISCodeDataValue.Length - 2] = common.ConvertHexToDecimal(data[4].Substring(0, data[4].Length - 1)).ToString();

                InstantScaleOBISCodeDataUnitValue[InstantScaleOBISCodeDataUnitValue.Length - 2] = common.ConvertHexToDecimal(data[5].Substring(10, 2)).ToString();
                InstantScaleOBISCode[InstantScaleOBISCode.Length - 2] = "0.0.96.0.149.255";
                InstantScaleClassID[InstantScaleClassID.Length - 2] = 4;
                InstantScaleAttribute[InstantScaleAttribute.Length - 2] = 2;

                InstantScaleOBISCodeDataValue[InstantScaleOBISCodeDataValue.Length - 2] = InstantScaleOBISCodeDataValue[GetScaleIndex("1.0.1.6.0.255", 4, 2)].ToString();

                //kVA
                InstantOBISCode[InstantOBISCode.Length - 1] = "0.0.96.0.150.255";
                InstantClassID[InstantClassID.Length - 1] = 4;
                InstantAttribute[InstantAttribute.Length - 1] = 2;
                InstantOBISCodeDataValue[InstantOBISCodeDataValue.Length - 1] = common.ConvertHexToDecimal(data[6].Substring(0, data[6].Length - 1)).ToString();

                InstantScaleOBISCodeDataUnitValue[InstantScaleOBISCodeDataUnitValue.Length - 1] = common.ConvertHexToDecimal(data[7].Substring(10, 2)).ToString();
                InstantScaleOBISCode[InstantScaleOBISCode.Length - 1] = "0.0.96.0.150.255";
                InstantScaleClassID[InstantScaleClassID.Length - 1] = 4;
                InstantScaleAttribute[InstantScaleAttribute.Length - 1] = 2;
                InstantScaleOBISCodeDataValue[InstantScaleOBISCodeDataValue.Length - 1] = InstantScaleOBISCodeDataValue[GetScaleIndex("1.0.1.6.0.255", 4, 2)].ToString();
            }
            #endregion

            ApplyScaleToInstant(master);
            master.Instant = GetInstantEntity();
        }
        private List<DLMS650InstantaneousEntity> GetInstantEntity()
        {
            List<DLMS650InstantaneousEntity> instant = new List<DLMS650InstantaneousEntity>();
            for (int counter = 0; counter < InstantOBISCodeDataValue.Length; counter++)
            {
                DLMS650InstantaneousEntity instantEntity = new DLMS650InstantaneousEntity();
                instantEntity.InstantPowerObisCode = InstantOBISCode[counter];//obis code
                instantEntity.InstantPowerAttribute = InstantAttribute[counter].ToString();//attribute id
                instantEntity.InstantPowerClassID = InstantClassID[counter].ToString();//class id
                instantEntity.InstantPowerColumnValue = InstantOBISCodeDataValue[counter];//value
                string strColumnName = GetColumnName(InstantOBISCode[counter], InstantClassID[counter], InstantAttribute[counter]);//name
                instantEntity.InstantPowerColumnName = SetUnitInColumnName(strColumnName, instantEntity.InstantPowerColumnValue);
                instantEntity.InstantPowerDataIndex = counter + 1;
                instant.Add(instantEntity);
            }
            return instant;
        }

        private string SetUnitInColumnName(string columnName, string columnUnit)
        {
            if (columnUnit.IndexOf('*') > 0)
            {
                if (columnUnit.Substring(columnUnit.IndexOf('*') + 1).StartsWith("M"))
                {
                    isHTCT = true;
                }
                return string.Format(columnName, columnUnit.Substring(columnUnit.IndexOf('*') + 1));
            }
            else
            {
                return string.Format(columnName, (isHTCT) ? "M" : "k");
            }
        }

        private int GetScaleIndex(string obisCode, int classId, int attributeId)
        {
            for (int counter = 0; counter < InstantScaleOBISCode.Length; counter++)
            {

                if (!string.IsNullOrEmpty(InstantScaleOBISCode[counter]))
                {
                    if (InstantScaleOBISCode[counter].Trim().Equals(obisCode) && InstantScaleClassID[counter] == classId)
                        return counter;
                }

            }
            // BhardwajG : 140482 : Return -1 instead of 0 as 0 means obis code is PRESENT at 0th index for PUMA,NON-PUMA meters.
            // This change is done for Ruby meters only, will be done for PUMA later - 01 April 2013
            //if (isPUMA)
            //{
            //    return 0;
            //}
            //else
            //{
            // BhardwajG : 140482 : Return -1 instead of 0 as 0 means obis code is PRESENT at 0th index for Non-PUMA.
            // This change is done for Ruby meters only, will be done for PUMA later - 01 April 2013
            return -1;
            //}
        }

        private void ApplyScaleToInstant(BillingGeneralNFDLMSEntity master)
        {
            int counter;
            StructureUnitEntity structureUnitEntity;
            int scaleVale = 0;
            DLMSObjectType dlmsObjectType;
            bool isHTCT = false;
            try
            {
                for (counter = 0; counter < InstantOBISCodeDataValue.Length; counter++)
                {
                    //added PUMA
                    if (CheckAndConvertDate(counter, GetColumnName(InstantOBISCode[counter], InstantClassID[counter], InstantAttribute[counter])))
                        continue;
                    dlmsObjectType = common.GetDLMSObjectType(GetColumnName(InstantOBISCode[counter], InstantClassID[counter], InstantAttribute[counter]));
                    int scaleId = GetScaleIndex(InstantOBISCode[counter], InstantClassID[counter], InstantAttribute[counter]);
                    //BhardwajG : Removed hardcoded condition for PUMA utility.
                    // Added condition for puma utility.
                    //if (isPUMA)
                    //{
                    //    // Added to solve bug 72638. Added condition on fixed indexes.
                    //    if (counter != 15 && counter != 17 && counter != 18 && counter != 19)
                    //    {
                    //        structureUnitEntity = structureUnitInfoBLL.GetDetailData(Int32.Parse(InstantScaleOBISCodeDataUnitValue[scaleId])) as StructureUnitEntity;
                    //        // Added to resolve bug 73370
                    //        scaleVale = Int32.Parse(InstantScaleOBISCodeDataValue[scaleId]);
                    //    }
                    //    else
                    //    {
                    //        // Give 255 as argument for no resolution.
                    //        structureUnitEntity = structureUnitInfoBLL.GetDetailData(255) as StructureUnitEntity;
                    //        // Added to resolve bug 73370
                    //        scaleVale = 0;
                    //    }
                    //}
                    //else
                    //{
                    // BhardwajG : if obis code is found in the list
                    if (scaleId > -1)
                    {
                        structureUnitEntity = structureUnitInfoBLL.GetDetailData(Int32.Parse(InstantScaleOBISCodeDataUnitValue[scaleId])) as StructureUnitEntity;
                        scaleVale = Int32.Parse(InstantScaleOBISCodeDataValue[scaleId]);
                    }
                    //BharwdajG : else find a default value
                    else
                    {
                        // Give 255 as argument for no resolution.
                        structureUnitEntity = structureUnitInfoBLL.GetDetailData(255) as StructureUnitEntity;
                        // Added to resolve bug 73370
                        scaleVale = 0;
                    }
                    //}
                    if ((!(dlmsObjectType == DLMSObjectType.Power)) && (scaleVale == 0))
                    {
                        // The HTCT meter will never enter into this, as scalar will never be zero.
                        if (scaleVale == 0)
                        {
                            if (structureUnitEntity != null)
                            {
                                //Bug Id:DLMS_0007
                                string unit = common.GetUnit(structureUnitEntity.StructureUnit);
                                if (unit.ToUpper().Contains("K"))
                                {
                                    //int mode = Convert.ToInt32(InstantOBISCodeDataValue[counter]) % 1000;
                                    //int reminder = Convert.ToInt32(InstantOBISCodeDataValue[counter]) / 1000;
                                    //string obisCodeDataVlaue = reminder.ToString() + "." + mode.ToString();
                                    // Added by Swati for DLMS_0040
                                    string obisCodeDataVlaue = (Convert.ToDouble(InstantOBISCodeDataValue[counter]) / 1000).ToString("0.000");
                                    InstantOBISCodeDataValue[counter] = string.Concat(obisCodeDataVlaue, "*", unit);
                                }
                                else
                                {
                                    // This method calculates cum power off duration on basis of utility.
                                    GetCumPowerOffDuration(counter, unit);

                                }
                            }
                            continue;
                        }
                    }
                    string dataValue = InstantOBISCodeDataValue[counter];
                    if (scaleVale > 128)
                    {
                        int ScaleDiff = 256 - scaleVale;
                        //ScaleDiff = ScaleDiff * -1;
                        if (isPUMA)
                        {
                            dataValue = common.GetSignValue(Int32.Parse(dataValue), ScaleDiff, "-", structureUnitEntity.StructureUnit, dlmsObjectType, out isHTCT, false).ToString();
                        }
                        else
                        {
                            dataValue = common.GetSignValue(Int32.Parse(dataValue), ScaleDiff, "-", structureUnitEntity.StructureUnit);
                        }
                    }
                    else
                    {
                        if (isPUMA)
                        {
                            dataValue = common.GetSignValue(Int32.Parse(dataValue), scaleVale, "+", structureUnitEntity.StructureUnit, dlmsObjectType, out isHTCT, false).ToString();
                        }
                        else
                        {
                            dataValue = common.GetSignValue(Int32.Parse(dataValue), scaleVale, "+", structureUnitEntity.StructureUnit);
                        }
                    }
                    if (isPUMA)
                    {
                        dataValue = dataValue + "*" + common.GetUnit(structureUnitEntity.StructureUnit, isHTCT);
                    }
                    else
                    {
                        dataValue = dataValue + "*" + common.GetUnit(structureUnitEntity.StructureUnit);
                    }
                    InstantOBISCodeDataValue[counter] = dataValue;

                    //Assign Meter Type to MeterDataType property 
                    master.MeterDataType = (isHTCT) ? MeterDataTypes.HTCT : MeterDataTypes.LTCT;
                }
            }
            catch (Exception ex)
            {
            }
        }
        private void GetCumPowerOffDuration(int counter, string unit)
        {
            if (isMPKWCL)
            {
                if (counter == 20)
                {
                    InstantOBISCodeDataValue[counter] = common.GetCumPowerOffDuration(InstantOBISCodeDataValue[counter]);
                }
                else
                {
                    InstantOBISCodeDataValue[counter] = string.Concat(InstantOBISCodeDataValue[counter], "*", unit);
                }
            }
            else
            {
                InstantOBISCodeDataValue[counter] = string.Concat(InstantOBISCodeDataValue[counter], "*", unit);
            }
        }
        private bool CheckAndConvertDate(int counter, string DataValue)
        {
            //added PUMA
            //if (counter == 0 || counter == 24 || counter == 26 || counter == 28)
            if ((DataValue.ToUpper().Contains("DATE")) || (DataValue.ToUpper().Contains("TIME")))
            {
                InstantOBISCodeDataValue[counter] = common.GetDateTimeString(InstantOBISCodeDataValue[counter]);
                return true;
            }
            return false;
        }

        private string GetColumnName(string obisCode, int classId, int attributeId)
        {
            if (obisCode == "0.0.1.0.0.255" && classId == 8 && attributeId == 2) return "Real Time Clock – Date and Time";
            else if (obisCode == "1.0.31.7.0.255" && classId == 3 && attributeId == 2) return "Current - IR";
            else if (obisCode == "1.0.51.7.0.255" && classId == 3 && attributeId == 2) return "Current – IY";
            else if (obisCode == "1.0.71.7.0.255" && classId == 3 && attributeId == 2) return "Current – IB";
            else if (obisCode == "1.0.32.7.0.255" && classId == 3 && attributeId == 2) return "Voltage – VRN";
            else if (obisCode == "1.0.52.7.0.255" && classId == 3 && attributeId == 2) return "Voltage – VYN";
            else if (obisCode == "1.0.72.7.0.255" && classId == 3 && attributeId == 2) return "Voltage – VBN";
            else if (obisCode == "1.0.33.7.0.255" && classId == 3 && attributeId == 2) return "Signed Power Factor – R phase";
            else if (obisCode == "1.0.53.7.0.255" && classId == 3 && attributeId == 2) return "Signed Power Factor - Y phase";
            else if (obisCode == "1.0.73.7.0.255" && classId == 3 && attributeId == 2) return "Signed Power Factor - B phase";
            else if (obisCode == "1.0.13.7.0.255" && classId == 3 && attributeId == 2) return "Three Phase Power Factor – PF";
            else if (obisCode == "1.0.14.7.0.255" && classId == 3 && attributeId == 2) return "Frequency";
            else if (obisCode == "1.0.9.7.0.255" && classId == 3 && attributeId == 2) return "Apparent Power – {0}";//kVA
            else if (obisCode == "1.0.1.7.0.255" && classId == 3 && attributeId == 2)
            {
                // Active Power Label updated
                if (isPUMA)
                    return "Active Power (ABS)";
                else
                    return "Signed Active Power – kW (+ Forward - Reverse)";
            }
            else if (obisCode == "1.0.3.7.0.255" && classId == 3 && attributeId == 2) return "Signed Reactive Power – {0} (+ Lag - Lead)";
            else if (obisCode == "1.0.1.8.0.255" && classId == 3 && attributeId == 2) return "Cumulative Energy – {0}";//kwh
            else if (obisCode == "1.0.5.8.0.255" && classId == 3 && attributeId == 2) return "Cumulative Energy – {0} – lag";//kvarh
            else if (obisCode == "1.0.8.8.0.255" && classId == 3 && attributeId == 2) return "Cumulative Energy – {0} – lead";//kvarh
            else if (obisCode == "1.0.9.8.0.255" && classId == 3 && attributeId == 2) return "Cumulative Energy – {0}";//kVAh
            else if (obisCode == "0.0.96.7.0.255" && classId == 1 && attributeId == 2) return "Number of Power - Failures";
            else if (obisCode == "0.0.94.91.8.255" && classId == 3 && attributeId == 2) return "Cumulative Power-Failure Duration";
            else if (obisCode == "0.0.94.91.0.255" && classId == 1 && attributeId == 2) return "Cumulative Tamper Count";
            else if (obisCode == "0.0.0.1.0.255" && classId == 1 && attributeId == 2) return "Cumulative Billing Count";
            else if (obisCode == "0.0.96.2.0.255" && classId == 1 && attributeId == 2) return "Cumulative Programming Count";
            else if (obisCode == "0.0.0.1.2.255" && classId == 3 && attributeId == 2) return "Billing Date";
            else if (obisCode == "1.0.1.6.0.255" && classId == 4 && attributeId == 2) return "Maximum Demand – {0}";//kW
            else if (obisCode == "1.0.1.6.0.255" && classId == 4 && attributeId == 5) return "Maximum Demand – {0}W DateTime";//kW
            else if (obisCode == "1.0.9.6.0.255" && classId == 4 && attributeId == 2) return "Maximum Demand – {0}";//kVA
            else if (obisCode == "1.0.9.6.0.255" && classId == 4 && attributeId == 5) return "Maximum Demand – {0}VA DateTime";//kVA
            //added PUMA
            else if (obisCode == "0.0.1.0.0.255" && classId == 8 && attributeId == 5) return "TimeStamp";
            //BhardwajG : Added for NDPL : Ruby and PUMA version's.
            // If the obis code received is 1.0.2.8.0.255, return cumulative export energy. 
            else if (obisCode == "1.0.2.8.0.255" && classId == 3 && attributeId == 2) return "Cumulative Export Energy – {0}";//kVA
            else if (obisCode == "0.0.96.0.149.255" && classId == 4 && attributeId == 2) return "Cumulative Maximum Demand – {0}";//kW
            else if (obisCode == "0.0.96.0.150.255" && classId == 4 && attributeId == 2) return "Cumulative Maximum Demand – {0}";//kVA

            /*VBM - Added for KSEB ruby  */           
            else if (obisCode == "0.0.96.1.140.255" && classId == 3 && attributeId == 2) return "Reverse kWh";
            else if (obisCode == "0.0.96.1.141.255" && classId == 3 && attributeId == 2) return "Reverse kVAh";

            else if (obisCode == "0.0.96.1.166.255" && classId == 3 && attributeId == 2) return "Reverse kVArh - lag";
            else if (obisCode == "0.0.96.1.167.255" && classId == 3 && attributeId == 2) return "Reverse kVArh - lead";
            else if (obisCode == "0.0.96.1.168.255" && classId == 1 && attributeId == 2) return "Present TOD Zone";

            else if (obisCode == "0.0.96.1.169.255" && classId == 3 && attributeId == 2) return "Cumulative kWh with high Resolution - T1";
            else if (obisCode == "0.0.96.1.170.255" && classId == 3 && attributeId == 2) return "Cumulative kWh with high Resolution - T2";
            else if (obisCode == "0.0.96.1.171.255" && classId == 3 && attributeId == 2) return "Cumulative kWh with high Resolution - T3";

            else if (obisCode == "0.0.96.1.172.255" && classId == 3 && attributeId == 2) return "Cumulative kWh with high Resolution - T4";
            else if (obisCode == "0.0.96.1.173.255" && classId == 3 && attributeId == 2) return "Cumulative kWh with high Resolution - T5";
            else if (obisCode == "0.0.96.1.174.255" && classId == 3 && attributeId == 2) return "Cumulative kWh with high Resolution - T6";

            else if (obisCode == "0.0.96.1.175.255" && classId == 3 && attributeId == 2) return "Cumulative kWh with high Resolution - T7";
            else if (obisCode == "0.0.96.1.176.255" && classId == 3 && attributeId == 2) return "Cumulative kWh with high Resolution - T8";
            /*VBM - Added for KSEB ruby  */
            


            else return "";
        }
    }
}