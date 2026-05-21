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
using LNG.Framework.Entity;

namespace LNG.Entity
{
    public class DLMS650LoadSurveyEntity : EntityBase
    {
        private long loadSurvey_ID;
        private long realTimeClockDateandTime;
        private string rPhaseCurrent;
        private string yPhaseCurrent;
        private string bPhaseCurrent;
        private string averageCurrent;
        private string rPhaseVoltage;
        private string yPhaseVoltage;
        private string bPhaseVoltage;
        private string averageVoltage;
        private string blockEnergykWh;
        private string blockEnergykvarhlag;
        private string blockEnergykvarhlead;
        private string blockEnergykVAh;
        private string powerFactor;
        private string temperature;
        private string tempflag;
        private string avgvolt3phase;
        private string avgrphPF;
        private string avgyphPF;
        private string avgbphPF;
        private string avgtotalPF;
        private string avgneutralcurrent;
        private string thdvr;
        private string thdvy;
        private string thdvb;
        private string thdir;
        private string thdiy;
        private string thdib;

        private string neutralcurrent;//add pradipta_load_neu
        private string avgphasecurrent;

        private long meterData_ID;
        private bool isPadded=false;
        private int mdIntervalPeriod ;
        private int isDLMS;
        private string blockEnergykWhExport;
        private string blockEnergykVAhExport;
        private string blockEnergykvarhlagQ3;
        private string blockEnergykvarhleadQ2;
        private string blockEnergykWhImport;
        private string blockEnergykVAhImport;
        private string blockEnergykWhRPhase;
        private string blockEnergykWhYPhase;
        private string blockEnergykWhBPhase;
        private string blockEnergykvarhlagQ1;
        private string blockEnergykvarhleadQ4;
        private string blockEnergykvarhQ12;
        private string blockEnergykvarhQ34;
        private string blockEnergykvarhQ14;
        private string blockEnergykvarhQ23;
        private string blockEnergyFundamentalkWhAbsolute;
               
        private string netkWh;
        private string netkVAh;
        //added PUMA
        private string frequency;
        private string tamperStatus;

        #region "BRPL HTCT parameter"
        //added for BRPL HTCT meter
        private string activePowerRPhase;
        private string activePowerYPhase;
        private string activePowerBPhase;
        private string apparentPowerRPhase;
        private string apparentPowerYPhase;
        private string apparentPowerBPhase;
        private string reactivePowerRPhase;
        private string reactivePowerYPhase;
        private string reactivePowerBPhase;
        private string powerOffDurationLSIP;
        #endregion

        public bool IsPadded
        {
            get { return isPadded; }
            set { isPadded = value; }
        }
        public long LoadSurvey_ID
        {
            get { return loadSurvey_ID; }
            set { loadSurvey_ID = value; }
        }

        public long RealTimeClockDateandTime
        {
            get { return realTimeClockDateandTime; }
            set { realTimeClockDateandTime = value; }
        }

        public int IsDLMS
        {
            get { return isDLMS; }
            set { isDLMS = value; }
        }
        public int MDIntervalPeriod
        {
            get { return mdIntervalPeriod; }
            set { mdIntervalPeriod = value; }
        }

        public string RPhaseCurrent
        {
            get { return rPhaseCurrent; }
            set { rPhaseCurrent = value; }
        }

        public string YPhaseCurrent
        {
            get { return yPhaseCurrent; }
            set { yPhaseCurrent = value; }
        }

        public string BPhaseCurrent
        {
            get { return bPhaseCurrent; }
            set { bPhaseCurrent = value; }
        }

        public string AverageCurrent
        {
            get { return averageCurrent; }
            set { averageCurrent = value; }
        }

        public string RPhaseVoltage
        {
            get { return rPhaseVoltage; }
            set { rPhaseVoltage = value; }
        }

        public string YPhaseVoltage
        {
            get { return yPhaseVoltage; }
            set { yPhaseVoltage = value; }
        }

        public string BPhaseVoltage
        {
            get { return bPhaseVoltage; }
            set { bPhaseVoltage = value; }
        }

        public string AverageVoltage
        {
            get { return averageVoltage; }
            set { averageVoltage = value; }
        }

        public string BlockEnergykWh
        {
            get { return blockEnergykWh; }
            set { blockEnergykWh = value; }
        }

        public string BlockEnergykvarhlag
        {
            get { return blockEnergykvarhlag; }
            set { blockEnergykvarhlag = value; }
        }

        public string BlockEnergykvarhlead
        {
            get { return blockEnergykvarhlead; }
            set { blockEnergykvarhlead = value; }
        }

        public string BlockEnergykVAh
        {
            get { return blockEnergykVAh; }
            set { blockEnergykVAh = value; }
        }

        public long MeterData_ID
        {
            get { return meterData_ID; }
            set { meterData_ID = value; }
        }

        public string Frequency
        {
            get { return frequency; }
            set { frequency = value; }
        }

        public string TamperStatus
        {
            get { return tamperStatus; }
            set { tamperStatus = value; }
        }
        public string PowerFactor
        {
            get { return powerFactor; }
            set { powerFactor = value; }
        }

        public string Temperature
        {
            get { return temperature; }
            set { temperature = value; }
        }
        public string TemperFlag
        {
            get { return tempflag; }
            set { tempflag = value; }
        }

        public string AVgVolt3phase
        {
            get { return avgvolt3phase; }
            set { avgvolt3phase = value; }
        }


