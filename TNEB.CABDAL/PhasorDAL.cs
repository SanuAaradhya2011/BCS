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
using System.Data;
using CAB.IECFramework;
using CAB.DALC.Data.DataServices;
using CAB.Entity;
using CAB.IECFramework.Entity;
using System.Data.Common;

namespace CAB.DALC.Data
{
    public class PhasorDAL : DALBase
    {
        private string Phasor_ID = "Phasor_ID"; 
        private string RPhaseVoltage = "RPhaseVoltage";
        private string YPhaseVoltage = "YPhaseVoltage";
        private string BPhaseVoltage = "BPhaseVoltage";
        private string RPhaseCurrent = "RPhaseCurrent";
        private string YPhaseCurrent = "YPhaseCurrent"; 
        private string BPhaseCurrent = "BPhaseCurrent";
        private string TotalActivePower = "TotalActivePower";
        private string TotalInductivePower = "TotalInductivePower";
        private string TotalCapacitivePower = "TotalCapacitivePower";
        private string TotalApparentPower = "TotalApparentPower";
        private string RPhasePF = "RPhasePF";
        private string YPhasePF = "YPhasePF";
        private string BPhasePF = "BPhasePF"; 
        private string TotalInstantaneousPF = "TotalInstantaneousPF";
        private string Frequency = "Frequency";
        private string PhaseSequence = "PhaseSequence";
        private string TotalkWDirection = "TotalkWDirection";
        private string RPhasekWDirection = "RPhasekWDirection";
        private string YPhasekWDirection = "YPhasekWDirection"; 
        private string BPhasekWDirection = "BPhasekWDirection"; 
        private string RPhaseChannel = "RPhaseChannel";
        private string YPhaseChannel = "YPhaseChannel";
        private string BPhaseChannel = "BPhaseChannel";
        private string RPhaseLagLead = "RPhaseLagLead";
        private string YPhaseLagLead = "YPhaseLagLead";
        private string BPhaseLagLead = "BPhaseLagLead"; 
        private string Total = "Total";
        private string YPhaseAngleWithRPhase = "YPhaseAngleWithRPhase";
        private string BPhaseAngleWithRPhase = "BPhaseAngleWithRPhase";
        private string AngleBWAnyPhasePresent = "AngleBWAnyPhasePresent"; 
        private string MeterData_ID = "MeterData_ID";

        public PhasorDAL()
            : base("MeterData_Phasor", "Phasor_ID")
        {
        }


