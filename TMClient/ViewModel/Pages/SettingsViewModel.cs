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

        public string InfoPort
        {
            get => infoPort;
            set
            {
                infoPort = value;
                OnPropertyChanged(nameof(InfoPort));
            }
        }
        private string infoPort = Preferences.Default.InfoPort.ToString();

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

        public bool IsSaveAuth
        {
            get => isSaveAuth;
            set
            {
                isSaveAuth = value;            
                OnPropertyChanged(nameof(IsSaveAuth));
            }
        }
        private bool isSaveAuth = Preferences.Default.IsSaveAuth;
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
                            error = "Некорректный адресс";
                        break;
                    case nameof(InfoPort):
                        if (!int.TryParse(InfoPort, out var auth) || auth >= ushort.MaxValue)
                            error = "Некорректный порт";
                        break;
                    case nameof(CachedUserLifeTime):
                        if (!int.TryParse(CachedUserLifeTime, out var cachedUser) || cachedUser < 0)
                            error = "Некорректное время";
                        break;
                    case nameof(CachedChatLifeTime):
                        if (!int.TryParse(CachedChatLifeTime, out var cachedChat) || cachedChat < 0)
                            error = "Некорректное время";
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
            InfoPort = Preferences.Default.InfoPort.ToString();
            CachedUserLifeTime = Preferences.Default.CachedUserLifetimeMinutes.ToString();
            CachedChatLifeTime = Preferences.Default.CachedChatLifetimeMinutes.ToString();
        }

        private void Save(object? obj)
        {
            Preferences.Default.ServerAddress = serverAddress;
            Preferences.Default.IsSaveAuth = IsSaveAuth;

            if (int.TryParse(InfoPort, out var auth))
                Preferences.Default.InfoPort = auth;

            if (int.TryParse(CachedUserLifeTime, out var user))
                Preferences.Default.CachedUserLifetimeMinutes =user;

            if (int.TryParse(CachedChatLifeTime, out var chat))
                Preferences.Default.CachedChatLifetimeMinutes = chat;

            Preferences.Default.Save();
            SuccessSave = true;
        }

    }
}
