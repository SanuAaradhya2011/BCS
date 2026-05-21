/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 														|
 * | 																												|
 * |											Author : Piyush Singh. 									|
 * |											Date   : 11/02/2010 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| */


using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using CAB.Framework.Entity;
using MySql.Data.MySqlClient;

namespace CAB.DALC.Data.DataServices
{
	public interface IDataHelper
	{
		/// <summary>
		/// Author : Piyush Singh
		/// get and set connection string
		/// </summary>
		string ConnectionString
		{
			get;
			set;
		}
		/// <summary>
		/// Author : Piyush Singh
		/// It Execute Non Query statement.
		/// </summary>
		/// <param name="request">DataRequest</param>
		/// <returns>int</returns>
		int ExecuteNonQuery(DataRequest request);

        /// <summary>
        /// Author : Piyush Singh
        /// It Execute Non Query statement.
        /// </summary>
        /// <param name="request">DataRequest</param>
        /// <returns>int</returns>
        int ExecuteNonQuery(List<DataRequest> requests);

		/// <summary>
		/// Author : Piyush Singh
		/// It Execute Query and fill in dataset.
		/// </summary>
		/// <param name="request">DataRequest</param>
		/// <param name="inputDataSet">DataSet</param>
		/// <returns>DataSet</returns>
		DataSet FillDataSet( DataRequest request, DataSet inputDataSet);
		/// <summary>
		/// Author : Piyush Singh
		/// it checks connection  string is true or not
		/// </summary>
		/// <param name="connString">string</param>
		/// <returns>bool</returns>
		bool TestConnectionString(string connString);
		/// <summary>
		/// Author : Piyush Singh
		/// It execute the reader.
		/// </summary>
		/// <param name="request">DataRequest</param>
		/// <returns>IDataReader</returns>
		IDataReader ExecuteReader(DataRequest request);
        /// <summary>
        /// Author : Piyush Singh
        /// It Execute scalar statement.
        /// </summary>
        /// <param name="request">DataRequest</param>
        /// <returns>object</returns>
		object ExecuteScalar(DataRequest request);
        /// <summary>
        /// bulk insert
        /// </summary>
        /// <param name="query"></param>
        /// <param name="table"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        int BatchInsert( DataTable table, MySqlCommand command);
	}
}
