using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAB.BCS.DLMS.Views
{
    /// <summary>
    /// This interface is used for the imlmenting CMRI as view from the UI.
    /// </summary>
    interface ICMRI
    {
        bool IsGeneral
        {
            get;
        }
        bool IsInstantaneous
        {
            get;
        }
        bool IsBilling
        {
            get;
        }
        bool IsLoadSurvey
        {
            get;
        }
        bool IsSelectAll
        {
            get;
        }
        bool IsMidNightEnergies
        {
            get;
        }
        bool IsEventLog
        {
            get;
        }
        System.Windows.Forms.CheckedListBox ListCMRI
        {
            get;
            set;
        }
        bool IsReadlast
        {
            get;           
        }
        bool IsReadBetweenBilling
        {
            get;            
        }
        bool IsReadBetweenLoadSurvey
        {
            get;
        }
        bool IsReadBetweenEventLog
        {
            get;
        }
        bool IsReadLastEventLog
        {
            get;
        }
        string BillingToDate
        {
            get;            
        }
        string BillingFromDate
        {
            get;           
        }
        string BillingLastFromDate
        {
            get;
        }

        string EventToDate
        {
            get;
        }
        string EventFromDate
        {
            get;
        }
        string EventLastFromDate
        {
            get;
        }

        DateTime LoadSurveyToDate
        {
            get;
        }
        DateTime LoadSurveyFromDate
        {
            get;
        }
        bool BtnReadAllCMRIEnabled
        {
            set;
        }
        bool BtnCMRICancelEnabled
        {
            set;
        }
        bool BtnLoadListEnabled
        {
            set;
        }
        bool BtnReadAllEnabled
        {
            set;
        }
        bool CheckCMRIBillingEnabled
        {
            set;
        }
        bool CheckCMRILoadSurveyEnabled
        {
            set;
        }
        bool CheckCMRITamperEnabled
        {
            set;
        }
        bool CheckCMRINameplateEnabled
        {
            set;
        }   
    }
}
