/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 														|
 * | 																												|
 * |											Author : Piyush Singh. 									|
 * |											Date   : 11/02/2010 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| */


using System.Data;

namespace ExceptionServices.Data
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
		/// It Execute Query and fill in dataset.
		/// </summary>
		/// <param name="request">DataRequest</param>
		/// <param name="inputDataSet">DataSet</param>
		/// <returns>DataSet</returns>
		DataSet FillDataSet( DataRequest request, DataSet inputDataSet);
		/// <summary>
		/// Author : Piyush Singh
		/// it checks connection  string is tru or not
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
	}
}
