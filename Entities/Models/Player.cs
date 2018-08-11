using Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Models
{
    public class Player : IPlayerInfo
    {
        public string ApiKey { get; set; }
        public string Name { get; set; }
        public ILocation Location { get; set; }
        public string ConnectionId { get; set; }
        public string ShipType { get; set; }
        public IEnumerable<IDistressSignal> DistressSignals { get; set; }
    }
}
