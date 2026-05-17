
/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 									        |
 * |											Date   : 25/03/2010 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| 
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using CAB.DALC.Data.DataServices;
using CAB.Entity;
using CAB.Framework;
using CAB.Framework.Entity;
using System.Data.Common;
using Hunt.EPIC.Logging;

namespace CAB.DALC.Data
{
    public class MeterUnitDAL : DALBase
    {
        private string MeterUnit_ID = "MeterUnit_ID";
        private string MeterUnit_Type = "MeterUnit_Type";
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(MeterUnitDAL).ToString());

        public void InsertDefaultMeterUnit()
        {
            string[] qry = new string[4];
            qry[0] = "Insert Into meterunit_master(MeterUnit_Type) values('kW')";
            qry[1] = "Insert Into meterunit_master(MeterUnit_Type) values('kVA')";
            qry[2] = "Insert Into meterunit_master(MeterUnit_Type) values('MW')";
            qry[3] = "Insert Into meterunit_master(MeterUnit_Type) values('MVA')";
            IDataHelper helper = DatabaseFactory.GetHelper();
            for (int i = 0; i < 4; i++)
            {
                DataRequest request = new DataRequest(qry[i]);
                helper.ExecuteNonQuery(request);
            }
        }
        public override DataSet ListDataSet()
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select MeterUnit_ID,MeterUnit_Type from meterunit_master");
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter unit viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ListDataSet()", ex);
                dataSet = null;
            }
            return dataSet;
        }
        public override IEntity InsertData(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public override IEntity InsertData(IList<IEntity> entities)
        {
            throw new NotImplementedException();
        }

        public override bool UpdateData(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteData(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public override IEntity GetDetailData(int id)
        {
            throw new NotImplementedException();
        }

        public override IList<IEntity> ListData()
        {
            throw new NotImplementedException();
        }

       

        public override IEntity RowToEntity(DataRow row)
        {
            throw new NotImplementedException();
        }
    }
}

