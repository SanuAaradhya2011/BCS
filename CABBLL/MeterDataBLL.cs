using System;
using System.Collections.Generic;
using System.Text;
using CAB.Framework;
using CAB.DALC.Data;
using CAB.Framework.Entity;
using CAB.Entity;
using System.Data.Common;
using System.Data;

namespace CAB.BLL
{
    public class MeterDataBLL : IBLL
    {
        MeterDataDAL meterDataDAL;
        private bool isPUMA = false;

        public MeterDataBLL()
        {
            if (UtilityDetails.GetUtilityDetails() == UtilityEntity.Generic)
            {
                isPUMA = true;
            }
            meterDataDAL = new MeterDataDAL(isPUMA);
        }

        public IList<IEntity> ListMeterSerialNumber()
        {
            return meterDataDAL.ListData();
        }
        public IEntity InsertData(IEntity entity)
        {
            return meterDataDAL.InsertData(entity);
        }
        public bool ValidateData(long fileUploadID, string meterID, long readingDateTime)
        {
            return meterDataDAL.ValidateData(fileUploadID, meterID, readingDateTime);
        }
        public DataSet ComboList(string value)
        {
            return meterDataDAL.ComboList(value);
        }
        /// <summary>
        /// gets meterIDs ,where uploaded data for loadsurvey is not null for metereID.
        /// </summary>
        /// <param name="isDate"></param>
        /// <returns></returns>
        public DataSet GetMeterIDLoadSurvey(bool isDate)
        {
            return meterDataDAL.GetMeterIDLoadSurvey(isDate);
        }

        /// <summary>
        /// gets meterIDs ,where uploaded data for midnight is not null for metereID.
        /// </summary>
        /// <param name="isDate"></param>
        /// <returns></returns>
        public DataSet GetMeterIDMidnight(bool isDate)
        {
            return meterDataDAL.GetMeterIDMidnight(isDate);
        }
        ///// <summary>
        ///// Updates parameter Communication Type in fileupload_master
        ///// </summary>
        ///// <param name="fileUpload_ID"></param>
        ///// <param name="value"></param>
        //public void UpdateCommunicationType(long fileUpload_ID, int value)
        //{
        //    meterDataDAL.UpdateCommType(fileUpload_ID, value);
        //}

        public DataSet ListDataSet(string types, string value, bool fFlag)
        {
            return meterDataDAL.ListDataSet(types, value, fFlag);
        }

        public DataSet ListExportDataSet(string fileName)
        {
            DataSet dataSet = meterDataDAL.ListExportDataSet(fileName);
            return CommonBLL.ExportData(dataSet);
        }
        public DataSet ListExportDataSet()
        {
            DataSet dataSet = meterDataDAL.ListExportDataSet();
            return CommonBLL.ExportData(dataSet);
        }
        public DataSet FileExportListDataSet(bool isTextFileExport)
        {
            DataSet dataSet = meterDataDAL.FileExportListDataSet(isTextFileExport);
            return CommonBLL.ExportListData(dataSet);
        }
        // SB Change Start 20170915
        public DataSet FileExportListFilteredDataSet(bool isTextFileExport, long lStartDate, long lEndDate)
        {
            DataSet dataSet = meterDataDAL.FileExportListFilteredDataSet(isTextFileExport, lStartDate, lEndDate);
            return CommonBLL.ExportListData(dataSet);
        }
        // SB Change End 20170915
        public DataSet PedFileExportListDataSet(bool isTextFileExport)
        {
            DataSet dataSet = meterDataDAL.PedFileExportListDataSet(isTextFileExport);
            return CommonBLL.ExportListDataPed(dataSet);
        }
        public DataSet AsciiPUMAFileExportListDataSet(string meterType)
        {
            DataSet dataSet = meterDataDAL.AsciiPUMAFileExportListDataSet(meterType);
            return CommonBLL.ExportListData(dataSet);
        }
        public DataSet AsciiPUMAListExportDataSet(string meterType)
        {
            DataSet dataSet = meterDataDAL.AsciiPUMAListExportDataSet(meterType);
            return CommonBLL.ExportData(dataSet);
        }
        public DataSet GetMeterIDFromMeterDataID(long activeMeterDataId)
        {
            return meterDataDAL.GetMeterIDFromMeterDataID(activeMeterDataId);
        }

        public DataSet GetMeterDataIDFromMeterID(string meterId)
        {
            return meterDataDAL.GetMeterDataIDFromMeterID(meterId);
        }

        public DataSet ListAllMeterID()
        {
            return meterDataDAL.GetAllMeterID();
        }

        public DataSet ListAllMeterIDValues()
        {
            return meterDataDAL.GetAllMeterIDValues();
        }

