/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 														|
 * | 																												|
 * |											Author : Piyush Singh. 									|
 * |											Date   : 11/02/2010 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| */


using System;
using System.Collections;
using System.Data;
using CAB.IECFramework;
using System.Data.OleDb;
using CAB.IECFramework.Utility;
using CAB.DALC.Data.DataServices;
using System.Data.Common;
using System.Collections.Generic;

namespace CAB.DALC.OLEDB.DataServices
{
	public class OledbHelper : IDataHelper 
	{
        public string ConnectionString { get; set; }
        public OledbHelper()
        {
            ConnectionString = ConfigInfo.GetConnectionString();
        }

        public int ExecuteNonQuery(List<DataRequest> requests)
        {
            OleDbConnection connection = new OleDbConnection(ConnectionString);
            OleDbCommand command = new OleDbCommand();
            OleDbParameter parameter = new OleDbParameter();
            int rowsAffected;
            try
            {
                connection.Open();
                command.Connection = connection;
                foreach (DataRequest request in requests)
                {
                    command.CommandText = request.Command;
                    command.CommandType = request.CommandType;
                    if (request.Parameters != null)
                    {
                        foreach (DataParameter parameters in request.Parameters)
                        {
                            command.Parameters.Add(parameters);
                        }
                    }
                    rowsAffected = command.ExecuteNonQuery();
                }
            }
            catch (Exception exception)
            {
                new CABException(exception);
            }
            finally
            {
                connection.Close();
            }
            return 0;
        }
		/// <summary>
		/// Author : Piyush Singh
		/// It execute non query.
		/// </summary>
		/// <param name="request">DataRequest</param>
		/// <returns>int</returns>
        public int ExecuteNonQuery(DataRequest request)
        {

            OleDbConnection connection = new OleDbConnection(ConnectionString);
            OleDbCommand command = new OleDbCommand();
            OleDbParameter parameter = new OleDbParameter();
            int rowsAffected;
            OleDbTransaction transaction = null;
            try
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = request.Command;
                command.CommandType = request.CommandType;
                if (request.Parameters != null)
                {
                    IEnumerator oEnumerator = request.Parameters.GetEnumerator();

                    while (oEnumerator.MoveNext())
                    {
                        parameter = ConvertParameterToOleDbParameter((DataParameter)oEnumerator.Current);
                        command.Parameters.Add(parameter);
                    }
                }
                if (request.IsTransactional)
                {
                    transaction = connection.BeginTransaction();
                }

                rowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception exception)
            {
                rowsAffected = -2;
                if (request.IsTransactional)
                {
                    transaction.Rollback();
                }
                  new CABException(exception);
            }
            finally
            {
                if (request.IsTransactional)
                {
                    transaction.Commit();
                }
                connection.Close();
            }
            return rowsAffected;
        }
		
		/// <summary>
		/// Author : Piyush Singh
		/// It convert parameter to oledb parameter.
		/// </summary>
		/// <param name="oP">DataRequest.DataParameter</param>
		/// <returns>OleDbParameter</returns>
		private OleDbParameter ConvertParameterToOleDbParameter(DataParameter parameter) 
		{
            OleDbParameter oOleDbParameter = new OleDbParameter(parameter.Name, GetOleDbType(parameter.DBType), parameter.Size);
            oOleDbParameter.Value = parameter.Value;
            oOleDbParameter.Direction = parameter.Direction;
			return oOleDbParameter;
		}

		/// <summary>
		/// Author : Piyush Singh
		/// It returns the database type
		/// </summary>
		/// <param name="dbType">System.Data.DbType</param>
		/// <returns>OleDbType</returns>
        private OleDbType GetOleDbType(System.Data.DbType dbType)
        {
            switch (dbType)
            {
                case DbType.Decimal:
                    return OleDbType.Decimal;
                case DbType.Double:
                    return OleDbType.Double;
                case DbType.Single:
                    return OleDbType.Single;
                case DbType.Guid:
                    return OleDbType.Guid;
                case DbType.Int16:
                    return OleDbType.SmallInt;
                case DbType.Int32:
                    return OleDbType.Integer;
                case DbType.Int64:
                    return OleDbType.BigInt;
                case DbType.AnsiString:
                    return OleDbType.VarChar;
                case DbType.String:
                    return OleDbType.VarWChar;
                case DbType.Binary:
                    return OleDbType.VarBinary;
                case DbType.Boolean:
                    return OleDbType.Boolean;
                case DbType.Byte:
                    return OleDbType.UnsignedTinyInt;
                case DbType.Object:
                    return OleDbType.Variant;
                default:
                    return OleDbType.VarChar;
            }
        }

