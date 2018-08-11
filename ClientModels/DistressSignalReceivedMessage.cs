using System;
using System.Collections.Generic;
using System.Text;

namespace ClientModels
{
    public class DistressSignalReceivedMessage
    {
        public string PlayerName { get; set; }
        public Location Location { get; set; }

    }
}
