using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using CAB.DALC.Data.DataServices;
using CAB.Framework;
using CAB.Framework.Entity;
using CAB.Framework.Utility;
using Hunt.EPIC.Logging;
namespace CAB.DALC.Data
{
    public class DBGenerationDAL : DALBase
    {
        private ApplicationType apptype;
        private UtilityEntity localUtilityEntity;
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(DBGenerationDAL).ToString());
        public DBGenerationDAL()
        {
            apptype = ConfigInfo.GetApplicationType();

        }
        public DBGenerationDAL(UtilityEntity utilityEntity)
        {
            localUtilityEntity = utilityEntity;
            apptype = ConfigInfo.GetApplicationType();

        }
        /// <summary>
        /// Author : VBM
        /// Purpose : Used to get query for creating all SPs in DB
        /// </summary>
        /// <returns></returns>
        private string QueryToCreateStoredProcedures()
        {

            // GPRS: change the spliter character to ~ as $ is used in xml xpath in BulkUpdateEndPointSyncStatus routine

            StringBuilder sqlQuery = new StringBuilder();
            /* VBM create SP  GetTamperStartEndDate */
            sqlQuery.AppendLine(@"CREATE  PROCEDURE DLMS_LTCT_650.`GetTamperStartEndDate`(MeterData_ID INT)");
            sqlQuery.AppendLine(@"BEGIN");
            sqlQuery.AppendLine(@"Select Min(DateTimeEvent) as TamperStartDate");
            sqlQuery.AppendLine(@",Max(DateTimeEvent)  as TamperEndDate from tamper_master");
            //BhardwajG : Use table name for fetching meter data ID
            sqlQuery.AppendLine(@"where tamper_master.MeterData_ID=MeterData_ID;");
            sqlQuery.AppendLine(@"END");
            /* VBM GetTamperStartEndDate */

            /* VBM create SP  GetTamperTypeWithCount */
            sqlQuery.AppendLine(@"~");
            sqlQuery.AppendLine(@"CREATE  PROCEDURE DLMS_LTCT_650.`GetTamperTypeWithCount`(MeterData_ID INT)");
            sqlQuery.AppendLine(@"BEGIN");
            sqlQuery.AppendLine(@"Select TM.EventCode as TamperId,");
            sqlQuery.AppendLine(@"TTM.TamperType as Description , Count(TM.EventCode) as Count");
            sqlQuery.AppendLine(@"from Tampertype_master TTM");
            sqlQuery.AppendLine(@",Tamper_master TM where TTM.TamperTypeId = TM.EventCode");
            sqlQuery.AppendLine(@"and TM.MeterData_ID=MeterData_ID");
            sqlQuery.AppendLine(@"and TTM.TamperCategory in(0,1)");
            sqlQuery.AppendLine(@"group by TM.EventCode;");
            sqlQuery.AppendLine(@"END");
            /* VBM GetTamperTypeWithCount */

            /* VBM create SP  GetTamperDetailByTamperType */
            sqlQuery.AppendLine(@"~");
            sqlQuery.AppendLine(@"CREATE  PROCEDURE DLMS_LTCT_650.`GetTamperDetailByTamperType`(MeterData_ID INT, EventCode INT,FromDate BIGINT,ToDate BIGINT)");
            sqlQuery.AppendLine(@"BEGIN");
            sqlQuery.AppendLine(@"Select TTM.TamperType as Description ,TM.EventCode as 'Event Code',TM.DateTimeEvent as 'Time Stamp',TM.CurrentIR,");
            sqlQuery.AppendLine(@"TM.CurrentIY,TM.CurrentIB,TM.PhaseCurrent,TM.VoltageVRN,TM.VoltageVYN,TM.VoltageVBN,TM.PhaseVoltage,TM.PowerFactorRphase,");
            sqlQuery.AppendLine(@"TM.PowerFactorYphase,PowerFactorBphase,TM.TotalPowerFactor,TM.Temprature,NeutralCurrent,ByPassCurrent,CumulativeEnergykWh,CumulativeEnergykVAh, CumulativeEnergykWhExport,CumulativeEnergykVAhExport,CumulativeEnergykWhImport,CumulativeEnergykVAhImport,CumulativeEnergykvarhLag,CumulativeEnergykvarhLead,ActiveCurrentR,ActiveCurrentY,ActiveCurrentB,HighNeutralCurrent,kWr,kWy,kWb,kVAr,kVAy,kVAb,CumulativeTamperCount,Frequency,PhaseCurrentInstant,THDVR,THDVY,THDVB,THDIR,THDIY,THDIB from Tamper_Master TM,"); // Story - 349654 - Neutral Current in Tamper // SB Code Change Start/End - 20171129 - Added 3 new columns.//SarkarA code change start 20180330 // add phase current instant, frequency/end
            sqlQuery.AppendLine(@"TamperType_Master TTM where TTM.TamperTypeID=TM.EventCode");
            sqlQuery.AppendLine("and TM.DateTimeEvent >= FromDate");
            sqlQuery.AppendLine("and TM.DateTimeEvent <=ToDate");
            sqlQuery.AppendLine(@"and TM.MeterData_ID=MeterData_ID and TM.EventCode=EventCode");
            sqlQuery.AppendLine(@"order by TM.DateTimeEvent ASC;");
            sqlQuery.AppendLine(@"END");
            /* VBM GetTamperDetailByTamperType */

            //BhardwajG : Get tamper details between date range
            //sqlQuery.AppendLine("DROP PROCEDURE IF EXISTS DLMS_LTCT_650.`GetTamperDetailByDates`;");
            sqlQuery.AppendLine(@"~");
            sqlQuery.AppendLine(@"CREATE  PROCEDURE DLMS_LTCT_650.`GetTamperDetailByDates`(MeterData_ID INT,FromDate BIGINT,ToDate BIGINT)");
            sqlQuery.AppendLine(@"BEGIN");
            sqlQuery.AppendLine(@"Select TM.EventCode as TamperId,");
            sqlQuery.AppendLine(@"TTM.TamperType as Description , Count(TM.EventCode) as Count");
            sqlQuery.AppendLine(@"from Tampertype_master TTM");
            sqlQuery.AppendLine(@",Tamper_master TM where TTM.TamperTypeId = TM.EventCode");
            sqlQuery.AppendLine(@"and TM.MeterData_ID=MeterData_ID");
            sqlQuery.AppendLine("and TM.DateTimeEvent >= FromDate");
            sqlQuery.AppendLine("and TM.DateTimeEvent <=ToDate");
            sqlQuery.AppendLine(@"and TM.MeterData_ID=MeterData_ID");
            sqlQuery.AppendLine(@"and TTM.TamperCategory in(0,1)");
            sqlQuery.AppendLine(@"group by TM.EventCode;");
            sqlQuery.AppendLine(@"END");

            //Get tamper details between date range with compartment ID
            //sqlQuery.AppendLine("DROP PROCEDURE IF EXISTS DLMS_LTCT_650.`GetTamperDetailByDates`;");
            sqlQuery.AppendLine(@"~");
            sqlQuery.AppendLine(@"CREATE  PROCEDURE DLMS_LTCT_650.`GetTamperDetailByDatesWithCompartmentID`(MeterData_ID INT,FromDate BIGINT,ToDate BIGINT,CompartmentNumber VARCHAR(5))");
            sqlQuery.AppendLine(@"BEGIN");
            sqlQuery.AppendLine(@"Select TM.EventCode as TamperId,");
            sqlQuery.AppendLine(@"TTM.TamperType as Description , Count(TM.EventCode) as Count");
            sqlQuery.AppendLine(@"from Tampertype_master TTM");
            sqlQuery.AppendLine(@",Tamper_master TM where TTM.TamperTypeId = TM.EventCode");
            sqlQuery.AppendLine(@"and TM.MeterData_ID=MeterData_ID");
            sqlQuery.AppendLine("and TM.DateTimeEvent >= FromDate");
            sqlQuery.AppendLine("and TM.DateTimeEvent <=ToDate");
            sqlQuery.AppendLine(@"and TM.MeterData_ID=MeterData_ID");
            sqlQuery.AppendLine(@"and TTM.TamperCategory in(0,1)");
            sqlQuery.AppendLine(@"and TTM.Compartment=CompartmentNumber");
            sqlQuery.AppendLine(@"group by TM.EventCode;");
            sqlQuery.AppendLine(@"END");

            // GPRS: to get GPRS sheduling tasks
            sqlQuery.AppendLine(@"~");
            sqlQuery.AppendLine(@"CREATE PROCEDURE DLMS_LTCT_650.`GetTasks`()");
            sqlQuery.AppendLine(@"BEGIN");
            sqlQuery.AppendLine(@"DECLARE done INT DEFAULT FALSE;");
            sqlQuery.AppendLine(@"DECLARE tempjob VARCHAR(500);");
            sqlQuery.AppendLine(@"DECLARE tempmeter_id VARCHAR(50);");
            sqlQuery.AppendLine(@"DECLARE temptaskid, tempgroupid INT;");
            sqlQuery.AppendLine(@"DECLARE cur1 CURSOR FOR SELECT gt.tasksid,gt.groupid,ggm.meter_ID,SUBSTRING_INDEX(gt.jobs,',',1) as 'job' FROM `gsm_tasks` gt");
            sqlQuery.AppendLine(@",`gsm_group_meters` ggm,");
            sqlQuery.AppendLine(@"`gsm_groups` gg");
            sqlQuery.AppendLine(@"where gg.CommunicationType='GPRS' and gg.group_id=gt.groupid and gt.taskstatus='Inqueue' and");
            sqlQuery.AppendLine(@"STR_TO_DATE(CONCAT(gt.startdate,' ',gt.starttime),'%d/%m/%Y %H:%i:%S')<=NOW()");
            sqlQuery.AppendLine(@"and gt.groupid=ggm.group_ID and ggm.status='S' ;");
            sqlQuery.AppendLine(@"DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = TRUE;");
            sqlQuery.AppendLine(@"OPEN cur1;");
            sqlQuery.AppendLine(@"insert_loop: LOOP");
            sqlQuery.AppendLine(@"FETCH cur1 INTO temptaskid,tempgroupid,tempmeter_id,tempjob;");
            sqlQuery.AppendLine(@"IF done THEN");
            sqlQuery.AppendLine(@"LEAVE insert_loop;");
            sqlQuery.AppendLine(@"END IF;");
            sqlQuery.AppendLine(@"INSERT INTO `gsm_task_logs`(tasksid,meter_id,group_id,isInstantCompleted,isBillingCompleted,isGeneralCompleted,isLoadSurveyCompleted,");
            sqlQuery.AppendLine(@"isTamperCompleted,currentJob,status,taskretries,creationDateTime,errorMessage,isDailyLoadCompleted) VALUES (temptaskid,tempmeter_id,tempgroupid,0,0,0,0,0,tempjob,'NS',0,DATE_FORMAT(NOW(), '%d/%m/%Y %H:%i:%S'),CONCAT(tempjob ,' started'),0);");
            sqlQuery.AppendLine(@"update gsm_tasks");
            sqlQuery.AppendLine(@"set taskstatus='InProgress'");
            sqlQuery.AppendLine(@"where gsm_tasks.tasksid=temptaskid and gsm_tasks.groupid=tempgroupid;");
            sqlQuery.AppendLine(@"END LOOP;");
            sqlQuery.AppendLine(@"CLOSE cur1;");
            sqlQuery.AppendLine(@"DROP TEMPORARY TABLE IF EXISTS `TEMP`;");
            sqlQuery.AppendLine(@"CREATE TEMPORARY Table Temp (");
            sqlQuery.AppendLine(@"select gtl.log_ID, gtl.tasksid ,gt.taskname ,gtl.group_id ,gtl.meter_id ,gtl.currentjob  ,mm.Meter_GPRSModem_IMEI ,");
            sqlQuery.AppendLine(@"gt.startdate ,gt.starttime  ,gt.taskretries ,mm.metermodel_ID");
            sqlQuery.AppendLine(@"from `gsm_task_logs` gtl, `meter_master` mm, `gsm_groups` gg, `gsm_tasks` gt");
            sqlQuery.AppendLine(@"where status='NS'");
            sqlQuery.AppendLine(@"and gg.CommunicationType='GPRS' and gg.group_id=gtl.group_id and gtl.meter_ID=mm.meter_ID and gt.tasksid=gtl.tasksid and gt.groupid=gtl.group_id);");
            sqlQuery.AppendLine(@"update gsm_task_logs");
            sqlQuery.AppendLine(@"set gsm_task_logs.status='IP'");
            sqlQuery.AppendLine(@"where gsm_task_logs.status='NS'");
            sqlQuery.AppendLine(@"and log_Id in (select log_Id from Temp);");
            sqlQuery.AppendLine(@"Select tasksid 'TASK ID', taskname 'TASK NAME', group_id 'GROUP ID', meter_id 'METER ID', currentjob 'JOB' , Meter_GPRSModem_IMEI 'IMEI', startdate 'START DATE', starttime 'START TIME' , taskretries 'RETRIES COUNT', metermodel_ID 'METER MODEL ID'");
            sqlQuery.AppendLine(@"from Temp ;");
            sqlQuery.AppendLine(@"END");


            // GPRS: to update GPRS sheduling tasks
            sqlQuery.AppendLine(@"~");
            sqlQuery.AppendLine(@"CREATE PROCEDURE DLMS_LTCT_650.`UpdateTask`(meterId VARCHAR(20),taskId INT,");
            sqlQuery.AppendLine(@"groupId BIGINT,status VARCHAR(15),taskretries INT,errorMessage VARCHAR(200),Job VARCHAR(50))");
            sqlQuery.AppendLine(@"BEGIN");
            sqlQuery.AppendLine(@"Declare NotYetExecuted INT DEFAULT -1 ;");
            sqlQuery.AppendLine(@"Declare NextJob Varchar(50) DEFAULT NULL;");
            sqlQuery.AppendLine(@"SELECT SUBSTRING_INDEX(SUBSTR(jobs,INSTR(jobs,JOB)+ Length(JOB)+1),',',1) INTO NextJob FROM gsm_tasks gt");
            sqlQuery.AppendLine(@"where gt.tasksid=taskId and gt.groupid=groupId;");
            sqlQuery.AppendLine(@"IF status='Failed' THEN");
            sqlQuery.AppendLine(@"update gsm_task_logs gtl");
            //Status changed to NC for failed task
            sqlQuery.AppendLine(@"set gtl.status='NC', gtl.taskretries=IF(taskretries >gtl.taskretries ,taskretries,gtl.taskretries), gtl.errorMessage=errorMessage,gtl.creationDateTime = DATE_FORMAT(now() ,'%d/%m/%Y %H:%i:%S')");
            sqlQuery.AppendLine(@"where");
            sqlQuery.AppendLine(@"tasksid=taskId and group_id=groupId and meter_id=meterId;");
            sqlQuery.AppendLine(@"ELSEIF status='Complete' AND Length(NextJob)=0 THEN");
            sqlQuery.AppendLine(@"CASE JOB");
            sqlQuery.AppendLine(@"WHEN 'General' THEN");
            sqlQuery.AppendLine(@"update gsm_task_logs gtl");
            sqlQuery.AppendLine(@"set gtl.status='C', gtl.taskretries=IF(taskretries >gtl.taskretries ,taskretries,gtl.taskretries), gtl.errorMessage=errorMessage, gtl.isGeneralCompleted=1,gtl.creationDateTime = DATE_FORMAT(now() ,'%d/%m/%Y %H:%i:%S')");
            sqlQuery.AppendLine(@"where");
            sqlQuery.AppendLine(@"tasksid=taskId and group_id=groupId and meter_id=meterId;");
            sqlQuery.AppendLine(@"WHEN 'Instantaneous' THEN");
            sqlQuery.AppendLine(@"update gsm_task_logs gtl");
            sqlQuery.AppendLine(@"set ");
            sqlQuery.AppendLine(@"gtl.status='C', gtl.taskretries=IF(taskretries >gtl.taskretries ,taskretries,gtl.taskretries), gtl.errorMessage=errorMessage, gtl.isInstantCompleted=1,gtl.creationDateTime = DATE_FORMAT(now() ,'%d/%m/%Y %H:%i:%S')");
            sqlQuery.AppendLine(@"where");
            sqlQuery.AppendLine(@"tasksid=taskId and group_id=groupId and meter_id=meterId;");
            sqlQuery.AppendLine(@"WHEN 'Billing' THEN");
            sqlQuery.AppendLine(@"update gsm_task_logs gtl");
            sqlQuery.AppendLine(@"set ");
            sqlQuery.AppendLine(@"gtl.status='C', gtl.taskretries=IF(taskretries >gtl.taskretries ,taskretries,gtl.taskretries), gtl.errorMessage=errorMessage, gtl.isBillingCompleted=1,gtl.creationDateTime = DATE_FORMAT(now() ,'%d/%m/%Y %H:%i:%S')");
            sqlQuery.AppendLine(@"where");
            sqlQuery.AppendLine(@"tasksid=taskId and group_id=groupId and meter_id=meterId;");
            sqlQuery.AppendLine(@"WHEN 'Load Survey' THEN");
            sqlQuery.AppendLine(@"update gsm_task_logs gtl");
            sqlQuery.AppendLine(@"set ");
            sqlQuery.AppendLine(@"gtl.status='C', gtl.taskretries=IF(taskretries >gtl.taskretries ,taskretries,gtl.taskretries), gtl.errorMessage=errorMessage, gtl.isLoadSurveyCompleted=1,gtl.creationDateTime = DATE_FORMAT(now() ,'%d/%m/%Y %H:%i:%S')");
            sqlQuery.AppendLine(@"where");
            sqlQuery.AppendLine(@"tasksid=taskId and group_id=groupId and meter_id=meterId;");
            sqlQuery.AppendLine(@"WHEN 'Tamper' THEN");
            sqlQuery.AppendLine(@"update gsm_task_logs gtl");
            sqlQuery.AppendLine(@"set ");
            sqlQuery.AppendLine(@"gtl.status='C', gtl.taskretries=IF(taskretries >gtl.taskretries ,taskretries,gtl.taskretries), gtl.errorMessage=errorMessage, gtl.isTamperCompleted=1,gtl.creationDateTime = DATE_FORMAT(now() ,'%d/%m/%Y %H:%i:%S')");
            sqlQuery.AppendLine(@"where tasksid=taskId and group_id=groupId and meter_id=meterId;");
            sqlQuery.AppendLine(@"end case;");
            sqlQuery.AppendLine(@"ELSEIF status='Complete' AND Length(NextJob)>0 THEN");
            sqlQuery.AppendLine(@"CASE JOB");
            sqlQuery.AppendLine(@"WHEN 'General' THEN");
            sqlQuery.AppendLine(@"update gsm_task_logs gtl");
            sqlQuery.AppendLine(@"set ");
            sqlQuery.AppendLine(@"gtl.status='NS', gtl.taskretries=IF(taskretries >gtl.taskretries ,taskretries,gtl.taskretries), gtl.errorMessage=errorMessage, gtl.isGeneralCompleted=1, gtl.CurrentJob=NextJob,gtl.creationDateTime = DATE_FORMAT(now() ,'%d/%m/%Y %H:%i:%S')");
            sqlQuery.AppendLine(@"where tasksid=taskId and group_id=groupId and meter_id=meterId;");
            sqlQuery.AppendLine(@"WHEN 'Instantaneous' THEN");
            sqlQuery.AppendLine(@"update gsm_task_logs gtl");
            sqlQuery.AppendLine(@"set ");
            sqlQuery.AppendLine(@"gtl.status='NS', gtl.taskretries=IF(taskretries >gtl.taskretries ,taskretries,gtl.taskretries), gtl.errorMessage=errorMessage, gtl.isInstantCompleted=1, gtl.CurrentJob=NextJob,gtl.creationDateTime = DATE_FORMAT(now() ,'%d/%m/%Y %H:%i:%S')");
            sqlQuery.AppendLine(@"where tasksid=taskId and group_id=groupId and meter_id=meterId;");
            sqlQuery.AppendLine(@"WHEN 'Billing' THEN");
            sqlQuery.AppendLine(@"update gsm_task_logs gtl");
            sqlQuery.AppendLine(@"set ");
            sqlQuery.AppendLine(@"gtl.status='NS', gtl.taskretries=IF(taskretries >gtl.taskretries ,taskretries,gtl.taskretries), gtl.errorMessage=errorMessage, gtl.isBillingCompleted=1, gtl.CurrentJob=NextJob,gtl.creationDateTime = DATE_FORMAT(now() ,'%d/%m/%Y %H:%i:%S')");
            sqlQuery.AppendLine(@"where tasksid=taskId and group_id=groupId and meter_id=meterId;");
            sqlQuery.AppendLine(@"WHEN 'Load Survey' THEN");
            sqlQuery.AppendLine(@"update gsm_task_logs gtl");
            sqlQuery.AppendLine(@"set ");
            sqlQuery.AppendLine(@"gtl.status='NS', gtl.taskretries=IF(taskretries >gtl.taskretries ,taskretries,gtl.taskretries), gtl.errorMessage=errorMessage, gtl.isLoadSurveyCompleted=1, gtl.CurrentJob=NextJob,gtl.creationDateTime = DATE_FORMAT(now() ,'%d/%m/%Y %H:%i:%S')");
            sqlQuery.AppendLine(@"where tasksid=taskId and group_id=groupId and meter_id=meterId;");
            sqlQuery.AppendLine(@"WHEN 'Tamper' THEN");
            sqlQuery.AppendLine(@"update gsm_task_logs gtl");
            sqlQuery.AppendLine(@"set ");
            sqlQuery.AppendLine(@"gtl.status='NS', gtl.taskretries=IF(taskretries >gtl.taskretries ,taskretries,gtl.taskretries), gtl.errorMessage=errorMessage, gtl.isTamperCompleted=1, gtl.CurrentJob=NextJob,gtl.creationDateTime = DATE_FORMAT(now() ,'%d/%m/%Y %H:%i:%S')");
            sqlQuery.AppendLine(@"where tasksid=taskId and group_id=groupId and meter_id=meterId;");
            sqlQuery.AppendLine(@"end case;");
            sqlQuery.AppendLine(@"ELSEIF status='InProgress' THEN");
            sqlQuery.AppendLine(@"update gsm_task_logs gtl");
            sqlQuery.AppendLine(@"set ");
            sqlQuery.AppendLine(@"gtl.taskretries=taskretries, gtl.errorMessage=errorMessage");
            sqlQuery.AppendLine(@"where tasksid=taskId and group_id=groupId and meter_id=meterId;");
            sqlQuery.AppendLine(@"END IF;");
            sqlQuery.AppendLine(@"IF NOT EXISTS");
            sqlQuery.AppendLine(@"(SELECT * FROM `dlms_ltct_650`.`gsm_task_logs` gtl");
            //Add condition for NC also 
            sqlQuery.AppendLine(@"where gtl.group_id=groupid and gtl.tasksid=taskid and gtl.status!='C' and gtl.status!='NC') THEN");
            sqlQuery.AppendLine(@"INSERT INTO `dlms_ltct_650`.`gsm_tasks_completed` (`tasksId`, `taskName`, `creationDateTime`, `groupId`, `startDate`, `startTime`, `taskType`, `repeatTask`, `jobs`, `taskRetries`, `taskPriority`, `taskStatus`)");
            sqlQuery.AppendLine(@"SELECT `gsm_tasks`.`tasksId`, `gsm_tasks`.`taskName`, DATE_FORMAT(now() ,'%d/%m/%Y %H:%i:%S'), `gsm_tasks`.`groupId`, `gsm_tasks`.`startDate`, `gsm_tasks`.`startTime`, `gsm_tasks`.`taskType`, `gsm_tasks`.`repeatTask`, `gsm_tasks`.`jobs`, `gsm_tasks`.`taskRetries`, `gsm_tasks`.`taskPriority`,  'Completed'");
            sqlQuery.AppendLine(@"FROM `dlms_ltct_650`.`gsm_tasks`");
            sqlQuery.AppendLine(@"where `gsm_tasks`.`tasksId`=taskId AND `gsm_tasks`.`groupId`=groupId;");
            sqlQuery.AppendLine(@"SELECT * FROM `dlms_ltct_650`.`gsm_tasks` where `gsm_tasks`.`tasksId`=taskId AND `gsm_tasks`.`groupId`=groupId;");
            sqlQuery.AppendLine(@"END IF;");
            sqlQuery.AppendLine(@"END");


            // GPRS: to update endpoints syncing status
            sqlQuery.AppendLine(@"~");
            sqlQuery.AppendLine(@"CREATE PROCEDURE DLMS_LTCT_650.`BulkUpdateEndPointSyncStatus`(meterIdsList VARCHAR(2000),count INT)");
            sqlQuery.AppendLine(@"BEGIN");
            sqlQuery.AppendLine(@"WHILE count > 0 DO");
            sqlQuery.AppendLine(@"update consumermeter set IsSyncedWithGPRSAdapter=1");
            sqlQuery.AppendLine(@"where");
            sqlQuery.AppendLine(@"meter_id = ExtractValue(meterIdsList, '//meterid[$count]');");
            sqlQuery.AppendLine(@"SET count = count-1;");
            sqlQuery.AppendLine(@"END WHILE;");
            sqlQuery.AppendLine(@"END");



            sqlQuery.AppendLine(@"~");
            sqlQuery.AppendLine(@"CREATE PROCEDURE DLMS_LTCT_650.`ReQueueFailedTasks`()");
            sqlQuery.AppendLine(@"BEGIN");
            sqlQuery.AppendLine(@"DELETE from gsm_task_logs where tasksid in(select tasksid from gsm_tasks,gsm_group_meters, gsm_groups");
            sqlQuery.AppendLine(@"where gsm_groups.CommunicationType='GPRS' and gsm_groups.group_id=gsm_tasks.groupid and gsm_tasks.taskstatus='InProgress' and");
            sqlQuery.AppendLine(@"STR_TO_DATE(CONCAT(gsm_tasks.startdate,' ',gsm_tasks.starttime),'%d/%m/%Y %H:%i:%S')<=NOW()");
            sqlQuery.AppendLine(@"and gsm_tasks.groupid=gsm_group_meters.group_ID and gsm_group_meters.status='S' );");
            sqlQuery.AppendLine(@"update  gsm_tasks ");
            sqlQuery.AppendLine(@"JOIN gsm_group_meters, gsm_groups");
            sqlQuery.AppendLine(@"SET gsm_tasks.taskstatus='Inqueue'");
            sqlQuery.AppendLine(@"where gsm_groups.CommunicationType='GPRS' and gsm_groups.group_id=gsm_tasks.groupid and gsm_tasks.taskstatus='InProgress' and");
            sqlQuery.AppendLine(@"STR_TO_DATE(CONCAT(gsm_tasks.startdate,' ',gsm_tasks.starttime),'%d/%m/%Y %H:%i:%S')<=NOW()");
            sqlQuery.AppendLine(@"and gsm_tasks.groupid=gsm_group_meters.group_ID and gsm_group_meters.status='S' ;");
            sqlQuery.AppendLine(@"END");

            //New procedure added for get File upload status
            sqlQuery.AppendLine(@"~");
            sqlQuery.AppendLine(@"CREATE PROCEDURE DLMS_LTCT_650.`GetFileUploadDetails`(meterDataId int)");
            sqlQuery.AppendLine(@"BEGIN");
            sqlQuery.AppendLine(@"DECLARE General INT DEFAULT 0;");
            sqlQuery.AppendLine(@"DECLARE Instant INT DEFAULT 0;");
            sqlQuery.AppendLine(@"DECLARE Billing INT DEFAULT 0;");
            sqlQuery.AppendLine(@"IF EXISTS (SELECT  * FROM `meterdata_general` where MeterData_Id = meterDataId limit 0,1)");
            sqlQuery.AppendLine(@"THEN ");
            sqlQuery.AppendLine(@"    SET General = 1;");
            sqlQuery.AppendLine(@"END IF;");
            sqlQuery.AppendLine(@"IF EXISTS (SELECT * FROM `meterdata_instantpower` where MeterData_Id = meterDataId limit 0,1)");
            sqlQuery.AppendLine(@"THEN ");
            sqlQuery.AppendLine(@"    SET Instant = 1;");
            sqlQuery.AppendLine(@"END IF;");
            sqlQuery.AppendLine(@"IF EXISTS (SELECT * FROM `meterdata_billing` where MeterData_Id = meterDataId limit 0,1)");
            sqlQuery.AppendLine(@"THEN ");
            sqlQuery.AppendLine(@"    SET Billing = 1;");
            sqlQuery.AppendLine(@"END IF;");
            sqlQuery.AppendLine(@"Select General as 'General', Instant as 'Instant' , Billing as 'Billing';");
            sqlQuery.AppendLine(@"END");

            sqlQuery.AppendLine(@"~");
            sqlQuery.AppendLine(@"CREATE PROCEDURE DLMS_LTCT_650.`InsertSchedulingColumns`(xml text)");
            sqlQuery.AppendLine(@"BEGIN ");
            sqlQuery.AppendLine(@"Declare count int;");
            sqlQuery.AppendLine(@"set count := extractValue(xml,'count(/Columns/Column)');");
            sqlQuery.AppendLine(@"WHILE count > 0 DO");
            sqlQuery.AppendLine(@"Insert into schedulingreportscolumn(Profile,DisplayName,MappedDBColumn,Utility,IsSelected) ");
            sqlQuery.AppendLine(@"values (ExtractValue(xml, '/descendant-or-self::Profile[$count]'),ExtractValue(xml, '/descendant-or-self::DisplayName[$count]')");
            sqlQuery.AppendLine(@",ExtractValue(xml, '/descendant-or-self::DBName[$count]'),ExtractValue(xml, '/descendant-or-self::Utility[$count]'),1);");
            sqlQuery.AppendLine(@"SET count = count-1;");
            sqlQuery.AppendLine(@"END WHILE;");
            sqlQuery.AppendLine(@"END");

            sqlQuery.AppendLine(@"~");
            sqlQuery.AppendLine(@"CREATE PROCEDURE DLMS_LTCT_650.`GetReportColumns`(profile varchar(20),utility varchar(50))");
            sqlQuery.AppendLine(@"BEGIN ");
            sqlQuery.AppendLine(@"Select DisplayName,MappedDBColumn,IsSelected,Identifier from schedulingreportscolumn where");
            sqlQuery.AppendLine(@"schedulingreportscolumn.Profile=profile ");
            sqlQuery.AppendLine(@"and (schedulingreportscolumn.Utility='' || UPPER(schedulingreportscolumn.Utility)=UPPER(utility));");
            sqlQuery.AppendLine(@"END");

            sqlQuery.AppendLine(@"~");
            sqlQuery.AppendLine(@"CREATE PROCEDURE DLMS_LTCT_650.`UpdateParametersSelection`(AvailableItemsXml text,SelectedItemsXml text,ProfileSelected varchar(50))");
            sqlQuery.AppendLine(@"BEGIN");
            sqlQuery.AppendLine(@"");
            sqlQuery.AppendLine(@"Declare count int;");
            sqlQuery.AppendLine(@"");
            sqlQuery.AppendLine(@"set count := extractValue(AvailableItemsXml,'count(/ArrayOfParameters/Parameters)');");
            sqlQuery.AppendLine(@"");
            sqlQuery.AppendLine(@"");
            sqlQuery.AppendLine(@"WHILE count > 0 DO");
            sqlQuery.AppendLine(@"Update schedulingreportscolumn");
            sqlQuery.AppendLine(@"set isselected=0");
            sqlQuery.AppendLine(@"where");
            sqlQuery.AppendLine(@"schedulingreportscolumn.identifier=CONVERT(ExtractValue(AvailableItemsXml, '/descendant-or-self::Id[$count]'),UNSIGNED INTEGER)");
            sqlQuery.AppendLine(@"and");
            sqlQuery.AppendLine(@"UPPER(schedulingreportscolumn.Profile)=UPPER(ProfileSelected);");
            sqlQuery.AppendLine(@"");
            sqlQuery.AppendLine(@"SET count = count-1;");
            sqlQuery.AppendLine(@"END WHILE;");
            sqlQuery.AppendLine(@"");
            sqlQuery.AppendLine(@"");
            sqlQuery.AppendLine(@"set count := extractValue(SelectedItemsXml,'count(/ArrayOfParameters/Parameters)');");
            sqlQuery.AppendLine(@"");
            sqlQuery.AppendLine(@"");
            sqlQuery.AppendLine(@"WHILE count > 0 DO");
            sqlQuery.AppendLine(@"Update schedulingreportscolumn");
            sqlQuery.AppendLine(@"set isselected=1");
            sqlQuery.AppendLine(@"where");
            sqlQuery.AppendLine(@"schedulingreportscolumn.identifier=CONVERT(ExtractValue(SelectedItemsXml, '/descendant-or-self::Id[$count]'),UNSIGNED INTEGER)");
            sqlQuery.AppendLine(@"and");
            sqlQuery.AppendLine(@"UPPER(schedulingreportscolumn.Profile)=UPPER(ProfileSelected);");
            sqlQuery.AppendLine(@"");
            sqlQuery.AppendLine(@"SET count = count-1;");
            sqlQuery.AppendLine(@"END WHILE;");
            sqlQuery.AppendLine(@"");
            sqlQuery.AppendLine(@"END");

            /* VBM GetTamperDetailByTamperType */

            /* VBM create SP  GetBillingDemand */
            //sqlQuery.AppendLine(@"$");
            //sqlQuery.AppendLine(@"CREATE  PROCEDURE " + apptype + ".`GetBillingDemand`(MeterData_ID INT)");
            //sqlQuery.AppendLine(@"BEGIN");
            //sqlQuery.AppendLine(@"SELECT BL.DataIndex,BL.BillingDate,BL.MDkWTZ0,LS.BlockEnergykvah,BL.MDkWDateTimeTZ0,");
            //sqlQuery.AppendLine(@"BL.MDkVATZ0,LS.BlockEnergykwh,BL.MDkVADateTimeTZ0");
            //sqlQuery.AppendLine(@"FROM `dlms_ltct_650`.`meterdata_billing` BL");
            //sqlQuery.AppendLine(@"inner join `dlms_ltct_650`.`meterdata_loadsurvey` LS ");
            //sqlQuery.AppendLine(@"on BL.meterdata_id = LS.meterdata_id where BL.meterdata_id=MeterData_ID and");
            //sqlQuery.AppendLine(@"(LS.realtimeclockdateandtime=BL.mdkwdatetimetz0 OR LS.realtimeclockdateandtime=BL.mdkvadatetimetz0);");
            //sqlQuery.AppendLine(@"END");
            /* VBM GetBillingDemand */

            //Procedure to get Search data for Task id 
            sqlQuery.AppendLine(@"~");
            sqlQuery.AppendLine(@"CREATE PROCEDURE DLMS_LTCT_650.`GetGPRSSearchData`(searchBy varchar(20), startDate varchar(50), endDate varchar(50))");
            sqlQuery.AppendLine(@"BEGIN");
            sqlQuery.AppendLine(@"If searchBy = 'SCHEDULE' Then");
            sqlQuery.AppendLine(@"    select ");
            sqlQuery.AppendLine(@"        TasksId as 'Id',");
            sqlQuery.AppendLine(@"        TaskName as 'Schedule Name',");
            sqlQuery.AppendLine(@"        concat(gtc.StartDate , ' ' , gtc.StartTime) as 'Schedule Date',");
            sqlQuery.AppendLine(@"        creationDateTime as 'Completion Date',");
            sqlQuery.AppendLine(@"        TaskType as 'Type',");
            sqlQuery.AppendLine(@"        Group_Name as 'Group Name',");
            sqlQuery.AppendLine(@"        Jobs as 'Profiles',");
            sqlQuery.AppendLine(@"        Communicationtype as 'Communication Type'");
            sqlQuery.AppendLine(@"    from gsm_tasks_completed gtc");
            sqlQuery.AppendLine(@"    inner Join gsm_groups grps on gtc.groupid = grps.group_id");
            sqlQuery.AppendLine(@"    Where STR_TO_DATE(creationDateTime,'%d/%m/%Y') between STR_TO_DATE(startDate,'%d/%m/%Y') and STR_TO_DATE(endDate,'%d/%m/%Y')");
            sqlQuery.AppendLine(@"    and CommunicationType in ('GPRS','TCP');");
            sqlQuery.AppendLine(@"END If ;");
            sqlQuery.AppendLine(@"IF searchBy = 'METER' Then");
            sqlQuery.AppendLine(@"");
            sqlQuery.AppendLine(@"    Select Distinct");
            sqlQuery.AppendLine(@"        MeterID as 'Meter Id',");
            sqlQuery.AppendLine(@"        TaskName as 'Schedule Name',");
            sqlQuery.AppendLine(@"        concat(gtc.StartDate , ' ' , gtc.StartTime) as 'Schedule Date',");
            sqlQuery.AppendLine(@"        creationDateTime as 'Completion Date',");
            sqlQuery.AppendLine(@"        TaskType as 'Type',");
            sqlQuery.AppendLine(@"        Group_Name as 'Group Name',");
            sqlQuery.AppendLine(@"        jobs as 'Profiles',");
            sqlQuery.AppendLine(@"       fm.FileName as 'File Name',");
            sqlQuery.AppendLine(@"       Communicationtype as 'Communication Type',");
            sqlQuery.AppendLine(@"       md.FileUpload_Id as 'File Upload Id',");
            sqlQuery.AppendLine(@"       TasksId as 'Id'");
            sqlQuery.AppendLine(@"    from meterData md");
            sqlQuery.AppendLine(@"    Inner Join gsm_tasks_completed gtc on md.taskid = gtc.tasksid");
            sqlQuery.AppendLine(@"    inner Join gsm_groups grps on gtc.groupid = grps.group_id");
            sqlQuery.AppendLine(@"    Inner Join fileupload_master fm on fm.FileUpload_ID = md.FileUpload_Id");
            sqlQuery.AppendLine(@"    Where STR_TO_DATE(creationDateTime,'%d/%m/%Y') between STR_TO_DATE(startDate,'%d/%m/%Y') and STR_TO_DATE(endDate,'%d/%m/%Y')");
            sqlQuery.AppendLine(@"    and CommunicationType in ('GPRS','TCP');");
            sqlQuery.AppendLine(@"End If;");
            sqlQuery.AppendLine(@"END");

            // Excel Export: Get Data For ExcelExport
            sqlQuery.AppendLine(@"~");
            sqlQuery.AppendLine(@"CREATE PROCEDURE DLMS_LTCT_650.`GetDataForExcelExport`(meterDataId long)");
            sqlQuery.AppendLine(@"BEGIN");
            sqlQuery.AppendLine(@"SELECT  general.meterSerialNumber as 'Meter Serial No.',general.metertype,fileUpload.ReadingDateTime as 'Meter Read Date & Time',");
            sqlQuery.AppendLine(@"billing.CumulativeEnergykWhTZ0 as 'Abs Active Energy (Kwh)(Current)',billing.MDkWTZ0 as 'Abs Active (MD) Kw',");
            sqlQuery.AppendLine(@"billing.CumulativeEnergykVAhTZ0 as 'Abs Apparent Energy (KVAh)( Current )',billing.MDkVATZ0 as 'Abs Apparent (MD) KVA', billing.CumulativeEnergykVAhTZ1 as 'KVAH(TOD1)',billing.CumulativeEnergykVAhTZ2 as 'KVAH(TOD2)',");
            sqlQuery.AppendLine(@"billing.CumulativeEnergykVAhTZ3 as 'KVAH(TOD3)',billing.CumulativeEnergykVAhTZ4 as 'KVAH(TOD4)',billing.CumulativeEnergykVAhTZ5 as 'KVAH(TOD5)',billing.CumulativeEnergykVAhTZ6 as 'KVAH(TOD6)',billing.CumulativeEnergykVAhTZ7 as 'KVAH(TOD7)',billing.CumulativeEnergykVAhTZ8 as 'KVAH(TOD8)' FROM ");
            sqlQuery.AppendLine(@"meterdata_billing billing,meterdata_general general,meterData fileUpload WHERE ");
            sqlQuery.AppendLine(@"billing.MeterData_ID=meterDataId AND billing.DataIndex = 0 AND billing.MeterData_ID = general.MeterData_ID AND billing.MeterData_ID = fileUpload.MeterData_ID ;");
            sqlQuery.AppendLine(@"END");

            // Custom Text File Export
            sqlQuery.AppendLine(@"~");
            sqlQuery.AppendLine(@"CREATE PROCEDURE DLMS_LTCT_650.`GetDataForTextExport`(MeterDataID varchar(20))");
            sqlQuery.AppendLine(@"BEGIN");
            sqlQuery.AppendLine(@"SELECT  general.meterSerialNumber AS 'Meter Serial No',fileUpload.ReadingDateTime, ");
            sqlQuery.AppendLine(@"d.InstantPowerColumnValue AS 'No.of Reset' ,e.InstantPowerColumnValue AS 'RN_Voltage',  ");
            sqlQuery.AppendLine(@"f.InstantPowerColumnValue AS 'YN_Voltage' ,g.InstantPowerColumnValue AS 'BN_Voltage' ,h.InstantPowerColumnValue AS 'RN_Current' ,i.InstantPowerColumnValue AS 'YN_Current',j.InstantPowerColumnValue AS 'BN_Current', ");
            sqlQuery.AppendLine(@"k.InstantPowerColumnValue AS 'Cumulative KW' ,l.InstantPowerColumnValue AS 'Cumulative KVA',o.InstantPowerColumnValue AS 'Cumulative KWH', ");
            sqlQuery.AppendLine(@"m.InstantPowerColumnValue AS 'Cumulative KVAH' ,n.InstantPowerColumnValue AS 'Cumulative RKVAH' , ");
            sqlQuery.AppendLine(@"billing.MDkWTZ1, billing.MDkVATZ1, billing.CumulativeEnergykWhTZ1, billing.CumulativeEnergykVAhTZ1,");
            sqlQuery.AppendLine(@"billing.MDkWTZ2, billing.MDkVATZ2, billing.CumulativeEnergykWhTZ2, billing.CumulativeEnergykVAhTZ2,");
            sqlQuery.AppendLine(@"billing.MDkWTZ3, billing.MDkVATZ3, billing.CumulativeEnergykWhTZ3, billing.CumulativeEnergykVAhTZ3,");
            sqlQuery.AppendLine(@"billing.MDkWTZ4, billing.MDkVATZ4, billing.CumulativeEnergykWhTZ4, billing.CumulativeEnergykVAhTZ4 FROM ");
            sqlQuery.AppendLine(@"meterdata_billing billing,meterdata_general general,meterData fileUpload,meterdata_instantpower d ,meterdata_instantpower e, ");
            sqlQuery.AppendLine(@"meterdata_instantpower f,meterdata_instantpower g,meterdata_instantpower h,meterdata_instantpower i,meterdata_instantpower j,meterdata_instantpower k,meterdata_instantpower l, ");
            sqlQuery.AppendLine(@"meterdata_instantpower m,meterdata_instantpower n,meterdata_instantpower o WHERE ");
            sqlQuery.AppendLine(@"billing.DataIndex = 0 AND billing.MeterData_ID = general.MeterData_ID AND billing.MeterData_ID = fileUpload.MeterData_ID AND billing.MeterData_ID = d.MeterData_ID AND billing.MeterData_ID = e.MeterData_ID AND ");
            sqlQuery.AppendLine(@"billing.MeterData_ID = f.MeterData_ID AND billing.MeterData_ID = g.MeterData_ID AND billing.MeterData_ID = h.MeterData_ID AND billing.MeterData_ID = i.MeterData_ID AND billing.MeterData_ID = j.MeterData_ID AND billing.MeterData_ID = k.MeterData_ID AND ");
            sqlQuery.AppendLine(@"billing.MeterData_ID = l.MeterData_ID AND billing.MeterData_ID = m.MeterData_ID AND billing.MeterData_ID = n.MeterData_ID AND billing.MeterData_ID = o.MeterData_ID AND ");
            sqlQuery.AppendLine(@"d.InstantPowerColumnName = 'Cumulative Billing Count' AND e.InstantPowerColumnName ='Voltage - VRN'  AND f.InstantPowerColumnName ='Voltage - VYN'  AND ");
            sqlQuery.AppendLine(@"g.InstantPowerColumnName ='Voltage - VBN'  AND h.InstantPowerColumnName ='Current - IR' AND i.InstantPowerColumnName ='Current - IY'AND j.InstantPowerColumnName ='Current - IB' AND k.InstantPowerColumnName ='Maximum Demand kW' AND ");
            sqlQuery.AppendLine(@"l.InstantPowerColumnName ='Maximum Demand kVA' AND m.InstantPowerColumnName ='Cumulative Energy kVAh' AND n.InstantPowerColumnName ='Cumulative Energy kvarh Lag' AND ");
            sqlQuery.AppendLine(@"o.InstantPowerColumnName ='Cumulative Energy kWh Import' AND ");//pks
            sqlQuery.AppendLine(@"fileUpload.MeterData_ID=MeterDataID; ");
            sqlQuery.AppendLine(@"END");

             
            //Get Data for GPRS TCP FTP Communication Logs
            sqlQuery.AppendLine(@"~");
            sqlQuery.AppendLine(@"CREATE PROCEDURE `GETRemoteCommunicationLogs`(startDate varchar(50), endDate varchar(50))");
            sqlQuery.AppendLine(@"BEGIN");
            sqlQuery.AppendLine(@"SELECT Meter_ID,TasksId as 'Iteration',");        
            sqlQuery.AppendLine(@"case Status when 'C' then 'COMPLETED' else 'NOT COMPLETED' end as 'Status',");
            sqlQuery.AppendLine(@"taskRetries as 'Attempt',");     
            sqlQuery.AppendLine(@"creationDateTime as 'Completion Date',");       
            sqlQuery.AppendLine(@"Group_Name as 'Group Name',");       
            sqlQuery.AppendLine(@"Communicationtype as 'Communication Type',");
            sqlQuery.AppendLine(@"errorMessage as 'Result'");
            sqlQuery.AppendLine(@"from dlms_ltct_650.gsm_task_logs gtl");
            sqlQuery.AppendLine(@"inner Join"); 
            sqlQuery.AppendLine(@"dlms_ltct_650.gsm_groups grps"); 
            sqlQuery.AppendLine(@"on gtl.GROUP_ID = grps.group_id");
            sqlQuery.AppendLine(@"Where STR_TO_DATE(creationDateTime,'%d/%m/%Y')"); 
            sqlQuery.AppendLine(@"between STR_TO_DATE(startDate,'%d/%m/%Y')"); 
            sqlQuery.AppendLine(@"and STR_TO_DATE(endDate,'%d/%m/%Y')");
            sqlQuery.AppendLine(@"and COALESCE(errorMessage, '') != ''");
            sqlQuery.AppendLine(@"and CommunicationType in ('GPRS','TCP','FTP');");
            sqlQuery.AppendLine(@"END");




            return sqlQuery.ToString();

        }

