
/* 
* |----------------------------------------------------------------------------------------------------------------|
* |											All rights reserved to Cabcon Technologies 	 								|
* | 																												|
* |											Author : Piyush Singh. 	 								|
* | 																												|
* |----------------------------------------------------------------------------------------------------------------| 
*/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using CAB.DALC.Data.DataServices;
using CAB.Entity;
using CAB.Framework;
using CAB.Framework.Entity;
using CAB.Framework.Utility;
using Hunt.EPIC.Logging;

namespace CAB.DALC.Data
{
    public class DLMS650TamperMasterDAL : DALBase
    {
        private string Tamper_ID = "Tamper_ID";
        private string DateTimeEvent = "DateTimeEvent";
        private string EventCode = "EventCode";
        private string CurrentIR = "CurrentIR";
        private string CurrentIY = "CurrentIY";
        private string CurrentIB = "CurrentIB";
        private string PhaseCurrent = "PhaseCurrent";
        private string VoltageVRN = "VoltageVRN";
        private string VoltageVYN = "VoltageVYN";
        private string VoltageVBN = "VoltageVBN";
        private string PhaseVoltage = "PhaseVoltage";
        private string PowerFactorRPhase = "PowerFactorRPhase";
        private string PowerFactorYPhase = "PowerFactorYPhase";
        private string PowerFactorBPhase = "PowerFactorBPhase";
        private string TotalPowerFactor = "TotalPowerFactor";
        private string CumulativeEnergykWh = "CumulativeEnergykWh";
        private string CumulativeEnergykVAh = "CumulativeEnergykVAh";
        private string CompartmentNumber = "CompartmentNumber";
        private string MeterData_ID = "MeterData_ID";
        private string NeutralCurrent = "NeutralCurrent";
        private string ByPassCurrent = "ByPassCurrent";  
        private string HighNeutralCurrent = "HighNeutralCurrent";
        private string Temprature = "Temprature";

        private string kWr = "kWr";
        private string kWy = "kWy";
        private string kWb = "kWb";

        private string kVAr = "kVAr";
        private string kVAy = "kVAy";
        private string kVAb = "kVAb";
        private string CumuTampercount = "CumulativeTamperCount";//smart meter


        // SB Code Change Start 20171116
        private string ActiveCurrentR = "ActiveCurrentR";
        private string ActiveCurrentY = "ActiveCurrentY";
        private string ActiveCurrentB = "ActiveCurrentB";
        // SB Code Change End 20171116
        private string MeterID = "MeterID";
        private string FileName = "FileName";
        private string CumulativeEnergykvarhLag = "CumulativeEnergykvarhLag";
        private string CumulativeEnergykvarhLead = "CumulativeEnergykvarhLead";
        // Net Metering new Parameters
        private string CumulativeEnergykWhImport = "CumulativeEnergykWhImport";
        private string CumulativeEnergykVAhImport = "CumulativeEnergykVAhImport";
        private string CumulativeEnergykWhExport = "CumulativeEnergykWhExport";
        private string CumulativeEnergykVAhExport = "CumulativeEnergykVAhExport";
        //private string kWhAbsolute = "kWhAbsolute";
        //private string kVAhAbsolute = "kVAhAbsolute";

        //SarkarA code change start 20180330 // add phase current instant, frequency
        private string Frequency = "Frequency";
        private string PhaseCurrentInstant = "PhaseCurrentInstant";
        //SarkarA code change end 20180330
        private string THDVR = "THDVR";
        private string THDVY = "THDVY";
        private string THDVB = "THDVB";
        private string THDIR = "THDIR";
        private string THDIY = "THDIY";
        private string THDIB = "THDIB";
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(DLMS650TamperMasterDAL).ToString());

        public DLMS650TamperMasterDAL()
            : base("Tamper_master", "Tamper_ID")
        {
        }

