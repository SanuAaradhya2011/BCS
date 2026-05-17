/* |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 									        |
 * |											Date   : 22/04/2010 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| 
 */

using System.Data;
using CAB.DALC.Data;
using CAB.Framework;
using CAB.Framework.Entity;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System;
using CAB.Framework.Utility;
using Hunt.EPIC.Logging;


namespace CAB.BLL
{
    public class ReportBLL : IBLL
    {
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(ReportBLL).ToString());
        ReportDAL reportDAL;

        public ReportBLL()
        {
            reportDAL = new ReportDAL();
        }

        public DataSet GetGeneralReportData(long activeMeterData_ID)
        {
            return reportDAL.GetGeneralReportData(activeMeterData_ID);
        }

        public DataSet GetInstantReportData(long activeMeterData_ID)
        {
            return reportDAL.GetInstantReportData(activeMeterData_ID);
        }

        public DataSet GetTamperReportData(long activeMeterData_ID)
        {
            return reportDAL.GetTamperReportData(activeMeterData_ID);
        }
        public DataSet GetPowerOnHoursReportData(long activeMeterData_ID)
        {
            return reportDAL.GetPowerOnHoursReportData(activeMeterData_ID);
        }

        public DataSet GetPowerFactorReportData(long activeMeterData_ID)
        {
            return reportDAL.GetPowerFactorReportData(activeMeterData_ID);
        }

        public DataSet GetCTRatioReportData(long activeMeterData_ID)
        {
            return reportDAL.GetCTRatioReportData(activeMeterData_ID);
        }

        //added for MVVNL
        public DataSet GetMidnightEnergiesReportData(long activeMeterData_ID)
        {
            return reportDAL.GetMidnightEnergiesReportData(activeMeterData_ID);
        }
        //added for MVVNL

        public DataSet GetLoadFactorReportData(long activeMeterData_ID)
        {
            return reportDAL.GetLoadFactorReportData(activeMeterData_ID);
        }

        public DataSet GetBillingTamperCounterReportData(long activeMeterData_ID)
        {
            return reportDAL.GetBillingTamperCounterReportData(activeMeterData_ID);
        }

        public DataSet GetMainEnergyReportData(long activeMeterData_ID)
        {
            return reportDAL.GetMainEnergyReportData(activeMeterData_ID);
        }

        public Dictionary<string, string> MergeDictionary(params Dictionary<string, string>[] dictionaries)
        {
            Dictionary<string, string> finalDictionary = new Dictionary<string, string>();
            foreach (Dictionary<string, string> dictionary in dictionaries)
            {
                foreach (KeyValuePair<string, string> kvp in dictionary)
                {
                    finalDictionary.Add(kvp.Key, kvp.Value);
                }
            }
            return finalDictionary;
        }

