/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 												|
 * | 											Date   : 25 March 2010												|
 * |----------------------------------------------------------------------------------------------------------------| 
 */
using System;
using System.Collections.Generic;
using CAB.DALC.Data;
using CAB.Entity;
using System.Text;
using CAB.Framework;
using CAB.Framework.Entity;
using CAB.Framework.Utility;
using System.Data;

namespace CAB.BLL
{
	public class ConsumerMeterDefinitionBLL : IBLL
	{
		MeterMasterDAL meterMasterDAL;
		ConsumerMasterDAL consumerMasterDAL;
		ConsumerMeterDAL consumerMeterDAL;
		SuspectedConsumerDAL suspectedConsumerDAL;

		public ConsumerMeterDefinitionBLL()
		{
			meterMasterDAL = new MeterMasterDAL();
			consumerMasterDAL = new ConsumerMasterDAL();
			consumerMeterDAL = new ConsumerMeterDAL();
			suspectedConsumerDAL = new SuspectedConsumerDAL();
		}

		/// <summary>
		/// To Get the Active Meter values
		/// </summary>
		/// <returns></returns>
		public DataSet GetActiveMeterData()
		{
            DataSet dataSet = meterMasterDAL.ListDataSet();
            return ConvertDataToDateTime(dataSet);
		}

        private DataSet ConvertDataToDateTime(DataSet dataSet)
        {
            DataTable table = new DataTable();
            foreach (DataColumn col in dataSet.Tables[0].Columns)
            {
                string columnName = col.ColumnName;
                table.Columns.Add(new DataColumn(columnName, typeof(System.String)));
            }
            for (int i = 0; i < dataSet.Tables[0].Rows.Count;i++ )
            {
                DataRow dr = dataSet.Tables[0].Rows[i];
                DataRow row = table.NewRow();
                for (int j = 0; j < dataSet.Tables[0].Columns.Count; j++)
                {
                    if(dataSet.Tables[0].Columns[j].ColumnName.ToLower().Contains("date"))
                        row[j] = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(dr[j])).Substring(0,10);
                    else
                        row[j] = dr[j];
                }
                table.Rows.Add(row);
            }
            DataSet dataset = new DataSet();
            dataset.Tables.Add(table);
            return dataset;
        }
		/// <summary>
		/// To Get the InActive Meter Values
		/// </summary>
		/// <returns></returns>
		public DataSet GetInActiveMeterData()
		{
			return meterMasterDAL.ListInactiveMeterDataSet();
		}

		/// <summary>
		/// To Get the Free Consumer Data
		/// </summary>
		/// <returns></returns>
		public DataSet GetFreeConsumerData()
		{
			return meterMasterDAL.ListFreeConsumersDataSet();
		}

		/// <summary>
		/// To Insert the Active Meter Data in to corresponding Tables
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		public bool InsertActiveMeterData(ConsumerMasterEntity consumerMasterEntity, MeterMasterEntity meterMasterEntity, ConsumerMeterEntity consumerMeterEntity)
		{
            return ((meterMasterDAL.InsertData(meterMasterEntity) != null) && (consumerMasterDAL.InsertData(consumerMasterEntity) != null) && (consumerMeterDAL.InsertData(consumerMeterEntity) != null));
		}

		public IEntity InsertSuspectedConsumerData(SuspectedConsumerEntity suspectedConsumerEntity)
		{
			return suspectedConsumerDAL.InsertData(suspectedConsumerEntity);
		}

		public bool DeleteSuspectedConsumerData(SuspectedConsumerEntity suspectedConsumerEntity)
		{
			return suspectedConsumerDAL.DeleteData(suspectedConsumerEntity);
		}

		public DataSet GetInactiveMeterID()
		{
			return meterMasterDAL.ListInactiveMeterID();
		}
		
		public IEntity GetDetailActiveMeterData(string meter_ID)
		{
			return meterMasterDAL.GetDetailData(meter_ID,1);
		}
		public IEntity GetDetailInactiveMeterData(string meter_ID)
		{
			return meterMasterDAL.GetDetailInactiveMeterData(meter_ID);
		}
	}
}
