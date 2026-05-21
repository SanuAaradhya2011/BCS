/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon								|
 * | 																												|
 * |											Author : Mahadevan. 	 											|
 * | 										    Date   : 25 March 2010												|
 * |----------------------------------------------------------------------------------------------------------------| 
 */
using System.Text;
using CAB.DALC.Data.DataServices;
using CAB.Entity.Base;
using CAB.Framework;
using CAB.Framework.Entity;
using CAB.Framework.Utility;
using CAB.Entity;
using System.Data;
using System.Data.Common;

namespace CAB.DALC.Data
{
	public class MeterUnitMasterDAL : DALBase
	{
		private string MeterUnit_ID = "MeterUnit_ID";
		private string MeterUnit_Type = "MeterUnit_Type";

		public override bool InsertData(IEntity entity)
		{
			throw new System.NotImplementedException();
		}

		public override bool UpdateData(IEntity entity)
		{
			throw new System.NotImplementedException();
		}

		public override bool DeleteData(IEntity entity)
		{
			throw new System.NotImplementedException();
		}

		public override IEntity GetDetailData(int id)
		{
			MeterUnitMasterEntity meterUnitMasterEntity = null;
			try
			{
				IDataHelper helper = DatabaseFactory.GetHelper();
				StringBuilder builder = new StringBuilder();
				builder.Append("Select MeterUnit_ID,MeterUnit_Type from meterunit_master where ");
				builder.Append(string.Concat(MeterUnit_ID, "=", ParameterName(MeterUnit_ID)));
				DataRequest request = new DataRequest(builder.ToString());
				request.AddParamter(ParameterName(MeterUnit_ID), id, DbType.String, 20);
				DataSet ds = new DataSet();
				ds = helper.FillDataSet(request, ds);
				if (ds.Tables[0].Rows.Count > 0)
					meterUnitMasterEntity = (MeterUnitMasterEntity)RowToEntity(ds.Tables[0].Rows[0]);
			}
			catch (CABException)
			{
				meterUnitMasterEntity = null;
			}
			return meterUnitMasterEntity;
		}

		public override System.Collections.Generic.IList<IEntity> ListData()
		{
			throw new System.NotImplementedException();
		}

		public override DataSet ListDataSet()
		{
			throw new System.NotImplementedException();
		}

		public override IEntity RowToEntity(DataRow row)
		{
			throw new System.NotImplementedException();
		}

        public override IEntity InsertData(IEntity entity, DbTransaction transaction, DbConnection connection)
        {
            throw new System.NotImplementedException();
        }
    }
}
