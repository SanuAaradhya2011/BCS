using System;
using System.IO;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        string path = @"C:\Users\PIYUSH SINGH\Downloads\3PhaseBCS\3PhaseBCS\CABApplication\ApplicationLogDetails.cs";
        string content = File.ReadAllText(path);

        if (!content.Contains("MessageBox.Show"))
        {
            content = content.Replace("SetupTimelineGrid();", "SetupTimelineGrid();\r\n            MessageBox.Show(\"Constructor hit! CABApplication\");");
            File.WriteAllText(path, content);
            Console.WriteLine("MessageBox injected.");
        }
    }
}
