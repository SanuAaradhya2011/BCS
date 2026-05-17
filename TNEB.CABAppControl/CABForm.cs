using System.Resources;
using System.Windows.Forms;
using System.Data;
using System.Threading;

namespace CAB.UI.Controls
{
    public partial class CABForm : Form
    {
        public delegate void IsStatusChanged(string msg);
        public event IsStatusChanged On_StatusChanged;

        public delegate void IsConnectionTypeChanged(string msg);
        public event IsConnectionTypeChanged On_ConnectionTypeChanged;

        public delegate void IsRightStatusChanged(string msg);
        public event IsRightStatusChanged On_RightStatusChanged;

        public delegate void IsListRefresh(bool flag);
        public event IsListRefresh OnListRefresh;

        private string translationKey;
        //for synchronizing between UI thread and meter reading thread
        private SynchronizationContext context = null;
        public CABForm()
        {
            context = SynchronizationContext.Current;
        }

        public string TranslationKey
        {
            get { return translationKey; }
            set
            {
                translationKey = value;
                if (string.IsNullOrEmpty(translationKey))
                    return;
                this.Text = MessageConstant.GetText(translationKey);
            }
        }
        private bool listRefresh;
        public bool ListRefresh
        {
            get
            {
                return listRefresh;
            }
            set
            {
                listRefresh = value;
                if (OnListRefresh != null)
                {
                    OnListRefresh(listRefresh);
                }
            }
        }
        /// <summary>
        /// Refresh list by fetching UI thread
        /// </summary>
        private bool listRefreshAsync;
        public bool ListRefreshAsync
        {
            get
            {
                return listRefreshAsync;
            }
            set
            {
                listRefreshAsync = value;
                SynchronizationContext context = SynchronizationContext.Current;
                context.Post(ChangeListRefreshAsync, listRefreshAsync);                
            }
        }     
        private string rightMessage;
        public string RightStatusMessage
        {
            get
            {
                return rightMessage;
            }
            set
            {
                rightMessage = value;
                if (On_RightStatusChanged != null)
                {
                    On_RightStatusChanged(rightMessage);
                }
            }
        }

        private string message;
        public string StatusMessage
        {
            get
            {
                return message;
            }
            set
            {
                message = value;
                if (On_StatusChanged != null)
                {
                    On_StatusChanged(message);
                }
            }
        }
        /// <summary>
        /// update the status message on UI thread
        /// </summary>
        private object messageAsync;
        public object StatusMessageAsync
        {
            get
            {
                return messageAsync;
            }
            set
            {
                messageAsync = value;               
                context.Post(ChangeStatusAsync, messageAsync);
                Application.DoEvents();
             
            }
        }
        /// <summary>
        /// Update the right status message on UI thread.
        /// </summary>
        private string rightMessageAsync;
        public string RightStatusMessageAsync
        {
            get
            {
                return rightMessageAsync;
            }
            set
            {
                rightMessageAsync = value;                
                context.Post(ChangeRightStatusMessageAsync, rightMessageAsync);
                Application.DoEvents();
                
            }
        }
       
        public string ConnectionTypeMessage
        {
            set
            {
                string message = "Connection: " + value;
                if (On_ConnectionTypeChanged != null)
                {
                    On_ConnectionTypeChanged(message);
                }
            }
        }
        public virtual DataSet GetSearchData()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add(new DataColumn("DisplayMember", typeof(System.String)));
            dataTable.Columns.Add(new DataColumn("ValueMember", typeof(System.String)));
            AddNewRow(dataTable, "All", "-");
            AddNewRow(dataTable, "User Name", "TEXT");
            AddNewRow(dataTable, "Login ID", "TEXT");
            AddNewRow(dataTable, "Category", "Category_Master|Category_Name|Category_ID");
            AddNewRow(dataTable, "Designation", "Designation_Master|Designation_Name|Designation_ID");
            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(dataTable);
            return dataSet;
        }

        public virtual void AddNewRow(DataTable dataTable, string displayMember, string valueMember)
        {
            DataRow dataRow = dataTable.NewRow();
            dataRow[0] = displayMember;
            dataRow[1] = valueMember;
            dataTable.Rows.Add(dataRow);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // CABForm
            // 
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.ControlBox = false;
            this.Name = "CABForm";
            this.Activated += new System.EventHandler(this.CABForm_Activated);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CABForm_FormClosed);
            this.ResumeLayout(false);

        }

        private void CABForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.StatusMessage = string.Empty;
            this.RightStatusMessage = string.Empty;
        }

        private void CABForm_Activated(object sender, System.EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="listRefresh"></param>
        private void ChangeListRefreshAsync(object listRefresh)
        {
            if (OnListRefresh != null)
            {
                OnListRefresh((bool)listRefresh);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        private void ChangeStatusAsync(object state)
        {
            if (On_StatusChanged != null)
            {
                On_StatusChanged(state.ToString());
                Application.DoEvents();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        private void ChangeRightStatusMessageAsync(object state)
        {
            if (On_RightStatusChanged != null)
            {
                On_RightStatusChanged(state.ToString());
                Application.DoEvents();
            }
        }

    }
}
