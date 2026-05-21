using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace LandisGyr.GSIS.CIM2ndEd
{
    public partial class PublishEventResponse
    {
        [MessageHeader(Namespace = "")]
        public string UserName { get; set; }
        [MessageHeader(Namespace = "")]
        public string Password { get; set; }
    }
}
