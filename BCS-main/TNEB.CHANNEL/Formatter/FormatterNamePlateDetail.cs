/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 									|
 * |											           				            								|
 * | 																    											|
 * |----------------------------------------------------------------------------------------------------------------| */

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CAB.Entity;
using CAB.IECChannel.ReadOut;
using System.Windows.Forms;

namespace CAB.IECChannel.Formatter
{
    public class FormatterNamePlateDetail
    {

        public void GetData(string data, ref string[] NamePlateDetail)
        {
            try
            {
                int counter = 0;
                data = data.Replace(FormatterConstant.ENTERCARRAGE, string.Empty);
                MatchCollection matches = FormatterCommon.ValidateData(data, FormatterConstant.NamePlateDetail);
                string[] namePlateDetailData = new string[matches.Count];
                foreach (Match match in matches)
                {
                    GroupCollection groups = match.Groups;
                    namePlateDetailData[counter++] = groups[0].Value;
                }
                NamePlateDetail = FormatterCommon.RemoveDuplicateData(namePlateDetailData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void SplitData(string tempData, ref IECBillingGeneralNFEntity billingGeneralNFEntity)
        {
            try
            {
                if (tempData == null)
                    return;
                string data = tempData.Trim();
                NamePlateDetailEntity namePlateDetailEntity = new NamePlateDetailEntity();
                string mID = data.Substring(data.IndexOf("XXX") + 4).Trim();
                namePlateDetailEntity.MeterID = mID.Substring(0, mID.IndexOf("/")).Trim();
                namePlateDetailEntity.ReadingDateTime = Convert.ToInt64(data.Substring(16, 14));
                //namePlateDetailEntity.CMRIID = data.Substring(31, 9).Trim();
                //string cmriType = Convert.ToString(namePlateDetailEntity.CMRIID.Substring(0, 1));
                //if (cmriType.Trim().ToUpper().Equals("A"))
                //    namePlateDetailEntity.CMRIType = "Analogic";
                //else if (cmriType.Trim().ToUpper().Equals("S"))
                //    namePlateDetailEntity.CMRIType = "Sands";
                //else
                //    namePlateDetailEntity.CMRIType = "BCS";
                string type = data.Substring(38, 2).Trim();
                if (type == "01")
                    namePlateDetailEntity.MeterType = "3PH 4W";
                else
                    namePlateDetailEntity.MeterType = "";
                string curRating = data.Substring(40, 2).Trim();
                if (curRating == "00")
                    namePlateDetailEntity.CurrentRating = "10-40";
                else if (curRating == "01")
                    namePlateDetailEntity.CurrentRating = "10-60";
                else if (curRating == "02")
                    namePlateDetailEntity.CurrentRating = "05-30";
                else if (curRating == "03")
                    namePlateDetailEntity.CurrentRating = "10-100";
                else if (curRating == "04")
                    namePlateDetailEntity.CurrentRating = "20-100";
                /* GKG added enum for TANGEDCO tender 05/06/2013 */
                else if (curRating == "05")
                {
                    namePlateDetailEntity.CurrentRating = "50-100";
                }
                /* GKG added enum for TANGEDCO tender 05/06/2013 */
                else
                    namePlateDetailEntity.CurrentRating = "";
                string meterConstant = FormatterCommon.FilterData(data, 42, 4);
                if (!string.IsNullOrEmpty(meterConstant))
                    meterConstant = meterConstant + " Imp/kWh; Imp/kvarh";
                namePlateDetailEntity.MeterConstant = meterConstant;
                string day = data.Substring(46, 2).Trim();
                namePlateDetailEntity.ManufacturingDate = FormatterCommon.FilterData(data, 48, 4);// string.Concat(System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Int16.Parse(day)), " - ", FormatterCommon.FilterData(data, 48, 4));
                if (data.Length > 52)
                {
                    string voltRating = data.Substring(52, 4).Trim();
                    voltRating = Convert.ToString(Convert.ToInt32(voltRating, 16), 2);
                    while (voltRating.Length < 16) { voltRating = "0" + voltRating; }
                    if (voltRating.Substring(0, 1) == "0")
                    {
                        voltRating = String.Format("{0:X4}", Convert.ToInt32(voltRating, 2));
                        voltRating = Convert.ToString(Convert.ToInt32(voltRating, 16));
                    }
                    else
                    {
                        voltRating = String.Format("{0:X4}", Convert.ToInt32(voltRating, 2));
                        voltRating = Convert.ToString(Convert.ToInt32(voltRating, 16));
                    }
                    namePlateDetailEntity.VoltageRating = voltRating;
                }
                else
                    namePlateDetailEntity.VoltageRating = "----";
                billingGeneralNFEntity.listNamePlateDetail.Add(namePlateDetailEntity);
            }
            catch (Exception)
            {
               // MessageBox.Show("Corrupted name plate detail available in file.", "E-250 BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

