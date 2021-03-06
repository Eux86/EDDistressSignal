﻿using AutoMapper;
using BusinessLayer.Interfaces;
using Entities.Interfaces;
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
        private IDistressSignalService _distressSignalService;
        private IPlayerInfoService _playerService;
        private IMapper _mapper;

        public DistressHub(
            IDistressSignalService distressSignalService,
            IPlayerInfoService playerService,
            IMapper mapper)
        {
            _distressSignalService = distressSignalService;
            _playerService = playerService;
            _mapper = mapper;
        }
        public async Task SendDistressSignal(ClientModels.SendDistressSignalMessage message)
        {
            var player = _playerService.GetPlayerInfoByApiKey(message.ApiKey);

            DistressSignal signal = new DistressSignal()
            {
                Player = player,
                SignalLocation = _mapper.Map<ClientModels.Location, Location>(message.Location),
                Time = DateTime.UtcNow,
            };
            _distressSignalService.AddDistressSignal(signal);

            var newMessage = new ClientModels.DistressSignalReceivedMessage()
            {
                Location = _mapper.Map<Location, ClientModels.Location>((Location)signal.SignalLocation),
                PlayerName = signal.Player.Name,
            };

            var inRange = await _playerService.GetPlayersInRangeAsync(signal.SignalLocation, 100);
            if (inRange != null)
            {
                foreach (var contact in inRange)
                {
                    if (contact.ApiKey != signal.Player.ApiKey) // Avoid sending the distress signal to the player who sent it
                        await Clients.Client(contact.ConnectionId).SendAsync("DistressSignalReceived", newMessage);
                }
            }

            await Clients.Caller.SendAsync("DistressSignalCreated", newMessage);
        }

        public void UpdatePlayerLocation(ClientModels.PlayerLocationMessage message)
        {

            var player = _playerService.GetPlayerInfoByApiKey(message.ApiKey);
            var location = (ILocation)_mapper.Map<ClientModels.Location, Location>(message.Location);
            player.Location = location;
        }

        public void UpdatePlayerInfo(ClientModels.PlayerInfoMessage message)
        {
            var playerInfo = (IPlayerInfo)_mapper.Map<ClientModels.PlayerInfoMessage, IPlayerInfo>(message);
            playerInfo.ConnectionId = Context.ConnectionId;
            _playerService.UpdatePlayerInfo(playerInfo);
        }

        public async void LogIn(ClientModels.LogInMessage message)
        {
            var clients = Clients;
            var callerId = Context.ConnectionId;
            var playerInfo = (IPlayerInfo)_mapper.Map<ClientModels.LogInMessage, IPlayerInfo>(message);
            playerInfo.ConnectionId = Context.ConnectionId;
            _playerService.UpdatePlayerInfo(playerInfo);

            var playerStatusMessage = new ClientModels.PlayerStatusMessage();

            var playersSignals = await _distressSignalService.GetPlayersSignalAsyncs(playerInfo);
            playerStatusMessage.OwnDistressSignals = _mapper.Map<IEnumerable<IDistressSignal>,List<ClientModels.DistressSignal>>(playersSignals);

            IEnumerable<IDistressSignal> signalsInRangeOfPlayer = await _distressSignalService.GetSignalsInRangeOfPlayerAsync(playerInfo, 100);
            playerStatusMessage.OtherDistressSignals = _mapper.Map<IEnumerable<IDistressSignal>, List<ClientModels.DistressSignal>>(signalsInRangeOfPlayer);

            IEnumerable<IDistressSignal> signalsAnswered = await _distressSignalService.GetSignalsAnsweredByPlayerAsync(playerInfo);
            playerStatusMessage.AnsweredDistressSignals = _mapper.Map<IEnumerable<IDistressSignal>, List<ClientModels.DistressSignal>>(signalsAnswered);

            await clients.Client(callerId).SendAsync("UpdatePlayerStatus", playerStatusMessage);
        }

        public async void LogOut(string apiKey)
        {
            await _playerService.DisconnectPlayerAsync(apiKey);
        }
    }
}
