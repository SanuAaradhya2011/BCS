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
    public class ParseFDLInstantData : ParseFDLData
    {
        string instantData = string.Empty;
       
        const string INVALIDVALUE = "00";
        const string INSTANTSECONDS = "99";

        public ParseFDLInstantData(string instantData,string fileText,long fileUploadID,long meterDataID,string demandResolution,string energyResolution)
            : base( fileText, fileUploadID, meterDataID)
        {
            this.instantData = instantData;
            this.demandResolution = demandResolution;
            this.energyResolution = energyResolution;
        }
        InstantaneousDataPacketStructure instantPacket;
        public override FDLFileParseStatuses Parse()
        {
            // To verify the BCC of general data for data authentication.
            if (!VerifyBCC(instantData))
            {
                SetParsingStatus(rmFDLParse.GetString("BCCMismatchInstantaneous"));
                return FDLFileParseStatuses.BCCMismatchGeneral;
            }
            try
            {
                //Extract the billing packet structure from xml.
                instantPacket = (InstantaneousDataPacketStructure)GetPacketParsingStructure("InstantaneousDataPacketStructure.xml", typeof(InstantaneousDataPacketStructure));
                //Set Packet Entries with size.
                SetPacketEntitesToParse(instantPacket);
                DefineParsingforPacketStructure(instantPacket.Tables["Parameter"]);
                List<IEntity> entities = new List<IEntity>();
                string currentPacket = string.Empty;
                currentPacket = instantData.Substring(0, 2 * packetLength);
                regExpPacketMatches = packetRegex.Match(currentPacket);
                if (regExpPacketMatches != null && regExpPacketMatches.Groups != null && regExpPacketMatches.Groups.Count > 1)
                {
                    GetFDLEnergyResolution();
                    GetFDLDemandResolution();
                    entities = FillInstantEntity();
                    entities = FillInstantValues1(entities);
                    entities = FillInstantValues2(entities);
                }
                new DLMS650InstantaneousBLL().InsertData(entities);
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return FDLFileParseStatuses.None;
        }
      
        private List<IEntity> FillInstantValues1(List<IEntity> entities)
        {
            try
            {
                for (int i = 11; i < 21; i++)
                {
                    DLMS650InstantaneousEntity dlms650InstantEntity = new DLMS650InstantaneousEntity();
                    dlms650InstantEntity.MeterDataID = meterDataID;
                    switch (i)
                    {
                        case 11:
                            dlms650InstantEntity = GetTotalPowerFactor(dlms650InstantEntity);
                            break;
                        case 12:
                            dlms650InstantEntity = GetFrequency(dlms650InstantEntity);
                            break;
                        case 13:
                            dlms650InstantEntity = GetApparentPower(dlms650InstantEntity);
                            break;
                        case 14:
                            dlms650InstantEntity = GetActivePower(dlms650InstantEntity);
                            break;
                        case 15:
                            dlms650InstantEntity = GetReactivePower(dlms650InstantEntity);
                            break;
                        case 16:
                            dlms650InstantEntity = GetNumberOfPowerFailures(dlms650InstantEntity);
                            break;
                        case 17:
                            dlms650InstantEntity = GetCumulativePowerFailureDuration(dlms650InstantEntity);
                            break;
                        case 18:
                            dlms650InstantEntity = GetCumulativeTamperCount(dlms650InstantEntity);
                            break;
                        case 19:
                            dlms650InstantEntity = GetCumulativeBillingCount(dlms650InstantEntity);
                            break;
                        case 20:
                            dlms650InstantEntity = GetCumulativeProgrammingCount(dlms650InstantEntity);
                            break;
                    }
                    entities.Add(dlms650InstantEntity);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return entities;
        }
        private List<IEntity> FillInstantValues2(List<IEntity> entities)
        {
            try
            {
                for (int i = 21; i < 30; i++)
                {
                    DLMS650InstantaneousEntity dlms650InstantEntity = new DLMS650InstantaneousEntity();
                    dlms650InstantEntity.MeterDataID = meterDataID;
                    switch (i)
                    {
                        case 21:
                            dlms650InstantEntity = GetBillingDate(dlms650InstantEntity);
                            break;
                        case 22:
                            dlms650InstantEntity = GetCumkW(dlms650InstantEntity);
                            break;
                        case 23:
                            dlms650InstantEntity = GetCumkvarhLag(dlms650InstantEntity);
                            break;
                        case 24:
                            dlms650InstantEntity = GetCumkvarhLead(dlms650InstantEntity);
                            break;
                        case 25:
                            dlms650InstantEntity = GetCumkVAh(dlms650InstantEntity);
                            break;
                        case 26:
                            dlms650InstantEntity = GetMaximumDemandkW(dlms650InstantEntity);
                            break;
                        case 27:
                            dlms650InstantEntity = GetMaximumDemandkWDateTime(dlms650InstantEntity);
                            break;
                        case 28:
                            dlms650InstantEntity = GetMaximumDemandkVA(dlms650InstantEntity);
                            break;
                        case 29:
                            dlms650InstantEntity = GetMaximumDemandkVADateTime(dlms650InstantEntity);
                            break;

                    }
                    entities.Add(dlms650InstantEntity);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
                return entities;

        }
        private  List<IEntity> FillInstantEntity()
        {
            List<IEntity> entities = new List<IEntity>();
            try
            {
                for (int i = 1; i < 11; i++)
                {
                    DLMS650InstantaneousEntity dlms650InstantEntity = new DLMS650InstantaneousEntity();
                    dlms650InstantEntity.MeterDataID = meterDataID;
                    switch (i)
                    {
                        case 1:
                            dlms650InstantEntity = GetRealTimeClockDateTime(dlms650InstantEntity);
                            break;
                        case 2:
                            dlms650InstantEntity = GetRPhCurrent(dlms650InstantEntity);
                            break;
                        case 3:
                            dlms650InstantEntity = GetYPhCurrent(dlms650InstantEntity);
                            break;
                        case 4:
                            dlms650InstantEntity = GetBPhCurrent(dlms650InstantEntity);
                            break;
                        case 5:
                            dlms650InstantEntity = GetRPhVoltage(dlms650InstantEntity);
                            break;
                        case 6:
                            dlms650InstantEntity = GetYPhVoltage(dlms650InstantEntity);
                            break;
                        case 7:
                            dlms650InstantEntity = GetBPhVoltage(dlms650InstantEntity);
                            break;
                        case 8:
                            dlms650InstantEntity = GetRPhPowerFactor(dlms650InstantEntity);
                            break;
                        case 9:
                            dlms650InstantEntity = GetYPhPowerFactor(dlms650InstantEntity);
                            break;
                        case 10:
                            dlms650InstantEntity = GetBPhPowerFactor(dlms650InstantEntity);
                            break;
                    }
                    entities.Add(dlms650InstantEntity);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return entities;
        }
        
        private DLMS650InstantaneousEntity GetRealTimeClockDateTime(DLMS650InstantaneousEntity dlms650InstantEntity)
        {
            dlms650InstantEntity.InstantPowerObisCode = "0.0.1.0.0.255";
            dlms650InstantEntity.InstantPowerAttribute = "2"; 
            dlms650InstantEntity.InstantPowerClassID = "8";
            dlms650InstantEntity.InstantPowerColumnName = "Real Time Clock – Date and Time";

            dlms650InstantEntity.InstantPowerColumnValue = 
                                              (2000 + Convert.ToInt32(regExpPacketMatches.Groups[6].Value)).ToString() +
                                               regExpPacketMatches.Groups[5].Value + regExpPacketMatches.Groups[4].Value +
                                               regExpPacketMatches.Groups[3].Value + regExpPacketMatches.Groups[2].Value
                                               + regExpPacketMatches.Groups[1].Value;
            dlms650InstantEntity.InstantPowerDataIndex = 1;
            return dlms650InstantEntity;
        }
        private DLMS650InstantaneousEntity GetRPhCurrent(DLMS650InstantaneousEntity dlms650InstantEntity)
        {
            packetByteIndex = 6;
            dlms650InstantEntity.InstantPowerObisCode = "1.0.31.7.0.255";
            dlms650InstantEntity.InstantPowerAttribute = "2";
            dlms650InstantEntity.InstantPowerClassID = "3";
            dlms650InstantEntity.InstantPowerColumnName = "Current - IR";
            dlms650InstantEntity.InstantPowerColumnValue =
                                              (FormatParsedValue(ParseValue(includedEntitiesInPacket[6], instantPacket.Tables["Parameter"]
                                              .Rows[6]["Sequence"].ToString()), PRECISION_CURRENT,DLMSObjectType.Current).ToString("0.000") + UNIT_CURRENT);
            dlms650InstantEntity.InstantPowerDataIndex = 2;
            return dlms650InstantEntity;
        }
        private DLMS650InstantaneousEntity GetYPhCurrent(DLMS650InstantaneousEntity dlms650InstantEntity)
        {
            
            dlms650InstantEntity.InstantPowerObisCode = "1.0.51.7.0.255";
            dlms650InstantEntity.InstantPowerAttribute = "2";
            dlms650InstantEntity.InstantPowerClassID = "3";
            dlms650InstantEntity.InstantPowerColumnName = "Current - IY";
            dlms650InstantEntity.InstantPowerColumnValue =
                                              (FormatParsedValue(ParseValue(includedEntitiesInPacket[7], instantPacket.Tables["Parameter"]
                                              .Rows[7]["Sequence"].ToString()), PRECISION_CURRENT,DLMSObjectType.Current).ToString("0.000") + UNIT_CURRENT);
            dlms650InstantEntity.InstantPowerDataIndex = 3;
            return dlms650InstantEntity;
        }
        private DLMS650InstantaneousEntity GetBPhCurrent(DLMS650InstantaneousEntity dlms650InstantEntity)
        {
           
            dlms650InstantEntity.InstantPowerObisCode = "1.0.71.7.0.255";
            dlms650InstantEntity.InstantPowerAttribute = "2";
            dlms650InstantEntity.InstantPowerClassID = "3";
            dlms650InstantEntity.InstantPowerColumnName = "Current - IB";
            dlms650InstantEntity.InstantPowerColumnValue =
                                              (FormatParsedValue(ParseValue(includedEntitiesInPacket[8], instantPacket.Tables["Parameter"]
                                              .Rows[8]["Sequence"].ToString()), PRECISION_CURRENT,DLMSObjectType.Current).ToString("0.000") + UNIT_CURRENT);
            dlms650InstantEntity.InstantPowerDataIndex = 4;
            return dlms650InstantEntity;
        }
        private DLMS650InstantaneousEntity GetRPhVoltage(DLMS650InstantaneousEntity dlms650InstantEntity)
        {
           
            dlms650InstantEntity.InstantPowerObisCode = "1.0.32.7.0.255";
            dlms650InstantEntity.InstantPowerAttribute = "2";
            dlms650InstantEntity.InstantPowerClassID = "3";
            dlms650InstantEntity.InstantPowerColumnName = "Voltage – VRN";
            dlms650InstantEntity.InstantPowerColumnValue =
                                              (FormatParsedValue(ParseValue(includedEntitiesInPacket[9], instantPacket.Tables["Parameter"]
                                              .Rows[9]["Sequence"].ToString()), PRECISION_VOLTAGE,DLMSObjectType.Voltage).ToString("0.00") + UNIT_VOLTAGE);
            dlms650InstantEntity.InstantPowerDataIndex = 5;
            return dlms650InstantEntity;
        }
        private DLMS650InstantaneousEntity GetYPhVoltage(DLMS650InstantaneousEntity dlms650InstantEntity)
        {
            
            dlms650InstantEntity.InstantPowerObisCode = "1.0.52.7.0.255";
            dlms650InstantEntity.InstantPowerAttribute = "2";
            dlms650InstantEntity.InstantPowerClassID = "3";
            dlms650InstantEntity.InstantPowerColumnName = "Voltage – VYN";
            dlms650InstantEntity.InstantPowerColumnValue =
                                              (FormatParsedValue(ParseValue(includedEntitiesInPacket[10], instantPacket.Tables["Parameter"]
                                              .Rows[10]["Sequence"].ToString()), PRECISION_VOLTAGE,DLMSObjectType.Voltage).ToString("0.00") + UNIT_VOLTAGE);
            dlms650InstantEntity.InstantPowerDataIndex = 6;
            return dlms650InstantEntity;
        }
        private DLMS650InstantaneousEntity GetBPhVoltage(DLMS650InstantaneousEntity dlms650InstantEntity)
        {
            
            dlms650InstantEntity.InstantPowerObisCode = "1.0.72.7.0.255";
            dlms650InstantEntity.InstantPowerAttribute = "2";
            dlms650InstantEntity.InstantPowerClassID = "3";
            dlms650InstantEntity.InstantPowerColumnName = "Voltage – VBN";
            dlms650InstantEntity.InstantPowerColumnValue =
                                              (FormatParsedValue(ParseValue(includedEntitiesInPacket[11], instantPacket.Tables["Parameter"]
                                              .Rows[11]["Sequence"].ToString()), PRECISION_VOLTAGE,DLMSObjectType.Voltage).ToString("0.00") + UNIT_VOLTAGE);
            dlms650InstantEntity.InstantPowerDataIndex = 7;
            return dlms650InstantEntity;
        }
        private DLMS650InstantaneousEntity GetRPhPowerFactor(DLMS650InstantaneousEntity dlms650InstantEntity)
        {
           
            dlms650InstantEntity.InstantPowerObisCode = "1.0.33.7.0.255";
            dlms650InstantEntity.InstantPowerAttribute = "2";
            dlms650InstantEntity.InstantPowerClassID = "3";
            dlms650InstantEntity.InstantPowerColumnName = "Signed Power Factor – R phase";
            dlms650InstantEntity.InstantPowerColumnValue =
                                              (FormatParsedSignedValue(ParseValue(includedEntitiesInPacket[12], instantPacket.Tables["Parameter"]
                                              .Rows[12]["Sequence"].ToString()), PRECISION_PF).ToString("0.0000"));
            dlms650InstantEntity.InstantPowerDataIndex = 8;
            return dlms650InstantEntity;
        }
        private DLMS650InstantaneousEntity GetYPhPowerFactor(DLMS650InstantaneousEntity dlms650InstantEntity)
        {
            dlms650InstantEntity.InstantPowerObisCode = "1.0.53.7.0.255";
            dlms650InstantEntity.InstantPowerAttribute = "2";
            dlms650InstantEntity.InstantPowerClassID = "3";
            dlms650InstantEntity.InstantPowerColumnName = "Signed Power Factor – Y phase";
            dlms650InstantEntity.InstantPowerColumnValue =
                                              (FormatParsedSignedValue(ParseValue(includedEntitiesInPacket[13], instantPacket.Tables["Parameter"]
                                              .Rows[13]["Sequence"].ToString()), PRECISION_PF).ToString("0.0000"));
            dlms650InstantEntity.InstantPowerDataIndex = 9;
            return dlms650InstantEntity;
        }
        private DLMS650InstantaneousEntity GetBPhPowerFactor(DLMS650InstantaneousEntity dlms650InstantEntity)
        {
            dlms650InstantEntity.InstantPowerObisCode = "1.0.73.7.0.255";
            dlms650InstantEntity.InstantPowerAttribute = "2";
            dlms650InstantEntity.InstantPowerClassID = "3";
            dlms650InstantEntity.InstantPowerColumnName = "Signed Power Factor – B phase";
            dlms650InstantEntity.InstantPowerColumnValue =
                                              (FormatParsedSignedValue(ParseValue(includedEntitiesInPacket[14], instantPacket.Tables["Parameter"].
                                              Rows[14]["Sequence"].ToString()), PRECISION_PF).ToString("0.0000"));
            dlms650InstantEntity.InstantPowerDataIndex = 10;
            return dlms650InstantEntity;
        }
        private DLMS650InstantaneousEntity GetTotalPowerFactor(DLMS650InstantaneousEntity dlms650InstantEntity)
        {
            
            dlms650InstantEntity.InstantPowerObisCode = "1.0.13.7.0.255";
            dlms650InstantEntity.InstantPowerAttribute = "2";
            dlms650InstantEntity.InstantPowerClassID = "3";
            dlms650InstantEntity.InstantPowerColumnName = "Three Phase Power Factor – PF";
            dlms650InstantEntity.InstantPowerColumnValue =
                                              (FormatParsedSignedValue(ParseValue(includedEntitiesInPacket[15], instantPacket.Tables["Parameter"].
                                              Rows[15]["Sequence"].ToString()), PRECISION_PF).ToString("0.0000"));
            dlms650InstantEntity.InstantPowerDataIndex = 11;
            return dlms650InstantEntity;
        }
        private DLMS650InstantaneousEntity GetFrequency(DLMS650InstantaneousEntity dlms650InstantEntity)
        {
            dlms650InstantEntity.InstantPowerObisCode = "1.0.14.7.0.255";
            dlms650InstantEntity.InstantPowerAttribute = "2";
            dlms650InstantEntity.InstantPowerClassID = "3";
            dlms650InstantEntity.InstantPowerColumnName = "Frequency";
            dlms650InstantEntity.InstantPowerColumnValue =
                                              (FormatParsedValue(ParseValue(includedEntitiesInPacket[16], instantPacket.Tables["Parameter"].
                                              Rows[16]["Sequence"].ToString()), PRECISION_FREQUENCY).ToString() + UNIT_FREQUENCY);
            dlms650InstantEntity.InstantPowerDataIndex = 12;
            return dlms650InstantEntity;
        }
        private DLMS650InstantaneousEntity GetApparentPower(DLMS650InstantaneousEntity dlms650InstantEntity)
        {
            dlms650InstantEntity.InstantPowerObisCode = "1.0.9.7.0.255";
            dlms650InstantEntity.InstantPowerAttribute = "2";
            dlms650InstantEntity.InstantPowerClassID = "3";
            dlms650InstantEntity.InstantPowerColumnName = string.Format("Apparent Power – {0}", UNIT_DEMANDKVA.Substring(1));//kVA
            dlms650InstantEntity.InstantPowerColumnValue =
                                              (FormatParsedSignedValue(ParseValue(includedEntitiesInPacket[17], instantPacket.Tables["Parameter"].
                                              Rows[17]["Sequence"].ToString()), PRECISION_POWER,DLMSObjectType.Power).ToString("0.00000") + UNIT_DEMANDKVA);
            dlms650InstantEntity.InstantPowerDataIndex = 13;
            return dlms650InstantEntity;
        }
        private DLMS650InstantaneousEntity GetActivePower(DLMS650InstantaneousEntity dlms650InstantEntity)
        {
            dlms650InstantEntity.InstantPowerObisCode = "1.0.1.7.0.255";
            dlms650InstantEntity.InstantPowerAttribute = "2";
            dlms650InstantEntity.InstantPowerClassID = "3";
            dlms650InstantEntity.InstantPowerColumnName = "Active Power (ABS)";
            dlms650InstantEntity.InstantPowerColumnValue =
                                              (FormatParsedSignedValue(ParseValue(includedEntitiesInPacket[18], instantPacket.Tables["Parameter"].
                                              Rows[18]["Sequence"].ToString()),
                                              PRECISION_POWER,DLMSObjectType.Power).ToString("0.00000") + UNIT_DEMANDKW);
            dlms650InstantEntity.InstantPowerDataIndex = 14;
            return dlms650InstantEntity;
        }
        private DLMS650InstantaneousEntity GetReactivePower(DLMS650InstantaneousEntity dlms650InstantEntity)
        {
            dlms650InstantEntity.InstantPowerObisCode = "1.0.3.7.0.255";
            dlms650InstantEntity.InstantPowerAttribute = "2";
            dlms650InstantEntity.InstantPowerClassID = "3";
            dlms650InstantEntity.InstantPowerColumnName = string.Format("Signed Reactive Power – {0} (+ Lag - Lead)",UNIT_DEMANDKVAR.Substring(1));//kvar
            dlms650InstantEntity.InstantPowerColumnValue =
                                              (FormatParsedSignedValue(ParseValue(includedEntitiesInPacket[19], instantPacket.Tables["Parameter"]
                                              .Rows[19]["Sequence"].ToString()), PRECISION_POWER, DLMSObjectType.Energy).ToString("0.00000") + UNIT_DEMANDKVAR);
            dlms650InstantEntity.InstantPowerDataIndex = 15;
            return dlms650InstantEntity;
        }
        private DLMS650InstantaneousEntity GetNumberOfPowerFailures(DLMS650InstantaneousEntity dlms650InstantEntity)
        {
            dlms650InstantEntity.InstantPowerObisCode = "0.0.96.7.0.255";
            dlms650InstantEntity.InstantPowerAttribute = "2";
            dlms650InstantEntity.InstantPowerClassID = "1";
            dlms650InstantEntity.InstantPowerColumnName = "Number of Power - Failures";
            dlms650InstantEntity.InstantPowerColumnValue =
                                              (FormatParsedValue(ParseValue(includedEntitiesInPacket[20], instantPacket.Tables["Parameter"]
                                              .Rows[20]["Sequence"].ToString()),0).ToString()) + "*";
            dlms650InstantEntity.InstantPowerDataIndex = 16;
            return dlms650InstantEntity;
        }
        private DLMS650InstantaneousEntity GetCumulativePowerFailureDuration(DLMS650InstantaneousEntity dlms650InstantEntity)
        {
            dlms650InstantEntity.InstantPowerObisCode = "0.0.94.91.8.255";
            dlms650InstantEntity.InstantPowerAttribute = "2";
            dlms650InstantEntity.InstantPowerClassID = "3";
            dlms650InstantEntity.InstantPowerColumnName = "Cumulative Power-Failure Duration";
            dlms650InstantEntity.InstantPowerColumnValue =
                                              (FormatParsedValue(ParseValue(includedEntitiesInPacket[21], instantPacket.Tables["Parameter"]
                                              .Rows[21]["Sequence"].ToString()),0).ToString()) + "*min";
            dlms650InstantEntity.InstantPowerDataIndex = 17;
            return dlms650InstantEntity;
        }
        private DLMS650InstantaneousEntity GetCumulativeTamperCount(DLMS650InstantaneousEntity dlms650InstantEntity)
        {
            dlms650InstantEntity.InstantPowerObisCode = "0.0.94.91.0.255";
            dlms650InstantEntity.InstantPowerAttribute = "2";
            dlms650InstantEntity.InstantPowerClassID = "1";
            dlms650InstantEntity.InstantPowerColumnName = "Cumulative Tamper Count";
            dlms650InstantEntity.InstantPowerColumnValue =
                                              (FormatParsedValue(ParseValue(includedEntitiesInPacket[22], instantPacket.Tables["Parameter"]
                                              .Rows[22]["Sequence"].ToString()), 0).ToString()) + "*";
            dlms650InstantEntity.InstantPowerDataIndex = 18;
            return dlms650InstantEntity;
        }
        private DLMS650InstantaneousEntity GetCumulativeBillingCount(DLMS650InstantaneousEntity dlms650InstantEntity)
        {
            dlms650InstantEntity.InstantPowerObisCode = "0.0.0.1.0.255";
            dlms650InstantEntity.InstantPowerAttribute = "2";
            dlms650InstantEntity.InstantPowerClassID = "1";
            dlms650InstantEntity.InstantPowerColumnName = "Cumulative Billing Count";
            dlms650InstantEntity.InstantPowerColumnValue =
                                              (FormatParsedValue(ParseValue(includedEntitiesInPacket[23], instantPacket.Tables["Parameter"].
                                              Rows[23]["Sequence"].ToString()), 0).ToString()) + "*";
            dlms650InstantEntity.InstantPowerDataIndex = 19;
            return dlms650InstantEntity;
        }
        private DLMS650InstantaneousEntity GetCumulativeProgrammingCount(DLMS650InstantaneousEntity dlms650InstantEntity)
        {
            dlms650InstantEntity.InstantPowerObisCode = "0.0.96.2.0.255";
            dlms650InstantEntity.InstantPowerAttribute = "2";
            dlms650InstantEntity.InstantPowerClassID = "1";
            dlms650InstantEntity.InstantPowerColumnName = "Cumulative Programming Count";
            dlms650InstantEntity.InstantPowerColumnValue =
                                              (FormatParsedValue(ParseValue(includedEntitiesInPacket[24], instantPacket.Tables["Parameter"]
                                              .Rows[24]["Sequence"].ToString()), 0).ToString()) + "*";
            dlms650InstantEntity.InstantPowerDataIndex = 20;
            return dlms650InstantEntity;
        }
        private DLMS650InstantaneousEntity GetBillingDate(DLMS650InstantaneousEntity dlms650InstantEntity)
        {
            dlms650InstantEntity.InstantPowerObisCode = "0.0.0.1.2.255";
            dlms650InstantEntity.InstantPowerAttribute = "2";
            dlms650InstantEntity.InstantPowerClassID = "3";
            dlms650InstantEntity.InstantPowerColumnName = "Billing Date";
            dlms650InstantEntity.InstantPowerColumnValue =
                                              (2000 + Convert.ToInt32(regExpPacketMatches.Groups[58].Value)).ToString() +
                                               regExpPacketMatches.Groups[57].Value + regExpPacketMatches.Groups[56].Value +
                                               regExpPacketMatches.Groups[55].Value + regExpPacketMatches.Groups[54].Value + "99";
            dlms650InstantEntity.InstantPowerDataIndex = 21;
            return dlms650InstantEntity;
        }
        private DLMS650InstantaneousEntity GetCumkW(DLMS650InstantaneousEntity dlms650InstantEntity)
        {
            packetByteIndex = 58;
            dlms650InstantEntity.InstantPowerObisCode = "1.0.1.8.0.255";
            dlms650InstantEntity.InstantPowerAttribute = "2";
            dlms650InstantEntity.InstantPowerClassID = "3";
            dlms650InstantEntity.InstantPowerColumnName = string.Format("Cumulative Energy – {0}",UNIT_ACTIVEENERGY.Substring(1));//kWh
            dlms650InstantEntity.InstantPowerColumnValue =
                                              (ApplyEnergyResolution(FormatParsedValue(ParseValue(includedEntitiesInPacket[30], instantPacket.Tables["Parameter"].
                                              Rows[30]["Sequence"].ToString()), PRECISION_ENERGY, DLMSObjectType.Energy).ToString()) + UNIT_ACTIVEENERGY);
            dlms650InstantEntity.InstantPowerDataIndex = 22;
            return dlms650InstantEntity;
        }
        private DLMS650InstantaneousEntity GetCumkvarhLag(DLMS650InstantaneousEntity dlms650InstantEntity)
        {
            dlms650InstantEntity.InstantPowerObisCode = "1.0.5.8.0.255";
            dlms650InstantEntity.InstantPowerAttribute = "2";
            dlms650InstantEntity.InstantPowerClassID = "3";
            dlms650InstantEntity.InstantPowerColumnName = string.Format("Cumulative Energy – {0} – lag",UNIT_REACTIVEENERGY.Substring(1));//kvarh
            dlms650InstantEntity.InstantPowerColumnValue =
                                              (ApplyEnergyResolution(FormatParsedValue(ParseValue(includedEntitiesInPacket[31], instantPacket.Tables["Parameter"]
                                              .Rows[31]["Sequence"].ToString()), PRECISION_ENERGY, DLMSObjectType.Energy).ToString()) + UNIT_REACTIVEENERGY);
            dlms650InstantEntity.InstantPowerDataIndex = 23;
            return dlms650InstantEntity;
        }
        
        private DLMS650InstantaneousEntity GetCumkvarhLead(DLMS650InstantaneousEntity dlms650InstantEntity)
        {
            dlms650InstantEntity.InstantPowerObisCode = "1.0.8.8.0.255";
            dlms650InstantEntity.InstantPowerAttribute = "2";
            dlms650InstantEntity.InstantPowerClassID = "3";
            dlms650InstantEntity.InstantPowerColumnName = string.Format("Cumulative Energy – {0} – lead",UNIT_REACTIVEENERGY.Substring(1));//kvarh
            dlms650InstantEntity.InstantPowerColumnValue =
                                              (ApplyEnergyResolution(FormatParsedValue(ParseValue(includedEntitiesInPacket[32], instantPacket.Tables["Parameter"]
                                              .Rows[32]["Sequence"].ToString()), PRECISION_ENERGY, DLMSObjectType.Energy).ToString()) + UNIT_REACTIVEENERGY);
            dlms650InstantEntity.InstantPowerDataIndex = 24;
            return dlms650InstantEntity;
        }
        private DLMS650InstantaneousEntity GetCumkVAh(DLMS650InstantaneousEntity dlms650InstantEntity)
        {
            dlms650InstantEntity.InstantPowerObisCode = "1.0.9.8.0.255";
            dlms650InstantEntity.InstantPowerAttribute = "2";
            dlms650InstantEntity.InstantPowerClassID = "3";
            dlms650InstantEntity.InstantPowerColumnName = string.Format("Cumulative Energy – {0}",UNIT_APPARENTENERGY.Substring(1));//kVAh
            dlms650InstantEntity.InstantPowerColumnValue =
                                              (ApplyEnergyResolution(FormatParsedValue(ParseValue(includedEntitiesInPacket[33], "1"), PRECISION_ENERGY,DLMSObjectType.Energy).ToString()) + UNIT_APPARENTENERGY);
            dlms650InstantEntity.InstantPowerDataIndex = 25;
            return dlms650InstantEntity;
        }
        private DLMS650InstantaneousEntity GetMaximumDemandkW(DLMS650InstantaneousEntity dlms650InstantEntity)
        {
            dlms650InstantEntity.InstantPowerObisCode = "1.0.1.6.0.255";
            dlms650InstantEntity.InstantPowerAttribute = "2";
            dlms650InstantEntity.InstantPowerClassID = "4";
            dlms650InstantEntity.InstantPowerColumnName = string.Format("Maximum Demand – {0}",UNIT_DEMANDKW.Substring(1));//kW
            dlms650InstantEntity.InstantPowerColumnValue =
                                              (ApplyDemandResolution(FormatParsedValue(ParseValue(includedEntitiesInPacket[34], instantPacket.Tables["Parameter"]
                                              .Rows[34]["Sequence"].ToString()), PRECISION_ENERGY,DLMSObjectType.Demand).ToString())+UNIT_DEMANDKW);
            dlms650InstantEntity.InstantPowerDataIndex = 26;
            return dlms650InstantEntity;
        }
        private DLMS650InstantaneousEntity GetMaximumDemandkWDateTime(DLMS650InstantaneousEntity dlms650InstantEntity)
        {
            dlms650InstantEntity.InstantPowerObisCode = "1.0.1.6.0.255";
            dlms650InstantEntity.InstantPowerAttribute = "5";
            dlms650InstantEntity.InstantPowerClassID = "4";
            dlms650InstantEntity.InstantPowerColumnName = string.Format("Maximum Demand – {0}W DateTime", (isHTCT) ? "M" : "k");
            dlms650InstantEntity.InstantPowerColumnValue =
                                              (2000 + Convert.ToInt32(regExpPacketMatches.Groups[99].Value)).ToString() +
                                               regExpPacketMatches.Groups[98].Value + regExpPacketMatches.Groups[97].Value +
                                               regExpPacketMatches.Groups[96].Value + regExpPacketMatches.Groups[95].Value + "99";
            dlms650InstantEntity.InstantPowerDataIndex = 27;
            return dlms650InstantEntity;
        }
        private DLMS650InstantaneousEntity GetMaximumDemandkVA(DLMS650InstantaneousEntity dlms650InstantEntity)
        {
            packetByteIndex = 99;
            dlms650InstantEntity.InstantPowerObisCode = "1.0.9.6.0.255";
            dlms650InstantEntity.InstantPowerAttribute = "2";
            dlms650InstantEntity.InstantPowerClassID = "4";
            dlms650InstantEntity.InstantPowerColumnName = string.Format("Maximum Demand – {0}",UNIT_DEMANDKVA.Substring(1));//kVA
            dlms650InstantEntity.InstantPowerColumnValue =
                                              (ApplyDemandResolution(FormatParsedValue(ParseValue(includedEntitiesInPacket[40], instantPacket.Tables["Parameter"]
                                              .Rows[40]["Sequence"].ToString()), PRECISION_ENERGY,DLMSObjectType.Demand).ToString()) + UNIT_DEMANDKVA);
            dlms650InstantEntity.InstantPowerDataIndex = 28;
            return dlms650InstantEntity;
        }
        private DLMS650InstantaneousEntity GetMaximumDemandkVADateTime(DLMS650InstantaneousEntity dlms650InstantEntity)
        {
            dlms650InstantEntity.InstantPowerObisCode = "1.0.9.6.0.255";
            dlms650InstantEntity.InstantPowerAttribute = "5";
            dlms650InstantEntity.InstantPowerClassID = "4";
            dlms650InstantEntity.InstantPowerColumnName = string.Format("Maximum Demand – {0}VA DateTime",(isHTCT)?"M":"k");
            dlms650InstantEntity.InstantPowerColumnValue =
                                              (2000 + Convert.ToInt32(regExpPacketMatches.Groups[108].Value)).ToString() +
                                               regExpPacketMatches.Groups[107].Value + regExpPacketMatches.Groups[106].Value +
                                               regExpPacketMatches.Groups[105].Value + regExpPacketMatches.Groups[104].Value + "99";
            dlms650InstantEntity.InstantPowerDataIndex = 29;
            return dlms650InstantEntity;
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
    }
}
