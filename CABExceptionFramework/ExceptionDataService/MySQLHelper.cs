/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 														|
 * | 																												|
 * |											Author : Piyush Singh. 									|
 * |											Date   : 12/03/2010 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| 
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;
using MySql.Data.MySqlClient;
using CAB.Framework;
using CAB.Framework.Utility;

namespace ExceptionServices.Data
{
    public class MySQLHelper : IDataHelper
    {
        public string ConnectionString { get; set; }
        private ExceptionSerializer serializer;
        ExceptionSerialize serialize;
        public MySQLHelper()
        {
            ConnectionString = ConfigInfo.GetConnectionString();
            serializer = new ExceptionSerializer();
            serialize = new ExceptionSerialize();
        }

        public int ExecuteNonQuery(DataRequest request)
        {
            MySqlConnection connection = new MySqlConnection(ConnectionString);
            MySqlCommand command = new MySqlCommand();
            MySqlParameter parameter = new MySqlParameter();
            int rowsAffected;
            MySqlTransaction transaction = null;
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
            catch (Exception ex)
            {
                rowsAffected = -2;
                if (request.IsTransactional)
                {
                    transaction.Rollback();
                }
                serialize.ExceptionObject = ex;
                serializer.SerializeObject("CABException.txt", serialize);
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

        private MySqlParameter ConvertParameterSQLDbParameter(DataParameter parameter)
        {
            MySqlParameter sqlParameter = new MySqlParameter(parameter.Name, GetSqlDbType(parameter.DBType), parameter.Size);
            sqlParameter.Value = parameter.Value;
            sqlParameter.Direction = parameter.Direction;
            return sqlParameter;
        }

        private MySqlDbType GetSqlDbType(System.Data.DbType dbType)
        {
            switch (dbType)
            {
                case DbType.Decimal:
                    return MySqlDbType.Decimal;
                case DbType.Double:
                    return MySqlDbType.Double;
                case DbType.Single:
                    return MySqlDbType.Float;
                case DbType.Guid:
                    return MySqlDbType.Guid; 
                case DbType.Int16:
                    return MySqlDbType.Int16;
                case DbType.Int32:
                    return MySqlDbType.Int32;
                case DbType.Int64:
                    return MySqlDbType.Int64;
                case DbType.AnsiString:
                    return MySqlDbType.VarChar;
                case DbType.String:
                    return MySqlDbType.VarChar;
                case DbType.Binary:
                    return MySqlDbType.VarBinary;
                case DbType.Boolean:
                    return MySqlDbType.Bit;
                case DbType.Byte:
                    return MySqlDbType.Byte;
                case DbType.Object:
                    return MySqlDbType.VarBinary;
                default:
                    return MySqlDbType.VarChar;
            }
        }

        public DataSet FillDataSet(DataRequest request, DataSet inputDataSet)
        {

            MySqlConnection connection = new MySqlConnection(ConnectionString);
            MySqlCommand command = new MySqlCommand();
            MySqlParameter parameter = new MySqlParameter();
            MySqlTransaction transaction = null;
            MySqlDataAdapter dataAdapter;
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
                dataAdapter = new MySqlDataAdapter();
                dataAdapter.SelectCommand = command;
                dataAdapter.Fill(outDataSet);
            }
            catch (Exception ex)
            {
                if (request.IsTransactional)
                {
                    transaction.Rollback();
                }
                serialize.ExceptionObject = ex;
                serializer.SerializeObject("CABException.txt", serialize);
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
            MySqlConnection connection = new MySqlConnection(connString);
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
            MySqlConnection connection = new MySqlConnection(ConnectionString);
            MySqlCommand command = new MySqlCommand();
            MySqlParameter parameter = new MySqlParameter();
            MySqlDataReader outData = null;
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
            catch (Exception ex)
            {
                serialize.ExceptionObject = ex;
                serializer.SerializeObject("CABException.txt", serialize);
            }
            finally
            {
                connection.Close();
            }
            return outData;
        }

        public object ExecuteScalar(DataRequest request)
        {
            MySqlConnection connection = new MySqlConnection(ConnectionString);
            MySqlCommand command = new MySqlCommand();
            MySqlParameter parameter = new MySqlParameter();
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
            catch (Exception ex)
            {
                serialize.ExceptionObject = ex;
                serializer.SerializeObject("CABException.txt", serialize);
            }
            finally
            {
                connection.Close();
            }
            return outData;
        }
    }
}
