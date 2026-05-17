using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using CAB.BLL;
using CAB.Entity;
using CAB.Serialization;
using CAB.UI.Controls;
using CABApplication.Export_Import;

namespace CAB.UI
{
    public partial class ASCIIExportSettings : MdiChildForm
    {
        private ASCIIExportSettingsBLL asciiExportSettingsBLL = new ASCIIExportSettingsBLL();
        private ASCIIExportItemSettings aSCIIExportItemSettings = null;
        private ASCIIExportSettingsEntity entity = null;
        public static AsciiSettings asciiExport = null;
        public static AsciiSettingsProfile asciiExportProfileParameters = null;
        bool isPUMA = false;
        bool isMVVNL = false;
        private static object syncRoot = new object();
        private Serializer serializer = null;

        public ASCIIExportSettings()
        {
            InitializeComponent();

            serializer = new Serializer();
            lock (syncRoot)
            {
                if (asciiExport == null)
                {
                    asciiExport = (AsciiSettings)serializer.DeserializeToObject(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "AsciiExportParameters.xml"), typeof(AsciiSettings));
                }
            }


        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            btnEdit.Enabled = true;
            btnDelete.Enabled = true;
            btnAdd.Enabled = true;
            lstFile.Enabled = true;
            this.StatusMessage = string.Empty;
            gbFormate.Text = "View export settings";
            btnSave.Visible = btnCancel.Visible = gbFormate.Enabled = false;
        }

        private void ASCIIExportSettings_Load(object sender, EventArgs e)
        {
            string columnName = "";
            //added for MVVNL
            //string utility = string.Empty;
            //utility = ConfigSettings.GetValue("Utility").ToUpper();
            //utility check changed on 16th May as per the utility check added to the solution
            // Check utility
            if (CAB.Framework.UtilityEntity.Generic == UtilityDetails.Utility)
            {
                isPUMA = true;
            }
            else if (CAB.Framework.UtilityEntity.MVVNL == UtilityDetails.Utility)
            {
                isMVVNL = true;
            }
            else
            {
                isMVVNL = false;
                isPUMA = false;
            }
            //VBM - Remove isPuma Check : UtilityDetails.ShowMidnight will do the needfull. 
            this.Text = "ASCII Export Settings";
            this.StatusMessage = string.Empty;
            DataSet dataSet = asciiExportSettingsBLL.ListDataSet();
            lstFile.DataSource = dataSet.Tables[0];
            lstFile.DisplayMember = "FileName";
            lstFile.ValueMember = "Asciiexportsettings_ID";

            if (lstFile.Items.Count == 0)
            {
                btnDelete.Enabled = false;
                btnEdit.Enabled = false;
            }

            addAsciiExportProfileTreeView();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int Asciiexportsettings_ID = 0;
            btnEdit.Enabled = true;
            btnDelete.Enabled = true;
            btnAdd.Enabled = true;
            lstFile.Enabled = true;
            string fileName = txtFileName.Text.Trim();
            if (string.IsNullOrEmpty(fileName))
            {
                this.StatusMessage = "Format name cannot be left blank.";
                txtFileName.Focus();
                return;
            }
            string delimeter = cmbDataSeparator.Text;
            if (string.IsNullOrEmpty(delimeter))
            {
                this.StatusMessage = "Please select delimeter.";
                cmbDataSeparator.Focus();
                return;
            }
            Asciiexportsettings_ID = entity.Asciiexportsettings_ID;
            if (asciiExportSettingsBLL.IsValidFile(fileName) && entity.Asciiexportsettings_ID == 0)
            {
                this.StatusMessage = "File already exist.";
                txtFileName.Focus();
                return;
            }

            if (entity.Asciiexportsettings_ID == 0)
            {
                entity.FileName = fileName;
                entity.Delimeter = delimeter;
                List<ConfigAsciiParameter> configAsciiParameterList = GetSelectedParameter();
                UpdateEntity(configAsciiParameterList);

                if (string.IsNullOrEmpty(entity.GeneralColumn) && string.IsNullOrEmpty(entity.BillingColumn) && string.IsNullOrEmpty(entity.InstantColumn) && string.IsNullOrEmpty(entity.TamperColumn) && string.IsNullOrEmpty(entity.LoadSurveyColumn) && string.IsNullOrEmpty(entity.MidnightEnergiesColumn) && string.IsNullOrEmpty(entity.SelfDiagnosisColumn))
                {
                    this.StatusMessage = "Please select Parameter";
                    return;
                }

                asciiExportSettingsBLL.InsertData(entity);
                this.StatusMessage = "File saved successfully.";
            }
            else
            {
                // EDIT :new entity object is created ,bcoz lstfile_selectionchanged() function doesn't is not called. 
                // in new entity i have taken values  
                entity = new ASCIIExportSettingsEntity();
                entity.FileName = fileName;
                entity.Delimeter = delimeter;
                entity.Asciiexportsettings_ID = Asciiexportsettings_ID;
                List<ConfigAsciiParameter> configAsciiParameterList = GetSelectedParameter();
                UpdateEntity(configAsciiParameterList);
                if (string.IsNullOrEmpty(entity.GeneralColumn) && string.IsNullOrEmpty(entity.BillingColumn) && string.IsNullOrEmpty(entity.InstantColumn) && string.IsNullOrEmpty(entity.TamperColumn) && string.IsNullOrEmpty(entity.LoadSurveyColumn) && string.IsNullOrEmpty(entity.MidnightEnergiesColumn) && string.IsNullOrEmpty(entity.SelfDiagnosisColumn))
                {
                    this.StatusMessage = "Please select Parameter";
                    return;
                }

                string fileId = ((System.Data.DataRowView)(lstFile.Items[lstFile.SelectedIndex])).Row.ItemArray[0].ToString();
                foreach (TreeNode rootNode in treeViewAsciiExport.Nodes)
                {
                    rootNode.Collapse();
                    rootNode.Expand();
                }
                if (entity != null)
                {
                    selectedFileTreeState(entity);
                    txtFileName.Text = entity.FileName;
                    for (int i = 0; i < cmbDataSeparator.Items.Count; i++)
                    {
                        cmbDataSeparator.SelectedIndex = i;
                        if (cmbDataSeparator.Text.Trim().Equals(entity.Delimeter.Trim()))
                            break;
                    }
                }

                asciiExportSettingsBLL.UpdateData(entity);
                this.StatusMessage = "File updated successfully.";
            }


            LoadList();
            gbFormate.Text = "View export settings";
            btnSave.Visible = btnCancel.Visible = gbFormate.Enabled = false;
        }

