/*
File Name: ParseFDLData.cs
Created By: Vivek Agrawal
Date : 27/Feb/2012
Purpose:Parsing Implementation of Fast Downloaded Data
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using CAB.Framework.Entity;
using CAB.Entity;
using CAB.Framework.Utility;
using CAB.BLL;
using LTCTDAL;
using System.Data;
using System.Collections.ObjectModel;
using CAB.DALC.Data;
using CABEntity;
using CAB.Framework;
namespace CHANNEL.Formatter
{
    #region ParseFDLData CLASS
    /// <summary>
    /// Created By : Vivek Agrawal
    /// Date : 27/Feb/2012
    /// Purpose : This class defines a template for Parsing 
    /// of packet data.
    /// </summary>
    public abstract class ParseFDLData : IPacketParseable
    {
        #region Declaration Section
        StringBuilder stringPatternPacket = new StringBuilder();
        protected int packetLength = 0;
        protected long meterDataID;
        protected string fileText = string.Empty;
        protected long fileUploadID;
        protected Regex packetRegex;
        protected Match regExpPacketMatches;
        protected int[] includedEntitiesInPacket;
        protected int packetByteIndex = 0;
        protected const string UNIT_CURRENT = "*A";
        protected const string UNIT_VOLTAGE = "*V";
        protected static string UNIT_ACTIVEENERGY = "*kWh";
        protected static string UNIT_REACTIVEENERGY = "*kVArh";
        protected static string UNIT_APPARENTENERGY = "*kVAh";
        protected static string UNIT_ACTIVEENERGYINMEGA = "*MWh";
        protected static string UNIT_REACTIVEENERGYINMEGA = "*MVArh";
        protected static string UNIT_APPARENTENERGYINMEGA = "*MVAh";
        protected const string UNIT_FREQUENCY = "*Hz";
        protected const int PRECISION_CURRENT = 3;
        protected const int PRECISION_VOLTAGE = 2;
        protected const int PRECISION_ENERGY = 6;
        protected const int PRECISION_PF = 4;
        protected const int PRECISION_PHASORPF = 4;
        protected const int PRECISION_POWER = 5;
        protected const int PRECISION_FREQUENCY = 2;
        protected const int PRECISION_PHASORFREQUENCY = 3;
        protected static string UNIT_DEMANDKVA = "*kVA";
        protected static string UNIT_DEMANDKW = "*kW";
        protected static string UNIT_DEMANDKVAR = "*kvar";
        protected static string UNIT_DEMANDMVA = "*MVA";
        protected static string UNIT_DEMANDMW = "*MW";
        protected static string UNIT_DEMANDMVAR = "*Mvar";
        protected int packetsParsed = 0;
        public delegate void OnFDLParsingStatusChanged(string statusMessage);
        public event OnFDLParsingStatusChanged OnfdlParsingStatusChanged;
        protected System.Resources.ResourceManager rmFDLParse;
        protected string demandResolution = string.Empty;
        protected string energyResolution = string.Empty;
        protected int fdlDemandResolution = 0;
        protected int fdlEnergyResolution = 0;
        protected static bool isHTCT = false;
        protected static int internalCTRatio = 0;
        protected static int internalPTRatio = 0;
        #endregion

        #region ParseFDLData Constructor
        /// <summary>
        /// Constructor  ParseFDLData
        /// </summary>
        /// <param name="fileText"></param>
        /// <param name="fileUploadID"></param>
        public ParseFDLData(string fileText, long fileUploadID, long meterDataID)
        {
            this.fileText = fileText;
            this.fileUploadID = fileUploadID;
            this.meterDataID = meterDataID;
            rmFDLParse = new System.Resources.ResourceManager("CHANNEL.Formatter.ParseFDLResource", System.Reflection.Assembly.GetExecutingAssembly());
        }
        #endregion

        #region DefineParsingforPacketStructure
        /// <summary>
        /// Created By : Vivek Agrawal
        /// Date : 27/Feb/2012
        /// Purpose : This function Set parsing method
        /// of a packet.
        /// </summary>
        /// <param name="packetStructure"></param>
        /// <param name="tagData"></param>
        protected void DefineParsingforPacketStructure(DataTable packetStructure)
        {//Set parsing structure of the packet.
            SetPacketRegularExpressionAndSize(packetStructure);
            //Set the regular expression with packet structure.
            packetRegex = new Regex(stringPatternPacket.ToString());
        }
        #endregion

        #region Set Packet Entities to Parse
        /// <summary>
        /// Created By : Vivek Agrawal
        /// Date : 29/Feb/2012
        /// Purpose :  Set Packet Entries with size.
        /// </summary>
        /// <param name="dsPacketStructure"></param>
        protected virtual void SetPacketEntitesToParse(DataSet dsPacketStructure)
        {
            includedEntitiesInPacket = new int[dsPacketStructure.Tables["Parameter"].Rows.Count];
            for (int i = 0; i < dsPacketStructure.Tables["Parameter"].Rows.Count; i++)
                includedEntitiesInPacket[i] = Convert.ToInt32(dsPacketStructure.Tables["Parameter"].Rows[i][1]);
        }
        #endregion

        #region SetPacketRegularExpressionAndSize
        /// <summary>
        /// Created By : Vivek Agrawal
        /// Date : 27/Feb/2012
        /// Purpose : Creates regular expression for  the 
        /// packet and also calculates the size for the packet.
        /// </summary>
        /// <param name="packetStructure"></param>
        private void SetPacketRegularExpressionAndSize(DataTable packetStructure)
        {
            int j, columnSize = 0;
            for (int i = 0; i < packetStructure.Rows.Count; i++)
            {//if any entry is 0 then it means that entry is absent in each packet.
                if (includedEntitiesInPacket[i] == 0)
                    continue;
                columnSize = Convert.ToInt32(packetStructure.Rows[i][1].ToString().Trim());
                for (j = 0; j < columnSize; j++)
                    stringPatternPacket = stringPatternPacket.Append("(.{2})");
                packetLength += columnSize;
            }
        }
        #endregion

        #region GetPacketParsingStructure
        /// <summary>
        /// Created By : Vivek Agrawal
        /// Date : 27/Feb/2012
        /// Purpose : Get the packet structure(parameters and size)
        /// from the xml file.
        /// </summary>
        /// <param name="xmlFileName"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public object GetPacketParsingStructure(string xmlFileName, Type type)
        {
            FastDownLoadingDAL fastDownLoadingDAL = new FastDownLoadingDAL();
            return fastDownLoadingDAL.LoadXMLFileInDataStructure(type, xmlFileName);
        }
        #endregion

        protected void SetParsingStatus(string status)
        {
            OnfdlParsingStatusChanged(status);
        }
        //protected void GetDemandResolution()
        //{
        //    switch (demandResolution)
        //    {

        //        case "1":
        //            demandResolution = "0.0";
        //            break;
        //        case "2":
        //            demandResolution = "0.00";
        //            break;
        //        case "3":
        //            demandResolution = "0.000";
        //            break;
        //        case "5":
        //            demandResolution = "0.0";
        //            break;
        //        case "6":
        //            demandResolution = "0.00";
        //            break;    
        //        case "7":
        //            demandResolution = "0.000";
        //            break;
        //        default:
        //            demandResolution = "0";
        //            break;
        //    }

        //}
        //GKG 18/03/2013 Please do not usae this function.As it updates 
        // global variable to support patch work
        protected void GetEnergyResolution()
        {
            switch (energyResolution)
            {
                case "0":
                    energyResolution = "0";
                    break;
                case "1":
                    energyResolution = "0.0";
                    break;
                case "2":
                    energyResolution = "0.00";
                    break;
                case "3":
                    energyResolution = "0.000";
                    break;
                case "4":
                    energyResolution = "0";
                    break;
                case "5":
                    energyResolution = "0.0";
                    break;
                case "6":
                    energyResolution = "0.00";
                    break;               
                default:
                    energyResolution = "0";
                    break;
               
            }
            

        }
        protected void GetFDLEnergyResolution()
        {
            switch (energyResolution)
            {
                case "0":
                    fdlEnergyResolution = 0;
                    break;
                case "1":
                    fdlEnergyResolution = 1;
                    break;
                case "2":
                    fdlEnergyResolution = 2;
                    break;
                case "3":
                    fdlEnergyResolution = 3;
                    break;
                case "4":
                    fdlEnergyResolution = 0;
                    break;
                case "5":
                    fdlEnergyResolution = 1;
                    break;
                case "6":
                    fdlEnergyResolution = 2;
                    break;
                default:
                    fdlEnergyResolution = 0;
                    break;

            }


        }
        protected void GetFDLDemandResolution()
        {
            switch (demandResolution)
            {

                case "1":
                    fdlDemandResolution = 1;
                    break;
                case "2":
                    fdlDemandResolution = 2;
                    break;
                case "3":
                    fdlDemandResolution = 3;
                    break;
                case "5":
                    fdlDemandResolution = 1;
                    break;
                case "6":
                    fdlDemandResolution = 2;
                    break;
                case "7":
                    fdlDemandResolution = 3;
                    break;
                default:
                    fdlDemandResolution = 0; 
                    break;
            }

        }
        protected string ApplyEnergyResolution(string value)
        {
            string strValue = value;
            string valueAfterDecimal = string.Empty;
            string valueBeforeDecimal = string.Empty;
            value = Decimal.Parse(value, System.Globalization.NumberStyles.Float).ToString();       
            try
            {
                if (strValue.Contains("."))
                {
                    valueAfterDecimal = strValue.Substring(strValue.IndexOf('.') + 1);
                    valueBeforeDecimal = strValue.Substring(0, strValue.IndexOf('.'));
                    if (fdlEnergyResolution > 0)
                    {
                        if (fdlEnergyResolution <= valueAfterDecimal.Length)
                        {
                            valueAfterDecimal = valueAfterDecimal.Substring(0, fdlEnergyResolution);
                        }
                        else
                        {
                            valueAfterDecimal = valueAfterDecimal + GetZeroesToAppend(fdlEnergyResolution - valueAfterDecimal.Length);
                        }
                        strValue = valueBeforeDecimal + "." + valueAfterDecimal;
                    }
                    else
                    {
                        strValue = valueBeforeDecimal;
                    }
                }
            }
            catch (Exception ex)
            {
                return strValue;
            }
            return strValue;
        }

        protected string ApplyEnergyResolutionForLS(string value)
        {
            int fdlLSEnergyResolution = 3;
            string strValue = value;
            string valueAfterDecimal = string.Empty;
            string valueBeforeDecimal = string.Empty;
            value = Decimal.Parse(value, System.Globalization.NumberStyles.Float).ToString();
            try
            {
                if (strValue.Contains("."))
                {
                    valueAfterDecimal = strValue.Substring(strValue.IndexOf('.') + 1);
                    valueBeforeDecimal = strValue.Substring(0, strValue.IndexOf('.'));
                    if (fdlLSEnergyResolution > 0)
                    {
                        if (fdlLSEnergyResolution <= valueAfterDecimal.Length)
                        {
                            valueAfterDecimal = valueAfterDecimal.Substring(0, fdlLSEnergyResolution);
                        }
                        else
                        {
                            valueAfterDecimal = valueAfterDecimal + GetZeroesToAppend(fdlLSEnergyResolution - valueAfterDecimal.Length);
                        }
                        strValue = valueBeforeDecimal + "." + valueAfterDecimal;
                    }
                    else
                    {
                        strValue = valueBeforeDecimal;
                    }
                }
            }
            catch (Exception ex)
            {
                return strValue;
            }
            return strValue;
        }
        protected string ApplyDemandResolution(string value)
        {
            string strValue = value;
            string valueAfterDecimal = string.Empty;
            string valueBeforeDecimal = string.Empty;
            try
            {
                if (strValue.Contains("."))
                {
                    valueAfterDecimal = strValue.Substring(strValue.IndexOf('.') + 1);
                    valueBeforeDecimal = strValue.Substring(0, strValue.IndexOf('.'));
                    if (fdlDemandResolution > 0)
                    {
                        if (fdlDemandResolution <= valueAfterDecimal.Length)
                        {
                            valueAfterDecimal = valueAfterDecimal.Substring(0, fdlDemandResolution);
                        }
                        else
                        {
                            valueAfterDecimal = valueAfterDecimal + GetZeroesToAppend(fdlDemandResolution - valueAfterDecimal.Length);
                        }
                        strValue = valueBeforeDecimal + "." + valueAfterDecimal;
                    }
                    else
                    {
                        strValue = valueBeforeDecimal;
                    }
                }
            }
            catch (Exception ex)
            {
                return strValue;
            }

            return strValue;
        }
        private string GetZeroesToAppend(int noOfZeroes)
        {
            string zeroes = string.Empty;
            for (int counter = 0; counter < noOfZeroes; counter++)
            {
                zeroes = zeroes + "0";
            }
            return zeroes;
        }
        protected decimal ApplyDemandResolution(decimal value)
        {

            return value;
        }
        #region CreatePacketEntity
        /// <summary>
        /// Created By : Vivek Agrawal
        /// Date : 27/Feb/2012
        /// </summary>
        /// <returns></returns>
        protected virtual EntityBase CreatePacketEntity()
        {
            return new EntityBase();
        }
        #endregion

        #region Parse
        /// <summary>
        /// Created By : Vivek Agrawal
        /// Date : 27/Feb/2012
        /// </summary>
        public virtual FDLFileParseStatuses Parse()
        {
            return FDLFileParseStatuses.None;
        }
        #endregion
        #region Parse
        /// <summary>
        /// Created By : Gaurav Bhardwaj
        /// Parameter : EntityBase
        /// Date : 16 Aug 2012
        /// </summary>
        /// <param name="entityBase"></param>
        /// <returns></returns>
        public virtual FDLFileParseStatuses Parse(out DLMS650NamePlateDetailsEntity entityBase)
        {
            entityBase = null;
            return FDLFileParseStatuses.None;
        }

        #endregion
        #region FormatParsedValue
        /// <summary>
        /// Created By : Vivek Agrawal
        /// Date : 01/march/2012
        /// Purpose : Parse the hex value & apply precision
        /// </summary>
        /// <param name="parsedvalue"></param>
        /// <param name="decimalplaces"></param>
        /// <returns></returns>
        protected decimal FormatParsedValue(string parsedvalue, double decimalplaces,DLMSObjectType dlmsObjectType)
        {
            decimal parsedResult = 0;
            if (!string.IsNullOrEmpty(parsedvalue))
            {
                parsedResult = Convert.ToDecimal(Convert.ToInt64(parsedvalue, 16));
                if (DLMSObjectType.Demand == dlmsObjectType)
                {
                    parsedResult = parsedResult * internalCTRatio * internalPTRatio;
                    if (isHTCT)
                    {
                        decimalplaces = decimalplaces + 3;
                    }
                }
                else if (DLMSObjectType.Power == dlmsObjectType)
                {
                    parsedResult = parsedResult * internalPTRatio * internalCTRatio;
                    if (isHTCT)
                    {
                        decimalplaces = decimalplaces + 3;
                    }
                }
                else if (DLMSObjectType.Energy == dlmsObjectType)
                {
                    parsedResult = parsedResult * internalCTRatio * internalPTRatio;
                    if (isHTCT)
                    {
                        decimalplaces = decimalplaces + 3;
                    }
                }
                else if (dlmsObjectType == DLMSObjectType.Current)
                {
                    parsedResult = parsedResult * internalCTRatio;
                }
                else if (dlmsObjectType == DLMSObjectType.Voltage)
                {
                    parsedResult = parsedResult * internalPTRatio;
                }
                else
                {
                    return parsedResult;
                }
                parsedResult = (parsedResult / (int)(Math.Pow(10.0, decimalplaces)));

            }
            return parsedResult;
        }

        protected decimal FormatParsedValue(string parsedvalue, double decimalplaces)
        {
            decimal parsedResult = 0;
            if (!string.IsNullOrEmpty(parsedvalue))
                parsedResult = (Convert.ToDecimal((Convert.ToInt32(parsedvalue, 16))) / (int)(Math.Pow(10.0, decimalplaces)));
            return parsedResult;
        }
        protected decimal FormatParsedSignedValue(string parsedvalue, double decimalplaces,DLMSObjectType dlmsObjectType)
        {
            decimal parsedResult = 0;
            if (!string.IsNullOrEmpty(parsedvalue))
            {
                if (parsedvalue.Length == 4)
                {
                    parsedResult = Convert.ToDecimal(Convert.ToInt16(parsedvalue, 16));
                }
                else
                {
                    parsedResult = Convert.ToDecimal(Convert.ToInt32(parsedvalue, 16));
                }
                if (DLMSObjectType.Demand == dlmsObjectType)
                {
                    parsedResult = parsedResult * internalCTRatio * internalPTRatio;
                    if (isHTCT)
                    {
                        decimalplaces = decimalplaces + 3;
                    }
                }
                else if (DLMSObjectType.Power == dlmsObjectType)
                {
                    parsedResult = parsedResult * internalPTRatio * internalCTRatio;
                    if (isHTCT)
                    {
                        decimalplaces = decimalplaces + 3;
                    }
                }
                else if (DLMSObjectType.Energy == dlmsObjectType)
                {
                    parsedResult = parsedResult * internalCTRatio * internalPTRatio;
                    if (isHTCT)
                    {
                        decimalplaces = decimalplaces + 3;
                    }
                }
                else if (dlmsObjectType == DLMSObjectType.Current)
                {
                    parsedResult = parsedResult * internalCTRatio;
                    if (isHTCT)
                    {
                        decimalplaces = decimalplaces + 3;
                    }
                }
                else if (dlmsObjectType == DLMSObjectType.Voltage)
                {
                    parsedResult = parsedResult * internalPTRatio;
                    if (isHTCT)
                    {
                        decimalplaces = decimalplaces + 3;
                    }
                }
                else
                {
                    return parsedResult;
                }            
                parsedResult = (parsedResult / (int)(Math.Pow(10.0, decimalplaces)));
            }
            return parsedResult;
        }
        protected decimal FormatParsedSignedValue(string parsedvalue, double decimalplaces)
        {
            decimal parsedResult = 0;
            if (!string.IsNullOrEmpty(parsedvalue))
            {               
                if(parsedvalue.Length==4)
                {               
                    parsedResult = Convert.ToDecimal(Convert.ToInt16(parsedvalue,16));
                }
                else
                {
                    parsedResult = Convert.ToDecimal(Convert.ToInt32(parsedvalue,16));
                }
            }
            parsedResult = (parsedResult / (int)(Math.Pow(10.0, decimalplaces)));
            return parsedResult;
        }
        protected decimal FormatParsedLongValue(string parsedvalue, double decimalplaces)
        {
            return (Convert.ToDecimal((Convert.ToInt64(parsedvalue, 16))) / (int)(Math.Pow(10.0, decimalplaces)));
        }
        protected decimal FormatParsedLongValue(string parsedvalue, double decimalplaces,DLMSObjectType dlmsObjectType)
        {
            decimal parsedResult = Convert.ToInt64(parsedvalue, 16);
            if (DLMSObjectType.Demand == dlmsObjectType)
            {
                parsedResult = parsedResult * internalCTRatio * internalPTRatio;
                if (isHTCT)
                {
                    decimalplaces = decimalplaces + 3;
                }
            }
            else if (DLMSObjectType.Power == dlmsObjectType)
            {
                parsedResult = parsedResult * internalPTRatio * internalCTRatio;
                if (isHTCT)
                {
                    decimalplaces = decimalplaces + 3;
                }
            }
            else if (DLMSObjectType.Energy == dlmsObjectType)
            {
                parsedResult = parsedResult * internalCTRatio * internalPTRatio;
                if (isHTCT)
                {
                    decimalplaces = decimalplaces + 3;
                }
            }
            else if (dlmsObjectType == DLMSObjectType.Current)
            {
                parsedResult = parsedResult * internalCTRatio;
            }
            else if (dlmsObjectType == DLMSObjectType.Voltage)
            {
                parsedResult = parsedResult * internalPTRatio;
            }        
            return (Convert.ToDecimal((parsedResult)) / (int)(Math.Pow(10.0, decimalplaces)));
        }
        #endregion

        #region ParseValue
        /// <summary>
        /// Created By : Vivek Agrawal
        /// Date : 01/march/2012
        /// Purpose : Parse the value from the string
        /// based on the size of parameter & parsing direction(sequence value)
        /// </summary>
        /// <param name="size"></param>
        /// <param name="sequence"></param>
        /// <returns></returns>
        protected string ParseValue(int size, string sequence)
        {
            string parsedValue = "";
            if (sequence == "0")
            {
                for (int i = packetByteIndex + size; i > packetByteIndex; i -= 1)
                    parsedValue += regExpPacketMatches.Groups[i].Value;

            }
            else
            {
                for (int i = packetByteIndex + 1; i <= packetByteIndex + size; i += 1)
                    parsedValue += regExpPacketMatches.Groups[i].Value;
            }
            packetByteIndex += size;
            return parsedValue;
        }
        #endregion

        protected long ParseDateTime()
        {
            long parsedValue;

            parsedValue = Convert.ToInt64((2000 + Convert.ToInt32(regExpPacketMatches.Groups[packetByteIndex + 5].Value)).ToString() +
                                                regExpPacketMatches.Groups[packetByteIndex + 4].Value + regExpPacketMatches.Groups[packetByteIndex + 3].Value +
                                                regExpPacketMatches.Groups[packetByteIndex + 2].Value + regExpPacketMatches.Groups[packetByteIndex + 1].Value + "99");



            packetByteIndex += 5;
            return parsedValue;
        }

        #region VerifyBCC
        /// <summary>
        /// Created By : Vivek Agrawal
        /// Date : 01/Marche/2012
        /// Purpose : Validation of BCC of  the readout data
        /// </summary>
        /// <param name="tagwiseData"></param>
        /// <returns></returns>
        protected bool VerifyBCC(string tagwiseData)
        {
            byte bccValue;
            bccValue = CalculateBcc(tagwiseData.Substring(0, tagwiseData.Length - 2));
            if (bccValue != Convert.ToByte(tagwiseData.Substring(tagwiseData.Length - 2, 2), 16))
                return false;
            return true;
        }
        #endregion

        #region CalculateBcc
        /// <summary>
        /// Created By : Vivek Agrawal
        /// Date : 01/March/2012
        /// Purpose : Calculate Bcc
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private byte CalculateBcc(string data)
        {
            byte bcc = 0;
            int i = 0;
            for (i = 0; i < data.Length; i += 2)
            {
                if (i == 0)
                {
                    bcc = Convert.ToByte(data.Substring(i, 2), 16);
                    continue;
                }//XOR Operation.
                bcc = (byte)(bcc ^ Convert.ToByte(data.Substring(i, 2), 16));
            }
            return bcc;
        }

        /// <summary>
        /// Sort the data based on passed pointer
        /// </summary>
        /// <param name="numberOfRecord"></param>
        /// <param name="pointerLoc"></param>
        /// <param name="dataSource"></param>
        /// <returns></returns>
        protected List<IEntity> ApplySorting(int numberOfRecord, int pointerLoc, List<IEntity> dataSource)
        {
            //If Pointer location is exceeding Number of Records. 
            if (numberOfRecord < pointerLoc)
            {
                pointerLoc = numberOfRecord;
            }
            
            List<IEntity> sortedData = new List<IEntity>();
            //If Pointer Location is at 0. i.e. No Billing Happen
            if (pointerLoc == 0 || (pointerLoc == numberOfRecord))
            {
                sortedData = dataSource;    
            }
            else
            {
                sortedData = dataSource.GetRange(pointerLoc, numberOfRecord - pointerLoc);
                sortedData.AddRange(dataSource.GetRange(0, pointerLoc));
            }

            return sortedData;
        }
        #endregion
    }
    #endregion


}
