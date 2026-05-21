#region NameSpaces
using System;
using System.Collections.Generic;
using CAB.Entity;
using CAB.Framework.Utility;
using CAB.Parser;
using CAB.Parser.Entity;
using System.Linq;
using Hunt.EPIC.Logging;
#endregion
namespace Common.EntityMapper
{
    /// <summary>
    /// Maps Billing  data to Billing Entity.
    /// </summary>
    public class BillingProfile
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(BillingProfile).ToString());
        #endregion

        #region Properties
        #endregion

        #region Constructor
        #endregion

        #region Public Methods
        /// <summary>
        /// Gets Billing  Entity from billing data.
        /// </summary>
        /// <param name="billingData"></param>
        /// <returns></returns>
        public BillingGeneralNFDLMSEntity GetMappedEntity(List<ProfileData> billingData, DLMS650NamePlateDetailsEntity generalEntity)
        {
            BillingGeneralNFDLMSEntity masterEntity = new BillingGeneralNFDLMSEntity();
            DLMS650BillingEntity billingEntity;
            List<DLMS650BillingEntity> billingEntityList = new List<DLMS650BillingEntity>();
            ConfigurationParser meterConfigParser = new ConfigurationParser(true);
            BillingParameterEntity billingColumns = new BillingParameterEntity();
            string lsColumnNames = "";
            string defaultValue = "----";
            DataElement dataElement = null;
            List<DataElement> billingRecords;

            // Story - Billing data sequence is changed for DLMS meters

            if (billingData[0].ListMeterDataPacket.Count > 1)
            {
                #region Billing Date Sorting Agreement Comments @Feb'19
                /*
                As per latest firmware implementation meter data received in FIFO order, but in all DLMS meters (1P & 3P) released before 2016, data was in LIFO order. 
                Billing data ordering is important as many calculation and reports are generated on received data and it must be in descending order of billing date for display and computation. 
                So, this condition checks 1st two date and if 1st date (current month billing date) < 2nd date (Last month billing date) it means data received in FIFO order and must be reversed, same can be achieved by sorting data in descending order of billing date. 
                Incase data are received in LIFO order i.e. 1st date (current month billing date) > 2nd date (Last month billing date) so no needs to do any sorting/reverse as it is as per expectation for user display & computation of consumption.
                Above logic was agreed with pdm during it’s 1st time of implementation in 2016, but again due to some field issue i.e. after manual billing it is observed that multiple bills generated on same billing time stamp and above condition of sorting get failed, so again it’s required a review to accommodate such scenario and it is agreed (agreement mail dt. 13th Feb 2019) that if both 1st & 2nd billing date are same then also we will sort billing data in descending order of date and new agreed condition is, if 1st date (current month billing date) <= 2nd date (Last month billing date) the do data sorting in descending order of billing date.
                */
                #endregion
                                
                if (billingData[0].ListMeterDataPacket[0].ReadingDate <= billingData[0].ListMeterDataPacket[1].ReadingDate)
                    billingData[0].ListMeterDataPacket = billingData[0].ListMeterDataPacket.OrderByDescending(x => x.ReadingDate).ToList();
            }

            if (billingData != null && billingData.Count > 0 && billingData[0].ListMeterDataPacket.Count > 0)
            {

                foreach (MeterDataPacket billingHistory in billingData[0].ListMeterDataPacket)
                {
                    billingEntity = new DLMS650BillingEntity();
                    billingRecords = billingHistory.ListDataElementValue;

                    // Dynamic visibility is implemented for kVArhLag and kVArhLead Parameter. User Story no 474879 
                    dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 32);
                    if (dataElement.DataDefinitionID == 0)
                        billingEntity.CumulativeEnergykvarhLag = null;
                    else
                      billingEntity.CumulativeEnergykvarhLag = CommonMapper.FormatData(dataElement);
                      
                    
                    // Dynamic visibility is implemented for kVArhLag and kVArhLead Parameter. User Story no 474879
                    dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 33);
                    if (dataElement.DataDefinitionID == 0)
                        billingEntity.CumulativeEnergykvarhLead = null;
                    else
                        billingEntity.CumulativeEnergykvarhLead = CommonMapper.FormatData(dataElement);

                    //search for date time as date time would be present if the file is read from fast download
                    dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 30);
                    //match the element value with 0 as 0 is the default value from GetDataElementByDataDefId
                    if (!string.IsNullOrEmpty(dataElement.Value) && dataElement.Value != "0")
                    {
                        billingEntity.BillingDate = CommonMapper.StringToLongDateTimeFormat(dataElement.Value);
                        billingEntity.DataIndex = billingData[0].ListMeterDataPacket.IndexOf(billingHistory) + 1;
                    }
                    else
                    {
                        billingEntity.BillingDate = DateUtility.DateTimeToLong(billingHistory.ReadingDate);
                        billingEntity.DataIndex = billingData[0].ListMeterDataPacket.IndexOf(billingHistory);
                    }

                    dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 302);
                    billingEntity.SystemPowerFactorforBillingPeriod = Convert.ToDecimal(dataElement.Value).ToString("0.00");
                    // Two decimal formatter removed due to JVVNL lpr requirement of showing the value upto three decimal place. BCS will show what ever data comes from meter
                    //billingEntity.SystemPowerFactorforBillingPeriod = Convert.ToDecimal(dataElement.Value).ToString();

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 302);
                    billingEntity.SystemPowerFactorforBillingPeriod = dataElement.Value;

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2109);
                    billingEntity.SystemPowerFactorImportforBillingPeriod = dataElement.Value;

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2110);
                    billingEntity.SystemPowerFactorExportforBillingPeriod = dataElement.Value;
                    //pradipta_start_081018

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2181);
                    billingEntity.BillingAveragekWImportLoadFactor = dataElement.Value;

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2182);
                    billingEntity.BillingAveragekWExportLoadFactor = dataElement.Value;


                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2183);
                    billingEntity.BillingAveragekVAImportLoadFactor = dataElement.Value;


                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2184);
                    billingEntity.BillingAveragekVAExportLoadFactor = dataElement.Value;
                    //pradipta_End_081018


                    #region WBTenderSpecificChanges_LoadFactor
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 85);
                    if (dataElement.DataDefinitionID != 0)
                    {
                        billingEntity.BillingAverageLoadFactor = dataElement.Value;
                    }
                    else
                    {
                        // user stroy: 490966 WB tender specific check implemented for Average Load factor OBIS code change
                        dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1108);
                        if (dataElement.DataDefinitionID != 0)
                        {
                            string wbFlag = "_WB";
                            billingEntity.BillingAverageLoadFactor = dataElement.Value + wbFlag;
                        }
                    }
                    #endregion
                    // Average Load
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2129); // replace 302 with 2129 when obis code come
                    if (dataElement.DataDefinitionID != 0)
                    {
                        billingEntity.BillingAverageLoad = dataElement.Value;
                    }

                    dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 31);
                    if (dataElement.DataDefinitionID == 0)
                        billingEntity.CumulativeEnergykWhTZ0 = null;
                    else
                    billingEntity.CumulativeEnergykWhTZ0 = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 304);
                    if (dataElement.DataDefinitionID == 0)
                        billingEntity.CumulativeEnergykWhTZ1 = null;
                    else
                    billingEntity.CumulativeEnergykWhTZ1 = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 305);
                    if (dataElement.DataDefinitionID == 0)
                        billingEntity.CumulativeEnergykWhTZ2 = null;
                    else
                    billingEntity.CumulativeEnergykWhTZ2 = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 306);
                    if (dataElement.DataDefinitionID == 0)
                        billingEntity.CumulativeEnergykWhTZ3 = null;
                    else
                    billingEntity.CumulativeEnergykWhTZ3 = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 307);
                    if (dataElement.DataDefinitionID == 0)
                        billingEntity.CumulativeEnergykWhTZ4 = null;
                    else
                    billingEntity.CumulativeEnergykWhTZ4 = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 308);
                    if (dataElement.DataDefinitionID == 0)
                        billingEntity.CumulativeEnergykWhTZ5 = null;
                    else
                    billingEntity.CumulativeEnergykWhTZ5 = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 309);
                    if (dataElement.DataDefinitionID == 0)
                        billingEntity.CumulativeEnergykWhTZ6 = null;
                    else
                    billingEntity.CumulativeEnergykWhTZ6 = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 310);
                    if (dataElement.DataDefinitionID == 0)
                        billingEntity.CumulativeEnergykWhTZ7 = null;
                    else
                    billingEntity.CumulativeEnergykWhTZ7 = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 311);
                    if (dataElement.DataDefinitionID == 0)
                        billingEntity.CumulativeEnergykWhTZ8 = null;
                    else
                    billingEntity.CumulativeEnergykWhTZ8 = CommonMapper.FormatData(dataElement);
                    //*********Sapphire S2 ***************
                    dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 2205);
                    if (dataElement.DataDefinitionID == 0)
                        billingEntity.kWhLag = null;
                    else
                        billingEntity.kWhLag = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 2206);
                    if (dataElement.DataDefinitionID == 0)
                        billingEntity.kWhLead = null;
                    else
                        billingEntity.kWhLead = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 2207);
                    if (dataElement.DataDefinitionID == 0)
                        billingEntity.kVAhLag = null;
                    else
                        billingEntity.kVAhLag = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 2208);
                    if (dataElement.DataDefinitionID == 0)
                        billingEntity.kVAhLead = null;
                    else
                        billingEntity.kVAhLead = CommonMapper.FormatData(dataElement);



                    #region Import_Export_Parameter

                    //KWH - Net

                    //dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 1079);
                    //billingEntity.CumulativeEnergykWhTZ0Net = CommonMapper.FormatData(dataElement);

                    //dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 1128);
                    //billingEntity.CumulativeEnergykWhTZ1Net = CommonMapper.FormatData(dataElement);

                    //dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 1129);
                    //billingEntity.CumulativeEnergykWhTZ2Net = CommonMapper.FormatData(dataElement);

                    //dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 1130);
                    //billingEntity.CumulativeEnergykWhTZ3Net = CommonMapper.FormatData(dataElement);

                    //dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 1131);
                    //billingEntity.CumulativeEnergykWhTZ4Net = CommonMapper.FormatData(dataElement);

                    //dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 1132);
                    //billingEntity.CumulativeEnergykWhTZ5Net = CommonMapper.FormatData(dataElement);

                    //dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 1133);
                    //billingEntity.CumulativeEnergykWhTZ6Net = CommonMapper.FormatData(dataElement);

                    //dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 1134);
                    //billingEntity.CumulativeEnergykWhTZ7Net = CommonMapper.FormatData(dataElement);

                    //dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 1135);
                    //billingEntity.CumulativeEnergykWhTZ8Net = CommonMapper.FormatData(dataElement);


                    //KVAH - Net

                    //dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 1080);
                    //billingEntity.CumulativeEnergykVAhTZ0Net = CommonMapper.FormatData(dataElement);

                    //dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 1140);
                    //billingEntity.CumulativeEnergykVAhTZ1Net = CommonMapper.FormatData(dataElement);

                    //dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 1141);
                    //billingEntity.CumulativeEnergykVAhTZ2Net = CommonMapper.FormatData(dataElement);

                    //dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 1142);
                    //billingEntity.CumulativeEnergykVAhTZ3Net = CommonMapper.FormatData(dataElement);

                    //dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 1143);
                    //billingEntity.CumulativeEnergykVAhTZ4Net = CommonMapper.FormatData(dataElement);

                    //dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 1144);
                    //billingEntity.CumulativeEnergykVAhTZ5Net = CommonMapper.FormatData(dataElement);

                    //dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 1145);
                    //billingEntity.CumulativeEnergykVAhTZ6Net = CommonMapper.FormatData(dataElement);

                    //dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 1146);
                    //billingEntity.CumulativeEnergykVAhTZ7Net = CommonMapper.FormatData(dataElement);

                    //dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 1147);
                    //billingEntity.CumulativeEnergykVAhTZ8Net = CommonMapper.FormatData(dataElement);


                    //MD-KW-Net

                    //dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 1148);
                    //billingEntity.MDkWTZ0Net = CommonMapper.FormatData(dataElement);

                    //dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 1150);
                    //billingEntity.MDkWTZ1Net = CommonMapper.FormatData(dataElement);

                    //dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 1152);
                    //billingEntity.MDkWTZ2Net = CommonMapper.FormatData(dataElement);

                    //dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 1154);
                    //billingEntity.MDkWTZ3Net = CommonMapper.FormatData(dataElement);

                    //dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 1156);
                    //billingEntity.MDkWTZ4Net = CommonMapper.FormatData(dataElement);

                    //dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 1158);
                    //billingEntity.MDkWTZ5Net = CommonMapper.FormatData(dataElement);

                    //dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 1160);
                    //billingEntity.MDkWTZ6Net = CommonMapper.FormatData(dataElement);

                    //dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 1162);
                    //billingEntity.MDkWTZ7Net = CommonMapper.FormatData(dataElement);

                    //dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 1164);
                    //billingEntity.MDkWTZ8Net = CommonMapper.FormatData(dataElement);


                    //MD-KW-TimeStamp-Net

                    //dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 1149);
                    //billingEntity.MDkWDateTimeTZ0Net = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));

                    //dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 1151);
                    //billingEntity.MDkWDateTimeTZ1Net = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));


                    //dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 1153);
                    //billingEntity.MDkWDateTimeTZ2Net = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));


                    //dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 1155);
                    //billingEntity.MDkWDateTimeTZ3Net = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));


                    //dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 1157);
                    //billingEntity.MDkWDateTimeTZ4Net = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));


                    //dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 1159);
                    //billingEntity.MDkWDateTimeTZ5Net = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));


                    //dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 1161);
                    //billingEntity.MDkWDateTimeTZ6Net = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));

                    //dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 1163);
                    //billingEntity.MDkWDateTimeTZ7Net = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));


                    //dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 1165);
                    //billingEntity.MDkWDateTimeTZ8Net = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));


                    //MD-KVA-Net

                    //dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 1126);
                    //billingEntity.MDkVATZ0Net = CommonMapper.FormatData(dataElement);

                    //dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 1166);
                    //billingEntity.MDkVATZ1Net = CommonMapper.FormatData(dataElement);

                    //dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 1168);
                    //billingEntity.MDkVATZ2Net = CommonMapper.FormatData(dataElement);

                    //dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 1170);
                    //billingEntity.MDkVATZ3Net = CommonMapper.FormatData(dataElement);

                    //dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 1172);
                    //billingEntity.MDkVATZ4Net = CommonMapper.FormatData(dataElement);

                    //dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 1174);
                    //billingEntity.MDkVATZ5Net = CommonMapper.FormatData(dataElement);

                    //dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 1176);
                    //billingEntity.MDkVATZ6Net = CommonMapper.FormatData(dataElement);

                    //dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 1178);
                    //billingEntity.MDkVATZ7Net = CommonMapper.FormatData(dataElement);

                    //dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 1180);
                    //billingEntity.MDkVATZ8Net = CommonMapper.FormatData(dataElement);



                    //MD-KVA-TimeStamp-Net

                    //dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 1127);
                    //billingEntity.MDkVADateTimeTZ0Net = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));

                    //dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 1167);
                    //billingEntity.MDkVADateTimeTZ1Net = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));


                    //dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 1169);
                    //billingEntity.MDkVADateTimeTZ2Net = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));


                    //dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 1171);
                    //billingEntity.MDkVADateTimeTZ3Net = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));


                    //dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 1173);
                    //billingEntity.MDkVADateTimeTZ4Net = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));


                    //dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 1175);
                    //billingEntity.MDkVADateTimeTZ5Net = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));


                    //dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 1177);
                    //billingEntity.MDkVADateTimeTZ6Net = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));


                    //dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 1179);
                    //billingEntity.MDkVADateTimeTZ7Net = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));


                    //dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 1181);
                    //billingEntity.MDkVADateTimeTZ8Net = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));



                    //KWH - Import

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2051);
                    if (dataElement.Value != null)
                        billingEntity.CumulativeEnergykWhTZ0Import = CommonMapper.FormatData(dataElement);


                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1199);
                    if (dataElement.Value != null)
                        billingEntity.CumulativeEnergykWhTZ1Import = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2000);
                    if (dataElement.Value != null)
                        billingEntity.CumulativeEnergykWhTZ2Import = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2001);
                    if (dataElement.Value != null)
                        billingEntity.CumulativeEnergykWhTZ3Import = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2002);
                    if (dataElement.Value != null)
                        billingEntity.CumulativeEnergykWhTZ4Import = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2003);
                    if (dataElement.Value != null)
                        billingEntity.CumulativeEnergykWhTZ5Import = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2004);
                    if (dataElement.Value != null)
                        billingEntity.CumulativeEnergykWhTZ6Import = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2005);
                    if (dataElement.Value != null)
                        billingEntity.CumulativeEnergykWhTZ7Import = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2006);
                    if (dataElement.Value != null)
                        billingEntity.CumulativeEnergykWhTZ8Import = CommonMapper.FormatData(dataElement);

                    //KVAH - Import

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2052);
                    if (dataElement.Value != null)
                        billingEntity.CumulativeEnergykVAhTZ0Import = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2007);

                    if (dataElement.Value != null)
                        billingEntity.CumulativeEnergykVAhTZ1Import = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2008);
                    if (dataElement.Value != null)
                        billingEntity.CumulativeEnergykVAhTZ2Import = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2009);
                    if (dataElement.Value != null)
                        billingEntity.CumulativeEnergykVAhTZ3Import = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2010);
                    if (dataElement.Value != null)
                        billingEntity.CumulativeEnergykVAhTZ4Import = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2011);
                    if (dataElement.Value != null)
                        billingEntity.CumulativeEnergykVAhTZ5Import = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2012);
                    if (dataElement.Value != null)
                        billingEntity.CumulativeEnergykVAhTZ6Import = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2013);
                    if (dataElement.Value != null)
                        billingEntity.CumulativeEnergykVAhTZ7Import = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2014);
                    if (dataElement.Value != null)
                        billingEntity.CumulativeEnergykVAhTZ8Import = CommonMapper.FormatData(dataElement);

                    //MD-KW-Import

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2015);
                    if (dataElement.Value != null)
                        billingEntity.MDkWTZ0Import = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2017);
                    if (dataElement.Value != null)
                        billingEntity.MDkWTZ1Import = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2019);
                    if (dataElement.Value != null)
                        billingEntity.MDkWTZ2Import = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2021);
                    if (dataElement.Value != null)
                        billingEntity.MDkWTZ3Import = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2023);
                    if (dataElement.Value != null)
                        billingEntity.MDkWTZ4Import = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2025);
                    if (dataElement.Value != null)
                        billingEntity.MDkWTZ5Import = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2027);
                    if (dataElement.Value != null)
                        billingEntity.MDkWTZ6Import = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2029);
                    if (dataElement.Value != null)
                        billingEntity.MDkWTZ7Import = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2031);
                    if (dataElement.Value != null)
                        billingEntity.MDkWTZ8Import = CommonMapper.FormatData(dataElement);


                    //MD-KW-TimeStamp-Import

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2016);
                    if (dataElement.Value != null)
                        billingEntity.MDkWDateTimeTZ0Import = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2018);
                    if (dataElement.Value != null)
                        billingEntity.MDkWDateTimeTZ1Import = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));


                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2020);
                    if (dataElement.Value != null)
                        billingEntity.MDkWDateTimeTZ2Import = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));


                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2022);
                    if (dataElement.Value != null)
                        billingEntity.MDkWDateTimeTZ3Import = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));


                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2024);
                    if (dataElement.Value != null)
                        billingEntity.MDkWDateTimeTZ4Import = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));


                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2026);
                    if (dataElement.Value != null)
                        billingEntity.MDkWDateTimeTZ5Import = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));


                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2028);
                    if (dataElement.Value != null)
                        billingEntity.MDkWDateTimeTZ6Import = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));


                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2030);
                    if (dataElement.Value != null)
                        billingEntity.MDkWDateTimeTZ7Import = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));


                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2032);
                    if (dataElement.Value != null)
                        billingEntity.MDkWDateTimeTZ8Import = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));


                    //MD-KVA-Import

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2033);
                    if (dataElement.Value != null)
                        billingEntity.MDkVATZ0Import = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2035);
                    if (dataElement.Value != null)
                        billingEntity.MDkVATZ1Import = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2037);
                    if (dataElement.Value != null)
                        billingEntity.MDkVATZ2Import = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2039);
                    if (dataElement.Value != null)
                        billingEntity.MDkVATZ3Import = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2041);
                    if (dataElement.Value != null)
                        billingEntity.MDkVATZ4Import = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2043);
                    if (dataElement.Value != null)
                        billingEntity.MDkVATZ5Import = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2045);
                    if (dataElement.Value != null)
                        billingEntity.MDkVATZ6Import = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2047);
                    if (dataElement.Value != null)
                        billingEntity.MDkVATZ7Import = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2049);
                    if (dataElement.Value != null)
                        billingEntity.MDkVATZ8Import = CommonMapper.FormatData(dataElement);


                    //MD-KVA-TimeStamp-Import

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2034);
                    if (dataElement.Value != null)
                        billingEntity.MDkVADateTimeTZ0Import = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2036);
                    if (dataElement.Value != null)
                        billingEntity.MDkVADateTimeTZ1Import = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));


                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2038);
                    if (dataElement.Value != null)
                        billingEntity.MDkVADateTimeTZ2Import = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));


                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2040);
                    if (dataElement.Value != null)
                        billingEntity.MDkVADateTimeTZ3Import = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));


                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2042);
                    if (dataElement.Value != null)
                        billingEntity.MDkVADateTimeTZ4Import = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));


                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2044);
                    if (dataElement.Value != null)
                        billingEntity.MDkVADateTimeTZ5Import = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));


                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2046);
                    if (dataElement.Value != null)
                        billingEntity.MDkVADateTimeTZ6Import = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));


                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2048);
                    if (dataElement.Value != null)
                        billingEntity.MDkVADateTimeTZ7Import = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));


                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2050);
                    if (dataElement.Value != null)
                        billingEntity.MDkVADateTimeTZ8Import = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));


                    //KWH - Export

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1079);
                    if (dataElement.Value != null)
                        billingEntity.CumulativeEnergykWhTZ0Export = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1128);
                    if (dataElement.Value != null)
                        billingEntity.CumulativeEnergykWhTZ1Export = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1129);
                    if (dataElement.Value != null)
                        billingEntity.CumulativeEnergykWhTZ2Export = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1130);
                    if (dataElement.Value != null)
                        billingEntity.CumulativeEnergykWhTZ3Export = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1131);
                    if (dataElement.Value != null)
                        billingEntity.CumulativeEnergykWhTZ4Export = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1132);
                    if (dataElement.Value != null)
                        billingEntity.CumulativeEnergykWhTZ5Export = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1133);
                    if (dataElement.Value != null)
                        billingEntity.CumulativeEnergykWhTZ6Export = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1134);
                    if (dataElement.Value != null)
                        billingEntity.CumulativeEnergykWhTZ7Export = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1135);
                    if (dataElement.Value != null)
                        billingEntity.CumulativeEnergykWhTZ8Export = CommonMapper.FormatData(dataElement);

                    //KVAH - Export

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1080);
                    if (dataElement.Value != null)
                        billingEntity.CumulativeEnergykVAhTZ0Export = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1140);
                    if (dataElement.Value != null)
                        billingEntity.CumulativeEnergykVAhTZ1Export = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1141);
                    if (dataElement.Value != null)
                        billingEntity.CumulativeEnergykVAhTZ2Export = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1142);
                    if (dataElement.Value != null)
                        billingEntity.CumulativeEnergykVAhTZ3Export = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1143);
                    if (dataElement.Value != null)
                        billingEntity.CumulativeEnergykVAhTZ4Export = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1144);
                    if (dataElement.Value != null)
                        billingEntity.CumulativeEnergykVAhTZ5Export = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1145);
                    if (dataElement.Value != null)
                        billingEntity.CumulativeEnergykVAhTZ6Export = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1146);
                    if (dataElement.Value != null)
                        billingEntity.CumulativeEnergykVAhTZ7Export = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1147);
                    if (dataElement.Value != null)
                        billingEntity.CumulativeEnergykVAhTZ8Export = CommonMapper.FormatData(dataElement);

                    //MD-KW-Export

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1148);
                    if (dataElement.Value != null)
                        billingEntity.MDkWTZ0Export = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1150);
                    if (dataElement.Value != null)
                        billingEntity.MDkWTZ1Export = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1152);
                    if (dataElement.Value != null)
                        billingEntity.MDkWTZ2Export = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1154);
                    if (dataElement.Value != null)
                        billingEntity.MDkWTZ3Export = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1156);
                    if (dataElement.Value != null)
                        billingEntity.MDkWTZ4Export = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1158);
                    if (dataElement.Value != null)
                        billingEntity.MDkWTZ5Export = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1160);
                    if (dataElement.Value != null)
                        billingEntity.MDkWTZ6Export = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1162);
                    if (dataElement.Value != null)
                        billingEntity.MDkWTZ7Export = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1164);
                    if (dataElement.Value != null)
                        billingEntity.MDkWTZ8Export = CommonMapper.FormatData(dataElement);


                    //MD-KW-TimeStamp-Export

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1149);
                    if (dataElement.Value != null)
                        billingEntity.MDkWDateTimeTZ0Export = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1151);
                    if (dataElement.Value != null)
                        billingEntity.MDkWDateTimeTZ1Export = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));


                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1153);
                    if (dataElement.Value != null)
                        billingEntity.MDkWDateTimeTZ2Export = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));


                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1155);
                    if (dataElement.Value != null)
                        billingEntity.MDkWDateTimeTZ3Export = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));


                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1157);
                    if (dataElement.Value != null)
                        billingEntity.MDkWDateTimeTZ4Export = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));


                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1159);
                    if (dataElement.Value != null)
                        billingEntity.MDkWDateTimeTZ5Export = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));


                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1161);
                    if (dataElement.Value != null)
                        billingEntity.MDkWDateTimeTZ6Export = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));


                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1163);
                    if (dataElement.Value != null)
                        billingEntity.MDkWDateTimeTZ7Export = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));


                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1165);
                    if (dataElement.Value != null)
                        billingEntity.MDkWDateTimeTZ8Export = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));


                    //MD-KVA-Export

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1126);
                    if (dataElement.Value != null)
                        billingEntity.MDkVATZ0Export = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1166);
                    if (dataElement.Value != null)
                        billingEntity.MDkVATZ1Export = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1168);
                    if (dataElement.Value != null)
                        billingEntity.MDkVATZ2Export = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1170);
                    if (dataElement.Value != null)
                        billingEntity.MDkVATZ3Export = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1172);
                    if (dataElement.Value != null)
                        billingEntity.MDkVATZ4Export = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1174);
                    if (dataElement.Value != null)
                        billingEntity.MDkVATZ5Export = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1176);
                    if (dataElement.Value != null)
                        billingEntity.MDkVATZ6Export = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1178);
                    if (dataElement.Value != null)
                        billingEntity.MDkVATZ7Export = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1180);
                    if (dataElement.Value != null)
                        billingEntity.MDkVATZ8Export = CommonMapper.FormatData(dataElement);


                    //MD-KVA-TimeStamp-Export

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1127);
                    if (dataElement.Value != null)
                        billingEntity.MDkVADateTimeTZ0Export = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1167);
                    if (dataElement.Value != null)
                        billingEntity.MDkVADateTimeTZ1Export = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));


                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1169);
                    if (dataElement.Value != null)
                        billingEntity.MDkVADateTimeTZ2Export = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));


                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1171);
                    if (dataElement.Value != null)
                        billingEntity.MDkVADateTimeTZ3Export = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));


                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1173);
                    if (dataElement.Value != null)
                        billingEntity.MDkVADateTimeTZ4Export = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));


                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1175);
                    if (dataElement.Value != null)
                        billingEntity.MDkVADateTimeTZ5Export = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));


                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1177);
                    if (dataElement.Value != null)
                        billingEntity.MDkVADateTimeTZ6Export = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));


                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1179);
                    if (dataElement.Value != null)
                        billingEntity.MDkVADateTimeTZ7Export = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));


                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1181);
                    if (dataElement.Value != null)
                        billingEntity.MDkVADateTimeTZ8Export = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));


                    //Cumulative-Energy-Lag-Q1

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1136);
                    if (dataElement.Value != null)
                        billingEntity.CumulativeEnergykvarhLagQ1 = (dataElement.Value);

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2139);//commet 2089                    
                    if (dataElement.Value != null)
                    
                        billingEntity.CumulativeEnergykvarhLagTZ1Q1 = (dataElement.Value);
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2090);



                    if (dataElement.Value != null)
                        billingEntity.CumulativeEnergykvarhLagTZ2Q1 = (dataElement.Value);
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2091);
                    if (dataElement.Value != null)
                        billingEntity.CumulativeEnergykvarhLagTZ3Q1 = (dataElement.Value);
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2092);
                    if (dataElement.Value != null)
                        billingEntity.CumulativeEnergykvarhLagTZ4Q1 = (dataElement.Value);
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2093);
                    if (dataElement.Value != null)
                        billingEntity.CumulativeEnergykvarhLagTZ5Q1 = (dataElement.Value);
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2094);
                    if (dataElement.Value != null)
                        billingEntity.CumulativeEnergykvarhLagTZ6Q1 = (dataElement.Value);
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2095);
                    if (dataElement.Value != null)
                        billingEntity.CumulativeEnergykvarhLagTZ7Q1 = (dataElement.Value);
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2096);
                    if (dataElement.Value != null)
                        billingEntity.CumulativeEnergykvarhLagTZ8Q1 = (dataElement.Value);

                    //Cumulative-Energy-Lead-Q4

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1139);
                    if (dataElement.Value != null)
                        billingEntity.CumulativeEnergykvarhLeadQ4 = (dataElement.Value);
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2081);
                    if (dataElement.Value != null)
                        billingEntity.CumulativeEnergykvarhLeadTZ1Q4 = (dataElement.Value);
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2082);
                    if (dataElement.Value != null)
                        billingEntity.CumulativeEnergykvarhLeadTZ2Q4 = (dataElement.Value);
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2083);
                    if (dataElement.Value != null)
                        billingEntity.CumulativeEnergykvarhLeadTZ3Q4 = (dataElement.Value);
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2084);
                    if (dataElement.Value != null)
                        billingEntity.CumulativeEnergykvarhLeadTZ4Q4 = (dataElement.Value);
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2085);
                    if (dataElement.Value != null)
                        billingEntity.CumulativeEnergykvarhLeadTZ5Q4 = (dataElement.Value);
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2086);
                    if (dataElement.Value != null)
                        billingEntity.CumulativeEnergykvarhLeadTZ6Q4 = (dataElement.Value);
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2087);
                    if (dataElement.Value != null)
                        billingEntity.CumulativeEnergykvarhLeadTZ7Q4 = (dataElement.Value);
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2088);
                    if (dataElement.Value != null)
                        billingEntity.CumulativeEnergykvarhLeadTZ8Q4 = (dataElement.Value);



                    //Cumulative-Energy-Lag-Q3

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1138);
                    if (dataElement.Value != null)
                        billingEntity.CumulativeEnergykvarhLagQ3 = (dataElement.Value);
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2067);
                    if (dataElement.Value != null)
                        billingEntity.CumulativeEnergykvarhLagTZ1Q3 = (dataElement.Value);
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2068);
                    if (dataElement.Value != null)
                        billingEntity.CumulativeEnergykvarhLagTZ2Q3 = (dataElement.Value);
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2069);
                    if (dataElement.Value != null)
                        billingEntity.CumulativeEnergykvarhLagTZ3Q3 = (dataElement.Value);
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2070);
                    if (dataElement.Value != null)
                        billingEntity.CumulativeEnergykvarhLagTZ4Q3 = (dataElement.Value);
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2071);
                    if (dataElement.Value != null)
                        billingEntity.CumulativeEnergykvarhLagTZ5Q3 = (dataElement.Value);
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2072);
                    if (dataElement.Value != null)
                        billingEntity.CumulativeEnergykvarhLagTZ6Q3 = (dataElement.Value);
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2073);
                    if (dataElement.Value != null)
                        billingEntity.CumulativeEnergykvarhLagTZ7Q3 = (dataElement.Value);
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2074);
                    if (dataElement.Value != null)
                        billingEntity.CumulativeEnergykvarhLagTZ8Q3 = (dataElement.Value);

                    //Cumulative-Energy-Lead-Q2

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1137);
                    if (dataElement.Value != null)
                        billingEntity.CumulativeEnergykvarhLeadQ2 = (dataElement.Value);
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2059);
                    if (dataElement.Value != null)
                        billingEntity.CumulativeEnergykvarhLeadTZ1Q2 = (dataElement.Value);
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2060);
                    if (dataElement.Value != null)
                        billingEntity.CumulativeEnergykvarhLeadTZ2Q2 = (dataElement.Value);
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2061);
                    if (dataElement.Value != null)
                        billingEntity.CumulativeEnergykvarhLeadTZ3Q2 = (dataElement.Value);
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2062);
                    if (dataElement.Value != null)
                        billingEntity.CumulativeEnergykvarhLeadTZ4Q2 = (dataElement.Value);
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2063);
                    if (dataElement.Value != null)
                        billingEntity.CumulativeEnergykvarhLeadTZ5Q2 = (dataElement.Value);
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2064);
                    if (dataElement.Value != null)
                        billingEntity.CumulativeEnergykvarhLeadTZ6Q2 = (dataElement.Value);
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2065);
                    if (dataElement.Value != null)
                        billingEntity.CumulativeEnergykvarhLeadTZ7Q2 = (dataElement.Value);
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2066);
                    if (dataElement.Value != null)
                        billingEntity.CumulativeEnergykvarhLeadTZ8Q2 = (dataElement.Value);


                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1184);
                    if (dataElement.Value != null)
                        billingEntity.CumEnergykWhRPhase = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1185);
                    if (dataElement.Value != null)
                        billingEntity.CumEnergykWhYPhase = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1186);
                    if (dataElement.Value != null)
                        billingEntity.CumEnergykWhBPhase = CommonMapper.FormatData(dataElement);

                    #endregion



                    dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 34);

                    if (dataElement.DataDefinitionID == 0)
                        billingEntity.CumulativeEnergykVAhTZ0 = null;
                    else
                        billingEntity.CumulativeEnergykVAhTZ0 = CommonMapper.FormatData(dataElement);
                    
                    dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 315);
                    if (dataElement.DataDefinitionID == 0)
                        billingEntity.CumulativeEnergykVAhTZ1 = null;
                    else
                    billingEntity.CumulativeEnergykVAhTZ1 = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 316);
                    if (dataElement.DataDefinitionID == 0)
                        billingEntity.CumulativeEnergykVAhTZ2 = null;
                    else
                    billingEntity.CumulativeEnergykVAhTZ2 = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 317);
                    if (dataElement.DataDefinitionID == 0)
                        billingEntity.CumulativeEnergykVAhTZ3 = null;
                    else
                    billingEntity.CumulativeEnergykVAhTZ3 = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 318);
                    if (dataElement.DataDefinitionID == 0)
                        billingEntity.CumulativeEnergykVAhTZ4 = null;
                    else
                    billingEntity.CumulativeEnergykVAhTZ4 = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 319);
                    if (dataElement.DataDefinitionID == 0)
                        billingEntity.CumulativeEnergykVAhTZ5 = null;
                    else
                    billingEntity.CumulativeEnergykVAhTZ5 = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 320);
                    if (dataElement.DataDefinitionID == 0)
                        billingEntity.CumulativeEnergykVAhTZ6 = null;
                    else
                    billingEntity.CumulativeEnergykVAhTZ6 = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 321);
                    if (dataElement.DataDefinitionID == 0)
                        billingEntity.CumulativeEnergykVAhTZ7 = null;
                    else
                    billingEntity.CumulativeEnergykVAhTZ7 = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 322);
                    if (dataElement.DataDefinitionID == 0)
                        billingEntity.CumulativeEnergykVAhTZ8 = null;
                    else
                    billingEntity.CumulativeEnergykVAhTZ8 = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 35);
                    if (dataElement.DataDefinitionID == 0)
                        billingEntity.MDkWTZ0 = null;
                    else
                    billingEntity.MDkWTZ0 = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 325);
                    if (dataElement.DataDefinitionID == 0)
                        billingEntity.MDkWTZ1 = null;
                    else
                    billingEntity.MDkWTZ1 = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 326);
                    if (dataElement.DataDefinitionID == 0)
                        billingEntity.MDkWTZ2 = null;
                    else
                    billingEntity.MDkWTZ2 = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 327);
                    if (dataElement.DataDefinitionID == 0)
                        billingEntity.MDkWTZ3 = null;
                    else
                    billingEntity.MDkWTZ3 = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 328);
                    if (dataElement.DataDefinitionID == 0)
                        billingEntity.MDkWTZ4 = null;
                    else
                    billingEntity.MDkWTZ4 = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 329);
                    if (dataElement.DataDefinitionID == 0)
                        billingEntity.MDkWTZ5 = null;
                    else
                    billingEntity.MDkWTZ5 = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 330);
                    if (dataElement.DataDefinitionID == 0)
                        billingEntity.MDkWTZ6 = null;
                    else
                    billingEntity.MDkWTZ6 = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 331);
                    if (dataElement.DataDefinitionID == 0)
                        billingEntity.MDkWTZ7 = null;
                    else
                    billingEntity.MDkWTZ7 = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 332);
                    if (dataElement.DataDefinitionID == 0)
                        billingEntity.MDkWTZ8 = null;
                    else
                    billingEntity.MDkWTZ8 = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 36);
                   
                    billingEntity.MDkWDateTimeTZ0 = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));

                    dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 333);
                    billingEntity.MDkWDateTimeTZ1 = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));

                    dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 334);
                    billingEntity.MDkWDateTimeTZ2 = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));

                    dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 335);
                    billingEntity.MDkWDateTimeTZ3 = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));

                    dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 336);
                    billingEntity.MDkWDateTimeTZ4 = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));

                    dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 337);
                    billingEntity.MDkWDateTimeTZ5 = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));

                    dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 338);
                    billingEntity.MDkWDateTimeTZ6 = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));

                    dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 339);
                    billingEntity.MDkWDateTimeTZ7 = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));

                    dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 340);
                    billingEntity.MDkWDateTimeTZ8 = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));

                    dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 37);
                    if (dataElement.DataDefinitionID == 0)
                        billingEntity.MDkVATZ0 = null;
                    else
                    billingEntity.MDkVATZ0 = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 343);
                    if (dataElement.DataDefinitionID == 0)
                        billingEntity.MDkVATZ1 = null;
                    else
                    billingEntity.MDkVATZ1 = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 344);
                    if (dataElement.DataDefinitionID == 0)
                        billingEntity.MDkVATZ2 = null;
                    else
                    billingEntity.MDkVATZ2 = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 345);
                    if (dataElement.DataDefinitionID == 0)
                        billingEntity.MDkVATZ3 = null;
                    else
                    billingEntity.MDkVATZ3 = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 346);
                    if (dataElement.DataDefinitionID == 0)
                        billingEntity.MDkVATZ4 = null;
                    else
                    billingEntity.MDkVATZ4 = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 347);
                    if (dataElement.DataDefinitionID == 0)
                        billingEntity.MDkVATZ5 = null;
                    else
                    billingEntity.MDkVATZ5 = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 348);
                    if (dataElement.DataDefinitionID == 0)
                        billingEntity.MDkVATZ6 = null;
                    else
                    billingEntity.MDkVATZ6 = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 349);
                    if (dataElement.DataDefinitionID == 0)
                        billingEntity.MDkVATZ7 = null;
                    else
                    billingEntity.MDkVATZ7 = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 350);
                    if (dataElement.DataDefinitionID == 0)
                        billingEntity.MDkVATZ8 = null;
                    else
                    billingEntity.MDkVATZ8 = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 38);
                    billingEntity.MDkVADateTimeTZ0 = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));

                    dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 351);
                    billingEntity.MDkVADateTimeTZ1 = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));

                    dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 352);
                    billingEntity.MDkVADateTimeTZ2 = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));

                    dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 353);
                    billingEntity.MDkVADateTimeTZ3 = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));

                    dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 354);
                    billingEntity.MDkVADateTimeTZ4 = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));

                    dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 355);
                    billingEntity.MDkVADateTimeTZ5 = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));

                    dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 356);
                    billingEntity.MDkVADateTimeTZ6 = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));

                    dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 357);
                    billingEntity.MDkVADateTimeTZ7 = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));

                    dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 358);
                    billingEntity.MDkVADateTimeTZ8 = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 27);
                    billingEntity.CumTamperCount = dataElement.Value == null ? -1 : Convert.ToInt64(dataElement.Value);

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 384); // Story - 345154
                    billingEntity.DeltaTamperCount = dataElement.Value == null ? -1 : Convert.ToInt64(dataElement.Value);

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 25);
                    billingEntity.CumPowerFailureCount = dataElement.Value == null ? -1 : Convert.ToInt64(dataElement.Value);

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 28);
                    billingEntity.CumBillingMDResetCount = dataElement.Value == null ? -1 : Convert.ToInt64(dataElement.Value);

                    dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 1063);
                    billingEntity.RPhaseMDDateTime = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));

                    dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 1064);
                    billingEntity.YPhaseMDDateTime = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));

                    dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 1065);
                    billingEntity.BPhaseMDDateTime = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));

                    dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 1066);
                    if (dataElement.DataDefinitionID == 0)
                        billingEntity.RPhaseMDkW = "------";
                    else
                        billingEntity.RPhaseMDkW = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 1067);
                    if (dataElement.DataDefinitionID == 0)
                        billingEntity.YPhaseMDkW = "------";
                    else
                        billingEntity.YPhaseMDkW = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 1068);
                    if (dataElement.DataDefinitionID == 0)
                        billingEntity.BPhaseMDkW = "------";
                    else
                        billingEntity.BPhaseMDkW = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 84);
                    if (dataElement.DataDefinitionID == 0)
                        billingEntity.CumulativeEnergyFraudkWh = null;
                    else
                        billingEntity.CumulativeEnergyFraudkWh = CommonMapper.FormatData(dataElement);
                    dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 1117);
                    if (dataElement.DataDefinitionID == 0)
                        billingEntity.CumulativeEnergyFraudkVAh = null;
                    else
                        billingEntity.CumulativeEnergyFraudkVAh = CommonMapper.FormatData(dataElement);


                    //user story 1000867
                    dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 2193);
                    if (dataElement.DataDefinitionID == 0)
                        billingEntity.MDkVArLagTZ0 = null;
                    else
                        billingEntity.MDkVArLagTZ0 = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 2194);
                    billingEntity.MDkVArLagDateTimeTZ0 = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));

                    dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 2195);
                    if (dataElement.DataDefinitionID == 0)
                        billingEntity.MDkVArLeadTZ0 = null;
                    else
                        billingEntity.MDkVArLeadTZ0 = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 2196);
                    billingEntity.MDkVArLeadDateTimeTZ0 = Convert.ToInt64(DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value));
                    //user story 1000867

                    #region JDVVNL
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2123);
                    if (dataElement.DataDefinitionID != 0)
                    {
                        billingEntity.MinimumVoltageLSIPAcrossDayRPhase = dataElement.Value;
                    }
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2124);
                    if (dataElement.DataDefinitionID != 0)
                    {
                        billingEntity.MinimumVoltageLSIPAcrossDayYPhase = dataElement.Value;
                    }

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2125);
                    if (dataElement.DataDefinitionID != 0)
                    {
                        billingEntity.MinimumVoltageLSIPAcrossDayBPhase = dataElement.Value;
                    }
                    //added abc in billing for 128K Meter

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2126);
                    if (dataElement.DataDefinitionID != 0)
                    {
                        billingEntity.ABCCodeBilling = dataElement.Value;
                    }

                    //added abc in billing for 128K Meter


                    #endregion

                    #region TOD Lag & Lead

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 362);
                    billingEntity.CumulativeEnergykvarhLagTZ1 = (dataElement.Value);
                    billingEntity.CumulativeEnergykvarhLagTZ1Q1 = billingEntity.CumulativeEnergykvarhLagTZ1;//add pradipta


                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 363);
                    billingEntity.CumulativeEnergykvarhLagTZ2 = (dataElement.Value);
                    billingEntity.CumulativeEnergykvarhLagTZ2Q1 = billingEntity.CumulativeEnergykvarhLagTZ2;//add pradipta

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 364);
                    billingEntity.CumulativeEnergykvarhLagTZ3 = (dataElement.Value);
                    billingEntity.CumulativeEnergykvarhLagTZ3Q1 = billingEntity.CumulativeEnergykvarhLagTZ3;//add pradipta //SarkarA code change 20180227 // corrected from TZ1Q3

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 365);
                    billingEntity.CumulativeEnergykvarhLagTZ4 = (dataElement.Value);
                    billingEntity.CumulativeEnergykvarhLagTZ4Q1 = billingEntity.CumulativeEnergykvarhLagTZ4;//add pradipta

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 366);
                    billingEntity.CumulativeEnergykvarhLagTZ5 = (dataElement.Value);
                    billingEntity.CumulativeEnergykvarhLagTZ5Q1 = billingEntity.CumulativeEnergykvarhLagTZ5;//add pradipta

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 367);
                    billingEntity.CumulativeEnergykvarhLagTZ6 = (dataElement.Value);
                    billingEntity.CumulativeEnergykvarhLagTZ6Q1 = billingEntity.CumulativeEnergykvarhLagTZ6;//add pradipta

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 368);
                    billingEntity.CumulativeEnergykvarhLagTZ7 = (dataElement.Value);
                    billingEntity.CumulativeEnergykvarhLagTZ7Q1 = billingEntity.CumulativeEnergykvarhLagTZ7;//add pradipta

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 369);
                    billingEntity.CumulativeEnergykvarhLagTZ8 = (dataElement.Value);
                    billingEntity.CumulativeEnergykvarhLagTZ8Q1 = billingEntity.CumulativeEnergykvarhLagTZ8;//add pradipta


                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 370);
                    billingEntity.CumulativeEnergykvarhLeadTZ1 = (dataElement.Value);
                    billingEntity.CumulativeEnergykvarhLeadTZ1Q4 = billingEntity.CumulativeEnergykvarhLeadTZ1;//ADD pradipta


                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 371);
                    billingEntity.CumulativeEnergykvarhLeadTZ2 = (dataElement.Value);
                    billingEntity.CumulativeEnergykvarhLeadTZ2Q4 = billingEntity.CumulativeEnergykvarhLeadTZ2;//ADD pradipta

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 372);
                    billingEntity.CumulativeEnergykvarhLeadTZ3 = (dataElement.Value);
                    billingEntity.CumulativeEnergykvarhLeadTZ3Q4 = billingEntity.CumulativeEnergykvarhLeadTZ3;//ADD pradipta

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 373);
                    billingEntity.CumulativeEnergykvarhLeadTZ4 = (dataElement.Value);
                    billingEntity.CumulativeEnergykvarhLeadTZ4Q4 = billingEntity.CumulativeEnergykvarhLeadTZ4;//ADD pradipta

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 374);
                    billingEntity.CumulativeEnergykvarhLeadTZ5 = (dataElement.Value);
                    billingEntity.CumulativeEnergykvarhLeadTZ5Q4 = billingEntity.CumulativeEnergykvarhLeadTZ5;//ADD pradipta

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 375);
                    billingEntity.CumulativeEnergykvarhLeadTZ6 = (dataElement.Value);
                    billingEntity.CumulativeEnergykvarhLeadTZ6Q4 = billingEntity.CumulativeEnergykvarhLeadTZ6;//ADD pradipta

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 376);
                    billingEntity.CumulativeEnergykvarhLeadTZ7 = (dataElement.Value);
                    billingEntity.CumulativeEnergykvarhLeadTZ7Q4 = billingEntity.CumulativeEnergykvarhLeadTZ7;//ADD pradipta

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 377);
                    billingEntity.CumulativeEnergykvarhLeadTZ8 = (dataElement.Value);
                    billingEntity.CumulativeEnergykvarhLeadTZ8Q4 = billingEntity.CumulativeEnergykvarhLeadTZ8;//ADD pradipta


                   
                    if (string.IsNullOrEmpty(billingEntity.CumulativeMDkw))
                    {
                        dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1115);

                        billingEntity.CumulativeMDkw = (dataElement.Value);
                    }
                    if (string.IsNullOrEmpty(billingEntity.CumulativeMDkva))
                    {
                        dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1116);
                        billingEntity.CumulativeMDkva = (dataElement.Value);
                    }
                    // **************  For Smart meter

                    if (string.IsNullOrEmpty(billingEntity.CumulativeMDkw))
                    {
                        dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1046);

                        billingEntity.CumulativeMDkw = (dataElement.Value);
                    }
                    if (string.IsNullOrEmpty(billingEntity.CumulativeMDkva))
                    {
                        dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1047);
                        billingEntity.CumulativeMDkva = (dataElement.Value);
                    }



                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1081);
                    billingEntity.CumulativeEnergykvarhLagTZ1 = (dataElement.Value);
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1082);
                    billingEntity.CumulativeEnergykvarhLagTZ2 = (dataElement.Value);
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1083);
                    billingEntity.CumulativeEnergykvarhLagTZ3 = (dataElement.Value);
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1084);
                    billingEntity.CumulativeEnergykvarhLagTZ4 = (dataElement.Value);
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1085);
                    billingEntity.CumulativeEnergykvarhLagTZ5 = (dataElement.Value);
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1086);
                    billingEntity.CumulativeEnergykvarhLagTZ6 = (dataElement.Value);
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1087);
                    billingEntity.CumulativeEnergykvarhLagTZ7 = (dataElement.Value);
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1088);
                    billingEntity.CumulativeEnergykvarhLagTZ8 = (dataElement.Value);

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1089);
                    billingEntity.CumulativeEnergykvarhLeadTZ1 = (dataElement.Value);
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1090);
                    billingEntity.CumulativeEnergykvarhLeadTZ2 = (dataElement.Value);
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1091);
                    billingEntity.CumulativeEnergykvarhLeadTZ3 = (dataElement.Value);
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1092);
                    billingEntity.CumulativeEnergykvarhLeadTZ4 = (dataElement.Value);
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1093);
                    billingEntity.CumulativeEnergykvarhLeadTZ5 = (dataElement.Value);
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1094);
                    billingEntity.CumulativeEnergykvarhLeadTZ6 = (dataElement.Value);
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1095);
                    billingEntity.CumulativeEnergykvarhLeadTZ7 = (dataElement.Value);
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1096);
                    billingEntity.CumulativeEnergykvarhLeadTZ8 = (dataElement.Value);


                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1097);
                    billingEntity.TODAveragePowerFactorTZ1 = (dataElement.Value);
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1098);
                    billingEntity.TODAveragePowerFactorTZ2 = (dataElement.Value);
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1099);
                    billingEntity.TODAveragePowerFactorTZ3 = (dataElement.Value);
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1100);
                    billingEntity.TODAveragePowerFactorTZ4 = (dataElement.Value);
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1101);
                    billingEntity.TODAveragePowerFactorTZ5 = (dataElement.Value);
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1102);
                    billingEntity.TODAveragePowerFactorTZ6 = (dataElement.Value);
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1103);
                    billingEntity.TODAveragePowerFactorTZ7 = (dataElement.Value);
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1104);
                    billingEntity.TODAveragePowerFactorTZ8 = (dataElement.Value);


                    #endregion

                    #region power on duration

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 26);
                    billingEntity.CumPowerOffDuration = dataElement.Value;

                    /// UGVCl tender Specific Change. It will be obsolete from next version
                    /// And commented code will uncommneted
                    try
                    {
                        //To solve the negative Power On Duration issue
                        if (generalEntity.InternalFirmwareVersion == "6.37" || generalEntity.InternalFirmwareVersion == "7.16"
                            || generalEntity.InternalFirmwareVersion == "8.06" || generalEntity.InternalFirmwareVersion == "7.22")
                        {
                            dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 83);
                            billingEntity.CumPowerOffDuration = dataElement.Value;
                        }
                        else
                        {
                            dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 83);
                            billingEntity.BillingWisePowerOffDuration = dataElement.Value;
                        }
                    }
                    catch (Exception ex)    //Exception log for catch block
                    {
                        logger.Log(LOGLEVELS.Error, "GetMappedEntity(List<ProfileData> billingData, DLMS650NamePlateDetailsEntity generalEntity)", ex);
                        dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 83);
                        billingEntity.BillingWisePowerOffDuration = dataElement.Value;
                    }
                    //dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 83);
                    //billingEntity.BillingWisePowerOffDuration = dataElement.Value;
                    /// UGVCl tender Specific Change. It will be obsolete from next version

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 191);
                    billingEntity.PowerOnDuration = dataElement.Value;

                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 196);
                    billingEntity.CumPowerOnDuration = dataElement.Value;

                    byte PowerOnDurationDisplay = 0;
                    PowerOnDurationDisplay = (byte)(PowerOnDurationDisplay | (billingEntity.BillingWisePowerOffDuration == null ? 0 : 1));
                    PowerOnDurationDisplay = (byte)(PowerOnDurationDisplay << 1);
                    PowerOnDurationDisplay = (byte)(PowerOnDurationDisplay | (billingEntity.CumPowerOffDuration == null ? 0 : 1));
                    PowerOnDurationDisplay = (byte)(PowerOnDurationDisplay << 1);
                    PowerOnDurationDisplay = (byte)(PowerOnDurationDisplay | (billingEntity.PowerOnDuration == null ? 0 : 1));
                    PowerOnDurationDisplay = (byte)(PowerOnDurationDisplay << 1);
                    PowerOnDurationDisplay = (byte)(PowerOnDurationDisplay | (billingEntity.CumPowerOnDuration == null ? 0 : 1));
                    billingEntity.PowerOnDurationDisplay = PowerOnDurationDisplay;

                    #endregion

                    #region TOD Average PF Smart Meter
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1021);
                    billingEntity.TODAveragePowerFactorTZ1 = (dataElement.Value);
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1022);
                    billingEntity.TODAveragePowerFactorTZ2 = (dataElement.Value);
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1023);
                    billingEntity.TODAveragePowerFactorTZ3 = (dataElement.Value);
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1024);
                    billingEntity.TODAveragePowerFactorTZ4 = (dataElement.Value);
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1025);
                    billingEntity.TODAveragePowerFactorTZ5 = (dataElement.Value);
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1026);
                    billingEntity.TODAveragePowerFactorTZ6 = (dataElement.Value);
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1027);
                    billingEntity.TODAveragePowerFactorTZ7 = (dataElement.Value);
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1028);
                    billingEntity.TODAveragePowerFactorTZ8 = (dataElement.Value);
                    #endregion
                    
                    #region TOD Average PF PUMA/RUBY Meter
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1097);
                    if(dataElement.Value != null)
                        billingEntity.TODAveragePowerFactorTZ1 = (dataElement.Value);
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1098);
                    if (dataElement.Value != null)
                        billingEntity.TODAveragePowerFactorTZ2 = (dataElement.Value);
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1099);
                    if (dataElement.Value != null)
                        billingEntity.TODAveragePowerFactorTZ3 = (dataElement.Value);
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1100);
                    if (dataElement.Value != null)
                        billingEntity.TODAveragePowerFactorTZ4 = (dataElement.Value);
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1101);
                    if (dataElement.Value != null)
                        billingEntity.TODAveragePowerFactorTZ5 = (dataElement.Value);
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1102);
                    if (dataElement.Value != null)
                        billingEntity.TODAveragePowerFactorTZ6 = (dataElement.Value);
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1103);
                    if (dataElement.Value != null)
                        billingEntity.TODAveragePowerFactorTZ7 = (dataElement.Value);
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1104);
                    if (dataElement.Value != null)
                        billingEntity.TODAveragePowerFactorTZ8 = (dataElement.Value);
                    #endregion
                    #region TOD Average Export PF 
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2197); //story 1024441 Add TOD Export PF
                    if (dataElement.Value != null)
                        billingEntity.TODAverageExportPowerFactorTZ1 = (dataElement.Value);
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2198);
                    if (dataElement.Value != null)
                        billingEntity.TODAverageExportPowerFactorTZ2 = (dataElement.Value);
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2199);
                    if (dataElement.Value != null)
                        billingEntity.TODAverageExportPowerFactorTZ3 = (dataElement.Value);
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2200);
                    if (dataElement.Value != null)
                        billingEntity.TODAverageExportPowerFactorTZ4 = (dataElement.Value);
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2201);
                    if (dataElement.Value != null)
                        billingEntity.TODAverageExportPowerFactorTZ5 = (dataElement.Value);
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2202);
                    if (dataElement.Value != null)
                        billingEntity.TODAverageExportPowerFactorTZ6 = (dataElement.Value);
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2203);
                    if (dataElement.Value != null)
                        billingEntity.TODAverageExportPowerFactorTZ7 = (dataElement.Value);
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2204);
                    if (dataElement.Value != null)
                        billingEntity.TODAverageExportPowerFactorTZ8 = (dataElement.Value);
                    #endregion
                    //WB Tender Specific Changes for Billing Type Parameters
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 360);
                    if (dataElement.DataDefinitionID != 0)
                    {
                        billingEntity.BillingType = dataElement.Value;
                    }
                    else
                    {
                        // user stroy: 490966 WB tender specific check implemented for Billing Reset Type OBIS code change
                        dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1107);
                        if (dataElement.DataDefinitionID != 0)
                        {
                            string wbFlag = "_WB";
                            billingEntity.BillingType = dataElement.Value + wbFlag;
                        }
                        else
                        {
                            dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 2160);
                            billingEntity.BillingType = dataElement.Value;
                        }
                    }
                    // CSPDCL Sapphire tender specific parameter, implemented for Billing Reset Type OBIS code change for Utility specefic
                    dataElement = CommonMapper.GetDataElementByDataDefId(billingRecords, 2211);
                    if (dataElement.DataDefinitionID != 0) billingEntity.BillingType = dataElement.Value;

                    //dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 360);
                    //billingEntity.BillingType = dataElement.Value;

                    //// user stroy: 490966 WB tender specific check implemented for Billing Reset Type OBIS code change
                    //if (string.IsNullOrEmpty(dataElement.Value) && dataElement.Value == "0")
                    //{
                    //    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(billingRecords, 1107);
                    //    billingEntity.BillingType = dataElement.Value;
                    //}

                    if (!string.IsNullOrEmpty(billingEntity.BillingDate.ToString()) && !(billingEntity.BillingDate == 0))
                    {
                        billingEntityList.Add(billingEntity);
                    }
                }
                masterEntity.Billing = billingEntityList;

                //Get billing columns.
                Dictionary<string, string> OBISLoadSurveyColumns = CommonMapper.GetOBISCodeColumnNamesBilling();
                foreach (DataElement data in billingData[0].ListMeterDataPacket[0].ListDataElementValue)
                {
                    DLMSCOMMAND obisInfo = meterConfigParser.GetObisInfoFromRepository(data.DataDefinitionID);
                    foreach (KeyValuePair<string, string> pair in OBISLoadSurveyColumns)
                    {
                        if (obisInfo.OBISCODE == pair.Key)
                        {
                            if (lsColumnNames.Split(',').ToList<string>().IndexOf(pair.Value) == -1)
                            {
                                lsColumnNames += pair.Value + ",";
                                break;
                            }
                        }
                    }
                }

                #region Net_Column_Added_for_Net_Meters_When_Not_Coming_From_Meter

                //NetkWh Ratewise Add in case MeterVariant is 3 or 4 and not present
                if ((generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.THREE || generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.FOUR) && (lsColumnNames.Split(',').ToList<string>().IndexOf("CumulativeEnergykWhNet") == -1))
                {
                    lsColumnNames += "CumulativeEnergykWhNet,";
                }
                if ((generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.THREE || generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.FOUR) && (lsColumnNames.Split(',').ToList<string>().IndexOf("CumulativeEnergykWhTZ1Net") == -1))
                {
                    lsColumnNames += "CumulativeEnergykWhTZ1Net,";
                }
                if ((generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.THREE || generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.FOUR) && (lsColumnNames.Split(',').ToList<string>().IndexOf("CumulativeEnergykWhTZ2Net") == -1))
                {
                    lsColumnNames += "CumulativeEnergykWhTZ2Net,";
                }
                if ((generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.THREE || generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.FOUR) && (lsColumnNames.Split(',').ToList<string>().IndexOf("CumulativeEnergykWhTZ3Net") == -1))
                {
                    lsColumnNames += "CumulativeEnergykWhTZ3Net,";
                }
                if ((generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.THREE || generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.FOUR) && (lsColumnNames.Split(',').ToList<string>().IndexOf("CumulativeEnergykWhTZ4Net") == -1))
                {
                    lsColumnNames += "CumulativeEnergykWhTZ4Net,";
                }
                if ((generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.THREE || generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.FOUR) && (lsColumnNames.Split(',').ToList<string>().IndexOf("CumulativeEnergykWhTZ5Net") == -1))
                {
                    lsColumnNames += "CumulativeEnergykWhTZ5Net,";
                }
                if ((generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.THREE || generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.FOUR) && (lsColumnNames.Split(',').ToList<string>().IndexOf("CumulativeEnergykWhTZ6Net") == -1))
                {
                    lsColumnNames += "CumulativeEnergykWhTZ6Net,";
                }
                if ((generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.THREE || generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.FOUR) && (lsColumnNames.Split(',').ToList<string>().IndexOf("CumulativeEnergykWhTZ7Net") == -1))
                {
                    lsColumnNames += "CumulativeEnergykWhTZ7Net,";
                }
                if ((generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.THREE || generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.FOUR) && (lsColumnNames.Split(',').ToList<string>().IndexOf("CumulativeEnergykWhTZ8Net") == -1))
                {
                    lsColumnNames += "CumulativeEnergykWhTZ8Net,";
                }



                //NetkVAh Ratewise Add in case MeterVariant is 3 or 4 and not present
                if ((generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.THREE || generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.FOUR) && (lsColumnNames.Split(',').ToList<string>().IndexOf("CumulativeEnergykVAhNet") == -1))
                {
                    lsColumnNames += "CumulativeEnergykVAhNet,";
                }
                if ((generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.THREE || generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.FOUR) && (lsColumnNames.Split(',').ToList<string>().IndexOf("CumulativeEnergykVAhTZ1Net") == -1))
                {
                    lsColumnNames += "CumulativeEnergykVAhTZ1Net,";
                }
                if ((generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.THREE || generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.FOUR) && (lsColumnNames.Split(',').ToList<string>().IndexOf("CumulativeEnergykVAhTZ2Net") == -1))
                {
                    lsColumnNames += "CumulativeEnergykVAhTZ2Net,";
                }
                if ((generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.THREE || generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.FOUR) && (lsColumnNames.Split(',').ToList<string>().IndexOf("CumulativeEnergykVAhTZ3Net") == -1))
                {
                    lsColumnNames += "CumulativeEnergykVAhTZ3Net,";
                }
                if ((generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.THREE || generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.FOUR) && (lsColumnNames.Split(',').ToList<string>().IndexOf("CumulativeEnergykVAhTZ4Net") == -1))
                {
                    lsColumnNames += "CumulativeEnergykVAhTZ4Net,";
                }
                if ((generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.THREE || generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.FOUR) && (lsColumnNames.Split(',').ToList<string>().IndexOf("CumulativeEnergykVAhTZ5Net") == -1))
                {
                    lsColumnNames += "CumulativeEnergykVAhTZ5Net,";
                }
                if ((generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.THREE || generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.FOUR) && (lsColumnNames.Split(',').ToList<string>().IndexOf("CumulativeEnergykVAhTZ6Net") == -1))
                {
                    lsColumnNames += "CumulativeEnergykVAhTZ6Net,";
                }
                if ((generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.THREE || generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.FOUR) && (lsColumnNames.Split(',').ToList<string>().IndexOf("CumulativeEnergykVAhTZ7Net") == -1))
                {
                    lsColumnNames += "CumulativeEnergykVAhTZ7Net,";
                }
                if ((generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.THREE || generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.FOUR) && (lsColumnNames.Split(',').ToList<string>().IndexOf("CumulativeEnergykVAhTZ8Net") == -1))
                {
                    lsColumnNames += "CumulativeEnergykVAhTZ8Net,";
                }



                //NetMDkW Ratewise Add in case MeterVariant is 3 or 4 and not present
                if ((generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.THREE || generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.FOUR) && (lsColumnNames.Split(',').ToList<string>().IndexOf("MDkWTZ0Net") == -1))
                {
                    lsColumnNames += "MDkWTZ0Net,";
                }

                if ((generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.THREE || generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.FOUR) && (lsColumnNames.Split(',').ToList<string>().IndexOf("MDkWTZ1Net") == -1))
                {
                    lsColumnNames += "MDkWTZ1Net,";
                }

                if ((generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.THREE || generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.FOUR) && (lsColumnNames.Split(',').ToList<string>().IndexOf("MDkWTZ2Net") == -1))
                {
                    lsColumnNames += "MDkWTZ2Net,";
                }

                if ((generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.THREE || generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.FOUR) && (lsColumnNames.Split(',').ToList<string>().IndexOf("MDkWTZ3Net") == -1))
                {
                    lsColumnNames += "MDkWTZ3Net,";
                }
                if ((generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.THREE || generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.FOUR) && (lsColumnNames.Split(',').ToList<string>().IndexOf("MDkWTZ4Net") == -1))
                {
                    lsColumnNames += "MDkWTZ4Net,";
                }
                if ((generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.THREE || generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.FOUR) && (lsColumnNames.Split(',').ToList<string>().IndexOf("MDkWTZ5Net") == -1))
                {
                    lsColumnNames += "MDkWTZ5Net,";
                }
                if ((generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.THREE || generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.FOUR) && (lsColumnNames.Split(',').ToList<string>().IndexOf("MDkWTZ6Net") == -1))
                {
                    lsColumnNames += "MDkWTZ6Net,";
                }
                if ((generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.THREE || generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.FOUR) && (lsColumnNames.Split(',').ToList<string>().IndexOf("MDkWTZ7Net") == -1))
                {
                    lsColumnNames += "MDkWTZ7Net,";
                }
                if ((generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.THREE || generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.FOUR) && (lsColumnNames.Split(',').ToList<string>().IndexOf("MDkWTZ8Net") == -1))
                {
                    lsColumnNames += "MDkWTZ8Net,";
                }

                //NetMDkWDateTime Ratewise Add in case MeterVariant is 3 or 4 and not present
                if ((generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.THREE || generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.FOUR) && (lsColumnNames.Split(',').ToList<string>().IndexOf("MDkWDateTimeTZ0Net") == -1))
                {
                    lsColumnNames += "MDkWDateTimeTZ0Net,";
                }
                if ((generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.THREE || generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.FOUR) && (lsColumnNames.Split(',').ToList<string>().IndexOf("MDkWDateTimeTZ1Net") == -1))
                {
                    lsColumnNames += "MDkWDateTimeTZ1Net,";
                }
                if ((generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.THREE || generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.FOUR) && (lsColumnNames.Split(',').ToList<string>().IndexOf("MDkWDateTimeTZ2Net") == -1))
                {
                    lsColumnNames += "MDkWDateTimeTZ2Net,";
                }
                if ((generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.THREE || generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.FOUR) && (lsColumnNames.Split(',').ToList<string>().IndexOf("MDkWDateTimeTZ3Net") == -1))
                {
                    lsColumnNames += "MDkWDateTimeTZ3Net,";
                }
                if ((generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.THREE || generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.FOUR) && (lsColumnNames.Split(',').ToList<string>().IndexOf("MDkWDateTimeTZ4Net") == -1))
                {
                    lsColumnNames += "MDkWDateTimeTZ4Net,";
                }
                if ((generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.THREE || generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.FOUR) && (lsColumnNames.Split(',').ToList<string>().IndexOf("MDkWDateTimeTZ5Net") == -1))
                {
                    lsColumnNames += "MDkWDateTimeTZ5Net,";
                }
                if ((generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.THREE || generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.FOUR) && (lsColumnNames.Split(',').ToList<string>().IndexOf("MDkWDateTimeTZ6Net") == -1))
                {
                    lsColumnNames += "MDkWDateTimeTZ6Net,";
                }
                if ((generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.THREE || generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.FOUR) && (lsColumnNames.Split(',').ToList<string>().IndexOf("MDkWDateTimeTZ7Net") == -1))
                {
                    lsColumnNames += "MDkWDateTimeTZ7Net,";
                }
                if ((generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.THREE || generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.FOUR) && (lsColumnNames.Split(',').ToList<string>().IndexOf("MDkWDateTimeTZ8Net") == -1))
                {
                    lsColumnNames += "MDkWDateTimeTZ8Net,";
                }

                ////NetMDkVA Ratewise Add in case MeterVariant is 3 or 4 and not present
                if ((generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.THREE || generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.FOUR) && (lsColumnNames.Split(',').ToList<string>().IndexOf("MDkVATZ0Net") == -1))
                {
                    lsColumnNames += "MDkVATZ0Net,";
                }


                if ((generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.THREE || generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.FOUR) && (lsColumnNames.Split(',').ToList<string>().IndexOf("MDkVATZ1Net") == -1))
                {
                    lsColumnNames += "MDkVATZ1Net,";
                }


                if ((generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.THREE || generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.FOUR) && (lsColumnNames.Split(',').ToList<string>().IndexOf("MDkVATZ2Net") == -1))
                {
                    lsColumnNames += "MDkVATZ2Net,";
                }


                if ((generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.THREE || generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.FOUR) && (lsColumnNames.Split(',').ToList<string>().IndexOf("MDkVATZ3Net") == -1))
                {
                    lsColumnNames += "MDkVATZ3Net,";
                }


                if ((generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.THREE || generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.FOUR) && (lsColumnNames.Split(',').ToList<string>().IndexOf("MDkVATZ4Net") == -1))
                {
                    lsColumnNames += "MDkVATZ4Net,";
                }


                if ((generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.THREE || generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.FOUR) && (lsColumnNames.Split(',').ToList<string>().IndexOf("MDkVATZ5Net") == -1))
                {
                    lsColumnNames += "MDkVATZ5Net,";
                }


                if ((generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.THREE || generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.FOUR) && (lsColumnNames.Split(',').ToList<string>().IndexOf("MDkVATZ6Net") == -1))
                {
                    lsColumnNames += "MDkVATZ6Net,";
                }

                if ((generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.THREE || generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.FOUR) && (lsColumnNames.Split(',').ToList<string>().IndexOf("MDkVATZ7Net") == -1))
                {
                    lsColumnNames += "MDkVATZ7Net,";
                }


                if ((generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.THREE || generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.FOUR) && (lsColumnNames.Split(',').ToList<string>().IndexOf("MDkVATZ8Net") == -1))
                {
                    lsColumnNames += "MDkVATZ8Net,";
                }


                //NetMDkVADateTime Ratewise Add in case MeterVariant is 3 or 4 and not present
                if ((generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.THREE || generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.FOUR) && (lsColumnNames.Split(',').ToList<string>().IndexOf("MDkVADateTimeTZ0Net") == -1))
                {
                    lsColumnNames += "MDkVADateTimeTZ0Net,";
                }
                if ((generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.THREE || generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.FOUR) && (lsColumnNames.Split(',').ToList<string>().IndexOf("MDkVADateTimeTZ1Net") == -1))
                {
                    lsColumnNames += "MDkVADateTimeTZ1Net,";
                }
                if ((generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.THREE || generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.FOUR) && (lsColumnNames.Split(',').ToList<string>().IndexOf("MDkVADateTimeTZ2Net") == -1))
                {
                    lsColumnNames += "MDkVADateTimeTZ2Net,";
                }
                if ((generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.THREE || generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.FOUR) && (lsColumnNames.Split(',').ToList<string>().IndexOf("MDkVADateTimeTZ3Net") == -1))
                {
                    lsColumnNames += "MDkVADateTimeTZ3Net,";
                }
                if ((generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.THREE || generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.FOUR) && (lsColumnNames.Split(',').ToList<string>().IndexOf("MDkVADateTimeTZ4Net") == -1))
                {
                    lsColumnNames += "MDkVADateTimeTZ4Net,";
                }
                if ((generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.THREE || generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.FOUR) && (lsColumnNames.Split(',').ToList<string>().IndexOf("MDkVADateTimeTZ5Net") == -1))
                {
                    lsColumnNames += "MDkVADateTimeTZ5Net,";
                }
                if ((generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.THREE || generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.FOUR) && (lsColumnNames.Split(',').ToList<string>().IndexOf("MDkVADateTimeTZ6Net") == -1))
                {
                    lsColumnNames += "MDkVADateTimeTZ6Net,";
                }
                if ((generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.THREE || generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.FOUR) && (lsColumnNames.Split(',').ToList<string>().IndexOf("MDkVADateTimeTZ7Net") == -1))
                {
                    lsColumnNames += "MDkVADateTimeTZ7Net,";
                }
                if ((generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.THREE || generalEntity.NetMeterVariantInfo == CAB.Framework.MeterVariant.FOUR) && (lsColumnNames.Split(',').ToList<string>().IndexOf("MDkVADateTimeTZ8Net") == -1))
                {
                    lsColumnNames += "MDkVADateTimeTZ8Net,";
                }


                #endregion

                billingColumns.ColumnsNames = lsColumnNames.Remove(lsColumnNames.LastIndexOf(","));
                masterEntity.BillingParameterColumns = billingColumns;

            }

            return masterEntity;
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
