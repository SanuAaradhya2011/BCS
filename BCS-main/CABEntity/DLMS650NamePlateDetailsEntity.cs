/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 	 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| 
 */
using System;
using System.Collections.Generic;
using System.Text;
using CAB.Framework.Entity;

namespace CAB.Entity
{
    public class DLMS650NamePlateDetailsEntity : EntityBase
    {
        private long general_ID;
        private long nameplate_ID;
        private string meterSerialNumber;
        private string manufacturername;
        private string firmwareVersionformeter;
        private string metertype;
        private string internalCTratio;
        private string internalPTratio;
        private string internalVTratio;
        private string category;
        private string meteryearofmanufacture;
        private string energyResolution;
        private string demandResolution;
        private string meterDataType;
        private string metetModelId;
        private long meterData_ID;
        private string metetModelNo;
        private string reverseKWh;

        private string internalFirmwareVersion;
        private string voltageRating;
        private string basicCurrentRating;
        private string currentRating;

        private string netMeterVariantInfo;
        private string MeterConstantInfo;
        private string PrimaryMeterConstantInfo; //PGVCL
        private string MeterClassInfo;
        private string LEDpulserate;


        private string displayProgrammingType;

        private string meterMonthOfManufacture;

        public long General_ID
        {
            get { return general_ID; }
            set { general_ID = value; }
        }

        public long Nameplate_ID
        {
            get { return nameplate_ID; }
            set { nameplate_ID = value; }
        }

        public string MeterSerialNumber
        {
            get { return meterSerialNumber; }
            set { meterSerialNumber = value; }
        }
        public string Manufacturername
        {
            get { return manufacturername; }
            set { manufacturername = value; }
        }
        public string FirmwareVersionformeter
        {
            get { return firmwareVersionformeter; }
            set { firmwareVersionformeter = value; }
        }
        public string Metertype
        {
            get { return metertype; }
            set { metertype = value; }
        }
        public string InternalCTratio
        {
            get { return internalCTratio; }
            set { internalCTratio = value; }
        }
        public string InternalPTratio
        {
            get { return internalPTratio; }
            set { internalPTratio = value; }
        }
        public string InternalVTratio
        {
            get { return internalVTratio; }
            set { internalVTratio = value; }
        }
        public string Category
        {
            get { return category; }
            set { category = value; }
        }
        public string Meteryearofmanufacture
        {
            get { return meteryearofmanufacture; }
            set { meteryearofmanufacture = value; }
        }
        public long MeterData_ID
        {
            get { return meterData_ID; }
            set { meterData_ID = value; }
        }
        public string EnergyResolution
        {
            get { return energyResolution; }
            set { energyResolution = value; }
        }
        public string DemandResolution
        {
            get { return demandResolution; }
            set { demandResolution = value; }
        }
        public string MeterDataType
        {
            get { return meterDataType; }
            set { meterDataType = value; }
        }
        public string MeterModelNo
        {
            get { return metetModelNo; }
            set { metetModelNo = value; }
        }

        public string InternalFirmwareVersion
        {
            get { return internalFirmwareVersion; }
            set { internalFirmwareVersion = value; }
        }
        public string VoltageRating
        {
            get { return voltageRating; }
            set { voltageRating = value; }
        }
        public string BasicCurrentRating
        {
            get { return basicCurrentRating; }
            set { basicCurrentRating = value; }
        }
        public string CurrentRating
        {
            get { return currentRating; }
            set { currentRating = value; }
        }
        public string ReverseKWh
        {
            get { return reverseKWh; }
            set { reverseKWh = value; }
        }
        public string AccuracyClass { get; set; }

        public string NetMeterVariantInfo
        {
            get { return netMeterVariantInfo; }
            set { netMeterVariantInfo = value; }
        }
        public string MeterConstant
        {
            get { return MeterConstantInfo; }
            set { MeterConstantInfo = value; }
        }
        public string DisplayProgrammingType
        {
            get { return displayProgrammingType; }
            set { displayProgrammingType = value; }
        }

        public string MeterMonthOfManufacture
        {
            get { return meterMonthOfManufacture; }
            set { meterMonthOfManufacture = value; }
        }
        public string PrimaryMeterConstant//PGVCL
        {
            get { return PrimaryMeterConstantInfo; }
            set { PrimaryMeterConstantInfo = value; }
        }

        public string MeterClass//CSPDCL
        {
            get { return MeterClassInfo; }
            set { MeterClassInfo = value; }
        }

        public string LEDPulseRate//CSPDCL
        {
            get { return LEDpulserate; }
            set { LEDpulserate = value; }
        }

    }
}
