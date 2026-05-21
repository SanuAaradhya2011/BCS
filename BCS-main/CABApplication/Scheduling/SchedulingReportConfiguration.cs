using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CAB.Framework;
using CAB.BLL;
using Hunt.EPIC.Logging;

namespace CABApplication.Scheduling
{
    public partial class SchedulingReportConfiguration : Form
    {
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(SchedulingReportConfiguration).ToString());
        public SchedulingReportConfiguration()
        {
          InitializeComponent();
          
           // Iterating through profile and creating tab and adding tab at runtime 
          foreach(string name in Enum.GetNames(typeof(Profile)))
          {
              try
              {
                  TabPage tabPage = new TabPage(((Profile)Enum.Parse(typeof(Profile), name)).GetDisplayName());
                  tabPage.Name = "tab" + name;
                  tabProfiles.TabPages.Add(tabPage);
                  CABAppControl.MultiSelect multiselect = new CABAppControl.MultiSelect(ReportBLL.GetSchedulingReportColumns(name.ToUpper(), UtilityDetails.GetUtilityDetails().ToString()));
                  tabPage.Controls.Add(multiselect);
              }
              catch (Exception ex)    //Exception log for catch block 
              {
                  logger.Log(LOGLEVELS.Error, "SchedulingReportConfiguration()", ex);
              }
          }

          // by default first tab i.e General will be selected 
          tabProfiles.SelectedIndex = 0;
                 
        }
      
    }
}
