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
	class MeterModelMasterDAL : DALBase
	{
		private string MeterModel_ID = "MeterModel_ID";
		private string MeterModel_Name = "MeterModel_Name";

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
			MeterModelMasterEntity meterModelMasterEntity = null;
			try
			{
				IDataHelper helper = DatabaseFactory.GetHelper();
				StringBuilder builder = new StringBuilder();
				builder.Append("Select MeterModel_ID,MeterModel_Name from metermodel_master where ");
				builder.Append(string.Concat(MeterModel_ID, "=", ParameterName(MeterModel_ID)));
				DataRequest request = new DataRequest(builder.ToString());
				request.AddParamter(ParameterName(MeterModel_ID), id, DbType.String, 20);
				DataSet ds = new DataSet();
				ds = helper.FillDataSet(request, ds);
				if (ds.Tables[0].Rows.Count > 0)
					meterModelMasterEntity = (MeterModelMasterEntity)RowToEntity(ds.Tables[0].Rows[0]);
			}
			catch (CABException)
			{
				meterModelMasterEntity = null;
			}
			return meterModelMasterEntity;
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
