using System;
using System.Collections.Generic;
using System.Text;

namespace ClientModels
{
    public class DistressSignal
    {
        public Player Player { get; set; }
        public Location SignalLocation { get; set; }
        public DateTime Time { get; set; }
        public bool Answered { get; set; }
        public IEnumerator<Player> PlayersAnswering { get; set; }
        public Guid Id { get; set; }
   
    }
}
