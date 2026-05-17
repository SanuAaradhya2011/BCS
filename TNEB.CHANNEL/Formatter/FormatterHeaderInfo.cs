using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CAB.Entity;
using System.Windows.Forms;

namespace CAB.IECChannel.Formatter
{
    public class FormatterHeaderInfo
    {
        MeterDataHeaderInfoEntity meterDataHeaderInfoEntity = new MeterDataHeaderInfoEntity();

        public void GetData(string data, ref string[] headerInfo)
        {
            try
            {
                int counter = 0;
                data = data.Replace(FormatterConstant.ENTERCARRAGE, string.Empty);
                MatchCollection matches = FormatterCommon.ValidateData(data, FormatterConstant.HEADERINFO);
                string[] headerInfoData = new string[matches.Count];
                foreach (Match match in matches)
                {
                    GroupCollection groups = match.Groups;
                    headerInfoData[counter++] = groups[0].Value;
                }
                headerInfo = FormatterCommon.RemoveDuplicateData(headerInfoData);
                
                //counter = 0;
                //int MaxLength = 0;
                //while (counter <= availableData.GetUpperBound(0))
                //{
                //    //string[] tempData = FormatterCommon.GetExpression(FormatterConstant.HEADERINFO).Split(availableData[counter]);
                //    //if (tempData.Length > MaxLength)
                //    //    MaxLength = tempData.Length;
                //    //for (MaxLength = 0; MaxLength < tempData.Length; MaxLength++)
                //    //    headerInfo[counter, MaxLength] = tempData[MaxLength];
                //    //counter++;
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void SplitData(string tempData, ref IECBillingGeneralNFEntity billingGeneralNFEntity)
        {
            bool isDataCorrupt = false;
            try
            {
                if (tempData == null)
                    return;
                bool OuterFlag = true;
                bool InnerFlag = true;
               
                MeterDataHeaderInfoEntity meterDataHeaderInfoEntity = new MeterDataHeaderInfoEntity();
                const string regexFraud = @"(HD(([\w\W]*?)\04))";
                MatchCollection matches = Regex.Matches(tempData, regexFraud, RegexOptions.Multiline | RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);
                string[] headerInfo = new string[matches.Count];

                string tmpText = tempData.Trim();
                tmpText = tmpText.Replace("\r", string.Empty);
                tmpText = tmpText.Replace("\n", string.Empty);
                tmpText = tmpText.Replace("\a", string.Empty);
                tmpText = tmpText.Replace("\b", string.Empty);

                meterDataHeaderInfoEntity.meterId = tmpText.Substring(tmpText.IndexOf("HD/") + 3);
                meterDataHeaderInfoEntity.meterId = meterDataHeaderInfoEntity.meterId.Substring(4, meterDataHeaderInfoEntity.meterId.IndexOf("/") - 4);

                string readingDateTime = tmpText.Substring(tmpText.IndexOf(meterDataHeaderInfoEntity.meterId) + 1 + meterDataHeaderInfoEntity.meterId.Length);
                readingDateTime = readingDateTime.Substring(0, readingDateTime.IndexOf("/"));

                meterDataHeaderInfoEntity.readingDateTime = Convert.ToInt64(readingDateTime);

                int count = 0;
                foreach (Match match in matches)
                {
                    GroupCollection groups = match.Groups;
                    headerInfo[count++] = groups["0"].Value;
                }

                string headerInfoData = string.Empty;
                /* GKG 28/01/2013 TANGEDCO ISSUE*/

                //if (headerInfo[0].Contains("/M"))
                //{
                //    headerInfoData = headerInfo[0].Substring(40, 50);
                //}
                //else
                //{
                //    headerInfoData = headerInfo[0].Substring(30, 50);
                //}

                if (headerInfo[0].Contains("/M"))
                {
                    headerInfoData = headerInfo[0].Substring(40, 52);
                }
                else
                {
                    headerInfoData = headerInfo[0].Substring(30, 52);
                }

                /* GKG 28/01/2013 TANGEDCO ISSUE*/
                if (headerInfoData.Substring(2, 2) == "01")

                    meterDataHeaderInfoEntity.MD1KWDemandType = "Block Demand";
                else
                    meterDataHeaderInfoEntity.MD1KWDemandType = "Sliding Demand";
                meterDataHeaderInfoEntity.MD1KWTimeInterval = Convert.ToString(Convert.ToInt32(headerInfoData.Substring(4, 2)));
                while (meterDataHeaderInfoEntity.MD1KWTimeInterval.Length < 2) meterDataHeaderInfoEntity.MD1KWTimeInterval = "0" + meterDataHeaderInfoEntity.MD1KWTimeInterval;
                if (meterDataHeaderInfoEntity.MD1KWTimeInterval == "00" || meterDataHeaderInfoEntity.MD1KWTimeInterval == "15" || meterDataHeaderInfoEntity.MD1KWTimeInterval == "30" || meterDataHeaderInfoEntity.MD1KWTimeInterval == "60") {isDataCorrupt = false; }
                else { meterDataHeaderInfoEntity.MD1KWTimeInterval = null; isDataCorrupt = true; }

                meterDataHeaderInfoEntity.MD1KWSubInterval = Convert.ToString(Convert.ToInt32(headerInfoData.Substring(6, 2)));
                while (meterDataHeaderInfoEntity.MD1KWSubInterval.Length < 2) meterDataHeaderInfoEntity.MD1KWSubInterval = "0" + meterDataHeaderInfoEntity.MD1KWSubInterval;
                if (meterDataHeaderInfoEntity.MD1KWSubInterval == "00" || meterDataHeaderInfoEntity.MD1KWSubInterval == "05" || meterDataHeaderInfoEntity.MD1KWSubInterval == "10" || meterDataHeaderInfoEntity.MD1KWSubInterval == "15" || meterDataHeaderInfoEntity.MD1KWSubInterval == "30") { isDataCorrupt = false; }
                else { meterDataHeaderInfoEntity.MD1KWSubInterval = null; isDataCorrupt = true; }

                if (headerInfoData.Substring(10, 2) == "01")
                    meterDataHeaderInfoEntity.MD2KVADemandType = "Block Demand";
                else
                    meterDataHeaderInfoEntity.MD2KVADemandType = "Sliding Demand";
                meterDataHeaderInfoEntity.MD2KVATimeInterval = Convert.ToString(Convert.ToInt32(headerInfoData.Substring(12, 2)));
                while (meterDataHeaderInfoEntity.MD2KVATimeInterval.Length < 2) meterDataHeaderInfoEntity.MD2KVATimeInterval = "0" + meterDataHeaderInfoEntity.MD2KVATimeInterval;
                if (meterDataHeaderInfoEntity.MD2KVATimeInterval == "00" || meterDataHeaderInfoEntity.MD2KVATimeInterval == "15" || meterDataHeaderInfoEntity.MD2KVATimeInterval == "30" || meterDataHeaderInfoEntity.MD2KVATimeInterval == "60") {  isDataCorrupt = false; }
                else { meterDataHeaderInfoEntity.MD2KVATimeInterval = null; isDataCorrupt = true; }

                meterDataHeaderInfoEntity.MD2KVASubInterval = Convert.ToString(Convert.ToInt32(headerInfoData.Substring(14, 2)));
                while (meterDataHeaderInfoEntity.MD2KVASubInterval.Length < 2) meterDataHeaderInfoEntity.MD2KVASubInterval = "0" + meterDataHeaderInfoEntity.MD2KVASubInterval;
                if (meterDataHeaderInfoEntity.MD2KVASubInterval == "00" || meterDataHeaderInfoEntity.MD2KVASubInterval == "05" || meterDataHeaderInfoEntity.MD2KVASubInterval == "10" || meterDataHeaderInfoEntity.MD2KVASubInterval == "15" || meterDataHeaderInfoEntity.MD2KVASubInterval == "30") { isDataCorrupt = false; }
                else { meterDataHeaderInfoEntity.MD2KVASubInterval = null; isDataCorrupt = true; }

                if (headerInfoData.Substring(24, 2) == "00")
                    meterDataHeaderInfoEntity.BillingType = "End of Month";
                else
                    meterDataHeaderInfoEntity.BillingType = "User Defined";

                if (meterDataHeaderInfoEntity.BillingType == "User Defined")
                {
                    meterDataHeaderInfoEntity.BillingDate = Convert.ToString(Convert.ToInt32(headerInfoData.Substring(26, 2)));
                    while (meterDataHeaderInfoEntity.BillingDate.Length < 2) { meterDataHeaderInfoEntity.BillingDate = "0" + meterDataHeaderInfoEntity.BillingDate; }
                    meterDataHeaderInfoEntity.BillingHour = Convert.ToString(Convert.ToInt32(headerInfoData.Substring(28, 2)));
                    while (meterDataHeaderInfoEntity.BillingHour.Length < 2) { meterDataHeaderInfoEntity.BillingHour = "0" + meterDataHeaderInfoEntity.BillingHour; }
                    meterDataHeaderInfoEntity.BillingMinute  = Convert.ToString(Convert.ToInt32(headerInfoData.Substring(30, 2)));
                    while (meterDataHeaderInfoEntity.BillingMinute.Length < 2) { meterDataHeaderInfoEntity.BillingMinute = "0" + meterDataHeaderInfoEntity.BillingMinute ; }
                }
                else
                {
                    meterDataHeaderInfoEntity.BillingDate = "--------";
                    meterDataHeaderInfoEntity.BillingHour = "--------";
                    meterDataHeaderInfoEntity.BillingMinute = "--------";
                }

                /* GKG 28/01/2013 TANGEDCO ISSUE*/
                if (meterDataHeaderInfoEntity.BillingType == "User Defined")
                {
                    string billingSubType = headerInfoData.Substring(50, 2);
                    if (billingSubType == "00")
                        meterDataHeaderInfoEntity.BillingType = meterDataHeaderInfoEntity.BillingType + " - Bimonthly - Even";
                    else if (billingSubType == "01")
                        meterDataHeaderInfoEntity.BillingType = meterDataHeaderInfoEntity.BillingType + " - Bimonthly - Odd";
                    else if (billingSubType == "02")
                        meterDataHeaderInfoEntity.BillingType = meterDataHeaderInfoEntity.BillingType + " - Monthly";
                }
                /* GKG 28/01/2013 TANGEDCO ISSUE*/
                if (headerInfoData.Substring(32, 2) == "00")
                    meterDataHeaderInfoEntity.PFLogic = "Lead as Unity";
                else
                    meterDataHeaderInfoEntity.PFLogic = "Lead as Lead";

                meterDataHeaderInfoEntity.PowerOffDays = Convert.ToString(Convert.ToInt32(headerInfoData.Substring(34, 6), 10));
                meterDataHeaderInfoEntity.MeterConstant = Convert.ToString(Convert.ToInt32(headerInfoData.Substring(40, 4), 16)) + " Imp/kWh; Imp/kvarh";
                meterDataHeaderInfoEntity.InternalCTPTRatio = Convert.ToString(Convert.ToInt32(headerInfoData.Substring(44, 2), 16));
                /* GKG 136014 FW version Not displaying correctly */
                //meterDataHeaderInfoEntity.SoftwareVersion = Convert.ToString(Convert.ToDouble(Convert.ToInt32(headerInfoData.Substring(46, 4), 16)) / 100);
                meterDataHeaderInfoEntity.SoftwareVersion = String.Format("{0:0.00}", Convert.ToDouble( Convert.ToInt32(headerInfoData.Substring(46, 4),16)) / 100);
                /* GKG 136014 FW version Not displaying correctly */
                if (!(InnerFlag == false && OuterFlag == false) )
                    billingGeneralNFEntity.listHeaderInfo.Add(meterDataHeaderInfoEntity);// = null;
                //else
                //    billingGeneralNFEntity.HeaderInfo = meterDataHeaderInfoEntity;
            }
            catch (Exception)
            {
               // MessageBox.Show("Corrupted Header information available in file.", "E-250 BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
              //  if(isDataCorrupt == true)
                  //  MessageBox.Show("Corrupted Header information available in file.", "E-250 BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }//
        }
    }
}
