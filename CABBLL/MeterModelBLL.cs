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
using CAB.Framework;
using CAB.Framework.Entity;
using System;

namespace CAB.BLL
{
    public class MeterModelBLL : IBLL
    {
        private MeterModelDAL meterModelDAL; 
        public MeterModelBLL()
        {
            meterModelDAL = new MeterModelDAL();
        } 
        public DataSet ListDataSet()
        {
            return meterModelDAL.ListDataSet(); 
        } 

        public void InsertDefaultData()
        {
            meterModelDAL.InsertDefaultMeterModel();
        }
        public void InsertMeterModelNumber()
        {
            meterModelDAL.InsertMeterModelNumber();
        }
    }
}



