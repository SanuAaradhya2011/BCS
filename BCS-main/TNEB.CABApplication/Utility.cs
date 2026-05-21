using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LTCTBLL;
using CABEntity;
using CAB.UI;
using System.Data;

namespace CABApplication
{
    public partial class Utility : Form
    {
        private string UtilityName;
        public bool utilityValidated = false;
        public string UtilityName1
        {
            get 
            {
                
                DataSet ds = new DataSet();
                ds= new UtilityBLL().FetchUtilityPassword();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    UtilityName = ds.Tables[0].Rows[0][0].ToString();
                   
                }
                return UtilityName;
            }

        }
        
        public string Utility_Password;
        public UtilityPassword UtilityPassword1
        {
            get;
            set;
        }
        public enum UtilityPassword
        {
            [DescriptionAttribute("7GD7WpNp")]
            TNEB,
            [DescriptionAttribute("m7UboMvP")]
            UGVCL,
            [DescriptionAttribute("7GD8WpNp")]
            TNEB1,
            [DescriptionAttribute("q6goJ8s9")]
            PVVNL,
            [DescriptionAttribute("23rtYuw1")]
            WBEXPORTVCL,
            [DescriptionAttribute("87ftBpm1")]
            JDVVNL
        }
        public Utility()
        {
            InitializeComponent();
        }

        private void Utility_Load(object sender, EventArgs e)
        {
            
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            UtilityName = txtUtilityName.Text;
            Utility_Password = txtUtilityPassword.Text;
            if(UtilityName == "" || Utility_Password == "")
            {
                MessageBox.Show("Please enter all details.");
                return;
            }

            if (Utility_Password != EnumUtil.StringValue(UtilityPassword.JDVVNL) && Utility_Password != EnumUtil.StringValue(UtilityPassword.TNEB) && Utility_Password != EnumUtil.StringValue(UtilityPassword.UGVCL)
                 && Utility_Password != EnumUtil.StringValue(UtilityPassword.PVVNL) && Utility_Password != EnumUtil.StringValue(UtilityPassword.TNEB1) && Utility_Password != EnumUtil.StringValue(UtilityPassword.WBEXPORTVCL))
             {
                    MessageBox.Show("Unauthorized Access");
                    utilityValidated = false;
                    return;
             }
             else
             {
                    InsertData(Utility_Password, UtilityName);
                    utilityValidated = true;
                    MessageBox.Show("Data saved successfully");
              }
            this.Close();
           
        }
        public void InsertData(string Utility_Password, string UtilityName)
        {
            new UtilityBLL().InsertData(Utility_Password, UtilityName);
        }

        private void Utility_FormClosed(object sender, FormClosedEventArgs e)
        {
           
        }

       
        

    }
}
