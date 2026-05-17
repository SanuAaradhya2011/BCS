#region NameSpaces
using System;
using CAB.DALC.Data;
using CAB.Framework;
using CAB.Framework.Entity;
using System.Data;
#endregion
namespace CAB.BLL
{
    /// <summary>
    /// Excel export BLL
    /// </summary>
    public class ExcelExportBLL : IBLL
    {
        #region Constants & Variables
        private ExcelExportDAL excelExportDAL;
        #endregion

        #region Constructer
        public ExcelExportBLL()
        {
            excelExportDAL = new ExcelExportDAL();
        }
        #endregion

        #region Public Methods       
        public DataSet GetDataForExcelExport(long meterDataID)
        {
            return FormatDataFoeExcelExport(excelExportDAL.GetDataForExcelExport(meterDataID));
        }
        private DataSet FormatDataFoeExcelExport(DataSet inputData)
        {
            if (inputData != null && inputData.Tables != null && inputData.Tables.Count > 0)
            {
                DataSet formattedData = new DataSet();
            }
            return inputData;
        }
        #endregion
    }
}
