using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using CAB.DALC.Data.DataServices;
using CAB.Entity;
using CAB.IECFramework;
using CAB.IECFramework.Entity;
using System.Data.Common;

namespace CAB.DALC.Data
{
    public class DBGenerationDAL : DALBase
    {
        private string GetTableCreationQuery()
        {
            StringBuilder sqlQuery = new StringBuilder();
            sqlQuery.Append(@"CREATE SCHEMA IF NOT EXISTS `rubyapp`;");
            sqlQuery.Append(@"USE `rubyapp`;");

            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `LoginMaster` (");
            sqlQuery.Append(@"`LogID` bigint(20) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`UserID` bigint(20),");
            sqlQuery.Append(@"`StartDateTime` bigint(20),");
            sqlQuery.Append(@"`EndDateTime` bigint(20),");
            sqlQuery.Append(@"PRIMARY KEY (`LogID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");

            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `areamaster` (");
            sqlQuery.Append(@"`Area_ID` bigint(20) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`Region_ID` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`Circle_ID` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`Divsion_ID` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`CMRI_ID` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`Area_ID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");


            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `areameter_master` (");
            sqlQuery.Append(@"`AreaMeter_ID` bigint(20) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`Area_ID` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`Meter_ID` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`AreaMeter_ID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");


            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `asciiexportsettings` (");
            sqlQuery.Append(@"`ASCIIExportSettings_ID` bigint(20) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`FileName` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Delimeter` varchar(2) DEFAULT NULL,");
            sqlQuery.Append(@"`GeneralColumn` longtext,");
            sqlQuery.Append(@"`GeneralDBColumn` longtext,");
            sqlQuery.Append(@"`BillingColumn` longtext,");
            sqlQuery.Append(@"`BillingDBColumn` longtext,");
            sqlQuery.Append(@"`TamperColumn` longtext,");
            sqlQuery.Append(@"`TamberDBColumn` longtext,");
            sqlQuery.Append(@"`InstantColumn` longtext,");
            sqlQuery.Append(@"`InstantDBColum` longtext,");
            sqlQuery.Append(@"`LoadSurveyColumn` longtext,");
            sqlQuery.Append(@"`LoadSurveyDBColumn` longtext,");
            sqlQuery.Append(@"PRIMARY KEY (`ASCIIExportSettings_ID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");


            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `categoryright` (");
            sqlQuery.Append(@"`Category_ID` smallint(6) NOT NULL,");
            sqlQuery.Append(@"`Module_ID` smallint(6) NOT NULL,");
            sqlQuery.Append(@"`DefaultRight` smallint(6) NOT NULL");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");


            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `category_master` (");
            sqlQuery.Append(@"`Category_ID` int(11) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`Category_Name` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`Category_ID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");

            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `circle_master` (");
            sqlQuery.Append(@"`Circle_ID` bigint(20) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`Circle_Name` varchar(50) NOT NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`Circle_ID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");


            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `cmri_master` (");
            sqlQuery.Append(@"`CMRI_ID` bigint(20) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`CMRI_Number` varchar(16) NOT NULL,");
            sqlQuery.Append(@"`CMRIType` varchar(50) NOT NULL,");
            sqlQuery.Append(@"`CMRI_Description` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`CMRI_ID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");


            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `consumerexportsettings` (");
            sqlQuery.Append(@"`ConsumerExportSettings_ID` bigint(20) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`FileName` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`ParametersName` varchar(1000) DEFAULT NULL,");
            sqlQuery.Append(@"`ParameterColumn` varchar(1500) DEFAULT NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`ConsumerExportSettings_ID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");


            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `consumermeter` (");
            sqlQuery.Append(@"`ConsumerMeter_ID` int(11) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`Meter_ID` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`Consumer_Number` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`Meter_AllocationDate` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`Meter_Location` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`Status` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`ConsumerMeter_ID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");


            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `consumertype_master` (");
            sqlQuery.Append(@"`ConsumerType_ID` bigint(20) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`ConsumerType_Name` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`ConsumerType_ID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");


            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `consumer_master` (");
            sqlQuery.Append(@"`Consumer_Number` varchar(20) NOT NULL,");
            sqlQuery.Append(@"`Consumer_Name` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`ConsumerType_ID` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"`Consumer_Phone` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`Consumer_HNumber` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`Consumer_Street` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`Consumer_City` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`Consumer_Email` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`Consumer_Number`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");


            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `designation_master` (");
            sqlQuery.Append(@"`Designation_ID` int(11) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`Designation_Name` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`Designation_ID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");


            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `division_master` (");
            sqlQuery.Append(@"`Division_ID` bigint(20) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`Division_Name` varchar(50) NOT NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`Division_ID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");


            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `dtmdailyprofileparameter` (");
            sqlQuery.Append(@"`ColumnsNames` varchar(800) DEFAULT NULL,");
            sqlQuery.Append(@"`MeterData_ID` bigint(20) DEFAULT NULL");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");


            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `exceptionlog` (");
            sqlQuery.Append(@"`Log_ID` bigint(20) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`Log_Date` bigint(20) NOT NULL,");
            sqlQuery.Append(@"`Log_Source` text,");
            sqlQuery.Append(@"`Log_Message` text,");
            sqlQuery.Append(@"`Log_MacID` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`UserInformation_ID` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"`Log_Exception` longblob,");
            sqlQuery.Append(@"PRIMARY KEY (`Log_ID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");

            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `fileupload_master` (");
            sqlQuery.Append(@"`FileUpload_ID` bigint(20) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`UploadingDateTime` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`FileContent` longblob,");
            sqlQuery.Append(@"`UserInformation_ID` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"`FileName` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`readingDateTime` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`FileType` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`FileUpload_ID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");

            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `group_master` (");
            sqlQuery.Append(@"`Group_ID` int(11) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`Group_Name` varchar(35) NOT NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`Group_ID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");


            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `gsmgroupschedule` (");
            sqlQuery.Append(@"`GSMGroupSchedule_ID` bigint(20) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`Group_Name` varchar(80) DEFAULT NULL,");
            sqlQuery.Append(@"`StartReadingDate` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`GSMSchedule_ID` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`Meter_ID` text,");
            sqlQuery.Append(@"PRIMARY KEY (`GSMGroupSchedule_ID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");


            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `gsmreadingstatus` (");
            sqlQuery.Append(@"`GSMReadingStatus_ID` bigint(20) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`ReadingDateTime` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`StatusMessage` varchar(100) DEFAULT NULL,");
            sqlQuery.Append(@"`FileName` varchar(100) DEFAULT NULL,");
            sqlQuery.Append(@"`FilePath` varchar(150) DEFAULT NULL,");
            sqlQuery.Append(@"`Status` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`GSMReadingStatus_ID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");


            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `gsmschedule` (");
            sqlQuery.Append(@"`gsmSchedule_ID` bigint(20) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`Schedule_Name` varchar(80) DEFAULT NULL,");
            sqlQuery.Append(@"`Schedule_Period` varchar(2) DEFAULT NULL,");
            sqlQuery.Append(@"`Period_DayName` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`Period_DayNumber` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"`CreationDate` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`CreationTime` varchar(10) DEFAULT NULL,");
            sqlQuery.Append(@"`Schedule_Parameter` varchar(200) DEFAULT NULL,");
            sqlQuery.Append(@"`Status` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`gsmSchedule_ID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");


            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `history_master` (");
            sqlQuery.Append(@"`History_ID` bigint(20) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`History_Name` varchar(10) DEFAULT NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`History_ID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");


            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `loadsurveyparameter` (");
            sqlQuery.Append(@"`ColumnsNames` varchar(1000) DEFAULT NULL,");
            sqlQuery.Append(@"`MeterData_ID` bigint(20) DEFAULT NULL");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");


            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `meterdata` (");
            sqlQuery.Append(@"`MeterData_ID` bigint(20) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`FileUpload_ID` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`MeterID` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`ReadingDateTime` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`UploadingDateTime` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`CMRIID` varchar(10) DEFAULT NULL,");
            sqlQuery.Append(@"`CMRIType` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`MeterData_ID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");


            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `meterdata_billing` (");
            sqlQuery.Append(@"`Billing_ID` bigint(20) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`BillingResetType` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`CumulativeEnergyKWH` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`CumulativeEnergyKVARHLag` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`CumulativeEnergyKVARHLead` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`CumulativeEnergyKVAH` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`CumulativeExportEnergyKWH` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`CumulativeExportEnergyKVAH` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`CumulativeMD1` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`CumulativeMD1TimeStamp` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`CumulativeMD2` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`CumulativeMD2TimeStamp` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`CumulativeMD3` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`CumulativeMD3TimeStamp` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`AveragePowerFactor` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`PowerOnHours` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`LoadFactor` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`MeterData_ID` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`History_ID` smallint(6) DEFAULT NULL,");
            sqlQuery.Append(@"`CTRatio` varchar(10) DEFAULT NULL,");
            sqlQuery.Append(@"`KwhLag` varchar(10) DEFAULT NULL,");
            sqlQuery.Append(@"`KwhLead` varchar(10) DEFAULT NULL,");
            sqlQuery.Append(@"`KvahLag` varchar(10) DEFAULT NULL,");
            sqlQuery.Append(@"`KvahLead` varchar(10) DEFAULT NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`Billing_ID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");

            


            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `meterdata_dtmdailyprofile` (");
            sqlQuery.Append(@"`DTMDailyProfile_ID` bigint(20) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`DailyProfileDate` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`CumulativeFundamentalKwh` varchar(45) DEFAULT NULL,");
            sqlQuery.Append(@"`CumulativekWh` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`CumulativekVArh_lag` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`CumulativekVArh_lead` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`CumulativekVAh` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`DailyMD1` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`MD1TimeStamp` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`DailyMD2` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`MD2TimeStamp` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`DailyMD3` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`MD3TimeStamp` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`MaxAvgVoltage` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`MinAvgVoltage` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`MaxAvgCurrent` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`MinAvgCurrent` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`AvailableDays` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`MaximumDays` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`MeterData_ID` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`PowerOnHours` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`DTMDailyProfile_ID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");


            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `meterdata_dtmloadsurvey` (");
            sqlQuery.Append(@"`DTMLoadSurvey_ID` bigint(20) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`DTMDateTime` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`KWh` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`KVAh` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`RPhaseKW` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`YPhaseKW` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`BPhaseKW` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`RPhaseKVAr` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`RPhaseType` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`YPhaseKVAr` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`YPhaseType` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`BPhaseKVAr` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`BPhaseType` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`RPhaseVoltage` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`YPhaseVoltage` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`BPhaseVoltage` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`PowerDownTime` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`MeterData_ID` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`DTMLoadSurvey_ID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");


            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `meterdata_fraudenergy` (");
            sqlQuery.Append(@"`FraudEnergy_ID` bigint(20) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`MagneticInfluenceKWh` varchar(20) NOT NULL,");
            sqlQuery.Append(@"`MagneticInflueneceKVARhLag` varchar(20) NOT NULL,");
            sqlQuery.Append(@"`MagneticInflueneceKVARhLead` varchar(20) NOT NULL,");
            sqlQuery.Append(@"`MagneticInflueneceKVAh` varchar(20) NOT NULL,");
            sqlQuery.Append(@"`ReverseEnergyKWh` varchar(20) NOT NULL,");
            sqlQuery.Append(@"`ReverseEnergyKVAh` varchar(20) NOT NULL,");
            sqlQuery.Append(@"`ReverseEnergyKVARhLag` varchar(20) NOT NULL,");
            sqlQuery.Append(@"`ReverseEnergyKVARhLead` varchar(20) NOT NULL,");
            sqlQuery.Append(@"`THDVoltageRPhase` varchar(20) NOT NULL,");
            sqlQuery.Append(@"`THDVoltageYPhase` varchar(20) NOT NULL,");
            sqlQuery.Append(@"`THDVoltageBPhase` varchar(20) NOT NULL,");
            sqlQuery.Append(@"`THDCurrentRPhase` varchar(20) NOT NULL,");
            sqlQuery.Append(@"`THDCurrentYPhase` varchar(20) NOT NULL,");
            sqlQuery.Append(@"`THDCurrentBPhase` varchar(20) NOT NULL,");
            sqlQuery.Append(@"`MeterData_ID` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`FraudEnergy_ID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");


            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `meterdata_general` (");
            sqlQuery.Append(@"`General_ID` bigint(20) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`MeterID` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`MeterDateTime` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`ErrorCode` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`MeterConstant` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`FirmwareVersion` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`CTRatio` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`VoltagePhaseSequence` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`CurrentPhaseSequence` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`TotalActiveEnergy` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`CumulativeExportEnergyKWH` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`CumulativeExportEnergyKVAH` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`CumulativeMD1` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`CumulativeMD2` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`CumulativeMD3` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`RisingDemandKW` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`ElapsedTimeKW` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`RisingDemandKVA` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`ElapsedTimeKVA` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`TotalPowerOnHours` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`CurrentMonthPowerOnHours` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`MDResetCounter` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`ReadoutCounter` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`ProgrammingCounter` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`CTRatioProgrammingCounter` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`LatestTamperOccurrenceID` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`OccurrenceTime` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`LatestTamperRestorationID` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`BateryModePowerOnHour` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`RestorationTime` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`MeterData_ID` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`PowerOffDays` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`General_ID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");


            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `meterdata_instantpower` (");
            sqlQuery.Append(@"`InstantPower_ID` bigint(20) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`MeterID` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`MeterDateTime` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`VoltageRPhase` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`VoltageYPhase` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`VoltageBPhase` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`CurrentRPhase` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`CurrentYPhase` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`CurrentBPhase` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`InstantActivepower` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`InstantReactiveLagPower` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`InstantReactiveLeadPower` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`InstantApparentPower` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`TotalPowerFactor` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`PowerFactorRPhase` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`PowerFactorYPhase` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`PowerFactorBPhase` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`AveragePowerFactor` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`Frequency` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`TotalFundamentalActiveEnergy` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`InstantActivepowerRPhase` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`InstantActivepowerYPhase` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`InstantActivepowerBPhase` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`InstantReactivepowerRPhase` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`InstantReactivepowerYPhase` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`InstantReactivepowerBPhase` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`InstantApparentpowerRPhase` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`InstantApparentpowerYPhase` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`InstantApparentpowerBPhase` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`MeterData_ID` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`InstantPower_ID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");


            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `meterdata_loadsurvey` (");
            sqlQuery.Append(@"`LoadSurvey_ID` int(11) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`MeterReadingDatetime` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`RPhaseVoltage` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`YPhaseVoltage` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`BPhaseVoltage` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`RPhaseCurrent` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`YPhaseCurrent` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`BPhaseCurrent` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`AvgVoltage` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`AvgCurrent` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`DemandKVARLead` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`DemandKVA` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`DemandKW` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`DemandKVARLag` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`PowerFactor` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`TamperStatus` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`LoadSurveyDateTime` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`MDIntervalPeriod` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"`IsDLMS` int(1) DEFAULT 0,");
            sqlQuery.Append(@"`MeterData_ID` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`LoadSurvey_ID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");


            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `meterdata_phasor` (");
            sqlQuery.Append(@"`Phasor_ID` bigint(20) NOT NULL AUTO_INCREMENT,"); 
            sqlQuery.Append(@"`RPhaseVoltage` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`YPhaseVoltage` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`BPhaseVoltage` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`RPhaseCurrent` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`YPhaseCurrent` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`BPhaseCurrent` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`TotalActivePower` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`TotalInductivePower` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`TotalCapacitivePower` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`TotalApparentPower` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`RPhasePF` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`YPhasePF` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`BPhasePF` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`TotalInstantaneousPF` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`Frequency` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`PhaseSequence` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`TotalkWDirection` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`RPhasekWDirection` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`YPhasekWDirection` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`BPhasekWDirection` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`RPhaseChannel` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`YPhaseChannel` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`BPhaseChannel` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`RPhaseLagLead` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`YPhaseLagLead` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`BPhaseLagLead` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`Total` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`YPhaseAngleWithRPhase` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`BPhaseAngleWithRPhase` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`AngleBWAnyPhasePresent` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`MeterData_ID` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`Phasor_ID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;"); 

            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `meterdata_powerfactor` (");
            sqlQuery.Append(@"`PowerFactor_ID` bigint(20) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`TotalPowerFactor` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`PowerFactorRPhase` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`PowerFactorYPhase` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`PowerFactorBPhase` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`CTRatio` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`TotalPowerOnHours` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`CurrentMonthPowerOnHours` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`LoadFactorCurrentMonth` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`PowerFactor_ID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");

            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `meterdata_programming` (");
            sqlQuery.Append(@"`Programming_ID` bigint(20) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`TotalProgrammingUpdates` varchar(20) NOT NULL,");
            sqlQuery.Append(@"`UpdateSequence` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`LastTimestamp` varchar(25) DEFAULT NULL,");
            sqlQuery.Append(@"`Description1` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Description2` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Description3` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Description4` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Description5` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Description6` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Description7` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Description8` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Description9` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Description10` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Description11` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Description12` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Description13` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Description14` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Description15` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Description16` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Description17` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Description18` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Description19` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`MeterData_ID` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`Programming_ID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");


            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `meterdata_rtcupdate` (");
            sqlQuery.Append(@"`RTCUpdate_ID` bigint(20) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`TotalRTCUpdates` varchar(20) NOT NULL,");
            sqlQuery.Append(@"`CurrentRTC1` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`PreviousRTC1` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`CurrentRTC2` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`PreviousRTC2` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`CurrentRTC3` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`PreviousRTC3` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`CurrentRTC4` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`PreviousRTC4` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`CurrentRTC5` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`PreviousRTC5` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`CurrentRTC6` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`PreviousRTC6` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`CurrentRTC7` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`PreviousRTC7` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`CurrentRTC8` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`PreviousRTC8` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`CurrentRTC9` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`PreviousRTC9` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`CurrentRTC10` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`PreviousRTC10` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`MeterData_ID` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`RTCUpdate_ID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");


            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `meterdata_tampercounter` (");
            sqlQuery.Append(@"`TamperCounter_ID` bigint(20) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`TotalTamperCounter` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"`PowerOnOffCounter` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"`LowLoadCounter` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"`OverLoadCounter` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"`TamperCounterGeneral_ID` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`MeterData_ID` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`TamperCounter_ID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");

            sqlQuery.Append(@" CREATE TABLE IF NOT EXISTS `meterdata_tampercountergeneral` (");
            sqlQuery.Append(@"`TamperCounterGeneral_ID` bigint(20) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`VoltageImbalanceRPhaseTamperCounter` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"`VoltageImbalanceYPhaseTamperCounter` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"`VoltageImbalanceBPhaseTamperCounter` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"`MissingPotentialRPhaseTamperCounter` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"`MissingPotentialYPhaseTamperCounter` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"`MissingPotentialBPhaseTamperCounter` int(11) DEFAULT NULL,"); 
            sqlQuery.Append(@"`CTShortTamperCounter` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"`CTOpenRPhaseTamperCounter` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"`CTOpenYPhaseTamperCounter` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"`CTOpenBPhaseTamperCounter` int(11) DEFAULT NULL,"); 
            sqlQuery.Append(@"`OnePhaseNeutralAbsentTamperCounter` int(11) DEFAULT NULL,"); 
            sqlQuery.Append(@"`VoltagePhaseReversalTamperCounter` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"`CurrentImbalanceRPhaseTamperCounter` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"`CurrentImbalanceYPhaseTamperCounter` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"`CurrentImbalanceBPhaseTamperCounter` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"`CurrentReversalRPhaseTamperCounter` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"`CurrentReversalYPhaseTamperCounter` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"`CurrentReversalBPhaseTamperCounter` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"`MagneticInfluenceTamperCounter` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"`NeutralDisturbanceTamperCounter` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"`FrontCoverOpeningTamperCounter` int(11) DEFAULT NULL,"); 
            sqlQuery.Append(@"`History_ID` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`MeterData_ID` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`RelatedTo` varchar(2) DEFAULT NULL,");
            sqlQuery.Append(@"`BillingTimeStamp` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`BillingCounter` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`TamperCounterGeneral_ID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");


            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `meterdata_tampersnapshot` (");
            sqlQuery.Append(@"`TamperSnapShot_ID` bigint(20) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`TamperCode` int(11) NOT NULL,");
            sqlQuery.Append(@"`TamperDescription` varchar(100) DEFAULT NULL,");
            sqlQuery.Append(@"`TamperOccurredTime` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`TamperRestoredTime` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`RVoltageOccurred` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`YVoltageOccurred` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`BVoltageOccurred` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`RCurrentOccurred` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`YCurrentOccurred` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`BCurrentOccurred` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`RPFOccurred` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`YPFOccurred` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`BPFOccurred` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`TotalPFOccurred` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`kWhOccurred` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`kVAhOccurred` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`RVoltageRestored` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`YVoltageRestored` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`BVoltageRestored` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`RCurrentRestored` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`YCurrentRestored` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`BCurrentRestored` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`RPFRestored` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`YPFRestored` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`BPFRestored` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`TotalPFRestored` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`kWhRestored` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`kVAhRestored` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`MeterData_ID` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`TamperSnapShot_ID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");


            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `meterdata_tariffinformation` (");
            sqlQuery.Append(@"`Tariff_ID` bigint(20) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`HistoryID` smallint(6) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff1_kWh` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff1_kVAh` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff1_kVARh_lag` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff1_kVARh_lead` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff1_MD1` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff1_MD1_TimeStamp` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff1_MD2` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff1_MD2_TimeStamp` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff1_MD3` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff1_MD3_TimeStamp` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff1_Aver_PF` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff2_kWh` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff2_kVAh` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff2_kVARh_lag` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff2_kVARh_lead` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff2_MD1` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff2_MD1_TimeStamp` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff2_MD2` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff2_MD2_TimeStamp` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff2_MD3` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff2_MD3_TimeStamp` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff2_Aver_PF` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff3_kWh` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff3_kVAh` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff3_kVARh_lag` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff3_kVARh_lead` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff3_MD1` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff3_MD1_TimeStamp` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff3_MD2` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff3_MD2_TimeStamp` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff3_MD3` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff3_MD3_TimeStamp` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff3_Aver_PF` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff4_kWh` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff4_kVAh` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff4_kVARh_lag` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff4_kVARh_lead` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff4_MD1` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff4_MD1_TimeStamp` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff4_MD2` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff4_MD2_TimeStamp` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff4_MD3` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff4_MD3_TimeStamp` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff4_Aver_PF` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff5_kWh` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff5_kVAh` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff5_kVARh_lag` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff5_kVARh_lead` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff5_MD1` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff5_MD1_TimeStamp` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff5_MD2` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff5_MD2_TimeStamp` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff5_MD3` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff5_MD3_TimeStamp` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff5_Aver_PF` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff6_kWh` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff6_kVAh` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff6_kVARh_lag` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff6_kVARh_lead` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff6_MD1` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff6_MD1_TimeStamp` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff6_MD2` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff6_MD2_TimeStamp` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff6_MD3` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff6_MD3_TimeStamp` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff6_Aver_PF` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff7_kWh` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff7_kVAh` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff7_kVARh_lag` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff7_kVARh_lead` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff7_MD1` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff7_MD1_TimeStamp` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff7_MD2` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff7_MD2_TimeStamp` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff7_MD3` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff7_MD3_TimeStamp` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff7_Aver_PF` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff8_kWh` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff8_kVAh` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff8_kVARh_lag` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff8_kVARh_lead` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff8_MD1` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff8_MD1_TimeStamp` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff8_MD2` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff8_MD2_TimeStamp` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff8_MD3` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff8_MD3_TimeStamp` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`Tariff8_Aver_PF` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`MeterData_ID` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`Tariff_ID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");

            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `metermodel_master` (");
            sqlQuery.Append(@"`MeterModel_ID` int(11) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`MeterModel_Name` varchar(50) NOT NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`MeterModel_ID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");


            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `metertype_master` (");
            sqlQuery.Append(@"`MeterType_ID` int(11) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`MeterType_Name` varchar(50) NOT NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`MeterType_ID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");


            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `meterunit_master` (");
            sqlQuery.Append(@"`MeterUnit_ID` int(11) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`MeterUnit_Type` varchar(50) NOT NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`MeterUnit_ID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");


            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `meter_master` (");
            sqlQuery.Append(@"`Meter_ID` varchar(20) NOT NULL,");
            sqlQuery.Append(@"`MeterType_ID` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"`MeterModel_ID` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"`Meter_EMF` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"`Meter_ContractDemand` double DEFAULT NULL,");
            sqlQuery.Append(@"`MeterUnit_ID` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"`Meter_CTPrimary` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"`Meter_CTSecondary` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"`Meter_PTPrimary` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"`Meter_PTSecondary` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"`Meter_InstalledCTPrimary` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"`Meter_InstalledCTSecondary` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"`Meter_InstalledPTPrimary` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"`Meter_InstalledPTSecondary` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"`Meter_Phone` varchar(15) DEFAULT NULL,");
            sqlQuery.Append(@"`Meter_Status` smallint(6) DEFAULT NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`Meter_ID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");

            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `modulecategory_master` (");
            sqlQuery.Append(@"`ModuleCategory_ID` int(11) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`Module_ID` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"`Category_ID` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`ModuleCategory_ID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");


            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `module_master` (");
            sqlQuery.Append(@"`Module_ID` int(11) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`Module_Name` varchar(35) DEFAULT NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`Module_ID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");

            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `rcdmeter_master` (");
            sqlQuery.Append(@"`RCDMeter_ID` bigint(20) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`Region_ID` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"`Circle_ID` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`Division_ID` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`CMRI_ID` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`Meter_ID` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`MeterAllocation_Date` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`RCDMeter_ID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");


            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `region_master` (");
            sqlQuery.Append(@"`Region_ID` int(11) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`Region_Name` varchar(50) NOT NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`Region_ID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");


            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `subgroupmeter_master` (");
            sqlQuery.Append(@"`SubGroupMeter_ID` bigint(20) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`SubGroup_ID` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"`Meter_ID` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`GroupAllocation_Date` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`SubGroupMeter_ID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");


            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `subgroup_master` (");
            sqlQuery.Append(@"`SubGroup_ID` int(11) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`SubGroup_Name` varchar(35) NOT NULL,");
            sqlQuery.Append(@"`SubGroup_Description` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Group_ID` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`SubGroup_ID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");


            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `suspectedconsumer` (");
            sqlQuery.Append(@"`SuspectedConsumer_ID` int(11) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`Consumer_Number` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`SuspectionStartDate` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`SuspectionEndDate` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`SuspectedConsumer_ID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");


            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `tampertype_master` (");
            sqlQuery.Append(@"`TamperTypeID` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"`TamperType` varchar(50) DEFAULT NULL");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");


            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `userinformation` (");
            sqlQuery.Append(@"`UserInformation_ID` int(11) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`Users_Name` varchar(35) NOT NULL,");
            sqlQuery.Append(@"`User_Password` varchar(10) NOT NULL,");
            sqlQuery.Append(@"`Category_ID` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"`Login_ID` varchar(20) NOT NULL,");
            sqlQuery.Append(@"`Designation_ID` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"`IsActive` int(11) DEFAULT '0',");
            sqlQuery.Append(@"PRIMARY KEY (`UserInformation_ID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");


            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `userlogactivity` (");
            sqlQuery.Append(@"`Activity_ID` bigint(20) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`UserInformation_ID` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"`Activity_DateTime` bigint(20) NOT NULL,");
            sqlQuery.Append(@"`Activity` varchar(150) DEFAULT NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`Activity_ID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");


            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `userrights` (");
            sqlQuery.Append(@"`Right_ID` int(11) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`Module_ID` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"`UserInformation_ID` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"`Permission` smallint(6) NOT NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`Right_ID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");


            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `versioninfo` (");
            sqlQuery.Append(@"`VersionInfo_ID` int(11) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`Version_Number` varchar(45) DEFAULT NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`VersionInfo_ID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");


            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `versionmaster` (");
            sqlQuery.Append(@"`VersionMaster_ID` bigint(20) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`Tamper` text,");
            sqlQuery.Append(@"`TamperShnapShot` text,");
            sqlQuery.Append(@"`LoadSurvey` text,");
            sqlQuery.Append(@"`Transactions` text,");
            sqlQuery.Append(@"`Phasor` text,");
            sqlQuery.Append(@"`FraudEnergy` text,");
            sqlQuery.Append(@"`DailyLoadProfile` text,");
            sqlQuery.Append(@"`DTMLoadSurvey` text,");
            sqlQuery.Append(@"`Billing` text,");
            sqlQuery.Append(@"`CurrentTariff` text,");
            sqlQuery.Append(@"`CurrentTamper` text,");
            sqlQuery.Append(@"`General` text,");
            sqlQuery.Append(@"`HistoryTariff` text,");
            sqlQuery.Append(@"`HistoryTamper` text,");
            sqlQuery.Append(@"`InstantPower` text,");
            sqlQuery.Append(@"`VersionID` bigint(20) NOT NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`VersionMaster_ID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");


            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `meterdata_headerinfo` (");
            sqlQuery.Append(@"`HeaderInfo_ID` bigint(20) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`MD1KWDemandType` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`MD1KWTimeInterval` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`MD1KWSubInterval` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`MD2KVADemandType` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`MD2KVATimeInterval` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`MD2KVASubInterval` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`PFLogic` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`PowerOffDays` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`MeterConstant` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`InternalCTPTRatio` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`SoftwareVersion` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`BillingType` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`BillingDate` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`BillingHour` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`BillingMinute` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`MeterData_ID` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`NoLoadDuration` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`NoSupplyDuration` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`HeaderInfo_ID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");
            
            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `meterdata_NamePlateDetail` (");
            sqlQuery.Append(@"`NamePlateDetailID` bigint(20) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`MeterID` varchar(50) NOT NULL,");
            sqlQuery.Append(@"`MeterType` varchar(50) NOT NULL,");
            sqlQuery.Append(@"`CurrentRating` varchar(50) NOT NULL,");
            sqlQuery.Append(@"`VoltageRating` varchar(50) NOT NULL,");
            sqlQuery.Append(@"`MeterConstant` varchar(50) NOT NULL,");
            sqlQuery.Append(@"`ManufacturingDate` varchar(50) NOT NULL,"); 
            sqlQuery.Append(@"`MeterData_ID` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`NamePlateDetailID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");//kvarSelectionEntity

            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `meterConfigurations` (");
            sqlQuery.Append(@"`MeterConfigurationID` bigint(20) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`MeterID` varchar(50) NOT NULL,");
            sqlQuery.Append(@"`FileUploadID` Bigint(20),");
            sqlQuery.Append(@"`KWDemandType` varchar(50) NOT NULL,");
            sqlQuery.Append(@"`KWInterval` int(2) NOT NULL,");
            sqlQuery.Append(@"`KWSubInterval` int(2) NOT NULL,");
            sqlQuery.Append(@"`KVADemandType` varchar(50) NOT NULL,");
            sqlQuery.Append(@"`KVAInterval` int(2) NOT NULL,");
            sqlQuery.Append(@"`KVASubInterval` int(2) NOT NULL,");
            sqlQuery.Append(@"`MeterData_ID` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`MeterConfigurationID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");

            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `kvarSelection` (");
            sqlQuery.Append(@"`kvarSelectionID` bigint(20) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`MeterID` varchar(50) NOT NULL,");
            sqlQuery.Append(@"`FileUploadID` Bigint(20),");
            sqlQuery.Append(@"`lagOnly` varchar(10) NOT NULL,");
            sqlQuery.Append(@"`lagandLead` varchar(50) NOT NULL,");
            sqlQuery.Append(@"`MeterData_ID` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`kvarSelectionID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");

            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `AutoLock` (");
            sqlQuery.Append(@"`AutoLockId` bigint(20) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`MeterID` varchar(50) NOT NULL,");
            sqlQuery.Append(@"`FileUploadID` Bigint(20),");
            sqlQuery.Append(@"`AutoLockStatus` varchar(20) NOT NULL,");            
            sqlQuery.Append(@"`MeterData_ID` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`AutoLockId`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");

            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `DailyLog` (");
            sqlQuery.Append(@"`DailyLogID` bigint(20) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`MeterID` varchar(50) NOT NULL,");
            sqlQuery.Append(@"`FileUploadID` Bigint(20),");
            sqlQuery.Append(@"`CumulativeKWh` varchar(50) NOT NULL,");
            sqlQuery.Append(@"`CumulativeKVARhLag` varchar(50) NOT NULL,");
            sqlQuery.Append(@"`CumulativeKVARhLead` varchar(50) NOT NULL,");
            sqlQuery.Append(@"`CumulativeKVAh` varchar(50) NOT NULL,");
            sqlQuery.Append(@"`DailyMD1` varchar(50) NOT NULL,");
            sqlQuery.Append(@"`DailyMD2` varchar(50) NOT NULL,");
            sqlQuery.Append(@"`MeterData_ID` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`DailyLogID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");

            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `BillingReset` (");
            sqlQuery.Append(@"`BillingResetID` bigint(20) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`MeterID` varchar(50) NOT NULL,");
            sqlQuery.Append(@"`FileUploadID` Bigint(20),");
            sqlQuery.Append(@"`ModeOfBilling` varchar(50) NOT NULL,");
            sqlQuery.Append(@"`BillingPeriod` varchar(50) NOT NULL,");
            sqlQuery.Append(@"`Day` varchar(50) NOT NULL,");
            sqlQuery.Append(@"`Hours` varchar(50) NOT NULL,");
            sqlQuery.Append(@"`Minutes` varchar(50) NOT NULL,");
            sqlQuery.Append(@"`ResetLockOutDays` varchar(50) NOT NULL,");
            sqlQuery.Append(@"`MeterData_ID` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`BillingResetID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");


            sqlQuery.Append(@"CREATE  TABLE IF NOT EXISTS `displayparamater` (");
            sqlQuery.Append(@" `DisplayParamaterType` INT NOT NULL ,  `DisplayParamaterName` VARCHAR(90) NULL ,");
            sqlQuery.Append(@"`MeterData_ID` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`DisplayParamaterValue` INT NULL ,`FileUploadID` Bigint(20),  `MeterID`  VARCHAR(90) NULL ");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");

            sqlQuery.Append(@"CREATE  TABLE IF NOT EXISTS `rtc` (");
            sqlQuery.Append(@"`MeterData_ID` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@" `MeterID`  VARCHAR(90) NULL,`FileUploadID` Bigint(20), `RTC`  VARCHAR(90) NULL");                      
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");

            sqlQuery.Append(@"CREATE  TABLE IF NOT EXISTS `tod` (");
            sqlQuery.Append(@"`MeterData_ID` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@" `MeterID`  VARCHAR(90) NULL, `FileUploadID` Bigint(20),`TODData`  TEXT NULL");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");

            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `RS232`(");
            sqlQuery.Append(@" `RS232ID` bigint(20) not null auto_increment,");
            sqlQuery.Append(@" `MeterID` varchar(90) null,");
            sqlQuery.Append(@"`MeterData_ID` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@" `RS232Status` varchar(20) not null,");
            sqlQuery.Append(@"`FileUploadID` Bigint(20),");
            sqlQuery.Append(@"PRIMARY KEY (`RS232ID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");

            sqlQuery.Append(@"Create Table if not exists `Utility`(");
            sqlQuery.Append(@"`Utility_ID` bigint(20) not null auto_increment,");
            sqlQuery.Append(@"`Utility_Name` varchar(20) null,");
            sqlQuery.Append(@"`Utility_Password` varchar(15) null,");
            sqlQuery.Append(@"PRIMARY KEY (`Utility_ID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");

            
            return sqlQuery.ToString();
        } 
        public bool CreateCABAppDatabase()
        {
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder(); 
                builder.Append(GetTableCreationQuery());
                DataRequest request = new DataRequest(builder.ToString());
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog("Database created");
                DeleteAllData(); 
                Flag = true;
            }
            catch (CABException)
            {
                Flag = false;
            }
            return Flag;
        } 

        public void DeleteAllData()
        { 
            try
            {
                string[] qry = GetTableList();
                for (int i = 0; i <= 64; i++)
                {
                    IDataHelper helper = DatabaseFactory.GetHelper();
                    DataRequest request = new DataRequest(qry[i]);
                    helper.ExecuteNonQuery(request);
                }
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog("Database data deleted");
            }
            catch (CABException)
            { 
            } 
        }
        private string[] GetTableList()
        {
            string[] qry = new string[66];
            qry[0] = "delete from areamaster";
            qry[1] = "delete from areameter_master";
            qry[2] = "delete from asciiexportsettings";
            qry[3] = "delete from categoryright";
            qry[4] = "delete from category_master";
            qry[5] = "delete from circle_master";
            qry[6] = "delete from cmri_master";
            qry[7] = "delete from consumerexportsettings";
            qry[8] = "delete from consumermeter";
            qry[9] = "delete from consumertype_master";
            qry[10] = "delete from consumer_master";
            qry[11] = "delete from designation_master";
            qry[12] = "delete from division_master";
            qry[13] = "delete from dtmdailyprofileparameter";
            qry[14] = "delete from exceptionlog";
            qry[15] = "delete from fileupload_master";
            qry[16] = "delete from group_master";
            qry[17] = "delete from gsmgroupschedule";
            qry[18] = "delete from gsmreadingstatus";
            qry[19] = "delete from gsmschedule";
            qry[20] = "delete from history_master";
            qry[21] = "delete from loadsurveyparameter";
            qry[22] = "delete from meterdata";
            qry[23] = "delete from meterdata_billing";
            qry[24] = "delete from meterdata_dtmdailyprofile";
            qry[25] = "delete from meterdata_dtmloadsurvey";
            qry[26] = "delete from meterdata_fraudenergy";
            qry[27] = "delete from meterdata_general";
            qry[28] = "delete from meterdata_instantpower";
            qry[29] = "delete from meterdata_loadsurvey";
            qry[30] = "delete from meterdata_phasor";
            qry[31] = "delete from meterdata_powerfactor";
            qry[32] = "delete from meterdata_programming";
            qry[33] = "delete from meterdata_rtcupdate";
            qry[34] = "delete from meterdata_tampercounter";
            qry[35] = "delete from meterdata_tampercountergeneral";
            qry[36] = "delete from meterdata_tampersnapshot";
            qry[37] = "delete from meterdata_tariffinformation";
            qry[38] = "delete from metermodel_master";
            qry[39] = "delete from metertype_master";
            qry[40] = "delete from meterunit_master";
            qry[41] = "delete from meter_master";
            qry[42] = "delete from modulecategory_master";
            qry[43] = "delete from module_master";
            qry[44] = "delete from rcdmeter_master";
            qry[45] = "delete from region_master";
            qry[46] = "delete from subgroupmeter_master";
            qry[47] = "delete from subgroup_master";
            qry[48] = "delete from suspectedconsumer";
            qry[49] = "delete from tampertype_master";
            qry[50] = "delete from userinformation";
            qry[51] = "delete from userlogactivity";
            qry[52] = "delete from userrights";
            qry[53] = "delete from versioninfo";
            qry[54] = "delete from versionmaster";
            qry[55] = "delete from meterdata_headerinfo";
            qry[56] = "delete from meterdata_NamePlateDetail";
            qry[57] = "delete from meterConfigurations";
            qry[58] = "delete from kvarSelection";
            qry[59] = "delete from DailyLog";
            qry[60] = "delete from BillingReset";
            qry[61] = "delete from displayparamater";
            qry[62] = "delete from rtc";
            qry[63] = "delete from tod";
            qry[64] = "delete from Utility";
            qry[65] = "delete from autolock";
           
            return qry;
        }

        private string[] GetDropTableList()
        {
            string[] qry = new string[67];
            qry[0] = "DROP TABLE IF EXISTS `areamaster`";
            qry[1] = "DROP TABLE IF EXISTS `areameter_master`";
            qry[2] = "DROP TABLE IF EXISTS `asciiexportsettings`";
            qry[3] = "DROP TABLE IF EXISTS `categoryright`";
            qry[4] = "DROP TABLE IF EXISTS `category_master`";
            qry[5] = "DROP TABLE IF EXISTS `circle_master`";
            qry[6] = "DROP TABLE IF EXISTS `cmri_master`";
            qry[7] = "DROP TABLE IF EXISTS `consumerexportsettings`";
            qry[8] = "DROP TABLE IF EXISTS `consumermeter`";
            qry[9] = "DROP TABLE IF EXISTS `consumertype_master`";
            qry[10] = "DROP TABLE IF EXISTS `consumer_master`";
            qry[11] = "DROP TABLE IF EXISTS `designation_master`";
            qry[12] = "DROP TABLE IF EXISTS `division_master`";
            qry[13] = "DROP TABLE IF EXISTS `dtmdailyprofileparameter`";
            qry[14] = "DROP TABLE IF EXISTS `exceptionlog`";
            qry[15] = "DROP TABLE IF EXISTS `fileupload_master`";
            qry[16] = "DROP TABLE IF EXISTS `group_master`";
            qry[17] = "DROP TABLE IF EXISTS `gsmgroupschedule`";
            qry[18] = "DROP TABLE IF EXISTS `gsmreadingstatus`";
            qry[19] = "DROP TABLE IF EXISTS `gsmschedule`";
            qry[20] = "DROP TABLE IF EXISTS `history_master`";
            qry[21] = "DROP TABLE IF EXISTS `loadsurveyparameter`";
            qry[22] = "DROP TABLE IF EXISTS `meterdata`";
            qry[23] = "DROP TABLE IF EXISTS `meterdata_billing`";
            qry[24] = "DROP TABLE IF EXISTS `meterdata_dtmdailyprofile`";
            qry[25] = "DROP TABLE IF EXISTS `meterdata_dtmloadsurvey`";
            qry[26] = "DROP TABLE IF EXISTS `meterdata_fraudenergy`";
            qry[27] = "DROP TABLE IF EXISTS `meterdata_general`";
            qry[28] = "DROP TABLE IF EXISTS `meterdata_instantpower`";
            qry[29] = "DROP TABLE IF EXISTS `meterdata_loadsurvey`";
            qry[30] = "DROP TABLE IF EXISTS `meterdata_phasor`";
            qry[31] = "DROP TABLE IF EXISTS `meterdata_powerfactor`";
            qry[32] = "DROP TABLE IF EXISTS `meterdata_programming`";
            qry[33] = "DROP TABLE IF EXISTS `meterdata_rtcupdate`";
            qry[34] = "DROP TABLE IF EXISTS `meterdata_tampercounter`";
            qry[35] = "DROP TABLE IF EXISTS `meterdata_tampercountergeneral`";
            qry[36] = "DROP TABLE IF EXISTS `meterdata_tampersnapshot`";
            qry[37] = "DROP TABLE IF EXISTS `meterdata_tariffinformation`";
            qry[38] = "DROP TABLE IF EXISTS `metermodel_master`";
            qry[39] = "DROP TABLE IF EXISTS `metertype_master`";
            qry[40] = "DROP TABLE IF EXISTS `meterunit_master`";
            qry[41] = "DROP TABLE IF EXISTS `meter_master`";
            qry[42] = "DROP TABLE IF EXISTS `modulecategory_master`";
            qry[43] = "DROP TABLE IF EXISTS `module_master`";
            qry[44] = "DROP TABLE IF EXISTS `rcdmeter_master`";
            qry[45] = "DROP TABLE IF EXISTS `region_master`";
            qry[46] = "DROP TABLE IF EXISTS `subgroupmeter_master`";
            qry[47] = "DROP TABLE IF EXISTS `subgroup_master`";
            qry[48] = "DROP TABLE IF EXISTS `suspectedconsumer`";
            qry[49] = "DROP TABLE IF EXISTS `tampertype_master`";
            qry[50] = "DROP TABLE IF EXISTS `userinformation`";
            qry[51] = "DROP TABLE IF EXISTS `userlogactivity`";
            qry[52] = "DROP TABLE IF EXISTS `userrights`";
            qry[53] = "DROP TABLE IF EXISTS `versioninfo`";
            qry[54] = "DROP TABLE IF EXISTS `versionmaster`";
            qry[55] = "DROP TABLE IF EXISTS `meterdata_headerinfo`";
            qry[56] = "DROP TABLE IF EXISTS `meterdata_NamePlateDetail`";
            qry[57] = "DROP TABLE IF EXISTS `meterConfigurations`";
            qry[58] = "DROP TABLE IF EXISTS `kvarSelection`";
            qry[59] = "DROP TABLE IF EXISTS `DailyLog`";
            qry[60] = "DROP TABLE IF EXISTS `displayparamater`";
            qry[61] = "DROP TABLE IF EXISTS `rtc`";
            qry[62] = "DROP TABLE IF EXISTS `tod`";
            qry[63] = "DROP TABLE IF EXISTS `BillingReset`";
            qry[64] = "DROP TABLE IF EXISTS `Utility`";
            qry[65] = "DROP TABLE IF EXISTS `autolock`";
            qry[66] = "DROP TABLE IF EXISTS `meterdata_nameplate`";
            
            return qry;
        }
        public void DropAllTable()
        {
            try
            {
                string[] qry = GetDropTableList();
                for (int i = 0; i <= 64; i++)
                {
                    IDataHelper helper = DatabaseFactory.GetHelper();
                    DataRequest request = new DataRequest(qry[i]);
                    helper.ExecuteNonQuery(request);
                }
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog("Database tables deleted");
            }
            catch (CABException)
            {
            }
        }

        public override IEntity InsertData(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public override IEntity InsertData(IList<IEntity> entities)
        {
            throw new NotImplementedException();
        }

        public override bool UpdateData(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteData(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public override IEntity GetDetailData(int id)
        {
            throw new NotImplementedException();
        }

        public override IList<IEntity> ListData()
        {
            throw new NotImplementedException();
        }

        public override DataSet ListDataSet()
        {
            throw new NotImplementedException();
        }

        public override IEntity RowToEntity(DataRow row)
        {
            throw new NotImplementedException();
        }
    }
}