        /// <summary>
        /// facade for datalayer 
        /// </summary>
        /// <param name="profile"></param>
        /// <param name="utility"></param>
        /// <returns></returns>
        public static DataSet GetSchedulingReportColumns(string profile, string utility)
        {
            return ReportDAL.GetSchedulingReportColumns(profile, utility);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="availableListXml"></param>
        /// <param name="selectedListXml"></param>
        /// <param name="Profile"></param>
        public static void UpdateParametersSelection(string availableListXml, string selectedListXml, string Profile)
        {
            ReportDAL.UpdateParametersSelection(availableListXml, selectedListXml, Profile);
        }


        /// <summary>
        /// Method to generate query dynamically and return the result set
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="profile"></param>
        /// <returns></returns>
        public static DataTable GetGPRSReportData(ReportConfigurationParameters parameters, Profile profile)
        {
            DataTable tableReport=new DataTable();
           
            try
            {

                // getting the report columns
                DataSet reportColumnDs = GetSchedulingReportColumns(profile.ToString(), UtilityDetails.GetUtilityDetails().ToString());

               DataRow[] selectedColumns= reportColumnDs.Tables[0].Select("IsSelected=1");


               string[] columnWithDisplayName = (from row in selectedColumns.AsEnumerable()
                                                       select row["MappedDBColumn"].ToString().Trim() + " '" + row["DisplayName"].ToString().Trim() + "'").ToArray<string>();


               string[] columnNames = (from row in selectedColumns.AsEnumerable()
                                        select row["MappedDBColumn"].ToString().Trim()).ToArray<string>();

               string[] displayNames = (from row in selectedColumns.AsEnumerable()
                                        select row["DisplayName"].ToString().Trim()).ToArray<string>();


               Dictionary<string,string>  columnValueLookup = (from row in selectedColumns.AsEnumerable()
                                        select new { columnName = row["DisplayName"].ToString().Trim(), Value = row["MappedDBColumn"].ToString().Trim() }).ToDictionary(a => a.columnName, a => a.Value);


                string queryString = string.Empty;
                

                switch (parameters.Type.ToString())
                {
                    case "RUNNING":

                        switch (profile.ToString())
                        {
                            case "GENERAL":
                                queryString = "Select " + string.Join(",", columnWithDisplayName) + " from meterdata_general Where meterdata_general.meterData_Id in (select meterdata_Id from gsm_tasks,meterdata where gsm_tasks.tasksid=meterdata.taskid and gsm_tasks.taskstatus='Inprogress')";
                            
                                break;
                            case "BILLING":
                                queryString = "Select " + string.Join(",", columnWithDisplayName) + ",meterid 'Meter Serial Number',DataIndex 'History' from meterdata_billing,meterdata Where meterdata_billing.meterData_Id in (select meterdata_Id from gsm_tasks,meterdata where gsm_tasks.tasksid=meterdata.taskid and gsm_tasks.taskstatus='Inprogress') and meterdata_billing.meterData_Id=meterdata.meterdata_Id group by meterdata_billing.meterdata_id,meterid,dataindex order by billing_id,dataindex";
                                break;
                            //case "LOADSURVEY":
                            //    queryString = "Select " + string.Join(",", columnWithDisplayName) + " from meterdata_loadsurvey Where meterdata_loadsurvey.meterData_Id in (select meterdata_Id from gsm_tasks,meterdata where gsm_tasks.tasksid=meterdata.taskid and gsm_tasks.taskstatus='Inprogress')";
                            //    break;
                            case "TAMPER":
                                queryString = "select DISTINCT EVENTCODE,meterid,tamper_master.meterdata_Id  from  meterdata left join tamper_master on meterdata.meterData_Id=tamper_master.meterData_Id where tamper_master.meterData_Id in (select meterdata_Id from gsm_tasks,meterdata where gsm_tasks.tasksid=meterdata.taskid and gsm_tasks.taskstatus='Inprogress') and tamper_master.meterData_Id=meterdata.meterData_Id";
                               // queryString = "select DISPLAYNAME,EVENTCODE,MAPPEDDBCOLUMN,meterid,meterData_Id from schedulingreportscolumn left join (select DISTINCT EVENTCODE,meterid,tamper_master.meterdata_Id  from  meterdata left join tamper_master on meterdata.meterData_Id=tamper_master.meterData_Id where tamper_master.meterData_Id in (select meterdata_Id from gsm_tasks,meterdata where gsm_tasks.tasksid=meterdata.taskid and gsm_tasks.taskstatus='Inprogress') and tamper_master.meterData_Id=meterdata.meterData_Id) result on result.eventcode=schedulingreportscolumn.MappedDbColumn where schedulingreportscolumn.profile='TAMPER' and isselected=1";
                                break;
                            case "INSTANTANEOUS":
                                queryString = "select DISPLAYNAME,InstantPowerColumnValue,MAPPEDDBCOLUMN,meterid,meterData_Id from schedulingreportscolumn left join (select DISTINCT  InstantPowerColumnName,InstantPowerColumnValue,meterid,meterdata.meterdata_Id from  gsm_tasks,meterdata left join meterdata_instantpower on meterdata.meterdata_Id=meterdata_instantpower.meterdata_Id where gsm_tasks.taskstatus='Inprogress' and meterdata.taskid=gsm_tasks.tasksid) result on result.InstantPowerColumnName=schedulingreportscolumn.MappedDbColumn where schedulingreportscolumn.profile='INSTANTANEOUS' and isselected=1";
                                break;

                        }

                        break;
                    case "METERWISE":

                    switch (profile.ToString())
                        {
                            case "GENERAL":
                                queryString = "Select " + string.Join(",", columnWithDisplayName) + " from meterdata_general Where meterdata_general.meterData_Id in (select meterdata_Id from gsm_tasks_completed,meterdata where gsm_tasks_completed.tasksid=meterdata.taskid and gsm_tasks_completed.startdate between " + parameters.StartDate.ToShortDateTimeCABFormat() + " and " + parameters.EndDate.ToShortDateTimeCABFormat() + "and meterdata.MeterId in (" + string.Join(",", parameters.Meters) + "))";
                                break;
                            case "BILLING":
                                queryString = "Select " + string.Join(",", columnWithDisplayName) + " ,meterid 'Meter Serial Number',DataIndex 'History' from meterdata_billing,meterdata Where meterdata_billing.meterData_Id in (select meterdata_Id from gsm_tasks_completed,meterdata where gsm_tasks_completed.tasksid=meterdata.taskid and gsm_tasks_completed.startdate between " + parameters.StartDate.ToShortDateTimeCABFormat() + " and " + parameters.EndDate.ToShortDateTimeCABFormat() + "and meterdata.MeterId in (" + string.Join(",", parameters.Meters) + ")) and meterdata_billing.meterData_Id=meterdata.meterdata_Id group by meterdata_billing.meterdata_id,meterid,dataindex order by billing_id,dataindex";
                                break;
                            //case "LOADSURVEY":
                            //    queryString = "Select " + string.Join(",", columnWithDisplayName) + " from meterdata_loadsurvey Where meterdata_loadsurvey.meterData_Id in (select meterdata_Id from gsm_tasks_completed,meterdata where gsm_tasks_completed.tasksid=meterdata.taskid and gsm_tasks_completed.startdate between " + parameters.StartDate.ToShortDateTimeCABFormat() + " and " + parameters.EndDate.ToShortDateTimeCABFormat() + "and meterdata.MeterId in (" + string.Join(",", parameters.Meters) + "))";
                            //    break;
                            case "TAMPER":
                                queryString = "select DISTINCT EVENTCODE,meterid,tamper_master.meterdata_Id  from  meterdata left join tamper_master on meterdata.meterData_Id=tamper_master.meterData_Id where tamper_master.meterData_Id in (select meterdata_Id from gsm_tasks_completed,meterdata where gsm_tasks_completed.tasksid=meterdata.taskid and gsm_tasks_completed.startdate between " + parameters.StartDate.ToShortDateTimeCABFormat() + " and " + parameters.EndDate.ToShortDateTimeCABFormat() + "and meterdata.MeterId in (" + string.Join(",", parameters.Meters) + ")) and tamper_master.meterData_Id=meterdata.meterData_Id";
                                break;
                            case "INSTANTANEOUS":
                                queryString = "select DISPLAYNAME,InstantPowerColumnValue,MAPPEDDBCOLUMN,meterid,meterData_Id from schedulingreportscolumn left join (select DISTINCT  InstantPowerColumnName,InstantPowerColumnValue,meterid,meterdata.meterdata_Id from  gsm_tasks_completed,meterdata left join meterdata_instantpower on meterdata.meterdata_Id=meterdata_instantpower.meterdata_Id where gsm_tasks_completed.tasksid=meterdata.taskid and gsm_tasks_completed.startdate between " + parameters.StartDate.ToShortDateTimeCABFormat() + " and " + parameters.EndDate.ToShortDateTimeCABFormat() + ") result on result.InstantPowerColumnName=schedulingreportscolumn.MappedDbColumn where schedulingreportscolumn.profile='INSTANTANEOUS' and isselected=1";
                                break;
                        }
                        break;
                    case "SCHEDULEWISE":

                        switch (profile.ToString())
                        {
                            case "GENERAL":
                                queryString = "Select " + string.Join(",", columnWithDisplayName) + " from meterdata_general Where meterdata_general.meterData_Id in (select meterdata_Id from gsm_tasks_completed,meterdata where gsm_tasks_completed.tasksid=meterdata.taskid and gsm_tasks_completed.startdate between " + parameters.StartDate.ToShortDateTimeCABFormat() + " and " + parameters.EndDate.ToShortDateTimeCABFormat() + "and gsm_tasks_completed.tasksid in (" + string.Join(",", parameters.ScheduleIds) + "))";
                                break;
                            case "BILLING":
                                queryString = "Select " + string.Join(",", columnWithDisplayName) + ",meterid 'Meter Serial Number',DataIndex 'History' from meterdata_billing,meterdata Where meterdata_billing.meterData_Id in (select meterdata_Id from gsm_tasks_completed,meterdata where gsm_tasks_completed.tasksid=meterdata.taskid and gsm_tasks_completed.startdate between " + parameters.StartDate.ToShortDateTimeCABFormat() + " and " + parameters.EndDate.ToShortDateTimeCABFormat() + "and gsm_tasks_completed.tasksid in (" + string.Join(",", parameters.ScheduleIds) + "))and meterdata_billing.meterData_Id=meterdata.meterdata_Idgroup by meterdata_billing.meterdata_id,meterid,dataindex order by billing_id,dataindex";
                                break;
                            //case "LOADSURVEY":
                            //    queryString = "Select " + string.Join(",", columnWithDisplayName) + " from meterdata_loadsurvey Where meterdata_loadsurvey.meterData_Id in (select meterdata_Id from gsm_tasks_completed,meterdata where gsm_tasks_completed.tasksid=meterdata.taskid and gsm_tasks_completed.startdate between " + parameters.StartDate.ToShortDateTimeCABFormat() + " and " + parameters.EndDate.ToShortDateTimeCABFormat() + "and gsm_tasks_completed.tasksid in (" + string.Join(",", parameters.ScheduleIds) + "))";
                            //    break;
                            case "TAMPER":
                                queryString = "select DISTINCT EVENTCODE,meterid,tamper_master.meterdata_Id  from  meterdata left join tamper_master on meterdata.meterData_Id=tamper_master.meterData_Id where tamper_master.meterData_Id in (select meterdata_Id from gsm_tasks_completed,meterdata where gsm_tasks_completed.tasksid=meterdata.taskid and gsm_tasks_completed.startdate between " + parameters.StartDate.ToShortDateTimeCABFormat() + " and " + parameters.EndDate.ToShortDateTimeCABFormat() + "and gsm_tasks_completed.tasksid in (" + string.Join(",", parameters.ScheduleIds) + ")) and tamper_master.meterData_Id=meterdata.meterData_Id"; 
                                break;
                            case "INSTANTANEOUS":
                                queryString = "select DISPLAYNAME,InstantPowerColumnValue,MAPPEDDBCOLUMN,meterid,meterData_Id from schedulingreportscolumn left join (select DISTINCT  InstantPowerColumnName,InstantPowerColumnValue,meterid,meterdata.meterdata_Id from  gsm_tasks_completed,meterdata left join meterdata_instantpower on meterdata.meterdata_Id=meterdata_instantpower.meterdata_Id where gsm_tasks_completed.tasksid=meterdata.taskid and gsm_tasks_completed.startdate between " + parameters.StartDate.ToShortDateTimeCABFormat() + " and " + parameters.EndDate.ToShortDateTimeCABFormat() + "and gsm_tasks_completed.tasksid in (" + string.Join(",", parameters.ScheduleIds) + ")) result on result.InstantPowerColumnName=schedulingreportscolumn.MappedDbColumn where schedulingreportscolumn.profile='INSTANTANEOUS' and isselected=1";
                                break;

                        }
                        break;
                }

                // call the datalayer to execute the getscripts
           

               tableReport = ReportDAL.GetGPRSReportData(queryString);

               // different transformation will be applied as per profile
                if (profile == Profile.INSTANTANEOUS)
                {
                    tableReport = ArrangeInstantTable(tableReport, displayNames);
            
                }
                if (profile == Profile.TAMPER)
                {
                    tableReport = ArrangeTamperTable(tableReport, displayNames, columnValueLookup);
                }
                if (profile == Profile.GENERAL && tableReport.Columns.Contains("Manufacturer name"))
                {
                    foreach(DataRow dr in tableReport.Rows)
                    {
                
                        if (dr["Manufacturer name"].ToString().Trim().ToUpper().Equals("LGZ"))
                            {
                                dr["Manufacturer name"] = "Cabcon Technologies ";
                            }
                    }

                    tableReport.Columns["Meter Serial Number"].SetOrdinal(0);
                }
                if (profile == Profile.BILLING)
                {
                  tableReport = ArrangeBillingTable(tableReport);
                }

               
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetGPRSReportData(ReportConfigurationParameters parameters, Profile profile)", ex);
            }

            return tableReport;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableReport"></param>
        /// <returns></returns>
        private static DataTable ArrangeTamperTable(DataTable source, string[] ColumnDisplayNames,Dictionary<string,string> columnValue)
        {
            DataTable newTable = new DataTable();
          
            try
            {
                DataColumn meterColumn = source.Columns["meterid"];

                var group = source.AsEnumerable().Select(r => r["meterData_Id"].ToString()).Distinct();

                // first column will be meter id
                newTable.Columns.Add(new DataColumn("Meter Serial Number", typeof(string)));


                // creating columns
                foreach (string columnName in ColumnDisplayNames)
                {
                    try
                    {
                        DataColumn newColumn = new DataColumn(columnName, typeof(bool));
                        newTable.Columns.Add(newColumn);
                    }
                    catch (Exception ex)    //Exception log for catch block
                    {
                        logger.Log(LOGLEVELS.Error, "ArrangeTamperTable(DataTable source, string[] ColumnDisplayNames,Dictionary<string,string> columnValue)", ex);
                    }
                }


                //now pivoting the table 
                foreach (string meterDataId in group)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(meterDataId))
                        {
                            DataRow newRow = newTable.NewRow();

                            var rowValues = (from row in source.AsEnumerable()
                                             where row["meterData_Id"].ToString() == meterDataId
                                             select row["EVENTCODE"].ToString()).ToList();


                            foreach (DataColumn column in newTable.Columns)
                            {
                                if(columnValue.ContainsKey(column.ColumnName))
                                {
                                    newRow[column.ColumnName] = rowValues.Contains(columnValue[column.ColumnName]);
                                }
                            }

                            
                            newRow["Meter Serial Number"] = source.AsEnumerable().Where(r => r["meterdata_id"].ToString().Equals(meterDataId)).Select(r => r["meterid"].ToString()).Cast<string>().ToArray()[0]; 

                            newTable.Rows.Add(newRow);
                        }
                    }
                    catch (Exception ex)    //Exception log for catch block
                    {
                        logger.Log(LOGLEVELS.Error, "ArrangeTamperTable(DataTable source, string[] ColumnDisplayNames,Dictionary<string,string> columnValue)", ex);
                    }

                }
            }

            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ArrangeTamperTable(DataTable source, string[] ColumnDisplayNames,Dictionary<string,string> columnValue)", ex);
            }

            return newTable;
        }
      

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private static DataTable ArrangeInstantTable(DataTable source,string [] ColumnDisplayNames)
        {
            DataTable newTable = new DataTable();
            try
            {
                DataColumn meterColumn = source.Columns["meterid"];

                var group = source.AsEnumerable().Select(r => r["meterData_Id"].ToString()).Distinct();

                // last column will be meter id
                newTable.Columns.Add(new DataColumn("Meter Serial Number", typeof(string)));

                // creating columns
                foreach (string columnName in ColumnDisplayNames)
                {
                    try
                    {
                        DataColumn newColumn = new DataColumn(columnName, typeof(string));
                        newTable.Columns.Add(newColumn);
                    }
                    catch (Exception ex)    //Exception log for catch block
                    {
                        logger.Log(LOGLEVELS.Error, "ArrangeInstantTable(DataTable source,string [] ColumnDisplayNames)", ex);
                    }
                }



                //now pivoting the table 
                foreach (string meterDataId in group)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(meterDataId))
                        {

                            DataRow newRow = newTable.NewRow();

                            var rowValues = (from row in source.AsEnumerable()
                                             where row["meterData_Id"].ToString() == meterDataId
                                             select new { columnName = row["DISPLAYNAME"].ToString().Trim(), columnValue = row["InstantPowerColumnValue"] }).ToDictionary(r => r.columnName, a => a.columnValue);


                            foreach (DataColumn column in newTable.Columns)
                            {
                                if (rowValues.ContainsKey(column.ColumnName))
                                {
                                    newRow[column.ColumnName] = ApplyTransformFunctions(column.ColumnName,rowValues[column.ColumnName].ToString().Trim());
                                }
                            }

                            newRow["Meter Serial Number"] = source.AsEnumerable().Where(r => r["meterdata_id"].ToString().Equals(meterDataId)).Select(r => r["meterid"].ToString()).Cast<string>().ToArray()[0]; 

                            newTable.Rows.Add(newRow);
                        }
                    }
                    catch (Exception ex)    //Exception log for catch block
                    {
                        logger.Log(LOGLEVELS.Error, "ArrangeInstantTable(DataTable source,string [] ColumnDisplayNames)", ex);
                    }
                
                }
            
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ArrangeInstantTable(DataTable source,string [] ColumnDisplayNames)", ex);
            }

            return newTable;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        private static DataTable ArrangeBillingTable(DataTable source)
        {
            DataTable newTable = new DataTable();

            try
            {
              
                // creating afresh columns as conversion will change datatype
                foreach (DataColumn column in source.Columns)
                {
                    try
                    { 
                        newTable.Columns.Add(column.ColumnName, typeof(string)); 
                    }
                    catch (Exception ex)    //Exception log for catch block
                    {
                        logger.Log(LOGLEVELS.Error, "ArrangeBillingTable(DataTable source)", ex);
                    }
                }


                foreach (DataRow dr in source.Rows)
                {
                    DataRow newRow = newTable.NewRow();

                    foreach (DataColumn column in source.Columns)
                    {
                        try
                        {
                            if (column.ColumnName.ToUpper().Contains("DATE"))
                            {
                                newRow[column.ColumnName] = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(dr[column.ColumnName]));
                            }
                            else if (dr[column.ColumnName].ToString().Contains("*"))
                            {
                                newRow[column.ColumnName] = SplitWithOutUnit(dr[column.ColumnName].ToString().Trim());
                            }
                            else if (column.ColumnName.ToString().ToUpper().Equals("HISTORY"))
                            {
                                if (dr[column.ColumnName].ToString().Trim() == "0")
                                {
                                    newRow[column.ColumnName] = "Current";
                                }
                                else
                                {
                                    newRow[column.ColumnName] = "History-" + dr[column.ColumnName].ToString().Trim();
                                }
                            }
                            else
                            {
                                newRow[column.ColumnName] = dr[column.ColumnName];
                            }

                            
                        }
                        catch (Exception ex)    //Exception log for catch block
                        {
                            logger.Log(LOGLEVELS.Error, "ArrangeBillingTable(DataTable source)", ex);
                        }
                    }

                    newTable.Rows.Add(newRow);
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ArrangeBillingTable(DataTable source)", ex);
            }


            newTable.Columns["Meter Serial Number"].SetOrdinal(0);
            newTable.Columns["History"].SetOrdinal(1);
            return newTable;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string ApplyTransformFunctions(string columnName,string value)
        {
            
                    try
                    {
                        if (columnName.ToUpper().Contains("DATE"))
                        {
                            return DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(value));
                        }
                        if (value.Contains("*"))
                        {
                            return SplitWithOutUnit(value);
                        }
                    }
                    catch (Exception ex)    //Exception log for catch block
                    {
                        logger.Log(LOGLEVELS.Error, "ApplyTransformFunctions(string columnName,string value)", ex);
                    }

                    return string.Empty;
        
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static string SplitWithOutUnit(string data)
        {
            string value = data;
            if (data.IndexOf('*') > 0)
            {
                string[] val = data.Split('*');
                value = val[0];
            }
            return value.Replace("000000.00", "0.000");
        }
    }

}
