#region NameSpaces
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;
using CAB.BLL;
using CAB.Framework.Utility;

#endregion

namespace CAB.UI.Controls
{
    /// <summary>
    /// Class containing User Control functionality for meter data readout options
    /// </summary>
    public partial class CABGridViewReadControl : UserControl
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        private int enumLength;
        #endregion

        #region Properties
        #endregion

        #region Constructor
        public CABGridViewReadControl()
        {
            InitializeComponent();
            this.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
        }
        #endregion

        #region Public Methods          
        
        /// adds the enum list to the Data Grid View.
        /// </summary>
        /// <param name="enumList">enum list must be passed as "new exampleEnum() "</param>
        /// <param name="IsOffline"></param>
        public void AddEnumList(List <Enum> enumList, bool IsOffline)
        {            
            int column = 0;
            Enum RemoveVal = null;
            enumLength = enumList.Count;
            foreach (var valEnum in enumList)
            {
                if (CommonBLL.GetEnumDescription((Enum)valEnum) != null)
                {
                    if (Convert.ToString(valEnum).Contains("MagneticTamperIcon") && ConfigInfo.RightID[8] == '0')
                    {
                        //Check MagnetTamperIcon for remove if Advance Right not given
                        RemoveVal = valEnum;
                        continue;
                    }
                    dataGridEnum.Rows.Add();
                    dataGridEnum[0, column].Value = column + 1;
                    dataGridEnum[1, column].Value = CommonBLL.GetEnumDescription((Enum)valEnum);

                   // HTCT Specific changes
                    if (!IsOffline)
                    {
                        if (ConfigInfo.MeterModel == "10" && CommonBLL.GetEnumDescription((Enum)valEnum) == "Kvah Selection")
                        {
                            dataGridEnum[1, column].Value = "Mvah Selection";
                        }
                    }
                    else
                    {
                        if (CommonBLL.GetEnumDescription((Enum)valEnum) == "Kvah Selection")
                        {
                            dataGridEnum[1, column].Value = "Kvah/Mvah Selection";
                        }
                    }
                    column++;
                }
            }
            for (int count = 0; count < dataGridEnum.Rows.Count; count++)
            {
                //if (Convert.ToString(dataGridEnum.Rows[count].Cells[1].Value).Contains("Tamper Icon") 
                //    && 
                //    ConfigInfo.RightID[8] == '0')
                //{
                //    dataGridEnum.Rows[count].Cells["Select"].Value = false;
                //    dataGridEnum.Rows[count].Cells["Select"].ReadOnly  = true;
                //    dataGridEnum.Rows[count].Cells["Status"].Value = "Readout Rights not given.";
                //}
                //else
                //{
                    dataGridEnum.Rows[count].Cells["Select"].Value = true;
                    dataGridEnum.Rows[count].Cells["Status"].Value = "Readout Not Started.";
                //}
            }

            //Remove MagnetTamperIcon if Advance Right not given
            if (RemoveVal != null)
            {
                RemoveEnumFromList(enumList, RemoveVal);
            }

            if (dataGridEnum.Rows.Count == enumLength + 1)
                dataGridEnum.AllowUserToAddRows = false;

            foreach (DataGridViewRow row in dataGridEnum.Rows)
            {
                if (row.Cells[1].Value.ToString().Equals("Instantaneous"))
                {
                    row.ReadOnly = true;
                    break;
                }
            }
        }

       

        /// <summary>
        /// Sets colour for the column Status
        /// </summary>
        /// <param name="value"></param>
        /// <param name="style"></param>
        public void SetColour(Enum value, DataGridViewCellStyle style)
        {
            string description = string.Empty;
            description = CommonBLL.GetEnumDescription(value);
            foreach (DataGridViewRow gridViewRow in dataGridEnum.Rows)
            {
                if (gridViewRow.Cells["description"].Value.ToString() == description)
                {
                    if (dataGridEnum.Columns.Contains("Status"))
                        gridViewRow.Cells["Status"].Style = style;
                }
            }
        }
        /// <summary>
        /// Method takes List as an argument and returns another list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumList"></param>
        /// <returns></returns>
        public List<T> GetSelectedProfilesList<T>(IList <T> enumList)
        {
            List<T> listMeterData = new List<T>();
            for (int count = 0; count< dataGridEnum.RowCount; count ++)
            {
                DataGridViewCheckBoxCell chkGrid = dataGridEnum.Rows[count].Cells[2] as DataGridViewCheckBoxCell;
                if (Convert.ToBoolean(chkGrid.Value))
                {
                    T value = enumList [count];
                    listMeterData.Add(value);
                }
            }
            return listMeterData;
        }

        /// <summary>
        /// Updates the status of GridView
        /// </summary>
        /// <param name="value"></param>
        /// <param name="statusMsg"></param>
        public void SetStatus(Enum value, string statusMsg)
        {
            string description = string.Empty;
            description = CommonBLL.GetEnumDescription(value);
            foreach (DataGridViewRow gridViewRow in dataGridEnum.Rows)
            {
                // HTCT Specific Changes
                if (gridViewRow.Cells["description"].Value.ToString() == description
                    || gridViewRow.Cells["description"].Value.ToString() == "Mvah Selection")
                {
                    if (dataGridEnum.Columns.Contains("Status"))
                        gridViewRow.Cells["Status"].Value = statusMsg;
                }
            }

        }

