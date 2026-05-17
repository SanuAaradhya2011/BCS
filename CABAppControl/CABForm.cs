using System.Data;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;
namespace CAB.UI.Controls
{
    public partial class CABForm : Form
    {
        public delegate void IsStatusChanged(string msg);
        public delegate void IsConnectionDetailStatusChanged(string msg);
        public event IsStatusChanged On_StatusChanged;
        public event IsConnectionDetailStatusChanged On_ConnectionDetailStatusChanged;

        public delegate void IsIconChanged(bool IconChanged);
        public event IsIconChanged On_IconChanged;
        private bool icon;
        public bool ChangeIcon
        {
            get
            {
                return icon;
            }
            set
            {
                icon = value;
                if (On_IconChanged != null)
                {
                    On_IconChanged(icon);
                }
            }
        }
        public delegate void IsRightStatusChanged(string msg);
        public event IsRightStatusChanged On_RightStatusChanged;

        public delegate void IsListRefresh(bool flag);
        public event IsListRefresh OnListRefresh;
        private SynchronizationContext context = null;                          
        private string translationKey;
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
        /// <summary>
        /// for getting and setting the message for status message UI thread
        /// </summary>
        private string connectionDetailStatusMessage;
        public string ConnectionDetailStatusMessage
        {
            get
            {
                return connectionDetailStatusMessage;
            }
            set
            {
                connectionDetailStatusMessage = value;
                if (On_ConnectionDetailStatusChanged != null)
                {
                    On_ConnectionDetailStatusChanged(connectionDetailStatusMessage);
                }
            }
        }
        /// <summary>
        /// Update the right status message on UI thread.
        /// </summary>
        private string connectionDetailStatusMessageAsync;
        public string ConnectionDetailStatusMessageAsync
        {
            get
            {
                return connectionDetailStatusMessageAsync;
            }
            set
            {
                connectionDetailStatusMessageAsync = value;
                context.Post(ChangeConnectionDetailStatusMessageAsync, connectionDetailStatusMessageAsync);
                Application.DoEvents();

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

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            AttachGridHoverEvents(this);
        }

        private void AttachGridHoverEvents(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                if (control is DataGridView dgv)
                {
                    dgv.CellMouseEnter -= Dgv_CellMouseEnter;
                    dgv.CellMouseLeave -= Dgv_CellMouseLeave;
                    dgv.CellMouseEnter += Dgv_CellMouseEnter;
                    dgv.CellMouseLeave += Dgv_CellMouseLeave;
                }
                else if (control.HasChildren)
                {
                    AttachGridHoverEvents(control);
                }
            }
        }

        private void Dgv_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && sender is DataGridView dgv)
            {
                dgv.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(220, 235, 255);
            }
        }

        private void Dgv_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && sender is DataGridView dgv)
            {
                dgv.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Empty;
            }
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
        public virtual bool Upload2NGFile(string fileName, string fileText, bool flag) { return false; }
        public virtual string GetContent(string filePath) { return string.Empty; }
        private string meterDataID;
        public string MeterDataID
        {
            get
            {
                return meterDataID;
            }
            set
            {
                meterDataID = value;
            }
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

        /// <summary>
        /// changes the status message for status bar in right side.
        /// </summary>
        /// <param name="state"></param>
        private void ChangeConnectionDetailStatusMessageAsync(object state)
        {
            if (On_ConnectionDetailStatusChanged != null)
            {
                On_ConnectionDetailStatusChanged(state.ToString());
                Application.DoEvents();
            }
        }
       
    }
}
