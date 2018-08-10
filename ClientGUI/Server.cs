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
        public event EventHandler<DistressSignalMessage> OnDistressSignal;

        private HubConnection _connection;

        public string ApiKey { get; set; }

        public Server()
        {
            _connection = new HubConnectionBuilder()
            .WithUrl("http://localhost:59608/distress")
            .Build();

        }

        public async Task Connect()
        {
            _connection.On<DistressSignalMessage>("ReceiveDistressSignal", (message) =>
            {
                OnDistressSignal?.Invoke(this, message);
            });

            await _connection.StartAsync();
        }

        internal async Task UpdatePlayerInfo(EDLog log)
        {
            PlayerInfoMessage message = new PlayerInfoMessage();
            message.ApiKey = ApiKey;
            message.Location = log.Location;
            await _connection.InvokeAsync("UpdatePlayerInfo", message);
        }

        internal async Task SendDistressSignal(EDLog log)
        {
            DistressSignalMessage message = new DistressSignalMessage();
            message.ApiKey = ApiKey;
            message.Location = log.Location;
            await _connection.InvokeAsync("SendDistressSignal", message);
        }
    }
}
