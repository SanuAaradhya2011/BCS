using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.Framework;
using CAB.Entity;
using CABEntity;
using CAB.Framework.Entity;
using CAB.BLL;
namespace CHANNEL.Formatter
{
    public class ParseFDLMidNightData : ParseFDLData
    {
        string midNightData = string.Empty;
        const string INVALIDVALUE = "00";
        const string INSTANTSECONDS = "99";

        public ParseFDLMidNightData(string midNightData, string fileText, long fileUploadID, long meterDataID, string demandResolution, string energyResolution)
            : base(fileText, fileUploadID, meterDataID)
        {
            this.midNightData = midNightData;
            this.demandResolution = demandResolution;
            this.energyResolution = energyResolution;
        }
        MidNightDataPacketStructure midNightPacket;

        public override FDLFileParseStatuses Parse()
        {
            // To verify the BCC of general data for data authentication.
            if (!VerifyBCC(midNightData))
            {
                SetParsingStatus(rmFDLParse.GetString("BCCMismatchMidNight"));
                return FDLFileParseStatuses.BCCMismatchGeneral;
            }
            try
            {
                //Extract the billing packet structure from xml.
                midNightPacket = (MidNightDataPacketStructure)GetPacketParsingStructure("MidNightDataPacketStructure.xml", typeof(MidNightDataPacketStructure));
                //Set Packet Entries with size.
                SetPacketEntitesToParse(midNightPacket);
                DefineParsingforPacketStructure(midNightPacket.Tables["Parameter"]);
                EntityBase entity;

                List<IEntity> entities = new List<IEntity>();
                int totalDataParametersLength = 2 * packetLength;
                string currentPacket = string.Empty;
                string billingDateTime = string.Empty;
                GetFDLDemandResolution();
                GetFDLEnergyResolution();
                for (int x = 0; x <= midNightData.Length - 5; x += totalDataParametersLength)
                {
                    currentPacket = midNightData.Substring(x, 2 * packetLength);
                    regExpPacketMatches = packetRegex.Match(currentPacket);
                    if (regExpPacketMatches != null && regExpPacketMatches.Groups != null && regExpPacketMatches.Groups.Count > 1)
                    {
                        //if (regExpPacketMatches.Groups[5].Value != INVALIDVALUE && regExpPacketMatches.Groups[4].Value != INVALIDVALUE && regExpPacketMatches.Groups[3].Value != INVALIDVALUE)
                        //{
                        // To fill billing entity with billing parameters value.
                        entity = CreatePacketEntity();
                        if (entity != null)
                        {
                            // added to solve bug 74492
                             /* VBM Modified to solve bug 137312 */
                             SetParsingStatus(rmFDLParse.GetString("MidnightStatus"));
                             /* VBM Modified to solve bug 137312 */
                            if (CheckDate(((DLMS650MidnightDataEntity)entity).RealTimeClockDateandTime.ToString()))
                            {
                                entities.Add(entity);
                            }
                        }
                        //}
                    }

                }


                if (entities.Count > 0)//Save the parsed tamper data in DB.
                {
                    int PointerLocation = GetCurrentPointerValue(midNightData);
                    List<IEntity> sortedEntity = ApplySorting(entities.Count, PointerLocation, entities);

                    //////Sort the entities with billing date time and assigning the dataindex to get the billing history.
                    //entities.Sort(delegate(IEntity beforeEntity, IEntity afterEntity)
                    //{
                    //    return ((DLMS650MidnightDataEntity)beforeEntity).RealTimeClockDateandTime.CompareTo(((DLMS650MidnightDataEntity)afterEntity).RealTimeClockDateandTime);
                    //});  

                    List<IEntity> reversedEntities = new List<IEntity>();
                    int j = 1;
                    for (int i = sortedEntity.Count - 1; i >= 0; i--, j++)
                    {
                        DLMS650MidnightDataEntity iEntity = (DLMS650MidnightDataEntity)sortedEntity[i];
                        reversedEntities.Add(iEntity);
                    }

                    new DLMS650MidnightDataBLL().InsertData(reversedEntities);
                }

                return FDLFileParseStatuses.None;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return FDLFileParseStatuses.None;
        }

        private int GetCurrentPointerValue(string billingData)
        {
            return Convert.ToInt32(billingData.Substring(billingData.Length - 4, 2), 16);
        }

        private bool CheckDate(string dateTime)
        {
            bool validDate = false;
            try
            {
                if (!string.IsNullOrEmpty(dateTime))
                {
                    if (dateTime.Length >= 8)
                    {
                        if (Convert.ToInt32(dateTime.Substring(0, 4)) >= DateTime.MinValue.Year && Convert.ToInt32(dateTime.Substring(4, 2)) >= DateTime.MinValue.Month && Convert.ToInt32(dateTime.Substring(6, 2)) >= DateTime.MinValue.Day)
                        {
                            validDate = true;
                        }
                    }
                    else
                    {
                        validDate = false;
                    }
                }
            }
            catch (Exception ex)
            {
                validDate = false;
            }
            return validDate;
        }
        protected override EntityBase CreatePacketEntity()
        {
            DLMS650MidnightDataEntity midNightEntity = new DLMS650MidnightDataEntity();
            try
            {
                packetByteIndex = 0;
                midNightEntity.MeterData_ID = meterDataID;
                GetCumkW(midNightEntity);
                GetCumkvarhLag(midNightEntity);
                GetCumkvarhLead(midNightEntity);
                GetCumkVAh(midNightEntity);
                GetDate(midNightEntity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return midNightEntity;

        }


        private DLMS650MidnightDataEntity GetCumkW(DLMS650MidnightDataEntity dlms650MidNightEntity)
        {
            dlms650MidNightEntity.CumEnergykWh =
                (ApplyEnergyResolution(FormatParsedValue(ParseValue(includedEntitiesInPacket[0], midNightPacket.Tables["Parameter"].
                Rows[0]["Sequence"].ToString()), PRECISION_ENERGY, DLMSObjectType.Energy).ToString()) + UNIT_ACTIVEENERGY);
                                                     
            return dlms650MidNightEntity;
        }

        private DLMS650MidnightDataEntity GetCumkvarhLag(DLMS650MidnightDataEntity dlms650MidNightEntity)
        {
            dlms650MidNightEntity.CumEnergykvarhlag =
                                              (ApplyEnergyResolution(FormatParsedValue(ParseValue(includedEntitiesInPacket[1], midNightPacket.Tables["Parameter"]
                                              .Rows[1]["Sequence"].ToString()), PRECISION_ENERGY, DLMSObjectType.Energy).ToString()) + UNIT_REACTIVEENERGY);
            return dlms650MidNightEntity;
        }

        private DLMS650MidnightDataEntity GetCumkvarhLead(DLMS650MidnightDataEntity dlms650MidNightEntity)
        {
            dlms650MidNightEntity.CumEnergykvarhlead =
                                              (ApplyEnergyResolution(FormatParsedValue(ParseValue(includedEntitiesInPacket[2], midNightPacket.Tables["Parameter"]
                                              .Rows[2]["Sequence"].ToString()), PRECISION_ENERGY, DLMSObjectType.Energy).ToString()) + UNIT_REACTIVEENERGY);

            return dlms650MidNightEntity;
        }

        private DLMS650MidnightDataEntity GetCumkVAh(DLMS650MidnightDataEntity dlms650MidNightEntity)
        {
            dlms650MidNightEntity.CumEnergykVAh =
                                             (ApplyEnergyResolution(FormatParsedValue(ParseValue(includedEntitiesInPacket[3], midNightPacket.Tables["Parameter"]
                                              .Rows[3]["Sequence"].ToString()), PRECISION_ENERGY, DLMSObjectType.Energy).ToString()) + UNIT_APPARENTENERGY);
            return dlms650MidNightEntity;
        }
        private DLMS650MidnightDataEntity GetDate(DLMS650MidnightDataEntity dlms650MidNightEntity)
        {

            dlms650MidNightEntity.RealTimeClockDateandTime = Convert.ToInt64((2000 + Convert.ToInt32(regExpPacketMatches.Groups[27].Value)).ToString() +
                                               regExpPacketMatches.Groups[26].Value + regExpPacketMatches.Groups[25].Value);

            return dlms650MidNightEntity;
        }
    }
}
