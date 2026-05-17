using System;
using System.Collections.Generic;
using CAB.Entity;
using CABEntity;
using CAB.BLL;
using CAB.Framework.Entity;

namespace CHANNEL.Formatter
{
    public class ParseFDLTOUData : ParseFDLData
    {
        /// <summary>
        /// 
        /// </summary>
        string touData = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="touData"></param>
        /// <param name="fileText"></param>
        /// <param name="fileUploadID"></param>
        /// <param name="meterDataID"></param>
        public ParseFDLTOUData(string touData, string fileText, long fileUploadID, long meterDataID)
            : base(fileText, fileUploadID, meterDataID)
        {
            this.touData = touData;
            this.meterDataID = meterDataID;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override FDLFileParseStatuses Parse()
        {
            try
            {
                if (!VerifyBCC(touData))
                {
                    SetParsingStatus(rmFDLParse.GetString("BCCMismatchTOU"));
                    return FDLFileParseStatuses.BCCMismatchGeneral;
                }
                /* VBM Added to solve bug 137464 */
                SetParsingStatus(rmFDLParse.GetString("TOUStatus"));
                /* VBM Added to solve bug 137464 */
                string currentTOUdata = this.touData;
                TOUEntity touEntity = new TOUEntity();
                List<TOU> touList = new List<TOU>();
                touEntity.tou = new List<TOU>();
                int counter = 2;
                while (counter < 62)
                {
                    TOU touItem = new TOU();
                    touItem.StartHour = ConvertHexToDecimal(currentTOUdata, counter);
                    counter = counter + 2;
                    touItem.StartMin = ConvertHexToDecimal(currentTOUdata, counter);
                    counter = counter + 2;
                    touItem.Tariff = ConvertHexToDecimal(currentTOUdata, counter);
                    counter = counter + 2;
                    if (touItem.Tariff == 0)
                        break;
                    touList.Add(touItem);
                }

                TOUBLL objTOUBLL = new TOUBLL();
                List<IEntity> entities = new List<IEntity>();
                foreach (TOU tou in touList)
                {
                    tou.MeterData_ID = this.meterDataID;
                    tou.SeasonNumber = 1;//Only One season for Fast download .
                    entities.Add(tou);
                }
                objTOUBLL.InsertData(entities);

                return FDLFileParseStatuses.None;
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataInStringFormat"></param>
        /// <param name="dataIndex"></param>
        /// <returns></returns>
        public byte ConvertHexToDecimal(string dataInStringFormat, int dataIndex)
        {
            string data = dataInStringFormat.Substring(dataIndex, 2);
            return byte.Parse(data, System.Globalization.NumberStyles.HexNumber);
        }
       
    }
}
