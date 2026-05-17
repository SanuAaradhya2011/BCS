/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh										|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| 
 */

using System.Data;
using CAB.DALC.Data;
using CAB.IECFramework;
using CAB.IECFramework.Entity;
using System;

namespace CAB.BLL
{
    public class MeterUnitBLL : IBLL
    {
        private MeterUnitDAL meterUnitDAL;
        public MeterUnitBLL()
        {
            meterUnitDAL = new MeterUnitDAL();
        } 
        public DataSet ListDataSet()
        {
            return meterUnitDAL.ListDataSet(); 
        } 

        public void InsertDefaultData()
        {
            meterUnitDAL.InsertDefaultMeterUnit();
        }
    }
}




