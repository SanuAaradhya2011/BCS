using System;
using System.Collections.Generic;
using System.Text;
using CAB.DALC.Data.DataServices;
using CAB.Entity;
using CAB.Framework.Entity;
using System.Data;
using CAB.Framework;
using Hunt.EPIC.Logging;

namespace CAB.DALC.Data
{
    /// <summary>
    /// Class containing Phasor DB operations
    /// </summary>
    public class DLMS650PhasorDAL : DALBase
    {
        #region "Private Members"
        private string phasorId ="PhasorId";
        private string MeterDataId = "MeterData_Id";
        private string PhasorDateTime = "PhasorDateTime";
        private string RPhaseCurrent = "RPhaseCurrent";
        private string YPhaseCurrent = "YPhaseCurrent";
        private string BPhaseCurrent = "BPhaseCurrent";
        private string RPhaseVoltage = "RPhaseVoltage";
        private string YPhaseVoltage = "YPhaseVoltage";
        private string BPhaseVoltage = "BPhaseVoltage";
        private string RPhasePowerFactor = "RPhasePowerFactor";
        private string YPhasePowerFactor = "YPhasePowerFactor";
        private string BPhasePowerFactor = "BPhasePowerFactor";
        private string TotalPhasePowerFactor = "TotalPhasePowerFactor";
        private string Frequency = "Frequency";
        private string ApparentPower = "ApparentPower";
        private string ActivePower = "ActivePower";
        private string ReactivePower = "ReactivePower";
        private string RPhaseNegativePowerFlag = "RPhaseNegativePowerFlag";
        private string YPhaseNegativePowerFlag = "YPhaseNegativePowerFlag";
        private string BPhaseNegativePowerFlag = "BPhaseNegativePowerFlag";
        private string RPhaseCapacitiveInductiveFlag = "RPhaseCapacitiveInductiveFlag";
        private string YPhaseCapacitiveInductiveFlag = "YPhaseCapacitiveInductiveFlag";
        private string BPhaseCapacitiveInductiveFlag = "BPhaseCapacitiveInductiveFlag";
        private string AngleYR = "AngleYR";
        private string AngleBR = "AngleBR";
        private string AngleBetweenTwo = "AngleBetweenTwo";
        private string RPhaseChannel = "RPhaseChannel";
        private string YPhaseChannel = "YPhaseChannel";
        private string BPhaseChannel = "BPhaseChannel";
        private string MeterData_ID = "MeterData_Id";
        private string PhaseSequence = "PhaseSequence";
        private string TotalActivePower = "TotalActivePower";
        private string TotalInductivePower = "TotalInductivePower";
        private string TotalCapacitivePower = "TotalCapacitivePower";
        private string TotalApparentPower = "TotalApparentPower";
        private string RPhasePF = "RPhasePF";
        private string YPhasePF = "YPhasePF";
        private string BPhasePF = "BPhasePF";
        private string TotalInstantaneousPF = "TotalInstantaneousPF";
        private string TotalkWDirection = "TotalkWDirection";
        private string RPhasekWDirection = "RPhasekWDirection";
        private string YPhasekWDirection = "YPhasekWDirection";
        private string BPhasekWDirection = "BPhasekWDirection";
        private string RPhaseLagLead = "RPhaseLagLead";
        private string YPhaseLagLead = "YPhaseLagLead";
        private string BPhaseLagLead = "BPhaseLagLead";
        private string Total = "Total";
        private string YPhaseAngleWithRPhase = "YPhaseAngleWithRPhase";
        private string BPhaseAngleWithRPhase = "BPhaseAngleWithRPhase";
        private string AngleBWAnyPhasePresent = "AngleBWAnyPhasePresent";
        private string CumulativeActiveEnergy = "CumulativeActiveEnergy";
        private string CumulativeApparentEnergy = "CumulativeApparentEnergy";
        private string CumulativeProgrammingCounter = "CumulativeProgrammingCounter";
        private string CumulativeBillingCounter = "CumulativeBillingCounter";
        private string CumulativeReactiveLagEnergy = "CumulativeReactiveLagEnergy";
        private string CumulativeReactiveLeadEnergy = "CumulativeReactiveLeadEnergy";
        private string CumulativeTamperCounter = "CumulativeTamperCounter";
        private string CumulativePowerFailMin = "CumulativePowerFailMin";
        private string MDOneKWData = "MDOneKWData";
        private string MDTwoKVAData = "MDTwoKVAData";
        private string NumberOfPowerFailMin = "NumberOfPowerFailMin";

        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(DLMS650PhasorDAL).ToString());
        #endregion

