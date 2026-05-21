using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using CAB.UI.Controls; 
using System.Management;
using CAB.IECFramework.Utility;

namespace CAB.UI
{
    public partial class RegisterProduct : Form
    {
        ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BaseBoard"); 
        MyCrypro objCrypto = new MyCrypro();

        public RegisterProduct()
        {
            InitializeComponent();
        }

        private void RegisterProduct_Load(object sender, EventArgs e)
        {
            btnproback.Enabled = false;
            txtcompantname.Focus();
        }

        private void CreateEULA()
        {
            try
            {
                foreach (ManagementObject wmi in searcher.Get())
                {
                    string SysInfo = Convert.ToString( wmi.GetPropertyValue("SerialNumber")).Trim();
                    SysInfo +=Convert.ToString( wmi.GetPropertyValue("Product")).Trim();

                    if (objCrypto.GenerateKey(SysInfo) != txt_ProductCode.Text)
                    {
                        MessageBox.Show("Invalid Product Key!", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    else
                    {
                        Random r = new Random();
                        string randnumber = "";
                        while (randnumber.Length < 200) randnumber += Convert.ToString(r.Next());
                        SysInfo = randnumber + "STARTS" + txt_ProductCode.Text + "ENDS" + randnumber;
                        randnumber = "";
                        while (randnumber.Length < 200) randnumber += Convert.ToString(r.Next());
                        SysInfo += "STARTP" + txtcompantname.Text + "ENDP" + randnumber;
                        randnumber = "";
                        while (randnumber.Length < 200) randnumber += Convert.ToString(r.Next());
                        string bcc = objCrypto.CalFileBcc(SysInfo);
                        SysInfo = objCrypto.EncryptString(SysInfo + bcc);

                        FileStream file1 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "EulaCfg.LTB", FileMode.Create);
                        StreamWriter wr1 = new StreamWriter(file1);
                        wr1.Write(SysInfo);
                        wr1.Close();
                        file1.Close();
                        MessageBox.Show("Registration succesful, Please restart your application ", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Application.Exit();
                    }
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString(), "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }
 

        public bool IsAlphaNumericchar(string strToCheck)
        {
            Regex objAlphaNumericPattern = new Regex("[^a-zA-Z0-9.{\\s}]");

            return !objAlphaNumericPattern.IsMatch(strToCheck);
        } 
       
        public bool IsAlphaNumeric(string strToCheck)
        {
           Regex objAlphaPattern = new Regex("[^a-zA-Z0-9]");

           return !objAlphaPattern.IsMatch(strToCheck);
        }

        private void btnproback_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtcompantname.Text.Trim() == "")
                {

                    MessageBox.Show("Company Name Cannot Be Blank", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtcompantname.Focus();
                    return;
                }

                if (!IsAlphaNumericchar(txtcompantname.Text))
                {

                    MessageBox.Show("Please Enter Valid Company Name", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtcompantname.Focus();
                    return;
                }
                if (txt_ProductCode.Text.Trim() == "")
                {

                    MessageBox.Show("Product Key Cannot Be Blank", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txt_ProductCode.Focus();
                    return;
                }
                string chkalphanum = txt_ProductCode.Text.Replace("-","");
                if (!IsAlphaNumericchar(chkalphanum))
                {

                    MessageBox.Show("Invalid Product Key!", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    txt_ProductCode.Focus();
                    return;
                }
                CreateEULA();
            }
            catch (Exception Ex)
            {
                MessageBox.Show("BCS Error", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Stop);

            }
        }

        private void btnproclose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnprofinish_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (ManagementObject wmi in searcher.Get())
                {

                    Random r = new Random();
                    string randnumber = "";
                    while (randnumber.Length < 200) randnumber += r.Next().ToString();
                    string SysInfo = randnumber + "STARTS" + Convert.ToString(wmi.GetPropertyValue("SerialNumber")) .Trim() + "ENDS" + randnumber;
                    randnumber = "";
                    while (randnumber.Length < 200) randnumber += Convert.ToString(r.Next());
                    SysInfo += "STARTP" + Convert.ToString(wmi.GetPropertyValue("Product")).Trim() + "ENDP" + randnumber;
                    randnumber = "";
                    while (randnumber.Length < 200) randnumber +=Convert.ToString( r.Next());
                    string bcc = objCrypto.CalFileBcc(SysInfo);
                    SysInfo = objCrypto.EncryptString(SysInfo + bcc);


                    SaveFileDialog savefile = new SaveFileDialog();
                    savefile.Filter = "Text files (*.txt)|*.txt";
                    savefile.RestoreDirectory = true;
                    if (savefile.ShowDialog() == DialogResult.OK)
                    {

                        FileStream file1 = new FileStream(savefile.FileName, FileMode.Create);
                        StreamWriter wr1 = new StreamWriter(file1);
                        wr1.Write(SysInfo);
                        wr1.Close();
                        file1.Close();
                        MessageBox.Show("For Product Key Send This File To Your Vendor.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                } 
            }
            catch (Exception Ex)
            {
                MessageBox.Show("BCS Error", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Stop);

            }
        }

        private void txt_ProductCode_TextChanged(object sender, EventArgs e)
        {
            if (txtcompantname.Text != "")
            {
                if (txt_ProductCode.Text.Length != 0)
                {
                    btnproback.Enabled = true;
                }
                else
                {
                    btnproback.Enabled = false;
                }
            }
        }

        private void txtcompantname_TextChanged(object sender, EventArgs e)
        {
            if (txt_ProductCode.Text != "")
            {
                if (txtcompantname.Text.Length != 0)
                {
                    btnproback.Enabled = true;
                }
                else
                {
                    btnproback.Enabled = false;
                }
            }
        }
    }
}