        /// <summary>
        /// updates the entity by adding query to entity parameters.
        /// </summary>
        /// <param name="configAsciiParameterList"></param>
        void UpdateEntity(List<ConfigAsciiParameter> configAsciiParameterList)
        {
            string tableName = string.Empty;
            string parameterQuery = string.Empty;
            string columnQuery = string.Empty;
            string selectedColumn = string.Empty;
            string selectedDBColumn = string.Empty;
            List<string> profileParameter = new List<string>();
            foreach (ConfigAsciiParameter parameter in configAsciiParameterList)
            {
                LoadParameters(parameter.ProfileName, parameter);
            }
        }

        /// <summary>
        /// this function has switch case to choose which profiles data must go in which parameter of entity.
        /// </summary>
        /// <param name="profileName"></param>
        /// <param name="parameter"></param>
        void LoadParameters(string profileName, ConfigAsciiParameter parameter)
        {

            string parameterQuery = string.Empty;
            string columnQuery = string.Empty;
            if (parameter.ParameterName.Count != 0 && parameter.DBname.Count != 0)
            {


                if (profileName == "Instant")
                {
                    foreach (string param in parameter.ParameterName)
                    {
                        parameterQuery = parameterQuery + "," + param;
                    }
                }
                else if (profileName == "Tamper")
                {
                    parameterQuery = "704,703,158,251,208,207,206,205,204,203,202,201,155,154,153,152,151,102,101,70,69,68,67,66,65,64,63,62,61,60,59,58,57,56,55,54,53,52,51,12,11,10,9,8,7,6,5,4,3,2,1,"; //SarkarA code change start 20180308 // add Current Mismatch Tamper/end
                }
                else
                    {
                        foreach (string param in parameter.ParameterName)
                        {
                            parameterQuery = param + "," + parameterQuery;
                        }
                        parameterQuery = parameterQuery.Substring(0, (parameterQuery.Length - 1));
                    }
                
                if (profileName != "Instant" && profileName !="Tamper")
                {
                    foreach (string dBName in parameter.DBname)
                    {
                        columnQuery = "A." + dBName + "," + columnQuery;
                    }
                }
                else if (profileName == "Instant")
                {
                    foreach (string dBName in parameter.DBname)
                    {
                        columnQuery = "'" + dBName + "'," + columnQuery;
                    }
                }
                else if (profileName == "Tamper")
                {
                    foreach (string dBName in parameter.DBname)
                    {
                        columnQuery =  dBName + "," + columnQuery;
                    }
                }

                columnQuery = columnQuery.Substring(0, (columnQuery.Length - 1));
                if (profileName != "Self Diagnostics" && profileName != "Instant" && profileName != "Tamper")
                {
                    columnQuery = String.Concat("Select B.MeterID,", columnQuery, " from ", parameter.TableName, " A,meterdata B where A.MeterData_ID=B.MeterData_ID and A.MeterData_ID=");
                }
                else if (profileName == "Self Diagnostics")
                {
                    columnQuery = String.Concat("Select B.MeterID,", columnQuery, " from ", parameter.TableName, " A,meterdata B where A.MeterDataId=B.MeterData_ID and A.MeterDataId=");
                }
                else if (profileName == "Instant")
                {
                    columnQuery = String.Concat("Select A.InstantPowerColumnValue,B.MeterID from ", parameter.TableName ," A,meterdata B where A.MeterData_ID = B.MeterData_ID and A.InstantPowerColumnName in (" , columnQuery ,") and A.MeterData_ID=");
                }
                else if (profileName == "Tamper")
                {
                    parameterQuery = parameterQuery.Substring(0, (parameterQuery.Length - 1));
                    columnQuery = String.Concat("Select B.MeterID,A.EventCode,A.DateTimeEvent,A.CurrentIR,A.CurrentIY,A.CurrentIB,A.PhaseCurrent,A.VoltageVRN,A.VoltageVYN,A.VoltageVBN,A.PhaseVoltage,A.PowerFactorRphase,A.PowerFactorYphase,A.PowerFactorBphase,A.CumulativeEnergykWh,A.CumulativeEnergykVAh,A.TotalPowerFactor from tamper_master A,meterdata B where A.MeterData_ID = B.MeterData_ID and EventCode in(" + parameterQuery + ")" + " and A.MeterData_ID=");
                }
                switch (parameter.ProfileName)
                {
                    case "General":
                        entity.GeneralColumn = parameterQuery;
                        entity.GeneralDBColumn = columnQuery;
                        break;
                    case "Billing":
                        entity.BillingColumn = parameterQuery;
                        entity.BillingDBColumn = columnQuery;
                        break;
                    case "Instant":
                        entity.InstantColumn = parameterQuery;
                        entity.InstantDBColum = columnQuery;
                        break;
                    case "Tamper":
                        entity.TamperColumn = parameterQuery;
                        entity.TamberDBColumn = columnQuery; 
                        break;
                    case "Load Survey":
                        entity.LoadSurveyColumn = parameterQuery;
                        entity.LoadSurveyDBColumn = columnQuery;
                        break;

                    case "Midnight Energies":
                        entity.MidnightEnergiesColumn = parameterQuery;
                        entity.MidnightEnergiesDBColumn = columnQuery;
                        break;
                    case "Self Diagnostics":
                        entity.SelfDiagnosisColumn = parameterQuery;
                        entity.SelfDiagnosisDBColumn = columnQuery;
                        break;
                }
            }
            else
            {
                switch (parameter.ProfileName)
                {
                    case "General":
                        entity.GeneralColumn = null;
                        entity.GeneralDBColumn = null;
                        break;
                    case "Billing":
                        entity.BillingColumn = null;
                        entity.BillingDBColumn = null;
                        break;
                    case "Load Survey":
                        entity.LoadSurveyColumn = null;
                        entity.LoadSurveyDBColumn = null;
                        break;
                    case "Midnight Energies":
                        entity.MidnightEnergiesColumn = null;
                        entity.MidnightEnergiesDBColumn = null;
                        break;
                    case "Self Diagnostics":
                        entity.SelfDiagnosisColumn = null;
                        entity.SelfDiagnosisDBColumn = null;
                        break;
                }

            }
        }

