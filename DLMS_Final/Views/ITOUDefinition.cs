using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;  
namespace CAB.BCS.DLMS.Views
{
    /// <summary>
    /// This interface is used for the imlmenting TOU as view from the UI.
    /// </summary>
    interface ITOUDefinition
    {
        byte Attribute
        {
            get;
            set;
        }
        byte[] BlockBuffer
        {
            get;            
        }
        /// <summary>
        /// Future activation date of the TOU.
        /// </summary>
        DateTime FutureActivationDate
        {
            get;
            set;
        }
        /// <summary>
        /// This label for showing the error message.
        /// </summary>
        Label LblMonth
        {
            get;
            set;
        }
        /// <summary>
        /// This datagridview array contains the day tables array from the UI.
        /// </summary>

        DataGridView[] TOUGridNames
        {
            get;          
        }

        /// <summary>
        /// This properties used for getting the datagrid object on the runtime from the view..
        /// </summary>
        DataGridView CurrentClickedGrid
        {
            get;
            set;
        }
        /// <summary>
        /// This is used for week profile grid object getting from the view.
        /// </summary>
        DataGridView GridActivationDate
        {
            get;            
        }
        /// <summary>
        /// This is used for session profile grid object getting from the view.
        /// </summary>
        DataGridView GridDayTables
        {
            get;           
        }
        /// <summary>
        /// This properties used for getting the datagrid object on the runtime from the view..
        /// </summary>
        object DataGridViewSenderObject
        {
            get;
            set;
        }
        /// <summary>
        /// This properties used for getting the datagrid object event on the runtime from the view..
        /// </summary>
        DataGridViewCellValidatingEventArgs EventArgs
        {
            set;
            get;
        }

    }
}
