using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using CAB.Framework.Utility;
using System.Threading;
using Hunt.EPIC.Logging;

namespace CABApplication
{
    public partial class frmUploadXmlToCC : Form
    {
        // Windo Added for Upload xml files to CC
        string filePath = ConfigInfo.CheckOrCreatePath() + "\\";
        string backupfilePath = ConfigInfo.CheckOrCreatePath() + "\\Backup\\";
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(frmUploadXmlToCC).ToString());
        int itotalPerFactor = 0;
        bool isException = false;
        bool isPassed = false;
        ToolDataProcessorService.UploadStatus _uploadstatus;

        public frmUploadXmlToCC()
        {
            InitializeComponent();
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;
        }

        private void updateListView()
        {
            Directory.CreateDirectory(backupfilePath);
            listAvailableFiles.Items.Clear();

            string[] FileList = Directory.GetFiles(filePath, "*.xml", SearchOption.TopDirectoryOnly);

            if (FileList == null || FileList.Count() == 0)
            {
                toolStripStatusLabel1.Text = "No readout data found";
                return;
            }

            for (int icount = 0; icount < FileList.Count(); icount++)
                listAvailableFiles.Items.Add(FileList[icount].Replace(filePath, ""));

        }

        private void frmUploadXmlToCC_Load(object sender, EventArgs e)
        {
            updateListView();

            backgroundWorker1.DoWork +=
                new DoWorkEventHandler(backgroundWorker1_DoWork);
            backgroundWorker1.RunWorkerCompleted +=
                new RunWorkerCompletedEventHandler(
            backgroundWorker1_RunWorkerCompleted);
            backgroundWorker1.ProgressChanged +=
                new ProgressChangedEventHandler(
            backgroundWorker1_ProgressChanged);

        }

        private void btnupload_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.IsBusy != true)
            {
                // Start the asynchronous operation.
                isPassed = false;
                updateListView();
                btnupload.Enabled = false;
                backgroundWorker1.RunWorkerAsync();
                
            }
        }

        private void btnclose_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.WorkerSupportsCancellation == true)
            {
                // Cancel the asynchronous operation.
                backgroundWorker1.CancelAsync();
            }

            this.Close();
        }

        // This event handler is where the time-consuming work is done.
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                BackgroundWorker worker = sender as BackgroundWorker;
                isException = false;
                isPassed = false;
                ToolDataProcessorService.ToolData[] TD = new ToolDataProcessorService.ToolData[1];

                if (listAvailableFiles == null || listAvailableFiles.Items.Count == 0)
                {
                    toolStripStatusLabel1.Text = "No data available";
                    e.Result = false;
                    isPassed = false;
                    return;
                }

                itotalPerFactor = 100 / listAvailableFiles.Items.Count;

                for (int icount = 0; icount < listAvailableFiles.Items.Count; icount++)
                {
                    if (worker.CancellationPending == true)
                    {
                        e.Cancel = true;
                        break;
                    }

                    string fileContent = File.ReadAllText(filePath + listAvailableFiles.Items[icount].ToString());
                    byte[] payloadBytes = Encoding.ASCII.GetBytes(fileContent);
                    TD[0] = new ToolDataProcessorService.ToolData();
                    TD[0].MessagePayload = payloadBytes;
                    TD[0].FileEntryName = listAvailableFiles.Items[icount].ToString();
                    TD[0].MessageType = 0x00;
                    TD[0].ReceivedTime = (uint)DateTime.Now.Year;
                    TD[0].User = "HHU";

                    Application.DoEvents();
                   
                    string strremoteadd = ConfigSettings.GetValue("RemoteUploadAddress");
                    string strendpoint = ConfigSettings.GetValue("EndPointUploadName");
                    ToolDataProcessorService.ToolDataProcessorClient serCall = new ToolDataProcessorService.ToolDataProcessorClient(ConfigSettings.GetValue("EndPointUploadName"), ConfigSettings.GetValue("RemoteUploadAddress"));
                    _uploadstatus = serCall.HHUUploadData(TD);
                    serCall.Close();
                    logger.Log(LOGLEVELS.Error, "UploadToxmlCC", new Exception(_uploadstatus.ToString()));
                    Directory.Move(filePath + listAvailableFiles.Items[icount].ToString(), backupfilePath + TD[0].FileEntryName);
                    worker.ReportProgress(icount);
                    Thread.Sleep(200);
                    isPassed = true;
                    
                    

                }
            }
            catch (Exception ex)
            {
                logger.Log(LOGLEVELS.Error, "UploadToxmlCC", ex);
                isException = true;
            }

           
        }

        // This event handler updates the progress.
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            
            toolStripStatusLabel1.Text = "Uploading " + listAvailableFiles.Items[e.ProgressPercentage].ToString() +  " ...";
            Application.DoEvents();
        }

        // This event handler deals with the results of the background operation.
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnupload.Enabled = true;
            updateListView();

            if (e.Cancelled == true)
            {
                toolStripStatusLabel1.Text = "Canceled!";
            }
            else if (e.Error != null)
            {
                toolStripStatusLabel1.Text = "Error: " + e.Error.Message;
            }
            else if (isException || !isPassed)
            {
                toolStripStatusLabel1.Text = "Operation Failed";
            }
            else
            {
                toolStripStatusLabel1.Text = "Operation Executed";
            }
        }
    }
}
