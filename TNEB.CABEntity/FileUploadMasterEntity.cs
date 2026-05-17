 /* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 									|
 * | 																												|
 * |											Author : Piyush Singh         									|
 * |											Date   : 10/03/2010 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| */


using CAB.IECFramework.Entity;
using System;

namespace CAB.Entity
{
    /// <summary>
    /// This class is used to hold the value of FileUploadMaster Table.
    /// </summary>
    public class IECFileUploadMasterEntity : EntityBase
    {

        //Private members
        private long fileUploadID; 
        private long uploadingDateTime;
        private long readingDateTime;
        private byte[] fileContent;
        private int userInformationID;
        private string fileName;
        private string fileType;



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
        /// This property is used to get and set the value of File type.
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

       
    }
    public enum FileUploadStatus
    {
        FileUploadingInitiated,
        FileCorrupt,
        BCC_Mismatch,
        FileAlreadyExists,
        FileIsTooLarge,
        UploadingGeneralData,
        UploadingInstantPowerData,
        UploadingBillingData,
        UploadingTariffEnergyData,
        UploadingTamperData,
        UploadingBillingHistoryData,
        UploadingTariffHistoryData,
        UploadingTamperHistoryData,
        UploadingFraudEnergyData,
        UploadingTransactionData,
        UploadingPhasorData,
        UploadingLoadSurveyData,
        UploadingTamperSnapShotData,
        UploadingDailyProfileData,
        UploadingHeaderInfoData,
        UploadingNamePlateDetails,
        UploadingMeterConfigurations,
        FileUploadedSuccessfully,
    }
    public enum ReadOutItem
    {
        General,
        FraudEnergy,
        Transaction,
        RTCUpdate,
        Phasor,
        LoadSurvey,
        Tamper,
        DailyProfile,
        MeterConfigurations,
        NamePlate,
        HeaderDetails,
        None
    }
    public class ReadOutCounterEntity
    {
        public string meterID;
        public Int64 readingDateTime;
        public Int64 meterDataID;
    }
        
}