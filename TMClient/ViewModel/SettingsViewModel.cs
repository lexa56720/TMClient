using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TMClient.Utils;

namespace TMClient.ViewModel
{
    internal partial class SettingsViewModel : BaseViewModel, IDataErrorInfo
    {
        public ICommand PageLoaded => new Command(LoadSettings);
        public ICommand SaveCommand => new Command(Save);

        public string ServerAddress
        {
            get => serverAddress;
            set
            {
                serverAddress = value;
                OnPropertyChanged(nameof(ServerAddress));
            }
        }
        private string serverAddress = Preferences.Default.ServerAddress;

        public string AuthPort
        {
            get => authPort;
            set
            {
                authPort = value;
                OnPropertyChanged(nameof(AuthPort));
            }
        }
        private string authPort = Preferences.Default.AuthPort.ToString();

        public string ApiPort
        {
            get => apiPort;
            set
            {
                apiPort = value;
                OnPropertyChanged(nameof(ApiPort));
            }
        }
        private string apiPort= Preferences.Default.ApiPort.ToString();

        public string NotificationPort
        {
            get => notificationPort;
            set
            {
                notificationPort = value;
                OnPropertyChanged(nameof(NotificationPort));
            }
        }
        private string notificationPort = Preferences.Default.LongPollPort.ToString();

        public string CachedUserLifeTime
        {
            get => cachedUserLifeTime;
            set
            {
                cachedUserLifeTime = value;
                OnPropertyChanged(nameof(CachedUserLifeTime));
            }
        }
        private string cachedUserLifeTime = Preferences.Default.CachedUserLifetimeMinutes.ToString();

        public string CachedChatLifeTime
        {
            get => cachedChatLifeTime;
            set
            {
                cachedChatLifeTime = value;
                OnPropertyChanged(nameof(cachedChatLifeTime));
            }
        }
        private string cachedChatLifeTime = Preferences.Default.CachedChatLifetimeMinutes.ToString();

        public bool SuccessSave
        {
            get => successSave;
            set
            {
                successSave = value;
                OnPropertyChanged(nameof(SuccessSave));
            }
        }
        private bool successSave;


        [GeneratedRegex("^(?:[0-9]{1,3}\\.){3}[0-9]{1,3}$")]
        private static partial Regex IpRegexClass();
        private Regex IpRegex = IpRegexClass();

        public string Error => throw new NotImplementedException();
        public string this[string columnName]
        {
            get
            {
                var error = string.Empty;
                switch (columnName)
                {
                    case nameof(ServerAddress):
                        if (!IpRegex.IsMatch(ServerAddress))
                            error = "Ip error";
                        break;
                    case nameof(AuthPort):
                        if (!int.TryParse(AuthPort, out var auth) || auth >= ushort.MaxValue)
                            error = "Auth port error";
                        break;
                    case nameof(ApiPort):
                        if (!int.TryParse(ApiPort, out var api) || api >= ushort.MaxValue)
                            error = "Api port error";
                        break;
                    case nameof(NotificationPort):
                        if (!int.TryParse(NotificationPort, out var notification) || notification >= ushort.MaxValue)
                            error = "Notification port error";
                        break;
                    case nameof(CachedUserLifeTime):
                        if (!int.TryParse(CachedUserLifeTime, out var cachedUser) || cachedUser < 0)
                            error = "Cached user lifetime error";
                        break;
                    case nameof(CachedChatLifeTime):
                        if (!int.TryParse(CachedChatLifeTime, out var cachedChat) || cachedChat < 0)
                            error = "Cached chat lifetime error";
                        break;
                }
                return error;
            }
        }

        public SettingsViewModel()
        {
        }
        private void LoadSettings()
        {
            ServerAddress = Preferences.Default.ServerAddress;
            AuthPort = Preferences.Default.AuthPort.ToString();
            ApiPort = Preferences.Default.ApiPort.ToString();
            NotificationPort = Preferences.Default.LongPollPort.ToString();
            CachedUserLifeTime = Preferences.Default.CachedUserLifetimeMinutes.ToString();
            CachedChatLifeTime = Preferences.Default.CachedChatLifetimeMinutes.ToString();
        }

        private void Save(object? obj)
        {
            Preferences.Default.ServerAddress = serverAddress;

            if (int.TryParse(AuthPort, out var auth))
                Preferences.Default.AuthPort = auth;

            if (int.TryParse(ApiPort, out var api))
                Preferences.Default.ApiPort = api;

            if (int.TryParse(NotificationPort, out var longPoll))
                Preferences.Default.LongPollPort= longPoll;

            if (int.TryParse(CachedUserLifeTime, out var user))
                Preferences.Default.CachedUserLifetimeMinutes =user;

            if (int.TryParse(CachedChatLifeTime, out var chat))
                Preferences.Default.CachedChatLifetimeMinutes = chat;

            Preferences.Default.Save();
            SuccessSave = true;
        }

    }
}
