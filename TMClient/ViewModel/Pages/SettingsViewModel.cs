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
        private string serverAddress = string.Empty;

        public string InfoPort
        {
            get => infoPort;
            set
            {
                infoPort = value;
                OnPropertyChanged(nameof(InfoPort));
            }
        }
        private string infoPort = string.Empty;

        public string CachedUserLifeTime
        {
            get => cachedUserLifeTime;
            set
            {
                cachedUserLifeTime = value;
                OnPropertyChanged(nameof(CachedUserLifeTime));
            }
        }
        private string cachedUserLifeTime = string.Empty;

        public string CachedChatLifeTime
        {
            get => cachedChatLifeTime;
            set
            {
                cachedChatLifeTime = value;
                OnPropertyChanged(nameof(cachedChatLifeTime));
            }
        }
        private string cachedChatLifeTime = string.Empty;

        public bool IsSaveAuth
        {
            get => isSaveAuth;
            set
            {
                isSaveAuth = value;
                OnPropertyChanged(nameof(IsSaveAuth));
            }
        }
        private bool isSaveAuth;
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

        public string SaveLocation
        {
            get => saveLocation;
            set
            {
                saveLocation = value;
                OnPropertyChanged(nameof(SaveLocation));
            }
        }
        private string saveLocation;

        public ICommand SaveLocationChangedCommand=>new Command(SaveLocationChanged);

        private void SaveLocationChanged(object? obj)
        {
            if (obj is not string path)
                return;
            SaveLocation = path;
        }

        public string Error => throw new NotImplementedException();
        public string this[string columnName]
        {
            get
            {
                var error = string.Empty;
                switch (columnName)
                {
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
            IsSaveAuth=Preferences.Default.IsSaveAuth;
            SaveLocation = Preferences.Default.SavingFolder;
        }

        private void Save(object? obj)
        {
            Preferences.Default.ServerAddress = serverAddress;
            Preferences.Default.IsSaveAuth = IsSaveAuth;

            if (int.TryParse(InfoPort, out var auth))
                Preferences.Default.InfoPort = auth;

            if (int.TryParse(CachedUserLifeTime, out var user))
                Preferences.Default.CachedUserLifetimeMinutes = user;

            if (int.TryParse(CachedChatLifeTime, out var chat))
                Preferences.Default.CachedChatLifetimeMinutes = chat;
            Preferences.Default.SavingFolder = SaveLocation;
            Preferences.Default.Save();
            SuccessSave = true;
        }
    }
}
