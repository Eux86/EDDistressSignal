using ClientModels;
using System.Collections.Generic;

namespace ClientGUI
{
    public class PlayerStateModel
    {
        public Location Location { get; set; }
        public Player PlayerInfo { get; set; }
        public IList<DistressSignal> OwnDistressSignals { get; set; }
        public IList<DistressSignal> ReceivedDistressSignals { get; set; }
        public IList<DistressSignal> AnsweredDistressSignals { get; set; }

        public PlayerStateModel()
        {
            OwnDistressSignals = new List<DistressSignal>();
            AnsweredDistressSignals = new List<DistressSignal>();
            ReceivedDistressSignals = new List<DistressSignal>();
        }
    }
}
