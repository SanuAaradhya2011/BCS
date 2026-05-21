using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CAB.UI.Controls;
using CAB.IECFramework;
using CAB.BLL;

namespace CAB.UI
{
    public partial class ASCIIExportItemSettings : MdiChildForm
    {
        public SettingTypes ParameterType { get; set; }
        private ASCIIExportSettingsBLL settings = new ASCIIExportSettingsBLL();
        public string SelectedColumn { get; set; }
        public string SelectedDBColumn { get; set; }
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
                if (lstAvailable.Items.Count > 0)
                    lstAvailable.SelectedIndex = 0;
            }
        }

        private void ASCIIExportItemSettings_Load(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            String[] data = null;
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
            else
                this.Text = "Parameter Selection form";
            lstAvailable.Items.Clear();
            lstSelected.Items.Clear();
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
                    lstSelected.Items.Add(val);
                    for (int j = 0; j < lstAvailable.Items.Count; j++)
                    {
                        if (lstAvailable.Items[j].ToString().Equals(val))
                        {
                            lstAvailable.Items.RemoveAt(j);
                            break;
                        }
                    }
                }
            }
            if (lstAvailable.Items.Count > 0)
                lstAvailable.SelectedIndex = 0;
            if (lstSelected.Items.Count > 0)
                lstSelected.SelectedIndex = 0;
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
                    paramColumnName = String.Concat("Select D.MeterID,", paramColumnName, " from meterdata_billing A,meterdata_tariffinformation B, meterdata_tampercountergeneral C,meterdata D where A.MeterData_ID= D.MeterData_ID and A.MeterData_ID=B.MeterData_ID and A.History_ID = B.HistoryID and B.HistoryID = C.History_ID and B.MeterData_ID=C.MeterData_ID and C.RelatedTo='B' and A.MeterData_ID=");
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
                    paramColumnName = String.Concat("Select B.MeterID,", paramColumnName, " from meterdata_general A,meterdata B,meterdata_headerinfo H where A.MeterData_ID=B.MeterData_ID and B.MeterData_ID = H.MeterData_ID and A.MeterData_ID=");//,meterdata_tariffinformation B, meterdata_tampercountergeneral C where A.MeterData_ID=B.MeterData_ID and B.MeterData_ID=C.MeterData_ID and C.RelatedTo='G'");
                }
            }
            if (ParameterType.Equals(SettingTypes.Instant))
            {
                foreach (object item in lstSelected.Items)
                {
                    paramName = string.Concat(item.ToString() + ",", paramName);
                    paramColumnName = string.Concat(settings.GetDBColumn(item.ToString(), SettingTypes.Instant), paramColumnName);
                }
                if (paramColumnName.Length > 1)
                {
                    paramColumnName = paramColumnName.Substring(0, (paramColumnName.Length - 1));
                    paramColumnName = String.Concat("Select B.MeterID,", paramColumnName, " from meterdata_instantpower A,meterdata B where A.MeterData_ID = B.MeterData_ID and A.MeterData_ID=");
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
                    paramColumnName = String.Concat("Select B.MeterID,", paramColumnName, " from meterdata_loadsurvey A,meterdata B where A.MeterData_ID = B.MeterData_ID and  A.MeterData_ID=");
                }
            }
            if (ParameterType.Equals(SettingTypes.Tamper))
            {
                foreach (object item in lstSelected.Items)
                {
                    paramName = string.Concat(item.ToString() + ",", paramName);
                    paramColumnName = string.Concat(settings.GetDBColumn(item.ToString(), SettingTypes.Tamper), paramColumnName);
                }
                if (paramColumnName.Length > 1)
                {
                    paramColumnName = paramColumnName.Substring(0, (paramColumnName.Length - 1));
                    paramColumnName = String.Concat("Select distinct D.MeterID,", paramColumnName, " from meterdata_tampercountergeneral A,meterdata_tampercounter B,meterdata_tampersnapshot C,meterdata D where B.MeterData_ID = D.MeterData_ID and A.TamperCounterGeneral_ID=B.TamperCounterGeneral_ID And B.MeterData_ID=C.MeterData_ID and B.MeterData_ID=");
                }
            }
            if (paramColumnName.Length > 1)
                paramName = paramName.Substring(0, (paramName.Length - 1));
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