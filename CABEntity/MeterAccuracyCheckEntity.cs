using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.Framework.Entity;
namespace CABEntity
{
    /// <summary>
    /// This class is used as an entity for energy parameters of meter accuracy check.
    /// </summary>
    public class MeterAccuracyCheckEntity : EntityBase
    {
        private string kWh, kVAh, kvarhLag, kvarhLead, exportKWh, exportkVAh, exportKvarhLag, exportKvarhLead;


        public string KWh
        { get; set; }

        public string KVAh
        { get; set; }
        public string KvarhLag
        { get; set; }
        public string KvarhLead
        { get; set; }
        public string ExportKWh
        { get; set; }

        public string ExportKVAh
        { get; set; }
        public string ExportKvarhLag
        { get; set; }
        public string ExportKvarhLead
        { get; set; }
        public bool isHTCT
        { get; set; }
    }
}
