using System.Data;

namespace ExceptionServices.Data
{
    public class DataParameter
    { 
        private string parameterName;
        private object parameterValue;
        private DbType parameterDbType;
        private ParameterDirection parameterDirection;
        private int parameterSize = 0;
        
        public int Size
        {
            get
            {
                return parameterSize;
            }
            set
            {
                parameterSize = value;
            }
        }
       
        public string Name
        {
            get
            {
                return parameterName;
            }
            set
            {
                parameterName = value;
            }
        }
        
        public object Value
        {
            get
            {
                return parameterValue;
            }
            set
            {
                parameterValue = value;
            }
        }
         
        public DbType DBType
        {
            get
            {
                return parameterDbType;
            }
            set
            {
                parameterDbType = value;
            }
        }
        
        public ParameterDirection Direction
        {
            get
            {
                return parameterDirection;
            }
            set
            {
                parameterDirection = value;
            }
        }
        
        public DataParameter()
        {
            parameterName = string.Empty;
            parameterValue = System.DBNull.Value;
            parameterDbType = DbType.String;
            parameterDirection = ParameterDirection.Input;
        }
        
        public DataParameter(string inParameterName, object inParameterValue, DbType inParameterDbType, ParameterDirection inParameterDirection, int inParameterSize)
        {
            parameterName = inParameterName;
            parameterValue = inParameterValue;
            parameterDbType = inParameterDbType;
            parameterDirection = inParameterDirection;
            parameterSize = inParameterSize;
        }

        public DataParameter(string inParameterName, object inParameterValue, DbType inParameterDbType, ParameterDirection inParameterDirection)
        {
            parameterName = inParameterName;
            parameterValue = inParameterValue;
            parameterDbType = inParameterDbType;
            parameterDirection = inParameterDirection; 
        }

    }
}
