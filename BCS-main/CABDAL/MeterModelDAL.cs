
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
    public class MeterModelDAL : DALBase
    {
        private string MeterModel_ID = "MeterModel_ID";
        private string MeterModel_Name = "MeterModel_Name";
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(MeterModelDAL).ToString());

        public void InsertDefaultMeterModel()
        {  
            string[] qry = new string[3];
            //qry[0] = "Insert Into metermodel_master(MeterModel_Name) values('E-150')";
            //qry[1] = "Insert Into metermodel_master(MeterModel_Name) values('E-250')";
            qry[0] = "Insert Into metermodel_master(MeterModel_Name) values('E-250')";
            qry[1] = "Insert Into metermodel_master(MeterModel_Name) values('E-650-HT')";
            qry[2] = "Insert Into metermodel_master(MeterModel_Name) values('E-650-LT')";
            
            IDataHelper helper = DatabaseFactory.GetHelper();
            for (int i = 0; i < 3; i++)
            {
                DataRequest request = new DataRequest(qry[i]);
                helper.ExecuteNonQuery(request);
            }
        }
        //VBM - Insert meter model number
        public void InsertMeterModelNumber()
        {
            List<string> query = new List<string>();            
            query.Add("Insert Into metermodel_master(MeterModel_Name) values('"+NamePlateConstants.RubyE250+"')");
            query.Add("Insert Into metermodel_master(MeterModel_Name) values('" + NamePlateConstants.PumaLTE650 + "')");
            query.Add("Insert Into metermodel_master(MeterModel_Name) values('"+NamePlateConstants.PumaHTE650+"')");
            query.Add("Insert Into metermodel_master(MeterModel_Name) values('" + NamePlateConstants.LTCTCortex + "')");
            query.Add("Insert Into metermodel_master(MeterModel_Name) values('" + NamePlateConstants.HTCTCortex + "')");
            query.Add("Insert Into metermodel_master(MeterModel_Name) values('" + NamePlateConstants.Ruby6Val + "')");
            query.Add("Insert Into metermodel_master(MeterModel_Name) values('" + NamePlateConstants.E350Val + "')");
            query.Add("Insert Into metermodel_master(MeterModel_Name) values('" + NamePlateConstants.E150Val + "')");
            query.Add("Insert Into metermodel_master(MeterModel_Name) values('" + NamePlateConstants.WBVal + "')");
            query.Add("Insert Into metermodel_master(MeterModel_Name) values('" + NamePlateConstants.PumaHTE650MW + "')");
            query.Add("Insert Into metermodel_master(MeterModel_Name) values('" + NamePlateConstants.Sapphire + "')");
            query.Add("Insert Into metermodel_master(MeterModel_Name) values('" + NamePlateConstants.Ruby6ukModel + "')");
            query.Add("Insert Into metermodel_master(MeterModel_Name) values('" + NamePlateConstants.TNModel + "')");
            query.Add("Insert Into metermodel_master(MeterModel_Name) values('" + NamePlateConstants.TwoTOUltModel + "')");
            query.Add("Insert Into metermodel_master(MeterModel_Name) values('" + NamePlateConstants.WBLTVal + "')");
            query.Add("Insert Into metermodel_master(MeterModel_Name) values('" + NamePlateConstants.E150Val + "')");
            query.Add("Insert Into metermodel_master(MeterModel_Name) values('" + NamePlateConstants.Sapphire + "')");
            query.Add("Insert Into metermodel_master(MeterModel_Name) values('" + NamePlateConstants.SapphireSTVal + "')");
            
            IDataHelper helper = DatabaseFactory.GetHelper();
            foreach(string record  in query )
            {
                DataRequest request = new DataRequest(record);
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
                builder.Append("Select MeterModel_ID,MeterModel_Name from metermodel_master");
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter model viewed"));
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
