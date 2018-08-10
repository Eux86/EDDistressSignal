using AutoMapper;
using BusinessLayer.Interfaces;
using Entities.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api
{
    public class DistressHub : Hub
    {
        private ILocationService _locationService;
        private IMapper _mapper;

        public DistressHub(
            ILocationService locationService,
            IMapper mapper)
        {
            _locationService = locationService;
            _mapper = mapper;
        }
        public async Task SendDistressSignal(ClientModels.DistressSignalMessage message)
        {
            await Clients.Others.SendAsync("ReceiveDistressSignal", message);
        }

        public async Task UpdatePlayerInfo(ClientModels.PlayerInfoMessage message)
        {
            Location location = _mapper.Map<ClientModels.Location,Location>(message.Location);
            _locationService.UpdatePlayerLocationAsync(message.ApiKey, location);
        }
    }
}
