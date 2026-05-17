using System.Collections.Generic; 
using System.Data;
using CAB.IECFramework;
using CAB.IECFramework.Entity;

namespace ExceptionServices.Data
{
    public abstract class DALBase : IDAL
    {
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

        public string ParameterName(string parameterName)
        {
            return string.Concat(DatabaseFactory.GetPlaceholder(), parameterName);
        }
    }
}
