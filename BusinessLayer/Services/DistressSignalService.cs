using BusinessLayer.Interfaces;
using Entities.Interfaces;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class DistressSignalService : IDistressSignalService
    {
        private List<IDistressSignal> _distressSignals = new List<IDistressSignal>();

        private IPlayerInfoService _playerInfoService;

        public DistressSignalService(
            IPlayerInfoService playerInfoService
         )
        {
            _playerInfoService = playerInfoService;
        }

        public void AddDistressSignal(DistressSignal distressSignal)
        {
            _distressSignals.Add(distressSignal);
            distressSignal.Player.DistressSignals.ToList().Add(distressSignal);
        }

        public void AddDistressSignal(ClientModels.DistressSignal distressSignal)
        {
            throw new NotImplementedException();
        }

        public void CancelDistressSignal(Guid signalId)
        {
            var toRemove = _distressSignals.Single(x => x.Id == signalId);
            _distressSignals.Remove(toRemove);
            toRemove.Player.DistressSignals.ToList().Remove(toRemove);
        }

        public async Task<IEnumerable<IDistressSignal>> GetPlayersSignalAsyncs(IPlayerInfo playerInfo)
        {
            return await Task.Run(() =>
            {
                return _distressSignals.Where(x => x.Player.ApiKey == playerInfo.ApiKey);
            });
        }

        public async Task<IEnumerable<IDistressSignal>> GetSignalsAnsweredByPlayerAsync(IPlayerInfo playerInfo)
        {
            return await Task.Run(() =>
            {
                return _distressSignals.Where(x => x.PlayersAnswering.Any(answ=>answ.ApiKey==playerInfo.ApiKey));
            });
        }

        public async Task<IEnumerable<IDistressSignal>> GetSignalsInRangeOfPlayerAsync(IPlayerInfo playerInfo, int distance)
        {
            return await Task.Run(() =>
            {
                return _distressSignals.Where(x => ClientModels.LocationHelper.Distance(
                    new ClientModels.Location() { StarPosX = x.SignalLocation.X, StarPosY = x.SignalLocation.Y, StarPosZ = x.SignalLocation.Z },
                    new ClientModels.Location() { StarPosX = playerInfo.Location.X, StarPosY = playerInfo.Location.Y, StarPosZ = playerInfo.Location.Z }) 
                    < distance);
            });
        }
    }
}
