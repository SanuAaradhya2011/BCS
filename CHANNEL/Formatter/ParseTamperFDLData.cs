/*
File Name: ParseFDLData.cs
Created By: Vivek Agrawal
Date : 29/Feb/2012
Purpose:Parsing Implementation of Fast Downloaded Tamper Data
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.BLL;
using CAB.Entity;
using System.Data;
using CAB.Framework.Entity;
using CAB.DALC.Data;
using CABEntity;
using CAB.Framework;
namespace CHANNEL.Formatter
{
    #region Parse Tamper FDL Data
    /// <summary>
    /// Created By : Vivek Agrawal
    /// Date : 27/Feb/2012
    /// Purpose : This class is responsible for Parsing 
    /// of Tamper Data.
    /// </summary>
    public class ParseTamperFDLData : ParseFDLData
    {
        TamperPacketStructure tamperPacket;
        string tamperData = string.Empty;
        DataView tamperMasterData;
        const string INVALIDTAMPERID = "00";
        public ParseTamperFDLData(string tamperData, string fileText, long fileUploadID, long meterDataID, string demandResolution, string energyResolution)
            : base(fileText, fileUploadID,meterDataID)
        {
            this.tamperData = tamperData;
            this.demandResolution = demandResolution;
            this.energyResolution = energyResolution;
        }
        #region Parse Tamper Data
        /// <summary>
        /// Created By : Vivek Agrawal
        /// Date : 27/Feb/2012
        /// Purpose :Parse the tamper data and stores it in DB.
        /// </summary>
        public override FDLFileParseStatuses Parse()
        {
            if (!VerifyBCC(tamperData))
            {
                SetParsingStatus(rmFDLParse.GetString("BCCMismatchTamper"));
                return FDLFileParseStatuses.BCCMismatchTamper;
            }
            //Define Parsing of Tamper data Packet .
            tamperPacket = (TamperPacketStructure)GetPacketParsingStructure("TamperDataPacketStructure.xml", typeof(TamperPacketStructure));
            //Set parameters to be parsed in Packet.
            SetPacketEntitesToParse(tamperPacket);
            //Define parsing of a packet.
            DefineParsingforPacketStructure(tamperPacket.Tables["Parameter"]);
            List<IEntity> entities = new List<IEntity>();
            string currentPacket = "";
            //Get Tamper Master data.
            tamperMasterData = new DataView(new TamperTypeMasterDAL().GetAllTamperMasterData().Tables[0]);
            int x = 0;
            packetsParsed = 0;
            /* GKG 139419 18/03/2013*/
            GetEnergyResolution();
            /* GKG 139419 18/03/2013*/
            for (; x <= tamperData.Length - 51; x += (2 * packetLength))
            {
                currentPacket = tamperData.Substring(x, 2 * packetLength);//Current Packet.
                regExpPacketMatches = packetRegex.Match(currentPacket);//Split the packet as per packet structure.
                if (regExpPacketMatches != null && regExpPacketMatches.Groups != null && regExpPacketMatches.Groups.Count > 1)
                {//If the packet has non zero value for tamper code then there is a tamper in the packet.
                    //So create the tamper entity object form the current packet.
                    if (regExpPacketMatches.Groups[6].Value != INVALIDTAMPERID)
                    {
                        /* GKG 139419 18/03/2013*/
                        //GetEnergyResolution();
                        /* GKG 139419 18/03/2013*/
                        entities.Add(CreatePacketEntity());
                    }
                }
            }
            if (entities.Count > 0)//Save the parsed tamper data in DB.
            {
                List<IEntity> sortedEntities = ApplyCompartmentWiseSorting(entities, tamperData);
                sortedEntities.Reverse(0, sortedEntities.Count);
                
                new DLMS650TamperMasterBLL().InsertData(sortedEntities);
            }
            return FDLFileParseStatuses.None; 
        }

        private List<IEntity> ApplyCompartmentWiseSorting(List<IEntity> entities, string tamperData)
        {
            List<IEntity> retEntities = new List<IEntity>();
            int compartmentMaxRecord, compartmentPointer;
            List<IEntity> entitiesInCompartment = new List<IEntity>();
            int pointerLocation = 50;
            //Find Compatment 1 Number of Records
            
            //Find Compartment 1 Entities and sort 
            entitiesInCompartment = GetCompartmentWiseEntities(entities, 1);
            compartmentMaxRecord = GetCompatmentMaxTamper(tamperData,ref pointerLocation);
            compartmentPointer = GetCurrentPointerValue(tamperData,ref pointerLocation);
            retEntities.AddRange(ApplySorting(entitiesInCompartment.Count, compartmentPointer, entitiesInCompartment));

            //Find Compartment 2 Entities and sort 
            entitiesInCompartment = GetCompartmentWiseEntities(entities, 2);
            compartmentMaxRecord = GetCompatmentMaxTamper(tamperData, ref pointerLocation);
            compartmentPointer = GetCurrentPointerValue(tamperData, ref pointerLocation);
            retEntities.AddRange(ApplySorting(entitiesInCompartment.Count, compartmentPointer, entitiesInCompartment));

            //Find Compartment 3 Entities and sort 
            entitiesInCompartment = GetCompartmentWiseEntities(entities, 3);
            compartmentMaxRecord = GetCompatmentMaxTamper(tamperData, ref pointerLocation);
            compartmentPointer = GetCurrentPointerValue(tamperData, ref pointerLocation);
            retEntities.AddRange(ApplySorting(entitiesInCompartment.Count, compartmentPointer, entitiesInCompartment));

            //Find Compartment 4 Entities and sort 
            entitiesInCompartment = GetCompartmentWiseEntities(entities, 4);
            compartmentMaxRecord = GetCompatmentMaxTamper(tamperData, ref pointerLocation);
            compartmentPointer = GetCurrentPointerValue(tamperData, ref pointerLocation);
            retEntities.AddRange(ApplySorting(entitiesInCompartment.Count, compartmentPointer, entitiesInCompartment));

            //Find Compartment 5 Entities and sort 
            entitiesInCompartment = GetCompartmentWiseEntities(entities, 5);
            compartmentMaxRecord = GetCompatmentMaxTamper(tamperData, ref pointerLocation);
            compartmentPointer = GetCurrentPointerValue(tamperData, ref pointerLocation);
            retEntities.AddRange(ApplySorting(entitiesInCompartment.Count, compartmentPointer, entitiesInCompartment));

            //Find Compartment 6 Entities and sort 
            entitiesInCompartment = GetCompartmentWiseEntities(entities, 6);
            compartmentMaxRecord = GetCompatmentMaxTamper(tamperData, ref pointerLocation);
            compartmentPointer = GetCurrentPointerValue(tamperData, ref pointerLocation);
            retEntities.AddRange(ApplySorting(entitiesInCompartment.Count, compartmentPointer, entitiesInCompartment));

            return retEntities;
        }

        private int GetCurrentPointerValue(string billingData,ref int pointerLocation)
        {
            int retValue = Convert.ToInt32(billingData.Substring(billingData.Length - pointerLocation, 4), 16);
            pointerLocation -= 4;
            return retValue;
        }

        private int GetCompatmentMaxTamper(string billingData,ref int location)
        {
            int retValue=  Convert.ToInt32(billingData.Substring(billingData.Length - location, 4), 16);
            location -= 4;
            return retValue;
        }

        private List<IEntity> GetCompartmentWiseEntities(List<IEntity> entities, int compartmentNum)
        {
            List<IEntity> returnEntity = entities.FindAll(delegate(IEntity entity)
            {
                DLMS650TamperEntity temperEntity = entity as DLMS650TamperEntity;
                
                return temperEntity.CompartmentNumber == compartmentNum;
            });
            return returnEntity;
        }
        #endregion


        #region CreatePacketEntity
        /// <summary>
        /// Created By : Vivek Agrawal
        /// Date : 27/Feb/2012
        /// Purpose : Creat Tamper Data Enitity object from tamper data packet.
        /// </summary>
        /// <returns></returns>
        protected override EntityBase CreatePacketEntity()
        {
            DLMS650TamperEntity tamperEntity = new DLMS650TamperEntity();
            packetByteIndex = 6;
            //
            tamperEntity.EventCode = Convert.ToInt32(regExpPacketMatches.Groups[6].Value, 16);
            tamperEntity.DateTimeEvent = Convert.ToInt64(
                                           (2000 + Convert.ToInt32(regExpPacketMatches.Groups[3].Value)).ToString() +
                                            regExpPacketMatches.Groups[2].Value + regExpPacketMatches.Groups[1].Value +
                                            regExpPacketMatches.Groups[4].Value + regExpPacketMatches.Groups[5].Value);

            SetCurrentValuesOnTamperOccurence(tamperEntity);
            SetVoltageValuesOnTamperOccurence(tamperEntity);
            SetPFValuesOnTamperOccurence(tamperEntity);
            //10,11,12
            // To solve tamper data energy value not in precision.
            // Added to display values to 3 decimal places in all energies for bug 74454.
            tamperEntity.CumulativeEnergykWh = FormatParsedValue(ParseValue(includedEntitiesInPacket[15], tamperPacket.Tables["Parameter"].Rows[6]["Sequence"].ToString()), PRECISION_ENERGY,DLMSObjectType.Energy).ToString(energyResolution)+UNIT_ACTIVEENERGY;
            //Set compartment no.
            tamperEntity.CompartmentNumber = MapTamperCompartmentAndEventCode(tamperEntity.EventCode);
            tamperEntity.MeterData_ID = meterDataID;
            packetsParsed++;
            // added to solve bug 74492
            
            SetParsingStatus(rmFDLParse.GetString("TamperPacketsStatus"));
            //SetParsingStatus(rmFDLParse.GetString("TamperPacketsStatus") + packetsParsed.ToString());
            return tamperEntity;
        }

        #endregion

        #region SetCurrentValuesOnTamperOccurence
        /// <summary>
        /// SetCurrentValuesOnTamperOccurence
        /// </summary>
        /// <param name="tamperEntity"></param>
        private void SetCurrentValuesOnTamperOccurence(DLMS650TamperEntity tamperEntity)
        {
            tamperEntity.CurrentIR = FormatParsedValue(ParseValue(includedEntitiesInPacket[6], tamperPacket.Tables["Parameter"].Rows[6]["Sequence"].ToString()), PRECISION_CURRENT,DLMSObjectType.Current).ToString("0.000") + UNIT_CURRENT;
            tamperEntity.CurrentIY = FormatParsedValue(ParseValue(includedEntitiesInPacket[7], tamperPacket.Tables["Parameter"].Rows[7]["Sequence"].ToString()), PRECISION_CURRENT, DLMSObjectType.Current).ToString("0.000") + UNIT_CURRENT;
            tamperEntity.CurrentIB = FormatParsedValue(ParseValue(includedEntitiesInPacket[8], tamperPacket.Tables["Parameter"].Rows[8]["Sequence"].ToString()), PRECISION_CURRENT, DLMSObjectType.Current).ToString("0.000") + UNIT_CURRENT;
        }
        #endregion

        
        #region SetVoltageValuesOnTamperOccurence
        /// <summary>
        /// SetVoltageValuesOnTamperOccurence
        /// </summary>
        /// <param name="tamperEntity"></param>
        private void SetVoltageValuesOnTamperOccurence(DLMS650TamperEntity tamperEntity)
        {
            tamperEntity.VoltageVRN = FormatParsedValue(ParseValue(includedEntitiesInPacket[9], tamperPacket.Tables["Parameter"].Rows[9]["Sequence"].ToString()), PRECISION_VOLTAGE, DLMSObjectType.Voltage).ToString("0.00") + UNIT_VOLTAGE;
            tamperEntity.VoltageVYN = FormatParsedValue(ParseValue(includedEntitiesInPacket[10], tamperPacket.Tables["Parameter"].Rows[10]["Sequence"].ToString()), PRECISION_VOLTAGE, DLMSObjectType.Voltage).ToString("0.00") + UNIT_VOLTAGE;
            tamperEntity.VoltageVBN = FormatParsedValue(ParseValue(includedEntitiesInPacket[11], tamperPacket.Tables["Parameter"].Rows[11]["Sequence"].ToString()), PRECISION_VOLTAGE, DLMSObjectType.Voltage).ToString("0.00") + UNIT_VOLTAGE;

        }
        #endregion

        #region SetPFValuesOnTamperOccurence
        /// <summary>
        /// SetPFValuesOnTamperOccurence
        /// </summary>
        /// <param name="tamperEntity"></param>
        private void SetPFValuesOnTamperOccurence(DLMS650TamperEntity tamperEntity)
        {
            /*GKG 139835 Tamper PF format Issue 20/03/2013 */
            //tamperEntity.PowerFactorRphase = FormatParsedValue(ParseValue(includedEntitiesInPacket[12], tamperPacket.Tables["Parameter"].Rows[12]["Sequence"].ToString()), PRECISION_PF).ToString("0.0000");
            //tamperEntity.PowerFactorYphase = FormatParsedValue(ParseValue(includedEntitiesInPacket[13], tamperPacket.Tables["Parameter"].Rows[13]["Sequence"].ToString()), PRECISION_PF).ToString("0.0000");
            //tamperEntity.PowerFactorBphase = FormatParsedValue(ParseValue(includedEntitiesInPacket[14], tamperPacket.Tables["Parameter"].Rows[14]["Sequence"].ToString()), PRECISION_PF).ToString("0.0000");
            tamperEntity.PowerFactorRphase = FormatParsedValue(ParseValue(includedEntitiesInPacket[12], tamperPacket.Tables["Parameter"].Rows[12]["Sequence"].ToString()), 2).ToString("0.0000");
            tamperEntity.PowerFactorYphase = FormatParsedValue(ParseValue(includedEntitiesInPacket[13], tamperPacket.Tables["Parameter"].Rows[13]["Sequence"].ToString()), 2).ToString("0.0000");
            tamperEntity.PowerFactorBphase = FormatParsedValue(ParseValue(includedEntitiesInPacket[14], tamperPacket.Tables["Parameter"].Rows[14]["Sequence"].ToString()), 2).ToString("0.0000");
            /*GKG 139835 Tamper PF format Issue 20/03/2013 */
        }
        #endregion

        #region MapTamperCompartmentAndEventCode
        /// <summary>
        /// Created By : Vivek Agrawal
        /// Date : 27/Feb/2012
        /// Purpose : Get Compartment value based on tamper code(Event code).
        /// </summary>
        /// <param name="eventCode"></param>
        /// <returns></returns>
        private Int64 MapTamperCompartmentAndEventCode(long eventCode)
        {

            Int64 code = 0;
            tamperMasterData.RowFilter = "TamperTypeID=" + eventCode.ToString();

            if (tamperMasterData.Table.Rows.Count > 0)
            {
                
                code= Convert.ToInt64(tamperMasterData[0]["Compartment"]);
            }

            return code;
        }
        #endregion


    }

    #endregion
}
