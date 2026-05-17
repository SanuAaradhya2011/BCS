using System;
using System.Data;
using System.Text;
using CAB.DALC.Data.DataServices;
using CAB.Entity;
using CAB.Framework;
using CAB.Framework.Entity;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Hunt.EPIC.Logging;

namespace CAB.DALC.Data
{
    public class ConsumerMeterDAL : DALBase
    {
        private string Meter_ID = "Meter_ID";
        private string Consumer_Number = "Consumer_Number";
        private string Meter_AllocationDate = "Meter_AllocationDate";
        private string Meter_Location = "Meter_Location";
        private string Status = "Status";
        private string Region_ID = "Region_ID";
        private string Circle_ID = "Circle_ID";
        private string Division_ID = "Division_ID";
        private string Communication_Type = "Communication_Type";
        private string Meter_Phone = "Meter_Phone";
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(ConsumerMeterDAL).ToString());

        /// <summary>
        /// Overload to support the existing 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override IEntity InsertData(IEntity entity)
        {
            return InsertData(entity, false);
        }

        /// <summary>
        /// GPRS: Overload the method to support GPRS bulk upload specific changes
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="isBulkUpload"></param>
        /// <returns></returns>
        public IEntity InsertData(IEntity entity, bool isBulkUpload)
        {
            ConsumerMeterEntity consumerMeterEntity = null;
            if (entity == null)
                return consumerMeterEntity;
            consumerMeterEntity = entity as ConsumerMeterEntity;
            bool Flag = false;
            //Fix defect #165662
            bool isGPRSEndPoint = !string.IsNullOrEmpty(consumerMeterEntity.Communcation_Type)
                                  && consumerMeterEntity.Communcation_Type.ToString().ToUpper() == CommunicationType.GPRS.ToString();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert Into consumermeter(Meter_ID,Consumer_Number,Meter_AllocationDate,Meter_Location,Status,Region_ID,Circle_ID,Division_ID,");

                if (isGPRSEndPoint && isBulkUpload)
                {
                    builder.Append("IsSyncedWithGPRSAdapter,");
                }

                builder.Append("Communication_Type) values(");
                builder.Append(string.Concat(ParameterName(Meter_ID), ","));
                builder.Append(string.Concat(ParameterName(Consumer_Number), ","));
                builder.Append(string.Concat(ParameterName(Meter_AllocationDate), ","));
                builder.Append(string.Concat(ParameterName(Meter_Location), ","));
                //if (ConfigInfo.GetTenderType() == TenderType.JUSCO)
                //{
                builder.Append(string.Concat(ParameterName(Status), ","));
                builder.Append(string.Concat(ParameterName(Region_ID), ","));
                builder.Append(string.Concat(ParameterName(Circle_ID), ","));
                builder.Append(string.Concat(ParameterName(Division_ID), ","));

                if (isGPRSEndPoint && isBulkUpload)
                {
                    builder.Append(string.Concat(ParameterName("IsSyncedWithGPRSAdapter"), ","));
                }
                builder.Append(string.Concat(ParameterName(Communication_Type), ")"));
                //}
                //else
                //    builder.Append(string.Concat(ParameterName(Status), ")"));

                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Meter_ID), consumerMeterEntity.Meter_ID, DbType.String, 20);
                request.AddParamter(ParameterName(Consumer_Number), consumerMeterEntity.Consumer_Number, DbType.String, 20);
                request.AddParamter(ParameterName(Meter_AllocationDate), consumerMeterEntity.Meter_AllocationDate, DbType.Int64);
                request.AddParamter(ParameterName(Meter_Location), consumerMeterEntity.Meter_Location, DbType.String, 20);
                request.AddParamter(ParameterName(Status), consumerMeterEntity.Status, DbType.Int32);
                //if (ConfigInfo.GetTenderType() == TenderType.JUSCO)
                //{
                request.AddParamter(ParameterName(Region_ID), consumerMeterEntity.Region_ID, DbType.Int32);
                request.AddParamter(ParameterName(Circle_ID), consumerMeterEntity.Circle_ID, DbType.Int32);
                request.AddParamter(ParameterName(Division_ID), consumerMeterEntity.Division_ID, DbType.Int32);