        public DataSet ListAllMeterIDLPRIDValues()
        {
            return meterDataDAL.GetAllMeterIDLPRIDValues();
        }


        public DataSet ListUnAssignedMeterID(int subGroupID)
        {
            return meterDataDAL.GetUnAssignedMeterID(subGroupID);
        }

        public IEntity GetDetailData(long id)
        {
            return meterDataDAL.GetDetailData(id);
        }

        public DataSet GetConsumerMeterDetails(long activeMeterData_ID)
        {
            return meterDataDAL.GetConsumerMeterDetails(activeMeterData_ID);
        }
        public DataSet ListNoPowerSupplyLoadTime(long activeMeterData_ID, DataSet NoSupplyLoadTime)
        {
            return meterDataDAL.ListNoPowerSupplyLoadTime(activeMeterData_ID, NoSupplyLoadTime);
        }

        public int GetAreaIDFromMeterID(string meterID)
        {
            return meterDataDAL.GetAreaIDFromMeterID(meterID);
        }
        public DataSet GetAreaDetails(int areaID)
        {
            return meterDataDAL.GetAreaDetails(areaID);
        }
        public IEntity GetDetailDataUploadId(long fileUploadId)
        {
            return meterDataDAL.GetDetailDataUploadId(fileUploadId);
        }
        public long GetMeterDataID(long fileUploadID)
        {
            return meterDataDAL.GetMeterDataID(fileUploadID);
        }
        public DataSet GetMeterDataSetID(long fileUploadID)
        {
            return meterDataDAL.GetMeterDataSetID(fileUploadID);
        }
        public bool DeleteData(long meterDataId)
        {
            return meterDataDAL.DeleteData(meterDataId);
        }
        // Added to solve bug 89140
        public bool DeleteDataFastDownLoad(long FileUploadID)
        {
            return meterDataDAL.DeleteDataFastDownload(FileUploadID);
        }
        public bool DeleteDataBasedOnFileID(long fileUploadId)
        {
            return meterDataDAL.DeleteDataBasedOnFileID(fileUploadId);
        }

        public long GetMeterData(string meterId)
        {
            return meterDataDAL.GetMeterData(meterId);
        }

        public bool CheckSimExist(string meterId)
        {
            return meterDataDAL.CheckSimExist(meterId);
        }
        public string GetSIMNumber(string meterId)
        {
            return meterDataDAL.GetSIMNumber(meterId);
        }
        public IEntity GetDetailData(string meterID, long fileUpload_ID, long readingDateTime)
        {
            return meterDataDAL.GetDetailData(meterID, fileUpload_ID, readingDateTime);
        }


        /// <summary>
        /// Method returns status of various Profile like General, Instant, Billing
        /// </summary>
        /// <param name="meterDataId"></param>
        /// <returns></returns>
        public DataSet GetFileUploadedStatus(int meterDataId)
        {
            return meterDataDAL.GetFileUploadedStatus(meterDataId);
        }
        /// <summary>
        /// gets consumer meter Details for meter ID.
        /// </summary>
        /// <param name="activeMeterID">A meterID can have multiple files (each having different MeterData_ID )</param>
        /// <returns></returns>
        public DataSet GetConsumerMeterDetailsForMeterID(string activeMeterID)
        {
            return meterDataDAL.GetConsumerMeterDetailsForMeterID(activeMeterID);
        }
        /// <summary>
        /// Get file Name
        /// </summary>
        /// <param name="meterDataID"></param>
        /// <returns></returns>
        public string GetFileType(long fileUploadId)
        {
            return meterDataDAL.GetFileType(fileUploadId);
        }

        /// <summary>
        /// Get file type by meterid
        /// </summary>
        /// <param name="meterId"></param>
        /// <returns></returns>
        public string GetFileTypeByMeterId(string meterId)
        {
            return meterDataDAL.GetFileTypeByMeterId(meterId);
        }
        /// <summary>
        /// get meter data for perticular cmri 
        /// </summary>
        /// <param name="cmriID"></param>
        /// <returns></returns>
        public MeterDataEntity GetDataforCMRI(string cmriID)
        {
            return meterDataDAL.GetDataforCMRI(cmriID);
        }
        /// <summary>
        /// update meter data for cmri 
        /// </summary>
        /// <param name="meterId"></param>
        /// <param name="cmriID"></param>
        /// <returns></returns>
        public DataSet UpdateCMRIID(string meterId, string cmriID)
        {
            return meterDataDAL.UpdateCMRIID(meterId, cmriID);
        }
        /// <summary>
        /// Fetch list for excel export
        /// </summary>
        /// <returns></returns>
        public DataSet ExcelExportListDataSet()
        {
            DataSet dataSet = meterDataDAL.ExcelExportListDataSet();
            return CommonBLL.ExcelExportListData(dataSet);
        }
    }
}
