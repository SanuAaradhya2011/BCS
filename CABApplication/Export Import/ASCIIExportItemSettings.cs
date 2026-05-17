using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CAB.UI.Controls;
using CAB.Framework;
using CAB.BLL;
using CAB.Framework.Utility;

namespace CAB.UI
{
    public partial class ASCIIExportItemSettings : MdiChildForm
    {
        
        public SettingTypes ParameterType { get; set; }
        private ASCIIExportSettingsBLL settings = new ASCIIExportSettingsBLL();
        public string SelectedColumn { get; set; }
        public string SelectedDBColumn { get; set; }
        String[] data = null;
        public ASCIIExportItemSettings()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int idx = lstAvailable.SelectedIndex;
            foreach (object item in lstAvailable.SelectedItems)
                lstSelected.Items.Add(item);
            for (int counter = 0; counter < lstSelected.Items.Count; counter++)
            {
                int i = 0;
                bool Flag = false;
                for (; i < lstAvailable.Items.Count; i++)
                {
                    if (lstAvailable.Items[i].ToString().Equals(lstSelected.Items[counter].ToString()))
                    {
                        Flag = true;
                        break;
                    }
                }
                if (Flag)
                    lstAvailable.Items.RemoveAt(i);
            }
            if (idx != 0)
                idx -= 1;
            if (lstAvailable.Items.Count >= idx && lstAvailable.Items.Count != 0 && idx > 0)
                lstAvailable.SelectedIndex = idx;
            else
            {
                if(lstAvailable.Items.Count>0)
                    lstAvailable.SelectedIndex = 0;
            }
        }

