/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 														|
 * | 																												|
 * |											Author : Piyush Singh. 									|
 * |											Date   : 26/02/2010 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| */

using CAB.DALC.ODBC.DataServices;
using CAB.DALC.OLEDB.DataServices;
using CAB.DALC.Oracle.DataServices;
using CAB.DALC.SQLDB.DataServices;
using CAB.Framework.Utility;
using CAB.Framework.Utility.DataBaseType;
using System.Data.Common;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Data.OleDb;
using System.Data.OracleClient;

namespace CAB.DALC.Data.DataServices
{
    /// <summary>
    /// This class is used to get the database Factory. To Do the DB Operation.
    /// </summary>
    public class DatabaseFactory
    {
        /// <summary>
        ///  This method is used to get the database Factory. To Do the DB Operation.
        /// </summary>
        /// <returns>IDataHelper</returns>
        public static IDataHelper GetHelper()
        {
            DBType dbType = ConfigInfo.GetDBType();
            IDataHelper helper = null;
            if (dbType.Equals(DBType.MS_SQL))
                helper = new SqldbHelper();
            else if (dbType.Equals(DBType.MS_Access)) 
                helper = new OledbHelper();
            else if (dbType.Equals(DBType.Oracle))
                helper = new OracleHelper();
            else if (dbType.Equals(DBType.My_SQL))
                helper = new MySQLHelper();
            return helper;
        }

        /// <summary>
        ///  This method is used to get the database Place holder to create DB parameter.
        /// </summary>
        /// <returns></returns>
        public static string GetPlaceholder()
        {
            DBType dbType = ConfigInfo.GetDBType();
            if (dbType.Equals(DBType.MS_SQL))
                return "@";
            else if (dbType.Equals(DBType.Oracle))
                return ":";
            else if (dbType.Equals(DBType.My_SQL))
                return "?";
            else if (dbType.Equals(DBType.MS_Access))
                return "@";
            else
                return string.Empty;
        }

        public static DbConnection GetConnection()
        {
            string ConnectionString = ConfigInfo.GetConnectionString();
            DbConnection connection=null;
            DBType dbType = ConfigInfo.GetDBType();
            if (dbType.Equals(DBType.MS_SQL))
                connection = new SqlConnection(ConnectionString);
            else if (dbType.Equals(DBType.MS_Access))
                connection = new OleDbConnection(ConnectionString);
            else if (dbType.Equals(DBType.Oracle))
                connection = new OracleConnection(ConnectionString);
            else if (dbType.Equals(DBType.My_SQL))
                connection = new MySqlConnection(ConnectionString);
            return connection;
        }

        #region "GPRS SPECFIC"
        //Following code will be used by BCS to identify the GPRS Database.
        //Database for GPRS could be SQL SERVER or MYSQL any.

        /// <summary>
        ///  This method is used to get the database Factory. 
        ///  Method will pick M2MDatabase type from config and returns appropriate Handler for same.
        /// </summary>
        /// <returns>IDataHelper</returns>
        public static IDataHelper GetDBHelperForGPRSAdapter()
        {
            IDataHelper helper = null;

            DBType dbType = ConfigInfo.GetGPRSAdapterDBType();
            if (dbType.Equals(DBType.MS_SQL))
            {
                helper = new SqldbHelper();
            }
            else if (dbType.Equals(DBType.MS_Access))
            {
                helper = new OledbHelper();
            }
            else if (dbType.Equals(DBType.Oracle))
            {
                helper = new OracleHelper();
            }
            else if (dbType.Equals(DBType.My_SQL))
            {
                helper = new MySQLHelper();
            }
            return helper;
        }

        #endregion
    }
}
