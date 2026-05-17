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
    public class TamperTypeBLL : IBLL
    {
       private TamperTypeMasterDAL tamperTypeMasterDAL;
       private int  kVAhSelectionTamperId = 158;
       private const int VoltagePhaseReversalOccurenceId = 13;
       private const int VoltagePhaseReversalRestorationId = 14;
       private const int MDReset = 159;
       private const int PushModeConfig = 160;
       private const int ScrollModeConfig = 161;
       private const int HRModeConfig = 162;
       private const int ScrollTimeConfig = 163;

       private const int DemandIntegrationPeriod = 152;
       private const int ProfileCapturePeriod = 153;
       private const int SingleActionScheduleForBillingDates = 154;

        public TamperTypeBLL()
        {
            tamperTypeMasterDAL = new TamperTypeMasterDAL();
        }
        public DataSet ExistOrInsert(int compartment)
        {
            bool Flag = true;
            DataSet dataSet = null;
            //BhardwajG : If ct/pt is disabled call overloaded function for CT/PT specific data
            if (UtilityDetails.DisableProgrammingCTPTRatio)
            {
                dataSet = tamperTypeMasterDAL.ListDataSet(compartment,true);
            }            
            else
            {
                dataSet = tamperTypeMasterDAL.ListDataSet(compartment);
            }
            if (dataSet != null)
            {
                if (dataSet.Tables.Count > 0)
                {
                    if (dataSet.Tables[0].Rows.Count > 0)
                        Flag = true;
                    else
                        Flag = false;
                }
                else
                    Flag = false;
            }
            if (!Flag)
            {
                tamperTypeMasterDAL.InsertDefaultTamper();
                //BhardwajG : Do not call the same function everytime, call only when default data was not inserted 
                //            on application initialization
                if (UtilityDetails.DisableProgrammingCTPTRatio)
                {
                    dataSet = tamperTypeMasterDAL.ListDataSet(compartment, true);
                }
                else
                {
                    dataSet = tamperTypeMasterDAL.ListDataSet(compartment);
                }
            }
            /* VBM - Display kvahSelectionTamper only when utility has this fature */
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                if (!UtilityDetails.ShowkVAhSelectionTamperInTransaction)
                {
                    foreach (DataRow trnsRow in dataSet.Tables[0].Rows)
                    {
                        if (Convert.ToInt32(trnsRow["TamperTypeID"]) == kVAhSelectionTamperId)
                        {
                            dataSet.Tables[0].Rows.Remove(trnsRow);
                            dataSet.AcceptChanges();
                            break;
                        }
                    }
                }
                //if (!UtilityDetails.ShowVoltagePhaseReversalTamper)
                //{
                //    foreach (DataRow trnsRow in dataSet.Tables[0].Rows)
                //    {
                //        if (Convert.ToInt32(trnsRow["TamperTypeID"]) == VoltagePhaseReversalOccurenceId)
                //        {
                //            dataSet.Tables[0].Rows.Remove(trnsRow);
                //            dataSet.AcceptChanges();
                //            break;
                //        }
                //    }
                //    foreach (DataRow trnsRow in dataSet.Tables[0].Rows)
                //    {
                //        if (Convert.ToInt32(trnsRow["TamperTypeID"]) == VoltagePhaseReversalRestorationId)
                //        {
                //            dataSet.Tables[0].Rows.Remove(trnsRow);
                //            dataSet.AcceptChanges();
                //            break;
                //        }
                //    }
                //}
                DataView view = new DataView(dataSet.Tables[0]);
                StringBuilder filterCondition = new StringBuilder();
                /* Voltage Phase sequence Reversal tamper is always available if meter is sending this tamper information
                 we are removing the redundant code which hide Tamper (Voltage Phase Sequence Reversal) ID: 13 | 14 in Generic BCS tool.*/
                //if (!UtilityDetails.ShowVoltagePhaseReversalTamper)
                //{
                //  filterCondition.Append("TamperTypeID =" + VoltagePhaseReversalRestorationId + " OR TamperTypeID =" + VoltagePhaseReversalOccurenceId);
                //}
                if (UtilityDetails.DisableProgrammingBillingDateTime)
                {
                    if (filterCondition.Length > 0)
                    {
                        filterCondition.Append(" OR TamperTypeID =" + SingleActionScheduleForBillingDates);
                    }
                    else
                    {
                        filterCondition.Append("TamperTypeID =" + SingleActionScheduleForBillingDates);
                    }
                }

                if (UtilityDetails.DisableProgrammingSurveyCapturePeriod)
                {
                    if (filterCondition.Length > 0)
                    {
                        filterCondition.Append(" OR TamperTypeID =" + ProfileCapturePeriod);
                    }
                    else
                    {
                        filterCondition.Append("TamperTypeID =" + ProfileCapturePeriod);
                    }
                }
                if (UtilityDetails.DisableProgrammingDemandIntegrationPeriod)
                {
                    if (filterCondition.Length > 0)
                    {
                        filterCondition.Append(" OR TamperTypeID =" + DemandIntegrationPeriod);
                    }
                    else
                    {
                        filterCondition.Append("TamperTypeID =" + DemandIntegrationPeriod);
                    }
                }
                if (!UtilityDetails.ShowMDResetTamper)
                {
                    if (filterCondition.Length > 0)
                    {
                        filterCondition.Append(" OR TamperTypeID =" + MDReset);
                    }
                    else
                    {
                        filterCondition.Append("TamperTypeID =" + MDReset);
                    }
                }
                /* we are removing the redundant code which hide Display Parameter tamper*/ 
                //if (!UtilityDetails.ShowDisplayParemeterTamper)
                //{
                //    if (filterCondition.Length > 0)
                //    {
                //        filterCondition.Append("OR TamperTypeID =" + ScrollTimeConfig + " OR TamperTypeID =" + ScrollModeConfig
                //                          + " OR TamperTypeID =" + PushModeConfig + " OR TamperTypeID =" + HRModeConfig);
                //    }
                //    else
                //    {
                //        filterCondition.Append("TamperTypeID =" + ScrollTimeConfig + " OR TamperTypeID =" + ScrollModeConfig
                //                          + " OR TamperTypeID =" + PushModeConfig + " OR TamperTypeID =" + HRModeConfig);
                //    }

                //}
                if (filterCondition.Length > 0)
                {
                    view.RowFilter = filterCondition.ToString();
                    //delete rows
                    foreach (DataRowView row in view)
                    {
                        row.Delete();
                    }
                    dataSet.AcceptChanges();
                }
                
            }
            /* VBM - Display kvahSelectionTamper only when utility has this fature */
            return dataSet;
        }

        public DataSet ListDataSet(int tamperType)
        {
            DataTable dt = null;
            if (tamperType != 4)
            {

                dt = new CommonBLL().AutoNumberedTable(tamperTypeMasterDAL.ListDataSet(3).Tables[0]);
                 dt.Rows[dt.Rows.Count - 1].Delete();
            }
            else
            {
                //BhardwajG : If CT/PT ratio is disabled then call overloaded function which takes care of CT/PT ratio
                if (UtilityDetails.DisableProgrammingCTPTRatio)
                {
                    dt = new CommonBLL().AutoNumberedTable(tamperTypeMasterDAL.ListDataSet(4,true).Tables[0]);
                }
                else
                {
                    dt = new CommonBLL().AutoNumberedTable(tamperTypeMasterDAL.ListDataSet(4).Tables[0]);
                }
            }
            
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            return ds;
        }
    }
}
