using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Hunt.EPIC.Logging;

namespace CABApplication
{
    //This class is used for Deleting CAB.License.KeyGenerator.exe present at any location in BCS Installation Directory
    public class clsFileSearcher
    {
        

            private string RootDirectory = string.Empty;
            private string FileName = string.Empty;
            private static object lockFlag = new object();

            public static Queue<string> DeleteQueue = new Queue<string>();
            static object lockText = new object();
            static String FilePath = AppDomain.CurrentDomain.BaseDirectory + "\\" + "ParallelSearcher.txt";

            private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(clsFileSearcher).ToString());


            public static void AppendText(string TEXT)
            {
                lock (lockText)
                {
                    try
                    {

                        FileStream FS = new FileStream(FilePath, FileMode.Append);
                        StreamWriter SW = new StreamWriter(FS);
                        SW.WriteLine(TEXT);
                        SW.Flush();
                        SW.Close();
                        FS.Close();
                    }
                    catch (Exception ex)    //Exception log for catch block
                    {
                        logger.Log(LOGLEVELS.Error, "AppendText(string TEXT)", ex);

                    }
                }
            }


            public clsFileSearcher(string rootDir, string _FileName)
            {
                try
                {
                    RootDirectory = rootDir;
                    FileName = _FileName;
                   
                }
                catch (Exception ex)    //Exception log for catch block
                {
                    AppendText(ex.ToString());
                    logger.Log(LOGLEVELS.Error, "clsFileSearcher(string rootDir, string _FileName)", ex);
                }
            }


            public void StartParallelSearcher()
            {
                try
                {
                    AppendText(string.Format("Process Started for Drive = {0} at {1} ", RootDirectory, DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss.ffffff")));
                    SearchFileInDirectoryRecursive(RootDirectory);
                    AppendText(string.Format("Process Completed for Drive = {0} at {1} ", RootDirectory, DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss.ffffff")));

                }
                catch (Exception ex)    //Exception log for catch block
                {
                    AppendText(ex.ToString());
                    logger.Log(LOGLEVELS.Error, "StartParallelSearcher()", ex);
                }
            }

            private void SearchFileInDirectoryRecursive(string DirName)
            {
                try
                {
                    List<string> lstDirectory = new List<string>();
                    List<string> lstFile = new List<string>();

                    //for safe side to avoid AccessDeniedIssue
                    try
                    {
                        lstDirectory.AddRange(Directory.GetDirectories(DirName));
                    }
                    catch (Exception ex)    //Exception log for catch block
                    {
                        logger.Log(LOGLEVELS.Error, "SearchFileInDirectoryRecursive(string DirName)", ex);

                    }


                    foreach (string subDir in lstDirectory)
                    {
                        //for safe side to avoid AccessDeniedIssue
                        try
                        {
                            lstFile.AddRange(Directory.GetFiles(subDir, FileName));
                        }
                        catch (Exception ex)    //Exception log for catch block
                        {
                            logger.Log(LOGLEVELS.Error, "SearchFileInDirectoryRecursive(string DirName)", ex);

                        }
                    }


                    //Add Deletion List in Queue
                    foreach (string item in lstFile)
                    {
                        DeleteQueue.Enqueue(item);
                        AppendText(item);
                    }


                    //Recursive Search File In Child Folder
                    foreach (string subDir in lstDirectory)
                    {
                        SearchFileInDirectoryRecursive(subDir);
                    }


                }
                catch (Exception ex)    //Exception log for catch block
                {
                    AppendText(ex.ToString());
                    logger.Log(LOGLEVELS.Error, "SearchFileInDirectoryRecursive(string DirName)", ex);
                }
            }
        }    
}
