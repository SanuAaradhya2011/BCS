using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CAB.Framework;
using CAB.BLL;
namespace CAB.BCS.DLMS.Utility
{
    internal static class CoreUtility
    {
        #region Enums
        public enum DLMSResultType
        {
            Success,
            Fail,
            AccessDenied,
            DataUnavailable,
            TimeOut,
            SignOnFailed,
            CosemConnectionFailed,
            MeterIDMismatch
        }
        #endregion

        #region Variables
        static ToolStripProgressBar objectToolStripProgressBar;
        static Cursor objectFromCursor;
        static string expMessage;
        static bool isPUMA = false;
        static bool isMVVNL = false;
        static bool isMPKWCL = false;
        #endregion

        #region Constants
        public const string BCS = "BCS";
        #endregion

        #region Public Properties
        public static Cursor ObjectFormCursor
        {
            get { return CoreUtility.objectFromCursor; }
            set { CoreUtility.objectFromCursor = value; }
        }
        public static ToolStripProgressBar ObjectToolStripProgressBar
        {
            get
            {
                return objectToolStripProgressBar;
            }
            set
            {
                objectToolStripProgressBar = value;
            }

        }


        public static string ExpMessage
        {
            get
            {
                return expMessage;
            }
            set
            {
                expMessage = value;
            }

        }


        public static bool IsPUMA
        {
            get
            {
                if (UtilityEntity.PUMA == UtilityDetails.Utility)
                {
                    isPUMA = true;
                }
                return isPUMA;
            }
        }

        public static bool IsMVVNL
        {
            get
            {
                if (UtilityEntity.MVVNL == UtilityDetails.Utility)
                {
                    isMVVNL = true;
                }
                return isMVVNL;
            }
        }
        public static bool IsMPKWCL
        {
            get
            {
                if (UtilityEntity.MPKWCL == UtilityDetails.Utility)
                {
                    isMPKWCL = true;
                }
                return isMPKWCL;

            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// This method is used for getting message from the resourse file from the UtilityMessage.resx
        /// </summary>
        /// <param name="message">Pas the key to get message</param>
        /// <returns>string</returns>
        public static string GetMessageFromResourceFile(string key)
        {
            System.Resources.ResourceManager resourceMgr = new System.Resources.ResourceManager("DLMS_Final.Utility.UtilityMessage", System.Reflection.Assembly.GetExecutingAssembly());
            return resourceMgr.GetString(key, new System.Globalization.CultureInfo("en-us"));
        }
        public static string GetMessageFromResourceFile(string resourceFileName, string key)
        {
            System.Resources.ResourceManager resourceMgr = new System.Resources.ResourceManager(resourceFileName, System.Reflection.Assembly.GetExecutingAssembly());
            return resourceMgr.GetString(key, new System.Globalization.CultureInfo("en-us"));
        }
        /// <summary>
        /// This method is used for getting string value from the enum.
        /// </summary>
        /// <param name="enumVlaue">Pass the enum value.</param>
        /// <returns>string</returns>
        public static string GetMessageFromEnum(int enumVlaue)
        {
            string message = string.Empty;
            switch (enumVlaue)
            {
                case 0:
                    message = EnumUtil.stringValueOf(DLMSResultType.Success);
                    break;
                case 1:
                    message = EnumUtil.stringValueOf(DLMSResultType.Fail);
                    break;
                case 2:
                    message = EnumUtil.stringValueOf(DLMSResultType.AccessDenied);
                    break;
                case 3:
                    message = EnumUtil.stringValueOf(DLMSResultType.DataUnavailable);
                    break;
                case 4:
                    message = EnumUtil.stringValueOf(DLMSResultType.TimeOut);
                    break;
                case 5:
                    message = EnumUtil.stringValueOf(DLMSResultType.SignOnFailed);
                    break;
                case 6:
                    message = EnumUtil.stringValueOf(DLMSResultType.CosemConnectionFailed);
                    break;
                case 7:
                    message = EnumUtil.stringValueOf(DLMSResultType.MeterIDMismatch);
                    break;
                default:
                    break;
            }


            return message;

        }
        public static void StartTimer()
        {
            ObjectFormCursor = Cursors.WaitCursor;  
            ObjectToolStripProgressBar.Visible = true;
        }

        public static void StopTimer()
        {
            ObjectFormCursor = Cursors.Default;
            ObjectToolStripProgressBar.Visible = false;
        }

        /// <summary>
        /// This method is used to get increment timer of the view.
        /// </summary>
        /// <param name="objProgressBar"></param>
        public static void GetIncrementedTimer()
        {
            if (ObjectToolStripProgressBar.Value > 99)
            {
                ObjectToolStripProgressBar.Value = 0;
            }
            else
            {
                ObjectToolStripProgressBar.Value = ObjectToolStripProgressBar.Value + 10;
            }
        }
        #endregion
    }
}
