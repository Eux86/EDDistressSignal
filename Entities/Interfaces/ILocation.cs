using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Interfaces
{
    public interface ILocation
    {
        string SystemName { get; set; }
        string DetailedLocation { get; set; }
        double X { get; set; }
        double Y { get; set; }
        double Z { get; set; }
    }
}
