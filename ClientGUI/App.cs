using ClientGUI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ClientGUI
{
    public partial class App : Application
    {

        private static ServerService _serverService;
        private static EdLogService _edLogService;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            _edLogService = new EdLogService();
            _serverService = new ServerService(_edLogService);


            var mainWindow = new MainWindow(_serverService,_edLogService);
            mainWindow.Show();
        }
    }
}
