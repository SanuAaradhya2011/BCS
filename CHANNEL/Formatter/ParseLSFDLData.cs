using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CHANNEL.Formatter;
using System.Data;
using CAB.Framework.Entity;
using CAB.Entity;
using CAB.BLL;
using CABEntity;
using CAB.Entity;
using CAB.Framework;
using System.ComponentModel;
using System.IO;

namespace CHANNEL
{
    #region Parse Load Survey FDL Data
    /// <summary>
    /// Created By : Vivek Agrawal
    /// Date : 27/Feb/2012
    /// Purpose : This class is responsible for Parsing 
    /// of Tamper Data.
    /// </summary>
    public class ParseLSFDLData : ParseFDLData
    {
        LSPacketStructure lsPacket;
        string lsData = string.Empty;
        const string LSSECONDS = "99";
        LoadSurveyParameterEntity lsParameterEntity = new LoadSurveyParameterEntity();
        // Added load survey columns names.
        public enum lsColumns
        {
            [DescriptionAttribute("realTimeClockDateandTime,")]
            realTimeClockDateandTime,
            [DescriptionAttribute("rPhaseCurrent,")]
            rPhaseCurrent,
            [DescriptionAttribute("yPhaseCurrent,")]
            yPhaseCurrent,
            [DescriptionAttribute("bPhaseCurrent,")]
            bPhaseCurrent,
            [DescriptionAttribute("rPhaseVoltage,")]
            rPhaseVoltage,
            [DescriptionAttribute("yPhaseVoltage,")]
            yPhaseVoltage,
            [DescriptionAttribute("bPhaseVoltage,")]
            bPhaseVoltage,
            [DescriptionAttribute("blockEnergykWh,")]
            blockEnergykWh,
            [DescriptionAttribute("blockEnergykvarhlag,")]
            blockEnergykvarhlag,
            [DescriptionAttribute("blockEnergykvarhlead,")]
            blockEnergykvarhlead,
            [DescriptionAttribute("blockEnergykVAh,")]
            blockEnergykVAh,
            [DescriptionAttribute("frequency,")]
            Frequency,
            [DescriptionAttribute("tamperStatus,")]
            TamperStatus
        }
        public ParseLSFDLData(string lsData, string fileText, long fileUploadID, long meterDataID, string demandResolution, string energyResolution)
            : base(fileText, fileUploadID, meterDataID)
        {
            this.lsData = lsData;
            this.demandResolution = demandResolution;
            //For Load Survey Energy will be fixed to 3 decimal place.
            this.energyResolution = energyResolution;
        }

        #region CountLPConfigBytes
        /// <summary>
        /// Created By : Vivek Agrawal
        /// Date : 01/Mar/2012
        /// Purpose : Check Number of LP config bytes from xml packet structure
        /// </summary>
        /// <param name="dsPacketStructure"></param>
        /// <returns></returns>
        private int CountLPConfigBytes(DataSet dsPacketStructure)
        {
            int configParmetersCount=0;
            for (int i = 0; i < dsPacketStructure.Tables["Parameter"].Rows.Count; i++)
            {
                if (dsPacketStructure.Tables["Parameter"].Rows[i]["LPConfig"].ToString().Trim() == "1")
                    configParmetersCount++;
            }
            return Convert.ToInt32(Math.Ceiling(configParmetersCount / 8.0));
        }
        #endregion

        #region SetPacketEntitesToParse
        /// <summary>
        /// Created By : Vivek Agrawal
        /// Date : 01/Mar/2012
        /// Purpose : 
        /// 1. Set size for each parameter in Packet 
        /// 2. Set the parameters in packet not to parse.
        /// </summary>
        /// <param name="dsPacketStructure"></param>
        protected override void SetPacketEntitesToParse(DataSet dsPacketStructure)
        {   
            base.SetPacketEntitesToParse(dsPacketStructure);
            //string length of LP config bytes.
            byte lpConfigCurrentByte;
            int i,j = 0;
            int lpConfigBytes = CountLPConfigBytes(dsPacketStructure);
            j=lpConfigBytes*2;
            for (i=0; i <lpConfigBytes; i++)
            {//Get LP config Byte1 which is actually stored at end of LPConfig string.
                lpConfigCurrentByte = Convert.ToByte(FormatParsedValue(lsData.Substring(j - 2, 2), 0));
                for (int b = 0; b < 8; b++)
                {
                    if ((5 + 8 * i + b) == includedEntitiesInPacket.Length)
                        break;
                    if (!IsBitSet(lpConfigCurrentByte, b))// Set the parameters in packet not to parse.
                        includedEntitiesInPacket[5+8*i+b] = 0;
                }
                j -= 2;
            }
            lsData = lsData.Substring(lpConfigBytes * 2);
        }
        #endregion

