using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;
using CAB.UI.Controls;
using CAB.BLL;
using CAB.Framework.Entity;
using CAB.Entity;
using CAB.Framework;


namespace MainForm.PresentationLayer.WindowsForms
{
    public partial class BackupData : MdiChildForm
    {
        private Dictionary<string, int> fileDictionary;

        public BackupData()
        {
            InitializeComponent();
        }
        
        private void BackupData_Activated(object sender, EventArgs e)
        {
            fileDictionary = new Dictionary<string, int>();
            FileUploadMasterBLL fileUploadMasterBLL = new FileUploadMasterBLL();
            fileUploadMasterBLL.ListDataSet();
            checkedListBox1.Items.Clear();
            foreach (KeyValuePair<string, int> kvp in fileDictionary)
            {
                checkedListBox1.Items.Add(kvp.Key);
            }

            if (checkedListBox1.Items.Count <= 0)
                btn_backup.Enabled = false;
        }


        private void btn_backup_Click(object sender, EventArgs e)
        {
            bool isBackupCompleted = false;
            if (checkedListBox1.CheckedItems.Count == 0)
            {
                CABMessageBox.ShowFilterMessage("M000003", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            SaveFileDialog saveBackupFileDialog = new SaveFileDialog();
            saveBackupFileDialog.Filter = "Backup files (*.Bak)|*.Bak";
            saveBackupFileDialog.RestoreDirectory = true;
            if (saveBackupFileDialog.ShowDialog() == DialogResult.OK)
            {
                isBackupCompleted = DataBackup(saveBackupFileDialog.FileName);
            }

            if (isBackupCompleted)
                this.StatusMessage = "";
        }

        private bool DataBackup(string fileName)
        {
            bool isBackupSucessful = false;
            try
            {
                int fileId = 0;

                FileUploadMasterEntity fileUploadMasterEntity = new FileUploadMasterEntity();
                FileUploadMasterBLL fileUploadMasterBLL = new FileUploadMasterBLL();
                XmlTextWriter xmlWriter = new XmlTextWriter(fileName, null);
                xmlWriter.Formatting = Formatting.Indented;

                xmlWriter.WriteStartDocument();                //Start the document
                xmlWriter.WriteStartElement("CDF");
                xmlWriter.WriteStartElement("UTILITYTYPE");
                xmlWriter.WriteStartAttribute("CODE");
                xmlWriter.WriteString("1");
                xmlWriter.WriteEndAttribute();
                int Rcnt = 0;

                foreach (object item in checkedListBox1.CheckedItems)
                {
                    xmlWriter.WriteStartElement("F" + Rcnt.ToString());
                    if (fileDictionary.TryGetValue(item.ToString(), out fileId))
                        fileUploadMasterEntity = fileUploadMasterBLL.ListDataSet(fileId) as FileUploadMasterEntity;


                    xmlWriter.WriteStartElement("FileUploadID");
                    xmlWriter.WriteString(fileUploadMasterEntity.FileUpload_ID.ToString());
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteStartElement("FileName");
                    xmlWriter.WriteString(fileUploadMasterEntity.FileName);
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("FileContent");
                    xmlWriter.WriteString(fileUploadMasterEntity.FileContent);
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("UploadingDateTime");
                    xmlWriter.WriteString(fileUploadMasterEntity.UploadingDateTime.ToString());
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("UserInformationID");
                    xmlWriter.WriteString(fileUploadMasterEntity.UserInformation_ID.ToString());
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteEndElement();
                    Rcnt++;

                }
                xmlWriter.WriteEndDocument();	                //closing the document		
                xmlWriter.Flush();                             //flush the writer
                xmlWriter.Close();
                isBackupSucessful= true;
            }
            catch (Exception ex)
            {
                throw new CABException();
            }
            return isBackupSucessful;
        }

        private void BD_chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            if (BD_chkSelectAll.Checked == true)
            {
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    checkedListBox1.SetItemChecked(i, true);
                }
            }
            else
            {
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    checkedListBox1.SetItemChecked(i, false);
                }
            }
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked)
            {
                if (checkedListBox1.CheckedItems.Count + 1 == checkedListBox1.Items.Count)
                {
                    BD_chkSelectAll.Checked = true;
                }
            }
            else if (e.NewValue == CheckState.Unchecked)
            {
                BD_chkSelectAll.Checked = false;
            }
        }

        private void btn_bkup_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BD_chkSelectAll_Click(object sender, EventArgs e)
        {
            if (BD_chkSelectAll.Checked == true)
            {
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    checkedListBox1.SetItemChecked(i, true);
                }
            }
            else
            {
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    checkedListBox1.SetItemChecked(i, false);
                }
            }
        }
    }
}
