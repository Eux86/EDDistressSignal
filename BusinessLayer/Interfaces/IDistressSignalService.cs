using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interfaces
{
    public interface IDistressSignalService
    {
        void AddDistressSignal(DistressSignal distressSignal);
        void CancelDistressSignal(Guid signalId);

    }
}
