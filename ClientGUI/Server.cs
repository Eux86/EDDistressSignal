using ClientModels;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientGUI
{
    public class Server
    {
        public event EventHandler<DistressSignalReceivedMessage> OnDistressSignal;
        public event EventHandler OnConnectionClosed;

        private HubConnection _connection;

        public string ApiKey { get; set; }

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
            return Task.CompletedTask;
        }

        public async Task Connect()
        {
            _connection.On<DistressSignalReceivedMessage>("DistressSignalReceived", (message) =>
            {
                OnDistressSignal?.Invoke(this, message);
            });

            await _connection.StartAsync();
        }

        internal async Task UpdatePlayerLocation(EDLog log)
        {
            PlayerLocationMessage message = new PlayerLocationMessage();
            message.ApiKey = ApiKey;
            message.Location = log.Location;
            await _connection.InvokeAsync("UpdatePlayerLocation", message);
        }

        internal async Task UpdatePlayerInfo(EDLog log)
        {
            PlayerInfoMessage message = new PlayerInfoMessage();
            message.ApiKey = ApiKey;
            message.Name = log.Player.Name;
            message.ShipType = log.Player.ShipType;
            await _connection.InvokeAsync("UpdatePlayerInfo", message);
        }

        internal async Task SendDistressSignal(EDLog log)
        {
            SendDistressSignalMessage message = new SendDistressSignalMessage();
            message.ApiKey = ApiKey;
            message.Location = log.Location;
            await _connection.InvokeAsync("SendDistressSignal", message);
        }
    }
}
