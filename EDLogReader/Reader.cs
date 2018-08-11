using ClientModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace EDLogReader
{
    // reads the log file and fires events for each new event found in the log since last reading
    public class Reader
    {
        public event EventHandler<EDLog> LocationUpdated;
        public event EventHandler<EDLog> PlayerInfoUpdated;
        public EDLog Log { get; private set; }

        private string _logDirectory = @"";
        // The time of the last read event in chronological order
        private DateTime _latestEventTime = DateTime.MinValue;
        private FileSystemWatcher _fileWatcher = new FileSystemWatcher();

        public Reader()
        {
            Log = new EDLog();
            _fileWatcher.Changed += fileWatcher_Changed;
            this._logDirectory = Environment.GetEnvironmentVariable("userprofile")+@"\Downloads\";
        }

        public void StartMonitoring()
        {
            _fileWatcher.Path = _logDirectory;
            _fileWatcher.EnableRaisingEvents = true;
        }

        public async Task ForceRead()
        {
            await ReadAsync(@"C:\Users\eugenio.ditullio\Downloads\journal.txt");
        }

        private void fileWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            ReadAsync(e.FullPath);
        }

        private async Task ReadAsync(string logPath)
        {
            await Task.Run(() =>
            {
                using (var fileStream = new FileStream(logPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (StreamReader file = new StreamReader(fileStream))
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    reader.SupportMultipleContent = true;

                    var jsonSerializer = new JsonSerializer();
                    DateTime newestEventTime = _latestEventTime;
                    while (reader.Read())
                    {
                        dynamic logEvent = jsonSerializer.Deserialize(reader);

                        if (_latestEventTime == null || logEvent.timestamp > _latestEventTime)
                        {
                            _latestEventTime = logEvent.timestamp;
                            HandleLogEvent(logEvent);
                        }
                    }
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

                    if (LocationUpdated != null)
                    {
                        var newLocation = new Location()
                        {
                            StarSystem = logEvent.StarSystem,
                            StarPosX = logEvent.StarPos[0],
                            StarPosY = logEvent.StarPos[1],
                            StarPosZ = logEvent.StarPos[2],
                        };
                        Log.Location = newLocation;

                        LocationUpdated(this, Log);
                    }
                    break;
                case "LoadGame":
                    if (PlayerInfoUpdated!=null)
                    {
                        var newPlayerInfo = new Player()
                        {
                            Name = logEvent.Commander,
                            ShipType = logEvent.Ship,
                        };
                        Log.Player = newPlayerInfo;
                        PlayerInfoUpdated(this, Log);
                    }
                    break;
            }
        }

    }
}
