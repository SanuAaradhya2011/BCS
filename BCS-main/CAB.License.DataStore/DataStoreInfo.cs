using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAB.License.DataStore
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.3082")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://tempuri.org/")]
    public class DataStoreInfo
    {

        private string infoUniqueClientID;
        private DateTime infoStartDateTime;
        private int infoNumberOfDaysElapsed;
        private string infolicenseFileName;
        private DateTime infoLastRunDate;
        public DataStoreInfo()
        { 
        
        }
        public DataStoreInfo(string uniqueClientID,DateTime startDateTime,DateTime lastRunDate)
        {
            this.infoUniqueClientID = uniqueClientID;
            this.infoStartDateTime = startDateTime;
            this.infoLastRunDate = lastRunDate;
            //this.infoNumberOfDaysElapsed = numberOfDaysElapsed;
        }
        public DateTime LastRunDate
        {
            get
            {
                return infoLastRunDate;
            }
            set
            {
                infoLastRunDate = value;
            }
        }
        public DateTime StartDateTime
        {
            get
            {
                return infoStartDateTime;
            }
            set
            {
                infoStartDateTime = value;
            }
        }

        

        public string UniqueClientId
        {
            get
            {
                return infoUniqueClientID;
            }
            set
            {
                infoUniqueClientID = value;
            }
        }

        public int NumberOfDaysElapsed
        {
            get
            {
                return infoNumberOfDaysElapsed;
            }
            set
            {
                infoNumberOfDaysElapsed = value;
            }

        }
        public string LicenseFileName
        {
            get
            {
                return infolicenseFileName;
            }
            set
            {
                infolicenseFileName = value;
            }
        }
    }
}
