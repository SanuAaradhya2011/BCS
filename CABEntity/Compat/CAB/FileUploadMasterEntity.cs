 /* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 									|
 * | 																												|
 * |											Author : Piyush Singh         									|
 * |											Date   : 10/03/2010 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| */


using LNG.Framework.Entity;
using System;

namespace LNG.Entity
{
    /// <summary>
    /// This class is used to hold the value of FileUploadMaster Table.
    /// </summary>
    public class FileUploadMasterEntity : EntityBase
    {

        //Private members
        private long fileUploadID; 
        private long uploadingDateTime;
        private long readingDateTime;
        private byte[] fileContent;
        private int userInformationID;
        private string fileName;
        private string fileType;
        private int commType;
        private string fileSize;
        private string cmriID;


        /// <summary>
        /// This property is used to get and set the value of File Upload ID.
        /// </summary>
        public long FileUpload_ID
        {
            get
            {
                return fileUploadID;
            }
            set
            {
                fileUploadID = value;
            }
        }
 

        /// <summary>
        /// This property is used to get and set the value of Uploading DateTime.
        /// </summary>
        public long UploadingDateTime
        {
            get
            {
                return uploadingDateTime;
            }
            set
            {
                uploadingDateTime = value;
            }
        }
        public long ReadingDateTime
        {
            get
            {
                return readingDateTime;
            }
            set
            {
                readingDateTime = value;
            }
        }
        /// <summary>
        /// This property is used to get and set the value of File Content.
        /// </summary>
        public byte[] FileContent
        {
            get
            {
                return fileContent;
            }
            set
            {
                fileContent = value;
            }
        }

        /// <summary>
        /// This property is used to get and set the value of User Information ID.
        /// </summary>
        public int UserInformation_ID
        {
            get
            {
                return userInformationID;
            }
            set
            {
                userInformationID = value;
            }
        }
        /// <summary>
        /// This property is used to get and set the value of File Name.
        /// </summary>
        public string FileName
        {
            get
            {
                return fileName;
            }
            set
            {
                fileName = value;
            }
        }
        /// <summary>
        /// This property is used to get and set the value of File Type.
        /// </summary>
        public string FileType
        {
            get
            {
                return fileType;
            }
            set
            {
                fileType = value;
            }
        }

        public int CommType
        {
            get
            {
                return commType;
            }
            set
            {
                commType = value;
            }
        }
        /// <summary>
        /// This property is used to get and set the value of File Size.
        /// </summary>
        public string FileSize
        {
            get
            {
                return fileSize;
            }
            set
            {
                fileSize = value;
            }
        }
        /// <summary>
        /// This property is used to get and set the value of CMRI ID.
        /// </summary>
        public string CMRIID
        {
            get
            {
                return cmriID;
            }
            set
            {
                cmriID = value;
            }
        }
        public string ReadingDateAndTime { get; set; } 

    }
   
   
    
}