        private DataRequest GetRequest(IEntity entity)
        {
            if (entity == null)
                return null;
            DLMS650TamperEntity dLMS650TamperEntity = entity as DLMS650TamperEntity;
            StringBuilder builder = new StringBuilder();
            builder.Append("Insert Into Tamper_Master(DateTimeEvent,EventCode,CurrentIR,CurrentIY,CurrentIB,PhaseCurrent,VoltageVRN,VoltageVYN,VoltageVBN,PhaseVoltage,PowerFactorRPhase,PowerFactorYPhase,PowerFactorBPhase,Temprature,TotalPowerFactor,CumulativeEnergykWh,CumulativeEnergykWhImport,CumulativeEnergykWhExport,CumulativeEnergykVAh,CumulativeEnergykVAhImport,CumulativeEnergykVAhExport,CumulativeEnergykvarhLag,CumulativeEnergykvarhLead, CompartmentNumber,MeterData_ID,NeutralCurrent,ByPassCurrent,ActiveCurrentR,ActiveCurrentY,ActiveCurrentB,HighNeutralCurrent,kWr,kWy,kWb,kVAr,kVAy,kVAb,CumulativeTamperCount,Frequency,PhaseCurrentInstant,THDVR,THDVY,THDVB,THDIR,THDIY,THDIB) values("); // SB Code Change Start/End - 20171116 - Columns added //SarkarA code change start 20180330 // add phase current instant, frequency/end
            builder.Append(string.Concat(ParameterName(DateTimeEvent), ","));
            builder.Append(string.Concat(ParameterName(EventCode), ","));
            builder.Append(string.Concat(ParameterName(CurrentIR), ","));
            builder.Append(string.Concat(ParameterName(CurrentIY), ","));
            builder.Append(string.Concat(ParameterName(CurrentIB), ","));
            builder.Append(string.Concat(ParameterName(PhaseCurrent), ","));
            builder.Append(string.Concat(ParameterName(VoltageVRN), ","));
            builder.Append(string.Concat(ParameterName(VoltageVYN), ","));
            builder.Append(string.Concat(ParameterName(VoltageVBN), ","));
            builder.Append(string.Concat(ParameterName(PhaseVoltage), ","));
            builder.Append(string.Concat(ParameterName(PowerFactorRPhase), ","));
            builder.Append(string.Concat(ParameterName(PowerFactorYPhase), ","));
            builder.Append(string.Concat(ParameterName(PowerFactorBPhase), ","));
            builder.Append(string.Concat(ParameterName(Temprature), ","));
            builder.Append(string.Concat(ParameterName(TotalPowerFactor), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykWh), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykWhImport), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykWhExport), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykVAh), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykVAhImport), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykVAhExport), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykvarhLag), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykvarhLead), ",")); 
            builder.Append(string.Concat(ParameterName(CompartmentNumber), ","));
            builder.Append(string.Concat(ParameterName(MeterData_ID), ","));
            // SB Code Change Start 20171116
            builder.Append(string.Concat(ParameterName(NeutralCurrent), ","));
            builder.Append(string.Concat(ParameterName(ByPassCurrent), ","));

            builder.Append(string.Concat(ParameterName(ActiveCurrentR), ","));
            builder.Append(string.Concat(ParameterName(ActiveCurrentY), ","));
            builder.Append(string.Concat(ParameterName(ActiveCurrentB), ","));

            builder.Append(string.Concat(ParameterName(HighNeutralCurrent), ","));

            builder.Append(string.Concat(ParameterName(kWr), ","));
            builder.Append(string.Concat(ParameterName(kWy), ","));
            builder.Append(string.Concat(ParameterName(kWb), ","));


            builder.Append(string.Concat(ParameterName(kVAr), ","));
            builder.Append(string.Concat(ParameterName(kVAy), ","));
            builder.Append(string.Concat(ParameterName(kVAb), ","));
            builder.Append(string.Concat(ParameterName(CumuTampercount), ","));//smart meter
            
            // SB Code Change End 20171116

            //SarkarA code change start 20180330 // add phase current instant, frequency
            builder.Append(string.Concat(ParameterName(Frequency), ","));
            builder.Append(string.Concat(ParameterName(PhaseCurrentInstant), ","));
            builder.Append(string.Concat(ParameterName(THDVR), ","));
            builder.Append(string.Concat(ParameterName(THDVY), ","));
            builder.Append(string.Concat(ParameterName(THDVB), ","));
            builder.Append(string.Concat(ParameterName(THDIR), ","));
            builder.Append(string.Concat(ParameterName(THDIY), ","));
            builder.Append(string.Concat(ParameterName(THDIB), ")"));            
            //SarkarA code change end 20180330
            DataRequest request = new DataRequest(builder.ToString());
            request.AddParamter(ParameterName(DateTimeEvent), dLMS650TamperEntity.DateTimeEvent, DbType.Int64);
            request.AddParamter(ParameterName(EventCode), dLMS650TamperEntity.EventCode, DbType.Int64);
            request.AddParamter(ParameterName(CurrentIR), dLMS650TamperEntity.CurrentIR, DbType.String, 40);
            request.AddParamter(ParameterName(CurrentIY), dLMS650TamperEntity.CurrentIY, DbType.String, 40);
            request.AddParamter(ParameterName(CurrentIB), dLMS650TamperEntity.CurrentIB, DbType.String, 40);
            request.AddParamter(ParameterName(PhaseCurrent), dLMS650TamperEntity.PhaseCurrent, DbType.String, 40);
            request.AddParamter(ParameterName(VoltageVRN), dLMS650TamperEntity.VoltageVRN, DbType.String, 40);
            request.AddParamter(ParameterName(VoltageVYN), dLMS650TamperEntity.VoltageVYN, DbType.String, 40);
            request.AddParamter(ParameterName(VoltageVBN), dLMS650TamperEntity.VoltageVBN, DbType.String, 40);
            request.AddParamter(ParameterName(PhaseVoltage), dLMS650TamperEntity.PhaseVoltage, DbType.String, 40);
            request.AddParamter(ParameterName(PowerFactorRPhase), dLMS650TamperEntity.PowerFactorRphase, DbType.String, 40);
            request.AddParamter(ParameterName(PowerFactorYPhase), dLMS650TamperEntity.PowerFactorYphase, DbType.String, 40);
            request.AddParamter(ParameterName(PowerFactorBPhase), dLMS650TamperEntity.PowerFactorBphase, DbType.String, 40);
            request.AddParamter(ParameterName(Temprature), dLMS650TamperEntity.Temprature, DbType.String, 40);
            request.AddParamter(ParameterName(TotalPowerFactor), dLMS650TamperEntity.TotalPowerFactor, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykWh), dLMS650TamperEntity.CumulativeEnergykWh, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykWhImport), dLMS650TamperEntity.CumulativeEnergykWhImport, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykWhExport), dLMS650TamperEntity.CumulativeEnergykWhExport, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykVAh), dLMS650TamperEntity.CumulativeEnergykVAh, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykVAhImport), dLMS650TamperEntity.CumulativeEnergykVAhImport, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykVAhExport), dLMS650TamperEntity.CumulativeEnergykVAhExport, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykvarhLag), dLMS650TamperEntity.CumulativeEnergykvarhLag, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykvarhLead), dLMS650TamperEntity.CumulativeEnergykvarhLead, DbType.String, 40);         
            request.AddParamter(ParameterName(CompartmentNumber), dLMS650TamperEntity.CompartmentNumber, DbType.Int64);
            request.AddParamter(ParameterName(MeterData_ID), dLMS650TamperEntity.MeterData_ID, DbType.Int64);
            request.AddParamter(ParameterName(NeutralCurrent), dLMS650TamperEntity.NeutralCurrent, DbType.String, 40);
            request.AddParamter(ParameterName(ByPassCurrent), dLMS650TamperEntity.ByPassCurrent, DbType.String, 40);

            // SB Code Change Start 20171116
            request.AddParamter(ParameterName(ActiveCurrentR), dLMS650TamperEntity.ActiveCurrentR, DbType.String, 40);
            request.AddParamter(ParameterName(ActiveCurrentY), dLMS650TamperEntity.ActiveCurrentY, DbType.String, 40);
            request.AddParamter(ParameterName(ActiveCurrentB), dLMS650TamperEntity.ActiveCurrentB, DbType.String, 40);
            // SB Code Change End 20171116

            request.AddParamter(ParameterName(HighNeutralCurrent), dLMS650TamperEntity.HighNeutralCurrent, DbType.String, 45);//add pradipta_neu

            request.AddParamter(ParameterName(kWr), dLMS650TamperEntity.kWr, DbType.String, 40);//pradipta_neu
            request.AddParamter(ParameterName(kWy), dLMS650TamperEntity.kWy, DbType.String, 40);//pradipta_neu
            request.AddParamter(ParameterName(kWb), dLMS650TamperEntity.kWb, DbType.String, 40);//pradipta_neu

            request.AddParamter(ParameterName(kVAr), dLMS650TamperEntity.kVAr, DbType.String, 40);//pradipta_neu
            request.AddParamter(ParameterName(kVAy), dLMS650TamperEntity.kVAy, DbType.String, 40);//pradipta_neu
            request.AddParamter(ParameterName(kVAb), dLMS650TamperEntity.kVAb, DbType.String, 40);//pradipta_neu
            request.AddParamter(ParameterName(CumuTampercount), dLMS650TamperEntity.CumulativeTampercount, DbType.String, 40);//smart meter

            //SarkarA code change start 20180330 // add phase current instant, frequency
            request.AddParamter(ParameterName(Frequency), dLMS650TamperEntity.Frequency, DbType.String, 40);
            request.AddParamter(ParameterName(PhaseCurrentInstant), dLMS650TamperEntity.PhaseCurrentInstant, DbType.String, 40);
            //SarkarA code change end 20180330
            request.AddParamter(ParameterName(THDVR), dLMS650TamperEntity.THDVR, DbType.String, 40);
            request.AddParamter(ParameterName(THDVY), dLMS650TamperEntity.THDVY, DbType.String, 40);
            request.AddParamter(ParameterName(THDVB), dLMS650TamperEntity.THDVB, DbType.String, 40);
            request.AddParamter(ParameterName(THDIR), dLMS650TamperEntity.THDIR, DbType.String, 40);
            request.AddParamter(ParameterName(THDIY), dLMS650TamperEntity.THDIY, DbType.String, 40);
            request.AddParamter(ParameterName(THDIB), dLMS650TamperEntity.THDIB, DbType.String, 40);          
            return request;
        }

        public override IEntity InsertData(IEntity entity)
        {
            DLMS650TamperEntity tamperEntity = entity as DLMS650TamperEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                DataRequest request = this.GetRequest(entity);
                helper.ExecuteNonQuery(request);
                Flag = true;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "InsertData(IEntity entity)", ex);
            }
            if (Flag)
                tamperEntity.Tamper_ID = long.Parse(this.GetPK());
            return tamperEntity;
        }
        public override IEntity InsertData(IList<IEntity> entities)
        {
            List<DataRequest> requests = new List<DataRequest>();
            foreach (IEntity entity in entities)
                requests.Add(this.GetRequest(entity));
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                helper.ExecuteNonQuery(requests);
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "IEntity InsertData(IList<IEntity> entities)", ex);
            }
            return null;
        }

        public DataSet ListDataSet(string eventCode, string meterDataID)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select A.Tamper_ID,B.TamperType as Description ,A.DateTimeEvent as 'Time Stamp',A.EventCode");
                builder.Append(" from Tamper_Master A,TamperType_Master B");
                builder.Append(" where B.TamperTypeID=A.EventCode and ");
                builder.Append(string.Concat("A.", MeterData_ID, "=", ParameterName(MeterData_ID)));
                builder.Append(string.Concat(" and A.", EventCode, "=", ParameterName(EventCode)));
                builder.Append(" order by Tamper_ID asc");
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataID, DbType.Int64);
                request.AddParamter(ParameterName(EventCode), eventCode, DbType.Int64);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ListDataSet(string eventCode, string meterDataID)", ex);
            }
            return dataSet;
        }

        public DataSet ListEventCodeORData(string eventCode1,string eventCode2, string meterDataID)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select A.Tamper_ID,B.TamperType as Description ,A.DateTimeEvent as 'Time Stamp',A.EventCode");
                builder.Append(" from Tamper_Master A,TamperType_Master B");
                builder.Append(" where B.TamperTypeID=A.EventCode and ");
                builder.Append(string.Concat("A.", MeterData_ID, "=", ParameterName(MeterData_ID)));
                builder.Append(string.Concat(" and (A.", EventCode, "=", ParameterName("EventCode1")));
                builder.Append(string.Concat(" or A.", EventCode, "=", ParameterName("EventCode2")));

                builder.Append(") order by Tamper_ID asc");
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataID, DbType.Int64);
                request.AddParamter("EventCode1", eventCode1, DbType.Int64);
                request.AddParamter("EventCode2", eventCode2, DbType.Int64);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ListEventCodeORData(string eventCode1,string eventCode2, string meterDataID)", ex);
            }
            return dataSet;
        }



        public DataSet ListAllTamperEventCode()
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select * from tampertype_Master");
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ListAllTamperEventCode()", ex);
            }
            return dataSet;
        }

        public DataSet ListDataSetWithColumns(long tamperId, long meterDataId, string tamperColumnParameters)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select distinct ");
                builder.Append(tamperColumnParameters);
                builder.Append(" from tamper_master where ");
                builder.Append(string.Concat(Tamper_ID, "=", ParameterName(Tamper_ID), " and "));
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                builder.Append(string.Concat(" order by DateTimeEvent"));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataId, DbType.Int64);
                request.AddParamter(ParameterName(Tamper_ID), tamperId, DbType.Int64);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Tamper data viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ListDataSetWithColumns(long tamperId, long meterDataId, string tamperColumnParameters)", ex);
                dataSet = null;
            }
            return dataSet;
        }

        public DataSet DetailData(long tamperID, long meterDataID)
        {
            DataSet dataSet = new DataSet();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select CurrentIR,");
                builder.Append("CurrentIY,CurrentIB,VoltageVRN,VoltageVYN,VoltageVBN,");
                builder.Append("PowerFactorRphase,PowerFactorYphase,PowerFactorBphase,TotalPowerFactor,CumulativeEnergykWh,CumulativeEnergykVAh from tamper_master where ");
                builder.Append(string.Concat(Tamper_ID, "=", ParameterName(Tamper_ID), " and "));
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Tamper_ID), tamperID, DbType.Int64);
                request.AddParamter(ParameterName(MeterData_ID), meterDataID, DbType.Int64);
                dataSet = helper.FillDataSet(request, dataSet);
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "DetailData(long tamperID, long meterDataID)", ex);
            }
            return dataSet;
        }

        public DataSet DetailData(long meterDataID)
        {
            DataSet dataSet = new DataSet();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select EventCode,DateTimeEvent,CurrentIR,");
                builder.Append("CurrentIY,CurrentIB,VoltageVRN,VoltageVYN,VoltageVBN,");
                builder.Append("PowerFactorRphase,PowerFactorYphase,PowerFactorBphase,CumulativeEnergykWh from tamper_master where CompartmentNumber=4 and ");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataID, DbType.Int64);
                dataSet = helper.FillDataSet(request, dataSet);
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "DetailData(long meterDataID)", ex);
            }
            return dataSet;
        }
        public DataSet DetailTransactionData(long meterDataID)
        {
            DataSet dataSet = new DataSet();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select a.EventCode,a.DateTimeEvent,a.CurrentIR,");
                builder.Append("a.CurrentIY,a.CurrentIB,a.PhaseCurrent,a.VoltageVRN,a.VoltageVYN,a.VoltageVBN,a.PhaseVoltage,");
                builder.Append("a.PowerFactorRphase,a.PowerFactorYphase,a.PowerFactorBphase,a.CumulativeEnergykWh,b.TamperType,CumulativeEnergykVAh, TotalPowerFactor from tamper_master a join tampertype_master b on a.EventCode = b.TamperTypeID where b.Compartment=4 and ");
                builder.Append(string.Concat("a.", MeterData_ID, "=", ParameterName(MeterData_ID)));
                // Added to solve bug 95346.
                // builder.Append(" order by a.DateTimeEvent desc");// Code changed to consider hours and min in sorting. 

                //Code changes to fix Fast downloading sorting issue.
                builder.Append(" order by a.Tamper_ID asc");

                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataID, DbType.Int64);
                dataSet = helper.FillDataSet(request, dataSet);
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "DetailTransactionData(long meterDataID)", ex);
            }
            return dataSet;
        }

        public DataSet AllDetailData(long meterDataID)
        {
            DataSet dataSet = new DataSet();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select a.EventCode,a.DateTimeEvent,a.CurrentIR,");
                builder.Append("a.CurrentIY,a.CurrentIB,a.PhaseCurrent,a.VoltageVRN,a.VoltageVYN,a.VoltageVBN,a.PhaseVoltage,");

                //SarkarA code change start 20180110 // Restore 1P Tamper Report // move new columns to end //SarkarA code change start 20180330 // add phase current instant, frequency
                builder.Append("a.PowerFactorRphase,a.PowerFactorYphase,a.PowerFactorBphase,a.TotalPowerFactor,a.CumulativeEnergykWh,a.CumulativeEnergykVAh,a.NeutralCurrent,a.ByPassCurrent, a.CumulativeEnergykvarhLag,a.CumulativeEnergykvarhLead, a.CumulativeEnergykWhImport, a.CumulativeEnergykWhExport,a.CumulativeEnergykVAhImport, a.CumulativeEnergykVAhExport,a.HighNeutralCurrent,a.kWr,a.kWy,a.kWb,a.kVAr,a.kVAy,a.kVAb,a.CumulativeTamperCount, a.ActiveCurrentR, a.ActiveCurrentY, a.ActiveCurrentB, a.Frequency, a.PhaseCurrentInstant,a.Temprature,a.THDVR,a.THDVY,a.THDVB,a.THDIR,a.THDIY,a.THDIB from dlms_ltct_650.tamper_master a join dlms_ltct_650.tampertype_master b on a.EventCode = b.TamperTypeID where b.Compartment != 4 and ");
                //SarkarA code change end 20180110

                builder.Append(string.Concat("a.", MeterData_ID, "=", ParameterName(MeterData_ID)));
                // added order by with datetime to solve bug 87492.
                //builder.Append(" order by a.EventCode,a.DateTimeEvent desc"); 

                //Code changes to fix fastdownload sorting issues
                builder.Append(" order by a.EventCode,a.Tamper_ID asc");

                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataID, DbType.Int64);
                dataSet = helper.FillDataSet(request, dataSet);
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, " AllDetailData(long meterDataID)", ex);
            }
            return dataSet;
        }

        public DataSet GetTamperSnapshotData(string meterID, List<string> columns, int tamperCode)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select m.MeterID, f.FileName, m.ReadingDateTime ");
                foreach (string column in columns)
                {
                    builder.Append(string.Concat(",", "ts.", column, " "));
                }
                builder.Append(",m.MeterData_ID from tamper_master ts inner join meterdata m on ts.MeterData_ID = m.MeterData_ID ");
                builder.Append("inner join fileupload_master f on m.FileUpload_ID = f.FileUpload_ID where ");
                builder.Append(string.Concat("ts.", EventCode, "=", ParameterName(EventCode), " ", "and", " "));
                builder.Append(string.Concat("m.", MeterID, "=", ParameterName(MeterID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(EventCode), tamperCode, DbType.Int32);
                request.AddParamter(ParameterName(MeterID), meterID, DbType.String, 20);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data Tamper Snapshot viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetTamperSnapshotData(string meterID, List<string> columns, int tamperCode)", ex);
            }
            return dataSet;
        }

        public DataSet GetTamperSnapshotDataByFileName(string meterID, string fileName, List<string> columns, int tamperCode)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select m.MeterID, f.FileName, m.ReadingDateTime ");
                foreach (string column in columns)
                {
                    builder.Append(string.Concat(",", "ts.", column, " "));
                }
                builder.Append(",m.MeterData_ID from tamper_master ts inner join meterdata m on ts.MeterData_ID = m.MeterData_ID ");
                builder.Append("inner join fileupload_master f on m.FileUpload_ID = f.FileUpload_ID where ");
                builder.Append(string.Concat("ts.", EventCode, "=", ParameterName(EventCode), " ", "and", " "));
                builder.Append(string.Concat("m.", MeterID, "=", ParameterName(MeterID)));
                builder.Append(string.Concat(" ", "and", " ", "f.", FileName, "=", ParameterName(FileName)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(EventCode), tamperCode, DbType.Int32);
                request.AddParamter(ParameterName(MeterID), meterID, DbType.String, 20);
                request.AddParamter(ParameterName(FileName), fileName, DbType.String, 150);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data Tamper Snapshot viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetTamperSnapshotDataByFileName(string meterID, string fileName, List<string> columns, int tamperCode)", ex);
            }
            return dataSet;
        }

        public DataSet GetTransactionSnapshotData(string meterID, List<string> columns, int tamperCode)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select m.MeterID, f.FileName, m.ReadingDateTime ");
                foreach (string column in columns)
                {
                    builder.Append(string.Concat(",", "ts.", column, " "));
                }
                builder.Append(",m.MeterData_ID from tamper_master ts inner join meterdata m on ts.MeterData_ID = m.MeterData_ID ");
                builder.Append("inner join fileupload_master f on m.FileUpload_ID = f.FileUpload_ID where ");
                builder.Append(string.Concat("ts.", EventCode, "=", ParameterName(EventCode), " ", "and", " "));
                builder.Append(string.Concat("m.", MeterID, "=", ParameterName(MeterID), " ", "and", " "));
                builder.Append("CompartmentNumber=4");
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(EventCode), tamperCode, DbType.Int32);
                request.AddParamter(ParameterName(MeterID), meterID, DbType.String, 20);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data Tamper Snapshot viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetTransactionSnapshotData(string meterID, List<string> columns, int tamperCode)", ex);
            }
            return dataSet;
        }

        public DataSet GetTransactionSnapshotDataByFileName(string meterID, string fileName, List<string> columns, int tamperCode)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select m.MeterID, f.FileName, m.ReadingDateTime ");
                foreach (string column in columns)
                {
                    builder.Append(string.Concat(",", "ts.", column, " "));
                }
                builder.Append(",m.MeterData_ID from tamper_master ts inner join meterdata m on ts.MeterData_ID = m.MeterData_ID ");
                builder.Append("inner join fileupload_master f on m.FileUpload_ID = f.FileUpload_ID where ");
                builder.Append(string.Concat("ts.", EventCode, "=", ParameterName(EventCode), " ", "and", " "));
                builder.Append(string.Concat("m.", MeterID, "=", ParameterName(MeterID), " ", "and", " "));
                builder.Append("CompartmentNumber=4");
                builder.Append(string.Concat(" ", "and", " ", "f.", FileName, "=", ParameterName(FileName)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(EventCode), tamperCode, DbType.Int32);
                request.AddParamter(ParameterName(MeterID), meterID, DbType.String, 20);
                request.AddParamter(ParameterName(FileName), fileName, DbType.String, 150);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data Tamper Snapshot viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetTransactionSnapshotDataByFileName(string meterID, string fileName, List<string> columns, int tamperCode)", ex);
            }
            return dataSet;
        }

        public override bool UpdateData(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public bool DeleteData(long meterDataID)
        {
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete from tamper_master where ");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataID, DbType.Int32);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data deleted"));
                Flag = true;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "DeleteData(long meterDataID)", ex);
            }
            return Flag;
        }

        /* VBM Added for New tamper Report */
        /// <summary>
        /// Author :VBM
        /// Gets meterDataId and Returns Start and End Date of tamper occurence.
        /// </summary>
        /// <param name="meterDataID"></param>
        /// <returns></returns>
        public DataSet GetTamperStartEndDate(long meterDataID)
        {
            DataSet dataSet = new DataSet();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("GetTamperStartEndDate");
                DataRequest request = new DataRequest(builder.ToString(), CommandType.StoredProcedure);
                request.AddParamter(ParameterName(MeterData_ID), meterDataID, DbType.Int32);
                dataSet = helper.FillDataSet(request, dataSet);
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetTamperStartEndDate(long meterDataID)", ex);
            }
            return dataSet;
        }
        /// <summary>
        /// Author :VBM
        /// Gets meterDataId and Returns Distinct Tampers and no of times each on eo them has occured
        /// </summary>
        /// <param name="meterDataID"></param>
        /// <returns></returns>
        public DataSet GetTampersAndCount(long meterDataID)
        {
            DataSet dataSet = new DataSet();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("GetTamperTypeWithCount");
                DataRequest request = new DataRequest(builder.ToString(), CommandType.StoredProcedure);
                request.AddParamter(ParameterName(MeterData_ID), meterDataID, DbType.Int32, 20);
                dataSet = helper.FillDataSet(request, dataSet);
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetTampersAndCount(long meterDataID)", ex);
            }
            return dataSet;
        }

        /// <summary>
        /// Author : VBM
        /// Takes tamper Id as paremeter and returns Detail of that particular tamper 
        /// </summary>
        /// <param name="meterDataID"></param>
        /// <param name="tamperId"></param>
        /// <returns></returns>
        public DataSet GetTamperDetailByTamperId(long meterDataID, int tamperId, long formDate, long toDate)
        {
            DataSet dataSet = new DataSet();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("GetTamperDetailByTamperType");
                DataRequest request = new DataRequest(builder.ToString(), CommandType.StoredProcedure);
                request.AddParamter(ParameterName(EventCode), tamperId, DbType.Int32);
                request.AddParamter(ParameterName(MeterData_ID), meterDataID, DbType.Int32, 20);
                request.AddParamter(ParameterName("FromDate"), formDate, DbType.Int64, 20);
                request.AddParamter(ParameterName("ToDate"), toDate, DbType.Int64, 20);
                dataSet = helper.FillDataSet(request, dataSet);
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetTamperDetailByTamperId(long meterDataID, int tamperId, long formDate, long toDate)", ex);
            }
            return dataSet;
        }
        /// <summary>
        /// BhardwajG : Get the tamper details by date range
        /// </summary>
        /// <param name="meterDataID"></param>
        /// <param name="formDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public DataSet GetTamperDetailByDateRange(int meterDataID, long formDate, long toDate)
        {
            DataSet dataSet = new DataSet();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("GetTamperDetailByDates");
                DataRequest request = new DataRequest(builder.ToString(), CommandType.StoredProcedure);
                request.AddParamter(ParameterName(MeterData_ID), meterDataID, DbType.Int32, 20);
                request.AddParamter(ParameterName("FromDate"), formDate, DbType.Int64, 20);
                request.AddParamter(ParameterName("ToDate"), toDate, DbType.Int64, 20);
                dataSet = helper.FillDataSet(request, dataSet);
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetTamperDetailByDateRange(int meterDataID, long formDate, long toDate)", ex);
            }
            return dataSet;
        }

        /// <summary>
        /// Get the tamper details by date range based on compartment ID
        /// </summary>
        /// <param name="meterDataID"></param>
        /// <param name="formDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public DataSet GetTamperDetailByDateRangeWithCompartmentID(int meterDataID, long formDate, long toDate, string compartmentNo)
        {
            DataSet dataSet = new DataSet();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("GetTamperDetailByDatesWithCompartmentID");
                DataRequest request = new DataRequest(builder.ToString(), CommandType.StoredProcedure);
                request.AddParamter(ParameterName(MeterData_ID), meterDataID, DbType.Int32, 20);
                request.AddParamter(ParameterName("FromDate"), formDate, DbType.Int64, 20);
                request.AddParamter(ParameterName("ToDate"), toDate, DbType.Int64, 20);
                request.AddParamter(ParameterName(CompartmentNumber), compartmentNo, DbType.String);
                dataSet = helper.FillDataSet(request, dataSet);
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetTamperDetailByDateRangeWithCompartmentID(int meterDataID, long formDate, long toDate, string compartmentNo)", ex);
            }
            return dataSet;
        }

        /// <summary>
        /// Used to get number of transactions for for a meter dataif at a specific time.
        /// </summary>
        /// <param name="meterDataId"></param>
        /// <param name="dateTimeEvent"></param>
        /// <returns></returns>
        public int GetTransactionCountFromTimeStamp(long meterDataId, string dateTimeEvent)
        {
            object count = 0;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select Count(1) from tamper_master where ");
                builder.Append(string.Concat(DateTimeEvent, "=", ParameterName(DateTimeEvent), " and "));
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(DateTimeEvent), dateTimeEvent, DbType.String);
                request.AddParamter(ParameterName(MeterData_ID), meterDataId, DbType.Int64);
                count = helper.ExecuteScalar(request);
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetTransactionCountFromTimeStamp(long meterDataId, string dateTimeEvent)", ex);
            }
            return Convert.ToInt32(count);
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

        public override bool DeleteData(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public DataTable DetailDataForAutomationExport(long meterDataID, string parameters, Dictionary<string, string> dictTamperCodeAndAbbreviation)
        {
            DataSet dataSet = null;
            try
            {
                parameters = "EventCode," + "DateTimeEvent," + parameters;
                parameters = UpdateColumnNames(parameters);
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select distinct ");
                builder.Append(parameters);
                builder.Append(" from tamper_master where ");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataID, DbType.Int64);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Tamper data viewed"));
                UpdateDataSet(dataSet, dictTamperCodeAndAbbreviation);


            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "DetailDataForAutomationExport(long meterDataID, string parameters, Dictionary<string, string> dictTamperCodeAndAbbreviation)", ex);
                dataSet = null;
            }
            return dataSet.Tables[0];
        }
        private void UpdateDataSet(DataSet dataSet, Dictionary<string, string> dictTamperCodeAndAbbreviation)
        {
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                DataColumn Col = dataSet.Tables[0].Columns.Add("Type");
                Col.SetOrdinal(0);

                foreach (DataRow dr in dataSet.Tables[0].Rows)
                {
                    dr["Type"] = dictTamperCodeAndAbbreviation[Convert.ToString(dr["ID"])];
                    dr["Date & Time (0.0.1.0.0.255;8;2)"] = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64((dr["Date & Time (0.0.1.0.0.255;8;2)"])));
                }
            }            
        }

        private string UpdateColumnNames(string parameters)
        {

            string convertedColumns = "*";
            try
            {               
                string[] lstColumnNames = parameters.Split(',');
                if (lstColumnNames != null)
                {
                    convertedColumns = "";
                    for (int i = 0; i < lstColumnNames.Length; i++)
                    {
                        string MappedValue = GetMappedValue(lstColumnNames[i]);
                        if (MappedValue != string.Empty)
                        {
                            convertedColumns += MappedValue;
                            if (i != (lstColumnNames.Length - 1))
                            {
                                convertedColumns += ",";
                            }
                        }
                    }
                }

            //string convertedColumns = "*";
            //try
            //{
            //    parameters = parameters.Replace("EventCode", "EventCode as 'ID'");
            //    parameters = parameters.Replace("DateTimeEvent", "CAST(DateTimeEvent as char(16)) as 'Date & Time (0.0.1.0.0.255;8;2)'");
            //    parameters = parameters.Replace("CurrentIR", "CurrentIR as 'Ir (A)(1.0.31.7.0.255)'");
            //    parameters = parameters.Replace("CurrentIY", "CurrentIY as 'Iy (A)(1.0.51.7.0.255)'");
            //    parameters = parameters.Replace("CurrentIB", "CurrentIB as 'Ib (A)(1.0.71.7.0.255)'");

            //    parameters = parameters.Replace("VoltageVRN", "VoltageVRN as 'Vr (V)(1.0.32.7.0.255)'");
            //    parameters = parameters.Replace("VoltageVYN", "VoltageVYN as 'Vy (V)(1.0.52.7.0.255)'");
            //    parameters = parameters.Replace("VoltageVBN", "VoltageVBN as 'Vb (V)(1.0.72.7.0.255)'");

            //    parameters = parameters.Replace("PowerFactorRphase", "PowerFactorRphase as 'PFr(1.0.33.7.0.255)'");
            //    parameters = parameters.Replace("PowerFactorYphase", "PowerFactorYphase as 'Pfy(1.0.53.7.0.255)'");
            //    parameters = parameters.Replace("PowerFactorBphase", "PowerFactorBphase as 'PFb(1.0.73.7.0.255)'");

            //    parameters = parameters.Replace("CumulativeEnergykWh", "CumulativeEnergykWh as 'kWh(1.0.1.8.0.255)'");
            //    parameters = parameters.Replace("CumulativeEnergykVAh", "CumulativeEnergykVAh as 'kVAh(1.0.9.8.0.255)'");

            //    convertedColumns = parameters;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "UpdateColumnNames(string parameters)", ex);
            }

            return convertedColumns;
        }

        private string GetMappedValue(string Value)
        {
            Dictionary<string, string> dicMatchString = new Dictionary<string, string>();
            dicMatchString.Add("EventCode", "EventCode as 'ID'");
            dicMatchString.Add("DateTimeEvent", "CAST(DateTimeEvent as char(16)) as 'Date & Time (0.0.1.0.0.255;8;2)'");
            dicMatchString.Add("cumEnergykVAh", "cumEnergykVAh as 'Cumulative Energy - {0}VAh (1.0.9.8.0.255;3;2)'");
            dicMatchString.Add("CurrentIR", "CurrentIR as 'Ir (A)(1.0.31.7.0.255)'");
            dicMatchString.Add("CurrentIY", "CurrentIY as 'Iy (A)(1.0.51.7.0.255)'");
            dicMatchString.Add("CurrentIB", "CurrentIB as 'Ib (A)(1.0.71.7.0.255)'");
            dicMatchString.Add("VoltageVRN", "VoltageVRN as 'Vr (V)(1.0.32.7.0.255)'");
            dicMatchString.Add("VoltageVYN", "VoltageVYN as 'Vy (V)(1.0.52.7.0.255)'");
            dicMatchString.Add("VoltageVBN", "VoltageVBN as 'Vb (V)(1.0.72.7.0.255)'");
            dicMatchString.Add("PowerFactorRphase", "PowerFactorRphase as 'PFr(1.0.33.7.0.255)'");
            dicMatchString.Add("PowerFactorYphase", "PowerFactorYphase as 'Pfy(1.0.53.7.0.255)'");
            dicMatchString.Add("PowerFactorBphase", "PowerFactorBphase as 'PFb(1.0.73.7.0.255)'");
            dicMatchString.Add("CumulativeEnergykWh", "CumulativeEnergykWh as 'kWh(1.0.1.8.0.255)'");
            dicMatchString.Add("CumulativeEnergykVAh", "CumulativeEnergykVAh as 'kVAh(1.0.9.8.0.255)'");

            dicMatchString.Add("CumulativeEnergykWhExport", "CumulativeEnergykWhExport as 'kWh Export(1.0.2.8.0.255)'");
            dicMatchString.Add("CumulativeEnergykVAhExport", "CumulativeEnergykVAhExport as 'kVAh Export(1.0.10.8.0.255)'");
            dicMatchString.Add("CumulativeEnergykWhImport", "CumulativeEnergykWhImport as 'kWh Import(1.0.143.128.128.255)'");
            dicMatchString.Add("CumulativeEnergykVAhImport", "CumulativeEnergykVAhImport as 'kVAh Import(1.0.144.128.128.255)'"); 

            if (dicMatchString.ContainsKey(Value))
            {
                return dicMatchString[Value];
            }
            else
            {
                return string.Empty;
            }
        }


       
    }
}

