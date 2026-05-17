using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LicenseKeyGenerator
{
    public class LicenseFileEntity
    {
         public string SystemMac { get; set; }
         public string SystemName { get; set; }
         public string SystemDomain { get; set; }
         public string SystemUser { get; set; }
         public string ProductVersion { get; set; }
         public string ProductName { get; set; }
         public string ProductKey { get; set; }
         public string FileError { get; set; }
         public string KeyCodeFile { get; set; }
         public string KeyCodeReadouts { get; set; }
         public string KeyCodeProgramming { get; set; }
         public string KeyCodeDebugging { get; set; }
         public string KeyCodeAction { get; set; }
         public string KeyCodeOthersSetting { get; set; }
         public string KeyCodeMeterType { get; set; }
         public int KeySubCodeAccessLevel { get; set; }
         public string KeySubCodeApplicationType { get; set; }
         public char[] MeterTypeList { get; set; }
         public List<bool[]> AccessPermissionList { get; set; }
         public string AccessLevel { get; set; }
         public string ToolType { get; set; }
         public DateTime KeyGeneratedDate { get; set; }
         public DateTime KeyExpiryDate { get; set; }
         public string KeyGeneratedBy { get; set; }
         public string KeyIssueTo { get; set; }
         public string ElapedSystemDate { get; set; }
         
    }
}
