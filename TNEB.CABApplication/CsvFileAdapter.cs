using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO; 
namespace CABApplication
{
     /// <summary>
    /// Class to store one CSV row
    /// </summary>
    public class CsvRow : List<string>
    {
        public string LineText { get; set; }
    }

    /// <summary>
    /// Class to read data from a CSV file
    /// </summary>
    public class CsvFileReader
    {
        StreamReader sw;
        public CsvFileReader(StreamReader strmReader)
        {
            sw = strmReader;
        }
        /// <summary>
        /// Read pending files to be imported which failed in previous uploading.
        /// </summary>
        /// <returns>string</returns>
        public static string ReadPendingFiles()
        {
            string filName = System.Windows.Forms.Application.StartupPath + "\\" + "Log" + "\\" + "FileStatus.csv";
            int fileCount = 0;
            try
            {
                if (File.Exists(filName))
                {
                    StreamReader sw = new StreamReader(filName);
                    CsvFileReader csvFileReader = new CsvFileReader(sw);
                    CsvRow row = new CsvRow();
                    string statusData = string.Empty;
                    while (csvFileReader.ReadRow(row))
                    {
                        if (row[3].Equals("2") || row[3].Equals("3") || row[3].Equals("4"))
                        {
                            fileCount++;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (fileCount == 0)
                return string.Empty;
            return "Please click here to import "+fileCount+" pending file(s).";
        }
        /// <summary>
        /// Read the row of the csv file.
        /// </summary>
        /// <param name="row"></param>
        /// <returns>true/false</returns>
        public bool ReadRow(CsvRow row)
        {
            if (sw == null)
                return false;
            try
            {
                row.LineText = sw.ReadLine();
                if (String.IsNullOrEmpty(row.LineText))
                    return false;

                int pos = 0;
                int rows = 0;

                while (pos < row.LineText.Length)
                {
                    string value;

                    // Special handling for quoted field
                    if (row.LineText[pos] == '"')
                    {
                        // Skip initial quote
                        pos++;

                        // Parse quoted value
                        int start = pos;
                        while (pos < row.LineText.Length)
                        {
                            // Test for quote character
                            if (row.LineText[pos] == '"')
                            {
                                // Found one
                                pos++;

                                // If two quotes together, keep one
                                // Otherwise, indicates end of value
                                if (pos >= row.LineText.Length || row.LineText[pos] != '"')
                                {
                                    pos--;
                                    break;
                                }
                            }
                            pos++;
                        }
                        value = row.LineText.Substring(start, pos - start);
                        value = value.Replace("\"\"", "\"");
                    }
                    else
                    {
                        // Parse unquoted value
                        int start = pos;
                        while (pos < row.LineText.Length && row.LineText[pos] != ',')
                            pos++;
                        value = row.LineText.Substring(start, pos - start);
                    }

                    // Add field to list
                    if (rows < row.Count)
                        row[rows] = value;
                    else
                        row.Add(value);
                    rows++;

                    // Eat up to and including next comma
                    while (pos < row.LineText.Length && row.LineText[pos] != ',')
                        pos++;
                    if (pos < row.LineText.Length)
                        pos++;
                }
                // Delete any unused items
                while (row.Count > rows)
                    row.RemoveAt(rows);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            // Return true if any columns read           
            return (row.Count>0) ;
        }
    }

    /// <summary>
    /// This class is used for writing the content into the CSV file.
    /// </summary>
    class CsvFileWriter
    {
       
        string file; 
        public CsvFileWriter(string filename)
        {
            file = filename; 
        }

        /// <summary>
        /// Writes a single row to a CSV file.
        /// </summary>
        /// <param name="row">The row to be written</param>
        public void WriteRow(CsvRow row)
        {
            StringBuilder builder = new StringBuilder();
            bool firstColumn = true;
            try
            {
                foreach (string value in row)
                {
                    // Add separator if this isn't the first value
                    if (!firstColumn)
                        builder.Append(',');
                    // Implement special handling for values that contain comma or quote
                    // Enclose in quotes and double up any double quotes
                    if (value.IndexOfAny(new char[] { '"', ',' }) != -1)
                        builder.AppendFormat("\"{0}\"", value.Replace("\"", "\"\""));
                    else
                        builder.Append(value);
                    firstColumn = false;
                }
                row.LineText = builder.ToString();
                FileDetailsIntoFile(row.LineText, file);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Create or update csv file.
        /// </summary>
        /// <param name="Message">string message to be write</param>
        /// <param name="file">file name</param>
        private  void FileDetailsIntoFile(string Message,string file)
        {
            FileStream fileStream = null;
            string directoryPath = System.Windows.Forms.Application.StartupPath + "\\" + "Log";
            string firsRow = string.Empty;
            try
            {
                if (File.Exists(file))
                {
                    fileStream = new FileStream(file, FileMode.Append, FileAccess.Write);

                }
                else
                {
                    if (!System.IO.Directory.Exists(directoryPath))
                    {
                        System.IO.Directory.CreateDirectory(directoryPath); 
                    }
                    fileStream = new FileStream(file, FileMode.OpenOrCreate, FileAccess.Write);
                    firsRow = "FileUpload_ID,UploadingDateTime,FileName,Status";
                }
                using (StreamWriter sw = new StreamWriter(fileStream))
                {
                    if (firsRow != string.Empty)
                        sw.WriteLine(firsRow);
                    sw.WriteLine(Message);
                    sw.Close();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                fileStream.Close();
            }
        }

    }
}


