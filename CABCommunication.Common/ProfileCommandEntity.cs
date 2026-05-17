///****************************************************************************
//'*
//'*  Projet       : TNEB PUMA
//'*
//'*  Module       : Entity List
//'*
//'*  Environment  : Visual Studio 2008 - C#.net
//'*
//'*------+----------+------------------------------------------------------------
//'*Vers |   Date    |    Programmer and Comments
//'*------+----------+------------------------------------------------------------
//'* 1.00 | 12/07/2013 | Vidya Bhooshan Mishra : creation.
//'*------+----------+------------------------------------------------------------
//'*      |          | XXXXX: Change Details
//'******************************************************************************/
#region Namespaces

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion

namespace CABCommunication.Common
{
    /// <summary>
    /// Represents the data structure that encapsulates the 
    /// properties of Profile reading commands
    /// </summary>
    public class ProfileCommand 
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        private ActionType action;
        private string  className;
        private byte meterModelNumber;        
        private int tagNumber;        
        private byte classId;        
        private string obisCode;       
        private byte attribute;
        private object writeDataBuffer;
        #endregion        

        #region Properties
       

        /// <summary>
        /// Gets/Sets calss name 
        /// </summary>
        public string ClassName
        {
            get { return className; }
            set { className = value; }
        }
        /// <summary>
        /// Gets/Sets action
        /// </summary>
        public ActionType Action
        {
            get { return action; }
            set { action = value; }
        }
        /// <summary>
        /// Gets/Sets meter model number
        /// </summary>
        public byte MeterModelNumber
        {
            get { return meterModelNumber; }
            set { meterModelNumber = value; }
        }

        /// <summary>
        /// Gets/Sets tag number
        /// </summary>
        public int TagNumber
        {
            get { return tagNumber; }
            set { tagNumber = value; }
        }
        /// <summary>
        /// Gets/Sets ClassId
        /// </summary>
        public byte ClassId
        {
            get { return classId; }
            set { classId = value; }
        }
        /// <summary>
        /// Gets/Sets Obis Code
        /// </summary>
        public string ObisCode
        {
            get { return obisCode; }
            set { obisCode = value; }
        }
        /// <summary>
        /// Gets/Sets Attribute
        /// </summary>
        public byte Attribute
        {
            get { return attribute; }
            set { attribute = value; }
        }

        /// <summary>
        /// Gets/Sets Write data Buffer
        /// </summary>
        public object WriteDataBuffer
        {
            get { return writeDataBuffer; }
            set { writeDataBuffer = value; }
        }
        /// <summary>
        /// Gets/Sets the MeterID
        /// </summary>
        public List<byte> MeterID
        {
            get;
            set;
        }

        /// <summary>
        /// Gets/Sets the Selective Access Property
        /// </summary>
        public bool SelectiveAccess
        {
            get;
            set;
        }
                

        /// <summary>
        /// Gets/Sets the Selective Access To-Time
        /// </summary>
        public DateTime ToTime
        {
            get;
            set;
        }
        /// <summary>
        /// Gets/sets the From-Time for selective access
        /// </summary>
        public DateTime FromTime
        {
            get;
            set;
        }
        #endregion

        #region Constructor
       /// <summary>
       /// 
       /// </summary>
       /// <param name="classId"></param>
       /// <param name="ObisCode"></param>
       /// <param name="attribute"></param>
        public ProfileCommand(byte classId,string ObisCode,byte attribute)
        {
            this.classId = classId;
            this.obisCode = ObisCode;
            this.attribute = attribute;
            this.action = ActionType.READ;
        }
        /// <summary>
        /// 
        /// </summary>
        public ProfileCommand()
        {
           
        }  
        #endregion

        #region Protected Methods
        #endregion

        #region Event Handlers
        #endregion

        #region Private Methods
        #endregion
    }
}
