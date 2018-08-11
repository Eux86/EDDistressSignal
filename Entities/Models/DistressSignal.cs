using Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Models
{
    public class DistressSignal : IDistressSignal
    {
        public IPlayerInfo Player { get; set; }
        public ILocation SignalLocation { get; set; }
        public DateTime Time { get; set; }
        public bool Answered { get; set; }
        public IEnumerator<IPlayerInfo> PlayersAnswering { get; set; }
        public Guid Id { get; set; }

        public DistressSignal()
        {
            Id = new Guid();
        }
    }
}
