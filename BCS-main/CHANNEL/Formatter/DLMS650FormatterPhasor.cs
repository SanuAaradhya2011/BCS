#region Namespaces
using System;
using System.Windows.Forms;

using CAB.BLL;
using CAB.Entity;
using CAB.Framework;
#endregion
namespace CAB.Channel.Formatter
{
    /// <summary>
    /// Class is responsible for parsing phasor data in DLMS format
    /// </summary>
    public class DLMS650FormatterPhasor : ReadBase
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        private DLMS650StructureInfoBLL structureInfoBLL;
        private DLMS650StructureUnitInfoBLL structureUnitInfoBLL;
        private DLMS650FormatterCommon common;
        private string[] PhasorOBISCode, PhasorOBISCodeDataValue, PhasorScaleOBISCode, PhasorScaleOBISCodeDataValue, PhasorScaleOBISCodeDataUnitValue;
        private int[] PhasorScaleClassID, PhasorScaleAttribute, PhasorClassID, PhasorAttribute, PhasorDataIndex;
        private int structure, structureLength, dataLength, datatype, totByte;
        private string utility = string.Empty;
        private const string Lag = "Lag";
        private const string Lead = "Lead";
        private const string Import = "Import";
        private const string Export = "Export";
        private const string Present = "Present";
        private const string Absent = "Absent";
        private const string Incorrect = "Incorrect";
        private const string Correct = "Correct";
        private bool isPUMA = false;
        private bool isMPKWCL = false;
        #endregion

        #region Properties
        #endregion

        #region Constructor
        public DLMS650FormatterPhasor()
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
        #endregion

        #region Public Methods
        /// <summary>
        /// Parse the phasor data and fills the master object
        /// </summary>
        /// <param name="data"></param>
        /// <param name="master"></param>
        public void LoadPhasorData(string[] data, BillingGeneralNFDLMSEntity master)
        {

            int counter;
            bool flag = false;
            for (counter = 0; counter < data.Length; counter++)
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
                this.StatusMessage = "Phasor data not found.";
                Application.DoEvents();
                return;
            }
            this.StatusMessage = "Uploading phasor data......";
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

            PhasorOBISCode = new string[arrayLength];
            PhasorOBISCodeDataValue = new string[arrayLength];
            PhasorClassID = new int[arrayLength];
            PhasorAttribute = new int[arrayLength];


