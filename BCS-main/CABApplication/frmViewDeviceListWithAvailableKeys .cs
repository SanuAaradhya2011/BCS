using Hunt.EPIC.Logging;
using CAB.Framework.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CABApplication
{
    public partial class frmViewDeviceListWithAvailableKeys : Form
    {
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(frmViewDeviceListWithAvailableKeys).ToString());
        public frmViewDeviceListWithAvailableKeys()
        {
            InitializeComponent();
        }

        private void FrmViewDeviceListWithAvailableKeys_Load(object sender, EventArgs e)
        {
            
                Application.DoEvents();
                Application.DoEvents();
                this.Cursor = Cursors.WaitCursor;
                listBoxAvailableMeters.Items.Clear();
                try
                {
                List<string> SecurityKeyDetails = Security_Key.SecurityKeyManager.GetSecurityKeys("", ConfigSettings.GetValue("PrivateKey"), AppDomain.CurrentDomain.BaseDirectory + "EndDeviceSecurityResponse.Xml");
                List<string> meterIDList = Security_Key.SecurityKeyManager.AvailableKeysMeterIDList.OrderBy(index => index).ToList();
                    foreach (string item in meterIDList)
                    {
                        listBoxAvailableMeters.Items.Add(item);
                    }
                }
                catch (Exception ex)
                {
                logger.Log(LOGLEVELS.Error, "Error in Loading Security Key File.", ex);
                MessageBox.Show(ex.Message, "BCS", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                finally
                {
                    this.Cursor = Cursors.Default;

                }

            
        }
    }
}