        /// <summary>
        /// Changes Cell Style based on the boolean flag
        /// </summary>
        /// <param name="flag"></param>
        public void SetDefaultCellStyle(bool flag)
        {
            if (flag)
                dataGridEnum.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
            else
                dataGridEnum.SelectionMode = DataGridViewSelectionMode.FullRowSelect;           
        }
        /// <summary>
        /// Deselects all the checkboxes
        /// </summary>
        public void DeselectCheckBoxes()
        {
            for (int count = 0; count < dataGridEnum.Rows.Count; count++)
            {
                dataGridEnum.Rows[count].Cells["Select"].Value = false;
            }
            checkBoxSelectAll.Checked = false;
        }       

        /// <summary>
        /// Column status is removed
        /// </summary>
        public void DisableStatusColumn()
        {
            dataGridEnum.Columns.Remove("Status"); 
        }

        /// <summary>
        /// Unchecks particular checkbox
        /// </summary>
        /// <param name="value"></param>
        public void UncheckCheckBox(Enum value)
        {
            string description = string.Empty;
            description = CommonBLL.GetEnumDescription(value);
            foreach (DataGridViewRow gridViewRow in dataGridEnum.Rows)
            {
                if (gridViewRow.Cells["description"].Value.ToString() == description)
                {
                    gridViewRow.Cells["Select"].Value = false;
                    break;
                }
            } 
        }

        /// <summary>
        /// This Method is used to make sure that all gridview check boxes corresponding to input enum list gets checked .
        /// </summary>
        /// <param name="list"></param>
        public void SelectUploadedParameters(List<System.Enum> list)
        {
            int count = 0;
            try
            {
                string description = string.Empty;
                foreach (Enum inputValue in list)
                {
                    description = CommonBLL.GetEnumDescription(inputValue);
                    foreach (DataGridViewRow gridViewRow in dataGridEnum.Rows)
                    {
                        if (gridViewRow.Cells["description"].Value.ToString() == description)
                        {
                            gridViewRow.Cells["Select"].Value = true;
                            count++;
                            break;
                        }
                    }                    
                }
                if (count == dataGridEnum.Rows.Count)
                {
                    checkBoxSelectAll.Checked = true;
                }
            }
            catch
            {

            }
        }

        #endregion

        #region Protected Methods
        #endregion

        #region Event Handlers

        private void checkBoxSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxSelectAll.Checked)
            {
                for (int count = 0; count < dataGridEnum.RowCount; count++)
                {
                    dataGridEnum.Rows[count].Cells["Select"].Value = true;

                }
            }
            else
            {
                for (int count = 0; count < dataGridEnum.RowCount; count++)
                {
                    if (!dataGridEnum.Rows[count].Cells["Description"].Value.ToString().Equals("Instantaneous"))
                    {
                        dataGridEnum.Rows[count].Cells["Select"].Value = false;
                    }
                }
            }
        }

        private void dataGridEnum_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            bool flag = true;
            for (int count = 0; count < dataGridEnum.Rows.Count; count++)
            {
                if (Convert.ToBoolean(dataGridEnum.Rows[count].Cells["Select"].Value) == false)
                {
                    flag = false;
                    break;
                }
            }

            checkBoxSelectAll.CheckedChanged -= checkBoxSelectAll_CheckedChanged;
            if (flag)
            {
                checkBoxSelectAll.Checked = true;
            }
            else
            {
                checkBoxSelectAll.Checked = false;
            }
            checkBoxSelectAll.CheckedChanged += checkBoxSelectAll_CheckedChanged;
        }

        private void dataGridEnum_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGridEnum.IsCurrentCellDirty)
            {
                dataGridEnum.CommitEdit(DataGridViewDataErrorContexts.CurrentCellChange);
            }
        }
        #endregion

        #region Private Methods 
        private void RemoveEnumFromList(List<Enum> enumList, Enum RemoveVal)
        {
            enumList.Remove(RemoveVal);
        }
        #endregion

        public void RefreshList(List<Enum> enumList,bool IsOffline)
        {
            try
            {
                dataGridEnum.Rows.Clear();
                int column = 0;
                Enum RemoveVal = null;
                enumLength = enumList.Count;
                foreach (var valEnum in enumList)
                {
                    if (CommonBLL.GetEnumDescription((Enum)valEnum) != null)
                    {
                        if (Convert.ToString(valEnum).Contains("MagneticTamperIcon") && ConfigInfo.RightID[8] == '0')
                        {
                            //Check MagnetTamperIcon for remove if Advance Right not given
                            RemoveVal = valEnum;
                            continue;
                        }
                        dataGridEnum.Rows.Add();
                        dataGridEnum[0, column].Value = column + 1;
                        dataGridEnum[1, column].Value = CommonBLL.GetEnumDescription((Enum)valEnum);
                        // HTCT Specific changes
                        if (!IsOffline)
                        {
                            if (ConfigInfo.MeterModel == "10" && CommonBLL.GetEnumDescription((Enum)valEnum) == "Kvah Selection")
                            {
                                dataGridEnum[1, column].Value = "Mvah Selection";
                            }
                        }
                        else
                        {
                            if (CommonBLL.GetEnumDescription((Enum)valEnum) == "Kvah Selection")
                            {
                                dataGridEnum[1, column].Value = "Kvah/Mvah Selection";
                            }
                        }
                        column++;
                    }
                }    
                //Remove MagnetTamperIcon if Advance Right not given
                if (RemoveVal != null)
                {
                    RemoveEnumFromList(enumList, RemoveVal);
                }

                if (dataGridEnum.Rows.Count == enumLength + 1)
                    dataGridEnum.AllowUserToAddRows = false;
            }
            catch (Exception ex)
            {
                
                
            }           
        }
    }
}

