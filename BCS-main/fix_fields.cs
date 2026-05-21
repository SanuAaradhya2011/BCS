using System;
using System.IO;

class Program
{
    static void Main()
    {
        string path = @"C:\Users\PIYUSH SINGH\Downloads\3PhaseBCS\3PhaseBCS\CABApplication\Manage Area\AreaMaster.cs";
        string content = File.ReadAllText(path);
        
        string declarations = @"        private CircleBLL circleBLL = new CircleBLL();
        private AreaMeterBLL areaMeterBLL = new AreaMeterBLL();
        private MeterDataBLL meterDataBLL = new MeterDataBLL();
        private FileUploadMasterBLL fileUploadMasterBLL = new FileUploadMasterBLL();
";
        
        if (!content.Contains("private CircleBLL circleBLL"))
        {
            content = content.Replace("public static int Region_ID;", declarations + "        public static int Region_ID;");
            File.WriteAllText(path, content);
            Console.WriteLine("Fixed missing fields.");
        }
    }
}
