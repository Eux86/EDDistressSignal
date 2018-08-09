using ClientModels;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClientGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private HubConnection _connection;

        public MainWindow()
        {
            InitializeComponent();

            _connection = new HubConnectionBuilder()
            .WithUrl("http://localhost:59608/distress")
            .Build();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await _connection.InvokeAsync("SendDistressSignal","test1", "test2");
            }
            catch (Exception ex)
            {
                messagesList.Items.Add(ex.Message);
            }
        }

        private async void connectButton_Click(object sender, RoutedEventArgs e)
        {
            _connection.On<string, string>("ReceiveDistressSignal", (user, message) =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    var newMessage = $"{user}: {message}";
                    messagesList.Items.Add(newMessage);
                });
            });

            try
            {
                await _connection.StartAsync();
                messagesList.Items.Add("Connection started");
                connectButton.IsEnabled = false;
                sendButton.IsEnabled = true;
            }
            catch (Exception ex)
            {
                messagesList.Items.Add(ex.Message);
            }
        }

        private async void UpdatePositionButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UpdateLocationMessage message = new UpdateLocationMessage();
                message.ApiKey = apiKey.Text;
                message.LocationX = float.Parse(locX.Text);
                message.LocationY = float.Parse(locY.Text);
                message.LocationZ = float.Parse(locZ.Text);
                await _connection.InvokeAsync("UpdateLocation", message);
            }
            catch (Exception ex)
            {
                messagesList.Items.Add(ex.Message);
            }
        }
    }
}