        #region Check Bit position
        /// <summary>
        /// Created By : Vivek Agrawal
        /// Date : 01/Mar/2012
        /// </summary>
        /// <param name="b"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        bool IsBitSet(byte b, int pos) 
        { 
            return (b & (1 << pos)) != 0;
        }
        #endregion

        #region Parse Loadsurvey Data
        /// <summary>
        /// Created By : Vivek Agrawal
        /// Date : 27/Feb/2012
        /// Purpose :Parse the tamper data and stores it in DB.
        /// </summary>
        public override FDLFileParseStatuses Parse()
        {
            int currentDataPointer = 0;
            string addedBytes = string.Empty;
           //Define Parsing of LS data Packet .
            if (!VerifyBCC(lsData))
            {
                SetParsingStatus(rmFDLParse.GetString("BCCMismatchLS"));
                return FDLFileParseStatuses.BCCMismatchLS; 
            }
            addedBytes = lsData.Substring(lsData.Length - 12, 10);
            lsData = lsData.Remove(lsData.Length-12,10);
            currentDataPointer =  Convert.ToInt32(addedBytes.Substring(addedBytes.Length - 4, 4), 16);
            lsPacket = (LSPacketStructure)GetPacketParsingStructure("LSDataPacketStructure.xml", typeof(LSPacketStructure));
            SetPacketEntitesToParse(lsPacket);
            DefineParsingforPacketStructure(lsPacket.Tables["Parameter"]);
            List<IEntity> entities = new List<IEntity>();
            string currentPacket = "";
            EntityBase entity;
            int x = 0;
            packetsParsed = 0;
            DLMS650LoadSurveyBLL loadSurveyBLL = new DLMS650LoadSurveyBLL();
            // Added to lost data bits to make full data. Hack..
            if (((lsData.Length - 3) % (2 * packetLength)) != 0)
            {
                int zerosToAppend = (2 * packetLength)-((lsData.Length - 3) % (2 * packetLength));
                string zerosString="";
                for (int i = 0; i < zerosToAppend; i++)
                    zerosString += "0";
                lsData = lsData.Insert(lsData.Length - 2,zerosString);
            }
            LoadSurveyParameterBLL loadSurveyParameterBLL = new LoadSurveyParameterBLL();
            string lsColumnNames = string.Empty;
            GetFDLDemandResolution();
            GetFDLEnergyResolution();
            //test code
            //int counter = 1;
            // test code
            for (; x < lsData.Length - 3; x += (2 * packetLength))
            {
                //following line commented to resolve bug 76117; 25th April 2012
                //if ((x + packetLength * 2) > (lsData.Length - 3))break;
                currentPacket= lsData.Substring(x, 2 * packetLength);//Current Packet.
                //test code
               // FWrite(currentPacket, counter);
               // counter++;
                // test code
                regExpPacketMatches = packetRegex.Match(currentPacket);//Split the packet as per packet structure.
                if (regExpPacketMatches != null && regExpPacketMatches.Groups != null && regExpPacketMatches.Groups.Count > 1)
                {   //So create the Load survey entity object form the current packet.
                    entity = CreatePacketEntity(out lsColumnNames);
                    // Checking columnames
                    if (!string.IsNullOrEmpty(lsColumnNames))
                    {
                        lsParameterEntity.ColumnsNames = lsColumnNames.Remove(lsColumnNames.LastIndexOf(","));
                        lsParameterEntity.MeterDataId = meterDataID;                        
                    }
                    if (entity != null)
                    {
                        entities.Add(entity);
                        packetsParsed++;
                        // added to solve bug 74492
                        SetParsingStatus(rmFDLParse.GetString("LSPacketsStatus"));
                        //SetParsingStatus(rmFDLParse.GetString("LSPacketsStatus") + packetsParsed.ToString());
                    }
                    //if (entities.Count > 500)
                    //{
                    //    loadSurveyBLL.InsertData(entities);
                    //    entities = new List<IEntity>();
                    //}
                }
            }
            // Inserting load survey columns into database.
            if (lsParameterEntity != null)
            {
                loadSurveyParameterBLL.InsertData(lsParameterEntity);
            }
              //Code for Check sum
                //to be implemented.
                //

            lsData = null;
            List<IEntity> listBeforeCurrentPointer = new List<IEntity>();
            if (currentDataPointer > 0 && currentDataPointer < entities.Count)
            {
                listBeforeCurrentPointer.AddRange(entities.GetRange(currentDataPointer, entities.Count - currentDataPointer));
                listBeforeCurrentPointer.AddRange(entities.GetRange(0, currentDataPointer));
            }
            else
            {
                listBeforeCurrentPointer = entities;
            }
            if (listBeforeCurrentPointer.Count > 0)//Save the parsed tamper data in DB.
                loadSurveyBLL.InsertData(listBeforeCurrentPointer);
            return FDLFileParseStatuses.None; 
        }
        #endregion
        //test code
        //private void FWrite(String sb, int counter )
       // {
        //    File.AppendAllText("CESCISSUE.txt", "\r\n" + counter.ToString() + " : " +  sb.ToString());
       // }
        //test code
        #region Check Validity of LS Record
        /// <summary>
        /// Created By : Vivek Agrawal
        /// Date : 01/Mar/2012
        /// Purpose :
        /// </summary>
        /// <returns></returns>
        private bool IsValidLSRecord()
        {
            for (int i = 1; i <= 3; i++)
            {
                if ((Convert.ToInt32(regExpPacketMatches.Groups[i].Value)) <= 0)
                    return false;
            }
            return true;

        }
        #endregion

