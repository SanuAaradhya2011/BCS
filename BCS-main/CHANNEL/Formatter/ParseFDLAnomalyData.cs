using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CABEntity;
using CAB.Framework.Entity;
using CAB.BLL;
using CAB.Entity;

namespace CHANNEL.Formatter
{
    public class ParseFDLAnomalyData : ParseFDLData
    {

        string anomalyData = string.Empty;
        public ParseFDLAnomalyData(string midNightData, string fileText, long fileUploadID, long meterDataID, string demandResolution, string energyResolution)
            : base(fileText, fileUploadID, meterDataID)
        {
            this.anomalyData = midNightData;
            this.demandResolution = demandResolution;
            this.energyResolution = energyResolution;
        }
        AnomalyPacketStructure anomalyPacket;
        public override FDLFileParseStatuses Parse()
        {
            // To verify the BCC of general data for data authentication.
            if (!VerifyBCC(anomalyData))
            {
                SetParsingStatus(rmFDLParse.GetString("BCCMismatchAnomaly"));
                return FDLFileParseStatuses.BCCMismatchGeneral;
            }
            try
            {
                //Extract the billing packet structure from xml.
                anomalyPacket = (AnomalyPacketStructure)GetPacketParsingStructure("AnomalyDataPacketStructure.xml", typeof(AnomalyPacketStructure));
                //Set Packet Entries with size.
                SetPacketEntitesToParse(anomalyPacket);
                DefineParsingforPacketStructure(anomalyPacket.Tables["Parameter"]);
                EntityBase entity;

                int totalDataParametersLength = 2 * packetLength;
                string currentPacket = string.Empty;
                string billingDateTime = string.Empty;
                GetFDLDemandResolution();
                GetFDLEnergyResolution();
                currentPacket = anomalyData.Substring(0, 2 * packetLength);
                regExpPacketMatches = packetRegex.Match(currentPacket);
                if (regExpPacketMatches != null && regExpPacketMatches.Groups != null && regExpPacketMatches.Groups.Count > 1)
                {
                    entity = getAnomalyEnity();
                    if (entity != null)
                    {
                        new AnomalyBLL().InsertData(entity);
                    }
                }
                return FDLFileParseStatuses.None;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return FDLFileParseStatuses.None;
        }
        /// <summary>
        /// VBM - Fill Anomlay entity and return it.
        /// </summary>
        /// <param name="phasorData"></param>
        /// <returns></returns>
        public AnomalyEntity GetAnomalyEntity()
        {
            try
            {
                //Extract the billing packet structure from xml.
                anomalyPacket = (AnomalyPacketStructure)GetPacketParsingStructure("AnomalyDataPacketStructure.xml", typeof(AnomalyPacketStructure));
                //Set Packet Entries with size.
                SetPacketEntitesToParse(anomalyPacket);
                DefineParsingforPacketStructure(anomalyPacket.Tables["Parameter"]);
               AnomalyEntity entity = null;

                int totalDataParametersLength = 2 * packetLength;
                string currentPacket = string.Empty;
                string billingDateTime = string.Empty;
                GetFDLDemandResolution();
                GetFDLEnergyResolution();
                currentPacket = anomalyData.Substring(0, 2 * packetLength);
                regExpPacketMatches = packetRegex.Match(currentPacket);
                if (regExpPacketMatches != null && regExpPacketMatches.Groups != null && regExpPacketMatches.Groups.Count > 1)
                {
                    entity = getAnomalyEnity();
                   
                }
                return entity;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private AnomalyEntity getAnomalyEnity()
        {
            AnomalyEntity entity = new AnomalyEntity();
            packetByteIndex = 0;
            try
            {
                entity.MeterDataId = meterDataID;
                entity.Flash = Convert.ToInt32(ParseValue(includedEntitiesInPacket[0], anomalyPacket.Tables["Parameter"].Rows[0]["Sequence"].ToString()));
                entity.EeProm = Convert.ToInt32(ParseValue(includedEntitiesInPacket[1], anomalyPacket.Tables["Parameter"].Rows[1]["Sequence"].ToString()));
                entity.Smps = Convert.ToInt32(ParseValue(includedEntitiesInPacket[2], anomalyPacket.Tables["Parameter"].Rows[2]["Sequence"].ToString()));
                entity.Rtc = Convert.ToInt32(ParseValue(includedEntitiesInPacket[3], anomalyPacket.Tables["Parameter"].Rows[3]["Sequence"].ToString()));
            }
            catch 
            {
            }
            return entity;
        }
    }
}
