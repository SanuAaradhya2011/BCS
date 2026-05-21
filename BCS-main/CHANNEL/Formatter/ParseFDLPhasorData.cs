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
using CABEntity;
using CAB.Framework;
namespace CHANNEL.Formatter
{
    public class ParseFDLPhasorData : ParseFDLData
    {
         string phasorData = string.Empty;
       
        const string INVALIDVALUE = "00";
        const string INSTANTSECONDS = "99";
        const string NOVALUE = "----";
        private const string ABSENT = "Absent";
        private const string PRESENT = "Present";
        private const string LAG = "Lag";
        private const string LEAD = "Lead";
        private const string IMPORT = "Import";
        private const string EXPORT = "Export";
        private const string CORRECT = "Correct";
        private const string INCORRECT = "Incorrect";
        PhasorDataPacketStructure phasorPacket;
        public ParseFDLPhasorData(string phasordata, string fileText, long fileUploadID, long meterDataID)
            : base( fileText, fileUploadID, meterDataID)
        {
            this.phasorData = phasordata;
        }
        //
        public override FDLFileParseStatuses Parse()
        {
            
            // To verify the BCC of general data for data authentication.
            if (!VerifyBCC(phasorData))
            {
                SetParsingStatus(rmFDLParse.GetString("BCCMismatchPhasor"));
                return FDLFileParseStatuses.BCCMismatchGeneral;
            }
            try
            {
                //Extract the billing packet structure from xml.
                phasorPacket = (PhasorDataPacketStructure)GetPacketParsingStructure("PhasorDataPacketStructure.xml", typeof(PhasorDataPacketStructure));
                //Set Packet Entries with size.
                SetPacketEntitesToParse(phasorPacket);
                DefineParsingforPacketStructure(phasorPacket.Tables["Parameter"]);
                List<IEntity> entities = new List<IEntity>();
                string currentPacket = string.Empty;
                currentPacket = phasorData.Substring(0, 2 * packetLength);
                regExpPacketMatches = packetRegex.Match(currentPacket);
                if (regExpPacketMatches != null && regExpPacketMatches.Groups != null && regExpPacketMatches.Groups.Count > 1)
                {
                    entities = FillPhasorEntity();
                }
                new DLMS650PhasorBLL().InsertData(entities[0]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return FDLFileParseStatuses.None;
        }
        /// <summary>
        /// VBM - Used to get PhasorEntity for normal mode read.
        /// </summary>
        /// <param name="phasorData"></param>
        /// <returns></returns>
        public PhasorEntity FillPhasorEntity(string phasorData, int ctRatio, int ptRatio )
        {
            List<IEntity> entities = new List<IEntity>();
            try
            {
                //Extract the billing packet structure from xml.
                phasorPacket = (PhasorDataPacketStructure)GetPacketParsingStructure("PhasorDataPacketStructure.xml", typeof(PhasorDataPacketStructure));
                //Set Packet Entries with size.
                SetPacketEntitesToParse(phasorPacket);
                DefineParsingforPacketStructure(phasorPacket.Tables["Parameter"]);                
                string currentPacket = string.Empty;
                currentPacket = phasorData.Substring(0, 2 * packetLength);
                regExpPacketMatches = packetRegex.Match(currentPacket);
                if (regExpPacketMatches != null && regExpPacketMatches.Groups != null && regExpPacketMatches.Groups.Count > 1)
                {
                    internalCTRatio = ctRatio;
                    internalPTRatio = ptRatio;
                    entities = FillPhasorEntity();
                }
                //new DLMS650PhasorBLL().InsertData(entities[0]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return (PhasorEntity)entities[0];
        }
        //private List<IEntity> FillPhasorValues1(List<IEntity> entities)
        //{
        //    try
        //    {
        //        for (int i = 11; i < 21; i++)
        //        {
        //            PhasorEntity dlms650PhasorEntity = new PhasorEntity();
        //            dlms650PhasorEntity.MeterDataId = meterDataID;
        //            switch (i)
        //            {
        //                case 11:
        //                    dlms650PhasorEntity = GetTotalPowerFactor(dlms650PhasorEntity);
        //                    break;
        //                case 12:
        //                    dlms650PhasorEntity = GetFrequency(dlms650PhasorEntity);
        //                    break;
        //                case 13:
        //                    dlms650PhasorEntity = GetApparentPower(dlms650PhasorEntity);
        //                    break;
        //                case 14:
        //                    dlms650PhasorEntity = GetActivePower(dlms650PhasorEntity);
        //                    break;
        //                case 15:
        //                    dlms650PhasorEntity = GetReactivePower(dlms650PhasorEntity);
        //                    break;
        //                case 16:
        //                    dlms650PhasorEntity = GetNumberOfPowerFailures(dlms650PhasorEntity);
        //                    break;
        //                case 17:
        //                    dlms650PhasorEntity = GetCumulativePowerFailureDuration(dlms650PhasorEntity);
        //                    break;
        //                case 18:
        //                    dlms650PhasorEntity = GetCumulativeTamperCount(dlms650PhasorEntity);
        //                    break;
        //                case 19:
        //                    dlms650PhasorEntity = GetCumulativeBillingCount(dlms650PhasorEntity);
        //                    break;
        //                case 20:
        //                    dlms650PhasorEntity = GetCumulativeProgrammingCount(dlms650PhasorEntity);
        //                    break;
        //            }
        //            entities.Add(dlms650PhasorEntity);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return entities;
        //}
        //private List<IEntity> FillPhasorValues2(List<IEntity> entities)
        //{
        //    try
        //    {
        //        for (int i = 21; i < 30; i++)
        //        {
        //            PhasorEntity dlms650PhasorEntity = new PhasorEntity();
        //            dlms650PhasorEntity.MeterDataId = meterDataID;
        //            switch (i)
        //            {
        //                case 21:
        //                    dlms650PhasorEntity = GetBillingDate(dlms650PhasorEntity);
        //                    break;
        //                case 22:
        //                    dlms650PhasorEntity = GetCumkW(dlms650PhasorEntity);
        //                    break;
        //                case 23:
        //                    dlms650PhasorEntity = GetCumkvarhLag(dlms650PhasorEntity);
        //                    break;
        //                case 24:
        //                    dlms650PhasorEntity = GetCumkvarhLead(dlms650PhasorEntity);
        //                    break;
        //                case 25:
        //                    dlms650PhasorEntity = GetCumkVAh(dlms650PhasorEntity);
        //                    break;
        //                case 26:
        //                    dlms650PhasorEntity = GetMaximumDemandkW(dlms650PhasorEntity);
        //                    break;
        //                case 27:
        //                    dlms650PhasorEntity = GetMaximumDemandkWDateTime(dlms650PhasorEntity);
        //                    break;
        //                case 28:
        //                    dlms650PhasorEntity = GetMaximumDemandkVA(dlms650PhasorEntity);
        //                    break;
        //                case 29:
        //                    dlms650PhasorEntity = GetMaximumDemandkVADateTime(dlms650PhasorEntity);
        //                    break;

        //            }
        //            entities.Add(dlms650PhasorEntity);
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        throw ex;
        //    }
        //        return entities;

        //}

        private List<IEntity> FillPhasorEntity()
        {
            List<IEntity> entities = new List<IEntity>();
            PhasorEntity dlms650PhasorEntity = null;
            try
            {
                dlms650PhasorEntity = new PhasorEntity();
                for (int counter = 1; counter < 40; counter++)
                {
                    
                    dlms650PhasorEntity.MeterDataId = meterDataID;
                    switch (counter)
                    {
                        case 1:
                            dlms650PhasorEntity = GetRealTimeClockDateTime(dlms650PhasorEntity);
                            break;
                        case 2:
                            dlms650PhasorEntity = GetRPhCurrent(dlms650PhasorEntity);
                            break;
                        case 3:
                            dlms650PhasorEntity = GetYPhCurrent(dlms650PhasorEntity);
                            break;
                        case 4:
                            dlms650PhasorEntity = GetBPhCurrent(dlms650PhasorEntity);
                            break;
                        case 5:
                            dlms650PhasorEntity = GetRPhVoltage(dlms650PhasorEntity);
                            break;
                        case 6:
                            dlms650PhasorEntity = GetYPhVoltage(dlms650PhasorEntity);
                            break;
                        case 7:
                            dlms650PhasorEntity = GetBPhVoltage(dlms650PhasorEntity);
                            break;
                        case 8:
                            dlms650PhasorEntity = GetRPhPowerFactor(dlms650PhasorEntity);
                            break;
                        case 9:
                            dlms650PhasorEntity = GetYPhPowerFactor(dlms650PhasorEntity);
                            break;
                        case 10:
                            dlms650PhasorEntity = GetBPhPowerFactor(dlms650PhasorEntity);
                            break;
                        case 11:
                            dlms650PhasorEntity = GetTotalPowerFactor(dlms650PhasorEntity);
                            break;
                        case 12:
                            dlms650PhasorEntity = GetFrequency(dlms650PhasorEntity);      
                            break;
                        case 13:
                            dlms650PhasorEntity = GetApparentPower(dlms650PhasorEntity);
                            break;
                        case 14:
                            dlms650PhasorEntity = GetActivePower(dlms650PhasorEntity);
                            break;
                        case 15:
                            dlms650PhasorEntity = GetReactivePower(dlms650PhasorEntity);
                            break;
                        case 16:
                            dlms650PhasorEntity = GetNumberOfPowerFailures(dlms650PhasorEntity);
                            break;
                        case 17:
                            dlms650PhasorEntity =  GetCumulativePowerFailureDuration(dlms650PhasorEntity);
                            break;
                        case 18:
                            dlms650PhasorEntity = GetCumulativeTamperCount(dlms650PhasorEntity);
                            break; 
                        case 19:
                            dlms650PhasorEntity = GetCumulativeBillingCount(dlms650PhasorEntity);
                            break;
                        case 20:
                            dlms650PhasorEntity = GetCumulativeProgrammingCount(dlms650PhasorEntity);
                            break;
                        case 21:
                            dlms650PhasorEntity = GetBillingDate(dlms650PhasorEntity);
                            break;
                        case 22:
                            dlms650PhasorEntity = GetCumActiveEnergy(dlms650PhasorEntity); 
                            break;
                        case 23:
                            dlms650PhasorEntity = GetCumkvarhLag(dlms650PhasorEntity);
                            break;
                        case 24:
                            dlms650PhasorEntity = GetCumkvarhLead(dlms650PhasorEntity);
                            break;
                        case 25:
                            dlms650PhasorEntity = GetCumkVAhApparentEnergy(dlms650PhasorEntity);
                            break;
                        case 26:
                            dlms650PhasorEntity = GetMaximumDemandkW(dlms650PhasorEntity);
                            break;
                        case 27:
                            dlms650PhasorEntity = GetMaximumDemandkWDateTime(dlms650PhasorEntity);
                            break;
                        case 28:
                            dlms650PhasorEntity = GetMaximumDemandkVA(dlms650PhasorEntity);
                            break;
                        case 29:
                            dlms650PhasorEntity = GetMaximumDemandkVADateTime(dlms650PhasorEntity);
                            break;
                        case 30:
                            dlms650PhasorEntity = GetRPhaseNegativePowerFlag(dlms650PhasorEntity);
                            break;
                        case 31:
                            dlms650PhasorEntity = GetYPhaseNegativePowerFlag(dlms650PhasorEntity);
                            break;
                        case 32:
                            dlms650PhasorEntity = GetBPhaseNegativePowerFlag(dlms650PhasorEntity);
                            break;
                        case 33:
                            dlms650PhasorEntity = GetRPhaseLagLeadFlag(dlms650PhasorEntity);
                            break;
                        case 34:
                            dlms650PhasorEntity = GetYPhaseLagLeadFlag(dlms650PhasorEntity);
                            break;
                        case 35:
                            dlms650PhasorEntity = GetBPhaseLagLeadFlag(dlms650PhasorEntity);
                            break;
                        case 36:
                            dlms650PhasorEntity = GetAngleYR(dlms650PhasorEntity);
                            break;
                        case 37:
                            dlms650PhasorEntity = GetAngleBR(dlms650PhasorEntity);
                            break;
                        case 38:
                            dlms650PhasorEntity = GetAngleBetweenTwo(dlms650PhasorEntity);
                            break; 
                        case 39:
                            dlms650PhasorEntity = GetPhaseSequence(dlms650PhasorEntity);
                            break; 
                    }
                 
                }
                
                entities.Add(dlms650PhasorEntity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return entities;
        }

       

        private PhasorEntity GetRealTimeClockDateTime(PhasorEntity dlms650PhasorEntity)
        {

            dlms650PhasorEntity.CurrentDateTime = Convert.ToInt64((2000 + Convert.ToInt32(regExpPacketMatches.Groups[6].Value)).ToString() +
                                               regExpPacketMatches.Groups[5].Value + regExpPacketMatches.Groups[4].Value +
                                               regExpPacketMatches.Groups[3].Value + regExpPacketMatches.Groups[2].Value +
                                               regExpPacketMatches.Groups[1].Value);
            packetByteIndex += 6;

            return dlms650PhasorEntity;
        }
        private PhasorEntity GetRPhCurrent(PhasorEntity dlms650PhasorEntity)
        {

            dlms650PhasorEntity.RPhaseCurrent =(FormatParsedValue(ParseValue(includedEntitiesInPacket[6], phasorPacket.Tables["Parameter"]
                                              .Rows[6]["Sequence"].ToString()), PRECISION_CURRENT,DLMSObjectType.Current).ToString());

            return dlms650PhasorEntity;
        }
        private PhasorEntity GetYPhCurrent(PhasorEntity dlms650PhasorEntity)
        {

     
            dlms650PhasorEntity.YPhaseCurrent = (FormatParsedValue(ParseValue(includedEntitiesInPacket[7], phasorPacket.Tables["Parameter"]
                                              .Rows[7]["Sequence"].ToString()), PRECISION_CURRENT,DLMSObjectType.Current).ToString());
            return dlms650PhasorEntity;
        }
        private PhasorEntity GetBPhCurrent(PhasorEntity dlms650PhasorEntity)
        {


            dlms650PhasorEntity.BPhaseCurrent = (FormatParsedValue(ParseValue(includedEntitiesInPacket[8], phasorPacket.Tables["Parameter"]
                                              .Rows[8]["Sequence"].ToString()), PRECISION_CURRENT,DLMSObjectType.Current).ToString());
            return dlms650PhasorEntity;
        }

        private PhasorEntity GetRPhVoltage(PhasorEntity dlms650PhasorEntity)
        {

            dlms650PhasorEntity.RPhaseVoltage = (FormatParsedValue(ParseValue(includedEntitiesInPacket[9], phasorPacket.Tables["Parameter"]
                                              .Rows[9]["Sequence"].ToString()), PRECISION_VOLTAGE,DLMSObjectType.Voltage).ToString());
            return dlms650PhasorEntity;
        }
        private PhasorEntity GetYPhVoltage(PhasorEntity dlms650PhasorEntity)
        {
            dlms650PhasorEntity.YPhaseVoltage = (FormatParsedValue(ParseValue(includedEntitiesInPacket[10], phasorPacket.Tables["Parameter"]
                                              .Rows[10]["Sequence"].ToString()), PRECISION_VOLTAGE,DLMSObjectType.Voltage).ToString());
             return dlms650PhasorEntity;
        }
        private PhasorEntity GetBPhVoltage(PhasorEntity dlms650PhasorEntity)
        {

            dlms650PhasorEntity.BPhaseVoltage = (FormatParsedValue(ParseValue(includedEntitiesInPacket[11], phasorPacket.Tables["Parameter"]
                                              .Rows[11]["Sequence"].ToString()), PRECISION_VOLTAGE, DLMSObjectType.Voltage).ToString());
            return dlms650PhasorEntity;
        }
        private PhasorEntity GetRPhPowerFactor(PhasorEntity dlms650PhasorEntity)
        {
           dlms650PhasorEntity.RPhasePowerFactor = (FormatParsedSignedValue(ParseValue(includedEntitiesInPacket[12], phasorPacket.Tables["Parameter"]
                                              .Rows[12]["Sequence"].ToString()), PRECISION_PHASORPF).ToString());
            return dlms650PhasorEntity;
        }
        private PhasorEntity GetYPhPowerFactor(PhasorEntity dlms650PhasorEntity)
        {
            dlms650PhasorEntity.YPhasePowerFactor = (FormatParsedSignedValue(ParseValue(includedEntitiesInPacket[13], phasorPacket.Tables["Parameter"]
                                              .Rows[13]["Sequence"].ToString()), PRECISION_PHASORPF).ToString());
            return dlms650PhasorEntity;
        }
        private PhasorEntity GetBPhPowerFactor(PhasorEntity dlms650PhasorEntity)
        {

            dlms650PhasorEntity.BPhasePowerFactor = (FormatParsedSignedValue(ParseValue(includedEntitiesInPacket[14], phasorPacket.Tables["Parameter"].
                                              Rows[14]["Sequence"].ToString()), PRECISION_PHASORPF).ToString());
               return dlms650PhasorEntity;
        }

        private PhasorEntity GetTotalPowerFactor(PhasorEntity dlms650PhasorEntity)
        {

            dlms650PhasorEntity.TotalPhasePowerFactor = (FormatParsedSignedValue(ParseValue(includedEntitiesInPacket[15], phasorPacket.Tables["Parameter"].
                                              Rows[15]["Sequence"].ToString()), PRECISION_PHASORPF).ToString());
            return dlms650PhasorEntity;
        }
        private PhasorEntity GetFrequency(PhasorEntity dlms650PhasorEntity)
        {
            dlms650PhasorEntity.Frequency =  (FormatParsedValue(ParseValue(includedEntitiesInPacket[16], phasorPacket.Tables["Parameter"].
                                              Rows[16]["Sequence"].ToString()), PRECISION_FREQUENCY).ToString());
            return dlms650PhasorEntity;
        }
        private PhasorEntity GetApparentPower(PhasorEntity dlms650PhasorEntity)
        {

            dlms650PhasorEntity.ApparentPower = (FormatParsedSignedValue(ParseValue(includedEntitiesInPacket[17], phasorPacket.Tables["Parameter"].
                                              Rows[17]["Sequence"].ToString()), PRECISION_POWER, DLMSObjectType.Power).ToString());
            dlms650PhasorEntity.ApparentPower = Decimal.Parse(dlms650PhasorEntity.ApparentPower, System.Globalization.NumberStyles.Float).ToString();
            return dlms650PhasorEntity;
        }
        private PhasorEntity GetActivePower(PhasorEntity dlms650PhasorEntity)
        {

            dlms650PhasorEntity.ActivePower = (FormatParsedSignedValue(ParseValue(includedEntitiesInPacket[18], phasorPacket.Tables["Parameter"].
                                              Rows[18]["Sequence"].ToString()),
                                              PRECISION_POWER,DLMSObjectType.Power).ToString());
            dlms650PhasorEntity.ActivePower = Decimal.Parse(dlms650PhasorEntity.ActivePower, System.Globalization.NumberStyles.Float).ToString();
            return dlms650PhasorEntity;
        }
        private PhasorEntity GetReactivePower(PhasorEntity dlms650PhasorEntity)
        {

            dlms650PhasorEntity.ReActivePower = (FormatParsedSignedValue(ParseValue(includedEntitiesInPacket[19], phasorPacket.Tables["Parameter"]
                                              .Rows[19]["Sequence"].ToString()), PRECISION_POWER,DLMSObjectType.Power).ToString());
            dlms650PhasorEntity.ReActivePower =  Decimal.Parse(dlms650PhasorEntity.ReActivePower, System.Globalization.NumberStyles.Float).ToString();
            return dlms650PhasorEntity;
        }
        private PhasorEntity GetNumberOfPowerFailures(PhasorEntity dlms650PhasorEntity)
        {
            dlms650PhasorEntity.NumberOfPowerFailMin = (FormatParsedValue(ParseValue(includedEntitiesInPacket[20], phasorPacket.Tables["Parameter"]
                                              .Rows[20]["Sequence"].ToString()), 0).ToString());
            return dlms650PhasorEntity;
        }
        private PhasorEntity GetCumulativePowerFailureDuration(PhasorEntity dlms650PhasorEntity)
        {
            dlms650PhasorEntity.CumulativePowerFailMin =  (FormatParsedValue(ParseValue(includedEntitiesInPacket[21], phasorPacket.Tables["Parameter"]
                                                          .Rows[21]["Sequence"].ToString()), 0).ToString());
            return dlms650PhasorEntity;
        }
        private PhasorEntity GetCumulativeTamperCount(PhasorEntity dlms650PhasorEntity)
        {
            dlms650PhasorEntity.CumulativeTamperCounter = (FormatParsedValue(ParseValue(includedEntitiesInPacket[22], phasorPacket.Tables["Parameter"]
                                                          .Rows[22]["Sequence"].ToString()), 0).ToString());
            return dlms650PhasorEntity;
        }
        private PhasorEntity GetCumulativeBillingCount(PhasorEntity dlms650PhasorEntity)
        {         
             dlms650PhasorEntity.CumulativeBillingCounter = (FormatParsedValue(ParseValue(includedEntitiesInPacket[23], phasorPacket.Tables["Parameter"].
                                              Rows[23]["Sequence"].ToString()), 0).ToString());
             return dlms650PhasorEntity;
        }
        private PhasorEntity GetCumulativeProgrammingCount(PhasorEntity dlms650PhasorEntity)
        {
 
            dlms650PhasorEntity.CumulativeProgrammingCounter =
                (FormatParsedValue(ParseValue(includedEntitiesInPacket[24], phasorPacket.Tables["Parameter"]
                                              .Rows[24]["Sequence"].ToString()), 0).ToString());
            return dlms650PhasorEntity;
        }
        private PhasorEntity GetBillingDate(PhasorEntity dlms650PhasorEntity)
        {

            dlms650PhasorEntity.BillingDate = Convert.ToInt64((2000 + Convert.ToInt32(regExpPacketMatches.Groups[58].Value)).ToString() +
                                               regExpPacketMatches.Groups[57].Value + regExpPacketMatches.Groups[56].Value +
                                               regExpPacketMatches.Groups[55].Value + regExpPacketMatches.Groups[54].Value + "99");
            packetByteIndex += 5;
            return dlms650PhasorEntity;
        }
        private PhasorEntity GetCumActiveEnergy(PhasorEntity dlms650PhasorEntity)
        {

            dlms650PhasorEntity.CumulativeActiveEnergy = (FormatParsedLongValue(ParseValue(includedEntitiesInPacket[30], phasorPacket.Tables["Parameter"].
                                              Rows[30]["Sequence"].ToString()), PRECISION_ENERGY, DLMSObjectType.Energy).ToString());
            return dlms650PhasorEntity;
        }
        private PhasorEntity GetCumkvarhLag(PhasorEntity dlms650PhasorEntity)
        {

            dlms650PhasorEntity.CumulativeReactiveLagEnergy = (FormatParsedLongValue(ParseValue(includedEntitiesInPacket[31], phasorPacket.Tables["Parameter"]
                                              .Rows[31]["Sequence"].ToString()), PRECISION_ENERGY, DLMSObjectType.Energy).ToString());
            return dlms650PhasorEntity;
        }
        private PhasorEntity GetCumkvarhLead(PhasorEntity dlms650PhasorEntity)
        {
            dlms650PhasorEntity.CumulativeReactiveLeadEnergy = (FormatParsedLongValue(ParseValue(includedEntitiesInPacket[32], phasorPacket.Tables["Parameter"]
                                              .Rows[32]["Sequence"].ToString()), PRECISION_ENERGY, DLMSObjectType.Energy).ToString());
            return dlms650PhasorEntity;
        }
        private PhasorEntity GetCumkVAhApparentEnergy(PhasorEntity dlms650PhasorEntity)
        {
            dlms650PhasorEntity.CumulativeApparentEnergy = (FormatParsedLongValue(ParseValue(includedEntitiesInPacket[33], phasorPacket.Tables["Parameter"]
                                              .Rows[33]["Sequence"].ToString()), PRECISION_ENERGY,DLMSObjectType.Energy).ToString());
            return dlms650PhasorEntity;
        }

        private PhasorEntity GetMaximumDemandkW(PhasorEntity dlms650PhasorEntity)
        {

            dlms650PhasorEntity.MDOneKWData = (FormatParsedValue(ParseValue(includedEntitiesInPacket[34], phasorPacket.Tables["Parameter"]
                                              .Rows[34]["Sequence"].ToString()), PRECISION_ENERGY,DLMSObjectType.Demand).ToString());
            return dlms650PhasorEntity;
        }
        private PhasorEntity GetMaximumDemandkWDateTime(PhasorEntity dlms650PhasorEntity)
        {

           dlms650PhasorEntity.MDOneKWTimeStamp = Convert.ToInt64((2000 + Convert.ToInt32(regExpPacketMatches.Groups[99].Value)).ToString() +
                                               regExpPacketMatches.Groups[98].Value + regExpPacketMatches.Groups[97].Value +
                                               regExpPacketMatches.Groups[96].Value + regExpPacketMatches.Groups[95].Value + "99");
           packetByteIndex += 5;
           return dlms650PhasorEntity;
        }
        private PhasorEntity GetMaximumDemandkVA(PhasorEntity dlms650PhasorEntity)
        {

           dlms650PhasorEntity.MDTwoKVAData = (FormatParsedValue(ParseValue(includedEntitiesInPacket[40], phasorPacket.Tables["Parameter"]
                                              .Rows[40]["Sequence"].ToString()), PRECISION_ENERGY,DLMSObjectType.Demand).ToString());
            
            return dlms650PhasorEntity;
        }
        private PhasorEntity GetMaximumDemandkVADateTime(PhasorEntity dlms650PhasorEntity)
        {      
            dlms650PhasorEntity.MDTwoKVATimeStamp = Convert.ToInt64((2000 + Convert.ToInt32(regExpPacketMatches.Groups[108].Value)).ToString() +
                                               regExpPacketMatches.Groups[107].Value + regExpPacketMatches.Groups[106].Value +
                                               regExpPacketMatches.Groups[105].Value + regExpPacketMatches.Groups[104].Value + "99");
            packetByteIndex += 5;
            return dlms650PhasorEntity;
        }
        private PhasorEntity GetRPhaseNegativePowerFlag(PhasorEntity dlms650PhasorEntity)
        {
            dlms650PhasorEntity.RPhaseNegativePowerFlag = dlms650PhasorEntity.MDTwoKVAData = (FormatParsedValue(ParseValue(includedEntitiesInPacket[46], phasorPacket.Tables["Parameter"]
                                              .Rows[46]["Sequence"].ToString()), 0).ToString());
            if (dlms650PhasorEntity.RPhaseNegativePowerFlag == "0")
            {
                dlms650PhasorEntity.RPhaseNegativePowerFlag = IMPORT;
            }
            else if (dlms650PhasorEntity.RPhaseNegativePowerFlag == "1")
            {
                dlms650PhasorEntity.RPhaseNegativePowerFlag = EXPORT;
            }
            else
            {
                dlms650PhasorEntity.RPhaseNegativePowerFlag = NOVALUE;
            }
            return dlms650PhasorEntity;
        }
        private PhasorEntity GetYPhaseNegativePowerFlag(PhasorEntity dlms650PhasorEntity)
        {
            dlms650PhasorEntity.YPhaseNegativePowerFlag = dlms650PhasorEntity.MDTwoKVAData = (FormatParsedValue(ParseValue(includedEntitiesInPacket[47], phasorPacket.Tables["Parameter"]
                                              .Rows[47]["Sequence"].ToString()), 0).ToString());
            if (dlms650PhasorEntity.YPhaseNegativePowerFlag == "0")
            {
                dlms650PhasorEntity.YPhaseNegativePowerFlag = IMPORT;
            }
            else if (dlms650PhasorEntity.YPhaseNegativePowerFlag == "1")
            {
                dlms650PhasorEntity.YPhaseNegativePowerFlag = EXPORT;
            }
            else
            {
                dlms650PhasorEntity.YPhaseNegativePowerFlag = NOVALUE;
            }
            return dlms650PhasorEntity;
        }
        private PhasorEntity GetBPhaseNegativePowerFlag(PhasorEntity dlms650PhasorEntity)
        {
            dlms650PhasorEntity.BPhaseNegativePowerFlag = dlms650PhasorEntity.MDTwoKVAData = (FormatParsedValue(ParseValue(includedEntitiesInPacket[48], phasorPacket.Tables["Parameter"]
                                              .Rows[48]["Sequence"].ToString()), 0).ToString());
            if (dlms650PhasorEntity.BPhaseNegativePowerFlag == "0")
            {
                dlms650PhasorEntity.BPhaseNegativePowerFlag = IMPORT;
            }
            else if (dlms650PhasorEntity.BPhaseNegativePowerFlag == "1")
            {
                dlms650PhasorEntity.BPhaseNegativePowerFlag = EXPORT;
            }
            else
            {
                dlms650PhasorEntity.BPhaseNegativePowerFlag = NOVALUE;
            }
            return dlms650PhasorEntity;
        }
        private PhasorEntity GetRPhaseLagLeadFlag(PhasorEntity dlms650PhasorEntity)
        {
            dlms650PhasorEntity.RPhaseCapacitiveInductiveFlag = (FormatParsedValue(ParseValue(includedEntitiesInPacket[49], phasorPacket.Tables["Parameter"]
                                              .Rows[49]["Sequence"].ToString()), 0).ToString());
            if (dlms650PhasorEntity.RPhaseCapacitiveInductiveFlag == "0")
            {
                dlms650PhasorEntity.RPhaseCapacitiveInductiveFlag = LEAD;
            }
            else if (dlms650PhasorEntity.RPhaseCapacitiveInductiveFlag == "1")
            {
                dlms650PhasorEntity.RPhaseCapacitiveInductiveFlag = LAG;
            }
            else
                dlms650PhasorEntity.RPhaseCapacitiveInductiveFlag = NOVALUE;
            return dlms650PhasorEntity;
        }
        private PhasorEntity GetYPhaseLagLeadFlag(PhasorEntity dlms650PhasorEntity)
        {
            dlms650PhasorEntity.YPhaseCapacitiveInductiveFlag = (FormatParsedValue(ParseValue(includedEntitiesInPacket[50], phasorPacket.Tables["Parameter"]
                                              .Rows[50]["Sequence"].ToString()), 0).ToString());
            if (dlms650PhasorEntity.YPhaseCapacitiveInductiveFlag == "0")
            {
                dlms650PhasorEntity.YPhaseCapacitiveInductiveFlag = LEAD;
            }
            else if (dlms650PhasorEntity.YPhaseCapacitiveInductiveFlag == "1")
            {
                dlms650PhasorEntity.YPhaseCapacitiveInductiveFlag = LAG;
            }
            else
            {
                dlms650PhasorEntity.YPhaseCapacitiveInductiveFlag = NOVALUE;
            }
            return dlms650PhasorEntity;
        }
        private PhasorEntity GetBPhaseLagLeadFlag(PhasorEntity dlms650PhasorEntity)
        {
            dlms650PhasorEntity.BPhaseCapacitiveInductiveFlag =(FormatParsedValue(ParseValue(includedEntitiesInPacket[51], phasorPacket.Tables["Parameter"]
                                              .Rows[51]["Sequence"].ToString()), 0).ToString());
            if (dlms650PhasorEntity.BPhaseCapacitiveInductiveFlag == "0")
            {
                dlms650PhasorEntity.BPhaseCapacitiveInductiveFlag = LEAD;
            }
            else if (dlms650PhasorEntity.BPhaseCapacitiveInductiveFlag == "1")
            {
                dlms650PhasorEntity.BPhaseCapacitiveInductiveFlag = LAG;
            }
            else
            {
                dlms650PhasorEntity.BPhaseCapacitiveInductiveFlag = NOVALUE;
            }
            return dlms650PhasorEntity;
        }
        private PhasorEntity GetAngleYR(PhasorEntity dlms650PhasorEntity)
        {
            dlms650PhasorEntity.AngleYR = (FormatParsedValue(ParseValue(includedEntitiesInPacket[52], phasorPacket.Tables["Parameter"]
                                              .Rows[52]["Sequence"].ToString()), 0).ToString());
            return dlms650PhasorEntity;
        }
        private PhasorEntity GetAngleBR(PhasorEntity dlms650PhasorEntity)
        {
            dlms650PhasorEntity.AngleBR = (FormatParsedValue(ParseValue(includedEntitiesInPacket[53], phasorPacket.Tables["Parameter"]
                                              .Rows[53]["Sequence"].ToString()), 0).ToString());
            return dlms650PhasorEntity;
        }
        private PhasorEntity GetAngleBetweenTwo(PhasorEntity dlms650PhasorEntity)
        {
            dlms650PhasorEntity.AngleBetweenTwo = (FormatParsedValue(ParseValue(includedEntitiesInPacket[54], phasorPacket.Tables["Parameter"]
                                              .Rows[54]["Sequence"].ToString()), 0).ToString() );
            return dlms650PhasorEntity;
        }
        private PhasorEntity GetPhaseSequence(PhasorEntity dlms650PhasorEntity)
        {
            int tempPhasorSequence = 0;
            try
            {
                dlms650PhasorEntity.PhaseSequence = INCORRECT;
                if (int.TryParse(FormatParsedValue(ParseValue(includedEntitiesInPacket[55], phasorPacket.Tables["Parameter"]
                                                  .Rows[55]["Sequence"].ToString()), 0).ToString(), out tempPhasorSequence))
                { 
                   if (tempPhasorSequence == 0 || tempPhasorSequence == 2)
                    {
                        dlms650PhasorEntity.PhaseSequence = CORRECT;
                    }
                }
               
            }
            catch (Exception ex)
            {
                dlms650PhasorEntity.PhaseSequence = INCORRECT;
                return dlms650PhasorEntity;
            }
            return dlms650PhasorEntity;
        }
        private string ParseValue(int size, string sequence)
        {
            string parsedValue = "";
            try
            {
                if (sequence == "1")
                {

                    for (int i = packetByteIndex + 1; i <= packetByteIndex + size; i += 1)
                        parsedValue += regExpPacketMatches.Groups[i].Value;

                }
                else
                {
                    for (int i = packetByteIndex + size; i > packetByteIndex; i -= 1)
                        parsedValue += regExpPacketMatches.Groups[i].Value;
                }

                packetByteIndex += size;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return parsedValue;
        }

        public PhasorEntity  getPhasorEntity()
        {
            List<IEntity> entities = new List<IEntity>();
            
            // To verify the BCC of general data for data authentication.
            if (!VerifyBCC(phasorData))
            {
                SetParsingStatus(rmFDLParse.GetString("BCCMismatchPhasor"));
                return null;
            }
            try
            {
                //Extract the billing packet structure from xml.
                phasorPacket = (PhasorDataPacketStructure)GetPacketParsingStructure("PhasorDataPacketStructure.xml", typeof(PhasorDataPacketStructure));
                //Set Packet Entries with size.
                SetPacketEntitesToParse(phasorPacket);
                DefineParsingforPacketStructure(phasorPacket.Tables["Parameter"]);
               
                string currentPacket = string.Empty;
                currentPacket = phasorData.Substring(0, 2 * packetLength);
                regExpPacketMatches = packetRegex.Match(currentPacket);
                if (regExpPacketMatches != null && regExpPacketMatches.Groups != null && regExpPacketMatches.Groups.Count > 1)
                {
                    entities = FillPhasorEntity();
            
                }
                

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return entities[0] as PhasorEntity;
        }
    }
}
