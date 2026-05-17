using System.Collections.Generic; 
using System.Data;
using CAB.IECFramework;
using CAB.IECFramework.Entity;
using System;
using System.Data.Common;
using System.Text;
using System.IO;
namespace CAB.DALC.Data.DataServices
{
    public abstract class DALBase : IDAL
    {
        private readonly string SNo = "S.No";
        private string TableName;
        private string PrimaryKeyColumn;
        public DALBase()
        {
        }
        public DALBase(string tableName, string primaryKeyColumn)
        {
            TableName = tableName;
            PrimaryKeyColumn = primaryKeyColumn;
        }

        public virtual IEntity ValidateUser(IEntity entity)
        {
            return null;
        }
        public abstract IEntity InsertData(IEntity entity);
        public abstract IEntity InsertData(IList<IEntity> entities);
        public abstract bool UpdateData(IEntity entity);
        public abstract bool DeleteData(IEntity entity);
        public abstract IEntity GetDetailData(int id);
        public abstract IList<IEntity> ListData();
        public abstract DataSet ListDataSet();
        public abstract IEntity RowToEntity(DataRow row);

        public bool NotNullAndNotDBNull(DataRow row, string ColumnName)
        {
            object obj = row[ColumnName];
            if (obj == null)
                return false;
            if(string.IsNullOrEmpty(obj.ToString()))
                return false;
            else
                return true;
        }
        public string GetPK()
        {
            if (TableName == "" || PrimaryKeyColumn == "")
                return string.Empty;
            string value = "0";
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                DataRequest request = new DataRequest(string.Concat("select ", PrimaryKeyColumn, " from ", TableName, " order by ", PrimaryKeyColumn, " desc"));
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    value = ds.Tables[0].Rows[0][PrimaryKeyColumn].ToString();
            }
            catch (Exception)
            {
                value = string.Empty;
            }
            return value;
        }
        public string ParameterName(string parameterName)
        {
            return string.Concat(DatabaseFactory.GetPlaceholder(), parameterName);
        }

        protected byte[] StringToBlob(string inputString)
        {
            MemoryStream stream = new MemoryStream();
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.Write(inputString);
                writer.Flush();
            }
            return stream.ToArray();
            //Encoding enc = Encoding.ASCII;
            //return enc.GetBytes(inputString);
        }

        public DataTable AutoNumberedTable(DataTable SourceTable)
        {
            DataTable ResultTable = new DataTable();
            DataColumn AutoNumberColumn = new DataColumn();
            AutoNumberColumn.ColumnName = SNo;
            AutoNumberColumn.DataType = typeof(int);
            AutoNumberColumn.AutoIncrement = true;
            AutoNumberColumn.AutoIncrementSeed = 1;
            AutoNumberColumn.AutoIncrementStep = 1;
            ResultTable.Columns.Add(AutoNumberColumn);
            ResultTable.Merge(SourceTable);
            return ResultTable;
        }
    }
}
