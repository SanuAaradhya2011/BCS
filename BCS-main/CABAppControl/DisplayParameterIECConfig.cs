using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;

namespace CABAppControl
{
    public partial class DisplayParameterIECConfig : UserControl
    {
        #region Constructor
        public DisplayParameterIECConfig()
        {
            InitializeComponent();            
            tabControlDisplayParams.TabPages.Remove(tabDisplayTimeouts);
        }
        #endregion

        #region DisplayParameterFillFromXML

        public void FillDisplayParameters()
        {
            XmlDataDocument xmlDatadoc = null;
            DataSet ds = null;
            try
            {
                //Read the Parameters from the XML file.
                xmlDatadoc = new XmlDataDocument();               
                xmlDatadoc.DataSet.ReadXml(string.Concat(Path.GetDirectoryName(Application.ExecutablePath), "\\", "DisplayParametersIECConfig.xml"));
                
                //deserialize xml data
                ds = xmlDatadoc.DataSet;

                //set datasource of gridview
                dgridPushDisplayParams.DataSource = ds.DefaultViewManager;
                dgridScrollDisplayParams.DataSource = ds.DefaultViewManager;
                dgridHighResolution.DataSource = ds.DefaultViewManager;

                //specify grdiview datamember
                dgridPushDisplayParams.DataMember = "PushDisplayParams";
                dgridScrollDisplayParams.DataMember = "ScrollDisplayParams";
                dgridHighResolution.DataMember = "HighResolution";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {   //dispose and free memory occupied by objects
                ds.Dispose();                
            }

            DataGridViewColumn chkboxColumn_PushParameters = new DataGridViewCheckBoxColumn();
            chkboxColumn_PushParameters.Name = "colInclude";
            chkboxColumn_PushParameters.HeaderText = "Include";
            if (!dgridPushDisplayParams.Columns.Contains("colInclude"))
            {
                dgridPushDisplayParams.Columns.Add(chkboxColumn_PushParameters);
            }

            DataGridViewColumn chkboxColumn_ScrollParameters = new DataGridViewCheckBoxColumn();
            chkboxColumn_ScrollParameters.Name = "colInclude";
            chkboxColumn_ScrollParameters.HeaderText = "Include";
            if (!dgridScrollDisplayParams.Columns.Contains("colInclude"))
            {
                dgridScrollDisplayParams.Columns.Add(chkboxColumn_ScrollParameters);
            }

            DataGridViewColumn chkboxColumn_HighResolution = new DataGridViewCheckBoxColumn();
            chkboxColumn_HighResolution.Name = "colInclude";
            chkboxColumn_HighResolution.HeaderText = "Include";
            if (!dgridHighResolution.Columns.Contains("colInclude"))
            {
                dgridHighResolution.Columns.Add(chkboxColumn_HighResolution);
            }

            dgridPushDisplayParams.Columns["SNo"].Width = dgridScrollDisplayParams.Columns["SNo"].Width = dgridHighResolution.Columns["SNo"].Width = 60;
            dgridPushDisplayParams.Columns["ID"].Width = dgridScrollDisplayParams.Columns["ID"].Width = dgridHighResolution.Columns["ID"].Width = 60;
            dgridPushDisplayParams.Columns["Description"].Width = dgridScrollDisplayParams.Columns["Description"].Width = dgridHighResolution.Columns["Description"].Width = 270;

            dgridPushDisplayParams.Columns[0].ReadOnly = dgridPushDisplayParams.Columns[1].ReadOnly =
                dgridScrollDisplayParams.Columns[0].ReadOnly = dgridScrollDisplayParams.Columns[1].ReadOnly =
                dgridHighResolution.Columns[0].ReadOnly = dgridHighResolution.Columns[1].ReadOnly = true;

            dgridPushDisplayParams.Columns[0].SortMode = dgridScrollDisplayParams.Columns[0].SortMode = dgridHighResolution.Columns[0].SortMode =
                dgridPushDisplayParams.Columns[1].SortMode = dgridScrollDisplayParams.Columns[1].SortMode = dgridHighResolution.Columns[1].SortMode =
                    dgridPushDisplayParams.Columns["colInclude"].SortMode = dgridScrollDisplayParams.Columns["colInclude"].SortMode = dgridHighResolution.Columns["colInclude"].SortMode
                = DataGridViewColumnSortMode.NotSortable;

            dgridPushDisplayParams.Select();
        }

        #endregion

        #region DisplayParameterGridViewManipulation

        private string GetPushButtonSelectedString()
        {
            string strSelectedList = string.Empty;
            try 
	        {
                if (dgridPushDisplayParams != null)
                {
                    if (dgridPushDisplayParams.Columns != null)
                    {
                        foreach (DataGridViewRow item in dgridPushDisplayParams.Rows)
                        {
                            if (Convert.ToBoolean(item.Cells["colInclude"].Value) == true)
                            {
                                int keyVal = Convert.ToInt32(item.Cells["ID"].Value);
                                strSelectedList += string.Format("{0:X2}", keyVal);         
                            }
                        }
                    }
                }
	        }
	        catch (Exception ex)
	        {
        		
        		
	        }
            return strSelectedList;
        }

        private string GetScrollButtonSelectedString()
        {
            string strSelectedList = string.Empty;
            try
            {
                if (dgridScrollDisplayParams != null)
                {
                    if (dgridScrollDisplayParams.Columns != null)
                    {
                        foreach (DataGridViewRow item in dgridScrollDisplayParams.Rows)
                        {
                            if (Convert.ToBoolean(item.Cells["colInclude"].Value) == true)
                            {
                                int keyVal = Convert.ToInt32(item.Cells["ID"].Value);
                                strSelectedList += string.Format("{0:X2}", keyVal);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {


            }
            return strSelectedList;
        }
        
        private string GetHighButtonSelectedString()
        {
            string strSelectedList = string.Empty;
            try
            {
                if (dgridHighResolution != null)
                {
                    if (dgridHighResolution.Columns != null)
                    {
                        foreach (DataGridViewRow item in dgridHighResolution.Rows)
                        {
                            if (Convert.ToBoolean(item.Cells["colInclude"].Value) == true)
                            {
                                int keyVal = Convert.ToInt32(item.Cells["ID"].Value);
                                strSelectedList += string.Format("{0:X2}", keyVal);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {


            }
            return strSelectedList;
        }

        public List<string> GetPushButtonSelectedList()
        {
            List<string> lstPush = new List<string>();
            try
            {
                string tempBytes = GetPushButtonSelectedString();                
                string commandByte = tempBytes.PadRight(128, '0');
                int paraIDX = 0;
                while (paraIDX < commandByte.Length)
                {
                    string temp = commandByte.Substring(paraIDX, 32);
                    paraIDX += 32;
                    lstPush.Add(temp);
                }
            }
            catch (Exception ex)
            {


            }
            return lstPush;
        }

        public List<string> GetScrollButtonSelectedList()
        {
            List<string> lstPush = new List<string>();
            try
            {
                string tempBytes = GetScrollButtonSelectedString();
                string commandByte = tempBytes.PadRight(128, '0');
                int paraIDX = 0;
                while (paraIDX < commandByte.Length)
                {
                    string temp = commandByte.Substring(paraIDX, 32);
                    paraIDX += 32;
                    lstPush.Add(temp);
                }
            }
            catch (Exception ex)
            {


            }
            return lstPush;
        }

        public List<string> GetHighButtonSelectedList()
        {
            List<string> lstPush = new List<string>();
            try
            {
                string tempBytes = GetHighButtonSelectedString();
                string commandByte = tempBytes.PadRight(128, '0');
                int paraIDX = 0;
                while (paraIDX < commandByte.Length)
                {
                    string temp = commandByte.Substring(paraIDX, 32);
                    paraIDX += 32;
                    lstPush.Add(temp);
                }
            }
            catch (Exception ex)
            {


            }
            return lstPush;
        }

        private void SetPushButtonSelectedList(List<string> strSelectedList)
        {
            try
            {
                if (dgridPushDisplayParams != null)
                {
                    if (dgridPushDisplayParams.Columns != null)
                    {
                        foreach (DataGridViewRow item in dgridPushDisplayParams.Rows)
                        {
                            string skeyVal = Convert.ToString(item.Cells["ID"].Value);                           
                            if (strSelectedList.Contains(skeyVal))
                            {
                                item.Cells["colInclude"].Value = true;
                            }
                            else
                            {
                                item.Cells["colInclude"].Value = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {


            }
        }

        private void SetScrollButtonSelectedList(List<string> strSelectedList)
        {
            try
            {
                if (dgridScrollDisplayParams != null)
                {
                    if (dgridScrollDisplayParams.Columns != null)
                    {
                        foreach (DataGridViewRow item in dgridScrollDisplayParams.Rows)
                        {
                            string skeyVal = Convert.ToString(item.Cells["ID"].Value);                           
                            if (strSelectedList.Contains(skeyVal))
                            {
                                item.Cells["colInclude"].Value = true;
                            }
                            else
                            {
                                item.Cells["colInclude"].Value = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {


            }
        }

        private void SetHighButtonSelectedList(List<string> strSelectedList)
        {
            try
            {
                if (dgridHighResolution != null)
                {
                    if (dgridHighResolution.Columns != null)
                    {
                        foreach (DataGridViewRow item in dgridHighResolution.Rows)
                        {
                            string skeyVal = Convert.ToString(item.Cells["ID"].Value);                           
                            if (strSelectedList.Contains(skeyVal))
                            {
                                item.Cells["colInclude"].Value = true;
                            }
                            else
                            {
                                item.Cells["colInclude"].Value = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {


            }
        }

        public void SetPushButtonSelectedList(Dictionary<string,string> dicSelectedList)
        {
            try
            {
                string strSelectedList = string.Empty;
                List<string> lstSelectedList = new List<string>();
                if (dicSelectedList.Count > 0)
                {
                    foreach (KeyValuePair<string,string> item in dicSelectedList)
                    {
                        if (item.Value.Contains('(') && item.Value.Contains(')'))
                        {
                            int startIndex = item.Value.IndexOf('(');
                            int length = item.Value.IndexOf(')') - startIndex;
                            if(length > 2)
                            {
                                strSelectedList += item.Value.Substring((startIndex + 1), (length-1));
                            }
                        }
                    }
                }
                if (strSelectedList.Length > 2)
                {
                    for (int count1 = 0; count1 <= strSelectedList.Length - 2; count1 += 2)
                        {
                            int itemIndex = Convert.ToInt16(Convert.ToInt32(strSelectedList.Substring(count1, 2), 16));
                            if (itemIndex != 0)
                            {
                                lstSelectedList.Add(itemIndex.ToString());
                            }
                        }
                   SetPushButtonSelectedList(lstSelectedList);
                }
                
            }
            catch (Exception ex)
            {


            }
        }

        public void SetScrollButtonSelectedList(Dictionary<string, string> dicSelectedList)
        {
            try
            {
                string strSelectedList = string.Empty;
                List<string> lstSelectedList = new List<string>();
                if (dicSelectedList.Count > 0)
                {
                    foreach (KeyValuePair<string, string> item in dicSelectedList)
                    {
                        if (item.Value.Contains('(') && item.Value.Contains(')'))
                        {
                            int startIndex = item.Value.IndexOf('(');
                            int length = item.Value.IndexOf(')') - startIndex;
                            if (length > 2)
                            {
                                strSelectedList += item.Value.Substring((startIndex + 1), (length - 1));
                            }
                        }
                    }
                }
                if (strSelectedList.Length > 2)
                {
                    for (int count1 = 0; count1 <= strSelectedList.Length - 2; count1 += 2)
                    {
                        int itemIndex = Convert.ToInt16(Convert.ToInt32(strSelectedList.Substring(count1, 2), 16));
                        if (itemIndex != 0)
                        {
                            lstSelectedList.Add(itemIndex.ToString());
                        }
                    }
                    SetScrollButtonSelectedList(lstSelectedList);
                }

            }
            catch (Exception ex)
            {


            }
        }

        public void SetHighButtonSelectedList(Dictionary<string, string> dicSelectedList)
        {
            try
            {
                string strSelectedList = string.Empty;
                List<string> lstSelectedList = new List<string>();
                if (dicSelectedList.Count > 0)
                {
                    foreach (KeyValuePair<string, string> item in dicSelectedList)
                    {
                        if (item.Value.Contains('(') && item.Value.Contains(')'))
                        {
                            int startIndex = item.Value.IndexOf('(');
                            int length = item.Value.IndexOf(')') - startIndex;
                            if (length > 2)
                            {
                                strSelectedList += item.Value.Substring((startIndex + 1), (length - 1));
                            }
                        }
                    }
                }
                if (strSelectedList.Length > 2)
                {
                    for (int count1 = 0; count1 <= strSelectedList.Length - 2; count1 += 2)
                    {
                        int itemIndex = Convert.ToInt16(Convert.ToInt32(strSelectedList.Substring(count1, 2), 16));
                        if (itemIndex != 0)
                        {
                            lstSelectedList.Add(itemIndex.ToString());
                        }
                    }
                    SetHighButtonSelectedList(lstSelectedList);
                }

            }
            catch (Exception ex)
            {


            }
        }



        #endregion


    }
}
