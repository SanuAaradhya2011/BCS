using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace DLMS_Final
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //dffbvghjhjghhghhhh
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new DLMSMain());
        }
    }
}
