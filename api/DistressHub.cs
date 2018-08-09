using AutoMapper;
using BusinessLayer.Interfaces;
using ClientModels;
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
        public async Task SendDistressSignal(string user, string message)
        {
            await Clients.Others.SendAsync("ReceiveDistressSignal", user, message);
        }

        public async Task UpdateLocation(UpdateLocationMessage message)
        {
            Location location = _mapper.Map<UpdateLocationMessage,Location>(message);
            _locationService.UpdatePlayerLocationAsync(message.ApiKey, location);
        }
    }
}
