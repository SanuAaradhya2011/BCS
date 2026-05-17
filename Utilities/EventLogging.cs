using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.ComponentModel;
using System.Security;
namespace Utilities
{
    public class EventLogging
    {
        static string sSource;
        static string sLog;
        static string sEvent;
        static int lineNumber = 0;
        static object t = "f";
        static string portName;
        /// <summary>
        /// This method is used for logging the event,exception or message into event viewer.
        /// </summary>
        /// <param name="Message">message to be logged</param>
        //public static void CallLogDetails(string Message)
        //{
        //    lock (t)
        //    {
        //        string message = Message;
        //        sSource = "DLMS GSM Communication";
        //        sLog = "Application";
        //        sEvent = Message;
        //        if (!EventLog.SourceExists(sSource))
        //            EventLog.CreateEventSource(sSource, sLog);
        //        EventLog.WriteEntry(sSource, sEvent, EventLogEntryType.Error, 1250);

        //        //CallLogDetailsIntoFile(message);
        //    }
        //}
        public static void CallLogDetails(string Message)
        {
            lock (t)
            {
                EventLog eventLog=new EventLog("Application", System.Environment.MachineName);
                //string message = Message;
                sSource = "DLMS GSM Communication";
                sLog = "Application";
                sEvent = Message;
                try
                {
                    if (!EventLog.SourceExists(sSource))
                    {
                        EventLog.CreateEventSource(sSource, sLog);
                    }
                    EventLog.WriteEntry(sSource, sEvent, EventLogEntryType.Error, 1250);
                    CallLogDetailsIntoFile(Message);
                }
                catch (SecurityException ex)
                {
                    // do nothing
                }
                catch (Win32Exception win32Exception)
                {                     
                     eventLog.Clear();
                     if (!EventLog.SourceExists(sSource))
                     {
                         EventLog.CreateEventSource(sSource, sLog);
                     }
                     EventLog.WriteEntry(sSource, "Exception occured while writing event log entry : " + win32Exception.Message, EventLogEntryType.Error, 1250);
                }
                catch (Exception ex)
                { 
                     eventLog.Clear();
                     if (!EventLog.SourceExists(sSource))
                     {
                         EventLog.CreateEventSource(sSource, sLog);
                     }
                    EventLog.WriteEntry(sSource, "Exception occured while writing event log entry : " + "Exception occured while writing event log entry : " + ex.Message, EventLogEntryType.Error, 1250);
               }
            }
        }
        /// <summary>
        /// This method is used for logging the event,exception or message into text file.
        /// </summary>
        /// <param name="Message">message to logged into file </param>      

        private static void CallLogDetailsIntoFile(string Message)
        {
            lineNumber++;
            string fileName = GetFileName();
            FileStream fileStream;
            try
            {
                if (File.Exists(fileName))
                {
                    fileStream = new FileStream(fileName, FileMode.Append, FileAccess.Write);
                }
                else
                {
                    fileStream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);
                }


                using (StreamWriter sw = new StreamWriter(fileStream))
                {
                    sw.WriteLine(lineNumber.ToString() + "." + Message + "....." + DateTime.Now);
                    sw.Close();
                    fileStream.Close();
                }
            }
            catch (Exception ex)
            { }

        }
        /// <summary>
        /// This method is used for getting filename for the logging.
        /// </summary>
        /// <returns></returns>
        private static string GetFileName()
        {
            string fileName = string.Concat(AppDomain.CurrentDomain.BaseDirectory, @"Logging\");
            if (!Directory.Exists(fileName))
            {
                Directory.CreateDirectory(fileName);
            }
            fileName = fileName + "logging.txt";

            return fileName;

        }
        /// <summary>
        /// This method is used for deleting the logging file if it exist.
        /// </summary>
        public static void fileDelete()
        {

            string fileName = GetFileName();
            File.Delete(fileName);
            lineNumber = 0;

        }
    }
}