        #region CreatePacketEntity
        /// <summary>
        /// Created By : Vivek Agrawal
        /// Date : 27/Feb/2012
        /// Purpose : Creat Tamper Data Enitity object from tamper data packet.
        /// </summary>
        /// <returns></returns>
        protected EntityBase CreatePacketEntity(out string lsColumnNames)
        {
            
            DLMS650LoadSurveyEntity lsEntity = new DLMS650LoadSurveyEntity();
            packetByteIndex = 5;
            lsColumnNames = string.Empty;
            if (!IsValidLSRecord())
                return null;
          // Filling load survey columns.
            lsEntity.RealTimeClockDateandTime= Convert.ToInt64(
                                           (2000 + Convert.ToInt32(regExpPacketMatches.Groups[3].Value)).ToString() +
                                            regExpPacketMatches.Groups[2].Value + regExpPacketMatches.Groups[1].Value +
                                            regExpPacketMatches.Groups[4].Value + regExpPacketMatches.Groups[5].Value + LSSECONDS);
            lsColumnNames += EnumUtil.stringValueOf(lsColumns.realTimeClockDateandTime);
            if (includedEntitiesInPacket[5] != 0)
            {
                lsEntity.RPhaseCurrent = FormatParsedValue(ParseValue(includedEntitiesInPacket[5], lsPacket.Tables["Parameter"].Rows[5]["Sequence"].ToString()), PRECISION_CURRENT, DLMSObjectType.Current).ToString("0.000") + UNIT_CURRENT;
                lsColumnNames += EnumUtil.stringValueOf(lsColumns.rPhaseCurrent);
            }

            if (includedEntitiesInPacket[6] != 0)
            {
                lsEntity.YPhaseCurrent = FormatParsedValue(ParseValue(includedEntitiesInPacket[6], lsPacket.Tables["Parameter"].Rows[6]["Sequence"].ToString()), PRECISION_CURRENT, DLMSObjectType.Current).ToString("0.000") + UNIT_CURRENT;
                lsColumnNames += EnumUtil.stringValueOf(lsColumns.yPhaseCurrent);
            }
            if (includedEntitiesInPacket[7] != 0)
            {
                lsEntity.BPhaseCurrent = FormatParsedValue(ParseValue(includedEntitiesInPacket[7], lsPacket.Tables["Parameter"].Rows[7]["Sequence"].ToString()), PRECISION_CURRENT, DLMSObjectType.Current).ToString("0.000") + UNIT_CURRENT;
                lsColumnNames += EnumUtil.stringValueOf(lsColumns.bPhaseCurrent);
            }
            if (includedEntitiesInPacket[8] != 0)
            {
                lsEntity.RPhaseVoltage = FormatParsedValue(ParseValue(includedEntitiesInPacket[8], lsPacket.Tables["Parameter"].Rows[8]["Sequence"].ToString()), PRECISION_VOLTAGE, DLMSObjectType.Voltage).ToString("0.00") + UNIT_VOLTAGE;
                lsColumnNames += EnumUtil.stringValueOf(lsColumns.rPhaseVoltage);
            }
            if (includedEntitiesInPacket[9] != 0)
            {
                lsEntity.YPhaseVoltage = FormatParsedValue(ParseValue(includedEntitiesInPacket[9], lsPacket.Tables["Parameter"].Rows[9]["Sequence"].ToString()), PRECISION_VOLTAGE, DLMSObjectType.Voltage).ToString("0.00") + UNIT_VOLTAGE;
                lsColumnNames += EnumUtil.stringValueOf(lsColumns.yPhaseVoltage);
            }
            if (includedEntitiesInPacket[10] != 0)
            {
                lsEntity.BPhaseVoltage = FormatParsedValue(ParseValue(includedEntitiesInPacket[10], lsPacket.Tables["Parameter"].Rows[10]["Sequence"].ToString()), PRECISION_VOLTAGE, DLMSObjectType.Voltage).ToString("0.00") + UNIT_VOLTAGE;
                lsColumnNames += EnumUtil.stringValueOf(lsColumns.bPhaseVoltage);
            }
            // Added to display values to 3 decimal places in all energies for bug 74454.
            if (includedEntitiesInPacket[11] != 0)
            {
                // Added to round off the energy values to 3 decimal places. 
                lsEntity.BlockEnergykWh = ApplyEnergyResolutionForLS(FormatParsedValue(ParseValue(includedEntitiesInPacket[11], lsPacket.Tables["Parameter"].Rows[11]["Sequence"].ToString()), PRECISION_ENERGY, DLMSObjectType.Energy).ToString()) + UNIT_ACTIVEENERGY; ;
                lsColumnNames += EnumUtil.stringValueOf(lsColumns.blockEnergykWh);
            }
            if (includedEntitiesInPacket[12] != 0)
            {

                // Added to round off the energy values to 3 decimal places. 
                lsEntity.BlockEnergykvarhlag = ApplyEnergyResolutionForLS(FormatParsedValue(ParseValue(includedEntitiesInPacket[12], lsPacket.Tables["Parameter"].Rows[11]["Sequence"].ToString()), PRECISION_ENERGY, DLMSObjectType.Energy).ToString()) + UNIT_REACTIVEENERGY; ;
                lsColumnNames += EnumUtil.stringValueOf(lsColumns.blockEnergykvarhlag);
            }
            if (includedEntitiesInPacket[13] != 0)
            {

                // Added to round off the energy values to 3 decimal places. 
                lsEntity.BlockEnergykvarhlead = ApplyEnergyResolutionForLS(FormatParsedValue(ParseValue(includedEntitiesInPacket[13], lsPacket.Tables["Parameter"].Rows[11]["Sequence"].ToString()), PRECISION_ENERGY, DLMSObjectType.Energy).ToString()) + UNIT_REACTIVEENERGY;
                lsColumnNames += EnumUtil.stringValueOf(lsColumns.blockEnergykvarhlead);
            }
            if (includedEntitiesInPacket[14] != 0)
            {

                // Added to round off the energy values to 3 decimal places. 
                lsEntity.BlockEnergykVAh = ApplyEnergyResolutionForLS(FormatParsedValue(ParseValue(includedEntitiesInPacket[14], lsPacket.Tables["Parameter"].Rows[11]["Sequence"].ToString()), PRECISION_ENERGY, DLMSObjectType.Energy).ToString()) + UNIT_APPARENTENERGY;
                lsColumnNames += EnumUtil.stringValueOf(lsColumns.blockEnergykVAh);
            }
            if (includedEntitiesInPacket[15] != 0)
            {
                lsEntity.Frequency = FormatParsedValue(ParseValue(includedEntitiesInPacket[15], lsPacket.Tables["Parameter"].Rows[15]["Sequence"].ToString()), 2).ToString();
                lsColumnNames += EnumUtil.stringValueOf(lsColumns.Frequency);
            }
            if (includedEntitiesInPacket[16] != 0)
            {
                lsEntity.TamperStatus = FormatParsedValue(ParseValue(includedEntitiesInPacket[16], lsPacket.Tables["Parameter"].Rows[16]["Sequence"].ToString()), 0).ToString();
                lsColumnNames += EnumUtil.stringValueOf(lsColumns.TamperStatus);
            }
            //lsEntity.TamperStatus = null;
            lsEntity.MeterData_ID = meterDataID;
            return lsEntity;
        }
        #endregion
        
    }

    #endregion
}
