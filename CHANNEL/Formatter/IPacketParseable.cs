using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CHANNEL.Formatter
{
    public interface IPacketParseable
    {
        object GetPacketParsingStructure(string xmlFileName, Type type);
    }
}
