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
            var path = OpenFilePickerWindow();
            if (string.IsNullOrEmpty(path))
            {
                var imageData = await OpenAvatarWindow(path);
                if (imageData.Length > 0)
                    await ChangeAvatarWindow(imageData);
            }
            await Messenger.Send(Messages.ModalClosed, true);
        }
        private async Task<byte[]> OpenAvatarWindow(string path)
        {
            var mainWindow = App.Current.MainWindow;
            var imageCutter = new ImagePickerWindow(path)
            {
                Owner = mainWindow,
                ShowInTaskbar = false
            };
            if (imageCutter.ShowDialog() == true)
                return imageCutter.Image;
            else
                return [];
        }
        private async Task ChangeAvatarWindow(byte[] imageData)
        {
            await Messenger.Send(Messages.LoadingStart, true);
            await Model.ChangeAvatar(imageData);
            await Messenger.Send(Messages.LoadingOver, true);
        }
        private string OpenFilePickerWindow()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Изображения|*.jpg;*.jpeg;*.png",
                CheckFileExists = true,
                CheckPathExists = true,
                ValidateNames = true,
                Multiselect = false,
            };
            bool? result = dialog.ShowDialog();
            if(result == true)
                return dialog.FileName;
            return string.Empty;
        }
    }

}
