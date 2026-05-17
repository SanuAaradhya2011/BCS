/* |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 									        |
 * |											Date   : 22/04/2010 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| 
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using CAB.DALC.Data.DataServices;
using CAB.Framework;
using CAB.Framework.Utility;
using Hunt.EPIC.Logging;

namespace CAB.DALC.Data
{
    public class ReportDAL : DALBase
    {

        private readonly string MeterId = "MeterId";
        private readonly string FileName = "FileName";
        private readonly string MeterData_ID = "MeterData_ID";
        private readonly string History_ID = "History_ID";
        private readonly string UploadingDateTime = "UploadingDateTime";
        private readonly string TamperSnapShot_ID = "TamperSnapShot_ID";
        private readonly string FromDate = "FromDate";
        private readonly string ToDate = "ToDate";
        private readonly string RelatedTo = "RelatedTo";
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(ReportDAL).ToString());

        /// <summary>
        /// Select all CAB file from the database.
        /// </summary>
        /// <returns>DataTable containing CAB files along with serial number.</returns>
        public DataTable SelectCABFile()
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append(string.Concat("select ", FileName, ",", UploadingDateTime, " from fileupload_master order by ", UploadingDateTime, " desc"));
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("CAB file retrieved"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "SelectCABFile()", ex);
            }
            return AutoNumberedTable(dataSet.Tables[0]);
        }

        /// <summary>
        /// To select MeterID for the MeterSelect Form where we need meter to be displayed for MeterWise Reports
        /// </summary>
        /// <returns></returns>
        public DataTable SelectMeterID()
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append(string.Concat("Select distinct ", MeterId, " from meterdata"));
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter ID retrieved"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "SelectMeterID()", ex);
            }
            return AutoNumberedTable(dataSet.Tables[0]);
        }


        /// <summary>
        /// Provides column names for the given table.
        /// </summary>
        /// <param name="ParameterName">Name of the table in database.</param>
        /// <returns>Column names for the given Table</returns>
        public DataTable SelectParameters(string tableName)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append(string.Concat("show columns from ", tableName));
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Columns from specified table retrieved"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "SelectParameters(string tableName)", ex);
            }
            return dataSet.Tables[0];
        }

        /*
         * For all the DataSet that is filled using the Stored procedure except Billing
         * XSD used : FileReportDataSet.
         * Table Used : DataReportsTable.
         * Reports used : MeterDetails.rpt
     
         * For Billing 
         * XSD Used : BillingReportsTable
         * Table Used : DataReports
         * Reports Used : BillingDetailsReport.rpt
         * Folder for Reports used : DataReports
         */

        /*This function returns Dataset of the Instant data.
        Stored Procedure :  GetInstantData */

        public DataSet GetInstantData(string FileName, string MeterNo, string Parameter1, string Parameter2, string Parameter3, string Parameter4, string Parameter5, string Parameter6, string Parameter7, string Parameter8, string Parameter9, string Parameter10)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append(string.Concat("select ", MeterId, Parameter1, Parameter2, Parameter3, Parameter4, Parameter5, Parameter6, Parameter7, Parameter8, Parameter9, Parameter10, " from meterdata_instantpower"));
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Instant data retrieved"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetInstantData(string FileName, string MeterNo, string Parameter1, string Parameter2, string Parameter3, string Parameter4, string Parameter5, string Parameter6, string Parameter7, string Parameter8, string Parameter9, string Parameter10)", ex);
            }
            return dataSet;
        }


        public DataSet GetGeneralData(string FileName, string MeterNo, string Parameter1, string Parameter2, string Parameter3, string Parameter4, string Parameter5, string Parameter6, string Parameter7, string Parameter8, string Parameter9, string Parameter10)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append(string.Concat("select ", MeterId, Parameter1, Parameter2, Parameter3, Parameter4, Parameter5, Parameter6, Parameter7, Parameter8, Parameter9, Parameter10, " from meterdata_general"));
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("General data retrieved"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetGeneralData(string FileName, string MeterNo, string Parameter1, string Parameter2, string Parameter3, string Parameter4, string Parameter5, string Parameter6, string Parameter7, string Parameter8, string Parameter9, string Parameter10)", ex);
            }
            return dataSet;
        }



        public DataSet GetLoadSurveyData(string FileName, string MeterNo, string Parameter1, string Parameter2, string Parameter3, string Parameter4, string Parameter5, string Parameter6, string Parameter7, string Parameter8, string Parameter9, string Parameter10)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append(string.Concat("select ", MeterId, Parameter1, Parameter2, Parameter3, Parameter4, Parameter5, Parameter6, Parameter7, Parameter8, Parameter9, Parameter10, " from meterdata_loadsurvey"));
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Load survey data retrieved"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetLoadSurveyData(string FileName, string MeterNo, string Parameter1, string Parameter2, string Parameter3, string Parameter4, string Parameter5, string Parameter6, string Parameter7, string Parameter8, string Parameter9, string Parameter10)", ex);
            }
            return dataSet;
        }


        public DataSet GetTamperData(long tamperSnapShotID, string Parameter1, string Parameter2, string Parameter3, string Parameter4, string Parameter5, string Parameter6, string Parameter7, string Parameter8, string Parameter9, string Parameter10)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append(string.Concat("select ", MeterId, Parameter1, Parameter2, Parameter3, Parameter4, Parameter5, Parameter6, Parameter7, Parameter8, Parameter9, Parameter10, " from meterdata_tampersnapshot"));
                builder.Append(string.Concat("where ", TamperSnapShot_ID, " = ", ParameterName(TamperSnapShot_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(TamperSnapShot_ID), tamperSnapShotID, DbType.Int64);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Tamper data retrieved"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetTamperData(long tamperSnapShotID, string Parameter1, string Parameter2, string Parameter3, string Parameter4, string Parameter5, string Parameter6, string Parameter7, string Parameter8, string Parameter9, string Parameter10)", ex);
            }
            return dataSet;
        }


        public DataSet Proc_Billing_Report_Select(string FileName, string MeterNo, string Parameter1, string Parameter2, string Parameter3, string Parameter4, string Parameter5, string Parameter6, string Parameter7, string Parameter8, string Parameter9, string Parameter10)
        {
            return null;
            //DataSet DS = new DataSet();
            //try
            //{
            //    OpenConn();
            //    MyComm = new SqlCommand("Proc_Billing_Report_Select", MyConn);
            //    MyComm.CommandType = CommandType.StoredProcedure;
            //    MyComm.Parameters.Add(new SqlParameter("@FileName", SqlDbType.VarChar, 50)).Value = FileName;
            //    MyComm.Parameters.Add(new SqlParameter("@MeterID", SqlDbType.VarChar, 50)).Value = MeterNo;
            //    MyComm.Parameters.Add(new SqlParameter("@Parameter1", SqlDbType.VarChar, 50)).Value = Parameter1;
            //    MyComm.Parameters.Add(new SqlParameter("@Parameter2", SqlDbType.VarChar, 50)).Value = Parameter2;
            //    MyComm.Parameters.Add(new SqlParameter("@Parameter3", SqlDbType.VarChar, 50)).Value = Parameter3;
            //    MyComm.Parameters.Add(new SqlParameter("@Parameter4", SqlDbType.VarChar, 50)).Value = Parameter4;
            //    MyComm.Parameters.Add(new SqlParameter("@Parameter5", SqlDbType.VarChar, 50)).Value = Parameter5;
            //    MyComm.Parameters.Add(new SqlParameter("@Parameter6", SqlDbType.VarChar, 50)).Value = Parameter6;
            //    MyComm.Parameters.Add(new SqlParameter("@Parameter7", SqlDbType.VarChar, 50)).Value = Parameter7;
            //    MyComm.Parameters.Add(new SqlParameter("@Parameter8", SqlDbType.VarChar, 50)).Value = Parameter8;
            //    MyComm.Parameters.Add(new SqlParameter("@Parameter9", SqlDbType.VarChar, 50)).Value = Parameter9;
            //    MyComm.Parameters.Add(new SqlParameter("@Parameter10", SqlDbType.VarChar, 50)).Value = Parameter10;
            //    SqlDataAdapter Adap = new SqlDataAdapter(MyComm);
            //    Adap.Fill(DS, "BillingReportsTable");

            //    CloseConn();
            //    return DS;

            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //    return null;
            //}
        }

        public DataTable GetFileNamesByMeterID(string meterID)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("select fm.filename from fileupload_master as fm inner join meterdata as md on fm.fileupload_id = md.fileupload_id");
                builder.Append(string.Concat("where md.", MeterId, " = ", ParameterName(MeterId)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterId), meterID, DbType.String, 20);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("File Names for the specified meter ID retrieved"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetFileNamesByMeterID(string meterID)", ex);
            }
            return AutoNumberedTable(dataSet.Tables[0]);
        }

        //public DataTable GetReadingDateTime()
        //{
        //    try
        //    {
        //        DataTable PTable = new DataTable();
        //        OpenConn();
        //        MyComm = new SqlCommand("Proc_ReadingDateTime_Report_Select", MyConn);
        //        MyComm.CommandType = CommandType.StoredProcedure;

        //        SqlDataAdapter Adap = new SqlDataAdapter(MyComm);
        //        Adap.Fill(PTable);
        //        CloseConn();
        //        return PTable;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        return null;
        //    }

        //}

        //public DataTable GetFileNameFromReadingDateTime(string DateValue)
        //{
        //    try
        //    {
        //        DataTable FileTable = new DataTable();
        //        OpenConn();
        //        MyComm = new SqlCommand("Proc_FileNameFromReadingDateTime_Report_Select", MyConn);
        //        MyComm.CommandType = CommandType.StoredProcedure;
        //        MyComm.Parameters.Add(new SqlParameter("@DateValue", SqlDbType.VarChar,50)).Value = Convert.ToDateTime(DateValue).ToString("yyyy-MM-dd hh:mm:ss tt");
        //        SqlDataAdapter Adap = new SqlDataAdapter(MyComm);

        //        Adap.Fill(FileTable);
        //        CloseConn();
        //        return FileTable;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        return null;
        //    }

        //}

        public DataTable GetFileNamesByDateRange(long fromDate, long toDate)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append(string.Concat("select ", FileName, " from fileupload_master"));
                builder.Append(string.Concat("where", UploadingDateTime, " between", ParameterName(FromDate), " and ", ParameterName(ToDate)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(FromDate), fromDate, DbType.Int64, 14);
                request.AddParamter(ParameterName(ToDate), toDate, DbType.Int64, 14);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("File Names for the specified Date retrieved"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetFileNamesByDateRange(long fromDate, long toDate)", ex);
            }
            return dataSet.Tables[0];
        }

        public DataTable GetMeterIDFromFileName(string fileName)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("select md.MeterID from meterdata as md inner join fileupload_master as fm on md.fileupload_id = fm.fileupload_id");
                builder.Append(string.Concat("where fm.", FileName, " = ", ParameterName(FileName)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(FileName), fileName, DbType.String, 40);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter ID retrieved"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetMeterIDFromFileName(string fileName)", ex);
            }
            return dataSet.Tables[0];
        }

        public DataSet GetGeneralReportData(long activeMeterData_ID)
        {
            //GeneralDAL generalDAL = new GeneralDAL();
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                DataRequest request = new DataRequest(builder.ToString());
                ApplicationType types = ConfigInfo.GetApplicationType();
                if (types.Equals(ApplicationType.DLMS_LTCT_650))
                {
                    builder.Append("select meterSerialNumber,metertype,manufacturername,firmwareVersionformeter,internalCTratio,meteryearofmanufacture ");
                    builder.Append("from meterdata_general ");
                    builder.Append(string.Concat("where ", MeterData_ID, " = ", ParameterName(MeterData_ID)));
                    request = new DataRequest(builder.ToString());
                    request.AddParamter(ParameterName(MeterData_ID), activeMeterData_ID, DbType.Int64);
                }
                else
                {
                    builder.Append("select g.MeterID, ");
                    builder.Append("g.MeterDateTime,g.ErrorCode,g.MeterConstant,g.FirmwareVersion,g.CTRatio,g.VoltagePhaseSequence, ");
                    builder.Append("i.TotalFundamentalActiveEnergy,g.CumulativeMD1 as CMD1,g.CumulativeMD2 as CMD2,g.CumulativeMD3 as CMD3,g.TotalPowerOnHours,g.BateryModePowerOnHour, ");
                    builder.Append("g.MDResetCounter,g.ReadoutCounter,g.ProgrammingCounter,g.CTRatioProgrammingCounter,g.LatestTamperOccurrenceID, ");
                    builder.Append("g.OccurrenceTime,g.LatestTamperRestorationID,g.RestorationTime, ");
                    builder.Append("b.CumulativeEnergyKWH,b.CumulativeEnergyKVAH,b.CumulativeEnergyKVARHLag,b.CumulativeEnergyKVARHLead, ");
                    builder.Append("b.CumulativeMD1,b.CumulativeMD1TimeStamp,b.CumulativeMD2,b.CumulativeMD2TimeStamp,b.CumulativeMD3,b.CumulativeMD3TimeStamp ");
                    builder.Append("from meterdata_general as g inner join meterdata_billing as b on g.MeterData_ID = b.MeterData_ID inner join meterdata_instantpower i on i.MeterData_ID = b.MeterData_ID ");
                    builder.Append(string.Concat("where g.", MeterData_ID, " = ", ParameterName(MeterData_ID), " and "));
                    //builder.Append(string.Concat("b.DataIndex < 13 and b.", History_ID, " = ", ParameterName(History_ID)));// Story - 365971 - 13 billing for Power ON Hours 
                    builder.Append(string.Concat("b.DataIndex < 61 and b.", History_ID, " = ", ParameterName(History_ID)));// Story - 581355 - 60 months billing for Nepal 1P VIM Tender requirement
                    request = new DataRequest(builder.ToString());
                    request.AddParamter(ParameterName(MeterData_ID), activeMeterData_ID, DbType.Int64);
                    request.AddParamter(ParameterName(History_ID), 0, DbType.Int32);
                }
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("General report data retrieved"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetGeneralReportData(long activeMeterData_ID)", ex);
            }
            return dataSet;
        }

        public DataSet GetInstantReportData(long activeMeterData_ID)
        {
            //GeneralDAL generalDAL = new GeneralDAL();
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                DataRequest request = new DataRequest(builder.ToString());
                ApplicationType types = ConfigInfo.GetApplicationType();
                if (types.Equals(ApplicationType.DLMS_LTCT_650))
                {
                    builder.Append("select InstantPower_ID,InstantPowerColumnName,InstantPowerColumnValue,InstantPowerObisCode, ");
                    builder.Append("InstantPowerClassID,InstantPowerAttribute,InstantPowerDataIndex,MeterData_ID ");
                    builder.Append("from meterdata_instantpower ");
                    builder.Append(string.Concat("where ", MeterData_ID, " = ", ParameterName(MeterData_ID)));
                    request = new DataRequest(builder.ToString());
                    request.AddParamter(ParameterName(MeterData_ID), activeMeterData_ID, DbType.Int64);
                }
                else
                {
                    builder.Append("select i.MeterID, ");
                    builder.Append("i.VoltageRPhase,i.VoltageYPhase,i.VoltageBPhase,i.CurrentRPhase,i.CurrentYPhase,i.CurrentBPhase, ");
                    builder.Append("i.InstantActivepower,i.InstantApparentPower,i.InstantReactiveLagPower,i.InstantReactiveLeadPower,i.TotalPowerFactor, ");
                    builder.Append("i.PowerFactorRPhase,i.PowerFactorYPhase,i.PowerFactorBPhase,b.AveragePowerFactor, ");
                    builder.Append("i.Frequency, ");
                    builder.Append("g.RisingDemandKW,g.ElapsedTimeKW,g.RisingDemandKVA,g.ElapsedTimeKVA ");
                    builder.Append("from meterdata_instantpower as i inner join meterdata_billing as b on i.MeterData_ID = b.MeterData_ID ");
                    builder.Append("inner join meterdata_general as g on i.MeterData_ID = g.MeterData_ID ");
                    builder.Append(string.Concat("where i.", MeterData_ID, " = ", ParameterName(MeterData_ID), " and "));
                    //builder.Append(string.Concat("b.DataIndex < 13 and b.", History_ID, " = ", ParameterName(History_ID)));// Story - 365971 - 13 billing for Power ON Hours 
                    builder.Append(string.Concat("b.DataIndex < 61 and b.", History_ID, " = ", ParameterName(History_ID)));// Story - 581355 - 60 months billing for Nepal 1P VIM Tender requirement
                    request = new DataRequest(builder.ToString());
                    request.AddParamter(ParameterName(MeterData_ID), activeMeterData_ID, DbType.Int64);
                    request.AddParamter(ParameterName(History_ID), 0, DbType.Int32);
                }
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Instant report data retrieved"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetInstantReportData(long activeMeterData_ID)", ex);
            }
            return dataSet;
        }

        public DataSet GetTamperReportData(long activeMeterData_ID)
        {
            //GeneralDAL generalDAL = new GeneralDAL();
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                //in XSD:- voltage_RPhase,voltage_YPhase,voltage_BPhase,Current_RPhase,Current_YPhase,Current_BPhase,activePower,apparentPower,reactivePowerLag,reaactivePowerLead,totalPowerFactor,PowerFactor_Rphase,PowerFactor_Yphase,PowerFactor_Bphase,AveragePowerFactor,Frequency,RisingDemand_KW,ElapsedTime_KW,RisingDemand_KVA,ElapsedTime_KVA
                //Not used:- InstantPower_ID, MeterID, MeterDateTime, TotalFundamentalActiveEnergy, MeterData_ID
                builder.Append("select tt.TamperType,count(ts.TamperCode) as TamperCounter, ");
                builder.Append("ts.TamperOccurredTime,ts.TamperRestoredTime,ts.RVoltageRestored,ts.YVoltageRestored,ts.BVoltageRestored, ");
                builder.Append("ts.RCurrentRestored,ts.YCurrentRestored,ts.BCurrentRestored,ts.RPFRestored,ts.YPFRestored, ");
                builder.Append("ts.BPFRestored,ts.TotalPFRestored,ts.kWhRestored,ts.kVAhRestored,ts.RVoltageOccurred, ");
                builder.Append("ts.YVoltageOccurred,ts.BVoltageOccurred,ts.RCurrentOccurred,ts.YCurrentOccurred,ts.BCurrentOccurred, ");
                builder.Append("ts.RPFOccurred,ts.YPFOccurred,ts.BPFOccurred,ts.TotalPFOccurred,ts.kWhOccurred,ts.kVAhOccurred ");
                builder.Append("from meterdata_tampersnapshot as ts inner join tampertype_master tt on ts.TamperCode = tt.TamperTypeID ");
                builder.Append(string.Concat("where ts.", MeterData_ID, " = ", ParameterName(MeterData_ID)));
                builder.Append(" group by TamperCode");
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), activeMeterData_ID, DbType.Int64);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Tamper report data retrieved"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetTamperReportData(long activeMeterData_ID)", ex);
            }
            return dataSet;
        }


        public DataSet GetPowerOnHoursReportData(long activeMeterData_ID)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                //in XSD:- voltage_RPhase,voltage_YPhase,voltage_BPhase,Current_RPhase,Current_YPhase,Current_BPhase,activePower,apparentPower,reactivePowerLag,reaactivePowerLead,totalPowerFactor,PowerFactor_Rphase,PowerFactor_Yphase,PowerFactor_Bphase,AveragePowerFactor,Frequency,RisingDemand_KW,ElapsedTime_KW,RisingDemand_KVA,ElapsedTime_KVA
                //Not used:- InstantPower_ID, MeterID, MeterDateTime, TotalFundamentalActiveEnergy, MeterData_ID
                builder.Append("select PowerOnHours, History_ID ");
                builder.Append("from meterdata_billing  ");
                //builder.Append(string.Concat("where DataIndex < 13 and ", MeterData_ID, " = ", ParameterName(MeterData_ID))); // Story - 365971 - 13 billing for Power ON Hours 
                builder.Append(string.Concat("where DataIndex < 61 and ", MeterData_ID, " = ", ParameterName(MeterData_ID))); // Story - 581355 - 60 months billing for Nepal 1P VIM Tender requirement
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), activeMeterData_ID, DbType.Int64);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Power on Hours report data retrieved"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetPowerOnHoursReportData(long activeMeterData_ID)", ex);
            }
            return dataSet;
        }

        public DataSet GetPowerFactorReportData(long activeMeterData_ID)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                //in XSD:- voltage_RPhase,voltage_YPhase,voltage_BPhase,Current_RPhase,Current_YPhase,Current_BPhase,activePower,apparentPower,reactivePowerLag,reaactivePowerLead,totalPowerFactor,PowerFactor_Rphase,PowerFactor_Yphase,PowerFactor_Bphase,AveragePowerFactor,Frequency,RisingDemand_KW,ElapsedTime_KW,RisingDemand_KVA,ElapsedTime_KVA
                //Not used:- InstantPower_ID, MeterID, MeterDateTime, TotalFundamentalActiveEnergy, MeterData_ID
                builder.Append("select AveragePowerFactor, History_ID ");
                builder.Append("from meterdata_billing  ");
                //builder.Append(string.Concat("where DataIndex < 13 and ", MeterData_ID, " = ", ParameterName(MeterData_ID))); // Story - 365971 - 13 billing for Power ON Hours
                builder.Append(string.Concat("where DataIndex < 61 and ", MeterData_ID, " = ", ParameterName(MeterData_ID))); // Story - 581355 - 60 months billing for Nepal 1P VIM Tender requirement
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), activeMeterData_ID, DbType.Int64);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("PF report data retrieved"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetPowerFactorReportData(long activeMeterData_ID)", ex);
            }
            return dataSet;
        }

        public DataSet GetCTRatioReportData(long activeMeterData_ID)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                //in XSD:- voltage_RPhase,voltage_YPhase,voltage_BPhase,Current_RPhase,Current_YPhase,Current_BPhase,activePower,apparentPower,reactivePowerLag,reaactivePowerLead,totalPowerFactor,PowerFactor_Rphase,PowerFactor_Yphase,PowerFactor_Bphase,AveragePowerFactor,Frequency,RisingDemand_KW,ElapsedTime_KW,RisingDemand_KVA,ElapsedTime_KVA
                //Not used:- InstantPower_ID, MeterID, MeterDateTime, TotalFundamentalActiveEnergy, MeterData_ID
                builder.Append("select CTRatio, History_ID ");
                builder.Append("from meterdata_billing  ");
                //builder.Append(string.Concat("where DataIndex < 13 and ", MeterData_ID, " = ", ParameterName(MeterData_ID))); // Story - 365971 - 13 billing for Power ON Hours
                builder.Append(string.Concat("where DataIndex < 61 and ", MeterData_ID, " = ", ParameterName(MeterData_ID))); // Story - 581355 - 60 months billing for Nepal 1P VIM Tender requirement
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), activeMeterData_ID, DbType.Int64);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("CT Ratio report data retrieved"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetCTRatioReportData(long activeMeterData_ID)", ex);
            }
            return dataSet;
        }

        //added for MVVNL
        public DataSet GetMidnightEnergiesReportData(long activeMeterData_ID)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("select realTimeClockDateandTime, cumEnergykWh,cumEnergykVAh, cumEnergykvarhlag, cumEnergykvarhlead ");
                builder.Append("from meterdata_midnightdata  ");
                builder.Append(string.Concat("where ", MeterData_ID, " = ", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), activeMeterData_ID, DbType.Int64);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Midnight Energies report data retrieved"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetMidnightEnergiesReportData(long activeMeterData_ID)", ex);
                throw;
            }
            return dataSet;
        }
        //added for MVVNL

        public DataSet GetLoadFactorReportData(long activeMeterData_ID)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();   
                StringBuilder builder = new StringBuilder();
                //in XSD:- voltage_RPhase,voltage_YPhase,voltage_BPhase,Current_RPhase,Current_YPhase,Current_BPhase,activePower,apparentPower,reactivePowerLag,reaactivePowerLead,totalPowerFactor,PowerFactor_Rphase,PowerFactor_Yphase,PowerFactor_Bphase,AveragePowerFactor,Frequency,RisingDemand_KW,ElapsedTime_KW,RisingDemand_KVA,ElapsedTime_KVA
                //Not used:- InstantPower_ID, MeterID, MeterDateTime, TotalFundamentalActiveEnergy, MeterData_ID
                builder.Append("select LoadFactor, History_ID ");
                builder.Append("from meterdata_billing  ");
                //builder.Append(string.Concat("where DataIndex < 13 and ", MeterData_ID, " = ", ParameterName(MeterData_ID))); // Story - 365971 - 13 billing for Power ON Hours
                builder.Append(string.Concat("where DataIndex < 61 and ", MeterData_ID, " = ", ParameterName(MeterData_ID))); // Story - 581355 - 60 months billing for Nepal 1P VIM Tender requirement
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), activeMeterData_ID, DbType.Int64);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Load Survey report data retrieved"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetLoadFactorReportData(long activeMeterData_ID)", ex);
            }
            return dataSet;
        }

        public DataSet GetBillingTamperCounterReportData(long activeMeterData_ID)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                //in XSD:- voltage_RPhase,voltage_YPhase,voltage_BPhase,Current_RPhase,Current_YPhase,Current_BPhase,activePower,apparentPower,reactivePowerLag,reaactivePowerLead,totalPowerFactor,PowerFactor_Rphase,PowerFactor_Yphase,PowerFactor_Bphase,AveragePowerFactor,Frequency,RisingDemand_KW,ElapsedTime_KW,RisingDemand_KVA,ElapsedTime_KVA
                //Not used:- InstantPower_ID, MeterID, MeterDateTime, TotalFundamentalActiveEnergy, MeterData_ID
                builder.Append("select * ");
                builder.Append("from meterdata_tampercountergeneral ");
                builder.Append(string.Concat("where ", MeterData_ID, " = ", ParameterName(MeterData_ID)));
                builder.Append(" and RelatedTo != 'T'");
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), activeMeterData_ID, DbType.Int64);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Billing Tamper Counters report data retrieved"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetBillingTamperCounterReportData(long activeMeterData_ID)", ex);
            }
            return dataSet;
        }

        public DataSet GetMainEnergyReportData(long activeMeterData_ID)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("select b.History_ID, t.BillingTimestamp, b.CumulativeEnergyKWh, b.CumulativeEnergyKVAh, ");
                builder.Append("b.CumulativeEnergyKVARhLag, b.CumulativeEnergyKVARhLead, ");
                builder.Append("b.CumulativeMD1, b.CumulativeMD1TimeStamp,  b.CumulativeMD2, b.CumulativeMD2TimeStamp , b.CumulativeMD3, b.CumulativeMD3TimeStamp "); 
                builder.Append("from meterdata_billing as b inner join meterdata_tampercountergeneral as t on b.MeterData_ID = t.MeterData_ID and b.History_ID = t.History_ID ");
                //builder.Append(string.Concat("where b.DataIndex < 13 and b.", MeterData_ID, " = ", ParameterName(MeterData_ID), " and "));// Story - 365971 - 13 billing for Power ON Hours
                builder.Append(string.Concat("where b.DataIndex < 61 and b.", MeterData_ID, " = ", ParameterName(MeterData_ID), " and "));// Story - 581355 - 60 months billing for Nepal 1P VIM Tender requirement
                builder.Append(string.Concat("t.RelatedTo in ('B','G')"));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), activeMeterData_ID, DbType.Int64);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Main Energy report data retrieved"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetMainEnergyReportData(long activeMeterData_ID)", ex);
            }
            return dataSet;
        }


        /// <summary>
        /// Datalayer method to invoke GetSchedulingReportColumns Stored procedure to retrive list of column for the passed profile and utility
        /// </summary>
        /// <param name="profile">Profile for which the Columns to be retrieved</param>
        /// <param name="utility">Utility for which the Columns to be retrieved</param>
        /// <returns></returns>
        public static DataSet GetSchedulingReportColumns(string profile,string utility)
        {
            DataSet ds = new DataSet();
            try
            {
               
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("GetReportColumns");
                DataRequest request = new DataRequest(builder.ToString(), CommandType.StoredProcedure);
                request.AddParamter(ParameterName("profile"), profile, DbType.String);
                request.AddParamter(ParameterName("utility"), utility, DbType.String);
                helper.FillDataSet(request, ds);

            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetSchedulingReportColumns(string profile,string utility)", ex);
            }
            return ds;
        }

        /// <summary>
        /// Method to udpate databse for the selected and available parameters
        /// </summary>
        /// <param name="availableListXml"></param>
        /// <param name="selectedListXml"></param>
        /// <param name="Profile"></param>
        public static void UpdateParametersSelection(string availableListXml, string selectedListXml, string Profile)
        {
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                DataRequest request = new DataRequest("UpdateParametersSelection", CommandType.StoredProcedure);
                request.AddParamter(ParameterName("AvailableItemsXml"), availableListXml, DbType.Xml);
                request.AddParamter(ParameterName("SelectedItemsXml"), selectedListXml, DbType.Xml);
                request.AddParamter(ParameterName("ProfileSelected"), Profile, DbType.String);
                helper.ExecuteNonQuery(request);

            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "UpdateParametersSelection(string availableListXml, string selectedListXml, string Profile)", ex);
            } 
         
        }


        /// <summary>
        /// Execute the passed script and return the result set
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        public static DataTable GetGPRSReportData(string queryString)
        {
            DataSet dataSet = new DataSet();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                DataRequest request = new DataRequest(queryString);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetGPRSReportData(string queryString)", ex);
            }
        
            return dataSet.Tables[0];
        }

        public override bool UpdateData(CAB.Framework.Entity.IEntity entity)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteData(CAB.Framework.Entity.IEntity entity)
        {
            throw new NotImplementedException();
        }

        public override CAB.Framework.Entity.IEntity GetDetailData(int id)
        {
            throw new NotImplementedException();
        }

        public override IList<CAB.Framework.Entity.IEntity> ListData()
        {
            throw new NotImplementedException();
        }

        public override DataSet ListDataSet()
        {
            throw new NotImplementedException();
        }

        public override CAB.Framework.Entity.IEntity RowToEntity(DataRow row)
        {
            throw new NotImplementedException();
        }

        public override CAB.Framework.Entity.IEntity InsertData(CAB.Framework.Entity.IEntity entity)
        {
            throw new NotImplementedException();
        }

        public override CAB.Framework.Entity.IEntity InsertData(IList<CAB.Framework.Entity.IEntity> entities)
        {
            throw new NotImplementedException();
        }
    }
}