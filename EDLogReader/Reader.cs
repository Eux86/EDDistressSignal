using ClientModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace EDLogReader
{
    // reads the log file and fires events for each new event found in the log since last reading
    public class Reader
    {
        public event EventHandler<string> JournalFileSelected;

        public event EventHandler<EDLog> LocationUpdated;
        public event EventHandler<EDLog> PlayerInfoUpdated;
        public EDLog Log { get; private set; }

        private string _logDirectory = @"";
        // The time of the last read event in chronological order
        private DateTime _latestEventTime = DateTime.MinValue;
        private FileSystemWatcher _fileWatcher = new FileSystemWatcher();

        private string _currentJournalFile;

        public Reader()
        {
            Log = new EDLog();
            _fileWatcher.Changed += fileWatcher_Changed;
            _fileWatcher.Created += fileWatcher_Created;
            _fileWatcher.NotifyFilter = NotifyFilters.LastAccess |
                         NotifyFilters.LastWrite |
                         NotifyFilters.FileName |
                         NotifyFilters.DirectoryName;
            this._logDirectory = Environment.GetEnvironmentVariable("userprofile") + @"\Saved Games\Frontier Developments\Elite Dangerous\";

            StartTicking();
        }

        /// <summary>
        /// Forces windows file system handler to fire the FileWatcher events
        /// </summary>
        private void StartTicking()
        {
            var myTimer = new System.Timers.Timer();
            // Tell the timer what to do when it elapses
            myTimer.Elapsed += delegate (object sender, System.Timers.ElapsedEventArgs e)
            {
                if (_currentJournalFile != null)
                {
                    using (FileStream fs = new FileStream(_currentJournalFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        fs.ReadByte();
                    }
                }
            };
            // Set it to go off every five seconds
            myTimer.Interval = 5000;
            // And start it        
            myTimer.Enabled = true;
        }

        public void StartMonitoring()
        {
            _fileWatcher.Path = _logDirectory;
            _fileWatcher.EnableRaisingEvents = true;
        }

        public async Task ForceRead()
        {
            var directory = new DirectoryInfo(Environment.GetEnvironmentVariable("userprofile") + @"\Saved Games\Frontier Developments\Elite Dangerous\");
            _currentJournalFile = directory.GetFiles("Journal.*").OrderByDescending(x => x.LastWriteTime).First().FullName;
            JournalFileSelected?.Invoke(this, _currentJournalFile);
            await ReadAsync();
        }

        private void fileWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            if (e.Name.StartsWith("Journal"))
            {
                _currentJournalFile = e.FullPath;
                ReadAsync();
            }
        }

        private void fileWatcher_Created(object sender, FileSystemEventArgs e)
        {
            if (e.Name.StartsWith("Journal"))
            {
                _currentJournalFile = e.FullPath;
                JournalFileSelected?.Invoke(this, _currentJournalFile);
                ReadAsync();
            }
        }

        private async Task ReadAsync()
        {
            await Task.Run(() =>
            {
                using (var fileStream = new FileStream(_currentJournalFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (StreamReader file = new StreamReader(fileStream))
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    reader.SupportMultipleContent = true;

                    var jsonSerializer = new JsonSerializer();
                    DateTime newestEventTime = _latestEventTime;
                    dynamic logEvent = null;
                    while (reader.Read())
                    {
                        logEvent = jsonSerializer.Deserialize(reader);

                        if (logEvent!=null && (_latestEventTime == null || logEvent.timestamp > _latestEventTime))
                        {
                            HandleLogEvent(logEvent);
                        }
                    }
                    if (logEvent!=null)
                        _latestEventTime = logEvent.timestamp;
                }
            });

        }

        private void HandleLogEvent(dynamic logEvent)
        {
            string evnt = logEvent.@event;
            switch (evnt)
            {
                case "FSDJump":
                case "Location":

                    var newLocation = new Location()
                    {
                        StarSystem = logEvent.StarSystem,
                        StarPosX = logEvent.StarPos[0],
                        StarPosY = logEvent.StarPos[1],
                        StarPosZ = logEvent.StarPos[2],
                    };
                    Log.Location = newLocation;
                    if (LocationUpdated != null)
                    {

                        LocationUpdated(this, Log);
                    }
                    break;
                case "LoadGame":
                    var newPlayerInfo = new Player()
                    {
                        Name = logEvent.Commander,
                        ShipType = logEvent.Ship,
                    };
                    Log.Player = newPlayerInfo;
                    if (PlayerInfoUpdated != null)
                    {
                        PlayerInfoUpdated(this, Log);
                    }
                    break;
            }
        }

    }
}
