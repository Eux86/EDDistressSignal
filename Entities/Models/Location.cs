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
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        
    }
}
