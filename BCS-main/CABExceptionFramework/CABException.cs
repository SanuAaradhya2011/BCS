using System;
using System.Runtime.Serialization;
using CAB.Entity.Base;
using CAB.Framework.Utility;
using ExceptionServices.Data;

namespace CAB.Framework
{
    public class CABException: ApplicationException
     {
        ApplicationLogEntity applicationLogEntity = new ApplicationLogEntity();
        ApplicationLogDAL applicationLogDAL = new ApplicationLogDAL();
        /// <summary>
        ///  Initializes a new instance of the System.ApplicationException class.
        /// </summary>
        public CABException()
        {
        }
    
        /// <summary>
        /// Initializes a new instance of the System.ApplicationException class with a specified error message.
        /// </summary>
        /// <param name="message">A message that describes the error.</param>
        public CABException(string message)
            : base(message)
        {
            applicationLogEntity.LogDate = DateUtility.DateTimeToLong(System.DateTime.Now);
            applicationLogEntity.LogMessage = message;
            applicationLogEntity.LogSource = null;
            applicationLogEntity.LogMacID = null; 
            applicationLogEntity.UserID = ConfigInfo.UserInformationID;
            applicationLogDAL.InsertData(applicationLogEntity);

        }

        /// <summary>
        /// Initializes a new instance of the System.ApplicationException class with a specified error message.
        /// </summary>
        /// <param name="message">A message that describes the error.</param>
        public CABException(Exception innerExc)
            : base(null, innerExc)
        {
            applicationLogEntity.LogDate = DateUtility.DateTimeToLong(System.DateTime.Now);
            applicationLogEntity.LogMessage = innerExc.Message;
            applicationLogEntity.LogSource = innerExc.StackTrace;
            applicationLogEntity.LogMacID = null; 
            applicationLogEntity.UserID = ConfigInfo.UserInformationID;
            applicationLogDAL.InsertData(applicationLogEntity);
        }
       
    
        /// <summary>
        /// Initializes a new instance of the System.ApplicationException class with serialized data.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected CABException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        
    
        /// <summary>
        /// Initializes a new instance of the System.ApplicationException class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException"> The exception that is the cause of the current exception. If the innerException parameter is not a null reference, the current exception is raised in a catch block that handles the inner exception.</param>
        public CABException(string message, Exception innerExc)
            : base(message, innerExc)
        {
            applicationLogEntity.LogDate = DateUtility.DateTimeToLong(System.DateTime.Now);
            applicationLogEntity.LogMessage = message;
            applicationLogEntity.LogSource = innerExc.StackTrace;
            applicationLogEntity.LogMacID = null;
            applicationLogEntity.UserID = ConfigInfo.UserInformationID; 
            applicationLogDAL.InsertData(applicationLogEntity);
        }
     }  
 } 
