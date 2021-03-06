﻿using ClientModels;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientGUI.Data
{
    public class Server
    {
        public event EventHandler<DistressSignalReceivedMessage> OnDistressSignal;
        public event EventHandler<DistressSignalReceivedMessage> DistressSignalCreated;
        public event EventHandler OnConnectionClosed;

        private HubConnection _connection;

        public string ApiKey { get; set; }
        public PlayerStatus PlayerStatus { get; set; }
        public bool Connected { get; internal set; }

        public Server()
        {
            _connection = new HubConnectionBuilder()
                //.WithUrl("http://eddistress.azurewebsites.net/distress")
                .WithUrl("http://localhost:30303/distress")
                .Build();

            _connection.Closed += connection_Closed;
        }

        private Task connection_Closed(Exception arg)
        {
            OnConnectionClosed?.Invoke(this, null);
            Connected = false;
            return Task.CompletedTask;
        }

        public async Task Connect()
        {
            _connection.On<DistressSignalReceivedMessage>("DistressSignalReceived", (message) =>
            {
                OnDistressSignal?.Invoke(this, message);
            });

            _connection.On<DistressSignalReceivedMessage>("DistressSignalCreated", (message) =>
            {
                DistressSignalCreated?.Invoke(this, message);
            });

            await _connection.StartAsync();
            Connected = true;
        }

        internal async Task UpdatePlayerLocationAsync(Location location)
        {
            PlayerLocationMessage message = new PlayerLocationMessage();
            message.ApiKey = ApiKey;
            message.Location = location;
            await _connection.InvokeAsync("UpdatePlayerLocation", message);
        }

        internal async Task UpdatePlayerInfoAsync(Player player)
        {
            PlayerInfoMessage message = new PlayerInfoMessage();
            message.ApiKey = ApiKey;
            message.Name = player.Name;
            message.ShipType = player.ShipType;
            await _connection.InvokeAsync("UpdatePlayerInfo", message);
        }

        internal async Task SendDistressSignalAsync(Location location)
        {
            SendDistressSignalMessage message = new SendDistressSignalMessage();
            message.ApiKey = ApiKey;
            message.Location = location;
            await _connection.InvokeAsync("SendDistressSignal", message);
        }

        internal async Task LogInAsync(EDLog log)
        {
            LogInMessage message = new LogInMessage();
            message.ApiKey = ApiKey;
            message.Name = log.Player.Name;
            message.ShipType = log.Player.ShipType;
            await _connection.InvokeAsync("LogIn", message);
        }

        internal async Task LogOutAsync(string apiKey)
        {
            await _connection.InvokeAsync("LogOut", ApiKey);
        }

        internal async Task UpdatePlayerStatus(PlayerStatusMessage playerStatusMessage)
        {
            //ToDO
            throw new NotImplementedException();
        }
    }
}