        private void LoadList()
        {
            DataSet dataSet = asciiExportSettingsBLL.ListDataSet();
            lstFile.DataSource = dataSet.Tables[0];
            lstFile.DisplayMember = "FileName";
            lstFile.ValueMember = "Asciiexportsettings_ID";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            entity = new ASCIIExportSettingsEntity();
            gbFormate.Text = "New export settings";
            SetTreeViewCheckBoxSelection(true);
            gbFormate.Enabled = true;
            btnSave.Visible = btnCancel.Visible = true;
            btnAdd.Enabled = btnDelete.Enabled = lstFile.Enabled = btnEdit.Enabled = false;
            txtFileName.Text = "";
            this.StatusMessage = string.Empty;
            cmbDataSeparator.SelectedIndex = 0;
            txtFileName.Enabled = true;
            txtFileName.Focus();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            this.Close();
        }

        private void lstFile_SelectedIndexChanged(object sender, EventArgs e)
        {
            entity = new ASCIIExportSettingsEntity();
            string fileId = ((System.Data.DataRowView)(lstFile.Items[lstFile.SelectedIndex])).Row.ItemArray[0].ToString();
            entity = asciiExportSettingsBLL.DetailData(fileId) as ASCIIExportSettingsEntity;
            foreach (TreeNode rootNode in treeViewAsciiExport.Nodes)
            {
                rootNode.Collapse();
                rootNode.Expand();
            }
            if (entity != null)
            {
                selectedFileTreeState(entity);
                txtFileName.Text = entity.FileName;
                for (int i = 0; i < cmbDataSeparator.Items.Count; i++)
                {
                    cmbDataSeparator.SelectedIndex = i;
                    if (cmbDataSeparator.Text.Trim().Equals(entity.Delimeter.Trim()))
                        break;
                }
            }
            gbFormate.Text = "View export settings";
            btnSave.Visible = btnCancel.Visible = gbFormate.Enabled = false;
        }

