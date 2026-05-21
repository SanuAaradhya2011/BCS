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
    public class DLMS650FormatterMidnightData : ReadBase
    {
        DLMS650StructureInfoBLL structureInfoBLL;
        DLMS650StructureUnitInfoBLL structureUnitInfoBLL; 
        private DLMS650FormatterCommon common;
        string[] MidDataOBISCode, MidDataOBISCodeTemp, MidDataScaleOBISCode, MidDataScaleOBISCodeDataValue, MidDataScaleOBISCodeDataUnitValue;
        string[,] MidDataOBISCodeDataValue;
        int[] MidDataScaleClassID, MidDataScaleAttribute, MidDataClassID, MidDataAttribute, MidDataDataIndex;
        int structure, structureLength, dataLength, datatype, totByte, pocketLength, pocketDataLength;
        Dictionary<string, string> GetOBISDictionary = new Dictionary<string, string>();
        Dictionary<string, string> GetOBISDictionaryColumns = new Dictionary<string, string>();
        //LoadSurveyParameterEntity lsParameterEntity = new LoadSurveyParameterEntity();
        public Dictionary<string, string> OBISColumns = new Dictionary<string, string>();
        public Dictionary<string, string> OBISLoadSurveyColumns = new Dictionary<string, string>();
        bool isPUMA = false;
        public DLMS650FormatterMidnightData()
        {
            structureInfoBLL = new DLMS650StructureInfoBLL();
            structureUnitInfoBLL = new DLMS650StructureUnitInfoBLL();
            common = new DLMS650FormatterCommon();
            if (UtilityDetails.GetUtilityDetails() == UtilityEntity.Generic)
            {
                isPUMA = true;
            }
        }
        public DLMS650FormatterMidnightData(UtilityEntity utilityEntity)
        {
            structureInfoBLL = new DLMS650StructureInfoBLL();
            structureUnitInfoBLL = new DLMS650StructureUnitInfoBLL();
            common = new DLMS650FormatterCommon();
            if (utilityEntity == UtilityEntity.Generic)
            {
                isPUMA = true;
            }
        }
        public DLMS650FormatterMidnightData(bool IsPUMA)
        {
            structureInfoBLL = new DLMS650StructureInfoBLL();
            structureUnitInfoBLL = new DLMS650StructureUnitInfoBLL();
            common = new DLMS650FormatterCommon();
            isPUMA = IsPUMA;            
        }
        public void LoadMidnightData(string[] data, BillingGeneralNFDLMSEntity master)
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
                this.StatusMessage = "Midnight data not found.";
                Application.DoEvents();
                return;
            }
            this.StatusMessage = "Uploading Midnight Data......";
            Application.DoEvents();

            GetOBISDictionary = GetOBISCodeValue();
            GetOBISDictionaryColumns = GetOBISCodeColumnNames();
            
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

            MidDataOBISCode = new string[arrayLength];
            MidDataClassID = new int[arrayLength];
            MidDataAttribute = new int[arrayLength];
            MidDataDataIndex = new int[arrayLength];
            MidDataOBISCodeTemp = new string[GetOBISDictionary.Count];

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
                    MidDataClassID[counter] = common.ConvertHexToDecimal(captureObject.Substring(index, totByte));
                    index += totByte;
                }
                //OBIS Code
                datatype = common.ConvertHexToDecimal(captureObject.Substring(index, 2)); index += 2;
                dataLength = common.ConvertHexToDecimal(captureObject.Substring(index, 2)); index += 2;
                MidDataOBISCode[counter] = common.GetOBISCode(captureObject.Substring(index, (dataLength * 2))); index += (dataLength * 2);
               
                //Attribute
                datatype = common.ConvertHexToDecimal(captureObject.Substring(index, 2)); index += 2;
                structureInfoEntity = structureInfoBLL.GetDetailData(datatype) as StructureInfoEntity;
                if (structureInfoEntity != null)
                {
                    totByte = structureInfoEntity.ValueInByte * 2;
                    MidDataAttribute[counter] = common.ConvertHexToDecimal(captureObject.Substring(index, totByte));
                    index += totByte;
                }
                //DataIndex
                datatype = common.ConvertHexToDecimal(captureObject.Substring(index, 2)); index += 2;
                structureInfoEntity = structureInfoBLL.GetDetailData(datatype) as StructureInfoEntity;
                if (structureInfoEntity != null)
                {
                    totByte = structureInfoEntity.ValueInByte * 2;
                    MidDataDataIndex[counter] = common.ConvertHexToDecimal(captureObject.Substring(index, totByte));
                    index += totByte;
                }
                counter++;
            }


            for (int count = 0; count < MidDataOBISCode.Length; count++)
            {
                foreach (KeyValuePair<string, string> pair in GetOBISDictionary)
                {
                    if (MidDataOBISCode[count].ToString() == pair.Key)
                    {
                        MidDataOBISCodeTemp[count] = pair.Value;

                        break;
                    }
                }
            }
            for (int colCount = 0; colCount < MidDataOBISCode.Length; colCount++)
            {
                foreach (KeyValuePair<string, string> pair in GetOBISDictionaryColumns)
                {
                    if (MidDataOBISCode[colCount].ToString() == pair.Key)
                    {
                        lsColumnNames += pair.Value + ",";
                        break;
                    }
                }
            }
            
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

            MidDataOBISCodeDataValue = new string[pocketLength, MidDataOBISCodeTemp.Length];//pocketDataLength];
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
                        MidDataOBISCodeDataValue[counter, Convert.ToInt16(MidDataOBISCodeTemp[innerCounter])] = captureObjectData.Substring(index, (dataLength * 2));
                        index += (dataLength * 2);
                    }
                    else
                    {
                        structureInfoEntity = structureInfoBLL.GetDetailData(datatype) as StructureInfoEntity;
                        if (structureInfoEntity != null)
                        {
                            totByte = structureInfoEntity.ValueInByte * 2;
                            //LSOBISCodeDataValue[counter,innerCounter] = captureObjectData.Substring(index, (dataLength * 2));  this line was changed to solve bug 73240; 11th april 2012
                            MidDataOBISCodeDataValue[counter, Convert.ToInt16(MidDataOBISCodeTemp[innerCounter])] = common.ConvertHexToDecimal(captureObjectData.Substring(index, totByte)).ToString();
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
            MidDataScaleOBISCode = new string[arrayLength];
            MidDataScaleClassID = new int[arrayLength];
            MidDataScaleAttribute = new int[arrayLength];
            MidDataScaleOBISCodeDataValue = new string[arrayLength];
            MidDataScaleOBISCodeDataUnitValue = new string[arrayLength];

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
                    MidDataScaleClassID[counter] = common.ConvertHexToDecimal(captureObjectScalerUnit.Substring(index, totByte));
                    index += totByte;
                }
                //OBIS Code
                datatype = common.ConvertHexToDecimal(captureObjectScalerUnit.Substring(index, 2)); index += 2;
                dataLength = common.ConvertHexToDecimal(captureObjectScalerUnit.Substring(index, 2)); index += 2;
                MidDataScaleOBISCode[counter] = common.GetOBISCode(captureObjectScalerUnit.Substring(index, (dataLength * 2))); index += (dataLength * 2);
                //Attribute
                datatype = common.ConvertHexToDecimal(captureObjectScalerUnit.Substring(index, 2)); index += 2;
                structureInfoEntity = structureInfoBLL.GetDetailData(datatype) as StructureInfoEntity;
                if (structureInfoEntity != null)
                {
                    totByte = structureInfoEntity.ValueInByte * 2;
                    MidDataScaleAttribute[counter] = common.ConvertHexToDecimal(captureObjectScalerUnit.Substring(index, totByte));
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
                    MidDataScaleOBISCodeDataValue[counter] = captureObjectDataScalerUnit.Substring(index, (dataLength * 2));
                    index += (dataLength * 2);
                }
                else
                {
                    structureInfoEntity = structureInfoBLL.GetDetailData(datatype) as StructureInfoEntity;
                    if (structureInfoEntity != null)
                    {
                        totByte = structureInfoEntity.ValueInByte * 2;
                        MidDataScaleOBISCodeDataValue[counter] = common.ConvertHexToDecimal(captureObjectDataScalerUnit.Substring(index, totByte)).ToString();
                        index += totByte;
                    }
                }
                datatype = common.ConvertHexToDecimal(captureObjectDataScalerUnit.Substring(index, 2)); index += 2;
                if (datatype == 9 || datatype == 10)
                {
                    dataLength = common.ConvertHexToDecimal(captureObjectDataScalerUnit.Substring(index, 2)); index += 2;
                    MidDataScaleOBISCodeDataValue[counter] = captureObjectDataScalerUnit.Substring(index, (dataLength * 2));
                    index += (dataLength * 2);
                }
                else
                {
                    structureInfoEntity = structureInfoBLL.GetDetailData(datatype) as StructureInfoEntity;
                    if (structureInfoEntity != null)
                    {
                        totByte = structureInfoEntity.ValueInByte * 2;
                        MidDataScaleOBISCodeDataUnitValue[counter] = common.ConvertHexToDecimal(captureObjectDataScalerUnit.Substring(index, totByte)).ToString();
                        index += totByte;
                    }
                }
                index += 4;
                counter++;//MidDataScaleOBISCodeDataValue
            }

            // This code is used for solving indexing issue with no of parameter.
            //string[] LSScaleOBISCodeDataValueTemp = new string[MidDataOBISCodeTemp.Length];

            //for (int i = 0; i < MidDataScaleOBISCodeDataValue.Length; i++)
            //{
            //    LSScaleOBISCodeDataValueTemp[Convert.ToInt32(MidDataOBISCodeTemp[i])] = MidDataScaleOBISCodeDataValue[i];
            //}
            //MidDataScaleOBISCodeDataValue = new string[MidDataOBISCodeTemp.Length];
            //MidDataScaleOBISCodeDataValue = LSScaleOBISCodeDataValueTemp;



            //string[] LSScaleOBISCodeDataUnitValueTemp = new string[MidDataOBISCodeTemp.Length];

            //for (int i = 0; i < MidDataScaleOBISCodeDataUnitValue.Length; i++)
            //{
            //    LSScaleOBISCodeDataUnitValueTemp[Convert.ToInt32(MidDataOBISCodeTemp[i])] = MidDataScaleOBISCodeDataUnitValue[i];
            //}
            //MidDataScaleOBISCodeDataUnitValue = new string[MidDataOBISCodeTemp.Length];
            //MidDataScaleOBISCodeDataUnitValue = MidDataScaleOBISCodeDataUnitValueTemp;

            #endregion

            ApplyScaleToMidData(master);
            master.MidnightData = GetMidnightDataEntity();
            
        }

        public Dictionary<string, string> GetOBISCodeValue()
        {       
           
            OBISColumns.Add("0.0.1.0.0.255", "0");
            OBISColumns.Add("1.0.1.8.0.255", "1");
            OBISColumns.Add("1.0.5.8.0.255", "2");
            OBISColumns.Add("1.0.8.8.0.255", "3");
            OBISColumns.Add("1.0.9.8.0.255", "4");
            OBISColumns.Add("1.0.2.8.0.255", "5");
            OBISColumns.Add("1.0.7.8.0.255", "6");
            OBISColumns.Add("1.0.6.8.0.255", "7");
            OBISColumns.Add("1.0.10.8.0.255", "8");

            OBISColumns.Add("1.0.143.128.128.255", "9"); // kWh import OBIS 
            OBISColumns.Add("1.0.145.128.128.255", "10"); //  kvarhlag Q1 OBIS
            OBISColumns.Add("1.0.146.128.128.255", "11"); //kvarhlead Q4 OBIS
            OBISColumns.Add("1.0.144.128.128.255", "12");  //kVAh import OBIS
            return OBISColumns;
        }

        public Dictionary<string, string> GetOBISCodeColumnNames()
        {
            OBISLoadSurveyColumns.Add("0.0.1.0.0.255", "realTimeClockDateandTime");
            OBISLoadSurveyColumns.Add("1.0.1.8.0.255", "blockEnergykWh");
            OBISLoadSurveyColumns.Add("1.0.5.8.0.255", "blockEnergykvarhlag");
            OBISLoadSurveyColumns.Add("1.0.8.8.0.255", "blockEnergykvarhlead");
            OBISLoadSurveyColumns.Add("1.0.9.8.0.255", "blockEnergykVAh");
            OBISLoadSurveyColumns.Add("1.0.2.8.0.255", "blockEnergykWhExport");
            OBISLoadSurveyColumns.Add("1.0.7.8.0.255", "blockEnergykvarhlagQ3");
            OBISLoadSurveyColumns.Add("1.0.6.8.0.255", "blockEnergykvarhleadQ2");
            OBISLoadSurveyColumns.Add("1.0.10.8.0.255", "blockEnergykVAhExport");
            OBISLoadSurveyColumns.Add("1.0.147.128.128.255", "blockEnergykWhImport");
            OBISLoadSurveyColumns.Add("1.0.149.128.128.255", "blockEnergykvarhlagQ1"); 
            OBISLoadSurveyColumns.Add("1.0.150.128.128.255", "blockEnergykvarhleadQ4");
            OBISLoadSurveyColumns.Add("1.0.148.128.128.255", "blockEnergykVAhImport");
            return OBISLoadSurveyColumns;
        }

        private List<DLMS650MidnightDataEntity> GetMidnightDataEntity()
        {
            List<DLMS650MidnightDataEntity> midData = new List<DLMS650MidnightDataEntity>();
            for (int counter = 0; counter < pocketLength; counter++)
            {
                DLMS650MidnightDataEntity midDataEntity = new DLMS650MidnightDataEntity();
                for (int innerCounter = 0; innerCounter < pocketDataLength; innerCounter++)
                {
                    switch (Convert.ToInt16(MidDataOBISCodeTemp[innerCounter]))
                    {
                        case 0:
                            midDataEntity.RealTimeClockDateandTime = Int64.Parse(MidDataOBISCodeDataValue[counter, Convert.ToInt16(MidDataOBISCodeTemp[innerCounter])]);
                            break;
                        case 1:
                            midDataEntity.CumEnergykWh = MidDataOBISCodeDataValue[counter, Convert.ToInt16(MidDataOBISCodeTemp[innerCounter])];
                            break;
                        case 2:
                            midDataEntity.CumEnergykvarhlag = MidDataOBISCodeDataValue[counter, Convert.ToInt16(MidDataOBISCodeTemp[innerCounter])];
                            break;
                        case 3:
                            midDataEntity.CumEnergykvarhlead = MidDataOBISCodeDataValue[counter, Convert.ToInt16(MidDataOBISCodeTemp[innerCounter])];
                            break;
                        case 4:
                            midDataEntity.CumEnergykVAh = MidDataOBISCodeDataValue[counter, Convert.ToInt16(MidDataOBISCodeTemp[innerCounter])];
                            break;
                        case 5:
                            midDataEntity.CumEnergykWhExport = MidDataOBISCodeDataValue[counter, Convert.ToInt16(MidDataOBISCodeTemp[innerCounter])];
                            break;
                        case 6:
                            midDataEntity.CumEnergykvarhlagQ3 = MidDataOBISCodeDataValue[counter, Convert.ToInt16(MidDataOBISCodeTemp[innerCounter])];
                            break;
                        case 7:
                            midDataEntity.CumEnergykvarhleadQ2 = MidDataOBISCodeDataValue[counter, Convert.ToInt16(MidDataOBISCodeTemp[innerCounter])];
                            break;
                        case 8:
                            midDataEntity.CumEnergykVAhExport = MidDataOBISCodeDataValue[counter, Convert.ToInt16(MidDataOBISCodeTemp[innerCounter])];
                            break;
                        case 9:
                            midDataEntity.CumEnergykWhImport = MidDataOBISCodeDataValue[counter, Convert.ToInt16(MidDataOBISCodeTemp[innerCounter])];
                            break;
                        case 10:
                            midDataEntity.CumEnergykvarhlagQ1 = MidDataOBISCodeDataValue[counter, Convert.ToInt16(MidDataOBISCodeTemp[innerCounter])];
                            break;
                        case 11:
                            midDataEntity.CumEnergykvarhleadQ4 = MidDataOBISCodeDataValue[counter, Convert.ToInt16(MidDataOBISCodeTemp[innerCounter])];
                            break;
                        case 12:
                            midDataEntity.CumEnergykVAhImport = MidDataOBISCodeDataValue[counter, Convert.ToInt16(MidDataOBISCodeTemp[innerCounter])];
                            break;
                    }
                }
                midData.Add(midDataEntity);
            }
            return midData;
        }
        
        private int GetScaleIndex(string obisCode, int classId, int attributeId)
        {
            for (int counter = 0; counter < pocketDataLength; counter++)
            {
                if (MidDataScaleOBISCode[counter].Trim().ToUpper().Equals(obisCode.Trim().ToUpper()) && MidDataScaleClassID[counter] == classId) 
                    return counter;
            }
            return 0;
        }
        private void ApplyScaleToMidData(BillingGeneralNFDLMSEntity master)
        {
            DLMSObjectType dlmsObjectType;
            bool isHTCT = false;
            try
            {
                for (int  counter = 0; counter < pocketLength; counter++)
                {
                    for (int innerCounter = 0; innerCounter < MidDataOBISCodeTemp.Length; innerCounter++)
                    {
                         if (CheckAndConvertDate(counter, innerCounter))
                            continue;


                        if (MidDataScaleOBISCodeDataUnitValue[innerCounter] == null)
                            continue;
                        StructureUnitEntity structureUnitEntity = structureUnitInfoBLL.GetDetailData(Int32.Parse(MidDataScaleOBISCodeDataUnitValue[innerCounter])) as StructureUnitEntity;
                        dlmsObjectType = common.GetDLMSObjectType(OBISLoadSurveyColumns[MidDataOBISCode[innerCounter]]);
                        int scaleVale = Int32.Parse(MidDataScaleOBISCodeDataValue[innerCounter]);
                        if (scaleVale == 0)
                        {
                            if (structureUnitEntity != null)
                            {
                               
                                string unit = common.GetUnit(structureUnitEntity.StructureUnit);
                                if (unit.ToUpper().Contains("K"))
                                {
                                    // Added by Swati
                                    string billingOBISCodeValue = (Convert.ToDouble(MidDataOBISCodeDataValue[counter, innerCounter]) / 1000).ToString("0.000");
                                    MidDataOBISCodeDataValue[counter, innerCounter] = string.Concat(billingOBISCodeValue, "*", unit);
                                }
                                else
                                {
                                    MidDataOBISCodeDataValue[counter, innerCounter] = string.Concat(MidDataOBISCodeDataValue[counter, innerCounter], "*", unit);
                                }
                            }

                            continue;
                        }
                        string dataValue = MidDataOBISCodeDataValue[counter, innerCounter];
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
                        MidDataOBISCodeDataValue[counter, innerCounter] = dataValue;
                    }
                }

                //Assign Meter Type to MeterDataType property 
                master.MeterDataType = (isHTCT) ? MeterDataTypes.HTCT : MeterDataTypes.LTCT;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private bool CheckAndConvertDate(int counter, int innerCounter)
        {

            if (innerCounter == 0)
            {
                string val = MidDataOBISCodeDataValue[counter, innerCounter];
                MidDataOBISCodeDataValue[counter, innerCounter] = common.GetDateTimeString(val);
                return true;
            }
            return false;
        } 
    }
}

