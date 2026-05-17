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
    public class DLMS650FormatterLoadSurvey : ReadBase
    {
        DLMS650StructureInfoBLL structureInfoBLL;
        DLMS650StructureUnitInfoBLL structureUnitInfoBLL;
        private DLMS650FormatterCommon common;
        string[] LSOBISCode, LSOBISCodeTemp, LSScaleOBISCode, LSScaleOBISCodeDataValue, LSScaleOBISCodeDataUnitValue;
        string[,] LSOBISCodeDataValue;
        int[] LSScaleClassID, LSScaleAttribute, LSClassID, LSAttribute, LSDataIndex;
        int structure, structureLength, dataLength, datatype, totByte, pocketLength, pocketDataLength;
        Dictionary<string, string> GetOBISDictionary = new Dictionary<string, string>();
        Dictionary<string, string> GetOBISDictionaryColumns = new Dictionary<string, string>();
        LoadSurveyParameterEntity lsParameterEntity = new LoadSurveyParameterEntity();
        bool isPUMA = false;
        public DLMS650FormatterLoadSurvey()
        {
            structureInfoBLL = new DLMS650StructureInfoBLL();
            structureUnitInfoBLL = new DLMS650StructureUnitInfoBLL();
            common = new DLMS650FormatterCommon();
            if (UtilityDetails.GetUtilityDetails() == UtilityEntity.Generic)
            {
                isPUMA = true;
            }
        }
        public DLMS650FormatterLoadSurvey(UtilityEntity utilityEntity)
        { 
            structureInfoBLL = new DLMS650StructureInfoBLL();
            structureUnitInfoBLL = new DLMS650StructureUnitInfoBLL();
            common = new DLMS650FormatterCommon();
            if (UtilityEntity.Generic == utilityEntity)
            {
                isPUMA = true;
            }
        }
        public DLMS650FormatterLoadSurvey(bool IsPUMA)
        {
            structureInfoBLL = new DLMS650StructureInfoBLL();
            structureUnitInfoBLL = new DLMS650StructureUnitInfoBLL();
            common = new DLMS650FormatterCommon();
            this.isPUMA = IsPUMA;            
        }
        public void LoadLSData(string[] data, BillingGeneralNFDLMSEntity master)
        {
            int counter = 0;
            string lsColumnNames = string.Empty;
            bool flag = false;
            for (counter = 0; counter < 4; counter++)
            {
                if (string.IsNullOrEmpty(data[counter]))
                {
                    flag = false;
                    break;
                }
                else
                    flag = true;
            }
            if (!flag)
            {
                this.StatusMessage = "Loadsurvey data not found.";
                Application.DoEvents();
                return;
            }
            this.StatusMessage = "Uploading Loadsurvey data......";
            Application.DoEvents();
            //BhardwajG : use ispuma variable already set in the load survey constructor as it would be used
            //            in both API and BCS.
            GetOBISDictionary = common.GetOBISCodeValue(isPUMA);
            GetOBISDictionaryColumns = common.GetOBISCodeColumnNames(isPUMA);

            string captureObject = data[0];
            //captureObject = captureObject.Substring(2, captureObject.Length - 2);
            string captureObjectData = data[1];
            //captureObjectData = captureObjectData.Substring(2, captureObjectData.Length - 2);
            string captureObjectScalerUnit = data[2];
            //captureObjectScalerUnit = captureObjectScalerUnit.Substring(2, captureObjectScalerUnit.Length - 2);
            string captureObjectDataScalerUnit = data[3];
            // captureObjectDataScalerUnit = captureObjectDataScalerUnit.Substring(2, captureObjectDataScalerUnit.Length - 2);
            int index = 0;
            int array = common.ConvertHexToDecimal(captureObject.Substring(index, 2)); index += 2;
            int arrayLength = common.ConvertHexToDecimal(captureObject.Substring(index, 2)); index += 2;

            LSOBISCode = new string[arrayLength];
            LSClassID = new int[arrayLength];
            LSAttribute = new int[arrayLength];
            LSDataIndex = new int[arrayLength];
            LSOBISCodeTemp = new string[GetOBISDictionary.Count];

            StructureInfoEntity structureInfoEntity = null;
            counter = 0;

            #region Reading Capture Objects
            // To solve bug 94904.
            try
            {
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
                        LSClassID[counter] = common.ConvertHexToDecimal(captureObject.Substring(index, totByte));
                        index += totByte;
                    }
                    //OBIS Code
                    datatype = common.ConvertHexToDecimal(captureObject.Substring(index, 2)); index += 2;
                    dataLength = common.ConvertHexToDecimal(captureObject.Substring(index, 2)); index += 2;
                    LSOBISCode[counter] = common.GetOBISCode(captureObject.Substring(index, (dataLength * 2))); index += (dataLength * 2);

                    //Attribute
                    datatype = common.ConvertHexToDecimal(captureObject.Substring(index, 2)); index += 2;
                    structureInfoEntity = structureInfoBLL.GetDetailData(datatype) as StructureInfoEntity;
                    if (structureInfoEntity != null)
                    {
                        totByte = structureInfoEntity.ValueInByte * 2;
                        LSAttribute[counter] = common.ConvertHexToDecimal(captureObject.Substring(index, totByte));
                        index += totByte;
                    }
                    //DataIndex
                    datatype = common.ConvertHexToDecimal(captureObject.Substring(index, 2)); index += 2;
                    structureInfoEntity = structureInfoBLL.GetDetailData(datatype) as StructureInfoEntity;
                    if (structureInfoEntity != null)
                    {
                        totByte = structureInfoEntity.ValueInByte * 2;
                        LSDataIndex[counter] = common.ConvertHexToDecimal(captureObject.Substring(index, totByte));
                        index += totByte;
                    }
                    counter++;
                }


                for (int count = 0; count < LSOBISCode.Length; count++)
                {
                    foreach (KeyValuePair<string, string> pair in GetOBISDictionary)
                    {
                        if (LSOBISCode[count].ToString() == pair.Key)
                        {
                            LSOBISCodeTemp[count] = pair.Value;

                            break;
                        }
                    }
                }
                for (int colCount = 0; colCount < LSOBISCode.Length; colCount++)
                {
                    foreach (KeyValuePair<string, string> pair in GetOBISDictionaryColumns)
                    {
                        if (LSOBISCode[colCount].ToString() == pair.Key)
                        {
                            lsColumnNames += pair.Value + ",";
                            break;
                        }
                    }
                }

                lsParameterEntity.ColumnsNames = lsColumnNames.Remove(lsColumnNames.LastIndexOf(","));
                master.LSParameterColumns = lsParameterEntity;

            #endregion

                #region Reading Capture Data
                counter = 0;
                index = 0;

                array = common.ConvertHexToDecimal(captureObjectData.Substring(index, 2)); index += 2;
                string val = common.ConvertHexToDecimal(captureObjectData.Substring(index, 2)).ToString(); ///index += 2;
                //added PUMA
                int Temp = Convert.ToInt32(val);
                index += 2;

                if (Temp > 128)    ///((Temp & 0x80) == 0x80)
                {
                    Temp = Temp - 128; /// &0x80;  ///captureObjectData.Substring(index, 2);
                    pocketLength = 0;
                    for (int LoopIndex = 0; LoopIndex < Temp; LoopIndex++)
                    {
                        pocketLength = (pocketLength << 8) | common.ConvertHexToDecimal(captureObjectData.Substring(index, 2));
                        index += 2;
                    }
                }
                else
                    pocketLength = Temp;

                int structureId = common.ConvertHexToDecimal(captureObjectData.Substring(index, 2)); index += 2;
                pocketDataLength = common.ConvertHexToDecimal(captureObjectData.Substring(index, 2)); index += 2;

                LSOBISCodeDataValue = new string[pocketLength, LSOBISCodeTemp.Length];//pocketDataLength];
                //int pocketCounter = 0;
                while (counter < pocketLength)//arrayLength 
                {
                    int innerCounter = 0;
                    while (innerCounter < arrayLength)
                    //while (innerCounter < LSOBISCodeTemp.Length)
                    {
                        datatype = common.ConvertHexToDecimal(captureObjectData.Substring(index, 2)); index += 2;
                        dataLength = 0;
                        totByte = 0;
                        if (datatype == 9 || datatype == 10)
                        {
                            //added PUMA
                            dataLength = common.ConvertHexToDecimal(captureObjectData.Substring(index, 2)); index += 2;
                            //LSOBISCodeDataValue[counter,innerCounter] = captureObjectData.Substring(index, (dataLength * 2));  this line was changed to solve bug 73240; 11th april 2012
                            LSOBISCodeDataValue[counter, Convert.ToInt16(LSOBISCodeTemp[innerCounter])] = captureObjectData.Substring(index, (dataLength * 2));
                            index += (dataLength * 2);
                        }
                        else
                        {
                            structureInfoEntity = structureInfoBLL.GetDetailData(datatype) as StructureInfoEntity;
                            if (structureInfoEntity != null)
                            {
                                totByte = structureInfoEntity.ValueInByte * 2;
                                //LSOBISCodeDataValue[counter,innerCounter] = captureObjectData.Substring(index, (dataLength * 2));  this line was changed to solve bug 73240; 11th april 2012
                                LSOBISCodeDataValue[counter, Convert.ToInt16(LSOBISCodeTemp[innerCounter])] = common.ConvertHexToDecimal(captureObjectData.Substring(index, totByte)).ToString();
                                index += totByte;
                            }
                        }
                        innerCounter++;
                    }
                    // pocketCounter++;
                    index += 4;
                    counter++;
                }

                #endregion

                #region capture Scaler Object
                counter = 0;
                index = 0;
                array = common.ConvertHexToDecimal(captureObjectScalerUnit.Substring(index, 2)); index += 2;
                arrayLength = common.ConvertHexToDecimal(captureObjectScalerUnit.Substring(index, 2)); index += 2;
                LSScaleOBISCode = new string[arrayLength];
                LSScaleClassID = new int[arrayLength];
                LSScaleAttribute = new int[arrayLength];
                LSScaleOBISCodeDataValue = new string[arrayLength];
                LSScaleOBISCodeDataUnitValue = new string[arrayLength];

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
                        LSScaleClassID[counter] = common.ConvertHexToDecimal(captureObjectScalerUnit.Substring(index, totByte));
                        index += totByte;
                    }
                    //OBIS Code
                    datatype = common.ConvertHexToDecimal(captureObjectScalerUnit.Substring(index, 2)); index += 2;
                    dataLength = common.ConvertHexToDecimal(captureObjectScalerUnit.Substring(index, 2)); index += 2;
                    LSScaleOBISCode[counter] = common.GetOBISCode(captureObjectScalerUnit.Substring(index, (dataLength * 2))); index += (dataLength * 2);
                    //Attribute
                    datatype = common.ConvertHexToDecimal(captureObjectScalerUnit.Substring(index, 2)); index += 2;
                    structureInfoEntity = structureInfoBLL.GetDetailData(datatype) as StructureInfoEntity;
                    if (structureInfoEntity != null)
                    {
                        totByte = structureInfoEntity.ValueInByte * 2;
                        LSScaleAttribute[counter] = common.ConvertHexToDecimal(captureObjectScalerUnit.Substring(index, totByte));
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
                array = common.ConvertHexToDecimal(captureObjectDataScalerUnit.Substring(index, 2)); index += 2;
                arrayLength = common.ConvertHexToDecimal(captureObjectDataScalerUnit.Substring(index, 2));

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
                        LSScaleOBISCodeDataValue[counter] = captureObjectDataScalerUnit.Substring(index, (dataLength * 2));
                        index += (dataLength * 2);
                    }
                    else
                    {
                        structureInfoEntity = structureInfoBLL.GetDetailData(datatype) as StructureInfoEntity;
                        if (structureInfoEntity != null)
                        {
                            totByte = structureInfoEntity.ValueInByte * 2;
                            LSScaleOBISCodeDataValue[counter] = common.ConvertHexToDecimal(captureObjectDataScalerUnit.Substring(index, totByte)).ToString();
                            index += totByte;
                        }
                    }
                    datatype = common.ConvertHexToDecimal(captureObjectDataScalerUnit.Substring(index, 2)); index += 2;
                    if (datatype == 9 || datatype == 10)
                    {
                        dataLength = common.ConvertHexToDecimal(captureObjectDataScalerUnit.Substring(index, 2)); index += 2;
                        LSScaleOBISCodeDataValue[counter] = captureObjectDataScalerUnit.Substring(index, (dataLength * 2));
                        index += (dataLength * 2);
                    }
                    else
                    {
                        structureInfoEntity = structureInfoBLL.GetDetailData(datatype) as StructureInfoEntity;
                        if (structureInfoEntity != null)
                        {
                            totByte = structureInfoEntity.ValueInByte * 2;
                            LSScaleOBISCodeDataUnitValue[counter] = common.ConvertHexToDecimal(captureObjectDataScalerUnit.Substring(index, totByte)).ToString();
                            index += totByte;
                        }
                    }
                    index += 4;
                    counter++;
                }

                // This code is used for solving indexing issue with no of parameter.
                string[] LSScaleOBISCodeDataValueTemp = new string[LSOBISCodeTemp.Length];

                for (int i = 0; i < LSScaleOBISCodeDataValue.Length; i++)
                {
                    // Added condition to check if parameters configured are less but meter is sending all data scales.
                    if (LSScaleOBISCodeDataValue.Length < LSOBISCodeTemp.Length)
                    {
                        LSScaleOBISCodeDataValueTemp[Convert.ToInt32(LSOBISCodeTemp[i])] = LSScaleOBISCodeDataValue[i];
                    }
                    else
                    {
                        LSScaleOBISCodeDataValueTemp[Convert.ToInt32(LSOBISCodeTemp[i])] = LSScaleOBISCodeDataValue[Convert.ToInt32(LSOBISCodeTemp[i])];
                    }
                }
                LSScaleOBISCodeDataValue = new string[LSOBISCodeTemp.Length];
                LSScaleOBISCodeDataValue = LSScaleOBISCodeDataValueTemp;



                string[] LSScaleOBISCodeDataUnitValueTemp = new string[LSOBISCodeTemp.Length];

                for (int i = 0; i < LSScaleOBISCodeDataUnitValue.Length; i++)
                {
                    // Added condition to check if parameters configured are less but meter is sending all data scales.
                    if (LSScaleOBISCodeDataUnitValue.Length < LSOBISCodeTemp.Length)
                    {
                        LSScaleOBISCodeDataUnitValueTemp[Convert.ToInt32(LSOBISCodeTemp[i])] = LSScaleOBISCodeDataUnitValue[i];
                    }
                    else
                    {
                        LSScaleOBISCodeDataUnitValueTemp[Convert.ToInt32(LSOBISCodeTemp[i])] = LSScaleOBISCodeDataUnitValue[Convert.ToInt32(LSOBISCodeTemp[i])];
                    }

                }
                LSScaleOBISCodeDataUnitValue = new string[LSOBISCodeTemp.Length];
                LSScaleOBISCodeDataUnitValue = LSScaleOBISCodeDataUnitValueTemp;

                #endregion

                ApplyScaleToLS(master);
                master.LoadSurvey = GetLSEntity();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<DLMS650LoadSurveyEntity> GetLSEntity()
        {
            List<DLMS650LoadSurveyEntity> ls = new List<DLMS650LoadSurveyEntity>();
            for (int counter = 0; counter < pocketLength; counter++)
            {
                DLMS650LoadSurveyEntity lsEntity = new DLMS650LoadSurveyEntity();
                for (int innerCounter = 0; innerCounter < pocketDataLength; innerCounter++)
                {
                    switch (Convert.ToInt16(LSOBISCodeTemp[innerCounter]))
                    {
                        case 0:
                            lsEntity.RealTimeClockDateandTime = Int64.Parse(LSOBISCodeDataValue[counter, Convert.ToInt16(LSOBISCodeTemp[innerCounter])]);
                            break;
                        case 1:
                            lsEntity.RPhaseCurrent = LSOBISCodeDataValue[counter, Convert.ToInt16(LSOBISCodeTemp[innerCounter])];
                            break;
                        case 2:
                            lsEntity.YPhaseCurrent = LSOBISCodeDataValue[counter, Convert.ToInt16(LSOBISCodeTemp[innerCounter])];
                            break;
                        case 3:
                            lsEntity.BPhaseCurrent = LSOBISCodeDataValue[counter, Convert.ToInt16(LSOBISCodeTemp[innerCounter])];
                            break;
                        case 4:
                            lsEntity.RPhaseVoltage = LSOBISCodeDataValue[counter, Convert.ToInt16(LSOBISCodeTemp[innerCounter])];
                            break;
                        case 5:
                            lsEntity.YPhaseVoltage = LSOBISCodeDataValue[counter, Convert.ToInt16(LSOBISCodeTemp[innerCounter])];
                            break;
                        case 6:
                            lsEntity.BPhaseVoltage = LSOBISCodeDataValue[counter, Convert.ToInt16(LSOBISCodeTemp[innerCounter])];
                            break;
                        case 7:
                            lsEntity.BlockEnergykWh = LSOBISCodeDataValue[counter, Convert.ToInt16(LSOBISCodeTemp[innerCounter])];
                            break;
                        case 8:
                            lsEntity.BlockEnergykvarhlag = LSOBISCodeDataValue[counter, Convert.ToInt16(LSOBISCodeTemp[innerCounter])];
                            break;
                        case 9:
                            lsEntity.BlockEnergykvarhlead = LSOBISCodeDataValue[counter, Convert.ToInt16(LSOBISCodeTemp[innerCounter])];
                            break;
                        case 10:
                            lsEntity.BlockEnergykVAh = LSOBISCodeDataValue[counter, Convert.ToInt16(LSOBISCodeTemp[innerCounter])];
                            break;
                        //added PUMA
                        case 11:
                            lsEntity.Frequency = LSOBISCodeDataValue[counter, Convert.ToInt16(LSOBISCodeTemp[innerCounter])];
                            break;
                        case 12:
                            lsEntity.TamperStatus = LSOBISCodeDataValue[counter, Convert.ToInt16(LSOBISCodeTemp[innerCounter])];
                            break;
                        // added Net metering Parameters
                        case 13:
                            lsEntity.BlockEnergykWhExport = LSOBISCodeDataValue[counter, Convert.ToInt16(LSOBISCodeTemp[innerCounter])];
                            break;
                        case 14:
                            lsEntity.BlockEnergykvarhlagQ3 = LSOBISCodeDataValue[counter, Convert.ToInt16(LSOBISCodeTemp[innerCounter])];
                            break;
                        case 15:
                            lsEntity.BlockEnergykvarhleadQ2 = LSOBISCodeDataValue[counter, Convert.ToInt16(LSOBISCodeTemp[innerCounter])];
                            break;
                        case 16:
                            lsEntity.BlockEnergykVAhExport = LSOBISCodeDataValue[counter, Convert.ToInt16(LSOBISCodeTemp[innerCounter])];
                            break;
                        case 17:
                            lsEntity.BlockEnergykWhImport = LSOBISCodeDataValue[counter, Convert.ToInt16(LSOBISCodeTemp[innerCounter])];
                            break;
                        case 18:
                            lsEntity.BlockEnergykvarhlagQ1 = LSOBISCodeDataValue[counter, Convert.ToInt16(LSOBISCodeTemp[innerCounter])];
                            break;
                        case 19:
                            lsEntity.BlockEnergykvarhleadQ4 = LSOBISCodeDataValue[counter, Convert.ToInt16(LSOBISCodeTemp[innerCounter])];
                            break;
                        case 20:
                            lsEntity.BlockEnergykVAhImport = LSOBISCodeDataValue[counter, Convert.ToInt16(LSOBISCodeTemp[innerCounter])];
                            break;
                    }
                }
                ls.Add(lsEntity);
            }
            return ls;
        }

        private int GetScaleIndex(string obisCode, int classId, int attributeId)
        {
            for (int counter = 0; counter < pocketDataLength; counter++)
            {
                if (LSScaleOBISCode[counter].Trim().ToUpper().Equals(obisCode.Trim().ToUpper()) && LSScaleClassID[counter] == classId)
                    return counter;
            }
            return 0;
        }
        private void ApplyScaleToLS(BillingGeneralNFDLMSEntity master)
        {
            bool isHTCT = false;
            int columnCounterForRow = 0;
            try
            {
                for (int counter = 0; counter < pocketLength; counter++)
                {
                    columnCounterForRow = 0;
                    for (int innerCounter = 0; innerCounter < LSScaleOBISCodeDataUnitValue.Length; innerCounter++)
                    {
                        if (CheckAndConvertDate(counter, innerCounter))
                            continue;
                        if (LSScaleOBISCodeDataUnitValue[innerCounter] == null)
                        {
                            continue;
                        }
                        else
                        {
                            //increment only if the value is not null
                            columnCounterForRow = columnCounterForRow + 1;
                        }
                                                
                        DLMSObjectType dlmsObjectType = common.GetDLMSObjectType(GetOBISDictionaryColumns[LSOBISCode[columnCounterForRow]]);
                        StructureUnitEntity structureUnitEntity = structureUnitInfoBLL.GetDetailData(Int32.Parse(LSScaleOBISCodeDataUnitValue[innerCounter])) as StructureUnitEntity;
                        int scaleVale = Int32.Parse(LSScaleOBISCodeDataValue[innerCounter]);
                        if ((!(dlmsObjectType == DLMSObjectType.Power)) && (scaleVale == 0))
                        {
                            if (scaleVale == 0)
                            {
                                if (structureUnitEntity != null)
                                {

                                    string unit = common.GetUnit(structureUnitEntity.StructureUnit);
                                    if (unit.ToUpper().Contains("K"))
                                    {
                                        // Added by Swati
                                        string billingOBISCodeValue = (Convert.ToDouble(LSOBISCodeDataValue[counter, innerCounter]) / 1000).ToString("0.000");
                                        LSOBISCodeDataValue[counter, innerCounter] = string.Concat(billingOBISCodeValue, "*", unit);
                                    }
                                    else
                                    {
                                        LSOBISCodeDataValue[counter, innerCounter] = string.Concat(LSOBISCodeDataValue[counter, innerCounter], "*", unit);
                                    }
                                }

                                continue;
                            }
                        }
                        string dataValue = LSOBISCodeDataValue[counter, innerCounter];
                        if (scaleVale > 128)
                        {
                            int ScaleDiff = 256 - scaleVale;
                            // Checking null conditions for bug 72996.
                            if (!string.IsNullOrEmpty(dataValue))
                            {
                                if (isPUMA)
                                {
                                    dataValue = common.GetSignValue(Int32.Parse(dataValue), ScaleDiff, "-", structureUnitEntity.StructureUnit, dlmsObjectType, out isHTCT, true).ToString();
                                }
                                else
                                {
                                    dataValue = common.GetSignValue(Int32.Parse(dataValue), ScaleDiff, "-", structureUnitEntity.StructureUnit).ToString();
                                }
                            }
                        }
                        else
                            // Checking null conditions for bug 72996.
                            if (!string.IsNullOrEmpty(dataValue))
                            {
                                if (isPUMA)
                                {
                                    dataValue = common.GetSignValue(Int32.Parse(dataValue), scaleVale, "+", structureUnitEntity.StructureUnit, dlmsObjectType, out isHTCT, true).ToString();
                                }
                                else
                                {
                                    dataValue = common.GetSignValue(Int32.Parse(dataValue), scaleVale, "+", structureUnitEntity.StructureUnit).ToString();
                                }
                                dataValue = dataValue + "*" + common.GetUnit(structureUnitEntity.StructureUnit,isHTCT);
                            }
                        LSOBISCodeDataValue[counter, innerCounter] = dataValue;
                    }
                }

                //Assign Meter Type to MeterDataType property 
                master.MeterDataType = (isHTCT) ? MeterDataTypes.HTCT : MeterDataTypes.LTCT;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool CheckAndConvertDate(int counter, int innerCounter)
        {
            try
            {
                if (innerCounter == 0)
                {
                    string val = LSOBISCodeDataValue[counter, innerCounter];
                    LSOBISCodeDataValue[counter, innerCounter] = common.GetDateTimeString(val);
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return false;
        }
    }
}

