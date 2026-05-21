/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon INDIA                                    |
 * |											Cabcon Ltd.                                                     |
 * |										                                    |
 * | 																												|
 * |											Author : Piyush Singh. 									|
 * |											Date   : 11/02/2010 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------|
 * 
 */


using System;
using System.Collections;
using System.Data;

namespace ExceptionServices.Data
{
	public class DataRequest
	{
			private CommandType commandType;
		private string command;
		private bool isTransactional;
		private ArrayList parameters;
		private DataSet dataSet;  
        private DataParameter param;

		 
		public CommandType CommandType
		{
			get
			{
				return commandType;
			}
			set
			{
				commandType = value;
			}
		}

		 
		public string Command
		{
			get
			{
				return command;
			}
			set
			{
				command = value;
			}
		}
	 
		public bool IsTransactional
		{
			get
			{
				return isTransactional; 
			}
			set
			{
				isTransactional = value;
			}
		}
		 
		public ArrayList Parameters
		{
			get
			{
				return parameters; 
			}
			set
			{
				parameters.Add(value);
			}
		}
		 
		public DataSet DataSet
		{
			get
			{
				return dataSet; 
			}
			
		}
		 
		 
		 
        public DataRequest()
        {
            parameters = new ArrayList();
            param = null;
            dataSet = new DataSet(); 
        }
	 
		public DataRequest(string command, CommandType cType): this()
		{
			this.command = command;
			this.commandType = cType;
		}

        public DataRequest(string command)
            : this()
        {
            this.command = command;
            this.commandType = CommandType.Text;
        }

        public bool AddParamter(string inParameterName, object inParameterValue, DbType inParameterDBType)
        {
            param = new DataParameter(inParameterName, inParameterValue, inParameterDBType, ParameterDirection.Input);
            this.parameters.Add(param);
            return true;
        }
		 
        public bool AddParamter(string inParameterName, object inParameterValue, DbType inParameterDBType, int inParameterSize)
		{
            param = new DataParameter(inParameterName, inParameterValue, inParameterDBType, ParameterDirection.Input, inParameterSize);
			this.parameters.Add(param);
			return true;
		}
               
        public bool AddParamter(string inParameterName, object inParameterValue, DbType inParameterDBType, ParameterDirection inParameterDirection, int inParameterSize)
		{
            param = new DataParameter(inParameterName, inParameterValue, inParameterDBType, inParameterDirection, inParameterSize);
			this.parameters.Add(param);
			return true;
		}
		
	}
	
}
