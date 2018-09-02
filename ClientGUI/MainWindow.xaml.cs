using ClientGUI.Services;
using ClientGUI.VM;
using ClientModels;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace ClientGUI
{
    public partial class MainWindow : Window
    {
        private EdLogService _edLogService;
        private ServerService _serverService;
        private MainWindowViewModel _model;

        public MainWindow(ServerService serverService, EdLogService edLogService)
        {
            InitializeComponent();
#if DEBUG
            Properties.Settings.Default.ApiKey = Guid.NewGuid().ToString();
#endif

            _model = this.DataContext as MainWindowViewModel;

            _edLogService = edLogService;
            _edLogService.JournalFileSelected += edLogService_JournalFileSelected;
            _edLogService.LocationUpdate += edLogService_LocationUpdate;

            _serverService = serverService;

            _serverService.DistressSignalReceived += _serverService_DistressSignalReceived;
            _serverService.DistressSignalCreated += serverService_DistressSignalCreated;
            _serverService.Connected += serverService_Connected;

            _serverService.Connect();

            StartKeyInterceptor();
        }

        private void edLogService_LocationUpdate(object sender, Location e)
        {
            _model.CurrentLocation = e;
        }

        private void serverService_Connected(object sender, EventArgs e)
        {
            Log("Connected");
        }

        #region reader events
        private void edLogService_JournalFileSelected(object sender, string e)
        {
            _model.CurrentJournalFilePath = e;
        }
        #endregion

        #region Server events
        private void serverService_DistressSignalCreated(object sender, DistressSignal e)
        {
            this.Dispatcher.Invoke(() =>
            {
                _model.SentDistressSignals.Add(e);
            });
        }

        private void _serverService_DistressSignalReceived(object sender, DistressSignal distressSignal)
        {
            this.Dispatcher.Invoke(() =>
            {
                _model.ReceivedDistressSignals.Add(distressSignal);
            });
        }

        #endregion

        #region Keyboard interceptor
        private void StartKeyInterceptor()
        {
            KeyboardHandler keyboardHandler = new KeyboardHandler(this);
            keyboardHandler.SendDistressSignalKeyPressed += KeyboardHandler_SendDistressSignalKeyPressed;
        }

        private async void KeyboardHandler_SendDistressSignalKeyPressed(object sender, EventArgs e)
        {
            Log("Send distress signal pressed!");
            await _serverService.SendDistressSignalAsync();
            
        }
        #endregion

        private void Log(string message)
        {
            this.Dispatcher.Invoke(() =>
            {
                messagesList.Items.Insert(0,message);
            });
        }

        #region GUI Actions

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await _serverService.SendDistressSignalAsync();
        }

        private void apiKeyButton_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.ApiKey = apiKey.Text;
            Properties.Settings.Default.Save();
            apiKey.IsEnabled = false;
            apiKeyButton.IsEnabled = false;
            //Initialization();
        }

        #endregion


    }
}
