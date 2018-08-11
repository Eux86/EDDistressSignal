using Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IPlayerInfoService
    {
        void UpdatePlayerInfo(IPlayerInfo playerInfo);
        IPlayerInfo GetPlayerInfoByApiKey(string apiKey);
        Task<IEnumerable<IPlayerInfo>> GetPlayersInRangeAsync(ILocation center, int distance);
    }
}
