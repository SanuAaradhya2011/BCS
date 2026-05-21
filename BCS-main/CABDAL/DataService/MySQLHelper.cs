
/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 														|
 * | 																												|
 * |											Author : Piyush Singh. 									|
 * |											Date   : 22/02/2010 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| */

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;
using MySql.Data.MySqlClient;
using CAB.Framework;
using CAB.Framework.Utility;
using CAB.DALC.Data.DataServices;
using System.Data.Common;
using CAB.Framework.Entity;
using Hunt.EPIC.Logging;

namespace CAB.DALC.SQLDB.DataServices
{
    public class MySQLHelper : IDataHelper
    {
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(MySQLHelper).ToString());
        public string ConnectionString { get; set; }
        public string ConnectionStringForSP { get; set; }

        public MySQLHelper()
        {
            ConnectionString = ConfigInfo.GetConnectionString();
            ConnectionStringForSP = ConfigInfo.GetConnectionStringForSp();
        }

        public int ExecuteNonQuery(List<DataRequest> requests)
        {
            MySqlConnection connection = new MySqlConnection(ConnectionString);
            MySqlParameter parameter = new MySqlParameter();
            int rowsAffected;
            try
            {
                connection.Open();

                foreach (DataRequest request in requests)
                {
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = connection;
                    command.CommandText = request.Command;
                    command.CommandType = request.CommandType;
                    if (request.Parameters != null)
                    {
                        foreach (DataParameter parameters in request.Parameters)
                        {
                            command.Parameters.Add(ConvertParameterSQLDbParameter(parameters));
                        }
                    }
                    rowsAffected = command.ExecuteNonQuery();
                }
            }
            catch (Exception exception)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, " ExecuteNonQuery(List<DataRequest> requests)", exception);
                new CABException(exception);
            }
            finally
            {
                connection.Close();
            }
            return 0;
        }
        /// <summary>
        /// to insert bulk data
        /// </summary>
        /// <param name="query"></param>
        /// <param name="table"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public int BatchInsert(DataTable table, MySqlCommand command)
        {
            MySqlConnection connection = new MySqlConnection(ConnectionString);
            MySqlParameter parameter = new MySqlParameter();
            MySqlDataAdapter dataAdapter = new MySqlDataAdapter();
            int records = 0;
            try
            {
                command.UpdatedRowSource = UpdateRowSource.None;
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                dataAdapter.InsertCommand = command;
                dataAdapter.UpdateBatchSize = 1000;
                records = dataAdapter.Update(table);

            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "BatchInsert(DataTable table, MySqlCommand command)", ex);
                throw (ex);
                //   new CABException(exception);
            }
            finally
            {
                connection.Close();
            }
            return records;
        }


        public int ExecuteNonQuery(DataRequest request)
        {
            //If Query type is stored procedure then pick the appropriate connection string.
            MySqlConnection connection = new MySqlConnection(request.CommandType == CommandType.StoredProcedure ? ConnectionStringForSP : ConnectionString);
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
            catch (MySqlException exception)    //Exception log for catch block
            {
                rowsAffected = -2;
                if (request.IsTransactional)
                {
                    transaction.Rollback();
                }
                if (exception.Number == 1042 || exception.Number == 1045)
                {
                    return -5;
                }
                new CABException(exception);
                logger.Log(LOGLEVELS.Error, "ExecuteNonQuery(DataRequest request)", exception);
            }
            catch (Exception exception)    //Exception log for catch block
            {
                rowsAffected = -2;
                if (request.IsTransactional)
                {
                    transaction.Rollback();
                }
                new CABException(exception);
                logger.Log(LOGLEVELS.Error, "ExecuteNonQuery(DataRequest request)", exception);
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
            MySqlConnection connection = null;
            if (request.CommandType == CommandType.StoredProcedure)
            {
                connection = new MySqlConnection(ConnectionStringForSP);
            }
            else
            {
                connection = new MySqlConnection(ConnectionString);
            }
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
            catch (Exception exception)    //Exception log for catch block
            {
                if (request.IsTransactional)
                {
                    transaction.Rollback();
                }
                new CABException(exception);
                logger.Log(LOGLEVELS.Error, "FillDataSet(DataRequest request, DataSet inputDataSet)", exception);
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
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "TestConnectionString(string connString)", ex);
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
            catch (Exception exception)    //Exception log for catch block
            {
                new CABException(exception);
                logger.Log(LOGLEVELS.Error, "ExecuteReader(DataRequest request)", exception);
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
            catch (Exception exception)    //Exception log for catch block
            {
                new CABException(exception);
                logger.Log(LOGLEVELS.Error, "ExecuteScalar(DataRequest request)", exception);
            }
            finally
            {
                connection.Close();
            }
            return outData;
        }

    }
}