        /// <summary>
        /// to save tree state for each selected file.
        /// </summary>
        /// <param name="settingEntity"></param>
        private void selectedFileTreeState(ASCIIExportSettingsEntity settingEntity)
        {
            SetTreeViewCheckBoxSelection(false);

            foreach (TreeNode node in treeViewAsciiExport.Nodes)
            {
                foreach (TreeNode profileNodes in node.Nodes)
                {
                    if (profileNodes.Text == "General" && !string.IsNullOrEmpty(settingEntity.GeneralColumn))
                    {
                        profileNodes.Checked = true;
                        getCheckedParameters(settingEntity.GeneralColumn, profileNodes);
                    }

                    if (profileNodes.Text == "Instant" && !string.IsNullOrEmpty(settingEntity.InstantColumn))
                    {
                        profileNodes.Checked = true;
                        getCheckedParameters(settingEntity.InstantColumn, profileNodes);
                    }

                    if (profileNodes.Text == "Billing" && !string.IsNullOrEmpty(settingEntity.BillingColumn))
                    {
                        profileNodes.Checked = true;
                        getCheckedParameters(settingEntity.BillingColumn, profileNodes);
                    }

                    if (profileNodes.Text == "Load Survey" && !string.IsNullOrEmpty(settingEntity.LoadSurveyColumn))
                    {
                        profileNodes.Checked = true;
                        getCheckedParameters(settingEntity.LoadSurveyColumn, profileNodes);
                    }

                    if (profileNodes.Text == "Tamper" && !string.IsNullOrEmpty(settingEntity.TamperColumn))
                    {
                        profileNodes.Checked = true;
                        getCheckedParameters(settingEntity.TamperColumn, profileNodes);
                    }

                    if (profileNodes.Text == "Midnight Energies" && !string.IsNullOrEmpty(settingEntity.MidnightEnergiesColumn))
                    {
                        profileNodes.Checked = true;
                        getCheckedParameters(settingEntity.MidnightEnergiesColumn, profileNodes);
                    }

                    if (profileNodes.Text == "Self Diagnostics" && !string.IsNullOrEmpty(settingEntity.SelfDiagnosisColumn))
                    {
                        profileNodes.Checked = true;
                        getCheckedParameters(settingEntity.SelfDiagnosisColumn, profileNodes);
                    }
                }
            }
        }