        #region "Constructor"
        public DLMS650PhasorDAL()
             : base("meterdata_phasor", "PhasorId")
        {
        }
        #endregion

        #region "Methods
        private DataRequest GetRequest(IEntity entity)
        {

            if (entity == null)
                return null;
            
            PhasorEntity phasorEntity = entity as PhasorEntity;

            StringBuilder builder = new StringBuilder();
            builder.Append("INSERT INTO meterdata_phasor(MeterData_Id,PhasorDateTime,RPhaseCurrent,YPhaseCurrent,BPhaseCurrent,RPhaseVoltage,YPhaseVoltage,BPhaseVoltage,RPhasePowerFactor,YPhasePowerFactor,BPhasePowerFactor,TotalPhasePowerFactor,Frequency,ApparentPower,ActivePower,ReactivePower,RPhaseNegativePowerFlag,YPhaseNegativePowerFlag,BPhaseNegativePowerFlag,RPhaseCapacitiveInductiveFlag,YPhaseCapacitiveInductiveFlag,BPhaseCapacitiveInductiveFlag,AngleYR,AngleBR,AngleBetweenTwo,RPhaseChannel,YPhaseChannel,PhaseSequence, BPhaseChannel )VALUES(");
            builder.Append(string.Concat(ParameterName(MeterDataId), ","));
            builder.Append(string.Concat(ParameterName(PhasorDateTime), ","));
            builder.Append(string.Concat(ParameterName(RPhaseCurrent), ","));
            builder.Append(string.Concat(ParameterName(YPhaseCurrent), ","));
            builder.Append(string.Concat(ParameterName(BPhaseCurrent), ","));
            builder.Append(string.Concat(ParameterName(RPhaseVoltage), ","));
            builder.Append(string.Concat(ParameterName(YPhaseVoltage), ","));
            builder.Append(string.Concat(ParameterName(BPhaseVoltage), ","));
            builder.Append(string.Concat(ParameterName(RPhasePowerFactor), ","));
            builder.Append(string.Concat(ParameterName(YPhasePowerFactor), ","));
            builder.Append(string.Concat(ParameterName(BPhasePowerFactor), ","));
            builder.Append(string.Concat(ParameterName(TotalPhasePowerFactor), ","));
            builder.Append(string.Concat(ParameterName(Frequency), ","));
            builder.Append(string.Concat(ParameterName(ApparentPower), ",")); 
            builder.Append(string.Concat(ParameterName(ActivePower), ","));
            builder.Append(string.Concat(ParameterName(ReactivePower), ","));
            builder.Append(string.Concat(ParameterName(RPhaseNegativePowerFlag), ","));
            builder.Append(string.Concat(ParameterName(YPhaseNegativePowerFlag), ","));
            builder.Append(string.Concat(ParameterName(BPhaseNegativePowerFlag), ","));
            builder.Append(string.Concat(ParameterName(RPhaseCapacitiveInductiveFlag), ","));
            builder.Append(string.Concat(ParameterName(YPhaseCapacitiveInductiveFlag), ","));
            builder.Append(string.Concat(ParameterName(BPhaseCapacitiveInductiveFlag), ","));
            builder.Append(string.Concat(ParameterName(AngleYR), ","));
            builder.Append(string.Concat(ParameterName(AngleBR), ",")); 
            builder.Append(string.Concat(ParameterName(AngleBetweenTwo), ","));
            builder.Append(string.Concat(ParameterName(RPhaseChannel), ","));
            builder.Append(string.Concat(ParameterName(YPhaseChannel), ","));
            builder.Append(string.Concat(ParameterName(PhaseSequence), ","));
            builder.Append(ParameterName(BPhaseChannel));
            builder.Append(")");
           
            DataRequest request = new DataRequest(builder.ToString());
            request.AddParamter(ParameterName(MeterDataId), phasorEntity.MeterDataId, DbType.Int64);
            request.AddParamter(ParameterName(PhasorDateTime), phasorEntity.CurrentDateTime, DbType.Int64);
            request.AddParamter(ParameterName(RPhaseCurrent), phasorEntity.RPhaseCurrent, DbType.String, 40);
            request.AddParamter(ParameterName(YPhaseCurrent), phasorEntity.YPhaseCurrent, DbType.String, 40);
            request.AddParamter(ParameterName(BPhaseCurrent), phasorEntity.BPhaseCurrent, DbType.String, 40);
            request.AddParamter(ParameterName(RPhaseVoltage), phasorEntity.RPhaseVoltage, DbType.String, 40);
            request.AddParamter(ParameterName(YPhaseVoltage), phasorEntity.YPhaseVoltage, DbType.String, 40);
            request.AddParamter(ParameterName(BPhaseVoltage), phasorEntity.BPhaseVoltage, DbType.String, 40);
            request.AddParamter(ParameterName(RPhasePowerFactor), phasorEntity.RPhasePowerFactor, DbType.String, 40);
            request.AddParamter(ParameterName(YPhasePowerFactor), phasorEntity.YPhasePowerFactor, DbType.String, 40);
            request.AddParamter(ParameterName(BPhasePowerFactor), phasorEntity.BPhasePowerFactor, DbType.String,40);
            request.AddParamter(ParameterName(TotalPhasePowerFactor), phasorEntity.TotalPhasePowerFactor, DbType.String, 40);
            request.AddParamter(ParameterName(Frequency), phasorEntity.Frequency, DbType.String, 40);
            request.AddParamter(ParameterName(ApparentPower), phasorEntity.ApparentPower, DbType.String, 40);
            request.AddParamter(ParameterName(ActivePower), phasorEntity.ActivePower, DbType.String, 40);
            request.AddParamter(ParameterName(ReactivePower), phasorEntity.ReActivePower, DbType.String, 40);
            request.AddParamter(ParameterName(RPhaseNegativePowerFlag), phasorEntity.RPhaseNegativePowerFlag, DbType.String, 40);
            request.AddParamter(ParameterName(YPhaseNegativePowerFlag), phasorEntity.YPhaseNegativePowerFlag, DbType.String, 40);
            request.AddParamter(ParameterName(BPhaseNegativePowerFlag), phasorEntity.BPhaseNegativePowerFlag, DbType.String, 40);
            request.AddParamter(ParameterName(RPhaseCapacitiveInductiveFlag), phasorEntity.RPhaseCapacitiveInductiveFlag, DbType.String, 40);
            request.AddParamter(ParameterName(YPhaseCapacitiveInductiveFlag), phasorEntity.YPhaseCapacitiveInductiveFlag, DbType.String, 40);
            request.AddParamter(ParameterName(BPhaseCapacitiveInductiveFlag), phasorEntity.BPhaseCapacitiveInductiveFlag, DbType.String, 40);
            request.AddParamter(ParameterName(AngleYR), phasorEntity.AngleYR, DbType.String, 40);
            request.AddParamter(ParameterName(AngleBR), phasorEntity.AngleBR, DbType.String, 40);
            request.AddParamter(ParameterName(AngleBetweenTwo), phasorEntity.AngleBetweenTwo, DbType.String, 40);
            request.AddParamter(ParameterName(RPhaseChannel), phasorEntity.RPhaseChannel, DbType.String, 40);
            request.AddParamter(ParameterName(YPhaseChannel), phasorEntity.YPhaseChannel, DbType.String, 40);
            request.AddParamter(ParameterName(PhaseSequence), phasorEntity.PhaseSequence, DbType.String, 40);
            request.AddParamter(ParameterName(BPhaseChannel), phasorEntity.BPhaseChannel, DbType.String, 40);
          
            return request;
        }


