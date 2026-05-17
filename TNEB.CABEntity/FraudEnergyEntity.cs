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
using CAB.IECFramework.Entity;

namespace CAB.Entity
{
    public class IECFraudEnergyEntity : EntityBase
    {
        private long fraudEnergy_ID;
        public long FraudEnergy_ID
        {
            get { return this.fraudEnergy_ID; }
            set { this.fraudEnergy_ID = value; }
        }
        private string magneticInfluenceKWh;
        public string MagneticInfluenceKWh
        {
            get { return this.magneticInfluenceKWh; }
            set { this.magneticInfluenceKWh = value; }
        }
        private string magneticInflueneceKVARhLag;
        public string MagneticInflueneceKVARhLag
        {
            get { return this.magneticInflueneceKVARhLag; }
            set { this.magneticInflueneceKVARhLag = value; }
        }
        private string magneticInflueneceKVARhLead;
        public string MagneticInflueneceKVARhLead
        {
            get { return this.magneticInflueneceKVARhLead; }
            set { this.magneticInflueneceKVARhLead = value; }
        }
        private string magneticInflueneceKVAh;
        public string MagneticInflueneceKVAh
        {
            get { return this.magneticInflueneceKVAh; }
            set { this.magneticInflueneceKVAh = value; }
        }
        private string reverseEnergyKWh;
        public string ReverseEnergyKWh
        {
            get { return this.reverseEnergyKWh; }
            set { this.reverseEnergyKWh = value; }
        }
        private string reverseEnergyKVAh;
        public string ReverseEnergyKVAh
        {
            get { return this.reverseEnergyKVAh; }
            set { this.reverseEnergyKVAh = value; }
        }
        private string reverseEnergyKVARhLag;
        public string ReverseEnergyKVARhLag
        {
            get { return this.reverseEnergyKVARhLag; }
            set { this.reverseEnergyKVARhLag = value; }
        }
        private string reverseEnergyKVARhLead;
        public string ReverseEnergyKVARhLead
        {
            get { return this.reverseEnergyKVARhLead; }
            set { this.reverseEnergyKVARhLead = value; }
        }
        private string thdVoltageRPhase;
        public string THDVoltageRPhase
        {
            get { return this.thdVoltageRPhase; }
            set { this.thdVoltageRPhase = value; }
        }
        private string thdVoltageYPhase;
        public string THDVoltageYPhase
        {
            get { return this.thdVoltageYPhase; }
            set { this.thdVoltageYPhase = value; }
        }
        private string thdVoltageBPhase;
        public string THDVoltageBPhase
        {
            get { return this.thdVoltageBPhase; }
            set { this.thdVoltageBPhase = value; }
        }

        private string thdCurrentRPhase;
        public string THDCurrentRPhase
        {
            get { return this.thdCurrentRPhase; }
            set { this.thdCurrentRPhase = value; }
        }

        private string thdCurrentYPhase;
        public string THDCurrentYPhase
        {
            get { return this.thdCurrentYPhase; }
            set { this.thdCurrentYPhase = value; }
        }

        private string thdCurrentBPhase;
        public string THDCurrentBPhase
        {
            get { return this.thdCurrentBPhase; }
            set { this.thdCurrentBPhase = value; }
        }

        private long meterData_ID;
        public long MeterData_ID
        {
            get { return this.meterData_ID; }
            set { this.meterData_ID = value; }
        }
        public long ReadingDateTime { get; set; }
        public string MeterID { get; set; }

        private string cmriID;
        private string cmriType;
        public string CMRIID
        {
            get
            {
                return cmriID;
            }
            set
            {
                cmriID = value;
            }
        }

        /// <summary>
        /// This property is used to get and set the value of CMRI Type
        /// </summary>
        public string CMRIType
        {
            get
            {
                return cmriType;
            }
            set
            {
                cmriType = value;
            }
        }
    }
}
