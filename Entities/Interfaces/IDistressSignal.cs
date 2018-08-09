using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Interfaces
{
    public interface IDistressSignal
    {
        string Id { get; set; }
        string PlayerName { get; set; }
        ILocation Location { get; set; }
        string ShipType { get; set; }
    }
}
