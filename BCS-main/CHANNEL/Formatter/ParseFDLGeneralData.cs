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
using System.Windows.Forms;
using CAB.Framework;
namespace CHANNEL.Formatter
{
    /// <summary>
    /// This class is used for the purpose of parsing the fastdownloading general data.
    /// </summary>
    public class ParseFDLGeneralData : ParseFDLData
    {
        bool isPUMA = false;
        string generalData = string.Empty;
        NamePlatePacketStructure generalPacket;
       
        const string INVALIDVALUE = "00";
        private const string THREEPHASETHREEWIRE = "3P-3W";
        private const string THREEPHASEFOURWIRE = "3P-4W";
        public ParseFDLGeneralData(string generalData,string fileText,long fileUploadID,long meterDataID)
            : base( fileText, fileUploadID, meterDataID)
        {
            this.generalData = generalData;
            if (UtilityDetails.Utility == UtilityEntity.Generic)
            {
                isPUMA = true; 
            }
        }
        /// <summary>
        /// This method is used to the parse general data. 
        /// </summary>
        /// <returns></returns>
        public override FDLFileParseStatuses Parse(out DLMS650NamePlateDetailsEntity entity)
        {
            // To verify the BCC of general data for data authentication.
            entity = new DLMS650NamePlateDetailsEntity();
            if (!VerifyBCC(generalData))
            {
                SetParsingStatus(rmFDLParse.GetString("BCCMismatchGeneral"));
                return FDLFileParseStatuses.BCCMismatchGeneral;
            }
            try
            {
                //Extract the billing packet structure from xml.
                generalPacket = (NamePlatePacketStructure)GetPacketParsingStructure("NamePlateDataPacketStructure.xml", typeof(NamePlatePacketStructure));
                //Set Packet Entries with size.
                SetPacketEntitesToParse(generalPacket);
                DefineParsingforPacketStructure(generalPacket.Tables["Parameter"]);
                /*GKG Meter ID dyanamic changes*/
                //int totalDataParametersLength = 2 * packetLength;
                //string currentPacket = string.Empty;
                //string billingDateTime = string.Empty;
                //currentPacket = generalData.Substring(0, 2 * packetLength);
                //regExpPacketMatches = packetRegex.Match(currentPacket);

                StringBuilder stringPatternPacket = new StringBuilder();

                for (int j = 0; j < generalData.Length / 2; j++)
                    stringPatternPacket = stringPatternPacket.Append("(.{2})");
                packetRegex = new Regex(stringPatternPacket.ToString());

                regExpPacketMatches = packetRegex.Match(generalData);
                /*GKG Meter ID dyanamic changes*/

                if (regExpPacketMatches != null && regExpPacketMatches.Groups != null && regExpPacketMatches.Groups.Count > 1)
                {
                        entity = CreatePacketEntity();
                        if (entity != null)
                        {
                            new DLMS650GeneralBLL().InsertData(entity);
                        }
                }
             
            
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return FDLFileParseStatuses.None;
        }
        private DLMS650NamePlateDetailsEntity CreatePacketEntity()
        {
            int demandResolution = 0;
            int energyResolution = 0;
            DLMS650NamePlateDetailsEntity namePlateDetails = new DLMS650NamePlateDetailsEntity();
            try
            {
                namePlateDetails.MeterData_ID = meterDataID;
                /*GKG Meter ID dyanamic changes*/

                //namePlateDetails.MeterSerialNumber = ((char)Convert.ToInt32(regExpPacketMatches.Groups[1].Value, 16)).ToString() + ((char)Convert.ToInt32(regExpPacketMatches.Groups[2].Value, 16)).ToString() +
                //    ((char)Convert.ToInt32(regExpPacketMatches.Groups[3].Value, 16)).ToString() + ((char)Convert.ToInt32(regExpPacketMatches.Groups[4].Value, 16)).ToString() +
                //    ((char)Convert.ToInt32(regExpPacketMatches.Groups[5].Value, 16)).ToString() + ((char)Convert.ToInt32(regExpPacketMatches.Groups[6].Value, 16)).ToString() +
                //((char)Convert.ToInt32(regExpPacketMatches.Groups[7].Value, 16)).ToString() + ((char)Convert.ToInt32(regExpPacketMatches.Groups[8].Value, 16)).ToString();
                //namePlateDetails.Manufacturername = ((char)Convert.ToInt32(regExpPacketMatches.Groups[9].Value, 16)).ToString() + ((char)Convert.ToInt32(regExpPacketMatches.Groups[10].Value, 16)).ToString() + ((char)Convert.ToInt32(regExpPacketMatches.Groups[11].Value, 16)).ToString();
                //namePlateDetails.FirmwareVersionformeter = ((char)Convert.ToInt32(regExpPacketMatches.Groups[12].Value, 16)).ToString() + ((char)Convert.ToInt32(regExpPacketMatches.Groups[13].Value, 16)).ToString() + ((char)Convert.ToInt32(regExpPacketMatches.Groups[14].Value, 16)).ToString();
                //namePlateDetails.Metertype = (Convert.ToInt32(regExpPacketMatches.Groups[15].Value, 16).ToString() == "0") ? THREEPHASETHREEWIRE : THREEPHASEFOURWIRE;
                //namePlateDetails.InternalCTratio = Convert.ToInt32(regExpPacketMatches.Groups[17].Value + regExpPacketMatches.Groups[16].Value, 16).ToString();
                //namePlateDetails.Meteryearofmanufacture = Convert.ToInt32(regExpPacketMatches.Groups[19].Value + regExpPacketMatches.Groups[18].Value, 16).ToString();
                //namePlateDetails.InternalPTratio = Convert.ToInt32(regExpPacketMatches.Groups[20].Value, 16).ToString();
                //int.TryParse(namePlateDetails.InternalCTratio, out internalCTRatio);
                //int.TryParse(namePlateDetails.InternalPTratio, out internalPTRatio);
                //namePlateDetails.EnergyResolution = Convert.ToInt32(regExpPacketMatches.Groups[21].Value).ToString();
                //namePlateDetails.DemandResolution = Convert.ToInt32(regExpPacketMatches.Groups[22].Value).ToString();

                int meterIdStartIndex = 1;
                int meterIDLen = 8;
                
                if (Convert.ToInt32(regExpPacketMatches.Groups[1].Value, 16) == 0x80) //0x80
                {
                    meterIdStartIndex = 3;
                    meterIDLen = Convert.ToInt32(regExpPacketMatches.Groups[2].Value, 16);
                }

                int dataByteIndex = meterIdStartIndex;

                for (int dataByte = 0; dataByte < meterIDLen; dataByte++)
                {
                    namePlateDetails.MeterSerialNumber = namePlateDetails.MeterSerialNumber + ((char)Convert.ToInt32(regExpPacketMatches.Groups[dataByte + meterIdStartIndex].Value, 16)).ToString();
                    dataByteIndex++;
                }


                namePlateDetails.Manufacturername = ((char)Convert.ToInt32(regExpPacketMatches.Groups[dataByteIndex++].Value, 16)).ToString() + ((char)Convert.ToInt32(regExpPacketMatches.Groups[dataByteIndex++].Value, 16)).ToString() + ((char)Convert.ToInt32(regExpPacketMatches.Groups[dataByteIndex++].Value, 16)).ToString();
                namePlateDetails.FirmwareVersionformeter = ((char)Convert.ToInt32(regExpPacketMatches.Groups[dataByteIndex++].Value, 16)).ToString() + ((char)Convert.ToInt32(regExpPacketMatches.Groups[dataByteIndex++].Value, 16)).ToString() + ((char)Convert.ToInt32(regExpPacketMatches.Groups[dataByteIndex++].Value, 16)).ToString();
                namePlateDetails.Metertype = (Convert.ToInt32(regExpPacketMatches.Groups[dataByteIndex++].Value, 16).ToString() == "0") ? THREEPHASETHREEWIRE : THREEPHASEFOURWIRE;

                namePlateDetails.InternalCTratio = Convert.ToInt32(regExpPacketMatches.Groups[dataByteIndex+1].Value + regExpPacketMatches.Groups[dataByteIndex].Value, 16).ToString();
                dataByteIndex++;
                dataByteIndex++;

                namePlateDetails.Meteryearofmanufacture = Convert.ToInt32(regExpPacketMatches.Groups[dataByteIndex+1].Value + regExpPacketMatches.Groups[dataByteIndex].Value, 16).ToString();
                dataByteIndex++;
                dataByteIndex++;

                if (UtilityDetails.ShowTwoBytePTRatio)
                {
                    namePlateDetails.InternalPTratio = Convert.ToInt32(regExpPacketMatches.Groups[dataByteIndex + 1].Value + regExpPacketMatches.Groups[dataByteIndex].Value, 16).ToString();
                    dataByteIndex++;
                    dataByteIndex++;
                }
                else
                {
                    namePlateDetails.InternalPTratio = Convert.ToInt32(regExpPacketMatches.Groups[dataByteIndex++].Value, 16).ToString();
                }
                int.TryParse(namePlateDetails.InternalCTratio, out internalCTRatio);
                int.TryParse(namePlateDetails.InternalPTratio, out internalPTRatio);
                namePlateDetails.EnergyResolution = Convert.ToInt32(regExpPacketMatches.Groups[dataByteIndex++].Value).ToString();
                namePlateDetails.DemandResolution = Convert.ToInt32(regExpPacketMatches.Groups[dataByteIndex++].Value).ToString();

                /*GKG Meter ID dyanamic changes*/
                if (int.TryParse(namePlateDetails.EnergyResolution,out energyResolution) && int.TryParse(namePlateDetails.DemandResolution,out demandResolution))
                {
                    if (demandResolution > 4 && energyResolution > 3)
                    {
                        isHTCT = true;
                        UNIT_REACTIVEENERGY = UNIT_REACTIVEENERGYINMEGA;
                        UNIT_DEMANDKW = UNIT_DEMANDMW;
                        UNIT_DEMANDKVAR = UNIT_DEMANDMVAR;
                        UNIT_DEMANDKVA = UNIT_DEMANDMVA;
                        UNIT_APPARENTENERGY = UNIT_APPARENTENERGYINMEGA;
                        UNIT_ACTIVEENERGY = UNIT_ACTIVEENERGYINMEGA;
                        namePlateDetails.MeterDataType = "HTCT";
                    }
                    else
                    {
                        isHTCT = false;
                        namePlateDetails.MeterDataType = "LTCT";
                    }
                }
                return namePlateDetails;
            }
            catch (Exception ex)
            {
                return null;
                throw new Exception("File corrupt");                
            }
          
        }
    }

}
