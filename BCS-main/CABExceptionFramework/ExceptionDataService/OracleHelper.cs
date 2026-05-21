
/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																			            						|
 * |											Author : Piyush Singh. 									|
 * |											Date   : 22/02/2010 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| */


using System;
using System.Collections;
using System.Data;
using CAB.Framework;
using System.Data.OracleClient;
using CAB.Framework.Utility;

namespace ExceptionServices.Data
{
	public class OracleHelper : IDataHelper 
	{
        public string ConnectionString { get; set; }
        public OracleHelper()
        {
            ConnectionString = ConfigInfo.GetConnectionString();
        }
		
		/// <summary>
		/// Author : Piyush Singh
		/// It execute non query.
		/// </summary>
		/// <param name="request">DataRequest</param>
		/// <returns>int</returns>
        public int ExecuteNonQuery(DataRequest request)
        {

            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand command = new OracleCommand();
            OracleParameter parameter = new OracleParameter();
            int rowsAffected;
            OracleTransaction transaction = null;
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
                        parameter = ConvertParameterToOracleParameter((DataParameter)oEnumerator.Current);
                        command.Parameters.Add(parameter);
                    }
                }
                if (request.IsTransactional)
                {
                    transaction = connection.BeginTransaction();
                }

                rowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception)
            {
                rowsAffected = -2;
                if (request.IsTransactional)
                {
                    transaction.Rollback();
                }
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
		/// It convert parameter to Oracle parameter.
		/// </summary>
		/// <param name="oP">DataRequest.DataParameter</param>
		/// <returns>OracleParameter</returns>
		private OracleParameter ConvertParameterToOracleParameter(DataParameter parameter) 
		{
            OracleParameter oOracleParameter = new OracleParameter(parameter.Name, GetOracleType(parameter.DBType), parameter.Size);
            oOracleParameter.Value = parameter.Value;
            oOracleParameter.Direction = parameter.Direction;
			return oOracleParameter;
		}

		/// <summary>
		/// Author : Piyush Singh
		/// It returns the database type
		/// </summary>
		/// <param name="dbType">System.Data.DbType</param>
		/// <returns>OracleType</returns>
		private OracleType GetOracleType( System.Data.DbType dbType)
		{
			switch (dbType)
			{	
				case DbType.Decimal:
					return OracleType.Number;
				case DbType.Double:
					return OracleType.Double;
                case DbType.Single:
                    return OracleType.Float;
                case DbType.Guid:
					return OracleType.Raw;
				case DbType.Int16:
                    return OracleType.Int16;
				case DbType.Int32:
					return OracleType.Int32;
				case DbType.Int64:
					return OracleType.Number;
				case DbType.AnsiString:
					return OracleType.VarChar;
                case DbType.String:
                    return OracleType.NVarChar;
                case DbType.Binary:
                    return OracleType.Raw;
                case DbType.Boolean:
                    return OracleType.Byte;
                case DbType.Byte:
                    return OracleType.Byte;
                case DbType.Object:
                    return OracleType.Blob;
				default:
					return OracleType.VarChar;  
		
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
            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand command = new OracleCommand();
            OracleParameter parameter = new OracleParameter();
            OracleDataAdapter dataAdapter;
            DataSet outDataSet = inputDataSet;
            OracleTransaction transaction = null;

            connection.Open();
            command.Connection = connection;
            command.CommandText = request.Command;
            command.CommandType = request.CommandType;
            command.CommandTimeout = 300;
            try
            {
                if (request.Parameters != null)
                {
                    IEnumerator oEnumerator = request.Parameters.GetEnumerator();
                    while (oEnumerator.MoveNext())
                    {
                        parameter = ConvertParameterToOracleParameter((DataParameter)oEnumerator.Current);
                        command.Parameters.Add(parameter);
                    }
                }
                if (request.IsTransactional)
                {
                    transaction = connection.BeginTransaction();
                }
                dataAdapter = new OracleDataAdapter();
                dataAdapter.SelectCommand = command;
                dataAdapter.Fill(outDataSet);
            }
            catch (Exception)
            {
                if (request.IsTransactional)
                {
                    transaction.Rollback();
                }
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
            OracleConnection connection = new OracleConnection(connString);
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
            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand command = new OracleCommand();
            OracleParameter parameter = new OracleParameter();
            OracleDataReader outData = null;
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
                        parameter = ConvertParameterToOracleParameter((DataParameter)oEnumerator.Current);
                        command.Parameters.Add(parameter);
                    }
                }
                outData = command.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception)
            {
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
            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand command = new OracleCommand();
            OracleParameter parameter = new OracleParameter();
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
                        parameter = ConvertParameterToOracleParameter((DataParameter)oEnumerator.Current);
                        command.Parameters.Add(parameter);
                    }
                }
                outData = command.ExecuteScalar();
            }
            catch (Exception)
            {
            }
            finally
            {
                connection.Close();
            }

            return outData;
        }
	}
}