        /// <summary>
        /// get the checked parameters for a given profile.
        /// </summary>
        /// <param name="columnDescription"></param>
        /// <param name="profileNode"></param>
        private void getCheckedParameters(string columnDescription, TreeNode profileNode)
        {
            string[] columnDescriptionList;
            columnDescriptionList = columnDescription.Split(',');
            foreach (TreeNode parameterNodes in profileNode.Nodes)
            {
                foreach (string columnDesc in columnDescriptionList)
                {
                    if (columnDesc == parameterNodes.Text)
                    {
                        parameterNodes.Checked = true;
                    }
                }
            }
        }

        /// <summary>
        /// select all  or delect all nodes of tree view.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetTreeViewCheckBoxSelection(bool selected)
        {
            foreach (TreeNode node in treeViewAsciiExport.Nodes)
            {
                foreach (TreeNode profileNodes in node.Nodes)
                {
                    profileNodes.Checked = selected;
                    foreach (TreeNode parameterNodes in profileNodes.Nodes)
                    {
                        parameterNodes.Checked = selected;
                    }
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            btnEdit.Enabled = true;
            btnAdd.Enabled = true;

            this.StatusMessage = string.Empty;
            if (lstFile.SelectedIndex != -1)
            {
                if (CABMessageBox.ShowFilterMessage("M000102", "A000001", MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.No))
                    return;
                //if (MessageBox.Show("Are you sure to delete this file", "Delete Customer Settings", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                //    return;
                int index = lstFile.SelectedIndex;

                string fileId = ((System.Data.DataRowView)(lstFile.Items[index])).Row.ItemArray[0].ToString();
                asciiExportSettingsBLL.DeleteSettings(Convert.ToInt32(fileId));
                this.StatusMessage = "File deleted successfully.";
                if (index != 0)
                    index--;
                LoadList();
                if (lstFile.Items.Count > 0)
                    lstFile.SelectedIndex = index;
            }
            if (lstFile.Items.Count == 0)
            {
                btnDelete.Enabled = false;
                btnEdit.Enabled = false;
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (lstFile.Items.Count == 0)
                return;
            string fileId = ((System.Data.DataRowView)(lstFile.Items[lstFile.SelectedIndex])).Row.ItemArray[0].ToString();
            entity = asciiExportSettingsBLL.DetailData(fileId) as ASCIIExportSettingsEntity;
            if (entity != null)
            {
                btnAdd.Enabled = btnDelete.Enabled = lstFile.Enabled = btnEdit.Enabled = false;
                txtFileName.Text = entity.FileName;
            }
            this.StatusMessage = string.Empty;
            gbFormate.Text = "Edit export settings";
            gbFormate.Enabled = true;
            btnSave.Visible = btnCancel.Visible = true;
            txtFileName.Enabled = false;
        }

        /// <summary>
        /// adds a tree view on load of the page.
        /// </summary>
        private void addAsciiExportProfileTreeView()
        {
            TreeNode rootNode = new TreeNode();
            rootNode.Text = "ASCII Settings";
            rootNode.Checked = true;
            treeViewAsciiExport.Nodes.Add(rootNode);

            foreach (AsciiSettingsProfile asciiExportProfile in asciiExport.Items)
            {
                if (asciiExportProfile.Visible == "true")
                {
                    TreeNode profileNode = new TreeNode();
                    profileNode.Text = asciiExportProfile.ProfileName;
                    profileNode.Checked = true;
                    rootNode.Nodes.Add(profileNode);

                    foreach (AsciiSettingsProfileProfileParameter obj in asciiExportProfile.ProfileParameter)
                    {
                        if (obj.Visible == "true")
                        {
                            TreeNode parameterNode = new TreeNode();
                            parameterNode.Text = obj.Description;
                            parameterNode.Checked = true;
                            profileNode.Nodes.Add(parameterNode);
                        }
                    }
                }
            }
            rootNode.Expand();
        }

        /// <summary>
        /// gets table name and column name by sending profile name and description respectively.
        /// </summary>
        /// <param name="asciiParameter"></param>
        /// <returns></returns>
        private ConfigAsciiParameter GetTableNameAndColumnName(ConfigAsciiParameter asciiParameter)
        {
            ConfigAsciiParameter configAsciiParameter = new ConfigAsciiParameter();
            AsciiSettingsProfile result = null;
            string asciiExportTableName = "";
            List<string> parameterColumnName = new List<string>();
            Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();
            string queryForAsciiExport = "";
            int countParameterList = -1;
            foreach (AsciiSettingsProfile asciiExportProfile in asciiExport.Items)
            {
                if (asciiExportProfile.ProfileName == asciiParameter.ProfileName)
                {
                    asciiExportTableName = asciiExportProfile.TableName;
                    foreach (AsciiSettingsProfileProfileParameter obj in asciiExportProfile.ProfileParameter)
                    {
                        foreach (string parameter in asciiParameter.ParameterName)
                        {
                            if (obj.Description == parameter)
                            {
                                parameterColumnName.Add(obj.ColumnName);
                            }
                        }
                    }
                }
            }
            configAsciiParameter = asciiParameter;
            configAsciiParameter.TableName = asciiExportTableName;
            configAsciiParameter.DBname = parameterColumnName;
            return configAsciiParameter;
        }

        /// <summary>
        /// fetches the selected parameters and profiles of the tree view.
        /// </summary>
        private List<ConfigAsciiParameter> GetSelectedParameter()
        {
            List<ConfigAsciiParameter> configAsciiParameterList = new List<ConfigAsciiParameter>();
            ConfigAsciiParameter asciiParameter = null;
            Dictionary<string, List<string>> dictionaryAsciiTableDetails = new Dictionary<string, List<string>>();

            foreach (TreeNode node in treeViewAsciiExport.Nodes)
            {
                foreach (TreeNode profileNodes in node.Nodes)
                {
                    if (profileNodes.Checked)
                    {
                        asciiParameter = new ConfigAsciiParameter();
                        asciiParameter.ParameterName = new List<string>();
                        asciiParameter.DBname = new List<string>();

                        foreach (TreeNode parameterNodes in profileNodes.Nodes)
                        {

                            if (parameterNodes.Checked == true)
                            {
                                asciiParameter.ParameterName.Add(parameterNodes.Text);
                            }
                        }
                        asciiParameter.ProfileName = profileNodes.Text;
                        configAsciiParameterList.Add(GetTableNameAndColumnName(asciiParameter));
                    }
                }
            }
            return configAsciiParameterList;
        }

        /// <summary>
        /// to select all and deselect all check boxes of tree view.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeViewAsciiExport_AfterCheck(object sender, TreeViewEventArgs e)
        {
            int flag = 0;
            if (e.Action != TreeViewAction.Unknown)
            {
                if (e.Node.Nodes.Count > 0)
                {
                    this.CheckAllChildNodes(e.Node, e.Node.Checked);
                }
                if (e.Node.Nodes.Count == 0)
                {
                    this.UncheckParentNodes(e.Node.Parent, e.Node.Checked);
                }
                foreach (TreeNode rootNode in treeViewAsciiExport.Nodes)
                {
                    foreach (TreeNode profileNode in rootNode.Nodes)
                    {
                        if (profileNode.Checked == true)
                        {
                            flag = 1;
                            break;
                        }
                    }
                }
                if (flag == 1)
                {
                    foreach (TreeNode rootNode in treeViewAsciiExport.Nodes)
                    {
                        rootNode.Checked = true;
                    }
                }
                else
                {
                    foreach (TreeNode rootNode in treeViewAsciiExport.Nodes)
                    {
                        rootNode.Checked = false;
                    }
                }
            }
        }

        /// <summary>
        /// this function runs recusively , and checks all child and parent nodes.
        /// </summary>
        /// <param name="treeNode"></param>
        /// <param name="nodeChecked"></param>
        private void CheckAllChildNodes(TreeNode treeNode, bool nodeChecked)
        {
            foreach (TreeNode node in treeNode.Nodes)
            {
                node.Checked = nodeChecked;
                if (node.Nodes.Count > 0)
                {
                    this.CheckAllChildNodes(node, nodeChecked);
                }
            }
        }

        /// <summary>
        /// this function runs recusively , and unchecks all child and parent nodes.
        /// </summary>
        /// <param name="treeNode"></param>
        /// <param name="nodeChecked"></param>
        private void UncheckParentNodes(TreeNode treeNode, bool nodeChecked)
        {
            int flag = 0;
            foreach (TreeNode node in treeNode.Nodes)
            {
                if (node.Checked == true)
                {
                    flag = 1;
                    break;
                }
                else
                {
                    flag = 0;
                }
            }
            if (flag == 1)
            {
                treeNode.Checked = true;
            }
            else
            {
                treeNode.Checked = false;
            }
        }
    }
}
