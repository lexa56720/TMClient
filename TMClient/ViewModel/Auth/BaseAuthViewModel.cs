﻿using ClientApiWrapper.Interfaces;
using TMClient.Model.Auth;
using TMClient.Utils;
using TMClient.View;

namespace TMClient.ViewModel.Auth
{
    internal abstract class BaseAuthViewModel<T> : BaseViewModel where T : BaseAuthModel
    {
        protected readonly Func<IApi?, bool> ReturnApi;

        public bool IsSaveAuth
        {
            get => isSaveAuth;
            set
            {
                isSaveAuth = value;
                Model.SetIsSaveAuth(value);
                OnPropertyChanged(nameof(IsSaveAuth));
            }
        }
        private bool isSaveAuth=Preferences.Default.IsSaveAuth;

        protected T Model { get; private set; }
        public BaseAuthViewModel(Func<IApi?, bool> returnApi)
        {
            Model = GetModel();
            ReturnApi = returnApi;
        }
        protected abstract T GetModel();
    }
}
