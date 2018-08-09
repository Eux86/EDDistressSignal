using Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Models
{
    public class Location : ILocation
    {
        public string SystemName { get; set; }
        public string DetailedLocation { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

    }
}