		/// <summary>
		/// Author : Piyush Singh
		/// it execute the query and fill in dataset.
		/// </summary>
		/// <param name="request">DataRequest</param>
		/// <param name="inputDataSet">DataSet</param>
		/// <returns>DataSet</returns>
        public DataSet FillDataSet(DataRequest request, DataSet inputDataSet)
        {
            OleDbConnection connection = new OleDbConnection(ConnectionString);
            OleDbCommand command = new OleDbCommand();
            OleDbParameter parameter = new OleDbParameter();
            OleDbDataAdapter dataAdapter;
            DataSet outDataSet = inputDataSet;
            OleDbTransaction transaction = null;
            try
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = request.Command;
                command.CommandType = request.CommandType;
                command.CommandTimeout = 300;
                if (request.Parameters != null)
                {
                    IEnumerator oEnumerator = request.Parameters.GetEnumerator();
                    while (oEnumerator.MoveNext())
                    {
                        parameter = ConvertParameterToOleDbParameter((DataParameter)oEnumerator.Current);
                        command.Parameters.Add(parameter);
                    }
                }
                if (request.IsTransactional)
                {
                    transaction = connection.BeginTransaction();
                }
                dataAdapter = new OleDbDataAdapter();
                dataAdapter.SelectCommand = command;
                dataAdapter.Fill(outDataSet);
            }
            catch (Exception exception)
            {
                if (request.IsTransactional)
                {
                    transaction.Rollback();
                }
                  new CABException(exception);
            }
            finally
            {
                if (request.IsTransactional)
                {
                    transaction.Commit();
                }
                connection.Close();
            }
            return outDataSet;
        }
		
		/// <summary>
		/// Author : Piyush Singh
		/// It test connection string is ok or not.
		/// </summary>
		/// <param name="connString">string</param>
		/// <returns><bool/returns>
		public bool TestConnectionString(string connString)
		{
			bool trusted;
            OleDbConnection connection = new OleDbConnection(connString);
				try
				{
					connection.Open();
                    trusted = true;
				}
				catch (Exception)
				{
                    trusted = false;
				}
            return trusted;
		}

		/// <summary>
		/// Author : Piyush Singh
		/// It execute the data reader.
		/// </summary>
		/// <param name="request">DataRequest</param>
		/// <returns>IDataReader</returns>
		public IDataReader ExecuteReader(DataRequest request)
		{
			OleDbConnection  connection = new OleDbConnection(ConnectionString);
			OleDbCommand command = new OleDbCommand(); 
			OleDbParameter  parameter = new OleDbParameter();
			OleDbDataReader  outData = null;
			try
			{			
			connection.Open();
			command.Connection = connection;
			command.CommandText = request.Command;
			command.CommandType = request.CommandType;
				if (request.Parameters != null) 
				{
					IEnumerator oEnumerator = request.Parameters.GetEnumerator();
					while( oEnumerator.MoveNext() )
					{	
						parameter = ConvertParameterToOleDbParameter ((DataParameter)oEnumerator.Current);
						command.Parameters.Add(parameter);
					}
				}
				outData = command.ExecuteReader(CommandBehavior.CloseConnection); 
			}
            catch (Exception exception)
			{
                  new CABException(exception);
			}
			finally
			{
                connection.Close();
			}
            return outData;
		}
		
		/// <summary>
		/// Author : Piyush Singh
		/// It execute the scalar
		/// </summary>
		/// <param name="request">DataRequest</param>
		/// <returns>object</returns>
        public object ExecuteScalar(DataRequest request)
        {
            OleDbConnection connection = new OleDbConnection(ConnectionString);
            OleDbCommand command = new OleDbCommand();
            OleDbParameter parameter = new OleDbParameter();
            object outData = null;
            try
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = request.Command;
                command.CommandType = request.CommandType;
                if (request.Parameters != null)
                {
                    IEnumerator oEnumerator = request.Parameters.GetEnumerator();
                    while (oEnumerator.MoveNext())
                    {
                        parameter = ConvertParameterToOleDbParameter((DataParameter)oEnumerator.Current);
                        command.Parameters.Add(parameter);
                    }
                }
                outData = command.ExecuteScalar();
            }
            catch (Exception exception)
            {
                  new CABException(exception);
            }
            finally
            {
                connection.Close();
            }
            return outData;
        } 
    }
}
