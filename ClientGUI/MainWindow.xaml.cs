using ClientModels;
using EDLogReader;
using System;
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

            try
            {
                _server = new Server();
                _server.ApiKey = "26C314EB - 317A - 4EFA - A872 - 3A8000F83AFC";
                _server.OnDistressSignal += server_OnDistressSignal;
                _server.Connect();
                Log("Connected");
            }
            catch (Exception ex)
            {
                Log(ex.Message);
            }

            try
            {
                _reader = new EDLogReader.Reader();
                _reader.updated += Reader_Updated;
                _reader.StartMonitoring();
                Log("Started LOG monitoring");
            }
            catch (Exception ex)
            {
                Log(ex.Message);
            }
        }

        private void server_OnDistressSignal(object sender, DistressSignalMessage e)
        {
            Log("DISTRESS SIGNAL RECEIVED! System: " + e.Location.StarSystem);
        }

        private async void Reader_Updated(object sender, EDLog e)
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
                await _server.UpdatePlayerInfo(e);
                Log("Sent updated player info");
            } catch(Exception ex)
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
                messagesList.Items.Add(message);
            });
        }


    }
}