            PhasorDataIndex = new int[arrayLength];
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
                    PhasorClassID[counter] = common.ConvertHexToDecimal(captureObject.Substring(index, totByte));
                    index += totByte;
                }
                //OBIS Code
                datatype = common.ConvertHexToDecimal(captureObject.Substring(index, 2)); index += 2;
                dataLength = common.ConvertHexToDecimal(captureObject.Substring(index, 2)); index += 2;
                PhasorOBISCode[counter] = common.GetOBISCode(captureObject.Substring(index, (dataLength * 2))); index += (dataLength * 2);
                //Attribute
                datatype = common.ConvertHexToDecimal(captureObject.Substring(index, 2)); index += 2;
                structureInfoEntity = structureInfoBLL.GetDetailData(datatype) as StructureInfoEntity;
                if (structureInfoEntity != null)
                {
                    totByte = structureInfoEntity.ValueInByte * 2;
                    PhasorAttribute[counter] = common.ConvertHexToDecimal(captureObject.Substring(index, totByte));
                    index += totByte;
                }
                //DataIndex
                datatype = common.ConvertHexToDecimal(captureObject.Substring(index, 2)); index += 2;
                structureInfoEntity = structureInfoBLL.GetDetailData(datatype) as StructureInfoEntity;
                if (structureInfoEntity != null)
                {
                    totByte = structureInfoEntity.ValueInByte * 2;
                    PhasorDataIndex[counter] = common.ConvertHexToDecimal(captureObject.Substring(index, totByte));
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
                    PhasorOBISCodeDataValue[counter] = captureObjectData.Substring(index, (dataLength * 2));
                    index += (dataLength * 2);
                }
                else
                {
                    structureInfoEntity = structureInfoBLL.GetDetailData(datatype) as StructureInfoEntity;
                    if (structureInfoEntity != null)
                    {
                        totByte = structureInfoEntity.ValueInByte * 2;
                        PhasorOBISCodeDataValue[counter] = common.ConvertHexToDecimal(captureObjectData.Substring(index, totByte)).ToString();
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


            PhasorScaleAttribute = new int[arrayLength];
            PhasorScaleClassID = new int[arrayLength];
            PhasorScaleOBISCode = new string[arrayLength];
            PhasorScaleOBISCodeDataUnitValue = new string[arrayLength];
            PhasorScaleOBISCodeDataValue = new string[arrayLength];


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
                    PhasorScaleClassID[counter] = common.ConvertHexToDecimal(captureObjectScalerUnit.Substring(index, totByte));
                    index += totByte;
                }
                //OBIS Code
                datatype = common.ConvertHexToDecimal(captureObjectScalerUnit.Substring(index, 2)); index += 2;
                dataLength = common.ConvertHexToDecimal(captureObjectScalerUnit.Substring(index, 2)); index += 2;
                PhasorScaleOBISCode[counter] = common.GetOBISCode(captureObjectScalerUnit.Substring(index, (dataLength * 2))); index += (dataLength * 2);
                //Attribute
                datatype = common.ConvertHexToDecimal(captureObjectScalerUnit.Substring(index, 2)); index += 2;
                structureInfoEntity = structureInfoBLL.GetDetailData(datatype) as StructureInfoEntity;
                if (structureInfoEntity != null)
                {
                    totByte = structureInfoEntity.ValueInByte * 2;
                    PhasorScaleAttribute[counter] = common.ConvertHexToDecimal(captureObjectScalerUnit.Substring(index, totByte));
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
                    PhasorScaleOBISCodeDataValue[counter] = captureObjectDataScalerUnit.Substring(index, (dataLength * 2));
                    index += (dataLength * 2);
                }
                else
                {
                    structureInfoEntity = structureInfoBLL.GetDetailData(datatype) as StructureInfoEntity;
                    if (structureInfoEntity != null)
                    {
                        totByte = structureInfoEntity.ValueInByte * 2;
                        PhasorScaleOBISCodeDataValue[counter] = common.ConvertHexToDecimal(captureObjectDataScalerUnit.Substring(index, totByte)).ToString();
                        index += totByte;
                    }
                }
                datatype = common.ConvertHexToDecimal(captureObjectDataScalerUnit.Substring(index, 2)); index += 2;
                if (datatype == 9 || datatype == 10)
                {
                    dataLength = common.ConvertHexToDecimal(captureObjectDataScalerUnit.Substring(index, 2)); index += 2;
                    PhasorScaleOBISCodeDataValue[counter] = captureObjectDataScalerUnit.Substring(index, (dataLength * 2));
                    index += (dataLength * 2);
                }
                else
                {
                    structureInfoEntity = structureInfoBLL.GetDetailData(datatype) as StructureInfoEntity;
                    if (structureInfoEntity != null)
                    {
                        totByte = structureInfoEntity.ValueInByte * 2;
                        PhasorScaleOBISCodeDataUnitValue[counter] = common.ConvertHexToDecimal(captureObjectDataScalerUnit.Substring(index, totByte)).ToString();
                        index += totByte;
                    }
                }
                index += 4;
                counter++;
            }
            #endregion

            //added PUMA


            ApplyScaleToPhasor(master);
            master.Phasor = GetPhasorEntity();
        }
        #endregion

        #region Protected Methods
        #endregion

        #region Event Handlers
        #endregion

        #region Private Methods
        /// <summary>
        /// Apply scalar ro phasor data
        /// </summary>
        /// <param name="master"></param>
        private void ApplyScaleToPhasor(BillingGeneralNFDLMSEntity master)
        {
            int counter;
            StructureUnitEntity structureUnitEntity;
            int scaleVale = 0;
            //DLMSObjectType dlmsObjectType;
            //bool isHTCT = false;
            try
            {
                for (counter = 0; counter < PhasorOBISCodeDataValue.Length; counter++)
                {
                    //added PUMA
                    if (CheckAndConvertDate(counter))
                        continue;
                    //dlmsObjectType = common.GetDLMSObjectType(GetColumnName(PhasorOBISCode[counter], PhasorClassID[counter], PhasorAttribute[counter]));
                    int scaleId = GetScaleIndex(PhasorOBISCode[counter], PhasorClassID[counter], PhasorAttribute[counter]);
                    if (scaleId > -1)
                    {
                        structureUnitEntity = structureUnitInfoBLL.GetDetailData(Int32.Parse(PhasorScaleOBISCodeDataUnitValue[scaleId])) as StructureUnitEntity;
                        scaleVale = Int32.Parse(PhasorScaleOBISCodeDataValue[scaleId]);
                    }
                    //BharwdajG : else find a default value
                    else
                    {
                        // Give 255 as argument for no resolution.
                        structureUnitEntity = structureUnitInfoBLL.GetDetailData(255) as StructureUnitEntity;
                        // Added to resolve bug 73370
                        scaleVale = 0;
                    }

                    string dataValue = PhasorOBISCodeDataValue[counter];
                    if (scaleVale > 128)
                    {
                        int ScaleDiff = 256 - scaleVale;
                        //ScaleDiff = ScaleDiff * -1;

                        dataValue = common.GetSignValue(Int32.Parse(dataValue), ScaleDiff, "-", structureUnitEntity.StructureUnit);

                    }
                    else
                    {

                        dataValue = common.GetSignValue(Int32.Parse(dataValue), scaleVale, "+", structureUnitEntity.StructureUnit);

                    }

                    //dataValue = dataValue + "*" + common.GetUnit(structureUnitEntity.StructureUnit);

                    PhasorOBISCodeDataValue[counter] = dataValue;

                    //Assign Meter Type to MeterDataType property 
                    //master.MeterDataType = (isHTCT) ? MeterDataTypes.HTCT : MeterDataTypes.LTCT;
                }
            }
            catch (Exception ex)
            {
            }
        }
        /// <summary>
        /// Gets the scale index of scalar
        /// </summary>
        /// <param name="obisCode"></param>
        /// <param name="classId"></param>
        /// <param name="attributeId"></param>
        /// <returns></returns>
        private int GetScaleIndex(string obisCode, int classId, int attributeId)
        {
            for (int counter = 0; counter < PhasorScaleOBISCode.Length; counter++)
            {

                if (!string.IsNullOrEmpty(PhasorScaleOBISCode[counter]))
                {
                    if (PhasorScaleOBISCode[counter].Trim().Equals(obisCode) && PhasorScaleClassID[counter] == classId)
                        return counter;
                }

            }
            return -1;
        }
        /// <summary>
        /// Checks if it is date and then converts date
        /// </summary>
        /// <param name="counter"></param>
        /// <returns></returns>
        private bool CheckAndConvertDate(int counter)
        {
            //added PUMA
            //if (counter == 0 || counter == 24 || counter == 26 || counter == 28)
            if (counter == 0)
            {
                PhasorOBISCodeDataValue[counter] = common.GetDateTimeString(PhasorOBISCodeDataValue[counter]);
                return true;
            }
            return false;
        }
        /// <summary>
        /// Gets the phasor entity 
        /// </summary>
        /// <returns></returns>
        private PhasorEntity GetPhasorEntity()
        {
            PhasorEntity phasorEntity = new PhasorEntity();
            for (int counter = 0; counter < PhasorOBISCodeDataValue.Length; counter++)
            {
                switch (counter)
                { 
                    case 0:
                        phasorEntity.CurrentDateTime = Convert.ToInt64(PhasorOBISCodeDataValue[counter]);
                        break;
                    case 1:
                        phasorEntity.RPhaseCurrent = PhasorOBISCodeDataValue[counter];
                        break;
                    case 2:
                        phasorEntity.YPhaseCurrent = PhasorOBISCodeDataValue[counter];
                        break;
                    case 3:
                        phasorEntity.BPhaseCurrent = PhasorOBISCodeDataValue[counter];
                        break;
                    case 4:
                        phasorEntity.RPhaseVoltage = PhasorOBISCodeDataValue[counter];
                        break;
                    case 5:
                        phasorEntity.YPhaseVoltage = PhasorOBISCodeDataValue[counter];
                        break;
                    case 6:
                        phasorEntity.BPhaseVoltage = PhasorOBISCodeDataValue[counter];
                        break;
                    case 7:
                        phasorEntity.RPhasePowerFactor = PhasorOBISCodeDataValue[counter];
                        break;
                    case 8:
                        phasorEntity.YPhasePowerFactor = PhasorOBISCodeDataValue[counter];
                        break;
                    case 9:
                        phasorEntity.BPhasePowerFactor = PhasorOBISCodeDataValue[counter];
                        break;
                    case 10:
                        phasorEntity.TotalPhasePowerFactor = PhasorOBISCodeDataValue[counter];
                        break;
                    case 11:
                        phasorEntity.Frequency = PhasorOBISCodeDataValue[counter];
                        break;
                    case 12:
                        phasorEntity.ApparentPower = PhasorOBISCodeDataValue[counter];
                        break;
                    case 13:
                        phasorEntity.ActivePower = PhasorOBISCodeDataValue[counter];
                        break;
                    case 14:
                        phasorEntity.ReActivePower = PhasorOBISCodeDataValue[counter];
                        break;
                    case 15:
                        phasorEntity.RPhaseNegativePowerFlag = PhasorOBISCodeDataValue[counter].Contains("-") ? Export : Import; 
                        break;
                    case 16:
                        phasorEntity.YPhaseNegativePowerFlag = PhasorOBISCodeDataValue[counter].Contains("-") ? Export : Import;
                        break;
                    case 17:
                        phasorEntity.BPhaseNegativePowerFlag = PhasorOBISCodeDataValue[counter].Contains("-") ? Export : Import;
                        break;
                    case 18:
                        phasorEntity.AngleYR = PhasorOBISCodeDataValue[counter];
                        break;
                    case 19:
                        phasorEntity.AngleBR = PhasorOBISCodeDataValue[counter];
                        break;
                    case 20:
                        phasorEntity.AngleBetweenTwo = PhasorOBISCodeDataValue[counter];
                        break;
                    case 21:
                        phasorEntity.PhaseSequence = PhasorOBISCodeDataValue[counter] == "132" ? Incorrect : Correct;
                        break;
                }
                

            }
            phasorEntity.RPhaseCapacitiveInductiveFlag = phasorEntity.RPhasePowerFactor.Contains("-") ? Lead : Lag;
            phasorEntity.YPhaseCapacitiveInductiveFlag = phasorEntity.YPhasePowerFactor.Contains("-") ? Lead : Lag;
            phasorEntity.BPhaseCapacitiveInductiveFlag = phasorEntity.BPhasePowerFactor.Contains("-") ? Lead : Lag;
            return phasorEntity;
        }
       
        #endregion

    }
}
