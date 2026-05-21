#region Namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.Framework.Entity;
#endregion
namespace CAB.Entity
{
    public class FraudEnergyEntity :EntityBase
    {

        #region Constant & Vatiables
        private long fraudEnergy_ID;
        private string magneticInfluenceKWh;
        private string magneticInflueneceKVARhLag;
        private string magneticInflueneceKVARhLead;
        private string magneticInflueneceKVAh;
        private string reverseEnergyKWh;
        private string reverseEnergyKVAh;
        private string reverseEnergyKVARhLag;
        private string reverseEnergyKVARhLead;
        private string thdVoltageRPhase;
        private string thdVoltageBPhase;
        private string thdVoltageYPhase;
        private string thdCurrentRPhase;
        private string thdCurrentBPhase;
        private string thdCurrentYPhase;
        private long meterData_ID;
        #endregion

        #region Properties
        public long FraudEnergy_ID
        {
            get { return this.fraudEnergy_ID; }
            set { this.fraudEnergy_ID = value; }
        }
       
        public string MagneticInfluenceKWh
        {
            get { return this.magneticInfluenceKWh; }
            set { this.magneticInfluenceKWh = value; }
        }
       
        public string MagneticInflueneceKVARhLag
        {
            get { return this.magneticInflueneceKVARhLag; }
            set { this.magneticInflueneceKVARhLag = value; }
        }
        
        public string MagneticInflueneceKVARhLead
        {
            get { return this.magneticInflueneceKVARhLead; }
            set { this.magneticInflueneceKVARhLead = value; }
        }
        
        public string MagneticInflueneceKVAh
        {
            get { return this.magneticInflueneceKVAh; }
            set { this.magneticInflueneceKVAh = value; }
        }
        
        public string ReverseEnergyKWh
        {
            get { return this.reverseEnergyKWh; }
            set { this.reverseEnergyKWh = value; }
        }
        
        public string ReverseEnergyKVAh
        {
            get { return this.reverseEnergyKVAh; }
            set { this.reverseEnergyKVAh = value; }
        }
       
        public string ReverseEnergyKVARhLag
        {
            get { return this.reverseEnergyKVARhLag; }
            set { this.reverseEnergyKVARhLag = value; }
        }
        
        public string ReverseEnergyKVARhLead
        {
            get { return this.reverseEnergyKVARhLead; }
            set { this.reverseEnergyKVARhLead = value; }
        }
        
        public string THDVoltageRPhase
        {
            get { return this.thdVoltageRPhase; }
            set { this.thdVoltageRPhase = value; }
        }
       
        public string THDVoltageYPhase
        {
            get { return this.thdVoltageYPhase; }
            set { this.thdVoltageYPhase = value; }
        }
        
        public string THDVoltageBPhase
        {
            get { return this.thdVoltageBPhase; }
            set { this.thdVoltageBPhase = value; }
        }

        
        public string THDCurrentRPhase
        {
            get { return this.thdCurrentRPhase; }
            set { this.thdCurrentRPhase = value; }
        }

        
        public string THDCurrentYPhase
        {
            get { return this.thdCurrentYPhase; }
            set { this.thdCurrentYPhase = value; }
        }

       
        public string THDCurrentBPhase
        {
            get { return this.thdCurrentBPhase; }
            set { this.thdCurrentBPhase = value; }
        }

        
        public long MeterData_ID
        {
            get { return this.meterData_ID; }
            set { this.meterData_ID = value; }
        }
        public long ReadingDateTime { get; set; }
        public string MeterID { get; set; }
        #endregion
    }
}
