#region Namespaces
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using CAB.DALC.Data.DataServices;
using CAB.Framework;
using CAB.Framework.Entity;
using Hunt.EPIC.Logging;

#endregion

namespace CAB.DALC.Data
{
    public class TextExportDAL : DALBase
    {
        #region Constants & variables
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(TextExportDAL).ToString());
        #endregion

        #region Public Methods

        public override IEntity InsertData(IEntity entitiy)
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
            DataSet dataSet = null;
            return dataSet;
 
        }
        public DataSet ListDataSetForTextExport(long meterDataID)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                DataRequest request = new DataRequest("GetDataForTextExport", CommandType.StoredProcedure);
                request.AddParamter("MeterDataID", meterDataID, DbType.Int64);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Data Selected for Excel Export."));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ListDataSetForTextExport(long meterDataID)", ex);
                dataSet = null;
            }
            return dataSet;
        }

        public DataSet GetMeterDataIDByFileName(string fileName)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("SELECT m.MeterData_ID from meterdata m, fileupload_master f where m.FileUpload_ID = f.FileUpload_ID and ");
                builder.Append(string.Concat("f.FileName = '", fileName + "'")); 
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName("FileName"), fileName, DbType.String);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetMeterDataIDByFileName(string fileName)", ex);
            }
            return dataSet;
        }

        /// <summary>
        /// Get WB Data For Text Export of  WBSEDCL 
        /// </summary>
        /// <param name="meterDataID"></param>
        /// <returns></returns>
        public DataSet GetWBDataForTextExport(long meterID)
        {
            DataSet dataSet = null;
            string meterid = string.Empty;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("SELECT cast(LPAD(general.meterSerialNumber,8,'0')as char) as meterSerialNumber ,cast(LPAD(mdata.ReadingDateTime,14,'0')as char) as ReadingDateTime");
                builder.Append(",cast(LPAD(floor(billing.CumulativeEnergykWhTZ0 *1000 ),12,'0') as char) as CumulativeEnergykWhTZ0,cast(LPAD(floor(billing.CumulativeEnergykWhTZ1 * 1000),12,'0')as char)as CumulativeEnergykWhTZ1");
                builder.Append(",cast(LPAD(floor(billing.CumulativeEnergykWhTZ2 *1000 ),12,'0') as char)as CumulativeEnergykWhTZ2,cast(LPAD(floor(billing.CumulativeEnergykWhTZ3 * 1000),12,'0')as char)as CumulativeEnergykWhTZ3");
                builder.Append(",cast(LPAD(floor(billing.MDKVATZ0*1000),9,'0')as char)as MDKVATZ0  FROM meterdata_billing billing, meterdata_general general");
                builder.Append(", meterdata mdata where general.meterData_ID = billing.meterData_ID and general.meterData_ID = mdata.meterData_ID and billing.DataIndex =1 ");
                builder.Append("and billing.meterData_ID IN (select meterdata_id from meterdata where ");
                //builder.Append(string.Concat("MeterId", "=", ParameterName("MeterId"),")"));
                builder.Append(string.Concat("FileUpload_ID", "=", ParameterName("FileUpload_ID"), ")"));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName("FileUpload_ID"), meterID, DbType.String);
                //data = helper.ExecuteScalar(request);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
              
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetWBDataForTextExport(long meterID)", ex);
            }
            return dataSet;
        }

        /// <summary>
        /// Get BSES Data For Text Export of  BRPL 
        /// </summary>
        /// <param name="meterDataID"></param>
        /// <returns></returns>
        public DataSet GetBSESDataForTextExport(long meterID)
        {
            DataSet dataSet = null;
            string meterid = string.Empty;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("SELECT cast(LPAD(general.meterSerialNumber,10,' ')as char) as meterSerialNumber ,cast(LPAD(billing.BillingDate,12,'0')as char) BillingDate");
                builder.Append(",cast(LPAD(floor(billing.CumulativeEnergykWhTZ0 *100 ),10,'0') as char) as CumulativeEnergykWhTZ0,cast(LPAD(floor(billing.CumulativeEnergykVAhTZ0 * 100),10,'0')as char)as CumulativeEnergykVAhTZ0");
                builder.Append(",cast(LPAD(floor(billing.MDkWTZ0 *100 ),10,'0') as char)as MDKWTZ0,cast(LPAD(floor(billing.MDkVATZ0 * 100),10,'0')as char)as MDKVATZ0");
                builder.Append(",cast(LPAD(floor(billing.BillingWisePowerOffDuration),10,'0')as char)as PowerOffDuration");
                builder.Append(",cast(LPAD(floor(billing.CumulativeEnergykVAhTZ1 * 100),10,'0')as char)as CumulativeEnergykWhTZ1,cast(LPAD(floor(billing.CumulativeEnergykVAhTZ2 * 100),10,'0')as char)as CumulativeEnergykWhTZ2");
                builder.Append(",cast(LPAD(floor(billing.CumulativeEnergykVAhTZ3 * 100),10,'0')as char)as CumulativeEnergykWhTZ3  FROM meterdata_billing billing, meterdata_general general");

                builder.Append(", meterdata mdata where general.meterData_ID = billing.meterData_ID and general.meterData_ID = mdata.meterData_ID and billing.DataIndex =0 ");





                builder.Append("and billing.meterData_ID IN (select meterdata_id from meterdata where ");
                //builder.Append(string.Concat("MeterId", "=", ParameterName("MeterId"),")"));
                builder.Append(string.Concat("FileUpload_ID", "=", ParameterName("FileUpload_ID"), ")"));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName("FileUpload_ID"), meterID, DbType.String);
                //data = helper.ExecuteScalar(request);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);

            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetBSESDataForTextExport(long meterID)", ex);
            }
            return dataSet;
        }

        public DataSet GetBSESDataForTextExport1(long meterID)
        {
            DataSet dataSet1 = null;
            string meterid = string.Empty;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("SELECT cast(LPAD(general.meterSerialNumber,10,' ')as char) as meterSerialNumber ,cast(LPAD(billing.BillingDate,12,'0')as char) BillingDate");
                builder.Append(",cast(LPAD(floor(billing.CumulativeEnergykWhTZ0 *100 ),10,'0') as char) as CumulativeEnergykWhTZ0,cast(LPAD(floor(billing.CumulativeEnergykVAhTZ0 * 100),10,'0')as char)as CumulativeEnergykVAhTZ0");
                builder.Append(",cast(LPAD(floor(billing.MDkWTZ0 *100 ),10,'0') as char)as MDKWTZ0,cast(LPAD(floor(billing.MDkVATZ0 * 100),10,'0')as char)as MDKVATZ0");
                builder.Append(",cast(LPAD(floor(billing.BillingWisePowerOffDuration),10,'0')as char)as PowerOffDuration");
                builder.Append(",cast(LPAD(floor(billing.CumulativeEnergykVAhTZ1 * 100),10,'0')as char)as CumulativeEnergykWhTZ1,cast(LPAD(floor(billing.CumulativeEnergykVAhTZ2 * 100),10,'0')as char)as CumulativeEnergykWhTZ2");
                builder.Append(",cast(LPAD(floor(billing.CumulativeEnergykVAhTZ3 * 100),10,'0')as char)as CumulativeEnergykWhTZ3  FROM meterdata_billing billing, meterdata_general general");

                builder.Append(", meterdata mdata where general.meterData_ID = billing.meterData_ID and general.meterData_ID = mdata.meterData_ID and billing.DataIndex =1 ");



                builder.Append("and billing.meterData_ID IN (select meterdata_id from meterdata where ");
                //builder.Append(string.Concat("MeterId", "=", ParameterName("MeterId"),")"));
                builder.Append(string.Concat("FileUpload_ID", "=", ParameterName("FileUpload_ID"), ")"));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName("FileUpload_ID"), meterID, DbType.String);
                //data = helper.ExecuteScalar(request);
                dataSet1 = new DataSet();
                dataSet1 = helper.FillDataSet(request, dataSet1);

            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetBSESDataForTextExport(long meterID)", ex);
            }
            return dataSet1;
        }


        public DataSet GetBSESDataForTextInstant(long meterID)
        {
            DataSet dataSet1 = null;
            string meterid = string.Empty;
            try
            {

                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                //builder.Append("Select InstantPowerColumnValue  from meterdata_instantpower where MeterData_ID = meterID and InstantPowerColumnName = Cumulative Power-Failure Duration");

                
               // builder.Append("Select InstantPowerColumnValue  from meterdata_instantpower where MeterData_ID = '");
                builder.Append("SELECT cast(LPAD(InstantPowerColumnValue,10,' ')as char) as PowerOffDuration from meterdata_instantpower where MeterData_ID = '");
                builder.Append(meterID.ToString());
                builder.Append("' and InstantPowerColumnName = 'Cumulative Power-Failure Duration'");
                //builder.Append("Select InstantPowerColumnValue  from meterdata_instantpower where ");
                //builder.Append(string.Concat("MeterData_ID", " = ", ParameterName("meterID"), " and InstantPowerColumnName = Cumulative Power-Failure Duration"));

                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName("meterID"), meterID, DbType.String);
                dataSet1 = new DataSet();
                dataSet1 = helper.FillDataSet(request, dataSet1);  
            }
               

               
            
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetBSESDataForTextExport(long meterID)", ex);
            }
            return dataSet1;
        }


        /// <summary>
        /// Get Reliance Mumbai Data For Text Export
        /// </summary>
        /// <param name="meterDataID"></param>
        /// <returns></returns>
        public DataSet GetRelianceMumbaiDataForTextExport(string meterID, string meterVariant)
        {
            DataSet dataSet = null;
            string meterid = string.Empty;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();

                if (meterVariant == MeterVariant.FOUR)
                {
                    builder.Append("SELECT cast(LPAD(case when consumer.Consumer_Number is null then 0 else consumer.Consumer_Number end,9,'0') as char) as Consumer_Number,");

                    builder.Append(" cast(LPAD(case when meterSerialNumber is null then 0 else meterSerialNumber end,7,'0') as char) as meterSerialNumber,");

                    builder.Append(" cast(LPAD(floor(case when CumulativeEnergykWhTZ0Import is null then 0 else CumulativeEnergykWhTZ0Import end *100 ),9,'0') as char) as CumulativeEnergykWhTZ0Import,");

                    builder.Append(" cast(LPAD(floor(case when CumulativeEnergykWhTZ1Import is null then 0 else CumulativeEnergykWhTZ1Import end *100 ),9,'0') as char) as CumulativeEnergykWhTZ1Import,");
                    builder.Append(" cast(LPAD(floor(case when CumulativeEnergykWhTZ2Import is null then 0 else CumulativeEnergykWhTZ2Import end *100 ),9,'0') as char) as CumulativeEnergykWhTZ2Import,");
                    builder.Append(" cast(LPAD(floor(case when CumulativeEnergykWhTZ3Import is null then 0 else CumulativeEnergykWhTZ3Import end *100 ),9,'0') as char) as CumulativeEnergykWhTZ3Import,");
                    builder.Append(" cast(LPAD(floor(case when CumulativeEnergykWhTZ4Import is null then 0 else CumulativeEnergykWhTZ4Import end *100 ),9,'0') as char) as CumulativeEnergykWhTZ4Import,");
                    builder.Append(" cast(LPAD(floor(case when CumulativeEnergykWhTZ5Import is null then 0 else CumulativeEnergykWhTZ5Import end *100 ),9,'0') as char) as CumulativeEnergykWhTZ5Import,");
                    builder.Append(" cast(LPAD(floor(case when CumulativeEnergykWhTZ6Import is null then 0 else CumulativeEnergykWhTZ6Import end *100 ),9,'0') as char) as CumulativeEnergykWhTZ6Import,");

                    builder.Append(" cast(LPAD(floor(case when MDkVATZ0Import is null then 0 else MDkVATZ0Import end *1000 ),8,'0') as char) as MDkVATZ0Import,");

                    builder.Append(" cast(LPAD(floor(case when MDkVATZ1Import is null then 0   else  MDkVATZ1Import end *1000 ),8,'0') as char) as MDkVATZ1Import,");
                    builder.Append(" cast(LPAD(floor(case when MDkVATZ2Import is null then 0   else  MDkVATZ2Import end *1000 ),8,'0') as char) as MDkVATZ2Import,");
                    builder.Append(" case when MDkVATZ3Import is null then null else cast(LPAD(floor(MDkVATZ3Import  *1000 ),8,'0') as char)end as MDkVATZ3Import, ");
                    builder.Append(" cast(LPAD(floor(case when MDkVATZ4Import is null then 0   else  MDkVATZ4Import end *1000 ),8,'0') as char) as MDkVATZ4Import,");
                    builder.Append(" cast(LPAD(floor(case when MDkVATZ5Import is null then 0   else  MDkVATZ5Import end *1000 ),8,'0') as char) as MDkVATZ5Import,");
                    builder.Append(" cast(LPAD(floor(case when MDkVATZ6Import is null then 0   else  MDkVATZ6Import end *1000 ),8,'0') as char) as MDkVATZ6Import,");

                    builder.Append(" cast(LPAD(floor(case when CumulativeEnergykvarhLagQ1 is null then 0 else CumulativeEnergykvarhLagQ1 end  *100 ),9,'0') as char) as CumulativeEnergykvarhLagQ1,");

                    builder.Append(" cast(LPAD(floor(case when CumulativeEnergykVAhTZ1Import is null then 0 else  CumulativeEnergykVAhTZ1Import end  *100 ),9,'0') as char) as CumulativeEnergykVAhTZ1Import,");
                    builder.Append(" cast(LPAD(floor(case when CumulativeEnergykVAhTZ2Import is null then 0 else  CumulativeEnergykVAhTZ2Import end  *100 ),9,'0') as char) as CumulativeEnergykVAhTZ2Import,");
                    builder.Append(" cast(LPAD(floor(case when CumulativeEnergykVAhTZ3Import is null then 0 else  CumulativeEnergykVAhTZ3Import end  *100 ),9,'0') as char) as CumulativeEnergykVAhTZ3Import,");
                    builder.Append(" cast(LPAD(floor(case when CumulativeEnergykVAhTZ4Import is null then 0 else  CumulativeEnergykVAhTZ4Import end  *100 ),9,'0') as char) as CumulativeEnergykVAhTZ4Import,");
                    builder.Append(" cast(LPAD(floor(case when CumulativeEnergykVAhTZ5Import is null then 0 else  CumulativeEnergykVAhTZ5Import end  *100 ),9,'0') as char) as CumulativeEnergykVAhTZ5Import,");
                    builder.Append(" cast(LPAD(floor(case when CumulativeEnergykVAhTZ6Import is null then 0 else  CumulativeEnergykVAhTZ6Import end  *100 ),9,'0') as char) as CumulativeEnergykVAhTZ6Import,");


                    builder.Append(" cast(LPAD(floor(case when MDkWTZ0Import is null then 0 else MDkWTZ0Import end *1000 ),8,'0') as char) as MDkWTZ0Import,");


                    builder.Append(" case when BillingDate is null then 0 else BillingDate end as BillingDate,");

                    builder.Append(" cast(LPAD(floor(case when CumulativeEnergykvarhLead is null then 0 else CumulativeEnergykvarhLead end *100 ),9,'0') as char) as CumulativeEnergykvarhLead");

                    builder.Append(" FROM `dlms_ltct_650`.meterdata_billing billing join `dlms_ltct_650`.meterdata_general general on general.meterData_ID = billing.meterData_ID");
                    builder.Append(" join `dlms_ltct_650`.meterdata mdata on general.meterData_ID = mdata.meterData_ID");
                    builder.Append(" left join `dlms_ltct_650`.`consumermeter` consumer on consumer. Meter_ID = mdata.MeterID");
                    builder.Append(" where billing.DataIndex =1 and ");
                    builder.Append(string.Concat("billing.meterData_ID", " = ", ParameterName("meterData_ID"), ""));


                }
                else
                {

                    builder.Append("SELECT cast(LPAD(case when consumer.Consumer_Number is null then 0 else consumer.Consumer_Number end,9,'0') as char) as Consumer_Number,");

                    builder.Append(" cast(LPAD(case when meterSerialNumber is null then 0 else meterSerialNumber end,7,'0') as char) as meterSerialNumber,");

                    builder.Append(" cast(LPAD(floor(case when CumulativeEnergykWhTZ0 is null then 0 else CumulativeEnergykWhTZ0 end *100 ),9,'0') as char) as CumulativeEnergykWhTZ0Import,");

                    builder.Append(" cast(LPAD(floor(case when CumulativeEnergykWhTZ1 is null then 0 else CumulativeEnergykWhTZ1 end *100 ),9,'0') as char) as CumulativeEnergykWhTZ1Import,");
                    builder.Append(" cast(LPAD(floor(case when CumulativeEnergykWhTZ2 is null then 0 else CumulativeEnergykWhTZ2 end *100 ),9,'0') as char) as CumulativeEnergykWhTZ2Import,");
                    builder.Append(" cast(LPAD(floor(case when CumulativeEnergykWhTZ3 is null then 0 else CumulativeEnergykWhTZ3 end *100 ),9,'0') as char) as CumulativeEnergykWhTZ3Import,");
                    builder.Append(" cast(LPAD(floor(case when CumulativeEnergykWhTZ4 is null then 0 else CumulativeEnergykWhTZ4 end *100 ),9,'0') as char) as CumulativeEnergykWhTZ4Import,");
                    builder.Append(" cast(LPAD(floor(case when CumulativeEnergykWhTZ5 is null then 0 else CumulativeEnergykWhTZ5 end *100 ),9,'0') as char) as CumulativeEnergykWhTZ5Import,");
                    builder.Append(" cast(LPAD(floor(case when CumulativeEnergykWhTZ6 is null then 0 else CumulativeEnergykWhTZ6 end *100 ),9,'0') as char) as CumulativeEnergykWhTZ6Import,");

                    builder.Append(" cast(LPAD(floor(case when MDkVATZ0 is null then 0 else MDkVATZ0 end *1000 ),8,'0') as char) as MDkVATZ0Import,");

                    builder.Append(" cast(LPAD(floor(case when MDkVATZ1 is null then 0   else  MDkVATZ1 end *1000 ),8,'0') as char) as MDkVATZ1Import,");
                    builder.Append(" cast(LPAD(floor(case when MDkVATZ2 is null then 0   else  MDkVATZ2 end *1000 ),8,'0') as char) as MDkVATZ2Import,");
                    builder.Append(" case when MDkVATZ3 is null then null else cast(LPAD(floor(MDkVATZ3  *1000 ),8,'0') as char)end as MDkVATZ3Import, ");
                    builder.Append(" cast(LPAD(floor(case when MDkVATZ4 is null then 0   else  MDkVATZ4 end *1000 ),8,'0') as char) as MDkVATZ4Import,");
                    builder.Append(" cast(LPAD(floor(case when MDkVATZ5 is null then 0   else  MDkVATZ5 end *1000 ),8,'0') as char) as MDkVATZ5Import,");
                    builder.Append(" cast(LPAD(floor(case when MDkVATZ6 is null then 0   else  MDkVATZ6 end *1000 ),8,'0') as char) as MDkVATZ6Import,");

                    builder.Append(" cast(LPAD(floor(case when CumulativeEnergykvarhLag is null then 0 else CumulativeEnergykvarhLag end  *100 ),9,'0') as char) as CumulativeEnergykvarhLagQ1,");

                    builder.Append(" cast(LPAD(floor(case when CumulativeEnergykVAhTZ1 is null then 0 else  CumulativeEnergykVAhTZ1 end  *100 ),9,'0') as char) as CumulativeEnergykVAhTZ1Import,");
                    builder.Append(" cast(LPAD(floor(case when CumulativeEnergykVAhTZ2 is null then 0 else  CumulativeEnergykVAhTZ2 end  *100 ),9,'0') as char) as CumulativeEnergykVAhTZ2Import,");
                    builder.Append(" cast(LPAD(floor(case when CumulativeEnergykVAhTZ3 is null then 0 else  CumulativeEnergykVAhTZ3 end  *100 ),9,'0') as char) as CumulativeEnergykVAhTZ3Import,");
                    builder.Append(" cast(LPAD(floor(case when CumulativeEnergykVAhTZ4 is null then 0 else  CumulativeEnergykVAhTZ4 end  *100 ),9,'0') as char) as CumulativeEnergykVAhTZ4Import,");
                    builder.Append(" cast(LPAD(floor(case when CumulativeEnergykVAhTZ5 is null then 0 else  CumulativeEnergykVAhTZ5 end  *100 ),9,'0') as char) as CumulativeEnergykVAhTZ5Import,");
                    builder.Append(" cast(LPAD(floor(case when CumulativeEnergykVAhTZ6 is null then 0 else  CumulativeEnergykVAhTZ6 end  *100 ),9,'0') as char) as CumulativeEnergykVAhTZ6Import,");


                    builder.Append(" cast(LPAD(floor(case when MDkWTZ0 is null then 0 else MDkWTZ0 end *1000 ),8,'0') as char) as MDkWTZ0Import,");


                    builder.Append(" case when BillingDate is null then 0 else BillingDate end as BillingDate,");

                    builder.Append(" cast(LPAD(floor(case when CumulativeEnergykvarhLead is null then 0 else CumulativeEnergykvarhLead end *100 ),9,'0') as char) as CumulativeEnergykvarhLead");

                    builder.Append(" FROM `dlms_ltct_650`.meterdata_billing billing join `dlms_ltct_650`.meterdata_general general on general.meterData_ID = billing.meterData_ID");
                    builder.Append(" join `dlms_ltct_650`.meterdata mdata on general.meterData_ID = mdata.meterData_ID");
                    builder.Append(" left join `dlms_ltct_650`.`consumermeter` consumer on consumer. Meter_ID = mdata.MeterID");
                    builder.Append(" where billing.DataIndex =1 and ");
                    builder.Append(string.Concat("billing.meterData_ID", " = ", ParameterName("meterData_ID"), ""));
                }


                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName("meterData_ID"), meterID, DbType.String);                
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);

            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetRelianceMumbaiDataForTextExport(string meterID, string meterVariant)", ex);
            }
            return dataSet;
        }
        public DataSet GetRelianceMumbaiDataForTextExportwithsolar(string meterID, string meterVariant, int solartype, int historyNo)
        {
            DataSet dataSet = null;
            string meterid = string.Empty;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();

                if (meterVariant == MeterVariant.FOUR)
                {
                    builder.Append("SELECT cast(IFNULL(consumer.Consumer_Number,null) as char) as Consumer_Number,");

                    builder.Append(" cast(LPAD(IFNULL(meterSerialNumber,0),7,'0') as char) as meterSerialNumber,");

                    builder.Append(" cast(LPAD(floor(IFNULL(COALESCE(CumulativeEnergykWhTZ0Import,CumulativeEnergykWhTZ0),0) *100 ),9,'0') as char) as CumulativeEnergykWhTZ0Import,");

                    builder.Append(" cast(LPAD(floor(IFNULL(COALESCE(CumulativeEnergykWhTZ1Import,CumulativeEnergykWhTZ1),0) *100 ),9,'0') as char) as CumulativeEnergykWhTZ1Import,");
                    builder.Append(" cast(LPAD(floor(IFNULL(COALESCE(CumulativeEnergykWhTZ2Import,CumulativeEnergykWhTZ2),0) *100 ),9,'0') as char) as CumulativeEnergykWhTZ2Import,");
                    builder.Append(" cast(LPAD(floor(IFNULL(COALESCE(CumulativeEnergykWhTZ3Import,CumulativeEnergykWhTZ3),0) *100 ),9,'0') as char) as CumulativeEnergykWhTZ3Import,");
                    builder.Append(" cast(LPAD(floor(IFNULL(COALESCE(CumulativeEnergykWhTZ4Import,CumulativeEnergykWhTZ4),0) *100 ),9,'0') as char) as CumulativeEnergykWhTZ4Import,");
                    builder.Append(" cast(LPAD(floor(IFNULL(COALESCE(CumulativeEnergykWhTZ5Import,CumulativeEnergykWhTZ5),0) *100 ),9,'0') as char) as CumulativeEnergykWhTZ5Import,");
                    builder.Append(" cast(LPAD(floor(IFNULL(COALESCE(CumulativeEnergykWhTZ6Import,CumulativeEnergykWhTZ6),0) *100 ),9,'0') as char) as CumulativeEnergykWhTZ6Import,");
         
                    builder.Append(" cast(LPAD(floor(IFNULL(COALESCE(MDkVATZ1Import,MDkVATZ1),0) *1000 ),8,'0') as char) as MDkVATZ1Import,");
                    builder.Append(" cast(LPAD(floor(IFNULL(COALESCE(MDkVATZ2Import,MDkVATZ2),0) *1000 ),8,'0') as char) as MDkVATZ2Import,");
                    builder.Append(" case when COALESCE(MDkVATZ3Import,MDkVATZ3) is null then null else cast(LPAD(floor(COALESCE(MDkVATZ3Import,MDkVATZ3)  *1000 ),8,'0') as char)end as MDkVATZ3Import, ");
                    builder.Append(" cast(LPAD(floor(IFNULL(COALESCE(MDkVATZ4Import,MDkVATZ4),0) *1000 ),8,'0') as char) as MDkVATZ4Import,");
                    builder.Append(" cast(LPAD(floor(IFNULL(COALESCE(MDkVATZ5Import,MDkVATZ5),0) *1000 ),8,'0') as char) as MDkVATZ5Import,");
                    builder.Append(" cast(LPAD(floor(IFNULL(COALESCE(MDkVATZ6Import,MDkVATZ6),0) *1000 ),8,'0') as char) as MDkVATZ6Import,");

                    builder.Append(" cast(LPAD(floor(IFNULL(COALESCE(CumulativeEnergykvarhLagQ1,CumulativeEnergykvarhLag),0) *100 ),9,'0') as char) as CumulativeEnergykvarhLagQ1,");
                    builder.Append(" cast(LPAD(floor(IFNULL(CumulativeEnergykvarhLagTZ1Q1,0)  *100 ),9,'0') as char) as CumulativeEnergykvarhLagTZ1,  ");
                    builder.Append(" cast(LPAD(floor(IFNULL(CumulativeEnergykvarhLagTZ2Q1,0)  *100 ),9,'0') as char) as CumulativeEnergykvarhLagTZ2,  ");
                    builder.Append(" cast(LPAD(floor(IFNULL(CumulativeEnergykvarhLagTZ3Q1,0)  *100 ),9,'0') as char) as CumulativeEnergykvarhLagTZ3,  ");
                    builder.Append(" cast(LPAD(floor(IFNULL(CumulativeEnergykvarhLagTZ4Q1,0)  *100 ),9,'0') as char) as CumulativeEnergykvarhLagTZ4,  ");
                    builder.Append(" cast(LPAD(floor(IFNULL(CumulativeEnergykvarhLagTZ5Q1,0)  *100 ),9,'0') as char) as CumulativeEnergykvarhLagTZ5,  ");
                    builder.Append(" cast(LPAD(floor(IFNULL(CumulativeEnergykvarhLagTZ6Q1,0)  *100 ),9,'0') as char) as CumulativeEnergykvarhLagTZ6,  "); 

                    builder.Append(" cast(LPAD(floor(IFNULL(COALESCE(MDkWTZ0Import,MDkWTZ0),0) *1000 ),8,'0') as char) as MDkWTZ0Import,");

                    builder.Append(" IFNULL(BillingDate,0) as BillingDate,");

                    builder.Append(" cast(LPAD(floor(IFNULL(CumulativeEnergykWhTZ0Export,0) *100 ),9,'0') as char) as CumulativeEnergykWhTZ0Export,");

                    // SB Change Start 20170919
                    builder.Append(" cast(LPAD(floor(IFNULL(CumulativeEnergykWhTZ1Export,0) *100 ),9,'0') as char) as CumulativeEnergykWhTZ1Export,");
                    builder.Append(" cast(LPAD(floor(IFNULL(CumulativeEnergykWhTZ2Export,0) *100 ),9,'0') as char) as CumulativeEnergykWhTZ2Export,");
                    builder.Append(" cast(LPAD(floor(IFNULL(CumulativeEnergykWhTZ3Export,0) *100 ),9,'0') as char) as CumulativeEnergykWhTZ3Export,");
                    builder.Append(" cast(LPAD(floor(IFNULL(CumulativeEnergykWhTZ4Export,0) *100 ),9,'0') as char) as CumulativeEnergykWhTZ4Export,");
                    builder.Append(" cast(LPAD(floor(IFNULL(CumulativeEnergykWhTZ5Export,0) *100 ),9,'0') as char) as CumulativeEnergykWhTZ5Export,");
                    //builder.Append(" cast(LPAD(floor(IFNULL(CumulativeEnergykWhTZ6Export,0) *100 ),9,'0') as char) as CumulativeEnergykWhTZ6Export,");

                    builder.Append(" cast(LPAD(floor(IFNULL(MDkWTZ0Export,0) *1000 ),8,'0') as char) as MDkWTZ0Export,");
                    // SB Change End 20170919

                    builder.Append(" CASE " +
                                            " WHEN NOT MDkVADateTimeTZ0Import IS NULL AND NOT MDkVADateTimeTZ0Import = 0 THEN MDkVADateTimeTZ0Import" +
                                            " WHEN NOT MDkVADateTimeTZ0 IS NULL AND NOT MDkVADateTimeTZ0 = 0 THEN MDkVADateTimeTZ0" +
                                            " ELSE 0 END " +
                                            " as MDkVADateTimeTZ0, ");

                    builder.Append(" cast(LPAD(floor(IFNULL(COALESCE(MDkVATZ0Import,MDkVATZ0),0) *1000 ),8,'0') as char) as MDkVATZ0Import,");

                    builder.Append(" cast(LPAD(floor(IFNULL(COALESCE(CumulativeEnergykvarhLeadQ4,CumulativeEnergykvarhLead),0) *100 ),9,'0') as char) as CumulativeEnergykvarhLead ");

                    if (solartype == (int)RelianceBilling.Solar)
                    {
                        builder.Append(",");
                        builder.Append(" cast(LPAD(floor(IFNULL(coalesce(CumulativeEnergykVAhTZ0Import,CumulativeEnergykVAhTZ0),0) *100 ),9,'0') as char) as CumulativeEnergykVAhTZ0Import,");
                        builder.Append(" cast(LPAD(floor(IFNULL(coalesce(CumulativeEnergykVAhTZ1Import,CumulativeEnergykVAhTZ1),0)  *100 ),9,'0') as char) as CumulativeEnergykVAhTZ1Import,");
                        builder.Append(" cast(LPAD(floor(IFNULL(coalesce(CumulativeEnergykVAhTZ2Import,CumulativeEnergykVAhTZ2),0)  *100 ),9,'0') as char) as CumulativeEnergykVAhTZ2Import,");
                        builder.Append(" cast(LPAD(floor(IFNULL(coalesce(CumulativeEnergykVAhTZ3Import,CumulativeEnergykVAhTZ3),0)  *100 ),9,'0') as char) as CumulativeEnergykVAhTZ3Import,");
                        builder.Append(" cast(LPAD(floor(IFNULL(coalesce(CumulativeEnergykVAhTZ4Import,CumulativeEnergykVAhTZ4),0)  *100 ),9,'0') as char) as CumulativeEnergykVAhTZ4Import,");
                        builder.Append(" cast(LPAD(floor(IFNULL(coalesce(CumulativeEnergykVAhTZ5Import,CumulativeEnergykVAhTZ5),0)  *100 ),9,'0') as char) as CumulativeEnergykVAhTZ5Import,");
                        builder.Append(" cast(LPAD(floor(IFNULL(coalesce(CumulativeEnergykVAhTZ6Import,CumulativeEnergykVAhTZ6),0)  *100 ),9,'0') as char) as CumulativeEnergykVAhTZ6Import");
                    }

                    builder.Append(" FROM `dlms_ltct_650`.meterdata_billing billing join `dlms_ltct_650`.meterdata_general general on general.meterData_ID = billing.meterData_ID");
                    builder.Append(" join `dlms_ltct_650`.meterdata mdata on general.meterData_ID = mdata.meterData_ID");
                    builder.Append(" left join `dlms_ltct_650`.`consumermeter` consumer on consumer. Meter_ID = mdata.MeterID");
                    builder.AppendFormat(" where billing.DataIndex ={0} and ", historyNo);
                    builder.Append(string.Concat("billing.meterData_ID", " = ", ParameterName("meterData_ID"), ""));


                }
                else
                {

                    builder.Append("SELECT cast(case when consumer.Consumer_Number is null then null else consumer.Consumer_Number end as char) as Consumer_Number,");

                    builder.Append(" cast(LPAD(case when meterSerialNumber is null then 0 else meterSerialNumber end,7,'0') as char) as meterSerialNumber,");

                    builder.Append(" cast(LPAD(floor(case when CumulativeEnergykWhTZ0 is null then 0 else CumulativeEnergykWhTZ0 end *100 ),9,'0') as char) as CumulativeEnergykWhTZ0Import,");

                    builder.Append(" cast(LPAD(floor(case when CumulativeEnergykWhTZ1 is null then 0 else CumulativeEnergykWhTZ1 end *100 ),9,'0') as char) as CumulativeEnergykWhTZ1Import,");
                    builder.Append(" cast(LPAD(floor(case when CumulativeEnergykWhTZ2 is null then 0 else CumulativeEnergykWhTZ2 end *100 ),9,'0') as char) as CumulativeEnergykWhTZ2Import,");
                    builder.Append(" cast(LPAD(floor(case when CumulativeEnergykWhTZ3 is null then 0 else CumulativeEnergykWhTZ3 end *100 ),9,'0') as char) as CumulativeEnergykWhTZ3Import,");
                    builder.Append(" cast(LPAD(floor(case when CumulativeEnergykWhTZ4 is null then 0 else CumulativeEnergykWhTZ4 end *100 ),9,'0') as char) as CumulativeEnergykWhTZ4Import,");
                    builder.Append(" cast(LPAD(floor(case when CumulativeEnergykWhTZ5 is null then 0 else CumulativeEnergykWhTZ5 end *100 ),9,'0') as char) as CumulativeEnergykWhTZ5Import,");
                    builder.Append(" cast(LPAD(floor(case when CumulativeEnergykWhTZ6 is null then 0 else CumulativeEnergykWhTZ6 end *100 ),9,'0') as char) as CumulativeEnergykWhTZ6Import,");

                    builder.Append(" cast(LPAD(floor(case when MDkVATZ1 is null then 0   else  MDkVATZ1 end *1000 ),8,'0') as char) as MDkVATZ1Import,  "); 
                    builder.Append(" cast(LPAD(floor(case when MDkVATZ2 is null then 0   else  MDkVATZ2 end *1000 ),8,'0') as char) as MDkVATZ2Import,  ");
                    builder.Append(" case when MDkVATZ3 is null then null else cast(LPAD(floor(MDkVATZ3  *1000 ),8,'0') as char)end as MDkVATZ3Import, ");
                    builder.Append(" cast(LPAD(floor(case when MDkVATZ4 is null then 0   else  MDkVATZ4 end *1000 ),8,'0') as char) as  MDkVATZ4Import,  "); 
                    builder.Append(" cast(LPAD(floor(case when MDkVATZ5 is null then 0   else  MDkVATZ5 end *1000 ),8,'0') as char) as MDkVATZ5Import,  ");
                    builder.Append(" cast(LPAD(floor(case when MDkVATZ6 is null then 0   else  MDkVATZ6 end *1000 ),8,'0') as char) as MDkVATZ6Import,  "); 

                    builder.Append(" cast(LPAD(floor(IFNULL(CumulativeEnergykvarhLag,0) *100 ),9,'0') as char) as CumulativeEnergykvarhLagQ1, "); 
                    builder.Append(" cast(LPAD(floor(IFNULL(CumulativeEnergykvarhLagTZ1Q1,0)  *100 ),9,'0') as char) as CumulativeEnergykvarhLagTZ1,  "); 
                    builder.Append(" cast(LPAD(floor(IFNULL(CumulativeEnergykvarhLagTZ2Q1,0)  *100 ),9,'0') as char) as CumulativeEnergykvarhLagTZ2,  "); 
                    builder.Append(" cast(LPAD(floor(IFNULL(CumulativeEnergykvarhLagTZ3Q1,0)  *100 ),9,'0') as char) as CumulativeEnergykvarhLagTZ3,  "); 
                    builder.Append(" cast(LPAD(floor(IFNULL(CumulativeEnergykvarhLagTZ4Q1,0)  *100 ),9,'0') as char) as CumulativeEnergykvarhLagTZ4,  "); 
                    builder.Append(" cast(LPAD(floor(IFNULL(CumulativeEnergykvarhLagTZ5Q1,0)  *100 ),9,'0') as char) as CumulativeEnergykvarhLagTZ5,  ");
                    builder.Append(" cast(LPAD(floor(IFNULL(CumulativeEnergykvarhLagTZ6Q1,0)  *100 ),9,'0') as char) as CumulativeEnergykvarhLagTZ6,  "); 

                    builder.Append(" cast(LPAD(floor(case when MDkWTZ0 is null then 0 else MDkWTZ0 end *1000 ),8,'0') as char) as MDkWTZ0Import,");
                    builder.Append(" case when BillingDate is null then 0 else BillingDate end as BillingDate,");

                    // SB Change Start 20170919
                    builder.Append(" cast(LPAD(floor(case when CumulativeEnergykWhTZ0Export is null then 0 else CumulativeEnergykWhTZ0Export end *100 ),9,'0') as char) as CumulativeEnergykWhTZ0Export,");

                    builder.Append(" cast(LPAD(floor(case when CumulativeEnergykWhTZ1Export is null then 0 else CumulativeEnergykWhTZ1Export end *100 ),9,'0') as char) as CumulativeEnergykWhTZ1Export,");
                    builder.Append(" cast(LPAD(floor(case when CumulativeEnergykWhTZ2Export is null then 0 else CumulativeEnergykWhTZ2Export end *100 ),9,'0') as char) as CumulativeEnergykWhTZ2Export,");
                    builder.Append(" cast(LPAD(floor(case when CumulativeEnergykWhTZ3Export is null then 0 else CumulativeEnergykWhTZ3Export end *100 ),9,'0') as char) as CumulativeEnergykWhTZ3Export,");
                    builder.Append(" cast(LPAD(floor(case when CumulativeEnergykWhTZ4Export is null then 0 else CumulativeEnergykWhTZ4Export end *100 ),9,'0') as char) as CumulativeEnergykWhTZ4Export,");
                    builder.Append(" cast(LPAD(floor(case when CumulativeEnergykWhTZ5Export is null then 0 else CumulativeEnergykWhTZ5Export end *100 ),9,'0') as char) as CumulativeEnergykWhTZ5Export,");

                    builder.Append(" cast(LPAD(floor(case when MDkWTZ0Export is null then 0 else MDkWTZ0Export end *1000 ),8,'0') as char) as MDkWTZ0Export,");
                    // SB Change End 20170919

                    builder.Append(" case when MDkVADateTimeTZ0 is null then 0 else MDkVADateTimeTZ0 end as MDkVADateTimeTZ0, ");
                    builder.Append(" cast(LPAD(floor(case when MDkVATZ0 is null then 0 else MDkVATZ0 end *1000 ),8,'0') as char) as MDkVATZ0Import,  "); 

                    builder.Append(" cast(LPAD(floor(case when CumulativeEnergykvarhLead is null then 0 else CumulativeEnergykvarhLead end *100 ),9,'0') as char) as CumulativeEnergykvarhLead ");

                    if (solartype == (int)RelianceBilling.Solar)
                    {
                        builder.Append(",");
                        builder.Append(" cast(LPAD(floor(case when CumulativeEnergykVAhTZ0 is null then 0 else  CumulativeEnergykVAhTZ0 end  *100 ),9,'0') as char) as CumulativeEnergykVAhTZ0Import, ");
                        builder.Append(" cast(LPAD(floor(case when CumulativeEnergykVAhTZ1 is null then 0 else  CumulativeEnergykVAhTZ1 end  *100 ),9,'0') as char) as CumulativeEnergykVAhTZ1Import,");
                        builder.Append(" cast(LPAD(floor(case when CumulativeEnergykVAhTZ2 is null then 0 else  CumulativeEnergykVAhTZ2 end  *100 ),9,'0') as char) as CumulativeEnergykVAhTZ2Import,");
                        builder.Append(" cast(LPAD(floor(case when CumulativeEnergykVAhTZ3 is null then 0 else  CumulativeEnergykVAhTZ3 end  *100 ),9,'0') as char) as CumulativeEnergykVAhTZ3Import,");
                        builder.Append(" cast(LPAD(floor(case when CumulativeEnergykVAhTZ4 is null then 0 else  CumulativeEnergykVAhTZ4 end  *100 ),9,'0') as char) as CumulativeEnergykVAhTZ4Import,");
                        builder.Append(" cast(LPAD(floor(case when CumulativeEnergykVAhTZ5 is null then 0 else  CumulativeEnergykVAhTZ5 end  *100 ),9,'0') as char) as CumulativeEnergykVAhTZ5Import,");
                        builder.Append(" cast(LPAD(floor(case when CumulativeEnergykVAhTZ6 is null then 0 else  CumulativeEnergykVAhTZ6 end  *100 ),9,'0') as char) as CumulativeEnergykVAhTZ6Import");
                    }

                    builder.Append(" FROM `dlms_ltct_650`.meterdata_billing billing join `dlms_ltct_650`.meterdata_general general on general.meterData_ID = billing.meterData_ID");
                    builder.Append(" join `dlms_ltct_650`.meterdata mdata on general.meterData_ID = mdata.meterData_ID");
                    builder.Append(" left join `dlms_ltct_650`.`consumermeter` consumer on consumer. Meter_ID = mdata.MeterID");
                    builder.AppendFormat(" where billing.DataIndex ={0} and ", historyNo);
                    builder.Append(string.Concat("billing.meterData_ID", " = ", ParameterName("meterData_ID"), ""));
                }


                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName("meterData_ID"), meterID, DbType.String);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);

            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetRelianceMumbaiDataForTextExportwithsolar(string meterID, string meterVariant)", ex);
                
            }
            return dataSet;
        }





        /// <summary>
        /// Get Puducherry Data For Text Export of  BRPL 
        /// </summary>
        /// <param name="meterDataID"></param>
        /// <returns></returns>
        public DataSet GetPuducherryDataForTextExport(long meterdataID, string meterid)
        {
            DataSet dataSet = null;
            //string meterid = string.Empty;

            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                //builder.Append("SELECT cast(LPAD(consumer.Consumer_Number,12,' ')as char) as ConsumerNumber ,cast(LPAD(consumer.ConsumerMeter_ID,5,'0')as char) as ConsumerTypeID , cast(LPAD(consumer.Meter_ID,10,' ')as char) as meterSerialNumber, instant.InstantPowerColumnValue as InstantPowerColumnValue FROM meterdata_instantpower instant, consumermeter consumer  where consumer.Meter_ID = '" + mid + "' and instant.MeterData_ID = '" + meterID + "';");
                //-----------------------
                
                
                ///****************

                builder.Append("SELECT  cast(LPAD(consumer.Consumer_Number,12,'0')as char) as ConsumerNumber ,cast(LPAD(cm.ConsumerType_ID,5,'0')as char) as ConsumerTypeID , cast(LPAD(consumer.Meter_ID,10,'0')as char) as meterSerialNumber, ");
                builder.Append("CONCAT(Substring((MAX(CASE WHEN a.InstantPowerColumnName = 'Real Time Clock - Date and Time' THEN a.InstantPowerColumnValue END)),7, 2), ");

                builder.Append("Substring((MAX(CASE WHEN a.InstantPowerColumnName = 'Real Time Clock - Date and Time' THEN a.InstantPowerColumnValue END)),5, 2), Substring((MAX(CASE WHEN a.InstantPowerColumnName = 'Real Time Clock - Date and Time' THEN a.InstantPowerColumnValue END)),1, 4)) AS RTC, ");
                builder.Append("cast(LPAD(floor(REPLACE(MAX(CASE WHEN a.InstantPowerColumnName = 'Cumulative Energy History 1' THEN a.InstantPowerColumnValue END),'*kWh', '')*100), 8, '0') AS char ) AS kWh1,");

                builder.Append("CONCAT(cast(LPAD(floor(REPLACE(MAX(CASE WHEN a.InstantPowerColumnName = 'Cumulative Energy Current' THEN a.InstantPowerColumnValue END),'*kWh', '')*100), 8, '0') AS char),");

                builder.Append("CONCAT(Substring((MAX(CASE WHEN a.InstantPowerColumnName = 'Real Time Clock - Date and Time' THEN a.InstantPowerColumnValue END)),7, 2),");

                builder.Append("Substring((MAX(CASE WHEN a.InstantPowerColumnName = 'Real Time Clock - Date and Time' THEN a.InstantPowerColumnValue END)),5, 2), Substring((MAX(CASE WHEN a.InstantPowerColumnName = 'Real Time Clock - Date and Time' THEN a.InstantPowerColumnValue END)),1, 4),");

                builder.Append("Substring((MAX(CASE WHEN a.InstantPowerColumnName = 'Real Time Clock - Date and Time' THEN a.InstantPowerColumnValue END)),9, 2), Substring((MAX(CASE WHEN a.InstantPowerColumnName = 'Real Time Clock - Date and Time' THEN a.InstantPowerColumnValue END)),11, 2),");

                builder.Append("Substring((MAX(CASE WHEN a.InstantPowerColumnName = 'Real Time Clock - Date and Time' THEN a.InstantPowerColumnValue END)),13, 2))) AS CumukWh, ");


                builder.Append("CONCAT(cast(LPAD(floor(REPLACE(MAX(CASE WHEN a.InstantPowerColumnName = 'Net Energy' THEN a.InstantPowerColumnValue END),'*kWh', '')*100), 8, '0') AS char),");

                builder.Append("CONCAT(Substring((MAX(CASE WHEN a.InstantPowerColumnName = 'Real Time Clock - Date and Time' THEN a.InstantPowerColumnValue END)),7, 2),");

                builder.Append("Substring((MAX(CASE WHEN a.InstantPowerColumnName = 'Real Time Clock - Date and Time' THEN a.InstantPowerColumnValue END)),5, 2), Substring((MAX(CASE WHEN a.InstantPowerColumnName = 'Real Time Clock - Date and Time' THEN a.InstantPowerColumnValue END)),1, 4),");

                builder.Append("Substring((MAX(CASE WHEN a.InstantPowerColumnName = 'Real Time Clock - Date and Time' THEN a.InstantPowerColumnValue END)),9, 2), Substring((MAX(CASE WHEN a.InstantPowerColumnName = 'Real Time Clock - Date and Time' THEN a.InstantPowerColumnValue END)),11, 2),");

                builder.Append("Substring((MAX(CASE WHEN a.InstantPowerColumnName = 'Real Time Clock - Date and Time' THEN a.InstantPowerColumnValue END)),13, 2))) AS NetkWh, ");

                builder.Append("CONCAT(LPAD(floor(REPLACE(MAX(CASE WHEN a.InstantPowerColumnName = 'Maximum Demand kVA History 1' THEN a.InstantPowerColumnValue END),'*kVA', '')*100), 4, '0'),");

                builder.Append("CONCAT(Substring((MAX(CASE WHEN a.InstantPowerColumnName = 'Maximum Demand kVA History 1 Date Time' THEN a.InstantPowerColumnValue END)),7, 2), Substring((MAX(CASE WHEN a.InstantPowerColumnName = 'Maximum Demand kVA History 1 Date Time' THEN a.InstantPowerColumnValue END)),5, 2),");

                builder.Append("REPLACE(Substring((MAX(CASE WHEN a.InstantPowerColumnName = 'Maximum Demand kVA History 1 Date Time' THEN a.InstantPowerColumnValue END)),1, 4), '2000', '0000'),");

                builder.Append("Substring((MAX(CASE WHEN a.InstantPowerColumnName = 'Maximum Demand kVA History 1 Date Time' THEN a.InstantPowerColumnValue END)),9, 2), Substring((MAX(CASE WHEN a.InstantPowerColumnName = 'Maximum Demand kVA History 1 Date Time' THEN a.InstantPowerColumnValue END)),11, 2))) AS mdkva1,");

                builder.Append("CONCAT(LPAD(floor(REPLACE(MAX(CASE WHEN a.InstantPowerColumnName = 'Maximum Demand kVA Current' THEN a.InstantPowerColumnValue END),'*kVA', '')*100), 4, '0'), CONCAT(Substring((MAX(CASE WHEN a.InstantPowerColumnName = 'Maximum Demand kVA Date Time' THEN a.InstantPowerColumnValue END)),7, 2),");

                builder.Append("Substring((MAX(CASE WHEN a.InstantPowerColumnName = 'Maximum Demand kVA Date Time' THEN a.InstantPowerColumnValue END)),5, 2), REPLACE(Substring((MAX(CASE WHEN a.InstantPowerColumnName = 'Maximum Demand kVA Date Time' THEN a.InstantPowerColumnValue END)),1, 4), '2000', '0000'),");

                builder.Append("Substring((MAX(CASE WHEN a.InstantPowerColumnName = 'Maximum Demand kVA Date Time' THEN a.InstantPowerColumnValue END)),9, 2), Substring((MAX(CASE WHEN a.InstantPowerColumnName = 'Maximum Demand kVA Date Time' THEN a.InstantPowerColumnValue END)),11, 2))) AS mdkva,");

                builder.Append("cast(LPAD(floor(MAX(CASE WHEN a.InstantPowerColumnName = 'Billing Avg PF History 1' THEN a.InstantPowerColumnValue END)*100), 4, '0') AS char)AS avgpf,");

                builder.Append("LPAD(MAX(CASE WHEN a.InstantPowerColumnName = 'Cumulative Tamper Counter' THEN a.InstantPowerColumnValue END), 4, '0') AS ctc,");

                builder.Append("LPAD(MAX(CASE WHEN a.InstantPowerColumnName = 'Billing Tamper Counter History 1' THEN a.InstantPowerColumnValue END), 4, '0') AS bctc");
                builder.Append(" FROM meterdata_instantpower a, consumermeter consumer, consumer_master  cm where consumer.Meter_ID = '" + meterid + "' and a.MeterData_ID = '" + meterdataID + "' and cm.Consumer_Number = consumer.Consumer_Number;");

                /*
                
                builder.Append("SELECT  cast(LPAD(consumer.Consumer_Number,12,' ')as char) as ConsumerNumber ,cast(LPAD(consumer.ConsumerMeter_ID,5,'0')as char) as ConsumerTypeID , cast(LPAD(consumer.Meter_ID,10,' ')as char) as meterSerialNumber, a.MeterData_ID,");
                builder.Append("a.InstantPowerColumnValue as 'ReadingDate',CONCAT(Substring((MAX(CASE WHEN a.InstantPowerColumnName = 'Real Time Clock - Date and Time' THEN a.InstantPowerColumnValue END)),7, 2), ");
                //*******
                builder.Append("Substring((MAX(CASE WHEN a.InstantPowerColumnName = 'Real Time Clock - Date and Time' THEN a.InstantPowerColumnValue END)),5, 2), Substring((MAX(CASE WHEN a.InstantPowerColumnName = 'Real Time Clock - Date and Time' THEN a.InstantPowerColumnValue END)),1, 4)) AS RTC, LPAD(floor(REPLACE(MAX(CASE WHEN a.InstantPowerColumnName = 'Cumulative Energy History 1' THEN a.InstantPowerColumnValue END),'*kWh', '')*100), 8, '0') AS kWh1,");
        builder.Append("CONCAT(LPAD(floor(REPLACE(MAX(CASE WHEN a.InstantPowerColumnName = 'Net Energy' THEN a.InstantPowerColumnValue END),'*kWh', '')*100), 8, '0'),");

//111111
                builder.Append("CONCAT(Substring((MAX(CASE WHEN a.InstantPowerColumnName = 'Real Time Clock - Date and Time' THEN a.InstantPowerColumnValue END)),7, 2), ");
                builder.Append("Substring((MAX(CASE WHEN a.InstantPowerColumnName = 'Real Time Clock - Date and Time' THEN a.InstantPowerColumnValue END)),5, 2), Substring((MAX(CASE WHEN a.InstantPowerColumnName = 'Real Time Clock - Date and Time' THEN a.InstantPowerColumnValue END)),1, 4),");
                builder.Append("Substring((MAX(CASE WHEN a.InstantPowerColumnName = 'Real Time Clock - Date and Time' THEN a.InstantPowerColumnValue END)),9, 2), Substring((MAX(CASE WHEN a.InstantPowerColumnName = 'Real Time Clock - Date and Time' THEN a.InstantPowerColumnValue END)),11, 2),");
                
                builder.Append("Substring((MAX(CASE WHEN a.InstantPowerColumnName = 'Real Time Clock - Date and Time' THEN a.InstantPowerColumnValue END)),13, 2))) AS NetkWh,");

        builder.Append("CONCAT(LPAD(floor(REPLACE(MAX(CASE WHEN a.InstantPowerColumnName = 'Maximum Demand kVA History 1' THEN a.InstantPowerColumnValue END),'*kWh', '')*100), 8, '0'),");
                
                builder.Append("CONCAT(Substring((MAX(CASE WHEN a.InstantPowerColumnName = 'Maximum Demand kVA History 1 Date Time' THEN a.InstantPowerColumnValue END)),7, 2), Substring((MAX(CASE WHEN a.InstantPowerColumnName = 'Maximum Demand kVA History 1 Date Time' THEN a.InstantPowerColumnValue END)),5, 2),");
                
                builder.Append("REPLACE(Substring((MAX(CASE WHEN a.InstantPowerColumnName = 'Maximum Demand kVA History 1 Date Time' THEN a.InstantPowerColumnValue END)),1, 4), '2000', '0000'),");
                
                builder.Append("Substring((MAX(CASE WHEN a.InstantPowerColumnName = 'Maximum Demand kVA History 1 Date Time' THEN a.InstantPowerColumnValue END)),9, 2), Substring((MAX(CASE WHEN a.InstantPowerColumnName = 'Maximum Demand kVA History 1 Date Time' THEN a.InstantPowerColumnValue END)),11, 2))) AS mdkva1,");

        builder.Append("CONCAT(LPAD(floor(REPLACE(MAX(CASE WHEN a.InstantPowerColumnName = 'Maximum Demand kVA' THEN a.InstantPowerColumnValue END),'*kWh', '')*100), 8, '0'), CONCAT(Substring((MAX(CASE WHEN a.InstantPowerColumnName = 'Maximum Demand kVA Date Time' THEN a.InstantPowerColumnValue END)),7, 2),");
                
                builder.Append("Substring((MAX(CASE WHEN a.InstantPowerColumnName = 'Maximum Demand kVA Date Time' THEN a.InstantPowerColumnValue END)),5, 2), REPLACE(Substring((MAX(CASE WHEN a.InstantPowerColumnName = 'Maximum Demand kVA Date Time' THEN a.InstantPowerColumnValue END)),1, 4), '2000', '0000'),");
                
                builder.Append("Substring((MAX(CASE WHEN a.InstantPowerColumnName = 'Maximum Demand kVA Date Time' THEN a.InstantPowerColumnValue END)),9, 2), Substring((MAX(CASE WHEN a.InstantPowerColumnName = 'Maximum Demand kVA Date Time' THEN a.InstantPowerColumnValue END)),11, 2))) AS mdkva,");

        builder.Append("LPAD(floor(MAX(CASE WHEN a.InstantPowerColumnName = 'Billing Avg PF History 1' THEN a.InstantPowerColumnValue END)*100), 4, '0') AS avgpf,");
        
                builder.Append("LPAD(MAX(CASE WHEN a.InstantPowerColumnName = Cumulative Tamper Counter THEN a.InstantPowerColumnValue END), 4, '0') AS ctc,");

        builder.Append("LPAD(MAX(CASE WHEN a.InstantPowerColumnName = Billing Tamper Counter History 1 THEN a.InstantPowerColumnValue END), 4, '0') AS bctc)) FROM meterdata_instantpower a, consumermeter consumer  where consumer.Meter_ID = '" + mid + "' and a.MeterData_ID = '" + meterID + "';");
                 */

                ////////***************************



                //-----------------------
                /*builder.Append("SELECT cast(LPAD(consumer.Consumer_Number,12,' ')as char) as ConsumerNumber ,cast(LPAD(consumer.ConsumerMeter_ID,5,'0')as char) as ConsumerTypeID , cast(LPAD(consumer.Meter_ID,10,' ')as char) as meterSerialNumber from consumermeter consumer where consumer.Meter_ID = '" + mid + "' select meterdata_instantpower instant where  instant.MeterData_ID = " + meterID + ";");*/
              //  builder.Append(",cast(LPAD(consumer.Meter_ID,10,' ')as char) as meterSerialNumber");
              //  builder.Append(",cast(LPAD(floor(instant.InstantPowerColumnValue *100 ),10,'0') as char) as InstantPowerColumnValue FROM meterdata_instantpower instant, consumermeter consumer");
              //  builder.Append(", meterdata mdata where consumer.meterData_ID = instant.meterData_ID and consumer.meterData_ID = mdata.meterData_ID and instant.meterData_ID = mdata.meterData_ID ");
                //builder.Append("and instant.meterData_ID IN (select meterdata_id from meterdata where ");
                //builder.Append(string.Concat("MeterId", "=", ParameterName("MeterId"),")"));
              //  builder.Append(string.Concat("FileUpload_ID", "=", ParameterName("FileUpload_ID"), ")"));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName("FileUpload_ID"), meterdataID, DbType.String);
                //data = helper.ExecuteScalar(request);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);

            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetPuducherryDataForTextExport(long meterdataID, string meterid)", ex);
            }
            return dataSet;
        }

        public DataSet GetCSPDCLDataForTextExport(string meterDataID)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder sqlBuilder = new StringBuilder();
                sqlBuilder.Append(@"use dlms_ltct_650;
                                    select H0.CumulativeEnergykWhTZ1, H0.CumulativeEnergykWhTZ2, H0.CumulativeEnergykWhTZ3, H1.CumulativeEnergykWhTZ1, H1.CumulativeEnergykWhTZ2, H1.CumulativeEnergykWhTZ3, H0.CumulativeEnergykWhTZ0, H1.CumulativeEnergykWhTZ0, H0.CumulativeEnergykVAhTZ0, H1.CumulativeEnergykVAhTZ0, H0.CumulativeEnergykvarhLag, H1.CumulativeEnergykvarhLag, H1.CumulativeEnergykvarhLead, H0.SystemPowerFactorforBillingPeriod, H0.MDkVATZ0, CumMD.CumMDKVA, BillCount.BillingCount, '' as EMF, H1.BillingDate, SNo.meterSerialNumber, H2.BillingDate, H0.CumulativeEnergykVAhTZ1, H0.CumulativeEnergykVAhTZ2, H0.CumulativeEnergykVAhTZ3, H1.CumulativeEnergykVAhTZ1, H1.CumulativeEnergykVAhTZ2, H1.CumulativeEnergykVAhTZ3
                                    from 
                                    ((select meterSerialNumber,MeterData_ID 
                                    from meterdata_general) as SNo
                                    left outer join
                                    (select InstantPowerColumnValue as BillingCount,MeterData_ID from meterdata_instantpower
                                    where InstantPowerObisCode in ('0.0.0.1.0.255')) as BillCount on SNo.MeterData_ID = BillCount.MeterData_ID
                                    left outer join
                                    (select InstantPowerColumnValue as CumMDKVA,MeterData_ID from meterdata_instantpower
                                    where InstantPowerObisCode in ('1.0.9.2.0.255')) as CumMD on SNo.MeterData_ID = CumMD.MeterData_ID
                                    left outer join
                                    (select MeterData_ID, CumulativeEnergykWhTZ0, CumulativeEnergykVAhTZ0, 
                                    CumulativeEnergykWhTZ1, CumulativeEnergykWhTZ2, CumulativeEnergykWhTZ3, CumulativeEnergykVAhTZ1, CumulativeEnergykVAhTZ2, CumulativeEnergykVAhTZ3, CumulativeEnergykvarhLag,
                                    MDkVATZ0, SystemPowerFactorforBillingPeriod
                                    from meterdata_billing where DataIndex = 0) as H0 on SNo.MeterData_ID = H0.MeterData_ID
                                    left outer join
                                    (select MeterData_ID, BillingDate, CumulativeEnergykWhTZ0, CumulativeEnergykVAhTZ0, 
                                    CumulativeEnergykWhTZ1, CumulativeEnergykWhTZ2, CumulativeEnergykWhTZ3, CumulativeEnergykVAhTZ1, CumulativeEnergykVAhTZ2, CumulativeEnergykVAhTZ3, CumulativeEnergykvarhLag,CumulativeEnergykvarhLead
                                    from meterdata_billing where DataIndex = 1) as H1 on SNo.MeterData_ID = H1.MeterData_ID
                                    left outer join
                                    (select BillingDate, MeterData_ID 
                                    from meterdata_billing where DataIndex = 2) as H2 on SNo.MeterData_ID = H2.MeterData_ID)
                                ");
                sqlBuilder.Append(string.Concat(" where SNo.MeterData_ID", " = ", ParameterName("meterData_ID"), ""));
                    
                DataRequest request = new DataRequest(sqlBuilder.ToString());
                request.AddParamter(ParameterName("meterData_ID"), meterDataID, DbType.String);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);

            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetRelianceLoadSurveyDataForTextExport(string meterDataID)", ex);
                throw ex;
            }
            return dataSet;
        }

        // SarkarA code change start 20171206//Reliance Mumbai Text Export
        /// <summary>
        /// Get LoadSurvey DataSet For Reliance Text Export with MeterData_ID as parameter 
        /// </summary>
        /// <param name="meterDataID"></param>
        /// <param name="sqlQuery"></param>
        /// <returns></returns>
        public DataSet GetRelianceLoadSurveyDataForTextExport(string meterDataID)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder sqlBuilder = new StringBuilder();
                //SarkarA code change start 201800227 // add net kwh,kvah
                sqlBuilder.Append(@"use `dlms_ltct_650`;
                                    SELECT B.MeterID, A.realTimeClockDateandTime,  A.frequency=null as 'IntervalStart', A.frequency=null as 'IntervalEnd',
                                    A.rPhaseVoltage, A.yPhaseVoltage, A.bPhaseVoltage, A.rPhaseCurrent, A.yPhaseCurrent, A.bPhaseCurrent, A.frequency,
                                    A.blockEnergykWh, A.blockEnergykWhExport, A.blockEnergykVAh, A.blockEnergykVAhExport,
                                    A.blockEnergykvarhlag, A.blockEnergykvarhleadQ2, A.blockEnergykvarhlagQ3, A.blockEnergykvarhlead,
                                    A.netkWh, A.netkVAh, A.frequency=null as PowerFactor
                                    FROM `meterdata_loadsurvey` as A join 
                                    (select MeterID,MeterData_ID as meter_Data_ID from meterdata) as B 
                                    on A.meterData_ID=B.meter_Data_ID");
                sqlBuilder.Append(string.Concat(" where MeterData_ID", " = ", ParameterName("meterData_ID"), ""));
                sqlBuilder.Append(" order by A.realTimeClockDateandTime DESC");
                //SarkarA code change end 201800227
                DataRequest request = new DataRequest(sqlBuilder.ToString());
                request.AddParamter(ParameterName("meterData_ID"), meterDataID, DbType.String);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);

            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetRelianceLoadSurveyDataForTextExport(string meterDataID)", ex);
                throw ex;
            }
            return dataSet;
        }

        /// <summary>
        /// Get Instant DataSet1 For Reliance Text Export with MeterData_ID as parameter 
        /// </summary>
        /// <param name="meterDataID"></param>
        /// <param name="sqlQuery"></param>
        /// <returns></returns>
        public DataSet GetRelianceInstantData1ForTextExport(string meterDataID)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder sqlBuilder = new StringBuilder();

                sqlBuilder.Append(@"use dlms_ltct_650;
                                    select Consumer_Number, MeterID,Q.metertype as MeterType_Name, CMRI_Number, UploadingDateTime, ReadingDateTime, PhaseSequence, EMFApplied, Q.internalCTratio, Q.internalPTratio,Meter_InstalledCTRatio, Meter_InstalledPTRatio, AngleYR, AngleBR from
	                                    (select  *  from
	                                    (select * from
	                                    meterdata as A 
	                                    left outer join
	                                    (select Consumer_Number, Meter_ID as Meter__ID  from consumermeter) as B
	                                    on A.MeterID=B.Meter__ID) 
	                                    as C
	                                    left outer join
	                                    (select MeterData_Id as MeterD_ID, PhaseSequence, AngleYR, AngleBR from meterdata_phasor) 
	                                    as D
	                                    on C.MeterData_ID= D.MeterD_Id) 
                                    as P
                                    left outer join
	                                    (SELECT  Y.meterSerialNumber, Y.metertype, Y.internalCTratio, Y.internalPTratio, Z.Meter_ID, Z.Meter_InstalledCTRatio, Z.Meter_InstalledPTRatio, if(Z.UseEMFInCalculations=1, 'Y', 'N') as EMFApplied 
	                                    FROM `meter_master`  as Z 
	                                    right outer join
	                                    meterdata_general as Y on Z.Meter_ID=Y.meterSerialNumber) 
                                    as Q
                                    on P.MeterID=Q.meterSerialNumber");
                sqlBuilder.Append(string.Concat(" where MeterData_ID", " = ", ParameterName("meterData_ID"), ""));

                DataRequest request = new DataRequest(sqlBuilder.ToString());
                request.AddParamter(ParameterName("meterData_ID"), meterDataID, DbType.String);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);

            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetRelianceInstantData1ForTextExport(string meterDataID)", ex);
                throw ex;
            }
            return dataSet;
        }

        /// <summary>
        /// Get Instant DataSet2 For Reliance Text Export with MeterData_ID as parameter 
        /// </summary>
        /// <param name="meterDataID"></param>
        /// <param name="sqlQuery"></param>
        /// <returns></returns>
        public DataSet GetRelianceInstantData2ForTextExport(string meterDataID)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder sqlBuilder = new StringBuilder();

                sqlBuilder.Append(@"use dlms_ltct_650;
                                    SELECT A.InstantPowerColumnName, A.InstantPowerColumnValue FROM meterdata_instantpower as A where A.InstantPowerColumnName in 
                                    ('Real Time Clock - Date and Time','Frequency','Current - IR','Current - IY','Current - IB','Voltage - VRN','Voltage - VYN','Voltage - VBN','Reactive Current - R','Reactive Current - Y','Reactive Current - B',
                                    'Signed Power Factor - R Phase (+Lag;-Lead)','Signed Power Factor - Y Phase (+Lag;-Lead)','Signed Power Factor - B Phase (+Lag;-Lead)','Signed Power Factor (+Lag;-Lead)','Active Power (ABS)','Signed Reactive Power - kvar (+Lag;-Lead)',
                                    'Neutral Current')");
                sqlBuilder.Append(string.Concat(" and MeterData_ID", " = ", ParameterName("meterData_ID"), ""));

                DataRequest request = new DataRequest(sqlBuilder.ToString());
                request.AddParamter(ParameterName("meterData_ID"), meterDataID, DbType.String);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);

            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetRelianceInstantData2ForTextExport(string meterDataID)", ex);
                throw ex;
            }
            return dataSet;
        }

        /// <summary>
        /// Get Tamper Event DataSet For Reliance Text Export with MeterData_ID as parameter 
        /// </summary>
        /// <param name="meterDataID"></param>
        /// <param name="sqlQuery"></param>
        /// <returns></returns>
        public DataSet GetRelianceEventDataForTextExport(string meterDataID)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder sqlBuilder = new StringBuilder();

                sqlBuilder.Append(@"use dlms_ltct_650;
                                    select TamperType, MeterID, C.TamperTypeID, DateTimeEvent from 
                                            (select MeterData_ID as Meter_Data_ID,TamperType, TamperTypeID, DateTimeEvent from 
                                            tampertype_master as A 
                                            inner join tamper_master as B on
                                            A.TamperTypeID=B.EventCode) as C 
                                       join 
                                            meterdata as D on C.Meter_Data_ID=D.MeterData_ID");
                sqlBuilder.Append(string.Concat(" where MeterData_ID", " = ", ParameterName("meterData_ID"), ""));

                DataRequest request = new DataRequest(sqlBuilder.ToString());
                request.AddParamter(ParameterName("meterData_ID"), meterDataID, DbType.String);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);

            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetRelianceEventDataForTextExport(string meterDataID)", ex);
                throw ex;
            }
            return dataSet;
        }
        // SarkarA code change end 20171206

        public DataSet GetTorrentDataForTextExport(string meterDataID)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder sqlBuilder = new StringBuilder();

                sqlBuilder.AppendFormat(@"use dlms_ltct_650;
                                        Select meterSerialNumber, ReadingDateTime, BillingDate, 'L+G' as Manufacturer, CumulativeEnergykWhTZ0, CumulativeEnergykvarhLag, MDkWTZ0 from 
                                        (Select meterSerialNumber,ReadingDateTime,A.MeterData_ID from meterdata_general as A join meterdata as B on A.MeterData_ID=B.MeterData_ID) as E
	                                        left outer join
	                                        (Select  D.BillingDate, C.CumulativeEnergykWhTZ0,C.CumulativeEnergykvarhLag, D.MDkWTZ0, C.MeterData_ID from meterdata_billing as C 
	                                            left outer join 
                                                (Select BillingDate,MDkWTZ0,MeterData_ID from meterdata_billing where MeterData_ID = {0} ORDER BY BillingDate DESC LIMIT 1,1 )  as D
			                                        on C.MeterData_ID=D.MeterData_ID) as F
		                                        on E.MeterData_ID = F.MeterData_ID
                                        where E.MeterData_ID = {0} LIMIT 1", ParameterName("meterData_ID"));

                DataRequest request = new DataRequest(sqlBuilder.ToString());
                request.AddParamter(ParameterName("meterData_ID"), meterDataID, DbType.String);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);

            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetTorrentDataForTextExport(string meterDataID)", ex);
                throw ex;
            }
            return dataSet;
        }

        public DataSet GetTorrentDataInstantForTextExport(string meterDataID)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder sqlBuilder = new StringBuilder();

                sqlBuilder.Append(@"use dlms_ltct_650;
                                    Select InstantPowerColumnName,InstantPowerColumnValue,InstantPowerObisCode  
                                    from meterdata_instantpower where InstantPowerObisCode in   
                                    ('0.0.0.1.0.255','1.0.5.8.0.255','0.0.96.1.149.255')");
                sqlBuilder.Append(string.Concat(" and MeterData_ID", " = ", ParameterName("meterData_ID"), ""));

                DataRequest request = new DataRequest(sqlBuilder.ToString());
                request.AddParamter(ParameterName("meterData_ID"), meterDataID, DbType.String);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);

            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetTorrentDataInstantForTextExport(string meterDataID)", ex);
                throw ex;
            }
            return dataSet;
        }

        //SarkarA-code-start-20171201
        public DataSet GetTorrentAgraDataForTextExport(string meterDataID)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder sqlBuilder = new StringBuilder();

                sqlBuilder.Append(@"use `dlms_ltct_650`;
                                                                        select  
                                    CUST.Consumer_Number, CUST.MeterID,
                                    CP2M.CM_CumulativeEnergykWhTZ0, CP2M.PM_CumulativeEnergykWhTZ0, CP2M.CM_CumulativeEnergykVahTZ0, CP2M.PM_CumulativeEnergykVAhTZ0, 
                                    Y.CumulativeMDKW, X.CumulativeMDKVA, CP2M.PM_MDkWTZ0, CP2M.PM2_MDkWTZ0, CP2M.PM_MDkVATZ0, CP2M.PM2_MDkVATZ0,
                                    CP2M.CM_BillingDate, CP2M.PM_BillingDate,
                                    Z.InstantReadingDateTime, CUST.UploadingDateTime, CUST.Meter_Location, CUST.Consumer_HNumber,
                                    CP2M.CM_CumulativeEnergykWhTZ1, CP2M.CM_CumulativeEnergykWhTZ2, CP2M.CM_CumulativeEnergykWhTZ3, CP2M.CM_CumulativeEnergykWhTZ4, CP2M.CM_CumulativeEnergykWhTZ5, CP2M.CM_CumulativeEnergykWhTZ6,CP2M.CM_CumulativeEnergykWhTZ7, CP2M.CM_CumulativeEnergykWhTZ8,
                                    CP2M.PM_CumulativeEnergykWhTZ1, CP2M.PM_CumulativeEnergykWhTZ2, CP2M.PM_CumulativeEnergykWhTZ3, CP2M.PM_CumulativeEnergykWhTZ4, CP2M.PM_CumulativeEnergykWhTZ5, CP2M.PM_CumulativeEnergykWhTZ6, CP2M.PM_CumulativeEnergykWhTZ7, CP2M.PM_CumulativeEnergykWhTZ8,
                                    CP2M.CM_CumulativeEnergykVAhTZ1, CP2M.CM_CumulativeEnergykVAhTZ2, CP2M.CM_CumulativeEnergykVAhTZ3, CP2M.CM_CumulativeEnergykVAhTZ4, CP2M.CM_CumulativeEnergykVAhTZ5, CP2M.CM_CumulativeEnergykVAhTZ6, CP2M.CM_CumulativeEnergykVAhTZ7, CP2M.CM_CumulativeEnergykVAhTZ8,
                                    CP2M.PM_CumulativeEnergykVAhTZ1, CP2M.PM_CumulativeEnergykVAhTZ2, CP2M.PM_CumulativeEnergykVAhTZ3, CP2M.PM_CumulativeEnergykVAhTZ4, CP2M.PM_CumulativeEnergykVAhTZ5, CP2M.PM_CumulativeEnergykVAhTZ6, CP2M.PM_CumulativeEnergykVAhTZ7, CP2M.PM_CumulativeEnergykVAhTZ8,
                                    CP2M.PM_MDkWTZ1, CP2M.PM_MDkWTZ2, CP2M.PM_MDkWTZ3, CP2M.PM_MDkWTZ4, CP2M.PM_MDkWTZ5, CP2M.PM_MDkWTZ6, CP2M.PM_MDkWTZ7, CP2M.PM_MDkWTZ8,
                                    CP2M.PM2_MDkWTZ1, CP2M.PM2_MDkWTZ2, CP2M.PM2_MDkWTZ3, CP2M.PM2_MDkWTZ4, CP2M.PM2_MDkWTZ5, CP2M.PM2_MDkWTZ6, CP2M.PM2_MDkWTZ7, CP2M.PM2_MDkWTZ8,
                                    CP2M.PM_MDkVATZ1, CP2M.PM_MDkVATZ2, CP2M.PM_MDkVATZ3, CP2M.PM_MDkVATZ4, CP2M.PM_MDkVATZ5, CP2M.PM_MDkVATZ6, CP2M.PM_MDkVATZ7, CP2M.PM_MDkVATZ8,
                                    CP2M.PM2_MDkVATZ1, CP2M.PM2_MDkVATZ2, CP2M.PM2_MDkVATZ3, CP2M.PM2_MDkVATZ4, CP2M.PM2_MDkVATZ5, CP2M.PM2_MDkVATZ6, CP2M.PM2_MDkVATZ7, CP2M.PM2_MDkVATZ8,
                                    CP2M.CM_CumulativeEnergykvarhLag, CP2M.PM_CumulativeEnergykvarhLag, CP2M.CM_CumulativeEnergykvarhLead, CP2M.PM_CumulativeEnergykvarhLead
                                    from
                                    (select C.MeterData_ID,  C.MeterID, C.ReadingDateTime, C.UploadingDateTime, D.Consumer_Number, D.Consumer_HNumber, D.Meter_Location  
                                    from (select MeterData_ID,MeterID,ReadingDateTime,UploadingDateTime from meterdata) as C
                                    left outer join

                                    (select B.Meter_ID,A.Consumer_Number,A.Consumer_HNumber,B.Meter_Location from consumer_master as A 
                                    left outer join consumermeter as B
                                    on A.Consumer_Number=B.Consumer_Number) as D on C.MeterID = D.Meter_ID) as CUST
                                    left outer join

                                    (select * from
                                    (select 
                                    CM.BillingDate as CM_BillingDate,CM.MeterData_ID as CM_MeterData_ID,
                                    CM.CumulativeEnergykWhTZ0 as CM_CumulativeEnergykWhTZ0,CM.CumulativeEnergykVAhTZ0 as CM_CumulativeEnergykVahTZ0,
                                    CM.MDkWTZ0 as CM_MDkWTZ0,CM.MDkVATZ0 as CM_MDkVATZ0,
                                    CM.CumulativeEnergykWhTZ1 as CM_CumulativeEnergykWhTZ1,CM.CumulativeEnergykWhTZ2 as CM_CumulativeEnergykWhTZ2,CM.CumulativeEnergykWhTZ3 as CM_CumulativeEnergykWhTZ3,CM.CumulativeEnergykWhTZ4 as CM_CumulativeEnergykWhTZ4,CM.CumulativeEnergykWhTZ5 as CM_CumulativeEnergykWhTZ5,CM.CumulativeEnergykWhTZ6 as CM_CumulativeEnergykWhTZ6,CM.CumulativeEnergykWhTZ7 as CM_CumulativeEnergykWhTZ7,CM.CumulativeEnergykWhTZ8 as CM_CumulativeEnergykWhTZ8,
                                    CM.CumulativeEnergykVAhTZ1 as CM_CumulativeEnergykVAhTZ1,CM.CumulativeEnergykVAhTZ2 as CM_CumulativeEnergykVAhTZ2,CM.CumulativeEnergykVAhTZ3 as CM_CumulativeEnergykVAhTZ3,CM.CumulativeEnergykVAhTZ4 as CM_CumulativeEnergykVAhTZ4,CM.CumulativeEnergykVAhTZ5 as CM_CumulativeEnergykVAhTZ5,CM.CumulativeEnergykVAhTZ6 as CM_CumulativeEnergykVAhTZ6,CM.CumulativeEnergykVAhTZ7 as CM_CumulativeEnergykVAhTZ7,CM.CumulativeEnergykVAhTZ8 as CM_CumulativeEnergykVAhTZ8,
                                    CM.CumulativeEnergykvarhLag as CM_CumulativeEnergykvarhLag,CM.CumulativeEnergykvarhLead as CM_CumulativeEnergykvarhLead,

                                    PM.BillingDate as PM_BillingDate,
                                    PM.CumulativeEnergykWhTZ0 as PM_CumulativeEnergykWhTZ0,PM.CumulativeEnergykVAhTZ0 as PM_CumulativeEnergykVAhTZ0,
                                    PM.MDkWTZ0 as PM_MDkWTZ0,PM.MDkVATZ0 as PM_MDkVATZ0,
                                    PM.CumulativeEnergykWhTZ1 as PM_CumulativeEnergykWhTZ1,PM.CumulativeEnergykWhTZ2 as PM_CumulativeEnergykWhTZ2,PM.CumulativeEnergykWhTZ3 as PM_CumulativeEnergykWhTZ3,PM.CumulativeEnergykWhTZ4 as PM_CumulativeEnergykWhTZ4,PM.CumulativeEnergykWhTZ5 as PM_CumulativeEnergykWhTZ5,PM.CumulativeEnergykWhTZ6 as PM_CumulativeEnergykWhTZ6,PM.CumulativeEnergykWhTZ7 as PM_CumulativeEnergykWhTZ7,PM.CumulativeEnergykWhTZ8 as PM_CumulativeEnergykWhTZ8,
                                    PM.CumulativeEnergykVAhTZ1 as PM_CumulativeEnergykVAhTZ1,PM.CumulativeEnergykVAhTZ2 as PM_CumulativeEnergykVAhTZ2,PM.CumulativeEnergykVAhTZ3 as PM_CumulativeEnergykVAhTZ3,PM.CumulativeEnergykVAhTZ4 as PM_CumulativeEnergykVAhTZ4,PM.CumulativeEnergykVAhTZ5 as PM_CumulativeEnergykVAhTZ5,PM.CumulativeEnergykVAhTZ6 as PM_CumulativeEnergykVAhTZ6,PM.CumulativeEnergykVAhTZ7 as PM_CumulativeEnergykVAhTZ7,PM.CumulativeEnergykVAhTZ8 as PM_CumulativeEnergykVAhTZ8,
                                    PM.MDkWTZ1 as PM_MDkWTZ1,PM.MDkWTZ2 as PM_MDkWTZ2,PM.MDkWTZ3 as PM_MDkWTZ3,PM.MDkWTZ4 as PM_MDkWTZ4,PM.MDkWTZ5 as PM_MDkWTZ5,PM.MDkWTZ6 as PM_MDkWTZ6,PM.MDkWTZ7 as PM_MDkWTZ7,PM.MDkWTZ8 as PM_MDkWTZ8,
                                    PM.MDkVATZ1 as PM_MDkVATZ1,PM.MDkVATZ2 as PM_MDkVATZ2,PM.MDkVATZ3 as PM_MDkVATZ3,PM.MDkVATZ4 as PM_MDkVATZ4,PM.MDkVATZ5 as PM_MDkVATZ5,PM.MDkVATZ6 as PM_MDkVATZ6,PM.MDkVATZ7 as PM_MDkVATZ7,PM.MDkVATZ8 as PM_MDkVATZ8,
                                    PM.CumulativeEnergykvarhLag as PM_CumulativeEnergykvarhLag,PM.CumulativeEnergykvarhLead as PM_CumulativeEnergykvarhLead
                                    from
                                    (select BillingDate,MeterData_ID,CumulativeEnergykWhTZ0,CumulativeEnergykVAhTZ0,MDkWTZ0,MDkVATZ0,
                                    CumulativeEnergykWhTZ1,CumulativeEnergykWhTZ2,CumulativeEnergykWhTZ3,CumulativeEnergykWhTZ4,CumulativeEnergykWhTZ5,CumulativeEnergykWhTZ6,CumulativeEnergykWhTZ7,CumulativeEnergykWhTZ8,
                                    CumulativeEnergykVAhTZ1,CumulativeEnergykVAhTZ2,CumulativeEnergykVAhTZ3,CumulativeEnergykVAhTZ4,CumulativeEnergykVAhTZ5,CumulativeEnergykVAhTZ6,CumulativeEnergykVAhTZ7,CumulativeEnergykVAhTZ8,
                                    CumulativeEnergykvarhLag,CumulativeEnergykvarhLead
                                    from meterdata_billing where DataIndex = 0) as CM
                                    left outer join

                                    (select BillingDate,MeterData_ID,CumulativeEnergykWhTZ0,CumulativeEnergykVAhTZ0,MDkWTZ0,MDkVATZ0,
                                    CumulativeEnergykWhTZ1,CumulativeEnergykWhTZ2,CumulativeEnergykWhTZ3,CumulativeEnergykWhTZ4,CumulativeEnergykWhTZ5,CumulativeEnergykWhTZ6,CumulativeEnergykWhTZ7,CumulativeEnergykWhTZ8,
                                    CumulativeEnergykVAhTZ1,CumulativeEnergykVAhTZ2,CumulativeEnergykVAhTZ3,CumulativeEnergykVAhTZ4,CumulativeEnergykVAhTZ5,CumulativeEnergykVAhTZ6,CumulativeEnergykVAhTZ7,CumulativeEnergykVAhTZ8,
                                    MDkWTZ1,MDkWTZ2,MDkWTZ3,MDkWTZ4,MDkWTZ5,MDkWTZ6,MDkWTZ7,MDkWTZ8,
                                    MDkVATZ1,MDkVATZ2,MDkVATZ3,MDkVATZ4,MDkVATZ5,MDkVATZ6,MDkVATZ7,MDkVATZ8,
                                    CumulativeEnergykvarhLag,CumulativeEnergykvarhLead
                                    from meterdata_billing where DataIndex = 1) as PM 
                                    on CM.MeterData_ID = PM.MeterData_ID) as CMPM 

                                    left outer join
                                    (select 
                                    PM2.BillingDate as PM2_BillingDate,MeterData_ID as PM2_MeterData_ID,
                                    PM2.MDkWTZ0 as PM2_MDkWTZ0,PM2.MDkVATZ0 as PM2_MDkVATZ0,
                                    PM2.MDkWTZ1 as PM2_MDkWTZ1,PM2.MDkWTZ2 as PM2_MDkWTZ2,PM2.MDkWTZ3 as PM2_MDkWTZ3,PM2.MDkWTZ4 as PM2_MDkWTZ4,PM2.MDkWTZ5 as PM2_MDkWTZ5,PM2.MDkWTZ6 as PM2_MDkWTZ6,PM2.MDkWTZ7 as PM2_MDkWTZ7,PM2.MDkWTZ8 as PM2_MDkWTZ8,
                                    PM2.MDkVATZ1 as PM2_MDkVATZ1,PM2.MDkVATZ2 as PM2_MDkVATZ2,PM2.MDkVATZ3 as PM2_MDkVATZ3,PM2.MDkVATZ4 as PM2_MDkVATZ4,PM2.MDkVATZ5 as PM2_MDkVATZ5,PM2.MDkVATZ6 as PM2_MDkVATZ6,PM2.MDkVATZ7 as PM2_MDkVATZ7,PM2.MDkVATZ8 as PM2_MDkVATZ8
                                    from meterdata_billing as PM2 where DataIndex = 2) as PM2

                                    on CMPM.CM_MeterData_ID=PM2.PM2_MeterData_ID) as CP2M

                                    on CUST.MeterData_ID=CP2M.CM_MeterData_ID

									left outer join 
									(select InstantPowerColumnValue as InstantReadingDateTime,MeterData_ID as I_MeterData_ID from meterdata_instantpower  where InstantPowerObisCode = '0.0.1.0.0.255') as Z 
                                    on  CUST.MeterData_ID=Z.I_MeterData_ID 
                                    
                                    left outer join 
									(select InstantPowerColumnValue as CumulativeMDKW,MeterData_ID as I_MeterData_ID from meterdata_instantpower  where InstantPowerObisCode = '0.0.1.4.8.255') as Y 
                                    on  CUST.MeterData_ID=Y.I_MeterData_ID 
                                    
                                    left outer join 
									(select InstantPowerColumnValue as CumulativeMDKVA,MeterData_ID as I_MeterData_ID from meterdata_instantpower  where InstantPowerColumnName like 'Cumulative%Active%MD%KVA') as X 
                                    on  CUST.MeterData_ID=X.I_MeterData_ID                                   
                                    ");

                sqlBuilder.Append(string.Concat(" where MeterData_ID", " = ", ParameterName("meterData_ID"), ""));



                DataRequest request = new DataRequest(sqlBuilder.ToString());
                request.AddParamter(ParameterName("meterData_ID"), meterDataID, DbType.String);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);

            }
            catch (CABException ex)
            {
                throw ex;
            }
            return dataSet;
        }
        //SarkarA-code-end-20171201

        public override IEntity RowToEntity(DataRow row)
        {
            throw new NotImplementedException();
        }

        public DataSet GetTataPowerAdaniBillingData(string meterDataID)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder sqlBuilder = new StringBuilder();

                sqlBuilder.Append(@"SELECT * FROM `dlms_ltct_650`.`meterdata_billing` ");

                sqlBuilder.Append(string.Concat(" where MeterData_ID", " = ", ParameterName("meterData_ID"), ""));

                DataRequest request = new DataRequest(sqlBuilder.ToString());
                request.AddParamter(ParameterName("meterData_ID"), meterDataID, DbType.String);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);

            }
            catch (CABException ex)
            {
                throw ex;
            }
            return dataSet;
        }

        public DataSet GetTataPowerAdaniInstantData(string meterDataID)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder sqlBuilder = new StringBuilder();

                sqlBuilder.Append(@"SELECT * FROM `dlms_ltct_650`.`meterdata_instantpower` ");

                sqlBuilder.Append(string.Concat(" where MeterData_ID", " = ", ParameterName("meterData_ID"), ""));

                DataRequest request = new DataRequest(sqlBuilder.ToString());
                request.AddParamter(ParameterName("meterData_ID"), meterDataID, DbType.String);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);

            }
            catch (CABException ex)
            {
                throw ex;
            }
            return dataSet;
        }

        public DataSet GetTataPowerAdaniGeneralData(string meterDataID)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder sqlBuilder = new StringBuilder();

                sqlBuilder.Append(@"SELECT * FROM 
                                `dlms_ltct_650`.`meterdata` A  left join `dlms_ltct_650`.`meterdata_general` B  
                                on A.MeterData_ID = B.MeterData_ID 
                                left join `dlms_ltct_650`.`meterdata_anomaly` C
                                on A.MeterData_ID = C.MeterDataId  ");

                sqlBuilder.Append(string.Concat(" where A.MeterData_ID", " = ", ParameterName("meterData_ID"), ""));

                DataRequest request = new DataRequest(sqlBuilder.ToString());
                request.AddParamter(ParameterName("meterData_ID"), meterDataID, DbType.String);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);

            }
            catch (CABException ex)
            {
                throw ex;
            }
            return dataSet;
        }

        public DataSet GetTataPowerAdaniTamperData(string meterDataID)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder sqlBuilder = new StringBuilder();

                sqlBuilder.Append(@"use dlms_ltct_650;
                                    select EventCode, count(EventCode) from 
                                    tampertype_master  A 
                                    inner join tamper_master  B on
                                    A.TamperTypeID=B.EventCode   
                                    join 
                                    meterdata D on B.MeterData_ID=D.MeterData_ID ");

                sqlBuilder.Append(string.Concat(" where D.MeterData_ID", " = ", ParameterName("meterData_ID"), ""));

                sqlBuilder.Append(@" and EventCode%2!=0
                                    group by EventCode
                                 ");

                DataRequest request = new DataRequest(sqlBuilder.ToString());
                request.AddParamter(ParameterName("meterData_ID"), meterDataID, DbType.String);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);

            }
            catch (CABException ex)
            {
                throw ex;
            }
            return dataSet;
        }

        public DataSet GetTataPowerAdaniLoadSurveyData(string meterDataID)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder sqlBuilder = new StringBuilder();

                sqlBuilder.Append(@"SELECT Min(realTimeClockDateandTime) as InitialLoadSurveyDate,Max(realTimeClockDateandTime) as FinalLoadSurveyDate FROM `dlms_ltct_650`.`meterdata_loadsurvey`
                                 ");

                sqlBuilder.Append(string.Concat(" where MeterData_ID", " = ", ParameterName("meterData_ID"), ""));

                DataRequest request = new DataRequest(sqlBuilder.ToString());
                request.AddParamter(ParameterName("meterData_ID"), meterDataID, DbType.String);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);

            }
            catch (CABException ex)
            {
                throw ex;
            }
            return dataSet;
        }

        #endregion
    }
}
