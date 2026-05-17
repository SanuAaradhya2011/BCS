using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using Microsoft.Win32;
using System.Windows.Forms;
using Utilities;
using CAB.License.DataStore;
using System.Security.Principal;
using System.Configuration;
using System.Xml;
namespace CABApplication
{
    [RunInstaller(true)]
    public partial class CABApplicationInstaller : Installer
    {
        public CABApplicationInstaller()
        {
            InitializeComponent();
        }
        public bool IsUserAdministrator()
        {
            //bool value to hold our return value
            bool isAdmin;
            try
            {
                //get the currently logged in user
                WindowsIdentity user = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(user);
                isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch (UnauthorizedAccessException ex)
            {
                isAdmin = false;
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                isAdmin = false;
                MessageBox.Show(ex.Message);
            }
            return isAdmin;
        }


        public override void Install(IDictionary stateSaver)
        {
            // if is not admin

            base.Install(stateSaver);
            if (!IsUserAdministrator())
            {
                Rollback(stateSaver);
            }
            try
            {
                const string keyPath = "SOFTWARE\\Cabcon\\AppData";
                const string keyValueName = "CLSID";
                const string productCode = "ProductCode";
                const string clientInfo = "AssemblyInfo";
                const string CANTBEINSTALLED = "Set up can not be installed ";
                const string USERDONTHAVEADMINRIGTHS = "Most probable cause:User doesn't have administrative rigths.";
                string versionName = string.Empty;

                try
                {
                    RegistryKey key = Registry.LocalMachine.OpenSubKey(keyPath, RegistryKeyPermissionCheck.ReadWriteSubTree);
                    string tgtDIR = Context.Parameters[productCode];

                    if (key == null)
                    {
                        key = Registry.LocalMachine.CreateSubKey(keyPath, RegistryKeyPermissionCheck.ReadWriteSubTree);
                        key.SetValue(keyValueName, tgtDIR);
                        key.SetValue(clientInfo, string.Empty);

                    }
                    //SetProductName();
                }
                catch (Exception ex)
                {

                    // Rollback Install
                    throw new InstallException(CANTBEINSTALLED + Environment.NewLine + USERDONTHAVEADMINRIGTHS);
                }

            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public void SetProductName()
        {
            try
            {
                const string VERSIONFILENAME = "Version.xml";
                const string PATH = "PATH";
                const string NODENAME = "Versions/Desktop";
                const string productName = "ProductName";
                string versionName = string.Empty;
                versionName = Context.Parameters[productName];
                string path = Context.Parameters[PATH];

                string newValue = string.Empty;
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(path + VERSIONFILENAME);
                XmlNode node = xmlDoc.SelectSingleNode(NODENAME);
                node.InnerText = versionName;
                xmlDoc.Save(path + VERSIONFILENAME);
            }
            catch (Exception ex)
            {
                //
            }
        }
        public override void Uninstall(IDictionary savedState)
        {
            base.Uninstall(savedState);
        }
        protected override void OnAfterInstall(IDictionary savedState)
        {
            base.OnAfterInstall(savedState);
        }
        public override void Commit(IDictionary savedState)
        {
            base.Commit(savedState);
        }
        public override void Rollback(IDictionary savedState)
        {
            base.Rollback(savedState);
        }
    }
}
