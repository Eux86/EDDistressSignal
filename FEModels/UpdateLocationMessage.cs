using System;
using System.Collections.Generic;
using System.Text;

namespace FEModels
{
    public class UpdateLocationMessage
    {
        public string ApiKey { get; set; }
        public float LocationX { get; set; }
        public float LocationY { get; set; }
        public float LocationZ { get; set; }
    }
}
