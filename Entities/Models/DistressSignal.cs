using Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Models
{
    public class DistressSignal : IDistressSignal
    {
        public string Id { get; set; }
        public string PlayerName { get; set; }
        public ILocation Location { get; set; }
        public string ShipType { get; set; }

    }
}
