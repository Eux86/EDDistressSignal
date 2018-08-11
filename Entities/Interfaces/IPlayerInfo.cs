using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Interfaces
{
    public interface IPlayerInfo
    {
        string ApiKey { get; set; }
        string Name { get; set; }
        ILocation Location { get; set; }
        IEnumerable<IDistressSignal> DistressSignals { get; set; }
        string ConnectionId { get; set; }
        string ShipType { get; set; }

    }
}
