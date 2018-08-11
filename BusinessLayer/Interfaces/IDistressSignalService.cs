using Entities.Interfaces;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IDistressSignalService
    {
        void AddDistressSignal(DistressSignal distressSignal);
        void CancelDistressSignal(Guid signalId);
        Task<IEnumerable<IDistressSignal>> GetPlayersSignalAsyncs(IPlayerInfo playerInfo);
        Task<IEnumerable<IDistressSignal>> GetSignalsInRangeOfPlayerAsync(IPlayerInfo playerInfo, int v);
        Task<IEnumerable<IDistressSignal>> GetSignalsAnsweredByPlayerAsync(IPlayerInfo playerInfo);
        Task CancelDistressSignalAsync(IPlayerInfo player);
    }
}
