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

    public partial class DisplayParameterIEC : UserControl
    {
        #region MemberVariables

        const int lstCountMax = 64;
        DataTable dtPushDisplayParameter = null;
        DataTable dtHighDisplayParameter = null;
        DataTable dtScrollDisplayParameter = null;
        private int totalPushCount = 0;
        private int totalScrollCount = 0;
        private int totalHighCount = 0;


        public int TotalPushCount
        {
            get
            {
                return totalPushCount;
            }           
        }


        public int TotalScrollCount
        {
            get
            {
                return totalScrollCount;
            }           
        }

        public int TotalHighCount
        {
            get
            {
                return totalHighCount;
            }            
        }



        #endregion

        #region Constructor

        public DisplayParameterIEC()
        {
            try
            {
                InitializeComponent();
                tabControlDisplayParams.TabPages.Remove(tabDisplayTimeouts);
            }
            catch (Exception ex)
            {
                
                
            }
        }

        #endregion  

        #region CommonFunction

        private void MoveListItemsUp(ListBox lstselected)
        {
            try
            {
                if (lstselected.SelectedItems.Count > 0)
                {
                    object selected = lstselected.SelectedItem;
                    int indx = lstselected.Items.IndexOf(selected);
                    int totl = lstselected.Items.Count;

                    if (indx == 0)
                    {
                        return;                       
                    }
                    else
                    {
                        lstselected.Items.Remove(selected);
                        lstselected.Items.Insert(indx - 1, selected);
                        lstselected.SetSelected(indx - 1, true);
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        private void MoveListItemsDown(ListBox lstselected)
        {
            try
            {
                if (lstselected.SelectedItems.Count > 0)
                {
                    object selected = lstselected.SelectedItem;
                    int indx = lstselected.Items.IndexOf(selected);
                    int totl = lstselected.Items.Count;

                    if (indx == totl - 1)
                    {
                        return;                      
                    }
                    else
                    {
                        lstselected.Items.Remove(selected);
                        lstselected.Items.Insert(indx + 1, selected);
                        lstselected.SetSelected(indx + 1, true);
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        private void MoveItem(ListBox lstSource, ListBox lstDestination)
        {
            try
            {
                if (lstSource != null && lstDestination != null && lstDestination.Items != null && lstSource.SelectedItems != null)
                {
                    if ((lstDestination.Items.Count + lstSource.SelectedItems.Count) >= lstCountMax)
                    {
                        MessageBox.Show("Selection of more than 64 parameters not allowed", "3 Phase BCS", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    foreach (DataRowView item in lstSource.SelectedItems)
                    {
                        lstDestination.DisplayMember = "Description";
                        lstDestination.Items.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {                
                
            }            
        }

        private void RemoveItem(ListBox lstDestination)
        {
            try
            {
                if (lstDestination != null && lstDestination.Items != null && lstDestination.SelectedItems != null)
                {
                    while (lstDestination.SelectedIndices.Count > 0)
                    {
                        lstDestination.Items.RemoveAt(lstDestination.SelectedIndex);
                        lstDestination.Refresh();
                    }
                }
            }
            catch (Exception ex)
            {                
                
            }            
        }

        private void MoveItemAll(ListBox lstSource, ListBox lstDestination)
        {
            try
            {
                if (lstSource != null && lstDestination != null && lstDestination.Items != null && lstSource.Items != null)
                {
                    if (lstSource.Items.Count >= lstCountMax)
                    {
                        MessageBox.Show("Selection of more than 64 parameters not allowed", "3 Phase BCS", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    lstDestination.Items.Clear();
                    foreach (DataRowView item in lstSource.Items)
                    {
                        lstDestination.DisplayMember = "Description";
                        lstDestination.Items.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                
                
            }            
        }

        private void RemoveItemAll(ListBox lstDestination)
        {
            try
            {
                if (lstDestination != null && lstDestination.Items != null)
                {
                    lstDestination.Items.Clear();
                }
            }
            catch (Exception ex)
            {                
                
            }            
        }

        #endregion

        #region UserControlEvents

        private void btnHighMove_Click(object sender, EventArgs e)
        {
            MoveItem(lstHighAll, lstHighSelected);
        }

        private void btnHighMoveAll_Click(object sender, EventArgs e)
        {            
            MoveItemAll(lstHighAll, lstHighSelected);
        }

        private void btnHighRemove_Click(object sender, EventArgs e)
        {
            RemoveItem(lstHighSelected);
        }

        private void btnHighRemoveAll_Click(object sender, EventArgs e)
        {
            RemoveItemAll(lstHighSelected);
        }

        private void btnHighAutoMoveDown_Click(object sender, EventArgs e)
        {
            MoveListItemsDown(lstHighSelected);
        }

        private void btnHighAutoMoveUp_Click(object sender, EventArgs e)
        {
            MoveListItemsUp(lstHighSelected);
        }

        private void btnScrollMove_Click(object sender, EventArgs e)
        {
            MoveItem(lstScrollAll, lstScrollSelected);
        }

        private void btnScrollMoveAll_Click(object sender, EventArgs e)
        {
            MoveItemAll(lstScrollAll, lstScrollSelected);
        }

        private void btmScrollRemove_Click(object sender, EventArgs e)
        {
            RemoveItem(lstScrollSelected);
        }

        private void btnScrollRemoveAll_Click(object sender, EventArgs e)
        {
            RemoveItemAll(lstScrollSelected);
        }

        private void btnScrollAutoMoveUP_Click(object sender, EventArgs e)
        {
            MoveListItemsUp(lstScrollSelected);
        }

        private void btnScrollAutoMoveDown_Click(object sender, EventArgs e)
        {
            MoveListItemsDown(lstScrollSelected);
        }

        private void btnPushMove_Click(object sender, EventArgs e)
        {
            MoveItem(lstPushAll, lstPushSelected);
        }

        private void btnPushMoveAll_Click(object sender, EventArgs e)
        {
            MoveItemAll(lstPushAll, lstPushSelected);
        }

        private void btnPushRemove_Click(object sender, EventArgs e)
        {
            RemoveItem(lstPushSelected);
        }

        private void btnPushRemoveAll_Click(object sender, EventArgs e)
        {
            RemoveItemAll(lstPushSelected);
        }

        private void btnPushAutoMoveUP_Click(object sender, EventArgs e)
        {
            MoveListItemsUp(lstPushSelected);
        }

        private void btnPushAutoMoveDOWN_Click(object sender, EventArgs e)
        {
            MoveListItemsDown(lstPushSelected);
        }


#endregion

        #region GlobalMemberFunction


        //public void FillDisplayParameters()
        //{
        //    DisplayParameterList objdispara = new DisplayParameterList();
        //    DataTable dtPushDisplayParameter = objdispara.GetDisplayParameterList();
        //    DataTable dtHighDisplayParameter = objdispara.GetDisplayParameterList();
        //    DataTable dtScrollDisplayParameter = objdispara.GetDisplayParameterList();
        //    lstPushAll.DisplayMember = "Description";
        //    lstScrollAll.DisplayMember = "Description";
        //    lstHighAll.DisplayMember = "Description";
        //    lstPushAll.DataSource = dtPushDisplayParameter;
        //    lstScrollAll.DataSource = dtPushDisplayParameter;
        //    lstHighAll.DataSource = dtPushDisplayParameter;
        //}

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
                if (ds.Tables.Contains("PushDisplayParams"))
                {
                    lstPushAll.DataSource = ds.Tables["PushDisplayParams"];
                    dtPushDisplayParameter = ds.Tables["PushDisplayParams"];
                    lstPushAll.DisplayMember = "Description";
                    totalPushCount = lstPushAll.Items.Count;                    
                }
                if (ds.Tables.Contains("ScrollDisplayParams"))
                {
                    lstScrollAll.DataSource = ds.Tables["ScrollDisplayParams"];
                    dtScrollDisplayParameter = ds.Tables["ScrollDisplayParams"];
                    lstScrollAll.DisplayMember = "Description";
                    totalScrollCount = lstScrollAll.Items.Count;
                }
                if (ds.Tables.Contains("HighResolution"))
                {
                    lstHighAll.DataSource = ds.Tables["HighResolution"];
                    dtHighDisplayParameter = ds.Tables["HighResolution"];
                    lstHighAll.DisplayMember = "Description";
                    totalHighCount = lstHighAll.Items.Count;
                }

                EnableDisableButton();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {   
                //dispose and free memory occupied by objects
                if (ds != null)
                {
                    ds.Dispose();
                }
            }
        }

        public void SetPushButtonSelectedList(Dictionary<string, string> dicSelectedList)
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

        #endregion

        #region LocalMemberFunction

        private void SetPushButtonSelectedList(List<string> lstSelectedList)
        {
            try
            {
                if (lstPushAll != null && lstPushSelected != null && lstPushAll.Items != null && lstPushSelected.Items != null && dtPushDisplayParameter != null)
                {
                    lstPushSelected.Items.Clear();
                    foreach (string item in lstSelectedList)
                    {
                        DataRow[] drArr = dtPushDisplayParameter.Select("ID = '" + item + "'");                          
                        if (drArr != null && drArr.Length > 0)
                        {
                            DataRowView drv = dtPushDisplayParameter.DefaultView[dtPushDisplayParameter.Rows.IndexOf(drArr[0])];
                            lstPushSelected.Items.Add(drv);
                            lstPushSelected.DisplayMember = "Description";
                        }
                    }
                }

            }
            catch (Exception ex)
            {


            }
        }      

        private void SetScrollButtonSelectedList(List<string> lstSelectedList)
        {
            try
            {
                if (lstScrollAll != null && lstScrollSelected != null && lstScrollAll.Items != null && lstScrollSelected.Items != null && dtScrollDisplayParameter != null)
                {
                    lstScrollSelected.Items.Clear();
                    foreach (string item in lstSelectedList)
                    {
                        DataRow[] drArr = dtScrollDisplayParameter.Select("ID = '" + item + "'");
                        if (drArr != null && drArr.Length > 0)
                        {
                            DataRowView drv = dtScrollDisplayParameter.DefaultView[dtScrollDisplayParameter.Rows.IndexOf(drArr[0])];
                            lstScrollSelected.Items.Add(drv);
                            lstScrollSelected.DisplayMember = "Description";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
                
            }            
        }      

        private void SetHighButtonSelectedList(List<string> lstSelectedList)
        {
            try
            {
                if (lstHighAll != null && lstHighSelected != null && lstHighAll.Items != null && lstHighSelected.Items != null && dtHighDisplayParameter != null)
                {
                    foreach (string item in lstSelectedList)
                    {
                        DataRow[] drArr = dtHighDisplayParameter.Select("ID = '" + item + "'");
                        if (drArr != null && drArr.Length > 0)
                        {
                            DataRowView drv = dtHighDisplayParameter.DefaultView[dtHighDisplayParameter.Rows.IndexOf(drArr[0])];
                            lstHighSelected.Items.Add(drv);
                            lstHighSelected.DisplayMember = "Description";
                        }
                    }
                }
            }
            catch (Exception ex)
            {                
                
            }            
        }        

        private string GetPushButtonSelectedString()
        {
            string strSelectedList = string.Empty;
            try
            {
                if (lstPushAll != null && lstPushSelected != null && lstPushAll.Items != null && lstPushSelected.Items != null )
                {
                    foreach (DataRowView item in lstPushSelected.Items)
                    {
                        int keyVal = Convert.ToInt32(item.Row["ID"]);
                        strSelectedList += string.Format("{0:X2}", keyVal);
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
                if (lstScrollAll != null && lstScrollSelected != null && lstScrollAll.Items != null && lstScrollSelected.Items != null )
                {
                    foreach (DataRowView item in lstScrollSelected.Items)
                    {
                        int keyVal = Convert.ToInt32(item.Row["ID"]);
                        strSelectedList += string.Format("{0:X2}", keyVal);
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
                if (lstHighAll != null && lstHighSelected != null && lstHighAll.Items != null && lstHighSelected.Items != null && lstHighSelected.SelectedItems != null)
                {
                    foreach (DataRowView item in lstHighSelected.SelectedItems)
                    {
                        int keyVal = Convert.ToInt32(item.Row["ID"]);
                        strSelectedList += string.Format("{0:X2}", keyVal);
                    }
                }
            }
            catch (Exception ex)
            {


            }
            return strSelectedList;
        }

        private void EnableDisableButton()
        {
            //Push 
            if (totalPushCount > 0)
            {
                EnableDisableButtonControl(groupBox1, true);
            }
            else
            {
                EnableDisableButtonControl(groupBox1,false);
            }
            //Scroll
            if (totalScrollCount > 0)
            {
                EnableDisableButtonControl(groupBox2, true);
            }
            else
            {
                EnableDisableButtonControl(groupBox2, false);
            }
            //High
            if (totalHighCount > 0)
            {
                EnableDisableButtonControl(groupBox3, true);
            }
            else
            {
                EnableDisableButtonControl(groupBox3, false);
            }


        }

        private void EnableDisableButtonControl(GroupBox groupBox1, bool Flag)
        {
            try
            {
                foreach (Control item in groupBox1.Controls)
                {
                    if (item.GetType() == typeof(Button))
                    {
                        Button bt = (Button)item;
                        bt.Enabled = Flag;
                    }
                }
            }
            catch (Exception ex)
            {                
                
            }
        }

        #endregion

    }


    public class DisplayParameterList
    {
        public DataTable GetDisplayParameterList()
        {
            DataTable dictionaryDisplayList = new DataTable();
            dictionaryDisplayList.Columns.Add("Description");
            dictionaryDisplayList.Columns.Add("Value");
            int ItemIDX = 1;
            dictionaryDisplayList.Rows.Add("Cummulative Active Energy", ItemIDX++);//1
            dictionaryDisplayList.Rows.Add("Rate 1 Active Energy", ItemIDX++);//2
            dictionaryDisplayList.Rows.Add("Rate 2 Active Energy", ItemIDX++);//3
            dictionaryDisplayList.Rows.Add("Rate 3 Active Energy", ItemIDX++);//4
            dictionaryDisplayList.Rows.Add("Rate 4 Active Energy", ItemIDX++);//5
            dictionaryDisplayList.Rows.Add("Rate 5 Active Energy", ItemIDX++);//6
            dictionaryDisplayList.Rows.Add("Rate 6 Active Energy", ItemIDX++);//7
            dictionaryDisplayList.Rows.Add("Active Rate", ItemIDX++);//8
            dictionaryDisplayList.Rows.Add("Instant Voltage", ItemIDX++);//9
            dictionaryDisplayList.Rows.Add("Phase Active Power", ItemIDX++);//10
            dictionaryDisplayList.Rows.Add("Neutral Active Power", ItemIDX++);//11
            dictionaryDisplayList.Rows.Add("High Resolution Active Energy", ItemIDX++);//12
            dictionaryDisplayList.Rows.Add("Demand Reset Counter", ItemIDX++);//13
            dictionaryDisplayList.Rows.Add("Fraud Reset Counter", ItemIDX++);//14
            dictionaryDisplayList.Rows.Add("Time", ItemIDX++);//15
            dictionaryDisplayList.Rows.Add("Date", ItemIDX++);//16
            dictionaryDisplayList.Rows.Add("Blank Test", ItemIDX++);//17
            dictionaryDisplayList.Rows.Add("All Segement Test", ItemIDX++);//18
            dictionaryDisplayList.Rows.Add("Odd Segement Test", ItemIDX++);//19
            dictionaryDisplayList.Rows.Add("Even Segement Test", ItemIDX++);//20

            dictionaryDisplayList.Rows.Add("Instant Phase Current", ItemIDX++);//21
            dictionaryDisplayList.Rows.Add("Instant Neutral Current", ItemIDX++);//22
            dictionaryDisplayList.Rows.Add("Present Active MD", ItemIDX++);//23

            dictionaryDisplayList.Rows.Add("Present Month Consumption", ItemIDX++);//24
            dictionaryDisplayList.Rows.Add("Instant PF", ItemIDX++);//25
            dictionaryDisplayList.Rows.Add("Instant Frequency", ItemIDX++);//26
            dictionaryDisplayList.Rows.Add("AC Magnet Field Count", ItemIDX++);//27
            dictionaryDisplayList.Rows.Add("Main Battery Voltage", ItemIDX++);//28
            dictionaryDisplayList.Rows.Add("RTC Battery Voltage", ItemIDX++);//29
            dictionaryDisplayList.Rows.Add("TLV Voltage", ItemIDX++);//30
            dictionaryDisplayList.Rows.Add("Billing Active Power", ItemIDX++);//31
            dictionaryDisplayList.Rows.Add("Billing Active Energy", ItemIDX++);//32
            dictionaryDisplayList.Rows.Add("Billing Power-On Minutes", ItemIDX++);//33
            dictionaryDisplayList.Rows.Add("Cummulative Power-On Minutes", ItemIDX++);//34
            //dictionaryDisplayLi.Rowsst.Add("CUM POWEROFF MINUTES", ItemIDX++);//35
            dictionaryDisplayList.Rows.Add("Billing Average PF", ItemIDX++);//36
            dictionaryDisplayList.Rows.Add("Meter ID", ItemIDX++);//37
            dictionaryDisplayList.Rows.Add("Meter ID LSB", ItemIDX++);//38
            dictionaryDisplayList.Rows.Add("RATE 1 Acitve MD", ItemIDX++);//39
            dictionaryDisplayList.Rows.Add("RATE 2 Acitve MD", ItemIDX++);//40
            dictionaryDisplayList.Rows.Add("RATE 3 Acitve MD", ItemIDX++);//41
            dictionaryDisplayList.Rows.Add("RATE 4 Acitve MD", ItemIDX++);//42
            dictionaryDisplayList.Rows.Add("RATE 5 Acitve MD", ItemIDX++);//43
            dictionaryDisplayList.Rows.Add("RATE 6 Acitve MD", ItemIDX++);//44

            dictionaryDisplayList.Rows.Add("Voltage Comp Counts", ItemIDX++);//45
            dictionaryDisplayList.Rows.Add("Current Comp Counts", ItemIDX++);//46
            dictionaryDisplayList.Rows.Add("Power-Fail Comp Counts", ItemIDX++);//47
            dictionaryDisplayList.Rows.Add("Transaction Comp Counts", ItemIDX++);//48
            dictionaryDisplayList.Rows.Add("Other Comp Counts", ItemIDX++);//49
            dictionaryDisplayList.Rows.Add("Non-Rollover Comp Counts", ItemIDX++);//50
            dictionaryDisplayList.Rows.Add("Connect-Disconnect Comp Counts", ItemIDX++);//51

            dictionaryDisplayList.Rows.Add("Total Tamper Conts", ItemIDX++);//52
            dictionaryDisplayList.Rows.Add("ABC String", ItemIDX++);//53

            dictionaryDisplayList.Rows.Add("Signed PF", ItemIDX++);//54
            dictionaryDisplayList.Rows.Add("Present Average PF", ItemIDX++);//55
            dictionaryDisplayList.Rows.Add("Last Bill Date", ItemIDX++);//56
            dictionaryDisplayList.Rows.Add("Last Bill Time", ItemIDX++);//57
            dictionaryDisplayList.Rows.Add("Cummulative Apparent Energy", ItemIDX++);//58
            dictionaryDisplayList.Rows.Add("Cummulative Reactive Energy-Lag", ItemIDX++);//59
            dictionaryDisplayList.Rows.Add("Cummulative Reactive Energy-Lead", ItemIDX++);//60
            dictionaryDisplayList.Rows.Add("Instant Apparent Power", ItemIDX++);//61
            dictionaryDisplayList.Rows.Add("Instant Reactive Power", ItemIDX++);//62
            dictionaryDisplayList.Rows.Add("High Resolution Apparent Energy", ItemIDX++);//63
            dictionaryDisplayList.Rows.Add("High Resolution Reactive Energy-Lag", ItemIDX++);//64
            dictionaryDisplayList.Rows.Add("High Resolution Reactive Energy-Lead", ItemIDX++);//65
            dictionaryDisplayList.Rows.Add("Present Apparent MD", ItemIDX++);//66
            dictionaryDisplayList.Rows.Add("Billing Apparent Power", ItemIDX++);//67
            dictionaryDisplayList.Rows.Add("Billing Apparent Energy", ItemIDX++);//68

            dictionaryDisplayList.Rows.Add("Case Tamper First Occurrance", ItemIDX++);//69
            dictionaryDisplayList.Rows.Add("Active Instant Current", ItemIDX++);//70
            dictionaryDisplayList.Rows.Add("Active Instant Power", ItemIDX++);//71
            dictionaryDisplayList.Rows.Add("Apparent Rate", ItemIDX++);//72
            dictionaryDisplayList.Rows.Add("Rate 1 Apparent MD", ItemIDX++);//73
            dictionaryDisplayList.Rows.Add("Rate 2 Apparent MD", ItemIDX++);//74
            dictionaryDisplayList.Rows.Add("Rate 3 Apparent MD", ItemIDX++);//75
            dictionaryDisplayList.Rows.Add("Rate 4 Apparent MD", ItemIDX++);//76
            dictionaryDisplayList.Rows.Add("Rate 5 Apparent MD", ItemIDX++);//77
            dictionaryDisplayList.Rows.Add("Rate 6 Apparent MD", ItemIDX++);//78
            dictionaryDisplayList.Rows.Add("Rate 1 Apparent Energy", ItemIDX++);//79
            dictionaryDisplayList.Rows.Add("Rate 2 Apparent Energy", ItemIDX++);//80
            dictionaryDisplayList.Rows.Add("Rate 3 Apparent Energy", ItemIDX++);//81
            dictionaryDisplayList.Rows.Add("Rate 4 Apparent Energy", ItemIDX++);//82
            dictionaryDisplayList.Rows.Add("Rate 5 Apparent Energy", ItemIDX++);//83
            dictionaryDisplayList.Rows.Add("Rate 6 Apparent Energy", ItemIDX++);//84
            dictionaryDisplayList.Rows.Add("Active Tariff Price", ItemIDX++);//85
            dictionaryDisplayList.Rows.Add("Billing Reactive Energy-Lag", ItemIDX++);//86
            dictionaryDisplayList.Rows.Add("Billing Reactive Energy-Lead", ItemIDX++);//87

            dictionaryDisplayList.Rows.Add("Voltage Comp Latest Event", ItemIDX++);//88
            dictionaryDisplayList.Rows.Add("Current Comp Latest Event", ItemIDX++);//89
            dictionaryDisplayList.Rows.Add("Power-Fail Comp Latest Event", ItemIDX++);//90
            dictionaryDisplayList.Rows.Add("Transaction Comp Latest Event", ItemIDX++);//91
            dictionaryDisplayList.Rows.Add("Other Comp Latest Event", ItemIDX++);//92
            dictionaryDisplayList.Rows.Add("Non-Rollover Comp Latest Event", ItemIDX++);//93
            dictionaryDisplayList.Rows.Add("Connect-Disconnect Comp Latest Event", ItemIDX++);//94

            dictionaryDisplayList.Rows.Add("Comms Remove Tamper First Occurrance", ItemIDX++);//95
            dictionaryDisplayList.Rows.Add("Relay Malfunction Tamper First Occurrance", ItemIDX++);//96
            dictionaryDisplayList.Rows.Add("RS 485 Address", ItemIDX++);//97


            return dictionaryDisplayList;
        }
    }


}
