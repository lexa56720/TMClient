using AsyncAwaitBestPractices.MVVM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TMClient.Model.Pages;
using TMClient.Utils;
using TMClient.Utils.Validations;
using TMClient.View;

namespace TMClient.ViewModel.Pages
{
    internal class ProfileViewModel : BaseViewModel, IDataErrorInfo
    {
        public ICommand ChangeAvatarCommand => new AsyncCommand(ChangeAvatar);

        public ICommand SaveNameCommand => new AsyncCommand(SaveName);

        public ICommand ResetNameCommand => new AsyncCommand(ResetName);

        public ICommand SavePasswordCommand => new AsyncCommand(SavePassword);

        public string CurrentPassword
        {
            get => currentPassword;
            set
            {
                currentPassword = value;
                OnPropertyChanged(nameof(CurrentPassword));
            }
        }
        private string currentPassword = string.Empty;

        public string NewPassword
        {
            get => newPassword;
            set
            {
                newPassword = value;
                OnPropertyChanged(nameof(RepeatNewPassword));
                OnPropertyChanged(nameof(NewPassword));
            }
        }
        private string newPassword = string.Empty;

        public string RepeatNewPassword
        {
            get => repeatNewPassword;
            set
            {
                repeatNewPassword = value;
                OnPropertyChanged(nameof(RepeatNewPassword));
            }
        }
        private string repeatNewPassword = string.Empty;

        public bool? SuccessSave
        {
            get => successSave;
            set
            {
                successSave = value;
                OnPropertyChanged(nameof(SuccessSave));
            }
        }
        private bool? successSave = null!;

        public bool? FailedSave
        {
            get => failedSave;
            set
            {
                failedSave = value;
                OnPropertyChanged(nameof(FailedSave));
            }
        }
        private bool? failedSave = null!;

        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        private string name = string.Empty;


        public string Error => throw new NotImplementedException();
        public string this[string columnName] => GetError(columnName);

        private string GetError(string columnName)
        {
            switch (columnName)
            {
                case nameof(CurrentPassword):
                    {
                        if (Model.IsCurrentPasswordValid(CurrentPassword))
                            return "Пароль не соответствует текущему";
                        break;
                    }
                case nameof(NewPassword):
                    {
                        var a = new PasswordRule();
                        var b = a.Validate(NewPassword, null);
                        if (!b.IsValid && !string.IsNullOrEmpty(NewPassword))
                            return b.ErrorContent.ToString();
                        break;
                    }
                case nameof(RepeatNewPassword):
                    {
                        if (!NewPassword.Equals(RepeatNewPassword))
                            return "Пароли не совпадают";
                        break;
                    }
            }
            return string.Empty;
        }

        private readonly ProfileModel Model = new();


        public ProfileViewModel()
        {
            Name = CurrentUser.Info.Name;
        }

        private async Task ResetName()
        {
            Name = CurrentUser.Info.Name;
        }
        private async Task SaveName()
        {
            await Messenger.Send(Messages.LoadingStart);
            SetSaveState(await Model.SaveName(Name));
            await Messenger.Send(Messages.LoadingOver);
        }

        private async Task SavePassword()
        {
            await Messenger.Send(Messages.LoadingStart);
            SetSaveState(await Model.SavePassword(CurrentPassword, NewPassword));
            await Messenger.Send(Messages.LoadingOver);
            RepeatNewPassword = NewPassword = CurrentPassword = string.Empty;
        }
        private async Task ChangeAvatar()
        {
            await Messenger.Send(Messages.ModalOpened, true);
            var imageData = GetImageData();
            if (imageData.Length > 0)
            {
                await Messenger.Send(Messages.LoadingStart, true);
                await Model.ChangeAvatar(imageData);
                await Messenger.Send(Messages.LoadingOver, true);
            }
            await Messenger.Send(Messages.ModalClosed, true);
        }
        private byte[] GetImageData()
        {
            var path = PathPicker.PickFiles("Изображения|*.jpg;*.jpeg;*.png", false).FirstOrDefault();
            if (!string.IsNullOrEmpty(path))
            {
                var mainWindow = App.Current.MainWindow;
                var imageCutter = new ImagePickerWindow(path)
                {
                    Owner = mainWindow,
                    ShowInTaskbar = false
                };
                if (imageCutter.ShowDialog() == true)
                    return imageCutter.Image;
            }
            return [];
        }
        private void SetSaveState(bool state)
        {
            if (state)
                SuccessSave = true;
            else
                FailedSave = true;
        }
    }
}