        public override IEntity InsertData(IEntity entity)
        {
            IECPhasorEntity phasorEntity = entity as IECPhasorEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert Into MeterData_Phasor(RPhaseVoltage,YPhaseVoltage,BPhaseVoltage,RPhaseCurrent,YPhaseCurrent,BPhaseCurrent,");
                builder.Append("TotalActivePower, TotalInductivePower, TotalCapacitivePower, TotalApparentPower, ");
                builder.Append("RPhasePF,YPhasePF,BPhasePF,TotalInstantaneousPF,Frequency,PhaseSequence,TotalkWDirection,");
                builder.Append("RPhasekWDirection,YPhasekWDirection,BPhasekWDirection,RPhaseChannel,YPhaseChannel,BPhaseChannel,RPhaseLagLead,YPhaseLagLead,");
                builder.Append("BPhaseLagLead,Total,YPhaseAngleWithRPhase,BPhaseAngleWithRPhase,AngleBWAnyPhasePresent,MeterData_ID) values(");
                builder.Append(string.Concat(ParameterName(RPhaseVoltage), ","));
                builder.Append(string.Concat(ParameterName(YPhaseVoltage), ","));
                builder.Append(string.Concat(ParameterName(BPhaseVoltage), ","));
                builder.Append(string.Concat(ParameterName(RPhaseCurrent), ","));
                builder.Append(string.Concat(ParameterName(YPhaseCurrent), ","));
                builder.Append(string.Concat(ParameterName(BPhaseCurrent), ","));
                builder.Append(string.Concat(ParameterName(TotalActivePower), ","));
                builder.Append(string.Concat(ParameterName(TotalInductivePower), ","));
                builder.Append(string.Concat(ParameterName(TotalCapacitivePower), ","));
                builder.Append(string.Concat(ParameterName(TotalApparentPower), ","));
                builder.Append(string.Concat(ParameterName(RPhasePF), ","));
                builder.Append(string.Concat(ParameterName(YPhasePF), ","));
                builder.Append(string.Concat(ParameterName(BPhasePF), ","));
                builder.Append(string.Concat(ParameterName(TotalInstantaneousPF), ","));
                builder.Append(string.Concat(ParameterName(Frequency), ","));
                builder.Append(string.Concat(ParameterName(PhaseSequence), ","));
                builder.Append(string.Concat(ParameterName(TotalkWDirection), ","));
                builder.Append(string.Concat(ParameterName(RPhasekWDirection), ","));
                builder.Append(string.Concat(ParameterName(YPhasekWDirection), ","));
                builder.Append(string.Concat(ParameterName(BPhasekWDirection), ","));
                builder.Append(string.Concat(ParameterName(RPhaseChannel), ","));
                builder.Append(string.Concat(ParameterName(YPhaseChannel), ","));
                builder.Append(string.Concat(ParameterName(BPhaseChannel), ","));
                builder.Append(string.Concat(ParameterName(RPhaseLagLead), ","));
                builder.Append(string.Concat(ParameterName(YPhaseLagLead), ","));
                builder.Append(string.Concat(ParameterName(BPhaseLagLead), ","));
                builder.Append(string.Concat(ParameterName(Total), ",")); 
                builder.Append(string.Concat(ParameterName(YPhaseAngleWithRPhase), ","));
                builder.Append(string.Concat(ParameterName(BPhaseAngleWithRPhase), ","));
                builder.Append(string.Concat(ParameterName(AngleBWAnyPhasePresent), ","));
                builder.Append(string.Concat(ParameterName(MeterData_ID), ")"));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(RPhaseVoltage), phasorEntity.RPhaseVoltage, DbType.String, 20);
                request.AddParamter(ParameterName(YPhaseVoltage), phasorEntity.YPhaseVoltage, DbType.String, 20);
                request.AddParamter(ParameterName(BPhaseVoltage), phasorEntity.BPhaseVoltage, DbType.String, 20);
                request.AddParamter(ParameterName(RPhaseCurrent), phasorEntity.RPhaseCurrent, DbType.String, 20);
                request.AddParamter(ParameterName(YPhaseCurrent), phasorEntity.YPhaseCurrent, DbType.String, 20);
                request.AddParamter(ParameterName(BPhaseCurrent), phasorEntity.BPhaseCurrent, DbType.String, 20);
                request.AddParamter(ParameterName(TotalActivePower), phasorEntity.TotalActivePower, DbType.String, 20);
                request.AddParamter(ParameterName(TotalInductivePower), phasorEntity.TotalInductivePower, DbType.String, 20);
                request.AddParamter(ParameterName(TotalCapacitivePower), phasorEntity.TotalCapacitivePower, DbType.String, 20);
                request.AddParamter(ParameterName(TotalApparentPower), phasorEntity.TotalApparentPower, DbType.String, 20);
                request.AddParamter(ParameterName(RPhasePF), phasorEntity.RPhasePF, DbType.String, 20);
                request.AddParamter(ParameterName(YPhasePF), phasorEntity.YPhasePF, DbType.String, 20);
                request.AddParamter(ParameterName(BPhasePF), phasorEntity.BPhasePF, DbType.String, 20);
                request.AddParamter(ParameterName(TotalInstantaneousPF), phasorEntity.TotalInstantaneousPF, DbType.String, 20);
                request.AddParamter(ParameterName(Frequency), phasorEntity.Frequency, DbType.String, 20);
                request.AddParamter(ParameterName(PhaseSequence), phasorEntity.PhaseSequence, DbType.String, 20);
                request.AddParamter(ParameterName(TotalkWDirection), phasorEntity.TotalkWDirection, DbType.String, 20);
                request.AddParamter(ParameterName(RPhasekWDirection), phasorEntity.RPhasekWDirection, DbType.String, 20);
                request.AddParamter(ParameterName(YPhasekWDirection), phasorEntity.YPhasekWDirection, DbType.String, 20);
                request.AddParamter(ParameterName(BPhasekWDirection), phasorEntity.BPhasekWDirection, DbType.String, 20);
                request.AddParamter(ParameterName(RPhaseChannel), phasorEntity.RPhaseChannel, DbType.String, 20);
                request.AddParamter(ParameterName(YPhaseChannel), phasorEntity.YPhaseChannel, DbType.String, 20);
                request.AddParamter(ParameterName(BPhaseChannel), phasorEntity.BPhaseChannel, DbType.String, 20);
                request.AddParamter(ParameterName(RPhaseLagLead), phasorEntity.RPhaseLagLead, DbType.String, 20);
                request.AddParamter(ParameterName(YPhaseLagLead), phasorEntity.YPhaseLagLead, DbType.String, 20);
                request.AddParamter(ParameterName(BPhaseLagLead), phasorEntity.BPhaseLagLead, DbType.String, 20);
                request.AddParamter(ParameterName(Total), phasorEntity.Total, DbType.String, 20); 
                request.AddParamter(ParameterName(YPhaseAngleWithRPhase), phasorEntity.YPhaseAngleWithRPhase, DbType.String, 20);
                request.AddParamter(ParameterName(BPhaseAngleWithRPhase), phasorEntity.BPhaseAngleWithRPhase, DbType.String, 20);
                request.AddParamter(ParameterName(AngleBWAnyPhasePresent), phasorEntity.AngleBWAnyPhasePresent, DbType.String, 20);
                request.AddParamter(ParameterName(MeterData_ID), phasorEntity.MeterData_ID, DbType.Int32);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data Phasor added"));
                Flag = true;
            }
            catch (Exception) { }
            if (Flag)
                phasorEntity.Phasor_ID = long.Parse(this.GetPK());
            return phasorEntity;
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
                builder.Append("Delete from MeterData_Phasor where ");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataID, DbType.Int32);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data deleted"));
                Flag = true;
            }
            catch (Exception) { }
            return Flag;
        }

        public override bool DeleteData(IEntity entity)
        {
            IECPhasorEntity phasorEntity = entity as IECPhasorEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete from MeterData_Phasor where ");
                builder.Append(string.Concat(Phasor_ID, "=", ParameterName(Phasor_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Phasor_ID), phasorEntity.Phasor_ID, DbType.UInt32);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data Phasor deleted"));
                Flag = true;
            }
            catch (Exception) { }
            return Flag;
        }

        public override IEntity GetDetailData(int id)
        {
            IECPhasorEntity phasorEntity = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select Phasor_ID,RPhaseVoltage,YPhaseVoltage,BPhaseVoltage,RPhaseCurrent,YPhaseCurrent,BPhaseCurrent,TotalActivePower,TotalInductivePower,TotalCapacitivePower,TotalApparentPower,RPhasePF,YPhasePF,BPhasePF,TotalInstantaneousPF,Frequency,PhaseSequence,TotalkWDirection,RPhasekWDirection,YPhasekWDirection,BPhasekWDirection,RPhaseChannel,YPhaseChannel,BPhaseChannel,RPhaseLagLead,YPhaseLagLead,BPhaseLagLead,Total,YPhaseAngleWithRPhase,BPhaseAngleWithRPhase,AngleBWAnyPhasePresent,MeterData_ID from MeterData_Phasor where ");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), id, DbType.UInt32);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    phasorEntity = (IECPhasorEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data Phasor viewed"));

            }
            catch (CABException)
            {
            }
            return phasorEntity;
        }

        public override IList<IEntity> ListData()
        {
            throw new NotImplementedException();
        }
        public override DataSet ListDataSet()
        {
            throw new NotImplementedException();
        }
        public DataSet ListDataSet(long meterDataId)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select ");  
                builder.Append("RPhaseVoltage as 'R Phase Voltage',");
                builder.Append("YPhaseVoltage as 'Y Phase Voltage',");
                builder.Append("BPhaseVoltage as 'B Phase Voltage',"); 
                builder.Append("RPhaseCurrent as 'R Phase Current',");
                builder.Append("YPhaseCurrent as 'Y Phase Current',");
                builder.Append("BPhaseCurrent as 'B Phase Current',");
                builder.Append("TotalActivePower as 'Total Active Power',");
                builder.Append("TotalInductivePower as 'Total Inductive Power',");
                builder.Append("TotalCapacitivePower as 'Total Capacitive Power',");
                builder.Append("TotalApparentPower as 'Total Apparent Power',"); 
                builder.Append("RPhasePF as 'R Phase PF',");
                builder.Append("YPhasePF as 'Y Phase PF',");
                builder.Append("BPhasePF as 'B Phase PF',"); 
                builder.Append("TotalInstantaneousPF as 'Total Instantaneous PF',");
                builder.Append("Frequency,"); 
                builder.Append("PhaseSequence as 'Phase Sequence',");
                builder.Append("TotalkWDirection as 'Total kW Direction',");
                builder.Append("RPhasekWDirection as 'R Phase kW Direction',");
                builder.Append("YPhasekWDirection as 'Y Phase kW Direction',"); 
                builder.Append("BPhasekWDirection as 'B Phase kW Direction',"); 
                builder.Append("RPhaseChannel as 'R Phase Channel',");
                builder.Append("YPhaseChannel as 'Y Phase Channel',");
                builder.Append("BPhaseChannel as 'B Phase Channel',");
                builder.Append("RPhaseLagLead as 'R Phase Lag/Lead',"); 
                builder.Append("YPhaseLagLead as 'Y Phase Lag/Lead',");
                builder.Append("BPhaseLagLead as 'B Phase Lag/Lead',");
                builder.Append("Total,");
                builder.Append("YPhaseAngleWithRPhase as 'Y Phase Angle With R Phase',");
                builder.Append("BPhaseAngleWithRPhase as 'B Phase Angle With R Phase',");
                builder.Append("AngleBWAnyPhasePresent as 'Angle B/W Any 2 Phase Present' from MeterData_Phasor where ");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataId, DbType.Int64);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data Phasor viewed"));
            }
            catch (CABException)
            {
            }
            return dataSet;
        }
        
        public override IEntity RowToEntity(DataRow row)
        {
            if (row == null) return null;
            IECPhasorEntity phasorEntity = new IECPhasorEntity();
            if (NotNullAndNotDBNull(row, Phasor_ID)) phasorEntity.Phasor_ID = Convert.ToInt64(row[Phasor_ID]);
            if (NotNullAndNotDBNull(row, RPhaseVoltage)) phasorEntity.RPhaseVoltage = Convert.ToString(row[RPhaseVoltage]);
            if (NotNullAndNotDBNull(row, YPhaseVoltage)) phasorEntity.YPhaseVoltage = Convert.ToString(row[YPhaseVoltage]);
            if (NotNullAndNotDBNull(row, BPhaseVoltage)) phasorEntity.BPhaseVoltage = Convert.ToString(row[BPhaseVoltage]);
            if (NotNullAndNotDBNull(row, RPhaseCurrent)) phasorEntity.RPhaseCurrent = Convert.ToString(row[RPhaseCurrent]);
            if (NotNullAndNotDBNull(row, YPhaseCurrent)) phasorEntity.YPhaseCurrent = Convert.ToString(row[YPhaseCurrent]);
            if (NotNullAndNotDBNull(row, BPhaseCurrent)) phasorEntity.BPhaseCurrent = Convert.ToString(row[BPhaseCurrent]);
            if (NotNullAndNotDBNull(row, TotalActivePower)) phasorEntity.TotalActivePower = Convert.ToString(row[TotalActivePower]);
            if (NotNullAndNotDBNull(row, TotalApparentPower)) phasorEntity.TotalApparentPower = Convert.ToString(row[TotalApparentPower]);
            if (NotNullAndNotDBNull(row, TotalCapacitivePower)) phasorEntity.TotalCapacitivePower = Convert.ToString(row[TotalCapacitivePower]);
            if (NotNullAndNotDBNull(row, TotalApparentPower)) phasorEntity.TotalApparentPower = Convert.ToString(row[TotalApparentPower]);
            if (NotNullAndNotDBNull(row, RPhasePF)) phasorEntity.RPhasePF = Convert.ToString(row[RPhasePF]);
            if (NotNullAndNotDBNull(row, YPhasePF)) phasorEntity.YPhasePF = Convert.ToString(row[YPhasePF]);
            if (NotNullAndNotDBNull(row, BPhasePF)) phasorEntity.BPhasePF = Convert.ToString(row[BPhasePF]);
            if (NotNullAndNotDBNull(row, TotalInstantaneousPF)) phasorEntity.TotalInstantaneousPF = Convert.ToString(row[TotalInstantaneousPF]);
            if (NotNullAndNotDBNull(row, Frequency)) phasorEntity.Frequency = Convert.ToString(row[Frequency]);
            if (NotNullAndNotDBNull(row, PhaseSequence)) phasorEntity.PhaseSequence = Convert.ToString(row[PhaseSequence]);
            if (NotNullAndNotDBNull(row, TotalkWDirection)) phasorEntity.TotalkWDirection = Convert.ToString(row[TotalkWDirection]);
            if (NotNullAndNotDBNull(row, RPhasekWDirection)) phasorEntity.RPhasekWDirection = Convert.ToString(row[RPhasekWDirection]);
            if (NotNullAndNotDBNull(row, YPhasekWDirection)) phasorEntity.YPhasekWDirection = Convert.ToString(row[YPhasekWDirection]);
            if (NotNullAndNotDBNull(row, BPhasekWDirection)) phasorEntity.BPhasekWDirection = Convert.ToString(row[BPhasekWDirection]);
            if (NotNullAndNotDBNull(row, RPhaseChannel)) phasorEntity.RPhaseChannel = Convert.ToString(row[RPhaseChannel]);
            if (NotNullAndNotDBNull(row, YPhaseChannel)) phasorEntity.YPhaseChannel = Convert.ToString(row[YPhaseChannel]);
            if (NotNullAndNotDBNull(row, BPhaseChannel)) phasorEntity.BPhaseChannel = Convert.ToString(row[BPhaseChannel]);
            if (NotNullAndNotDBNull(row, RPhaseLagLead)) phasorEntity.RPhaseLagLead = Convert.ToString(row[RPhaseLagLead]);
            if (NotNullAndNotDBNull(row, YPhaseLagLead)) phasorEntity.YPhaseLagLead = Convert.ToString(row[YPhaseLagLead]);
            if (NotNullAndNotDBNull(row, BPhaseLagLead)) phasorEntity.BPhaseLagLead = Convert.ToString(row[BPhaseLagLead]);
            if (NotNullAndNotDBNull(row, Total)) phasorEntity.Total = Convert.ToString(row[Total]);
            if (NotNullAndNotDBNull(row, YPhaseAngleWithRPhase)) phasorEntity.YPhaseAngleWithRPhase = Convert.ToString(row[YPhaseAngleWithRPhase]);
            if (NotNullAndNotDBNull(row, BPhaseAngleWithRPhase)) phasorEntity.BPhaseAngleWithRPhase = Convert.ToString(row[BPhaseAngleWithRPhase]);
            if (NotNullAndNotDBNull(row, AngleBWAnyPhasePresent)) phasorEntity.AngleBWAnyPhasePresent = Convert.ToString(row[AngleBWAnyPhasePresent]);
            if (NotNullAndNotDBNull(row, MeterData_ID)) phasorEntity.MeterData_ID = Convert.ToInt64(row[MeterData_ID]);
            UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data Phasor viewed"));
            return phasorEntity;
        }
        public  IEntity RowToEntity(DataRow row,bool flag)
        {
            if (row == null) return null;
            IECPhasorEntity phasorEntity = new IECPhasorEntity();           
            if (NotNullAndNotDBNull(row, RPhaseVoltage)) phasorEntity.RPhaseVoltage = Convert.ToString(row[RPhaseVoltage]);
            if (NotNullAndNotDBNull(row, YPhaseVoltage)) phasorEntity.YPhaseVoltage = Convert.ToString(row[YPhaseVoltage]);
            if (NotNullAndNotDBNull(row, BPhaseVoltage)) phasorEntity.BPhaseVoltage = Convert.ToString(row[BPhaseVoltage]);
            if (NotNullAndNotDBNull(row, RPhaseCurrent)) phasorEntity.RPhaseCurrent = Convert.ToString(row[RPhaseCurrent]);
            if (NotNullAndNotDBNull(row, YPhaseCurrent)) phasorEntity.YPhaseCurrent = Convert.ToString(row[YPhaseCurrent]);
            if (NotNullAndNotDBNull(row, BPhaseCurrent)) phasorEntity.BPhaseCurrent = Convert.ToString(row[BPhaseCurrent]);
            if (NotNullAndNotDBNull(row, TotalActivePower)) phasorEntity.TotalActivePower = Convert.ToString(row[TotalActivePower]);
            if (NotNullAndNotDBNull(row, TotalApparentPower)) phasorEntity.TotalApparentPower = Convert.ToString(row[TotalApparentPower]);
            if (NotNullAndNotDBNull(row, TotalCapacitivePower)) phasorEntity.TotalCapacitivePower = Convert.ToString(row[TotalCapacitivePower]);
            if (NotNullAndNotDBNull(row, TotalApparentPower)) phasorEntity.TotalApparentPower = Convert.ToString(row[TotalApparentPower]);
            if (NotNullAndNotDBNull(row, RPhasePF)) phasorEntity.RPhasePF = Convert.ToString(row[RPhasePF]);
            if (NotNullAndNotDBNull(row, YPhasePF)) phasorEntity.YPhasePF = Convert.ToString(row[YPhasePF]);
            if (NotNullAndNotDBNull(row, BPhasePF)) phasorEntity.BPhasePF = Convert.ToString(row[BPhasePF]);
            if (NotNullAndNotDBNull(row, TotalInstantaneousPF)) phasorEntity.TotalInstantaneousPF = Convert.ToString(row[TotalInstantaneousPF]);
            if (NotNullAndNotDBNull(row, Frequency)) phasorEntity.Frequency = Convert.ToString(row[Frequency]);
            if (NotNullAndNotDBNull(row, PhaseSequence)) phasorEntity.PhaseSequence = Convert.ToString(row[PhaseSequence]);
            if (NotNullAndNotDBNull(row, TotalkWDirection)) phasorEntity.TotalkWDirection = Convert.ToString(row[TotalkWDirection]);
            if (NotNullAndNotDBNull(row, RPhasekWDirection)) phasorEntity.RPhasekWDirection = Convert.ToString(row[RPhasekWDirection]);
            if (NotNullAndNotDBNull(row, YPhasekWDirection)) phasorEntity.YPhasekWDirection = Convert.ToString(row[YPhasekWDirection]);
            if (NotNullAndNotDBNull(row, BPhasekWDirection)) phasorEntity.BPhasekWDirection = Convert.ToString(row[BPhasekWDirection]);
            if (NotNullAndNotDBNull(row, RPhaseChannel)) phasorEntity.RPhaseChannel = Convert.ToString(row[RPhaseChannel]);
            if (NotNullAndNotDBNull(row, YPhaseChannel)) phasorEntity.YPhaseChannel = Convert.ToString(row[YPhaseChannel]);
            if (NotNullAndNotDBNull(row, BPhaseChannel)) phasorEntity.BPhaseChannel = Convert.ToString(row[BPhaseChannel]);
            if (NotNullAndNotDBNull(row, RPhaseLagLead)) phasorEntity.RPhaseLagLead = Convert.ToString(row[RPhaseLagLead]);
            if (NotNullAndNotDBNull(row, YPhaseLagLead)) phasorEntity.YPhaseLagLead = Convert.ToString(row[YPhaseLagLead]);
            if (NotNullAndNotDBNull(row, BPhaseLagLead)) phasorEntity.BPhaseLagLead = Convert.ToString(row[BPhaseLagLead]);
            if (NotNullAndNotDBNull(row, Total)) phasorEntity.Total = Convert.ToString(row[Total]);
            if (NotNullAndNotDBNull(row, YPhaseAngleWithRPhase)) phasorEntity.YPhaseAngleWithRPhase = Convert.ToString(row[YPhaseAngleWithRPhase]);
            if (NotNullAndNotDBNull(row, BPhaseAngleWithRPhase)) phasorEntity.BPhaseAngleWithRPhase = Convert.ToString(row[BPhaseAngleWithRPhase]);
            if (NotNullAndNotDBNull(row, AngleBWAnyPhasePresent)) phasorEntity.AngleBWAnyPhasePresent = Convert.ToString(row[AngleBWAnyPhasePresent]);           
            UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data Phasor viewed"));
            return phasorEntity;
        }

        public override IEntity InsertData(IList<IEntity> entities)
        {
            throw new NotImplementedException();
        }
    }
}
