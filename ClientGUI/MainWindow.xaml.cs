using ClientModels;
using EDLogReader;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace ClientGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Server _server;
        private Reader _reader;

        public MainWindow()
        {
            InitializeComponent();
            Initialization();
        }

        private async void Initialization()
        {
#if DEBUG
            Properties.Settings.Default.ApiKey = Guid.NewGuid().ToString();
#endif

            apiKey.Text = Properties.Settings.Default.ApiKey;
            if (!string.IsNullOrEmpty(Properties.Settings.Default.ApiKey))
            {
                await ConnectToServer();
                _reader = new Reader();
                _reader.LocationUpdated += Reader_LocationUpdated;
                _reader.PlayerInfoUpdated += reader_PlayerInfoUpdated;
                _reader.JournalFileSelected += reader_JournalFileSelected;
                await _reader.ForceRead();
                //await _server.LogInAsync(_reader.Log);
                _reader.StartMonitoring();
                Log("Started LOG monitoring");
            } else
            {
                Log("Configure your API KEY before continuing");
                apiKey.IsEnabled = true;
                apiKeyButton.IsEnabled = true;
            }

            KeyboardHandler keyboardHandler = new KeyboardHandler(this);
            keyboardHandler.SendDistressSignalKeyPressed += KeyboardHandler_SendDistressSignalKeyPressed;

        }

        private async void KeyboardHandler_SendDistressSignalKeyPressed(object sender, EventArgs e)
        {
            Log("Send distress signal pressed!");
            await _reader.ForceRead();
            await _server.SendDistressSignal(_reader.Log);
        }

        private void reader_JournalFileSelected(object sender, string e)
        {
            this.Dispatcher.Invoke(() =>
            {
                journalFilePath.Text = e;
            });
        }

        private async Task ConnectToServer()
        {
            try
            {
                _server = new Server();
                _server.ApiKey = Properties.Settings.Default.ApiKey;
                _server.OnDistressSignal += server_OnDistressSignal;
                _server.OnConnectionClosed += server_OnConnectionClosed;
                Log("Connecting");
                await _server.Connect();
                Log("Connected");
            }
            catch (Exception ex)
            {
                Log(ex.Message);
            }
        }

        private void server_OnConnectionClosed(object sender, EventArgs e)
        {
            Log("Connecion closed");
        }

        private void server_OnDistressSignal(object sender, DistressSignalReceivedMessage e)
        {
            Log($"DISTRESS SIGNAL DETECTED! Cmdr {e.PlayerName} in System {e.Location.StarSystem} at {LocationHelper.Distance(_reader.Log.Location,e.Location)}Ly from current position");
        }

        private async void Reader_LocationUpdated(object sender, EDLog e)
        {
            this.Dispatcher.Invoke(() =>
            {
                Console.WriteLine("Location updated: " + e.Location.StarSystem);
                locX.Text = e.Location.StarPosX.ToString();
                locY.Text = e.Location.StarPosY.ToString();
                locZ.Text = e.Location.StarPosZ.ToString();
                StarSystemName.Text = e.Location.StarSystem;
            });
            try
            {
                if (_server.Connected)
                {
                    await _server.UpdatePlayerLocation(e);
                }
                Log("Sent updated player location");
            }
            catch (Exception ex)
            {
                Log(ex.Message);
            }

        }

        private async void reader_PlayerInfoUpdated(object sender, EDLog e)
        {
            try
            {
                if (_server.Connected)
                {
                    await _server.UpdatePlayerInfo(e);
                }
                Log("Sent updated player info");
            }
            catch (Exception ex)
            {
                Log(ex.Message);
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await _server.SendDistressSignal(_reader.Log);
        }

        private void Log(string message)
        {
            this.Dispatcher.Invoke(() =>
            {
                messagesList.Items.Insert(0,message);
            });
        }

        private void apiKeyButton_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.ApiKey = apiKey.Text;
            Properties.Settings.Default.Save();
            apiKey.IsEnabled = false;
            apiKeyButton.IsEnabled = false;
            Initialization();
        }

        private async void manualPositionButton_Click(object sender, RoutedEventArgs e)
        {
            _reader.Log.Location.StarPosX = float.Parse(locX.Text);
            _reader.Log.Location.StarPosY = float.Parse(locY.Text);
            _reader.Log.Location.StarPosZ = float.Parse(locZ.Text);
            await _server.UpdatePlayerLocation(_reader.Log);
        }
    }
}
