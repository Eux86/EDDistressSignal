using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class DistressSignalModel
    {
        public string PlayerName { get; set; }
        public string SystemName { get; set; }
        public string LocalArea { get; set; }
        public string ShipType { get; set; }
    }
}
