using System;
using System.Collections.Generic;
using System.Text;

namespace ClientModels
{
    public class DistressSignalMessage
    {
        public string ApiKey { get; set; }
        public Location Location { get; set; }
    }
}
