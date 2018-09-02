using ClientGUI.Data;
using ClientModels;
using System;
using System.Threading.Tasks;

namespace ClientGUI.Services
{
    public class ServerService
    {
        public event EventHandler Connecting;
        public event EventHandler Connected;
        public event EventHandler Disconnected;
        public event EventHandler<DistressSignal> DistressSignalReceived;
        public event EventHandler<DistressSignal> DistressSignalCreated;

        private Server _server = new Server();
        private PlayerStateModel _playerState = new PlayerStateModel();
        private EdLogService _logService;

        public bool IsConnected { get; internal set; }

        public ServerService(EdLogService logService)
        {
            _logService = logService;
            _logService.LocationUpdate += logService_LocationUpdate;
            _logService.PlayerInfoUpdated += logService_PlayerInfoUpdated;
        }

        public async Task Connect()
        {
            _logService.Start();
            Connecting?.Invoke(this, null);
            _server = new Server();
            _server.ApiKey = Properties.Settings.Default.ApiKey;
            _server.OnDistressSignal += server_OnDistressSignal;
            _server.OnConnectionClosed += server_OnConnectionClosed;
            _server.DistressSignalCreated += _server_DistressSignalCreated;
            await _server.Connect();
            Connected?.Invoke(this, null);
            IsConnected = true;
        }

        private async void logService_PlayerInfoUpdated(object sender, Player e)
        {
            await UpdatePlayerInfoAsync(e);
        }

        private async void logService_LocationUpdate(object sender, Location e)
        {
            await UpdatePlayerLocationAsync(e);
        }

        private void _server_DistressSignalCreated(object sender, DistressSignalReceivedMessage e)
        {
            var ds = new DistressSignal()
            {
                Player = _playerState.PlayerInfo,
                SignalLocation = e.Location,
                Time = DateTime.Now,
            };
            DistressSignalCreated?.Invoke(this, ds);
        }

        private void server_OnConnectionClosed(object sender, EventArgs e)
        {
            Disconnected?.Invoke(this, null);
            IsConnected = false;
        }

        private void server_OnDistressSignal(object sender, DistressSignalReceivedMessage e)
        {
            var ds = new DistressSignal()
            {
                Player = new Player { Name = e.PlayerName },
                SignalLocation = e.Location,
                Time = DateTime.Now,
            };

            _playerState.ReceivedDistressSignals.Add(ds);

            DistressSignalReceived?.Invoke(this, ds);
        }

        public async Task SendDistressSignalAsync()
        {
            await _server.SendDistressSignalAsync(await _logService.GetPlayerLocationAsync());
        }

        internal async Task UpdatePlayerLocationAsync(Location location)
        {
            await _server.UpdatePlayerLocationAsync(location);
            _playerState.Location = location;
        }

        private async Task UpdatePlayerInfoAsync(Player player)
        {
            await _server.UpdatePlayerInfoAsync(player);
            _playerState.PlayerInfo = player;
        }
    }
}