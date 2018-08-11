using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Interfaces
{
    public interface IPlayerLocation
    {
        IPlayerInfo Player { get; set; }
        ILocation Location { get; set; }
    }
}
