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
    public class ParseFDLBillingData : ParseFDLData
    {
        BillingPacketStructure billingPacket;
        string billingData = string.Empty;
        //Match regExpPacketMatches;
        const string BILLINGSECONDS = "99";
        const string INVALIDVALUE = "00";
        public ParseFDLBillingData(string billingData, string fileText, long fileUploadID, long meterDataID, string demandResolution, string energyResolution)
            : base(fileText, fileUploadID,meterDataID)
        {
            this.billingData = billingData;
            this.demandResolution = demandResolution;
            this.energyResolution = energyResolution;
        }
        /// <summary>
        /// Created by : Swati Chaudhary
        /// Date : 29 Feb 2012
        /// Purpose: This method parse billing data and call method CreatePacketentity() to fill the billing Entity and save billing data into db.
        /// </summary>
        public override FDLFileParseStatuses Parse()
        {
            if (!VerifyBCC(billingData))
            {
                SetParsingStatus(rmFDLParse.GetString("BCCMismatchTamper"));
                return FDLFileParseStatuses.BCCMismatchTamper;
            }
            try
            {
                //Extract the billing packet structure from xml.
                billingPacket = (BillingPacketStructure)GetPacketParsingStructure("BillingDataPacketStructure.xml", typeof(BillingPacketStructure));
                //Set Packet Entries with size.
                SetPacketEntitesToParse(billingPacket);
                EntityBase entity;
                DefineParsingforPacketStructure(billingPacket.Tables["Parameter"]);
                List<IEntity> entities = new List<IEntity>();
                int totalDataParametersLength = 2 * packetLength;
                string currentPacket = string.Empty;
                string billingDateTime = string.Empty;
                GetFDLDemandResolution();
                GetFDLEnergyResolution();
                
                //Code changed billingData.Length -3 to billingData.Length -5 in order to omit extra byte for Data Pointer//Subhash 
                for (int x = 0; x <= billingData.Length - 5; x += totalDataParametersLength)
                {
                    // 
                    currentPacket = billingData.Substring(x, 2 * packetLength);
                    regExpPacketMatches = packetRegex.Match(currentPacket);
                    if (regExpPacketMatches != null && regExpPacketMatches.Groups != null && regExpPacketMatches.Groups.Count > 1)
                    {
                        if (regExpPacketMatches.Groups[5].Value != INVALIDVALUE && regExpPacketMatches.Groups[4].Value != INVALIDVALUE && regExpPacketMatches.Groups[3].Value != INVALIDVALUE)
                        {
                            // To fill billing entity with billing parameters value.
                            entity = CreatePacketEntity();
                            if (entity != null)
                            {
                                // added to solve bug 74492
                                SetParsingStatus(rmFDLParse.GetString("BillingStatus"));
                                entities.Add(entity);
                                
                            }
                        }
                    }

                }
                
                
                if (entities.Count > 0)//Save the parsed tamper data in DB.
                {
                    int PointerLocation = GetCurrentPointerValue(billingData);
                    List<IEntity> sortedEntity = ApplySorting(entities.Count, PointerLocation, entities);

                    List<IEntity> reversedEntities = new List<IEntity>();
                    //Sort the entities w     ith billing date time and assigning the dataindex to get the billing history.
                    //entities.Sort(delegate(IEntity beforeEntity, IEntity afterEntity)
                    //{
                    //    return DateUtility.DateTimeToLong(((DLMS650BillingEntity)beforeEntity).BillingDateTime).CompareTo(DateUtility.DateTimeToLong(((DLMS650BillingEntity)afterEntity).BillingDateTime));
                    //}); 
                    int j = 1;
                    for (int i = sortedEntity.Count - 1; i >= 0; i--, j++)
                    {
                        DLMS650BillingEntity iEntity = (DLMS650BillingEntity)sortedEntity[i];
                        iEntity.DataIndex = j;
                        reversedEntities.Add(iEntity);
                    }
                    new DLMS650BillingBLL().InsertData(reversedEntities);
                }

                return FDLFileParseStatuses.None; 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private int GetCurrentPointerValue(string billingData)
        {
           return Convert.ToInt32(billingData.Substring(billingData.Length - 4, 2),16);
        }


        
        /// <summary>
        /// Created by : Swati Chaudhary
        /// Date : 29 Feb 2012
        /// Purpose: This method fills the billing Entity with billing parameters values.
        /// </summary>
        /// <param name="billingIndex"></param>
        /// <returns>billingEntity</returns>
        protected override EntityBase CreatePacketEntity()
        {
            DLMS650BillingEntity billingEntity = new DLMS650BillingEntity();
            try
            {
                packetByteIndex = 5;
                int billingYear = 2000 + Convert.ToInt32(regExpPacketMatches.Groups[5].Value);
                int billingMonth = Convert.ToInt32(regExpPacketMatches.Groups[4].Value);
                int billingDay = Convert.ToInt32(regExpPacketMatches.Groups[3].Value);
                int billingHour =  Convert.ToInt32(regExpPacketMatches.Groups[2].Value);
                int billingMinutes = Convert.ToInt32(regExpPacketMatches.Groups[1].Value);
                billingEntity.BillingDateTime = new DateTime(billingYear, billingMonth, billingDay, billingHour, billingMinutes, 0);
                billingEntity.BillingDate = Convert.ToInt64(
                                              (2000 + Convert.ToInt32(regExpPacketMatches.Groups[5].Value)).ToString() +
                                               regExpPacketMatches.Groups[4].Value + regExpPacketMatches.Groups[3].Value +
                                               regExpPacketMatches.Groups[2].Value + regExpPacketMatches.Groups[1].Value + BILLINGSECONDS);
                
                billingEntity.MeterData_ID = meterDataID;
                billingEntity.SystemPowerFactorforBillingPeriod = FormatParsedValue(ParseValue(includedEntitiesInPacket[5], billingPacket.Tables["Parameter"].Rows[5]["Sequence"].ToString()), 2).ToString("0.00")+"*"; ;
                FillCumkWEnergy(billingEntity);
                FillMDkWValues(billingEntity);
                //FillMDkWDateTimeValues(billingEntity);
                FillMDkVAValues(billingEntity);
               // FillMDkVADateTimeValues(billingEntity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return billingEntity;   

        }
        /// <summary>
        /// Created by : Swati Chaudhary
        /// Date : 29 Feb 2012
        /// Purpose: This method fills the billing Entity with Cumulative Energy parameters values.
        /// </summary>
        /// <param name="billingEntity"></param>
        /// <returns>billingEntity</returns>
        private EntityBase FillCumkWEnergy(DLMS650BillingEntity billingEntity)
        {
            try
            {
                // Displaying value in 2 decimal places to solve bug 72932
                billingEntity.CumulativeEnergykWhTZ0 = ApplyEnergyResolution(FormatParsedValue(ParseValue(includedEntitiesInPacket[6], billingPacket.Tables["Parameter"].Rows[6]["Sequence"].ToString()), 6,DLMSObjectType.Energy).ToString())+UNIT_ACTIVEENERGY;
                billingEntity.CumulativeEnergykWhTZ1 = ApplyEnergyResolution(FormatParsedValue(ParseValue(includedEntitiesInPacket[7], billingPacket.Tables["Parameter"].Rows[7]["Sequence"].ToString()), 6,DLMSObjectType.Energy).ToString()) + UNIT_ACTIVEENERGY;
                billingEntity.CumulativeEnergykWhTZ2 = ApplyEnergyResolution(FormatParsedValue(ParseValue(includedEntitiesInPacket[8], billingPacket.Tables["Parameter"].Rows[8]["Sequence"].ToString()), 6, DLMSObjectType.Energy).ToString()) + UNIT_ACTIVEENERGY;
                billingEntity.CumulativeEnergykWhTZ3 = ApplyEnergyResolution(FormatParsedValue(ParseValue(includedEntitiesInPacket[9], billingPacket.Tables["Parameter"].Rows[9]["Sequence"].ToString()), 6, DLMSObjectType.Energy).ToString()) + UNIT_ACTIVEENERGY;
                billingEntity.CumulativeEnergykWhTZ4 = ApplyEnergyResolution(FormatParsedValue(ParseValue(includedEntitiesInPacket[10], billingPacket.Tables["Parameter"].Rows[10]["Sequence"].ToString()), 6, DLMSObjectType.Energy).ToString()) + UNIT_ACTIVEENERGY;
                billingEntity.CumulativeEnergykWhTZ5 = ApplyEnergyResolution(FormatParsedValue(ParseValue(includedEntitiesInPacket[11], billingPacket.Tables["Parameter"].Rows[11]["Sequence"].ToString()), 6, DLMSObjectType.Energy).ToString()) + UNIT_ACTIVEENERGY;
                billingEntity.CumulativeEnergykWhTZ6 = ApplyEnergyResolution(FormatParsedValue(ParseValue(includedEntitiesInPacket[12], billingPacket.Tables["Parameter"].Rows[12]["Sequence"].ToString()), 6, DLMSObjectType.Energy).ToString()) + UNIT_ACTIVEENERGY;
                billingEntity.CumulativeEnergykWhTZ7 = ApplyEnergyResolution(FormatParsedValue(ParseValue(includedEntitiesInPacket[13], billingPacket.Tables["Parameter"].Rows[13]["Sequence"].ToString()), 6, DLMSObjectType.Energy).ToString()) + UNIT_ACTIVEENERGY;
                billingEntity.CumulativeEnergykWhTZ8 = ApplyEnergyResolution(FormatParsedValue(ParseValue(includedEntitiesInPacket[14], billingPacket.Tables["Parameter"].Rows[14]["Sequence"].ToString()), 6, DLMSObjectType.Energy).ToString()) + UNIT_ACTIVEENERGY;
                billingEntity.CumulativeEnergykvarhLag = ApplyEnergyResolution(FormatParsedValue(ParseValue(includedEntitiesInPacket[15], billingPacket.Tables["Parameter"].Rows[15]["Sequence"].ToString()), 6, DLMSObjectType.Energy).ToString()) + UNIT_REACTIVEENERGY;
                billingEntity.CumulativeEnergykvarhLead = ApplyEnergyResolution(FormatParsedValue(ParseValue(includedEntitiesInPacket[16], billingPacket.Tables["Parameter"].Rows[16]["Sequence"].ToString()), 6, DLMSObjectType.Energy).ToString()) + UNIT_REACTIVEENERGY;
                billingEntity.CumulativeEnergykVAhTZ0 = ApplyEnergyResolution(FormatParsedValue(ParseValue(includedEntitiesInPacket[17], billingPacket.Tables["Parameter"].Rows[17]["Sequence"].ToString()), 6, DLMSObjectType.Energy).ToString()) + UNIT_APPARENTENERGY;
                billingEntity.CumulativeEnergykVAhTZ1 = ApplyEnergyResolution(FormatParsedValue(ParseValue(includedEntitiesInPacket[18], billingPacket.Tables["Parameter"].Rows[18]["Sequence"].ToString()), 6, DLMSObjectType.Energy).ToString()) + UNIT_APPARENTENERGY;
                billingEntity.CumulativeEnergykVAhTZ2 = ApplyEnergyResolution(FormatParsedValue(ParseValue(includedEntitiesInPacket[19], billingPacket.Tables["Parameter"].Rows[19]["Sequence"].ToString()), 6, DLMSObjectType.Energy).ToString()) + UNIT_APPARENTENERGY;
                billingEntity.CumulativeEnergykVAhTZ3 = ApplyEnergyResolution(FormatParsedValue(ParseValue(includedEntitiesInPacket[20], billingPacket.Tables["Parameter"].Rows[20]["Sequence"].ToString()), 6, DLMSObjectType.Energy).ToString()) + UNIT_APPARENTENERGY;
                billingEntity.CumulativeEnergykVAhTZ4 = ApplyEnergyResolution(FormatParsedValue(ParseValue(includedEntitiesInPacket[21], billingPacket.Tables["Parameter"].Rows[21]["Sequence"].ToString()), 6, DLMSObjectType.Energy).ToString()) + UNIT_APPARENTENERGY;
                billingEntity.CumulativeEnergykVAhTZ5 = ApplyEnergyResolution(FormatParsedValue(ParseValue(includedEntitiesInPacket[22], billingPacket.Tables["Parameter"].Rows[22]["Sequence"].ToString()), 6, DLMSObjectType.Energy).ToString()) + UNIT_APPARENTENERGY;
                billingEntity.CumulativeEnergykVAhTZ6 = ApplyEnergyResolution(FormatParsedValue(ParseValue(includedEntitiesInPacket[23], billingPacket.Tables["Parameter"].Rows[23]["Sequence"].ToString()), 6, DLMSObjectType.Energy).ToString()) + UNIT_APPARENTENERGY;
                billingEntity.CumulativeEnergykVAhTZ7 = ApplyEnergyResolution(FormatParsedValue(ParseValue(includedEntitiesInPacket[24], billingPacket.Tables["Parameter"].Rows[24]["Sequence"].ToString()), 6, DLMSObjectType.Energy).ToString()) + UNIT_APPARENTENERGY;
                billingEntity.CumulativeEnergykVAhTZ8 = ApplyEnergyResolution(FormatParsedValue(ParseValue(includedEntitiesInPacket[25], billingPacket.Tables["Parameter"].Rows[25]["Sequence"].ToString()), 6, DLMSObjectType.Energy).ToString()) + UNIT_APPARENTENERGY;
            }
            catch (Exception ex)
            {
                throw ex; 
            }
            return billingEntity;
        }
        /// <summary>
        /// Created by : Swati Chaudhary
        /// Date : 29 Feb 2012
        /// Purpose: This method fills the billing Entity with Maximum demand kW parameters values.
        /// </summary>
        /// <param name="billingEntity"></param>
        /// <returns>billingEntity</returns>
        private EntityBase FillMDkWValues(DLMS650BillingEntity billingEntity)
        {
            try
            {
                // Displaying value in 2 decimal places to solve bug 72932
                billingEntity.MDkWTZ0 = ApplyDemandResolution(FormatParsedValue(ParseValue(includedEntitiesInPacket[26], billingPacket.Tables["Parameter"].Rows[26]["Sequence"].ToString()), 6,DLMSObjectType.Demand).ToString())+UNIT_DEMANDKW;
                billingEntity.MDkWDateTimeTZ0 = ParseDateTime();
                billingEntity.MDkWTZ1 = ApplyDemandResolution(FormatParsedValue(ParseValue(includedEntitiesInPacket[32], billingPacket.Tables["Parameter"].Rows[32]["Sequence"].ToString()), 6, DLMSObjectType.Demand).ToString()) + UNIT_DEMANDKW;
                billingEntity.MDkWDateTimeTZ1 = ParseDateTime();
                billingEntity.MDkWTZ2 = ApplyDemandResolution(FormatParsedValue(ParseValue(includedEntitiesInPacket[38], billingPacket.Tables["Parameter"].Rows[38]["Sequence"].ToString()), 6, DLMSObjectType.Demand).ToString()) + UNIT_DEMANDKW;
                billingEntity.MDkWDateTimeTZ2 = ParseDateTime();
                billingEntity.MDkWTZ3 = ApplyDemandResolution(FormatParsedValue(ParseValue(includedEntitiesInPacket[44], billingPacket.Tables["Parameter"].Rows[44]["Sequence"].ToString()), 6, DLMSObjectType.Demand).ToString()) + UNIT_DEMANDKW;
                billingEntity.MDkWDateTimeTZ3 = ParseDateTime();
                billingEntity.MDkWTZ4 = ApplyDemandResolution(FormatParsedValue(ParseValue(includedEntitiesInPacket[50], billingPacket.Tables["Parameter"].Rows[50]["Sequence"].ToString()), 6, DLMSObjectType.Demand).ToString()) + UNIT_DEMANDKW;
                billingEntity.MDkWDateTimeTZ4 = ParseDateTime();
                billingEntity.MDkWTZ5 = ApplyDemandResolution(FormatParsedValue(ParseValue(includedEntitiesInPacket[56], billingPacket.Tables["Parameter"].Rows[56]["Sequence"].ToString()), 6, DLMSObjectType.Demand).ToString()) + UNIT_DEMANDKW;
                billingEntity.MDkWDateTimeTZ5 = ParseDateTime();
                billingEntity.MDkWTZ6 = ApplyDemandResolution(FormatParsedValue(ParseValue(includedEntitiesInPacket[62], billingPacket.Tables["Parameter"].Rows[62]["Sequence"].ToString()), 6, DLMSObjectType.Demand).ToString()) + UNIT_DEMANDKW;
                billingEntity.MDkWDateTimeTZ6 = ParseDateTime();
                billingEntity.MDkWTZ7 = ApplyDemandResolution(FormatParsedValue(ParseValue(includedEntitiesInPacket[68], billingPacket.Tables["Parameter"].Rows[68]["Sequence"].ToString()), 6, DLMSObjectType.Demand).ToString()) + UNIT_DEMANDKW;
                billingEntity.MDkWDateTimeTZ7 = ParseDateTime();
                billingEntity.MDkWTZ8 = ApplyDemandResolution(FormatParsedValue(ParseValue(includedEntitiesInPacket[74], billingPacket.Tables["Parameter"].Rows[74]["Sequence"].ToString()), 6, DLMSObjectType.Demand).ToString()) + UNIT_DEMANDKW;
                billingEntity.MDkWDateTimeTZ8 = ParseDateTime();
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return billingEntity;
        }
        /// <summary>
        /// Created by : Swati Chaudhary
        /// Date : 29 Feb 2012
        /// Purpose: This method fills the billing Entity with Maximum demand kVA parameters values.
        /// </summary>
        /// <param name="billingEntity"></param>
        /// <returns>billingEntity</returns>
        private EntityBase FillMDkVAValues(DLMS650BillingEntity billingEntity)
        {
            try
            {
                // Displaying value in 2 decimal places to solve bug 72932
                billingEntity.MDkVATZ0 = ApplyDemandResolution(FormatParsedValue(ParseValue(includedEntitiesInPacket[80], billingPacket.Tables["Parameter"].Rows[80]["Sequence"].ToString()), 6, DLMSObjectType.Demand).ToString()) + UNIT_DEMANDKVA;
                billingEntity.MDkVADateTimeTZ0 = ParseDateTime();
                billingEntity.MDkVATZ1 = ApplyDemandResolution(FormatParsedValue(ParseValue(includedEntitiesInPacket[86], billingPacket.Tables["Parameter"].Rows[86]["Sequence"].ToString()), 6, DLMSObjectType.Demand).ToString()) + UNIT_DEMANDKVA;
                billingEntity.MDkVADateTimeTZ1 = ParseDateTime();
                billingEntity.MDkVATZ2 = ApplyDemandResolution(FormatParsedValue(ParseValue(includedEntitiesInPacket[92], billingPacket.Tables["Parameter"].Rows[92]["Sequence"].ToString()), 6, DLMSObjectType.Demand).ToString()) + UNIT_DEMANDKVA;
                billingEntity.MDkVADateTimeTZ2 = ParseDateTime();
                billingEntity.MDkVATZ3 = ApplyDemandResolution(FormatParsedValue(ParseValue(includedEntitiesInPacket[98], billingPacket.Tables["Parameter"].Rows[98]["Sequence"].ToString()), 6, DLMSObjectType.Demand).ToString()) + UNIT_DEMANDKVA;
                billingEntity.MDkVADateTimeTZ3 = ParseDateTime();
                billingEntity.MDkVATZ4 = ApplyDemandResolution(FormatParsedValue(ParseValue(includedEntitiesInPacket[104], billingPacket.Tables["Parameter"].Rows[104]["Sequence"].ToString()), 6, DLMSObjectType.Demand).ToString()) + UNIT_DEMANDKVA;
                billingEntity.MDkVADateTimeTZ4 = ParseDateTime();
                billingEntity.MDkVATZ5 = ApplyDemandResolution(FormatParsedValue(ParseValue(includedEntitiesInPacket[110], billingPacket.Tables["Parameter"].Rows[110]["Sequence"].ToString()), 6, DLMSObjectType.Demand).ToString()) + UNIT_DEMANDKVA;
                billingEntity.MDkVADateTimeTZ5 = ParseDateTime();
                billingEntity.MDkVATZ6 = ApplyDemandResolution(FormatParsedValue(ParseValue(includedEntitiesInPacket[116], billingPacket.Tables["Parameter"].Rows[116]["Sequence"].ToString()), 6, DLMSObjectType.Demand).ToString()) + UNIT_DEMANDKVA;
                billingEntity.MDkVADateTimeTZ6 = ParseDateTime();
                billingEntity.MDkVATZ7 = ApplyDemandResolution(FormatParsedValue(ParseValue(includedEntitiesInPacket[122], billingPacket.Tables["Parameter"].Rows[122]["Sequence"].ToString()), 6, DLMSObjectType.Demand).ToString()) + UNIT_DEMANDKVA;
                billingEntity.MDkVADateTimeTZ7 = ParseDateTime();
                billingEntity.MDkVATZ8 = ApplyDemandResolution(FormatParsedValue(ParseValue(includedEntitiesInPacket[128], billingPacket.Tables["Parameter"].Rows[128]["Sequence"].ToString()), 6, DLMSObjectType.Demand).ToString()) + UNIT_DEMANDKVA;
                billingEntity.MDkVADateTimeTZ8 = ParseDateTime();
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return billingEntity;
        }
    }
}
