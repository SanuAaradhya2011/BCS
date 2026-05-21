using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using CAB.BCS.DLMS.Utility;

namespace CAB.BCS.DLMS.Utility
{
    class CABFileOperation
    {
        #region  SaveDataToFile
        /// <summary>
        /// Created By : Vivek Agrawal
        /// Date : 25 Feb 2012
        /// Purpose: Save the down loaded data in file.
        /// </summary>
        /// <param name="completeDownLoadedData"></param>
        //private void SaveDataToFile(string completeDownLoadedData, string lngFileName, string meterID)
        //{//Create file Name.(Directory +FileName).
        //    String fileName = string.Concat(AppDomain.CurrentDomain.BaseDirectory, @"FDLFILES\");
        //    FileStream fileStream = null;
        //    StreamWriter streamWriter = null;
        //    try
        //    {
        //        //Stream filelng = File.OpenRead(lngFileName);

        //        //  StreamExtensions.CopyTo(filelng, fileStream);
        //        //filelng.Close();

        //        StreamReader streamCABFile = new StreamReader(lngFileName);
        //        string lngFileData = streamCABFile.ReadToEnd();
        //        streamCABFile.Close();
        //        completeDownLoadedData = lngFileData + "FDLDATA\\" + completeDownLoadedData;
        //        //Encrypt the data.
        //        //completeDownLoadedData = ConfigInfo.EncryptFile(completeDownLoadedData);

        //        //If directory  not exists then create it.
        //        if (!Directory.Exists(fileName))
        //            Directory.CreateDirectory(fileName);
        //        //Create file name.
        //        fileName = fileName + meterID + "_" + String.Format("{0:00}", DateTime.Now.Day) + "_" + String.Format("{0:00}", DateTime.Now.Month) + "_" + String.Format("{0:0000}", DateTime.Now.Year) + "_" + String.Format("{0:00}", DateTime.Now.Hour) + "_" + String.Format("{0:00}", DateTime.Now.Minute) + "_" + String.Format("{0:00}", DateTime.Now.Second) + ".FDL";
        //        fileStream = new FileStream(fileName, FileMode.Create);
        //        streamWriter = new StreamWriter(fileStream);
        //        streamWriter.Write(completeDownLoadedData);

        //        //Show the name of the file in which downloaded data is stored.
        //        MessageBox.Show(resourceMgr.GetString("DataSaveIn") + fileName, " BCS");
        //    }
        //    catch (IOException)
        //    {//if there is any exception while storing the data in file the show the message.
        //        MessageBox.Show(resourceMgr.GetString("FileCreationError"));
        //    }
        //    catch (System.Security.SecurityException)
        //    {
        //        MessageBox.Show(resourceMgr.GetString("Permissiondenied") + fileName);
        //    }
        //    finally
        //    {
        //        if (streamWriter != null) streamWriter.Close();
        //        if (fileStream != null) fileStream.Close();
        //        completeDownLoadedData = string.Empty;
        //        File.Delete(lngFileName);
        //    }
        //}
        #endregion

        public static string GetMD5ChecksumForFile(string filename)
        {
            if (filename == null)
                throw new ArgumentNullException(CoreUtility.GetMessageFromResourceFile("FileNameCanNotBeNull"));

            if (!File.Exists(filename))
                throw new ArgumentException(string.Format(CoreUtility.GetMessageFromResourceFile("FileNotExists"), filename));

            using (FileStream fstream = new FileStream(filename, FileMode.Open))
            {
                byte[] hash = new  System.Security.Cryptography.MD5CryptoServiceProvider().ComputeHash(fstream);
                StringBuilder sb = new StringBuilder(32);
                foreach (byte hex in hash)
                    sb.Append(hex.ToString("X2"));
                return sb.ToString().ToUpper();
            }
        }
    }
}
