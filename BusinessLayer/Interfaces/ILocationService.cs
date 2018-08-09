using Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interfaces
{
    public interface ILocationService
    {
        void UpdatePlayerLocationAsync(string apiKey, ILocation location);
    }
}
