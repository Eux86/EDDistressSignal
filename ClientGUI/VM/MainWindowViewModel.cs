using ClientModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ClientGUI.VM
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private ObservableCollection<DistressSignal> _receivedDistressSignals;
        private ObservableCollection<DistressSignal> _sentDistressSignals;
        private ObservableCollection<DistressSignal> _answeredDistressSignals;
        private Location _currentLocation;
        private string _currentJournalFilePath;

        public ObservableCollection<DistressSignal> ReceivedDistressSignals
        {
            get => _receivedDistressSignals;
            set
            {
                _receivedDistressSignals = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<DistressSignal> SentDistressSignals
        {
            get => _sentDistressSignals;
            set
            {
                _sentDistressSignals = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<DistressSignal> AnsweredDistressSignals
        {
            get => _answeredDistressSignals;
            set
            {
                _answeredDistressSignals = value;
                NotifyPropertyChanged();
            }
        }

        public Location CurrentLocation
        {
            get => _currentLocation;
            set
            {
                _currentLocation = value;
                NotifyPropertyChanged();
            }
        }

        public string CurrentJournalFilePath
        {
            get => _currentJournalFilePath;
            set {
                _currentJournalFilePath = value;
                NotifyPropertyChanged();
            }
        }

        public MainWindowViewModel()
        {
            ReceivedDistressSignals = new ObservableCollection<DistressSignal>();
            AnsweredDistressSignals = new ObservableCollection<DistressSignal>();
            SentDistressSignals = new ObservableCollection<DistressSignal>();
            CurrentLocation = new Location();
        }

    }
}