        private void ASCIIExportItemSettings_Load(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
   
            if (ParameterType.Equals(SettingTypes.Billing))
            {
                this.Text = "Billing Parameter";
                data = settings.GetBillingDisplayColumnName();
            }
            else if (ParameterType.Equals(SettingTypes.General))
            {
                this.Text = "General Parameter";
                data = settings.GetGeneralDisplayColumnName();
            }
            else if (ParameterType.Equals(SettingTypes.Instant))
            {
                this.Text = "Instant Parameter";
                data = settings.GetInstantDisplayColumnName();
            }
            else if (ParameterType.Equals(SettingTypes.Tamper))
            {
                this.Text = "Tamper Parameter";
                data = settings.GetTamperDisplayColumnName();
            }
            else if (ParameterType.Equals(SettingTypes.LoadSurvey))
            {
                this.Text = "Load Survey Parameter";
                data = settings.GetLoadSurveyDisplayColumnName();
            } 
            //added for MVVNL
            else if (ParameterType.Equals(SettingTypes.MidnightEnergies))
            {
                this.Text = "Midnight Energies";
                data = settings.GetMidnightEnergiesColumnName();
            }
            else if (ParameterType.Equals(SettingTypes.SelfDiagnosis))
            {
                this.Text = "Self Diagnostics";
                data = settings.GetSelfDiagnosisDisplayColumnName();
            }
            //added for MVVNL
            else
                this.Text = "Parameter Selection form";
            lstAvailable.Items.Clear();
            lstSelected.Items.Clear();

            if (ParameterType.Equals(SettingTypes.Tamper))
            {

                string[] dbData = settings.GetTamperDBColumnName();
                string dbDataValue = string.Empty;
                bool selected;
                if (!string.IsNullOrEmpty(SelectedColumn))
                {
                    string[] selectedData = SelectedColumn.Split(',');
                    selectedData = selectedData.Reverse().ToArray();
                    for (int dbCounter = 0; dbCounter <dbData.Length ; dbCounter++)                   
                    {
                        
                        selected = false;
                        dbDataValue = dbData[dbCounter].Substring(0, dbData[dbCounter].Length - 1);
                   
                        for (int counter = 0; counter < selectedData.Length; counter++)
                        {
                            string dbValue = selectedData[counter];
                                                      
                            if (dbDataValue.Equals(dbValue))
                            {
                                lstSelected.Items.Add(data[dbCounter]);
                                selected=true;
                                continue;
                            }
                         }
                        if (!selected)
                            lstAvailable.Items.Add(data[dbCounter]);
                     
                    }
                  
                }
                else
                {
                    if (data != null)
                    {
                        for (int counter = 0; counter < data.Length; counter++)
                            lstAvailable.Items.Add(data[counter].Trim());
                    }
                }

            }
            else
            {
                if (data != null)
                {
                    for (int counter = 0; counter < data.Length; counter++)
                        lstAvailable.Items.Add(data[counter].Trim());
                }
                if (!string.IsNullOrEmpty(SelectedColumn))
                {
                    string[] colData = SelectedColumn.Split(',');
                    for (int i = colData.Length - 1; i >= 0; i--)
                    {
                        string val = colData[i].Trim();
                        if (!string.IsNullOrEmpty(val))//added to avoid spaces in selected items list box
                        {
                            lstSelected.Items.Add(val);
                        }
                        for (int j = 0; j < lstAvailable.Items.Count; j++)
                        {
                            string colval = lstAvailable.Items[j].ToString();
                            if (colval == val)
                            {
                                lstAvailable.Items.RemoveAt(j);
                                break;
                            }
                        }
                    }
                }
            }
            if(lstAvailable.Items.Count> 0)
                lstAvailable.SelectedIndex = 0;
            if (lstSelected.Items.Count >  0)
                lstSelected.SelectedIndex = 0;
        }
        private string IndexOf(object item)
        {
            for (int counter = 0; counter < data.Length - 1; counter++)
            {
                if(data[counter].ToString().Trim().Equals(item.ToString().Trim()))
                {
                    return counter.ToString();
                }
            }
            return string.Empty;
         
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            string paramName = String.Empty;
            string paramColumnName = String.Empty;
            
            if (ParameterType.Equals(SettingTypes.Billing))
            {
               
                foreach (object item in lstSelected.Items)
                {
                    paramName = string.Concat(item.ToString() + ",", paramName);
                    paramColumnName = string.Concat(settings.GetDBColumn(item.ToString(), SettingTypes.Billing), paramColumnName);
                }
                if (paramColumnName.Length > 1)
                {
                    paramColumnName = paramColumnName.Substring(0, (paramColumnName.Length - 1));
                    if(ApplicationType.IEC_LTCT_650==ConfigInfo.GetApplicationType())
                        paramColumnName = String.Concat("Select  D.MeterID,", paramColumnName, " from meterdata_billing A,meterdata_tariffinformation B, meterdata_tampercountergeneral C,meterdata D where A.MeterData_ID=B.MeterData_ID and B.MeterData_ID=C.MeterData_ID and A.MeterData_ID = D.MeterData_ID and C.RelatedTo='B' and A.History_ID=0 and A.MeterData_ID=");
                    else if(ApplicationType.DLMS_LTCT_650==ConfigInfo.GetApplicationType())
                        paramColumnName = String.Concat("Select  B.MeterID,", paramColumnName, " from meterdata_billing A ,meterdata B where A.MeterData_ID = B.MeterData_ID and A.MeterData_ID=");

                }
            }
            if (ParameterType.Equals(SettingTypes.General))
            {
                foreach (object item in lstSelected.Items)
                {
                    paramName = string.Concat(item.ToString() + ",", paramName);
                    paramColumnName = string.Concat(settings.GetDBColumn(item.ToString(), SettingTypes.General), paramColumnName);
                }
                if (paramColumnName.Length > 1)
                {
                    paramColumnName = paramColumnName.Substring(0, (paramColumnName.Length - 1));
                    paramColumnName = String.Concat("Select B.MeterID,", paramColumnName, " from meterdata_general A,meterdata B where A.MeterData_ID=B.MeterData_ID and A.MeterData_ID="); 
                }
            }
            if (ParameterType.Equals(SettingTypes.Instant))
            {
                foreach (object item in lstSelected.Items)
                {
                    // Fix by Swati 23/02/2012 for instant data header and data coming in wrong order.
                    paramName = string.Concat(paramName + ",", item.ToString());
                    paramColumnName = string.Concat(settings.GetDBColumn(item.ToString(), SettingTypes.Instant), paramColumnName);
                }
               // paramName = paramName.Substring(1, paramName.Length - 1);
                if (paramColumnName.Length > 1)
                {
                    paramColumnName = paramColumnName.Substring(0, (paramColumnName.Length - 1));
                    if(ApplicationType.IEC_LTCT_650==ConfigInfo.GetApplicationType())
                        paramColumnName = String.Concat("Select B.MeterID,", paramColumnName, " from meterdata_instantpower A,meterdata B where A.MeterData_ID=B.MeterData_ID and A.MeterData_ID=");
                    else if(ApplicationType.DLMS_LTCT_650==ConfigInfo.GetApplicationType())
                        paramColumnName = String.Concat("Select A.InstantPowerColumnValue,B.MeterID from meterdata_instantpower A,meterdata B where A.MeterData_ID = B.MeterData_ID and A.InstantPowerColumnName in (" + paramColumnName +") and A.MeterData_ID=");
                }
            }
            if (ParameterType.Equals(SettingTypes.LoadSurvey))
            {
                foreach (object item in lstSelected.Items)
                {
                    paramName = string.Concat(item.ToString() + ",", paramName);
                    paramColumnName = string.Concat(settings.GetDBColumn(item.ToString(), SettingTypes.LoadSurvey), paramColumnName);
                }
                if (paramColumnName.Length > 1)
                {
                    paramColumnName = paramColumnName.Substring(0, (paramColumnName.Length - 1));
                    paramColumnName = String.Concat("Select B.MeterID,", paramColumnName, " from meterdata_loadsurvey A,meterdata B where A.MeterData_ID = B.MeterData_ID and A.MeterData_ID=");
                }
            }
            if (ParameterType.Equals(SettingTypes.Tamper))
            {
                foreach (object item in lstSelected.Items)
                {
                   
                    paramColumnName = string.Concat(settings.GetDBColumn(item.ToString(), SettingTypes.Tamper), paramColumnName);
                    paramName = paramColumnName;
                }
                if (paramColumnName.Length > 1)
                {
                 
                    paramColumnName = paramColumnName.Substring(0, (paramColumnName.Length - 1));
                    if (ApplicationType.IEC_LTCT_650 == ConfigInfo.GetApplicationType())
                        paramColumnName = String.Concat("Select ", paramColumnName, " from meterdata_tampercountergeneral A,meterdata_tampercounter B,meterdata_tampersnapshot C where A.TamperCounterGeneral_ID=B.TamperCounterGeneral_ID And B.MeterData_ID=C.MeterData_ID and B.MeterData_ID=");
                    else if (ApplicationType.DLMS_LTCT_650 == ConfigInfo.GetApplicationType())
                    {
                        paramColumnName = String.Concat("Select B.MeterID,A.EventCode,A.DateTimeEvent,A.CurrentIR,A.CurrentIY,A.CurrentIB,A.VoltageVRN,A.VoltageVYN,A.VoltageVBN,A.PowerFactorRphase,A.PowerFactorYphase,A.PowerFactorBphase,A.CumulativeEnergykWh,A.CumulativeEnergykVAh from tamper_master A,meterdata B where A.MeterData_ID = B.MeterData_ID and EventCode in(" + paramColumnName + ")" + " and A.MeterData_ID=");
                    }
                }
            }
            //added for MVVNL
            if (ParameterType.Equals(SettingTypes.MidnightEnergies))
            {
                foreach (object item in lstSelected.Items)
                {

                    paramName = string.Concat(paramName, item.ToString() + ",");
                    paramColumnName = string.Concat(paramColumnName, settings.GetDBColumn(item.ToString(), SettingTypes.MidnightEnergies));
                }
                if (paramColumnName.Length > 1)
                {
                    paramName = paramName.Substring(0, (paramName.Length - 1));
                    paramColumnName = paramColumnName.Substring(0, (paramColumnName.Length - 1));
                    if (ApplicationType.DLMS_LTCT_650 == ConfigInfo.GetApplicationType())
                    {
                        paramColumnName = String.Concat("Select B.MeterID,", paramColumnName, " from meterdata_midnightdata A,meterdata B where A.MeterData_ID=B.MeterData_ID and A.MeterData_ID=");
                    }
                }
            }

            if (ParameterType.Equals(SettingTypes.SelfDiagnosis))
            {
                foreach (object item in lstSelected.Items)
                {

                    paramName = string.Concat(paramName, item.ToString() + " ,");
                    paramColumnName = string.Concat(paramColumnName, settings.GetDBColumn(item.ToString(), SettingTypes.SelfDiagnosis));
                }
                if (paramColumnName.Length > 1)
                {
                    paramName = paramName.Substring(0, (paramName.Length - 1));
                    paramColumnName = paramColumnName.Substring(0, (paramColumnName.Length - 1));
                    if (ApplicationType.DLMS_LTCT_650 == ConfigInfo.GetApplicationType())
                    {
                        paramColumnName = String.Concat("Select B.MeterID ,", paramColumnName , " from meterdata_anomaly A,meterdata B where A.MeterDataId=B.MeterData_ID and B.MeterData_ID=");
                    }
                }
            }
            //added for MVVNL

            if (paramColumnName.Length > 1)
              //paramName = paramName.Substring(0, (paramName.Length - 1));
         

            SelectedColumn = paramName;
            SelectedDBColumn = paramColumnName;
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (lstAvailable.Items.Count == 0)
                return;
            foreach (object item in lstAvailable.Items)
                lstSelected.Items.Add(item);
            lstAvailable.Items.Clear(); 
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (object item in lstSelected.SelectedItems)
                lstAvailable.Items.Add(item);
            for (int SelectedIndex = lstSelected.SelectedIndices.Count; SelectedIndex > 0; SelectedIndex--)
                lstSelected.Items.RemoveAt(lstSelected.SelectedIndex);
            if (lstSelected.Items.Count != 0)
            {
                lstSelected.SelectedIndex = 0;
                lstAvailable.SelectedIndex = -1;
            } 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (lstSelected.Items.Count == 0)
                return;
            foreach (object item in lstSelected.Items)
                lstAvailable.Items.Add(item);
            lstSelected.Items.Clear(); 
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            this.Close();
        }
    }
}