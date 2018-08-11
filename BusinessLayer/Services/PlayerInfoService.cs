using BusinessLayer.Interfaces;
using Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Entities.Models;
using AutoMapper;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class PlayerInfoService : IPlayerInfoService
    {
        List<IPlayerInfo> _players = new List<IPlayerInfo>();
        private IDistressSignalService _distressSignalService;

        public PlayerInfoService(
            IDistressSignalService distressSignalService)
        {
            _distressSignalService = distressSignalService;
        }

        public IPlayerInfo GetPlayerInfoByApiKey(string apiKey)
        {
            return _players.Single(x => x.ApiKey == apiKey);
        }

        public void UpdatePlayerInfo(IPlayerInfo playerInfo)
        {
            var old = _players.SingleOrDefault(x => x.ApiKey == playerInfo.ApiKey);
            if (old == null)
            {
                _players.Add(playerInfo);
            }
            else
            {
                Mapper.Map<IPlayerInfo, IPlayerInfo>(playerInfo, old);
                old.Name = playerInfo.Name;
            }

        }

        public Task<IEnumerable<IPlayerInfo>> GetPlayersInRangeAsync(ILocation center, int distance)
        {
            return Task.Run(() =>
            {

                var inRange = _players.Where(x => ClientModels.LocationHelper.Distance(
                    new ClientModels.Location() { StarPosX = center.X, StarPosY = center.Y, StarPosZ = center.Z },
                    new ClientModels.Location() { StarPosX = x.Location.X, StarPosY = x.Location.Y, StarPosZ = x.Location.Z }
                    ) < distance);
                return inRange;
            });
        }

        public async Task DisconnectPlayerAsync(string apiKey)
        {
            var playerToDelete = await Task.Run(() =>
            {
                return _players.Single(x => x.ApiKey == apiKey);
            });
            await _distressSignalService.CancelDistressSignalAsync(playerToDelete);
            _players.Remove(playerToDelete);
        }
    }
}
