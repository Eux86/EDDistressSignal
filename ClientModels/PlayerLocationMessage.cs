using System;
using System.Collections.Generic;
using System.Text;

namespace ClientModels
{
    public class PlayerLocationMessage
    {
        public string ApiKey { get; set; }
        public Location Location { get; set; }
    }
}
