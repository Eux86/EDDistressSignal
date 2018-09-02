using System;
using System.Threading.Tasks;
using ClientModels;
using EDLogReader;

namespace ClientGUI.Services
{
    public class EdLogService
    {
        public event EventHandler<string> JournalFileSelected;
        public event EventHandler<Location> LocationUpdate;
        public event EventHandler<Player> PlayerInfoUpdated;

        Reader _reader = new Reader();

        public EdLogService()
        {
            _reader.JournalFileSelected += reader_JournalFileSelected;
            _reader.LocationUpdated += reader_LocationUpdated;
            _reader.PlayerInfoUpdated += reader_PlayerInfoUpdated;
        }

        private void reader_PlayerInfoUpdated(object sender, EDLog e)
        {
            PlayerInfoUpdated?.Invoke(this, e.Player);
        }

        private void reader_LocationUpdated(object sender, ClientModels.EDLog e)
        {
            LocationUpdate?.Invoke(this, e.Location);
        }

        private void reader_JournalFileSelected(object sender, string e)
        {
            JournalFileSelected?.Invoke(this,e);
        }

        internal void Start()
        {
            _reader.Start();
        }

        internal async Task UpdateData()
        {
            await _reader.ForceRead();
        }

        internal async Task<Location> GetPlayerLocationAsync()
        {
            await _reader.ForceRead();
            return _reader.Log.Location;
        }
    }
}