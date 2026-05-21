using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using CAB.IECFramework;
using CAB.IECFramework.Utility;
using CAB.DALC.Data.DataServices;
using System.Data.Common;

namespace CAB.DALC.SQLDB.DataServices
{
    public class SqldbHelper : IDataHelper
    {
        public string ConnectionString{get;set;}

        public SqldbHelper()
        {
            ConnectionString = ConfigInfo.GetConnectionString();
        }
 
        public int ExecuteNonQuery(DataRequest request)
        {

            SqlConnection connection = new SqlConnection(ConnectionString);
            SqlCommand command = new SqlCommand();
            SqlParameter parameter = new SqlParameter();
            int rowsAffected;
            SqlTransaction transaction = null;
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
                            parameter = ConvertParameterSQLDbParameter((DataParameter)oEnumerator.Current);
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

        public int ExecuteNonQuery(List<DataRequest> requests)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            SqlCommand command = new SqlCommand();
            SqlParameter parameter = new SqlParameter();
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
                        foreach(DataParameter parameters in request.Parameters) 
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

        private SqlParameter ConvertParameterSQLDbParameter(DataParameter parameter)
        {
            SqlParameter sqlParameter = new SqlParameter(parameter.Name, GetSqlDbType(parameter.DBType), parameter.Size);
            sqlParameter.Value = parameter.Value;
            sqlParameter.Direction = parameter.Direction;
            return sqlParameter;
        }
 
        private SqlDbType GetSqlDbType(System.Data.DbType dbType)
        {
            switch (dbType)
            {
                case DbType.Decimal:
                    return SqlDbType.Decimal;
                case DbType.Double:
                    return SqlDbType.Float;
                case DbType.Single:
                    return SqlDbType.Real;
                case DbType.Guid:
                    return SqlDbType.UniqueIdentifier;
                case DbType.Int16:
                    return SqlDbType.SmallInt;
                case DbType.Int32:
                    return SqlDbType.Int;
                case DbType.Int64:
                    return SqlDbType.BigInt;
                case DbType.AnsiString:
                    return SqlDbType.VarChar;
                case DbType.String:
                    return SqlDbType.NVarChar;
                case DbType.Binary:
                    return SqlDbType.VarBinary;
                case DbType.Boolean:
                    return SqlDbType.Bit;
                case DbType.Byte:
                    return SqlDbType.TinyInt;
                case DbType.Object:
                    return SqlDbType.Variant;
                default:
                    return SqlDbType.VarChar;
            }
        }

        public DataSet FillDataSet(DataRequest request, DataSet inputDataSet)
        {

            SqlConnection connection = new SqlConnection(ConnectionString);
            SqlCommand command = new SqlCommand();
            SqlParameter parameter = new SqlParameter();
            SqlTransaction transaction = null;
            SqlDataAdapter dataAdapter;
            DataSet outDataSet = inputDataSet;
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
                        parameter = ConvertParameterSQLDbParameter((DataParameter)oEnumerator.Current);
                        command.Parameters.Add(parameter);
                    }
                }
                if (request.IsTransactional)
                {
                    transaction = connection.BeginTransaction();
                }
                dataAdapter = new SqlDataAdapter();
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
 
        public bool TestConnectionString(string connString)
        { 
            bool trusted;
            SqlConnection connection = new SqlConnection(connString);
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

        public IDataReader ExecuteReader(DataRequest request)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            SqlCommand command = new SqlCommand();
            SqlParameter parameter = new SqlParameter();
            SqlDataReader outData = null;
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
                        parameter = ConvertParameterSQLDbParameter((DataParameter)oEnumerator.Current);
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

        public object ExecuteScalar(DataRequest request)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            SqlCommand command = new SqlCommand();
            SqlParameter parameter = new SqlParameter();
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
                        parameter = ConvertParameterSQLDbParameter((DataParameter)oEnumerator.Current);
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
            return (outData);
        }

    }
} 
