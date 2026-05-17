using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using CAB.Framework;
using System.IO;
using System.Runtime.Serialization;
using CAB.BLL;

namespace CABAppControl
{
    public partial class MultiSelect : UserControl
    {

        public MultiSelect(DataSet ds)
        {
            InitializeComponent();

            PopulateParamList(ds);
        }

        /// <summary>
        /// Method populates the parameter list
        /// </summary>
        /// <param name="ds"></param>
        private void PopulateParamList(DataSet ds)
        {
            try
            {
                if (ds != null && ds.Tables[0] != null)
                {
                    // getting list of available parameters 
                    var availableList = (from row in ds.Tables[0].AsEnumerable()
                                         where row["IsSelected"].ToString() != "1"
                                         orderby row["DisplayName"]
                                         select new Parameters { DisplayName = row["DisplayName"].ToString(), Id = row["Identifier"].ToString() }).ToArray();

                    // getting list of selected parameters
                    var selectedList = (from row in ds.Tables[0].AsEnumerable()
                                        where row["IsSelected"].ToString() == "1"
                                        orderby row["DisplayName"]
                                        select new Parameters { DisplayName = row["DisplayName"].ToString(), Id = row["Identifier"].ToString() }).ToArray();

                    if (availableList.Count() > 0)
                    {
                        listAvailable.Items.AddRange(availableList);
                        listAvailable.DisplayMember = "DisplayName";
                        listAvailable.ValueMember = "Id";
                    }

                    if (selectedList.Count() > 0)
                    {
                        lstSelected.Items.AddRange(selectedList);
                        lstSelected.DisplayMember = "DisplayName";
                        lstSelected.ValueMember = "Id";
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// event handler to  move multiple parameters from selected to available
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectedToAvailableMany_Click(object sender, EventArgs e)
        {
            listAvailable.Items.AddRange(lstSelected.SelectedItems.Cast<object>().ToArray());
            listAvailable.DisplayMember = "DisplayName";
            listAvailable.ValueMember = "Id";

            Object[] selectedItem = lstSelected.SelectedItems.Cast<object>().ToArray();

            foreach (object obj in selectedItem)
            {
                lstSelected.Items.Remove(obj);
            }

        }

        /// <summary>
        /// event handler to  move all parameters from selected to available
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectedToAvailableAll_Click(object sender, EventArgs e)
        {
            listAvailable.Items.AddRange(lstSelected.Items.Cast<object>().ToArray());
            listAvailable.DisplayMember = "DisplayName";
            listAvailable.ValueMember = "Id";
            lstSelected.Items.Clear();
        }


        /// <summary>
        /// event handler to  move multiple parameters from  available to selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAvailableToSelectedMany_Click(object sender, EventArgs e)
        {
            lstSelected.Items.AddRange(listAvailable.SelectedItems.Cast<object>().ToArray());
            lstSelected.DisplayMember = "DisplayName";
            lstSelected.ValueMember = "Id";

            Object[] availableItem = listAvailable.SelectedItems.Cast<object>().ToArray();

            foreach (object obj in availableItem)
            {
                listAvailable.Items.Remove(obj);
            }
        }

        /// <summary>
        /// event handler to  move all parameters from  available to selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAvailableToSelectedAll_Click(object sender, EventArgs e)
        {
            lstSelected.Items.AddRange(listAvailable.Items.Cast<object>().ToArray());
            lstSelected.DisplayMember = "DisplayName";
            lstSelected.ValueMember = "Id";
            listAvailable.Items.Clear();
        }

        /// <summary>
        /// eventhandler to save selected and available parameters to database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (lstSelected.Items.Count > 0)
            {
                string ProfileName = string.Empty;
                string[] names = Enum.GetNames(typeof(Profile));

                //getting the profile for which event is triggered
                foreach (string name in names)
                {
                    if (((Profile)Enum.Parse(typeof(Profile), name)).GetDisplayName().Equals(this.Parent.Text))
                    {
                        ProfileName = name;
                        break;
                    }
                }

                XmlSerializer xmlSer = new XmlSerializer(typeof(Parameters[]));
                MemoryStream stream = new MemoryStream();
                string availableItemsXml = string.Empty;
                string selectedItemsXml = string.Empty;

                try
                {
                    Parameters[] availableItems = listAvailable.Items.Cast<Parameters>().ToArray();

                    // serializing items to xml
                    xmlSer.Serialize(stream, availableItems);
                    availableItemsXml = Encoding.UTF8.GetString(stream.ToArray());
                }
                catch { }


                try
                {
                    Parameters[] selectedItems = lstSelected.Items.Cast<Parameters>().ToArray();
                    stream = new MemoryStream();

                    // serializing items to xml
                    xmlSer.Serialize(stream, selectedItems);
                    selectedItemsXml = Encoding.UTF8.GetString(stream.ToArray());
                }
                catch { }


                // Update database for the changes made
                ReportBLL.UpdateParametersSelection(availableItemsXml, selectedItemsXml, ProfileName);
                MessageBox.Show("Report parameters saved successfully.", "BCS",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Please select atleast one parameter for report.", "BCS",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        ///  inner class to represent list item,marked it serializable 
        /// </summary>
      [Serializable]
      public class Parameters
        {
            public string DisplayName { get; set; }
            public string Id { get; set; }
        }

    }


}
