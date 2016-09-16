﻿using Mirror.Networking;
using System;
using Windows.UI.Xaml;

namespace Mirror.ViewModels
{
    public class HudViewModel : BaseViewModel
    {
        Uri _connectivitySource = new Uri($"ms-appx:///Assets/wifi-1.png");

        public Uri ConnectivitySource
        {
            get { return _connectivitySource; }
            set
            {
                _connectivitySource = value;
                OnPropertyChanged(this, nameof(ConnectivitySource));
            }
        }

        public HudViewModel(DependencyObject dependency) : base(dependency)
        {
            Internet.ConnectionChanged = OnConnectionChanged;
            Internet.Initialize();
        }        

        void OnConnectionChanged(ConnectionStatus connection)
        {
            switch (connection.Type)
            {
                case ConnectionType.Ethernet:
                    ConnectivitySource = new Uri("ms-appx:///Assets/ethernet.png");
                    break;
                case ConnectionType.Wifi:
                    string wifi = "wifi";
                    var bars = connection.SignalBars.GetValueOrDefault();
                    if (bars > 0 && bars < 4) wifi += $"-{bars}";
                    ConnectivitySource = new Uri($"ms-appx:///Assets/{wifi}.png");
                    break;

                case ConnectionType.None:
                case ConnectionType.Cellular:
                default:
                    ConnectivitySource = null;
                    break;
            }
        }

        public override string ToFormattedString(DateTime? dateContext) => _connectivitySource.ToString();
    }
}