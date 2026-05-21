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

    public class DLMS650FormatterTamper: ReadBase
    {
        private DLMS650StructureInfoBLL structureInfoBLL;
        private DLMS650StructureUnitInfoBLL structureUnitInfoBLL;
        private DLMS650FormatterCommon common;
        string[] TamperOBISCode, TamperScaleOBISCode, TamperScaleOBISCodeDataValue, TamperScaleOBISCodeDataUnitValue;
        string[,] TamperOBISCodeDataValue;
        int[] TamperScaleClassID, TamperScaleAttribute, TamperClassID, TamperAttribute, TamperDataIndex;
        int structure, structureLength, dataLength, datatype, totByte, pocketLength, pocketDataLength;
        bool IsCurrent = false;
        bool isPUMA = false;
        DLMSObjectType dlmsObjectType = DLMSObjectType.None;
        public DLMS650FormatterTamper()
        {
            structureInfoBLL = new DLMS650StructureInfoBLL();
            structureUnitInfoBLL = new DLMS650StructureUnitInfoBLL();
            common = new DLMS650FormatterCommon();
            if (UtilityDetails.GetUtilityDetails() == UtilityEntity.Generic)
            {
                isPUMA = true;
            }
        }
        public DLMS650FormatterTamper(UtilityEntity utilityEntity)
        {
            structureInfoBLL = new DLMS650StructureInfoBLL();
            structureUnitInfoBLL = new DLMS650StructureUnitInfoBLL();
            common = new DLMS650FormatterCommon();
            if (UtilityEntity.Generic == utilityEntity)
            {
                isPUMA = true;
            }
        }
        public void LoadTamperData(string[] data, BillingGeneralNFDLMSEntity master)
        {
            int counter = 0;
            int tempCounter = 0;
            int flagCounter = 0;
            for (counter = 0; counter < data.Length; counter++)
            {
                if (string.IsNullOrEmpty(data[counter]))
                {
                    flagCounter++;                    
                }                
            }
            if (flagCounter == 24)
            {
                this.StatusMessage = "Tamper data not found.";
                Application.DoEvents();
                return;
            }
            this.StatusMessage = "Uploading Tamper data......";
            Application.DoEvents();
            master.Tamper = new List<DLMS650TamperEntity>();
            for (int TempCount = 0; TempCount < 24;)
            {
                string captureObject = data[TempCount];
                string captureObjectData = data[TempCount+1];
                string captureObjectScalerUnit = data[TempCount+2];
                string captureObjectDataScalerUnit = data[TempCount+3];
                if (string.IsNullOrEmpty(captureObjectData))
                {
                    TempCount+=4;
                    continue;
                }
                int index = 0;
                int array = common.ConvertHexToDecimal(captureObject.Substring(index, 2)); index += 2;
                int arrayLength = common.ConvertHexToDecimal(captureObject.Substring(index, 2)); index += 2;

                TamperOBISCode = new string[arrayLength];
                TamperClassID = new int[arrayLength];
                TamperAttribute = new int[arrayLength];
                TamperDataIndex = new int[arrayLength];
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
                        TamperClassID[counter] = common.ConvertHexToDecimal(captureObject.Substring(index, totByte));
                        index += totByte;
                    }
                    //OBIS Code
                    datatype = common.ConvertHexToDecimal(captureObject.Substring(index, 2)); index += 2;
                    dataLength = common.ConvertHexToDecimal(captureObject.Substring(index, 2)); index += 2;
                    TamperOBISCode[counter] = common.GetOBISCode(captureObject.Substring(index, (dataLength * 2))); index += (dataLength * 2);
                    //Attribute
                    datatype = common.ConvertHexToDecimal(captureObject.Substring(index, 2)); index += 2;
                    structureInfoEntity = structureInfoBLL.GetDetailData(datatype) as StructureInfoEntity;
                    if (structureInfoEntity != null)
                    {
                        totByte = structureInfoEntity.ValueInByte * 2;
                        TamperAttribute[counter] = common.ConvertHexToDecimal(captureObject.Substring(index, totByte));
                        index += totByte;
                    }
                    //DataIndex
                    datatype = common.ConvertHexToDecimal(captureObject.Substring(index, 2)); index += 2;
                    structureInfoEntity = structureInfoBLL.GetDetailData(datatype) as StructureInfoEntity;
                    if (structureInfoEntity != null)
                    {
                        totByte = structureInfoEntity.ValueInByte * 2;
                        TamperDataIndex[counter] = common.ConvertHexToDecimal(captureObject.Substring(index, totByte));
                        index += totByte;
                    }
                    counter++;
                }
                #endregion

                #region Reading Capture Data

                counter = 0;
                index = 0;
                tempCounter = 0;
                array = common.ConvertHexToDecimal(captureObjectData.Substring(index, 2)); index += 2;
                pocketLength = common.ConvertHexToDecimal(captureObjectData.Substring(index, 2)); index += 2;
                if (pocketLength > 128)
                {
                    tempCounter = pocketLength - 128;
                    // number of bytes
                    pocketLength = common.ConvertHexToDecimal(captureObjectData.Substring(index, tempCounter * 2)); index +=  tempCounter * 2;
                }
                structure = common.ConvertHexToDecimal(captureObjectData.Substring(index, 2)); index += 2;
                pocketDataLength = common.ConvertHexToDecimal(captureObjectData.Substring(index, 2)); index += 2;
             

                TamperOBISCodeDataValue = new string[pocketLength, pocketDataLength];
                int pocketCounter = 0;
                while (counter < pocketLength)//arrayLength
                {
                    int innerCounter = 0;
                    while (innerCounter < pocketDataLength)
                    {
                        datatype = common.ConvertHexToDecimal(captureObjectData.Substring(index, 2)); index += 2;
                        dataLength = 0;
                        totByte = 0;
                        if (datatype == 9 || datatype == 10)
                        {
                            dataLength = common.ConvertHexToDecimal(captureObjectData.Substring(index, 2)); index += 2;
                            TamperOBISCodeDataValue[pocketCounter, innerCounter] = captureObjectData.Substring(index, (dataLength * 2));
                            index += (dataLength * 2);
                        }
                        else
                        {
                            structureInfoEntity = structureInfoBLL.GetDetailData(datatype) as StructureInfoEntity;
                            if (structureInfoEntity != null)
                            {
                                totByte = structureInfoEntity.ValueInByte * 2;
                                TamperOBISCodeDataValue[pocketCounter, innerCounter] = common.ConvertHexToDecimal(captureObjectData.Substring(index, totByte)).ToString();
                                index += totByte;
                            }
                        }
                        innerCounter++;
                    }
                    pocketCounter++;
                    index += 4;
                    counter++;
                }
                #endregion

                #region capture Scaler Object
                counter = 0;
                index = 0;
                array = common.ConvertHexToDecimal(captureObjectScalerUnit.Substring(index, 2)); index += 2;
                arrayLength = common.ConvertHexToDecimal(captureObjectScalerUnit.Substring(index, 2)); index += 2;

                TamperScaleOBISCode = new string[arrayLength];
                TamperScaleClassID = new int[arrayLength];
                TamperScaleAttribute = new int[arrayLength];
                TamperScaleOBISCodeDataValue = new string[arrayLength];
                TamperScaleOBISCodeDataUnitValue = new string[arrayLength];
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
                        TamperScaleClassID[counter] = common.ConvertHexToDecimal(captureObjectScalerUnit.Substring(index, totByte));
                        index += totByte;
                    }
                    //OBIS Code
                    datatype = common.ConvertHexToDecimal(captureObjectScalerUnit.Substring(index, 2)); index += 2;
                    dataLength = common.ConvertHexToDecimal(captureObjectScalerUnit.Substring(index, 2)); index += 2;
                    TamperScaleOBISCode[counter] = common.GetOBISCode(captureObjectScalerUnit.Substring(index, (dataLength * 2))); index += (dataLength * 2);
                    //Attribute
                    datatype = common.ConvertHexToDecimal(captureObjectScalerUnit.Substring(index, 2)); index += 2;
                    structureInfoEntity = structureInfoBLL.GetDetailData(datatype) as StructureInfoEntity;
                    if (structureInfoEntity != null)
                    {
                        totByte = structureInfoEntity.ValueInByte * 2;
                        TamperScaleAttribute[counter] = common.ConvertHexToDecimal(captureObjectScalerUnit.Substring(index, totByte));
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
                        TamperScaleOBISCodeDataValue[counter] = captureObjectDataScalerUnit.Substring(index, (dataLength * 2));
                        index += (dataLength * 2);
                    }
                    else
                    {
                        structureInfoEntity = structureInfoBLL.GetDetailData(datatype) as StructureInfoEntity;
                        if (structureInfoEntity != null)
                        {
                            totByte = structureInfoEntity.ValueInByte * 2;
                            TamperScaleOBISCodeDataValue[counter] = common.ConvertHexToDecimal(captureObjectDataScalerUnit.Substring(index, totByte)).ToString();
                            index += totByte;
                        }
                    }
                    datatype = common.ConvertHexToDecimal(captureObjectDataScalerUnit.Substring(index, 2)); index += 2;
                    if (datatype == 9 || datatype == 10)
                    {
                        dataLength = common.ConvertHexToDecimal(captureObjectDataScalerUnit.Substring(index, 2)); index += 2;
                        TamperScaleOBISCodeDataValue[counter] = captureObjectDataScalerUnit.Substring(index, (dataLength * 2));
                        index += (dataLength * 2);
                    }
                    else
                    {
                        structureInfoEntity = structureInfoBLL.GetDetailData(datatype) as StructureInfoEntity;
                        if (structureInfoEntity != null)
                        {
                            totByte = structureInfoEntity.ValueInByte * 2;
                            TamperScaleOBISCodeDataUnitValue[counter] = common.ConvertHexToDecimal(captureObjectDataScalerUnit.Substring(index, totByte)).ToString();
                            index += totByte;
                        }
                    }
                    index += 4;
                    counter++;
                }
                #endregion

                ApplyScaleToTamper(master);
                TempCount += 4;
                GetTamperEntity(master, TempCount/4); 
            }
        }
        private void GetTamperEntity(BillingGeneralNFDLMSEntity master,int compartment)
        { 
            for (int counter = 0; counter < pocketLength; counter++)
            {
                if (Int64.Parse(TamperOBISCodeDataValue[counter, 1]) == 0)
                    continue;
                DLMS650TamperEntity TamperEntity = new DLMS650TamperEntity();
                for (int innerCounter = 0; innerCounter < pocketDataLength; innerCounter++)
                {
                    switch (innerCounter)
                    {  
                        case 0:
                            TamperEntity.DateTimeEvent = Int64.Parse(TamperOBISCodeDataValue[counter, innerCounter]);
                            break;
                        case 1:
                            TamperEntity.EventCode =Int64.Parse(TamperOBISCodeDataValue[counter, innerCounter]);
                            break;
                        case 2:
                            TamperEntity.CurrentIR = TamperOBISCodeDataValue[counter, innerCounter];
                            break;
                        case 3:
                            TamperEntity.CurrentIY = TamperOBISCodeDataValue[counter, innerCounter];
                            break;
                        case 4:
                            TamperEntity.CurrentIB = TamperOBISCodeDataValue[counter, innerCounter];
                            break;
                        case 5:
                            TamperEntity.VoltageVRN = TamperOBISCodeDataValue[counter, innerCounter];
                            break;
                        case 6:
                            TamperEntity.VoltageVYN = TamperOBISCodeDataValue[counter, innerCounter];
                            break;
                        case 7:
                            TamperEntity.VoltageVBN = TamperOBISCodeDataValue[counter, innerCounter];
                            break;
                        case 8:
                            TamperEntity.PowerFactorRphase = TamperOBISCodeDataValue[counter, innerCounter];
                            break;
                        case 9:
                            TamperEntity.PowerFactorYphase = TamperOBISCodeDataValue[counter, innerCounter];
                            break;
                        case 10:
                            TamperEntity.PowerFactorBphase = TamperOBISCodeDataValue[counter, innerCounter];
                            break;
                        case 11:
                            TamperEntity.CumulativeEnergykWh = TamperOBISCodeDataValue[counter, innerCounter];
                            break;
                        case 12:
                            //BhardwajG : Added for NDPL PUMA : If the tamper profile has 13 parameters in snapshot. 13th parameter
                            //is apparent energy. 
                            TamperEntity.CumulativeEnergykVAh = TamperOBISCodeDataValue[counter, innerCounter];
                            break; 
                    }
                }
                TamperEntity.CompartmentNumber = compartment;
                master.Tamper.Add(TamperEntity);
            } 
        }

        private int GetScaleIndex(string obisCode, int classId, int attributeId)
        {
            for (int counter = 0; counter < pocketDataLength; counter++)
            {
                if (TamperScaleOBISCode[counter].Trim().ToUpper().Equals(obisCode.Trim().ToUpper()) && TamperScaleClassID[counter] == classId)
                    return counter;
            }
            return 0;
        }
        private void ApplyScaleToTamper(BillingGeneralNFDLMSEntity master)
        {
            DLMSObjectType dlmsObjectType;
            bool isHTCT = false;
            try
            {
                for (int counter = 0; counter < pocketLength; counter++)
                {
                    if (Int64.Parse(TamperOBISCodeDataValue[counter, 1]) == 0)
                        continue;
                    for (int innerCounter = 0; innerCounter < pocketDataLength; innerCounter++)
                    {
                        if (CheckAndConvertDate(counter, innerCounter))
                            continue;
                        int scaleId = GetScaleIndex(TamperOBISCode[innerCounter], TamperClassID[innerCounter], TamperAttribute[innerCounter]);
                        StructureUnitEntity structureUnitEntity = structureUnitInfoBLL.GetDetailData(Int32.Parse(TamperScaleOBISCodeDataUnitValue[scaleId])) as StructureUnitEntity;
                        dlmsObjectType = GetDLMSObjectType(TamperOBISCode[innerCounter], TamperClassID[innerCounter], TamperAttribute[innerCounter]);
                        int scaleVale = Int32.Parse(TamperScaleOBISCodeDataValue[scaleId]);
                        if (scaleVale == 0)
                        {
                            if (structureUnitEntity != null)
                            {
                                if (!string.IsNullOrEmpty(structureUnitEntity.StructureUnit))//Hack need to do:
                                {
                                    //TamperOBISCodeDataValue[counter, innerCounter] = string.Concat(TamperOBISCodeDataValue[counter, innerCounter], "*", common.GetUnit(structureUnitEntity.StructureUnit));
                                    string unit = common.GetUnit(structureUnitEntity.StructureUnit);
                                    if (unit.ToUpper().Contains("K"))
                                    {
                                        //int mode = Convert.ToInt32(TamperOBISCodeDataValue[counter,innerCounter]) % 1000;
                                        //int reminder = Convert.ToInt32(TamperOBISCodeDataValue[counter, innerCounter]) / 1000;
                                        //string obisCodeDataVlaue = reminder.ToString() + "." + mode.ToString();
                                        
                                        // Added by Swati for DLMS_0084
                                        string obisCodeDataVlaue = (Convert.ToDouble(TamperOBISCodeDataValue[counter, innerCounter]) / 1000).ToString("0.000");
                                        TamperOBISCodeDataValue[counter, innerCounter] = string.Concat(obisCodeDataVlaue, "*", unit);
                                    }
                                    else
                                    {
                                        TamperOBISCodeDataValue[counter, innerCounter] = string.Concat(TamperOBISCodeDataValue[counter, innerCounter], "*", unit);
                                    }
                                }
                                else
                                {
                                    TamperOBISCodeDataValue[counter, innerCounter] = TamperOBISCodeDataValue[counter, innerCounter];
                                }

                                
                                continue;
                            }
                        }
                        string dataValue = TamperOBISCodeDataValue[counter, innerCounter];
                        if (scaleVale > 128)
                        {
                            int ScaleDiff = 256 - scaleVale;
                            if (isPUMA)
                            {
                                dataValue = common.GetSignValue(Int32.Parse(dataValue), ScaleDiff, "-", structureUnitEntity.StructureUnit, dlmsObjectType, out isHTCT, false).ToString();
                            }
                            else
                            {
                                dataValue = common.GetSignValue(Int32.Parse(dataValue), ScaleDiff, "-", structureUnitEntity.StructureUnit).ToString();
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
                                dataValue = common.GetSignValue(Int32.Parse(dataValue), scaleVale, "+", structureUnitEntity.StructureUnit).ToString();
                            }
                        }
                        if (!string.IsNullOrEmpty(structureUnitEntity.StructureUnit))
                        {
                            if (isPUMA)
                            {
                                dataValue = dataValue + "*" + common.GetUnit(structureUnitEntity.StructureUnit, isHTCT);
                            }
                            else
                            {
                                dataValue = dataValue + "*" + common.GetUnit(structureUnitEntity.StructureUnit);
                            }
                        }
                        TamperOBISCodeDataValue[counter, innerCounter] = dataValue;
                    }
                }

                //Assign Meter Type to MeterDataType property 
                master.MeterDataType = (isHTCT) ? MeterDataTypes.HTCT : MeterDataTypes.LTCT;
            }
            catch (Exception)
            {

            }
        }

        private bool CheckAndConvertDate(int counter, int innerCounter)
        {

            if (innerCounter == 0 || innerCounter == 23 || innerCounter == 25 || innerCounter == 27 || innerCounter == 29 || innerCounter == 31 || innerCounter == 33 || innerCounter == 35 || innerCounter == 37 || innerCounter == 39 || innerCounter == 41 || innerCounter == 43 || innerCounter == 45 || innerCounter == 47 || innerCounter == 49 || innerCounter == 51 || innerCounter == 53 || innerCounter == 55 || innerCounter == 57)
            {
                 string val = TamperOBISCodeDataValue[counter, innerCounter];
                 if (innerCounter == 0)
                 {
                     if (val.Substring(14, 2).Trim().ToUpper().Equals("FF"))
                         IsCurrent = false;
                     else
                         IsCurrent = true;
                 }
                TamperOBISCodeDataValue[counter, innerCounter] = common.GetDateTimeString(val);
                return true;
            }
            return false;
        }
        private DLMSObjectType GetDLMSObjectType(string columnName, int classID, int attribute)
        {
            if (columnName == "0.0.1.0.0.255" && classID == 8 && attribute == 2)
            {
                return DLMSObjectType.None;
            }
            else if (columnName == "0.0.96.11.0.255" && classID == 1 && attribute == 2)
            {
                return DLMSObjectType.None;
            }
            else if (columnName == "1.0.31.7.0.255" && classID == 3 && attribute == 2)
            {
                return DLMSObjectType.None;
            }
            else if (columnName == "1.0.51.7.0.255" && classID == 3 && attribute == 2)
            {
                return DLMSObjectType.None;
            }
            else if (columnName == "1.0.71.7.0.255" && classID == 3 && attribute == 2)
            {
                return DLMSObjectType.None;
            }
            else if (columnName == "1.0.32.7.0.255" && classID == 3 && attribute == 2)
            {
                return DLMSObjectType.None;
            }
            else if (columnName == "1.0.52.7.0.255" && classID == 3 && attribute == 2)
            {
                return DLMSObjectType.None;
            }
            else if (columnName == "1.0.72.7.0.255" && classID == 3 && attribute == 2)
            {
                return DLMSObjectType.None;
            }
            else if (columnName == "1.0.33.7.0.255" && classID == 3 && attribute == 2)
            {
                return DLMSObjectType.None;
            }
            else if (columnName == "1.0.53.7.0.255" && classID == 3 && attribute == 2)
            {
                return DLMSObjectType.None;
            }
            else if (columnName == "1.0.73.7.0.255" && classID == 3 && attribute == 2)
            {
                return DLMSObjectType.None;
            }
            else if (columnName == "1.0.1.8.0.255" && classID == 3 && attribute == 2)
            {
                return DLMSObjectType.Energy;
            }
            else
            {
                return DLMSObjectType.None;
            }

        }
    }
}
