using Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Models
{
    class PlayerLocation : IPlayerLocation
    {
        public IPlayerInfo Player { get; set; }
        public ILocation Location { get; set; }
    }
}
