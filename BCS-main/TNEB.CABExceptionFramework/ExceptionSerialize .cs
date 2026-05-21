/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 									|
 * |											Date   : 12/03/2010 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| 
 */

using System;
using System.Runtime.Serialization;
using CAB.IECFramework.Utility;
using CAB.IECFramework.Entity;
using CAB.Entity.Base;
namespace CAB.IECFramework
{
    [Serializable()]
    public class ExceptionSerialize : ISerializable
    {
        private Exception exceptionObject;
        private int createBy=ConfigInfo.UserInformationID;
        private long creationDate=DateUtility.DateTimeToLong(System.DateTime.Now); 
        private string machinName = ConfigInfo.MachineName;
        private ApplicationLogEntity applicationLogEntity;
        public string MachinName
        {
            get
            {
                return machinName;
            }
            set
            {
                machinName = value;
            }
        }
        public long CreationDate
        {
            get
            {
                return creationDate;
            }
            set
            {
                creationDate = value;
            }
        }
        public int CreateBy
        {
            get
            {
                return createBy;
            }
            set
            {
                createBy = value;
            }
        }
        public Exception ExceptionObject
        {
            get
            {
                return this.exceptionObject;
            }
            set
            {
                this.exceptionObject = value;
            }
        }
        public IEntity GetEntity()
        {
            return applicationLogEntity;
        }


        public ExceptionSerialize()
        {
        }

       
        public ExceptionSerialize(SerializationInfo info, StreamingContext ctxt)
        {
            this.exceptionObject = (CABException)info.GetValue("CABException", typeof(CABException));
            this.CreateBy = (int)info.GetValue("CreateBy", typeof(int));
            this.CreationDate = (long)info.GetValue("CreationDate", typeof(long));
            this.MachinName = (string)info.GetValue("MachineName", typeof(string));
            applicationLogEntity = new ApplicationLogEntity();
            applicationLogEntity.LogDate = this.CreationDate;
            applicationLogEntity.LogMessage = this.exceptionObject.Message;
            applicationLogEntity.LogSource = this.exceptionObject.Source;
            applicationLogEntity.LogMacID = this.MachinName; 
            applicationLogEntity.UserID = this.CreateBy;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("CABException", this.ExceptionObject);
            info.AddValue("CreateBy", this.CreateBy);
            info.AddValue("CreationDate", this.CreationDate);
            info.AddValue("MachineName", this.MachinName);
        }
    }
}