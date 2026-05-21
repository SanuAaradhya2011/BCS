

/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 														|
 * | 																												|
 * |											Author : Piyush Singh. 									|
 * |											Date   : 22/02/2010 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| */


using System;
using System.Collections;
using System.Data;
using CAB.IECFramework;
using System.Data.Odbc;
using CAB.IECFramework.Utility;

namespace ExceptionServices.Data
{
	public class ODBCHelper : IDataHelper 
	{
        public string ConnectionString { get; set; }
        public ODBCHelper()
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

            OdbcConnection connection = new OdbcConnection(ConnectionString);
            OdbcCommand command = new OdbcCommand();
            OdbcParameter parameter = new OdbcParameter();
            int rowsAffected;
            OdbcTransaction transaction = null;
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
                        parameter = ConvertParameterToOdbcParameter((DataParameter)oEnumerator.Current);
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
		/// It convert parameter to Odbc parameter.
		/// </summary>
		/// <param name="oP">DataRequest.DataParameter</param>
		/// <returns>OdbcParameter</returns>
		private OdbcParameter ConvertParameterToOdbcParameter(DataParameter parameter) 
		{
            OdbcParameter oOdbcParameter = new OdbcParameter(parameter.Name, GetOdbcType(parameter.DBType), parameter.Size);
            oOdbcParameter.Value = parameter.Value;
            oOdbcParameter.Direction = parameter.Direction;
			return oOdbcParameter;
		}

		/// <summary>
		/// Author : Piyush Singh
		/// It returns the database type
		/// </summary>
		/// <param name="dbType">System.Data.DbType</param>
		/// <returns>OdbcType</returns>
        private OdbcType GetOdbcType(System.Data.DbType dbType)
        {
            switch (dbType)
            {
                case DbType.Decimal:
                    return OdbcType.Numeric;
                case DbType.Double:
                    return OdbcType.Double;
                case DbType.Single:
                    return OdbcType.Real;
                case DbType.Guid:
                    return OdbcType.UniqueIdentifier;
                case DbType.Int16:
                    return OdbcType.SmallInt;
                case DbType.Int32:
                    return OdbcType.Int;
                case DbType.Int64:
                    return OdbcType.BigInt;
                case DbType.UInt64:
                    return OdbcType.BigInt;
                case DbType.AnsiString:
                    return OdbcType.VarChar;
                case DbType.String:
                    return OdbcType.NVarChar;
                case DbType.Binary:
                    return OdbcType.Binary;
                case DbType.Boolean:
                    return OdbcType.Bit;
                case DbType.Byte:
                    return OdbcType.TinyInt;
                //case DbType.Object:
                //    return OdbcType.bl;
                default:
                    return OdbcType.VarChar;
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
            OdbcConnection connection = new OdbcConnection(ConnectionString);
            OdbcCommand command = new OdbcCommand();
            OdbcParameter parameter = new OdbcParameter();
            OdbcDataAdapter dataAdapter;
            DataSet outDataSet = inputDataSet;
            OdbcTransaction transaction = null;
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
                        parameter = ConvertParameterToOdbcParameter((DataParameter)oEnumerator.Current);
                        command.Parameters.Add(parameter);
                    }
                }
                if (request.IsTransactional)
                {
                    transaction = connection.BeginTransaction();
                }
                dataAdapter = new OdbcDataAdapter();
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
            OdbcConnection connection = new OdbcConnection(connString);
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
            OdbcConnection connection = new OdbcConnection(ConnectionString);
            OdbcCommand command = new OdbcCommand();
            OdbcParameter parameter = new OdbcParameter();
            OdbcDataReader outData = null;
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
                        parameter = ConvertParameterToOdbcParameter((DataParameter)oEnumerator.Current);
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
            OdbcConnection connection = new OdbcConnection(ConnectionString);
            OdbcCommand command = new OdbcCommand();
            OdbcParameter parameter = new OdbcParameter();
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
                        parameter = ConvertParameterToOdbcParameter((DataParameter)oEnumerator.Current);
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
