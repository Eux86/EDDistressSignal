using System;
using System.Collections.Generic;
using System.Text;

namespace ClientModels
{
    public class PlayerStatusMessage
    {
        public List<DistressSignal> OwnDistressSignals { get; set; }
        public List<DistressSignal> AnsweredDistressSignals { get; set; }
        public List<DistressSignal> OtherDistressSignals { get; set; }
    }
}