        public override IEntity InsertData(IEntity entity)
        {
            PhasorEntity phasorEntity = entity as PhasorEntity;
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
                phasorEntity.PhasorId = long.Parse(this.GetPK());
            return phasorEntity;
        }

        public override IEntity InsertData(IList<IEntity> entities)
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
                builder.Append("Delete from  meterdata_phasor where ");
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
        public override bool UpdateData(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteData(IEntity entity)
        {
            throw new NotImplementedException();
        }
     
        public DataSet GetPhasorDetailData(int meterDataId)
        {
            DataSet dsPhasor = null;
            
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("SELECT PhasorId,MeterData_Id,PhasorDateTime,RPhaseCurrent,YPhaseCurrent,BPhaseCurrent,RPhaseVoltage,");
                builder.Append("YPhaseVoltage,BPhaseVoltage,RPhasePowerFactor,YPhasePowerFactor,BPhasePowerFactor,TotalPhasePowerFactor,");
                builder.Append("Frequency,ApparentPower,ActivePower,ReactivePower,RPhaseNegativePowerFlag,YPhaseNegativePowerFlag,BPhaseNegativePowerFlag,");
                builder.Append("RPhaseCapacitiveInductiveFlag,YPhaseCapacitiveInductiveFlag,BPhaseCapacitiveInductiveFlag,AngleYR,AngleBR,AngleBetweenTwo,");
                builder.Append("RPhaseChannel,YPhaseChannel,BPhaseChannel, PhaseSequence FROM meterdata_phasor ");
                builder.Append("Where ");
                builder.Append(string.Concat(MeterData_ID, " =", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataId, DbType.Int64);
                dsPhasor = new DataSet();
                dsPhasor = helper.FillDataSet(request, dsPhasor);
               

                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Phasor Data viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetPhasorDetailData(int meterDataId)", ex);
                return null;
            }
            return dsPhasor;
        }
               
      

        public override IList<IEntity> ListData()
        {
            throw new NotImplementedException();
        }

        public override DataSet ListDataSet()
        {
            throw new NotImplementedException();
        }

        public DataSet GetPhasorDataByMeter(string meterDataId)
        {
            DataSet dsPhasor = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("SELECT PhasorId,MeterData_Id,PhasorDateTime,RPhaseCurrent,YPhaseCurrent,BPhaseCurrent,RPhaseVoltage,");
                builder.Append("YPhaseVoltage,BPhaseVoltage,RPhasePowerFactor,YPhasePowerFactor,BPhasePowerFactor,TotalPhasePowerFactor,");
                builder.Append("Frequency,ApparentPower,ActivePower,ReactivePower,RPhaseNegativePowerFlag,YPhaseNegativePowerFlag,BPhaseNegativePowerFlag,");
                builder.Append("RPhaseCapacitiveInductiveFlag,YPhaseCapacitiveInductiveFlag,BPhaseCapacitiveInductiveFlag,AngleYR,AngleBR,AngleBetweenTwo,");
                builder.Append("RPhaseChannel,YPhaseChannel,BPhaseChannel,PhaseSequence FROM meterdata_phasor ");
                builder.Append("Where ");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataId, DbType.String, 20);
                dsPhasor = new DataSet();
                dsPhasor = helper.FillDataSet(request, dsPhasor);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Phasor Data viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetPhasorDataByMeter(string meterDataId)", ex);
            }
            return dsPhasor;
        }

        #endregion

        public override IEntity GetDetailData(int id)
        {
            throw new NotImplementedException();
        }

        public override IEntity RowToEntity(DataRow row)
        {
            if (row == null) return null;
            PhasorEntity phasorEntity = new PhasorEntity();
            if (NotNullAndNotDBNull(row, phasorId)) phasorEntity.PhasorId = Convert.ToInt64(row[phasorId]);
            if (NotNullAndNotDBNull(row, MeterDataId)) phasorEntity.MeterDataId = Convert.ToInt64(row[MeterDataId]);
            if (NotNullAndNotDBNull(row, PhasorDateTime)) phasorEntity.CurrentDateTime = Convert.ToInt64(row[PhasorDateTime]);
            if (NotNullAndNotDBNull(row, RPhaseCurrent)) phasorEntity.RPhaseCurrent = Convert.ToString(row[RPhaseCurrent]);
            if (NotNullAndNotDBNull(row, YPhaseCurrent)) phasorEntity.YPhaseCurrent = Convert.ToString(row[YPhaseCurrent]);
            if (NotNullAndNotDBNull(row, BPhaseCurrent)) phasorEntity.BPhaseCurrent = Convert.ToString(row[BPhaseCurrent]);
            if (NotNullAndNotDBNull(row, RPhaseVoltage)) phasorEntity.RPhaseVoltage = Convert.ToString(row[RPhaseVoltage]);
            if (NotNullAndNotDBNull(row, YPhaseVoltage)) phasorEntity.YPhaseVoltage = Convert.ToString(row[YPhaseVoltage]);
            if (NotNullAndNotDBNull(row, BPhaseVoltage)) phasorEntity.BPhaseVoltage = Convert.ToString(row[BPhaseVoltage]);
            if (NotNullAndNotDBNull(row, RPhasePowerFactor)) phasorEntity.RPhasePowerFactor = phasorEntity.RPhasePF = Convert.ToString(row[RPhasePowerFactor]);
            if (NotNullAndNotDBNull(row, YPhasePowerFactor)) phasorEntity.YPhasePowerFactor =  phasorEntity.YPhasePF = Convert.ToString(row[YPhasePowerFactor]);
            if (NotNullAndNotDBNull(row, BPhasePowerFactor)) phasorEntity.BPhasePowerFactor = phasorEntity.BPhasePF = Convert.ToString(row[BPhasePowerFactor]);
            if (NotNullAndNotDBNull(row, TotalPhasePowerFactor)) phasorEntity.TotalPhasePowerFactor  = Convert.ToString(row[TotalPhasePowerFactor]);
            if (NotNullAndNotDBNull(row, Frequency)) phasorEntity.Frequency = Convert.ToString(row[Frequency]);
            if (NotNullAndNotDBNull(row, ApparentPower)) phasorEntity.ApparentPower  = Convert.ToString(row[ApparentPower]);
            if (NotNullAndNotDBNull(row, ActivePower)) phasorEntity.ActivePower = Convert.ToString(row[ActivePower]);
            if (NotNullAndNotDBNull(row, ReactivePower)) phasorEntity.ReActivePower = Convert.ToString(row[ReactivePower]);
            if (NotNullAndNotDBNull(row, RPhaseNegativePowerFlag)) phasorEntity.RPhaseNegativePowerFlag = phasorEntity.RPhasekWDirection = Convert.ToString(row[RPhaseNegativePowerFlag]);
            if (NotNullAndNotDBNull(row, YPhaseNegativePowerFlag)) phasorEntity.YPhaseNegativePowerFlag = phasorEntity.YPhasekWDirection = Convert.ToString(row[YPhaseNegativePowerFlag]);
            if (NotNullAndNotDBNull(row, BPhaseNegativePowerFlag)) phasorEntity.BPhaseNegativePowerFlag = phasorEntity.BPhasekWDirection = Convert.ToString(row[BPhaseNegativePowerFlag]);
            if (NotNullAndNotDBNull(row, RPhaseCapacitiveInductiveFlag)) phasorEntity.RPhaseCapacitiveInductiveFlag = phasorEntity.RPhaseLagLead = Convert.ToString(row[RPhaseCapacitiveInductiveFlag]);
            if (NotNullAndNotDBNull(row, YPhaseCapacitiveInductiveFlag)) phasorEntity.YPhaseCapacitiveInductiveFlag = phasorEntity.YPhaseLagLead = Convert.ToString(row[YPhaseCapacitiveInductiveFlag]);
            if (NotNullAndNotDBNull(row, BPhaseCapacitiveInductiveFlag)) phasorEntity.BPhaseCapacitiveInductiveFlag = phasorEntity.BPhaseLagLead = Convert.ToString(row[BPhaseCapacitiveInductiveFlag]);
            if (NotNullAndNotDBNull(row, AngleYR)) phasorEntity.AngleYR = phasorEntity.YPhaseAngleWithRPhase = Convert.ToString(row[AngleYR]);
            if (NotNullAndNotDBNull(row, AngleBR)) phasorEntity.AngleBR = phasorEntity.BPhaseAngleWithRPhase  = Convert.ToString(row[AngleBR]);
            if (NotNullAndNotDBNull(row, AngleBetweenTwo)) phasorEntity.AngleBetweenTwo = phasorEntity.AngleBWAnyPhasePresent = Convert.ToString(row[AngleBetweenTwo]);
            if (NotNullAndNotDBNull(row, PhaseSequence)) phasorEntity.PhaseSequence = Convert.ToString(row[PhaseSequence]);
            UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data Phasor viewed"));
            return phasorEntity;
        }
        /// <summary>
        /// IEC Phasor data display
        /// </summary>
        /// <param name="row"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public IEntity RowToEntity(DataRow row, bool flag)
        {
            if (row == null) return null;
            PhasorEntity phasorEntity = new PhasorEntity();
            if (NotNullAndNotDBNull(row, RPhaseVoltage)) phasorEntity.RPhaseVoltage = Convert.ToString(row[RPhaseVoltage]);
            if (NotNullAndNotDBNull(row, YPhaseVoltage)) phasorEntity.YPhaseVoltage = Convert.ToString(row[YPhaseVoltage]);
            if (NotNullAndNotDBNull(row, BPhaseVoltage)) phasorEntity.BPhaseVoltage = Convert.ToString(row[BPhaseVoltage]);
            if (NotNullAndNotDBNull(row, RPhaseCurrent)) phasorEntity.RPhaseCurrent = Convert.ToString(row[RPhaseCurrent]);
            if (NotNullAndNotDBNull(row, YPhaseCurrent)) phasorEntity.YPhaseCurrent = Convert.ToString(row[YPhaseCurrent]);
            if (NotNullAndNotDBNull(row, BPhaseCurrent)) phasorEntity.BPhaseCurrent = Convert.ToString(row[BPhaseCurrent]);
            if (NotNullAndNotDBNull(row, TotalActivePower)) phasorEntity.TotalActivePower = phasorEntity.ActivePower = Convert.ToString(row[TotalActivePower]);
            if (NotNullAndNotDBNull(row, TotalApparentPower)) phasorEntity.TotalApparentPower = phasorEntity.ApparentPower = Convert.ToString(row[TotalApparentPower]);
            if (NotNullAndNotDBNull(row, TotalCapacitivePower)) phasorEntity.TotalCapacitivePower = phasorEntity.ReActivePower = Convert.ToString(row[TotalCapacitivePower]);            
            if (NotNullAndNotDBNull(row, RPhasePF)) phasorEntity.RPhasePowerFactor = phasorEntity.RPhasePF = Convert.ToString(row[RPhasePF]);
            if (NotNullAndNotDBNull(row, YPhasePF))  phasorEntity.YPhasePowerFactor = phasorEntity.YPhasePF = Convert.ToString(row[YPhasePF]);
            if (NotNullAndNotDBNull(row, BPhasePF)) phasorEntity.BPhasePowerFactor = phasorEntity.BPhasePF = Convert.ToString(row[BPhasePF]);
            if (NotNullAndNotDBNull(row, TotalInstantaneousPF))  phasorEntity.TotalPhasePowerFactor = Convert.ToString(row[TotalInstantaneousPF]);
            if (NotNullAndNotDBNull(row, Frequency)) phasorEntity.Frequency = Convert.ToString(row[Frequency]);
            if (NotNullAndNotDBNull(row, PhaseSequence)) phasorEntity.PhaseSequence = Convert.ToString(row[PhaseSequence]);
            if (NotNullAndNotDBNull(row, TotalkWDirection)) phasorEntity.TotalkWDirection = Convert.ToString(row[TotalkWDirection]);
            if (NotNullAndNotDBNull(row, RPhasekWDirection)) phasorEntity.RPhaseNegativePowerFlag = phasorEntity.RPhasekWDirection = Convert.ToString(row[RPhasekWDirection]);
            if (NotNullAndNotDBNull(row, YPhasekWDirection)) phasorEntity.YPhaseNegativePowerFlag = phasorEntity.YPhasekWDirection = Convert.ToString(row[YPhasekWDirection]);
            if (NotNullAndNotDBNull(row, BPhasekWDirection)) phasorEntity.BPhaseNegativePowerFlag = phasorEntity.BPhasekWDirection = Convert.ToString(row[BPhasekWDirection]);
            if (NotNullAndNotDBNull(row, RPhaseChannel)) phasorEntity.RPhaseChannel = Convert.ToString(row[RPhaseChannel]);
            if (NotNullAndNotDBNull(row, YPhaseChannel)) phasorEntity.YPhaseChannel = Convert.ToString(row[YPhaseChannel]);
            if (NotNullAndNotDBNull(row, BPhaseChannel)) phasorEntity.BPhaseChannel = Convert.ToString(row[BPhaseChannel]);
            if (NotNullAndNotDBNull(row, RPhaseLagLead)) phasorEntity.RPhaseCapacitiveInductiveFlag = phasorEntity.RPhaseLagLead = Convert.ToString(row[RPhaseLagLead]);
            if (NotNullAndNotDBNull(row, YPhaseLagLead)) phasorEntity.YPhaseCapacitiveInductiveFlag = phasorEntity.YPhaseLagLead = Convert.ToString(row[YPhaseLagLead]);
            if (NotNullAndNotDBNull(row, BPhaseLagLead)) phasorEntity.BPhaseCapacitiveInductiveFlag = phasorEntity.BPhaseLagLead = Convert.ToString(row[BPhaseLagLead]);
            if (NotNullAndNotDBNull(row, Total)) phasorEntity.Total = Convert.ToString(row[Total]);
            if (NotNullAndNotDBNull(row, YPhaseAngleWithRPhase)) phasorEntity.YPhaseAngleWithRPhase = phasorEntity.AngleYR = Convert.ToString(row[YPhaseAngleWithRPhase]);
            if (NotNullAndNotDBNull(row, BPhaseAngleWithRPhase)) phasorEntity.BPhaseAngleWithRPhase = phasorEntity.AngleBR = Convert.ToString(row[BPhaseAngleWithRPhase]);
            if (NotNullAndNotDBNull(row, AngleBWAnyPhasePresent)) phasorEntity.AngleBWAnyPhasePresent = phasorEntity.AngleBetweenTwo = Convert.ToString(row[AngleBWAnyPhasePresent]);
            UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data Phasor viewed"));
            return phasorEntity;
        }

    }
        

}