                if (isGPRSEndPoint && isBulkUpload)
                {
                    request.AddParamter(ParameterName("IsSyncedWithGPRSAdapter"), 0, DbType.Boolean);
                }
                consumerMeterEntity.Communcation_Type = !string.IsNullOrEmpty(consumerMeterEntity.Communcation_Type) ? consumerMeterEntity.Communcation_Type.Trim().ToUpper() : consumerMeterEntity.Communcation_Type;
                request.AddParamter(ParameterName(Communication_Type), consumerMeterEntity.Communcation_Type, DbType.String);
                //}
                helper.ExecuteNonQuery(request);
                Flag = true;
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("New consumer added"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "InsertData(IEntity entity, bool isBulkUpload)", ex);
                consumerMeterEntity = null;
            }
            return consumerMeterEntity;
        }

        //Consumer Meter only the Date can be updated the other fields should be deleted since the Meter ID and Consumer Number
        //once entered cannot be modified.
        public override bool UpdateData(IEntity entity)
        {
            bool Flag = false;
            try
            {
                ConsumerMeterEntity consumerMeterEntity = entity as ConsumerMeterEntity;
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Update consumermeter Set ");
                builder.Append(string.Concat(Meter_AllocationDate, "=", ParameterName(Meter_AllocationDate), ","));
                builder.Append(string.Concat(Meter_Location, "=", ParameterName(Meter_Location), ","));
                builder.Append(string.Concat(Consumer_Number, "=", ParameterName(Consumer_Number), ","));

                //if (ConfigInfo.GetTenderType() == TenderType.JUSCO)
                //{
                builder.Append(string.Concat(Status, "=", ParameterName(Status), ","));
                builder.Append(string.Concat(Region_ID, "=", ParameterName(Region_ID), ","));
                builder.Append(string.Concat(Circle_ID, "=", ParameterName(Circle_ID), ","));
                builder.Append(string.Concat(Division_ID, "=", ParameterName(Division_ID), ","));
                builder.Append(string.Concat(Communication_Type, "=", ParameterName(Communication_Type)));
                //}
                //else
                //    builder.Append(string.Concat(Status, "=", ParameterName(Status)));
                builder.Append(string.Concat(" Where ", Meter_ID, "=", ParameterName(Meter_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Meter_AllocationDate), consumerMeterEntity.Meter_AllocationDate, DbType.Int64);
                request.AddParamter(ParameterName(Meter_Location), consumerMeterEntity.Meter_Location, DbType.String, 20);
                request.AddParamter(ParameterName(Consumer_Number), consumerMeterEntity.Consumer_Number, DbType.String, 20);
                request.AddParamter(ParameterName(Status), consumerMeterEntity.Status, DbType.Int32);
                request.AddParamter(ParameterName(Region_ID), consumerMeterEntity.Region_ID, DbType.Int32);
                request.AddParamter(ParameterName(Circle_ID), consumerMeterEntity.Circle_ID, DbType.Int32);
                request.AddParamter(ParameterName(Division_ID), consumerMeterEntity.Division_ID, DbType.Int32);
                request.AddParamter(ParameterName(Communication_Type), consumerMeterEntity.Communcation_Type, DbType.String, 20);
                request.AddParamter(ParameterName(Meter_ID), consumerMeterEntity.Meter_ID, DbType.String, 20);
                helper.ExecuteNonQuery(request);
                Flag = true;
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Consumer data upadted"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "UpdateData(IEntity entity)", ex);
                Flag = false;
            }
            return Flag;
        }

        public override bool DeleteData(IEntity entity)
        {
            bool Flag = false;
            try
            {
                ConsumerMeterEntity consumerMeterEntity = entity as ConsumerMeterEntity;
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete from consumermeter where ");
                builder.Append(string.Concat(Consumer_Number, "=", ParameterName(Consumer_Number), " and "));
                builder.Append(string.Concat(Meter_ID, "=", ParameterName(Meter_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Consumer_Number), consumerMeterEntity.Consumer_Number, DbType.String, 20);
                request.AddParamter(ParameterName(Meter_ID), consumerMeterEntity.Meter_ID, DbType.String, 20);
                helper.ExecuteNonQuery(request);
                Flag = true;
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Consumer data deleted"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "DeleteData(IEntity entity)", ex);
                Flag = false;
            }
            return Flag;
        }

        public bool DeleteData(string consumerNumber)
        {
            bool Flag = false;
            try
            {

                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete from consumermeter where ");
                builder.Append(string.Concat(Consumer_Number, "=", ParameterName(Consumer_Number)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Consumer_Number), consumerNumber, DbType.String, 20);
                helper.ExecuteNonQuery(request);
                Flag = true;
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Consumer data deleted"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "DeleteData(string consumerNumber)", ex);
                Flag = false;
            }
            return Flag;
        }

        public bool GetConsumerCount(string consumerID)
        {
            bool Flag = false;

            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select count(*) from consumermeter where ");
                builder.Append(string.Concat(Consumer_Number, "=", ParameterName(Consumer_Number)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Consumer_Number), consumerID, DbType.String, 20);
                object data = helper.ExecuteScalar(request);
                if (Convert.ToInt32(data.ToString()) > 0)
                {
                    Flag = true;
                }
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("The meter count for a specified consumer retrieved"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetConsumerCount(string consumerID)", ex);
                Flag = false;
            }
            return Flag;
        }

        public override IEntity GetDetailData(int id)
        {
            ConsumerMeterEntity consumerMeterEntity = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select Meter_ID,Consumer_Number,Meter_AllocationDate from consumermeter where ");
                builder.Append(string.Concat(Consumer_Number, "=", ParameterName(Consumer_Number)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Consumer_Number), id, DbType.String, 20);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    consumerMeterEntity = (ConsumerMeterEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Data for a specified consumer retrieved"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetDetailData(int id)", ex);
                consumerMeterEntity = null;
            }
            return consumerMeterEntity;
        }

        public IEntity GetDetailData(string consumer_Number, string meter_ID)
        {
            ConsumerMeterEntity consumerMeterEntity = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select Meter_ID,Consumer_Number,Meter_AllocationDate,Meter_Location,Status");
                //if (ConfigInfo.GetTenderType() == TenderType.JUSCO)
                builder.Append(",Region_ID,Circle_ID,Division_ID,Communication_Type");
                builder.Append(" from consumermeter where ");
                builder.Append(string.Concat(Consumer_Number, "=", ParameterName(Consumer_Number), " and "));
                builder.Append(string.Concat(Meter_ID, "=", ParameterName(Meter_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Consumer_Number), consumer_Number, DbType.String, 20);
                request.AddParamter(ParameterName(Meter_ID), meter_ID, DbType.String, 20);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds != null)
                    if (ds.Tables.Count > 0)
                        if (ds.Tables[0].Rows.Count > 0)
                            consumerMeterEntity = (ConsumerMeterEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Data for a specified consumer retrieved"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetDetailData(string consumer_Number, string meter_ID)", ex);
                consumerMeterEntity = null;
            }
            return consumerMeterEntity;
        }

        public DataSet ListActiveMeterID(string consumerNumber)
        {
            DataSet ds = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                //if Meter_Status = 0 then the Inactive Meters are selected.Here the consumer Meter table and consumer_master tables are not included.
                builder.Append("select Meter_ID from consumermeter where Status = 1 and ");
                builder.Append(string.Concat(Consumer_Number, "=", ParameterName(Consumer_Number)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Consumer_Number), consumerNumber, DbType.String, 20);
                ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter ID for a specified consumer retrieved"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ListActiveMeterID(string consumerNumber)", ex);
                ds = null;
            }
            return ds;
        }

        /// <summary>
        /// GPRS: Made changes to support GPRS also
        /// </summary>
        /// <param name="divisionID"></param>
        /// <param name="commType"></param>
        /// <returns></returns>
        public DataSet GetMetersbyDivisionID(long groupid, int divisionID, string commType)
        {
            DataSet ds = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                //if Meter_Status = 0 then the Inactive Meters are selected.Here the consumer Meter table and consumer_master tables are not included.
                //BhardwajG : If the communication type is GSM or PSTN

                builder.Append("Select m.Meter_ID from meter_master m join consumermeter c on m.Meter_ID = c.Meter_ID where c.Communication_Type='" + commType.ToUpper() + "' and Division_ID = " + divisionID + " and m.meter_ID not in(select meter_id from gsm_group_meters where group_id=" + groupid + " and status='S')");

                DataRequest request = new DataRequest(builder.ToString());
                ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetMetersbyDivisionID(long groupid, int divisionID, string commType)", ex);
                ds = null;
            }
            return ds;
        }


        /// <summary>
        /// Method to retrieve meters for the division and communication type
        /// </summary>
        /// <param name="divisionID"></param>
        /// <param name="commType"></param>
        /// <returns></returns>
        public DataSet GetMetersbyDivisionID(int divisionID, string commType)
        {
            DataSet ds = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                //if Meter_Status = 0 then the Inactive Meters are selected.Here the consumer Meter table and consumer_master tables are not included.
                //BhardwajG : If the communication type is GSM or PSTN

                builder.Append("Select m.Meter_ID from meter_master m join consumermeter c on m.Meter_ID = c.Meter_ID where c.Communication_Type='" + commType.ToUpper() + "' and Division_ID = " + divisionID);

                DataRequest request = new DataRequest(builder.ToString());
                ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetMetersbyDivisionID(int divisionID, string commType)", ex);
                ds = null;
            }
            return ds;
        }


        public DataSet ListInactiveMeterID()
        {
            DataSet ds = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                //if Meter_Status = 0 then the Inactive Meters are selected.Here the consumer Meter table and consumer_master tables are not included.
                builder.Append("select Meter_ID from meter_master where Meter_Status = 0 and Meter_ID not in (select Meter_ID from ConsumerMeter where status = 1) ");
                builder.Append("union select MeterId from meterdata where MeterID not in (select Meter_ID from ConsumerMeter where status = 1)");
                DataRequest request = new DataRequest(builder.ToString());
                ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("List of Inactive meter ID's retrieved"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ListInactiveMeterID()", ex);
                ds = null;
            }
            return ds;
        }

        public override System.Collections.Generic.IList<IEntity> ListData()
        {
            throw new System.NotImplementedException();
        }

        public override DataSet ListDataSet()
        {
            DataSet ds = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("SELECT Consumer_Master.Consumer_Number, Consumer_Master.Consumer_Name, Consumer_Master.ConsumerType_ID, Consumer_Master.Consumer_Phone, Consumer_Master.Consumer_HNumber, Consumer_Master.Consumer_Street," +
                    "Consumer_Master.Consumer_City,");
                //if (ConfigInfo.GetTenderType() == TenderType.JUSCO)
                //{
                builder.Append("region_master.Region_Name,circle_master.Circle_Name,division_master.division_name,consumerMeter.communication_Type,");
                //}
                builder.Append("consumerMeter.Region_ID,consumerMeter.Circle_ID,consumerMeter.Division_ID,consumerMeter.communiation_Type, Consumer_Master.Consumer_Email, Meter_Master.Meter_ID,Meter_Master.MeterType_ID,Meter_Master.MeterModel_ID,Meter_Master.Meter_EMF, " +
                "Meter_Master.Meter_ContractDemand,Meter_Master.MeterUnit_ID,Meter_Master.Meter_CTPrimary,Meter_Master.Meter_CTSecondary,Meter_Master.Meter_PTPrimary,Meter_Master.Meter_PTSecondary,Meter_Master.Meter_InstalledCTPrimary," +
                "Meter_Master.Meter_InstalledCTSecondary,Meter_Master.Meter_InstalledPTPrimary,Meter_Master.Meter_InstalledPTSecondary,Meter_Master.Meter_Phone,Meter_Master.Meter_Status,consumerMeter.Meter_AllocationDate,ConsumerMeter.Meter_Location,ConsumerMeter.Status" +
                " FROM (Consumer_Master INNER JOIN ConsumerMeter ON Consumer_Master.Consumer_Number = ConsumerMeter.Consumer_Number) INNER JOIN Meter_Master ON ConsumerMeter.Meter_ID = Meter_Master.Meter_ID");
                //if (ConfigInfo.GetTenderType() == TenderType.JUSCO)
                //{
                builder.Append(" LEFT OUTER JOIN region_master ON consumermeter.Region_ID = region_master.Region_ID ");
                builder.Append("LEFT OUTER JOIN circle_master ON consumermeter.Circle_ID = circle_master.Circle_ID ");
                builder.Append("LEFT OUTER JOIN division_master on consumermeter.Division_ID = division_master.Division_ID");
                //}
                DataRequest request = new DataRequest(builder.ToString());
                ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Consumer data viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ListDataSet()", ex);
                ds = null;
            }
            return ds;
        }

        public override IEntity RowToEntity(DataRow row)
        {
            if (row == null) return null;
            ConsumerMeterEntity consumerMeterEntity = new ConsumerMeterEntity();

            if (NotNullAndNotDBNull(row, Meter_ID)) consumerMeterEntity.Meter_ID = Convert.ToString(row[Meter_ID]);
            if (NotNullAndNotDBNull(row, Consumer_Number)) consumerMeterEntity.Consumer_Number = Convert.ToString(row[Consumer_Number]);
            if (NotNullAndNotDBNull(row, Meter_AllocationDate)) consumerMeterEntity.Meter_AllocationDate = Convert.ToInt64(row[Meter_AllocationDate]);
            if (NotNullAndNotDBNull(row, Meter_Location)) consumerMeterEntity.Meter_Location = Convert.ToString(row[Meter_Location]);
            if (NotNullAndNotDBNull(row, Status)) consumerMeterEntity.Status = Convert.ToInt32(row[Status]);
            //if (ConfigInfo.GetTenderType() == TenderType.JUSCO)
            //{
            if (NotNullAndNotDBNull(row, Region_ID)) consumerMeterEntity.Region_ID = Convert.ToInt32(row[Region_ID]);
            if (NotNullAndNotDBNull(row, Circle_ID)) consumerMeterEntity.Circle_ID = Convert.ToInt32(row[Circle_ID]);
            if (NotNullAndNotDBNull(row, Division_ID)) consumerMeterEntity.Division_ID = Convert.ToInt32(row[Division_ID]);
            if (NotNullAndNotDBNull(row, Communication_Type)) consumerMeterEntity.Communcation_Type = Convert.ToString(row[Communication_Type]);
            //}
            return consumerMeterEntity;
        }

        public bool GetDataAvailability(IEntity entity)
        {
            bool Flag = false;
            try
            {
                ConsumerMeterEntity consumerMeterEntity = entity as ConsumerMeterEntity;
                IDataHelper helper = DatabaseFactory.GetHelper();

                StringBuilder builder = new StringBuilder();
                builder.Append("select count(*) recNo from consumermeter where ");
                //builder.Append(string.Concat(Consumer_Number, "=", ParameterName(Consumer_Number), " and "));
                builder.Append(string.Concat(Meter_ID, "=", ParameterName(Meter_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                //request.AddParamter(ParameterName(Consumer_Number), consumerMeterEntity.Consumer_Number, DbType.String, 20);
                request.AddParamter(ParameterName(Meter_ID), consumerMeterEntity.Meter_ID, DbType.String, 20);
                object data = helper.ExecuteScalar(request);

                if (Convert.ToInt32(data.ToString()) > 0)
                {
                    return Flag = true;
                }
                return Flag;
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Consumer data viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetDataAvailability(IEntity entity)", ex);
                Flag = false;
            }
            return Flag;
        }

        public bool GetConsumerMeterAvailability(IEntity entity)
        {
            bool Flag = false;
            try
            {
                ConsumerMeterEntity consumerMeterEntity = entity as ConsumerMeterEntity;
                IDataHelper helper = DatabaseFactory.GetHelper();

                StringBuilder builder = new StringBuilder();
                builder.Append("select count(*) recNo from consumermeter where ");
                builder.Append(string.Concat(Consumer_Number, "=", ParameterName(Consumer_Number), " and "));
                builder.Append(string.Concat(Meter_ID, "=", ParameterName(Meter_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Consumer_Number), consumerMeterEntity.Consumer_Number, DbType.String, 20);
                request.AddParamter(ParameterName(Meter_ID), consumerMeterEntity.Meter_ID, DbType.String, 20);
                object data = helper.ExecuteScalar(request);

                if (Convert.ToInt32(data.ToString()) > 0)
                {
                    return Flag = true;
                }
                return Flag;
                //UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Consumer data viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetConsumerMeterAvailability(IEntity entity)", ex);
                Flag = false;
            }
            return Flag;
        }

        public DataSet ComboList(bool isConsumer)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                if (isConsumer)
                    builder.Append("Select distinct Consumer_Number from consumermeter order by Consumer_Number desc");
                else
                    builder.Append("Select distinct Meter_Location from consumermeter order by Meter_Location desc");
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("File viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ComboList(bool isConsumer)", ex);
            }
            return dataSet;
        }
        public DataSet ListDataSet(string readingType, string value)
        {
            DataSet slData = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                DataRequest request = null;
                if (readingType.Equals("CN"))
                {
                    builder.Append("Select Meter_ID,Meter_Location,Meter_AllocationDate from consumermeter where ");
                    builder.Append(string.Concat(Consumer_Number, "=", ParameterName(Consumer_Number)));
                    request = new DataRequest(builder.ToString());
                    request.AddParamter(ParameterName(Consumer_Number), value, DbType.String, 40);
                }
                if (readingType.Equals("L"))
                {
                    builder.Append("Select Meter_ID,Consumer_Number,Meter_AllocationDate from consumermeter where ");
                    builder.Append(string.Concat(Meter_Location, "=", ParameterName(Meter_Location)));
                    request = new DataRequest(builder.ToString());
                    request.AddParamter(ParameterName(Meter_Location), value, DbType.String, 40);
                }
                DataSet dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                slData = new DataSet();
                slData.Tables.Add(AutoNumberedTable(dataSet.Tables[0]));
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data viewed."));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ListDataSet(string readingType, string value)", ex);
            }
            return slData;
        }

        public int GetSIMNumber(string meterNumber)
        {
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("select Meter_Phone from meter_master where ");
                builder.Append(string.Concat(Meter_ID, "=", ParameterName(Meter_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Meter_ID), meterNumber, DbType.Int32);
                object data = helper.ExecuteScalar(request);
                return Convert.ToInt32(data);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Group data viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetSIMNumber(string meterNumber)", ex);
                return 0;
            }
        }
        public DataSet GetActiveMeterList()
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                //BhardwajG : If the communication type is GSM or PSTN
                builder.Append("select Meter_ID from consumermeter where Communication_Type in ('" + CommunicationType.GSM.ToString() + "','" + CommunicationType.PSTN.ToString() + "'");

                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);

                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Group data viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetActiveMeterList()", ex);
                return dataSet;
            }
            return dataSet;
        }

        public override IEntity InsertData(System.Collections.Generic.IList<IEntity> entities)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// bulk insert with origional => GPRS: Overload the method to support GPRS bulk upload specific changes
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="isBulkUpload"></param>
        /// <returns></returns>
        public  void BatchData(System.Collections.Generic.IList<IEntity> entities, bool isBulkUpload)
        {

         // List<DataRequest> requests = new List<DataRequest>();
          IDataHelper helper = DatabaseFactory.GetHelper();
            //bool Flag = false;
            //Fix defect #165662
           // bool isGPRSEndPoint;
            StringBuilder builder = new StringBuilder();
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("Meter_ID", typeof(System.String)));
            table.Columns.Add(new DataColumn("Consumer_Number", typeof(System.String)));
            table.Columns.Add(new DataColumn("Meter_AllocationDate", typeof(System.Int64)));
            table.Columns.Add(new DataColumn("Meter_Location", typeof(System.String)));
            table.Columns.Add(new DataColumn("Status", typeof(System.Int32)));
            table.Columns.Add(new DataColumn("Region_ID", typeof(System.Int32)));
            table.Columns.Add(new DataColumn("Circle_ID", typeof(System.Int32)));
            table.Columns.Add(new DataColumn("Division_ID", typeof(System.Int32)));
            table.Columns.Add(new DataColumn("Communication_Type", typeof(System.String)));
            try
            {
                foreach (IEntity entity in entities)
                {
                    ConsumerMeterEntity consumerMeterEntity = entity as ConsumerMeterEntity;
                    DataRow dr = table.NewRow();
                    dr["Meter_ID"] = consumerMeterEntity.Meter_ID;
                    dr["Consumer_Number"] = consumerMeterEntity.Consumer_Number;
                    dr["Meter_AllocationDate"] = consumerMeterEntity.Meter_AllocationDate;
                    dr["Meter_Location"] = consumerMeterEntity.Meter_Location;
                    dr["Status"] = consumerMeterEntity.Status;
                    dr["Region_ID"] = consumerMeterEntity.Region_ID;
                    dr["Circle_ID"] = consumerMeterEntity.Circle_ID;
                    dr["Division_ID"] = consumerMeterEntity.Division_ID;
                    consumerMeterEntity.Communcation_Type = !string.IsNullOrEmpty(consumerMeterEntity.Communcation_Type) ? consumerMeterEntity.Communcation_Type.Trim().ToUpper() : consumerMeterEntity.Communcation_Type;
                    dr["Communication_Type"] = consumerMeterEntity.Communcation_Type;
                    table.Rows.Add(dr);

                }

                //isGPRSEndPoint = !string.IsNullOrEmpty(consumerMeterEntity.Communcation_Type)
                //                  && consumerMeterEntity.Communcation_Type.ToString().ToUpper() == CommunicationType.GPRS.ToString();



                builder.Append("Insert Into consumermeter(Meter_ID,Consumer_Number,Meter_AllocationDate,Meter_Location,Status,Region_ID,Circle_ID,Division_ID,");

                //if (isGPRSEndPoint && isBulkUpload)
                //{
                //    builder.Append("IsSyncedWithGPRSAdapter,");
                //}

                builder.Append("Communication_Type) values(");
                builder.Append(string.Concat(ParameterName(Meter_ID), ","));
                builder.Append(string.Concat(ParameterName(Consumer_Number), ","));
                builder.Append(string.Concat(ParameterName(Meter_AllocationDate), ","));
                builder.Append(string.Concat(ParameterName(Meter_Location), ","));
                //if (ConfigInfo.GetTenderType() == TenderType.JUSCO)
                //{
                builder.Append(string.Concat(ParameterName(Status), ","));
                builder.Append(string.Concat(ParameterName(Region_ID), ","));
                builder.Append(string.Concat(ParameterName(Circle_ID), ","));
                builder.Append(string.Concat(ParameterName(Division_ID), ","));

                //if (isGPRSEndPoint && isBulkUpload)
                //{
                //    builder.Append(string.Concat(ParameterName("IsSyncedWithGPRSAdapter"), ","));
                //}
                builder.Append(string.Concat(ParameterName(Communication_Type), ")"));
                //}
                //else
                //    builder.Append(string.Concat(ParameterName(Status), ")"));

                //DataRequest request = new DataRequest(builder.ToString());
                //request.AddParamter(ParameterName(Meter_ID), consumerMeterEntity.Meter_ID, DbType.String, 20);
                //request.AddParamter(ParameterName(Consumer_Number), consumerMeterEntity.Consumer_Number, DbType.String, 20);
                //request.AddParamter(ParameterName(Meter_AllocationDate), consumerMeterEntity.Meter_AllocationDate, DbType.Int64);
                //request.AddParamter(ParameterName(Meter_Location), consumerMeterEntity.Meter_Location, DbType.String, 20);
                //request.AddParamter(ParameterName(Status), consumerMeterEntity.Status, DbType.Int32);
                ////if (ConfigInfo.GetTenderType() == TenderType.JUSCO)
                ////{
                //request.AddParamter(ParameterName(Region_ID), consumerMeterEntity.Region_ID, DbType.Int32);
                //request.AddParamter(ParameterName(Circle_ID), consumerMeterEntity.Circle_ID, DbType.Int32);
                //request.AddParamter(ParameterName(Division_ID), consumerMeterEntity.Division_ID, DbType.Int32);
                MySqlCommand command = new MySqlCommand(builder.ToString());
                command.CommandType = CommandType.Text;
                command.Parameters.Add("?Meter_ID", MySqlDbType.String).SourceColumn = "Meter_ID";
                command.Parameters.Add("?Consumer_Number", MySqlDbType.String).SourceColumn = "Consumer_Number";
                command.Parameters.Add("?Meter_AllocationDate", MySqlDbType.Int64).SourceColumn = "Meter_AllocationDate";
                command.Parameters.Add("?Meter_Location", MySqlDbType.String).SourceColumn = "Meter_Location";
                command.Parameters.Add("?Status", MySqlDbType.Int32).SourceColumn = "Status";
                command.Parameters.Add("?Region_ID", MySqlDbType.Int32).SourceColumn = "Region_ID";
                command.Parameters.Add("?Circle_ID", MySqlDbType.Int32).SourceColumn = "Circle_ID";
                command.Parameters.Add("?Division_ID", MySqlDbType.Int32).SourceColumn = "Division_ID";
                command.Parameters.Add("?Communication_Type", MySqlDbType.String).SourceColumn = "Communication_Type";


                command.UpdatedRowSource = UpdateRowSource.None;

                //if (isGPRSEndPoint && isBulkUpload)
                //{
                //    request.AddParamter(ParameterName("IsSyncedWithGPRSAdapter"), 0, DbType.Boolean);
                //}
                // request.AddParamter(ParameterName(Communication_Type), consumerMeterEntity.Communcation_Type, DbType.String);
                //}
                // requests.Add(request);  
                helper.BatchInsert(table, command);
                //IDataHelper helper = DatabaseFactory.GetHelper();
                //helper.ExecuteNonQuery(requests);
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "BatchData(System.Collections.Generic.IList<IEntity> entities, bool isBulkUpload)", ex);
            }
          
        }


        /// <summary>
        /// This function retrieves the consumer numbers associated with a particular phone number from the database.
        /// </summary>
        /// <param name="meterNumber"></param>
        /// <returns>Theresulting consumer number</returns>
        public string GetConsumerNumber(long meterNumber)
        {
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("select c.Consumer_Number from consumermeter c,meter_master m where ");
                builder.Append(string.Concat("m.", Meter_ID, "=", "c.", Meter_ID, " and "));
                builder.Append(string.Concat("m.", Meter_Phone, "=", ParameterName(Meter_Phone)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Meter_Phone), meterNumber, DbType.Int64);
                object data = helper.ExecuteScalar(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Consumer number retrieved."));
                return Convert.ToString(data);
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetConsumerNumber(long meterNumber)", ex);
                return String.Empty;
            }
        }


        /// <summary>
        /// Method the detetmine whether passed meter exists into system or not
        /// </summary>
        /// <param name="meterNumber"></param>
        /// <param name="commType"></param>
        /// <returns></returns>
        public DataSet IsMeterExists(string meterNumber, string commType)
        {
            DataSet dataSet = null;

            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("select Meter_ID from consumermeter where Meter_ID='" + meterNumber + "' and Communication_Type ='" + commType + "'");
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);

            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "IsMeterExists(string meterNumber, string commType)", ex);
                dataSet = null;
            }

            return dataSet;

        }

    }
}
