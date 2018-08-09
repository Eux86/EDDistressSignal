using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interfaces
{
    public interface IDistressSignalService
    {
        string CreateNewDistressSignal(DistressSignal distressSignal);
    }
}
