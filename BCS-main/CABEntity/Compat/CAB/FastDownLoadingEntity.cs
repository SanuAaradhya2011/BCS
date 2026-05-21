using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LNGEntity
{
    public enum FastDownLoadStatuses
    {
        NoOptionToDownLoad,
        IncorrectMeterID,
        BlankMeterID,
        ErrorInCommunication,
        BuffersizeNotSufficient,
        None
    }
    public enum FastDownLoadOptions
    {
        General,
        Instant,
        TamperData,
        LSData,
        BillingData,
        Phasor,
        MidNight,
        Anomaly,
        /* GKG JVVNL Current TOU Read */
        TOU
    }
    public enum FDLFileUploadStatuses
    {
        BCCMismatchTamper,
        BCCMismatchLS,
        BCCMismatchBilling,
        BCCMatched,
        FileCorrupt,
        UnableToReadPacketStructureXMLFile,
        NoDataToUpload,
        BCCMismatchGeneral,
        BCCMismatchPhasor,
        BCCMismatchInstantaneous,
        FileUploadedSuccessfully
    }
    public enum FDLFileParseStatuses
    {
        BCCMismatchTamper,
        BCCMismatchLS,
        BCCMismatchBilling,
        BCCMismatchGeneral,
        BCCMismatchInstantaneous,
        BCSMismatchPhasor,
        None
    }
}

