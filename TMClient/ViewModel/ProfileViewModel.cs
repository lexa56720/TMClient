using AsyncAwaitBestPractices.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TMClient.Model;
using TMClient.Utils;
using TMClient.View;

namespace TMClient.ViewModel
{
    internal class ProfileViewModel : BaseViewModel
    {
        public ICommand ChangeAvatarCommand => new AsyncCommand(ChangeAvatar);

        private readonly ProfileModel Model = new();

        private async Task ChangeAvatar()
        {
            await Messenger.Send(Messages.ModalOpened, true);
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Изображения|*.jpg;*.jpeg;*.png",
                CheckFileExists = true,
                CheckPathExists = true,
                ValidateNames = true,
                Multiselect = false,
            };
            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                var mainWindow = App.Current.MainWindow;
                var imageCutter = new ImagePickerWindow(dialog.FileName)
                {
                    Owner = mainWindow,
                    ShowInTaskbar = false
                };
                result = imageCutter.ShowDialog();
                if (result == true)
                    await Model.ChangeAvatar(imageCutter.Image);
            }
            await Messenger.Send(Messages.ModalClosed, true);
        }
    }
}
