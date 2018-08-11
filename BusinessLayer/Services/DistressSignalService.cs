using BusinessLayer.Interfaces;
using Entities.Interfaces;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

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
        }

        public void CancelDistressSignal(Guid signalId)
        {
            _distressSignals.Remove(_distressSignals.Single(x => x.Id == signalId));
        }
    }
}
