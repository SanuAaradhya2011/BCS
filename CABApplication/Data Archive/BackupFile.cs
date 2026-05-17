using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CAB.UI.Controls;
using CAB.BLL;
using System.Xml;
using CAB.Entity;
using CAB.Framework.Utility;

namespace CAB.UI
{
    public partial class BackupFile : MdiChildForm
    {
        FileUploadMasterBLL fileUploadMasterBLL = new FileUploadMasterBLL();
        public BackupFile()
        {
            this.Text = "Backup Data";
            InitializeComponent();
        }

        private void BackupFile_Load(object sender, EventArgs e)
        {
            this.Text = "Backup Data";
            DataSet dataSet = fileUploadMasterBLL.ComboList();
            chkFileUploadList.Items.Clear();
            if (dataSet != null)
            {
                foreach (DataRow Drow in dataSet.Tables[0].Rows)
                {
                    chkFileUploadList.Items.Add(Drow["FileName"].ToString());
                }
            }
        } 

        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            CheckedAll(chkSelectAll.Checked);
        }

        private void CheckedAll(bool status)
        {
            for (int i = 0; i < chkFileUploadList.Items.Count; i++)
            {
                chkFileUploadList.SetItemChecked(i, status);
            } 
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.StatusMessage = "";
            Application.DoEvents();
            this.Close();
        }

        private void GetData(string fileName)
        {
            XmlTextWriter writer = new XmlTextWriter(fileName, null);
            writer.Formatting = Formatting.Indented; 
            DataSet dataSet = new DataSet();
            writer.WriteStartDocument();
            writer.WriteStartElement("MeterData");
            foreach (object item in chkFileUploadList.CheckedItems)
            { 
                FileUploadMasterEntity fileUploadMasterEntity = fileUploadMasterBLL.ValidateFile(item.ToString()) as FileUploadMasterEntity;
                if (fileUploadMasterEntity != null)
                {
                    writer.WriteStartElement("Table");
                    string FileContent=string.Empty;
                    if(fileUploadMasterEntity.FileContent!=null)
                      FileContent = Encoding.ASCII.GetString(fileUploadMasterEntity.FileContent);   
                    writer.WriteStartElement("FileUpload_ID");
                    writer.WriteString(fileUploadMasterEntity.FileUpload_ID.ToString());
                    writer.WriteEndElement(); 

                    writer.WriteStartElement("FileName");
                    writer.WriteString(fileUploadMasterEntity.FileName);
                    writer.WriteEndElement();

                    writer.WriteStartElement("FileContent");
                    writer.WriteString(FileContent);
                    writer.WriteEndElement();
                    writer.WriteString("~");

                    writer.WriteStartElement("fileCreatedOn");
                    writer.WriteString(fileUploadMasterEntity.UploadingDateTime.ToString());
                    writer.WriteEndElement();

                    writer.WriteStartElement("userID");
                    writer.WriteString(fileUploadMasterEntity.UserInformation_ID.ToString());
                    writer.WriteEndElement(); 

                    writer.WriteStartElement("fileLocationPath");
                    writer.WriteString(ConfigInfo.GetLocation());
                    writer.WriteEndElement();
                    writer.WriteEndElement(); 
                }                
            }
            writer.WriteEndDocument();	               
            writer.Flush();                             
            writer.Close();
        }

        private void btnBackup_Click(object sender, EventArgs e)
        {
            if (chkFileUploadList.Items.Count <= 0)
            {
                this.StatusMessage = "File not exist";
                Application.DoEvents();
            }
            else
            {
                if (chkFileUploadList.CheckedItems.Count > 0)
                { 
                    SaveFileDialog savefile = new SaveFileDialog(); 
                    savefile.Filter = "Backup files (*.Bak)|*.Bak";

                    if (chkFileUploadList.CheckedItems.IndexOf(".Bak") > -1)
                     savefile.FilterIndex = 0; 
                    else
                        savefile.FilterIndex = 1;
                    savefile.RestoreDirectory = true;
                    if (savefile.ShowDialog() == DialogResult.OK)
                    {
                        GetData(savefile.FileName);
						this.StatusMessage = "Backup created successfully.";
                        Application.DoEvents();
                    }
                }
                else
                {
                    this.StatusMessage = "No file selected.";
                    Application.DoEvents();
                }
            }
        }

        private void BackupFile_Activated(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            Application.DoEvents();
        }

        private void BackupFile_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.StatusMessage = string.Empty;
            Application.DoEvents();
        } 
    }
}
