#region Namespaces
#endregion
namespace CAB.Parser
{
    /// <summary>
    /// Enum for storing DLMS Data types
    /// </summary>
    public enum ProtocolDataType
    {
        None = 255,
        NullDate = 0,
        Array = 1,
        Structure = 2,
        Boolean = 3,
        DoubleLong = 5,
        DoubleLongUnsigned = 6,
        OctetString = 9,
        VisibleString = 10,
        Integer = 15,
        Long = 16,
        Unsigned = 17,
        LongUnsigned = 18,
        Long64 = 20,
        Long64Unsigned = 21,
        Enum = 22,
        Float32 = 23,
        Float64 = 24,
        DateTime = 25,
        Date = 26,
        Time = 27
    }
}
