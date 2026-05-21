using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.BLL;
using CAB.Channel.Formatter;
using CAB.Entity;
using CABEntity;
using CAB.Framework;

namespace CHANNEL.Formatter
{
    /// <summary>
    /// This class is used to parse all energy parameters for meter accuracy check.
    /// </summary>
    public class DLMS650FormatterMeterAccuracyCheck
    {
        DLMS650StructureInfoBLL structureInfoBLL;
        DLMS650StructureUnitInfoBLL structureUnitInfoBLL;
        private DLMS650FormatterCommon common;
        string[] AccuracyOBISCode, AccuracyOBISCodeDataValue, AccuracyScaleOBISCode, AccuracyScaleOBISCodeDataValue, AccuracyScaleOBISCodeDataUnitValue;
        int[] AccuracyScaleClassID, AccuracyScaleAttribute, AccuracyClassID, AccuracyAttribute, AccuracyDataIndex;
        int structure, structureLength, dataLength, datatype, totByte;
        MeterAccuracyCheckEntity meterAccuracyCheckEntity; 
        private bool isHTCT = false;
        public DLMS650FormatterMeterAccuracyCheck()
        {
            structureInfoBLL = new DLMS650StructureInfoBLL();
            structureUnitInfoBLL = new DLMS650StructureUnitInfoBLL();
            common = new DLMS650FormatterCommon();
            meterAccuracyCheckEntity = new MeterAccuracyCheckEntity();
        }
        /// <summary>
        /// This method is used for get data for Accuracy check parameters.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public MeterAccuracyCheckEntity GetDataForAccuracyCheck(string data)
        {
            string[] accuracyData = data.Split('\n');
            try
            {
                // To read data and scalar.
                GetCaptureObjects(accuracyData[0]);
                GetCaptureData(accuracyData[1], accuracyData[0]);
                GetCaptureScalarObjects(accuracyData[2]);
                GetCaptureScalarUnitObject(accuracyData[3], accuracyData[2]);
                ApplyScaleToAccuracyParameters();
                meterAccuracyCheckEntity = FillAccuracyCheckEntity();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return meterAccuracyCheckEntity;

        }
        /// <summary>
        /// This method is used to get the scale index to obtain the sacle value for the paarmeter
        /// </summary>
        /// <param name="obisCode"></param>
        /// <param name="classId"></param>
        /// <param name="attributeId"></param>
        /// <returns></returns>
        private int GetScaleIndex(string obisCode, int classId, int attributeId)
        {
            try
            {
                for (int counter = 0; counter < AccuracyScaleOBISCode.Length; counter++)
                {

                    if (!string.IsNullOrEmpty(AccuracyScaleOBISCode[counter]))
                    {
                        if (AccuracyScaleOBISCode[counter].Trim().Equals(obisCode) && AccuracyScaleClassID[counter] == classId)
                            return counter;
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return 0;
        }
        /// <summary>
        /// This method is used to fill the meter accuarcy entity.
        /// </summary>
        /// <returns></returns>
        private MeterAccuracyCheckEntity FillAccuracyCheckEntity()
        {
            try
            {
                for (int counter = 0; counter < AccuracyOBISCodeDataValue.Length; counter++)
                {
                    switch (counter)
                    {

                        case 0:
                            meterAccuracyCheckEntity.KWh =AccuracyOBISCodeDataValue[counter].Substring(0, AccuracyOBISCodeDataValue[counter].IndexOf('*'));
                            break;
                        case 1:
                            meterAccuracyCheckEntity.KvarhLag = AccuracyOBISCodeDataValue[counter].Substring(0, AccuracyOBISCodeDataValue[counter].IndexOf('*'));
                            break;
                        case 2:
                            meterAccuracyCheckEntity.KvarhLead = AccuracyOBISCodeDataValue[counter].Substring(0, AccuracyOBISCodeDataValue[counter].IndexOf('*'));
                            break;
                        case 3:
                            meterAccuracyCheckEntity.KVAh = AccuracyOBISCodeDataValue[counter].Substring(0, AccuracyOBISCodeDataValue[counter].IndexOf('*'));
                            break;
                    }
                }
                /*VBM Apply fixed HTCT check for Excelpower utility , this is temprorary solution and it will be modified later*/
                if (UtilityDetails.PrimaryUtlityName == UtilityEntity.EXCELPOWER.ToString())
                {
                    meterAccuracyCheckEntity.isHTCT = true;
                }
                else
                {
                    meterAccuracyCheckEntity.isHTCT = isHTCT;
                }
                /*VBM Apply fixed HTCT check for Excelpower utility , this is temprorary solution and it will be modified later*/
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return meterAccuracyCheckEntity;
        }
        /// <summary>
        /// This method is used to apply unit and resolution to parameters data.
        /// </summary>
        public void ApplyScaleToAccuracyParameters()
        {
            int counter;
            StructureUnitEntity structureUnitEntity;
            DLMSObjectType dlmsObjectType = DLMSObjectType.Energy;
            int scaleVale = 0;            
            try
            {
                for (counter = 0; counter < AccuracyOBISCodeDataValue.Length; counter++)
                {                    
                    int scaleId = GetScaleIndex(AccuracyOBISCode[counter], AccuracyClassID[counter], AccuracyAttribute[counter]);
                    structureUnitEntity = structureUnitInfoBLL.GetDetailData(Int32.Parse(AccuracyScaleOBISCodeDataUnitValue[scaleId])) as StructureUnitEntity;
                    scaleVale = Int32.Parse(AccuracyScaleOBISCodeDataValue[scaleId]);
                    if (scaleVale == 0)
                    {
                        if (structureUnitEntity != null)
                        {
                            string unit = common.GetUnit(structureUnitEntity.StructureUnit);
                            if (unit.ToUpper().Contains("K"))
                            {
                                string obisCodeDataVlaue = (Convert.ToDouble(AccuracyOBISCodeDataValue[counter]) / 1000).ToString("0.000");
                                AccuracyOBISCodeDataValue[counter] = string.Concat(obisCodeDataVlaue, "*", unit);
                            }

                        }
                        continue;
                    }
                    string dataValue = AccuracyOBISCodeDataValue[counter];
                    if (scaleVale > 128)
                    {
                        int ScaleDiff = 256 - scaleVale;
                        dataValue = common.GetSignValue(Int32.Parse(dataValue), ScaleDiff, "-", structureUnitEntity.StructureUnit, dlmsObjectType,out isHTCT,false).ToString();
                    }
                    else
                        dataValue = common.GetSignValue(Int32.Parse(dataValue), scaleVale, "+", structureUnitEntity.StructureUnit, dlmsObjectType, out isHTCT, false).ToString();

                    dataValue = dataValue + "*" + common.GetUnit(structureUnitEntity.StructureUnit,isHTCT);
                    AccuracyOBISCodeDataValue[counter] = dataValue;
                }
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// This method is used to parse scalar unit data.
        /// </summary>
        /// <param name="captureObjectDataScalerUnit"></param>
        /// <param name="captureObjectScalerUnit"></param>
        private void GetCaptureScalarUnitObject(string captureObjectDataScalerUnit, string captureObjectScalerUnit)
        {
            StructureInfoEntity structureInfoEntity = null;
            int counter = 0, array, arrayLength;
            int index = 0;
            try
            {
                array = common.ConvertHexToDecimal(captureObjectScalerUnit.Substring(index, 2)); index += 2;
                arrayLength = common.ConvertHexToDecimal(captureObjectScalerUnit.Substring(index, 2)); index += 2;
                AccuracyScaleOBISCodeDataValue = new string[arrayLength];
                index = 0;
                array = common.ConvertHexToDecimal(captureObjectDataScalerUnit.Substring(index, 2));
                index += 2;
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
                        AccuracyScaleOBISCodeDataValue[counter] = captureObjectDataScalerUnit.Substring(index, (dataLength * 2));
                        index += (dataLength * 2);
                    }
                    else
                    {
                        structureInfoEntity = structureInfoBLL.GetDetailData(datatype) as StructureInfoEntity;
                        if (structureInfoEntity != null)
                        {
                            totByte = structureInfoEntity.ValueInByte * 2;
                            AccuracyScaleOBISCodeDataValue[counter] = common.ConvertHexToDecimal(captureObjectDataScalerUnit.Substring(index, totByte)).ToString();
                            index += totByte;
                        }
                    }
                    datatype = common.ConvertHexToDecimal(captureObjectDataScalerUnit.Substring(index, 2)); index += 2;
                    if (datatype == 9 || datatype == 10)
                    {
                        dataLength = common.ConvertHexToDecimal(captureObjectDataScalerUnit.Substring(index, 2)); index += 2;
                        AccuracyScaleOBISCodeDataValue[counter] = captureObjectDataScalerUnit.Substring(index, (dataLength * 2));
                        index += (dataLength * 2);
                    }
                    else
                    {
                        structureInfoEntity = structureInfoBLL.GetDetailData(datatype) as StructureInfoEntity;
                        if (structureInfoEntity != null)
                        {
                            totByte = structureInfoEntity.ValueInByte * 2;
                            AccuracyScaleOBISCodeDataUnitValue[counter] = common.ConvertHexToDecimal(captureObjectDataScalerUnit.Substring(index, totByte)).ToString();
                            index += totByte;
                        }
                    }
                    index += 4;
                    counter++;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// This method is used to parse scalar objects.
        /// </summary>
        /// <param name="captureObjectScalerUnit"></param>
        private void GetCaptureScalarObjects(string captureObjectScalerUnit)
        {
            StructureInfoEntity structureInfoEntity = null;
            int counter = 0, array, arrayLength;
            int index = 0;
            try
            {
                array = common.ConvertHexToDecimal(captureObjectScalerUnit.Substring(index, 2)); index += 2;
                arrayLength = common.ConvertHexToDecimal(captureObjectScalerUnit.Substring(index, 2)); index += 2;
                AccuracyScaleAttribute = new int[arrayLength];
                AccuracyScaleClassID = new int[arrayLength];
                AccuracyScaleOBISCode = new string[arrayLength];
                AccuracyScaleOBISCodeDataUnitValue = new string[arrayLength];
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
                        AccuracyScaleClassID[counter] = common.ConvertHexToDecimal(captureObjectScalerUnit.Substring(index, totByte));
                        index += totByte;
                    }
                    //OBIS Code
                    datatype = common.ConvertHexToDecimal(captureObjectScalerUnit.Substring(index, 2)); index += 2;
                    dataLength = common.ConvertHexToDecimal(captureObjectScalerUnit.Substring(index, 2)); index += 2;
                    AccuracyScaleOBISCode[counter] = common.GetOBISCode(captureObjectScalerUnit.Substring(index, (dataLength * 2))); index += (dataLength * 2);
                    //Attribute
                    datatype = common.ConvertHexToDecimal(captureObjectScalerUnit.Substring(index, 2)); index += 2;
                    structureInfoEntity = structureInfoBLL.GetDetailData(datatype) as StructureInfoEntity;
                    if (structureInfoEntity != null)
                    {
                        totByte = structureInfoEntity.ValueInByte * 2;
                        AccuracyScaleAttribute[counter] = common.ConvertHexToDecimal(captureObjectScalerUnit.Substring(index, totByte));
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
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// This method is used to parse parameters data.
        /// </summary>
        /// <param name="captureData"></param>
        /// <param name="captureObject"></param>
        private void GetCaptureData(string captureData,string captureObject)
        {
            StructureInfoEntity structureInfoEntity = null;
            int counter = 0, array,arrayLength;
            int index = 0;
            try
            {
                array = common.ConvertHexToDecimal(captureObject.Substring(index, 2)); index += 2;
                arrayLength = common.ConvertHexToDecimal(captureObject.Substring(index, 2)); index += 6;
                AccuracyOBISCodeDataValue = new string[arrayLength];
                while (counter < arrayLength)
                {
                    datatype = common.ConvertHexToDecimal(captureData.Substring(index, 2)); index += 2;
                    dataLength = 0;
                    totByte = 0;
                    if (datatype == 9 || datatype == 10)
                    {
                        dataLength = common.ConvertHexToDecimal(captureData.Substring(index, 2)); index += 2;
                        AccuracyOBISCodeDataValue[counter] = captureData.Substring(index, (dataLength * 2));
                        index += (dataLength * 2);
                    }
                    else
                    {
                        structureInfoEntity = structureInfoBLL.GetDetailData(datatype) as StructureInfoEntity;
                        if (structureInfoEntity != null)
                        {
                            totByte = structureInfoEntity.ValueInByte * 2;
                            AccuracyOBISCodeDataValue[counter] = common.ConvertHexToDecimal(captureData.Substring(index, totByte)).ToString();
                            index += totByte;
                        }
                    }
                    counter++;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        /// <summary>
        /// This method is used to parse capture objects .
        /// </summary>
        /// <param name="captureObject"></param>
        private void GetCaptureObjects(string captureObject)
        {
            StructureInfoEntity structureInfoEntity = null;
            int counter = 0,array,arrayLength;
            int index = 0;
            try
            {
                array = common.ConvertHexToDecimal(captureObject.Substring(index, 2)); index += 2;
                arrayLength = common.ConvertHexToDecimal(captureObject.Substring(index, 2)); index += 2;
                AccuracyOBISCode = new string[arrayLength];
                AccuracyDataIndex = new int[arrayLength];
                AccuracyClassID = new int[arrayLength];
                AccuracyAttribute = new int[arrayLength];
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
                        AccuracyClassID[counter] = common.ConvertHexToDecimal(captureObject.Substring(index, totByte));
                        index += totByte;
                    }
                    //OBIS Code
                    datatype = common.ConvertHexToDecimal(captureObject.Substring(index, 2)); index += 2;
                    dataLength = common.ConvertHexToDecimal(captureObject.Substring(index, 2)); index += 2;
                    AccuracyOBISCode[counter] = common.GetOBISCode(captureObject.Substring(index, (dataLength * 2))); index += (dataLength * 2);
                    //Attribute
                    datatype = common.ConvertHexToDecimal(captureObject.Substring(index, 2)); index += 2;
                    structureInfoEntity = structureInfoBLL.GetDetailData(datatype) as StructureInfoEntity;
                    if (structureInfoEntity != null)
                    {
                        totByte = structureInfoEntity.ValueInByte * 2;
                        AccuracyAttribute[counter] = common.ConvertHexToDecimal(captureObject.Substring(index, totByte));
                        index += totByte;
                    }
                    //DataIndex
                    datatype = common.ConvertHexToDecimal(captureObject.Substring(index, 2)); index += 2;
                    structureInfoEntity = structureInfoBLL.GetDetailData(datatype) as StructureInfoEntity;
                    if (structureInfoEntity != null)
                    {
                        totByte = structureInfoEntity.ValueInByte * 2;
                        AccuracyDataIndex[counter] = common.ConvertHexToDecimal(captureObject.Substring(index, totByte));
                        index += totByte;
                    }
                    counter++;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
