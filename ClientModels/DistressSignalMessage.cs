﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ClientModels
{
    public class SendDistressSignalMessage
    {
        public string ApiKey { get; set; }
        public Location Location { get; set; }
    }
}