        public string AvgRphPF
        {
            get { return avgrphPF; }
            set { avgrphPF = value; }
        }
        public string AvgYphPF
        {
            get { return avgyphPF; }
            set { avgyphPF = value; }
        }
        public string AvgBphPF
        {
            get { return avgbphPF; }
            set { avgbphPF = value; }
        }
        public string AvgTotalPF
        {
            get { return avgtotalPF; }
            set { avgtotalPF = value; }
        }

        public string AvgNeuCurrent
        {
            get { return avgneutralcurrent; }
            set { avgneutralcurrent = value; }
        }

        public string THDVR
        {
            get { return thdvr; }
            set { thdvr = value; }
        }

        public string THDVY
        {
            get { return thdvy; }
            set { thdvy = value; }
        }
        public string THDVB
        {
            get { return thdvb; }
            set { thdvb = value; }
        }
        public string THDIR
        {
            get { return thdir; }
            set { thdir = value; }
        }
        public string THDIY
        {
            get { return thdiy; }
            set { thdiy = value; }
        }
        public string THDIB
        {
            get { return thdib; }
            set { thdib = value; }
        }

        public string NeuCurrent//add pradipta_load_neu
        {
            get { return neutralcurrent; }
            set { neutralcurrent = value; }
        }

        public string AvgPhaseCurrent
        {
            get { return avgphasecurrent; }
            set { avgphasecurrent = value; }
        }

        #region "Net Metering Parameters
        public string BlockEnergykWhExport
        {
            get { return blockEnergykWhExport; }
            set { blockEnergykWhExport = value; }
        }

        public string BlockEnergykWhImport
        {
            get { return blockEnergykWhImport; }
            set { blockEnergykWhImport = value; }
        }

        public string BlockEnergykWhRPhase
        {
            get { return blockEnergykWhRPhase; }
            set { blockEnergykWhRPhase = value; }
        }

        public string BlockEnergykWhYPhase
        {
            get { return blockEnergykWhYPhase; }
            set { blockEnergykWhYPhase = value; }
        }

        public string BlockEnergykWhBPhase
        {
            get { return blockEnergykWhBPhase; }
            set { blockEnergykWhBPhase = value; }
        }

        public string BlockEnergykVAhExport
        {
            get { return blockEnergykVAhExport; }
            set { blockEnergykVAhExport = value; }
        }

        public string BlockEnergykVAhImport
        {
            get { return blockEnergykVAhImport; }
            set { blockEnergykVAhImport = value; }
        }             

        public string BlockEnergykvarhlagQ3
        {
            get { return blockEnergykvarhlagQ3; }
            set { blockEnergykvarhlagQ3 = value; }
        }

        public string BlockEnergykvarhleadQ2
        {
            get { return blockEnergykvarhleadQ2; }
            set { blockEnergykvarhleadQ2 = value; }
        }

        public string BlockEnergykvarhlagQ1
        {
            get { return blockEnergykvarhlagQ1; }
            set { blockEnergykvarhlagQ1 = value; }
        }

        public string BlockEnergykvarhleadQ4
        {
            get { return blockEnergykvarhleadQ4; }
            set { blockEnergykvarhleadQ4 = value; }
        }

        public string BlockEnergykvarhQ12
        {
            get { return blockEnergykvarhQ12; }
            set { blockEnergykvarhQ12 = value; }
        }

        public string BlockEnergykvarhQ34
        {
            get { return blockEnergykvarhQ34; }
            set { blockEnergykvarhQ34 = value; }
        }

        public string BlockEnergykvarhQ14
        {
            get { return blockEnergykvarhQ14; }
            set { blockEnergykvarhQ14 = value; }
        }

        public string BlockEnergykvarhQ23
        {
            get { return blockEnergykvarhQ23; }
            set { blockEnergykvarhQ23 = value; }
        }

        public string BlockEnergyFundamentalkWhAbsolute
        {
            get { return blockEnergyFundamentalkWhAbsolute; }
            set { blockEnergyFundamentalkWhAbsolute = value; }
        }             

        public string NetkWh
        {
            get { return netkWh; }
            set { netkWh = value; }
        }

        public string NetkVAh
        {
            get { return netkVAh; }
            set { netkVAh = value; }
        }

      

        #endregion

        #region "BRPL HTCT parameter"
        public string ActivePowerRPhase
        {
            get { return activePowerRPhase; }
            set { activePowerRPhase = value; }
        }

        public string ActivePowerYPhase
        {
            get { return activePowerYPhase; }
            set { activePowerYPhase = value; }
        }

        public string ActivePowerBPhase
        {
            get { return activePowerBPhase; }
            set { activePowerBPhase = value; }
        }

        public string ApparentPowerRPhase
        {
            get { return apparentPowerRPhase; }
            set { apparentPowerRPhase = value; }
        }

        public string ApparentPowerYPhase
        {
            get { return apparentPowerYPhase; }
            set { apparentPowerYPhase = value; }
        }

        public string ApparentPowerBPhase
        {
            get { return apparentPowerBPhase; }
            set { apparentPowerBPhase = value; }
        }

        public string ReactivePowerRPhase
        {
            get { return reactivePowerRPhase; }
            set { reactivePowerRPhase = value; }
        }

        public string ReactivePowerYPhase
        {
            get { return reactivePowerYPhase; }
            set { reactivePowerYPhase = value; }
        }

        public string ReactivePowerBPhase
        {
            get { return reactivePowerBPhase; }
            set { reactivePowerBPhase = value; }
        }

        public string PowerOffDurationLSIP
        {
            get { return powerOffDurationLSIP; }
            set { powerOffDurationLSIP = value; }
        }
        #endregion

    }
}
