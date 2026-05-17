using System;
using LNG.Framework.Entity;
/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											Entity calss for anamoly parameters 																						|
 * |											Author : Deep       									|
 * |											Date   : 24-Apr-2018											|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| */


namespace LNG.Entity
{
    public class LoadSwitchEntity : EntityBase
    {
        #region Constant & Vatiables
        private string controleventforconnectdisconnect;
        private long Realtimeclock;
        private string reasonswitchoperation;
        private string cumulativeenergykwh;
        private string cumulativeenergykwhtz1;
        private string cumulativeenergykwhtz2;
        private string cumulativeenergykwhtz3;
        private string cumulativeenergykwhtz4;
        private string cumulativeenergykwhtz5;
        private string cumulativeenergykwhtz6;
        private string cumulativeenergykwhtz7;
        private string cumulativeenergykwhtz8;
        private string cumulativeenergykvah;
        private string cumulativeenergykvahtz1;
        private string cumulativeenergykvahtz2;
        private string cumulativeenergykvahtz3;
        private string cumulativeenergykvahtz4;
        private string cumulativeenergykvahtz5;
        private string cumulativeenergykvahtz6;
        private string cumulativeenergykvahtz7;
        private string cumulativeenergykvahtz8;
        private long meterData_ID;
        #endregion

        #region Properties
        public string ControlEventConnectDisconnect
        {
            get { return this.controleventforconnectdisconnect; }
            set { this.controleventforconnectdisconnect = value; }
        }

        public long RealTimeClock
        {
            get { return this.Realtimeclock; }
            set { this.Realtimeclock = value; }
        }
       
        public string ReasonSwitchOperation
        {
            get { return this.reasonswitchoperation; }
            set { this.reasonswitchoperation = value; }
        }

        public string CumulativeEnergykwh
        {
            get { return this.cumulativeenergykwh; }
            set { this.cumulativeenergykwh = value; }
        }

        public string CumulativeEnergykwhTZ1
        {
            get { return this.cumulativeenergykwhtz1; }
            set { this.cumulativeenergykwhtz1 = value; }
        }

        public string CumulativeEnergykwhTZ2
        {
            get { return this.cumulativeenergykwhtz2; }
            set { this.cumulativeenergykwhtz2 = value; }
        }

        public string CumulativeEnergykwhTZ3
        {
            get { return this.cumulativeenergykwhtz3; }
            set { this.cumulativeenergykwhtz3 = value; }
        }

        public string CumulativeEnergykwhTZ4
        {
            get { return this.cumulativeenergykwhtz4; }
            set { this.cumulativeenergykwhtz4 = value; }
        }

        public string CumulativeEnergykwhTZ5
        {
            get { return this.cumulativeenergykwhtz5; }
            set { this.cumulativeenergykwhtz5 = value; }
        }

        public string CumulativeEnergykwhTZ6
        {
            get { return this.cumulativeenergykwhtz6; }
            set { this.cumulativeenergykwhtz6 = value; }
        }

        public string CumulativeEnergykwhTZ7
        {
            get { return this.cumulativeenergykwhtz7; }
            set { this.cumulativeenergykwhtz7 = value; }
        }

        public string CumulativeEnergykwhTZ8
        {
            get { return this.cumulativeenergykwhtz8; }
            set { this.cumulativeenergykwhtz8 = value; }
        }

        
        public string CumulativeEnergykvah
        {
            get { return this.cumulativeenergykvah; }
            set { this.cumulativeenergykvah = value; }
        }


        public string CumulativeEnergykvahTZ1
        {
            get { return this.cumulativeenergykvahtz1; }
            set { this.cumulativeenergykvahtz1 = value; }
        }


        public string CumulativeEnergykvahTZ2
        {
            get { return this.cumulativeenergykvahtz2; }
            set { this.cumulativeenergykvahtz2 = value; }
        }
        public string CumulativeEnergykvahTZ3
        {
            get { return this.cumulativeenergykvahtz3; }
            set { this.cumulativeenergykvahtz3 = value; }
        }
        public string CumulativeEnergykvahTZ4
        {
            get { return this.cumulativeenergykvahtz4; }
            set { this.cumulativeenergykvahtz4 = value; }
        }
        public string CumulativeEnergykvahTZ5
        {
            get { return this.cumulativeenergykvahtz5; }
            set { this.cumulativeenergykvahtz5 = value; }
        }
        public string CumulativeEnergykvahTZ6
        {
            get { return this.cumulativeenergykvahtz6; }
            set { this.cumulativeenergykvahtz6 = value; }
        }
        public string CumulativeEnergykvahTZ7
        {
            get { return this.cumulativeenergykvahtz7; }
            set { this.cumulativeenergykvahtz7 = value; }
        }
        public string CumulativeEnergykvahTZ8
        {
            get { return this.cumulativeenergykvahtz8; }
            set { this.cumulativeenergykvahtz8 = value; }
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

