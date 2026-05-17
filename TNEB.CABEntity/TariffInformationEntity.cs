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
    public class TariffInformationEntity : EntityBase
    {
        private long tariffInformation_ID;
        public long TariffInformation_ID
        {
            get { return this.tariffInformation_ID; }
            set { this.tariffInformation_ID = value; }
        }
        private string averagePowerFactor;
        public string AveragePowerFactor
        {
            get { return this.averagePowerFactor; }
            set { this.averagePowerFactor = value; }
        }
        private int tariff_Number;
        public int Tariff_Number
        {
            get { return this.tariff_Number; }
            set { this.tariff_Number = value; }
        }
        private long history_ID;
        public long History_ID
        {
            get { return this.history_ID; }
            set { this.history_ID = value; }
        }
    }
}