        private string GetTableCreationQuery()
        {
            StringBuilder sqlQuery = new StringBuilder();
            sqlQuery.Append(@"CREATE SCHEMA IF NOT EXISTS `" + apptype + "`;");
            sqlQuery.Append(@"USE `" + apptype + "`;");

            #region "Common"
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
            //added for MVVNL
            sqlQuery.Append(@"`MidnightEnergiesColumn` longtext,");
            sqlQuery.Append(@"`MidnightEnergiesDBColumn` longtext,");

            sqlQuery.Append(@"`SelfDiagnosticsColumn` longtext,");
            sqlQuery.Append(@"`SelfDiagnosticsDBColumn` longtext,");
            //added for MVVNL
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
            //if (ConfigInfo.GetTenderType() == TenderType.JUSCO)
            //{
            sqlQuery.Append(@"`Region_ID` Bigint(20) NOT NULL,");

            //}
            sqlQuery.Append(@"PRIMARY KEY (`Circle_ID`),");
            //Added to avoid duplicate entries.
            sqlQuery.Append(@"UNIQUE KEY(`Circle_Name`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");


            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `cmri_master` (");
            sqlQuery.Append(@"`CMRI_ID` bigint(20) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`CMRI_Number` varchar(16) NOT NULL,");
            sqlQuery.Append(@"`CMRI_Description` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`CMRI_ID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");


            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `consumerexportsettings` (");
            sqlQuery.Append(@"`ConsumerExportSettings_ID` bigint(20) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`FileName` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`ParametersName` varchar(1000) DEFAULT NULL,");
            sqlQuery.Append(@"`ParameterColumn` varchar(2000) DEFAULT NULL,");//data limit changed from 1500 to 2000 to resolve bug 73549; 11th April 2012 
            sqlQuery.Append(@"PRIMARY KEY (`ConsumerExportSettings_ID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");


            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `consumermeter` (");
            sqlQuery.Append(@"`ConsumerMeter_ID` int(11) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`Meter_ID` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`Consumer_Number` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`Meter_AllocationDate` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`Meter_Location` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`Status` int(11) DEFAULT NULL,");
            //if (TenderType.JUSCO == ConfigInfo.GetTenderType())
            //{
            sqlQuery.Append(@"`Region_ID` Bigint(20) NOT NULL,");
            sqlQuery.Append(@"`Circle_ID` Bigint(20) NOT NULL,");
            sqlQuery.Append(@"`Division_ID` Bigint(20) NOT NULL,");
            // GPRS: new column to support endpoint syncing
            sqlQuery.Append(@"`IsSyncedWithGPRSAdapter` tinyint(1) DEFAULT NULL,");
            sqlQuery.Append(@"`Communication_Type` varchar(20) DEFAULT NULL,");

            //}
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
            //if (ConfigInfo.GetTenderType() == TenderType.JUSCO)
            //{
            sqlQuery.Append(@"`Region_ID` Bigint(20) NOT NULL,");
            sqlQuery.Append(@"`Circle_ID` Bigint(20) NOT NULL,");

            //}
            sqlQuery.Append(@"PRIMARY KEY (`Division_ID`),");
            //Added to avoid duplicate entries.
            sqlQuery.Append(@"UNIQUE KEY(`Division_Name`)");
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
            sqlQuery.Append(@"`FileUpload_ID` bigint(150) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`UploadingDateTime` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`FileContent` longblob,");
            sqlQuery.Append(@"`UserInformation_ID` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"`FileName` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`readingDateTime` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`FileType` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`CommType` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"`FileSize` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`CMRI_Number` varchar(20) DEFAULT NULL,");
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

            //if (ConfigInfo.GetTenderType() == TenderType.JUSCO)
            //{
            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `gsm_groups` (");
            sqlQuery.Append(@"`Group_ID` bigint(20) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`Group_Name` varchar(50) NOT NULL,");
            sqlQuery.Append(@"`Group_Type` varchar(1) NOT NULL,");
            sqlQuery.Append(@"`Region_ID` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"`Circle_ID` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"`Division_ID` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"`CommunicationType` varchar(10) DEFAULT NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`Group_ID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");

            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `gsm_task_logs` (");
            sqlQuery.Append(@"`Log_ID` bigint(20) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`tasksId` Int NOT NULL,");
            sqlQuery.Append(@"`Group_ID` bigint(20) NOT NULL,");
            sqlQuery.Append(@"`Meter_ID` varchar(20) NOT NULL,");
            sqlQuery.Append(@"`isGeneralCompleted` int(1) NOT NULL,");
            sqlQuery.Append(@"`isInstantCompleted` int(1) NOT NULL,");
            sqlQuery.Append(@"`isBillingCompleted` int(1) NOT NULL,");
            //adding the columns for load survey and tamper logs
            sqlQuery.Append(@"`isLoadSurveyCompleted` int(1) NOT NULL,");
            sqlQuery.Append(@"`isTamperCompleted` int(1) NOT NULL,");
            sqlQuery.Append(@"`isDailyLoadCompleted` int(1) NOT NULL,");
            sqlQuery.Append(@"`Status` varchar(15) NOT NULL,");
            sqlQuery.Append(@"`taskRetries` INT NOT NULL ,");
            sqlQuery.Append(@"`creationDateTime` VARCHAR(30) NOT NULL ,");
            sqlQuery.Append(@"`currentJob` VARCHAR(50) NULL ,");
            sqlQuery.Append(@"`errorMessage` VARCHAR(200),");
            sqlQuery.Append(@"PRIMARY KEY (`Log_ID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");

            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `gsm_group_meters` (");
            sqlQuery.Append(@"`Group_Meter_ID` bigint(20) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`Group_ID` bigint(20) NOT NULL,");
            sqlQuery.Append(@"`Meter_ID` varchar(20) NOT NULL,");
            sqlQuery.Append(@"`Status` varchar(1) NOT NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`Group_Meter_ID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");

            sqlQuery.Append(@"CREATE  TABLE IF NOT EXISTS `gsm_tasks` (");
            sqlQuery.Append(@"`tasksId` INT NOT NULL AUTO_INCREMENT , ");
            sqlQuery.Append(@"`taskName` VARCHAR(100) NOT NULL ,");
            sqlQuery.Append(@"`creationDateTime` VARCHAR(30) NOT NULL ,");
            sqlQuery.Append(@"`groupId` INT NOT NULL ,");
            sqlQuery.Append(@"`startDate` VARCHAR(10) NOT NULL ,");
            sqlQuery.Append(@"`startTime` VARCHAR(5) NOT NULL ,");
            sqlQuery.Append(@"`taskType` VARCHAR(50) NOT NULL ,");
            sqlQuery.Append(@"`repeatTask` VARCHAR(200) NOT NULL ,");
            sqlQuery.Append(@"`jobs` VARCHAR(100) NOT NULL ,");
            sqlQuery.Append(@"`jobdetails` VARCHAR(500) NULL ,");
            sqlQuery.Append(@"`taskRetries` INT NOT NULL ,");
            sqlQuery.Append(@"`taskPriority` INT NULL ,");
            sqlQuery.Append(@"`taskStatus` VARCHAR(12) NOT NULL ,");
            sqlQuery.Append(@"PRIMARY KEY (`tasksId`) ,");
            sqlQuery.Append(@"UNIQUE INDEX `taskName_UNIQUE` (`taskName` ASC)) ");
            sqlQuery.Append(@"ENGINE=InnoDB DEFAULT CHARSET=utf8;");

            //adding the table for load survey task information
            sqlQuery.Append(@"CREATE  TABLE IF NOT EXISTS `gsm_tasks_LoadSurvey` (");
            sqlQuery.Append(@"`tasks_LoadSurveyID` INT NOT NULL AUTO_INCREMENT , ");
            sqlQuery.Append(@"`tasksId` INT NOT NULL ,");
            sqlQuery.Append(@"`LoadSurvey_FromDate` bigint(20) NOT NULL,");
            sqlQuery.Append(@"`LoadSurvey_ToDate` bigint(20) NOT NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`tasks_LoadSurveyID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");

            //ashish
            sqlQuery.Append(@"CREATE  TABLE IF NOT EXISTS `gsm_tasks_completed` (");
            sqlQuery.Append(@"`tasksId` INT NOT NULL , ");
            sqlQuery.Append(@"`taskName` VARCHAR(100) NOT NULL ,");
            sqlQuery.Append(@"`creationDateTime` VARCHAR(30) NOT NULL ,");
            sqlQuery.Append(@"`groupId` INT NOT NULL ,");
            sqlQuery.Append(@"`startDate` VARCHAR(10) NOT NULL ,");
            sqlQuery.Append(@"`startTime` VARCHAR(5) NOT NULL ,");
            sqlQuery.Append(@"`taskType` VARCHAR(50) NOT NULL ,");
            sqlQuery.Append(@"`repeatTask` VARCHAR(200) NOT NULL ,");
            sqlQuery.Append(@"`jobs` VARCHAR(100) NOT NULL ,");
            sqlQuery.Append(@"`jobdetails` VARCHAR(500) NULL ,");
            sqlQuery.Append(@"`taskRetries` INT NOT NULL ,");
            sqlQuery.Append(@"`taskPriority` INT NULL ,");
            sqlQuery.Append(@"`taskStatus` VARCHAR(12) NOT NULL ,");
            sqlQuery.Append(@"PRIMARY KEY (`tasksId`))");
            sqlQuery.Append(@"ENGINE=InnoDB DEFAULT CHARSET=utf8;");

            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `gsmreadingstatus` (");
            sqlQuery.Append(@"`GSMReadingStatus_ID` bigint(20) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`ReadingDateTime` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`StatusMessage` varchar(5000) DEFAULT NULL,");
            sqlQuery.Append(@"`FileName` varchar(150) DEFAULT NULL,");
            sqlQuery.Append(@"`FilePath` varchar(500) DEFAULT NULL,");
            sqlQuery.Append(@"`GSMSchedule_ID` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`GSMGroupSchedule_ID` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`Meter_ID` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`SchedulePeriod` varchar(5) DEFAULT NULL,");
            sqlQuery.Append(@"`Status` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`GSMReadingStatus_ID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");


            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `gsmschedule` (");
            sqlQuery.Append(@"`gsmSchedule_ID` bigint(20) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`Schedule_Name` varchar(80) DEFAULT NULL,");
            sqlQuery.Append(@"`Schedule_Period` varchar(2) DEFAULT NULL,");
            sqlQuery.Append(@"`Period_DayName` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`Period_DayNumber` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"`ActivationDate` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`ActivationTime` varchar(10) DEFAULT NULL,");
            sqlQuery.Append(@"`CreationDate` bigint(20) DEFAULT NULL,");
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


            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `billingparameter` (");
            sqlQuery.Append(@"`ColumnsNames` varchar(5000) DEFAULT NULL,");
            sqlQuery.Append(@"`MeterData_ID` bigint(20) DEFAULT NULL");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");


            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `meterdata` (");
            sqlQuery.Append(@"`MeterData_ID` bigint(20) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`FileUpload_ID` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`MeterID` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`ReadingDateTime` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`UploadingDateTime` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`CMRI_Number` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`MeterData_ID`)");
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
            // changed for PUMA Enhancement on 
            sqlQuery.Append(@"`Meter_EMF` varchar(40) DEFAULT NULL,");
            sqlQuery.Append(@"`Meter_ContractDemand` double DEFAULT NULL,");
            sqlQuery.Append(@"`MeterUnit_ID` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"`Meter_CTPrimary` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"`Meter_CTSecondary` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"`Meter_PTPrimary` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"`Meter_PTSecondary` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"`Meter_InstalledCTPrimary` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"`Meter_InstalledCTSecondary` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"`Meter_InstalledCTRatio` int(3) NOT NULL,");
            sqlQuery.Append(@"`Meter_InstalledPTPrimary` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"`Meter_InstalledPTSecondary` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"`Meter_InstalledPTRatio` int(3) NOT NULL,");
            sqlQuery.Append(@"`Meter_Phone` varchar(15) DEFAULT NULL,");
            sqlQuery.Append(@"`Meter_Status` smallint(6) DEFAULT NULL,");
            // GPRS specific columns
            sqlQuery.Append(@"`Meter_GPRSModem_IMEI` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`GPRSModemConnectionType` tinyint(1) DEFAULT NULL,");
            sqlQuery.Append(@"`GPRSModemIpType` BIT(1) DEFAULT NULL,");
            // adding extra column for storing checkbox value on consumer meter definition page based on 
            // which emf,ct,pt ratio will be used in calculations - DHBVNL Jun1 2011.
            sqlQuery.Append(@"`UseEMFInCalculations` int(11) DEFAULT '1',");
            sqlQuery.Append(@"PRIMARY KEY (`Meter_ID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");

            // Adding extra table for logging meter master details when installed CT/PT 
            // is changed - DHBVNL June 2011

            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `meter_master_log` (");
            sqlQuery.Append(@"`Meter_Master_Log_ID` int(11) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`Meter_ID` varchar(20) NOT NULL,");
            sqlQuery.Append(@"`Meter_InstalledCTPrimary` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"`Meter_InstalledCTSecondary` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"`Meter_InstalledCTRatio` int(3) NOT NULL,");
            sqlQuery.Append(@"`Meter_InstalledPTPrimary` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"`Meter_InstalledPTSecondary` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"`Meter_InstalledPTRatio` int(3) NOT NULL,");
            sqlQuery.Append(@"`Meter_EMF` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"`UpdatedOn` bigint(20) NOT NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`Meter_Master_Log_ID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");

            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `midnightparameter` (");
            sqlQuery.Append(@"`ColumnsNames` varchar(1000) DEFAULT NULL,");
            sqlQuery.Append(@"`MeterData_ID` bigint(20) DEFAULT NULL");
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
            sqlQuery.Append(@"PRIMARY KEY (`Region_ID`),");
            //Added to avoid duplicate entries.
            sqlQuery.Append(@"UNIQUE KEY(`Region_Name`)");
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

            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `system_settings` (");
            sqlQuery.Append(@"`System_Setting_ID` int(11) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`Name` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"`Value` varchar(50) DEFAULT NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`System_Setting_ID`)");
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

            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `LoginMaster` (");
            sqlQuery.Append(@"`LogID` bigint(20) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`UserID` bigint(20),");
            sqlQuery.Append(@"`StartDateTime` bigint(20),");
            sqlQuery.Append(@"`EndDateTime` bigint(20),");
            sqlQuery.Append(@"PRIMARY KEY (`LogID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");

            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `tampertype_master` (");
            sqlQuery.Append(@"`TamperTypeID` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"`TamperType` varchar(150) DEFAULT NULL,");
            sqlQuery.Append(@"`MeterType` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"`Compartment` int(11) DEFAULT NULL,");
            /* VBM to support new tamper report */
            sqlQuery.Append(@"`TamperCategory` int(11) DEFAULT NULL,");
            /* VBM to support new tamper report */
            sqlQuery.Append(@"PRIMARY KEY (`TamperTypeID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");

            sqlQuery.Append(@"Create Table if not exists `Utility`(");
            sqlQuery.Append(@"`Utility_ID` bigint(20) not null auto_increment,");
            sqlQuery.Append(@"`Utility_Name` varchar(20) null,");
            sqlQuery.Append(@"`Utility_Password` varchar(15) null,");
            sqlQuery.Append(@"PRIMARY KEY (`Utility_ID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");

            // creating tabname table
            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `tabname` (");
            sqlQuery.Append(@"`MeterData_ID` bigint(20) NOT NULL ,");
            sqlQuery.Append(@"`AnalysisTab_ID` varchar(20) DEFAULT NULL,");
            sqlQuery.Append(@"`IsVisible` bit DEFAULT NULL,");
            sqlQuery.Append(@"`Description` varchar(100) DEFAULT NULL");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");

            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `tamperparameter` (");
            sqlQuery.Append(@"`ColumnsNames` varchar(1000) DEFAULT NULL,");
            sqlQuery.Append(@"`MeterData_ID` bigint(20) DEFAULT NULL");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");

            sqlQuery.Append(@"CREATE  TABLE IF NOT EXISTS `TOD` (");
            sqlQuery.Append(@"`TODId` bigint(20) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`MeterData_ID` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`TODData`  TEXT NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`TODId`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");


            sqlQuery.Append(@"CREATE  TABLE IF NOT EXISTS `DisplayParamater` (");
            sqlQuery.Append(@"`DisplayParamaterId` bigint(20) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`DisplayParamaterType` bigint(20) NOT NULL,");
            sqlQuery.Append(@"`DisplayParamaterName` VARCHAR(90) NULL ,");
            sqlQuery.Append(@"`MeterData_ID` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`DisplayParamaterValue` INT  NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`DisplayParamaterId`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");

            sqlQuery.Append(@"CREATE  TABLE IF NOT EXISTS `RTC` (");
            sqlQuery.Append(@"`RTCId` bigint(20) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`MeterData_ID` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`RTC`  varchar(90) NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`RTCId`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");

            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `BillingType` (");
            sqlQuery.Append(@"`BillingTypeID` bigint(20) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`ModeOfBilling` varchar(50) NOT NULL,");
            sqlQuery.Append(@"`BillingPeriod` varchar(50) NOT NULL,");
            sqlQuery.Append(@"`Day` varchar(50) NOT NULL,");
            sqlQuery.Append(@"`Hours` varchar(50) NOT NULL,");
            sqlQuery.Append(@"`Minutes` varchar(50) NOT NULL,");
            sqlQuery.Append(@"`BillingType` varchar(50) NOT NULL,");
            sqlQuery.Append(@"`ResetLockOutDays` varchar(50) NOT NULL,");
            sqlQuery.Append(@"`MeterData_ID` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`BillingTypeID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");

            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `kvarSelection` (");
            sqlQuery.Append(@"`kvarSelectionID` bigint(20) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`lagOnly` varchar(10) NOT NULL,");
            sqlQuery.Append(@"`lagandLead` varchar(50) NOT NULL,");
            sqlQuery.Append(@"`MeterData_ID` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`kvarSelectionID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");

            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `MdWithIP` (");
            sqlQuery.Append(@"`MdWithIPID` bigint(20) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`KWDemandType` varchar(50) NOT NULL,");
            sqlQuery.Append(@"`KWInterval` int(2) NOT NULL,");
            sqlQuery.Append(@"`KWSubInterval` int(2) NOT NULL,");
            sqlQuery.Append(@"`KVADemandType` varchar(50) NOT NULL,");
            sqlQuery.Append(@"`KVAInterval` int(2) NOT NULL,");
            sqlQuery.Append(@"`KVASubInterval` int(2) NOT NULL,");
            sqlQuery.Append(@"`MeterData_ID` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`MdWithIPID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");

            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `RS232`(");
            sqlQuery.Append(@" `RS232ID` bigint(20) not null auto_increment,");
            sqlQuery.Append(@"`MeterData_ID` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@" `RS232Status` varchar(20) not null,");
            sqlQuery.Append(@"PRIMARY KEY (`RS232ID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");

            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `RS485`(");
            sqlQuery.Append(@" `DCID` bigint(20) not null auto_increment,");
            sqlQuery.Append(@"`MeterData_ID` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@" `DCData` varchar(20) not null,");
            sqlQuery.Append(@"PRIMARY KEY (`DCID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");


            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `DailyLog` (");
            sqlQuery.Append(@"`DailyLogID` bigint(20) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`CumulativeKWh` varchar(50) NOT NULL,");
            sqlQuery.Append(@"`CumulativeKVARhLag` varchar(50) NOT NULL,");
            sqlQuery.Append(@"`CumulativeKVARhLead` varchar(50) NOT NULL,");
            sqlQuery.Append(@"`CumulativeKVAh` varchar(50) NOT NULL,");
            sqlQuery.Append(@"`DailyMD1` varchar(50) NOT NULL,");
            sqlQuery.Append(@"`DailyMD2` varchar(50) NOT NULL,");
            sqlQuery.Append(@"`MeterData_ID` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`DailyLogID`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");

            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `AutoLock` (");
            sqlQuery.Append(@"`AutoLockId` bigint(20) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`AutoLockStatus` varchar(20) NOT NULL,");
            sqlQuery.Append(@"`MeterData_ID` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`AutoLockId`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");

            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `LSIP` (");
            sqlQuery.Append(@"`LSIPId` bigint(20) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`LSIPValue` INT NOT NULL,");
            sqlQuery.Append(@"`MeterData_ID` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`LSIPId`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");

            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `DIP` (");
            sqlQuery.Append(@"`DIPId` bigint(20) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`DIPValue` INT NOT NULL,");
            sqlQuery.Append(@"`MeterData_ID` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`DIPId`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");


            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `ManualBilling` (");
            sqlQuery.Append(@"`ManualBillingId` bigint(20) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`ManualBillingStatus` varchar(20) NOT NULL,");
            sqlQuery.Append(@"`MeterData_ID` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`ManualBillingId`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");

            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `SoftwareBilling` (");
            sqlQuery.Append(@"`SoftwareBillingId` bigint(20) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`SoftwareBillingStatus` varchar(20) NOT NULL,");
            sqlQuery.Append(@"`MeterData_ID` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`SoftwareBillingId`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");

            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `ManualButtonMDReset` (");
            sqlQuery.Append(@"`ManualMDResetId` bigint(20) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`ManualMDResetStatus` varchar(20) NOT NULL,");
            sqlQuery.Append(@"`MeterData_ID` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`ManualMDResetId`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");



            //sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `utility` (");
            //sqlQuery.Append(@"`UtilityName` varchar(50) DEFAULT NULL,");
            //sqlQuery.Append(@"`UtilityPassword` varchar(50) DEFAULT NULL");
            //sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");


            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `loadcontrol` (");
            sqlQuery.Append(@"`LCId` bigint(20) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`MeterData_ID` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`LCData` text,");
            sqlQuery.Append(@"PRIMARY KEY (`LCId`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");


            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `disconnectcontrol` (");
            sqlQuery.Append(@"`DCId` bigint(20) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`MeterData_ID` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`DCData` text,");
            sqlQuery.Append(@"PRIMARY KEY (`DCId`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");
            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `paymentmode` (");
            sqlQuery.Append(@"`PMId` bigint(20) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`MeterData_ID` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`PMData` text,");
            sqlQuery.Append(@"PRIMARY KEY (`PMId`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");

            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `meteringmode` (");
            sqlQuery.Append(@"`MMId` bigint(20) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`MeterData_ID` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`MMData` text,");
            sqlQuery.Append(@"PRIMARY KEY (`MMId`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");

            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `loadlimit` (");
            sqlQuery.Append(@"`LLId` bigint(20) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`MeterData_ID` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`LLData` text,");
            sqlQuery.Append(@"PRIMARY KEY (`LLId`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");

            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `Slidingdemand` (");
            sqlQuery.Append(@"`SDId` bigint(20) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`MeterData_ID` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`SDData` text,");
            sqlQuery.Append(@"PRIMARY KEY (`SDId`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");


            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `Opticallock` (");
            sqlQuery.Append(@"`OPId` bigint(20) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`MeterData_ID` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`OPData` text,");
            sqlQuery.Append(@"PRIMARY KEY (`OPId`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");

            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `RJLock` (");
            sqlQuery.Append(@"`RJId` bigint(20) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`MeterData_ID` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"`RJData` text,");
            sqlQuery.Append(@"PRIMARY KEY (`RJId`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");

            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `gsmconfigstatus` (");
            sqlQuery.Append(@"`GSMId` bigint(20) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`MeterID` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"`SimNo` varchar(150) DEFAULT NULL,");
            sqlQuery.Append(@"`Status` varchar(150) DEFAULT NULL,");
            sqlQuery.Append(@"`Reason` varchar(150) DEFAULT NULL,");
            sqlQuery.Append(@"`TaskID` varchar(150) DEFAULT NULL,");
            sqlQuery.Append(@"`creationDateTime` VARCHAR(30) NOT NULL ,");
            sqlQuery.Append(@"PRIMARY KEY (`GSMId`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");

            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `gsmreadstatus` (");
            sqlQuery.Append(@"`GSMId` bigint(20) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`MeterID` int(11) DEFAULT NULL,");
            sqlQuery.Append(@"`SimNo` varchar(150) DEFAULT NULL,");
            sqlQuery.Append(@"`Status` varchar(150) DEFAULT NULL,");
            sqlQuery.Append(@"`Reason` varchar(150) DEFAULT NULL,");
            sqlQuery.Append(@"`TaskID` varchar(150) DEFAULT NULL,");
            sqlQuery.Append(@"`creationDateTime` VARCHAR(30) NOT NULL ,");
            sqlQuery.Append(@"PRIMARY KEY (`GSMId`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");

            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `pulseEnergy` (");
            sqlQuery.Append(@"`pulseEnergyId` bigint(20) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`pulseEnergyValue` varchar(10) NOT NULL,");
            sqlQuery.Append(@"`MeterData_ID` bigint(20) DEFAULT NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`pulseEnergyId`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");

            #endregion

            if (apptype.Equals(ApplicationType.DLMS_LTCT_650))
            {
                #region "DLMS"
                sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `StructureInfo` (");
                sqlQuery.Append(@"`StructureInfoID` int(11) NOT NULL AUTO_INCREMENT,");
                sqlQuery.Append(@"`StructureID` int(11) DEFAULT NULL,");
                sqlQuery.Append(@"`StructureName` varchar(50) DEFAULT NULL,");
                sqlQuery.Append(@"`ValueInBit` int(11) DEFAULT NULL,");
                sqlQuery.Append(@"`ValueInByte` int(11) DEFAULT NULL,");
                sqlQuery.Append(@"`SignType` varchar(50) DEFAULT NULL,");
                sqlQuery.Append(@"PRIMARY KEY (`StructureInfoID`)");
                sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");

                sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `StructureUnitInfo` (");
                sqlQuery.Append(@"`StructureUnitInfoID` int(11) NOT NULL AUTO_INCREMENT,");
                sqlQuery.Append(@"`StructureUnitID` int(11) DEFAULT NULL,");
                sqlQuery.Append(@"`StructureUnitName` varchar(50) DEFAULT NULL,");
                sqlQuery.Append(@"`StructureUnit` varchar(10) DEFAULT NULL,");
                sqlQuery.Append(@"PRIMARY KEY (`StructureUnitInfoID`)");
                sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");

                sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `StructureOBISInfo` (");
                sqlQuery.Append(@"`StructureOBISInfoID` int(11) NOT NULL AUTO_INCREMENT,");
                sqlQuery.Append(@"`ClassID` int(11) DEFAULT NULL,");
                sqlQuery.Append(@"`Attribute`int(11) DEFAULT NULL,");
                sqlQuery.Append(@"`OBISName` varchar(100) DEFAULT NULL,");
                sqlQuery.Append(@"`OBISCode` varchar(100) DEFAULT NULL,");
                sqlQuery.Append(@"PRIMARY KEY (`StructureOBISInfoID`)");
                sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");

                sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `meterdata_general` (");
                sqlQuery.Append(@"`General_ID` bigint(20) NOT NULL AUTO_INCREMENT,");
                sqlQuery.Append(@"`meterSerialNumber` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`manufacturername` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`firmwareVersionformeter` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`metertype` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`internalCTratio` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`internalPTratio` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`energyResolution` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`demandResolution` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`Category` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`meteryearofmanufacture` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`MeterData_ID` bigint(20) DEFAULT NULL,");
                sqlQuery.Append(@"`MeterDataType` varchar(20) DEFAULT NULL,");
                sqlQuery.Append(@"`MeterModelNo` varchar(20) DEFAULT NULL,");
                sqlQuery.Append(@"`InternalFirmwareVersion` varchar(20) DEFAULT NULL,");
                sqlQuery.Append(@"`VoltageRating` varchar(20) DEFAULT NULL,");
                sqlQuery.Append(@"`BasicCurrentRating` varchar(20) DEFAULT NULL,");
                sqlQuery.Append(@"`NetMeterVariantInfo` varchar(20) DEFAULT NULL,");
                sqlQuery.Append(@"`PrimaryMeterConstantInfo` varchar(20) DEFAULT NULL,");//PGVCL
                sqlQuery.Append(@"`MeterConstantInfo` varchar(20) DEFAULT NULL,");                          
                sqlQuery.Append(@"`DisplayProgrammingType` varchar(20) DEFAULT NULL,");
                sqlQuery.Append(@"PRIMARY KEY (`General_ID`)");
                sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");

                sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `meterdata_instantpower` (");
                sqlQuery.Append(@"`InstantPower_ID` bigint(20) NOT NULL AUTO_INCREMENT,");
                sqlQuery.Append(@"`InstantPowerColumnName` varchar(60) DEFAULT NULL,");
                sqlQuery.Append(@"`InstantPowerColumnValue` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`InstantPowerObisCode` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`InstantPowerClassID` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`InstantPowerAttribute` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`InstantPowerDataIndex` bigint(20) NOT NULL,");
                sqlQuery.Append(@"`MeterData_ID` bigint(20) DEFAULT NULL,");
                sqlQuery.Append(@"PRIMARY KEY (`InstantPower_ID`)");
                sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");

                sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `meterdata_loadsurvey` (");
                sqlQuery.Append(@"`LoadSurvey_ID` bigint(20) NOT NULL AUTO_INCREMENT,");
                sqlQuery.Append(@"`realTimeClockDateandTime` bigint(20) NOT NULL,");
                sqlQuery.Append(@"`rPhaseCurrent` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`yPhaseCurrent` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`bPhaseCurrent` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`averageCurrent` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`rPhaseVoltage` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`yPhaseVoltage` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`bPhaseVoltage` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`averageVoltage` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`blockEnergykWh` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`blockEnergykvarhlag` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`blockEnergykvarhlead` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`blockEnergykVAh` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`blockEnergykWhExport` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`blockEnergykvarhlagQ3` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`blockEnergykvarhleadQ2` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`blockEnergykVAhExport` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`blockEnergykWhImport` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`blockEnergykvarhlagQ1` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`blockEnergykvarhleadQ4` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`blockEnergykVAhImport` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`blockEnergykWhRPhase` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`blockEnergykWhYPhase` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`blockEnergykWhBPhase` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`blockEnergykvarhQ12` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`blockEnergykvarhQ34` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`blockEnergykvarhQ14` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`blockEnergykvarhQ23` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`blockEnergyFundamentalkWhAbsolute` varchar(40) DEFAULT NULL,");

                sqlQuery.Append(@"`netkWh` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`netkVAh` varchar(40) DEFAULT NULL,");

                sqlQuery.Append(@"`activePowerRPhase` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`activePowerYPhase` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`activePowerBPhase` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`apparentPowerRPhase` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`apparentPowerYPhase` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`apparentPowerBPhase` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`reactivePowerRPhase` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`reactivePowerYPhase` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`reactivePowerBPhase` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`powerOffDurationLSIP` varchar(40) DEFAULT NULL,");

                sqlQuery.Append(@"`temperature` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`NeutralCurrent` varchar(40) DEFAULT NULL,");//add pradipta_load_neu
                sqlQuery.Append(@"`AvgPhaseCurrent` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`MDIntervalPeriod` INT(11) DEFAULT NULL,");
                sqlQuery.Append(@"`IsDLMS` INT(1) DEFAULT 0,");
                sqlQuery.Append(@"`MeterData_ID` bigint(20) DEFAULT NULL,");
                //added PUMA
                sqlQuery.Append(@"`frequency` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`tamperStatus` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`tamperflag` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`Avgvoltageof3phase` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`AvgRphasePF` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`AvgYphasePF` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`AvgBphasePF` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`AvgTotalPF` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`AvgNeutralCurrent` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`ThdVr` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`ThdVy` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`ThdVb` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`ThdIr` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`ThdIy` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`ThdIb` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"PRIMARY KEY (`LoadSurvey_ID`)");
                sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");


                //added for MVVNL
                sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `meterdata_midnightdata` (");
                sqlQuery.Append(@"`MidnightData_ID` bigint(20) NOT NULL AUTO_INCREMENT,");
                sqlQuery.Append(@"`realTimeClockDateandTime` bigint(20) NOT NULL,");
                sqlQuery.Append(@"`cumEnergykWh` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`cumEnergykvarhlag` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`cumEnergykvarhlead` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`cumEnergykVAh` varchar(40) DEFAULT NULL,");

                sqlQuery.Append(@"`cumEnergykWhExport` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`cumEnergykvarhlagQ3` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`cumEnergykvarhleadQ2` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`cumEnergykVAhExport` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`cumEnergykWhImport` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`cumEnergykvarhlagQ1` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`cumEnergykvarhleadQ4` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`cumEnergykVAhImport` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`cumEnergykWhRPhase` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`cumEnergykWhYPhase` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`cumEnergykWhBPhase` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`cumEnergykvarhQ12` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`cumEnergykvarhQ34` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`cumEnergykvarhQ14` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`cumEnergykvarhQ23` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`fundamentalAbsolutekWH` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`netkWh` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`netkVAh` varchar(40) DEFAULT NULL,");

                sqlQuery.Append(@"`minVoltageLSIPAcrossDayRPhase` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`minVoltageLSIPAcrossDayYPhase` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`minVoltageLSIPAcrossDayBPhase` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`highestCurrentLSIPAcrossDayRPhase` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`highestCurrentLSIPAcrossDayYPhase` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`highestCurrentLSIPAcrossDayBPhase` varchar(40) DEFAULT NULL,");

                sqlQuery.Append(@"`mDKW` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`mDKWDateTime` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`mDKVA` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`mDKVADateTime` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`MeterData_ID` bigint(20) DEFAULT NULL,");
                sqlQuery.Append(@"`PowerOnDuration` VARCHAR(40) ,");
                sqlQuery.Append(@"`PowerFailureDuration` VARCHAR(40) ,");
                sqlQuery.Append(@"`PowerOnDurationThreePhases` VARCHAR(40) ,");
                sqlQuery.Append(@"`PowerOnDurationGeneric` VARCHAR(40) ,");
                sqlQuery.Append(@"`PowerOnDurationGeneric1P` VARCHAR(40) ,");
                sqlQuery.Append(@"PRIMARY KEY (`MidnightData_ID`)");
                sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");
                //added for MVVNL

                sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `meterdata_billing` (");
                sqlQuery.Append(@"`Billing_ID` bigint(20) NOT NULL AUTO_INCREMENT ,");
                sqlQuery.Append(@"`BillingDate` bigint(20) NULL ,");
                sqlQuery.Append(@"`SystemPowerFactorforBillingPeriod` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykWhTZ0` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykWhTZ1` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykWhTZ2` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykWhTZ3` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykWhTZ4` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykWhTZ5` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykWhTZ6` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykWhTZ7` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykWhTZ8` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykvarhLag` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykvarhLagTZ1` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykvarhLagTZ2` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykvarhLagTZ3` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykvarhLagTZ4` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykvarhLagTZ5` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykvarhLagTZ6` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykvarhLagTZ7` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykvarhLagTZ8` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykvarhLead` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykvarhLeadTZ1` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykvarhLeadTZ2` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykvarhLeadTZ3` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykvarhLeadTZ4` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykvarhLeadTZ5` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykvarhLeadTZ6` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykvarhLeadTZ7` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykvarhLeadTZ8` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykVAhTZ0` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykVAhTZ1` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykVAhTZ2` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykVAhTZ3` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykVAhTZ4` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykVAhTZ5` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykVAhTZ6` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykVAhTZ7` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykVAhTZ8` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`MDkWTZ0` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`MDkWDateTimeTZ0` bigint(20) NULL ,");
                sqlQuery.Append(@"`MDkWTZ1` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`MDkWDateTimeTZ1` bigint(20) NULL ,");
                sqlQuery.Append(@"`MDkWTZ2` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`MDkWDateTimeTZ2` bigint(20) NULL ,");
                sqlQuery.Append(@"`MDkWTZ3` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`MDkWDateTimeTZ3` bigint(20) NULL ,");
                sqlQuery.Append(@"`MDkWTZ4` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`MDkWDateTimeTZ4` bigint(20) NULL ,");
                sqlQuery.Append(@"`MDkWTZ5` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`MDkWDateTimeTZ5` bigint(20) NULL ,");
                sqlQuery.Append(@"`MDkWTZ6` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`MDkWDateTimeTZ6` bigint(20) NULL ,");
                sqlQuery.Append(@"`MDkWTZ7` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`MDkWDateTimeTZ7` bigint(20) NULL ,");
                sqlQuery.Append(@"`MDkWTZ8` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`MDkWDateTimeTZ8` bigint(20) NULL ,");
                sqlQuery.Append(@"`MDkVATZ0` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`MDkVADateTimeTZ0` bigint(20) NULL ,");
                sqlQuery.Append(@"`MDkVATZ1` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`MDkVADateTimeTZ1` bigint(20) NULL ,");
                sqlQuery.Append(@"`MDkVATZ2` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`MDkVADateTimeTZ2` bigint(20) NULL ,");
                sqlQuery.Append(@"`MDkVATZ3` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`MDkVADateTimeTZ3` bigint(20) NULL ,");
                sqlQuery.Append(@"`MDkVATZ4` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`MDkVADateTimeTZ4` bigint(20) NULL ,");
                sqlQuery.Append(@"`MDkVATZ5` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`MDkVADateTimeTZ5` bigint(20) NULL ,");
                sqlQuery.Append(@"`MDkVATZ6` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`MDkVADateTimeTZ6` bigint(20) NULL ,");
                sqlQuery.Append(@"`MDkVATZ7` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`MDkVADateTimeTZ7` bigint(20) NULL ,");
                sqlQuery.Append(@"`MDkVATZ8` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`MDkVADateTimeTZ8` bigint(20) NULL ,");
                sqlQuery.Append(@"`TODAveragePowerFactorTZ1` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`TODAveragePowerFactorTZ2` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`TODAveragePowerFactorTZ3` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`TODAveragePowerFactorTZ4` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`TODAveragePowerFactorTZ5` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`TODAveragePowerFactorTZ6` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`TODAveragePowerFactorTZ7` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`TODAveragePowerFactorTZ8` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`TODAverageExportPowerFactorTZ1` VARCHAR(40) NULL ,"); //story 1024441 Add TOD Export PF
                sqlQuery.Append(@"`TODAverageExportPowerFactorTZ2` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`TODAverageExportPowerFactorTZ3` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`TODAverageExportPowerFactorTZ4` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`TODAverageExportPowerFactorTZ5` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`TODAverageExportPowerFactorTZ6` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`TODAverageExportPowerFactorTZ7` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`TODAverageExportPowerFactorTZ8` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`RPhaseMDkW` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`RPhaseMDDateTime` bigint(20) NULL ,");
                sqlQuery.Append(@"`YPhaseMDkW` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`YPhaseMDDateTime` bigint(20) NULL ,");
                sqlQuery.Append(@"`BPhaseMDkW` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`BPhaseMDDateTime` bigint(20) NULL ,");               
                
                
                //NetStart
                //KWH - Net
                sqlQuery.Append(@"`CumulativeEnergykWhTZ0Net` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykWhTZ1Net` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykWhTZ2Net` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykWhTZ3Net` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykWhTZ4Net` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykWhTZ5Net` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykWhTZ6Net` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykWhTZ7Net` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykWhTZ8Net` VARCHAR(40) NULL ,");
                //KVAH - Net
                sqlQuery.Append(@"`CumulativeEnergykVAhTZ0Net` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykVAhTZ1Net` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykVAhTZ2Net` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykVAhTZ3Net` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykVAhTZ4Net` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykVAhTZ5Net` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykVAhTZ6Net` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykVAhTZ7Net` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykVAhTZ8Net` VARCHAR(40) NULL ,");
                //MD - KW - Net
                sqlQuery.Append(@"`MDkWTZ0Net` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`MDkWTZ1Net` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`MDkWTZ2Net` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`MDkWTZ3Net` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`MDkWTZ4Net` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`MDkWTZ5Net` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`MDkWTZ6Net` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`MDkWTZ7Net` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`MDkWTZ8Net` VARCHAR(40) NULL ,");
                //MD - KVA - Net
                sqlQuery.Append(@"`MDkVATZ0Net` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`MDkVATZ1Net` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`MDkVATZ2Net` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`MDkVATZ3Net` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`MDkVATZ4Net` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`MDkVATZ5Net` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`MDkVATZ6Net` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`MDkVATZ7Net` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`MDkVATZ8Net` VARCHAR(40) NULL ,");
                //MD - KW - DateTime - Net
                sqlQuery.Append(@"`MDkWDateTimeTZ0Net` bigint(20) NULL ,");
                sqlQuery.Append(@"`MDkWDateTimeTZ1Net` bigint(20) NULL ,");
                sqlQuery.Append(@"`MDkWDateTimeTZ2Net` bigint(20) NULL ,");
                sqlQuery.Append(@"`MDkWDateTimeTZ3Net` bigint(20) NULL ,");
                sqlQuery.Append(@"`MDkWDateTimeTZ4Net` bigint(20) NULL ,");
                sqlQuery.Append(@"`MDkWDateTimeTZ5Net` bigint(20) NULL ,");
                sqlQuery.Append(@"`MDkWDateTimeTZ6Net` bigint(20) NULL ,");
                sqlQuery.Append(@"`MDkWDateTimeTZ7Net` bigint(20) NULL ,");
                sqlQuery.Append(@"`MDkWDateTimeTZ8Net` bigint(20) NULL ,");
                //MD - KVA - DateTime - Net
                sqlQuery.Append(@"`MDkVADateTimeTZ0Net` bigint(20) NULL ,");
                sqlQuery.Append(@"`MDkVADateTimeTZ1Net` bigint(20) NULL ,");
                sqlQuery.Append(@"`MDkVADateTimeTZ2Net` bigint(20) NULL ,");
                sqlQuery.Append(@"`MDkVADateTimeTZ3Net` bigint(20) NULL ,");
                sqlQuery.Append(@"`MDkVADateTimeTZ4Net` bigint(20) NULL ,");
                sqlQuery.Append(@"`MDkVADateTimeTZ5Net` bigint(20) NULL ,");
                sqlQuery.Append(@"`MDkVADateTimeTZ6Net` bigint(20) NULL ,");
                sqlQuery.Append(@"`MDkVADateTimeTZ7Net` bigint(20) NULL ,");
                sqlQuery.Append(@"`MDkVADateTimeTZ8Net` bigint(20) NULL ,");
                //NetEnd



                //ImportStart
                //KWH - Import
                sqlQuery.Append(@"`CumulativeEnergykWhTZ0Import` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykWhTZ1Import` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykWhTZ2Import` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykWhTZ3Import` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykWhTZ4Import` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykWhTZ5Import` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykWhTZ6Import` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykWhTZ7Import` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykWhTZ8Import` VARCHAR(40) NULL ,");
                //KVAH - Import
                sqlQuery.Append(@"`CumulativeEnergykVAhTZ0Import` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykVAhTZ1Import` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykVAhTZ2Import` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykVAhTZ3Import` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykVAhTZ4Import` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykVAhTZ5Import` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykVAhTZ6Import` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykVAhTZ7Import` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykVAhTZ8Import` VARCHAR(40) NULL ,");
                //MD - KW - Import
                sqlQuery.Append(@"`MDkWTZ0Import` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`MDkWTZ1Import` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`MDkWTZ2Import` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`MDkWTZ3Import` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`MDkWTZ4Import` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`MDkWTZ5Import` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`MDkWTZ6Import` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`MDkWTZ7Import` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`MDkWTZ8Import` VARCHAR(40) NULL ,");
                //MD - KVA - Import
                sqlQuery.Append(@"`MDkVATZ0Import` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`MDkVATZ1Import` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`MDkVATZ2Import` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`MDkVATZ3Import` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`MDkVATZ4Import` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`MDkVATZ5Import` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`MDkVATZ6Import` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`MDkVATZ7Import` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`MDkVATZ8Import` VARCHAR(40) NULL ,");
                //MD - KW - DateTime - Import
                sqlQuery.Append(@"`MDkWDateTimeTZ0Import` bigint(20) NULL ,");
                sqlQuery.Append(@"`MDkWDateTimeTZ1Import` bigint(20) NULL ,");
                sqlQuery.Append(@"`MDkWDateTimeTZ2Import` bigint(20) NULL ,");
                sqlQuery.Append(@"`MDkWDateTimeTZ3Import` bigint(20) NULL ,");
                sqlQuery.Append(@"`MDkWDateTimeTZ4Import` bigint(20) NULL ,");
                sqlQuery.Append(@"`MDkWDateTimeTZ5Import` bigint(20) NULL ,");
                sqlQuery.Append(@"`MDkWDateTimeTZ6Import` bigint(20) NULL ,");
                sqlQuery.Append(@"`MDkWDateTimeTZ7Import` bigint(20) NULL ,");
                sqlQuery.Append(@"`MDkWDateTimeTZ8Import` bigint(20) NULL ,");
                //MD - KVA - DateTime - Import
                sqlQuery.Append(@"`MDkVADateTimeTZ0Import` bigint(20) NULL ,");
                sqlQuery.Append(@"`MDkVADateTimeTZ1Import` bigint(20) NULL ,");
                sqlQuery.Append(@"`MDkVADateTimeTZ2Import` bigint(20) NULL ,");
                sqlQuery.Append(@"`MDkVADateTimeTZ3Import` bigint(20) NULL ,");
                sqlQuery.Append(@"`MDkVADateTimeTZ4Import` bigint(20) NULL ,");
                sqlQuery.Append(@"`MDkVADateTimeTZ5Import` bigint(20) NULL ,");
                sqlQuery.Append(@"`MDkVADateTimeTZ6Import` bigint(20) NULL ,");
                sqlQuery.Append(@"`MDkVADateTimeTZ7Import` bigint(20) NULL ,");
                sqlQuery.Append(@"`MDkVADateTimeTZ8Import` bigint(20) NULL ,");
                //ImportEnd

                //ExportStart               
                //KWH - Export
                sqlQuery.Append(@"`CumulativeEnergykWhTZ0Export` VARCHAR(40) NULL ,");               
                sqlQuery.Append(@"`CumulativeEnergykWhTZ1Export` VARCHAR(40) NULL ,");               
                sqlQuery.Append(@"`CumulativeEnergykWhTZ2Export` VARCHAR(40) NULL ,");               
                sqlQuery.Append(@"`CumulativeEnergykWhTZ3Export` VARCHAR(40) NULL ,");               
                sqlQuery.Append(@"`CumulativeEnergykWhTZ4Export` VARCHAR(40) NULL ,");               
                sqlQuery.Append(@"`CumulativeEnergykWhTZ5Export` VARCHAR(40) NULL ,");               
                sqlQuery.Append(@"`CumulativeEnergykWhTZ6Export` VARCHAR(40) NULL ,");               
                sqlQuery.Append(@"`CumulativeEnergykWhTZ7Export` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykWhTZ8Export` VARCHAR(40) NULL ,");
                //KVAH - Export
                sqlQuery.Append(@"`CumulativeEnergykVAhTZ0Export` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykVAhTZ1Export` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykVAhTZ2Export` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykVAhTZ3Export` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykVAhTZ4Export` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykVAhTZ5Export` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykVAhTZ6Export` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykVAhTZ7Export` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykVAhTZ8Export` VARCHAR(40) NULL ,");    
                //MD - KW - Export
                sqlQuery.Append(@"`MDkWTZ0Export` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`MDkWTZ1Export` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`MDkWTZ2Export` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`MDkWTZ3Export` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`MDkWTZ4Export` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`MDkWTZ5Export` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`MDkWTZ6Export` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`MDkWTZ7Export` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`MDkWTZ8Export` VARCHAR(40) NULL ,"); 
                //MD - KVA - Export
                sqlQuery.Append(@"`MDkVATZ0Export` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`MDkVATZ1Export` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`MDkVATZ2Export` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`MDkVATZ3Export` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`MDkVATZ4Export` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`MDkVATZ5Export` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`MDkVATZ6Export` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`MDkVATZ7Export` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`MDkVATZ8Export` VARCHAR(40) NULL ,"); 
                //MD - KW - DateTime - Export
                sqlQuery.Append(@"`MDkWDateTimeTZ0Export` bigint(20) NULL ,");
                sqlQuery.Append(@"`MDkWDateTimeTZ1Export` bigint(20) NULL ,");
                sqlQuery.Append(@"`MDkWDateTimeTZ2Export` bigint(20) NULL ,");
                sqlQuery.Append(@"`MDkWDateTimeTZ3Export` bigint(20) NULL ,");
                sqlQuery.Append(@"`MDkWDateTimeTZ4Export` bigint(20) NULL ,");
                sqlQuery.Append(@"`MDkWDateTimeTZ5Export` bigint(20) NULL ,");
                sqlQuery.Append(@"`MDkWDateTimeTZ6Export` bigint(20) NULL ,");
                sqlQuery.Append(@"`MDkWDateTimeTZ7Export` bigint(20) NULL ,");
                sqlQuery.Append(@"`MDkWDateTimeTZ8Export` bigint(20) NULL ,");
                //MD - KVA - DateTime - Export
                sqlQuery.Append(@"`MDkVADateTimeTZ0Export` bigint(20) NULL ,");
                sqlQuery.Append(@"`MDkVADateTimeTZ1Export` bigint(20) NULL ,");
                sqlQuery.Append(@"`MDkVADateTimeTZ2Export` bigint(20) NULL ,");
                sqlQuery.Append(@"`MDkVADateTimeTZ3Export` bigint(20) NULL ,");
                sqlQuery.Append(@"`MDkVADateTimeTZ4Export` bigint(20) NULL ,");
                sqlQuery.Append(@"`MDkVADateTimeTZ5Export` bigint(20) NULL ,");
                sqlQuery.Append(@"`MDkVADateTimeTZ6Export` bigint(20) NULL ,");
                sqlQuery.Append(@"`MDkVADateTimeTZ7Export` bigint(20) NULL ,");
                sqlQuery.Append(@"`MDkVADateTimeTZ8Export` bigint(20) NULL ,");
                //Cumulative-Energy-Lag-Q1
                sqlQuery.Append(@"`CumulativeEnergykvarhLagQ1` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykvarhLagTZ1Q1` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykvarhLagTZ2Q1` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykvarhLagTZ3Q1` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykvarhLagTZ4Q1` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykvarhLagTZ5Q1` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykvarhLagTZ6Q1` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykvarhLagTZ7Q1` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykvarhLagTZ8Q1` VARCHAR(40) NULL ,");
                //Cumulative-Energy-Lead-Q4    
                sqlQuery.Append(@"`CumulativeEnergykvarhLeadQ4` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykvarhLeadTZ1Q4` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykvarhLeadTZ2Q4` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykvarhLeadTZ3Q4` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykvarhLeadTZ4Q4` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykvarhLeadTZ5Q4` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykvarhLeadTZ6Q4` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykvarhLeadTZ7Q4` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykvarhLeadTZ8Q4` VARCHAR(40) NULL ,"); 
                //Cumulative-Energy-Lag-Q3
                sqlQuery.Append(@"`CumulativeEnergykvarhLagQ3` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykvarhLagTZ1Q3` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykvarhLagTZ2Q3` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykvarhLagTZ3Q3` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykvarhLagTZ4Q3` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykvarhLagTZ5Q3` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykvarhLagTZ6Q3` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykvarhLagTZ7Q3` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykvarhLagTZ8Q3` VARCHAR(40) NULL ,"); 
                //Cumulative-Energy-Lead-Q2    
                sqlQuery.Append(@"`CumulativeEnergykvarhLeadQ2` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykvarhLeadTZ1Q2` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykvarhLeadTZ2Q2` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykvarhLeadTZ3Q2` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykvarhLeadTZ4Q2` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykvarhLeadTZ5Q2` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykvarhLeadTZ6Q2` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykvarhLeadTZ7Q2` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumulativeEnergykvarhLeadTZ8Q2` VARCHAR(40) NULL ,"); 

                //ExportEnd

                sqlQuery.Append(@"`SystemPowerFactorImportforBillingPeriod` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`SystemPowerFactorExportforBillingPeriod` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumEnergykWhRPhase` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumEnergykWhYPhase` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CumEnergykWhBPhase` VARCHAR(40) NULL ,");

                #region JDVVNL
                sqlQuery.Append(@"`MinimumVoltageLSIPAcrossDayRPhase` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`MinimumVoltageLSIPAcrossDayYPhase` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`MinimumVoltageLSIPAcrossDayBPhase` VARCHAR(40) NULL ,");       
                #endregion

                sqlQuery.Append(@"`BillingAverageLoad` VARCHAR(40) NULL ,");

                //if (UtilityEntity.MPKWCL == localUtilityEntity)
                //{
                //    sqlQuery.Append(@"`CumPowerOffDuration` VARCHAR(40) NULL ,");

                sqlQuery.Append(@"`CumTamperCount` bigint(20) NULL ,");
                //}
                sqlQuery.Append(@"`CumPowerFailureCount` bigint(20) NULL ,");
                sqlQuery.Append(@"`MeterData_ID` bigint(20) NULL ,");
                sqlQuery.Append(@"`DataIndex` bigint(20) NULL ,");
                sqlQuery.Append(@"`BillingResetType` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`PowerOffDuration` VARCHAR(40) ,");
                sqlQuery.Append(@"`BillingWisePowerOffDuration` VARCHAR(40) ,");
                sqlQuery.Append(@"`BillingAvgLoadFactor` VARCHAR(40) ,");
                //pradipta_start_081018

                sqlQuery.Append(@"`BillingAvgkWImportLoadFactor` VARCHAR(40) ,");
                sqlQuery.Append(@"`BillingAvgkWExportLoadFactor` VARCHAR(40) ,");

                sqlQuery.Append(@"`BillingAvgkVAImportLoadFactor` VARCHAR(40) ,");
                sqlQuery.Append(@"`BillingAvgkVAExportLoadFactor` VARCHAR(40) ,");
                //pradipta_End_081018


                sqlQuery.Append(@"`PowerOnDuration` VARCHAR(40) ,");
                sqlQuery.Append(@"`CumPowerOnDuration` VARCHAR(40) ,");
                sqlQuery.Append(@"`PowerOnDurationDisplay` int(11) ,");
                sqlQuery.Append(@"`CumBillingMDResetCount` bigint(20) NULL ,");
                sqlQuery.Append(@"`DeltaTamperCount` bigint(20) NULL ,"); // Story - 345154
                sqlQuery.Append(@"`ABCCodeBilling` VARCHAR(40) ,"); // ABC Code added for 128k
                sqlQuery.Append(@"`CumulativeMDkW` VARCHAR(40) ,");
                sqlQuery.Append(@"`CumulativeMDkva` VARCHAR(40) ,");
                sqlQuery.Append(@"`CumulativeEnergyFraudkWh` VARCHAR(40) NULL,");
                sqlQuery.Append(@"`CumulativeEnergyFraudkVAh` VARCHAR(40) NULL,");

                // User Story - 1000867
                sqlQuery.Append(@"`MDkVArLagTZ0` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`MDkVArLagDateTimeTZ0` bigint(20) NULL ,");
                sqlQuery.Append(@"`MDkVArLeadTZ0` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`MDkVArLeadDateTimeTZ0` bigint(20) NULL ,");
                // For Sapphire S2 KSEB
                sqlQuery.Append(@"`kWhLag` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`kWhLead` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`kVAhLag` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`kVAhLead` VARCHAR(40) NULL ,");

                sqlQuery.Append(@"PRIMARY KEY (`Billing_ID`)");
                sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");

                sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `tamper_master` (");
                sqlQuery.Append(@"`Tamper_ID` bigint(20) NOT NULL AUTO_INCREMENT,");
                sqlQuery.Append(@"`DateTimeEvent` bigint(20) DEFAULT NULL,");
                sqlQuery.Append(@"`EventCode` bigint(20) DEFAULT NULL,");
                sqlQuery.Append(@"`CurrentIR` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`CurrentIY` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`CurrentIB` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`PhaseCurrent` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`VoltageVRN` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`VoltageVYN` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`VoltageVBN` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`PhaseVoltage` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`PowerFactorRphase` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`PowerFactorYphase` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`PowerFactorBphase` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`Temprature` varchar(40) DEFAULT NULL,");                
                sqlQuery.Append(@"`CumulativeEnergykWh` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`CumulativeEnergykVAh` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`CumulativeEnergykvarhLag` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`CumulativeEnergykvarhLead` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`TotalPowerFactor` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`CompartmentNumber` bigint(20) DEFAULT NULL,");
                sqlQuery.Append(@"`MeterData_ID` bigint(20) DEFAULT NULL,");
                sqlQuery.Append(@"`NeutralCurrent` varchar(40) DEFAULT NULL,");
				sqlQuery.Append(@"`ByPassCurrent` varchar(40) DEFAULT NULL,");
                // Net metering parameters 
                sqlQuery.Append(@"`CumulativeEnergykWhImport` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`CumulativeEnergykVAhImport` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`CumulativeEnergykWhExport` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`CumulativeEnergykVAhExport` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`HighNeutralCurrent` varchar(40) DEFAULT NULL,");//add pradipta_neu
                sqlQuery.Append(@"`kWr` varchar(40) DEFAULT NULL,");//pradipta_neu
                sqlQuery.Append(@"`kWy` varchar(40) DEFAULT NULL,");//pradipta_neu
                sqlQuery.Append(@"`kWb` varchar(40) DEFAULT NULL,");//pradipta_neu
                sqlQuery.Append(@"`kVAr` varchar(40) DEFAULT NULL,");//pradipta_neu
                sqlQuery.Append(@"`kVAy` varchar(40) DEFAULT NULL,");//pradipta_neu
                sqlQuery.Append(@"`kVAb` varchar(40) DEFAULT NULL,");//pradipta_neu
                sqlQuery.Append(@"`CumulativeTamperCount` varchar(40) DEFAULT NULL,");//smart meter
                // SB code change start - 20171129
                sqlQuery.Append(@"`ActiveCurrentR` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`ActiveCurrentY` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`ActiveCurrentB` varchar(40) DEFAULT NULL,");// SB code change end - 20171129
                //SarkarA code change start 20180330 // add phase current instant, frequency
                sqlQuery.Append(@"`Frequency` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`PhaseCurrentInstant` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`THDVR` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`THDVY` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`THDVB` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`THDIR` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`THDIY` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`THDIB` varchar(40) DEFAULT NULL,");

                //SarkarA code change end 20180330
                //sqlQuery.Append(@"`kWhAbsolute` varchar(40) DEFAULT NULL,");
                //sqlQuery.Append(@"`kVAhAbsolute` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"PRIMARY KEY (`Tamper_ID`)");
                sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");
                //Add table for anomaly flag
                sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `meterdata_anomaly` (");
                sqlQuery.Append(@"`AnomalyId` bigint(20) NOT NULL AUTO_INCREMENT,");
                sqlQuery.Append(@"`MeterDataId` bigint(20) NOT NULL,");
                sqlQuery.Append(@"`FlashStatus` TINYINT(1) NULL,");
                sqlQuery.Append(@"`EepRamStatus` TINYINT(1) NULL,");
                sqlQuery.Append(@"`SmpsStatus` TINYINT(1) NULL,");
                sqlQuery.Append(@"`RtcStatus` TINYINT(1) NULL,");
                sqlQuery.Append(@"`RTCBatteryStatus` TINYINT(1) NULL,");
                sqlQuery.Append(@"`MainBatteryStatus` TINYINT(1) NULL,");
                //sqlQuery.Append(@"`ErrorCodeStatus` TINYINT(1) NULL,");
                sqlQuery.Append(@"`ErrorCodeStatus` bigint(20) NULL,");
                sqlQuery.Append(@"PRIMARY KEY (`AnomalyId`)");
                sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");

                //Add table for fraud energy. 
                sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `meterdata_fraudenergy` (");
                sqlQuery.Append(@"`FraudEnergy_ID` bigint(20) NOT NULL AUTO_INCREMENT,");
                sqlQuery.Append(@"`MagneticInfluenceKWh` varchar(20) NOT NULL,");
                sqlQuery.Append(@"`MagneticInflueneceKVARhLag` varchar(20) NOT NULL,");
                sqlQuery.Append(@"`MagneticInflueneceKVARhLead` varchar(20) NOT NULL,");
                sqlQuery.Append(@"`MagneticInflueneceKVAh` varchar(20) NOT NULL,");
                sqlQuery.Append(@"`ReverseEnergyKWh` varchar(20) NOT NULL,");
                sqlQuery.Append(@"`ReverseEnergyKVAh` varchar(20) NOT NULL,");
                sqlQuery.Append(@"`ReverseEnergyKVARhLag` varchar(20),");
                sqlQuery.Append(@"`ReverseEnergyKVARhLead` varchar(20),");
                sqlQuery.Append(@"`THDVoltageRPhase` varchar(20),");
                sqlQuery.Append(@"`THDVoltageYPhase` varchar(20),");
                sqlQuery.Append(@"`THDVoltageBPhase` varchar(20),");
                sqlQuery.Append(@"`THDCurrentRPhase` varchar(20),");
                sqlQuery.Append(@"`THDCurrentYPhase` varchar(20),");
                sqlQuery.Append(@"`THDCurrentBPhase` varchar(20),");
                sqlQuery.Append(@"`MeterData_ID` bigint(20) DEFAULT NULL,");
                sqlQuery.Append(@"PRIMARY KEY (`FraudEnergy_ID`)");
                sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");

                /* GKG JVVNL Current TOU Read */
                //Add table for TOU Data
                sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `meterdata_tou` (");
                sqlQuery.Append(@"`TouId` bigint(20) NOT NULL AUTO_INCREMENT,");
                sqlQuery.Append(@"`MeterDataId` bigint(20) NOT NULL,");
                sqlQuery.Append(@"`StartHour` TINYINT UNSIGNED  NULL,");
                sqlQuery.Append(@"`StartMin` TINYINT UNSIGNED  NULL,");
                sqlQuery.Append(@"`Tariff` TINYINT UNSIGNED  NULL,");
                sqlQuery.Append(@"`SeasonNumber` TINYINT UNSIGNED  NULL,");
                sqlQuery.Append(@"PRIMARY KEY (`TouId`)");
                sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");
                /* GKG JVVNL Current TOU Read */


                sqlQuery.AppendLine(@"drop table if exists SchedulingReportsColumn ;");
                sqlQuery.AppendLine(@"Create table SchedulingReportsColumn");
                sqlQuery.AppendLine(@"(");
                sqlQuery.AppendLine(@"Identifier INT NOT NULL AUTO_INCREMENT,");
                sqlQuery.AppendLine(@"Profile varchar(20) NOT NULL,");
                sqlQuery.AppendLine(@"Utility varchar(50),");
                sqlQuery.AppendLine(@"DisplayName varchar(200) NOT NULL,");
                sqlQuery.AppendLine(@"MappedDBColumn varchar(200) ,");
                sqlQuery.AppendLine(@"IsSelected BIT,");
                sqlQuery.AppendLine(@"PRIMARY KEY(`Identifier`)");
                sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");


                //Add table for NamePlateProfile Data

                sqlQuery.Append(@"CREATE  TABLE IF NOT EXISTS `meterdata_Nameplate` (");
                sqlQuery.Append(@"`Nameplate_ID` BIGINT(20) NOT NULL AUTO_INCREMENT ,");
                sqlQuery.Append(@"`meterSerialNumber` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`manufacturername` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`firmwareVersionformeter` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`metertype` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`Category` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`CurrentRating` VARCHAR(20) NULL ,");
                sqlQuery.Append(@"`internalCTratio` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`internalVTratio` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`meteryearofmanufacture` VARCHAR(40) NULL ,");                
                sqlQuery.Append(@"`PrimaryMeterConstantInfo` VARCHAR(40) NULL ,");//PGVCL
                sqlQuery.Append(@"`MeterConstantInfo` VARCHAR(40) NULL ,");
                sqlQuery.Append(@"`MeterMonthOfManufacture` varchar(20) DEFAULT NULL,");
                sqlQuery.Append(@"`AccuracyClass` varchar(20) DEFAULT NULL,");               
                sqlQuery.Append(@"`MeterData_ID` bigint,");                
                sqlQuery.Append(@"PRIMARY KEY (`Nameplate_ID`)");
                sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");


                sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `meterdata_adhocread` (");
                sqlQuery.Append(@"`Adhoc_ID` bigint(20) NOT NULL AUTO_INCREMENT,");
                sqlQuery.Append(@"`AdhocColumnName` varchar(60) DEFAULT NULL,");
                sqlQuery.Append(@"`AdhocColumnValue` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`AdhocObisCode` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`AdhocClassID` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`AdhocAttribute` varchar(40) DEFAULT NULL,");
                sqlQuery.Append(@"`MeterData_ID` bigint(20) DEFAULT NULL,");
                sqlQuery.Append(@"PRIMARY KEY (`Adhoc_ID`)");
                sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");
                #endregion
            }
           
            #region "Phasor Table"
            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `meterdata_phasor` (");
            sqlQuery.Append(@"`PhasorId` bigint(20) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`MeterData_Id` bigint(20) NOT NULL,");
            sqlQuery.Append(@"`PhasorDateTime` bigint(20) NOT NULL,");
            sqlQuery.Append(@"`RPhaseCurrent` varchar(20) NULL,");
            sqlQuery.Append(@"`YPhaseCurrent` varchar(20) NULL,");
            sqlQuery.Append(@"`BPhaseCurrent` varchar(20) NULL,");
            sqlQuery.Append(@"`RPhaseVoltage` varchar(20) NULL,");
            sqlQuery.Append(@"`YPhaseVoltage` varchar(20) NULL,");
            sqlQuery.Append(@"`BPhaseVoltage` varchar(20) NULL,");
            sqlQuery.Append(@"`RPhasePowerFactor` varchar(20) NULL,");
            sqlQuery.Append(@"`YPhasePowerFactor` varchar(20) NULL,");
            sqlQuery.Append(@"`BPhasePowerFactor` varchar(20) NULL,");
            sqlQuery.Append(@"`TotalPhasePowerFactor` varchar(20) NULL,");
            sqlQuery.Append(@"`Frequency` varchar(20) NULL,");
            sqlQuery.Append(@"`ApparentPower` varchar(20) NULL,");
            sqlQuery.Append(@"`ActivePower` varchar(20) NULL,");
            sqlQuery.Append(@"`ReactivePower` varchar(20) NULL,");
            sqlQuery.Append(@"`RPhaseNegativePowerFlag` varchar(20) NULL,");
            sqlQuery.Append(@"`YPhaseNegativePowerFlag` varchar(20) NULL,");
            sqlQuery.Append(@"`BPhaseNegativePowerFlag` varchar(20) NULL,");
            sqlQuery.Append(@"`RPhaseCapacitiveInductiveFlag` varchar(20) NULL,");
            sqlQuery.Append(@"`YPhaseCapacitiveInductiveFlag` varchar(20) NULL,");
            sqlQuery.Append(@"`BPhaseCapacitiveInductiveFlag` varchar(20) NULL,");
            sqlQuery.Append(@"`AngleYR` varchar(20) NULL,");
            sqlQuery.Append(@"`AngleBR` varchar(20) NULL,");
            sqlQuery.Append(@"`AngleBetweenTwo` varchar(20) NULL,");
            sqlQuery.Append(@"`RPhaseChannel` varchar(20) NULL,");
            sqlQuery.Append(@"`YPhaseChannel` varchar(20) NULL,");
            sqlQuery.Append(@"`BPhaseChannel` varchar(20) NULL,");
            sqlQuery.Append(@"`PhaseSequence` varchar(20) NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`PhasorId`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");
            #endregion

            #region "Load Switch table"
            sqlQuery.Append(@"CREATE TABLE IF NOT EXISTS `meterdata_loadswitch` (");
            sqlQuery.Append(@"`LoadSwitchId` bigint(20) NOT NULL AUTO_INCREMENT,");
            sqlQuery.Append(@"`MeterData_Id` bigint(20) NULL,");
            sqlQuery.Append(@"`Controleventconnectdisconnect` varchar(20) NULL,");
            sqlQuery.Append(@"`RTC` varchar(20) NULL,");
            sqlQuery.Append(@"`Switchoperationreason` varchar(20) NULL,");
            sqlQuery.Append(@"`Cumulativeenergykwh` varchar(20) NULL,");
            sqlQuery.Append(@"`CumulativeenergykwhTZ1` varchar(20) NULL,");
            sqlQuery.Append(@"`CumulativeenergykwhTZ2` varchar(20) NULL,");
            sqlQuery.Append(@"`CumulativeenergykwhTZ3` varchar(20) NULL,");
            sqlQuery.Append(@"`CumulativeenergykwhTZ4` varchar(20) NULL,");
            sqlQuery.Append(@"`CumulativeenergykwhTZ5` varchar(20) NULL,");
            sqlQuery.Append(@"`CumulativeenergykwhTZ6` varchar(20) NULL,");
            sqlQuery.Append(@"`CumulativeenergykwhTZ7` varchar(20) NULL,");
            sqlQuery.Append(@"`CumulativeenergykwhTZ8` varchar(20) NULL,");
            sqlQuery.Append(@"`Cumulativeenergykvah` varchar(20) NULL,");
            sqlQuery.Append(@"`CumulativeenergykvahTZ1` varchar(20) NULL,");
            sqlQuery.Append(@"`CumulativeenergykvahTZ2` varchar(20) NULL,");
            sqlQuery.Append(@"`CumulativeenergykvahTZ3` varchar(20) NULL,");
            sqlQuery.Append(@"`CumulativeenergykvahTZ4` varchar(20) NULL,");
            sqlQuery.Append(@"`CumulativeenergykvahTZ5` varchar(20) NULL,");
            sqlQuery.Append(@"`CumulativeenergykvahTZ6` varchar(20) NULL,");
            sqlQuery.Append(@"`CumulativeenergykvahTZ7` varchar(20) NULL,");
            sqlQuery.Append(@"`CumulativeenergykvahTZ8` varchar(20) NULL,");
            sqlQuery.Append(@"PRIMARY KEY (`LoadSwitchId`)");
            sqlQuery.Append(@") ENGINE=InnoDB DEFAULT CHARSET=utf8;");
            #endregion
            
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
                /* VBM add  */
                string[] strArrayQuery = QueryToCreateStoredProcedures().Split('~');
                foreach (string strQuery in strArrayQuery)
                {
                    DataRequest dataRequest = new DataRequest(strQuery);
                    helper.ExecuteNonQuery(dataRequest);
                }

                InsertMasterData();
                Flag = true;
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "CreateCABAppDatabase()", ex);
                Flag = false;
            }
            return Flag;
        }


        public void InsertMasterData()
        {
            SystemSettingsDAL settingsDAL = new SystemSettingsDAL();
            settingsDAL.InsertSystemSettings();

            // GPRS ,Insertion of master data for GPRS reports 
            try
            {
                // Load the xml
                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                xmlDoc.Load("SchedulingReportColumns.xml");

                // InsertGPRSSchedulingReportColumns
                settingsDAL.InsertGPRSSchedulingReportColumns(xmlDoc.OuterXml);


            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "InsertMasterData()", ex);
            }

        }
        public void DeleteAllData()
        {
            try
            {
                string[] qry = GetTableList();
                for (int i = 0; i < qry.Length; i++)
                {
                    IDataHelper helper = DatabaseFactory.GetHelper();
                    DataRequest request = new DataRequest(qry[i]);
                    if (helper.ExecuteNonQuery(request) == -5)
                        throw new CABException();

                }
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog("Database data deleted");
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "DeleteAllData()", ex);
                throw new Exception();
            }
        }

        private string[] GetTableList()
        {
            List<string> query = new List<string>();

            query.Add("delete from areamaster");
            query.Add("delete from areameter_master");
            query.Add("delete from asciiexportsettings");
            query.Add("delete from categoryright");
            query.Add("delete from category_master");
            query.Add("delete from circle_master");
            query.Add("delete from cmri_master");
            query.Add("delete from consumerexportsettings");
            query.Add("delete from consumermeter");
            query.Add("delete from consumertype_master");
            query.Add("delete from consumer_master");
            query.Add("delete from designation_master");
            query.Add("delete from division_master");
            query.Add("delete from dtmdailyprofileparameter");
            query.Add("delete from exceptionlog");
            query.Add("delete from fileupload_master");
            query.Add("delete from group_master");
            query.Add("delete from gsmgroupschedule");
            query.Add("delete from gsmreadingstatus");
            query.Add("delete from gsmschedule");
            query.Add("delete from history_master");
            query.Add("delete from loadsurveyparameter");
            query.Add("delete from meterdata");
            query.Add("delete from meterdata_billing");
            query.Add("delete from meterdata_dtmdailyprofile");
            query.Add("delete from meterdata_dtmloadsurvey");
            query.Add("delete from meterdata_fraudenergy");
            query.Add("delete from meterdata_general");
            query.Add("delete from meterdata_instantpower");
            query.Add("delete from meterdata_loadsurvey");
            query.Add("delete from meterdata_phasor");
            query.Add("delete from meterdata_powerfactor");
            query.Add("delete from meterdata_programming");
            query.Add("delete from meterdata_rtcupdate");
            query.Add("delete from meterdata_tampercounter");
            query.Add("delete from meterdata_tampercountergeneral");
            query.Add("delete from meterdata_tampersnapshot");
            query.Add("delete from meterdata_tariffinformation");
            query.Add("delete from metermodel_master");
            query.Add("delete from metertype_master");
            query.Add("delete from meterunit_master");
            query.Add("delete from meter_master");
            query.Add("delete from midnightparameter");
            query.Add("delete from billingparameter");
            query.Add("delete from modulecategory_master");
            query.Add("delete from module_master");
            query.Add("delete from rcdmeter_master");
            query.Add("delete from region_master");
            query.Add("delete from subgroupmeter_master");
            query.Add("delete from subgroup_master");
            query.Add("delete from suspectedconsumer");
            query.Add("delete from tampertype_master");
            query.Add("delete from tamper_master");
            query.Add("delete from userinformation");
            query.Add("delete from userlogactivity");
            query.Add("delete from userrights");
            query.Add("delete from versioninfo");
            query.Add("delete from versionmaster");
            query.Add("delete from meter_master_log");
            query.Add("delete from system_settings");
            query.Add("delete from meterdata_phasor");
            query.Add("delete from tabname");
            query.Add("delete from TOD");
            query.Add("delete from DisplayParamater");
            query.Add("delete from RTC");
            query.Add("delete from kvarselection");
            query.Add("delete from rs232");
            query.Add("delete from rs485");
            query.Add("delete from MDWithIp");
            query.Add("delete from DailyLog");
            query.Add("delete from BillingType");
            query.Add("delete from AutoLock");
            query.Add("delete from LSIP");
            query.Add("delete from DIP");
            query.Add("delete from tamperparameter");
            query.Add("delete from ManualBilling");
            query.Add("delete from SoftwareBilling");
            query.Add("delete from disconnectcontrol");
            query.Add("delete from loadcontrol");
            query.Add("delete from paymentmode");
            query.Add("delete from meteringmode");
            query.Add("delete from loadlimit");
            query.Add("delete from Slidingdemand");
            query.Add("delete from Opticallock");
            query.Add("delete from RJLock");
            query.Add("delete from meterdata_loadswitch");
            query.Add("delete from gsmconfigstatus");
            query.Add("delete from gsmreadstatus");
            query.Add("delete from pulseEnergy");
            query.Add("delete from ManualButtonMDReset");
            query.Add("delete from meterdata_adhocread");

            if (apptype.Equals(ApplicationType.DLMS_LTCT_650))
            {
                query.Add("delete from StructureInfo");
                query.Add("delete from StructureUnitInfo");
                query.Add("delete from StructureOBISInfo");
                query.Add("delete from meterdata_midnightdata");
                query.Add("delete from meterdata_anomaly");
                query.Add("delete from meterdata_tou");

                if (ConfigInfo.GetTenderType() == TenderType.JUSCO)
                {
                    query.Add("delete from gsm_tasks");
                    query.Add("delete from gsm_groups");
                    query.Add("delete from gsm_group_meters");
                    query.Add("delete from gsm_task_logs");
                    query.Add("delete from gsm_tasks_completed");
                }

            }

            return query.ToArray();
        }

        private string[] GetDropTableList()
        {
            List<string> query = new List<string>();

            query.Add("DROP TABLE IF EXISTS `areamaster`");
            query.Add("DROP TABLE IF EXISTS `areameter_master`");
            query.Add("DROP TABLE IF EXISTS `asciiexportsettings`");
            query.Add("DROP TABLE IF EXISTS `categoryright`");
            query.Add("DROP TABLE IF EXISTS `category_master`");
            query.Add("DROP TABLE IF EXISTS `circle_master`");
            query.Add("DROP TABLE IF EXISTS `cmri_master`");
            query.Add("DROP TABLE IF EXISTS `consumerexportsettings`");
            query.Add("DROP TABLE IF EXISTS `consumermeter`");
            query.Add("DROP TABLE IF EXISTS `consumertype_master`");
            query.Add("DROP TABLE IF EXISTS `consumer_master`");
            query.Add("DROP TABLE IF EXISTS `designation_master`");
            query.Add("DROP TABLE IF EXISTS `division_master`");
            query.Add("DROP TABLE IF EXISTS `dtmdailyprofileparameter`");
            query.Add("DROP TABLE IF EXISTS `exceptionlog`");
            query.Add("DROP TABLE IF EXISTS `fileupload_master`");
            query.Add("DROP TABLE IF EXISTS `group_master`");
            query.Add("DROP TABLE IF EXISTS `gsmgroupschedule`");
            query.Add("DROP TABLE IF EXISTS `gsmreadingstatus`");
            query.Add("DROP TABLE IF EXISTS `gsmschedule`");
            query.Add("DROP TABLE IF EXISTS `history_master`");
            query.Add("DROP TABLE IF EXISTS `loadsurveyparameter`");
            query.Add("DROP TABLE IF EXISTS `meterdata`");
            query.Add("DROP TABLE IF EXISTS `meterdata_billing`");
            query.Add("DROP TABLE IF EXISTS `meterdata_dtmdailyprofile`");
            query.Add("DROP TABLE IF EXISTS `meterdata_dtmloadsurvey`");
            query.Add("DROP TABLE IF EXISTS `meterdata_fraudenergy`");
            query.Add("DROP TABLE IF EXISTS `meterdata_general`");
            query.Add("DROP TABLE IF EXISTS `meterdata_nameplate`");
            query.Add("DROP TABLE IF EXISTS `meterdata_instantpower`");
            query.Add("DROP TABLE IF EXISTS `meterdata_loadsurvey`");
            query.Add("DROP TABLE IF EXISTS `meterdata_phasor`");
            query.Add("DROP TABLE IF EXISTS `meterdata_powerfactor`");
            query.Add("DROP TABLE IF EXISTS `meterdata_programming`");
            query.Add("DROP TABLE IF EXISTS `meterdata_rtcupdate`");
            query.Add("DROP TABLE IF EXISTS `meterdata_tampercounter`");
            query.Add("DROP TABLE IF EXISTS `meterdata_tampercountergeneral`");
            query.Add("DROP TABLE IF EXISTS `meterdata_tampersnapshot`");
            query.Add("DROP TABLE IF EXISTS `meterdata_tariffinformation`");
            query.Add("DROP TABLE IF EXISTS `meterdata_midnightdata`");
            query.Add("DROP TABLE IF EXISTS `metermodel_master`");
            query.Add("DROP TABLE IF EXISTS `metertype_master`");
            query.Add("DROP TABLE IF EXISTS `meterunit_master`");
            query.Add("DROP TABLE IF EXISTS `meter_master`");
            query.Add("DROP TABLE IF EXISTS `midnightparameter`");
            query.Add("DROP TABLE IF EXISTS `billingparameter`");
            query.Add("DROP TABLE IF EXISTS `modulecategory_master`");
            query.Add("DROP TABLE IF EXISTS `module_master`");
            query.Add("DROP TABLE IF EXISTS `rcdmeter_master`");
            query.Add("DROP TABLE IF EXISTS `region_master`");
            query.Add("DROP TABLE IF EXISTS `subgroupmeter_master`");
            query.Add("DROP TABLE IF EXISTS `subgroup_master`");
            query.Add("DROP TABLE IF EXISTS `suspectedconsumer`");
            query.Add("DROP TABLE IF EXISTS `tampertype_master`");
            query.Add("DROP TABLE IF EXISTS `tamper_master`");
            query.Add("DROP TABLE IF EXISTS `userinformation`");
            query.Add("DROP TABLE IF EXISTS `userlogactivity`");
            query.Add("DROP TABLE IF EXISTS `userrights`");
            query.Add("DROP TABLE IF EXISTS `versioninfo`");
            query.Add("DROP TABLE IF EXISTS `versionmaster`");
            query.Add("DROP TABLE IF EXISTS `tabname`");
            query.Add("DROP TABLE IF EXISTS `TOD`");
            query.Add("DROP TABLE IF EXISTS `DisplayParamater`");
            query.Add("DROP TABLE IF EXISTS `RTC`");
            query.Add("DROP TABLE IF EXISTS `kvarselection`");
            query.Add("DROP TABLE IF EXISTS `rs232`");
            query.Add("DROP TABLE IF EXISTS `rs485`");
            query.Add("DROP TABLE IF EXISTS `MDWithIp`");
            query.Add("DROP TABLE IF EXISTS `DailyLog`");
            query.Add("DROP TABLE IF EXISTS `BillingType`");
            query.Add("DROP TABLE IF EXISTS `AutoLock`");
            query.Add("DROP TABLE IF EXISTS `LSIP`");
            query.Add("DROP TABLE IF EXISTS `DIP`");
            query.Add("DROP TABLE IF EXISTS `tamperparameter`");
            query.Add("DROP TABLE IF EXISTS `ManualBilling`");
            query.Add("DROP TABLE IF EXISTS `SoftwareBilling`");
            query.Add("DROP TABLE IF EXISTS 'disconnectcontrol'");
            query.Add("DROP TABLE IF EXISTS 'loadcontrol'");
            query.Add("DROP TABLE IF EXISTS `paymentmode`");
            query.Add("DROP TABLE IF EXISTS `meteringmode`");
            query.Add("DROP TABLE IF EXISTS 'loadlimit'");
            query.Add("DROP TABLE IF EXISTS 'Slidingdemand'");
            query.Add("DROP TABLE IF EXISTS 'Opticallock'");
            query.Add("DROP TABLE IF EXISTS 'RJLock'");
            query.Add("DROP TABLE IF EXISTS 'meterdata_loadswitch'");
            query.Add("DROP TABLE IF EXISTS `pulseEnergy`");
            query.Add("DROP TABLE IF EXISTS `ManualButtonMDReset`");
            query.Add("DROP TABLE IF EXISTS `meterdata_adhocread`");

            // Adding extra table for logging BillingType master details when installed CT/PT 
            // is changed - DHBVNL June 2011
            query.Add("DROP TABLE IF EXISTS `meter_master_log`");
            query.Add("DROP TABLE IF EXISTS `system_settings`");
            query.Add("DROP TABLE IF EXISTS `meterdata_phasor`");
            if (apptype.Equals(ApplicationType.DLMS_LTCT_650))
            {
                query.Add("DROP TABLE IF EXISTS `StructureInfo`");
                query.Add("DROP TABLE IF EXISTS `StructureUnitInfo`");
                query.Add("DROP TABLE IF EXISTS `StructureOBISInfo`");
                query.Add("delete from meterdata_midnightdata");
                query.Add("DROP TABLE IF EXISTS meterdata_anomaly");
                /* GKG JVVNL Current TOU Read */
                query.Add("DROP TABLE IF EXISTS meterdata_tou");
                /* GKG JVVNL Current TOU Read */
                /* VBM  Drop SP GetTamperStartEndDate */
                query.Add("DROP PROCEDURE IF EXISTS GetTamperStartEndDate");
                /* VBM  Drop SP GetTamperStartEndDate*/
                /* VBM  Drop SP GetTamperDetailByTamperType */
                query.Add("DROP PROCEDURE IF EXISTS GetTamperDetailByTamperType;DROP PROCEDURE IF EXISTS GetTamperDetailByDates");
                query.Add("DROP PROCEDURE IF EXISTS GetTamperDetailByDatesWithCompartmentID");
                /* VBM  Drop SP GetTamperDetailByTamperType*/
                /* VBM  Drop SP GetTamperTypeWithCount */
                query.Add("DROP PROCEDURE IF EXISTS GetTamperTypeWithCount");
                /* VBM  Drop SP GetTamperTypeWithCount*/
                /* VBM  Drop SP GetBillingDemand */
                query.Add("DROP PROCEDURE IF EXISTS GetBillingDemand");

                // GPRS: Added to support gprs routine 
                query.Add("DROP PROCEDURE IF EXISTS GetTasks");

                query.Add("DROP PROCEDURE IF EXISTS UpdateTask");

                query.Add("DROP PROCEDURE IF EXISTS ReQueueFailedTasks");

                query.Add("DROP PROCEDURE IF EXISTS BulkUpdateEndPointSyncStatus");
                query.Add("DROP PROCEDURE IF EXISTS GetFileUploadDetails");
                query.Add("DROP PROCEDURE IF EXISTS InsertSchedulingColumns");

                query.Add("DROP PROCEDURE IF EXISTS UpdateParametersSelection");
                query.Add("DROP PROCEDURE IF EXISTS GetReportColumns");
                query.Add("DROP PROCEDURE IF EXISTS GetDataForExcelExport");
                query.Add("DROP PROCEDURE IF EXISTS GetDataForTextExport");
                query.Add("DROP PROCEDURE IF EXISTS GetGPRSSearchData");
                query.Add("DROP PROCEDURE IF EXISTS GETRemoteCommunicationLogs");

                if (ConfigInfo.GetTenderType() == TenderType.JUSCO)
                {
                    query.Add("DROP TABLE IF EXISTS `gsm_groups`");
                    query.Add("DROP TABLE IF EXISTS `gsm_group_meters`");
                    query.Add("DROP TABLE IF EXISTS `gsm_tasks`");
                    query.Add("DROP TABLE IF EXISTS gsm_task_logs");
                    query.Add("DROP TABLE IF EXISTS gsm_tasks_completed");
                }
            }            
            query.Add("DROP TABLE IF EXISTS utility");
            query.Add("DROP TABLE IF EXISTS gsmconfigstatus");
            query.Add("DROP TABLE IF EXISTS gsmreadstatus");
            
            return query.ToArray();
        }

        public void DropAllTable()
        {
            try
            {
                string[] qry = GetDropTableList();
                for (int i = 0; i < qry.Length; i++)
                {
                    IDataHelper helper = DatabaseFactory.GetHelper();
                    DataRequest request = new DataRequest(qry[i]);
                    helper.ExecuteNonQuery(request);
                }
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog("Database tables deleted");
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "DropAllTable()", ex);
            }
        }

        #region Not Implemented
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
        #endregion
    }
}
