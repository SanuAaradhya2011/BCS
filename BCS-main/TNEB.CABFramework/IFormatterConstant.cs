using System;
using System.Collections.Generic; 
using System.Text;

namespace CAB.IECFramework
{
    public interface IFormatterConstant
    {
        int VoltageConversionFactor { get; }
        int DemandConversionFactor { get; }
        int CurrentConversionFactor { get; }
        int PFConversionFactor { get; }
        string Voltage { get; }
        string Current { get; }
        string PowerFactor { get; }
        string Energy { get; }
        string Power { get; }
    }
}